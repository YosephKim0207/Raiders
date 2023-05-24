using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x0200095D RID: 2397
	[Tooltip("Tests if an FSM is in the specified State.")]
	[ActionCategory(ActionCategory.Logic)]
	[ActionTarget(typeof(PlayMakerFSM), "gameObject,fsmName", false)]
	public class FsmStateTest : FsmStateAction
	{
		// Token: 0x06003450 RID: 13392 RVA: 0x0010FB88 File Offset: 0x0010DD88
		public override void Reset()
		{
			this.gameObject = null;
			this.fsmName = null;
			this.stateName = null;
			this.trueEvent = null;
			this.falseEvent = null;
			this.storeResult = null;
			this.everyFrame = false;
		}

		// Token: 0x06003451 RID: 13393 RVA: 0x0010FBBC File Offset: 0x0010DDBC
		public override void OnEnter()
		{
			this.DoFsmStateTest();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x06003452 RID: 13394 RVA: 0x0010FBD8 File Offset: 0x0010DDD8
		public override void OnUpdate()
		{
			this.DoFsmStateTest();
		}

		// Token: 0x06003453 RID: 13395 RVA: 0x0010FBE0 File Offset: 0x0010DDE0
		private void DoFsmStateTest()
		{
			GameObject value = this.gameObject.Value;
			if (value == null)
			{
				return;
			}
			if (value != this.previousGo)
			{
				this.fsm = ActionHelpers.GetGameObjectFsm(value, this.fsmName.Value);
				this.previousGo = value;
			}
			if (this.fsm == null)
			{
				return;
			}
			bool flag = false;
			if (this.fsm.ActiveStateName == this.stateName.Value)
			{
				base.Fsm.Event(this.trueEvent);
				flag = true;
			}
			else
			{
				base.Fsm.Event(this.falseEvent);
			}
			this.storeResult.Value = flag;
		}

		// Token: 0x04002576 RID: 9590
		[RequiredField]
		[Tooltip("The GameObject that owns the FSM.")]
		public FsmGameObject gameObject;

		// Token: 0x04002577 RID: 9591
		[Tooltip("Optional name of Fsm on Game Object. Useful if there is more than one FSM on the GameObject.")]
		[UIHint(UIHint.FsmName)]
		public FsmString fsmName;

		// Token: 0x04002578 RID: 9592
		[Tooltip("Check to see if the FSM is in this state.")]
		[RequiredField]
		public FsmString stateName;

		// Token: 0x04002579 RID: 9593
		[Tooltip("Event to send if the FSM is in the specified state.")]
		public FsmEvent trueEvent;

		// Token: 0x0400257A RID: 9594
		[Tooltip("Event to send if the FSM is NOT in the specified state.")]
		public FsmEvent falseEvent;

		// Token: 0x0400257B RID: 9595
		[Tooltip("Store the result of this test in a bool variable. Useful if other actions depend on this test.")]
		[UIHint(UIHint.Variable)]
		public FsmBool storeResult;

		// Token: 0x0400257C RID: 9596
		[Tooltip("Repeat every frame. Useful if you're waiting for a particular state.")]
		public bool everyFrame;

		// Token: 0x0400257D RID: 9597
		private GameObject previousGo;

		// Token: 0x0400257E RID: 9598
		private PlayMakerFSM fsm;
	}
}
