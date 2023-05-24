using System;
using InControl;
using UnityEngine;

namespace VirtualDeviceExample
{
	// Token: 0x02000684 RID: 1668
	public class VirtualDevice : InputDevice
	{
		// Token: 0x060025F5 RID: 9717 RVA: 0x000A2ACC File Offset: 0x000A0CCC
		public VirtualDevice()
			: base("Virtual Controller")
		{
			base.AddControl(InputControlType.LeftStickLeft, "Left Stick Left");
			base.AddControl(InputControlType.LeftStickRight, "Left Stick Right");
			base.AddControl(InputControlType.LeftStickUp, "Left Stick Up");
			base.AddControl(InputControlType.LeftStickDown, "Left Stick Down");
			base.AddControl(InputControlType.RightStickLeft, "Right Stick Left");
			base.AddControl(InputControlType.RightStickRight, "Right Stick Right");
			base.AddControl(InputControlType.RightStickUp, "Right Stick Up");
			base.AddControl(InputControlType.RightStickDown, "Right Stick Down");
			base.AddControl(InputControlType.Action1, "A");
			base.AddControl(InputControlType.Action2, "B");
			base.AddControl(InputControlType.Action3, "X");
			base.AddControl(InputControlType.Action4, "Y");
		}

		// Token: 0x060025F6 RID: 9718 RVA: 0x000A2B88 File Offset: 0x000A0D88
		public override void Update(ulong updateTick, float deltaTime)
		{
			Vector2 vectorFromKeyboard = this.GetVectorFromKeyboard(deltaTime, true);
			base.UpdateLeftStickWithValue(vectorFromKeyboard, updateTick, deltaTime);
			Vector2 vectorFromMouse = this.GetVectorFromMouse(deltaTime, true);
			base.UpdateRightStickWithRawValue(vectorFromMouse, updateTick, deltaTime);
			base.UpdateWithState(InputControlType.Action1, Input.GetKey(KeyCode.Space), updateTick, deltaTime);
			base.UpdateWithState(InputControlType.Action2, Input.GetKey(KeyCode.S), updateTick, deltaTime);
			base.UpdateWithState(InputControlType.Action3, Input.GetKey(KeyCode.D), updateTick, deltaTime);
			base.UpdateWithState(InputControlType.Action4, Input.GetKey(KeyCode.F), updateTick, deltaTime);
			base.Commit(updateTick, deltaTime);
		}

		// Token: 0x060025F7 RID: 9719 RVA: 0x000A2C08 File Offset: 0x000A0E08
		private Vector2 GetVectorFromKeyboard(float deltaTime, bool smoothed)
		{
			if (smoothed)
			{
				this.kx = this.ApplySmoothing(this.kx, this.GetXFromKeyboard(), deltaTime, 0.1f);
				this.ky = this.ApplySmoothing(this.ky, this.GetYFromKeyboard(), deltaTime, 0.1f);
			}
			else
			{
				this.kx = this.GetXFromKeyboard();
				this.ky = this.GetYFromKeyboard();
			}
			return new Vector2(this.kx, this.ky);
		}

		// Token: 0x060025F8 RID: 9720 RVA: 0x000A2C88 File Offset: 0x000A0E88
		private float GetXFromKeyboard()
		{
			float num = ((!Input.GetKey(KeyCode.LeftArrow)) ? 0f : (-1f));
			float num2 = ((!Input.GetKey(KeyCode.RightArrow)) ? 0f : 1f);
			return num + num2;
		}

		// Token: 0x060025F9 RID: 9721 RVA: 0x000A2CD8 File Offset: 0x000A0ED8
		private float GetYFromKeyboard()
		{
			float num = ((!Input.GetKey(KeyCode.UpArrow)) ? 0f : 1f);
			float num2 = ((!Input.GetKey(KeyCode.DownArrow)) ? 0f : (-1f));
			return num + num2;
		}

		// Token: 0x060025FA RID: 9722 RVA: 0x000A2D28 File Offset: 0x000A0F28
		private Vector2 GetVectorFromMouse(float deltaTime, bool smoothed)
		{
			if (smoothed)
			{
				this.mx = this.ApplySmoothing(this.mx, Input.GetAxisRaw("mouse x") * 0.05f, deltaTime, 0.1f);
				this.my = this.ApplySmoothing(this.my, Input.GetAxisRaw("mouse y") * 0.05f, deltaTime, 0.1f);
			}
			else
			{
				this.mx = Input.GetAxisRaw("mouse x") * 0.05f;
				this.my = Input.GetAxisRaw("mouse y") * 0.05f;
			}
			return new Vector2(this.mx, this.my);
		}

		// Token: 0x060025FB RID: 9723 RVA: 0x000A2DD0 File Offset: 0x000A0FD0
		private float ApplySmoothing(float lastValue, float thisValue, float deltaTime, float sensitivity)
		{
			sensitivity = Mathf.Clamp(sensitivity, 0.001f, 1f);
			if (Mathf.Approximately(sensitivity, 1f))
			{
				return thisValue;
			}
			return Mathf.Lerp(lastValue, thisValue, deltaTime * sensitivity * 100f);
		}

		// Token: 0x040019D2 RID: 6610
		private const float sensitivity = 0.1f;

		// Token: 0x040019D3 RID: 6611
		private const float mouseScale = 0.05f;

		// Token: 0x040019D4 RID: 6612
		private float kx;

		// Token: 0x040019D5 RID: 6613
		private float ky;

		// Token: 0x040019D6 RID: 6614
		private float mx;

		// Token: 0x040019D7 RID: 6615
		private float my;
	}
}
