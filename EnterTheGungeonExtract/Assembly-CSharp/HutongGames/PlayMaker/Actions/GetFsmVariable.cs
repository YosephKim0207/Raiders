using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000987 RID: 2439
	[Tooltip("Get the value of a variable in another FSM and store it in a variable of the same name in this FSM.")]
	[ActionTarget(typeof(PlayMakerFSM), "gameObject,fsmName", false)]
	[ActionCategory(ActionCategory.StateMachine)]
	public class GetFsmVariable : FsmStateAction
	{
		// Token: 0x06003509 RID: 13577 RVA: 0x0011230C File Offset: 0x0011050C
		public override void Reset()
		{
			this.gameObject = null;
			this.fsmName = string.Empty;
			this.storeValue = new FsmVar();
		}

		// Token: 0x0600350A RID: 13578 RVA: 0x00112330 File Offset: 0x00110530
		public override void OnEnter()
		{
			this.InitFsmVar();
			this.DoGetFsmVariable();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x0600350B RID: 13579 RVA: 0x00112350 File Offset: 0x00110550
		public override void OnUpdate()
		{
			this.DoGetFsmVariable();
		}

		// Token: 0x0600350C RID: 13580 RVA: 0x00112358 File Offset: 0x00110558
		private void InitFsmVar()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
			if (ownerDefaultTarget == null)
			{
				return;
			}
			if (ownerDefaultTarget != this.cachedGO)
			{
				this.sourceFsm = ActionHelpers.GetGameObjectFsm(ownerDefaultTarget, this.fsmName.Value);
				this.sourceVariable = this.sourceFsm.FsmVariables.GetVariable(this.storeValue.variableName);
				this.targetVariable = base.Fsm.Variables.GetVariable(this.storeValue.variableName);
				this.storeValue.Type = this.targetVariable.VariableType;
				if (!string.IsNullOrEmpty(this.storeValue.variableName) && this.sourceVariable == null)
				{
					base.LogWarning("Missing Variable: " + this.storeValue.variableName);
				}
				this.cachedGO = ownerDefaultTarget;
			}
		}

		// Token: 0x0600350D RID: 13581 RVA: 0x00112448 File Offset: 0x00110648
		private void DoGetFsmVariable()
		{
			if (this.storeValue.IsNone)
			{
				return;
			}
			this.InitFsmVar();
			this.storeValue.GetValueFrom(this.sourceVariable);
			this.storeValue.ApplyValueTo(this.targetVariable);
		}

		// Token: 0x04002668 RID: 9832
		[RequiredField]
		[Tooltip("The GameObject that owns the FSM")]
		public FsmOwnerDefault gameObject;

		// Token: 0x04002669 RID: 9833
		[Tooltip("Optional name of FSM on Game Object")]
		[UIHint(UIHint.FsmName)]
		public FsmString fsmName;

		// Token: 0x0400266A RID: 9834
		[RequiredField]
		[HideTypeFilter]
		[UIHint(UIHint.Variable)]
		[Tooltip("Store the value of the FsmVariable")]
		public FsmVar storeValue;

		// Token: 0x0400266B RID: 9835
		[Tooltip("Repeat every frame.")]
		public bool everyFrame;

		// Token: 0x0400266C RID: 9836
		private GameObject cachedGO;

		// Token: 0x0400266D RID: 9837
		private PlayMakerFSM sourceFsm;

		// Token: 0x0400266E RID: 9838
		private INamedVariable sourceVariable;

		// Token: 0x0400266F RID: 9839
		private NamedVariable targetVariable;
	}
}
