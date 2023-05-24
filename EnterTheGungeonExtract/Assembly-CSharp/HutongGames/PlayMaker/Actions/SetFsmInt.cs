using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000ADC RID: 2780
	[Tooltip("Set the value of an Integer Variable in another FSM.")]
	[ActionTarget(typeof(PlayMakerFSM), "gameObject,fsmName", false)]
	[ActionCategory(ActionCategory.StateMachine)]
	public class SetFsmInt : FsmStateAction
	{
		// Token: 0x06003AD6 RID: 15062 RVA: 0x0012A538 File Offset: 0x00128738
		public override void Reset()
		{
			this.gameObject = null;
			this.fsmName = string.Empty;
			this.setValue = null;
		}

		// Token: 0x06003AD7 RID: 15063 RVA: 0x0012A558 File Offset: 0x00128758
		public override void OnEnter()
		{
			this.DoSetFsmInt();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x06003AD8 RID: 15064 RVA: 0x0012A574 File Offset: 0x00128774
		private void DoSetFsmInt()
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
			FsmInt fsmInt = this.fsm.FsmVariables.GetFsmInt(this.variableName.Value);
			if (fsmInt != null)
			{
				fsmInt.Value = this.setValue.Value;
			}
			else
			{
				base.LogWarning("Could not find variable: " + this.variableName.Value);
			}
		}

		// Token: 0x06003AD9 RID: 15065 RVA: 0x0012A688 File Offset: 0x00128888
		public override void OnUpdate()
		{
			this.DoSetFsmInt();
		}

		// Token: 0x04002D03 RID: 11523
		[Tooltip("The GameObject that owns the FSM.")]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		// Token: 0x04002D04 RID: 11524
		[Tooltip("Optional name of FSM on Game Object")]
		[UIHint(UIHint.FsmName)]
		public FsmString fsmName;

		// Token: 0x04002D05 RID: 11525
		[Tooltip("The name of the FSM variable.")]
		[RequiredField]
		[UIHint(UIHint.FsmInt)]
		public FsmString variableName;

		// Token: 0x04002D06 RID: 11526
		[Tooltip("Set the value of the variable.")]
		[RequiredField]
		public FsmInt setValue;

		// Token: 0x04002D07 RID: 11527
		[Tooltip("Repeat every frame. Useful if the value is changing.")]
		public bool everyFrame;

		// Token: 0x04002D08 RID: 11528
		private GameObject goLastFrame;

		// Token: 0x04002D09 RID: 11529
		private string fsmNameLastFrame;

		// Token: 0x04002D0A RID: 11530
		private PlayMakerFSM fsm;
	}
}
