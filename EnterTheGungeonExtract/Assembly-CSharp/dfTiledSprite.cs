using System;
using UnityEngine;

// Token: 0x02000407 RID: 1031
[dfTooltip("Implements a Sprite that can be tiled horizontally and vertically")]
[dfHelp("http://www.daikonforge.com/docs/df-gui/classdf_tiled_sprite.html")]
[ExecuteInEditMode]
[dfCategory("Basic Controls")]
[AddComponentMenu("Daikon Forge/User Interface/Sprite/Tiled")]
[Serializable]
public class dfTiledSprite : dfSprite
{
	// Token: 0x1700050E RID: 1294
	// (get) Token: 0x06001764 RID: 5988 RVA: 0x0006F23C File Offset: 0x0006D43C
	// (set) Token: 0x06001765 RID: 5989 RVA: 0x0006F244 File Offset: 0x0006D444
	public Vector2 TileScale
	{
		get
		{
			return this.tileScale;
		}
		set
		{
			if (Vector2.Distance(value, this.tileScale) > 1E-45f)
			{
				this.tileScale = Vector2.Max(Vector2.one * 0.1f, value);
				this.Invalidate();
			}
		}
	}

	// Token: 0x1700050F RID: 1295
	// (get) Token: 0x06001766 RID: 5990 RVA: 0x0006F280 File Offset: 0x0006D480
	// (set) Token: 0x06001767 RID: 5991 RVA: 0x0006F288 File Offset: 0x0006D488
	public Vector2 TileScroll
	{
		get
		{
			return this.tileScroll;
		}
		set
		{
			if (Vector2.Distance(value, this.tileScroll) > 1E-45f)
			{
				this.tileScroll = value;
				this.Invalidate();
			}
		}
	}

	// Token: 0x06001768 RID: 5992 RVA: 0x0006F2B0 File Offset: 0x0006D4B0
	protected override void OnRebuildRenderData()
	{
		if (base.Atlas == null)
		{
			return;
		}
		dfAtlas.ItemInfo spriteInfo = base.SpriteInfo;
		if (spriteInfo == null)
		{
			return;
		}
		this.renderData.Material = base.Atlas.Material;
		dfList<Vector3> vertices = this.renderData.Vertices;
		dfList<Vector2> uv = this.renderData.UV;
		dfList<Color32> colors = this.renderData.Colors;
		dfList<int> triangles = this.renderData.Triangles;
		Vector2[] array = this.buildQuadUV();
		Vector2 vector = Vector2.Scale(spriteInfo.sizeInPixels, this.tileScale);
		Vector2 vector2 = new Vector2(this.tileScroll.x % 1f, this.tileScroll.y % 1f);
		float num = ((!this.EnableBlackLineFix) ? 0f : (-0.1f));
		for (float num2 = -Mathf.Abs(vector2.y * vector.y); num2 < this.size.y; num2 += vector.y)
		{
			for (float num3 = -Mathf.Abs(vector2.x * vector.x); num3 < this.size.x; num3 += vector.x)
			{
				int count = vertices.Count;
				vertices.Add(new Vector3(num3, -num2));
				vertices.Add(new Vector3(num3 + vector.x, -num2));
				vertices.Add(new Vector3(num3 + vector.x, -num2 + -vector.y + num));
				vertices.Add(new Vector3(num3, -num2 + -vector.y + num));
				this.addQuadTriangles(triangles, count);
				this.addQuadUV(uv, array);
				this.addQuadColors(colors);
			}
		}
		this.clipQuads(vertices, uv);
		float num4 = base.PixelsToUnits();
		Vector3 vector3 = this.pivot.TransformToUpperLeft(this.size);
		for (int i = 0; i < vertices.Count; i++)
		{
			vertices[i] = (vertices[i] + vector3) * num4;
		}
	}

	// Token: 0x06001769 RID: 5993 RVA: 0x0006F4E4 File Offset: 0x0006D6E4
	private void clipQuads(dfList<Vector3> verts, dfList<Vector2> uv)
	{
		float num = 0f;
		float num2 = this.size.x;
		float num3 = -this.size.y;
		float num4 = 0f;
		if (this.fillAmount < 1f)
		{
			if (this.fillDirection == dfFillDirection.Horizontal)
			{
				if (!this.invertFill)
				{
					num2 = this.size.x * this.fillAmount;
				}
				else
				{
					num = this.size.x - this.size.x * this.fillAmount;
				}
			}
			else if (!this.invertFill)
			{
				num3 = -this.size.y * this.fillAmount;
			}
			else
			{
				num4 = -this.size.y * (1f - this.fillAmount);
			}
		}
		for (int i = 0; i < verts.Count; i += 4)
		{
			Vector3 vector = verts[i];
			Vector3 vector2 = verts[i + 1];
			Vector3 vector3 = verts[i + 2];
			Vector3 vector4 = verts[i + 3];
			float num5 = vector2.x - vector.x;
			float num6 = vector.y - vector4.y;
			if (vector.x < num)
			{
				float num7 = (num - vector.x) / num5;
				int num8 = i;
				vector = new Vector3(Mathf.Max(num, vector.x), vector.y, vector.z);
				verts[num8] = vector;
				int num9 = i + 1;
				vector2 = new Vector3(Mathf.Max(num, vector2.x), vector2.y, vector2.z);
				verts[num9] = vector2;
				int num10 = i + 2;
				vector3 = new Vector3(Mathf.Max(num, vector3.x), vector3.y, vector3.z);
				verts[num10] = vector3;
				int num11 = i + 3;
				vector4 = new Vector3(Mathf.Max(num, vector4.x), vector4.y, vector4.z);
				verts[num11] = vector4;
				float num12 = Mathf.Lerp(uv[i].x, uv[i + 1].x, num7);
				uv[i] = new Vector2(num12, uv[i].y);
				uv[i + 3] = new Vector2(num12, uv[i + 3].y);
				num5 = vector2.x - vector.x;
			}
			if (vector2.x > num2)
			{
				float num13 = 1f - (num2 - vector2.x + num5) / num5;
				int num14 = i;
				vector = new Vector3(Mathf.Min(vector.x, num2), vector.y, vector.z);
				verts[num14] = vector;
				int num15 = i + 1;
				vector2 = new Vector3(Mathf.Min(vector2.x, num2), vector2.y, vector2.z);
				verts[num15] = vector2;
				int num16 = i + 2;
				vector3 = new Vector3(Mathf.Min(vector3.x, num2), vector3.y, vector3.z);
				verts[num16] = vector3;
				int num17 = i + 3;
				vector4 = new Vector3(Mathf.Min(vector4.x, num2), vector4.y, vector4.z);
				verts[num17] = vector4;
				float num18 = Mathf.Lerp(uv[i + 1].x, uv[i].x, num13);
				uv[i + 1] = new Vector2(num18, uv[i + 1].y);
				uv[i + 2] = new Vector2(num18, uv[i + 2].y);
				num5 = vector2.x - vector.x;
			}
			if (vector4.y < num3)
			{
				float num19 = 1f - Mathf.Abs(-num3 + vector.y) / num6;
				int num20 = i;
				vector = new Vector3(vector.x, Mathf.Max(vector.y, num3), vector2.z);
				verts[num20] = vector;
				int num21 = i + 1;
				vector2 = new Vector3(vector2.x, Mathf.Max(vector2.y, num3), vector2.z);
				verts[num21] = vector2;
				int num22 = i + 2;
				vector3 = new Vector3(vector3.x, Mathf.Max(vector3.y, num3), vector3.z);
				verts[num22] = vector3;
				int num23 = i + 3;
				vector4 = new Vector3(vector4.x, Mathf.Max(vector4.y, num3), vector4.z);
				verts[num23] = vector4;
				float num24 = Mathf.Lerp(uv[i + 3].y, uv[i].y, num19);
				uv[i + 3] = new Vector2(uv[i + 3].x, num24);
				uv[i + 2] = new Vector2(uv[i + 2].x, num24);
				num6 = Mathf.Abs(vector4.y - vector.y);
			}
			if (vector.y > num4)
			{
				float num25 = Mathf.Abs(num4 - vector.y) / num6;
				int num26 = i;
				vector = new Vector3(vector.x, Mathf.Min(num4, vector.y), vector.z);
				verts[num26] = vector;
				int num27 = i + 1;
				vector2 = new Vector3(vector2.x, Mathf.Min(num4, vector2.y), vector2.z);
				verts[num27] = vector2;
				int num28 = i + 2;
				vector3 = new Vector3(vector3.x, Mathf.Min(num4, vector3.y), vector3.z);
				verts[num28] = vector3;
				int num29 = i + 3;
				vector4 = new Vector3(vector4.x, Mathf.Min(num4, vector4.y), vector4.z);
				verts[num29] = vector4;
				float num30 = Mathf.Lerp(uv[i].y, uv[i + 3].y, num25);
				uv[i] = new Vector2(uv[i].x, num30);
				uv[i + 1] = new Vector2(uv[i + 1].x, num30);
			}
		}
	}

	// Token: 0x0600176A RID: 5994 RVA: 0x0006FB84 File Offset: 0x0006DD84
	private void addQuadTriangles(dfList<int> triangles, int baseIndex)
	{
		for (int i = 0; i < dfTiledSprite.quadTriangles.Length; i++)
		{
			triangles.Add(dfTiledSprite.quadTriangles[i] + baseIndex);
		}
	}

	// Token: 0x0600176B RID: 5995 RVA: 0x0006FBB8 File Offset: 0x0006DDB8
	private void addQuadColors(dfList<Color32> colors)
	{
		colors.EnsureCapacity(colors.Count + 4);
		Color32 color = base.ApplyOpacity((!base.IsEnabled) ? this.disabledColor : this.color);
		for (int i = 0; i < 4; i++)
		{
			colors.Add(color);
		}
	}

	// Token: 0x0600176C RID: 5996 RVA: 0x0006FC10 File Offset: 0x0006DE10
	private void addQuadUV(dfList<Vector2> uv, Vector2[] spriteUV)
	{
		uv.AddRange(spriteUV);
	}

	// Token: 0x0600176D RID: 5997 RVA: 0x0006FC1C File Offset: 0x0006DE1C
	private Vector2[] buildQuadUV()
	{
		dfAtlas.ItemInfo spriteInfo = base.SpriteInfo;
		Rect region = spriteInfo.region;
		dfTiledSprite.quadUV[0] = new Vector2(region.x, region.yMax);
		dfTiledSprite.quadUV[1] = new Vector2(region.xMax, region.yMax);
		dfTiledSprite.quadUV[2] = new Vector2(region.xMax, region.y);
		dfTiledSprite.quadUV[3] = new Vector2(region.x, region.y);
		Vector2 vector = Vector2.zero;
		if (this.flip.IsSet(dfSpriteFlip.FlipHorizontal))
		{
			vector = dfTiledSprite.quadUV[1];
			dfTiledSprite.quadUV[1] = dfTiledSprite.quadUV[0];
			dfTiledSprite.quadUV[0] = vector;
			vector = dfTiledSprite.quadUV[3];
			dfTiledSprite.quadUV[3] = dfTiledSprite.quadUV[2];
			dfTiledSprite.quadUV[2] = vector;
		}
		if (this.flip.IsSet(dfSpriteFlip.FlipVertical))
		{
			vector = dfTiledSprite.quadUV[0];
			dfTiledSprite.quadUV[0] = dfTiledSprite.quadUV[3];
			dfTiledSprite.quadUV[3] = vector;
			vector = dfTiledSprite.quadUV[1];
			dfTiledSprite.quadUV[1] = dfTiledSprite.quadUV[2];
			dfTiledSprite.quadUV[2] = vector;
		}
		return dfTiledSprite.quadUV;
	}

	// Token: 0x040012E4 RID: 4836
	private static int[] quadTriangles = new int[] { 0, 1, 3, 3, 1, 2 };

	// Token: 0x040012E5 RID: 4837
	private static Vector2[] quadUV = new Vector2[4];

	// Token: 0x040012E6 RID: 4838
	[SerializeField]
	protected Vector2 tileScale = Vector2.one;

	// Token: 0x040012E7 RID: 4839
	[SerializeField]
	protected Vector2 tileScroll = Vector2.zero;

	// Token: 0x040012E8 RID: 4840
	public bool EnableBlackLineFix;
}
