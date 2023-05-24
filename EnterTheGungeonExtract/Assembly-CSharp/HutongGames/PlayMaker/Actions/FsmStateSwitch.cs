using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x0200095C RID: 2396
	[Tooltip("Sends Events based on the current State of an FSM.")]
	[ActionTarget(typeof(PlayMakerFSM), "gameObject,fsmName", false)]
	[ActionCategory(ActionCategory.Logic)]
	public class FsmStateSwitch : FsmStateAction
	{
		// Token: 0x0600344B RID: 13387 RVA: 0x0010FA74 File Offset: 0x0010DC74
		public override void Reset()
		{
			this.gameObject = null;
			this.fsmName = null;
			this.compareTo = new FsmString[1];
			this.sendEvent = new FsmEvent[1];
			this.everyFrame = false;
		}

		// Token: 0x0600344C RID: 13388 RVA: 0x0010FAA4 File Offset: 0x0010DCA4
		public override void OnEnter()
		{
			this.DoFsmStateSwitch();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x0600344D RID: 13389 RVA: 0x0010FAC0 File Offset: 0x0010DCC0
		public override void OnUpdate()
		{
			this.DoFsmStateSwitch();
		}

		// Token: 0x0600344E RID: 13390 RVA: 0x0010FAC8 File Offset: 0x0010DCC8
		private void DoFsmStateSwitch()
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
			string activeStateName = this.fsm.ActiveStateName;
			for (int i = 0; i < this.compareTo.Length; i++)
			{
				if (activeStateName == this.compareTo[i].Value)
				{
					base.Fsm.Event(this.sendEvent[i]);
					return;
				}
			}
		}

		// Token: 0x0400256F RID: 9583
		[Tooltip("The GameObject that owns the FSM.")]
		[RequiredField]
		public FsmGameObject gameObject;

		// Token: 0x04002570 RID: 9584
		[UIHint(UIHint.FsmName)]
		[Tooltip("Optional name of Fsm on GameObject. Useful if there is more than one FSM on the GameObject.")]
		public FsmString fsmName;

		// Token: 0x04002571 RID: 9585
		[CompoundArray("State Switches", "Compare State", "Send Event")]
		public FsmString[] compareTo;

		// Token: 0x04002572 RID: 9586
		public FsmEvent[] sendEvent;

		// Token: 0x04002573 RID: 9587
		[Tooltip("Repeat every frame. Useful if you're waiting for a particular result.")]
		public bool everyFrame;

		// Token: 0x04002574 RID: 9588
		private GameObject previousGo;

		// Token: 0x04002575 RID: 9589
		private PlayMakerFSM fsm;
	}
}
