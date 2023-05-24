using System;
using System.Linq;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000CC8 RID: 3272
	[Tooltip("Takes a pickup from the player (gun or item).")]
	[ActionCategory(".NPCs")]
	public class TakePickup : FsmStateAction
	{
		// Token: 0x06004588 RID: 17800 RVA: 0x00168914 File Offset: 0x00166B14
		public override void Reset()
		{
			this.pickupId = -1;
			this.failure = null;
		}

		// Token: 0x06004589 RID: 17801 RVA: 0x0016892C File Offset: 0x00166B2C
		public override string ErrorCheck()
		{
			string text = string.Empty;
			if (PickupObjectDatabase.GetById(this.pickupId.Value) == null)
			{
				text += "Invalid item ID.\n";
			}
			return text;
		}

		// Token: 0x0600458A RID: 17802 RVA: 0x00168968 File Offset: 0x00166B68
		public override void OnEnter()
		{
			TalkDoerLite component = base.Owner.GetComponent<TalkDoerLite>();
			PlayerController talkingPlayer = component.TalkingPlayer;
			if (talkingPlayer.inventory.AllGuns.Any((Gun g) => g.PickupObjectId == this.pickupId.Value))
			{
				Gun gun = component.TalkingPlayer.inventory.AllGuns.Find((Gun g) => g.PickupObjectId == this.pickupId.Value);
				talkingPlayer.inventory.RemoveGunFromInventory(gun);
				UnityEngine.Object.Destroy(gun.gameObject);
			}
			else if (talkingPlayer.activeItems.Any((PlayerItem a) => a.PickupObjectId == this.pickupId.Value))
			{
				talkingPlayer.RemoveActiveItem(this.pickupId.Value);
			}
			else if (talkingPlayer.passiveItems.Any((PassiveItem p) => p.PickupObjectId == this.pickupId.Value))
			{
				talkingPlayer.RemovePassiveItem(this.pickupId.Value);
			}
			else
			{
				base.Fsm.Event(this.failure);
			}
			base.Finish();
		}

		// Token: 0x040037C2 RID: 14274
		[Tooltip("Item to take.")]
		public FsmInt pickupId;

		// Token: 0x040037C3 RID: 14275
		[Tooltip("The event to send if the player does not have the pickup.")]
		public FsmEvent failure;
	}
}
