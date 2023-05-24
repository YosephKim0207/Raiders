using System;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000CD5 RID: 3285
	[ActionCategory(".NPCs")]
	[Tooltip("Checks whether or not the player has a certain amount of health.")]
	public class TestPlayerHealth : FsmStateAction
	{
		// Token: 0x060045C4 RID: 17860 RVA: 0x0016A06C File Offset: 0x0016826C
		public override void Reset()
		{
			this.UsePercentage = false;
			this.value = 0f;
			this.greaterThan = null;
			this.greaterThanOrEqual = null;
			this.equal = null;
			this.lessThanOrEqual = null;
			this.lessThan = null;
			this.everyFrame = false;
		}

		// Token: 0x060045C5 RID: 17861 RVA: 0x0016A0C0 File Offset: 0x001682C0
		public override string ErrorCheck()
		{
			if (FsmEvent.IsNullOrEmpty(this.greaterThan) && FsmEvent.IsNullOrEmpty(this.greaterThanOrEqual) && FsmEvent.IsNullOrEmpty(this.equal) && FsmEvent.IsNullOrEmpty(this.lessThanOrEqual) && FsmEvent.IsNullOrEmpty(this.lessThan))
			{
				return "Action sends no events!";
			}
			return string.Empty;
		}

		// Token: 0x060045C6 RID: 17862 RVA: 0x0016A128 File Offset: 0x00168328
		public override void OnEnter()
		{
			this.m_talkDoer = base.Owner.GetComponent<TalkDoerLite>();
			this.DoCompare();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x060045C7 RID: 17863 RVA: 0x0016A154 File Offset: 0x00168354
		public override void OnUpdate()
		{
			this.DoCompare();
		}

		// Token: 0x060045C8 RID: 17864 RVA: 0x0016A15C File Offset: 0x0016835C
		private void DoCompare()
		{
			float num = this.m_talkDoer.TalkingPlayer.healthHaver.GetCurrentHealth();
			if (this.UsePercentage.Value)
			{
				num = this.m_talkDoer.TalkingPlayer.healthHaver.GetCurrentHealthPercentage();
			}
			if (num > this.value.Value)
			{
				base.Fsm.Event(this.greaterThan);
			}
			if (num >= this.value.Value)
			{
				base.Fsm.Event(this.greaterThanOrEqual);
			}
			if (num == this.value.Value)
			{
				base.Fsm.Event(this.equal);
			}
			if (num <= this.value.Value)
			{
				base.Fsm.Event(this.lessThanOrEqual);
			}
			if (num < this.value.Value)
			{
				base.Fsm.Event(this.lessThan);
			}
		}

		// Token: 0x04003807 RID: 14343
		[Tooltip("Check Percent")]
		public FsmBool UsePercentage;

		// Token: 0x04003808 RID: 14344
		[Tooltip("Value to check.")]
		public FsmFloat value;

		// Token: 0x04003809 RID: 14345
		[Tooltip("Event sent if the amount is greater than <value>.")]
		public FsmEvent greaterThan;

		// Token: 0x0400380A RID: 14346
		[Tooltip("Event sent if the amount is greater than or equal to <value>.")]
		public FsmEvent greaterThanOrEqual;

		// Token: 0x0400380B RID: 14347
		[Tooltip("Event sent if the amount equals <value>.")]
		public FsmEvent equal;

		// Token: 0x0400380C RID: 14348
		[Tooltip("Event sent if the amount is less than or equal to <value>.")]
		public FsmEvent lessThanOrEqual;

		// Token: 0x0400380D RID: 14349
		[Tooltip("Event sent if the amount is less than <value>.")]
		public FsmEvent lessThan;

		// Token: 0x0400380E RID: 14350
		public bool everyFrame;

		// Token: 0x0400380F RID: 14351
		private TalkDoerLite m_talkDoer;
	}
}
