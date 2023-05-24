using System;
using System.Collections.Generic;
using FullSerializer;

// Token: 0x02001566 RID: 5478
public class MidGamePlayerData
{
	// Token: 0x06007D83 RID: 32131 RVA: 0x0032CD88 File Offset: 0x0032AF88
	public MidGamePlayerData(PlayerController p)
	{
		this.CharacterIdentity = p.characterIdentity;
		this.CostumeID = ((!p.IsUsingAlternateCostume) ? 0 : 1);
		this.MasteryTokensCollected = p.MasteryTokensCollectedThisRun;
		this.CharacterUsesRandomGuns = p.CharacterUsesRandomGuns;
		this.ChallengeMode = ChallengeManager.ChallengeModeType;
		this.HasTakenDamageThisRun = p.HasTakenDamageThisRun;
		this.HasFiredNonStartingGun = p.HasFiredNonStartingGun;
		this.HasBloodthirst = p.GetComponent<Bloodthirst>();
		this.IsTemporaryEeveeForUnlock = p.IsTemporaryEeveeForUnlock;
		this.CurrentHealth = p.healthHaver.GetCurrentHealth();
		this.CurrentArmor = p.healthHaver.Armor;
		this.CurrentKeys = p.carriedConsumables.KeyBullets;
		this.CurrentCurrency = p.carriedConsumables.Currency;
		this.CurrentBlanks = p.Blanks;
		this.guns = new List<MidGameGunData>();
		if (p.inventory != null && p.inventory.AllGuns != null)
		{
			for (int i = 0; i < p.inventory.AllGuns.Count; i++)
			{
				if (!p.inventory.AllGuns[i].PreventSaveSerialization)
				{
					this.guns.Add(new MidGameGunData(p.inventory.AllGuns[i]));
				}
			}
		}
		this.activeItems = new List<MidGameActiveItemData>();
		if (p.activeItems != null)
		{
			for (int j = 0; j < p.activeItems.Count; j++)
			{
				if (!p.activeItems[j].PreventSaveSerialization)
				{
					this.activeItems.Add(new MidGameActiveItemData(p.activeItems[j]));
				}
			}
		}
		this.passiveItems = new List<MidGamePassiveItemData>();
		if (p.passiveItems != null)
		{
			for (int k = 0; k < p.passiveItems.Count; k++)
			{
				if (!p.passiveItems[k].PreventSaveSerialization)
				{
					this.passiveItems.Add(new MidGamePassiveItemData(p.passiveItems[k]));
				}
			}
		}
		this.ownerlessStatModifiers = new List<StatModifier>();
		if (p.ownerlessStatModifiers != null)
		{
			for (int l = 0; l < p.ownerlessStatModifiers.Count; l++)
			{
				if (!p.ownerlessStatModifiers[l].ignoredForSaveData)
				{
					this.ownerlessStatModifiers.Add(p.ownerlessStatModifiers[l]);
				}
			}
		}
	}

	// Token: 0x04008090 RID: 32912
	[fsProperty]
	public PlayableCharacters CharacterIdentity;

	// Token: 0x04008091 RID: 32913
	[fsProperty]
	public float CurrentHealth = 1f;

	// Token: 0x04008092 RID: 32914
	[fsProperty]
	public float CurrentArmor;

	// Token: 0x04008093 RID: 32915
	[fsProperty]
	public int CurrentKeys;

	// Token: 0x04008094 RID: 32916
	[fsProperty]
	public int CurrentCurrency;

	// Token: 0x04008095 RID: 32917
	[fsProperty]
	public int CurrentBlanks;

	// Token: 0x04008096 RID: 32918
	[fsProperty]
	public List<MidGameGunData> guns;

	// Token: 0x04008097 RID: 32919
	[fsProperty]
	public List<MidGameActiveItemData> activeItems;

	// Token: 0x04008098 RID: 32920
	[fsProperty]
	public List<MidGamePassiveItemData> passiveItems;

	// Token: 0x04008099 RID: 32921
	[fsProperty]
	public List<StatModifier> ownerlessStatModifiers;

	// Token: 0x0400809A RID: 32922
	[fsProperty]
	public int CostumeID;

	// Token: 0x0400809B RID: 32923
	[fsProperty]
	public int MasteryTokensCollected;

	// Token: 0x0400809C RID: 32924
	[fsProperty]
	public bool CharacterUsesRandomGuns;

	// Token: 0x0400809D RID: 32925
	[fsProperty]
	public ChallengeModeType ChallengeMode;

	// Token: 0x0400809E RID: 32926
	[fsProperty]
	public bool HasTakenDamageThisRun;

	// Token: 0x0400809F RID: 32927
	[fsProperty]
	public bool HasFiredNonStartingGun;

	// Token: 0x040080A0 RID: 32928
	[fsProperty]
	public bool HasBloodthirst;

	// Token: 0x040080A1 RID: 32929
	[fsProperty]
	public bool IsTemporaryEeveeForUnlock;
}
