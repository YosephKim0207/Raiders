using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000AE1 RID: 2785
	[Tooltip("Set the value of a String Variable in another FSM.")]
	[ActionCategory(ActionCategory.StateMachine)]
	[ActionTarget(typeof(PlayMakerFSM), "gameObject,fsmName", false)]
	public class SetFsmString : FsmStateAction
	{
		// Token: 0x06003AEF RID: 15087 RVA: 0x0012AC78 File Offset: 0x00128E78
		public override void Reset()
		{
			this.gameObject = null;
			this.fsmName = string.Empty;
			this.setValue = null;
		}

		// Token: 0x06003AF0 RID: 15088 RVA: 0x0012AC98 File Offset: 0x00128E98
		public override void OnEnter()
		{
			this.DoSetFsmString();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x06003AF1 RID: 15089 RVA: 0x0012ACB4 File Offset: 0x00128EB4
		private void DoSetFsmString()
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
			FsmString fsmString = this.fsm.FsmVariables.GetFsmString(this.variableName.Value);
			if (fsmString != null)
			{
				fsmString.Value = this.setValue.Value;
			}
			else
			{
				base.LogWarning("Could not find variable: " + this.variableName.Value);
			}
		}

		// Token: 0x06003AF2 RID: 15090 RVA: 0x0012ADC8 File Offset: 0x00128FC8
		public override void OnUpdate()
		{
			this.DoSetFsmString();
		}

		// Token: 0x04002D2B RID: 11563
		[RequiredField]
		[Tooltip("The GameObject that owns the FSM.")]
		public FsmOwnerDefault gameObject;

		// Token: 0x04002D2C RID: 11564
		[Tooltip("Optional name of FSM on Game Object.")]
		[UIHint(UIHint.FsmName)]
		public FsmString fsmName;

		// Token: 0x04002D2D RID: 11565
		[Tooltip("The name of the FSM variable.")]
		[UIHint(UIHint.FsmString)]
		[RequiredField]
		public FsmString variableName;

		// Token: 0x04002D2E RID: 11566
		[Tooltip("Set the value of the variable.")]
		public FsmString setValue;

		// Token: 0x04002D2F RID: 11567
		[Tooltip("Repeat every frame. Useful if the value is changing.")]
		public bool everyFrame;

		// Token: 0x04002D30 RID: 11568
		private GameObject goLastFrame;

		// Token: 0x04002D31 RID: 11569
		private string fsmNameLastFrame;

		// Token: 0x04002D32 RID: 11570
		private PlayMakerFSM fsm;
	}
}
