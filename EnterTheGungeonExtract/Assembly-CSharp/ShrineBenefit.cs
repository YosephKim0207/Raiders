using System;
using System.Collections.Generic;
using Dungeonator;
using UnityEngine;

// Token: 0x020010E0 RID: 4320
[Serializable]
public class ShrineBenefit
{
	// Token: 0x06005F35 RID: 24373 RVA: 0x00249A24 File Offset: 0x00247C24
	public void ApplyBenefit(PlayerController interactor)
	{
		int num = Mathf.RoundToInt(this.amount);
		switch (this.benefitType)
		{
		case ShrineBenefit.BenefitType.MONEY:
			interactor.carriedConsumables.Currency += num;
			break;
		case ShrineBenefit.BenefitType.HEALTH:
			if (interactor.healthHaver.GetCurrentHealthPercentage() >= 1f)
			{
				interactor.Blanks++;
			}
			else
			{
				interactor.healthHaver.ApplyHealing(this.amount);
			}
			break;
		case ShrineBenefit.BenefitType.ARMOR:
			interactor.healthHaver.Armor += this.amount;
			break;
		case ShrineBenefit.BenefitType.BLANK:
			interactor.Blanks += num;
			break;
		case ShrineBenefit.BenefitType.KEY:
			interactor.carriedConsumables.KeyBullets += num;
			break;
		case ShrineBenefit.BenefitType.AMMO_PERCENTAGE:
			interactor.ResetTarnisherClipCapacity();
			if (this.appliesToAllGuns)
			{
				for (int i = 0; i < interactor.inventory.AllGuns.Count; i++)
				{
					if (interactor.inventory.AllGuns[i].CanGainAmmo)
					{
						int num2 = Mathf.FloorToInt((float)interactor.inventory.AllGuns[i].AdjustedMaxAmmo * this.amount);
						if (num2 <= 0)
						{
							AkSoundEngine.PostEvent("Play_OBJ_ammo_pickup_01", interactor.gameObject);
							num2 = Mathf.FloorToInt((float)interactor.inventory.AllGuns[i].ammo * this.amount);
						}
						if (num2 <= 0)
						{
							Debug.LogError("Shrine is attempting to give negative ammo!");
							num2 = 1;
						}
						interactor.inventory.AllGuns[i].GainAmmo(num2);
					}
				}
			}
			else if (interactor.inventory.CurrentGun != null && interactor.inventory.CurrentGun.CanGainAmmo)
			{
				int num3 = Mathf.FloorToInt((float)interactor.inventory.CurrentGun.AdjustedMaxAmmo * this.amount);
				if (num3 <= 0)
				{
					AkSoundEngine.PostEvent("Play_OBJ_ammo_pickup_01", interactor.gameObject);
					num3 = Mathf.FloorToInt((float)interactor.inventory.CurrentGun.ammo * this.amount);
				}
				if (num3 <= 0)
				{
					Debug.LogError("Shrine is attempting to give negative ammo!");
					num3 = 1;
				}
				interactor.inventory.CurrentGun.GainAmmo(num3);
			}
			break;
		case ShrineBenefit.BenefitType.STATS:
		{
			for (int j = 0; j < this.statMods.Length; j++)
			{
				if (interactor.ownerlessStatModifiers == null)
				{
					interactor.ownerlessStatModifiers = new List<StatModifier>();
				}
				interactor.ownerlessStatModifiers.Add(this.statMods[j]);
			}
			interactor.stats.RecalculateStats(interactor, false, false);
			break;
		}
		case ShrineBenefit.BenefitType.CLEANSE_CURSE:
		{
			StatModifier statModifier = new StatModifier();
			statModifier.amount = Mathf.Min(this.amount, (float)(PlayerStats.GetTotalCurse() * -1));
			statModifier.modifyType = StatModifier.ModifyMethod.ADDITIVE;
			statModifier.statToBoost = PlayerStats.StatType.Curse;
			interactor.ownerlessStatModifiers.Add(statModifier);
			interactor.stats.RecalculateStats(interactor, false, false);
			break;
		}
		case ShrineBenefit.BenefitType.SPAWN_CHEST:
		{
			IntVector2 intVector = interactor.CurrentRoom.GetBestRewardLocation(new IntVector2(2, 3), RoomHandler.RewardLocationStyle.CameraCenter, true) + new IntVector2(0, 2);
			if (this.IsRNGChest)
			{
				Chest chest = GameManager.Instance.RewardManager.SpawnTotallyRandomChest(intVector);
				if (chest != null)
				{
					chest.RegisterChestOnMinimap(interactor.CurrentRoom);
				}
			}
			else
			{
				Chest chest2 = GameManager.Instance.RewardManager.SpawnRewardChestAt(intVector, -1f, PickupObject.ItemQuality.EXCLUDED);
				if (chest2 != null)
				{
					chest2.RegisterChestOnMinimap(interactor.CurrentRoom);
				}
			}
			break;
		}
		case ShrineBenefit.BenefitType.SPECIFIC_ITEM:
			LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(this.targetItemID).gameObject, interactor);
			break;
		case ShrineBenefit.BenefitType.COMPANION:
		{
			GameStatsManager.Instance.RegisterStatChange(TrackedStats.TIMES_COMPANION_SHRINED, 1f);
			CompanionItem companionItem = LootEngine.GetItemOfTypeAndQuality<CompanionItem>(PickupObject.ItemQuality.A, GameManager.Instance.RewardManager.ItemsLootTable, true);
			if (GameStatsManager.Instance.GetPlayerStatValue(TrackedStats.TIMES_COMPANION_SHRINED) >= 2f)
			{
				GameStatsManager.Instance.SetFlag(GungeonFlags.ITEMSPECIFIC_TURKEY, true);
				if (GameStatsManager.Instance.GetPlayerStatValue(TrackedStats.TIMES_COMPANION_SHRINED) == 2f)
				{
					companionItem = PickupObjectDatabase.GetById(this.TurkeyCompanionForCompanionShrine) as CompanionItem;
				}
			}
			if (GameStatsManager.Instance.IsRainbowRun)
			{
				LootEngine.SpawnBowlerNote(GameManager.Instance.RewardManager.BowlerNoteOtherSource, interactor.transform.position + new Vector3(0f, -0.5f, 0f), interactor.CurrentRoom, true);
			}
			else if (companionItem)
			{
				LootEngine.GivePrefabToPlayer(companionItem.gameObject, interactor);
			}
			break;
		}
		case ShrineBenefit.BenefitType.BLOODTHIRST:
			interactor.gameObject.GetOrAddComponent<Bloodthirst>();
			break;
		}
	}

	// Token: 0x0400597B RID: 22907
	public ShrineBenefit.BenefitType benefitType;

	// Token: 0x0400597C RID: 22908
	public float amount;

	// Token: 0x0400597D RID: 22909
	[ShowInInspectorIf("benefitType", 5, false)]
	public bool appliesToAllGuns;

	// Token: 0x0400597E RID: 22910
	public StatModifier[] statMods;

	// Token: 0x0400597F RID: 22911
	public string rngString;

	// Token: 0x04005980 RID: 22912
	public float rngWeight = 1f;

	// Token: 0x04005981 RID: 22913
	[PickupIdentifier]
	public int targetItemID;

	// Token: 0x04005982 RID: 22914
	[PickupIdentifier]
	public int TurkeyCompanionForCompanionShrine;

	// Token: 0x04005983 RID: 22915
	[NonSerialized]
	public bool IsRNGChest;

	// Token: 0x020010E1 RID: 4321
	public enum BenefitType
	{
		// Token: 0x04005985 RID: 22917
		MONEY,
		// Token: 0x04005986 RID: 22918
		HEALTH,
		// Token: 0x04005987 RID: 22919
		ARMOR,
		// Token: 0x04005988 RID: 22920
		BLANK,
		// Token: 0x04005989 RID: 22921
		KEY,
		// Token: 0x0400598A RID: 22922
		AMMO_PERCENTAGE,
		// Token: 0x0400598B RID: 22923
		STATS,
		// Token: 0x0400598C RID: 22924
		CLEANSE_CURSE,
		// Token: 0x0400598D RID: 22925
		SPAWN_CHEST,
		// Token: 0x0400598E RID: 22926
		SPECIFIC_ITEM,
		// Token: 0x0400598F RID: 22927
		COMPANION,
		// Token: 0x04005990 RID: 22928
		BLOODTHIRST
	}
}
