using System;

namespace InControl
{
	// Token: 0x02000813 RID: 2067
	public class XboxOneInputDevice : InputDevice
	{
		// Token: 0x06002C00 RID: 11264 RVA: 0x000DEDF0 File Offset: 0x000DCFF0
		public XboxOneInputDevice(uint joystickId)
			: base("Xbox One Controller")
		{
			this.JoystickId = joystickId;
			base.SortOrder = (int)joystickId;
			base.Meta = "Xbox One Device #" + joystickId;
			base.DeviceClass = InputDeviceClass.Controller;
			base.DeviceStyle = InputDeviceStyle.XboxOne;
			this.CacheAnalogAxisNames();
			base.AddControl(InputControlType.LeftStickLeft, "Left Stick Left", 0.2f, 0.9f);
			base.AddControl(InputControlType.LeftStickRight, "Left Stick Right", 0.2f, 0.9f);
			base.AddControl(InputControlType.LeftStickUp, "Left Stick Up", 0.2f, 0.9f);
			base.AddControl(InputControlType.LeftStickDown, "Left Stick Down", 0.2f, 0.9f);
			base.AddControl(InputControlType.RightStickLeft, "Right Stick Left", 0.2f, 0.9f);
			base.AddControl(InputControlType.RightStickRight, "Right Stick Right", 0.2f, 0.9f);
			base.AddControl(InputControlType.RightStickUp, "Right Stick Up", 0.2f, 0.9f);
			base.AddControl(InputControlType.RightStickDown, "Right Stick Down", 0.2f, 0.9f);
			base.AddControl(InputControlType.LeftTrigger, "Left Trigger", 0.2f, 0.9f);
			base.AddControl(InputControlType.RightTrigger, "Right Trigger", 0.2f, 0.9f);
			base.AddControl(InputControlType.DPadUp, "DPad Up", 0.2f, 0.9f);
			base.AddControl(InputControlType.DPadDown, "DPad Down", 0.2f, 0.9f);
			base.AddControl(InputControlType.DPadLeft, "DPad Left", 0.2f, 0.9f);
			base.AddControl(InputControlType.DPadRight, "DPad Right", 0.2f, 0.9f);
			base.AddControl(InputControlType.Action1, "A");
			base.AddControl(InputControlType.Action2, "B");
			base.AddControl(InputControlType.Action3, "X");
			base.AddControl(InputControlType.Action4, "Y");
			base.AddControl(InputControlType.LeftBumper, "Left Bumper");
			base.AddControl(InputControlType.RightBumper, "Right Bumper");
			base.AddControl(InputControlType.LeftStickButton, "Left Stick Button");
			base.AddControl(InputControlType.RightStickButton, "Right Stick Button");
			base.AddControl(InputControlType.View, "View");
			base.AddControl(InputControlType.Menu, "Menu");
		}

		// Token: 0x1700083A RID: 2106
		// (get) Token: 0x06002C01 RID: 11265 RVA: 0x000DF014 File Offset: 0x000DD214
		// (set) Token: 0x06002C02 RID: 11266 RVA: 0x000DF01C File Offset: 0x000DD21C
		internal uint JoystickId { get; private set; }

		// Token: 0x1700083B RID: 2107
		// (get) Token: 0x06002C03 RID: 11267 RVA: 0x000DF028 File Offset: 0x000DD228
		// (set) Token: 0x06002C04 RID: 11268 RVA: 0x000DF030 File Offset: 0x000DD230
		public ulong ControllerId { get; private set; }

		// Token: 0x06002C05 RID: 11269 RVA: 0x000DF03C File Offset: 0x000DD23C
		public override void Update(ulong updateTick, float deltaTime)
		{
		}

		// Token: 0x1700083C RID: 2108
		// (get) Token: 0x06002C06 RID: 11270 RVA: 0x000DF040 File Offset: 0x000DD240
		public bool IsConnected
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06002C07 RID: 11271 RVA: 0x000DF044 File Offset: 0x000DD244
		public override void Vibrate(float leftMotor, float rightMotor)
		{
		}

		// Token: 0x06002C08 RID: 11272 RVA: 0x000DF048 File Offset: 0x000DD248
		public void Vibrate(float leftMotor, float rightMotor, float leftTrigger, float rightTrigger)
		{
		}

		// Token: 0x06002C09 RID: 11273 RVA: 0x000DF04C File Offset: 0x000DD24C
		private string AnalogAxisNameForId(uint analogId)
		{
			return this.analogAxisNameForId[(int)((UIntPtr)analogId)];
		}

		// Token: 0x06002C0A RID: 11274 RVA: 0x000DF058 File Offset: 0x000DD258
		private void CacheAnalogAxisNameForId(uint analogId)
		{
			this.analogAxisNameForId[(int)((UIntPtr)analogId)] = string.Concat(new object[] { "joystick ", this.JoystickId, " analog ", analogId });
		}

		// Token: 0x06002C0B RID: 11275 RVA: 0x000DF098 File Offset: 0x000DD298
		private void CacheAnalogAxisNames()
		{
			this.analogAxisNameForId = new string[16];
			this.CacheAnalogAxisNameForId(0U);
			this.CacheAnalogAxisNameForId(1U);
			this.CacheAnalogAxisNameForId(3U);
			this.CacheAnalogAxisNameForId(4U);
			this.CacheAnalogAxisNameForId(8U);
			this.CacheAnalogAxisNameForId(9U);
		}

		// Token: 0x04001DF2 RID: 7666
		private const uint AnalogLeftStickX = 0U;

		// Token: 0x04001DF3 RID: 7667
		private const uint AnalogLeftStickY = 1U;

		// Token: 0x04001DF4 RID: 7668
		private const uint AnalogRightStickX = 3U;

		// Token: 0x04001DF5 RID: 7669
		private const uint AnalogRightStickY = 4U;

		// Token: 0x04001DF6 RID: 7670
		private const uint AnalogLeftTrigger = 8U;

		// Token: 0x04001DF7 RID: 7671
		private const uint AnalogRightTrigger = 9U;

		// Token: 0x04001DF8 RID: 7672
		private const float LowerDeadZone = 0.2f;

		// Token: 0x04001DF9 RID: 7673
		private const float UpperDeadZone = 0.9f;

		// Token: 0x04001DFC RID: 7676
		private string[] analogAxisNameForId;
	}
}
