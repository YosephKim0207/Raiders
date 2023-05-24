using System;

namespace Dungeonator
{
	// Token: 0x02000EAD RID: 3757
	public struct CellOcclusionData
	{
		// Token: 0x06004F87 RID: 20359 RVA: 0x001B9C40 File Offset: 0x001B7E40
		public CellOcclusionData(CellData cell)
		{
			this.cellOcclusion = 1f;
			this.minCellOccluionHistory = 1f;
			this.remainingDelay = 0f;
			this.occlusionParentDefintion = null;
			this.cellRoomVisiblityCount = 0;
			this.cellRoomVisitedCount = 0;
			this.cellVisibleTargetOcclusion = 0f;
			this.cellVisitedTargetOcclusion = 0.7f;
			this.cellOcclusionDirty = false;
			this.sharedRoomAndExitCell = false;
			this.overrideOcclusion = false;
		}

		// Token: 0x04004723 RID: 18211
		public float cellOcclusion;

		// Token: 0x04004724 RID: 18212
		public float minCellOccluionHistory;

		// Token: 0x04004725 RID: 18213
		public RuntimeExitDefinition occlusionParentDefintion;

		// Token: 0x04004726 RID: 18214
		public int cellRoomVisiblityCount;

		// Token: 0x04004727 RID: 18215
		public int cellRoomVisitedCount;

		// Token: 0x04004728 RID: 18216
		public float cellVisibleTargetOcclusion;

		// Token: 0x04004729 RID: 18217
		public float cellVisitedTargetOcclusion;

		// Token: 0x0400472A RID: 18218
		public float remainingDelay;

		// Token: 0x0400472B RID: 18219
		public bool cellOcclusionDirty;

		// Token: 0x0400472C RID: 18220
		public bool sharedRoomAndExitCell;

		// Token: 0x0400472D RID: 18221
		public bool overrideOcclusion;
	}
}
