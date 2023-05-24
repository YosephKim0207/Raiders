using System;
using System.Collections.Generic;

// Token: 0x0200148C RID: 5260
[Serializable]
public class EnemyReplacementTier
{
	// Token: 0x04007988 RID: 31112
	public GlobalDungeonData.ValidTilesets TargetTileset;

	// Token: 0x04007989 RID: 31113
	public float ChanceToReplace = 0.2f;

	// Token: 0x0400798A RID: 31114
	public int MinRoomWidth = -1;

	// Token: 0x0400798B RID: 31115
	[EnemyIdentifier]
	public List<string> TargetGUIDs;

	// Token: 0x0400798C RID: 31116
	[EnemyIdentifier]
	public List<string> ReplacementGUIDs;
}
