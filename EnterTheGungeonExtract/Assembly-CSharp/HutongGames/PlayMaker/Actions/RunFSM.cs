using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000AAE RID: 2734
	[ActionCategory(ActionCategory.StateMachine)]
	[Tooltip("Creates an FSM from a saved FSM Template.")]
	public class RunFSM : RunFSMAction
	{
		// Token: 0x060039F8 RID: 14840 RVA: 0x0012785C File Offset: 0x00125A5C
		public override void Reset()
		{
			this.fsmTemplateControl = new FsmTemplateControl();
			this.storeID = null;
			this.runFsm = null;
		}

		// Token: 0x060039F9 RID: 14841 RVA: 0x00127878 File Offset: 0x00125A78
		public override void Awake()
		{
			if (this.fsmTemplateControl.fsmTemplate != null && Application.isPlaying)
			{
				this.runFsm = base.Fsm.CreateSubFsm(this.fsmTemplateControl);
			}
		}

		// Token: 0x060039FA RID: 14842 RVA: 0x001278B4 File Offset: 0x00125AB4
		public override void OnEnter()
		{
			if (this.runFsm == null)
			{
				base.Finish();
				return;
			}
			this.fsmTemplateControl.UpdateValues();
			this.fsmTemplateControl.ApplyOverrides(this.runFsm);
			this.runFsm.OnEnable();
			if (!this.runFsm.Started)
			{
				this.runFsm.Start();
			}
			this.storeID.Value = this.fsmTemplateControl.ID;
			this.CheckIfFinished();
		}

		// Token: 0x060039FB RID: 14843 RVA: 0x00127934 File Offset: 0x00125B34
		protected override void CheckIfFinished()
		{
			if (this.runFsm == null || this.runFsm.Finished)
			{
				base.Finish();
				base.Fsm.Event(this.finishEvent);
			}
		}

		// Token: 0x04002C37 RID: 11319
		public FsmTemplateControl fsmTemplateControl = new FsmTemplateControl();

		// Token: 0x04002C38 RID: 11320
		[UIHint(UIHint.Variable)]
		public FsmInt storeID;

		// Token: 0x04002C39 RID: 11321
		[Tooltip("Event to send when the FSM has finished (usually because it ran a Finish FSM action).")]
		public FsmEvent finishEvent;
	}
}
