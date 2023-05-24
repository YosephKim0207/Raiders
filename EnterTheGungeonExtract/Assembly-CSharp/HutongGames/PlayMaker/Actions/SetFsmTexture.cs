using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000AE2 RID: 2786
	[ActionTarget(typeof(PlayMakerFSM), "gameObject,fsmName", false)]
	[ActionCategory(ActionCategory.StateMachine)]
	[Tooltip("Set the value of a Texture Variable in another FSM.")]
	public class SetFsmTexture : FsmStateAction
	{
		// Token: 0x06003AF4 RID: 15092 RVA: 0x0012ADD8 File Offset: 0x00128FD8
		public override void Reset()
		{
			this.gameObject = null;
			this.fsmName = string.Empty;
			this.variableName = string.Empty;
			this.setValue = null;
			this.everyFrame = false;
		}

		// Token: 0x06003AF5 RID: 15093 RVA: 0x0012AE10 File Offset: 0x00129010
		public override void OnEnter()
		{
			this.DoSetFsmBool();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x06003AF6 RID: 15094 RVA: 0x0012AE2C File Offset: 0x0012902C
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
			FsmTexture fsmTexture = this.fsm.FsmVariables.FindFsmTexture(this.variableName.Value);
			if (fsmTexture != null)
			{
				fsmTexture.Value = this.setValue.Value;
			}
			else
			{
				base.LogWarning("Could not find variable: " + this.variableName.Value);
			}
		}

		// Token: 0x06003AF7 RID: 15095 RVA: 0x0012AF40 File Offset: 0x00129140
		public override void OnUpdate()
		{
			this.DoSetFsmBool();
		}

		// Token: 0x04002D33 RID: 11571
		[Tooltip("The GameObject that owns the FSM.")]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		// Token: 0x04002D34 RID: 11572
		[Tooltip("Optional name of FSM on Game Object")]
		[UIHint(UIHint.FsmName)]
		public FsmString fsmName;

		// Token: 0x04002D35 RID: 11573
		[Tooltip("The name of the FSM variable.")]
		[UIHint(UIHint.FsmTexture)]
		[RequiredField]
		public FsmString variableName;

		// Token: 0x04002D36 RID: 11574
		[Tooltip("Set the value of the variable.")]
		public FsmTexture setValue;

		// Token: 0x04002D37 RID: 11575
		[Tooltip("Repeat every frame. Useful if the value is changing.")]
		public bool everyFrame;

		// Token: 0x04002D38 RID: 11576
		private GameObject goLastFrame;

		// Token: 0x04002D39 RID: 11577
		private string fsmNameLastFrame;

		// Token: 0x04002D3A RID: 11578
		private PlayMakerFSM fsm;
	}
}
