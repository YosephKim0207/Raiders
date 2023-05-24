using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace InControl
{
	// Token: 0x020006AF RID: 1711
	public class InputDevice
	{
		// Token: 0x060027BC RID: 10172 RVA: 0x000A95C0 File Offset: 0x000A77C0
		public InputDevice()
			: this(string.Empty)
		{
		}

		// Token: 0x060027BD RID: 10173 RVA: 0x000A95D0 File Offset: 0x000A77D0
		public InputDevice(string name)
			: this(name, false)
		{
		}

		// Token: 0x060027BE RID: 10174 RVA: 0x000A95DC File Offset: 0x000A77DC
		public InputDevice(string name, bool rawSticks)
		{
			this.Name = name;
			this.RawSticks = rawSticks;
			this.Meta = string.Empty;
			this.GUID = Guid.NewGuid();
			this.LastChangeTick = 0UL;
			this.LastChangeTime = 0f;
			this.SortOrder = int.MaxValue;
			this.DeviceClass = InputDeviceClass.Unknown;
			this.DeviceStyle = InputDeviceStyle.Unknown;
			this.Passive = false;
			this.ControlsByTarget = new InputControl[521];
			this.controls = new List<InputControl>(32);
			this.Controls = new ReadOnlyCollection<InputControl>(this.controls);
			this.RemoveAliasControls();
		}

		// Token: 0x1700077D RID: 1917
		// (get) Token: 0x060027BF RID: 10175 RVA: 0x000A9684 File Offset: 0x000A7884
		// (set) Token: 0x060027C0 RID: 10176 RVA: 0x000A968C File Offset: 0x000A788C
		public string Name { get; protected set; }

		// Token: 0x1700077E RID: 1918
		// (get) Token: 0x060027C1 RID: 10177 RVA: 0x000A9698 File Offset: 0x000A7898
		// (set) Token: 0x060027C2 RID: 10178 RVA: 0x000A96A0 File Offset: 0x000A78A0
		public string Meta { get; protected set; }

		// Token: 0x1700077F RID: 1919
		// (get) Token: 0x060027C3 RID: 10179 RVA: 0x000A96AC File Offset: 0x000A78AC
		// (set) Token: 0x060027C4 RID: 10180 RVA: 0x000A96B4 File Offset: 0x000A78B4
		public int SortOrder { get; protected set; }

		// Token: 0x17000780 RID: 1920
		// (get) Token: 0x060027C5 RID: 10181 RVA: 0x000A96C0 File Offset: 0x000A78C0
		// (set) Token: 0x060027C6 RID: 10182 RVA: 0x000A96C8 File Offset: 0x000A78C8
		public InputDeviceClass DeviceClass { get; protected set; }

		// Token: 0x17000781 RID: 1921
		// (get) Token: 0x060027C7 RID: 10183 RVA: 0x000A96D4 File Offset: 0x000A78D4
		// (set) Token: 0x060027C8 RID: 10184 RVA: 0x000A96DC File Offset: 0x000A78DC
		public InputDeviceStyle DeviceStyle { get; protected set; }

		// Token: 0x17000782 RID: 1922
		// (get) Token: 0x060027C9 RID: 10185 RVA: 0x000A96E8 File Offset: 0x000A78E8
		// (set) Token: 0x060027CA RID: 10186 RVA: 0x000A96F0 File Offset: 0x000A78F0
		public Guid GUID { get; private set; }

		// Token: 0x17000783 RID: 1923
		// (get) Token: 0x060027CB RID: 10187 RVA: 0x000A96FC File Offset: 0x000A78FC
		// (set) Token: 0x060027CC RID: 10188 RVA: 0x000A9704 File Offset: 0x000A7904
		public GameOptions.ControllerSymbology ControllerSymbology
		{
			get
			{
				return this.m_controllerSymbology;
			}
			protected set
			{
				this.m_controllerSymbology = value;
			}
		}

		// Token: 0x17000784 RID: 1924
		// (get) Token: 0x060027CD RID: 10189 RVA: 0x000A9710 File Offset: 0x000A7910
		// (set) Token: 0x060027CE RID: 10190 RVA: 0x000A9718 File Offset: 0x000A7918
		public ulong LastChangeTick { get; protected set; }

		// Token: 0x17000785 RID: 1925
		// (get) Token: 0x060027CF RID: 10191 RVA: 0x000A9724 File Offset: 0x000A7924
		// (set) Token: 0x060027D0 RID: 10192 RVA: 0x000A972C File Offset: 0x000A792C
		public float LastChangeTime { get; protected set; }

		// Token: 0x17000786 RID: 1926
		// (get) Token: 0x060027D1 RID: 10193 RVA: 0x000A9738 File Offset: 0x000A7938
		// (set) Token: 0x060027D2 RID: 10194 RVA: 0x000A9740 File Offset: 0x000A7940
		public bool IsAttached { get; private set; }

		// Token: 0x17000787 RID: 1927
		// (get) Token: 0x060027D3 RID: 10195 RVA: 0x000A974C File Offset: 0x000A794C
		// (set) Token: 0x060027D4 RID: 10196 RVA: 0x000A9754 File Offset: 0x000A7954
		private protected bool RawSticks { protected get; private set; }

		// Token: 0x17000788 RID: 1928
		// (get) Token: 0x060027D5 RID: 10197 RVA: 0x000A9760 File Offset: 0x000A7960
		// (set) Token: 0x060027D6 RID: 10198 RVA: 0x000A9768 File Offset: 0x000A7968
		public ReadOnlyCollection<InputControl> Controls { get; protected set; }

		// Token: 0x17000789 RID: 1929
		// (get) Token: 0x060027D7 RID: 10199 RVA: 0x000A9774 File Offset: 0x000A7974
		// (set) Token: 0x060027D8 RID: 10200 RVA: 0x000A977C File Offset: 0x000A797C
		private protected InputControl[] ControlsByTarget { protected get; private set; }

		// Token: 0x1700078A RID: 1930
		// (get) Token: 0x060027D9 RID: 10201 RVA: 0x000A9788 File Offset: 0x000A7988
		// (set) Token: 0x060027DA RID: 10202 RVA: 0x000A9790 File Offset: 0x000A7990
		public TwoAxisInputControl LeftStick { get; private set; }

		// Token: 0x1700078B RID: 1931
		// (get) Token: 0x060027DB RID: 10203 RVA: 0x000A979C File Offset: 0x000A799C
		// (set) Token: 0x060027DC RID: 10204 RVA: 0x000A97A4 File Offset: 0x000A79A4
		public TwoAxisInputControl RightStick { get; private set; }

		// Token: 0x1700078C RID: 1932
		// (get) Token: 0x060027DD RID: 10205 RVA: 0x000A97B0 File Offset: 0x000A79B0
		// (set) Token: 0x060027DE RID: 10206 RVA: 0x000A97B8 File Offset: 0x000A79B8
		public TwoAxisInputControl DPad { get; private set; }

		// Token: 0x1700078D RID: 1933
		// (get) Token: 0x060027DF RID: 10207 RVA: 0x000A97C4 File Offset: 0x000A79C4
		// (set) Token: 0x060027E0 RID: 10208 RVA: 0x000A97CC File Offset: 0x000A79CC
		protected InputDevice.AnalogSnapshotEntry[] AnalogSnapshot { get; set; }

		// Token: 0x060027E1 RID: 10209 RVA: 0x000A97D8 File Offset: 0x000A79D8
		internal void OnAttached()
		{
			this.IsAttached = true;
			this.AddAliasControls();
		}

		// Token: 0x060027E2 RID: 10210 RVA: 0x000A97E8 File Offset: 0x000A79E8
		internal void OnDetached()
		{
			this.IsAttached = false;
			this.StopVibration();
			this.RemoveAliasControls();
		}

		// Token: 0x060027E3 RID: 10211 RVA: 0x000A9800 File Offset: 0x000A7A00
		private void AddAliasControls()
		{
			this.RemoveAliasControls();
			if (this.IsKnown)
			{
				this.LeftStick = new TwoAxisInputControl();
				this.RightStick = new TwoAxisInputControl();
				this.DPad = new TwoAxisInputControl();
				this.AddControl(InputControlType.LeftStickX, "Left Stick X");
				this.AddControl(InputControlType.LeftStickY, "Left Stick Y");
				this.AddControl(InputControlType.RightStickX, "Right Stick X");
				this.AddControl(InputControlType.RightStickY, "Right Stick Y");
				this.AddControl(InputControlType.DPadX, "DPad X");
				this.AddControl(InputControlType.DPadY, "DPad Y");
				this.AddControl(InputControlType.Command, "Command");
				this.ExpireControlCache();
			}
		}

		// Token: 0x060027E4 RID: 10212 RVA: 0x000A98BC File Offset: 0x000A7ABC
		private void RemoveAliasControls()
		{
			this.LeftStick = TwoAxisInputControl.Null;
			this.RightStick = TwoAxisInputControl.Null;
			this.DPad = TwoAxisInputControl.Null;
			this.RemoveControl(InputControlType.LeftStickX);
			this.RemoveControl(InputControlType.LeftStickY);
			this.RemoveControl(InputControlType.RightStickX);
			this.RemoveControl(InputControlType.RightStickY);
			this.RemoveControl(InputControlType.DPadX);
			this.RemoveControl(InputControlType.DPadY);
			this.RemoveControl(InputControlType.Command);
			this.ExpireControlCache();
		}

		// Token: 0x060027E5 RID: 10213 RVA: 0x000A9940 File Offset: 0x000A7B40
		protected void ClearControls()
		{
			Array.Clear(this.ControlsByTarget, 0, this.ControlsByTarget.Length);
			this.controls.Clear();
			this.ExpireControlCache();
		}

		// Token: 0x060027E6 RID: 10214 RVA: 0x000A9968 File Offset: 0x000A7B68
		public bool HasControl(InputControlType controlType)
		{
			return this.ControlsByTarget[(int)controlType] != null;
		}

		// Token: 0x060027E7 RID: 10215 RVA: 0x000A9978 File Offset: 0x000A7B78
		public InputControl GetControl(InputControlType controlType)
		{
			InputControl inputControl = this.ControlsByTarget[(int)controlType];
			return inputControl ?? InputControl.Null;
		}

		// Token: 0x1700078E RID: 1934
		public InputControl this[InputControlType controlType]
		{
			get
			{
				return this.GetControl(controlType);
			}
		}

		// Token: 0x060027E9 RID: 10217 RVA: 0x000A99A8 File Offset: 0x000A7BA8
		public static InputControlType GetInputControlTypeByName(string inputControlName)
		{
			return (InputControlType)Enum.Parse(typeof(InputControlType), inputControlName);
		}

		// Token: 0x060027EA RID: 10218 RVA: 0x000A99C0 File Offset: 0x000A7BC0
		public InputControl GetControlByName(string controlName)
		{
			InputControlType inputControlTypeByName = InputDevice.GetInputControlTypeByName(controlName);
			return this.GetControl(inputControlTypeByName);
		}

		// Token: 0x060027EB RID: 10219 RVA: 0x000A99DC File Offset: 0x000A7BDC
		public InputControl AddControl(InputControlType controlType, string handle)
		{
			InputControl inputControl = this.ControlsByTarget[(int)controlType];
			if (inputControl == null)
			{
				inputControl = new InputControl(handle, controlType);
				this.ControlsByTarget[(int)controlType] = inputControl;
				this.controls.Add(inputControl);
				this.ExpireControlCache();
			}
			return inputControl;
		}

		// Token: 0x060027EC RID: 10220 RVA: 0x000A9A1C File Offset: 0x000A7C1C
		public InputControl AddControl(InputControlType controlType, string handle, float lowerDeadZone, float upperDeadZone)
		{
			InputControl inputControl = this.AddControl(controlType, handle);
			inputControl.LowerDeadZone = lowerDeadZone;
			inputControl.UpperDeadZone = upperDeadZone;
			return inputControl;
		}

		// Token: 0x060027ED RID: 10221 RVA: 0x000A9A44 File Offset: 0x000A7C44
		private void RemoveControl(InputControlType controlType)
		{
			InputControl inputControl = this.ControlsByTarget[(int)controlType];
			if (inputControl != null)
			{
				this.ControlsByTarget[(int)controlType] = null;
				this.controls.Remove(inputControl);
				this.ExpireControlCache();
			}
		}

		// Token: 0x060027EE RID: 10222 RVA: 0x000A9A7C File Offset: 0x000A7C7C
		public void ClearInputState()
		{
			this.LeftStick.ClearInputState();
			this.RightStick.ClearInputState();
			this.DPad.ClearInputState();
			int count = this.Controls.Count;
			for (int i = 0; i < count; i++)
			{
				InputControl inputControl = this.Controls[i];
				if (inputControl != null)
				{
					inputControl.ClearInputState();
				}
			}
		}

		// Token: 0x060027EF RID: 10223 RVA: 0x000A9AE4 File Offset: 0x000A7CE4
		protected void UpdateWithState(InputControlType controlType, bool state, ulong updateTick, float deltaTime)
		{
			this.GetControl(controlType).UpdateWithState(state, updateTick, deltaTime);
		}

		// Token: 0x060027F0 RID: 10224 RVA: 0x000A9AF8 File Offset: 0x000A7CF8
		protected void UpdateWithValue(InputControlType controlType, float value, ulong updateTick, float deltaTime)
		{
			this.GetControl(controlType).UpdateWithValue(value, updateTick, deltaTime);
		}

		// Token: 0x060027F1 RID: 10225 RVA: 0x000A9B0C File Offset: 0x000A7D0C
		internal void UpdateLeftStickWithValue(Vector2 value, ulong updateTick, float deltaTime)
		{
			this.LeftStickLeft.UpdateWithValue(Mathf.Max(0f, -value.x), updateTick, deltaTime);
			this.LeftStickRight.UpdateWithValue(Mathf.Max(0f, value.x), updateTick, deltaTime);
			if (InputManager.InvertYAxis)
			{
				this.LeftStickUp.UpdateWithValue(Mathf.Max(0f, -value.y), updateTick, deltaTime);
				this.LeftStickDown.UpdateWithValue(Mathf.Max(0f, value.y), updateTick, deltaTime);
			}
			else
			{
				this.LeftStickUp.UpdateWithValue(Mathf.Max(0f, value.y), updateTick, deltaTime);
				this.LeftStickDown.UpdateWithValue(Mathf.Max(0f, -value.y), updateTick, deltaTime);
			}
		}

		// Token: 0x060027F2 RID: 10226 RVA: 0x000A9BE8 File Offset: 0x000A7DE8
		internal void UpdateLeftStickWithRawValue(Vector2 value, ulong updateTick, float deltaTime)
		{
			this.LeftStickLeft.UpdateWithRawValue(Mathf.Max(0f, -value.x), updateTick, deltaTime);
			this.LeftStickRight.UpdateWithRawValue(Mathf.Max(0f, value.x), updateTick, deltaTime);
			if (InputManager.InvertYAxis)
			{
				this.LeftStickUp.UpdateWithRawValue(Mathf.Max(0f, -value.y), updateTick, deltaTime);
				this.LeftStickDown.UpdateWithRawValue(Mathf.Max(0f, value.y), updateTick, deltaTime);
			}
			else
			{
				this.LeftStickUp.UpdateWithRawValue(Mathf.Max(0f, value.y), updateTick, deltaTime);
				this.LeftStickDown.UpdateWithRawValue(Mathf.Max(0f, -value.y), updateTick, deltaTime);
			}
		}

		// Token: 0x060027F3 RID: 10227 RVA: 0x000A9CC4 File Offset: 0x000A7EC4
		internal void CommitLeftStick()
		{
			this.LeftStickUp.Commit();
			this.LeftStickDown.Commit();
			this.LeftStickLeft.Commit();
			this.LeftStickRight.Commit();
		}

		// Token: 0x060027F4 RID: 10228 RVA: 0x000A9CF4 File Offset: 0x000A7EF4
		internal void UpdateRightStickWithValue(Vector2 value, ulong updateTick, float deltaTime)
		{
			this.RightStickLeft.UpdateWithValue(Mathf.Max(0f, -value.x), updateTick, deltaTime);
			this.RightStickRight.UpdateWithValue(Mathf.Max(0f, value.x), updateTick, deltaTime);
			if (InputManager.InvertYAxis)
			{
				this.RightStickUp.UpdateWithValue(Mathf.Max(0f, -value.y), updateTick, deltaTime);
				this.RightStickDown.UpdateWithValue(Mathf.Max(0f, value.y), updateTick, deltaTime);
			}
			else
			{
				this.RightStickUp.UpdateWithValue(Mathf.Max(0f, value.y), updateTick, deltaTime);
				this.RightStickDown.UpdateWithValue(Mathf.Max(0f, -value.y), updateTick, deltaTime);
			}
		}

		// Token: 0x060027F5 RID: 10229 RVA: 0x000A9DD0 File Offset: 0x000A7FD0
		internal void UpdateRightStickWithRawValue(Vector2 value, ulong updateTick, float deltaTime)
		{
			this.RightStickLeft.UpdateWithRawValue(Mathf.Max(0f, -value.x), updateTick, deltaTime);
			this.RightStickRight.UpdateWithRawValue(Mathf.Max(0f, value.x), updateTick, deltaTime);
			if (InputManager.InvertYAxis)
			{
				this.RightStickUp.UpdateWithRawValue(Mathf.Max(0f, -value.y), updateTick, deltaTime);
				this.RightStickDown.UpdateWithRawValue(Mathf.Max(0f, value.y), updateTick, deltaTime);
			}
			else
			{
				this.RightStickUp.UpdateWithRawValue(Mathf.Max(0f, value.y), updateTick, deltaTime);
				this.RightStickDown.UpdateWithRawValue(Mathf.Max(0f, -value.y), updateTick, deltaTime);
			}
		}

		// Token: 0x060027F6 RID: 10230 RVA: 0x000A9EAC File Offset: 0x000A80AC
		internal void CommitRightStick()
		{
			this.RightStickUp.Commit();
			this.RightStickDown.Commit();
			this.RightStickLeft.Commit();
			this.RightStickRight.Commit();
		}

		// Token: 0x060027F7 RID: 10231 RVA: 0x000A9EDC File Offset: 0x000A80DC
		public virtual void Update(ulong updateTick, float deltaTime)
		{
		}

		// Token: 0x060027F8 RID: 10232 RVA: 0x000A9EE0 File Offset: 0x000A80E0
		private bool AnyCommandControlIsPressed()
		{
			for (int i = 100; i <= 113; i++)
			{
				InputControl inputControl = this.ControlsByTarget[i];
				if (inputControl != null && inputControl.IsPressed)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060027F9 RID: 10233 RVA: 0x000A9F20 File Offset: 0x000A8120
		private void ProcessLeftStick(ulong updateTick, float deltaTime)
		{
			float num = Utility.ValueFromSides(this.LeftStickLeft.NextRawValue, this.LeftStickRight.NextRawValue);
			float num2 = Utility.ValueFromSides(this.LeftStickDown.NextRawValue, this.LeftStickUp.NextRawValue, InputManager.InvertYAxis);
			Vector2 vector;
			if (this.RawSticks || this.LeftStickLeft.Raw || this.LeftStickRight.Raw || this.LeftStickUp.Raw || this.LeftStickDown.Raw)
			{
				vector = new Vector2(num, num2);
			}
			else
			{
				float num3 = Utility.Max(this.LeftStickLeft.LowerDeadZone, this.LeftStickRight.LowerDeadZone, this.LeftStickUp.LowerDeadZone, this.LeftStickDown.LowerDeadZone);
				float num4 = Utility.Min(this.LeftStickLeft.UpperDeadZone, this.LeftStickRight.UpperDeadZone, this.LeftStickUp.UpperDeadZone, this.LeftStickDown.UpperDeadZone);
				vector = Utility.ApplyCircularDeadZone(num, num2, num3, num4);
			}
			this.LeftStick.Raw = true;
			this.LeftStick.UpdateWithAxes(vector.x, vector.y, updateTick, deltaTime);
			this.LeftStickX.Raw = true;
			this.LeftStickX.CommitWithValue(vector.x, updateTick, deltaTime);
			this.LeftStickY.Raw = true;
			this.LeftStickY.CommitWithValue(vector.y, updateTick, deltaTime);
			this.LeftStickLeft.SetValue(this.LeftStick.Left.Value, updateTick);
			this.LeftStickRight.SetValue(this.LeftStick.Right.Value, updateTick);
			this.LeftStickUp.SetValue(this.LeftStick.Up.Value, updateTick);
			this.LeftStickDown.SetValue(this.LeftStick.Down.Value, updateTick);
		}

		// Token: 0x060027FA RID: 10234 RVA: 0x000AA10C File Offset: 0x000A830C
		private void ProcessRightStick(ulong updateTick, float deltaTime)
		{
			float num = Utility.ValueFromSides(this.RightStickLeft.NextRawValue, this.RightStickRight.NextRawValue);
			float num2 = Utility.ValueFromSides(this.RightStickDown.NextRawValue, this.RightStickUp.NextRawValue, InputManager.InvertYAxis);
			Vector2 vector;
			if (this.RawSticks || this.RightStickLeft.Raw || this.RightStickRight.Raw || this.RightStickUp.Raw || this.RightStickDown.Raw)
			{
				vector = new Vector2(num, num2);
			}
			else
			{
				float num3 = Utility.Max(this.RightStickLeft.LowerDeadZone, this.RightStickRight.LowerDeadZone, this.RightStickUp.LowerDeadZone, this.RightStickDown.LowerDeadZone);
				float num4 = Utility.Min(this.RightStickLeft.UpperDeadZone, this.RightStickRight.UpperDeadZone, this.RightStickUp.UpperDeadZone, this.RightStickDown.UpperDeadZone);
				vector = Utility.ApplyCircularDeadZone(num, num2, num3, num4);
			}
			this.RightStick.Raw = true;
			this.RightStick.UpdateWithAxes(vector.x, vector.y, updateTick, deltaTime);
			this.RightStickX.Raw = true;
			this.RightStickX.CommitWithValue(vector.x, updateTick, deltaTime);
			this.RightStickY.Raw = true;
			this.RightStickY.CommitWithValue(vector.y, updateTick, deltaTime);
			this.RightStickLeft.SetValue(this.RightStick.Left.Value, updateTick);
			this.RightStickRight.SetValue(this.RightStick.Right.Value, updateTick);
			this.RightStickUp.SetValue(this.RightStick.Up.Value, updateTick);
			this.RightStickDown.SetValue(this.RightStick.Down.Value, updateTick);
		}

		// Token: 0x060027FB RID: 10235 RVA: 0x000AA2F8 File Offset: 0x000A84F8
		private void ProcessDPad(ulong updateTick, float deltaTime)
		{
			float num = Utility.ValueFromSides(this.DPadLeft.NextRawValue, this.DPadRight.NextRawValue);
			float num2 = Utility.ValueFromSides(this.DPadDown.NextRawValue, this.DPadUp.NextRawValue, InputManager.InvertYAxis);
			Vector2 vector;
			if (this.RawSticks || this.DPadLeft.Raw || this.DPadRight.Raw || this.DPadUp.Raw || this.DPadDown.Raw)
			{
				vector = new Vector2(num, num2);
			}
			else
			{
				float num3 = Utility.Max(this.DPadLeft.LowerDeadZone, this.DPadRight.LowerDeadZone, this.DPadUp.LowerDeadZone, this.DPadDown.LowerDeadZone);
				float num4 = Utility.Min(this.DPadLeft.UpperDeadZone, this.DPadRight.UpperDeadZone, this.DPadUp.UpperDeadZone, this.DPadDown.UpperDeadZone);
				vector = Utility.ApplySeparateDeadZone(num, num2, num3, num4);
			}
			this.DPad.Raw = true;
			this.DPad.UpdateWithAxes(vector.x, vector.y, updateTick, deltaTime);
			this.DPadX.Raw = true;
			this.DPadX.CommitWithValue(vector.x, updateTick, deltaTime);
			this.DPadY.Raw = true;
			this.DPadY.CommitWithValue(vector.y, updateTick, deltaTime);
			this.DPadLeft.SetValue(this.DPad.Left.Value, updateTick);
			this.DPadRight.SetValue(this.DPad.Right.Value, updateTick);
			this.DPadUp.SetValue(this.DPad.Up.Value, updateTick);
			this.DPadDown.SetValue(this.DPad.Down.Value, updateTick);
		}

		// Token: 0x060027FC RID: 10236 RVA: 0x000AA4E4 File Offset: 0x000A86E4
		public void Commit(ulong updateTick, float deltaTime)
		{
			if (this.IsKnown)
			{
				this.ProcessLeftStick(updateTick, deltaTime);
				this.ProcessRightStick(updateTick, deltaTime);
				this.ProcessDPad(updateTick, deltaTime);
			}
			int count = this.Controls.Count;
			for (int i = 0; i < count; i++)
			{
				InputControl inputControl = this.Controls[i];
				if (inputControl != null)
				{
					inputControl.Commit();
					if (inputControl.HasChanged && !inputControl.Passive)
					{
						this.LastChangeTick = updateTick;
						this.LastChangeTime = Time.realtimeSinceStartup;
					}
				}
			}
			if (this.IsKnown)
			{
				this.Command.CommitWithState(this.AnyCommandControlIsPressed(), updateTick, deltaTime);
			}
		}

		// Token: 0x060027FD RID: 10237 RVA: 0x000AA594 File Offset: 0x000A8794
		public bool LastChangeAfterTime(InputDevice device)
		{
			return this.LastChangeTime > device.LastChangeTime + 0.5f;
		}

		// Token: 0x060027FE RID: 10238 RVA: 0x000AA5AC File Offset: 0x000A87AC
		public bool LastChangedAfter(InputDevice device)
		{
			if (Application.platform != RuntimePlatform.PS4 && Application.platform != RuntimePlatform.XboxOne)
			{
				return this.LastChangeTick > device.LastChangeTick + 1UL;
			}
			return device == null || this.LastChangeTick > device.LastChangeTick;
		}

		// Token: 0x060027FF RID: 10239 RVA: 0x000AA5FC File Offset: 0x000A87FC
		internal void RequestActivation()
		{
			this.LastChangeTick = InputManager.CurrentTick;
			this.LastChangeTime = InputManager.CurrentTick;
		}

		// Token: 0x06002800 RID: 10240 RVA: 0x000AA618 File Offset: 0x000A8818
		public virtual void Vibrate(float leftMotor, float rightMotor)
		{
		}

		// Token: 0x06002801 RID: 10241 RVA: 0x000AA61C File Offset: 0x000A881C
		public void Vibrate(float intensity)
		{
			this.Vibrate(intensity, intensity);
		}

		// Token: 0x06002802 RID: 10242 RVA: 0x000AA628 File Offset: 0x000A8828
		public void StopVibration()
		{
			this.Vibrate(0f);
		}

		// Token: 0x06002803 RID: 10243 RVA: 0x000AA638 File Offset: 0x000A8838
		public virtual void SetLightColor(float red, float green, float blue)
		{
		}

		// Token: 0x06002804 RID: 10244 RVA: 0x000AA63C File Offset: 0x000A883C
		public void SetLightColor(Color color)
		{
			this.SetLightColor(color.r * color.a, color.g * color.a, color.b * color.a);
		}

		// Token: 0x06002805 RID: 10245 RVA: 0x000AA674 File Offset: 0x000A8874
		public virtual void SetLightFlash(float flashOnDuration, float flashOffDuration)
		{
		}

		// Token: 0x06002806 RID: 10246 RVA: 0x000AA678 File Offset: 0x000A8878
		public void StopLightFlash()
		{
			this.SetLightFlash(1f, 0f);
		}

		// Token: 0x1700078F RID: 1935
		// (get) Token: 0x06002807 RID: 10247 RVA: 0x000AA68C File Offset: 0x000A888C
		public virtual bool IsSupportedOnThisPlatform
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000790 RID: 1936
		// (get) Token: 0x06002808 RID: 10248 RVA: 0x000AA690 File Offset: 0x000A8890
		public virtual bool IsKnown
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000791 RID: 1937
		// (get) Token: 0x06002809 RID: 10249 RVA: 0x000AA694 File Offset: 0x000A8894
		public bool IsUnknown
		{
			get
			{
				return !this.IsKnown;
			}
		}

		// Token: 0x17000792 RID: 1938
		// (get) Token: 0x0600280A RID: 10250 RVA: 0x000AA6A0 File Offset: 0x000A88A0
		[Obsolete("Use InputDevice.CommandIsPressed instead.", false)]
		public bool MenuIsPressed
		{
			get
			{
				return this.IsKnown && this.Command.IsPressed;
			}
		}

		// Token: 0x17000793 RID: 1939
		// (get) Token: 0x0600280B RID: 10251 RVA: 0x000AA6BC File Offset: 0x000A88BC
		[Obsolete("Use InputDevice.CommandWasPressed instead.", false)]
		public bool MenuWasPressed
		{
			get
			{
				return this.IsKnown && this.Command.WasPressed;
			}
		}

		// Token: 0x17000794 RID: 1940
		// (get) Token: 0x0600280C RID: 10252 RVA: 0x000AA6D8 File Offset: 0x000A88D8
		[Obsolete("Use InputDevice.CommandWasReleased instead.", false)]
		public bool MenuWasReleased
		{
			get
			{
				return this.IsKnown && this.Command.WasReleased;
			}
		}

		// Token: 0x17000795 RID: 1941
		// (get) Token: 0x0600280D RID: 10253 RVA: 0x000AA6F4 File Offset: 0x000A88F4
		public bool CommandIsPressed
		{
			get
			{
				return this.IsKnown && this.Command.IsPressed;
			}
		}

		// Token: 0x17000796 RID: 1942
		// (get) Token: 0x0600280E RID: 10254 RVA: 0x000AA710 File Offset: 0x000A8910
		public bool CommandWasPressed
		{
			get
			{
				return this.IsKnown && this.Command.WasPressed;
			}
		}

		// Token: 0x17000797 RID: 1943
		// (get) Token: 0x0600280F RID: 10255 RVA: 0x000AA72C File Offset: 0x000A892C
		public bool CommandWasReleased
		{
			get
			{
				return this.IsKnown && this.Command.WasReleased;
			}
		}

		// Token: 0x17000798 RID: 1944
		// (get) Token: 0x06002810 RID: 10256 RVA: 0x000AA748 File Offset: 0x000A8948
		public InputControl AnyButton
		{
			get
			{
				int count = this.Controls.Count;
				for (int i = 0; i < count; i++)
				{
					InputControl inputControl = this.Controls[i];
					if (inputControl != null && inputControl.IsButton && inputControl.IsPressed)
					{
						return inputControl;
					}
				}
				return InputControl.Null;
			}
		}

		// Token: 0x17000799 RID: 1945
		// (get) Token: 0x06002811 RID: 10257 RVA: 0x000AA7A4 File Offset: 0x000A89A4
		public bool AnyButtonIsPressed
		{
			get
			{
				int count = this.Controls.Count;
				for (int i = 0; i < count; i++)
				{
					InputControl inputControl = this.Controls[i];
					if (inputControl != null && inputControl.IsButton && inputControl.IsPressed)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x1700079A RID: 1946
		// (get) Token: 0x06002812 RID: 10258 RVA: 0x000AA7FC File Offset: 0x000A89FC
		public bool AnyButtonWasPressed
		{
			get
			{
				int count = this.Controls.Count;
				for (int i = 0; i < count; i++)
				{
					InputControl inputControl = this.Controls[i];
					if (inputControl != null && inputControl.IsButton && inputControl.WasPressed)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x1700079B RID: 1947
		// (get) Token: 0x06002813 RID: 10259 RVA: 0x000AA854 File Offset: 0x000A8A54
		public bool AnyButtonWasReleased
		{
			get
			{
				int count = this.Controls.Count;
				for (int i = 0; i < count; i++)
				{
					InputControl inputControl = this.Controls[i];
					if (inputControl != null && inputControl.IsButton && inputControl.WasReleased)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x1700079C RID: 1948
		// (get) Token: 0x06002814 RID: 10260 RVA: 0x000AA8AC File Offset: 0x000A8AAC
		public TwoAxisInputControl Direction
		{
			get
			{
				return (this.DPad.UpdateTick <= this.LeftStick.UpdateTick) ? this.LeftStick : this.DPad;
			}
		}

		// Token: 0x1700079D RID: 1949
		// (get) Token: 0x06002815 RID: 10261 RVA: 0x000AA8DC File Offset: 0x000A8ADC
		public InputControl LeftStickUp
		{
			get
			{
				InputControl inputControl;
				if ((inputControl = this.cachedLeftStickUp) == null)
				{
					inputControl = (this.cachedLeftStickUp = this.GetControl(InputControlType.LeftStickUp));
				}
				return inputControl;
			}
		}

		// Token: 0x1700079E RID: 1950
		// (get) Token: 0x06002816 RID: 10262 RVA: 0x000AA908 File Offset: 0x000A8B08
		public InputControl LeftStickDown
		{
			get
			{
				InputControl inputControl;
				if ((inputControl = this.cachedLeftStickDown) == null)
				{
					inputControl = (this.cachedLeftStickDown = this.GetControl(InputControlType.LeftStickDown));
				}
				return inputControl;
			}
		}

		// Token: 0x1700079F RID: 1951
		// (get) Token: 0x06002817 RID: 10263 RVA: 0x000AA934 File Offset: 0x000A8B34
		public InputControl LeftStickLeft
		{
			get
			{
				InputControl inputControl;
				if ((inputControl = this.cachedLeftStickLeft) == null)
				{
					inputControl = (this.cachedLeftStickLeft = this.GetControl(InputControlType.LeftStickLeft));
				}
				return inputControl;
			}
		}

		// Token: 0x170007A0 RID: 1952
		// (get) Token: 0x06002818 RID: 10264 RVA: 0x000AA960 File Offset: 0x000A8B60
		public InputControl LeftStickRight
		{
			get
			{
				InputControl inputControl;
				if ((inputControl = this.cachedLeftStickRight) == null)
				{
					inputControl = (this.cachedLeftStickRight = this.GetControl(InputControlType.LeftStickRight));
				}
				return inputControl;
			}
		}

		// Token: 0x170007A1 RID: 1953
		// (get) Token: 0x06002819 RID: 10265 RVA: 0x000AA98C File Offset: 0x000A8B8C
		public InputControl RightStickUp
		{
			get
			{
				InputControl inputControl;
				if ((inputControl = this.cachedRightStickUp) == null)
				{
					inputControl = (this.cachedRightStickUp = this.GetControl(InputControlType.RightStickUp));
				}
				return inputControl;
			}
		}

		// Token: 0x170007A2 RID: 1954
		// (get) Token: 0x0600281A RID: 10266 RVA: 0x000AA9B8 File Offset: 0x000A8BB8
		public InputControl RightStickDown
		{
			get
			{
				InputControl inputControl;
				if ((inputControl = this.cachedRightStickDown) == null)
				{
					inputControl = (this.cachedRightStickDown = this.GetControl(InputControlType.RightStickDown));
				}
				return inputControl;
			}
		}

		// Token: 0x170007A3 RID: 1955
		// (get) Token: 0x0600281B RID: 10267 RVA: 0x000AA9E4 File Offset: 0x000A8BE4
		public InputControl RightStickLeft
		{
			get
			{
				InputControl inputControl;
				if ((inputControl = this.cachedRightStickLeft) == null)
				{
					inputControl = (this.cachedRightStickLeft = this.GetControl(InputControlType.RightStickLeft));
				}
				return inputControl;
			}
		}

		// Token: 0x170007A4 RID: 1956
		// (get) Token: 0x0600281C RID: 10268 RVA: 0x000AAA10 File Offset: 0x000A8C10
		public InputControl RightStickRight
		{
			get
			{
				InputControl inputControl;
				if ((inputControl = this.cachedRightStickRight) == null)
				{
					inputControl = (this.cachedRightStickRight = this.GetControl(InputControlType.RightStickRight));
				}
				return inputControl;
			}
		}

		// Token: 0x170007A5 RID: 1957
		// (get) Token: 0x0600281D RID: 10269 RVA: 0x000AAA3C File Offset: 0x000A8C3C
		public InputControl DPadUp
		{
			get
			{
				InputControl inputControl;
				if ((inputControl = this.cachedDPadUp) == null)
				{
					inputControl = (this.cachedDPadUp = this.GetControl(InputControlType.DPadUp));
				}
				return inputControl;
			}
		}

		// Token: 0x170007A6 RID: 1958
		// (get) Token: 0x0600281E RID: 10270 RVA: 0x000AAA68 File Offset: 0x000A8C68
		public InputControl DPadDown
		{
			get
			{
				InputControl inputControl;
				if ((inputControl = this.cachedDPadDown) == null)
				{
					inputControl = (this.cachedDPadDown = this.GetControl(InputControlType.DPadDown));
				}
				return inputControl;
			}
		}

		// Token: 0x170007A7 RID: 1959
		// (get) Token: 0x0600281F RID: 10271 RVA: 0x000AAA94 File Offset: 0x000A8C94
		public InputControl DPadLeft
		{
			get
			{
				InputControl inputControl;
				if ((inputControl = this.cachedDPadLeft) == null)
				{
					inputControl = (this.cachedDPadLeft = this.GetControl(InputControlType.DPadLeft));
				}
				return inputControl;
			}
		}

		// Token: 0x170007A8 RID: 1960
		// (get) Token: 0x06002820 RID: 10272 RVA: 0x000AAAC0 File Offset: 0x000A8CC0
		public InputControl DPadRight
		{
			get
			{
				InputControl inputControl;
				if ((inputControl = this.cachedDPadRight) == null)
				{
					inputControl = (this.cachedDPadRight = this.GetControl(InputControlType.DPadRight));
				}
				return inputControl;
			}
		}

		// Token: 0x170007A9 RID: 1961
		// (get) Token: 0x06002821 RID: 10273 RVA: 0x000AAAEC File Offset: 0x000A8CEC
		public InputControl Action1
		{
			get
			{
				InputControl inputControl;
				if ((inputControl = this.cachedAction1) == null)
				{
					inputControl = (this.cachedAction1 = this.GetControl(InputControlType.Action1));
				}
				return inputControl;
			}
		}

		// Token: 0x170007AA RID: 1962
		// (get) Token: 0x06002822 RID: 10274 RVA: 0x000AAB18 File Offset: 0x000A8D18
		public InputControl Action2
		{
			get
			{
				InputControl inputControl;
				if ((inputControl = this.cachedAction2) == null)
				{
					inputControl = (this.cachedAction2 = this.GetControl(InputControlType.Action2));
				}
				return inputControl;
			}
		}

		// Token: 0x170007AB RID: 1963
		// (get) Token: 0x06002823 RID: 10275 RVA: 0x000AAB44 File Offset: 0x000A8D44
		public InputControl Action3
		{
			get
			{
				InputControl inputControl;
				if ((inputControl = this.cachedAction3) == null)
				{
					inputControl = (this.cachedAction3 = this.GetControl(InputControlType.Action3));
				}
				return inputControl;
			}
		}

		// Token: 0x170007AC RID: 1964
		// (get) Token: 0x06002824 RID: 10276 RVA: 0x000AAB70 File Offset: 0x000A8D70
		public InputControl Action4
		{
			get
			{
				InputControl inputControl;
				if ((inputControl = this.cachedAction4) == null)
				{
					inputControl = (this.cachedAction4 = this.GetControl(InputControlType.Action4));
				}
				return inputControl;
			}
		}

		// Token: 0x170007AD RID: 1965
		// (get) Token: 0x06002825 RID: 10277 RVA: 0x000AAB9C File Offset: 0x000A8D9C
		public InputControl LeftTrigger
		{
			get
			{
				InputControl inputControl;
				if ((inputControl = this.cachedLeftTrigger) == null)
				{
					inputControl = (this.cachedLeftTrigger = this.GetControl(InputControlType.LeftTrigger));
				}
				return inputControl;
			}
		}

		// Token: 0x170007AE RID: 1966
		// (get) Token: 0x06002826 RID: 10278 RVA: 0x000AABC8 File Offset: 0x000A8DC8
		public InputControl RightTrigger
		{
			get
			{
				InputControl inputControl;
				if ((inputControl = this.cachedRightTrigger) == null)
				{
					inputControl = (this.cachedRightTrigger = this.GetControl(InputControlType.RightTrigger));
				}
				return inputControl;
			}
		}

		// Token: 0x170007AF RID: 1967
		// (get) Token: 0x06002827 RID: 10279 RVA: 0x000AABF4 File Offset: 0x000A8DF4
		public InputControl LeftBumper
		{
			get
			{
				InputControl inputControl;
				if ((inputControl = this.cachedLeftBumper) == null)
				{
					inputControl = (this.cachedLeftBumper = this.GetControl(InputControlType.LeftBumper));
				}
				return inputControl;
			}
		}

		// Token: 0x170007B0 RID: 1968
		// (get) Token: 0x06002828 RID: 10280 RVA: 0x000AAC20 File Offset: 0x000A8E20
		public InputControl RightBumper
		{
			get
			{
				InputControl inputControl;
				if ((inputControl = this.cachedRightBumper) == null)
				{
					inputControl = (this.cachedRightBumper = this.GetControl(InputControlType.RightBumper));
				}
				return inputControl;
			}
		}

		// Token: 0x170007B1 RID: 1969
		// (get) Token: 0x06002829 RID: 10281 RVA: 0x000AAC4C File Offset: 0x000A8E4C
		public InputControl LeftStickButton
		{
			get
			{
				InputControl inputControl;
				if ((inputControl = this.cachedLeftStickButton) == null)
				{
					inputControl = (this.cachedLeftStickButton = this.GetControl(InputControlType.LeftStickButton));
				}
				return inputControl;
			}
		}

		// Token: 0x170007B2 RID: 1970
		// (get) Token: 0x0600282A RID: 10282 RVA: 0x000AAC78 File Offset: 0x000A8E78
		public InputControl RightStickButton
		{
			get
			{
				InputControl inputControl;
				if ((inputControl = this.cachedRightStickButton) == null)
				{
					inputControl = (this.cachedRightStickButton = this.GetControl(InputControlType.RightStickButton));
				}
				return inputControl;
			}
		}

		// Token: 0x170007B3 RID: 1971
		// (get) Token: 0x0600282B RID: 10283 RVA: 0x000AACA4 File Offset: 0x000A8EA4
		public InputControl LeftStickX
		{
			get
			{
				InputControl inputControl;
				if ((inputControl = this.cachedLeftStickX) == null)
				{
					inputControl = (this.cachedLeftStickX = this.GetControl(InputControlType.LeftStickX));
				}
				return inputControl;
			}
		}

		// Token: 0x170007B4 RID: 1972
		// (get) Token: 0x0600282C RID: 10284 RVA: 0x000AACD4 File Offset: 0x000A8ED4
		public InputControl LeftStickY
		{
			get
			{
				InputControl inputControl;
				if ((inputControl = this.cachedLeftStickY) == null)
				{
					inputControl = (this.cachedLeftStickY = this.GetControl(InputControlType.LeftStickY));
				}
				return inputControl;
			}
		}

		// Token: 0x170007B5 RID: 1973
		// (get) Token: 0x0600282D RID: 10285 RVA: 0x000AAD04 File Offset: 0x000A8F04
		public InputControl RightStickX
		{
			get
			{
				InputControl inputControl;
				if ((inputControl = this.cachedRightStickX) == null)
				{
					inputControl = (this.cachedRightStickX = this.GetControl(InputControlType.RightStickX));
				}
				return inputControl;
			}
		}

		// Token: 0x170007B6 RID: 1974
		// (get) Token: 0x0600282E RID: 10286 RVA: 0x000AAD34 File Offset: 0x000A8F34
		public InputControl RightStickY
		{
			get
			{
				InputControl inputControl;
				if ((inputControl = this.cachedRightStickY) == null)
				{
					inputControl = (this.cachedRightStickY = this.GetControl(InputControlType.RightStickY));
				}
				return inputControl;
			}
		}

		// Token: 0x170007B7 RID: 1975
		// (get) Token: 0x0600282F RID: 10287 RVA: 0x000AAD64 File Offset: 0x000A8F64
		public InputControl DPadX
		{
			get
			{
				InputControl inputControl;
				if ((inputControl = this.cachedDPadX) == null)
				{
					inputControl = (this.cachedDPadX = this.GetControl(InputControlType.DPadX));
				}
				return inputControl;
			}
		}

		// Token: 0x170007B8 RID: 1976
		// (get) Token: 0x06002830 RID: 10288 RVA: 0x000AAD94 File Offset: 0x000A8F94
		public InputControl DPadY
		{
			get
			{
				InputControl inputControl;
				if ((inputControl = this.cachedDPadY) == null)
				{
					inputControl = (this.cachedDPadY = this.GetControl(InputControlType.DPadY));
				}
				return inputControl;
			}
		}

		// Token: 0x170007B9 RID: 1977
		// (get) Token: 0x06002831 RID: 10289 RVA: 0x000AADC4 File Offset: 0x000A8FC4
		public InputControl Command
		{
			get
			{
				InputControl inputControl;
				if ((inputControl = this.cachedCommand) == null)
				{
					inputControl = (this.cachedCommand = this.GetControl(InputControlType.Command));
				}
				return inputControl;
			}
		}

		// Token: 0x06002832 RID: 10290 RVA: 0x000AADF4 File Offset: 0x000A8FF4
		private void ExpireControlCache()
		{
			this.cachedLeftStickUp = null;
			this.cachedLeftStickDown = null;
			this.cachedLeftStickLeft = null;
			this.cachedLeftStickRight = null;
			this.cachedRightStickUp = null;
			this.cachedRightStickDown = null;
			this.cachedRightStickLeft = null;
			this.cachedRightStickRight = null;
			this.cachedDPadUp = null;
			this.cachedDPadDown = null;
			this.cachedDPadLeft = null;
			this.cachedDPadRight = null;
			this.cachedAction1 = null;
			this.cachedAction2 = null;
			this.cachedAction3 = null;
			this.cachedAction4 = null;
			this.cachedLeftTrigger = null;
			this.cachedRightTrigger = null;
			this.cachedLeftBumper = null;
			this.cachedRightBumper = null;
			this.cachedLeftStickButton = null;
			this.cachedRightStickButton = null;
			this.cachedLeftStickX = null;
			this.cachedLeftStickY = null;
			this.cachedRightStickX = null;
			this.cachedRightStickY = null;
			this.cachedDPadX = null;
			this.cachedDPadY = null;
			this.cachedCommand = null;
		}

		// Token: 0x170007BA RID: 1978
		// (get) Token: 0x06002833 RID: 10291 RVA: 0x000AAECC File Offset: 0x000A90CC
		internal virtual int NumUnknownAnalogs
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x170007BB RID: 1979
		// (get) Token: 0x06002834 RID: 10292 RVA: 0x000AAED0 File Offset: 0x000A90D0
		internal virtual int NumUnknownButtons
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x06002835 RID: 10293 RVA: 0x000AAED4 File Offset: 0x000A90D4
		internal virtual bool ReadRawButtonState(int index)
		{
			return false;
		}

		// Token: 0x06002836 RID: 10294 RVA: 0x000AAED8 File Offset: 0x000A90D8
		internal virtual float ReadRawAnalogValue(int index)
		{
			return 0f;
		}

		// Token: 0x06002837 RID: 10295 RVA: 0x000AAEE0 File Offset: 0x000A90E0
		internal void TakeSnapshot()
		{
			if (this.AnalogSnapshot == null)
			{
				this.AnalogSnapshot = new InputDevice.AnalogSnapshotEntry[this.NumUnknownAnalogs];
			}
			for (int i = 0; i < this.NumUnknownAnalogs; i++)
			{
				float num = Utility.ApplySnapping(this.ReadRawAnalogValue(i), 0.5f);
				this.AnalogSnapshot[i].value = num;
			}
		}

		// Token: 0x06002838 RID: 10296 RVA: 0x000AAF44 File Offset: 0x000A9144
		internal UnknownDeviceControl GetFirstPressedAnalog()
		{
			if (this.AnalogSnapshot != null)
			{
				for (int i = 0; i < this.NumUnknownAnalogs; i++)
				{
					InputControlType inputControlType = InputControlType.Analog0 + i;
					float num = Utility.ApplySnapping(this.ReadRawAnalogValue(i), 0.5f);
					float num2 = num - this.AnalogSnapshot[i].value;
					this.AnalogSnapshot[i].TrackMinMaxValue(num);
					if (num2 > 0.1f)
					{
						num2 = this.AnalogSnapshot[i].maxValue - this.AnalogSnapshot[i].value;
					}
					if (num2 < -0.1f)
					{
						num2 = this.AnalogSnapshot[i].minValue - this.AnalogSnapshot[i].value;
					}
					if (num2 > 1.9f)
					{
						return new UnknownDeviceControl(inputControlType, InputRangeType.MinusOneToOne);
					}
					if (num2 < -0.9f)
					{
						return new UnknownDeviceControl(inputControlType, InputRangeType.ZeroToMinusOne);
					}
					if (num2 > 0.9f)
					{
						return new UnknownDeviceControl(inputControlType, InputRangeType.ZeroToOne);
					}
				}
			}
			return UnknownDeviceControl.None;
		}

		// Token: 0x06002839 RID: 10297 RVA: 0x000AB050 File Offset: 0x000A9250
		internal UnknownDeviceControl GetFirstPressedButton()
		{
			for (int i = 0; i < this.NumUnknownButtons; i++)
			{
				if (this.ReadRawButtonState(i))
				{
					return new UnknownDeviceControl(InputControlType.Button0 + i, InputRangeType.ZeroToOne);
				}
			}
			return UnknownDeviceControl.None;
		}

		// Token: 0x04001BC4 RID: 7108
		public static readonly InputDevice Null = new InputDevice("None");

		// Token: 0x04001BCB RID: 7115
		private GameOptions.ControllerSymbology m_controllerSymbology = GameOptions.ControllerSymbology.Xbox;

		// Token: 0x04001BD0 RID: 7120
		private List<InputControl> controls;

		// Token: 0x04001BD6 RID: 7126
		public bool Passive;

		// Token: 0x04001BD8 RID: 7128
		private InputControl cachedLeftStickUp;

		// Token: 0x04001BD9 RID: 7129
		private InputControl cachedLeftStickDown;

		// Token: 0x04001BDA RID: 7130
		private InputControl cachedLeftStickLeft;

		// Token: 0x04001BDB RID: 7131
		private InputControl cachedLeftStickRight;

		// Token: 0x04001BDC RID: 7132
		private InputControl cachedRightStickUp;

		// Token: 0x04001BDD RID: 7133
		private InputControl cachedRightStickDown;

		// Token: 0x04001BDE RID: 7134
		private InputControl cachedRightStickLeft;

		// Token: 0x04001BDF RID: 7135
		private InputControl cachedRightStickRight;

		// Token: 0x04001BE0 RID: 7136
		private InputControl cachedDPadUp;

		// Token: 0x04001BE1 RID: 7137
		private InputControl cachedDPadDown;

		// Token: 0x04001BE2 RID: 7138
		private InputControl cachedDPadLeft;

		// Token: 0x04001BE3 RID: 7139
		private InputControl cachedDPadRight;

		// Token: 0x04001BE4 RID: 7140
		private InputControl cachedAction1;

		// Token: 0x04001BE5 RID: 7141
		private InputControl cachedAction2;

		// Token: 0x04001BE6 RID: 7142
		private InputControl cachedAction3;

		// Token: 0x04001BE7 RID: 7143
		private InputControl cachedAction4;

		// Token: 0x04001BE8 RID: 7144
		private InputControl cachedLeftTrigger;

		// Token: 0x04001BE9 RID: 7145
		private InputControl cachedRightTrigger;

		// Token: 0x04001BEA RID: 7146
		private InputControl cachedLeftBumper;

		// Token: 0x04001BEB RID: 7147
		private InputControl cachedRightBumper;

		// Token: 0x04001BEC RID: 7148
		private InputControl cachedLeftStickButton;

		// Token: 0x04001BED RID: 7149
		private InputControl cachedRightStickButton;

		// Token: 0x04001BEE RID: 7150
		private InputControl cachedLeftStickX;

		// Token: 0x04001BEF RID: 7151
		private InputControl cachedLeftStickY;

		// Token: 0x04001BF0 RID: 7152
		private InputControl cachedRightStickX;

		// Token: 0x04001BF1 RID: 7153
		private InputControl cachedRightStickY;

		// Token: 0x04001BF2 RID: 7154
		private InputControl cachedDPadX;

		// Token: 0x04001BF3 RID: 7155
		private InputControl cachedDPadY;

		// Token: 0x04001BF4 RID: 7156
		private InputControl cachedCommand;

		// Token: 0x020006B0 RID: 1712
		protected struct AnalogSnapshotEntry
		{
			// Token: 0x0600283B RID: 10299 RVA: 0x000AB0A8 File Offset: 0x000A92A8
			public void TrackMinMaxValue(float currentValue)
			{
				this.maxValue = Mathf.Max(this.maxValue, currentValue);
				this.minValue = Mathf.Min(this.minValue, currentValue);
			}

			// Token: 0x04001BF5 RID: 7157
			public float value;

			// Token: 0x04001BF6 RID: 7158
			public float maxValue;

			// Token: 0x04001BF7 RID: 7159
			public float minValue;
		}
	}
}
