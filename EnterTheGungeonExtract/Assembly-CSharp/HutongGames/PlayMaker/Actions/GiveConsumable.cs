using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000C9F RID: 3231
	[ActionCategory(".NPCs")]
	[Tooltip("Gives a consumable to the player (heart, key, currency, etc.).")]
	public class GiveConsumable : FsmStateAction
	{
		// Token: 0x06004519 RID: 17689 RVA: 0x0016649C File Offset: 0x0016469C
		public override void Reset()
		{
			this.consumableType = BravePlayMakerUtility.ConsumableType.Currency;
			this.amount = 0f;
		}

		// Token: 0x0600451A RID: 17690 RVA: 0x001664B8 File Offset: 0x001646B8
		public override string ErrorCheck()
		{
			string text = string.Empty;
			if (!this.amount.UsesVariable && this.amount.Value <= 0f)
			{
				text += "Need to give at least some number of consumable.\n";
			}
			return text;
		}

		// Token: 0x0600451B RID: 17691 RVA: 0x00166500 File Offset: 0x00164700
		public override void OnEnter()
		{
			TalkDoerLite component = base.Owner.GetComponent<TalkDoerLite>();
			PlayerController talkingPlayer = component.TalkingPlayer;
			float consumableValue = BravePlayMakerUtility.GetConsumableValue(talkingPlayer, this.consumableType);
			BravePlayMakerUtility.SetConsumableValue(talkingPlayer, this.consumableType, consumableValue + this.amount.Value);
			if (this.consumableType == BravePlayMakerUtility.ConsumableType.Hearts && this.amount.Value > 0f)
			{
				GameObject gameObject = BraveResources.Load<GameObject>("Global VFX/VFX_Healing_Sparkles_001", ".prefab");
				if (gameObject != null)
				{
					talkingPlayer.PlayEffectOnActor(gameObject, Vector3.zero, true, false, false);
				}
			}
			base.Finish();
		}

		// Token: 0x04003745 RID: 14149
		[Tooltip("Type of consumable to give.")]
		public BravePlayMakerUtility.ConsumableType consumableType;

		// Token: 0x04003746 RID: 14150
		[Tooltip("Amount of the consumable to give.")]
		public FsmFloat amount;
	}
}
