using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x0200097B RID: 2427
	[Tooltip("Get the value of a Color Variable from another FSM.")]
	[ActionTarget(typeof(PlayMakerFSM), "gameObject,fsmName", false)]
	[ActionCategory(ActionCategory.StateMachine)]
	public class GetFsmColor : FsmStateAction
	{
		// Token: 0x060034CD RID: 13517 RVA: 0x001116C4 File Offset: 0x0010F8C4
		public override void Reset()
		{
			this.gameObject = null;
			this.fsmName = string.Empty;
			this.storeValue = null;
		}

		// Token: 0x060034CE RID: 13518 RVA: 0x001116E4 File Offset: 0x0010F8E4
		public override void OnEnter()
		{
			this.DoGetFsmColor();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x060034CF RID: 13519 RVA: 0x00111700 File Offset: 0x0010F900
		public override void OnUpdate()
		{
			this.DoGetFsmColor();
		}

		// Token: 0x060034D0 RID: 13520 RVA: 0x00111708 File Offset: 0x0010F908
		private void DoGetFsmColor()
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
			FsmColor fsmColor = this.fsm.FsmVariables.GetFsmColor(this.variableName.Value);
			if (fsmColor == null)
			{
				return;
			}
			this.storeValue.Value = fsmColor.Value;
		}

		// Token: 0x04002615 RID: 9749
		[RequiredField]
		public FsmOwnerDefault gameObject;

		// Token: 0x04002616 RID: 9750
		[Tooltip("Optional name of FSM on Game Object")]
		[UIHint(UIHint.FsmName)]
		public FsmString fsmName;

		// Token: 0x04002617 RID: 9751
		[UIHint(UIHint.FsmColor)]
		[RequiredField]
		public FsmString variableName;

		// Token: 0x04002618 RID: 9752
		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmColor storeValue;

		// Token: 0x04002619 RID: 9753
		public bool everyFrame;

		// Token: 0x0400261A RID: 9754
		private GameObject goLastFrame;

		// Token: 0x0400261B RID: 9755
		private PlayMakerFSM fsm;
	}
}
