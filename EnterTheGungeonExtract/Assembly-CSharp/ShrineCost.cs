using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020010E2 RID: 4322
[Serializable]
public class ShrineCost
{
	// Token: 0x06005F37 RID: 24375 RVA: 0x00249F20 File Offset: 0x00248120
	public bool CheckCost(PlayerController interactor)
	{
		switch (this.costType)
		{
		case ShrineCost.CostType.MONEY:
			return interactor.carriedConsumables.Currency >= this.cost;
		case ShrineCost.CostType.HEALTH:
			if (this.AllowsArmorConversionForRobot && interactor.characterIdentity == PlayableCharacters.Robot)
			{
				return interactor.healthHaver.Armor > (float)(this.cost * 2);
			}
			return interactor.healthHaver.GetCurrentHealth() > (float)this.cost;
		case ShrineCost.CostType.ARMOR:
			return interactor.healthHaver.Armor >= (float)this.cost;
		case ShrineCost.CostType.BLANK:
			return interactor.Blanks >= this.cost;
		case ShrineCost.CostType.KEY:
			return interactor.carriedConsumables.InfiniteKeys || interactor.carriedConsumables.KeyBullets >= this.cost;
		case ShrineCost.CostType.CURRENT_GUN:
			return interactor.CurrentGun != null && interactor.CurrentGun.CanActuallyBeDropped(interactor) && !interactor.CurrentGun.InfiniteAmmo;
		case ShrineCost.CostType.BEATEN_GAME:
			return !GameStatsManager.Instance.HasPast(GameManager.Instance.PrimaryPlayer.characterIdentity) || GameStatsManager.Instance.GetCharacterSpecificFlag(GameManager.Instance.PrimaryPlayer.characterIdentity, CharacterSpecificGungeonFlags.KILLED_PAST);
		case ShrineCost.CostType.STATS:
			return (interactor.characterIdentity == PlayableCharacters.Robot && this.AllowsArmorConversionForRobot && this.statMods[0].statToBoost == PlayerStats.StatType.Health && this.statMods[0].amount * -2f < interactor.healthHaver.Armor) || this.statMods[0].statToBoost != PlayerStats.StatType.Health || this.statMods[0].amount * -1f < interactor.healthHaver.GetMaxHealth();
		case ShrineCost.CostType.MONEY_PER_CURSE:
			return interactor.carriedConsumables.Currency >= this.cost * PlayerStats.GetTotalCurse();
		case ShrineCost.CostType.SPECIFIC_ITEM:
		{
			bool flag = false;
			for (int i = 0; i < interactor.passiveItems.Count; i++)
			{
				if (interactor.passiveItems[i].PickupObjectId == this.targetItemID)
				{
					flag = true;
				}
			}
			return flag;
		}
		default:
			return false;
		}
	}

	// Token: 0x06005F38 RID: 24376 RVA: 0x0024A16C File Offset: 0x0024836C
	public void ApplyCost(PlayerController interactor)
	{
		switch (this.costType)
		{
		case ShrineCost.CostType.MONEY:
			interactor.carriedConsumables.Currency -= this.cost;
			break;
		case ShrineCost.CostType.HEALTH:
			if (this.AllowsArmorConversionForRobot && interactor.characterIdentity == PlayableCharacters.Robot)
			{
				interactor.healthHaver.Armor = interactor.healthHaver.Armor - (float)(this.cost * 2);
				return;
			}
			interactor.healthHaver.NextDamageIgnoresArmor = true;
			interactor.healthHaver.ApplyDamage((float)this.cost, Vector2.zero, StringTableManager.GetEnemiesString("#SHRINE", -1), CoreDamageTypes.None, DamageCategory.Environment, true, null, false);
			break;
		case ShrineCost.CostType.ARMOR:
			interactor.healthHaver.Armor -= (float)this.cost;
			break;
		case ShrineCost.CostType.BLANK:
			interactor.Blanks -= this.cost;
			break;
		case ShrineCost.CostType.KEY:
			if (interactor.carriedConsumables.InfiniteKeys)
			{
				return;
			}
			interactor.carriedConsumables.KeyBullets -= this.cost;
			break;
		case ShrineCost.CostType.CURRENT_GUN:
			interactor.inventory.DestroyCurrentGun();
			break;
		case ShrineCost.CostType.STATS:
		{
			for (int i = 0; i < this.statMods.Length; i++)
			{
				if (interactor.ownerlessStatModifiers == null)
				{
					interactor.ownerlessStatModifiers = new List<StatModifier>();
				}
				interactor.ownerlessStatModifiers.Add(this.statMods[i]);
			}
			if (interactor.characterIdentity == PlayableCharacters.Robot && this.AllowsArmorConversionForRobot && this.statMods[0].statToBoost == PlayerStats.StatType.Health && this.statMods[0].amount * -2f < interactor.healthHaver.Armor)
			{
				interactor.healthHaver.Armor = interactor.healthHaver.Armor - this.statMods[0].amount * -2f;
			}
			interactor.stats.RecalculateStats(interactor, false, false);
			break;
		}
		case ShrineCost.CostType.MONEY_PER_CURSE:
			interactor.carriedConsumables.Currency -= Mathf.FloorToInt((float)(this.cost * PlayerStats.GetTotalCurse()));
			break;
		case ShrineCost.CostType.SPECIFIC_ITEM:
			interactor.RemovePassiveItem(this.targetItemID);
			break;
		}
	}

	// Token: 0x04005991 RID: 22929
	public ShrineCost.CostType costType;

	// Token: 0x04005992 RID: 22930
	public int cost;

	// Token: 0x04005993 RID: 22931
	public bool AllowsArmorConversionForRobot;

	// Token: 0x04005994 RID: 22932
	public StatModifier[] statMods;

	// Token: 0x04005995 RID: 22933
	public string rngString;

	// Token: 0x04005996 RID: 22934
	public float rngWeight = 1f;

	// Token: 0x04005997 RID: 22935
	[PickupIdentifier]
	public int targetItemID;

	// Token: 0x020010E3 RID: 4323
	public enum CostType
	{
		// Token: 0x04005999 RID: 22937
		MONEY,
		// Token: 0x0400599A RID: 22938
		HEALTH,
		// Token: 0x0400599B RID: 22939
		ARMOR,
		// Token: 0x0400599C RID: 22940
		BLANK,
		// Token: 0x0400599D RID: 22941
		KEY,
		// Token: 0x0400599E RID: 22942
		CURRENT_GUN,
		// Token: 0x0400599F RID: 22943
		BEATEN_GAME,
		// Token: 0x040059A0 RID: 22944
		STATS,
		// Token: 0x040059A1 RID: 22945
		MONEY_PER_CURSE,
		// Token: 0x040059A2 RID: 22946
		SPECIFIC_ITEM
	}
}
