using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000982 RID: 2434
	[ActionCategory(ActionCategory.StateMachine)]
	[ActionTarget(typeof(PlayMakerFSM), "gameObject,fsmName", false)]
	[Tooltip("Get the value of a Quaternion Variable from another FSM.")]
	public class GetFsmQuaternion : FsmStateAction
	{
		// Token: 0x060034F0 RID: 13552 RVA: 0x00111DD4 File Offset: 0x0010FFD4
		public override void Reset()
		{
			this.gameObject = null;
			this.fsmName = string.Empty;
			this.variableName = string.Empty;
			this.storeValue = null;
			this.everyFrame = false;
		}

		// Token: 0x060034F1 RID: 13553 RVA: 0x00111E0C File Offset: 0x0011000C
		public override void OnEnter()
		{
			this.DoGetFsmVariable();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x060034F2 RID: 13554 RVA: 0x00111E28 File Offset: 0x00110028
		public override void OnUpdate()
		{
			this.DoGetFsmVariable();
		}

		// Token: 0x060034F3 RID: 13555 RVA: 0x00111E30 File Offset: 0x00110030
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
			FsmQuaternion fsmQuaternion = this.fsm.FsmVariables.GetFsmQuaternion(this.variableName.Value);
			if (fsmQuaternion != null)
			{
				this.storeValue.Value = fsmQuaternion.Value;
			}
		}

		// Token: 0x04002646 RID: 9798
		[Tooltip("The GameObject that owns the FSM.")]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		// Token: 0x04002647 RID: 9799
		[UIHint(UIHint.FsmName)]
		[Tooltip("Optional name of FSM on Game Object")]
		public FsmString fsmName;

		// Token: 0x04002648 RID: 9800
		[UIHint(UIHint.FsmQuaternion)]
		[RequiredField]
		public FsmString variableName;

		// Token: 0x04002649 RID: 9801
		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmQuaternion storeValue;

		// Token: 0x0400264A RID: 9802
		[Tooltip("Repeat every frame.")]
		public bool everyFrame;

		// Token: 0x0400264B RID: 9803
		private GameObject goLastFrame;

		// Token: 0x0400264C RID: 9804
		protected PlayMakerFSM fsm;
	}
}
