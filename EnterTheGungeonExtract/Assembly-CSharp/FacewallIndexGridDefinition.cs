using System;
using UnityEngine;

// Token: 0x02000E73 RID: 3699
[Serializable]
public class FacewallIndexGridDefinition
{
	// Token: 0x0400456E RID: 17774
	public TileIndexGrid grid;

	// Token: 0x0400456F RID: 17775
	public int minWidth = 3;

	// Token: 0x04004570 RID: 17776
	public int maxWidth = 10;

	// Token: 0x04004571 RID: 17777
	[Header("Intermediary Tiles")]
	public bool hasIntermediaries;

	// Token: 0x04004572 RID: 17778
	public int minIntermediaryBuffer = 4;

	// Token: 0x04004573 RID: 17779
	public int maxIntermediaryBuffer = 20;

	// Token: 0x04004574 RID: 17780
	public int minIntermediaryLength = 1;

	// Token: 0x04004575 RID: 17781
	public int maxIntermediaryLength = 1;

	// Token: 0x04004576 RID: 17782
	[Header("Options")]
	public bool topsMatchBottoms;

	// Token: 0x04004577 RID: 17783
	public bool middleSectionSequential;

	// Token: 0x04004578 RID: 17784
	public bool canExistInCorners = true;

	// Token: 0x04004579 RID: 17785
	public bool forceEdgesInCorners;

	// Token: 0x0400457A RID: 17786
	public bool canAcceptWallDecoration;

	// Token: 0x0400457B RID: 17787
	public bool canAcceptFloorDecoration = true;

	// Token: 0x0400457C RID: 17788
	public DungeonTileStampData.IntermediaryMatchingStyle forcedStampMatchingStyle;

	// Token: 0x0400457D RID: 17789
	public bool canBePlacedInExits;

	// Token: 0x0400457E RID: 17790
	public float chanceToPlaceIfPossible = 0.1f;

	// Token: 0x0400457F RID: 17791
	public float perTileFailureRate = 0.05f;
}
