using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000ADE RID: 2782
	[ActionCategory(ActionCategory.StateMachine)]
	[Tooltip("Set the value of an Object Variable in another FSM.")]
	[ActionTarget(typeof(PlayMakerFSM), "gameObject,fsmName", false)]
	public class SetFsmObject : FsmStateAction
	{
		// Token: 0x06003AE0 RID: 15072 RVA: 0x0012A810 File Offset: 0x00128A10
		public override void Reset()
		{
			this.gameObject = null;
			this.fsmName = string.Empty;
			this.variableName = string.Empty;
			this.setValue = null;
			this.everyFrame = false;
		}

		// Token: 0x06003AE1 RID: 15073 RVA: 0x0012A848 File Offset: 0x00128A48
		public override void OnEnter()
		{
			this.DoSetFsmBool();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x06003AE2 RID: 15074 RVA: 0x0012A864 File Offset: 0x00128A64
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
			FsmObject fsmObject = this.fsm.FsmVariables.GetFsmObject(this.variableName.Value);
			if (fsmObject != null)
			{
				fsmObject.Value = this.setValue.Value;
			}
			else
			{
				base.LogWarning("Could not find variable: " + this.variableName.Value);
			}
		}

		// Token: 0x06003AE3 RID: 15075 RVA: 0x0012A978 File Offset: 0x00128B78
		public override void OnUpdate()
		{
			this.DoSetFsmBool();
		}

		// Token: 0x04002D13 RID: 11539
		[Tooltip("The GameObject that owns the FSM.")]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		// Token: 0x04002D14 RID: 11540
		[Tooltip("Optional name of FSM on Game Object")]
		[UIHint(UIHint.FsmName)]
		public FsmString fsmName;

		// Token: 0x04002D15 RID: 11541
		[RequiredField]
		[Tooltip("The name of the FSM variable.")]
		[UIHint(UIHint.FsmObject)]
		public FsmString variableName;

		// Token: 0x04002D16 RID: 11542
		[Tooltip("Set the value of the variable.")]
		public FsmObject setValue;

		// Token: 0x04002D17 RID: 11543
		[Tooltip("Repeat every frame. Useful if the value is changing.")]
		public bool everyFrame;

		// Token: 0x04002D18 RID: 11544
		private GameObject goLastFrame;

		// Token: 0x04002D19 RID: 11545
		private string fsmNameLastFrame;

		// Token: 0x04002D1A RID: 11546
		private PlayMakerFSM fsm;
	}
}
