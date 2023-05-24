using System;

// Token: 0x020016D8 RID: 5848
[Serializable]
public class AlphabetSoupEntry
{
	// Token: 0x04008D45 RID: 36165
	[LongNumericEnum]
	public CustomSynergyType RequiredSynergy;

	// Token: 0x04008D46 RID: 36166
	public string[] Words;

	// Token: 0x04008D47 RID: 36167
	public string[] AudioEvents;

	// Token: 0x04008D48 RID: 36168
	public Projectile BaseProjectile;
}
