using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x0200097E RID: 2430
	[Tooltip("Get the value of a Game Object Variable from another FSM.")]
	[ActionTarget(typeof(PlayMakerFSM), "gameObject,fsmName", false)]
	[ActionCategory(ActionCategory.StateMachine)]
	public class GetFsmGameObject : FsmStateAction
	{
		// Token: 0x060034DC RID: 13532 RVA: 0x001119BC File Offset: 0x0010FBBC
		public override void Reset()
		{
			this.gameObject = null;
			this.fsmName = string.Empty;
			this.storeValue = null;
		}

		// Token: 0x060034DD RID: 13533 RVA: 0x001119DC File Offset: 0x0010FBDC
		public override void OnEnter()
		{
			this.DoGetFsmGameObject();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x060034DE RID: 13534 RVA: 0x001119F8 File Offset: 0x0010FBF8
		public override void OnUpdate()
		{
			this.DoGetFsmGameObject();
		}

		// Token: 0x060034DF RID: 13535 RVA: 0x00111A00 File Offset: 0x0010FC00
		private void DoGetFsmGameObject()
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
			FsmGameObject fsmGameObject = this.fsm.FsmVariables.GetFsmGameObject(this.variableName.Value);
			if (fsmGameObject == null)
			{
				return;
			}
			this.storeValue.Value = fsmGameObject.Value;
		}

		// Token: 0x0400262A RID: 9770
		[RequiredField]
		public FsmOwnerDefault gameObject;

		// Token: 0x0400262B RID: 9771
		[Tooltip("Optional name of FSM on Game Object")]
		[UIHint(UIHint.FsmName)]
		public FsmString fsmName;

		// Token: 0x0400262C RID: 9772
		[UIHint(UIHint.FsmGameObject)]
		[RequiredField]
		public FsmString variableName;

		// Token: 0x0400262D RID: 9773
		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmGameObject storeValue;

		// Token: 0x0400262E RID: 9774
		public bool everyFrame;

		// Token: 0x0400262F RID: 9775
		private GameObject goLastFrame;

		// Token: 0x04002630 RID: 9776
		private PlayMakerFSM fsm;
	}
}
