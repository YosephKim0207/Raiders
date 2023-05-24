using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000A20 RID: 2592
	[ActionCategory(ActionCategory.Transform)]
	[Tooltip("Moves a Game Object towards a Target. Optionally sends an event when successful. The Target can be specified as a Game Object or a world Position. If you specify both, then the Position is used as a local offset from the Object's Position.")]
	public class MoveTowards : FsmStateAction
	{
		// Token: 0x06003780 RID: 14208 RVA: 0x0011E2A4 File Offset: 0x0011C4A4
		public override void Reset()
		{
			this.gameObject = null;
			this.targetObject = null;
			this.maxSpeed = 10f;
			this.finishDistance = 1f;
			this.finishEvent = null;
		}

		// Token: 0x06003781 RID: 14209 RVA: 0x0011E2DC File Offset: 0x0011C4DC
		public override void OnUpdate()
		{
			this.DoMoveTowards();
		}

		// Token: 0x06003782 RID: 14210 RVA: 0x0011E2E4 File Offset: 0x0011C4E4
		private void DoMoveTowards()
		{
			if (!this.UpdateTargetPos())
			{
				return;
			}
			this.go.transform.position = Vector3.MoveTowards(this.go.transform.position, this.targetPos, this.maxSpeed.Value * Time.deltaTime);
			float magnitude = (this.go.transform.position - this.targetPos).magnitude;
			if (magnitude < this.finishDistance.Value)
			{
				base.Fsm.Event(this.finishEvent);
				base.Finish();
			}
		}

		// Token: 0x06003783 RID: 14211 RVA: 0x0011E388 File Offset: 0x0011C588
		public bool UpdateTargetPos()
		{
			this.go = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
			if (this.go == null)
			{
				return false;
			}
			this.goTarget = this.targetObject.Value;
			if (this.goTarget == null && this.targetPosition.IsNone)
			{
				return false;
			}
			if (this.goTarget != null)
			{
				this.targetPos = (this.targetPosition.IsNone ? this.goTarget.transform.position : this.goTarget.transform.TransformPoint(this.targetPosition.Value));
			}
			else
			{
				this.targetPos = this.targetPosition.Value;
			}
			this.targetPosWithVertical = this.targetPos;
			if (this.ignoreVertical.Value)
			{
				this.targetPos.y = this.go.transform.position.y;
			}
			return true;
		}

		// Token: 0x06003784 RID: 14212 RVA: 0x0011E4A0 File Offset: 0x0011C6A0
		public Vector3 GetTargetPos()
		{
			return this.targetPos;
		}

		// Token: 0x06003785 RID: 14213 RVA: 0x0011E4A8 File Offset: 0x0011C6A8
		public Vector3 GetTargetPosWithVertical()
		{
			return this.targetPosWithVertical;
		}

		// Token: 0x04002972 RID: 10610
		[Tooltip("The GameObject to Move")]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		// Token: 0x04002973 RID: 10611
		[Tooltip("A target GameObject to move towards. Or use a world Target Position below.")]
		public FsmGameObject targetObject;

		// Token: 0x04002974 RID: 10612
		[Tooltip("A world position if no Target Object. Otherwise used as a local offset from the Target Object.")]
		public FsmVector3 targetPosition;

		// Token: 0x04002975 RID: 10613
		[Tooltip("Ignore any height difference in the target.")]
		public FsmBool ignoreVertical;

		// Token: 0x04002976 RID: 10614
		[Tooltip("The maximum movement speed. HINT: You can make this a variable to change it over time.")]
		[HasFloatSlider(0f, 20f)]
		public FsmFloat maxSpeed;

		// Token: 0x04002977 RID: 10615
		[Tooltip("Distance at which the move is considered finished, and the Finish Event is sent.")]
		[HasFloatSlider(0f, 5f)]
		public FsmFloat finishDistance;

		// Token: 0x04002978 RID: 10616
		[Tooltip("Event to send when the Finish Distance is reached.")]
		public FsmEvent finishEvent;

		// Token: 0x04002979 RID: 10617
		private GameObject go;

		// Token: 0x0400297A RID: 10618
		private GameObject goTarget;

		// Token: 0x0400297B RID: 10619
		private Vector3 targetPos;

		// Token: 0x0400297C RID: 10620
		private Vector3 targetPosWithVertical;
	}
}
