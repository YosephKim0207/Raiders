using System;

// Token: 0x02000389 RID: 905
public interface IDFVirtualScrollingTile
{
	// Token: 0x1700035F RID: 863
	// (get) Token: 0x06000F7E RID: 3966
	// (set) Token: 0x06000F7F RID: 3967
	int VirtualScrollItemIndex { get; set; }

	// Token: 0x06000F80 RID: 3968
	void OnScrollPanelItemVirtualize(object backingListItem);

	// Token: 0x06000F81 RID: 3969
	dfPanel GetDfPanel();
}
