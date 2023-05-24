using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000980 RID: 2432
	[Tooltip("Get the value of a Material Variable from another FSM.")]
	[ActionTarget(typeof(PlayMakerFSM), "gameObject,fsmName", false)]
	[ActionCategory(ActionCategory.StateMachine)]
	public class GetFsmMaterial : FsmStateAction
	{
		// Token: 0x060034E6 RID: 13542 RVA: 0x00111BB4 File Offset: 0x0010FDB4
		public override void Reset()
		{
			this.gameObject = null;
			this.fsmName = string.Empty;
			this.variableName = string.Empty;
			this.storeValue = null;
			this.everyFrame = false;
		}

		// Token: 0x060034E7 RID: 13543 RVA: 0x00111BEC File Offset: 0x0010FDEC
		public override void OnEnter()
		{
			this.DoGetFsmVariable();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x060034E8 RID: 13544 RVA: 0x00111C08 File Offset: 0x0010FE08
		public override void OnUpdate()
		{
			this.DoGetFsmVariable();
		}

		// Token: 0x060034E9 RID: 13545 RVA: 0x00111C10 File Offset: 0x0010FE10
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
			FsmMaterial fsmMaterial = this.fsm.FsmVariables.GetFsmMaterial(this.variableName.Value);
			if (fsmMaterial != null)
			{
				this.storeValue.Value = fsmMaterial.Value;
			}
		}

		// Token: 0x04002638 RID: 9784
		[Tooltip("The GameObject that owns the FSM.")]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		// Token: 0x04002639 RID: 9785
		[Tooltip("Optional name of FSM on Game Object")]
		[UIHint(UIHint.FsmName)]
		public FsmString fsmName;

		// Token: 0x0400263A RID: 9786
		[UIHint(UIHint.FsmMaterial)]
		[RequiredField]
		public FsmString variableName;

		// Token: 0x0400263B RID: 9787
		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmMaterial storeValue;

		// Token: 0x0400263C RID: 9788
		[Tooltip("Repeat every frame.")]
		public bool everyFrame;

		// Token: 0x0400263D RID: 9789
		private GameObject goLastFrame;

		// Token: 0x0400263E RID: 9790
		protected PlayMakerFSM fsm;
	}
}
