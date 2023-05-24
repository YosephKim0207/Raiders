using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000D09 RID: 3337
public class BasicStatPickup : PassiveItem
{
	// Token: 0x06004666 RID: 18022 RVA: 0x0016D5EC File Offset: 0x0016B7EC
	public override void Pickup(PlayerController player)
	{
		if (this.m_pickedUp)
		{
			return;
		}
		if (this.ArmorToGive > 0 && !this.m_pickedUpThisRun)
		{
			player.healthHaver.Armor += (float)this.ArmorToGive;
		}
		else if (!this.m_pickedUpThisRun && this.IsMasteryToken && player.characterIdentity == PlayableCharacters.Robot)
		{
			player.healthHaver.Armor += 1f;
		}
		if (this.ModifiesDodgeRoll)
		{
			player.rollStats.rollDistanceMultiplier *= this.DodgeRollDistanceMultiplier;
			player.rollStats.rollTimeMultiplier *= this.DodgeRollTimeMultiplier;
			player.rollStats.additionalInvulnerabilityFrames += this.AdditionalInvulnerabilityFrames;
		}
		if (!this.m_pickedUpThisRun && this.IsJunk && player.characterIdentity == PlayableCharacters.Robot)
		{
			StatModifier statModifier = new StatModifier();
			statModifier.statToBoost = PlayerStats.StatType.Damage;
			statModifier.amount = 0.05f;
			statModifier.modifyType = StatModifier.ModifyMethod.ADDITIVE;
			player.ownerlessStatModifiers.Add(statModifier);
			player.stats.RecalculateStats(player, false, false);
		}
		if (!this.m_pickedUpThisRun && this.GivesCurrency)
		{
			player.carriedConsumables.Currency += this.CurrencyToGive;
		}
		if (!this.m_pickedUpThisRun && player.characterIdentity == PlayableCharacters.Robot)
		{
			for (int i = 0; i < this.modifiers.Count; i++)
			{
				if (this.modifiers[i].statToBoost == PlayerStats.StatType.Health && this.modifiers[i].amount > 0f)
				{
					int num = Mathf.FloorToInt(this.modifiers[i].amount * (float)UnityEngine.Random.Range(GameManager.Instance.RewardManager.RobotMinCurrencyPerHealthItem, GameManager.Instance.RewardManager.RobotMaxCurrencyPerHealthItem + 1));
					LootEngine.SpawnCurrency(player.CenterPosition, num, false);
				}
			}
		}
		base.Pickup(player);
	}

	// Token: 0x06004667 RID: 18023 RVA: 0x0016D808 File Offset: 0x0016BA08
	public override DebrisObject Drop(PlayerController player)
	{
		DebrisObject debrisObject = base.Drop(player);
		if (this.ModifiesDodgeRoll)
		{
			player.rollStats.rollDistanceMultiplier /= this.DodgeRollDistanceMultiplier;
			player.rollStats.rollTimeMultiplier /= this.DodgeRollTimeMultiplier;
			player.rollStats.additionalInvulnerabilityFrames -= this.AdditionalInvulnerabilityFrames;
			player.rollStats.additionalInvulnerabilityFrames = Mathf.Max(player.rollStats.additionalInvulnerabilityFrames, 0);
		}
		debrisObject.GetComponent<BasicStatPickup>().m_pickedUpThisRun = true;
		return debrisObject;
	}

	// Token: 0x06004668 RID: 18024 RVA: 0x0016D89C File Offset: 0x0016BA9C
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x040038D9 RID: 14553
	[BetterList]
	public List<StatModifier> modifiers;

	// Token: 0x040038DA RID: 14554
	public int ArmorToGive;

	// Token: 0x040038DB RID: 14555
	public bool ModifiesDodgeRoll;

	// Token: 0x040038DC RID: 14556
	[ShowInInspectorIf("ModifiesDodgeRoll", false)]
	public float DodgeRollTimeMultiplier = 0.9f;

	// Token: 0x040038DD RID: 14557
	[ShowInInspectorIf("ModifiesDodgeRoll", false)]
	public float DodgeRollDistanceMultiplier = 1.25f;

	// Token: 0x040038DE RID: 14558
	[ShowInInspectorIf("ModifiesDodgeRoll", false)]
	public int AdditionalInvulnerabilityFrames;

	// Token: 0x040038DF RID: 14559
	public bool IsJunk;

	// Token: 0x040038E0 RID: 14560
	public bool GivesCurrency;

	// Token: 0x040038E1 RID: 14561
	public int CurrencyToGive;

	// Token: 0x040038E2 RID: 14562
	public bool IsMasteryToken;
}
