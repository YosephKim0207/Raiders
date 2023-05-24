using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

// Token: 0x0200049B RID: 1179
public class dfMarkupBoxText : dfMarkupBox
{
	// Token: 0x06001B5F RID: 7007 RVA: 0x000814C0 File Offset: 0x0007F6C0
	public dfMarkupBoxText(dfMarkupElement element, dfMarkupDisplayType display, dfMarkupStyle style)
		: base(element, display, style)
	{
	}

	// Token: 0x1700059E RID: 1438
	// (get) Token: 0x06001B60 RID: 7008 RVA: 0x000814D8 File Offset: 0x0007F6D8
	// (set) Token: 0x06001B61 RID: 7009 RVA: 0x000814E0 File Offset: 0x0007F6E0
	public string Text { get; private set; }

	// Token: 0x1700059F RID: 1439
	// (get) Token: 0x06001B62 RID: 7010 RVA: 0x000814EC File Offset: 0x0007F6EC
	public bool IsWhitespace
	{
		get
		{
			return this.isWhitespace;
		}
	}

	// Token: 0x06001B63 RID: 7011 RVA: 0x000814F4 File Offset: 0x0007F6F4
	public static dfMarkupBoxText Obtain(dfMarkupElement element, dfMarkupDisplayType display, dfMarkupStyle style)
	{
		if (dfMarkupBoxText.objectPool.Count > 0)
		{
			dfMarkupBoxText dfMarkupBoxText = dfMarkupBoxText.objectPool.Dequeue();
			dfMarkupBoxText.Element = element;
			dfMarkupBoxText.Display = display;
			dfMarkupBoxText.Style = style;
			dfMarkupBoxText.Position = Vector2.zero;
			dfMarkupBoxText.Size = Vector2.zero;
			dfMarkupBoxText.Baseline = (int)((float)style.FontSize * 1.1f);
			dfMarkupBoxText.Margins = default(dfMarkupBorders);
			dfMarkupBoxText.Padding = default(dfMarkupBorders);
			return dfMarkupBoxText;
		}
		return new dfMarkupBoxText(element, display, style);
	}

	// Token: 0x06001B64 RID: 7012 RVA: 0x00081584 File Offset: 0x0007F784
	public override void Release()
	{
		base.Release();
		this.Text = string.Empty;
		this.renderData.Clear();
		dfMarkupBoxText.objectPool.Enqueue(this);
	}

	// Token: 0x06001B65 RID: 7013 RVA: 0x000815B0 File Offset: 0x0007F7B0
	internal void SetText(string text)
	{
		this.Text = text;
		if (this.Style.Font == null)
		{
			return;
		}
		this.isWhitespace = dfMarkupBoxText.whitespacePattern.IsMatch(this.Text);
		string text2 = ((!this.Style.PreserveWhitespace && this.isWhitespace) ? " " : this.Text);
		int fontSize = this.Style.FontSize;
		Vector2 vector = new Vector2(0f, (float)this.Style.LineHeight);
		this.Style.Font.RequestCharacters(text2, this.Style.FontSize, this.Style.FontStyle);
		CharacterInfo characterInfo = default(CharacterInfo);
		for (int i = 0; i < text2.Length; i++)
		{
			if (this.Style.Font.BaseFont.GetCharacterInfo(text2[i], out characterInfo, fontSize, this.Style.FontStyle))
			{
				float num = (float)characterInfo.maxX;
				if (text2[i] == ' ')
				{
					num = Mathf.Max(num, (float)fontSize * 0.33f);
				}
				else if (text2[i] == '\t')
				{
					num += (float)(fontSize * 3);
				}
				vector.x += num;
			}
		}
		this.Size = vector;
		dfDynamicFont font = this.Style.Font;
		float num2 = (float)fontSize / (float)font.FontSize;
		this.Baseline = Mathf.CeilToInt((float)font.Baseline * num2);
	}

	// Token: 0x06001B66 RID: 7014 RVA: 0x00081750 File Offset: 0x0007F950
	protected override dfRenderData OnRebuildRenderData()
	{
		this.renderData.Clear();
		if (this.Style.Font == null)
		{
			return null;
		}
		if (this.Style.TextDecoration == dfMarkupTextDecoration.Underline)
		{
			this.renderUnderline();
		}
		this.renderText(this.Text);
		return this.renderData;
	}

	// Token: 0x06001B67 RID: 7015 RVA: 0x000817AC File Offset: 0x0007F9AC
	private void renderUnderline()
	{
	}

	// Token: 0x06001B68 RID: 7016 RVA: 0x000817B0 File Offset: 0x0007F9B0
	private void renderText(string text)
	{
		dfDynamicFont font = this.Style.Font;
		int fontSize = this.Style.FontSize;
		FontStyle fontStyle = this.Style.FontStyle;
		CharacterInfo characterInfo = default(CharacterInfo);
		dfList<Vector3> vertices = this.renderData.Vertices;
		dfList<int> triangles = this.renderData.Triangles;
		dfList<Vector2> uv = this.renderData.UV;
		dfList<Color32> colors = this.renderData.Colors;
		float num = (float)fontSize / (float)font.FontSize;
		float num2 = (float)font.Descent * num;
		float num3 = 0f;
		font.RequestCharacters(text, fontSize, fontStyle);
		this.renderData.Material = font.Material;
		for (int i = 0; i < text.Length; i++)
		{
			if (font.BaseFont.GetCharacterInfo(text[i], out characterInfo, fontSize, fontStyle))
			{
				dfMarkupBoxText.addTriangleIndices(vertices, triangles);
				float num4 = (float)(font.FontSize + characterInfo.maxY - fontSize) + num2;
				float num5 = num3 + (float)characterInfo.minX;
				float num6 = num4;
				float num7 = num5 + (float)characterInfo.glyphWidth;
				float num8 = num6 - (float)characterInfo.glyphHeight;
				Vector3 vector = new Vector3(num5, num6);
				Vector3 vector2 = new Vector3(num7, num6);
				Vector3 vector3 = new Vector3(num7, num8);
				Vector3 vector4 = new Vector3(num5, num8);
				vertices.Add(vector);
				vertices.Add(vector2);
				vertices.Add(vector3);
				vertices.Add(vector4);
				Color color = this.Style.Color;
				colors.Add(color);
				colors.Add(color);
				colors.Add(color);
				colors.Add(color);
				uv.Add(characterInfo.uvTopLeft);
				uv.Add(characterInfo.uvTopRight);
				uv.Add(characterInfo.uvBottomRight);
				uv.Add(characterInfo.uvBottomLeft);
				num3 += (float)Mathf.CeilToInt((float)characterInfo.maxX);
			}
		}
	}

	// Token: 0x06001B69 RID: 7017 RVA: 0x000819BC File Offset: 0x0007FBBC
	private static void addTriangleIndices(dfList<Vector3> verts, dfList<int> triangles)
	{
		int count = verts.Count;
		int[] triangle_INDICES = dfMarkupBoxText.TRIANGLE_INDICES;
		for (int i = 0; i < triangle_INDICES.Length; i++)
		{
			triangles.Add(count + triangle_INDICES[i]);
		}
	}

	// Token: 0x04001577 RID: 5495
	private static int[] TRIANGLE_INDICES = new int[] { 0, 1, 2, 0, 2, 3 };

	// Token: 0x04001578 RID: 5496
	private static Queue<dfMarkupBoxText> objectPool = new Queue<dfMarkupBoxText>();

	// Token: 0x04001579 RID: 5497
	private static Regex whitespacePattern = new Regex("\\s+");

	// Token: 0x0400157B RID: 5499
	private dfRenderData renderData = new dfRenderData();

	// Token: 0x0400157C RID: 5500
	private bool isWhitespace;
}
