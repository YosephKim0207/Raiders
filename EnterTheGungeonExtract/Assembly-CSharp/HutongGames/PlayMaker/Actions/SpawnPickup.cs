using System;
using Dungeonator;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000CBC RID: 3260
	[Tooltip("Spawns a pickup (gun or item) in the world or gives it directly to the player.")]
	[ActionCategory(".NPCs")]
	public class SpawnPickup : FsmStateAction
	{
		// Token: 0x06004565 RID: 17765 RVA: 0x00167D50 File Offset: 0x00165F50
		public override void Reset()
		{
			this.mode = SpawnPickup.Mode.SpecifyPickup;
			this.pickupId = -1;
			this.lootTable = null;
			this.spawnLocation = SpawnPickup.SpawnLocation.GiveToPlayer;
			this.spawnOffset = Vector2.zero;
		}

		// Token: 0x06004566 RID: 17766 RVA: 0x00167D80 File Offset: 0x00165F80
		public override string ErrorCheck()
		{
			string text = string.Empty;
			if (this.mode == SpawnPickup.Mode.SpecifyPickup && PickupObjectDatabase.GetById(this.pickupId.Value) == null)
			{
				text += "Invalid item ID.\n";
			}
			if (this.mode == SpawnPickup.Mode.LootTable && !this.lootTable)
			{
				text += "Invalid loot table.\n";
			}
			return text;
		}

		// Token: 0x06004567 RID: 17767 RVA: 0x00167DF0 File Offset: 0x00165FF0
		public override void OnEnter()
		{
			TalkDoerLite component = base.Owner.GetComponent<TalkDoerLite>();
			PlayerController playerController = ((!component.TalkingPlayer) ? GameManager.Instance.PrimaryPlayer : component.TalkingPlayer);
			GameObject gameObject = null;
			bool flag = false;
			if (this.mode == SpawnPickup.Mode.SpecifyPickup)
			{
				gameObject = PickupObjectDatabase.GetById(this.pickupId.Value).gameObject;
			}
			else if (this.mode == SpawnPickup.Mode.LootTable)
			{
				gameObject = this.lootTable.SelectByWeightWithoutDuplicatesFullPrereqs(null, true, false);
				flag = true;
			}
			else if (this.mode == SpawnPickup.Mode.DaveStyleFloorReward)
			{
				gameObject = GameManager.Instance.RewardManager.GetRewardObjectDaveStyle(playerController);
				flag = true;
			}
			else
			{
				Debug.LogError("Tried to give an item to the player but no valid mode was selected.");
			}
			if (flag && GameStatsManager.Instance.IsRainbowRun)
			{
				Vector2 vector = GameManager.Instance.PrimaryPlayer.CenterPosition + new Vector2(-0.5f, -0.5f);
				LootEngine.SpawnBowlerNote(GameManager.Instance.RewardManager.BowlerNoteOtherSource, vector, playerController.CurrentRoom, true);
			}
			else if (this.spawnLocation == SpawnPickup.SpawnLocation.GiveToPlayer)
			{
				LootEngine.TryGivePrefabToPlayer(gameObject, playerController, false);
			}
			else if (this.spawnLocation == SpawnPickup.SpawnLocation.GiveToBothPlayers)
			{
				LootEngine.TryGivePrefabToPlayer(gameObject, GameManager.Instance.PrimaryPlayer, false);
				if (GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER)
				{
					LootEngine.TryGivePrefabToPlayer(gameObject, GameManager.Instance.SecondaryPlayer, false);
				}
			}
			else
			{
				Vector2 vector2;
				if (this.spawnLocation == SpawnPickup.SpawnLocation.AtPlayer || this.spawnLocation == SpawnPickup.SpawnLocation.OffsetFromPlayer)
				{
					vector2 = playerController.specRigidbody.UnitCenter;
				}
				else if (this.spawnLocation == SpawnPickup.SpawnLocation.AtTalkDoer || this.spawnLocation == SpawnPickup.SpawnLocation.OffsetFromTalkDoer)
				{
					vector2 = ((!(component.specRigidbody != null)) ? component.sprite.WorldCenter : component.specRigidbody.UnitCenter);
				}
				else if (this.spawnLocation == SpawnPickup.SpawnLocation.RoomSpawnPoint)
				{
					vector2 = playerController.CurrentRoom.GetBestRewardLocation(IntVector2.One, RoomHandler.RewardLocationStyle.Original, false).ToVector2();
				}
				else
				{
					Debug.LogError("Tried to give an item to the player but no valid spawn location was selected.");
					vector2 = GameManager.Instance.PrimaryPlayer.CenterPosition;
				}
				if (this.spawnLocation == SpawnPickup.SpawnLocation.OffsetFromPlayer || this.spawnLocation == SpawnPickup.SpawnLocation.OffsetFromTalkDoer)
				{
					vector2 += this.spawnOffset;
				}
				LootEngine.SpawnItem(gameObject, vector2, Vector2.zero, 0f, true, false, false);
				LootEngine.DoDefaultItemPoof(vector2, false, false);
			}
			base.Finish();
		}

		// Token: 0x04003799 RID: 14233
		public SpawnPickup.Mode mode;

		// Token: 0x0400379A RID: 14234
		[Tooltip("Item to spawn.")]
		public FsmInt pickupId;

		// Token: 0x0400379B RID: 14235
		[Tooltip("Loot table to choose an item from.")]
		public GenericLootTable lootTable;

		// Token: 0x0400379C RID: 14236
		[Tooltip("Where to spawn the item at.")]
		public SpawnPickup.SpawnLocation spawnLocation;

		// Token: 0x0400379D RID: 14237
		[Tooltip("Offset from the TalkDoer to spawn the item at.")]
		public Vector2 spawnOffset;

		// Token: 0x02000CBD RID: 3261
		public enum Mode
		{
			// Token: 0x0400379F RID: 14239
			SpecifyPickup,
			// Token: 0x040037A0 RID: 14240
			LootTable,
			// Token: 0x040037A1 RID: 14241
			DaveStyleFloorReward
		}

		// Token: 0x02000CBE RID: 3262
		public enum SpawnLocation
		{
			// Token: 0x040037A3 RID: 14243
			GiveToPlayer,
			// Token: 0x040037A4 RID: 14244
			AtPlayer,
			// Token: 0x040037A5 RID: 14245
			AtTalkDoer,
			// Token: 0x040037A6 RID: 14246
			OffsetFromPlayer,
			// Token: 0x040037A7 RID: 14247
			OffsetFromTalkDoer,
			// Token: 0x040037A8 RID: 14248
			RoomSpawnPoint,
			// Token: 0x040037A9 RID: 14249
			GiveToBothPlayers
		}
	}
}
