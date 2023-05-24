using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000AD7 RID: 2775
	[Tooltip("Set the value of a Bool Variable in another FSM.")]
	[ActionTarget(typeof(PlayMakerFSM), "gameObject,fsmName", false)]
	[ActionCategory(ActionCategory.StateMachine)]
	public class SetFsmBool : FsmStateAction
	{
		// Token: 0x06003ABD RID: 15037 RVA: 0x00129E64 File Offset: 0x00128064
		public override void Reset()
		{
			this.gameObject = null;
			this.fsmName = string.Empty;
			this.setValue = null;
		}

		// Token: 0x06003ABE RID: 15038 RVA: 0x00129E84 File Offset: 0x00128084
		public override void OnEnter()
		{
			this.DoSetFsmBool();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x06003ABF RID: 15039 RVA: 0x00129EA0 File Offset: 0x001280A0
		private void DoSetFsmBool()
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
			FsmBool fsmBool = this.fsm.FsmVariables.FindFsmBool(this.variableName.Value);
			if (fsmBool != null)
			{
				fsmBool.Value = this.setValue.Value;
			}
			else
			{
				base.LogWarning("Could not find variable: " + this.variableName.Value);
			}
		}

		// Token: 0x06003AC0 RID: 15040 RVA: 0x00129FB4 File Offset: 0x001281B4
		public override void OnUpdate()
		{
			this.DoSetFsmBool();
		}

		// Token: 0x04002CDB RID: 11483
		[RequiredField]
		[Tooltip("The GameObject that owns the FSM.")]
		public FsmOwnerDefault gameObject;

		// Token: 0x04002CDC RID: 11484
		[UIHint(UIHint.FsmName)]
		[Tooltip("Optional name of FSM on Game Object")]
		public FsmString fsmName;

		// Token: 0x04002CDD RID: 11485
		[Tooltip("The name of the FSM variable.")]
		[UIHint(UIHint.FsmBool)]
		[RequiredField]
		public FsmString variableName;

		// Token: 0x04002CDE RID: 11486
		[RequiredField]
		[Tooltip("Set the value of the variable.")]
		public FsmBool setValue;

		// Token: 0x04002CDF RID: 11487
		[Tooltip("Repeat every frame. Useful if the value is changing.")]
		public bool everyFrame;

		// Token: 0x04002CE0 RID: 11488
		private GameObject goLastFrame;

		// Token: 0x04002CE1 RID: 11489
		private string fsmNameLastFrame;

		// Token: 0x04002CE2 RID: 11490
		private PlayMakerFSM fsm;
	}
}
