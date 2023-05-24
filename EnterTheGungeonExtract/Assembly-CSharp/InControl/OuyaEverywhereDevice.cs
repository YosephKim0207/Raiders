using System;

namespace InControl
{
	// Token: 0x02000686 RID: 1670
	public class OuyaEverywhereDevice : InputDevice
	{
		// Token: 0x06002601 RID: 9729 RVA: 0x000A2F90 File Offset: 0x000A1190
		public OuyaEverywhereDevice(int deviceIndex)
			: base("OUYA Controller")
		{
			this.DeviceIndex = deviceIndex;
			base.SortOrder = deviceIndex;
			base.Meta = "OUYA Everywhere Device #" + deviceIndex;
			base.AddControl(InputControlType.LeftStickLeft, "Left Stick Left");
			base.AddControl(InputControlType.LeftStickRight, "Left Stick Right");
			base.AddControl(InputControlType.LeftStickUp, "Left Stick Up");
			base.AddControl(InputControlType.LeftStickDown, "Left Stick Down");
			base.AddControl(InputControlType.RightStickLeft, "Right Stick Left");
			base.AddControl(InputControlType.RightStickRight, "Right Stick Right");
			base.AddControl(InputControlType.RightStickUp, "Right Stick Up");
			base.AddControl(InputControlType.RightStickDown, "Right Stick Down");
			base.AddControl(InputControlType.LeftTrigger, "Left Trigger");
			base.AddControl(InputControlType.RightTrigger, "Right Trigger");
			base.AddControl(InputControlType.DPadUp, "DPad Up");
			base.AddControl(InputControlType.DPadDown, "DPad Down");
			base.AddControl(InputControlType.DPadLeft, "DPad Left");
			base.AddControl(InputControlType.DPadRight, "DPad Right");
			base.AddControl(InputControlType.Action1, "O");
			base.AddControl(InputControlType.Action2, "A");
			base.AddControl(InputControlType.Action3, "Y");
			base.AddControl(InputControlType.Action4, "U");
			base.AddControl(InputControlType.LeftBumper, "Left Bumper");
			base.AddControl(InputControlType.RightBumper, "Right Bumper");
			base.AddControl(InputControlType.LeftStickButton, "Left Stick Button");
			base.AddControl(InputControlType.RightStickButton, "Right Stick Button");
			base.AddControl(InputControlType.Menu, "Menu");
		}

		// Token: 0x170006FC RID: 1788
		// (get) Token: 0x06002602 RID: 9730 RVA: 0x000A3108 File Offset: 0x000A1308
		// (set) Token: 0x06002603 RID: 9731 RVA: 0x000A3110 File Offset: 0x000A1310
		public int DeviceIndex { get; private set; }

		// Token: 0x06002604 RID: 9732 RVA: 0x000A311C File Offset: 0x000A131C
		public void BeforeAttach()
		{
		}

		// Token: 0x06002605 RID: 9733 RVA: 0x000A3120 File Offset: 0x000A1320
		public override void Update(ulong updateTick, float deltaTime)
		{
		}

		// Token: 0x170006FD RID: 1789
		// (get) Token: 0x06002606 RID: 9734 RVA: 0x000A3124 File Offset: 0x000A1324
		public bool IsConnected
		{
			get
			{
				return false;
			}
		}

		// Token: 0x040019DB RID: 6619
		private const float LowerDeadZone = 0.2f;

		// Token: 0x040019DC RID: 6620
		private const float UpperDeadZone = 0.9f;
	}
}
