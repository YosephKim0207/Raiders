using System;
using System.Linq;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000CC9 RID: 3273
	public class TakeSherpaPickup : FsmStateAction
	{
		// Token: 0x06004590 RID: 17808 RVA: 0x00168AD8 File Offset: 0x00166CD8
		protected bool TakeAwayPickup(PlayerController player, int pickupId)
		{
			if (player.inventory.AllGuns.Any((Gun g) => g.PickupObjectId == pickupId))
			{
				Gun gun = player.inventory.AllGuns.Find((Gun g) => g.PickupObjectId == pickupId);
				player.inventory.RemoveGunFromInventory(gun);
				UnityEngine.Object.Destroy(gun.gameObject);
			}
			else if (player.activeItems.Any((PlayerItem a) => a.PickupObjectId == pickupId))
			{
				player.RemoveActiveItem(pickupId);
			}
			else
			{
				if (!player.passiveItems.Any((PassiveItem p) => p.PickupObjectId == pickupId))
				{
					return false;
				}
				if (this.numToTake.Value > 1)
				{
					for (int i = 0; i < this.numToTake.Value; i++)
					{
						player.RemovePassiveItem(pickupId);
					}
				}
				else
				{
					player.RemovePassiveItem(pickupId);
				}
			}
			return true;
		}

		// Token: 0x06004591 RID: 17809 RVA: 0x00168BE8 File Offset: 0x00166DE8
		public override void OnEnter()
		{
			TalkDoerLite component = base.Owner.GetComponent<TalkDoerLite>();
			PlayerController talkingPlayer = component.TalkingPlayer;
			if (this.parentAction == null)
			{
				for (int i = 0; i < base.Fsm.PreviousActiveState.Actions.Length; i++)
				{
					if (base.Fsm.PreviousActiveState.Actions[i] is SherpaDetectItem)
					{
						this.parentAction = base.Fsm.PreviousActiveState.Actions[i] as SherpaDetectItem;
						break;
					}
				}
			}
			PrepareTakeSherpaPickup prepareTakeSherpaPickup = null;
			for (int j = 0; j < base.Fsm.ActiveState.Actions.Length; j++)
			{
				if (base.Fsm.ActiveState.Actions[j] is PrepareTakeSherpaPickup)
				{
					prepareTakeSherpaPickup = base.Fsm.ActiveState.Actions[j] as PrepareTakeSherpaPickup;
					break;
				}
			}
			PickupObject pickupObject = this.parentAction.AllValidTargets[prepareTakeSherpaPickup.CurrentPickupTargetIndex];
			if (!this.TakeAwayPickup(talkingPlayer, pickupObject.PickupObjectId))
			{
				base.Fsm.Event(this.failure);
			}
			else
			{
				this.parentAction = null;
			}
			base.Finish();
		}

		// Token: 0x040037C4 RID: 14276
		[Tooltip("The event to send if the player does not have the pickup.")]
		public FsmEvent failure;

		// Token: 0x040037C5 RID: 14277
		public FsmInt numToTake = 1;

		// Token: 0x040037C6 RID: 14278
		[NonSerialized]
		public SherpaDetectItem parentAction;
	}
}
