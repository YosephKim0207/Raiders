using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000986 RID: 2438
	[Tooltip("Get the value of a Texture Variable from another FSM.")]
	[ActionTarget(typeof(PlayMakerFSM), "gameObject,fsmName", false)]
	[ActionCategory(ActionCategory.StateMachine)]
	public class GetFsmTexture : FsmStateAction
	{
		// Token: 0x06003504 RID: 13572 RVA: 0x001121FC File Offset: 0x001103FC
		public override void Reset()
		{
			this.gameObject = null;
			this.fsmName = string.Empty;
			this.variableName = string.Empty;
			this.storeValue = null;
			this.everyFrame = false;
		}

		// Token: 0x06003505 RID: 13573 RVA: 0x00112234 File Offset: 0x00110434
		public override void OnEnter()
		{
			this.DoGetFsmVariable();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x06003506 RID: 13574 RVA: 0x00112250 File Offset: 0x00110450
		public override void OnUpdate()
		{
			this.DoGetFsmVariable();
		}

		// Token: 0x06003507 RID: 13575 RVA: 0x00112258 File Offset: 0x00110458
		private void DoGetFsmVariable()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
			if (ownerDefaultTarget == null)
			{
				return;
			}
			if (ownerDefaultTarget != this.goLastFrame)
			{
				this.goLastFrame = ownerDefaultTarget;
				this.fsm = ActionHelpers.GetGameObjectFsm(ownerDefaultTarget, this.fsmName.Value);
			}
			if (this.fsm == null || this.storeValue == null)
			{
				return;
			}
			FsmTexture fsmTexture = this.fsm.FsmVariables.GetFsmTexture(this.variableName.Value);
			if (fsmTexture != null)
			{
				this.storeValue.Value = fsmTexture.Value;
			}
		}

		// Token: 0x04002661 RID: 9825
		[RequiredField]
		[Tooltip("The GameObject that owns the FSM.")]
		public FsmOwnerDefault gameObject;

		// Token: 0x04002662 RID: 9826
		[Tooltip("Optional name of FSM on Game Object")]
		[UIHint(UIHint.FsmName)]
		public FsmString fsmName;

		// Token: 0x04002663 RID: 9827
		[RequiredField]
		[UIHint(UIHint.FsmTexture)]
		public FsmString variableName;

		// Token: 0x04002664 RID: 9828
		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmTexture storeValue;

		// Token: 0x04002665 RID: 9829
		[Tooltip("Repeat every frame.")]
		public bool everyFrame;

		// Token: 0x04002666 RID: 9830
		private GameObject goLastFrame;

		// Token: 0x04002667 RID: 9831
		protected PlayMakerFSM fsm;
	}
}
