using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000976 RID: 2422
	[ActionCategory(ActionCategory.Device)]
	[Tooltip("Gets the rotation of the device around its z axis (into the screen). For example when you steer with the iPhone in a driving game.")]
	public class GetDeviceRoll : FsmStateAction
	{
		// Token: 0x060034BB RID: 13499 RVA: 0x0011117C File Offset: 0x0010F37C
		public override void Reset()
		{
			this.baseOrientation = GetDeviceRoll.BaseOrientation.LandscapeLeft;
			this.storeAngle = null;
			this.limitAngle = new FsmFloat
			{
				UseVariable = true
			};
			this.smoothing = 5f;
			this.everyFrame = true;
		}

		// Token: 0x060034BC RID: 13500 RVA: 0x001111C4 File Offset: 0x0010F3C4
		public override void OnEnter()
		{
			this.DoGetDeviceRoll();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x060034BD RID: 13501 RVA: 0x001111E0 File Offset: 0x0010F3E0
		public override void OnUpdate()
		{
			this.DoGetDeviceRoll();
		}

		// Token: 0x060034BE RID: 13502 RVA: 0x001111E8 File Offset: 0x0010F3E8
		private void DoGetDeviceRoll()
		{
			float x = Input.acceleration.x;
			float y = Input.acceleration.y;
			float num = 0f;
			GetDeviceRoll.BaseOrientation baseOrientation = this.baseOrientation;
			if (baseOrientation != GetDeviceRoll.BaseOrientation.Portrait)
			{
				if (baseOrientation != GetDeviceRoll.BaseOrientation.LandscapeLeft)
				{
					if (baseOrientation == GetDeviceRoll.BaseOrientation.LandscapeRight)
					{
						num = -Mathf.Atan2(y, x);
					}
				}
				else
				{
					num = Mathf.Atan2(y, -x);
				}
			}
			else
			{
				num = -Mathf.Atan2(x, -y);
			}
			if (!this.limitAngle.IsNone)
			{
				num = Mathf.Clamp(57.29578f * num, -this.limitAngle.Value, this.limitAngle.Value);
			}
			if (this.smoothing.Value > 0f)
			{
				num = Mathf.LerpAngle(this.lastZAngle, num, this.smoothing.Value * Time.deltaTime);
			}
			this.lastZAngle = num;
			this.storeAngle.Value = num;
		}

		// Token: 0x040025F1 RID: 9713
		[Tooltip("How the user is expected to hold the device (where angle will be zero).")]
		public GetDeviceRoll.BaseOrientation baseOrientation;

		// Token: 0x040025F2 RID: 9714
		[UIHint(UIHint.Variable)]
		public FsmFloat storeAngle;

		// Token: 0x040025F3 RID: 9715
		public FsmFloat limitAngle;

		// Token: 0x040025F4 RID: 9716
		public FsmFloat smoothing;

		// Token: 0x040025F5 RID: 9717
		public bool everyFrame;

		// Token: 0x040025F6 RID: 9718
		private float lastZAngle;

		// Token: 0x02000977 RID: 2423
		public enum BaseOrientation
		{
			// Token: 0x040025F8 RID: 9720
			Portrait,
			// Token: 0x040025F9 RID: 9721
			LandscapeLeft,
			// Token: 0x040025FA RID: 9722
			LandscapeRight
		}
	}
}
