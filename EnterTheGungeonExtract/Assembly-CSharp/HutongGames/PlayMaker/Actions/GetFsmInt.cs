using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x0200097F RID: 2431
	[ActionTarget(typeof(PlayMakerFSM), "gameObject,fsmName", false)]
	[Tooltip("Get the value of an Integer Variable from another FSM.")]
	[ActionCategory(ActionCategory.StateMachine)]
	public class GetFsmInt : FsmStateAction
	{
		// Token: 0x060034E1 RID: 13537 RVA: 0x00111AB8 File Offset: 0x0010FCB8
		public override void Reset()
		{
			this.gameObject = null;
			this.fsmName = string.Empty;
			this.storeValue = null;
		}

		// Token: 0x060034E2 RID: 13538 RVA: 0x00111AD8 File Offset: 0x0010FCD8
		public override void OnEnter()
		{
			this.DoGetFsmInt();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x060034E3 RID: 13539 RVA: 0x00111AF4 File Offset: 0x0010FCF4
		public override void OnUpdate()
		{
			this.DoGetFsmInt();
		}

		// Token: 0x060034E4 RID: 13540 RVA: 0x00111AFC File Offset: 0x0010FCFC
		private void DoGetFsmInt()
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
			FsmInt fsmInt = this.fsm.FsmVariables.GetFsmInt(this.variableName.Value);
			if (fsmInt == null)
			{
				return;
			}
			this.storeValue.Value = fsmInt.Value;
		}

		// Token: 0x04002631 RID: 9777
		[RequiredField]
		public FsmOwnerDefault gameObject;

		// Token: 0x04002632 RID: 9778
		[Tooltip("Optional name of FSM on Game Object")]
		[UIHint(UIHint.FsmName)]
		public FsmString fsmName;

		// Token: 0x04002633 RID: 9779
		[UIHint(UIHint.FsmInt)]
		[RequiredField]
		public FsmString variableName;

		// Token: 0x04002634 RID: 9780
		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmInt storeValue;

		// Token: 0x04002635 RID: 9781
		public bool everyFrame;

		// Token: 0x04002636 RID: 9782
		private GameObject goLastFrame;

		// Token: 0x04002637 RID: 9783
		private PlayMakerFSM fsm;
	}
}
