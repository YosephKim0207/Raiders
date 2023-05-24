using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x020008F5 RID: 2293
	[Tooltip("Sends events based on the direction of Input Axis (Left/Right/Up/Down...).")]
	[ActionCategory(ActionCategory.Input)]
	public class AxisEvent : FsmStateAction
	{
		// Token: 0x06003298 RID: 12952 RVA: 0x00109D5C File Offset: 0x00107F5C
		public override void Reset()
		{
			this.horizontalAxis = "Horizontal";
			this.verticalAxis = "Vertical";
			this.leftEvent = null;
			this.rightEvent = null;
			this.upEvent = null;
			this.downEvent = null;
			this.anyDirection = null;
			this.noDirection = null;
		}

		// Token: 0x06003299 RID: 12953 RVA: 0x00109DB4 File Offset: 0x00107FB4
		public override void OnUpdate()
		{
			float num = ((!(this.horizontalAxis.Value != string.Empty)) ? 0f : Input.GetAxis(this.horizontalAxis.Value));
			float num2 = ((!(this.verticalAxis.Value != string.Empty)) ? 0f : Input.GetAxis(this.verticalAxis.Value));
			if ((num * num + num2 * num2).Equals(0f))
			{
				if (this.noDirection != null)
				{
					base.Fsm.Event(this.noDirection);
				}
				return;
			}
			float num3 = Mathf.Atan2(num2, num) * 57.29578f + 45f;
			if (num3 < 0f)
			{
				num3 += 360f;
			}
			int num4 = (int)(num3 / 90f);
			if (num4 == 0 && this.rightEvent != null)
			{
				base.Fsm.Event(this.rightEvent);
			}
			else if (num4 == 1 && this.upEvent != null)
			{
				base.Fsm.Event(this.upEvent);
			}
			else if (num4 == 2 && this.leftEvent != null)
			{
				base.Fsm.Event(this.leftEvent);
			}
			else if (num4 == 3 && this.downEvent != null)
			{
				base.Fsm.Event(this.downEvent);
			}
			else if (this.anyDirection != null)
			{
				base.Fsm.Event(this.anyDirection);
			}
		}

		// Token: 0x040023B8 RID: 9144
		[Tooltip("Horizontal axis as defined in the Input Manager")]
		public FsmString horizontalAxis;

		// Token: 0x040023B9 RID: 9145
		[Tooltip("Vertical axis as defined in the Input Manager")]
		public FsmString verticalAxis;

		// Token: 0x040023BA RID: 9146
		[Tooltip("Event to send if input is to the left.")]
		public FsmEvent leftEvent;

		// Token: 0x040023BB RID: 9147
		[Tooltip("Event to send if input is to the right.")]
		public FsmEvent rightEvent;

		// Token: 0x040023BC RID: 9148
		[Tooltip("Event to send if input is to the up.")]
		public FsmEvent upEvent;

		// Token: 0x040023BD RID: 9149
		[Tooltip("Event to send if input is to the down.")]
		public FsmEvent downEvent;

		// Token: 0x040023BE RID: 9150
		[Tooltip("Event to send if input is in any direction.")]
		public FsmEvent anyDirection;

		// Token: 0x040023BF RID: 9151
		[Tooltip("Event to send if no axis input (centered).")]
		public FsmEvent noDirection;
	}
}
