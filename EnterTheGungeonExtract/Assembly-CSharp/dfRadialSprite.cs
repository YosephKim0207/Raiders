using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020003EC RID: 1004
[dfTooltip("Implements a sprite that can be filled in a radial fashion instead of strictly horizontally or vertically like other sprite classes. Useful for spell cooldown effects, map effects, etc.")]
[dfCategory("Basic Controls")]
[AddComponentMenu("Daikon Forge/User Interface/Sprite/Radial")]
[ExecuteInEditMode]
[dfHelp("http://www.daikonforge.com/docs/df-gui/classdf_radial_sprite.html")]
[Serializable]
public class dfRadialSprite : dfSprite
{
	// Token: 0x1700049D RID: 1181
	// (get) Token: 0x0600153B RID: 5435 RVA: 0x000627A8 File Offset: 0x000609A8
	// (set) Token: 0x0600153C RID: 5436 RVA: 0x000627B0 File Offset: 0x000609B0
	public dfPivotPoint FillOrigin
	{
		get
		{
			return this.fillOrigin;
		}
		set
		{
			if (value != this.fillOrigin)
			{
				this.fillOrigin = value;
				this.Invalidate();
			}
		}
	}

	// Token: 0x0600153D RID: 5437 RVA: 0x000627CC File Offset: 0x000609CC
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
		List<Vector3> list = null;
		List<int> list2 = null;
		List<Vector2> list3 = null;
		this.buildMeshData(ref list, ref list2, ref list3);
		Color32[] array = this.buildColors(list.Count);
		this.renderData.Vertices.AddRange(list);
		this.renderData.Triangles.AddRange(list2);
		this.renderData.UV.AddRange(list3);
		this.renderData.Colors.AddRange(array);
	}

	// Token: 0x0600153E RID: 5438 RVA: 0x0006287C File Offset: 0x00060A7C
	private void buildMeshData(ref List<Vector3> verts, ref List<int> indices, ref List<Vector2> uv)
	{
		List<Vector3> list;
		verts = (list = new List<Vector3>());
		List<Vector3> list2 = list;
		verts.AddRange(dfRadialSprite.baseVerts);
		int num;
		int num2;
		switch (this.fillOrigin)
		{
		case dfPivotPoint.TopLeft:
			num = 4;
			num2 = 5;
			list2.RemoveAt(6);
			list2.RemoveAt(0);
			break;
		case dfPivotPoint.TopCenter:
			num = 6;
			num2 = 0;
			break;
		case dfPivotPoint.TopRight:
			num = 4;
			num2 = 0;
			list2.RemoveAt(2);
			list2.RemoveAt(0);
			break;
		case dfPivotPoint.MiddleLeft:
			num = 6;
			num2 = 6;
			break;
		case dfPivotPoint.MiddleCenter:
			num = 8;
			list2.Add(list2[0]);
			list2.Insert(0, Vector3.zero);
			num2 = 0;
			break;
		case dfPivotPoint.MiddleRight:
			num = 6;
			num2 = 2;
			break;
		case dfPivotPoint.BottomLeft:
			num = 4;
			num2 = 4;
			list2.RemoveAt(6);
			list2.RemoveAt(4);
			break;
		case dfPivotPoint.BottomCenter:
			num = 6;
			num2 = 4;
			break;
		case dfPivotPoint.BottomRight:
			num = 4;
			num2 = 2;
			list2.RemoveAt(4);
			list2.RemoveAt(2);
			break;
		default:
			throw new NotImplementedException();
		}
		this.makeFirst(list2, num2);
		List<int> list3;
		indices = (list3 = this.buildTriangles(list2));
		List<int> list4 = list3;
		float num3 = 1f / (float)num;
		float num4 = this.fillAmount.Quantize(num3);
		int num5 = Mathf.CeilToInt(num4 / num3) + 1;
		for (int i = num5; i < num; i++)
		{
			if (this.invertFill)
			{
				list4.RemoveRange(0, 3);
			}
			else
			{
				list2.RemoveAt(list2.Count - 1);
				list4.RemoveRange(list4.Count - 3, 3);
			}
		}
		if (this.fillAmount < 1f)
		{
			int num6 = list4[(!this.invertFill) ? (list4.Count - 2) : 2];
			int num7 = list4[(!this.invertFill) ? (list4.Count - 1) : 1];
			float num8 = (base.FillAmount - num4) / num3;
			list2[num7] = Vector3.Lerp(list2[num6], list2[num7], num8);
		}
		uv = this.buildUV(list2);
		float num9 = base.PixelsToUnits();
		Vector2 vector = num9 * this.size;
		Vector3 vector2 = this.pivot.TransformToCenter(this.size) * num9;
		for (int j = 0; j < list2.Count; j++)
		{
			list2[j] = Vector3.Scale(list2[j], vector) + vector2;
		}
	}

	// Token: 0x0600153F RID: 5439 RVA: 0x00062B10 File Offset: 0x00060D10
	private void makeFirst(List<Vector3> list, int index)
	{
		if (index == 0)
		{
			return;
		}
		List<Vector3> range = list.GetRange(index, list.Count - index);
		list.RemoveRange(index, list.Count - index);
		list.InsertRange(0, range);
	}

	// Token: 0x06001540 RID: 5440 RVA: 0x00062B4C File Offset: 0x00060D4C
	private List<int> buildTriangles(List<Vector3> verts)
	{
		List<int> list = new List<int>();
		int count = verts.Count;
		for (int i = 1; i < count - 1; i++)
		{
			list.Add(0);
			list.Add(i);
			list.Add(i + 1);
		}
		return list;
	}

	// Token: 0x06001541 RID: 5441 RVA: 0x00062B94 File Offset: 0x00060D94
	private List<Vector2> buildUV(List<Vector3> verts)
	{
		dfAtlas.ItemInfo spriteInfo = base.SpriteInfo;
		if (spriteInfo == null)
		{
			return null;
		}
		Rect region = spriteInfo.region;
		if (this.flip.IsSet(dfSpriteFlip.FlipHorizontal))
		{
			region = new Rect(region.xMax, region.y, -region.width, region.height);
		}
		if (this.flip.IsSet(dfSpriteFlip.FlipVertical))
		{
			region = new Rect(region.x, region.yMax, region.width, -region.height);
		}
		Vector2 vector = new Vector2(region.x, region.y);
		Vector2 vector2 = new Vector2(0.5f, 0.5f);
		Vector2 vector3 = new Vector2(region.width, region.height);
		List<Vector2> list = new List<Vector2>(verts.Count);
		for (int i = 0; i < verts.Count; i++)
		{
			Vector2 vector4 = verts[i] + vector2;
			list.Add(Vector2.Scale(vector4, vector3) + vector);
		}
		return list;
	}

	// Token: 0x06001542 RID: 5442 RVA: 0x00062CB8 File Offset: 0x00060EB8
	private Color32[] buildColors(int vertCount)
	{
		Color32 color = base.ApplyOpacity((!base.IsEnabled) ? this.disabledColor : this.color);
		Color32[] array = new Color32[vertCount];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = color;
		}
		return array;
	}

	// Token: 0x04001217 RID: 4631
	private static Vector3[] baseVerts = new Vector3[]
	{
		new Vector3(0f, 0.5f, 0f),
		new Vector3(0.5f, 0.5f, 0f),
		new Vector3(0.5f, 0f, 0f),
		new Vector3(0.5f, -0.5f, 0f),
		new Vector3(0f, -0.5f, 0f),
		new Vector3(-0.5f, -0.5f, 0f),
		new Vector3(-0.5f, 0f, 0f),
		new Vector3(-0.5f, 0.5f, 0f)
	};

	// Token: 0x04001218 RID: 4632
	[SerializeField]
	protected dfPivotPoint fillOrigin = dfPivotPoint.MiddleCenter;
}
