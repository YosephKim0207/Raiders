using System;
using System.Collections.Generic;

// Token: 0x02000E8A RID: 3722
[Serializable]
public abstract class StampDataBase
{
	// Token: 0x06004EEB RID: 20203 RVA: 0x001B5478 File Offset: 0x001B3678
	public float GetRelativeWeight(int roomSubType)
	{
		for (int i = 0; i < this.roomTypeData.Count; i++)
		{
			if (this.roomTypeData[i].roomSubType == roomSubType)
			{
				return this.roomTypeData[i].roomRelativeWeight;
			}
		}
		return this.relativeWeight;
	}

	// Token: 0x04004617 RID: 17943
	public int width = 1;

	// Token: 0x04004618 RID: 17944
	public int height = 1;

	// Token: 0x04004619 RID: 17945
	public float relativeWeight = 1f;

	// Token: 0x0400461A RID: 17946
	public DungeonTileStampData.StampPlacementRule placementRule;

	// Token: 0x0400461B RID: 17947
	public DungeonTileStampData.StampSpace occupySpace;

	// Token: 0x0400461C RID: 17948
	public DungeonTileStampData.StampCategory stampCategory;

	// Token: 0x0400461D RID: 17949
	public int preferredIntermediaryStamps;

	// Token: 0x0400461E RID: 17950
	public DungeonTileStampData.IntermediaryMatchingStyle intermediaryMatchingStyle;

	// Token: 0x0400461F RID: 17951
	public bool requiresForcedMatchingStyle;

	// Token: 0x04004620 RID: 17952
	public Opulence opulence;

	// Token: 0x04004621 RID: 17953
	public List<StampPerRoomPlacementSettings> roomTypeData;

	// Token: 0x04004622 RID: 17954
	public int indexOfSymmetricPartner = -1;

	// Token: 0x04004623 RID: 17955
	public bool preventRoomRepeats;
}
