using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000984 RID: 2436
	[Tooltip("Gets the name of the specified FSMs current state. Either reference the fsm component directly, or find it on a game object.")]
	[ActionTarget(typeof(PlayMakerFSM), "fsmComponent", false)]
	[ActionCategory(ActionCategory.StateMachine)]
	public class GetFsmState : FsmStateAction
	{
		// Token: 0x060034FA RID: 13562 RVA: 0x00111FF4 File Offset: 0x001101F4
		public override void Reset()
		{
			this.fsmComponent = null;
			this.gameObject = null;
			this.fsmName = string.Empty;
			this.storeResult = null;
			this.everyFrame = false;
		}

		// Token: 0x060034FB RID: 13563 RVA: 0x00112024 File Offset: 0x00110224
		public override void OnEnter()
		{
			this.DoGetFsmState();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x060034FC RID: 13564 RVA: 0x00112040 File Offset: 0x00110240
		public override void OnUpdate()
		{
			this.DoGetFsmState();
		}

		// Token: 0x060034FD RID: 13565 RVA: 0x00112048 File Offset: 0x00110248
		private void DoGetFsmState()
		{
			if (this.fsm == null)
			{
				if (this.fsmComponent != null)
				{
					this.fsm = this.fsmComponent;
				}
				else
				{
					GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
					if (ownerDefaultTarget != null)
					{
						this.fsm = ActionHelpers.GetGameObjectFsm(ownerDefaultTarget, this.fsmName.Value);
					}
				}
				if (this.fsm == null)
				{
					this.storeResult.Value = string.Empty;
					return;
				}
			}
			this.storeResult.Value = this.fsm.ActiveStateName;
		}

		// Token: 0x04002654 RID: 9812
		[Tooltip("Drag a PlayMakerFSM component here.")]
		public PlayMakerFSM fsmComponent;

		// Token: 0x04002655 RID: 9813
		[Tooltip("If not specifyng the component above, specify the GameObject that owns the FSM")]
		public FsmOwnerDefault gameObject;

		// Token: 0x04002656 RID: 9814
		[Tooltip("Optional name of Fsm on Game Object. If left blank it will find the first PlayMakerFSM on the GameObject.")]
		[UIHint(UIHint.FsmName)]
		public FsmString fsmName;

		// Token: 0x04002657 RID: 9815
		[Tooltip("Store the state name in a string variable.")]
		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmString storeResult;

		// Token: 0x04002658 RID: 9816
		[Tooltip("Repeat every frame. E.g.,  useful if you're waiting for the state to change.")]
		public bool everyFrame;

		// Token: 0x04002659 RID: 9817
		private PlayMakerFSM fsm;
	}
}
