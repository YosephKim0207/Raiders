using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

// Token: 0x02000493 RID: 1171
[AddComponentMenu("Daikon Forge/User Interface/Dynamic Font")]
[ExecuteInEditMode]
[Serializable]
public class dfDynamicFont : dfFontBase
{
	// Token: 0x17000589 RID: 1417
	// (get) Token: 0x06001AF3 RID: 6899 RVA: 0x0007E160 File Offset: 0x0007C360
	// (set) Token: 0x06001AF4 RID: 6900 RVA: 0x0007E1C4 File Offset: 0x0007C3C4
	public override Material Material
	{
		get
		{
			if (this.baseFont != null && this.material != null)
			{
				this.material.mainTexture = this.baseFont.material.mainTexture;
				this.material.shader = this.Shader;
			}
			return this.material;
		}
		set
		{
			if (value != this.material)
			{
				this.material = value;
				dfGUIManager.RefreshAll();
			}
		}
	}

	// Token: 0x1700058A RID: 1418
	// (get) Token: 0x06001AF5 RID: 6901 RVA: 0x0007E1E4 File Offset: 0x0007C3E4
	// (set) Token: 0x06001AF6 RID: 6902 RVA: 0x0007E210 File Offset: 0x0007C410
	public Shader Shader
	{
		get
		{
			if (this.shader == null)
			{
				this.shader = Shader.Find("Daikon Forge/Dynamic Font Shader");
			}
			return this.shader;
		}
		set
		{
			this.shader = value;
			dfGUIManager.RefreshAll();
		}
	}

	// Token: 0x1700058B RID: 1419
	// (get) Token: 0x06001AF7 RID: 6903 RVA: 0x0007E220 File Offset: 0x0007C420
	public override Texture Texture
	{
		get
		{
			return this.baseFont.material.mainTexture;
		}
	}

	// Token: 0x1700058C RID: 1420
	// (get) Token: 0x06001AF8 RID: 6904 RVA: 0x0007E234 File Offset: 0x0007C434
	public override bool IsValid
	{
		get
		{
			return this.baseFont != null && this.Material != null && this.Texture != null;
		}
	}

	// Token: 0x1700058D RID: 1421
	// (get) Token: 0x06001AF9 RID: 6905 RVA: 0x0007E268 File Offset: 0x0007C468
	// (set) Token: 0x06001AFA RID: 6906 RVA: 0x0007E270 File Offset: 0x0007C470
	public override int FontSize
	{
		get
		{
			return this.baseFontSize;
		}
		set
		{
			if (value != this.baseFontSize)
			{
				this.baseFontSize = value;
				dfGUIManager.RefreshAll();
			}
		}
	}

	// Token: 0x1700058E RID: 1422
	// (get) Token: 0x06001AFB RID: 6907 RVA: 0x0007E28C File Offset: 0x0007C48C
	// (set) Token: 0x06001AFC RID: 6908 RVA: 0x0007E294 File Offset: 0x0007C494
	public override int LineHeight
	{
		get
		{
			return this.lineHeight;
		}
		set
		{
			if (value != this.lineHeight)
			{
				this.lineHeight = value;
				dfGUIManager.RefreshAll();
			}
		}
	}

	// Token: 0x06001AFD RID: 6909 RVA: 0x0007E2B0 File Offset: 0x0007C4B0
	public override dfFontRendererBase ObtainRenderer()
	{
		return dfDynamicFont.DynamicFontRenderer.Obtain(this);
	}

	// Token: 0x1700058F RID: 1423
	// (get) Token: 0x06001AFE RID: 6910 RVA: 0x0007E2B8 File Offset: 0x0007C4B8
	// (set) Token: 0x06001AFF RID: 6911 RVA: 0x0007E2C0 File Offset: 0x0007C4C0
	public Font BaseFont
	{
		get
		{
			return this.baseFont;
		}
		set
		{
			if (value != this.baseFont)
			{
				this.baseFont = value;
				dfGUIManager.RefreshAll();
			}
		}
	}

	// Token: 0x17000590 RID: 1424
	// (get) Token: 0x06001B00 RID: 6912 RVA: 0x0007E2E0 File Offset: 0x0007C4E0
	// (set) Token: 0x06001B01 RID: 6913 RVA: 0x0007E2E8 File Offset: 0x0007C4E8
	public int Baseline
	{
		get
		{
			return this.baseline;
		}
		set
		{
			if (value != this.baseline)
			{
				this.baseline = value;
				dfGUIManager.RefreshAll();
			}
		}
	}

	// Token: 0x17000591 RID: 1425
	// (get) Token: 0x06001B02 RID: 6914 RVA: 0x0007E304 File Offset: 0x0007C504
	public int Descent
	{
		get
		{
			return this.LineHeight - this.baseline;
		}
	}

	// Token: 0x06001B03 RID: 6915 RVA: 0x0007E314 File Offset: 0x0007C514
	public static dfDynamicFont FindByName(string name)
	{
		for (int i = 0; i < dfDynamicFont.loadedFonts.Count; i++)
		{
			if (string.Equals(dfDynamicFont.loadedFonts[i].name, name, StringComparison.OrdinalIgnoreCase))
			{
				return dfDynamicFont.loadedFonts[i];
			}
		}
		GameObject gameObject = BraveResources.Load(name, ".prefab") as GameObject;
		if (gameObject == null)
		{
			return null;
		}
		dfDynamicFont component = gameObject.GetComponent<dfDynamicFont>();
		if (component == null)
		{
			return null;
		}
		dfDynamicFont.loadedFonts.Add(component);
		return component;
	}

	// Token: 0x06001B04 RID: 6916 RVA: 0x0007E3A4 File Offset: 0x0007C5A4
	public Vector2 MeasureText(string text, int size, FontStyle style)
	{
		this.RequestCharacters(text, size, style);
		float num = (float)size / (float)this.FontSize;
		int num2 = Mathf.CeilToInt((float)this.Baseline * num);
		Vector2 vector = new Vector2(0f, (float)num2);
		CharacterInfo characterInfo = default(CharacterInfo);
		for (int i = 0; i < text.Length; i++)
		{
			this.BaseFont.GetCharacterInfo(text[i], out characterInfo, size, style);
			float num3 = Mathf.Ceil((float)characterInfo.maxX);
			if (text[i] == ' ')
			{
				num3 = Mathf.Ceil((float)characterInfo.advance);
			}
			else if (text[i] == '\t')
			{
				num3 += (float)(size * 4);
			}
			vector.x += num3;
		}
		return vector;
	}

	// Token: 0x06001B05 RID: 6917 RVA: 0x0007E478 File Offset: 0x0007C678
	public void RequestCharacters(string text, int size, FontStyle style)
	{
		if (this.baseFont == null)
		{
			throw new NullReferenceException("Base Font not assigned: " + base.name);
		}
		dfFontManager.Invalidate(this);
		this.baseFont.RequestCharactersInTexture(text, size, style);
	}

	// Token: 0x06001B06 RID: 6918 RVA: 0x0007E4B8 File Offset: 0x0007C6B8
	public virtual void AddCharacterRequest(string characters, int fontSize, FontStyle style)
	{
		dfFontManager.FlagPendingRequests(this);
		dfDynamicFont.FontCharacterRequest fontCharacterRequest = dfDynamicFont.FontCharacterRequest.Obtain();
		fontCharacterRequest.Characters = characters;
		fontCharacterRequest.FontSize = fontSize;
		fontCharacterRequest.FontStyle = style;
		this.requests.Add(fontCharacterRequest);
	}

	// Token: 0x06001B07 RID: 6919 RVA: 0x0007E4F4 File Offset: 0x0007C6F4
	public virtual void FlushCharacterRequests()
	{
		for (int i = 0; i < this.requests.Count; i++)
		{
			dfDynamicFont.FontCharacterRequest fontCharacterRequest = this.requests[i];
			this.baseFont.RequestCharactersInTexture(fontCharacterRequest.Characters, fontCharacterRequest.FontSize, fontCharacterRequest.FontStyle);
		}
		this.requests.ReleaseItems();
	}

	// Token: 0x04001537 RID: 5431
	private static List<dfDynamicFont> loadedFonts = new List<dfDynamicFont>();

	// Token: 0x04001538 RID: 5432
	[SerializeField]
	private Font baseFont;

	// Token: 0x04001539 RID: 5433
	[SerializeField]
	private Material material;

	// Token: 0x0400153A RID: 5434
	[SerializeField]
	private Shader shader;

	// Token: 0x0400153B RID: 5435
	[SerializeField]
	private int baseFontSize = -1;

	// Token: 0x0400153C RID: 5436
	[SerializeField]
	private int baseline = -1;

	// Token: 0x0400153D RID: 5437
	[SerializeField]
	private int lineHeight;

	// Token: 0x0400153E RID: 5438
	protected dfList<dfDynamicFont.FontCharacterRequest> requests = new dfList<dfDynamicFont.FontCharacterRequest>();

	// Token: 0x02000494 RID: 1172
	protected class FontCharacterRequest : IPoolable
	{
		// Token: 0x06001B0A RID: 6922 RVA: 0x0007E568 File Offset: 0x0007C768
		public static dfDynamicFont.FontCharacterRequest Obtain()
		{
			return (dfDynamicFont.FontCharacterRequest.pool.Count <= 0) ? new dfDynamicFont.FontCharacterRequest() : dfDynamicFont.FontCharacterRequest.pool.Pop();
		}

		// Token: 0x06001B0B RID: 6923 RVA: 0x0007E590 File Offset: 0x0007C790
		public void Release()
		{
			this.Characters = null;
			this.FontSize = 0;
			this.FontStyle = FontStyle.Normal;
			dfDynamicFont.FontCharacterRequest.pool.Add(this);
		}

		// Token: 0x0400153F RID: 5439
		private static dfList<dfDynamicFont.FontCharacterRequest> pool = new dfList<dfDynamicFont.FontCharacterRequest>();

		// Token: 0x04001540 RID: 5440
		public string Characters;

		// Token: 0x04001541 RID: 5441
		public int FontSize;

		// Token: 0x04001542 RID: 5442
		public FontStyle FontStyle;
	}

	// Token: 0x02000495 RID: 1173
	public class DynamicFontRenderer : dfFontRendererBase, IPoolable
	{
		// Token: 0x06001B0D RID: 6925 RVA: 0x0007E5C0 File Offset: 0x0007C7C0
		internal DynamicFontRenderer()
		{
		}

		// Token: 0x17000592 RID: 1426
		// (get) Token: 0x06001B0E RID: 6926 RVA: 0x0007E5C8 File Offset: 0x0007C7C8
		public int LineCount
		{
			get
			{
				return this.lines.Count;
			}
		}

		// Token: 0x17000593 RID: 1427
		// (get) Token: 0x06001B0F RID: 6927 RVA: 0x0007E5D8 File Offset: 0x0007C7D8
		// (set) Token: 0x06001B10 RID: 6928 RVA: 0x0007E5E0 File Offset: 0x0007C7E0
		public dfAtlas SpriteAtlas { get; set; }

		// Token: 0x17000594 RID: 1428
		// (get) Token: 0x06001B11 RID: 6929 RVA: 0x0007E5EC File Offset: 0x0007C7EC
		// (set) Token: 0x06001B12 RID: 6930 RVA: 0x0007E5F4 File Offset: 0x0007C7F4
		public dfRenderData SpriteBuffer { get; set; }

		// Token: 0x06001B13 RID: 6931 RVA: 0x0007E600 File Offset: 0x0007C800
		public static dfFontRendererBase Obtain(dfDynamicFont font)
		{
			dfDynamicFont.DynamicFontRenderer dynamicFontRenderer = ((dfDynamicFont.DynamicFontRenderer.objectPool.Count <= 0) ? new dfDynamicFont.DynamicFontRenderer() : dfDynamicFont.DynamicFontRenderer.objectPool.Dequeue());
			dynamicFontRenderer.Reset();
			dynamicFontRenderer.Font = font;
			dynamicFontRenderer.inUse = true;
			return dynamicFontRenderer;
		}

		// Token: 0x06001B14 RID: 6932 RVA: 0x0007E648 File Offset: 0x0007C848
		public override void Release()
		{
			if (!this.inUse)
			{
				return;
			}
			this.inUse = false;
			this.Reset();
			if (this.tokens != null)
			{
				this.tokens.Release();
				this.tokens = null;
			}
			if (this.lines != null)
			{
				this.lines.ReleaseItems();
				this.lines.Release();
				this.lines = null;
			}
			base.BottomColor = null;
			dfDynamicFont.DynamicFontRenderer.objectPool.Enqueue(this);
		}

		// Token: 0x06001B15 RID: 6933 RVA: 0x0007E6D0 File Offset: 0x0007C8D0
		public override float[] GetCharacterWidths(string text)
		{
			float num = 0f;
			return this.GetCharacterWidths(text, 0, text.Length - 1, out num);
		}

		// Token: 0x06001B16 RID: 6934 RVA: 0x0007E6F8 File Offset: 0x0007C8F8
		public float[] GetCharacterWidths(string text, int startIndex, int endIndex, out float totalWidth)
		{
			totalWidth = 0f;
			dfDynamicFont dfDynamicFont = (dfDynamicFont)base.Font;
			int num = Mathf.CeilToInt((float)dfDynamicFont.FontSize * base.TextScale);
			float[] array = new float[text.Length];
			float num2 = 0f;
			float num3 = 0f;
			dfDynamicFont.RequestCharacters(text, num, FontStyle.Normal);
			CharacterInfo characterInfo = default(CharacterInfo);
			int i = startIndex;
			while (i <= endIndex)
			{
				if (dfDynamicFont.BaseFont.GetCharacterInfo(text[i], out characterInfo, num, FontStyle.Normal))
				{
					if (text[i] == '\t')
					{
						num3 += (float)base.TabSize;
					}
					else if (text[i] == ' ')
					{
						num3 += (float)characterInfo.advance;
					}
					else
					{
						num3 += (float)characterInfo.maxX;
					}
					array[i] = (num3 - num2) * base.PixelRatio;
				}
				i++;
				num2 = num3;
			}
			return array;
		}

		// Token: 0x06001B17 RID: 6935 RVA: 0x0007E7F0 File Offset: 0x0007C9F0
		public override Vector2 MeasureString(string text)
		{
			dfDynamicFont dfDynamicFont = (dfDynamicFont)base.Font;
			int num = Mathf.CeilToInt((float)dfDynamicFont.FontSize * base.TextScale);
			dfDynamicFont.RequestCharacters(text, num, FontStyle.Normal);
			this.tokenize(text);
			dfList<dfDynamicFont.LineRenderInfo> dfList = this.calculateLinebreaks();
			float num2 = 0f;
			float num3 = 0f;
			for (int i = 0; i < dfList.Count; i++)
			{
				num2 = Mathf.Max(dfList[i].lineWidth, num2);
				num3 += dfList[i].lineHeight;
			}
			this.tokens.Release();
			this.tokens = null;
			Vector2 vector = new Vector2(num2, num3);
			return vector;
		}

		// Token: 0x06001B18 RID: 6936 RVA: 0x0007E8A0 File Offset: 0x0007CAA0
		public override void Render(string text, dfRenderData destination)
		{
			dfDynamicFont.DynamicFontRenderer.textColors.Clear();
			dfDynamicFont.DynamicFontRenderer.textColors.Push(Color.white);
			dfDynamicFont dfDynamicFont = (dfDynamicFont)base.Font;
			int num = Mathf.CeilToInt((float)dfDynamicFont.FontSize * base.TextScale);
			dfDynamicFont.RequestCharacters(text, num, FontStyle.Normal);
			this.tokenize(text);
			dfList<dfDynamicFont.LineRenderInfo> dfList = this.calculateLinebreaks();
			destination.EnsureCapacity(this.getAnticipatedVertCount(this.tokens));
			int num2 = 0;
			int num3 = 0;
			Vector3 vector = (base.VectorOffset / base.PixelRatio).CeilToInt();
			for (int i = 0; i < dfList.Count; i++)
			{
				dfDynamicFont.LineRenderInfo lineRenderInfo = dfList[i];
				int count = destination.Vertices.Count;
				int num4 = ((this.SpriteBuffer == null) ? 0 : this.SpriteBuffer.Vertices.Count);
				this.renderLine(dfList[i], dfDynamicFont.DynamicFontRenderer.textColors, vector, destination);
				vector.y -= lineRenderInfo.lineHeight;
				num2 = Mathf.Max((int)lineRenderInfo.lineWidth, num2);
				num3 += Mathf.CeilToInt(lineRenderInfo.lineHeight);
				if (lineRenderInfo.lineWidth > base.MaxSize.x)
				{
					this.clipRight(destination, count);
					this.clipRight(this.SpriteBuffer, num4);
				}
				this.clipBottom(destination, count);
				this.clipBottom(this.SpriteBuffer, num4);
			}
			base.RenderedSize = new Vector2(Mathf.Min(base.MaxSize.x, (float)num2), Mathf.Min(base.MaxSize.y, (float)num3)) * base.TextScale;
			this.tokens.Release();
			this.tokens = null;
		}

		// Token: 0x06001B19 RID: 6937 RVA: 0x0007EA70 File Offset: 0x0007CC70
		private int getAnticipatedVertCount(dfList<dfMarkupToken> tokens)
		{
			int num = 4 + ((!base.Shadow) ? 0 : 4) + ((!base.Outline) ? 0 : 4);
			int num2 = 0;
			for (int i = 0; i < tokens.Count; i++)
			{
				dfMarkupToken dfMarkupToken = tokens[i];
				if (dfMarkupToken.TokenType == dfMarkupTokenType.Text)
				{
					num2 += num * dfMarkupToken.Length;
				}
				else if (dfMarkupToken.TokenType == dfMarkupTokenType.StartTag)
				{
					num2 += 4;
				}
			}
			return num2;
		}

		// Token: 0x06001B1A RID: 6938 RVA: 0x0007EAF4 File Offset: 0x0007CCF4
		private void renderLine(dfDynamicFont.LineRenderInfo line, Stack<Color32> colors, Vector3 position, dfRenderData destination)
		{
			position.x += (float)this.calculateLineAlignment(line);
			for (int i = line.startOffset; i <= line.endOffset; i++)
			{
				dfMarkupToken dfMarkupToken = this.tokens[i];
				dfMarkupTokenType tokenType = dfMarkupToken.TokenType;
				if (tokenType == dfMarkupTokenType.Text)
				{
					this.renderText(dfMarkupToken, colors.Peek(), position, destination);
				}
				else if (tokenType == dfMarkupTokenType.StartTag)
				{
					if (dfMarkupToken.Matches("sprite") && this.SpriteAtlas != null && this.SpriteBuffer != null)
					{
						this.renderSprite(dfMarkupToken, colors.Peek(), position, this.SpriteBuffer);
					}
					else if (dfMarkupToken.Matches("color"))
					{
						colors.Push(this.parseColor(dfMarkupToken));
					}
				}
				else if (tokenType == dfMarkupTokenType.EndTag && dfMarkupToken.Matches("color") && colors.Count > 1)
				{
					colors.Pop();
				}
				position.x += (float)dfMarkupToken.Width;
				if (dfMarkupToken.Width > 0)
				{
					position += base.PerCharacterAccumulatedOffset;
				}
			}
		}

		// Token: 0x06001B1B RID: 6939 RVA: 0x0007EC2C File Offset: 0x0007CE2C
		private void renderText(dfMarkupToken token, Color32 color, Vector3 position, dfRenderData renderData)
		{
			try
			{
				dfDynamicFont dfDynamicFont = (dfDynamicFont)base.Font;
				int num = Mathf.CeilToInt((float)dfDynamicFont.FontSize * base.TextScale);
				FontStyle fontStyle = FontStyle.Normal;
				CharacterInfo characterInfo = default(CharacterInfo);
				int descent = dfDynamicFont.Descent;
				dfList<Vector3> vertices = renderData.Vertices;
				dfList<int> triangles = renderData.Triangles;
				dfList<Vector2> uv = renderData.UV;
				dfList<Color32> colors = renderData.Colors;
				float num2 = position.x;
				float y = position.y;
				renderData.Material = dfDynamicFont.Material;
				Color32 color2 = this.applyOpacity(this.multiplyColors(color, base.DefaultColor));
				Color32 color3 = color2;
				if (base.BottomColor != null)
				{
					color3 = this.applyOpacity(this.multiplyColors(color, base.BottomColor.Value));
				}
				for (int i = 0; i < token.Length; i++)
				{
					if (i > 0)
					{
						num2 += (float)base.CharacterSpacing * base.TextScale;
					}
					if (dfDynamicFont.baseFont.GetCharacterInfo(token[i], out characterInfo, num, fontStyle))
					{
						int num3 = dfDynamicFont.FontSize + characterInfo.maxY - num + descent;
						float num4 = num2 + (float)characterInfo.minX;
						float num5 = y + (float)num3;
						float num6 = num4 + (float)characterInfo.glyphWidth;
						float num7 = num5 - (float)characterInfo.glyphHeight;
						Vector3 vector = new Vector3(num4, num5) * base.PixelRatio;
						Vector3 vector2 = new Vector3(num6, num5) * base.PixelRatio;
						Vector3 vector3 = new Vector3(num6, num7) * base.PixelRatio;
						Vector3 vector4 = new Vector3(num4, num7) * base.PixelRatio;
						if (base.Shadow)
						{
							dfDynamicFont.DynamicFontRenderer.addTriangleIndices(vertices, triangles);
							Vector3 vector5 = base.ShadowOffset * base.PixelRatio;
							vertices.Add(vector + vector5);
							vertices.Add(vector2 + vector5);
							vertices.Add(vector3 + vector5);
							vertices.Add(vector4 + vector5);
							Color32 color4 = this.applyOpacity(base.ShadowColor);
							colors.Add(color4);
							colors.Add(color4);
							colors.Add(color4);
							colors.Add(color4);
							dfDynamicFont.DynamicFontRenderer.addUVCoords(uv, characterInfo);
						}
						if (base.Outline)
						{
							for (int j = 0; j < dfDynamicFont.DynamicFontRenderer.OUTLINE_OFFSETS.Length; j++)
							{
								dfDynamicFont.DynamicFontRenderer.addTriangleIndices(vertices, triangles);
								Vector3 vector6 = dfDynamicFont.DynamicFontRenderer.OUTLINE_OFFSETS[j] * (float)base.OutlineSize * base.PixelRatio;
								vertices.Add(vector + vector6);
								vertices.Add(vector2 + vector6);
								vertices.Add(vector3 + vector6);
								vertices.Add(vector4 + vector6);
								Color32 color5 = this.applyOpacity(base.OutlineColor);
								colors.Add(color5);
								colors.Add(color5);
								colors.Add(color5);
								colors.Add(color5);
								dfDynamicFont.DynamicFontRenderer.addUVCoords(uv, characterInfo);
							}
						}
						dfDynamicFont.DynamicFontRenderer.addTriangleIndices(vertices, triangles);
						vertices.Add(vector);
						vertices.Add(vector2);
						vertices.Add(vector3);
						vertices.Add(vector4);
						colors.Add(color2);
						colors.Add(color2);
						colors.Add(color3);
						colors.Add(color3);
						dfDynamicFont.DynamicFontRenderer.addUVCoords(uv, characterInfo);
						num2 += (float)Mathf.CeilToInt((float)characterInfo.maxX);
					}
				}
			}
			finally
			{
			}
		}

		// Token: 0x06001B1C RID: 6940 RVA: 0x0007F018 File Offset: 0x0007D218
		private static void addUVCoords(dfList<Vector2> uvs, CharacterInfo glyph)
		{
			uvs.Add(glyph.uvTopLeft);
			uvs.Add(glyph.uvTopRight);
			uvs.Add(glyph.uvBottomRight);
			uvs.Add(glyph.uvBottomLeft);
		}

		// Token: 0x06001B1D RID: 6941 RVA: 0x0007F050 File Offset: 0x0007D250
		private void renderSprite(dfMarkupToken token, Color32 color, Vector3 position, dfRenderData destination)
		{
			try
			{
				string value = token.GetAttribute(0).Value.Value;
				dfAtlas.ItemInfo itemInfo = this.SpriteAtlas[value];
				if (!(itemInfo == null))
				{
					dfSprite.RenderOptions renderOptions = new dfSprite.RenderOptions
					{
						atlas = this.SpriteAtlas,
						color = color,
						fillAmount = 1f,
						flip = dfSpriteFlip.None,
						offset = position,
						pixelsToUnits = base.PixelRatio,
						size = new Vector2((float)token.Width, (float)token.Height),
						spriteInfo = itemInfo
					};
					dfSprite.renderSprite(this.SpriteBuffer, renderOptions);
				}
			}
			finally
			{
			}
		}

		// Token: 0x06001B1E RID: 6942 RVA: 0x0007F11C File Offset: 0x0007D31C
		private Color32 parseColor(dfMarkupToken token)
		{
			Color color = Color.white;
			if (token.AttributeCount == 1)
			{
				string value = token.GetAttribute(0).Value.Value;
				if (value.Length == 7 && value[0] == '#')
				{
					uint num = 0U;
					uint.TryParse(value.Substring(1), NumberStyles.HexNumber, null, out num);
					color = this.UIntToColor(num | 4278190080U);
				}
				else
				{
					color = dfMarkupStyle.ParseColor(value, base.DefaultColor);
				}
			}
			return this.applyOpacity(color);
		}

		// Token: 0x06001B1F RID: 6943 RVA: 0x0007F1B4 File Offset: 0x0007D3B4
		private Color32 UIntToColor(uint color)
		{
			byte b = (byte)(color >> 24);
			byte b2 = (byte)(color >> 16);
			byte b3 = (byte)(color >> 8);
			byte b4 = (byte)(color >> 0);
			return new Color32(b2, b3, b4, b);
		}

		// Token: 0x06001B20 RID: 6944 RVA: 0x0007F1E0 File Offset: 0x0007D3E0
		private dfList<dfDynamicFont.LineRenderInfo> calculateLinebreaks()
		{
			dfList<dfDynamicFont.LineRenderInfo> dfList;
			try
			{
				if (this.lines != null)
				{
					dfList = this.lines;
				}
				else
				{
					this.lines = dfList<dfDynamicFont.LineRenderInfo>.Obtain();
					dfDynamicFont dfDynamicFont = (dfDynamicFont)base.Font;
					int num = 0;
					int num2 = 0;
					int num3 = 0;
					int num4 = 0;
					float num5 = (float)dfDynamicFont.Baseline * base.TextScale;
					while (num3 < this.tokens.Count && (float)this.lines.Count * num5 <= base.MaxSize.y + num5)
					{
						dfMarkupToken dfMarkupToken = this.tokens[num3];
						dfMarkupTokenType tokenType = dfMarkupToken.TokenType;
						if (tokenType == dfMarkupTokenType.Newline)
						{
							this.lines.Add(dfDynamicFont.LineRenderInfo.Obtain(num2, num3));
							num = (num2 = ++num3);
							num4 = 0;
						}
						else
						{
							int num6 = Mathf.CeilToInt((float)dfMarkupToken.Width);
							bool flag = base.WordWrap && num > num2 && (tokenType == dfMarkupTokenType.Text || (tokenType == dfMarkupTokenType.StartTag && dfMarkupToken.Matches("sprite")));
							if (flag && (float)(num4 + num6) >= base.MaxSize.x)
							{
								if (num > num2)
								{
									this.lines.Add(dfDynamicFont.LineRenderInfo.Obtain(num2, num - 1));
									num3 = (num2 = ++num);
									num4 = 0;
								}
								else
								{
									this.lines.Add(dfDynamicFont.LineRenderInfo.Obtain(num2, num - 1));
									num = (num2 = ++num3);
									num4 = 0;
								}
							}
							else
							{
								if (tokenType == dfMarkupTokenType.Whitespace)
								{
									num = num3;
								}
								num4 += num6;
								num3++;
							}
						}
					}
					if (num2 < this.tokens.Count)
					{
						this.lines.Add(dfDynamicFont.LineRenderInfo.Obtain(num2, this.tokens.Count - 1));
					}
					for (int i = 0; i < this.lines.Count; i++)
					{
						this.calculateLineSize(this.lines[i]);
					}
					dfList = this.lines;
				}
			}
			finally
			{
			}
			return dfList;
		}

		// Token: 0x06001B21 RID: 6945 RVA: 0x0007F418 File Offset: 0x0007D618
		private int calculateLineAlignment(dfDynamicFont.LineRenderInfo line)
		{
			float lineWidth = line.lineWidth;
			if (base.TextAlign == TextAlignment.Left || lineWidth < 1f)
			{
				return 0;
			}
			float num;
			if (base.TextAlign == TextAlignment.Right)
			{
				num = base.MaxSize.x - lineWidth;
			}
			else
			{
				num = (base.MaxSize.x - lineWidth) * 0.5f;
			}
			return Mathf.CeilToInt(Mathf.Max(0f, num));
		}

		// Token: 0x06001B22 RID: 6946 RVA: 0x0007F494 File Offset: 0x0007D694
		private void calculateLineSize(dfDynamicFont.LineRenderInfo line)
		{
			dfDynamicFont dfDynamicFont = (dfDynamicFont)base.Font;
			line.lineHeight = (float)dfDynamicFont.Baseline * base.TextScale;
			int num = 0;
			for (int i = line.startOffset; i <= line.endOffset; i++)
			{
				num += this.tokens[i].Width;
			}
			line.lineWidth = (float)num;
		}

		// Token: 0x06001B23 RID: 6947 RVA: 0x0007F4FC File Offset: 0x0007D6FC
		private void tokenize(string text)
		{
			try
			{
				if (base.ProcessMarkup)
				{
					this.tokens = dfMarkupTokenizer.Tokenize(text);
				}
				else
				{
					this.tokens = dfPlainTextTokenizer.Tokenize(text);
				}
				for (int i = 0; i < this.tokens.Count; i++)
				{
					this.calculateTokenRenderSize(this.tokens[i]);
				}
			}
			finally
			{
			}
		}

		// Token: 0x06001B24 RID: 6948 RVA: 0x0007F578 File Offset: 0x0007D778
		private void calculateTokenRenderSize(dfMarkupToken token)
		{
			try
			{
				float num = 0f;
				dfDynamicFont dfDynamicFont = (dfDynamicFont)base.Font;
				CharacterInfo characterInfo = default(CharacterInfo);
				if (token.TokenType == dfMarkupTokenType.Text)
				{
					int num2 = Mathf.CeilToInt((float)dfDynamicFont.FontSize * base.TextScale);
					for (int i = 0; i < token.Length; i++)
					{
						char c = token[i];
						dfDynamicFont.baseFont.GetCharacterInfo(c, out characterInfo, num2, FontStyle.Normal);
						if (c == '\t')
						{
							num += (float)base.TabSize;
						}
						else
						{
							num += ((c == ' ') ? ((float)characterInfo.advance + (float)base.CharacterSpacing * base.TextScale) : ((float)characterInfo.maxX));
						}
					}
					if (token.Length > 2)
					{
						num += (float)((token.Length - 2) * base.CharacterSpacing) * base.TextScale;
					}
					token.Height = base.Font.LineHeight;
					token.Width = Mathf.CeilToInt(num);
				}
				else if (token.TokenType == dfMarkupTokenType.Whitespace)
				{
					int num3 = Mathf.CeilToInt((float)dfDynamicFont.FontSize * base.TextScale);
					float num4 = (float)base.CharacterSpacing * base.TextScale;
					for (int j = 0; j < token.Length; j++)
					{
						char c = token[j];
						if (c == '\t')
						{
							num += (float)base.TabSize;
						}
						else if (c == ' ')
						{
							dfDynamicFont.baseFont.GetCharacterInfo(c, out characterInfo, num3, FontStyle.Normal);
							num += (float)characterInfo.advance + num4;
						}
					}
					token.Height = base.Font.LineHeight;
					token.Width = Mathf.CeilToInt(num);
				}
				else if (token.TokenType == dfMarkupTokenType.StartTag && token.Matches("sprite") && this.SpriteAtlas != null && token.AttributeCount == 1)
				{
					Texture2D texture = this.SpriteAtlas.Texture;
					float num5 = (float)dfDynamicFont.Baseline * base.TextScale;
					string value = token.GetAttribute(0).Value.Value;
					dfAtlas.ItemInfo itemInfo = this.SpriteAtlas[value];
					if (itemInfo != null)
					{
						float num6 = itemInfo.region.width * (float)texture.width / (itemInfo.region.height * (float)texture.height);
						num = (float)Mathf.CeilToInt(num5 * num6);
					}
					token.Height = Mathf.CeilToInt(num5);
					token.Width = Mathf.CeilToInt(num);
				}
			}
			finally
			{
			}
		}

		// Token: 0x06001B25 RID: 6949 RVA: 0x0007F838 File Offset: 0x0007DA38
		private float getTabStop(float position)
		{
			float num = base.PixelRatio * base.TextScale;
			if (base.TabStops != null && base.TabStops.Count > 0)
			{
				for (int i = 0; i < base.TabStops.Count; i++)
				{
					if ((float)base.TabStops[i] * num > position)
					{
						return (float)base.TabStops[i] * num;
					}
				}
			}
			if (base.TabSize > 0)
			{
				return position + (float)base.TabSize * num;
			}
			return position + (float)(base.Font.FontSize * 4) * num;
		}

		// Token: 0x06001B26 RID: 6950 RVA: 0x0007F8DC File Offset: 0x0007DADC
		private void clipRight(dfRenderData destination, int startIndex)
		{
			if (destination == null)
			{
				return;
			}
			float num = base.VectorOffset.x + base.MaxSize.x * base.PixelRatio;
			dfList<Vector3> vertices = destination.Vertices;
			dfList<Vector2> uv = destination.UV;
			for (int i = startIndex; i < vertices.Count; i += 4)
			{
				Vector3 vector = vertices[i];
				Vector3 vector2 = vertices[i + 1];
				Vector3 vector3 = vertices[i + 2];
				Vector3 vector4 = vertices[i + 3];
				float num2 = vector2.x - vector.x;
				if (vector2.x > num)
				{
					float num3 = 1f - (num - vector2.x + num2) / num2;
					dfList<Vector3> dfList = vertices;
					int num4 = i;
					vector = new Vector3(Mathf.Min(vector.x, num), vector.y, vector.z);
					dfList[num4] = vector;
					dfList<Vector3> dfList2 = vertices;
					int num5 = i + 1;
					vector2 = new Vector3(Mathf.Min(vector2.x, num), vector2.y, vector2.z);
					dfList2[num5] = vector2;
					dfList<Vector3> dfList3 = vertices;
					int num6 = i + 2;
					vector3 = new Vector3(Mathf.Min(vector3.x, num), vector3.y, vector3.z);
					dfList3[num6] = vector3;
					dfList<Vector3> dfList4 = vertices;
					int num7 = i + 3;
					vector4 = new Vector3(Mathf.Min(vector4.x, num), vector4.y, vector4.z);
					dfList4[num7] = vector4;
					float num8 = Mathf.Lerp(uv[i + 1].x, uv[i].x, num3);
					uv[i + 1] = new Vector2(num8, uv[i + 1].y);
					uv[i + 2] = new Vector2(num8, uv[i + 2].y);
					num2 = vector2.x - vector.x;
				}
			}
		}

		// Token: 0x06001B27 RID: 6951 RVA: 0x0007FAE4 File Offset: 0x0007DCE4
		private void clipBottom(dfRenderData destination, int startIndex)
		{
			if (destination == null)
			{
				return;
			}
			float num = base.VectorOffset.y - base.MaxSize.y * base.PixelRatio;
			dfList<Vector3> vertices = destination.Vertices;
			dfList<Vector2> uv = destination.UV;
			dfList<Color32> colors = destination.Colors;
			for (int i = startIndex; i < vertices.Count; i += 4)
			{
				Vector3 vector = vertices[i];
				Vector3 vector2 = vertices[i + 1];
				Vector3 vector3 = vertices[i + 2];
				Vector3 vector4 = vertices[i + 3];
				float num2 = vector.y - vector4.y;
				if (vector4.y <= num)
				{
					float num3 = 1f - Mathf.Abs(-num + vector.y) / num2;
					dfList<Vector3> dfList = vertices;
					int num4 = i;
					vector = new Vector3(vector.x, Mathf.Max(vector.y, num), vector2.z);
					dfList[num4] = vector;
					dfList<Vector3> dfList2 = vertices;
					int num5 = i + 1;
					vector2 = new Vector3(vector2.x, Mathf.Max(vector2.y, num), vector2.z);
					dfList2[num5] = vector2;
					dfList<Vector3> dfList3 = vertices;
					int num6 = i + 2;
					vector3 = new Vector3(vector3.x, Mathf.Max(vector3.y, num), vector3.z);
					dfList3[num6] = vector3;
					dfList<Vector3> dfList4 = vertices;
					int num7 = i + 3;
					vector4 = new Vector3(vector4.x, Mathf.Max(vector4.y, num), vector4.z);
					dfList4[num7] = vector4;
					uv[i + 3] = Vector2.Lerp(uv[i + 3], uv[i], num3);
					uv[i + 2] = Vector2.Lerp(uv[i + 2], uv[i + 1], num3);
					Color color = Color.Lerp(colors[i + 3], colors[i], num3);
					colors[i + 3] = color;
					colors[i + 2] = color;
				}
			}
		}

		// Token: 0x06001B28 RID: 6952 RVA: 0x0007FD04 File Offset: 0x0007DF04
		private Color32 applyOpacity(Color32 color)
		{
			color.a = (byte)(base.Opacity * 255f);
			return color;
		}

		// Token: 0x06001B29 RID: 6953 RVA: 0x0007FD1C File Offset: 0x0007DF1C
		private static void addTriangleIndices(dfList<Vector3> verts, dfList<int> triangles)
		{
			int count = verts.Count;
			int[] triangle_INDICES = dfDynamicFont.DynamicFontRenderer.TRIANGLE_INDICES;
			for (int i = 0; i < triangle_INDICES.Length; i++)
			{
				triangles.Add(count + triangle_INDICES[i]);
			}
		}

		// Token: 0x06001B2A RID: 6954 RVA: 0x0007FD58 File Offset: 0x0007DF58
		private Color multiplyColors(Color lhs, Color rhs)
		{
			return new Color(lhs.r * rhs.r, lhs.g * rhs.g, lhs.b * rhs.b, lhs.a * rhs.a);
		}

		// Token: 0x04001543 RID: 5443
		private static Queue<dfDynamicFont.DynamicFontRenderer> objectPool = new Queue<dfDynamicFont.DynamicFontRenderer>();

		// Token: 0x04001544 RID: 5444
		private static Vector2[] OUTLINE_OFFSETS = new Vector2[]
		{
			new Vector2(-1f, -1f),
			new Vector2(-1f, 1f),
			new Vector2(1f, -1f),
			new Vector2(1f, 1f)
		};

		// Token: 0x04001545 RID: 5445
		private static int[] TRIANGLE_INDICES = new int[] { 0, 1, 3, 3, 1, 2 };

		// Token: 0x04001546 RID: 5446
		private static Stack<Color32> textColors = new Stack<Color32>();

		// Token: 0x04001549 RID: 5449
		private dfList<dfDynamicFont.LineRenderInfo> lines;

		// Token: 0x0400154A RID: 5450
		private dfList<dfMarkupToken> tokens;

		// Token: 0x0400154B RID: 5451
		private bool inUse;
	}

	// Token: 0x02000496 RID: 1174
	private class LineRenderInfo : IPoolable
	{
		// Token: 0x06001B2C RID: 6956 RVA: 0x0007FE58 File Offset: 0x0007E058
		private LineRenderInfo()
		{
		}

		// Token: 0x17000595 RID: 1429
		// (get) Token: 0x06001B2D RID: 6957 RVA: 0x0007FE60 File Offset: 0x0007E060
		public int length
		{
			get
			{
				return this.endOffset - this.startOffset + 1;
			}
		}

		// Token: 0x06001B2E RID: 6958 RVA: 0x0007FE74 File Offset: 0x0007E074
		public static dfDynamicFont.LineRenderInfo Obtain(int start, int end)
		{
			dfDynamicFont.LineRenderInfo lineRenderInfo = ((dfDynamicFont.LineRenderInfo.pool.Count <= 0) ? new dfDynamicFont.LineRenderInfo() : dfDynamicFont.LineRenderInfo.pool.Pop());
			lineRenderInfo.startOffset = start;
			lineRenderInfo.endOffset = end;
			lineRenderInfo.lineHeight = 0f;
			return lineRenderInfo;
		}

		// Token: 0x06001B2F RID: 6959 RVA: 0x0007FEC0 File Offset: 0x0007E0C0
		public void Release()
		{
			this.startOffset = (this.endOffset = 0);
			this.lineWidth = (this.lineHeight = 0f);
			dfDynamicFont.LineRenderInfo.pool.Add(this);
		}

		// Token: 0x0400154C RID: 5452
		public int startOffset;

		// Token: 0x0400154D RID: 5453
		public int endOffset;

		// Token: 0x0400154E RID: 5454
		public float lineWidth;

		// Token: 0x0400154F RID: 5455
		public float lineHeight;

		// Token: 0x04001550 RID: 5456
		private static dfList<dfDynamicFont.LineRenderInfo> pool = new dfList<dfDynamicFont.LineRenderInfo>();
	}
}
