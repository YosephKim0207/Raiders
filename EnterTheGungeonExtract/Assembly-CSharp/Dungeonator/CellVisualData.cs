using System;
using UnityEngine;

namespace Dungeonator
{
	// Token: 0x02000EAB RID: 3755
	public struct CellVisualData
	{
		// Token: 0x06004F86 RID: 20358 RVA: 0x001B9B2C File Offset: 0x001B7D2C
		public void CopyFrom(CellVisualData other)
		{
			this.roomVisualTypeIndex = other.roomVisualTypeIndex;
			this.isDecal = other.isDecal;
			this.isPattern = other.isPattern;
			this.IsPhantomCarpet = other.IsPhantomCarpet;
			this.floorType = other.floorType;
			this.precludeAllTileDrawing = other.precludeAllTileDrawing;
			this.shouldIgnoreWallDrawing = other.shouldIgnoreWallDrawing;
			this.shouldIgnoreBorders = other.shouldIgnoreBorders;
			this.floorTileOverridden = other.floorTileOverridden;
			this.preventFloorStamping = other.preventFloorStamping;
			this.faceWallOverrideIndex = other.faceWallOverrideIndex;
			this.pitOverrideIndex = other.pitOverrideIndex;
			this.inheritedOverrideIndex = other.inheritedOverrideIndex;
			this.inheritedOverrideIndexIsFloor = other.inheritedOverrideIndexIsFloor;
			this.IsFeatureCell = other.IsFeatureCell;
			this.IsFeatureAdditional = other.IsFeatureAdditional;
			this.UsesCustomIndexOverride01 = other.UsesCustomIndexOverride01;
			this.CustomIndexOverride01Layer = other.CustomIndexOverride01Layer;
			this.CustomIndexOverride01 = other.CustomIndexOverride01;
			this.RatChunkBorderIndex = other.RatChunkBorderIndex;
		}

		// Token: 0x040046ED RID: 18157
		public int roomVisualTypeIndex;

		// Token: 0x040046EE RID: 18158
		public bool isDecal;

		// Token: 0x040046EF RID: 18159
		public bool isPattern;

		// Token: 0x040046F0 RID: 18160
		public bool IsChannel;

		// Token: 0x040046F1 RID: 18161
		public bool IsPhantomCarpet;

		// Token: 0x040046F2 RID: 18162
		public CellVisualData.CellFloorType floorType;

		// Token: 0x040046F3 RID: 18163
		public bool absorbsDebris;

		// Token: 0x040046F4 RID: 18164
		public bool facewallGridPreventsWallSpaceStamp;

		// Token: 0x040046F5 RID: 18165
		public bool containsWallSpaceStamp;

		// Token: 0x040046F6 RID: 18166
		public bool containsObjectSpaceStamp;

		// Token: 0x040046F7 RID: 18167
		public DungeonTileStampData.IntermediaryMatchingStyle forcedMatchingStyle;

		// Token: 0x040046F8 RID: 18168
		public bool precludeAllTileDrawing;

		// Token: 0x040046F9 RID: 18169
		public bool shouldIgnoreWallDrawing;

		// Token: 0x040046FA RID: 18170
		public bool shouldIgnoreBorders;

		// Token: 0x040046FB RID: 18171
		public bool hasAlreadyBeenTilemapped;

		// Token: 0x040046FC RID: 18172
		public bool hasBeenLit;

		// Token: 0x040046FD RID: 18173
		public bool floorTileOverridden;

		// Token: 0x040046FE RID: 18174
		public bool preventFloorStamping;

		// Token: 0x040046FF RID: 18175
		public int doorFeetOverrideMode;

		// Token: 0x04004700 RID: 18176
		public bool containsLight;

		// Token: 0x04004701 RID: 18177
		public GameObject lightObject;

		// Token: 0x04004702 RID: 18178
		public LightStampData facewallLightStampData;

		// Token: 0x04004703 RID: 18179
		public LightStampData sidewallLightStampData;

		// Token: 0x04004704 RID: 18180
		public DungeonData.Direction lightDirection;

		// Token: 0x04004705 RID: 18181
		public int distanceToNearestLight;

		// Token: 0x04004706 RID: 18182
		public int faceWallOverrideIndex;

		// Token: 0x04004707 RID: 18183
		public int pitOverrideIndex;

		// Token: 0x04004708 RID: 18184
		public int inheritedOverrideIndex;

		// Token: 0x04004709 RID: 18185
		public bool inheritedOverrideIndexIsFloor;

		// Token: 0x0400470A RID: 18186
		public bool ceilingHasBeenProcessed;

		// Token: 0x0400470B RID: 18187
		public bool occlusionHasBeenProcessed;

		// Token: 0x0400470C RID: 18188
		public bool hasStampedPath;

		// Token: 0x0400470D RID: 18189
		public int pathTilesetGridIndex;

		// Token: 0x0400470E RID: 18190
		public bool IsFacewallForInteriorTransition;

		// Token: 0x0400470F RID: 18191
		public int InteriorTransitionIndex;

		// Token: 0x04004710 RID: 18192
		public bool IsFeatureCell;

		// Token: 0x04004711 RID: 18193
		public bool IsFeatureAdditional;

		// Token: 0x04004712 RID: 18194
		public bool UsesCustomIndexOverride01;

		// Token: 0x04004713 RID: 18195
		public int CustomIndexOverride01Layer;

		// Token: 0x04004714 RID: 18196
		public int CustomIndexOverride01;

		// Token: 0x04004715 RID: 18197
		public bool RequiresPitBordering;

		// Token: 0x04004716 RID: 18198
		public bool HasTriggeredPitVFX;

		// Token: 0x04004717 RID: 18199
		public float PitVFXCooldown;

		// Token: 0x04004718 RID: 18200
		public float PitParticleCooldown;

		// Token: 0x04004719 RID: 18201
		public int RatChunkBorderIndex;

		// Token: 0x02000EAC RID: 3756
		public enum CellFloorType
		{
			// Token: 0x0400471B RID: 18203
			Stone,
			// Token: 0x0400471C RID: 18204
			Water,
			// Token: 0x0400471D RID: 18205
			Carpet,
			// Token: 0x0400471E RID: 18206
			Ice,
			// Token: 0x0400471F RID: 18207
			Grass,
			// Token: 0x04004720 RID: 18208
			Bone,
			// Token: 0x04004721 RID: 18209
			Flesh,
			// Token: 0x04004722 RID: 18210
			ThickGoop
		}
	}
}
