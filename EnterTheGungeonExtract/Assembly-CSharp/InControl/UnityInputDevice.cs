using System;
using UnityEngine;

namespace InControl
{
	// Token: 0x02000803 RID: 2051
	public class UnityInputDevice : InputDevice
	{
		// Token: 0x06002B59 RID: 11097 RVA: 0x000DBBB0 File Offset: 0x000D9DB0
		public UnityInputDevice(UnityInputDeviceProfileBase deviceProfile)
			: this(deviceProfile, 0, string.Empty)
		{
		}

		// Token: 0x06002B5A RID: 11098 RVA: 0x000DBBC0 File Offset: 0x000D9DC0
		public UnityInputDevice(int joystickId, string joystickName)
			: this(null, joystickId, joystickName)
		{
		}

		// Token: 0x06002B5B RID: 11099 RVA: 0x000DBBCC File Offset: 0x000D9DCC
		public UnityInputDevice(UnityInputDeviceProfileBase deviceProfile, int joystickId, string joystickName)
		{
			this.profile = deviceProfile;
			this.JoystickId = joystickId;
			if (joystickId != 0)
			{
				base.SortOrder = 100 + joystickId;
			}
			UnityInputDevice.SetupAnalogQueries();
			UnityInputDevice.SetupButtonQueries();
			base.AnalogSnapshot = null;
			if (this.IsKnown)
			{
				base.Name = this.profile.Name;
				base.Meta = this.profile.Meta;
				base.ControllerSymbology = this.profile.ControllerSymbology;
				base.DeviceClass = this.profile.DeviceClass;
				base.DeviceStyle = this.profile.DeviceStyle;
				int analogCount = this.profile.AnalogCount;
				for (int i = 0; i < analogCount; i++)
				{
					InputControlMapping inputControlMapping = this.profile.AnalogMappings[i];
					if (Utility.TargetIsAlias(inputControlMapping.Target))
					{
						Debug.LogError(string.Concat(new object[] { "Cannot map control \"", inputControlMapping.Handle, "\" as InputControlType.", inputControlMapping.Target, " in profile \"", deviceProfile.Name, "\" because this target is reserved as an alias. The mapping will be ignored." }));
					}
					else
					{
						InputControl inputControl = base.AddControl(inputControlMapping.Target, inputControlMapping.Handle);
						inputControl.Sensitivity = Mathf.Min(this.profile.Sensitivity, inputControlMapping.Sensitivity);
						inputControl.LowerDeadZone = Mathf.Max(this.profile.LowerDeadZone, inputControlMapping.LowerDeadZone);
						inputControl.UpperDeadZone = Mathf.Min(this.profile.UpperDeadZone, inputControlMapping.UpperDeadZone);
						inputControl.Raw = inputControlMapping.Raw;
						inputControl.Passive = inputControlMapping.Passive;
					}
				}
				int buttonCount = this.profile.ButtonCount;
				for (int j = 0; j < buttonCount; j++)
				{
					InputControlMapping inputControlMapping2 = this.profile.ButtonMappings[j];
					if (Utility.TargetIsAlias(inputControlMapping2.Target))
					{
						Debug.LogError(string.Concat(new object[] { "Cannot map control \"", inputControlMapping2.Handle, "\" as InputControlType.", inputControlMapping2.Target, " in profile \"", deviceProfile.Name, "\" because this target is reserved as an alias. The mapping will be ignored." }));
					}
					else
					{
						InputControl inputControl2 = base.AddControl(inputControlMapping2.Target, inputControlMapping2.Handle);
						inputControl2.Passive = inputControlMapping2.Passive;
					}
				}
			}
			else
			{
				base.Name = "Unknown Device";
				base.Meta = "\"" + joystickName + "\"";
				for (int k = 0; k < this.NumUnknownButtons; k++)
				{
					base.AddControl(InputControlType.Button0 + k, "Button " + k);
				}
				for (int l = 0; l < this.NumUnknownAnalogs; l++)
				{
					base.AddControl(InputControlType.Analog0 + l, "Analog " + l, 0.2f, 0.9f);
				}
			}
		}

		// Token: 0x17000828 RID: 2088
		// (get) Token: 0x06002B5C RID: 11100 RVA: 0x000DBEE4 File Offset: 0x000DA0E4
		// (set) Token: 0x06002B5D RID: 11101 RVA: 0x000DBEEC File Offset: 0x000DA0EC
		internal int JoystickId { get; private set; }

		// Token: 0x06002B5E RID: 11102 RVA: 0x000DBEF8 File Offset: 0x000DA0F8
		public override void Update(ulong updateTick, float deltaTime)
		{
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

		// Token: 0x06002B5F RID: 11103 RVA: 0x000DC050 File Offset: 0x000DA250
		private static void SetupAnalogQueries()
		{
			if (UnityInputDevice.analogQueries == null)
			{
				UnityInputDevice.analogQueries = new string[10, 20];
				for (int i = 1; i <= 10; i++)
				{
					for (int j = 0; j < 20; j++)
					{
						UnityInputDevice.analogQueries[i - 1, j] = string.Concat(new object[] { "joystick ", i, " analog ", j });
					}
				}
			}
		}

		// Token: 0x06002B60 RID: 11104 RVA: 0x000DC0D8 File Offset: 0x000DA2D8
		private static void SetupButtonQueries()
		{
			if (UnityInputDevice.buttonQueries == null)
			{
				UnityInputDevice.buttonQueries = new string[10, 20];
				for (int i = 1; i <= 10; i++)
				{
					for (int j = 0; j < 20; j++)
					{
						UnityInputDevice.buttonQueries[i - 1, j] = string.Concat(new object[] { "joystick ", i, " button ", j });
					}
				}
			}
		}

		// Token: 0x06002B61 RID: 11105 RVA: 0x000DC160 File Offset: 0x000DA360
		private static string GetAnalogKey(int joystickId, int analogId)
		{
			return UnityInputDevice.analogQueries[joystickId - 1, analogId];
		}

		// Token: 0x06002B62 RID: 11106 RVA: 0x000DC170 File Offset: 0x000DA370
		private static string GetButtonKey(int joystickId, int buttonId)
		{
			return UnityInputDevice.buttonQueries[joystickId - 1, buttonId];
		}

		// Token: 0x06002B63 RID: 11107 RVA: 0x000DC180 File Offset: 0x000DA380
		internal override bool ReadRawButtonState(int index)
		{
			if (index < 20)
			{
				string text = UnityInputDevice.buttonQueries[this.JoystickId - 1, index];
				return Input.GetKey(text);
			}
			return false;
		}

		// Token: 0x06002B64 RID: 11108 RVA: 0x000DC1B4 File Offset: 0x000DA3B4
		internal override float ReadRawAnalogValue(int index)
		{
			if (index < 20)
			{
				string text = UnityInputDevice.analogQueries[this.JoystickId - 1, index];
				return Input.GetAxisRaw(text);
			}
			return 0f;
		}

		// Token: 0x17000829 RID: 2089
		// (get) Token: 0x06002B65 RID: 11109 RVA: 0x000DC1EC File Offset: 0x000DA3EC
		public override bool IsSupportedOnThisPlatform
		{
			get
			{
				return this.profile == null || this.profile.IsSupportedOnThisPlatform;
			}
		}

		// Token: 0x1700082A RID: 2090
		// (get) Token: 0x06002B66 RID: 11110 RVA: 0x000DC208 File Offset: 0x000DA408
		public override bool IsKnown
		{
			get
			{
				return this.profile != null;
			}
		}

		// Token: 0x1700082B RID: 2091
		// (get) Token: 0x06002B67 RID: 11111 RVA: 0x000DC218 File Offset: 0x000DA418
		internal override int NumUnknownButtons
		{
			get
			{
				return 20;
			}
		}

		// Token: 0x1700082C RID: 2092
		// (get) Token: 0x06002B68 RID: 11112 RVA: 0x000DC21C File Offset: 0x000DA41C
		internal override int NumUnknownAnalogs
		{
			get
			{
				return 20;
			}
		}

		// Token: 0x04001D94 RID: 7572
		private static string[,] analogQueries;

		// Token: 0x04001D95 RID: 7573
		private static string[,] buttonQueries;

		// Token: 0x04001D96 RID: 7574
		public const int MaxDevices = 10;

		// Token: 0x04001D97 RID: 7575
		public const int MaxButtons = 20;

		// Token: 0x04001D98 RID: 7576
		public const int MaxAnalogs = 20;

		// Token: 0x04001D9A RID: 7578
		private UnityInputDeviceProfileBase profile;
	}
}
