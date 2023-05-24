using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace InControl
{
	// Token: 0x02000754 RID: 1876
	public class NativeInputDevice : InputDevice
	{
		// Token: 0x06002989 RID: 10633 RVA: 0x000BCAD4 File Offset: 0x000BACD4
		internal NativeInputDevice()
		{
		}

		// Token: 0x170007F1 RID: 2033
		// (get) Token: 0x0600298A RID: 10634 RVA: 0x000BCADC File Offset: 0x000BACDC
		// (set) Token: 0x0600298B RID: 10635 RVA: 0x000BCAE4 File Offset: 0x000BACE4
		internal uint Handle { get; private set; }

		// Token: 0x170007F2 RID: 2034
		// (get) Token: 0x0600298C RID: 10636 RVA: 0x000BCAF0 File Offset: 0x000BACF0
		// (set) Token: 0x0600298D RID: 10637 RVA: 0x000BCAF8 File Offset: 0x000BACF8
		internal NativeDeviceInfo Info { get; private set; }

		// Token: 0x0600298E RID: 10638 RVA: 0x000BCB04 File Offset: 0x000BAD04
		internal void Initialize(uint deviceHandle, NativeDeviceInfo deviceInfo, NativeInputDeviceProfile deviceProfile)
		{
			this.Handle = deviceHandle;
			this.Info = deviceInfo;
			this.profile = deviceProfile;
			base.SortOrder = (int)(1000U + this.Handle);
			this.numUnknownButtons = Math.Min((int)this.Info.numButtons, 20);
			this.numUnknownAnalogs = Math.Min((int)this.Info.numAnalogs, 20);
			this.buttons = new short[this.Info.numButtons];
			this.analogs = new short[this.Info.numAnalogs];
			base.AnalogSnapshot = null;
			base.ClearInputState();
			base.ClearControls();
			if (this.IsKnown)
			{
				base.Name = this.profile.Name ?? this.Info.name;
				base.Meta = this.profile.Meta ?? this.Info.name;
				base.DeviceClass = this.profile.DeviceClass;
				base.DeviceStyle = this.profile.DeviceStyle;
				int analogCount = this.profile.AnalogCount;
				for (int i = 0; i < analogCount; i++)
				{
					InputControlMapping inputControlMapping = this.profile.AnalogMappings[i];
					InputControl inputControl = base.AddControl(inputControlMapping.Target, inputControlMapping.Handle);
					inputControl.Sensitivity = Mathf.Min(this.profile.Sensitivity, inputControlMapping.Sensitivity);
					inputControl.LowerDeadZone = Mathf.Max(this.profile.LowerDeadZone, inputControlMapping.LowerDeadZone);
					inputControl.UpperDeadZone = Mathf.Min(this.profile.UpperDeadZone, inputControlMapping.UpperDeadZone);
					inputControl.Raw = inputControlMapping.Raw;
					inputControl.Passive = inputControlMapping.Passive;
				}
				int buttonCount = this.profile.ButtonCount;
				for (int j = 0; j < buttonCount; j++)
				{
					InputControlMapping inputControlMapping2 = this.profile.ButtonMappings[j];
					InputControl inputControl2 = base.AddControl(inputControlMapping2.Target, inputControlMapping2.Handle);
					inputControl2.Passive = inputControlMapping2.Passive;
				}
			}
			else
			{
				base.Name = "Unknown Device";
				base.Meta = this.Info.name;
				for (int k = 0; k < this.NumUnknownButtons; k++)
				{
					base.AddControl(InputControlType.Button0 + k, "Button " + k);
				}
				for (int l = 0; l < this.NumUnknownAnalogs; l++)
				{
					base.AddControl(InputControlType.Analog0 + l, "Analog " + l, 0.2f, 0.9f);
				}
			}
			this.skipUpdateFrames = 1;
		}

		// Token: 0x0600298F RID: 10639 RVA: 0x000BCDF4 File Offset: 0x000BAFF4
		internal void Initialize(uint deviceHandle, NativeDeviceInfo deviceInfo)
		{
			this.Initialize(deviceHandle, deviceInfo, this.profile);
		}

		// Token: 0x06002990 RID: 10640 RVA: 0x000BCE04 File Offset: 0x000BB004
		public override void Update(ulong updateTick, float deltaTime)
		{
			if (this.skipUpdateFrames > 0)
			{
				this.skipUpdateFrames--;
				return;
			}
			IntPtr intPtr;
			if (Native.GetDeviceState(this.Handle, out intPtr))
			{
				Marshal.Copy(intPtr, this.buttons, 0, this.buttons.Length);
				intPtr = new IntPtr(intPtr.ToInt64() + (long)(this.buttons.Length * 2));
				Marshal.Copy(intPtr, this.analogs, 0, this.analogs.Length);
			}
			if (this.IsKnown)
			{
				int analogCount = this.profile.AnalogCount;
				for (int i = 0; i < analogCount; i++)
				{
					InputControlMapping inputControlMapping = this.profile.AnalogMappings[i];
					float value = inputControlMapping.Source.GetValue(this);
					InputControl control = base.GetControl(inputControlMapping.Target);
					if (!inputControlMapping.IgnoreInitialZeroValue || !control.IsOnZeroTick || !Utility.IsZero(value))
					{
						float num = inputControlMapping.MapValue(value);
						control.UpdateWithValue(num, updateTick, deltaTime);
					}
				}
				int buttonCount = this.profile.ButtonCount;
				for (int j = 0; j < buttonCount; j++)
				{
					InputControlMapping inputControlMapping2 = this.profile.ButtonMappings[j];
					bool state = inputControlMapping2.Source.GetState(this);
					base.UpdateWithState(inputControlMapping2.Target, state, updateTick, deltaTime);
				}
			}
			else
			{
				for (int k = 0; k < this.NumUnknownButtons; k++)
				{
					base.UpdateWithState(InputControlType.Button0 + k, this.ReadRawButtonState(k), updateTick, deltaTime);
				}
				for (int l = 0; l < this.NumUnknownAnalogs; l++)
				{
					base.UpdateWithValue(InputControlType.Analog0 + l, this.ReadRawAnalogValue(l), updateTick, deltaTime);
				}
			}
		}

		// Token: 0x06002991 RID: 10641 RVA: 0x000BCFD0 File Offset: 0x000BB1D0
		internal override bool ReadRawButtonState(int index)
		{
			return index < this.buttons.Length && this.buttons[index] > -32767;
		}

		// Token: 0x06002992 RID: 10642 RVA: 0x000BCFF4 File Offset: 0x000BB1F4
		internal override float ReadRawAnalogValue(int index)
		{
			if (index < this.analogs.Length)
			{
				return (float)this.analogs[index] / 32767f;
			}
			return 0f;
		}

		// Token: 0x06002993 RID: 10643 RVA: 0x000BD01C File Offset: 0x000BB21C
		private byte FloatToByte(float value)
		{
			return (byte)(Mathf.Clamp01(value) * 255f);
		}

		// Token: 0x06002994 RID: 10644 RVA: 0x000BD02C File Offset: 0x000BB22C
		public override void Vibrate(float leftMotor, float rightMotor)
		{
			Native.SetHapticState(this.Handle, this.FloatToByte(leftMotor), this.FloatToByte(rightMotor));
		}

		// Token: 0x06002995 RID: 10645 RVA: 0x000BD048 File Offset: 0x000BB248
		public override void SetLightColor(float red, float green, float blue)
		{
			Native.SetLightColor(this.Handle, this.FloatToByte(red), this.FloatToByte(green), this.FloatToByte(blue));
		}

		// Token: 0x06002996 RID: 10646 RVA: 0x000BD06C File Offset: 0x000BB26C
		public override void SetLightFlash(float flashOnDuration, float flashOffDuration)
		{
			Native.SetLightFlash(this.Handle, this.FloatToByte(flashOnDuration), this.FloatToByte(flashOffDuration));
		}

		// Token: 0x06002997 RID: 10647 RVA: 0x000BD088 File Offset: 0x000BB288
		public bool HasSameVendorID(NativeDeviceInfo deviceInfo)
		{
			return this.Info.HasSameVendorID(deviceInfo);
		}

		// Token: 0x06002998 RID: 10648 RVA: 0x000BD0A4 File Offset: 0x000BB2A4
		public bool HasSameProductID(NativeDeviceInfo deviceInfo)
		{
			return this.Info.HasSameProductID(deviceInfo);
		}

		// Token: 0x06002999 RID: 10649 RVA: 0x000BD0C0 File Offset: 0x000BB2C0
		public bool HasSameVersionNumber(NativeDeviceInfo deviceInfo)
		{
			return this.Info.HasSameVersionNumber(deviceInfo);
		}

		// Token: 0x0600299A RID: 10650 RVA: 0x000BD0DC File Offset: 0x000BB2DC
		public bool HasSameLocation(NativeDeviceInfo deviceInfo)
		{
			return this.Info.HasSameLocation(deviceInfo);
		}

		// Token: 0x0600299B RID: 10651 RVA: 0x000BD0F8 File Offset: 0x000BB2F8
		public bool HasSameSerialNumber(NativeDeviceInfo deviceInfo)
		{
			return this.Info.HasSameSerialNumber(deviceInfo);
		}

		// Token: 0x170007F3 RID: 2035
		// (get) Token: 0x0600299C RID: 10652 RVA: 0x000BD114 File Offset: 0x000BB314
		public override bool IsSupportedOnThisPlatform
		{
			get
			{
				return this.profile == null || this.profile.IsSupportedOnThisPlatform;
			}
		}

		// Token: 0x170007F4 RID: 2036
		// (get) Token: 0x0600299D RID: 10653 RVA: 0x000BD130 File Offset: 0x000BB330
		public override bool IsKnown
		{
			get
			{
				return this.profile != null;
			}
		}

		// Token: 0x170007F5 RID: 2037
		// (get) Token: 0x0600299E RID: 10654 RVA: 0x000BD140 File Offset: 0x000BB340
		internal override int NumUnknownButtons
		{
			get
			{
				return this.numUnknownButtons;
			}
		}

		// Token: 0x170007F6 RID: 2038
		// (get) Token: 0x0600299F RID: 10655 RVA: 0x000BD148 File Offset: 0x000BB348
		internal override int NumUnknownAnalogs
		{
			get
			{
				return this.numUnknownAnalogs;
			}
		}

		// Token: 0x04001C7F RID: 7295
		private const int maxUnknownButtons = 20;

		// Token: 0x04001C80 RID: 7296
		private const int maxUnknownAnalogs = 20;

		// Token: 0x04001C83 RID: 7299
		private short[] buttons;

		// Token: 0x04001C84 RID: 7300
		private short[] analogs;

		// Token: 0x04001C85 RID: 7301
		private NativeInputDeviceProfile profile;

		// Token: 0x04001C86 RID: 7302
		private int skipUpdateFrames;

		// Token: 0x04001C87 RID: 7303
		private int numUnknownButtons;

		// Token: 0x04001C88 RID: 7304
		private int numUnknownAnalogs;
	}
}
