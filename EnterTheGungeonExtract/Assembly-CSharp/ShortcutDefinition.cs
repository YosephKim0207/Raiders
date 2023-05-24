using System;

// Token: 0x02001206 RID: 4614
[Serializable]
public struct ShortcutDefinition
{
	// Token: 0x04006304 RID: 25348
	public string targetLevelName;

	// Token: 0x04006305 RID: 25349
	[LongEnum]
	public GungeonFlags requiredFlag;

	// Token: 0x04006306 RID: 25350
	public string sherpaTextKey;

	// Token: 0x04006307 RID: 25351
	public string elevatorFloorSpriteName;

	// Token: 0x04006308 RID: 25352
	public bool IsBossRush;

	// Token: 0x04006309 RID: 25353
	public bool IsSuperBossRush;
}
