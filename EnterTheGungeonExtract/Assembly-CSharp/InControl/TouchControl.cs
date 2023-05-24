using System;
using UnityEngine;

namespace InControl
{
	// Token: 0x02000763 RID: 1891
	public abstract class TouchControl : MonoBehaviour
	{
		// Token: 0x06002A28 RID: 10792
		public abstract void CreateControl();

		// Token: 0x06002A29 RID: 10793
		public abstract void DestroyControl();

		// Token: 0x06002A2A RID: 10794
		public abstract void ConfigureControl();

		// Token: 0x06002A2B RID: 10795
		public abstract void SubmitControlState(ulong updateTick, float deltaTime);

		// Token: 0x06002A2C RID: 10796
		public abstract void CommitControlState(ulong updateTick, float deltaTime);

		// Token: 0x06002A2D RID: 10797
		public abstract void TouchBegan(Touch touch);

		// Token: 0x06002A2E RID: 10798
		public abstract void TouchMoved(Touch touch);

		// Token: 0x06002A2F RID: 10799
		public abstract void TouchEnded(Touch touch);

		// Token: 0x06002A30 RID: 10800
		public abstract void DrawGizmos();

		// Token: 0x06002A31 RID: 10801 RVA: 0x000BFF4C File Offset: 0x000BE14C
		private void OnEnable()
		{
			TouchManager.OnSetup += this.Setup;
		}

		// Token: 0x06002A32 RID: 10802 RVA: 0x000BFF60 File Offset: 0x000BE160
		private void OnDisable()
		{
			this.DestroyControl();
			Resources.UnloadUnusedAssets();
		}

		// Token: 0x06002A33 RID: 10803 RVA: 0x000BFF70 File Offset: 0x000BE170
		private void Setup()
		{
			if (!base.enabled)
			{
				return;
			}
			this.CreateControl();
			this.ConfigureControl();
		}

		// Token: 0x06002A34 RID: 10804 RVA: 0x000BFF8C File Offset: 0x000BE18C
		protected Vector3 OffsetToWorldPosition(TouchControlAnchor anchor, Vector2 offset, TouchUnitType offsetUnitType, bool lockAspectRatio)
		{
			Vector3 vector;
			if (offsetUnitType == TouchUnitType.Pixels)
			{
				vector = TouchUtility.RoundVector(offset) * TouchManager.PixelToWorld;
			}
			else if (lockAspectRatio)
			{
				vector = offset * TouchManager.PercentToWorld;
			}
			else
			{
				vector = Vector3.Scale(offset, TouchManager.ViewSize);
			}
			return TouchManager.ViewToWorldPoint(TouchUtility.AnchorToViewPoint(anchor)) + vector;
		}

		// Token: 0x06002A35 RID: 10805 RVA: 0x000BFFFC File Offset: 0x000BE1FC
		protected void SubmitButtonState(TouchControl.ButtonTarget target, bool state, ulong updateTick, float deltaTime)
		{
			if (TouchManager.Device == null || target == TouchControl.ButtonTarget.None)
			{
				return;
			}
			InputControl control = TouchManager.Device.GetControl((InputControlType)target);
			if (control != null && control != InputControl.Null)
			{
				control.UpdateWithState(state, updateTick, deltaTime);
			}
		}

		// Token: 0x06002A36 RID: 10806 RVA: 0x000C0044 File Offset: 0x000BE244
		protected void SubmitButtonValue(TouchControl.ButtonTarget target, float value, ulong updateTick, float deltaTime)
		{
			if (TouchManager.Device == null || target == TouchControl.ButtonTarget.None)
			{
				return;
			}
			InputControl control = TouchManager.Device.GetControl((InputControlType)target);
			if (control != null && control != InputControl.Null)
			{
				control.UpdateWithValue(value, updateTick, deltaTime);
			}
		}

		// Token: 0x06002A37 RID: 10807 RVA: 0x000C008C File Offset: 0x000BE28C
		protected void CommitButton(TouchControl.ButtonTarget target)
		{
			if (TouchManager.Device == null || target == TouchControl.ButtonTarget.None)
			{
				return;
			}
			InputControl control = TouchManager.Device.GetControl((InputControlType)target);
			if (control != null && control != InputControl.Null)
			{
				control.Commit();
			}
		}

		// Token: 0x06002A38 RID: 10808 RVA: 0x000C00D0 File Offset: 0x000BE2D0
		protected void SubmitAnalogValue(TouchControl.AnalogTarget target, Vector2 value, float lowerDeadZone, float upperDeadZone, ulong updateTick, float deltaTime)
		{
			if (TouchManager.Device == null || target == TouchControl.AnalogTarget.None)
			{
				return;
			}
			Vector2 vector = Utility.ApplyCircularDeadZone(value, lowerDeadZone, upperDeadZone);
			if (target == TouchControl.AnalogTarget.LeftStick || target == TouchControl.AnalogTarget.Both)
			{
				TouchManager.Device.UpdateLeftStickWithValue(vector, updateTick, deltaTime);
			}
			if (target == TouchControl.AnalogTarget.RightStick || target == TouchControl.AnalogTarget.Both)
			{
				TouchManager.Device.UpdateRightStickWithValue(vector, updateTick, deltaTime);
			}
		}

		// Token: 0x06002A39 RID: 10809 RVA: 0x000C0134 File Offset: 0x000BE334
		protected void CommitAnalog(TouchControl.AnalogTarget target)
		{
			if (TouchManager.Device == null || target == TouchControl.AnalogTarget.None)
			{
				return;
			}
			if (target == TouchControl.AnalogTarget.LeftStick || target == TouchControl.AnalogTarget.Both)
			{
				TouchManager.Device.CommitLeftStick();
			}
			if (target == TouchControl.AnalogTarget.RightStick || target == TouchControl.AnalogTarget.Both)
			{
				TouchManager.Device.CommitRightStick();
			}
		}

		// Token: 0x06002A3A RID: 10810 RVA: 0x000C0184 File Offset: 0x000BE384
		protected void SubmitRawAnalogValue(TouchControl.AnalogTarget target, Vector2 rawValue, ulong updateTick, float deltaTime)
		{
			if (TouchManager.Device == null || target == TouchControl.AnalogTarget.None)
			{
				return;
			}
			if (target == TouchControl.AnalogTarget.LeftStick || target == TouchControl.AnalogTarget.Both)
			{
				TouchManager.Device.UpdateLeftStickWithRawValue(rawValue, updateTick, deltaTime);
			}
			if (target == TouchControl.AnalogTarget.RightStick || target == TouchControl.AnalogTarget.Both)
			{
				TouchManager.Device.UpdateRightStickWithRawValue(rawValue, updateTick, deltaTime);
			}
		}

		// Token: 0x06002A3B RID: 10811 RVA: 0x000C01DC File Offset: 0x000BE3DC
		protected static Vector3 SnapTo(Vector2 vector, TouchControl.SnapAngles snapAngles)
		{
			if (snapAngles == TouchControl.SnapAngles.None)
			{
				return vector;
			}
			float num = 360f / (float)snapAngles;
			return TouchControl.SnapTo(vector, num);
		}

		// Token: 0x06002A3C RID: 10812 RVA: 0x000C0208 File Offset: 0x000BE408
		protected static Vector3 SnapTo(Vector2 vector, float snapAngle)
		{
			float num = Vector2.Angle(vector, Vector2.up);
			if (num < snapAngle / 2f)
			{
				return Vector2.up * vector.magnitude;
			}
			if (num > 180f - snapAngle / 2f)
			{
				return -Vector2.up * vector.magnitude;
			}
			float num2 = Mathf.Round(num / snapAngle);
			float num3 = num2 * snapAngle - num;
			Vector3 vector2 = Vector3.Cross(Vector2.up, vector);
			Quaternion quaternion = Quaternion.AngleAxis(num3, vector2);
			return quaternion * vector;
		}

		// Token: 0x06002A3D RID: 10813 RVA: 0x000C02B0 File Offset: 0x000BE4B0
		private void OnDrawGizmosSelected()
		{
			if (!base.enabled)
			{
				return;
			}
			if (TouchManager.ControlsShowGizmos != TouchManager.GizmoShowOption.WhenSelected)
			{
				return;
			}
			if (Utility.GameObjectIsCulledOnCurrentCamera(base.gameObject))
			{
				return;
			}
			if (!Application.isPlaying)
			{
				this.ConfigureControl();
			}
			this.DrawGizmos();
		}

		// Token: 0x06002A3E RID: 10814 RVA: 0x000C02FC File Offset: 0x000BE4FC
		private void OnDrawGizmos()
		{
			if (!base.enabled)
			{
				return;
			}
			if (TouchManager.ControlsShowGizmos == TouchManager.GizmoShowOption.UnlessPlaying)
			{
				if (Application.isPlaying)
				{
					return;
				}
			}
			else if (TouchManager.ControlsShowGizmos != TouchManager.GizmoShowOption.Always)
			{
				return;
			}
			if (Utility.GameObjectIsCulledOnCurrentCamera(base.gameObject))
			{
				return;
			}
			if (!Application.isPlaying)
			{
				this.ConfigureControl();
			}
			this.DrawGizmos();
		}

		// Token: 0x02000764 RID: 1892
		public enum ButtonTarget
		{
			// Token: 0x04001D08 RID: 7432
			None,
			// Token: 0x04001D09 RID: 7433
			DPadDown = 12,
			// Token: 0x04001D0A RID: 7434
			DPadLeft,
			// Token: 0x04001D0B RID: 7435
			DPadRight,
			// Token: 0x04001D0C RID: 7436
			DPadUp = 11,
			// Token: 0x04001D0D RID: 7437
			LeftTrigger = 15,
			// Token: 0x04001D0E RID: 7438
			RightTrigger,
			// Token: 0x04001D0F RID: 7439
			LeftBumper,
			// Token: 0x04001D10 RID: 7440
			RightBumper,
			// Token: 0x04001D11 RID: 7441
			Action1,
			// Token: 0x04001D12 RID: 7442
			Action2,
			// Token: 0x04001D13 RID: 7443
			Action3,
			// Token: 0x04001D14 RID: 7444
			Action4,
			// Token: 0x04001D15 RID: 7445
			Action5,
			// Token: 0x04001D16 RID: 7446
			Action6,
			// Token: 0x04001D17 RID: 7447
			Action7,
			// Token: 0x04001D18 RID: 7448
			Action8,
			// Token: 0x04001D19 RID: 7449
			Action9,
			// Token: 0x04001D1A RID: 7450
			Action10,
			// Token: 0x04001D1B RID: 7451
			Action11,
			// Token: 0x04001D1C RID: 7452
			Action12,
			// Token: 0x04001D1D RID: 7453
			Menu = 106,
			// Token: 0x04001D1E RID: 7454
			Button0 = 500,
			// Token: 0x04001D1F RID: 7455
			Button1,
			// Token: 0x04001D20 RID: 7456
			Button2,
			// Token: 0x04001D21 RID: 7457
			Button3,
			// Token: 0x04001D22 RID: 7458
			Button4,
			// Token: 0x04001D23 RID: 7459
			Button5,
			// Token: 0x04001D24 RID: 7460
			Button6,
			// Token: 0x04001D25 RID: 7461
			Button7,
			// Token: 0x04001D26 RID: 7462
			Button8,
			// Token: 0x04001D27 RID: 7463
			Button9,
			// Token: 0x04001D28 RID: 7464
			Button10,
			// Token: 0x04001D29 RID: 7465
			Button11,
			// Token: 0x04001D2A RID: 7466
			Button12,
			// Token: 0x04001D2B RID: 7467
			Button13,
			// Token: 0x04001D2C RID: 7468
			Button14,
			// Token: 0x04001D2D RID: 7469
			Button15,
			// Token: 0x04001D2E RID: 7470
			Button16,
			// Token: 0x04001D2F RID: 7471
			Button17,
			// Token: 0x04001D30 RID: 7472
			Button18,
			// Token: 0x04001D31 RID: 7473
			Button19
		}

		// Token: 0x02000765 RID: 1893
		public enum AnalogTarget
		{
			// Token: 0x04001D33 RID: 7475
			None,
			// Token: 0x04001D34 RID: 7476
			LeftStick,
			// Token: 0x04001D35 RID: 7477
			RightStick,
			// Token: 0x04001D36 RID: 7478
			Both
		}

		// Token: 0x02000766 RID: 1894
		public enum SnapAngles
		{
			// Token: 0x04001D38 RID: 7480
			None,
			// Token: 0x04001D39 RID: 7481
			Four = 4,
			// Token: 0x04001D3A RID: 7482
			Eight = 8,
			// Token: 0x04001D3B RID: 7483
			Sixteen = 16
		}
	}
}
