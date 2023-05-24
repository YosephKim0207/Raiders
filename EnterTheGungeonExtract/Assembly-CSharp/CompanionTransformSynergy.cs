using System;

// Token: 0x02001383 RID: 4995
[Serializable]
public class CompanionTransformSynergy
{
	// Token: 0x040070D1 RID: 28881
	[LongNumericEnum]
	public CustomSynergyType RequiredSynergy;

	// Token: 0x040070D2 RID: 28882
	[EnemyIdentifier]
	public string SynergyCompanionGuid;
}
