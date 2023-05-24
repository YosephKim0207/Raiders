using System;
using XInputDotNetPure;

namespace InControl
{
	// Token: 0x02000815 RID: 2069
	public class XInputDevice : InputDevice
	{
		// Token: 0x06002C12 RID: 11282 RVA: 0x000DF228 File Offset: 0x000DD428
		public XInputDevice(int deviceIndex, XInputDeviceManager owner)
			: base("XInput Controller")
		{
			this.owner = owner;
			this.DeviceIndex = deviceIndex;
			base.SortOrder = deviceIndex;
			base.Meta = "XInput Device #" + deviceIndex;
			base.DeviceClass = InputDeviceClass.Controller;
			base.DeviceStyle = InputDeviceStyle.XboxOne;
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
			base.AddControl(InputControlType.Start, "Start");
			base.AddControl(InputControlType.Back, "Back");
		}

		// Token: 0x1700083D RID: 2109
		// (get) Token: 0x06002C13 RID: 11283 RVA: 0x000DF450 File Offset: 0x000DD650
		// (set) Token: 0x06002C14 RID: 11284 RVA: 0x000DF458 File Offset: 0x000DD658
		public int DeviceIndex { get; private set; }

		// Token: 0x06002C15 RID: 11285 RVA: 0x000DF464 File Offset: 0x000DD664
		public override void Update(ulong updateTick, float deltaTime)
		{
			this.GetState();
			base.UpdateLeftStickWithValue(this.state.ThumbSticks.Left.Vector, updateTick, deltaTime);
			base.UpdateRightStickWithValue(this.state.ThumbSticks.Right.Vector, updateTick, deltaTime);
			base.UpdateWithValue(InputControlType.LeftTrigger, this.state.Triggers.Left, updateTick, deltaTime);
			base.UpdateWithValue(InputControlType.RightTrigger, this.state.Triggers.Right, updateTick, deltaTime);
			base.UpdateWithState(InputControlType.DPadUp, this.state.DPad.Up == ButtonState.Pressed, updateTick, deltaTime);
			base.UpdateWithState(InputControlType.DPadDown, this.state.DPad.Down == ButtonState.Pressed, updateTick, deltaTime);
			base.UpdateWithState(InputControlType.DPadLeft, this.state.DPad.Left == ButtonState.Pressed, updateTick, deltaTime);
			base.UpdateWithState(InputControlType.DPadRight, this.state.DPad.Right == ButtonState.Pressed, updateTick, deltaTime);
			base.UpdateWithState(InputControlType.Action1, this.state.Buttons.A == ButtonState.Pressed, updateTick, deltaTime);
			base.UpdateWithState(InputControlType.Action2, this.state.Buttons.B == ButtonState.Pressed, updateTick, deltaTime);
			base.UpdateWithState(InputControlType.Action3, this.state.Buttons.X == ButtonState.Pressed, updateTick, deltaTime);
			base.UpdateWithState(InputControlType.Action4, this.state.Buttons.Y == ButtonState.Pressed, updateTick, deltaTime);
			base.UpdateWithState(InputControlType.LeftBumper, this.state.Buttons.LeftShoulder == ButtonState.Pressed, updateTick, deltaTime);
			base.UpdateWithState(InputControlType.RightBumper, this.state.Buttons.RightShoulder == ButtonState.Pressed, updateTick, deltaTime);
			base.UpdateWithState(InputControlType.LeftStickButton, this.state.Buttons.LeftStick == ButtonState.Pressed, updateTick, deltaTime);
			base.UpdateWithState(InputControlType.RightStickButton, this.state.Buttons.RightStick == ButtonState.Pressed, updateTick, deltaTime);
			base.UpdateWithState(InputControlType.Start, this.state.Buttons.Start == ButtonState.Pressed, updateTick, deltaTime);
			base.UpdateWithState(InputControlType.Back, this.state.Buttons.Back == ButtonState.Pressed, updateTick, deltaTime);
			base.Commit(updateTick, deltaTime);
		}

		// Token: 0x06002C16 RID: 11286 RVA: 0x000DF6D0 File Offset: 0x000DD8D0
		public override void Vibrate(float leftMotor, float rightMotor)
		{
			GamePad.SetVibration((PlayerIndex)this.DeviceIndex, leftMotor, rightMotor);
		}

		// Token: 0x06002C17 RID: 11287 RVA: 0x000DF6E0 File Offset: 0x000DD8E0
		internal void GetState()
		{
			this.state = this.owner.GetState(this.DeviceIndex);
		}

		// Token: 0x1700083E RID: 2110
		// (get) Token: 0x06002C18 RID: 11288 RVA: 0x000DF6FC File Offset: 0x000DD8FC
		public bool IsConnected
		{
			get
			{
				return this.state.IsConnected;
			}
		}

		// Token: 0x04001DFF RID: 7679
		private const float LowerDeadZone = 0.2f;

		// Token: 0x04001E00 RID: 7680
		private const float UpperDeadZone = 0.9f;

		// Token: 0x04001E01 RID: 7681
		private XInputDeviceManager owner;

		// Token: 0x04001E02 RID: 7682
		private GamePadState state;
	}
}
