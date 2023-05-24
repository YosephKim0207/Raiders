using System;
using System.Linq;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000CD4 RID: 3284
	[Tooltip("Checks whether or not the player has a specific pickup (gun or item).")]
	[ActionCategory(".NPCs")]
	public class TestPickup : FsmStateAction
	{
		// Token: 0x060045BC RID: 17852 RVA: 0x00169E98 File Offset: 0x00168098
		public override void Reset()
		{
			this.pickupId = -1;
			this.success = null;
			this.failure = null;
		}

		// Token: 0x060045BD RID: 17853 RVA: 0x00169EB4 File Offset: 0x001680B4
		public override string ErrorCheck()
		{
			string text = string.Empty;
			if (PickupObjectDatabase.GetById(this.pickupId.Value) == null)
			{
				text += "Invalid item ID.\n";
			}
			return text;
		}

		// Token: 0x060045BE RID: 17854 RVA: 0x00169EF0 File Offset: 0x001680F0
		public override void OnEnter()
		{
			TalkDoerLite component = base.Owner.GetComponent<TalkDoerLite>();
			if (component.TalkingPlayer.inventory.AllGuns.Any((Gun g) => g.PickupObjectId == this.pickupId.Value))
			{
				base.Fsm.Event(this.success);
			}
			else if (component.TalkingPlayer.activeItems.Any((PlayerItem a) => a.PickupObjectId == this.pickupId.Value))
			{
				base.Fsm.Event(this.success);
			}
			else if (component.TalkingPlayer.passiveItems.Any((PassiveItem p) => p.PickupObjectId == this.pickupId.Value))
			{
				base.Fsm.Event(this.success);
			}
			else if (component.TalkingPlayer.additionalItems.Any((PickupObject p) => p.PickupObjectId == this.pickupId.Value))
			{
				base.Fsm.Event(this.success);
			}
			else
			{
				base.Fsm.Event(this.failure);
			}
			base.Finish();
		}

		// Token: 0x04003804 RID: 14340
		[Tooltip("Item to check.")]
		public FsmInt pickupId;

		// Token: 0x04003805 RID: 14341
		[Tooltip("The event to send if the player has the pickup.")]
		public FsmEvent success;

		// Token: 0x04003806 RID: 14342
		[Tooltip("The event to send if the player does not have the pickup.")]
		public FsmEvent failure;
	}
}
