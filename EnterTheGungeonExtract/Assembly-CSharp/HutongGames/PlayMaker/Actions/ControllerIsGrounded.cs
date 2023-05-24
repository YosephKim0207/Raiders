using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000910 RID: 2320
	[Tooltip("Tests if a Character Controller on a Game Object was touching the ground during the last move.")]
	[ActionCategory(ActionCategory.Character)]
	public class ControllerIsGrounded : FsmStateAction
	{
		// Token: 0x0600331E RID: 13086 RVA: 0x0010C290 File Offset: 0x0010A490
		public override void Reset()
		{
			this.gameObject = null;
			this.trueEvent = null;
			this.falseEvent = null;
			this.storeResult = null;
			this.everyFrame = false;
		}

		// Token: 0x0600331F RID: 13087 RVA: 0x0010C2B8 File Offset: 0x0010A4B8
		public override void OnEnter()
		{
			this.DoControllerIsGrounded();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x06003320 RID: 13088 RVA: 0x0010C2D4 File Offset: 0x0010A4D4
		public override void OnUpdate()
		{
			this.DoControllerIsGrounded();
		}

		// Token: 0x06003321 RID: 13089 RVA: 0x0010C2DC File Offset: 0x0010A4DC
		private void DoControllerIsGrounded()
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
			if (this.controller == null)
			{
				return;
			}
			bool isGrounded = this.controller.isGrounded;
			this.storeResult.Value = isGrounded;
			base.Fsm.Event((!isGrounded) ? this.falseEvent : this.trueEvent);
		}

		// Token: 0x04002442 RID: 9282
		[Tooltip("The GameObject to check.")]
		[CheckForComponent(typeof(CharacterController))]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		// Token: 0x04002443 RID: 9283
		[Tooltip("Event to send if touching the ground.")]
		public FsmEvent trueEvent;

		// Token: 0x04002444 RID: 9284
		[Tooltip("Event to send if not touching the ground.")]
		public FsmEvent falseEvent;

		// Token: 0x04002445 RID: 9285
		[UIHint(UIHint.Variable)]
		[Tooltip("Store the result in a bool variable.")]
		public FsmBool storeResult;

		// Token: 0x04002446 RID: 9286
		[Tooltip("Repeat every frame while the state is active.")]
		public bool everyFrame;

		// Token: 0x04002447 RID: 9287
		private GameObject previousGo;

		// Token: 0x04002448 RID: 9288
		private CharacterController controller;
	}
}
