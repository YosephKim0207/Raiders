using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000AE4 RID: 2788
	[Tooltip("Set the value of a Vector2 Variable in another FSM.")]
	[ActionTarget(typeof(PlayMakerFSM), "gameObject,fsmName", false)]
	[ActionCategory(ActionCategory.StateMachine)]
	public class SetFsmVector2 : FsmStateAction
	{
		// Token: 0x06003AFE RID: 15102 RVA: 0x0012B0F0 File Offset: 0x001292F0
		public override void Reset()
		{
			this.gameObject = null;
			this.fsmName = string.Empty;
			this.setValue = null;
		}

		// Token: 0x06003AFF RID: 15103 RVA: 0x0012B110 File Offset: 0x00129310
		public override void OnEnter()
		{
			this.DoSetFsmVector2();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x06003B00 RID: 15104 RVA: 0x0012B12C File Offset: 0x0012932C
		private void DoSetFsmVector2()
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
			FsmVector2 fsmVector = this.fsm.FsmVariables.GetFsmVector2(this.variableName.Value);
			if (fsmVector != null)
			{
				fsmVector.Value = this.setValue.Value;
			}
			else
			{
				base.LogWarning("Could not find variable: " + this.variableName.Value);
			}
		}

		// Token: 0x06003B01 RID: 15105 RVA: 0x0012B240 File Offset: 0x00129440
		public override void OnUpdate()
		{
			this.DoSetFsmVector2();
		}

		// Token: 0x04002D46 RID: 11590
		[Tooltip("The GameObject that owns the FSM.")]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		// Token: 0x04002D47 RID: 11591
		[Tooltip("Optional name of FSM on Game Object")]
		[UIHint(UIHint.FsmName)]
		public FsmString fsmName;

		// Token: 0x04002D48 RID: 11592
		[Tooltip("The name of the FSM variable.")]
		[UIHint(UIHint.FsmVector2)]
		[RequiredField]
		public FsmString variableName;

		// Token: 0x04002D49 RID: 11593
		[Tooltip("Set the value of the variable.")]
		[RequiredField]
		public FsmVector2 setValue;

		// Token: 0x04002D4A RID: 11594
		[Tooltip("Repeat every frame. Useful if the value is changing.")]
		public bool everyFrame;

		// Token: 0x04002D4B RID: 11595
		private GameObject goLastFrame;

		// Token: 0x04002D4C RID: 11596
		private string fsmNameLastFrame;

		// Token: 0x04002D4D RID: 11597
		private PlayMakerFSM fsm;
	}
}
