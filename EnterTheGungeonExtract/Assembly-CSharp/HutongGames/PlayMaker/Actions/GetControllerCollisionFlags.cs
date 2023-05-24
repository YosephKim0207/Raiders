using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000973 RID: 2419
	[Tooltip("Gets the Collision Flags from a Character Controller on a Game Object. Collision flags give you a broad overview of where the character collided with any other object.")]
	[ActionCategory(ActionCategory.Character)]
	public class GetControllerCollisionFlags : FsmStateAction
	{
		// Token: 0x060034AD RID: 13485 RVA: 0x00110E28 File Offset: 0x0010F028
		public override void Reset()
		{
			this.gameObject = null;
			this.isGrounded = null;
			this.none = null;
			this.sides = null;
			this.above = null;
			this.below = null;
		}

		// Token: 0x060034AE RID: 13486 RVA: 0x00110E54 File Offset: 0x0010F054
		public override void OnUpdate()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
			if (ownerDefaultTarget == null)
			{
				return;
			}
			if (ownerDefaultTarget != this.previousGo)
			{
				this.controller = ownerDefaultTarget.GetComponent<CharacterController>();
				this.previousGo = ownerDefaultTarget;
			}
			if (this.controller != null)
			{
				this.isGrounded.Value = this.controller.isGrounded;
				FsmBool fsmBool = this.none;
				CollisionFlags collisionFlags = this.controller.collisionFlags;
				fsmBool.Value = false;
				this.sides.Value = (this.controller.collisionFlags & CollisionFlags.Sides) != CollisionFlags.None;
				this.above.Value = (this.controller.collisionFlags & CollisionFlags.Above) != CollisionFlags.None;
				this.below.Value = (this.controller.collisionFlags & CollisionFlags.Below) != CollisionFlags.None;
			}
		}

		// Token: 0x040025DD RID: 9693
		[Tooltip("The GameObject with a Character Controller component.")]
		[CheckForComponent(typeof(CharacterController))]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		// Token: 0x040025DE RID: 9694
		[Tooltip("True if the Character Controller capsule is on the ground")]
		[UIHint(UIHint.Variable)]
		public FsmBool isGrounded;

		// Token: 0x040025DF RID: 9695
		[Tooltip("True if no collisions in last move.")]
		[UIHint(UIHint.Variable)]
		public FsmBool none;

		// Token: 0x040025E0 RID: 9696
		[Tooltip("True if the Character Controller capsule was hit on the sides.")]
		[UIHint(UIHint.Variable)]
		public FsmBool sides;

		// Token: 0x040025E1 RID: 9697
		[Tooltip("True if the Character Controller capsule was hit from above.")]
		[UIHint(UIHint.Variable)]
		public FsmBool above;

		// Token: 0x040025E2 RID: 9698
		[Tooltip("True if the Character Controller capsule was hit from below.")]
		[UIHint(UIHint.Variable)]
		public FsmBool below;

		// Token: 0x040025E3 RID: 9699
		private GameObject previousGo;

		// Token: 0x040025E4 RID: 9700
		private CharacterController controller;
	}
}
