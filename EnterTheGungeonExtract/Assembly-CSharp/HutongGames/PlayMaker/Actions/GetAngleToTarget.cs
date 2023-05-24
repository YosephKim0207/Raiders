using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000966 RID: 2406
	[ActionCategory(ActionCategory.Transform)]
	[Tooltip("Gets the Angle between a GameObject's forward axis and a Target. The Target can be defined as a GameObject or a world Position. If you specify both, then the Position will be used as a local offset from the Target Object's position.")]
	public class GetAngleToTarget : FsmStateAction
	{
		// Token: 0x0600347B RID: 13435 RVA: 0x001102A4 File Offset: 0x0010E4A4
		public override void Reset()
		{
			this.gameObject = null;
			this.targetObject = null;
			this.targetPosition = new FsmVector3
			{
				UseVariable = true
			};
			this.ignoreHeight = true;
			this.storeAngle = null;
			this.everyFrame = false;
		}

		// Token: 0x0600347C RID: 13436 RVA: 0x001102F0 File Offset: 0x0010E4F0
		public override void OnLateUpdate()
		{
			this.DoGetAngleToTarget();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x0600347D RID: 13437 RVA: 0x0011030C File Offset: 0x0010E50C
		private void DoGetAngleToTarget()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
			if (ownerDefaultTarget == null)
			{
				return;
			}
			GameObject value = this.targetObject.Value;
			if (value == null && this.targetPosition.IsNone)
			{
				return;
			}
			Vector3 vector;
			if (value != null)
			{
				vector = (this.targetPosition.IsNone ? value.transform.position : value.transform.TransformPoint(this.targetPosition.Value));
			}
			else
			{
				vector = this.targetPosition.Value;
			}
			if (this.ignoreHeight.Value)
			{
				vector.y = ownerDefaultTarget.transform.position.y;
			}
			Vector3 vector2 = vector - ownerDefaultTarget.transform.position;
			this.storeAngle.Value = Vector3.Angle(vector2, ownerDefaultTarget.transform.forward);
		}

		// Token: 0x040025A7 RID: 9639
		[Tooltip("The game object whose forward axis we measure from. If the target is dead ahead the angle will be 0.")]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		// Token: 0x040025A8 RID: 9640
		[Tooltip("The target object to measure the angle to. Or use target position.")]
		public FsmGameObject targetObject;

		// Token: 0x040025A9 RID: 9641
		[Tooltip("The world position to measure an angle to. If Target Object is also specified, this vector is used as an offset from that object's position.")]
		public FsmVector3 targetPosition;

		// Token: 0x040025AA RID: 9642
		[Tooltip("Ignore height differences when calculating the angle.")]
		public FsmBool ignoreHeight;

		// Token: 0x040025AB RID: 9643
		[RequiredField]
		[UIHint(UIHint.Variable)]
		[Tooltip("Store the angle in a float variable.")]
		public FsmFloat storeAngle;

		// Token: 0x040025AC RID: 9644
		[Tooltip("Repeat every frame.")]
		public bool everyFrame;
	}
}
