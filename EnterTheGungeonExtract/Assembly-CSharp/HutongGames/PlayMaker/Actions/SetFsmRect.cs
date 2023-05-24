using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000AE0 RID: 2784
	[Tooltip("Set the value of a Rect Variable in another FSM.")]
	[ActionCategory(ActionCategory.StateMachine)]
	[ActionTarget(typeof(PlayMakerFSM), "gameObject,fsmName", false)]
	public class SetFsmRect : FsmStateAction
	{
		// Token: 0x06003AEA RID: 15082 RVA: 0x0012AB00 File Offset: 0x00128D00
		public override void Reset()
		{
			this.gameObject = null;
			this.fsmName = string.Empty;
			this.variableName = string.Empty;
			this.setValue = null;
			this.everyFrame = false;
		}

		// Token: 0x06003AEB RID: 15083 RVA: 0x0012AB38 File Offset: 0x00128D38
		public override void OnEnter()
		{
			this.DoSetFsmBool();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x06003AEC RID: 15084 RVA: 0x0012AB54 File Offset: 0x00128D54
		private void DoSetFsmBool()
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
			FsmRect fsmRect = this.fsm.FsmVariables.GetFsmRect(this.variableName.Value);
			if (fsmRect != null)
			{
				fsmRect.Value = this.setValue.Value;
			}
			else
			{
				base.LogWarning("Could not find variable: " + this.variableName.Value);
			}
		}

		// Token: 0x06003AED RID: 15085 RVA: 0x0012AC68 File Offset: 0x00128E68
		public override void OnUpdate()
		{
			this.DoSetFsmBool();
		}

		// Token: 0x04002D23 RID: 11555
		[RequiredField]
		[Tooltip("The GameObject that owns the FSM.")]
		public FsmOwnerDefault gameObject;

		// Token: 0x04002D24 RID: 11556
		[Tooltip("Optional name of FSM on Game Object")]
		[UIHint(UIHint.FsmName)]
		public FsmString fsmName;

		// Token: 0x04002D25 RID: 11557
		[Tooltip("The name of the FSM variable.")]
		[RequiredField]
		[UIHint(UIHint.FsmRect)]
		public FsmString variableName;

		// Token: 0x04002D26 RID: 11558
		[RequiredField]
		[Tooltip("Set the value of the variable.")]
		public FsmRect setValue;

		// Token: 0x04002D27 RID: 11559
		[Tooltip("Repeat every frame. Useful if the value is changing.")]
		public bool everyFrame;

		// Token: 0x04002D28 RID: 11560
		private GameObject goLastFrame;

		// Token: 0x04002D29 RID: 11561
		private string fsmNameLastFrame;

		// Token: 0x04002D2A RID: 11562
		private PlayMakerFSM fsm;
	}
}
