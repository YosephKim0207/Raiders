using System;
using System.Collections.Generic;
using Dungeonator;
using UnityEngine;

// Token: 0x0200148D RID: 5261
public class RewardManager : ScriptableObject
{
	// Token: 0x170011C2 RID: 4546
	// (get) Token: 0x06007793 RID: 30611 RVA: 0x002FAA84 File Offset: 0x002F8C84
	public PickupObject FullHeartPrefab
	{
		get
		{
			return PickupObjectDatabase.GetById(this.FullHeartIdPrefab);
		}
	}

	// Token: 0x170011C3 RID: 4547
	// (get) Token: 0x06007794 RID: 30612 RVA: 0x002FAA94 File Offset: 0x002F8C94
	public PickupObject HalfHeartPrefab
	{
		get
		{
			return PickupObjectDatabase.GetById(this.HalfHeartIdPrefab);
		}
	}

	// Token: 0x170011C4 RID: 4548
	// (get) Token: 0x06007795 RID: 30613 RVA: 0x002FAAA4 File Offset: 0x002F8CA4
	public FloorRewardData CurrentRewardData
	{
		get
		{
			return this.GetRewardDataForFloor(GameManager.Instance.BestGenerationDungeonPrefab.tileIndices.tilesetId);
		}
	}

	// Token: 0x06007796 RID: 30614 RVA: 0x002FAAC0 File Offset: 0x002F8CC0
	private FloorRewardData GetRewardDataForFloor(GlobalDungeonData.ValidTilesets targetTileset)
	{
		FloorRewardData floorRewardData = null;
		for (int i = 0; i < this.FloorRewardData.Count; i++)
		{
			if ((this.FloorRewardData[i].AssociatedTilesets | targetTileset) == this.FloorRewardData[i].AssociatedTilesets)
			{
				floorRewardData = this.FloorRewardData[i];
			}
		}
		if (floorRewardData == null)
		{
			floorRewardData = this.FloorRewardData[0];
		}
		return floorRewardData;
	}

	// Token: 0x06007797 RID: 30615 RVA: 0x002FAB38 File Offset: 0x002F8D38
	public GameObject GetShopItemResourcefulRatStyle(List<GameObject> excludedObjects = null, System.Random safeRandom = null)
	{
		PickupObject.ItemQuality itemQuality = PickupObject.ItemQuality.D;
		GlobalDungeonData.ValidTilesets tilesetId = GameManager.Instance.Dungeon.tileIndices.tilesetId;
		if (tilesetId != GlobalDungeonData.ValidTilesets.GUNGEON)
		{
			if (tilesetId != GlobalDungeonData.ValidTilesets.MINEGEON)
			{
				if (tilesetId != GlobalDungeonData.ValidTilesets.CATACOMBGEON)
				{
					if (tilesetId == GlobalDungeonData.ValidTilesets.FORGEGEON)
					{
						itemQuality = PickupObject.ItemQuality.B;
					}
				}
				else
				{
					itemQuality = PickupObject.ItemQuality.B;
				}
			}
			else
			{
				itemQuality = PickupObject.ItemQuality.C;
			}
		}
		else
		{
			itemQuality = PickupObject.ItemQuality.D;
		}
		return this.GetRawItem(this.GunsLootTable, itemQuality, excludedObjects, true, safeRandom);
	}

	// Token: 0x06007798 RID: 30616 RVA: 0x002FABAC File Offset: 0x002F8DAC
	public GameObject GetRewardObjectShopStyle(PlayerController player, bool forceGun = false, bool forceItem = false, List<GameObject> excludedObjects = null)
	{
		FloorRewardData currentRewardData = this.CurrentRewardData;
		bool flag = ((!GameManager.Instance.IsSeeded) ? UnityEngine.Random.value : BraveRandom.GenerationRandomValue()) > 0.5f;
		if (forceGun)
		{
			flag = true;
		}
		if (forceItem)
		{
			flag = false;
		}
		PickupObject.ItemQuality shopTargetQuality = currentRewardData.GetShopTargetQuality(GameManager.Instance.IsSeeded);
		System.Random random = null;
		if (GameManager.Instance.IsSeeded)
		{
			random = BraveRandom.GeneratorRandom;
		}
		GenericLootTable genericLootTable;
		PickupObject.ItemQuality itemQuality;
		System.Random random2;
		if (flag)
		{
			List<GameObject> list = new List<GameObject>();
			this.ExcludeUnfinishedGunIfNecessary(list);
			genericLootTable = this.GunsLootTable;
			itemQuality = shopTargetQuality;
			random2 = random;
			return this.GetItemForPlayer(player, genericLootTable, itemQuality, excludedObjects, false, random2, false, list, false, RewardManager.RewardSource.UNSPECIFIED);
		}
		List<GameObject> list2 = new List<GameObject>();
		this.BuildExcludedShopList(list2);
		genericLootTable = this.ItemsLootTable;
		itemQuality = shopTargetQuality;
		random2 = random;
		return this.GetItemForPlayer(player, genericLootTable, itemQuality, excludedObjects, false, random2, false, list2, false, RewardManager.RewardSource.UNSPECIFIED);
	}

	// Token: 0x06007799 RID: 30617 RVA: 0x002FAC9C File Offset: 0x002F8E9C
	private void ExcludeUnfinishedGunIfNecessary(List<GameObject> excluded)
	{
		for (int i = 0; i < this.GunsLootTable.defaultItemDrops.elements.Count; i++)
		{
			WeightedGameObject weightedGameObject = this.GunsLootTable.defaultItemDrops.elements[i];
			if (weightedGameObject.gameObject)
			{
				PickupObject component = weightedGameObject.gameObject.GetComponent<PickupObject>();
				if (component && component.PickupObjectId == GlobalItemIds.UnfinishedGun && GameStatsManager.HasInstance && GameStatsManager.Instance.GetFlag(GungeonFlags.ITEMSPECIFIC_AMMONOMICON_COMPLETE))
				{
					excluded.Add(weightedGameObject.gameObject);
				}
			}
		}
	}

	// Token: 0x0600779A RID: 30618 RVA: 0x002FAD48 File Offset: 0x002F8F48
	private void BuildExcludedShopList(List<GameObject> excluded)
	{
		for (int i = 0; i < this.ItemsLootTable.defaultItemDrops.elements.Count; i++)
		{
			WeightedGameObject weightedGameObject = this.ItemsLootTable.defaultItemDrops.elements[i];
			if (weightedGameObject.gameObject)
			{
				PickupObject component = weightedGameObject.gameObject.GetComponent<PickupObject>();
				if (component && component.ShouldBeExcludedFromShops)
				{
					excluded.Add(weightedGameObject.gameObject);
				}
				else if (component && component.PickupObjectId == GlobalItemIds.UnfinishedGun && GameStatsManager.HasInstance && GameStatsManager.Instance.GetFlag(GungeonFlags.ITEMSPECIFIC_AMMONOMICON_COMPLETE))
				{
					excluded.Add(weightedGameObject.gameObject);
				}
			}
		}
	}

	// Token: 0x0600779B RID: 30619 RVA: 0x002FAE1C File Offset: 0x002F901C
	public bool IsBossRewardForcedGun()
	{
		if (GameManager.Instance.CurrentGameMode != GameManager.GameMode.BOSSRUSH)
		{
			bool flag = true;
			for (int i = 0; i < GameManager.Instance.AllPlayers.Length; i++)
			{
				if (GameManager.Instance.AllPlayers[i] && (GameManager.Instance.AllPlayers[i].HasReceivedNewGunThisFloor || GameManager.Instance.AllPlayers[i].CharacterUsesRandomGuns))
				{
					flag = false;
				}
			}
			if (flag)
			{
				Debug.LogWarning("Forcing boss drop GUN!");
				return true;
			}
		}
		return false;
	}

	// Token: 0x170011C5 RID: 4549
	// (get) Token: 0x0600779C RID: 30620 RVA: 0x002FAEB0 File Offset: 0x002F90B0
	private float ItemVsGunChanceBossReward
	{
		get
		{
			return 0.625f;
		}
	}

	// Token: 0x0600779D RID: 30621 RVA: 0x002FAEB8 File Offset: 0x002F90B8
	public GameObject GetRewardObjectForBossSeeded(List<PickupObject> AlreadyGenerated, bool forceGun)
	{
		FloorRewardData currentRewardData = this.CurrentRewardData;
		bool flag = forceGun || BraveRandom.GenerationRandomValue() > this.ItemVsGunChanceBossReward;
		if (flag)
		{
			PickupObject.ItemQuality randomBossTargetQuality = currentRewardData.GetRandomBossTargetQuality(BraveRandom.GeneratorRandom);
			return this.GetItemForSeededRun(this.GunsLootTable, randomBossTargetQuality, AlreadyGenerated, BraveRandom.GeneratorRandom, true);
		}
		return this.GetItemForSeededRun((!flag) ? this.ItemsLootTable : this.GunsLootTable, this.GetDaveStyleItemQuality(), AlreadyGenerated, BraveRandom.GeneratorRandom, true);
	}

	// Token: 0x0600779E RID: 30622 RVA: 0x002FAF34 File Offset: 0x002F9134
	public GameObject GetRewardObjectBossStyle(PlayerController player)
	{
		FloorRewardData currentRewardData = this.CurrentRewardData;
		bool flag;
		if (GameManager.Instance.BestGenerationDungeonPrefab.tileIndices.tilesetId == GlobalDungeonData.ValidTilesets.CASTLEGEON && GameManager.Instance.CurrentGameType == GameManager.GameType.SINGLE_PLAYER && player && player.inventory != null && player.inventory.GunCountModified <= 3)
		{
			flag = UnityEngine.Random.value > 0.2f;
		}
		else if (GameManager.Instance.CurrentGameType == GameManager.GameType.SINGLE_PLAYER && player && player.inventory != null && player.inventory.GunCountModified <= 2)
		{
			flag = UnityEngine.Random.value > 0.3f;
		}
		else
		{
			flag = UnityEngine.Random.value > this.ItemVsGunChanceBossReward;
		}
		if (this.IsBossRewardForcedGun())
		{
			flag = true;
		}
		if ((GameManager.Instance.CurrentGameMode == GameManager.GameMode.BOSSRUSH || GameManager.Instance.CurrentGameMode == GameManager.GameMode.SUPERBOSSRUSH) && !GameManager.Instance.Dungeon.HasGivenBossrushGun)
		{
			GameManager.Instance.Dungeon.HasGivenBossrushGun = true;
			flag = true;
		}
		if (flag)
		{
			PickupObject.ItemQuality randomBossTargetQuality = currentRewardData.GetRandomBossTargetQuality(null);
			return this.GetItemForPlayer(player, this.GunsLootTable, randomBossTargetQuality, null, false, null, false, null, false, RewardManager.RewardSource.BOSS_PEDESTAL);
		}
		return this.GetRewardItemDaveStyle(player, true);
	}

	// Token: 0x0600779F RID: 30623 RVA: 0x002FB084 File Offset: 0x002F9284
	private PickupObject.ItemQuality GetDaveStyleItemQuality()
	{
		float num = 0.1f;
		float num2 = 0.4f;
		float num3 = 0.7f;
		float num4 = 0.95f;
		float value = UnityEngine.Random.value;
		PickupObject.ItemQuality itemQuality = PickupObject.ItemQuality.D;
		if (value > num && value <= num2)
		{
			itemQuality = PickupObject.ItemQuality.C;
		}
		else if (value > num2 && value <= num3)
		{
			itemQuality = PickupObject.ItemQuality.B;
		}
		else if (value > num3 && value <= num4)
		{
			itemQuality = PickupObject.ItemQuality.A;
		}
		else if (value > num4)
		{
			itemQuality = PickupObject.ItemQuality.S;
		}
		return itemQuality;
	}

	// Token: 0x060077A0 RID: 30624 RVA: 0x002FB108 File Offset: 0x002F9308
	private GameObject GetRewardItemDaveStyle(PlayerController player, bool bossStyle = false)
	{
		PickupObject.ItemQuality daveStyleItemQuality = this.GetDaveStyleItemQuality();
		Debug.Log("Get Reward Item Dave Style: " + daveStyleItemQuality.ToString());
		RewardManager.RewardSource rewardSource = ((!bossStyle) ? RewardManager.RewardSource.UNSPECIFIED : RewardManager.RewardSource.BOSS_PEDESTAL);
		GenericLootTable itemsLootTable = this.ItemsLootTable;
		PickupObject.ItemQuality itemQuality = daveStyleItemQuality;
		List<GameObject> list = null;
		RewardManager.RewardSource rewardSource2 = rewardSource;
		return this.GetItemForPlayer(player, itemsLootTable, itemQuality, list, false, null, bossStyle, null, false, rewardSource2);
	}

	// Token: 0x060077A1 RID: 30625 RVA: 0x002FB170 File Offset: 0x002F9370
	public GameObject GetRewardObjectDaveStyle(PlayerController player)
	{
		FloorRewardData currentRewardData = this.CurrentRewardData;
		bool flag = UnityEngine.Random.value > 0.5f;
		if (flag)
		{
			PickupObject.ItemQuality randomTargetQuality = currentRewardData.GetRandomTargetQuality(false, false);
			return this.GetItemForPlayer(player, this.GunsLootTable, randomTargetQuality, null, false, null, false, null, false, RewardManager.RewardSource.UNSPECIFIED);
		}
		return this.GetRewardItemDaveStyle(player, false);
	}

	// Token: 0x060077A2 RID: 30626 RVA: 0x002FB1C0 File Offset: 0x002F93C0
	public static bool PlayerHasItemInSynergyContainingOtherItem(PlayerController player, PickupObject prefab)
	{
		bool flag = false;
		return RewardManager.PlayerHasItemInSynergyContainingOtherItem(player, prefab, ref flag);
	}

	// Token: 0x060077A3 RID: 30627 RVA: 0x002FB1D8 File Offset: 0x002F93D8
	public static bool TestItemWouldCompleteSpecificSynergy(AdvancedSynergyEntry entry, PickupObject newPickup)
	{
		return entry.ActivationStatus != SynergyEntry.SynergyActivation.INACTIVE && !entry.SynergyIsAvailable(GameManager.Instance.PrimaryPlayer, GameManager.Instance.SecondaryPlayer, -1) && entry.SynergyIsAvailable(GameManager.Instance.PrimaryPlayer, GameManager.Instance.SecondaryPlayer, newPickup.PickupObjectId);
	}

	// Token: 0x060077A4 RID: 30628 RVA: 0x002FB23C File Offset: 0x002F943C
	public static bool AnyPlayerHasItemInSynergyContainingOtherItem(PickupObject prefab, ref bool usesStartingItem)
	{
		for (int i = 0; i < GameManager.Instance.AllPlayers.Length; i++)
		{
			PlayerController playerController = GameManager.Instance.AllPlayers[i];
			if (playerController && RewardManager.PlayerHasItemInSynergyContainingOtherItem(playerController, prefab, ref usesStartingItem))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x060077A5 RID: 30629 RVA: 0x002FB290 File Offset: 0x002F9490
	public static bool PlayerHasItemInSynergyContainingOtherItem(PlayerController player, PickupObject prefab, ref bool usesStartingItem)
	{
		int pickupObjectId = prefab.PickupObjectId;
		foreach (AdvancedSynergyEntry advancedSynergyEntry in GameManager.Instance.SynergyManager.synergies)
		{
			if (advancedSynergyEntry.ActivationStatus != SynergyEntry.SynergyActivation.INACTIVE)
			{
				if (advancedSynergyEntry.ActivationStatus != SynergyEntry.SynergyActivation.DEMO)
				{
					if (advancedSynergyEntry.ActivationStatus != SynergyEntry.SynergyActivation.ACTIVE_UNBOOSTED)
					{
						bool flag = advancedSynergyEntry.ContainsPickup(pickupObjectId);
						if (flag)
						{
							bool flag2 = false;
							for (int j = 0; j < player.inventory.AllGuns.Count; j++)
							{
								bool flag3 = advancedSynergyEntry.ContainsPickup(player.inventory.AllGuns[j].PickupObjectId);
								if (flag3)
								{
									flag3 = RewardManager.TestItemWouldCompleteSpecificSynergy(advancedSynergyEntry, prefab);
								}
								flag2 = flag2 || flag3;
								if (flag3)
								{
									usesStartingItem |= player.startingGunIds.Contains(player.inventory.AllGuns[j].PickupObjectId);
								}
								if (flag3)
								{
									usesStartingItem |= player.startingAlternateGunIds.Contains(player.inventory.AllGuns[j].PickupObjectId);
								}
							}
							if (!flag2)
							{
								for (int k = 0; k < player.activeItems.Count; k++)
								{
									bool flag4 = advancedSynergyEntry.ContainsPickup(player.activeItems[k].PickupObjectId);
									if (flag4)
									{
										flag4 = RewardManager.TestItemWouldCompleteSpecificSynergy(advancedSynergyEntry, prefab);
									}
									flag2 = flag2 || flag4;
									if (flag4)
									{
										usesStartingItem |= player.startingActiveItemIds.Contains(player.activeItems[k].PickupObjectId);
									}
								}
							}
							if (!flag2)
							{
								for (int l = 0; l < player.passiveItems.Count; l++)
								{
									bool flag5 = advancedSynergyEntry.ContainsPickup(player.passiveItems[l].PickupObjectId);
									if (flag5)
									{
										flag5 = RewardManager.TestItemWouldCompleteSpecificSynergy(advancedSynergyEntry, prefab);
									}
									flag2 = flag2 || flag5;
									if (flag5)
									{
										usesStartingItem |= player.startingPassiveItemIds.Contains(player.passiveItems[l].PickupObjectId);
									}
								}
							}
							if (!flag2 && SynercacheManager.UseCachedSynergyIDs)
							{
								for (int m = 0; m < SynercacheManager.LastCachedSynergyIDs.Count; m++)
								{
									flag2 |= advancedSynergyEntry.ContainsPickup(SynercacheManager.LastCachedSynergyIDs[m]);
									flag2 |= advancedSynergyEntry.ContainsPickup(SynercacheManager.LastCachedSynergyIDs[m]);
								}
							}
							if (flag2)
							{
								return true;
							}
						}
					}
				}
			}
		}
		return false;
	}

	// Token: 0x060077A6 RID: 30630 RVA: 0x002FB534 File Offset: 0x002F9734
	public static bool CheckQualityForItem(PickupObject prefab, PlayerController player, PickupObject.ItemQuality targetQuality, bool completesSynergy, RewardManager.RewardSource source)
	{
		bool flag = prefab.quality == targetQuality;
		if (!player)
		{
			return flag;
		}
		bool flag2 = completesSynergy || GameManager.Instance.RewardManager.SynergyCompletionIgnoresQualities;
		if (GameStatsManager.Instance.GetNumberOfSynergiesEncounteredThisRun() == 0 && source == RewardManager.RewardSource.BOSS_PEDESTAL)
		{
			flag2 = true;
		}
		if (!flag && flag2 && RewardManager.PlayerHasItemInSynergyContainingOtherItem(player, prefab))
		{
			flag = true;
		}
		return flag;
	}

	// Token: 0x060077A7 RID: 30631 RVA: 0x002FB5A8 File Offset: 0x002F97A8
	public static bool AnyPlayerHasItem(int id)
	{
		for (int i = 0; i < GameManager.Instance.AllPlayers.Length; i++)
		{
			PlayerController playerController = GameManager.Instance.AllPlayers[i];
			if (playerController && (playerController.HasPassiveItem(id) || playerController.HasActiveItem(id) || playerController.HasGun(id)))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x060077A8 RID: 30632 RVA: 0x002FB614 File Offset: 0x002F9814
	public static float GetMultiplierForItem(PickupObject prefab, PlayerController player, bool completesSynergy)
	{
		if (!prefab)
		{
			return 1f;
		}
		float num = 1f;
		int pickupObjectId = prefab.PickupObjectId;
		if (player == null)
		{
			return num;
		}
		bool flag = false;
		float num2 = SynergyFactorConstants.GetSynergyFactor();
		if (completesSynergy)
		{
			if (RewardManager.AnyPlayerHasItem(prefab.PickupObjectId))
			{
				return 0f;
			}
			if (prefab is BasicStatPickup && (prefab as BasicStatPickup).IsMasteryToken)
			{
				return 0f;
			}
			num2 = 100000000f;
		}
		if (num2 > 1f || flag)
		{
			bool flag2 = false;
			if (RewardManager.AnyPlayerHasItemInSynergyContainingOtherItem(prefab, ref flag2))
			{
				if (completesSynergy && flag2)
				{
					num2 = 10000f;
				}
				else if (flag2)
				{
					num2 = 1f;
				}
				num *= num2;
			}
		}
		for (int i = 0; i < player.lootModData.Count; i++)
		{
			if (player.lootModData[i].AssociatedPickupId == pickupObjectId)
			{
				num *= player.lootModData[i].DropRateMultiplier;
			}
		}
		return num;
	}

	// Token: 0x060077A9 RID: 30633 RVA: 0x002FB730 File Offset: 0x002F9930
	public GameObject GetRawItem(GenericLootTable tableToUse, PickupObject.ItemQuality targetQuality, List<GameObject> excludedObjects, bool ignorePlayerTraits = false, System.Random safeRandom = null)
	{
		bool flag = false;
		while (targetQuality >= PickupObject.ItemQuality.COMMON)
		{
			if (targetQuality > PickupObject.ItemQuality.COMMON)
			{
				flag = true;
			}
			List<WeightedGameObject> compiledRawItems = tableToUse.GetCompiledRawItems();
			List<KeyValuePair<WeightedGameObject, float>> list = new List<KeyValuePair<WeightedGameObject, float>>();
			float num = 0f;
			for (int i = 0; i < compiledRawItems.Count; i++)
			{
				if (compiledRawItems[i].gameObject != null)
				{
					PickupObject component = compiledRawItems[i].gameObject.GetComponent<PickupObject>();
					if (!(component == null))
					{
						bool flag2 = component.quality == targetQuality;
						if (component != null && flag2)
						{
							bool flag3 = true;
							float weight = compiledRawItems[i].weight;
							if (excludedObjects == null || !excludedObjects.Contains(component.gameObject))
							{
								if (!component.PrerequisitesMet())
								{
									flag3 = false;
								}
								if (flag3)
								{
									num += weight;
									KeyValuePair<WeightedGameObject, float> keyValuePair = new KeyValuePair<WeightedGameObject, float>(compiledRawItems[i], weight);
									list.Add(keyValuePair);
								}
							}
						}
					}
				}
			}
			if (num > 0f && list.Count > 0)
			{
				float num3;
				if (ignorePlayerTraits)
				{
					float num2 = (float)safeRandom.NextDouble();
					num3 = num * num2;
				}
				else
				{
					num3 = num * UnityEngine.Random.value;
				}
				for (int j = 0; j < list.Count; j++)
				{
					num3 -= list[j].Value;
					if (num3 <= 0f)
					{
						return list[j].Key.gameObject;
					}
				}
				return list[list.Count - 1].Key.gameObject;
			}
			targetQuality--;
			if (targetQuality < PickupObject.ItemQuality.COMMON && !flag)
			{
				targetQuality = PickupObject.ItemQuality.D;
			}
		}
		return null;
	}

	// Token: 0x060077AA RID: 30634 RVA: 0x002FB910 File Offset: 0x002F9B10
	public GameObject GetItemForPlayer(PlayerController player, GenericLootTable tableToUse, PickupObject.ItemQuality targetQuality, List<GameObject> excludedObjects, bool ignorePlayerTraits = false, System.Random safeRandom = null, bool bossStyle = false, List<GameObject> additionalExcludedObjects = null, bool forceSynergyCompletion = false, RewardManager.RewardSource rewardSource = RewardManager.RewardSource.UNSPECIFIED)
	{
		bool flag = false;
		while (targetQuality >= PickupObject.ItemQuality.COMMON)
		{
			if (targetQuality > PickupObject.ItemQuality.COMMON)
			{
				flag = true;
			}
			List<WeightedGameObject> compiledRawItems = tableToUse.GetCompiledRawItems();
			List<KeyValuePair<WeightedGameObject, float>> list = new List<KeyValuePair<WeightedGameObject, float>>();
			float num = 0f;
			List<KeyValuePair<WeightedGameObject, float>> list2 = new List<KeyValuePair<WeightedGameObject, float>>();
			float num2 = 0f;
			for (int i = 0; i < compiledRawItems.Count; i++)
			{
				if (compiledRawItems[i].gameObject != null)
				{
					PickupObject component = compiledRawItems[i].gameObject.GetComponent<PickupObject>();
					if (!(component == null))
					{
						if (!bossStyle || !(component is GungeonMapItem))
						{
							bool flag2 = RewardManager.CheckQualityForItem(component, player, targetQuality, forceSynergyCompletion, rewardSource);
							if ((component.ItemSpansBaseQualityTiers || component.ItemRespectsHeartMagnificence) && targetQuality != PickupObject.ItemQuality.D && targetQuality != PickupObject.ItemQuality.COMMON && targetQuality != PickupObject.ItemQuality.S)
							{
								flag2 = true;
							}
							if (!ignorePlayerTraits && component is SpiceItem && player && player.spiceCount > 0)
							{
								Debug.Log("BAM spicing it up");
								flag2 = true;
							}
							if (component != null && flag2)
							{
								bool flag3 = true;
								float num3 = compiledRawItems[i].weight;
								if (excludedObjects == null || !excludedObjects.Contains(component.gameObject))
								{
									if (additionalExcludedObjects == null || !additionalExcludedObjects.Contains(component.gameObject))
									{
										if (!component.PrerequisitesMet())
										{
											flag3 = false;
										}
										if (component is Gun)
										{
											Gun gun = component as Gun;
											if (gun.InfiniteAmmo && !gun.CanBeDropped && gun.quality == PickupObject.ItemQuality.SPECIAL)
											{
												goto IL_389;
											}
											GunClass gunClass = gun.gunClass;
											if (!ignorePlayerTraits && gunClass != GunClass.NONE)
											{
												int num4 = ((!(player == null) && player.inventory != null) ? player.inventory.ContainsGunOfClass(gunClass, true) : 0);
												float modifierForClass = LootDataGlobalSettings.Instance.GetModifierForClass(gunClass);
												num3 *= Mathf.Pow(modifierForClass, (float)num4);
											}
										}
										if (!ignorePlayerTraits)
										{
											float multiplierForItem = RewardManager.GetMultiplierForItem(component, player, forceSynergyCompletion);
											num3 *= multiplierForItem;
										}
										bool flag4 = !GameManager.Instance.IsSeeded;
										EncounterTrackable component2 = component.GetComponent<EncounterTrackable>();
										if (component2 != null && flag4)
										{
											int num5 = 0;
											if (Application.isPlaying)
											{
												num5 = GameStatsManager.Instance.QueryEncounterableDifferentiator(component2);
											}
											if (num5 > 0 || (Application.isPlaying && GameManager.Instance.ExtantShopTrackableGuids.Contains(component2.EncounterGuid)))
											{
												flag3 = false;
												num2 += num3;
												KeyValuePair<WeightedGameObject, float> keyValuePair = new KeyValuePair<WeightedGameObject, float>(compiledRawItems[i], num3);
												list2.Add(keyValuePair);
											}
											else if (Application.isPlaying && GameStatsManager.Instance.QueryEncounterable(component2) == 0 && GameStatsManager.Instance.QueryEncounterableAnnouncement(component2.EncounterGuid))
											{
												num3 *= 10f;
											}
										}
										if (component.ItemSpansBaseQualityTiers || component.ItemRespectsHeartMagnificence)
										{
											if (RewardManager.AdditionalHeartTierMagnificence >= 3f)
											{
												num3 *= this.ThreeOrMoreHeartMagMultiplier;
											}
											else if (RewardManager.AdditionalHeartTierMagnificence >= 1f)
											{
												num3 *= this.OneOrTwoHeartMagMultiplier;
											}
										}
										if (flag3)
										{
											num += num3;
											KeyValuePair<WeightedGameObject, float> keyValuePair2 = new KeyValuePair<WeightedGameObject, float>(compiledRawItems[i], num3);
											list.Add(keyValuePair2);
										}
									}
								}
							}
						}
					}
				}
				IL_389:;
			}
			if (list.Count == 0 && list2.Count > 0)
			{
				list = list2;
				num = num2;
			}
			if (num > 0f && list.Count > 0)
			{
				float num7;
				if (ignorePlayerTraits)
				{
					float num6 = (float)safeRandom.NextDouble();
					Debug.LogError("safe random: " + num6);
					num7 = num * num6;
				}
				else
				{
					num7 = num * UnityEngine.Random.value;
				}
				for (int j = 0; j < list.Count; j++)
				{
					num7 -= list[j].Value;
					if (num7 <= 0f)
					{
						return list[j].Key.gameObject;
					}
				}
				return list[list.Count - 1].Key.gameObject;
			}
			targetQuality--;
			if (targetQuality < PickupObject.ItemQuality.COMMON && !flag)
			{
				targetQuality = PickupObject.ItemQuality.D;
			}
		}
		return null;
	}

	// Token: 0x060077AB RID: 30635 RVA: 0x002FBDC0 File Offset: 0x002F9FC0
	public GameObject GetItemForSeededRun(GenericLootTable tableToUse, PickupObject.ItemQuality targetQuality, List<PickupObject> AlreadyGeneratedItems, System.Random safeRandom, bool bossStyle = false)
	{
		bool flag = false;
		while (targetQuality >= PickupObject.ItemQuality.COMMON)
		{
			if (targetQuality > PickupObject.ItemQuality.COMMON)
			{
				flag = true;
			}
			List<WeightedGameObject> compiledRawItems = tableToUse.GetCompiledRawItems();
			List<KeyValuePair<WeightedGameObject, float>> list = new List<KeyValuePair<WeightedGameObject, float>>();
			float num = 0f;
			for (int i = 0; i < compiledRawItems.Count; i++)
			{
				if (compiledRawItems[i].gameObject != null)
				{
					PickupObject component = compiledRawItems[i].gameObject.GetComponent<PickupObject>();
					if (!(component == null))
					{
						if (!bossStyle || !(component is GungeonMapItem))
						{
							bool flag2 = component.quality == targetQuality;
							if ((component.ItemSpansBaseQualityTiers || component.ItemRespectsHeartMagnificence) && targetQuality != PickupObject.ItemQuality.D && targetQuality != PickupObject.ItemQuality.COMMON && targetQuality != PickupObject.ItemQuality.S)
							{
								flag2 = true;
							}
							if (component != null && flag2)
							{
								bool flag3 = true;
								float num2 = compiledRawItems[i].weight;
								if (AlreadyGeneratedItems == null || !AlreadyGeneratedItems.Contains(component))
								{
									if (!component.PrerequisitesMet())
									{
										flag3 = false;
									}
									if (component is Gun)
									{
										Gun gun = component as Gun;
										if (gun.InfiniteAmmo && !gun.CanBeDropped && gun.quality == PickupObject.ItemQuality.SPECIAL)
										{
											goto IL_26E;
										}
									}
									if (!GameManager.Instance.RewardManager.IsItemInSeededManifests(component))
									{
										float num3 = 1f;
										if (AlreadyGeneratedItems != null)
										{
											for (int j = 0; j < AlreadyGeneratedItems.Count; j++)
											{
												for (int k = 0; k < AlreadyGeneratedItems[j].associatedItemChanceMods.Length; k++)
												{
													if (AlreadyGeneratedItems[j].associatedItemChanceMods[k].AssociatedPickupId == component.PickupObjectId)
													{
														num3 *= AlreadyGeneratedItems[j].associatedItemChanceMods[k].DropRateMultiplier;
													}
												}
											}
										}
										num2 *= num3;
										if (component.ItemSpansBaseQualityTiers || component.ItemRespectsHeartMagnificence)
										{
											if (RewardManager.AdditionalHeartTierMagnificence >= 3f)
											{
												num2 *= this.ThreeOrMoreHeartMagMultiplier;
											}
											else if (RewardManager.AdditionalHeartTierMagnificence >= 1f)
											{
												num2 *= this.OneOrTwoHeartMagMultiplier;
											}
										}
										if (flag3)
										{
											num += num2;
											KeyValuePair<WeightedGameObject, float> keyValuePair = new KeyValuePair<WeightedGameObject, float>(compiledRawItems[i], num2);
											list.Add(keyValuePair);
										}
									}
								}
							}
						}
					}
				}
				IL_26E:;
			}
			if (num > 0f && list.Count > 0)
			{
				float num4 = (float)safeRandom.NextDouble();
				float num5 = num * num4;
				for (int l = 0; l < list.Count; l++)
				{
					num5 -= list[l].Value;
					if (num5 <= 0f)
					{
						return list[l].Key.gameObject;
					}
				}
				return list[list.Count - 1].Key.gameObject;
			}
			targetQuality--;
			if (targetQuality < PickupObject.ItemQuality.COMMON && !flag)
			{
				targetQuality = PickupObject.ItemQuality.D;
			}
		}
		return null;
	}

	// Token: 0x060077AC RID: 30636 RVA: 0x002FC104 File Offset: 0x002FA304
	private Chest GetTargetChestPrefab(PickupObject.ItemQuality targetQuality)
	{
		Chest chest = null;
		switch (targetQuality)
		{
		case PickupObject.ItemQuality.D:
			chest = this.D_Chest;
			break;
		case PickupObject.ItemQuality.C:
			chest = this.C_Chest;
			break;
		case PickupObject.ItemQuality.B:
			chest = this.B_Chest;
			break;
		case PickupObject.ItemQuality.A:
			chest = this.A_Chest;
			break;
		case PickupObject.ItemQuality.S:
			chest = this.S_Chest;
			break;
		}
		return chest;
	}

	// Token: 0x060077AD RID: 30637 RVA: 0x002FC174 File Offset: 0x002FA374
	private Chest SpawnInternal(IntVector2 position, float gunVersusItemPercentChance, PickupObject.ItemQuality targetQuality, Chest overrideChestPrefab = null)
	{
		Chest chest = overrideChestPrefab ?? this.GetTargetChestPrefab(targetQuality);
		GenericLootTable genericLootTable = ((UnityEngine.Random.value >= gunVersusItemPercentChance) ? this.ItemsLootTable : this.GunsLootTable);
		Chest chest2 = Chest.Spawn(chest, position);
		chest2.lootTable.lootTable = genericLootTable;
		if (chest2.lootTable.canDropMultipleItems && chest2.lootTable.overrideItemLootTables != null && chest2.lootTable.overrideItemLootTables.Count > 0)
		{
			chest2.lootTable.overrideItemLootTables[0] = genericLootTable;
		}
		return chest2;
	}

	// Token: 0x060077AE RID: 30638 RVA: 0x002FC20C File Offset: 0x002FA40C
	public Chest SpawnRoomClearChestAt(IntVector2 position)
	{
		FloorRewardData rewardDataForFloor = this.GetRewardDataForFloor(GameManager.Instance.BestGenerationDungeonPrefab.tileIndices.tilesetId);
		PickupObject.ItemQuality itemQuality = rewardDataForFloor.GetRandomRoomTargetQuality();
		int num = -1;
		if ((itemQuality == PickupObject.ItemQuality.D || itemQuality == PickupObject.ItemQuality.C) && PlayerController.AnyoneHasActiveBonusSynergy(CustomSynergyType.DOUBLE_CHEST_FRIENDS, out num))
		{
			itemQuality = rewardDataForFloor.GetRandomRoomTargetQuality();
		}
		Chest chest = null;
		if (UnityEngine.Random.value < this.RoomClearRainbowChance)
		{
			chest = this.Rainbow_Chest;
		}
		return this.SpawnInternal(position, rewardDataForFloor.GunVersusItemPercentChance, itemQuality, chest);
	}

	// Token: 0x060077AF RID: 30639 RVA: 0x002FC28C File Offset: 0x002FA48C
	public DebrisObject SpawnTotallyRandomItem(Vector2 position, PickupObject.ItemQuality startQuality = PickupObject.ItemQuality.D, PickupObject.ItemQuality endQuality = PickupObject.ItemQuality.S)
	{
		PickupObject.ItemQuality itemQuality = (PickupObject.ItemQuality)UnityEngine.Random.Range((int)startQuality, (int)(endQuality + 1));
		return LootEngine.SpawnItem(this.GetItemForPlayer(GameManager.Instance.PrimaryPlayer, (UnityEngine.Random.value >= 0.5f) ? this.ItemsLootTable : this.GunsLootTable, itemQuality, null, false, null, false, null, false, RewardManager.RewardSource.UNSPECIFIED).gameObject, position, Vector2.zero, 0f, true, false, false);
	}

	// Token: 0x060077B0 RID: 30640 RVA: 0x002FC2F8 File Offset: 0x002FA4F8
	public Chest SpawnTotallyRandomChest(IntVector2 position)
	{
		PickupObject.ItemQuality itemQuality = (PickupObject.ItemQuality)UnityEngine.Random.Range(1, 6);
		if (PassiveItem.IsFlagSetAtAll(typeof(SevenLeafCloverItem)))
		{
			itemQuality = ((UnityEngine.Random.value >= 0.5f) ? PickupObject.ItemQuality.S : PickupObject.ItemQuality.A);
		}
		return this.SpawnInternal(position, 0.5f, itemQuality, null);
	}

	// Token: 0x060077B1 RID: 30641 RVA: 0x002FC348 File Offset: 0x002FA548
	public PickupObject.ItemQuality GetQualityFromChest(Chest c)
	{
		if (this.CompareChest(c, this.D_Chest))
		{
			return PickupObject.ItemQuality.D;
		}
		if (this.CompareChest(c, this.C_Chest))
		{
			return PickupObject.ItemQuality.C;
		}
		if (this.CompareChest(c, this.B_Chest))
		{
			return PickupObject.ItemQuality.B;
		}
		if (this.CompareChest(c, this.A_Chest))
		{
			return PickupObject.ItemQuality.A;
		}
		if (this.CompareChest(c, this.S_Chest))
		{
			return PickupObject.ItemQuality.S;
		}
		return PickupObject.ItemQuality.EXCLUDED;
	}

	// Token: 0x060077B2 RID: 30642 RVA: 0x002FC3BC File Offset: 0x002FA5BC
	private bool CompareChest(Chest c1, Chest c2)
	{
		return c1.lootTable.D_Chance == c2.lootTable.D_Chance && c1.lootTable.C_Chance == c2.lootTable.C_Chance && c1.lootTable.B_Chance == c2.lootTable.B_Chance && c1.lootTable.A_Chance == c2.lootTable.A_Chance && c1.lootTable.S_Chance == c2.lootTable.S_Chance;
	}

	// Token: 0x060077B3 RID: 30643 RVA: 0x002FC450 File Offset: 0x002FA650
	public Chest SpawnRewardChestAt(IntVector2 position, float overrideGunVsItemChance = -1f, PickupObject.ItemQuality excludedQuality = PickupObject.ItemQuality.EXCLUDED)
	{
		FloorRewardData rewardDataForFloor = this.GetRewardDataForFloor(GameManager.Instance.BestGenerationDungeonPrefab.tileIndices.tilesetId);
		PickupObject.ItemQuality itemQuality = rewardDataForFloor.GetRandomTargetQuality(false, false);
		if (PassiveItem.IsFlagSetAtAll(typeof(SevenLeafCloverItem)))
		{
			itemQuality = ((UnityEngine.Random.value >= 0.5f) ? PickupObject.ItemQuality.S : PickupObject.ItemQuality.A);
		}
		return this.SpawnInternal(position, (overrideGunVsItemChance < 0f) ? rewardDataForFloor.GunVersusItemPercentChance : overrideGunVsItemChance, itemQuality, null);
	}

	// Token: 0x060077B4 RID: 30644 RVA: 0x002FC4CC File Offset: 0x002FA6CC
	public Chest GenerationSpawnRewardChestAt(IntVector2 positionInRoom, RoomHandler targetRoom, PickupObject.ItemQuality? targetQuality = null, float overrideMimicChance = -1f)
	{
		System.Random random = ((!GameManager.Instance.IsSeeded) ? null : BraveRandom.GeneratorRandom);
		FloorRewardData rewardDataForFloor = this.GetRewardDataForFloor(GameManager.Instance.BestGenerationDungeonPrefab.tileIndices.tilesetId);
		bool flag = StaticReferenceManager.DChestsSpawnedInTotal >= 2;
		if (targetQuality == null)
		{
			targetQuality = new PickupObject.ItemQuality?(rewardDataForFloor.GetRandomTargetQuality(true, flag));
			if (PassiveItem.IsFlagSetAtAll(typeof(SevenLeafCloverItem)))
			{
				targetQuality = new PickupObject.ItemQuality?((((random == null) ? UnityEngine.Random.value : ((float)random.NextDouble())) >= 0.5f) ? PickupObject.ItemQuality.S : PickupObject.ItemQuality.A);
			}
		}
		if (targetQuality == PickupObject.ItemQuality.D && StaticReferenceManager.DChestsSpawnedOnFloor >= 1 && GameManager.Instance.Dungeon.tileIndices.tilesetId != GlobalDungeonData.ValidTilesets.CASTLEGEON)
		{
			targetQuality = new PickupObject.ItemQuality?(PickupObject.ItemQuality.C);
		}
		Vector2 zero = Vector2.zero;
		if (targetQuality == PickupObject.ItemQuality.A || targetQuality == PickupObject.ItemQuality.S)
		{
			zero = new Vector2(-0.5f, 0f);
		}
		Chest chest = this.GetTargetChestPrefab(targetQuality.Value);
		if (GameStatsManager.Instance.GetFlag(GungeonFlags.SYNERGRACE_UNLOCKED) && GameManager.Instance.BestGenerationDungeonPrefab.tileIndices.tilesetId != GlobalDungeonData.ValidTilesets.CASTLEGEON)
		{
			float num = ((random == null) ? UnityEngine.Random.value : ((float)random.NextDouble()));
			if (num < this.GlobalSynerchestChance)
			{
				chest = this.Synergy_Chest;
				zero = new Vector2(-0.1875f, 0f);
			}
		}
		Chest.GeneralChestType generalChestType = ((BraveRandom.GenerationRandomValue() >= rewardDataForFloor.GunVersusItemPercentChance) ? Chest.GeneralChestType.ITEM : Chest.GeneralChestType.WEAPON);
		if (StaticReferenceManager.ItemChestsSpawnedOnFloor > 0 && StaticReferenceManager.WeaponChestsSpawnedOnFloor == 0)
		{
			generalChestType = Chest.GeneralChestType.WEAPON;
		}
		else if (StaticReferenceManager.WeaponChestsSpawnedOnFloor > 0 && StaticReferenceManager.ItemChestsSpawnedOnFloor == 0)
		{
			generalChestType = Chest.GeneralChestType.ITEM;
		}
		GenericLootTable genericLootTable = ((generalChestType != Chest.GeneralChestType.WEAPON) ? this.ItemsLootTable : this.GunsLootTable);
		GameObject gameObject = DungeonPlaceableUtility.InstantiateDungeonPlaceable(chest.gameObject, targetRoom, positionInRoom, true, AIActor.AwakenAnimationType.Default, false);
		gameObject.transform.position = gameObject.transform.position + zero.ToVector3ZUp(0f);
		Chest component = gameObject.GetComponent<Chest>();
		if (overrideMimicChance >= 0f)
		{
			component.overrideMimicChance = overrideMimicChance;
		}
		Component[] componentsInChildren = gameObject.GetComponentsInChildren(typeof(IPlaceConfigurable));
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			IPlaceConfigurable placeConfigurable = componentsInChildren[i] as IPlaceConfigurable;
			if (placeConfigurable != null)
			{
				placeConfigurable.ConfigureOnPlacement(targetRoom);
			}
		}
		if (targetQuality == PickupObject.ItemQuality.A)
		{
			GameManager.Instance.Dungeon.GeneratedMagnificence += 1f;
			component.GeneratedMagnificence += 1f;
		}
		else if (targetQuality == PickupObject.ItemQuality.S)
		{
			GameManager.Instance.Dungeon.GeneratedMagnificence += 1f;
			component.GeneratedMagnificence += 1f;
		}
		if (component.specRigidbody)
		{
			component.specRigidbody.Reinitialize();
		}
		component.ChestType = generalChestType;
		component.lootTable.lootTable = genericLootTable;
		if (component.lootTable.canDropMultipleItems && component.lootTable.overrideItemLootTables != null && component.lootTable.overrideItemLootTables.Count > 0)
		{
			component.lootTable.overrideItemLootTables[0] = genericLootTable;
		}
		if (targetQuality == PickupObject.ItemQuality.D && !component.IsMimic)
		{
			StaticReferenceManager.DChestsSpawnedOnFloor++;
			StaticReferenceManager.DChestsSpawnedInTotal++;
			component.IsLocked = true;
			if (component.LockAnimator)
			{
				component.LockAnimator.renderer.enabled = true;
			}
		}
		targetRoom.RegisterInteractable(component);
		if (this.SeededRunManifests.ContainsKey(GameManager.Instance.BestGenerationDungeonPrefab.tileIndices.tilesetId))
		{
			component.GenerationDetermineContents(this.SeededRunManifests[GameManager.Instance.BestGenerationDungeonPrefab.tileIndices.tilesetId], random);
		}
		return component;
	}

	// Token: 0x060077B5 RID: 30645 RVA: 0x002FC964 File Offset: 0x002FAB64
	public bool IsItemInSeededManifests(PickupObject testItem)
	{
		foreach (KeyValuePair<GlobalDungeonData.ValidTilesets, FloorRewardManifest> keyValuePair in this.SeededRunManifests)
		{
			FloorRewardManifest value = keyValuePair.Value;
			if (value.CheckManifestDifferentiator(testItem))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x060077B6 RID: 30646 RVA: 0x002FC9D8 File Offset: 0x002FABD8
	public FloorRewardManifest GetSeededManifestForCurrentFloor()
	{
		GlobalDungeonData.ValidTilesets tilesetId = GameManager.Instance.BestGenerationDungeonPrefab.tileIndices.tilesetId;
		if (this.SeededRunManifests != null && this.SeededRunManifests.ContainsKey(tilesetId))
		{
			return this.SeededRunManifests[tilesetId];
		}
		return null;
	}

	// Token: 0x060077B7 RID: 30647 RVA: 0x002FCA24 File Offset: 0x002FAC24
	public void CopyBossGunChancesFromTierZero(int targetTier)
	{
		this.FloorRewardData[targetTier].D_BossGun_Chance = this.FloorRewardData[0].D_BossGun_Chance;
		this.FloorRewardData[targetTier].C_BossGun_Chance = this.FloorRewardData[0].C_BossGun_Chance;
		this.FloorRewardData[targetTier].B_BossGun_Chance = this.FloorRewardData[0].B_BossGun_Chance;
		this.FloorRewardData[targetTier].A_BossGun_Chance = this.FloorRewardData[0].A_BossGun_Chance;
		this.FloorRewardData[targetTier].S_BossGun_Chance = this.FloorRewardData[0].S_BossGun_Chance;
	}

	// Token: 0x060077B8 RID: 30648 RVA: 0x002FCADC File Offset: 0x002FACDC
	public void CopyShopChancesFromTierZero(int targetTier)
	{
		this.FloorRewardData[targetTier].D_Shop_Chance = this.FloorRewardData[0].D_Shop_Chance;
		this.FloorRewardData[targetTier].C_Shop_Chance = this.FloorRewardData[0].C_Shop_Chance;
		this.FloorRewardData[targetTier].B_Shop_Chance = this.FloorRewardData[0].B_Shop_Chance;
		this.FloorRewardData[targetTier].A_Shop_Chance = this.FloorRewardData[0].A_Shop_Chance;
		this.FloorRewardData[targetTier].S_Shop_Chance = this.FloorRewardData[0].S_Shop_Chance;
	}

	// Token: 0x060077B9 RID: 30649 RVA: 0x002FCB94 File Offset: 0x002FAD94
	public void CopyChestChancesFromTierZero(int targetTier)
	{
		this.FloorRewardData[targetTier].D_Chest_Chance = this.FloorRewardData[0].D_Chest_Chance;
		this.FloorRewardData[targetTier].C_Chest_Chance = this.FloorRewardData[0].C_Chest_Chance;
		this.FloorRewardData[targetTier].B_Chest_Chance = this.FloorRewardData[0].B_Chest_Chance;
		this.FloorRewardData[targetTier].A_Chest_Chance = this.FloorRewardData[0].A_Chest_Chance;
		this.FloorRewardData[targetTier].S_Chest_Chance = this.FloorRewardData[0].S_Chest_Chance;
	}

	// Token: 0x060077BA RID: 30650 RVA: 0x002FCC4C File Offset: 0x002FAE4C
	public void CopyDropChestChancesFromTierZero(int targetTier)
	{
		this.FloorRewardData[targetTier].D_RoomChest_Chance = this.FloorRewardData[0].D_RoomChest_Chance;
		this.FloorRewardData[targetTier].C_RoomChest_Chance = this.FloorRewardData[0].C_RoomChest_Chance;
		this.FloorRewardData[targetTier].B_RoomChest_Chance = this.FloorRewardData[0].B_RoomChest_Chance;
		this.FloorRewardData[targetTier].A_RoomChest_Chance = this.FloorRewardData[0].A_RoomChest_Chance;
		this.FloorRewardData[targetTier].S_RoomChest_Chance = this.FloorRewardData[0].S_RoomChest_Chance;
	}

	// Token: 0x060077BB RID: 30651 RVA: 0x002FCD04 File Offset: 0x002FAF04
	public void CopyTertiaryBossSpawnsFromTierZero(int targetTier)
	{
		this.FloorRewardData[targetTier].TertiaryBossRewardSets = new List<TertiaryBossRewardSet>(this.FloorRewardData[0].TertiaryBossRewardSets);
	}

	// Token: 0x0400798D RID: 31117
	[NonSerialized]
	public static float AdditionalHeartTierMagnificence;

	// Token: 0x0400798E RID: 31118
	[SerializeField]
	public List<FloorRewardData> FloorRewardData;

	// Token: 0x0400798F RID: 31119
	[Header("Chest Definitions")]
	public Chest D_Chest;

	// Token: 0x04007990 RID: 31120
	public Chest C_Chest;

	// Token: 0x04007991 RID: 31121
	public Chest B_Chest;

	// Token: 0x04007992 RID: 31122
	public Chest A_Chest;

	// Token: 0x04007993 RID: 31123
	public Chest S_Chest;

	// Token: 0x04007994 RID: 31124
	public Chest Rainbow_Chest;

	// Token: 0x04007995 RID: 31125
	public Chest Synergy_Chest;

	// Token: 0x04007996 RID: 31126
	[Header("Loot Table Definitions")]
	public GenericLootTable GunsLootTable;

	// Token: 0x04007997 RID: 31127
	public GenericLootTable ItemsLootTable;

	// Token: 0x04007998 RID: 31128
	[Header("Global Currency Settings")]
	public float BossGoldCoinChance = 0.0003f;

	// Token: 0x04007999 RID: 31129
	public float PowerfulGoldCoinChance = 0.000125f;

	// Token: 0x0400799A RID: 31130
	public float NormalGoldCoinChance = 5E-05f;

	// Token: 0x0400799B RID: 31131
	[Space(5f)]
	public int RobotMinCurrencyPerHealthItem = 5;

	// Token: 0x0400799C RID: 31132
	public int RobotMaxCurrencyPerHealthItem = 10;

	// Token: 0x0400799D RID: 31133
	[Header("Synergy Settings")]
	public float GlobalSynerchestChance = 0.02f;

	// Token: 0x0400799E RID: 31134
	public float SynergyCompletionMultiplier = 1f;

	// Token: 0x0400799F RID: 31135
	public bool SynergyCompletionIgnoresQualities;

	// Token: 0x040079A0 RID: 31136
	[Header("Additional Settings")]
	public float EarlyChestChanceIfNotChump = 0.2f;

	// Token: 0x040079A1 RID: 31137
	public float RoomClearRainbowChance = 0.0001f;

	// Token: 0x040079A2 RID: 31138
	[PickupIdentifier]
	public int FullHeartIdPrefab = -1;

	// Token: 0x040079A3 RID: 31139
	[PickupIdentifier]
	public int HalfHeartIdPrefab = -1;

	// Token: 0x040079A4 RID: 31140
	public float SinglePlayerPickupIncrementModifier = 1.25f;

	// Token: 0x040079A5 RID: 31141
	public float CoopPickupIncrementModifier = 1.5f;

	// Token: 0x040079A6 RID: 31142
	public float CoopAmmoChanceModifier = 1.5f;

	// Token: 0x040079A7 RID: 31143
	public float GunMimicMimicGunChance = 0.001f;

	// Token: 0x040079A8 RID: 31144
	[Header("Bonus Enemy Spawn Settings")]
	public BonusEnemySpawns KeybulletsChances;

	// Token: 0x040079A9 RID: 31145
	public BonusEnemySpawns ChanceBulletChances;

	// Token: 0x040079AA RID: 31146
	public BonusEnemySpawns WallMimicChances;

	// Token: 0x040079AB RID: 31147
	[Header("Heart Magnificence Settings")]
	public float OneOrTwoHeartMagMultiplier = 0.333f;

	// Token: 0x040079AC RID: 31148
	public float ThreeOrMoreHeartMagMultiplier = 0.1f;

	// Token: 0x040079AD RID: 31149
	[Header("Chest Destruction Settings")]
	public float ChestDowngradeChance = 0.25f;

	// Token: 0x040079AE RID: 31150
	public float ChestHalfHeartChance = 0.2f;

	// Token: 0x040079AF RID: 31151
	public float ChestJunkChance = 0.45f;

	// Token: 0x040079B0 RID: 31152
	public float ChestExplosionChance = 0.1f;

	// Token: 0x040079B1 RID: 31153
	public float ChestJunkanUnlockedChance = 0.05f;

	// Token: 0x040079B2 RID: 31154
	public float HasKeyJunkMultiplier = 3f;

	// Token: 0x040079B3 RID: 31155
	public float HasJunkanJunkMultiplier = 1.5f;

	// Token: 0x040079B4 RID: 31156
	[Header("Data References (for Brents)")]
	[EnemyIdentifier]
	public string FacelessCultistGuid;

	// Token: 0x040079B5 RID: 31157
	public float FacelessChancePerFloor = 0.15f;

	// Token: 0x040079B6 RID: 31158
	[Header("Bowler Notes")]
	public GameObject BowlerNotePostRainbow;

	// Token: 0x040079B7 RID: 31159
	public GameObject BowlerNoteChest;

	// Token: 0x040079B8 RID: 31160
	public GameObject BowlerNoteOtherSource;

	// Token: 0x040079B9 RID: 31161
	public GameObject BowlerNoteMimic;

	// Token: 0x040079BA RID: 31162
	public GameObject BowlerNoteShop;

	// Token: 0x040079BB RID: 31163
	public GameObject BowlerNoteBoss;

	// Token: 0x040079BC RID: 31164
	[Header("Demo Mode Stuff For Pax EAST 2018")]
	[EnemyIdentifier]
	public string PhaseSpiderGUID;

	// Token: 0x040079BD RID: 31165
	[EnemyIdentifier]
	public string ChancebulonGUID;

	// Token: 0x040079BE RID: 31166
	[EnemyIdentifier]
	public string DisplacerBeastGUID;

	// Token: 0x040079BF RID: 31167
	[EnemyIdentifier]
	public string GripmasterGUID;

	// Token: 0x040079C0 RID: 31168
	public List<EnemyReplacementTier> ReplacementTiers;

	// Token: 0x040079C1 RID: 31169
	[NonSerialized]
	public Dictionary<GlobalDungeonData.ValidTilesets, FloorRewardManifest> SeededRunManifests = new Dictionary<GlobalDungeonData.ValidTilesets, FloorRewardManifest>();

	// Token: 0x0200148E RID: 5262
	public enum RewardSource
	{
		// Token: 0x040079C3 RID: 31171
		UNSPECIFIED,
		// Token: 0x040079C4 RID: 31172
		BOSS_PEDESTAL
	}
}
