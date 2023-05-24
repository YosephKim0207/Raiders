using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000AD8 RID: 2776
	[ActionTarget(typeof(PlayMakerFSM), "gameObject,fsmName", false)]
	[Tooltip("Set the value of a Color Variable in another FSM.")]
	[ActionCategory(ActionCategory.StateMachine)]
	public class SetFsmColor : FsmStateAction
	{
		// Token: 0x06003AC2 RID: 15042 RVA: 0x00129FC4 File Offset: 0x001281C4
		public override void Reset()
		{
			this.gameObject = null;
			this.fsmName = string.Empty;
			this.setValue = null;
		}

		// Token: 0x06003AC3 RID: 15043 RVA: 0x00129FE4 File Offset: 0x001281E4
		public override void OnEnter()
		{
			this.DoSetFsmColor();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x06003AC4 RID: 15044 RVA: 0x0012A000 File Offset: 0x00128200
		private void DoSetFsmColor()
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
			FsmColor fsmColor = this.fsm.FsmVariables.GetFsmColor(this.variableName.Value);
			if (fsmColor != null)
			{
				fsmColor.Value = this.setValue.Value;
			}
			else
			{
				base.LogWarning("Could not find variable: " + this.variableName.Value);
			}
		}

		// Token: 0x06003AC5 RID: 15045 RVA: 0x0012A114 File Offset: 0x00128314
		public override void OnUpdate()
		{
			this.DoSetFsmColor();
		}

		// Token: 0x04002CE3 RID: 11491
		[Tooltip("The GameObject that owns the FSM.")]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		// Token: 0x04002CE4 RID: 11492
		[UIHint(UIHint.FsmName)]
		[Tooltip("Optional name of FSM on Game Object")]
		public FsmString fsmName;

		// Token: 0x04002CE5 RID: 11493
		[Tooltip("The name of the FSM variable.")]
		[RequiredField]
		[UIHint(UIHint.FsmColor)]
		public FsmString variableName;

		// Token: 0x04002CE6 RID: 11494
		[RequiredField]
		[Tooltip("Set the value of the variable.")]
		public FsmColor setValue;

		// Token: 0x04002CE7 RID: 11495
		[Tooltip("Repeat every frame. Useful if the value is changing.")]
		public bool everyFrame;

		// Token: 0x04002CE8 RID: 11496
		private GameObject goLastFrame;

		// Token: 0x04002CE9 RID: 11497
		private string fsmNameLastFrame;

		// Token: 0x04002CEA RID: 11498
		private PlayMakerFSM fsm;
	}
}
