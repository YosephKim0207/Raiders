using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000416 RID: 1046
public class dfVirtualScrollData<T>
{
	// Token: 0x060017C2 RID: 6082 RVA: 0x000718F0 File Offset: 0x0006FAF0
	public void GetNewLimits(bool isVerticalFlow, bool getMaxes, out int index, out float newY)
	{
		IDFVirtualScrollingTile idfvirtualScrollingTile = this.Tiles[0];
		index = idfvirtualScrollingTile.VirtualScrollItemIndex;
		newY = ((!isVerticalFlow) ? idfvirtualScrollingTile.GetDfPanel().RelativePosition.x : idfvirtualScrollingTile.GetDfPanel().RelativePosition.y);
		foreach (IDFVirtualScrollingTile idfvirtualScrollingTile2 in this.Tiles)
		{
			dfPanel dfPanel = idfvirtualScrollingTile2.GetDfPanel();
			float num = ((!isVerticalFlow) ? dfPanel.RelativePosition.x : dfPanel.RelativePosition.y);
			if (getMaxes)
			{
				if (num > newY)
				{
					newY = num;
				}
				if (idfvirtualScrollingTile2.VirtualScrollItemIndex > index)
				{
					index = idfvirtualScrollingTile2.VirtualScrollItemIndex;
				}
			}
			else
			{
				if (num < newY)
				{
					newY = num;
				}
				if (idfvirtualScrollingTile2.VirtualScrollItemIndex < index)
				{
					index = idfvirtualScrollingTile2.VirtualScrollItemIndex;
				}
			}
		}
		if (getMaxes)
		{
			index++;
		}
		else
		{
			index--;
		}
	}

	// Token: 0x04001316 RID: 4886
	public IList<T> BackingList;

	// Token: 0x04001317 RID: 4887
	public List<IDFVirtualScrollingTile> Tiles = new List<IDFVirtualScrollingTile>();

	// Token: 0x04001318 RID: 4888
	public RectOffset ItemPadding;

	// Token: 0x04001319 RID: 4889
	public Vector2 LastScrollPosition = Vector2.zero;

	// Token: 0x0400131A RID: 4890
	public int MaxExtraOffscreenTiles = 10;

	// Token: 0x0400131B RID: 4891
	public IDFVirtualScrollingTile DummyTop;

	// Token: 0x0400131C RID: 4892
	public IDFVirtualScrollingTile DummyBottom;

	// Token: 0x0400131D RID: 4893
	public bool IsPaging;

	// Token: 0x0400131E RID: 4894
	public bool IsInitialized;
}
