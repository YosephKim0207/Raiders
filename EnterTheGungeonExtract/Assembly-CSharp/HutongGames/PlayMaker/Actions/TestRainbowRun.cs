using System;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000C78 RID: 3192
	[Tooltip("Sends Events based on whether or not the player is in rainbow mode.")]
	[ActionCategory(".Brave")]
	public class TestRainbowRun : FsmStateAction
	{
		// Token: 0x0600448A RID: 17546 RVA: 0x001624D4 File Offset: 0x001606D4
		public override void Reset()
		{
			this.isTrue = null;
			this.isFalse = null;
			this.everyFrame = false;
		}

		// Token: 0x0600448B RID: 17547 RVA: 0x001624EC File Offset: 0x001606EC
		public override void OnEnter()
		{
			base.Fsm.Event((!GameStatsManager.Instance.rainbowRunToggled) ? this.isFalse : this.isTrue);
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x0600448C RID: 17548 RVA: 0x0016252C File Offset: 0x0016072C
		public override void OnUpdate()
		{
			base.Fsm.Event((!GameStatsManager.Instance.rainbowRunToggled) ? this.isFalse : this.isTrue);
		}

		// Token: 0x04003695 RID: 13973
		[Tooltip("Event to send if rainbow mode is active.")]
		public FsmEvent isTrue;

		// Token: 0x04003696 RID: 13974
		[Tooltip("Event to send if rainbow mode is inactive.")]
		public FsmEvent isFalse;

		// Token: 0x04003697 RID: 13975
		[Tooltip("Repeat every frame while the state is active.")]
		public bool everyFrame;
	}
}
