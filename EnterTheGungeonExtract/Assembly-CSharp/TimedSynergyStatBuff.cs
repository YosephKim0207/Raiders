using System;

// Token: 0x02001401 RID: 5121
[Serializable]
public class TimedSynergyStatBuff
{
	// Token: 0x040075D8 RID: 30168
	[LongNumericEnum]
	public CustomSynergyType RequiredSynergy;

	// Token: 0x040075D9 RID: 30169
	public PlayerStats.StatType statToBoost;

	// Token: 0x040075DA RID: 30170
	public StatModifier.ModifyMethod modifyType;

	// Token: 0x040075DB RID: 30171
	public float amount;

	// Token: 0x040075DC RID: 30172
	public float duration;
}
