using System;
using System.Collections.Generic;
using Dungeonator;
using UnityEngine;

// Token: 0x0200155D RID: 5469
[Serializable]
public class LootData
{
	// Token: 0x06007D43 RID: 32067 RVA: 0x0032A0F4 File Offset: 0x003282F4
	private void ClearPerDropData()
	{
		this.PreferGunDrop = false;
		this.ForceNotCommon = false;
	}

	// Token: 0x06007D44 RID: 32068 RVA: 0x0032A104 File Offset: 0x00328304
	public PickupObject GetSingleItemForPlayer(PlayerController player, int tierShift = 0)
	{
		GameObject itemForPlayer = this.GetItemForPlayer(player, this.lootTable, null, tierShift, false, null, null);
		this.ClearPerDropData();
		if (itemForPlayer != null)
		{
			return itemForPlayer.GetComponent<PickupObject>();
		}
		return null;
	}

	// Token: 0x06007D45 RID: 32069 RVA: 0x0032A148 File Offset: 0x00328348
	public List<PickupObject> GetItemsForPlayer(PlayerController player, int tierShift = 0, GenericLootTable OverrideDropTable = null, System.Random generatorRandom = null)
	{
		this.LastGenerationNumSynergiesCalculated = 0;
		List<GameObject> list = new List<GameObject>();
		List<PickupObject> list2 = new List<PickupObject>();
		int num = ((!this.canDropMultipleItems) ? 1 : this.multipleItemDropChances.SelectByWeight(generatorRandom));
		bool flag = false;
		for (int i = 0; i < num; i++)
		{
			GameObject gameObject;
			if (num > 1 && this.overrideItemLootTables.Count > i && this.overrideItemLootTables[i] != null)
			{
				PickupObject.ItemQuality? itemQuality = null;
				if (this.overrideItemQualities != null && this.overrideItemQualities.Count > i)
				{
					itemQuality = new PickupObject.ItemQuality?(this.overrideItemQualities[i]);
				}
				gameObject = this.GetItemForPlayer(player, this.overrideItemLootTables[i], list, tierShift, flag, itemQuality, generatorRandom);
			}
			else
			{
				GenericLootTable genericLootTable = this.lootTable;
				List<GameObject> list3 = list;
				bool flag2 = flag;
				gameObject = this.GetItemForPlayer(player, genericLootTable, list3, tierShift, flag2, null, generatorRandom);
			}
			if (gameObject != null)
			{
				PickupObject component = gameObject.GetComponent<PickupObject>();
				if (component is Gun && this.onlyOneGunCanDrop)
				{
					flag = true;
				}
				list2.Add(component);
				list.Add(gameObject);
			}
			this.ClearPerDropData();
		}
		if (GameStatsManager.Instance.GetFlag(GungeonFlags.ITEMSPECIFIC_AMMONOMICON_COMPLETE))
		{
			for (int j = 0; j < list2.Count; j++)
			{
				if (list2[j].PickupObjectId == GlobalItemIds.UnfinishedGun)
				{
					list2[j] = PickupObjectDatabase.GetById(GlobalItemIds.FinishedGun);
				}
			}
		}
		return list2;
	}

	// Token: 0x06007D46 RID: 32070 RVA: 0x0032A308 File Offset: 0x00328508
	public GameObject GetItemForPlayer(PlayerController player, GenericLootTable tableToUse, List<GameObject> excludedObjects, int tierShift = 0, bool excludeGuns = false, PickupObject.ItemQuality? overrideQuality = null, System.Random generatorRandom = null)
	{
		PickupObject.ItemQuality itemQuality = ((overrideQuality == null) ? this.GetTargetItemQuality(player, generatorRandom) : overrideQuality.Value);
		itemQuality = (PickupObject.ItemQuality)Mathf.Min(5, Mathf.Max(0, (int)(itemQuality + tierShift)));
		bool flag = false;
		bool flag2 = GameStatsManager.HasInstance && GameStatsManager.Instance.IsRainbowRun;
		List<int> rainbowRunForceExcludedIDs = GameManager.Instance.RainbowRunForceExcludedIDs;
		List<int> rainbowRunForceIncludedIDs = GameManager.Instance.RainbowRunForceIncludedIDs;
		if (this.CompletesSynergy)
		{
			SynercacheManager.UseCachedSynergyIDs = true;
		}
		while (itemQuality >= PickupObject.ItemQuality.COMMON)
		{
			if (itemQuality > PickupObject.ItemQuality.COMMON)
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
					bool flag3 = RewardManager.CheckQualityForItem(component, player, itemQuality, this.CompletesSynergy, RewardManager.RewardSource.UNSPECIFIED);
					if ((component.ItemSpansBaseQualityTiers || component.ItemRespectsHeartMagnificence) && itemQuality != PickupObject.ItemQuality.D && itemQuality != PickupObject.ItemQuality.COMMON && itemQuality != PickupObject.ItemQuality.S)
					{
						flag3 = true;
					}
					if (component is SpiceItem && player != null && player.spiceCount > 0)
					{
						flag3 = true;
					}
					if (component != null && flag3)
					{
						bool flag4 = true;
						float num3 = compiledRawItems[i].weight;
						if (excludedObjects == null || !excludedObjects.Contains(component.gameObject))
						{
							if (flag2)
							{
								if (rainbowRunForceExcludedIDs != null && rainbowRunForceExcludedIDs.Contains(component.PickupObjectId))
								{
									goto IL_530;
								}
								if ((itemQuality == PickupObject.ItemQuality.D || itemQuality == PickupObject.ItemQuality.C) && rainbowRunForceIncludedIDs != null && !rainbowRunForceIncludedIDs.Contains(component.PickupObjectId))
								{
									goto IL_530;
								}
							}
							if (!(component is Gun) || !excludeGuns)
							{
								if (!component.PrerequisitesMet())
								{
									flag4 = false;
								}
								if (component is Gun)
								{
									Gun gun = component as Gun;
									if (gun.InfiniteAmmo && !gun.CanBeDropped && gun.quality == PickupObject.ItemQuality.SPECIAL)
									{
										goto IL_530;
									}
									GunClass gunClass = gun.gunClass;
									if (gunClass != GunClass.NONE)
									{
										int num4 = ((!(player == null)) ? player.inventory.ContainsGunOfClass(gunClass, true) : 0);
										float modifierForClass = LootDataGlobalSettings.Instance.GetModifierForClass(gunClass);
										num3 *= Mathf.Pow(modifierForClass, (float)num4);
									}
									if (this.PreferGunDrop)
									{
										num3 *= 1000f;
									}
								}
								float multiplierForItem = RewardManager.GetMultiplierForItem(component, player, this.CompletesSynergy);
								if (this.CompletesSynergy && multiplierForItem > 100000f)
								{
									this.LastGenerationNumSynergiesCalculated++;
								}
								num3 *= multiplierForItem;
								if (RoomHandler.unassignedInteractableObjects != null)
								{
									for (int j = 0; j < RoomHandler.unassignedInteractableObjects.Count; j++)
									{
										IPlayerInteractable playerInteractable = RoomHandler.unassignedInteractableObjects[j];
										if (playerInteractable is PickupObject)
										{
											PickupObject pickupObject = playerInteractable as PickupObject;
											if (pickupObject && pickupObject.PickupObjectId == component.PickupObjectId)
											{
												flag4 = false;
												num2 += num3;
												KeyValuePair<WeightedGameObject, float> keyValuePair = new KeyValuePair<WeightedGameObject, float>(compiledRawItems[i], num3);
												list2.Add(keyValuePair);
												break;
											}
										}
									}
								}
								if (GameManager.Instance.IsSeeded)
								{
									if (GameManager.Instance.RewardManager.IsItemInSeededManifests(component))
									{
										flag4 = false;
										num2 += num3;
										KeyValuePair<WeightedGameObject, float> keyValuePair2 = new KeyValuePair<WeightedGameObject, float>(compiledRawItems[i], num3);
										list2.Add(keyValuePair2);
									}
								}
								else
								{
									EncounterTrackable component2 = component.GetComponent<EncounterTrackable>();
									if (component2 != null)
									{
										int num5 = 0;
										if (Application.isPlaying)
										{
											num5 = GameStatsManager.Instance.QueryEncounterableDifferentiator(component2);
										}
										if (this.CompletesSynergy)
										{
											num5 = 0;
										}
										if (num5 > 0 || (Application.isPlaying && GameManager.Instance.ExtantShopTrackableGuids.Contains(component2.EncounterGuid)))
										{
											flag4 = false;
											num2 += num3;
											KeyValuePair<WeightedGameObject, float> keyValuePair3 = new KeyValuePair<WeightedGameObject, float>(compiledRawItems[i], num3);
											list2.Add(keyValuePair3);
										}
										else if (Application.isPlaying && GameStatsManager.Instance.QueryEncounterable(component2) == 0 && GameStatsManager.Instance.QueryEncounterableAnnouncement(component2.EncounterGuid))
										{
											num3 *= 10f;
										}
									}
								}
								if (component.ItemSpansBaseQualityTiers || component.ItemRespectsHeartMagnificence)
								{
									if (RewardManager.AdditionalHeartTierMagnificence >= 3f)
									{
										num3 *= GameManager.Instance.RewardManager.ThreeOrMoreHeartMagMultiplier;
									}
									else if (RewardManager.AdditionalHeartTierMagnificence >= 1f)
									{
										num3 *= GameManager.Instance.RewardManager.OneOrTwoHeartMagMultiplier;
									}
								}
								if (flag4)
								{
									num += num3;
									KeyValuePair<WeightedGameObject, float> keyValuePair4 = new KeyValuePair<WeightedGameObject, float>(compiledRawItems[i], num3);
									list.Add(keyValuePair4);
								}
							}
						}
					}
				}
				IL_530:;
			}
			if (list.Count == 0 && list2.Count > 0)
			{
				list = list2;
				num = num2;
			}
			if (num > 0f && list.Count > 0)
			{
				float num6 = num * ((generatorRandom == null) ? UnityEngine.Random.value : ((float)generatorRandom.NextDouble()));
				for (int k = 0; k < list.Count; k++)
				{
					num6 -= list[k].Value;
					if (num6 <= 0f)
					{
						string text = ((!(list[k].Key.gameObject != null)) ? "noll" : list[k].Key.gameObject.name);
						Debug.Log(string.Concat(new object[]
						{
							"returning item ",
							text,
							" #",
							k.ToString(),
							" of ",
							list.Count,
							"|",
							itemQuality.ToString()
						}));
						SynercacheManager.UseCachedSynergyIDs = false;
						return list[k].Key.gameObject;
					}
				}
				Debug.Log("returning last possible item");
				SynercacheManager.UseCachedSynergyIDs = false;
				return list[list.Count - 1].Key.gameObject;
			}
			itemQuality--;
			if (itemQuality < PickupObject.ItemQuality.COMMON && !flag)
			{
				itemQuality = PickupObject.ItemQuality.D;
			}
		}
		SynercacheManager.UseCachedSynergyIDs = false;
		Debug.LogError("Failed to get any item at all.");
		return null;
	}

	// Token: 0x06007D47 RID: 32071 RVA: 0x0032AA18 File Offset: 0x00328C18
	protected PickupObject.ItemQuality GetTargetItemTier(System.Random generatorRandom)
	{
		float num = ((!this.ForceNotCommon) ? this.Common_Chance : 0f);
		float d_Chance = this.D_Chance;
		float c_Chance = this.C_Chance;
		float b_Chance = this.B_Chance;
		float a_Chance = this.A_Chance;
		float s_Chance = this.S_Chance;
		float num2 = num + d_Chance + c_Chance + b_Chance + a_Chance + s_Chance;
		if (num2 == 0f)
		{
			return PickupObject.ItemQuality.D;
		}
		float num3 = num2 * ((generatorRandom == null) ? UnityEngine.Random.value : ((float)generatorRandom.NextDouble()));
		float num4 = 0f;
		num4 += num;
		if (num4 > num3)
		{
			return PickupObject.ItemQuality.COMMON;
		}
		num4 += d_Chance;
		if (num4 > num3)
		{
			return PickupObject.ItemQuality.D;
		}
		num4 += c_Chance;
		if (num4 > num3)
		{
			return PickupObject.ItemQuality.C;
		}
		num4 += b_Chance;
		if (num4 > num3)
		{
			return PickupObject.ItemQuality.B;
		}
		num4 += a_Chance;
		if (num4 > num3)
		{
			return PickupObject.ItemQuality.A;
		}
		num4 += s_Chance;
		if (num4 > num3)
		{
			return PickupObject.ItemQuality.S;
		}
		return PickupObject.ItemQuality.S;
	}

	// Token: 0x06007D48 RID: 32072 RVA: 0x0032AB10 File Offset: 0x00328D10
	protected PickupObject.ItemQuality GetTargetItemQuality(PlayerController player, System.Random generatorRandom)
	{
		return this.GetTargetItemTier(generatorRandom);
	}

	// Token: 0x04008053 RID: 32851
	public GenericLootTable lootTable;

	// Token: 0x04008054 RID: 32852
	public List<GenericLootTable> overrideItemLootTables;

	// Token: 0x04008055 RID: 32853
	[NonSerialized]
	public List<PickupObject.ItemQuality> overrideItemQualities;

	// Token: 0x04008056 RID: 32854
	public float Common_Chance;

	// Token: 0x04008057 RID: 32855
	public float D_Chance;

	// Token: 0x04008058 RID: 32856
	public float C_Chance;

	// Token: 0x04008059 RID: 32857
	public float B_Chance;

	// Token: 0x0400805A RID: 32858
	public float A_Chance;

	// Token: 0x0400805B RID: 32859
	public float S_Chance;

	// Token: 0x0400805C RID: 32860
	public bool CompletesSynergy;

	// Token: 0x0400805D RID: 32861
	public bool canDropMultipleItems;

	// Token: 0x0400805E RID: 32862
	public bool onlyOneGunCanDrop = true;

	// Token: 0x0400805F RID: 32863
	[ShowInInspectorIf("canDropMultipleItems", false)]
	public WeightedIntCollection multipleItemDropChances;

	// Token: 0x04008060 RID: 32864
	[NonSerialized]
	public bool ForceNotCommon;

	// Token: 0x04008061 RID: 32865
	[NonSerialized]
	public bool PreferGunDrop;

	// Token: 0x04008062 RID: 32866
	[NonSerialized]
	public int LastGenerationNumSynergiesCalculated;
}
