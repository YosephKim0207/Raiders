using System;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000C7E RID: 3198
	[ActionCategory(".Brave")]
	[Tooltip("Sends Events based on whether or not the player is in turbo mode.")]
	public class TestTurboMode : FsmStateAction
	{
		// Token: 0x0600449B RID: 17563 RVA: 0x00162A28 File Offset: 0x00160C28
		public override void Reset()
		{
			this.isTrue = null;
			this.isFalse = null;
			this.everyFrame = false;
		}

		// Token: 0x0600449C RID: 17564 RVA: 0x00162A40 File Offset: 0x00160C40
		public override void OnEnter()
		{
			base.Fsm.Event((!GameStatsManager.Instance.isTurboMode) ? this.isFalse : this.isTrue);
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x0600449D RID: 17565 RVA: 0x00162A80 File Offset: 0x00160C80
		public override void OnUpdate()
		{
			base.Fsm.Event((!GameStatsManager.Instance.isTurboMode) ? this.isFalse : this.isTrue);
		}

		// Token: 0x040036B1 RID: 14001
		[Tooltip("Event to send if turbo mode is active.")]
		public FsmEvent isTrue;

		// Token: 0x040036B2 RID: 14002
		[Tooltip("Event to send if turbo mode is inactive.")]
		public FsmEvent isFalse;

		// Token: 0x040036B3 RID: 14003
		[Tooltip("Repeat every frame while the state is active.")]
		public bool everyFrame;
	}
}
