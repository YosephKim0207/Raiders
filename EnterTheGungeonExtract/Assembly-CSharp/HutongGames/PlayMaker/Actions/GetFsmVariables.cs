using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000988 RID: 2440
	[Tooltip("Get the values of multiple variables in another FSM and store in variables of the same name in this FSM.")]
	[ActionTarget(typeof(PlayMakerFSM), "gameObject,fsmName", false)]
	[ActionCategory(ActionCategory.StateMachine)]
	public class GetFsmVariables : FsmStateAction
	{
		// Token: 0x0600350F RID: 13583 RVA: 0x0011248C File Offset: 0x0011068C
		public override void Reset()
		{
			this.gameObject = null;
			this.fsmName = string.Empty;
			this.getVariables = null;
		}

		// Token: 0x06003510 RID: 13584 RVA: 0x001124AC File Offset: 0x001106AC
		private void InitFsmVars()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
			if (ownerDefaultTarget == null)
			{
				return;
			}
			if (ownerDefaultTarget != this.cachedGO)
			{
				this.sourceVariables = new INamedVariable[this.getVariables.Length];
				this.targetVariables = new NamedVariable[this.getVariables.Length];
				for (int i = 0; i < this.getVariables.Length; i++)
				{
					string variableName = this.getVariables[i].variableName;
					this.sourceFsm = ActionHelpers.GetGameObjectFsm(ownerDefaultTarget, this.fsmName.Value);
					this.sourceVariables[i] = this.sourceFsm.FsmVariables.GetVariable(variableName);
					this.targetVariables[i] = base.Fsm.Variables.GetVariable(variableName);
					this.getVariables[i].Type = this.targetVariables[i].VariableType;
					if (!string.IsNullOrEmpty(variableName) && this.sourceVariables[i] == null)
					{
						base.LogWarning("Missing Variable: " + variableName);
					}
					this.cachedGO = ownerDefaultTarget;
				}
			}
		}

		// Token: 0x06003511 RID: 13585 RVA: 0x001125CC File Offset: 0x001107CC
		public override void OnEnter()
		{
			this.InitFsmVars();
			this.DoGetFsmVariables();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x06003512 RID: 13586 RVA: 0x001125EC File Offset: 0x001107EC
		public override void OnUpdate()
		{
			this.DoGetFsmVariables();
		}

		// Token: 0x06003513 RID: 13587 RVA: 0x001125F4 File Offset: 0x001107F4
		private void DoGetFsmVariables()
		{
			this.InitFsmVars();
			for (int i = 0; i < this.getVariables.Length; i++)
			{
				this.getVariables[i].GetValueFrom(this.sourceVariables[i]);
				this.getVariables[i].ApplyValueTo(this.targetVariables[i]);
			}
		}

		// Token: 0x04002670 RID: 9840
		[Tooltip("The GameObject that owns the FSM")]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		// Token: 0x04002671 RID: 9841
		[UIHint(UIHint.FsmName)]
		[Tooltip("Optional name of FSM on Game Object")]
		public FsmString fsmName;

		// Token: 0x04002672 RID: 9842
		[UIHint(UIHint.Variable)]
		[Tooltip("Store the values of the FsmVariables")]
		[HideTypeFilter]
		[RequiredField]
		public FsmVar[] getVariables;

		// Token: 0x04002673 RID: 9843
		[Tooltip("Repeat every frame.")]
		public bool everyFrame;

		// Token: 0x04002674 RID: 9844
		private GameObject cachedGO;

		// Token: 0x04002675 RID: 9845
		private PlayMakerFSM sourceFsm;

		// Token: 0x04002676 RID: 9846
		private INamedVariable[] sourceVariables;

		// Token: 0x04002677 RID: 9847
		private NamedVariable[] targetVariables;
	}
}
