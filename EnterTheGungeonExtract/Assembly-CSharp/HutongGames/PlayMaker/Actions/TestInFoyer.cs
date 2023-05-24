using System;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000C77 RID: 3191
	[ActionCategory(".Brave")]
	[Tooltip("Sends Events based on whether or not the player is in the foyer.")]
	public class TestInFoyer : FsmStateAction
	{
		// Token: 0x06004486 RID: 17542 RVA: 0x00162444 File Offset: 0x00160644
		public override void Reset()
		{
			this.isTrue = null;
			this.isFalse = null;
			this.everyFrame = false;
		}

		// Token: 0x06004487 RID: 17543 RVA: 0x0016245C File Offset: 0x0016065C
		public override void OnEnter()
		{
			base.Fsm.Event((!GameManager.Instance.IsFoyer) ? this.isFalse : this.isTrue);
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x06004488 RID: 17544 RVA: 0x0016249C File Offset: 0x0016069C
		public override void OnUpdate()
		{
			base.Fsm.Event((!GameManager.Instance.IsFoyer) ? this.isFalse : this.isTrue);
		}

		// Token: 0x04003692 RID: 13970
		[Tooltip("Event to send if the player is in the foyer.")]
		public FsmEvent isTrue;

		// Token: 0x04003693 RID: 13971
		[Tooltip("Event to send if the player is not in the foyer.")]
		public FsmEvent isFalse;

		// Token: 0x04003694 RID: 13972
		[Tooltip("Repeat every frame while the state is active.")]
		public bool everyFrame;
	}
}
