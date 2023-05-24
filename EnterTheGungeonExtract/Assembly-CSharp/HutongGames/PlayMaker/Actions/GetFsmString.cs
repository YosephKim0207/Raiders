using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000985 RID: 2437
	[Tooltip("Get the value of a String Variable from another FSM.")]
	[ActionTarget(typeof(PlayMakerFSM), "gameObject,fsmName", false)]
	[ActionCategory(ActionCategory.StateMachine)]
	public class GetFsmString : FsmStateAction
	{
		// Token: 0x060034FF RID: 13567 RVA: 0x00112100 File Offset: 0x00110300
		public override void Reset()
		{
			this.gameObject = null;
			this.fsmName = string.Empty;
			this.storeValue = null;
		}

		// Token: 0x06003500 RID: 13568 RVA: 0x00112120 File Offset: 0x00110320
		public override void OnEnter()
		{
			this.DoGetFsmString();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x06003501 RID: 13569 RVA: 0x0011213C File Offset: 0x0011033C
		public override void OnUpdate()
		{
			this.DoGetFsmString();
		}

		// Token: 0x06003502 RID: 13570 RVA: 0x00112144 File Offset: 0x00110344
		private void DoGetFsmString()
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
			FsmString fsmString = this.fsm.FsmVariables.GetFsmString(this.variableName.Value);
			if (fsmString == null)
			{
				return;
			}
			this.storeValue.Value = fsmString.Value;
		}

		// Token: 0x0400265A RID: 9818
		[RequiredField]
		public FsmOwnerDefault gameObject;

		// Token: 0x0400265B RID: 9819
		[Tooltip("Optional name of FSM on Game Object")]
		[UIHint(UIHint.FsmName)]
		public FsmString fsmName;

		// Token: 0x0400265C RID: 9820
		[UIHint(UIHint.FsmString)]
		[RequiredField]
		public FsmString variableName;

		// Token: 0x0400265D RID: 9821
		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmString storeValue;

		// Token: 0x0400265E RID: 9822
		public bool everyFrame;

		// Token: 0x0400265F RID: 9823
		private GameObject goLastFrame;

		// Token: 0x04002660 RID: 9824
		private PlayMakerFSM fsm;
	}
}
