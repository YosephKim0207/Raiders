using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x0200097C RID: 2428
	[Tooltip("Get the value of an Enum Variable from another FSM.")]
	[ActionTarget(typeof(PlayMakerFSM), "gameObject,fsmName", false)]
	[ActionCategory(ActionCategory.StateMachine)]
	public class GetFsmEnum : FsmStateAction
	{
		// Token: 0x060034D2 RID: 13522 RVA: 0x001117C0 File Offset: 0x0010F9C0
		public override void Reset()
		{
			this.gameObject = null;
			this.fsmName = string.Empty;
			this.storeValue = null;
		}

		// Token: 0x060034D3 RID: 13523 RVA: 0x001117E0 File Offset: 0x0010F9E0
		public override void OnEnter()
		{
			this.DoGetFsmEnum();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x060034D4 RID: 13524 RVA: 0x001117FC File Offset: 0x0010F9FC
		public override void OnUpdate()
		{
			this.DoGetFsmEnum();
		}

		// Token: 0x060034D5 RID: 13525 RVA: 0x00111804 File Offset: 0x0010FA04
		private void DoGetFsmEnum()
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
			FsmEnum fsmEnum = this.fsm.FsmVariables.GetFsmEnum(this.variableName.Value);
			if (fsmEnum == null)
			{
				return;
			}
			this.storeValue.Value = fsmEnum.Value;
		}

		// Token: 0x0400261C RID: 9756
		[Tooltip("The target FSM")]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		// Token: 0x0400261D RID: 9757
		[UIHint(UIHint.FsmName)]
		[Tooltip("Optional name of FSM on Game Object")]
		public FsmString fsmName;

		// Token: 0x0400261E RID: 9758
		[UIHint(UIHint.FsmBool)]
		[RequiredField]
		public FsmString variableName;

		// Token: 0x0400261F RID: 9759
		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmEnum storeValue;

		// Token: 0x04002620 RID: 9760
		[Tooltip("Repeat every frame")]
		public bool everyFrame;

		// Token: 0x04002621 RID: 9761
		private GameObject goLastFrame;

		// Token: 0x04002622 RID: 9762
		private PlayMakerFSM fsm;
	}
}
