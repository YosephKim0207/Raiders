using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x020008EC RID: 2284
	[Obsolete("This action was wip and accidentally released.")]
	[Tooltip("Set an item in an Array Variable in another FSM.")]
	[ActionCategory(ActionCategory.Array)]
	[ActionTarget(typeof(PlayMakerFSM), "gameObject,fsmName", false)]
	public class FsmArraySet : FsmStateAction
	{
		// Token: 0x06003274 RID: 12916 RVA: 0x001093E4 File Offset: 0x001075E4
		public override void Reset()
		{
			this.gameObject = null;
			this.fsmName = string.Empty;
			this.setValue = null;
		}

		// Token: 0x06003275 RID: 12917 RVA: 0x00109404 File Offset: 0x00107604
		public override void OnEnter()
		{
			this.DoSetFsmString();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x06003276 RID: 12918 RVA: 0x00109420 File Offset: 0x00107620
		private void DoSetFsmString()
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
			if (ownerDefaultTarget != this.goLastFrame)
			{
				this.goLastFrame = ownerDefaultTarget;
				this.fsm = ActionHelpers.GetGameObjectFsm(ownerDefaultTarget, this.fsmName.Value);
			}
			if (this.fsm == null)
			{
				base.LogWarning("Could not find FSM: " + this.fsmName.Value);
				return;
			}
			FsmString fsmString = this.fsm.FsmVariables.GetFsmString(this.variableName.Value);
			if (fsmString != null)
			{
				fsmString.Value = this.setValue.Value;
			}
			else
			{
				base.LogWarning("Could not find variable: " + this.variableName.Value);
			}
		}

		// Token: 0x06003277 RID: 12919 RVA: 0x00109508 File Offset: 0x00107708
		public override void OnUpdate()
		{
			this.DoSetFsmString();
		}

		// Token: 0x04002392 RID: 9106
		[Tooltip("The GameObject that owns the FSM.")]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		// Token: 0x04002393 RID: 9107
		[UIHint(UIHint.FsmName)]
		[Tooltip("Optional name of FSM on Game Object.")]
		public FsmString fsmName;

		// Token: 0x04002394 RID: 9108
		[Tooltip("The name of the FSM variable.")]
		[RequiredField]
		public FsmString variableName;

		// Token: 0x04002395 RID: 9109
		[Tooltip("Set the value of the variable.")]
		public FsmString setValue;

		// Token: 0x04002396 RID: 9110
		[Tooltip("Repeat every frame. Useful if the value is changing.")]
		public bool everyFrame;

		// Token: 0x04002397 RID: 9111
		private GameObject goLastFrame;

		// Token: 0x04002398 RID: 9112
		private PlayMakerFSM fsm;
	}
}
