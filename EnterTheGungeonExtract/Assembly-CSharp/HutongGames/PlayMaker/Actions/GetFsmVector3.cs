using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x0200098A RID: 2442
	[ActionTarget(typeof(PlayMakerFSM), "gameObject,fsmName", false)]
	[Tooltip("Get the value of a Vector3 Variable from another FSM.")]
	[ActionCategory(ActionCategory.StateMachine)]
	public class GetFsmVector3 : FsmStateAction
	{
		// Token: 0x0600351A RID: 13594 RVA: 0x00112750 File Offset: 0x00110950
		public override void Reset()
		{
			this.gameObject = null;
			this.fsmName = string.Empty;
			this.storeValue = null;
		}

		// Token: 0x0600351B RID: 13595 RVA: 0x00112770 File Offset: 0x00110970
		public override void OnEnter()
		{
			this.DoGetFsmVector3();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x0600351C RID: 13596 RVA: 0x0011278C File Offset: 0x0011098C
		public override void OnUpdate()
		{
			this.DoGetFsmVector3();
		}

		// Token: 0x0600351D RID: 13597 RVA: 0x00112794 File Offset: 0x00110994
		private void DoGetFsmVector3()
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
			FsmVector3 fsmVector = this.fsm.FsmVariables.GetFsmVector3(this.variableName.Value);
			if (fsmVector == null)
			{
				return;
			}
			this.storeValue.Value = fsmVector.Value;
		}

		// Token: 0x0400267F RID: 9855
		[RequiredField]
		public FsmOwnerDefault gameObject;

		// Token: 0x04002680 RID: 9856
		[UIHint(UIHint.FsmName)]
		[Tooltip("Optional name of FSM on Game Object")]
		public FsmString fsmName;

		// Token: 0x04002681 RID: 9857
		[RequiredField]
		[UIHint(UIHint.FsmVector3)]
		public FsmString variableName;

		// Token: 0x04002682 RID: 9858
		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmVector3 storeValue;

		// Token: 0x04002683 RID: 9859
		public bool everyFrame;

		// Token: 0x04002684 RID: 9860
		private GameObject goLastFrame;

		// Token: 0x04002685 RID: 9861
		private PlayMakerFSM fsm;
	}
}
