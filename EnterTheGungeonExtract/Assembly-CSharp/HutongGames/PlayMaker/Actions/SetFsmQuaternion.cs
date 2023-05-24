using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000ADF RID: 2783
	[Tooltip("Set the value of a Quaternion Variable in another FSM.")]
	[ActionTarget(typeof(PlayMakerFSM), "gameObject,fsmName", false)]
	[ActionCategory(ActionCategory.StateMachine)]
	public class SetFsmQuaternion : FsmStateAction
	{
		// Token: 0x06003AE5 RID: 15077 RVA: 0x0012A988 File Offset: 0x00128B88
		public override void Reset()
		{
			this.gameObject = null;
			this.fsmName = string.Empty;
			this.variableName = string.Empty;
			this.setValue = null;
			this.everyFrame = false;
		}

		// Token: 0x06003AE6 RID: 15078 RVA: 0x0012A9C0 File Offset: 0x00128BC0
		public override void OnEnter()
		{
			this.DoSetFsmQuaternion();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x06003AE7 RID: 15079 RVA: 0x0012A9DC File Offset: 0x00128BDC
		private void DoSetFsmQuaternion()
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
			FsmQuaternion fsmQuaternion = this.fsm.FsmVariables.GetFsmQuaternion(this.variableName.Value);
			if (fsmQuaternion != null)
			{
				fsmQuaternion.Value = this.setValue.Value;
			}
			else
			{
				base.LogWarning("Could not find variable: " + this.variableName.Value);
			}
		}

		// Token: 0x06003AE8 RID: 15080 RVA: 0x0012AAF0 File Offset: 0x00128CF0
		public override void OnUpdate()
		{
			this.DoSetFsmQuaternion();
		}

		// Token: 0x04002D1B RID: 11547
		[RequiredField]
		[Tooltip("The GameObject that owns the FSM.")]
		public FsmOwnerDefault gameObject;

		// Token: 0x04002D1C RID: 11548
		[Tooltip("Optional name of FSM on Game Object")]
		[UIHint(UIHint.FsmName)]
		public FsmString fsmName;

		// Token: 0x04002D1D RID: 11549
		[RequiredField]
		[UIHint(UIHint.FsmQuaternion)]
		[Tooltip("The name of the FSM variable.")]
		public FsmString variableName;

		// Token: 0x04002D1E RID: 11550
		[RequiredField]
		[Tooltip("Set the value of the variable.")]
		public FsmQuaternion setValue;

		// Token: 0x04002D1F RID: 11551
		[Tooltip("Repeat every frame. Useful if the value is changing.")]
		public bool everyFrame;

		// Token: 0x04002D20 RID: 11552
		private GameObject goLastFrame;

		// Token: 0x04002D21 RID: 11553
		private string fsmNameLastFrame;

		// Token: 0x04002D22 RID: 11554
		private PlayMakerFSM fsm;
	}
}
