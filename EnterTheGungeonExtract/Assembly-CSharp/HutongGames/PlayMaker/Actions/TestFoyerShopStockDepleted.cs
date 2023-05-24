using System;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000CD3 RID: 3283
	[ActionCategory(".NPCs")]
	public class TestFoyerShopStockDepleted : FsmStateAction
	{
		// Token: 0x060045B7 RID: 17847 RVA: 0x00169D98 File Offset: 0x00167F98
		public override void Reset()
		{
			this.CurrentStockDepleted = null;
			this.AllStockDepleted = null;
			this.NotDepleted = null;
		}

		// Token: 0x060045B8 RID: 17848 RVA: 0x00169DB0 File Offset: 0x00167FB0
		public override string ErrorCheck()
		{
			if (FsmEvent.IsNullOrEmpty(this.CurrentStockDepleted) && FsmEvent.IsNullOrEmpty(this.AllStockDepleted) && FsmEvent.IsNullOrEmpty(this.NotDepleted))
			{
				return "Action sends no events!";
			}
			return string.Empty;
		}

		// Token: 0x060045B9 RID: 17849 RVA: 0x00169DF0 File Offset: 0x00167FF0
		public override void OnEnter()
		{
			this.m_talkDoer = base.Owner.GetComponent<TalkDoerLite>();
			this.DoCompare();
			base.Finish();
		}

		// Token: 0x060045BA RID: 17850 RVA: 0x00169E10 File Offset: 0x00168010
		private void DoCompare()
		{
			if (this.m_talkDoer.ShopStockStatus == Tribool.Complete)
			{
				base.Fsm.Event(this.AllStockDepleted);
			}
			else if (this.m_talkDoer.ShopStockStatus == Tribool.Ready)
			{
				base.Fsm.Event(this.CurrentStockDepleted);
			}
			else
			{
				base.Fsm.Event(this.NotDepleted);
			}
		}

		// Token: 0x04003800 RID: 14336
		public FsmEvent CurrentStockDepleted;

		// Token: 0x04003801 RID: 14337
		public FsmEvent AllStockDepleted;

		// Token: 0x04003802 RID: 14338
		public FsmEvent NotDepleted;

		// Token: 0x04003803 RID: 14339
		private TalkDoerLite m_talkDoer;
	}
}
