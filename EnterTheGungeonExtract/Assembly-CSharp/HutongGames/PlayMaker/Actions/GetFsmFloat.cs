using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x0200097D RID: 2429
	[ActionCategory(ActionCategory.StateMachine)]
	[ActionTarget(typeof(PlayMakerFSM), "gameObject,fsmName", false)]
	[Tooltip("Get the value of a Float Variable from another FSM.")]
	public class GetFsmFloat : FsmStateAction
	{
		// Token: 0x060034D7 RID: 13527 RVA: 0x001118BC File Offset: 0x0010FABC
		public override void Reset()
		{
			this.gameObject = null;
			this.fsmName = string.Empty;
			this.storeValue = null;
		}

		// Token: 0x060034D8 RID: 13528 RVA: 0x001118DC File Offset: 0x0010FADC
		public override void OnEnter()
		{
			this.DoGetFsmFloat();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x060034D9 RID: 13529 RVA: 0x001118F8 File Offset: 0x0010FAF8
		public override void OnUpdate()
		{
			this.DoGetFsmFloat();
		}

		// Token: 0x060034DA RID: 13530 RVA: 0x00111900 File Offset: 0x0010FB00
		private void DoGetFsmFloat()
		{
			if (this.storeValue.IsNone)
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
				this.fsm = ActionHelpers.GetGameObjectFsm(ownerDefaultTarget, this.fsmName.Value);
				this.goLastFrame = ownerDefaultTarget;
			}
			if (this.fsm == null)
			{
				return;
			}
			FsmFloat fsmFloat = this.fsm.FsmVariables.GetFsmFloat(this.variableName.Value);
			if (fsmFloat == null)
			{
				return;
			}
			this.storeValue.Value = fsmFloat.Value;
		}

		// Token: 0x04002623 RID: 9763
		[RequiredField]
		public FsmOwnerDefault gameObject;

		// Token: 0x04002624 RID: 9764
		[UIHint(UIHint.FsmName)]
		[Tooltip("Optional name of FSM on Game Object")]
		public FsmString fsmName;

		// Token: 0x04002625 RID: 9765
		[RequiredField]
		[UIHint(UIHint.FsmFloat)]
		public FsmString variableName;

		// Token: 0x04002626 RID: 9766
		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmFloat storeValue;

		// Token: 0x04002627 RID: 9767
		public bool everyFrame;

		// Token: 0x04002628 RID: 9768
		private GameObject goLastFrame;

		// Token: 0x04002629 RID: 9769
		private PlayMakerFSM fsm;
	}
}
