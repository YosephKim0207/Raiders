using System;
using UnityEngine;

// Token: 0x02000E62 RID: 3682
public static class DepthLookupManager
{
	// Token: 0x06004E5A RID: 20058 RVA: 0x001B15A4 File Offset: 0x001AF7A4
	public static void PinRendererToRenderer(Renderer attachment, Renderer target)
	{
		tk2dSprite component = attachment.GetComponent<tk2dSprite>();
		if (component != null)
		{
			component.automaticallyManagesDepth = false;
		}
		attachment.sortingLayerName = target.sortingLayerName;
		attachment.sortingOrder = target.sortingOrder;
	}

	// Token: 0x06004E5B RID: 20059 RVA: 0x001B15E4 File Offset: 0x001AF7E4
	public static void ProcessRenderer(Renderer r)
	{
		DepthLookupManager.AssignRendererToSortingLayer(r, DepthLookupManager.GungeonSortingLayer.PLAYFIELD);
		DepthLookupManager.UpdateRenderer(r);
	}

	// Token: 0x06004E5C RID: 20060 RVA: 0x001B15F4 File Offset: 0x001AF7F4
	public static void ProcessRenderer(Renderer r, DepthLookupManager.GungeonSortingLayer l)
	{
		DepthLookupManager.AssignRendererToSortingLayer(r, l);
		DepthLookupManager.UpdateRenderer(r);
	}

	// Token: 0x06004E5D RID: 20061 RVA: 0x001B1604 File Offset: 0x001AF804
	public static void UpdateRenderer(Renderer r)
	{
	}

	// Token: 0x06004E5E RID: 20062 RVA: 0x001B1608 File Offset: 0x001AF808
	public static void UpdateRendererWithWorldYPosition(Renderer r, float worldY)
	{
	}

	// Token: 0x06004E5F RID: 20063 RVA: 0x001B160C File Offset: 0x001AF80C
	public static void AssignSortingOrder(Renderer r, int order)
	{
	}

	// Token: 0x06004E60 RID: 20064 RVA: 0x001B1610 File Offset: 0x001AF810
	public static void AssignRendererToSortingLayer(Renderer r, DepthLookupManager.GungeonSortingLayer targetLayer)
	{
		string text = string.Empty;
		switch (targetLayer)
		{
		case DepthLookupManager.GungeonSortingLayer.BACKGROUND:
			text = "Background";
			break;
		case DepthLookupManager.GungeonSortingLayer.PLAYFIELD:
			text = "Player";
			break;
		case DepthLookupManager.GungeonSortingLayer.FOREGROUND:
			text = "Foreground";
			break;
		default:
			BraveUtility.Log("Switching on invalid sorting layer in AssignRendererToSortingLayer!", Color.red, BraveUtility.LogVerbosity.IMPORTANT);
			break;
		}
		r.sortingLayerName = text;
	}

	// Token: 0x06004E61 RID: 20065 RVA: 0x001B1678 File Offset: 0x001AF878
	private static void AssignSortingOrderByDepth(Renderer r, float yPosition)
	{
	}

	// Token: 0x040044BD RID: 17597
	public static float DEPTH_RESOLUTION_PER_UNIT = 5f;

	// Token: 0x02000E63 RID: 3683
	public enum GungeonSortingLayer
	{
		// Token: 0x040044BF RID: 17599
		BACKGROUND,
		// Token: 0x040044C0 RID: 17600
		PLAYFIELD,
		// Token: 0x040044C1 RID: 17601
		FOREGROUND
	}
}
