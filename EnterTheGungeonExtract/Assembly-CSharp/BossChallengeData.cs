using System;

// Token: 0x0200126A RID: 4714
[Serializable]
public class BossChallengeData
{
	// Token: 0x0400660C RID: 26124
	public string Annotation;

	// Token: 0x0400660D RID: 26125
	[EnemyIdentifier]
	public string[] BossGuids;

	// Token: 0x0400660E RID: 26126
	public int NumToSelect;

	// Token: 0x0400660F RID: 26127
	public ChallengeModifier[] Modifiers;
}
