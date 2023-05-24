using System;
using System.Collections.Generic;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000C71 RID: 3185
	[Tooltip("Completes a synergy. Requires SynergraceTestCompletionPossible.")]
	[ActionCategory(".Brave")]
	public class SynergraceGiveSelectedPickup : BraveFsmStateAction
	{
		// Token: 0x06004471 RID: 17521 RVA: 0x00161DF8 File Offset: 0x0015FFF8
		public override void OnEnter()
		{
			base.OnEnter();
			TalkDoerLite component = base.Owner.GetComponent<TalkDoerLite>();
			SynergraceTestCompletionPossible synergraceTestCompletionPossible = base.FindActionOfType<SynergraceTestCompletionPossible>();
			if (component && component.TalkingPlayer && synergraceTestCompletionPossible != null && synergraceTestCompletionPossible.SelectedPickupGameObject)
			{
				Chest chest = Chest.Spawn(GameManager.Instance.RewardManager.Synergy_Chest, component.transform.position.IntXY(VectorConversions.Floor) + new IntVector2(1, -5));
				if (chest)
				{
					chest.IsLocked = false;
					PickupObject component2 = synergraceTestCompletionPossible.SelectedPickupGameObject.GetComponent<PickupObject>();
					if (component2)
					{
						chest.forceContentIds = new List<int>();
						chest.forceContentIds.Add(component2.PickupObjectId);
					}
				}
				else
				{
					LootEngine.TryGivePrefabToPlayer(synergraceTestCompletionPossible.SelectedPickupGameObject, component.TalkingPlayer, false);
				}
				synergraceTestCompletionPossible.SelectedPickupGameObject = null;
				component.TalkingPlayer.HandleItemPurchased(null);
			}
			base.Finish();
		}
	}
}
