using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000ADB RID: 2779
	[Tooltip("Set the value of a Game Object Variable in another FSM. Accept null reference")]
	[ActionTarget(typeof(PlayMakerFSM), "gameObject,fsmName", false)]
	[ActionCategory(ActionCategory.StateMachine)]
	public class SetFsmGameObject : FsmStateAction
	{
		// Token: 0x06003AD1 RID: 15057 RVA: 0x0012A3E4 File Offset: 0x001285E4
		public override void Reset()
		{
			this.gameObject = null;
			this.fsmName = string.Empty;
			this.setValue = null;
			this.everyFrame = false;
		}

		// Token: 0x06003AD2 RID: 15058 RVA: 0x0012A40C File Offset: 0x0012860C
		public override void OnEnter()
		{
			this.DoSetFsmGameObject();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x06003AD3 RID: 15059 RVA: 0x0012A428 File Offset: 0x00128628
		private void DoSetFsmGameObject()
		{
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
				return;
			}
			FsmGameObject fsmGameObject = this.fsm.FsmVariables.FindFsmGameObject(this.variableName.Value);
			if (fsmGameObject != null)
			{
				fsmGameObject.Value = ((this.setValue != null) ? this.setValue.Value : null);
			}
			else
			{
				base.LogWarning("Could not find variable: " + this.variableName.Value);
			}
		}

		// Token: 0x06003AD4 RID: 15060 RVA: 0x0012A528 File Offset: 0x00128728
		public override void OnUpdate()
		{
			this.DoSetFsmGameObject();
		}

		// Token: 0x04002CFB RID: 11515
		[RequiredField]
		[Tooltip("The GameObject that owns the FSM.")]
		public FsmOwnerDefault gameObject;

		// Token: 0x04002CFC RID: 11516
		[UIHint(UIHint.FsmName)]
		[Tooltip("Optional name of FSM on Game Object")]
		public FsmString fsmName;

		// Token: 0x04002CFD RID: 11517
		[UIHint(UIHint.FsmGameObject)]
		[Tooltip("The name of the FSM variable.")]
		[RequiredField]
		public FsmString variableName;

		// Token: 0x04002CFE RID: 11518
		[Tooltip("Set the value of the variable.")]
		public FsmGameObject setValue;

		// Token: 0x04002CFF RID: 11519
		[Tooltip("Repeat every frame. Useful if the value is changing.")]
		public bool everyFrame;

		// Token: 0x04002D00 RID: 11520
		private GameObject goLastFrame;

		// Token: 0x04002D01 RID: 11521
		private string fsmNameLastFrame;

		// Token: 0x04002D02 RID: 11522
		private PlayMakerFSM fsm;
	}
}
