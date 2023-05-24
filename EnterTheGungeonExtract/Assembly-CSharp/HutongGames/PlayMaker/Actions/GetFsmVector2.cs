using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000989 RID: 2441
	[Tooltip("Get the value of a Vector2 Variable from another FSM.")]
	[ActionCategory(ActionCategory.StateMachine)]
	[ActionTarget(typeof(PlayMakerFSM), "gameObject,fsmName", false)]
	public class GetFsmVector2 : FsmStateAction
	{
		// Token: 0x06003515 RID: 13589 RVA: 0x00112654 File Offset: 0x00110854
		public override void Reset()
		{
			this.gameObject = null;
			this.fsmName = string.Empty;
			this.storeValue = null;
		}

		// Token: 0x06003516 RID: 13590 RVA: 0x00112674 File Offset: 0x00110874
		public override void OnEnter()
		{
			this.DoGetFsmVector2();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x06003517 RID: 13591 RVA: 0x00112690 File Offset: 0x00110890
		public override void OnUpdate()
		{
			this.DoGetFsmVector2();
		}

		// Token: 0x06003518 RID: 13592 RVA: 0x00112698 File Offset: 0x00110898
		private void DoGetFsmVector2()
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
			FsmVector2 fsmVector = this.fsm.FsmVariables.GetFsmVector2(this.variableName.Value);
			if (fsmVector == null)
			{
				return;
			}
			this.storeValue.Value = fsmVector.Value;
		}

		// Token: 0x04002678 RID: 9848
		[RequiredField]
		public FsmOwnerDefault gameObject;

		// Token: 0x04002679 RID: 9849
		[UIHint(UIHint.FsmName)]
		[Tooltip("Optional name of FSM on Game Object")]
		public FsmString fsmName;

		// Token: 0x0400267A RID: 9850
		[RequiredField]
		[UIHint(UIHint.FsmVector2)]
		public FsmString variableName;

		// Token: 0x0400267B RID: 9851
		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmVector2 storeValue;

		// Token: 0x0400267C RID: 9852
		public bool everyFrame;

		// Token: 0x0400267D RID: 9853
		private GameObject goLastFrame;

		// Token: 0x0400267E RID: 9854
		private PlayMakerFSM fsm;
	}
}
