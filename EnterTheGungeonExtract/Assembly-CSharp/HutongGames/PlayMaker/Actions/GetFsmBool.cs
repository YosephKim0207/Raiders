using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x0200097A RID: 2426
	[Tooltip("Get the value of a Bool Variable from another FSM.")]
	[ActionTarget(typeof(PlayMakerFSM), "gameObject,fsmName", false)]
	[ActionCategory(ActionCategory.StateMachine)]
	public class GetFsmBool : FsmStateAction
	{
		// Token: 0x060034C8 RID: 13512 RVA: 0x001115C8 File Offset: 0x0010F7C8
		public override void Reset()
		{
			this.gameObject = null;
			this.fsmName = string.Empty;
			this.storeValue = null;
		}

		// Token: 0x060034C9 RID: 13513 RVA: 0x001115E8 File Offset: 0x0010F7E8
		public override void OnEnter()
		{
			this.DoGetFsmBool();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x060034CA RID: 13514 RVA: 0x00111604 File Offset: 0x0010F804
		public override void OnUpdate()
		{
			this.DoGetFsmBool();
		}

		// Token: 0x060034CB RID: 13515 RVA: 0x0011160C File Offset: 0x0010F80C
		private void DoGetFsmBool()
		{
			if (this.storeValue == null)
			{
				return;
			}
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
			if (this.fsm == null)
			{
				return;
			}
			FsmBool fsmBool = this.fsm.FsmVariables.GetFsmBool(this.variableName.Value);
			if (fsmBool == null)
			{
				return;
			}
			this.storeValue.Value = fsmBool.Value;
		}

		// Token: 0x0400260E RID: 9742
		[RequiredField]
		public FsmOwnerDefault gameObject;

		// Token: 0x0400260F RID: 9743
		[Tooltip("Optional name of FSM on Game Object")]
		[UIHint(UIHint.FsmName)]
		public FsmString fsmName;

		// Token: 0x04002610 RID: 9744
		[UIHint(UIHint.FsmBool)]
		[RequiredField]
		public FsmString variableName;

		// Token: 0x04002611 RID: 9745
		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmBool storeValue;

		// Token: 0x04002612 RID: 9746
		public bool everyFrame;

		// Token: 0x04002613 RID: 9747
		private GameObject goLastFrame;

		// Token: 0x04002614 RID: 9748
		private PlayMakerFSM fsm;
	}
}
