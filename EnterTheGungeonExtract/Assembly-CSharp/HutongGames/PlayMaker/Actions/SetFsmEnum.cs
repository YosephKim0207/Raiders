using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000AD9 RID: 2777
	[Tooltip("Set the value of a String Variable in another FSM.")]
	[ActionTarget(typeof(PlayMakerFSM), "gameObject,fsmName", false)]
	[ActionCategory(ActionCategory.StateMachine)]
	public class SetFsmEnum : FsmStateAction
	{
		// Token: 0x06003AC7 RID: 15047 RVA: 0x0012A124 File Offset: 0x00128324
		public override void Reset()
		{
			this.gameObject = null;
			this.fsmName = string.Empty;
			this.setValue = null;
		}

		// Token: 0x06003AC8 RID: 15048 RVA: 0x0012A144 File Offset: 0x00128344
		public override void OnEnter()
		{
			this.DoSetFsmEnum();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x06003AC9 RID: 15049 RVA: 0x0012A160 File Offset: 0x00128360
		private void DoSetFsmEnum()
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
			FsmEnum fsmEnum = this.fsm.FsmVariables.GetFsmEnum(this.variableName.Value);
			if (fsmEnum != null)
			{
				fsmEnum.Value = this.setValue.Value;
			}
			else
			{
				base.LogWarning("Could not find variable: " + this.variableName.Value);
			}
		}

		// Token: 0x06003ACA RID: 15050 RVA: 0x0012A274 File Offset: 0x00128474
		public override void OnUpdate()
		{
			this.DoSetFsmEnum();
		}

		// Token: 0x04002CEB RID: 11499
		[Tooltip("The GameObject that owns the FSM.")]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		// Token: 0x04002CEC RID: 11500
		[Tooltip("Optional name of FSM on Game Object.")]
		[UIHint(UIHint.FsmName)]
		public FsmString fsmName;

		// Token: 0x04002CED RID: 11501
		[UIHint(UIHint.FsmEnum)]
		[RequiredField]
		[Tooltip("Enum variable name needs to match the FSM variable name on Game Object.")]
		public FsmString variableName;

		// Token: 0x04002CEE RID: 11502
		[RequiredField]
		public FsmEnum setValue;

		// Token: 0x04002CEF RID: 11503
		[Tooltip("Repeat every frame. Useful if the value is changing.")]
		public bool everyFrame;

		// Token: 0x04002CF0 RID: 11504
		private GameObject goLastFrame;

		// Token: 0x04002CF1 RID: 11505
		private string fsmNameLastFrame;

		// Token: 0x04002CF2 RID: 11506
		private PlayMakerFSM fsm;
	}
}
