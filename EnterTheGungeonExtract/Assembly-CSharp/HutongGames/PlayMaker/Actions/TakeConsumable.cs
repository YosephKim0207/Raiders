using System;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000CC7 RID: 3271
	[ActionCategory(".NPCs")]
	[Tooltip("Takes a consumable from the player (heart, key, currency, etc.).")]
	public class TakeConsumable : FsmStateAction
	{
		// Token: 0x06004584 RID: 17796 RVA: 0x00168820 File Offset: 0x00166A20
		public override void Reset()
		{
			this.consumableType = BravePlayMakerUtility.ConsumableType.Currency;
			this.amount = 0f;
			this.failure = null;
		}

		// Token: 0x06004585 RID: 17797 RVA: 0x00168840 File Offset: 0x00166A40
		public override string ErrorCheck()
		{
			string text = string.Empty;
			if (!this.amount.UsesVariable && this.amount.Value <= 0f)
			{
				text += "Need to take at least some number of consumable.\n";
			}
			return text;
		}

		// Token: 0x06004586 RID: 17798 RVA: 0x00168888 File Offset: 0x00166A88
		public override void OnEnter()
		{
			TalkDoerLite component = base.Owner.GetComponent<TalkDoerLite>();
			PlayerController talkingPlayer = component.TalkingPlayer;
			float consumableValue = BravePlayMakerUtility.GetConsumableValue(talkingPlayer, this.consumableType);
			if (consumableValue >= this.amount.Value)
			{
				BravePlayMakerUtility.SetConsumableValue(talkingPlayer, this.consumableType, consumableValue - this.amount.Value);
				base.Fsm.Event(this.success);
			}
			else
			{
				base.Fsm.Event(this.failure);
			}
			base.Finish();
		}

		// Token: 0x040037BE RID: 14270
		[Tooltip("Type of consumable to take.")]
		public BravePlayMakerUtility.ConsumableType consumableType;

		// Token: 0x040037BF RID: 14271
		[Tooltip("Amount of the consumable to take.")]
		public FsmFloat amount;

		// Token: 0x040037C0 RID: 14272
		[Tooltip("The event to send if the player pays.")]
		public FsmEvent success;

		// Token: 0x040037C1 RID: 14273
		[Tooltip("The event to send if the player does not have enough of the consumable.")]
		public FsmEvent failure;
	}
}
