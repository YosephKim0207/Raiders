using System;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000CD2 RID: 3282
	[Tooltip("Only use this in the Foyer!")]
	[ActionCategory(".NPCs")]
	public class TestCoopMode : FsmStateAction
	{
		// Token: 0x060045B3 RID: 17843 RVA: 0x00169D08 File Offset: 0x00167F08
		public override void Reset()
		{
			this.isTrue = null;
			this.isFalse = null;
			this.everyFrame = false;
		}

		// Token: 0x060045B4 RID: 17844 RVA: 0x00169D20 File Offset: 0x00167F20
		public override void OnEnter()
		{
			base.Fsm.Event((GameManager.Instance.CurrentGameType != GameManager.GameType.COOP_2_PLAYER) ? this.isFalse : this.isTrue);
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x060045B5 RID: 17845 RVA: 0x00169D60 File Offset: 0x00167F60
		public override void OnUpdate()
		{
			base.Fsm.Event((GameManager.Instance.CurrentGameType != GameManager.GameType.COOP_2_PLAYER) ? this.isFalse : this.isTrue);
		}

		// Token: 0x040037FD RID: 14333
		[Tooltip("Event to send if the player is in the foyer.")]
		public FsmEvent isTrue;

		// Token: 0x040037FE RID: 14334
		[Tooltip("Event to send if the player is not in the foyer.")]
		public FsmEvent isFalse;

		// Token: 0x040037FF RID: 14335
		[Tooltip("Repeat every frame while the state is active.")]
		public bool everyFrame;
	}
}
