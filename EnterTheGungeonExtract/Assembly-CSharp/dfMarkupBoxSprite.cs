using System;
using UnityEngine;

// Token: 0x02000499 RID: 1177
public class dfMarkupBoxSprite : dfMarkupBox
{
	// Token: 0x06001B4E RID: 6990 RVA: 0x00080F9C File Offset: 0x0007F19C
	public dfMarkupBoxSprite(dfMarkupElement element, dfMarkupDisplayType display, dfMarkupStyle style)
		: base(element, display, style)
	{
	}

	// Token: 0x1700059B RID: 1435
	// (get) Token: 0x06001B4F RID: 6991 RVA: 0x00080FB4 File Offset: 0x0007F1B4
	// (set) Token: 0x06001B50 RID: 6992 RVA: 0x00080FBC File Offset: 0x0007F1BC
	public dfAtlas Atlas { get; set; }

	// Token: 0x1700059C RID: 1436
	// (get) Token: 0x06001B51 RID: 6993 RVA: 0x00080FC8 File Offset: 0x0007F1C8
	// (set) Token: 0x06001B52 RID: 6994 RVA: 0x00080FD0 File Offset: 0x0007F1D0
	public string Source { get; set; }

	// Token: 0x06001B53 RID: 6995 RVA: 0x00080FDC File Offset: 0x0007F1DC
	internal void LoadImage(dfAtlas atlas, string source)
	{
		dfAtlas.ItemInfo itemInfo = atlas[source];
		if (itemInfo == null)
		{
			throw new InvalidOperationException("Sprite does not exist in atlas: " + source);
		}
		this.Atlas = atlas;
		this.Source = source;
		this.Size = itemInfo.sizeInPixels;
		this.Baseline = (int)this.Size.y;
	}

	// Token: 0x06001B54 RID: 6996 RVA: 0x0008103C File Offset: 0x0007F23C
	protected override dfRenderData OnRebuildRenderData()
	{
		this.renderData.Clear();
		if (this.Atlas != null && this.Atlas[this.Source] != null)
		{
			dfSprite.RenderOptions renderOptions = new dfSprite.RenderOptions
			{
				atlas = this.Atlas,
				spriteInfo = this.Atlas[this.Source],
				pixelsToUnits = 1f,
				size = this.Size,
				color = this.Style.Color,
				baseIndex = 0,
				fillAmount = 1f,
				flip = dfSpriteFlip.None
			};
			dfSlicedSprite.renderSprite(this.renderData, renderOptions);
			this.renderData.Material = this.Atlas.Material;
			this.renderData.Transform = Matrix4x4.identity;
		}
		return this.renderData;
	}

	// Token: 0x06001B55 RID: 6997 RVA: 0x00081134 File Offset: 0x0007F334
	private static void addTriangleIndices(dfList<Vector3> verts, dfList<int> triangles)
	{
		int count = verts.Count;
		int[] triangle_INDICES = dfMarkupBoxSprite.TRIANGLE_INDICES;
		for (int i = 0; i < triangle_INDICES.Length; i++)
		{
			triangles.Add(count + triangle_INDICES[i]);
		}
	}

	// Token: 0x0400156F RID: 5487
	private static int[] TRIANGLE_INDICES = new int[] { 0, 1, 2, 0, 2, 3 };

	// Token: 0x04001572 RID: 5490
	private dfRenderData renderData = new dfRenderData();
}
