using System;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000CD0 RID: 3280
	[Tooltip("Checks whether or not the player has a certain amount of money.")]
	[ActionCategory(".NPCs")]
	public class TestConsumable : FsmStateAction
	{
		// Token: 0x060045A8 RID: 17832 RVA: 0x00169A18 File Offset: 0x00167C18
		public override void Reset()
		{
			this.consumableType = BravePlayMakerUtility.ConsumableType.Currency;
			this.value = 0f;
			this.greaterThan = null;
			this.greaterThanOrEqual = null;
			this.equal = null;
			this.lessThanOrEqual = null;
			this.lessThan = null;
			this.everyFrame = false;
		}

		// Token: 0x060045A9 RID: 17833 RVA: 0x00169A68 File Offset: 0x00167C68
		public override string ErrorCheck()
		{
			if (FsmEvent.IsNullOrEmpty(this.greaterThan) && FsmEvent.IsNullOrEmpty(this.greaterThanOrEqual) && FsmEvent.IsNullOrEmpty(this.equal) && FsmEvent.IsNullOrEmpty(this.lessThanOrEqual) && FsmEvent.IsNullOrEmpty(this.lessThan))
			{
				return "Action sends no events!";
			}
			return string.Empty;
		}

		// Token: 0x060045AA RID: 17834 RVA: 0x00169AD0 File Offset: 0x00167CD0
		public override void OnEnter()
		{
			this.m_talkDoer = base.Owner.GetComponent<TalkDoerLite>();
			this.DoCompare();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x060045AB RID: 17835 RVA: 0x00169AFC File Offset: 0x00167CFC
		public override void OnUpdate()
		{
			this.DoCompare();
		}

		// Token: 0x060045AC RID: 17836 RVA: 0x00169B04 File Offset: 0x00167D04
		private void DoCompare()
		{
			float consumableValue = BravePlayMakerUtility.GetConsumableValue(this.m_talkDoer.TalkingPlayer, this.consumableType);
			if (consumableValue > this.value.Value)
			{
				base.Fsm.Event(this.greaterThan);
			}
			if (consumableValue >= this.value.Value)
			{
				base.Fsm.Event(this.greaterThanOrEqual);
			}
			if (consumableValue == this.value.Value)
			{
				base.Fsm.Event(this.equal);
			}
			if (consumableValue <= this.value.Value)
			{
				base.Fsm.Event(this.lessThanOrEqual);
			}
			if (consumableValue < this.value.Value)
			{
				base.Fsm.Event(this.lessThan);
			}
		}

		// Token: 0x040037F0 RID: 14320
		[Tooltip("Type of consumable to check.")]
		public BravePlayMakerUtility.ConsumableType consumableType;

		// Token: 0x040037F1 RID: 14321
		[Tooltip("Value to check.")]
		public FsmFloat value;

		// Token: 0x040037F2 RID: 14322
		[Tooltip("Event sent if the amount is greater than <value>.")]
		public FsmEvent greaterThan;

		// Token: 0x040037F3 RID: 14323
		[Tooltip("Event sent if the amount is greater than or equal to <value>.")]
		public FsmEvent greaterThanOrEqual;

		// Token: 0x040037F4 RID: 14324
		[Tooltip("Event sent if the amount equals <value>.")]
		public FsmEvent equal;

		// Token: 0x040037F5 RID: 14325
		[Tooltip("Event sent if the amount is less than or equal to <value>.")]
		public FsmEvent lessThanOrEqual;

		// Token: 0x040037F6 RID: 14326
		[Tooltip("Event sent if the amount is less than <value>.")]
		public FsmEvent lessThan;

		// Token: 0x040037F7 RID: 14327
		public bool everyFrame;

		// Token: 0x040037F8 RID: 14328
		private TalkDoerLite m_talkDoer;
	}
}
