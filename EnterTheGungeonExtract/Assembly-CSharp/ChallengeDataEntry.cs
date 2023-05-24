using System;
using System.Collections.Generic;

// Token: 0x02001269 RID: 4713
[Serializable]
public class ChallengeDataEntry
{
	// Token: 0x0600699F RID: 27039 RVA: 0x002965A4 File Offset: 0x002947A4
	public int GetWeightForFloor(GlobalDungeonData.ValidTilesets tileset)
	{
		if (this.tilesetsWithCustomValues.Contains(tileset))
		{
			return this.CustomValues[this.tilesetsWithCustomValues.IndexOf(tileset)];
		}
		return 1;
	}

	// Token: 0x04006607 RID: 26119
	public string Annotation;

	// Token: 0x04006608 RID: 26120
	public ChallengeModifier challenge;

	// Token: 0x04006609 RID: 26121
	[EnumFlags]
	public GlobalDungeonData.ValidTilesets excludedTilesets;

	// Token: 0x0400660A RID: 26122
	public List<GlobalDungeonData.ValidTilesets> tilesetsWithCustomValues;

	// Token: 0x0400660B RID: 26123
	public List<int> CustomValues;
}
