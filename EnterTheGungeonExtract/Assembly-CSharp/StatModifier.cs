using System;
using UnityEngine;

// Token: 0x02000D0A RID: 3338
[Serializable]
public class StatModifier
{
	// Token: 0x0600466A RID: 18026 RVA: 0x0016D8AC File Offset: 0x0016BAAC
	public static StatModifier Create(PlayerStats.StatType targetStat, StatModifier.ModifyMethod method, float amt)
	{
		return new StatModifier
		{
			statToBoost = targetStat,
			amount = amt,
			modifyType = method
		};
	}

	// Token: 0x17000A54 RID: 2644
	// (get) Token: 0x0600466B RID: 18027 RVA: 0x0016D8D8 File Offset: 0x0016BAD8
	public bool PersistsOnCoopDeath
	{
		get
		{
			return this.statToBoost == PlayerStats.StatType.Curse && this.amount > 0f;
		}
	}

	// Token: 0x040038E3 RID: 14563
	public PlayerStats.StatType statToBoost;

	// Token: 0x040038E4 RID: 14564
	public StatModifier.ModifyMethod modifyType;

	// Token: 0x040038E5 RID: 14565
	public float amount;

	// Token: 0x040038E6 RID: 14566
	[NonSerialized]
	public bool hasBeenOwnerlessProcessed;

	// Token: 0x040038E7 RID: 14567
	[NonSerialized]
	public bool ignoredForSaveData;

	// Token: 0x040038E8 RID: 14568
	[HideInInspector]
	public bool isMeatBunBuff;

	// Token: 0x02000D0B RID: 3339
	public enum ModifyMethod
	{
		// Token: 0x040038EA RID: 14570
		ADDITIVE,
		// Token: 0x040038EB RID: 14571
		MULTIPLICATIVE
	}
}
