using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000E83 RID: 3715
[Serializable]
public class DungeonTileStampData : ScriptableObject
{
	// Token: 0x06004EE1 RID: 20193 RVA: 0x001B4924 File Offset: 0x001B2B24
	public bool ContainsTileIndex(int index)
	{
		if (this.stamps == null)
		{
			return false;
		}
		foreach (TileStampData tileStampData in this.stamps)
		{
			if (tileStampData.stampTileIndices.Contains(index))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06004EE2 RID: 20194 RVA: 0x001B4974 File Offset: 0x001B2B74
	protected bool IsValidRoomType(StampDataBase s, int roomType)
	{
		if (s.roomTypeData.Count == 0)
		{
			return true;
		}
		for (int i = 0; i < s.roomTypeData.Count; i++)
		{
			if (s.roomTypeData[i].roomSubType == roomType)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06004EE3 RID: 20195 RVA: 0x001B49CC File Offset: 0x001B2BCC
	public StampDataBase GetStampDataSimple(DungeonTileStampData.StampPlacementRule placement, Opulence oppan, int roomType, int maxWidth = 1000)
	{
		WeightedList<StampDataBase> weightedList = new WeightedList<StampDataBase>();
		for (int i = 0; i < this.stamps.Length; i++)
		{
			TileStampData tileStampData = this.stamps[i];
			if (!tileStampData.requiresForcedMatchingStyle)
			{
				if (placement == tileStampData.placementRule && this.IsValidRoomType(tileStampData, roomType) && tileStampData.width <= maxWidth)
				{
					weightedList.Add(tileStampData, tileStampData.GetRelativeWeight(roomType) * this.tileStampWeight);
				}
			}
		}
		for (int j = 0; j < this.spriteStamps.Length; j++)
		{
			SpriteStampData spriteStampData = this.spriteStamps[j];
			if (!spriteStampData.requiresForcedMatchingStyle)
			{
				if (placement == spriteStampData.placementRule && this.IsValidRoomType(spriteStampData, roomType) && spriteStampData.width <= maxWidth)
				{
					weightedList.Add(spriteStampData, spriteStampData.GetRelativeWeight(roomType) * this.spriteStampWeight);
				}
			}
		}
		for (int k = 0; k < this.objectStamps.Length; k++)
		{
			ObjectStampData objectStampData = this.objectStamps[k];
			if (!objectStampData.requiresForcedMatchingStyle)
			{
				if (placement == objectStampData.placementRule && this.IsValidRoomType(objectStampData, roomType) && objectStampData.width <= maxWidth)
				{
					weightedList.Add(objectStampData, objectStampData.GetRelativeWeight(roomType) * this.objectStampWeight);
				}
			}
		}
		if (weightedList.elements == null || weightedList.elements.Count == 0)
		{
			return null;
		}
		return weightedList.SelectByWeight();
	}

	// Token: 0x06004EE4 RID: 20196 RVA: 0x001B4B5C File Offset: 0x001B2D5C
	public StampDataBase AttemptGetSimilarStampForRoomDuplication(StampDataBase source, List<StampDataBase> excluded, int roomType)
	{
		WeightedList<StampDataBase> weightedList = new WeightedList<StampDataBase>();
		for (int i = 0; i < this.stamps.Length; i++)
		{
			StampDataBase stampDataBase = this.stamps[i];
			if (this.IsValidRoomType(stampDataBase, roomType) && source.stampCategory == stampDataBase.stampCategory && source.placementRule == stampDataBase.placementRule && source.width == stampDataBase.width && source.height == stampDataBase.height && source.occupySpace == stampDataBase.occupySpace && source.preventRoomRepeats == stampDataBase.preventRoomRepeats && !excluded.Contains(stampDataBase))
			{
				weightedList.Add(stampDataBase, stampDataBase.GetRelativeWeight(roomType));
			}
		}
		if (weightedList.elements == null || weightedList.elements.Count == 0)
		{
			return null;
		}
		return weightedList.SelectByWeight();
	}

	// Token: 0x06004EE5 RID: 20197 RVA: 0x001B4C44 File Offset: 0x001B2E44
	public StampDataBase GetStampDataSimple(List<DungeonTileStampData.StampPlacementRule> placements, Opulence oppan, int roomType, int maxWidth, bool excludeWallSpace, List<StampDataBase> excluded)
	{
		WeightedList<StampDataBase> weightedList = new WeightedList<StampDataBase>();
		for (int i = 0; i < this.stamps.Length; i++)
		{
			TileStampData tileStampData = this.stamps[i];
			if (!tileStampData.preventRoomRepeats || !excluded.Contains(tileStampData))
			{
				if (!tileStampData.requiresForcedMatchingStyle)
				{
					bool flag = tileStampData.placementRule == DungeonTileStampData.StampPlacementRule.ALONG_LEFT_WALLS || tileStampData.placementRule == DungeonTileStampData.StampPlacementRule.ALONG_RIGHT_WALLS;
					if (!excludeWallSpace || flag || tileStampData.height <= 1)
					{
						if (!excludeWallSpace || tileStampData.width <= 1)
						{
							if (!excludeWallSpace || tileStampData.occupySpace == DungeonTileStampData.StampSpace.OBJECT_SPACE)
							{
								if (placements.Contains(tileStampData.placementRule) && this.IsValidRoomType(tileStampData, roomType) && tileStampData.width <= maxWidth)
								{
									weightedList.Add(tileStampData, tileStampData.GetRelativeWeight(roomType) * this.tileStampWeight);
								}
							}
						}
					}
				}
			}
		}
		for (int j = 0; j < this.spriteStamps.Length; j++)
		{
			SpriteStampData spriteStampData = this.spriteStamps[j];
			if (!spriteStampData.preventRoomRepeats || !excluded.Contains(spriteStampData))
			{
				if (!excludeWallSpace || spriteStampData.height <= 1)
				{
					if (!excludeWallSpace || spriteStampData.width <= 1)
					{
						if (!spriteStampData.requiresForcedMatchingStyle)
						{
							if (!excludeWallSpace || spriteStampData.occupySpace == DungeonTileStampData.StampSpace.OBJECT_SPACE)
							{
								if (placements.Contains(spriteStampData.placementRule) && this.IsValidRoomType(spriteStampData, roomType) && spriteStampData.width <= maxWidth)
								{
									weightedList.Add(spriteStampData, spriteStampData.GetRelativeWeight(roomType) * this.spriteStampWeight);
								}
							}
						}
					}
				}
			}
		}
		for (int k = 0; k < this.objectStamps.Length; k++)
		{
			ObjectStampData objectStampData = this.objectStamps[k];
			if (!objectStampData.preventRoomRepeats || !excluded.Contains(objectStampData))
			{
				bool flag2 = objectStampData.placementRule == DungeonTileStampData.StampPlacementRule.ALONG_LEFT_WALLS || objectStampData.placementRule == DungeonTileStampData.StampPlacementRule.ALONG_RIGHT_WALLS;
				if (!excludeWallSpace || flag2 || objectStampData.height <= 1)
				{
					if (!excludeWallSpace || objectStampData.width <= 1)
					{
						if (!objectStampData.requiresForcedMatchingStyle)
						{
							if (!excludeWallSpace || objectStampData.occupySpace == DungeonTileStampData.StampSpace.OBJECT_SPACE)
							{
								if (placements.Contains(objectStampData.placementRule) && this.IsValidRoomType(objectStampData, roomType) && objectStampData.width <= maxWidth)
								{
									weightedList.Add(objectStampData, objectStampData.GetRelativeWeight(roomType) * this.objectStampWeight);
								}
							}
						}
					}
				}
			}
		}
		if (weightedList.elements == null || weightedList.elements.Count == 0)
		{
			return null;
		}
		return weightedList.SelectByWeight();
	}

	// Token: 0x06004EE6 RID: 20198 RVA: 0x001B4F60 File Offset: 0x001B3160
	public StampDataBase GetStampDataSimpleWithForcedRule(List<DungeonTileStampData.StampPlacementRule> placements, DungeonTileStampData.IntermediaryMatchingStyle forcedStyle, Opulence oppan, int roomType, int maxWidth = 1000, bool excludeWallSpace = false)
	{
		WeightedList<StampDataBase> weightedList = new WeightedList<StampDataBase>();
		for (int i = 0; i < this.stamps.Length; i++)
		{
			TileStampData tileStampData = this.stamps[i];
			bool flag = tileStampData.placementRule == DungeonTileStampData.StampPlacementRule.ALONG_LEFT_WALLS || tileStampData.placementRule == DungeonTileStampData.StampPlacementRule.ALONG_RIGHT_WALLS;
			if (!excludeWallSpace || flag || tileStampData.height <= 1)
			{
				if (!excludeWallSpace || tileStampData.width <= 1)
				{
					if (!excludeWallSpace || tileStampData.occupySpace == DungeonTileStampData.StampSpace.OBJECT_SPACE)
					{
						if (placements.Contains(tileStampData.placementRule) && this.IsValidRoomType(tileStampData, roomType) && tileStampData.width <= maxWidth && tileStampData.intermediaryMatchingStyle == forcedStyle)
						{
							weightedList.Add(tileStampData, tileStampData.GetRelativeWeight(roomType) * this.tileStampWeight);
						}
					}
				}
			}
		}
		for (int j = 0; j < this.spriteStamps.Length; j++)
		{
			SpriteStampData spriteStampData = this.spriteStamps[j];
			if (!excludeWallSpace || spriteStampData.height <= 1)
			{
				if (!excludeWallSpace || spriteStampData.width <= 1)
				{
					if (!excludeWallSpace || spriteStampData.occupySpace == DungeonTileStampData.StampSpace.OBJECT_SPACE)
					{
						if (placements.Contains(spriteStampData.placementRule) && this.IsValidRoomType(spriteStampData, roomType) && spriteStampData.width <= maxWidth && spriteStampData.intermediaryMatchingStyle == forcedStyle)
						{
							weightedList.Add(spriteStampData, spriteStampData.GetRelativeWeight(roomType) * this.spriteStampWeight);
						}
					}
				}
			}
		}
		for (int k = 0; k < this.objectStamps.Length; k++)
		{
			ObjectStampData objectStampData = this.objectStamps[k];
			bool flag2 = objectStampData.placementRule == DungeonTileStampData.StampPlacementRule.ALONG_LEFT_WALLS || objectStampData.placementRule == DungeonTileStampData.StampPlacementRule.ALONG_RIGHT_WALLS;
			if (!excludeWallSpace || flag2 || objectStampData.height <= 1)
			{
				if (!excludeWallSpace || objectStampData.width <= 1)
				{
					if (!excludeWallSpace || objectStampData.occupySpace == DungeonTileStampData.StampSpace.OBJECT_SPACE)
					{
						if (placements.Contains(objectStampData.placementRule) && this.IsValidRoomType(objectStampData, roomType) && objectStampData.width <= maxWidth && objectStampData.intermediaryMatchingStyle == forcedStyle)
						{
							weightedList.Add(objectStampData, objectStampData.GetRelativeWeight(roomType) * this.objectStampWeight);
						}
					}
				}
			}
		}
		if (weightedList.elements == null || weightedList.elements.Count == 0)
		{
			return null;
		}
		return weightedList.SelectByWeight();
	}

	// Token: 0x06004EE7 RID: 20199 RVA: 0x001B521C File Offset: 0x001B341C
	public StampDataBase GetStampDataComplex(DungeonTileStampData.StampPlacementRule placement, DungeonTileStampData.StampSpace space, DungeonTileStampData.StampCategory category, Opulence oppan, int roomType, int maxWidth = 1000)
	{
		return this.GetStampDataComplex(new List<DungeonTileStampData.StampPlacementRule> { placement }, space, category, oppan, roomType, maxWidth);
	}

	// Token: 0x06004EE8 RID: 20200 RVA: 0x001B5248 File Offset: 0x001B3448
	public StampDataBase GetStampDataComplex(List<DungeonTileStampData.StampPlacementRule> placements, DungeonTileStampData.StampSpace space, DungeonTileStampData.StampCategory category, Opulence oppan, int roomType, int maxWidth = 1000)
	{
		bool flag = placements.Contains(DungeonTileStampData.StampPlacementRule.ALONG_LEFT_WALLS) || placements.Contains(DungeonTileStampData.StampPlacementRule.ALONG_RIGHT_WALLS);
		WeightedList<StampDataBase> weightedList = new WeightedList<StampDataBase>();
		for (int i = 0; i < this.stamps.Length; i++)
		{
			TileStampData tileStampData = this.stamps[i];
			if (!tileStampData.requiresForcedMatchingStyle)
			{
				if (placements.Contains(tileStampData.placementRule) && tileStampData.occupySpace == space && this.IsValidRoomType(tileStampData, roomType) && ((!flag) ? tileStampData.width : tileStampData.height) <= maxWidth)
				{
					weightedList.Add(tileStampData, tileStampData.GetRelativeWeight(roomType) * this.tileStampWeight);
				}
			}
		}
		for (int j = 0; j < this.spriteStamps.Length; j++)
		{
			SpriteStampData spriteStampData = this.spriteStamps[j];
			if (!spriteStampData.requiresForcedMatchingStyle)
			{
				if (placements.Contains(spriteStampData.placementRule) && spriteStampData.occupySpace == space && this.IsValidRoomType(spriteStampData, roomType) && ((!flag) ? spriteStampData.width : spriteStampData.height) <= maxWidth)
				{
					weightedList.Add(spriteStampData, spriteStampData.GetRelativeWeight(roomType) * this.spriteStampWeight);
				}
			}
		}
		for (int k = 0; k < this.objectStamps.Length; k++)
		{
			ObjectStampData objectStampData = this.objectStamps[k];
			if (!objectStampData.requiresForcedMatchingStyle)
			{
				if (placements.Contains(objectStampData.placementRule) && objectStampData.occupySpace == space && this.IsValidRoomType(objectStampData, roomType) && ((!flag) ? objectStampData.width : objectStampData.height) <= maxWidth)
				{
					weightedList.Add(objectStampData, objectStampData.GetRelativeWeight(roomType) * this.objectStampWeight);
				}
			}
		}
		return weightedList.SelectByWeight();
	}

	// Token: 0x040045EA RID: 17898
	public float tileStampWeight = 1f;

	// Token: 0x040045EB RID: 17899
	public float spriteStampWeight;

	// Token: 0x040045EC RID: 17900
	public float objectStampWeight = 1f;

	// Token: 0x040045ED RID: 17901
	public TileStampData[] stamps;

	// Token: 0x040045EE RID: 17902
	public SpriteStampData[] spriteStamps;

	// Token: 0x040045EF RID: 17903
	public ObjectStampData[] objectStamps;

	// Token: 0x040045F0 RID: 17904
	public float SymmetricFrameChance = 0.5f;

	// Token: 0x040045F1 RID: 17905
	public float SymmetricCompleteChance = 0.25f;

	// Token: 0x02000E84 RID: 3716
	public enum StampPlacementRule
	{
		// Token: 0x040045F3 RID: 17907
		ON_LOWER_FACEWALL,
		// Token: 0x040045F4 RID: 17908
		ON_UPPER_FACEWALL,
		// Token: 0x040045F5 RID: 17909
		BELOW_LOWER_FACEWALL,
		// Token: 0x040045F6 RID: 17910
		ALONG_LEFT_WALLS,
		// Token: 0x040045F7 RID: 17911
		ON_TOPWALL,
		// Token: 0x040045F8 RID: 17912
		ON_ANY_FLOOR,
		// Token: 0x040045F9 RID: 17913
		ABOVE_UPPER_FACEWALL,
		// Token: 0x040045FA RID: 17914
		ON_ANY_CEILING,
		// Token: 0x040045FB RID: 17915
		ALONG_RIGHT_WALLS,
		// Token: 0x040045FC RID: 17916
		BELOW_LOWER_FACEWALL_LEFT_CORNER,
		// Token: 0x040045FD RID: 17917
		BELOW_LOWER_FACEWALL_RIGHT_CORNER
	}

	// Token: 0x02000E85 RID: 3717
	public enum StampSpace
	{
		// Token: 0x040045FF RID: 17919
		OBJECT_SPACE,
		// Token: 0x04004600 RID: 17920
		WALL_SPACE,
		// Token: 0x04004601 RID: 17921
		BOTH_SPACES
	}

	// Token: 0x02000E86 RID: 3718
	public enum StampCategory
	{
		// Token: 0x04004603 RID: 17923
		STRUCTURAL,
		// Token: 0x04004604 RID: 17924
		NATURAL,
		// Token: 0x04004605 RID: 17925
		MUNDANE,
		// Token: 0x04004606 RID: 17926
		DECORATIVE
	}

	// Token: 0x02000E87 RID: 3719
	public enum IntermediaryMatchingStyle
	{
		// Token: 0x04004608 RID: 17928
		ANY,
		// Token: 0x04004609 RID: 17929
		COLUMN,
		// Token: 0x0400460A RID: 17930
		WALL_HOLE,
		// Token: 0x0400460B RID: 17931
		BANNER,
		// Token: 0x0400460C RID: 17932
		PORTRAIT,
		// Token: 0x0400460D RID: 17933
		WALL_HOLE_FILLER,
		// Token: 0x0400460E RID: 17934
		SKELETON,
		// Token: 0x0400460F RID: 17935
		ROCK
	}
}
