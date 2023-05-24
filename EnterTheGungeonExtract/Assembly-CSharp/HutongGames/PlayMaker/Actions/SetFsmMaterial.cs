using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000ADD RID: 2781
	[Tooltip("Set the value of a Material Variable in another FSM.")]
	[ActionTarget(typeof(PlayMakerFSM), "gameObject,fsmName", false)]
	[ActionCategory(ActionCategory.StateMachine)]
	public class SetFsmMaterial : FsmStateAction
	{
		// Token: 0x06003ADB RID: 15067 RVA: 0x0012A698 File Offset: 0x00128898
		public override void Reset()
		{
			this.gameObject = null;
			this.fsmName = string.Empty;
			this.variableName = string.Empty;
			this.setValue = null;
			this.everyFrame = false;
		}

		// Token: 0x06003ADC RID: 15068 RVA: 0x0012A6D0 File Offset: 0x001288D0
		public override void OnEnter()
		{
			this.DoSetFsmBool();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x06003ADD RID: 15069 RVA: 0x0012A6EC File Offset: 0x001288EC
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
			FsmMaterial fsmMaterial = this.fsm.FsmVariables.GetFsmMaterial(this.variableName.Value);
			if (fsmMaterial != null)
			{
				fsmMaterial.Value = this.setValue.Value;
			}
			else
			{
				base.LogWarning("Could not find variable: " + this.variableName.Value);
			}
		}

		// Token: 0x06003ADE RID: 15070 RVA: 0x0012A800 File Offset: 0x00128A00
		public override void OnUpdate()
		{
			this.DoSetFsmBool();
		}

		// Token: 0x04002D0B RID: 11531
		[Tooltip("The GameObject that owns the FSM.")]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		// Token: 0x04002D0C RID: 11532
		[Tooltip("Optional name of FSM on Game Object")]
		[UIHint(UIHint.FsmName)]
		public FsmString fsmName;

		// Token: 0x04002D0D RID: 11533
		[Tooltip("The name of the FSM variable.")]
		[UIHint(UIHint.FsmMaterial)]
		[RequiredField]
		public FsmString variableName;

		// Token: 0x04002D0E RID: 11534
		[Tooltip("Set the value of the variable.")]
		[RequiredField]
		public FsmMaterial setValue;

		// Token: 0x04002D0F RID: 11535
		[Tooltip("Repeat every frame. Useful if the value is changing.")]
		public bool everyFrame;

		// Token: 0x04002D10 RID: 11536
		private GameObject goLastFrame;

		// Token: 0x04002D11 RID: 11537
		private string fsmNameLastFrame;

		// Token: 0x04002D12 RID: 11538
		private PlayMakerFSM fsm;
	}
}
