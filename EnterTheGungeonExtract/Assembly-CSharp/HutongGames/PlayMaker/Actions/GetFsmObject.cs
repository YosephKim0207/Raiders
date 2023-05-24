using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000981 RID: 2433
	[Tooltip("Get the value of an Object Variable from another FSM.")]
	[ActionTarget(typeof(PlayMakerFSM), "gameObject,fsmName", false)]
	[ActionCategory(ActionCategory.StateMachine)]
	public class GetFsmObject : FsmStateAction
	{
		// Token: 0x060034EB RID: 13547 RVA: 0x00111CC4 File Offset: 0x0010FEC4
		public override void Reset()
		{
			this.gameObject = null;
			this.fsmName = string.Empty;
			this.variableName = string.Empty;
			this.storeValue = null;
			this.everyFrame = false;
		}

		// Token: 0x060034EC RID: 13548 RVA: 0x00111CFC File Offset: 0x0010FEFC
		public override void OnEnter()
		{
			this.DoGetFsmVariable();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x060034ED RID: 13549 RVA: 0x00111D18 File Offset: 0x0010FF18
		public override void OnUpdate()
		{
			this.DoGetFsmVariable();
		}

		// Token: 0x060034EE RID: 13550 RVA: 0x00111D20 File Offset: 0x0010FF20
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
			FsmObject fsmObject = this.fsm.FsmVariables.GetFsmObject(this.variableName.Value);
			if (fsmObject != null)
			{
				this.storeValue.Value = fsmObject.Value;
			}
		}

		// Token: 0x0400263F RID: 9791
		[Tooltip("The GameObject that owns the FSM.")]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		// Token: 0x04002640 RID: 9792
		[Tooltip("Optional name of FSM on Game Object")]
		[UIHint(UIHint.FsmName)]
		public FsmString fsmName;

		// Token: 0x04002641 RID: 9793
		[UIHint(UIHint.FsmObject)]
		[RequiredField]
		public FsmString variableName;

		// Token: 0x04002642 RID: 9794
		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmObject storeValue;

		// Token: 0x04002643 RID: 9795
		[Tooltip("Repeat every frame.")]
		public bool everyFrame;

		// Token: 0x04002644 RID: 9796
		private GameObject goLastFrame;

		// Token: 0x04002645 RID: 9797
		protected PlayMakerFSM fsm;
	}
}
