using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

// Token: 0x020003C3 RID: 963
[AddComponentMenu("Daikon Forge/User Interface/Font Definition")]
[dfHelp("http://www.daikonforge.com/docs/df-gui/classdf_font.html")]
[Serializable]
public class dfFont : dfFontBase
{
	// Token: 0x170003E4 RID: 996
	// (get) Token: 0x0600122B RID: 4651 RVA: 0x000533FC File Offset: 0x000515FC
	public List<dfFont.GlyphDefinition> Glyphs
	{
		get
		{
			return this.glyphs;
		}
	}

	// Token: 0x170003E5 RID: 997
	// (get) Token: 0x0600122C RID: 4652 RVA: 0x00053404 File Offset: 0x00051604
	public List<dfFont.GlyphKerning> KerningInfo
	{
		get
		{
			return this.kerning;
		}
	}

	// Token: 0x170003E6 RID: 998
	// (get) Token: 0x0600122D RID: 4653 RVA: 0x0005340C File Offset: 0x0005160C
	// (set) Token: 0x0600122E RID: 4654 RVA: 0x00053414 File Offset: 0x00051614
	public dfAtlas Atlas
	{
		get
		{
			return this.atlas;
		}
		set
		{
			if (value != this.atlas)
			{
				this.atlas = value;
				this.glyphMap = null;
			}
		}
	}

	// Token: 0x170003E7 RID: 999
	// (get) Token: 0x0600122F RID: 4655 RVA: 0x00053438 File Offset: 0x00051638
	// (set) Token: 0x06001230 RID: 4656 RVA: 0x00053448 File Offset: 0x00051648
	public override Material Material
	{
		get
		{
			return this.Atlas.Material;
		}
		set
		{
			throw new InvalidOperationException();
		}
	}

	// Token: 0x170003E8 RID: 1000
	// (get) Token: 0x06001231 RID: 4657 RVA: 0x00053450 File Offset: 0x00051650
	public override Texture Texture
	{
		get
		{
			return this.Atlas.Texture;
		}
	}

	// Token: 0x170003E9 RID: 1001
	// (get) Token: 0x06001232 RID: 4658 RVA: 0x00053460 File Offset: 0x00051660
	// (set) Token: 0x06001233 RID: 4659 RVA: 0x00053468 File Offset: 0x00051668
	public string Sprite
	{
		get
		{
			return this.sprite;
		}
		set
		{
			if (value != this.sprite)
			{
				this.sprite = value;
				this.glyphMap = null;
			}
		}
	}

	// Token: 0x170003EA RID: 1002
	// (get) Token: 0x06001234 RID: 4660 RVA: 0x0005348C File Offset: 0x0005168C
	public override bool IsValid
	{
		get
		{
			return !(this.Atlas == null) && !(this.Atlas[this.Sprite] == null);
		}
	}

	// Token: 0x170003EB RID: 1003
	// (get) Token: 0x06001235 RID: 4661 RVA: 0x000534C0 File Offset: 0x000516C0
	public string FontFace
	{
		get
		{
			return this.face;
		}
	}

	// Token: 0x170003EC RID: 1004
	// (get) Token: 0x06001236 RID: 4662 RVA: 0x000534C8 File Offset: 0x000516C8
	// (set) Token: 0x06001237 RID: 4663 RVA: 0x000534D0 File Offset: 0x000516D0
	public override int FontSize
	{
		get
		{
			return this.size;
		}
		set
		{
			throw new InvalidOperationException();
		}
	}

	// Token: 0x170003ED RID: 1005
	// (get) Token: 0x06001238 RID: 4664 RVA: 0x000534D8 File Offset: 0x000516D8
	// (set) Token: 0x06001239 RID: 4665 RVA: 0x000534E0 File Offset: 0x000516E0
	public override int LineHeight
	{
		get
		{
			return this.lineHeight;
		}
		set
		{
			this.lineHeight = value;
		}
	}

	// Token: 0x170003EE RID: 1006
	// (get) Token: 0x0600123A RID: 4666 RVA: 0x000534EC File Offset: 0x000516EC
	public bool Bold
	{
		get
		{
			return this.bold;
		}
	}

	// Token: 0x170003EF RID: 1007
	// (get) Token: 0x0600123B RID: 4667 RVA: 0x000534F4 File Offset: 0x000516F4
	public bool Italic
	{
		get
		{
			return this.italic;
		}
	}

	// Token: 0x170003F0 RID: 1008
	// (get) Token: 0x0600123C RID: 4668 RVA: 0x000534FC File Offset: 0x000516FC
	public int[] Padding
	{
		get
		{
			return this.padding;
		}
	}

	// Token: 0x170003F1 RID: 1009
	// (get) Token: 0x0600123D RID: 4669 RVA: 0x00053504 File Offset: 0x00051704
	public int[] Spacing
	{
		get
		{
			return this.spacing;
		}
	}

	// Token: 0x170003F2 RID: 1010
	// (get) Token: 0x0600123E RID: 4670 RVA: 0x0005350C File Offset: 0x0005170C
	public int Outline
	{
		get
		{
			return this.outline;
		}
	}

	// Token: 0x170003F3 RID: 1011
	// (get) Token: 0x0600123F RID: 4671 RVA: 0x00053514 File Offset: 0x00051714
	public int Count
	{
		get
		{
			return this.glyphs.Count;
		}
	}

	// Token: 0x06001240 RID: 4672 RVA: 0x00053524 File Offset: 0x00051724
	public void OnEnable()
	{
		this.glyphMap = null;
	}

	// Token: 0x06001241 RID: 4673 RVA: 0x00053530 File Offset: 0x00051730
	public override dfFontRendererBase ObtainRenderer()
	{
		return dfFont.BitmappedFontRenderer.Obtain(this);
	}

	// Token: 0x06001242 RID: 4674 RVA: 0x00053538 File Offset: 0x00051738
	public void AddKerning(int first, int second, int amount)
	{
		this.kerning.Add(new dfFont.GlyphKerning
		{
			first = first,
			second = second,
			amount = amount
		});
	}

	// Token: 0x06001243 RID: 4675 RVA: 0x0005356C File Offset: 0x0005176C
	public int GetKerning(char previousChar, char currentChar)
	{
		int num;
		try
		{
			if (this.kerningMap == null)
			{
				this.buildKerningMap();
			}
			dfFont.GlyphKerningList glyphKerningList = null;
			if (!this.kerningMap.TryGetValue((int)previousChar, out glyphKerningList))
			{
				num = 0;
			}
			else
			{
				num = glyphKerningList.GetKerning((int)previousChar, (int)currentChar);
			}
		}
		finally
		{
		}
		return num;
	}

	// Token: 0x06001244 RID: 4676 RVA: 0x000535C8 File Offset: 0x000517C8
	private void buildKerningMap()
	{
		Dictionary<int, dfFont.GlyphKerningList> dictionary = (this.kerningMap = new Dictionary<int, dfFont.GlyphKerningList>());
		for (int i = 0; i < this.kerning.Count; i++)
		{
			dfFont.GlyphKerning glyphKerning = this.kerning[i];
			if (!dictionary.ContainsKey(glyphKerning.first))
			{
				dictionary[glyphKerning.first] = new dfFont.GlyphKerningList();
			}
			dfFont.GlyphKerningList glyphKerningList = dictionary[glyphKerning.first];
			glyphKerningList.Add(glyphKerning);
		}
	}

	// Token: 0x06001245 RID: 4677 RVA: 0x00053648 File Offset: 0x00051848
	public dfFont.GlyphDefinition GetGlyph(char id)
	{
		if (this.glyphMap == null)
		{
			this.glyphMap = new Dictionary<int, dfFont.GlyphDefinition>();
			for (int i = 0; i < this.glyphs.Count; i++)
			{
				dfFont.GlyphDefinition glyphDefinition = this.glyphs[i];
				this.glyphMap[glyphDefinition.id] = glyphDefinition;
			}
		}
		dfFont.GlyphDefinition glyphDefinition2 = null;
		this.glyphMap.TryGetValue((int)id, out glyphDefinition2);
		return glyphDefinition2;
	}

	// Token: 0x04001015 RID: 4117
	[SerializeField]
	protected dfAtlas atlas;

	// Token: 0x04001016 RID: 4118
	[SerializeField]
	protected string sprite;

	// Token: 0x04001017 RID: 4119
	[SerializeField]
	protected string face = string.Empty;

	// Token: 0x04001018 RID: 4120
	[SerializeField]
	protected int size;

	// Token: 0x04001019 RID: 4121
	[SerializeField]
	protected bool bold;

	// Token: 0x0400101A RID: 4122
	[SerializeField]
	protected bool italic;

	// Token: 0x0400101B RID: 4123
	[SerializeField]
	protected string charset;

	// Token: 0x0400101C RID: 4124
	[SerializeField]
	protected int stretchH;

	// Token: 0x0400101D RID: 4125
	[SerializeField]
	protected bool smooth;

	// Token: 0x0400101E RID: 4126
	[SerializeField]
	protected int aa;

	// Token: 0x0400101F RID: 4127
	[SerializeField]
	protected int[] padding;

	// Token: 0x04001020 RID: 4128
	[SerializeField]
	protected int[] spacing;

	// Token: 0x04001021 RID: 4129
	[SerializeField]
	protected int outline;

	// Token: 0x04001022 RID: 4130
	[SerializeField]
	protected int lineHeight;

	// Token: 0x04001023 RID: 4131
	[SerializeField]
	private List<dfFont.GlyphDefinition> glyphs = new List<dfFont.GlyphDefinition>();

	// Token: 0x04001024 RID: 4132
	[SerializeField]
	protected List<dfFont.GlyphKerning> kerning = new List<dfFont.GlyphKerning>();

	// Token: 0x04001025 RID: 4133
	private Dictionary<int, dfFont.GlyphDefinition> glyphMap;

	// Token: 0x04001026 RID: 4134
	private Dictionary<int, dfFont.GlyphKerningList> kerningMap;

	// Token: 0x020003C4 RID: 964
	private class GlyphKerningList
	{
		// Token: 0x06001247 RID: 4679 RVA: 0x000536CC File Offset: 0x000518CC
		public void Add(dfFont.GlyphKerning kerning)
		{
			this.list[kerning.second] = kerning.amount;
		}

		// Token: 0x06001248 RID: 4680 RVA: 0x000536E8 File Offset: 0x000518E8
		public int GetKerning(int firstCharacter, int secondCharacter)
		{
			int num = 0;
			this.list.TryGetValue(secondCharacter, out num);
			return num;
		}

		// Token: 0x04001027 RID: 4135
		private Dictionary<int, int> list = new Dictionary<int, int>();
	}

	// Token: 0x020003C5 RID: 965
	[Serializable]
	public class GlyphKerning : IComparable<dfFont.GlyphKerning>
	{
		// Token: 0x0600124A RID: 4682 RVA: 0x00053710 File Offset: 0x00051910
		public int CompareTo(dfFont.GlyphKerning other)
		{
			if (this.first == other.first)
			{
				return this.second.CompareTo(other.second);
			}
			return this.first.CompareTo(other.first);
		}

		// Token: 0x04001028 RID: 4136
		public int first;

		// Token: 0x04001029 RID: 4137
		public int second;

		// Token: 0x0400102A RID: 4138
		public int amount;
	}

	// Token: 0x020003C6 RID: 966
	[Serializable]
	public class GlyphDefinition : IComparable<dfFont.GlyphDefinition>
	{
		// Token: 0x0600124C RID: 4684 RVA: 0x00053750 File Offset: 0x00051950
		public int CompareTo(dfFont.GlyphDefinition other)
		{
			return this.id.CompareTo(other.id);
		}

		// Token: 0x0400102B RID: 4139
		[SerializeField]
		public int id;

		// Token: 0x0400102C RID: 4140
		[SerializeField]
		public int x;

		// Token: 0x0400102D RID: 4141
		[SerializeField]
		public int y;

		// Token: 0x0400102E RID: 4142
		[SerializeField]
		public int width;

		// Token: 0x0400102F RID: 4143
		[SerializeField]
		public int height;

		// Token: 0x04001030 RID: 4144
		[SerializeField]
		public int xoffset;

		// Token: 0x04001031 RID: 4145
		[SerializeField]
		public int yoffset;

		// Token: 0x04001032 RID: 4146
		[SerializeField]
		public int xadvance;

		// Token: 0x04001033 RID: 4147
		[SerializeField]
		public bool rotated;
	}

	// Token: 0x020003C7 RID: 967
	public class BitmappedFontRenderer : dfFontRendererBase, IPoolable
	{
		// Token: 0x0600124D RID: 4685 RVA: 0x00053764 File Offset: 0x00051964
		internal BitmappedFontRenderer()
		{
		}

		// Token: 0x170003F4 RID: 1012
		// (get) Token: 0x0600124E RID: 4686 RVA: 0x0005376C File Offset: 0x0005196C
		public int LineCount
		{
			get
			{
				return this.lines.Count;
			}
		}

		// Token: 0x0600124F RID: 4687 RVA: 0x0005377C File Offset: 0x0005197C
		public static dfFontRendererBase Obtain(dfFont font)
		{
			dfFont.BitmappedFontRenderer bitmappedFontRenderer = ((dfFont.BitmappedFontRenderer.objectPool.Count <= 0) ? new dfFont.BitmappedFontRenderer() : dfFont.BitmappedFontRenderer.objectPool.Dequeue());
			bitmappedFontRenderer.Reset();
			bitmappedFontRenderer.Font = font;
			return bitmappedFontRenderer;
		}

		// Token: 0x06001250 RID: 4688 RVA: 0x000537BC File Offset: 0x000519BC
		public override void Release()
		{
			this.Reset();
			if (this.tokens != null)
			{
				this.tokens.ReleaseItems();
				this.tokens.Release();
			}
			this.tokens = null;
			if (this.lines != null)
			{
				this.lines.Release();
				this.lines = null;
			}
			dfFont.LineRenderInfo.ResetPool();
			base.BottomColor = null;
			dfFont.BitmappedFontRenderer.objectPool.Enqueue(this);
		}

		// Token: 0x06001251 RID: 4689 RVA: 0x00053834 File Offset: 0x00051A34
		public override float[] GetCharacterWidths(string text)
		{
			float num = 0f;
			return this.GetCharacterWidths(text, 0, text.Length - 1, out num);
		}

		// Token: 0x06001252 RID: 4690 RVA: 0x0005385C File Offset: 0x00051A5C
		public float[] GetCharacterWidths(string text, int startIndex, int endIndex, out float totalWidth)
		{
			totalWidth = 0f;
			dfFont dfFont = (dfFont)base.Font;
			float[] array = new float[text.Length];
			float num = base.TextScale * base.PixelRatio;
			float num2 = (float)base.CharacterSpacing * num;
			for (int i = startIndex; i <= endIndex; i++)
			{
				dfFont.GlyphDefinition glyph = dfFont.GetGlyph(text[i]);
				if (glyph != null)
				{
					if (i > 0)
					{
						array[i - 1] += num2;
						totalWidth += num2;
					}
					float num3 = (float)glyph.xadvance * num;
					array[i] = num3;
					totalWidth += num3;
				}
			}
			return array;
		}

		// Token: 0x06001253 RID: 4691 RVA: 0x00053910 File Offset: 0x00051B10
		public override Vector2 MeasureString(string text)
		{
			this.tokenize(text);
			dfList<dfFont.LineRenderInfo> dfList = this.calculateLinebreaks();
			int num = 0;
			int num2 = 0;
			for (int i = 0; i < dfList.Count; i++)
			{
				num = Mathf.Max((int)dfList[i].lineWidth, num);
				num2 += (int)dfList[i].lineHeight;
			}
			return new Vector2((float)num, (float)num2) * base.TextScale;
		}

		// Token: 0x06001254 RID: 4692 RVA: 0x00053980 File Offset: 0x00051B80
		public override void Render(string text, dfRenderData destination)
		{
			dfFont.BitmappedFontRenderer.textColors.Clear();
			dfFont.BitmappedFontRenderer.textColors.Push(Color.white);
			this.tokenize(text);
			dfList<dfFont.LineRenderInfo> dfList = this.calculateLinebreaks();
			destination.EnsureCapacity(this.getAnticipatedVertCount(this.tokens));
			int num = 0;
			int num2 = 0;
			Vector3 vectorOffset = base.VectorOffset;
			float num3 = base.TextScale * base.PixelRatio;
			for (int i = 0; i < dfList.Count; i++)
			{
				dfFont.LineRenderInfo lineRenderInfo = dfList[i];
				int count = destination.Vertices.Count;
				this.renderLine(dfList[i], dfFont.BitmappedFontRenderer.textColors, vectorOffset, destination);
				vectorOffset.y -= (float)base.Font.LineHeight * num3;
				num = Mathf.Max((int)lineRenderInfo.lineWidth, num);
				num2 += (int)lineRenderInfo.lineHeight;
				if (lineRenderInfo.lineWidth * base.TextScale > base.MaxSize.x)
				{
					this.clipRight(destination, count);
				}
				if ((float)num2 * base.TextScale > base.MaxSize.y)
				{
					this.clipBottom(destination, count);
				}
			}
			base.RenderedSize = new Vector2(Mathf.Min(base.MaxSize.x, (float)num), Mathf.Min(base.MaxSize.y, (float)num2)) * base.TextScale;
		}

		// Token: 0x06001255 RID: 4693 RVA: 0x00053AF8 File Offset: 0x00051CF8
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

		// Token: 0x06001256 RID: 4694 RVA: 0x00053B7C File Offset: 0x00051D7C
		private void renderLine(dfFont.LineRenderInfo line, Stack<Color32> colors, Vector3 position, dfRenderData destination)
		{
			float num = base.TextScale * base.PixelRatio;
			position.x += (float)this.calculateLineAlignment(line) * num;
			for (int i = line.startOffset; i <= line.endOffset; i++)
			{
				dfMarkupToken dfMarkupToken = this.tokens[i];
				dfMarkupTokenType tokenType = dfMarkupToken.TokenType;
				if (tokenType == dfMarkupTokenType.Text)
				{
					this.renderText(dfMarkupToken, colors.Peek(), position, destination);
					position += base.PerCharacterAccumulatedOffset * (float)dfMarkupToken.Length;
				}
				else if (tokenType == dfMarkupTokenType.StartTag)
				{
					if (dfMarkupToken.Matches("sprite"))
					{
						this.renderSprite(dfMarkupToken, colors.Peek(), position, destination);
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
				position.x += (float)dfMarkupToken.Width * num;
			}
		}

		// Token: 0x06001257 RID: 4695 RVA: 0x00053CA4 File Offset: 0x00051EA4
		private void renderText(dfMarkupToken token, Color32 color, Vector3 position, dfRenderData destination)
		{
			try
			{
				dfList<Vector3> vertices = destination.Vertices;
				dfList<int> triangles = destination.Triangles;
				dfList<Color32> colors = destination.Colors;
				dfList<Vector2> uv = destination.UV;
				dfFont dfFont = (dfFont)base.Font;
				dfAtlas.ItemInfo itemInfo = dfFont.Atlas[dfFont.sprite];
				Texture texture = dfFont.Texture;
				float num = 1f / (float)texture.width;
				float num2 = 1f / (float)texture.height;
				float num3 = base.TextScale * base.PixelRatio;
				char c = '\0';
				char c2 = '\0';
				Color32 color2 = this.applyOpacity(this.multiplyColors(color, base.DefaultColor));
				Color32 color3 = color2;
				if (base.BottomColor != null)
				{
					color3 = this.applyOpacity(this.multiplyColors(color, base.BottomColor.Value));
				}
				int i = 0;
				while (i < token.Length)
				{
					c2 = token[i];
					if (c2 != '\0')
					{
						dfFont.GlyphDefinition glyph = dfFont.GetGlyph(c2);
						if (glyph != null)
						{
							int kerning = dfFont.GetKerning(c, c2);
							float num4 = position.x + (float)(glyph.xoffset + kerning) * num3;
							float num5 = position.y - (float)glyph.yoffset * num3;
							float num6 = (float)glyph.width * num3;
							float num7 = (float)glyph.height * num3;
							float num8 = num4 + num6;
							float num9 = num5 - num7;
							Vector3 vector = new Vector3(num4, num5);
							Vector3 vector2 = new Vector3(num8, num5);
							Vector3 vector3 = new Vector3(num8, num9);
							Vector3 vector4 = new Vector3(num4, num9);
							float num10 = itemInfo.region.x + (float)glyph.x * num;
							float num11 = itemInfo.region.yMax - (float)glyph.y * num2;
							float num12 = num10 + (float)glyph.width * num;
							float num13 = num11 - (float)glyph.height * num2;
							if (base.Shadow)
							{
								dfFont.BitmappedFontRenderer.addTriangleIndices(vertices, triangles);
								Vector3 vector5 = base.ShadowOffset * num3;
								vertices.Add(vector + vector5);
								vertices.Add(vector2 + vector5);
								vertices.Add(vector3 + vector5);
								vertices.Add(vector4 + vector5);
								Color32 color4 = this.applyOpacity(base.ShadowColor);
								colors.Add(color4);
								colors.Add(color4);
								colors.Add(color4);
								colors.Add(color4);
								uv.Add(new Vector2(num10, num11));
								uv.Add(new Vector2(num12, num11));
								uv.Add(new Vector2(num12, num13));
								uv.Add(new Vector2(num10, num13));
							}
							if (base.Outline)
							{
								for (int j = 0; j < dfFont.BitmappedFontRenderer.OUTLINE_OFFSETS.Length; j++)
								{
									dfFont.BitmappedFontRenderer.addTriangleIndices(vertices, triangles);
									Vector3 vector6 = dfFont.BitmappedFontRenderer.OUTLINE_OFFSETS[j] * (float)base.OutlineSize * num3;
									vertices.Add(vector + vector6);
									vertices.Add(vector2 + vector6);
									vertices.Add(vector3 + vector6);
									vertices.Add(vector4 + vector6);
									Color32 color5 = this.applyOpacity(base.OutlineColor);
									colors.Add(color5);
									colors.Add(color5);
									colors.Add(color5);
									colors.Add(color5);
									uv.Add(new Vector2(num10, num11));
									uv.Add(new Vector2(num12, num11));
									uv.Add(new Vector2(num12, num13));
									uv.Add(new Vector2(num10, num13));
								}
							}
							dfFont.BitmappedFontRenderer.addTriangleIndices(vertices, triangles);
							vertices.Add(vector);
							vertices.Add(vector2);
							vertices.Add(vector3);
							vertices.Add(vector4);
							colors.Add(color2);
							colors.Add(color2);
							colors.Add(color3);
							colors.Add(color3);
							if (destination.Glitchy)
							{
								float num14 = num12 - num10;
								float num15 = num13 - num11;
								float num16 = UnityEngine.Random.value * num14 * UnityEngine.Random.Range(2f, 3f);
								float num17 = UnityEngine.Random.value * num15 * UnityEngine.Random.Range(2f, 3f);
								num14 *= UnityEngine.Random.Range(2f, 5f);
								num15 *= UnityEngine.Random.Range(2f, 5f);
								Vector2 vector7 = new Vector2(num10 - num16, num11 - num17);
								uv.Add(vector7);
								uv.Add(vector7 + new Vector2(num14, 0f));
								uv.Add(vector7 + new Vector2(num14, num15));
								uv.Add(vector7 + new Vector2(0f, num15));
							}
							else
							{
								uv.Add(new Vector2(num10, num11));
								uv.Add(new Vector2(num12, num11));
								uv.Add(new Vector2(num12, num13));
								uv.Add(new Vector2(num10, num13));
							}
							position.x += (float)(glyph.xadvance + kerning + base.CharacterSpacing) * num3;
							position += base.PerCharacterAccumulatedOffset;
						}
					}
					i++;
					c = c2;
				}
			}
			finally
			{
			}
		}

		// Token: 0x06001258 RID: 4696 RVA: 0x00054248 File Offset: 0x00052448
		private void renderSprite(dfMarkupToken token, Color32 color, Vector3 position, dfRenderData destination)
		{
			try
			{
				dfList<Vector3> vertices = destination.Vertices;
				dfList<int> triangles = destination.Triangles;
				dfList<Color32> colors = destination.Colors;
				dfList<Vector2> uv = destination.UV;
				dfFont dfFont = (dfFont)base.Font;
				string value = token.GetAttribute(0).Value.Value;
				dfAtlas.ItemInfo itemInfo = dfFont.Atlas[value];
				if (!(itemInfo == null))
				{
					float num = (float)token.Height * base.TextScale * base.PixelRatio;
					float num2 = (float)token.Width * base.TextScale * base.PixelRatio;
					float x = position.x;
					float num3 = position.y;
					if (base.Font.IsSpriteScaledUIFont())
					{
						num3 = position.y + (float)token.Height * base.TextScale * base.PixelRatio * 0.2f;
					}
					int count = vertices.Count;
					vertices.Add(new Vector3(x, num3));
					vertices.Add(new Vector3(x + num2, num3));
					vertices.Add(new Vector3(x + num2, num3 - num));
					vertices.Add(new Vector3(x, num3 - num));
					triangles.Add(count);
					triangles.Add(count + 1);
					triangles.Add(count + 3);
					triangles.Add(count + 3);
					triangles.Add(count + 1);
					triangles.Add(count + 2);
					Color32 color2 = ((!base.ColorizeSymbols) ? this.applyOpacity(base.DefaultColor) : this.applyOpacity(color));
					colors.Add(color2);
					colors.Add(color2);
					colors.Add(color2);
					colors.Add(color2);
					Rect region = itemInfo.region;
					uv.Add(new Vector2(region.x, region.yMax));
					uv.Add(new Vector2(region.xMax, region.yMax));
					uv.Add(new Vector2(region.xMax, region.y));
					uv.Add(new Vector2(region.x, region.y));
				}
			}
			finally
			{
			}
		}

		// Token: 0x06001259 RID: 4697 RVA: 0x0005448C File Offset: 0x0005268C
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

		// Token: 0x0600125A RID: 4698 RVA: 0x00054524 File Offset: 0x00052724
		private Color32 UIntToColor(uint color)
		{
			byte b = (byte)(color >> 24);
			byte b2 = (byte)(color >> 16);
			byte b3 = (byte)(color >> 8);
			byte b4 = (byte)(color >> 0);
			return new Color32(b2, b3, b4, b);
		}

		// Token: 0x0600125B RID: 4699 RVA: 0x00054550 File Offset: 0x00052750
		private dfList<dfFont.LineRenderInfo> calculateLinebreaks()
		{
			dfList<dfFont.LineRenderInfo> dfList;
			try
			{
				if (this.lines != null)
				{
					dfList = this.lines;
				}
				else
				{
					this.lines = dfList<dfFont.LineRenderInfo>.Obtain();
					int num = 0;
					int num2 = 0;
					int num3 = 0;
					int num4 = 0;
					float num5 = (float)base.Font.LineHeight * base.TextScale;
					while (num3 < this.tokens.Count && (float)this.lines.Count * num5 < base.MaxSize.y)
					{
						dfMarkupToken dfMarkupToken = this.tokens[num3];
						dfMarkupTokenType tokenType = dfMarkupToken.TokenType;
						if (tokenType == dfMarkupTokenType.Newline)
						{
							this.lines.Add(dfFont.LineRenderInfo.Obtain(num2, num3));
							num = (num2 = ++num3);
							num4 = 0;
						}
						else
						{
							int num6 = Mathf.CeilToInt((float)dfMarkupToken.Width * base.TextScale);
							bool flag = base.WordWrap && num > num2 && (tokenType == dfMarkupTokenType.Text || (tokenType == dfMarkupTokenType.StartTag && dfMarkupToken.Matches("sprite")));
							if (flag && (float)(num4 + num6) >= base.MaxSize.x)
							{
								if (num > num2)
								{
									this.lines.Add(dfFont.LineRenderInfo.Obtain(num2, num - 1));
									num3 = (num2 = ++num);
									num4 = 0;
								}
								else
								{
									this.lines.Add(dfFont.LineRenderInfo.Obtain(num2, num - 1));
									num = (num2 = ++num3);
									num4 = 0;
								}
								if (GameManager.Options.CurrentLanguage == StringTableManager.GungeonSupportedLanguages.JAPANESE || GameManager.Options.CurrentLanguage == StringTableManager.GungeonSupportedLanguages.CHINESE)
								{
									num2--;
								}
							}
							else
							{
								if (tokenType == dfMarkupTokenType.Whitespace)
								{
									num = num3;
								}
								else if (tokenType == dfMarkupTokenType.Text && (GameManager.Options.CurrentLanguage == StringTableManager.GungeonSupportedLanguages.JAPANESE || GameManager.Options.CurrentLanguage == StringTableManager.GungeonSupportedLanguages.CHINESE))
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
						this.lines.Add(dfFont.LineRenderInfo.Obtain(num2, this.tokens.Count - 1));
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

		// Token: 0x0600125C RID: 4700 RVA: 0x000547CC File Offset: 0x000529CC
		private int calculateLineAlignment(dfFont.LineRenderInfo line)
		{
			float lineWidth = line.lineWidth;
			if (base.TextAlign == TextAlignment.Left || lineWidth == 0f)
			{
				return 0;
			}
			int num;
			if (base.TextAlign == TextAlignment.Right)
			{
				num = Mathf.FloorToInt(base.MaxSize.x / base.TextScale - lineWidth);
			}
			else
			{
				num = Mathf.FloorToInt((base.MaxSize.x / base.TextScale - lineWidth) * 0.5f);
			}
			return Mathf.Max(0, num);
		}

		// Token: 0x0600125D RID: 4701 RVA: 0x00054854 File Offset: 0x00052A54
		private void calculateLineSize(dfFont.LineRenderInfo line)
		{
			line.lineHeight = (float)base.Font.LineHeight;
			int num = 0;
			for (int i = line.startOffset; i <= line.endOffset; i++)
			{
				num += this.tokens[i].Width;
			}
			line.lineWidth = (float)num;
		}

		// Token: 0x0600125E RID: 4702 RVA: 0x000548B0 File Offset: 0x00052AB0
		private dfList<dfMarkupToken> tokenize(string text)
		{
			dfList<dfMarkupToken> dfList;
			try
			{
				if (this.tokens != null)
				{
					if (object.ReferenceEquals(this.tokens[0].Source, text))
					{
						return this.tokens;
					}
					this.tokens.ReleaseItems();
					this.tokens.Release();
				}
				if (GameManager.Options.CurrentLanguage == StringTableManager.GungeonSupportedLanguages.JAPANESE || GameManager.Options.CurrentLanguage == StringTableManager.GungeonSupportedLanguages.CHINESE)
				{
					this.tokens = dfJapaneseMarkupTokenizer.Tokenize(text);
				}
				else if (base.ProcessMarkup)
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
				dfList = this.tokens;
			}
			finally
			{
			}
			return dfList;
		}

		// Token: 0x0600125F RID: 4703 RVA: 0x000549AC File Offset: 0x00052BAC
		private void calculateTokenRenderSize(dfMarkupToken token)
		{
			try
			{
				dfFont dfFont = (dfFont)base.Font;
				int num = 0;
				char c = '\0';
				char c2 = '\0';
				bool flag = token.TokenType == dfMarkupTokenType.Whitespace || token.TokenType == dfMarkupTokenType.Text;
				bool flag2 = false;
				if (flag)
				{
					int i = 0;
					while (i < token.Length)
					{
						c2 = token[i];
						if (c2 == '\t')
						{
							num += base.TabSize;
						}
						else
						{
							dfFont.GlyphDefinition glyph = dfFont.GetGlyph(c2);
							if (glyph != null)
							{
								if (i > 0)
								{
									num += dfFont.GetKerning(c, c2);
									num += base.CharacterSpacing;
								}
								num += glyph.xadvance;
							}
						}
						i++;
						c = c2;
					}
				}
				else if (token.TokenType == dfMarkupTokenType.StartTag && token.Matches("sprite"))
				{
					if (token.AttributeCount < 1)
					{
						throw new Exception("Missing sprite name in markup");
					}
					Texture texture = dfFont.Texture;
					int lineHeight = dfFont.LineHeight;
					string value = token.GetAttribute(0).Value.Value;
					dfAtlas.ItemInfo itemInfo = dfFont.atlas[value];
					if (itemInfo != null)
					{
						float num2 = itemInfo.region.width * (float)texture.width / (itemInfo.region.height * (float)texture.height);
						num = Mathf.CeilToInt((float)lineHeight * num2);
						float num3 = 1f;
						if (base.Font.IsSpriteScaledUIFont())
						{
							num3 = 5f;
						}
						flag2 = true;
						token.Height = Mathf.CeilToInt(itemInfo.region.height * (float)texture.height * num3);
						token.Width = Mathf.CeilToInt(itemInfo.region.width * (float)texture.width * num3);
					}
				}
				if (!flag2)
				{
					token.Height = base.Font.LineHeight;
					token.Width = num;
				}
			}
			finally
			{
			}
		}

		// Token: 0x06001260 RID: 4704 RVA: 0x00054BC0 File Offset: 0x00052DC0
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

		// Token: 0x06001261 RID: 4705 RVA: 0x00054C64 File Offset: 0x00052E64
		private void clipRight(dfRenderData destination, int startIndex)
		{
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

		// Token: 0x06001262 RID: 4706 RVA: 0x00054E68 File Offset: 0x00053068
		private void clipBottom(dfRenderData destination, int startIndex)
		{
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
					float num8 = Mathf.Lerp(uv[i + 3].y, uv[i].y, num3);
					uv[i + 3] = new Vector2(uv[i + 3].x, num8);
					uv[i + 2] = new Vector2(uv[i + 2].x, num8);
					Color color = Color.Lerp(colors[i + 3], colors[i], num3);
					colors[i + 3] = color;
					colors[i + 2] = color;
				}
			}
		}

		// Token: 0x06001263 RID: 4707 RVA: 0x000550B0 File Offset: 0x000532B0
		private Color32 applyOpacity(Color32 color)
		{
			color.a = (byte)(base.Opacity * 255f);
			return color;
		}

		// Token: 0x06001264 RID: 4708 RVA: 0x000550C8 File Offset: 0x000532C8
		private static void addTriangleIndices(dfList<Vector3> verts, dfList<int> triangles)
		{
			int count = verts.Count;
			for (int i = 0; i < dfFont.BitmappedFontRenderer.TRIANGLE_INDICES.Length; i++)
			{
				triangles.Add(count + dfFont.BitmappedFontRenderer.TRIANGLE_INDICES[i]);
			}
		}

		// Token: 0x06001265 RID: 4709 RVA: 0x00055104 File Offset: 0x00053304
		private Color multiplyColors(Color lhs, Color rhs)
		{
			return new Color(lhs.r * rhs.r, lhs.g * rhs.g, lhs.b * rhs.b, lhs.a * rhs.a);
		}

		// Token: 0x04001034 RID: 4148
		private static Queue<dfFont.BitmappedFontRenderer> objectPool = new Queue<dfFont.BitmappedFontRenderer>();

		// Token: 0x04001035 RID: 4149
		private static Vector2[] OUTLINE_OFFSETS = new Vector2[]
		{
			new Vector2(-1f, -1f),
			new Vector2(-1f, 1f),
			new Vector2(1f, -1f),
			new Vector2(1f, 1f)
		};

		// Token: 0x04001036 RID: 4150
		private static int[] TRIANGLE_INDICES = new int[] { 0, 1, 3, 3, 1, 2 };

		// Token: 0x04001037 RID: 4151
		private static Stack<Color32> textColors = new Stack<Color32>();

		// Token: 0x04001038 RID: 4152
		private dfList<dfFont.LineRenderInfo> lines;

		// Token: 0x04001039 RID: 4153
		private dfList<dfMarkupToken> tokens;
	}

	// Token: 0x020003C8 RID: 968
	private class LineRenderInfo
	{
		// Token: 0x06001267 RID: 4711 RVA: 0x00055204 File Offset: 0x00053404
		private LineRenderInfo()
		{
		}

		// Token: 0x170003F5 RID: 1013
		// (get) Token: 0x06001268 RID: 4712 RVA: 0x0005520C File Offset: 0x0005340C
		public int length
		{
			get
			{
				return this.endOffset - this.startOffset + 1;
			}
		}

		// Token: 0x06001269 RID: 4713 RVA: 0x00055220 File Offset: 0x00053420
		public static void ResetPool()
		{
			dfFont.LineRenderInfo.poolIndex = 0;
		}

		// Token: 0x0600126A RID: 4714 RVA: 0x00055228 File Offset: 0x00053428
		public static dfFont.LineRenderInfo Obtain(int start, int end)
		{
			if (dfFont.LineRenderInfo.poolIndex >= dfFont.LineRenderInfo.pool.Count - 1)
			{
				dfFont.LineRenderInfo.pool.Add(new dfFont.LineRenderInfo());
			}
			dfFont.LineRenderInfo lineRenderInfo = dfFont.LineRenderInfo.pool[dfFont.LineRenderInfo.poolIndex++];
			lineRenderInfo.startOffset = start;
			lineRenderInfo.endOffset = end;
			lineRenderInfo.lineHeight = 0f;
			return lineRenderInfo;
		}

		// Token: 0x0400103A RID: 4154
		public int startOffset;

		// Token: 0x0400103B RID: 4155
		public int endOffset;

		// Token: 0x0400103C RID: 4156
		public float lineWidth;

		// Token: 0x0400103D RID: 4157
		public float lineHeight;

		// Token: 0x0400103E RID: 4158
		private static dfList<dfFont.LineRenderInfo> pool = new dfList<dfFont.LineRenderInfo>();

		// Token: 0x0400103F RID: 4159
		private static int poolIndex = 0;
	}
}
