using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200148F RID: 5263
[Serializable]
public class FloorRewardData
{
	// Token: 0x060077BE RID: 30654 RVA: 0x002FCED4 File Offset: 0x002FB0D4
	public float SumChances()
	{
		return this.D_Chest_Chance + this.C_Chest_Chance + this.B_Chest_Chance + this.A_Chest_Chance + this.S_Chest_Chance;
	}

	// Token: 0x060077BF RID: 30655 RVA: 0x002FCEF8 File Offset: 0x002FB0F8
	public float SumRoomChances()
	{
		return this.D_RoomChest_Chance + this.C_RoomChest_Chance + this.B_RoomChest_Chance + this.A_RoomChest_Chance + this.S_RoomChest_Chance;
	}

	// Token: 0x060077C0 RID: 30656 RVA: 0x002FCF1C File Offset: 0x002FB11C
	public float SumBossGunChances()
	{
		return this.D_BossGun_Chance + this.C_BossGun_Chance + this.B_BossGun_Chance + this.A_BossGun_Chance + this.S_BossGun_Chance;
	}

	// Token: 0x060077C1 RID: 30657 RVA: 0x002FCF40 File Offset: 0x002FB140
	public float SumShopChances()
	{
		return this.D_Shop_Chance + this.C_Shop_Chance + this.B_Shop_Chance + this.A_Shop_Chance + this.S_Shop_Chance;
	}

	// Token: 0x060077C2 RID: 30658 RVA: 0x002FCF64 File Offset: 0x002FB164
	public float DetermineCurrentMagnificence(bool isGenerationForMagnificence = false)
	{
		float num = 0f;
		if (GameManager.Instance.PrimaryPlayer != null)
		{
			num += GameManager.Instance.PrimaryPlayer.stats.Magnificence;
		}
		if (GameManager.Instance.Dungeon != null)
		{
			if (isGenerationForMagnificence)
			{
				num += GameManager.Instance.Dungeon.GeneratedMagnificence * 2f;
			}
			else
			{
				num += GameManager.Instance.Dungeon.GeneratedMagnificence;
			}
		}
		return num;
	}

	// Token: 0x060077C3 RID: 30659 RVA: 0x002FCFF0 File Offset: 0x002FB1F0
	public PickupObject.ItemQuality GetTargetQualityFromChances(float fran, float dChance, float cChance, float bChance, float aChance, float sChance, bool isGenerationForMagnificence = false)
	{
		float num = this.DetermineCurrentMagnificence(isGenerationForMagnificence);
		if (fran < dChance)
		{
			return MagnificenceConstants.ModifyQualityByMagnificence(PickupObject.ItemQuality.D, num, dChance, cChance, bChance);
		}
		if (fran < dChance + cChance)
		{
			return MagnificenceConstants.ModifyQualityByMagnificence(PickupObject.ItemQuality.C, num, dChance, cChance, bChance);
		}
		if (fran < dChance + cChance + bChance)
		{
			return MagnificenceConstants.ModifyQualityByMagnificence(PickupObject.ItemQuality.B, num, dChance, cChance, bChance);
		}
		if (fran < dChance + cChance + bChance + aChance)
		{
			return MagnificenceConstants.ModifyQualityByMagnificence(PickupObject.ItemQuality.A, num, dChance, cChance, bChance);
		}
		return MagnificenceConstants.ModifyQualityByMagnificence(PickupObject.ItemQuality.S, num, dChance, cChance, bChance);
	}

	// Token: 0x060077C4 RID: 30660 RVA: 0x002FD06C File Offset: 0x002FB26C
	public PickupObject.ItemQuality GetShopTargetQuality(bool useSeedRandom = false)
	{
		float num = this.SumShopChances();
		float num2 = ((!useSeedRandom) ? UnityEngine.Random.value : BraveRandom.GenerationRandomValue()) * num;
		return this.GetTargetQualityFromChances(num2, this.D_Shop_Chance, this.C_Shop_Chance, this.B_Shop_Chance, this.A_Shop_Chance, this.S_Shop_Chance, false);
	}

	// Token: 0x060077C5 RID: 30661 RVA: 0x002FD0C0 File Offset: 0x002FB2C0
	public PickupObject.ItemQuality GetRandomBossTargetQuality(System.Random safeRandom = null)
	{
		float num = this.SumBossGunChances();
		float num2 = ((safeRandom == null) ? UnityEngine.Random.value : ((float)safeRandom.NextDouble())) * num;
		PickupObject.ItemQuality targetQualityFromChances = this.GetTargetQualityFromChances(num2, this.D_BossGun_Chance, this.C_BossGun_Chance, this.B_BossGun_Chance, this.A_BossGun_Chance, this.S_BossGun_Chance, false);
		Debug.Log(targetQualityFromChances + " <= boss quality");
		return targetQualityFromChances;
	}

	// Token: 0x060077C6 RID: 30662 RVA: 0x002FD12C File Offset: 0x002FB32C
	public PickupObject.ItemQuality GetRandomTargetQuality(bool isGenerationForMagnificence = false, bool forceDChanceZero = false)
	{
		float num = ((!forceDChanceZero) ? this.SumChances() : (this.C_Chest_Chance + this.B_Chest_Chance + this.A_Chest_Chance + this.S_Chest_Chance));
		float num2;
		if (isGenerationForMagnificence)
		{
			num2 = BraveRandom.GenerationRandomValue() * num;
		}
		else
		{
			num2 = UnityEngine.Random.value * num;
		}
		return this.GetTargetQualityFromChances(num2, (!forceDChanceZero) ? this.D_Chest_Chance : 0f, this.C_Chest_Chance, this.B_Chest_Chance, this.A_Chest_Chance, this.S_Chest_Chance, isGenerationForMagnificence);
	}

	// Token: 0x060077C7 RID: 30663 RVA: 0x002FD1C0 File Offset: 0x002FB3C0
	public PickupObject.ItemQuality GetRandomRoomTargetQuality()
	{
		float num = this.SumRoomChances();
		float num2 = UnityEngine.Random.value * num;
		float num3 = this.D_RoomChest_Chance;
		float num4 = this.C_RoomChest_Chance;
		float num5 = this.B_RoomChest_Chance;
		if (PassiveItem.IsFlagSetAtAll(typeof(AmazingChestAheadItem)))
		{
			float num6 = num3 / 2f;
			num3 -= num6;
			num4 += num6 / 2f;
			num5 += num6 / 2f;
		}
		return this.GetTargetQualityFromChances(num2, num3, num4, num5, this.A_RoomChest_Chance, this.S_RoomChest_Chance, false);
	}

	// Token: 0x040079C5 RID: 31173
	public string Annotation;

	// Token: 0x040079C6 RID: 31174
	[EnumFlags]
	public GlobalDungeonData.ValidTilesets AssociatedTilesets;

	// Token: 0x040079C7 RID: 31175
	[Header("Currency Drops")]
	public float AverageCurrencyDropsThisFloor = 60f;

	// Token: 0x040079C8 RID: 31176
	public float CurrencyDropsStandardDeviation = 15f;

	// Token: 0x040079C9 RID: 31177
	public float MinimumCurrencyDropsThisFloor = 40f;

	// Token: 0x040079CA RID: 31178
	[RewardManagerReset("Chest Type Chances", "Copy From Tier 0", "CopyChestChancesFromTierZero", 0)]
	public float D_Chest_Chance = 0.2f;

	// Token: 0x040079CB RID: 31179
	public float C_Chest_Chance = 0.2f;

	// Token: 0x040079CC RID: 31180
	public float B_Chest_Chance = 0.2f;

	// Token: 0x040079CD RID: 31181
	public float A_Chest_Chance = 0.2f;

	// Token: 0x040079CE RID: 31182
	public float S_Chest_Chance = 0.2f;

	// Token: 0x040079CF RID: 31183
	[Header("Global Drops")]
	public float ChestSystem_ChestChanceLowerBound = 0.01f;

	// Token: 0x040079D0 RID: 31184
	public float ChestSystem_ChestChanceUpperBound = 0.2f;

	// Token: 0x040079D1 RID: 31185
	public float ChestSystem_Increment = 0.03f;

	// Token: 0x040079D2 RID: 31186
	[Space(3f)]
	public float GunVersusItemPercentChance = 0.5f;

	// Token: 0x040079D3 RID: 31187
	[Space(3f)]
	public float PercentOfRoomClearRewardsThatAreChests = 0.2f;

	// Token: 0x040079D4 RID: 31188
	public GenericLootTable SingleItemRewardTable;

	// Token: 0x040079D5 RID: 31189
	[Space(3f)]
	public float FloorChanceToDropAmmo = 0.0625f;

	// Token: 0x040079D6 RID: 31190
	public float FloorChanceForSpreadAmmo = 0.5f;

	// Token: 0x040079D7 RID: 31191
	[RewardManagerReset("Global Drop Type Chances", "Copy From Tier 0", "CopyDropChestChancesFromTierZero", 2)]
	public float D_RoomChest_Chance = 0.2f;

	// Token: 0x040079D8 RID: 31192
	public float C_RoomChest_Chance = 0.2f;

	// Token: 0x040079D9 RID: 31193
	public float B_RoomChest_Chance = 0.2f;

	// Token: 0x040079DA RID: 31194
	public float A_RoomChest_Chance = 0.2f;

	// Token: 0x040079DB RID: 31195
	public float S_RoomChest_Chance = 0.2f;

	// Token: 0x040079DC RID: 31196
	[RewardManagerReset("Boss Gun Qualities", "Copy From Tier 0", "CopyBossGunChancesFromTierZero", 0)]
	public float D_BossGun_Chance = 0.1f;

	// Token: 0x040079DD RID: 31197
	public float C_BossGun_Chance = 0.3f;

	// Token: 0x040079DE RID: 31198
	public float B_BossGun_Chance = 0.3f;

	// Token: 0x040079DF RID: 31199
	public float A_BossGun_Chance = 0.2f;

	// Token: 0x040079E0 RID: 31200
	public float S_BossGun_Chance = 0.1f;

	// Token: 0x040079E1 RID: 31201
	[RewardManagerReset("Shop Gun/Item Qualities", "Copy From Tier 0", "CopyShopChancesFromTierZero", 0)]
	public float D_Shop_Chance = 0.1f;

	// Token: 0x040079E2 RID: 31202
	public float C_Shop_Chance = 0.3f;

	// Token: 0x040079E3 RID: 31203
	public float B_Shop_Chance = 0.3f;

	// Token: 0x040079E4 RID: 31204
	public float A_Shop_Chance = 0.2f;

	// Token: 0x040079E5 RID: 31205
	public float S_Shop_Chance = 0.1f;

	// Token: 0x040079E6 RID: 31206
	public float ReplaceFirstRewardWithPickup = 0.2f;

	// Token: 0x040079E7 RID: 31207
	[Header("Meta Currency")]
	public int MinMetaCurrencyFromBoss;

	// Token: 0x040079E8 RID: 31208
	public int MaxMetaCurrencyFromBoss;

	// Token: 0x040079E9 RID: 31209
	public bool AlternateItemChestChances;

	// Token: 0x040079EA RID: 31210
	[ShowInInspectorIf("AlternateItemChestChances", false)]
	public float D_Item_Chest_Chance = 0.2f;

	// Token: 0x040079EB RID: 31211
	[ShowInInspectorIf("AlternateItemChestChances", false)]
	public float C_Item_Chest_Chance = 0.2f;

	// Token: 0x040079EC RID: 31212
	[ShowInInspectorIf("AlternateItemChestChances", false)]
	public float B_Item_Chest_Chance = 0.2f;

	// Token: 0x040079ED RID: 31213
	[ShowInInspectorIf("AlternateItemChestChances", false)]
	public float A_Item_Chest_Chance = 0.2f;

	// Token: 0x040079EE RID: 31214
	[ShowInInspectorIf("AlternateItemChestChances", false)]
	public float S_Item_Chest_Chance = 0.2f;

	// Token: 0x040079EF RID: 31215
	[RewardManagerReset("For Bosses", "Copy From Tier 0", "CopyTertiaryBossSpawnsFromTierZero", 1)]
	public GenericLootTable FallbackBossLootTable;

	// Token: 0x040079F0 RID: 31216
	public List<TertiaryBossRewardSet> TertiaryBossRewardSets;
}
