using System;
using System.Collections.Generic;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000CB1 RID: 3249
	[Tooltip("Spawns the Artful Dodger gun in the world or gives it directly to the player.")]
	[ActionCategory(".NPCs")]
	public class SpawnArtfulDodgerGun : FsmStateAction
	{
		// Token: 0x06004555 RID: 17749 RVA: 0x00167784 File Offset: 0x00165984
		public override void Reset()
		{
			this.mode = SpawnArtfulDodgerGun.Mode.SpecifyPickup;
			this.pickupId = -1;
			this.numberOfBouncesAllowed = 3;
			this.numberOfShotsAllowed = 3;
			this.lootTable = null;
			this.spawnOffset = Vector2.zero;
		}

		// Token: 0x06004556 RID: 17750 RVA: 0x001677C4 File Offset: 0x001659C4
		public override string ErrorCheck()
		{
			string text = string.Empty;
			if (this.mode == SpawnArtfulDodgerGun.Mode.SpecifyPickup && PickupObjectDatabase.GetById(this.pickupId.Value) == null)
			{
				text += "Invalid item ID.\n";
			}
			if (this.mode == SpawnArtfulDodgerGun.Mode.LootTable && !this.lootTable)
			{
				text += "Invalid loot table.\n";
			}
			return text;
		}

		// Token: 0x06004557 RID: 17751 RVA: 0x00167834 File Offset: 0x00165A34
		public override void OnEnter()
		{
			TalkDoerLite component = base.Owner.GetComponent<TalkDoerLite>();
			GameObject gameObject = null;
			if (this.mode == SpawnArtfulDodgerGun.Mode.SpecifyPickup)
			{
				gameObject = PickupObjectDatabase.GetById(this.pickupId.Value).gameObject;
			}
			else if (this.mode == SpawnArtfulDodgerGun.Mode.LootTable)
			{
				gameObject = this.lootTable.SelectByWeightWithoutDuplicatesFullPrereqs(null, true, false);
			}
			else
			{
				Debug.LogError("Tried to give an item to the player but no valid mode was selected.");
			}
			PlayerController playerController = ((!component.TalkingPlayer) ? GameManager.Instance.PrimaryPlayer : component.TalkingPlayer);
			Gun gun = null;
			if (playerController.CurrentGun)
			{
				MimicGunController component2 = playerController.CurrentGun.GetComponent<MimicGunController>();
				if (component2)
				{
					component2.ForceClearMimic(true);
				}
			}
			if (LootEngine.TryGivePrefabToPlayer(gameObject, playerController, true))
			{
				gun = playerController.GetComponentInChildren<ArtfulDodgerGunController>().GetComponent<Gun>();
			}
			List<ArtfulDodgerRoomController> componentsAbsoluteInRoom = component.GetAbsoluteParentRoom().GetComponentsAbsoluteInRoom<ArtfulDodgerRoomController>();
			ArtfulDodgerRoomController artfulDodgerRoomController = ((componentsAbsoluteInRoom == null || componentsAbsoluteInRoom.Count <= 0) ? null : componentsAbsoluteInRoom[0]);
			gun.CurrentAmmo = ((!(artfulDodgerRoomController == null)) ? Mathf.RoundToInt(artfulDodgerRoomController.NumberShots) : this.numberOfShotsAllowed.Value);
			PostShootProjectileModifier postShootProjectileModifier = gun.gameObject.AddComponent<PostShootProjectileModifier>();
			postShootProjectileModifier.NumberBouncesToSet = ((!(artfulDodgerRoomController == null)) ? Mathf.RoundToInt(artfulDodgerRoomController.NumberBounces) : this.numberOfBouncesAllowed.Value);
			artfulDodgerRoomController.Activate(base.Fsm);
			base.Finish();
		}

		// Token: 0x04003771 RID: 14193
		public SpawnArtfulDodgerGun.Mode mode;

		// Token: 0x04003772 RID: 14194
		[Tooltip("Item to spawn.")]
		public FsmInt pickupId;

		// Token: 0x04003773 RID: 14195
		public FsmInt numberOfBouncesAllowed = 3;

		// Token: 0x04003774 RID: 14196
		public FsmInt numberOfShotsAllowed = 3;

		// Token: 0x04003775 RID: 14197
		[Tooltip("Loot table to choose an item from.")]
		public GenericLootTable lootTable;

		// Token: 0x04003776 RID: 14198
		[Tooltip("Offset from the TalkDoer to spawn the item at.")]
		public Vector2 spawnOffset;

		// Token: 0x02000CB2 RID: 3250
		public enum Mode
		{
			// Token: 0x04003778 RID: 14200
			SpecifyPickup,
			// Token: 0x04003779 RID: 14201
			LootTable
		}

		// Token: 0x02000CB3 RID: 3251
		public enum SpawnLocation
		{
			// Token: 0x0400377B RID: 14203
			GiveToPlayer,
			// Token: 0x0400377C RID: 14204
			AtPlayer,
			// Token: 0x0400377D RID: 14205
			AtTalkDoer,
			// Token: 0x0400377E RID: 14206
			OffsetFromPlayer,
			// Token: 0x0400377F RID: 14207
			OffsetFromTalkDoer
		}
	}
}
