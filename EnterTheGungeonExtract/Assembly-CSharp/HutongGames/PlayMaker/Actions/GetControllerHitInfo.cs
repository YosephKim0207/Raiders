using System;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000974 RID: 2420
	[ActionCategory(ActionCategory.Character)]
	[Tooltip("Gets info on the last Character Controller collision and store in variables.")]
	public class GetControllerHitInfo : FsmStateAction
	{
		// Token: 0x060034B0 RID: 13488 RVA: 0x00110F48 File Offset: 0x0010F148
		public override void Reset()
		{
			this.gameObjectHit = null;
			this.contactPoint = null;
			this.contactNormal = null;
			this.moveDirection = null;
			this.moveLength = null;
			this.physicsMaterialName = null;
		}

		// Token: 0x060034B1 RID: 13489 RVA: 0x00110F74 File Offset: 0x0010F174
		public override void OnPreprocess()
		{
			base.Fsm.HandleControllerColliderHit = true;
		}

		// Token: 0x060034B2 RID: 13490 RVA: 0x00110F84 File Offset: 0x0010F184
		private void StoreTriggerInfo()
		{
			if (base.Fsm.ControllerCollider == null)
			{
				return;
			}
			this.gameObjectHit.Value = base.Fsm.ControllerCollider.gameObject;
			this.contactPoint.Value = base.Fsm.ControllerCollider.point;
			this.contactNormal.Value = base.Fsm.ControllerCollider.normal;
			this.moveDirection.Value = base.Fsm.ControllerCollider.moveDirection;
			this.moveLength.Value = base.Fsm.ControllerCollider.moveLength;
			this.physicsMaterialName.Value = base.Fsm.ControllerCollider.collider.material.name;
		}

		// Token: 0x060034B3 RID: 13491 RVA: 0x00111050 File Offset: 0x0010F250
		public override void OnEnter()
		{
			this.StoreTriggerInfo();
			base.Finish();
		}

		// Token: 0x060034B4 RID: 13492 RVA: 0x00111060 File Offset: 0x0010F260
		public override string ErrorCheck()
		{
			return ActionHelpers.CheckOwnerPhysicsSetup(base.Owner);
		}

		// Token: 0x040025E5 RID: 9701
		[Tooltip("Store the GameObject hit in the last collision.")]
		[UIHint(UIHint.Variable)]
		public FsmGameObject gameObjectHit;

		// Token: 0x040025E6 RID: 9702
		[Tooltip("Store the contact point of the last collision in world coordinates.")]
		[UIHint(UIHint.Variable)]
		public FsmVector3 contactPoint;

		// Token: 0x040025E7 RID: 9703
		[Tooltip("Store the normal of the last collision.")]
		[UIHint(UIHint.Variable)]
		public FsmVector3 contactNormal;

		// Token: 0x040025E8 RID: 9704
		[Tooltip("Store the direction of the last move before the collision.")]
		[UIHint(UIHint.Variable)]
		public FsmVector3 moveDirection;

		// Token: 0x040025E9 RID: 9705
		[Tooltip("Store the distance of the last move before the collision.")]
		[UIHint(UIHint.Variable)]
		public FsmFloat moveLength;

		// Token: 0x040025EA RID: 9706
		[Tooltip("Store the physics material of the Game Object Hit. Useful for triggering different effects. Audio, particles...")]
		[UIHint(UIHint.Variable)]
		public FsmString physicsMaterialName;
	}
}
