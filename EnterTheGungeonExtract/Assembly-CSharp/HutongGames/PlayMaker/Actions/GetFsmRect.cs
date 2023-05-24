using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000983 RID: 2435
	[Tooltip("Get the value of a Rect Variable from another FSM.")]
	[ActionCategory(ActionCategory.StateMachine)]
	[ActionTarget(typeof(PlayMakerFSM), "gameObject,fsmName", false)]
	public class GetFsmRect : FsmStateAction
	{
		// Token: 0x060034F5 RID: 13557 RVA: 0x00111EE4 File Offset: 0x001100E4
		public override void Reset()
		{
			this.gameObject = null;
			this.fsmName = string.Empty;
			this.variableName = string.Empty;
			this.storeValue = null;
			this.everyFrame = false;
		}

		// Token: 0x060034F6 RID: 13558 RVA: 0x00111F1C File Offset: 0x0011011C
		public override void OnEnter()
		{
			this.DoGetFsmVariable();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x060034F7 RID: 13559 RVA: 0x00111F38 File Offset: 0x00110138
		public override void OnUpdate()
		{
			this.DoGetFsmVariable();
		}

		// Token: 0x060034F8 RID: 13560 RVA: 0x00111F40 File Offset: 0x00110140
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
			FsmRect fsmRect = this.fsm.FsmVariables.GetFsmRect(this.variableName.Value);
			if (fsmRect != null)
			{
				this.storeValue.Value = fsmRect.Value;
			}
		}

		// Token: 0x0400264D RID: 9805
		[RequiredField]
		[Tooltip("The GameObject that owns the FSM.")]
		public FsmOwnerDefault gameObject;

		// Token: 0x0400264E RID: 9806
		[Tooltip("Optional name of FSM on Game Object")]
		[UIHint(UIHint.FsmName)]
		public FsmString fsmName;

		// Token: 0x0400264F RID: 9807
		[RequiredField]
		[UIHint(UIHint.FsmRect)]
		public FsmString variableName;

		// Token: 0x04002650 RID: 9808
		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmRect storeValue;

		// Token: 0x04002651 RID: 9809
		[Tooltip("Repeat every frame.")]
		public bool everyFrame;

		// Token: 0x04002652 RID: 9810
		private GameObject goLastFrame;

		// Token: 0x04002653 RID: 9811
		protected PlayMakerFSM fsm;
	}
}
