using System;

namespace FullInspector.Rotorz.ReorderableList
{
	// Token: 0x020005FE RID: 1534
	[Flags]
	public enum ReorderableListFlags
	{
		// Token: 0x040018F6 RID: 6390
		DisableReordering = 1,
		// Token: 0x040018F7 RID: 6391
		HideAddButton = 2,
		// Token: 0x040018F8 RID: 6392
		HideRemoveButtons = 4,
		// Token: 0x040018F9 RID: 6393
		DisableContextMenu = 8,
		// Token: 0x040018FA RID: 6394
		DisableDuplicateCommand = 16,
		// Token: 0x040018FB RID: 6395
		DisableAutoFocus = 32,
		// Token: 0x040018FC RID: 6396
		ShowIndices = 64,
		// Token: 0x040018FD RID: 6397
		DisableClipping = 128
	}
}
