using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000AE5 RID: 2789
	[ActionCategory(ActionCategory.StateMachine)]
	[ActionTarget(typeof(PlayMakerFSM), "gameObject,fsmName", false)]
	[Tooltip("Set the value of a Vector3 Variable in another FSM.")]
	public class SetFsmVector3 : FsmStateAction
	{
		// Token: 0x06003B03 RID: 15107 RVA: 0x0012B250 File Offset: 0x00129450
		public override void Reset()
		{
			this.gameObject = null;
			this.fsmName = string.Empty;
			this.setValue = null;
		}

		// Token: 0x06003B04 RID: 15108 RVA: 0x0012B270 File Offset: 0x00129470
		public override void OnEnter()
		{
			this.DoSetFsmVector3();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x06003B05 RID: 15109 RVA: 0x0012B28C File Offset: 0x0012948C
		private void DoSetFsmVector3()
		{
			if (this.setValue == null)
			{
				return;
			}
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
			if (ownerDefaultTarget == null)
			{
				return;
			}
			if (ownerDefaultTarget != this.goLastFrame || this.fsmName.Value != this.fsmNameLastFrame)
			{
				this.goLastFrame = ownerDefaultTarget;
				this.fsmNameLastFrame = this.fsmName.Value;
				this.fsm = ActionHelpers.GetGameObjectFsm(ownerDefaultTarget, this.fsmName.Value);
			}
			if (this.fsm == null)
			{
				base.LogWarning("Could not find FSM: " + this.fsmName.Value);
				return;
			}
			FsmVector3 fsmVector = this.fsm.FsmVariables.GetFsmVector3(this.variableName.Value);
			if (fsmVector != null)
			{
				fsmVector.Value = this.setValue.Value;
			}
			else
			{
				base.LogWarning("Could not find variable: " + this.variableName.Value);
			}
		}

		// Token: 0x06003B06 RID: 15110 RVA: 0x0012B3A0 File Offset: 0x001295A0
		public override void OnUpdate()
		{
			this.DoSetFsmVector3();
		}

		// Token: 0x04002D4E RID: 11598
		[Tooltip("The GameObject that owns the FSM.")]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		// Token: 0x04002D4F RID: 11599
		[UIHint(UIHint.FsmName)]
		[Tooltip("Optional name of FSM on Game Object")]
		public FsmString fsmName;

		// Token: 0x04002D50 RID: 11600
		[Tooltip("The name of the FSM variable.")]
		[UIHint(UIHint.FsmVector3)]
		[RequiredField]
		public FsmString variableName;

		// Token: 0x04002D51 RID: 11601
		[Tooltip("Set the value of the variable.")]
		[RequiredField]
		public FsmVector3 setValue;

		// Token: 0x04002D52 RID: 11602
		[Tooltip("Repeat every frame. Useful if the value is changing.")]
		public bool everyFrame;

		// Token: 0x04002D53 RID: 11603
		private GameObject goLastFrame;

		// Token: 0x04002D54 RID: 11604
		private string fsmNameLastFrame;

		// Token: 0x04002D55 RID: 11605
		private PlayMakerFSM fsm;
	}
}
