using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000ADA RID: 2778
	[ActionCategory(ActionCategory.StateMachine)]
	[ActionTarget(typeof(PlayMakerFSM), "gameObject,fsmName", false)]
	[Tooltip("Set the value of a Float Variable in another FSM.")]
	public class SetFsmFloat : FsmStateAction
	{
		// Token: 0x06003ACC RID: 15052 RVA: 0x0012A284 File Offset: 0x00128484
		public override void Reset()
		{
			this.gameObject = null;
			this.fsmName = string.Empty;
			this.setValue = null;
		}

		// Token: 0x06003ACD RID: 15053 RVA: 0x0012A2A4 File Offset: 0x001284A4
		public override void OnEnter()
		{
			this.DoSetFsmFloat();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x06003ACE RID: 15054 RVA: 0x0012A2C0 File Offset: 0x001284C0
		private void DoSetFsmFloat()
		{
			if (this.setValue == null)
			{
				return;
			}
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
			if (ownerDefaultTarget == null)
			{
				return;
			}
			if (ownerDefaultTarget != this.goLastFrame || this.fsmName.Value != this.fsmNameLastFrame)
			{
				this.goLastFrame = ownerDefaultTarget;
				this.fsmNameLastFrame = this.fsmName.Value;
				this.fsm = ActionHelpers.GetGameObjectFsm(ownerDefaultTarget, this.fsmName.Value);
			}
			if (this.fsm == null)
			{
				base.LogWarning("Could not find FSM: " + this.fsmName.Value);
				return;
			}
			FsmFloat fsmFloat = this.fsm.FsmVariables.GetFsmFloat(this.variableName.Value);
			if (fsmFloat != null)
			{
				fsmFloat.Value = this.setValue.Value;
			}
			else
			{
				base.LogWarning("Could not find variable: " + this.variableName.Value);
			}
		}

		// Token: 0x06003ACF RID: 15055 RVA: 0x0012A3D4 File Offset: 0x001285D4
		public override void OnUpdate()
		{
			this.DoSetFsmFloat();
		}

		// Token: 0x04002CF3 RID: 11507
		[RequiredField]
		[Tooltip("The GameObject that owns the FSM.")]
		public FsmOwnerDefault gameObject;

		// Token: 0x04002CF4 RID: 11508
		[UIHint(UIHint.FsmName)]
		[Tooltip("Optional name of FSM on Game Object")]
		public FsmString fsmName;

		// Token: 0x04002CF5 RID: 11509
		[UIHint(UIHint.FsmFloat)]
		[Tooltip("The name of the FSM variable.")]
		[RequiredField]
		public FsmString variableName;

		// Token: 0x04002CF6 RID: 11510
		[RequiredField]
		[Tooltip("Set the value of the variable.")]
		public FsmFloat setValue;

		// Token: 0x04002CF7 RID: 11511
		[Tooltip("Repeat every frame. Useful if the value is changing.")]
		public bool everyFrame;

		// Token: 0x04002CF8 RID: 11512
		private GameObject goLastFrame;

		// Token: 0x04002CF9 RID: 11513
		private string fsmNameLastFrame;

		// Token: 0x04002CFA RID: 11514
		private PlayMakerFSM fsm;
	}
}
