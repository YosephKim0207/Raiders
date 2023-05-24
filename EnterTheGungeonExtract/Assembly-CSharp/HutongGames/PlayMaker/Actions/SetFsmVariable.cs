using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000AE3 RID: 2787
	[Tooltip("Set the value of a variable in another FSM.")]
	[ActionTarget(typeof(PlayMakerFSM), "gameObject,fsmName", false)]
	[ActionCategory(ActionCategory.StateMachine)]
	public class SetFsmVariable : FsmStateAction
	{
		// Token: 0x06003AF9 RID: 15097 RVA: 0x0012AF50 File Offset: 0x00129150
		public override void Reset()
		{
			this.gameObject = null;
			this.fsmName = string.Empty;
			this.setValue = new FsmVar();
		}

		// Token: 0x06003AFA RID: 15098 RVA: 0x0012AF74 File Offset: 0x00129174
		public override void OnEnter()
		{
			this.DoSetFsmVariable();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x06003AFB RID: 15099 RVA: 0x0012AF90 File Offset: 0x00129190
		public override void OnUpdate()
		{
			this.DoSetFsmVariable();
		}

		// Token: 0x06003AFC RID: 15100 RVA: 0x0012AF98 File Offset: 0x00129198
		private void DoSetFsmVariable()
		{
			if (this.setValue.IsNone || string.IsNullOrEmpty(this.variableName.Value))
			{
				return;
			}
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
			if (ownerDefaultTarget == null)
			{
				return;
			}
			if (ownerDefaultTarget != this.cachedGameObject || this.fsmName.Value != this.cachedFsmName)
			{
				this.targetFsm = ActionHelpers.GetGameObjectFsm(ownerDefaultTarget, this.fsmName.Value);
				if (this.targetFsm == null)
				{
					return;
				}
				this.cachedGameObject = ownerDefaultTarget;
				this.cachedFsmName = this.fsmName.Value;
			}
			if (this.variableName.Value != this.cachedVariableName)
			{
				this.targetVariable = this.targetFsm.FsmVariables.FindVariable(this.setValue.Type, this.variableName.Value);
				this.cachedVariableName = this.variableName.Value;
			}
			if (this.targetVariable == null)
			{
				base.LogWarning("Missing Variable: " + this.variableName.Value);
				return;
			}
			this.setValue.ApplyValueTo(this.targetVariable);
		}

		// Token: 0x04002D3B RID: 11579
		[Tooltip("The GameObject that owns the FSM")]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		// Token: 0x04002D3C RID: 11580
		[Tooltip("Optional name of FSM on Game Object")]
		[UIHint(UIHint.FsmName)]
		public FsmString fsmName;

		// Token: 0x04002D3D RID: 11581
		[Tooltip("The name of the variable in the target FSM.")]
		public FsmString variableName;

		// Token: 0x04002D3E RID: 11582
		[RequiredField]
		public FsmVar setValue;

		// Token: 0x04002D3F RID: 11583
		[Tooltip("Repeat every frame.")]
		public bool everyFrame;

		// Token: 0x04002D40 RID: 11584
		private PlayMakerFSM targetFsm;

		// Token: 0x04002D41 RID: 11585
		private NamedVariable targetVariable;

		// Token: 0x04002D42 RID: 11586
		private INamedVariable sourceVariable;

		// Token: 0x04002D43 RID: 11587
		private GameObject cachedGameObject;

		// Token: 0x04002D44 RID: 11588
		private string cachedFsmName;

		// Token: 0x04002D45 RID: 11589
		private string cachedVariableName;
	}
}
