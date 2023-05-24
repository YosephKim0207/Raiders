using System;
using System.Collections.Generic;
using System.Text;
using tk2dRuntime;
using UnityEngine;

// Token: 0x02000B95 RID: 2965
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[AddComponentMenu("2D Toolkit/Text/tk2dTextMesh")]
[ExecuteInEditMode]
public class tk2dTextMesh : MonoBehaviour, ISpriteCollectionForceBuild
{
	// Token: 0x1700094C RID: 2380
	// (get) Token: 0x06003DF0 RID: 15856 RVA: 0x001385E0 File Offset: 0x001367E0
	public string FormattedText
	{
		get
		{
			return this._formattedText;
		}
	}

	// Token: 0x06003DF1 RID: 15857 RVA: 0x001385E8 File Offset: 0x001367E8
	private void UpgradeData()
	{
		if (this.data.version != 1)
		{
			this.data.font = this._font;
			this.data.text = this._text;
			this.data.color = this._color;
			this.data.color2 = this._color2;
			this.data.useGradient = this._useGradient;
			this.data.textureGradient = this._textureGradient;
			this.data.anchor = this._anchor;
			this.data.scale = this._scale;
			this.data.kerning = this._kerning;
			this.data.maxChars = this._maxChars;
			this.data.inlineStyling = this._inlineStyling;
			this.data.formatting = this._formatting;
			this.data.wordWrapWidth = this._wordWrapWidth;
			this.data.spacing = this.spacing;
			this.data.lineSpacing = this.lineSpacing;
		}
		this.data.version = 1;
	}

	// Token: 0x06003DF2 RID: 15858 RVA: 0x00138714 File Offset: 0x00136914
	private static int GetInlineStyleCommandLength(int cmdSymbol)
	{
		int num = 0;
		if (cmdSymbol != 67)
		{
			if (cmdSymbol != 71)
			{
				if (cmdSymbol != 99)
				{
					if (cmdSymbol == 103)
					{
						num = 9;
					}
				}
				else
				{
					num = 5;
				}
			}
			else
			{
				num = 17;
			}
		}
		else
		{
			num = 9;
		}
		return num;
	}

	// Token: 0x06003DF3 RID: 15859 RVA: 0x00138768 File Offset: 0x00136968
	public string FormatText(string unformattedString)
	{
		string empty = string.Empty;
		this.FormatText(ref empty, unformattedString, false);
		return empty;
	}

	// Token: 0x06003DF4 RID: 15860 RVA: 0x00138788 File Offset: 0x00136988
	private void FormatText()
	{
		this.FormatText(ref this._formattedText, this.data.text, false);
	}

	// Token: 0x06003DF5 RID: 15861 RVA: 0x001387A4 File Offset: 0x001369A4
	public string GetStrippedWoobleString(string _source)
	{
		for (int i = 0; i < _source.Length; i++)
		{
			if (_source[i] == '{' && _source[i + 1] == 'w' && _source[i + 3] == '}')
			{
				int num = -1;
				for (int j = i + 3; j < _source.Length; j++)
				{
					if (_source[j] == '{' && _source[j + 1] == 'w')
					{
						num = j - 5;
						_source = _source.Remove(j, 3);
						break;
					}
				}
				if (num != -1)
				{
					_source = _source.Remove(i, 4);
				}
			}
		}
		this.FormatText(ref this._formattedText, _source, true);
		return _source;
	}

	// Token: 0x06003DF6 RID: 15862 RVA: 0x00138868 File Offset: 0x00136A68
	public string PreprocessWoobleSignifiers(string _source)
	{
		List<tk2dTextMesh.WoobleDefinition> list = new List<tk2dTextMesh.WoobleDefinition>();
		for (int i = 0; i < _source.Length; i++)
		{
			if (_source[i] == '{' && _source[i + 1] == 'w' && _source[i + 3] == '}')
			{
				int num = -1;
				for (int j = i + 3; j < _source.Length; j++)
				{
					if (_source[j] == '{' && _source[j + 1] == 'w')
					{
						num = j - 5;
						_source = _source.Remove(j, 3);
						break;
					}
				}
				if (num != -1)
				{
					string text = _source.Substring(i, 4);
					_source = _source.Remove(i, 4);
					char c = text[2];
					tk2dTextMesh.WoobleDefinition woobleDefinition = new tk2dTextMesh.WoobleDefinition();
					switch (c)
					{
					case 'q':
						woobleDefinition.style = tk2dTextMesh.WoobleStyle.SEQUENTIAL;
						break;
					case 'r':
						woobleDefinition.style = tk2dTextMesh.WoobleStyle.RANDOM_SEQUENTIAL;
						break;
					case 's':
						woobleDefinition.style = tk2dTextMesh.WoobleStyle.SIMULTANEOUS;
						break;
					default:
						if (c != 'b')
						{
							if (c == 'j')
							{
								woobleDefinition.style = tk2dTextMesh.WoobleStyle.RANDOM_JITTER;
							}
						}
						else
						{
							woobleDefinition.style = tk2dTextMesh.WoobleStyle.SEQUENTIAL_RAINBOW;
						}
						break;
					}
					woobleDefinition.startIndex = i;
					woobleDefinition.endIndex = num;
					list.Add(woobleDefinition);
				}
			}
		}
		this.woobleStartIndices = new int[list.Count];
		this.woobleEndIndices = new int[list.Count];
		this.woobleStyles = new tk2dTextMesh.WoobleStyle[list.Count];
		for (int k = 0; k < list.Count; k++)
		{
			this.woobleStartIndices[k] = list[k].startIndex;
			this.woobleEndIndices[k] = list[k].endIndex;
			this.woobleStyles[k] = list[k].style;
		}
		this.FormatText(ref this._formattedText, _source, true);
		return _source;
	}

	// Token: 0x06003DF7 RID: 15863 RVA: 0x00138A64 File Offset: 0x00136C64
	private void PushbackWooblesByAmount(int newCharIndex, int amt, int max)
	{
		for (int i = 0; i < this.woobleStyles.Length; i++)
		{
			if (this.woobleStartIndices[i] >= newCharIndex)
			{
				this.woobleStartIndices[i] = Mathf.Min(this.woobleStartIndices[i] + amt, max);
			}
			if (this.woobleEndIndices[i] >= newCharIndex)
			{
				this.woobleEndIndices[i] = Mathf.Min(this.woobleEndIndices[i] + amt, max);
			}
		}
	}

	// Token: 0x06003DF8 RID: 15864 RVA: 0x00138AD8 File Offset: 0x00136CD8
	private void FormatText(ref string _targetString, string _source, bool doPushback = false)
	{
		this.InitInstance();
		if (!this.formatting || this.wordWrapWidth == 0 || this._fontInst.texelSize == Vector2.zero)
		{
			_targetString = _source;
			return;
		}
		float num = this._fontInst.texelSize.x * (float)this.wordWrapWidth;
		StringBuilder stringBuilder = new StringBuilder(_source.Length);
		float num2 = 0f;
		float num3 = 0f;
		int num4 = -1;
		int num5 = -1;
		bool flag = false;
		for (int i = 0; i < _source.Length; i++)
		{
			char c = _source[i];
			tk2dFontChar tk2dFontChar = null;
			bool flag2 = c == '^';
			bool flag3 = false;
			if (c == '[' && i < _source.Length - 1 && _source[i + 1] != ']')
			{
				for (int j = i; j < _source.Length; j++)
				{
					char c2 = _source[j];
					if (c2 == ']')
					{
						flag3 = true;
						int num6 = j - i + 1;
						string text = _source.Substring(i + 9, num6 - 10);
						tk2dFontChar = tk2dTextGeomGen.GetSpecificSpriteCharDef(text);
						for (int k = 0; k < num6; k++)
						{
							if (i + k < _source.Length)
							{
								stringBuilder.Append(_source[i + k]);
							}
						}
						i += num6 - 1;
						break;
					}
				}
			}
			if (!flag3)
			{
				if (this._fontInst.useDictionary)
				{
					if (!this._fontInst.charDict.ContainsKey((int)c))
					{
						c = '\0';
					}
					tk2dFontChar = this._fontInst.charDict[(int)c];
				}
				else
				{
					if ((int)c >= this._fontInst.chars.Length)
					{
						c = '\0';
					}
					tk2dFontChar = this._fontInst.chars[(int)c];
				}
			}
			if (flag2)
			{
				c = '^';
			}
			if (flag)
			{
				flag = false;
			}
			else
			{
				if (this.data.inlineStyling && c == '^' && i + 1 < _source.Length)
				{
					if (_source[i + 1] != '^')
					{
						int inlineStyleCommandLength = tk2dTextMesh.GetInlineStyleCommandLength((int)_source[i + 1]);
						int num7 = 1 + inlineStyleCommandLength;
						for (int l = 0; l < num7; l++)
						{
							if (i + l < _source.Length)
							{
								stringBuilder.Append(_source[i + l]);
							}
						}
						i += num7 - 1;
						goto IL_3EE;
					}
					flag = true;
					stringBuilder.Append('^');
				}
				if (c == '\n')
				{
					num2 = 0f;
					num3 = 0f;
					num4 = stringBuilder.Length;
					num5 = i;
				}
				else if (c == ' ')
				{
					num2 += (tk2dFontChar.advance + this.data.spacing) * this.data.scale.x;
					num3 = num2;
					num4 = stringBuilder.Length;
					num5 = i;
				}
				else if (num2 + tk2dFontChar.p1.x * this.data.scale.x > num)
				{
					if (num3 > 0f)
					{
						num3 = 0f;
						num2 = 0f;
						stringBuilder.Remove(num4 + 1, stringBuilder.Length - num4 - 1);
						stringBuilder.Append('\n');
						i = num5;
						if (doPushback)
						{
							this.PushbackWooblesByAmount(i, 1, _source.Length);
						}
						goto IL_3EE;
					}
					stringBuilder.Append('\n');
					if (doPushback)
					{
						this.PushbackWooblesByAmount(i, 1, _source.Length);
					}
					num2 = (tk2dFontChar.advance + this.data.spacing) * this.data.scale.x;
				}
				else
				{
					num2 += (tk2dFontChar.advance + this.data.spacing) * this.data.scale.x;
				}
				if (!flag3)
				{
					stringBuilder.Append(c);
				}
			}
			IL_3EE:;
		}
		_targetString = stringBuilder.ToString();
	}

	// Token: 0x06003DF9 RID: 15865 RVA: 0x00138EF0 File Offset: 0x001370F0
	private void SetNeedUpdate(tk2dTextMesh.UpdateFlags uf)
	{
		if (this.updateFlags == tk2dTextMesh.UpdateFlags.UpdateNone)
		{
			this.updateFlags |= uf;
			tk2dUpdateManager.QueueCommit(this);
		}
		else
		{
			this.updateFlags |= uf;
		}
	}

	// Token: 0x1700094D RID: 2381
	// (get) Token: 0x06003DFA RID: 15866 RVA: 0x00138F24 File Offset: 0x00137124
	// (set) Token: 0x06003DFB RID: 15867 RVA: 0x00138F38 File Offset: 0x00137138
	public tk2dFontData font
	{
		get
		{
			this.UpgradeData();
			return this.data.font;
		}
		set
		{
			this.UpgradeData();
			this.data.font = value;
			this._fontInst = this.data.font.inst;
			this.SetNeedUpdate(tk2dTextMesh.UpdateFlags.UpdateText);
			this.UpdateMaterial();
		}
	}

	// Token: 0x1700094E RID: 2382
	// (get) Token: 0x06003DFC RID: 15868 RVA: 0x00138F70 File Offset: 0x00137170
	// (set) Token: 0x06003DFD RID: 15869 RVA: 0x00138F84 File Offset: 0x00137184
	public bool formatting
	{
		get
		{
			this.UpgradeData();
			return this.data.formatting;
		}
		set
		{
			this.UpgradeData();
			if (this.data.formatting != value)
			{
				this.data.formatting = value;
				this.SetNeedUpdate(tk2dTextMesh.UpdateFlags.UpdateText);
			}
		}
	}

	// Token: 0x1700094F RID: 2383
	// (get) Token: 0x06003DFE RID: 15870 RVA: 0x00138FB0 File Offset: 0x001371B0
	// (set) Token: 0x06003DFF RID: 15871 RVA: 0x00138FC4 File Offset: 0x001371C4
	public int wordWrapWidth
	{
		get
		{
			this.UpgradeData();
			return this.data.wordWrapWidth;
		}
		set
		{
			this.UpgradeData();
			if (this.data.wordWrapWidth != value)
			{
				this.data.wordWrapWidth = value;
				this.SetNeedUpdate(tk2dTextMesh.UpdateFlags.UpdateText);
			}
		}
	}

	// Token: 0x17000950 RID: 2384
	// (get) Token: 0x06003E00 RID: 15872 RVA: 0x00138FF0 File Offset: 0x001371F0
	// (set) Token: 0x06003E01 RID: 15873 RVA: 0x00139004 File Offset: 0x00137204
	public string text
	{
		get
		{
			this.UpgradeData();
			return this.data.text;
		}
		set
		{
			this.UpgradeData();
			this.data.text = value;
			this.SetNeedUpdate(tk2dTextMesh.UpdateFlags.UpdateText);
		}
	}

	// Token: 0x17000951 RID: 2385
	// (get) Token: 0x06003E02 RID: 15874 RVA: 0x00139020 File Offset: 0x00137220
	// (set) Token: 0x06003E03 RID: 15875 RVA: 0x00139034 File Offset: 0x00137234
	public Color color
	{
		get
		{
			this.UpgradeData();
			return this.data.color;
		}
		set
		{
			this.UpgradeData();
			this.data.color = value;
			this.SetNeedUpdate(tk2dTextMesh.UpdateFlags.UpdateColors);
		}
	}

	// Token: 0x17000952 RID: 2386
	// (get) Token: 0x06003E04 RID: 15876 RVA: 0x00139050 File Offset: 0x00137250
	// (set) Token: 0x06003E05 RID: 15877 RVA: 0x00139064 File Offset: 0x00137264
	public Color color2
	{
		get
		{
			this.UpgradeData();
			return this.data.color2;
		}
		set
		{
			this.UpgradeData();
			this.data.color2 = value;
			this.SetNeedUpdate(tk2dTextMesh.UpdateFlags.UpdateColors);
		}
	}

	// Token: 0x17000953 RID: 2387
	// (get) Token: 0x06003E06 RID: 15878 RVA: 0x00139080 File Offset: 0x00137280
	// (set) Token: 0x06003E07 RID: 15879 RVA: 0x00139094 File Offset: 0x00137294
	public bool useGradient
	{
		get
		{
			this.UpgradeData();
			return this.data.useGradient;
		}
		set
		{
			this.UpgradeData();
			this.data.useGradient = value;
			this.SetNeedUpdate(tk2dTextMesh.UpdateFlags.UpdateColors);
		}
	}

	// Token: 0x17000954 RID: 2388
	// (get) Token: 0x06003E08 RID: 15880 RVA: 0x001390B0 File Offset: 0x001372B0
	// (set) Token: 0x06003E09 RID: 15881 RVA: 0x001390C4 File Offset: 0x001372C4
	public TextAnchor anchor
	{
		get
		{
			this.UpgradeData();
			return this.data.anchor;
		}
		set
		{
			this.UpgradeData();
			this.data.anchor = value;
			this.SetNeedUpdate(tk2dTextMesh.UpdateFlags.UpdateText);
		}
	}

	// Token: 0x17000955 RID: 2389
	// (get) Token: 0x06003E0A RID: 15882 RVA: 0x001390E0 File Offset: 0x001372E0
	// (set) Token: 0x06003E0B RID: 15883 RVA: 0x001390F4 File Offset: 0x001372F4
	public Vector3 scale
	{
		get
		{
			this.UpgradeData();
			return this.data.scale;
		}
		set
		{
			this.UpgradeData();
			this.data.scale = value;
			this.SetNeedUpdate(tk2dTextMesh.UpdateFlags.UpdateText);
		}
	}

	// Token: 0x17000956 RID: 2390
	// (get) Token: 0x06003E0C RID: 15884 RVA: 0x00139110 File Offset: 0x00137310
	// (set) Token: 0x06003E0D RID: 15885 RVA: 0x00139124 File Offset: 0x00137324
	public bool kerning
	{
		get
		{
			this.UpgradeData();
			return this.data.kerning;
		}
		set
		{
			this.UpgradeData();
			this.data.kerning = value;
			this.SetNeedUpdate(tk2dTextMesh.UpdateFlags.UpdateText);
		}
	}

	// Token: 0x17000957 RID: 2391
	// (get) Token: 0x06003E0E RID: 15886 RVA: 0x00139140 File Offset: 0x00137340
	// (set) Token: 0x06003E0F RID: 15887 RVA: 0x00139154 File Offset: 0x00137354
	public int maxChars
	{
		get
		{
			this.UpgradeData();
			return this.data.maxChars;
		}
		set
		{
			this.UpgradeData();
			this.data.maxChars = value;
			this.SetNeedUpdate(tk2dTextMesh.UpdateFlags.UpdateBuffers);
		}
	}

	// Token: 0x17000958 RID: 2392
	// (get) Token: 0x06003E10 RID: 15888 RVA: 0x00139170 File Offset: 0x00137370
	// (set) Token: 0x06003E11 RID: 15889 RVA: 0x00139184 File Offset: 0x00137384
	public int textureGradient
	{
		get
		{
			this.UpgradeData();
			return this.data.textureGradient;
		}
		set
		{
			this.UpgradeData();
			this.data.textureGradient = value % this.font.gradientCount;
			this.SetNeedUpdate(tk2dTextMesh.UpdateFlags.UpdateText);
		}
	}

	// Token: 0x17000959 RID: 2393
	// (get) Token: 0x06003E12 RID: 15890 RVA: 0x001391AC File Offset: 0x001373AC
	// (set) Token: 0x06003E13 RID: 15891 RVA: 0x001391C0 File Offset: 0x001373C0
	public bool inlineStyling
	{
		get
		{
			this.UpgradeData();
			return this.data.inlineStyling;
		}
		set
		{
			this.UpgradeData();
			this.data.inlineStyling = value;
			this.SetNeedUpdate(tk2dTextMesh.UpdateFlags.UpdateText);
		}
	}

	// Token: 0x1700095A RID: 2394
	// (get) Token: 0x06003E14 RID: 15892 RVA: 0x001391DC File Offset: 0x001373DC
	// (set) Token: 0x06003E15 RID: 15893 RVA: 0x001391F0 File Offset: 0x001373F0
	public float Spacing
	{
		get
		{
			this.UpgradeData();
			return this.data.spacing;
		}
		set
		{
			this.UpgradeData();
			if (this.data.spacing != value)
			{
				this.data.spacing = value;
				this.SetNeedUpdate(tk2dTextMesh.UpdateFlags.UpdateText);
			}
		}
	}

	// Token: 0x1700095B RID: 2395
	// (get) Token: 0x06003E16 RID: 15894 RVA: 0x0013921C File Offset: 0x0013741C
	// (set) Token: 0x06003E17 RID: 15895 RVA: 0x00139230 File Offset: 0x00137430
	public float LineSpacing
	{
		get
		{
			this.UpgradeData();
			return this.data.lineSpacing;
		}
		set
		{
			this.UpgradeData();
			if (this.data.lineSpacing != value)
			{
				this.data.lineSpacing = value;
				this.SetNeedUpdate(tk2dTextMesh.UpdateFlags.UpdateText);
			}
		}
	}

	// Token: 0x1700095C RID: 2396
	// (get) Token: 0x06003E18 RID: 15896 RVA: 0x0013925C File Offset: 0x0013745C
	// (set) Token: 0x06003E19 RID: 15897 RVA: 0x0013926C File Offset: 0x0013746C
	public int SortingOrder
	{
		get
		{
			return this.CachedRenderer.sortingOrder;
		}
		set
		{
			if (this.CachedRenderer.sortingOrder != value)
			{
				this.data.renderLayer = value;
				this.CachedRenderer.sortingOrder = value;
			}
		}
	}

	// Token: 0x06003E1A RID: 15898 RVA: 0x00139298 File Offset: 0x00137498
	private void InitInstance()
	{
		if (this._fontInst == null && this.data.font != null)
		{
			this._fontInst = this.data.font.inst;
		}
	}

	// Token: 0x1700095D RID: 2397
	// (get) Token: 0x06003E1B RID: 15899 RVA: 0x001392D8 File Offset: 0x001374D8
	private Renderer CachedRenderer
	{
		get
		{
			if (this._cachedRenderer == null)
			{
				this._cachedRenderer = base.GetComponent<Renderer>();
			}
			return this._cachedRenderer;
		}
	}

	// Token: 0x06003E1C RID: 15900 RVA: 0x00139300 File Offset: 0x00137500
	private void Awake()
	{
		this.UpgradeData();
		if (this.data.font != null)
		{
			this._fontInst = this.data.font.inst;
		}
		this.updateFlags = tk2dTextMesh.UpdateFlags.UpdateBuffers;
		if (this.data.font != null)
		{
			this.Init();
			this.UpdateMaterial();
		}
		this.updateFlags = tk2dTextMesh.UpdateFlags.UpdateNone;
	}

	// Token: 0x06003E1D RID: 15901 RVA: 0x00139370 File Offset: 0x00137570
	private void Update()
	{
		if (this.supportsWooblyText && Application.isPlaying)
		{
			this.UpdateWooblyTextBuffers();
		}
	}

	// Token: 0x06003E1E RID: 15902 RVA: 0x00139390 File Offset: 0x00137590
	protected void InitWooblyTextBuffers()
	{
		if (this.indices == null)
		{
			this.indices = new List<int>();
		}
		this.indices.Clear();
		int num = 0;
		if (this.woobleBuffer == null || this.woobleBuffer.Length != this.FormattedText.Length)
		{
			if (this.woobleBuffer == null)
			{
				this.woobleBuffer = new Vector2[this.FormattedText.Length];
				this.woobleTimes = new float[this.FormattedText.Length];
				this.woobleRainbowBuffer = new bool[this.FormattedText.Length];
			}
			else
			{
				Array.Resize<float>(ref this.woobleTimes, this.FormattedText.Length);
				Array.Resize<Vector2>(ref this.woobleBuffer, this.FormattedText.Length);
				Array.Resize<bool>(ref this.woobleRainbowBuffer, this.FormattedText.Length);
			}
			for (int i = 0; i < this.woobleStartIndices.Length; i++)
			{
				int num2 = this.woobleStartIndices[i];
				int num3 = this.woobleEndIndices[i];
				switch (this.woobleStyles[i])
				{
				case tk2dTextMesh.WoobleStyle.SEQUENTIAL:
				{
					for (int j = num2; j <= num3; j++)
					{
						if (j >= 0 && j < this.woobleTimes.Length)
						{
							if (num3 + 1 - num2 > 0)
							{
								float num4 = ((float)j - (float)num2 * 1f) / (float)(num3 + 1 - num2);
								this.woobleTimes[j] = -1f * num4;
								this.woobleRainbowBuffer[j] = false;
							}
						}
					}
					break;
				}
				case tk2dTextMesh.WoobleStyle.RANDOM_JITTER:
				{
					for (int k = num2; k <= num3; k++)
					{
						int num5 = num3 - num2;
						int num6 = Mathf.FloorToInt((float)num5 / 2f + 1f);
						this.indices.Add(num2 + num);
						num = (num + num6) % Mathf.Max(1, num5);
					}
					for (int l = num2; l <= num3; l++)
					{
						if (l >= 0 && l < this.woobleTimes.Length)
						{
							if (num3 + 1 - num2 > 0)
							{
								int num7 = this.indices[l - num2];
								float num8 = ((float)num7 - (float)num2 * 1f) / (float)(num3 + 1 - num2);
								this.woobleTimes[l] = -1f * num8;
								this.woobleRainbowBuffer[l] = false;
							}
						}
					}
					break;
				}
				case tk2dTextMesh.WoobleStyle.RANDOM_SEQUENTIAL:
				{
					for (int m = num2; m <= num3; m++)
					{
						int num9 = num3 - num2;
						int num10 = Mathf.FloorToInt((float)num9 / 2f + 1f);
						this.indices.Add(num2 + num);
						num = (num + num10) % Mathf.Max(1, num9);
					}
					for (int n = num2; n <= num3; n++)
					{
						if (n >= 0 && n < this.woobleTimes.Length)
						{
							if (num3 + 1 - num2 > 0)
							{
								int num11 = this.indices[n - num2];
								float num12 = ((float)num11 - (float)num2 * 1f) / (float)(num3 + 1 - num2);
								this.woobleTimes[n] = -1f * num12;
								this.woobleRainbowBuffer[n] = false;
							}
						}
					}
					break;
				}
				case tk2dTextMesh.WoobleStyle.SEQUENTIAL_RAINBOW:
				{
					for (int num13 = num2; num13 <= num3; num13++)
					{
						if (num13 >= 0 && num13 < this.woobleTimes.Length)
						{
							if (num3 + 1 - num2 > 0)
							{
								float num14 = ((float)num13 - (float)num2 * 1f) / (float)(num3 + 1 - num2);
								this.woobleTimes[num13] = -1f * num14;
								this.woobleRainbowBuffer[num13] = true;
							}
						}
					}
					break;
				}
				}
			}
		}
	}

	// Token: 0x06003E1F RID: 15903 RVA: 0x0013976C File Offset: 0x0013796C
	protected void UpdateWooblyTextBuffers()
	{
		if (this.woobleBuffer == null || this.woobleBuffer.Length != this.FormattedText.Length)
		{
			this.InitWooblyTextBuffers();
		}
		float num = 3f;
		for (int i = 0; i < this.woobleStartIndices.Length; i++)
		{
			int num2 = this.woobleStartIndices[i];
			int num3 = this.woobleEndIndices[i];
			switch (this.woobleStyles[i])
			{
			case tk2dTextMesh.WoobleStyle.SEQUENTIAL:
			case tk2dTextMesh.WoobleStyle.RANDOM_SEQUENTIAL:
			{
				for (int j = num2; j <= num3; j++)
				{
					if (j >= 0 && j < this.woobleTimes.Length)
					{
						this.woobleTimes[j] = this.woobleTimes[j] + BraveTime.DeltaTime * num;
						float num4 = ((this.woobleTimes[j] >= 0f) ? (BraveMathCollege.HermiteInterpolation(Mathf.PingPong(this.woobleTimes[j], 1f)) * 0.25f - 0.0625f) : 0f);
						this.woobleBuffer[j] = new Vector2(0f, num4);
					}
				}
				break;
			}
			case tk2dTextMesh.WoobleStyle.SIMULTANEOUS:
			{
				for (int k = num2; k <= num3; k++)
				{
					if (k >= 0 && k < this.woobleTimes.Length)
					{
						this.woobleTimes[k] = this.woobleTimes[k] + BraveTime.DeltaTime * num;
						float num5 = ((this.woobleTimes[k] >= 0f) ? (BraveMathCollege.HermiteInterpolation(Mathf.PingPong(this.woobleTimes[k], 1f)) * 0.25f - 0.0625f) : 0f);
						this.woobleBuffer[k] = new Vector2(0f, num5);
					}
				}
				break;
			}
			case tk2dTextMesh.WoobleStyle.RANDOM_JITTER:
			{
				for (int l = num2; l <= num3; l++)
				{
					if (l >= 0 && l < this.woobleTimes.Length)
					{
						this.woobleTimes[l] = this.woobleTimes[l] + BraveTime.DeltaTime * num;
						if (this.woobleTimes[l] > 1f)
						{
							this.woobleTimes[l] -= 1f;
							this.woobleBuffer[l] = Vector2.Scale(new Vector2(0.03125f, 0.03125f), BraveUtility.GetMajorAxis(UnityEngine.Random.insideUnitCircle.normalized));
							this.woobleBuffer[l].x = Mathf.Abs(this.woobleBuffer[l].x);
							this.woobleBuffer[(l != num2) ? (l - 1) : num3] = Vector2.zero;
						}
					}
				}
				break;
			}
			case tk2dTextMesh.WoobleStyle.SEQUENTIAL_RAINBOW:
			{
				for (int m = num2; m <= num3; m++)
				{
					if (m >= 0 && m < this.woobleTimes.Length)
					{
						this.woobleTimes[m] = this.woobleTimes[m] + BraveTime.DeltaTime * num;
						float num6 = ((this.woobleTimes[m] >= 0f) ? (BraveMathCollege.HermiteInterpolation(Mathf.PingPong(this.woobleTimes[m], 1f)) * 0.25f - 0.0625f) : 0f);
						this.woobleBuffer[m] = new Vector2(0f, num6);
					}
				}
				break;
			}
			}
		}
		this.SetNeedUpdate(tk2dTextMesh.UpdateFlags.UpdateText);
	}

	// Token: 0x06003E20 RID: 15904 RVA: 0x00139B20 File Offset: 0x00137D20
	protected void OnDestroy()
	{
		if (this.meshFilter == null)
		{
			this.meshFilter = base.GetComponent<MeshFilter>();
		}
		if (this.meshFilter != null)
		{
			this.mesh = this.meshFilter.sharedMesh;
		}
		if (this.mesh)
		{
			UnityEngine.Object.DestroyImmediate(this.mesh, true);
			this.meshFilter.mesh = null;
		}
	}

	// Token: 0x1700095E RID: 2398
	// (get) Token: 0x06003E21 RID: 15905 RVA: 0x00139B94 File Offset: 0x00137D94
	private bool useInlineStyling
	{
		get
		{
			return this.inlineStyling && this._fontInst.textureGradients;
		}
	}

	// Token: 0x06003E22 RID: 15906 RVA: 0x00139BB0 File Offset: 0x00137DB0
	public int NumDrawnCharacters()
	{
		int num = this.NumTotalCharacters();
		if (num > this.data.maxChars)
		{
			num = this.data.maxChars;
		}
		return num;
	}

	// Token: 0x06003E23 RID: 15907 RVA: 0x00139BE4 File Offset: 0x00137DE4
	public int NumTotalCharacters()
	{
		this.InitInstance();
		if ((this.updateFlags & (tk2dTextMesh.UpdateFlags.UpdateText | tk2dTextMesh.UpdateFlags.UpdateBuffers)) != tk2dTextMesh.UpdateFlags.UpdateNone)
		{
			this.FormatText();
		}
		int num = 0;
		for (int i = 0; i < this._formattedText.Length; i++)
		{
			int num2 = (int)this._formattedText[i];
			bool flag = num2 == 94;
			if (this._fontInst.useDictionary)
			{
				if (!this._fontInst.charDict.ContainsKey(num2))
				{
					num2 = 0;
				}
			}
			else if (num2 >= this._fontInst.chars.Length)
			{
				num2 = 0;
			}
			if (flag)
			{
				num2 = 94;
			}
			if (num2 != 10)
			{
				if (this.data.inlineStyling && num2 == 94 && i + 1 < this._formattedText.Length)
				{
					if (this._formattedText[i + 1] != '^')
					{
						i += tk2dTextMesh.GetInlineStyleCommandLength((int)this._formattedText[i + 1]);
						goto IL_F5;
					}
					i++;
				}
				num++;
			}
			IL_F5:;
		}
		return num;
	}

	// Token: 0x06003E24 RID: 15908 RVA: 0x00139CFC File Offset: 0x00137EFC
	[Obsolete("Use GetEstimatedMeshBoundsForString().size instead")]
	public Vector2 GetMeshDimensionsForString(string str)
	{
		return tk2dTextGeomGen.GetMeshDimensionsForString(str, tk2dTextGeomGen.Data(this.data, this._fontInst, this._formattedText));
	}

	// Token: 0x06003E25 RID: 15909 RVA: 0x00139D1C File Offset: 0x00137F1C
	public Bounds GetEstimatedMeshBoundsForString(string str)
	{
		this.InitInstance();
		tk2dTextGeomGen.GeomData geomData = tk2dTextGeomGen.Data(this.data, this._fontInst, this._formattedText);
		Vector2 meshDimensionsForString = tk2dTextGeomGen.GetMeshDimensionsForString(this.FormatText(str), geomData);
		float yanchorForHeight = tk2dTextGeomGen.GetYAnchorForHeight(meshDimensionsForString.y, geomData);
		float xanchorForWidth = tk2dTextGeomGen.GetXAnchorForWidth(meshDimensionsForString.x, geomData);
		float num = (this._fontInst.lineHeight + this.data.lineSpacing) * this.data.scale.y;
		return new Bounds(new Vector3(xanchorForWidth + meshDimensionsForString.x * 0.5f, yanchorForHeight + meshDimensionsForString.y * 0.5f + num, 0f), Vector3.Scale(meshDimensionsForString, new Vector3(1f, -1f, 1f)));
	}

	// Token: 0x06003E26 RID: 15910 RVA: 0x00139DEC File Offset: 0x00137FEC
	public Bounds GetTrueBounds()
	{
		return this.mesh.bounds;
	}

	// Token: 0x06003E27 RID: 15911 RVA: 0x00139DFC File Offset: 0x00137FFC
	public void Init(bool force)
	{
		if (force)
		{
			this.SetNeedUpdate(tk2dTextMesh.UpdateFlags.UpdateBuffers);
		}
		this.Init();
	}

	// Token: 0x06003E28 RID: 15912 RVA: 0x00139E14 File Offset: 0x00138014
	private void UpdateRainbowStatus(bool[] rainbowValues)
	{
		if (Foyer.DoIntroSequence)
		{
			return;
		}
		bool flag = false;
		for (int i = 0; i < rainbowValues.Length; i++)
		{
			flag = flag || rainbowValues[i];
		}
		if (flag)
		{
			this.color = Color.white;
		}
		else
		{
			this.color = Color.black;
		}
	}

	// Token: 0x06003E29 RID: 15913 RVA: 0x00139E70 File Offset: 0x00138070
	public void Init()
	{
		if (this._fontInst && ((this.updateFlags & tk2dTextMesh.UpdateFlags.UpdateBuffers) != tk2dTextMesh.UpdateFlags.UpdateNone || this.mesh == null))
		{
			this._fontInst.InitDictionary();
			this.FormatText();
			tk2dTextGeomGen.GeomData geomData = tk2dTextGeomGen.Data(this.data, this._fontInst, this._formattedText);
			int num;
			int num2;
			tk2dTextGeomGen.GetTextMeshGeomDesc(out num, out num2, geomData);
			this.vertices = new Vector3[num];
			this.uvs = new Vector2[num];
			this.colors = new Color32[num];
			this.untintedColors = new Color32[num];
			if (this._fontInst.textureGradients)
			{
				this.uv2 = new Vector2[num];
			}
			int[] array = new int[num2];
			if (this.supportsWooblyText)
			{
				this.InitWooblyTextBuffers();
			}
			if (this.supportsWooblyText)
			{
				this.UpdateRainbowStatus(this.woobleRainbowBuffer);
			}
			int num3 = ((!this.supportsWooblyText) ? tk2dTextGeomGen.SetTextMeshGeom(this.vertices, this.uvs, this.uv2, this.untintedColors, 0, geomData, this.visibleCharacters) : tk2dTextGeomGen.SetTextMeshGeom(this.vertices, this.uvs, this.uv2, this.untintedColors, 0, geomData, this.visibleCharacters, this.woobleBuffer, this.woobleRainbowBuffer));
			if (!this._fontInst.isPacked)
			{
				Color32 color = this.data.color;
				Color32 color2 = ((!this.data.useGradient) ? this.data.color : this.data.color2);
				for (int i = 0; i < num; i++)
				{
					Color32 color3 = ((i % 4 >= 2) ? color2 : color);
					byte b = this.untintedColors[i].r * color3.r / byte.MaxValue;
					byte b2 = this.untintedColors[i].g * color3.g / byte.MaxValue;
					byte b3 = this.untintedColors[i].b * color3.b / byte.MaxValue;
					byte b4 = this.untintedColors[i].a * color3.a / byte.MaxValue;
					if (this._fontInst.premultipliedAlpha)
					{
						b = b * b4 / byte.MaxValue;
						b2 = b2 * b4 / byte.MaxValue;
						b3 = b3 * b4 / byte.MaxValue;
					}
					this.colors[i] = new Color32(b, b2, b3, b4);
				}
			}
			else
			{
				this.colors = this.untintedColors;
			}
			tk2dTextGeomGen.SetTextMeshIndices(array, 0, 0, geomData, num3);
			if (this.mesh == null)
			{
				if (this.meshFilter == null)
				{
					this.meshFilter = base.GetComponent<MeshFilter>();
				}
				this.mesh = new Mesh();
				this.mesh.hideFlags = HideFlags.DontSave;
				this.meshFilter.mesh = this.mesh;
			}
			else
			{
				this.mesh.Clear();
			}
			this.mesh.vertices = this.vertices;
			this.mesh.uv = this.uvs;
			if (this.font.textureGradients)
			{
				this.mesh.uv2 = this.uv2;
			}
			this.mesh.triangles = array;
			this.mesh.colors32 = this.colors;
			this.mesh.RecalculateBounds();
			this.mesh.bounds = tk2dBaseSprite.AdjustedMeshBounds(this.mesh.bounds, this.data.renderLayer);
			this.updateFlags = tk2dTextMesh.UpdateFlags.UpdateNone;
		}
	}

	// Token: 0x06003E2A RID: 15914 RVA: 0x0013A240 File Offset: 0x00138440
	public void Commit()
	{
		tk2dUpdateManager.FlushQueues();
	}

	// Token: 0x06003E2B RID: 15915 RVA: 0x0013A248 File Offset: 0x00138448
	public void CheckFontsForLanguage()
	{
		this.InitInstance();
		if (this.m_defaultAssignedFont == null)
		{
			this.m_defaultAssignedFont = this.font;
		}
		tk2dFontData defaultAssignedFont;
		if (GameManager.Options.CurrentLanguage == StringTableManager.GungeonSupportedLanguages.JAPANESE)
		{
			defaultAssignedFont = (ResourceCache.Acquire("Alternate Fonts/JackeyFont_TK2D") as GameObject).GetComponent<tk2dFont>().data;
		}
		else if (GameManager.Options.CurrentLanguage == StringTableManager.GungeonSupportedLanguages.RUSSIAN)
		{
			defaultAssignedFont = (ResourceCache.Acquire("Alternate Fonts/PixelaCYR_15_TK2D") as GameObject).GetComponent<tk2dFont>().data;
		}
		else if (GameManager.Options.CurrentLanguage == StringTableManager.GungeonSupportedLanguages.CHINESE)
		{
			defaultAssignedFont = (ResourceCache.Acquire("Alternate Fonts/SimSun12_TK2D") as GameObject).GetComponent<tk2dFont>().data;
		}
		else if (GameManager.Options.CurrentLanguage == StringTableManager.GungeonSupportedLanguages.KOREAN)
		{
			defaultAssignedFont = (ResourceCache.Acquire("Alternate Fonts/NanumGothic16TK2D") as GameObject).GetComponent<tk2dFont>().data;
		}
		else
		{
			defaultAssignedFont = this.m_defaultAssignedFont;
		}
		if (defaultAssignedFont != null && this.font != defaultAssignedFont)
		{
			this.font = defaultAssignedFont;
			this.Init(true);
		}
	}

	// Token: 0x06003E2C RID: 15916 RVA: 0x0013A36C File Offset: 0x0013856C
	public void DoNotUse__CommitInternal()
	{
		this.InitInstance();
		this.CheckFontsForLanguage();
		if (this._fontInst == null)
		{
			return;
		}
		this._fontInst.InitDictionary();
		if ((this.updateFlags & tk2dTextMesh.UpdateFlags.UpdateBuffers) != tk2dTextMesh.UpdateFlags.UpdateNone || this.mesh == null)
		{
			this.Init();
		}
		else
		{
			if ((this.updateFlags & tk2dTextMesh.UpdateFlags.UpdateText) != tk2dTextMesh.UpdateFlags.UpdateNone)
			{
				this.FormatText();
				tk2dTextGeomGen.GeomData geomData = tk2dTextGeomGen.Data(this.data, this._fontInst, this._formattedText);
				if (this.supportsWooblyText && this.woobleBuffer.Length != this.FormattedText.Length)
				{
					this.InitWooblyTextBuffers();
				}
				if (this.supportsWooblyText)
				{
					this.UpdateRainbowStatus(this.woobleRainbowBuffer);
				}
				int num = ((!this.supportsWooblyText || !Application.isPlaying) ? tk2dTextGeomGen.SetTextMeshGeom(this.vertices, this.uvs, this.uv2, this.untintedColors, 0, geomData, this.visibleCharacters) : tk2dTextGeomGen.SetTextMeshGeom(this.vertices, this.uvs, this.uv2, this.untintedColors, 0, geomData, this.visibleCharacters, this.woobleBuffer, this.woobleRainbowBuffer));
				float num2 = float.MaxValue;
				float num3 = float.MinValue;
				for (int i = 0; i < this.vertices.Length; i++)
				{
					num2 = Mathf.Min(num2, this.vertices[i].x);
					num3 = Mathf.Max(num3, this.vertices[i].x);
				}
				int num4 = 0;
				int num5 = 0;
				int num6 = 0;
				for (int j = 0; j < geomData.formattedText.Length; j++)
				{
					string formattedText = geomData.formattedText;
					int num7 = (int)formattedText[j];
					if (num7 == 91 && j < formattedText.Length - 1 && formattedText[j + 1] != ']')
					{
						for (int k = j; k < formattedText.Length; k++)
						{
							char c = formattedText[k];
							if (c == ']')
							{
								int num8 = k - j;
								string text = formattedText.Substring(j + 9, num8 - 10);
								GameObject gameObject;
								if (this.m_inlineSprites.Count > num4)
								{
									gameObject = this.m_inlineSprites[num4];
								}
								else
								{
									gameObject = (GameObject)UnityEngine.Object.Instantiate(BraveResources.Load("ControllerButtonSprite", ".prefab"));
								}
								tk2dSprite component = gameObject.GetComponent<tk2dSprite>();
								component.HeightOffGround = 3f;
								DepthLookupManager.AssignRendererToSortingLayer(component.renderer, DepthLookupManager.GungeonSortingLayer.FOREGROUND);
								gameObject.SetLayerRecursively(base.gameObject.layer);
								component.spriteId = component.GetSpriteIdByName(text);
								component.transform.parent = base.transform;
								component.transform.localPosition = tk2dTextGeomGen.inlineSpriteOffsetsForLastString[num6];
								if (!this.m_inlineSprites.Contains(gameObject))
								{
									this.m_inlineSprites.Add(gameObject);
									this.m_inlineSpriteIndices.Add(j - num5);
								}
								else
								{
									this.m_inlineSpriteIndices[this.m_inlineSprites.IndexOf(gameObject)] = j - num5;
								}
								j += num8;
								num5 += num8;
								num6++;
								num4++;
								break;
							}
						}
					}
				}
				for (int l = num4; l < this.m_inlineSprites.Count; l++)
				{
					if (Application.isPlaying)
					{
						UnityEngine.Object.Destroy(this.m_inlineSprites[l]);
					}
					else
					{
						UnityEngine.Object.DestroyImmediate(this.m_inlineSprites[l]);
					}
					this.m_inlineSprites.RemoveAt(l);
					this.m_inlineSpriteIndices.RemoveAt(l);
					l--;
				}
				for (int m = 0; m < this.m_inlineSprites.Count; m++)
				{
					if (this.m_inlineSpriteIndices[m] > this.visibleCharacters)
					{
						this.m_inlineSprites[m].GetComponent<Renderer>().enabled = false;
					}
					else
					{
						this.m_inlineSprites[m].GetComponent<Renderer>().enabled = true;
					}
				}
				for (int n = num; n < this.data.maxChars; n++)
				{
					this.vertices[n * 4] = (this.vertices[n * 4 + 1] = (this.vertices[n * 4 + 2] = (this.vertices[n * 4 + 3] = Vector3.zero)));
				}
				this.mesh.vertices = this.vertices;
				this.mesh.uv = this.uvs;
				if (this._fontInst.textureGradients)
				{
					this.mesh.uv2 = this.uv2;
				}
				if (this._fontInst.isPacked)
				{
					this.colors = this.untintedColors;
					this.mesh.colors32 = this.colors;
				}
				if (this.data.inlineStyling)
				{
					this.SetNeedUpdate(tk2dTextMesh.UpdateFlags.UpdateColors);
				}
				this.mesh.RecalculateBounds();
				this.mesh.bounds = tk2dBaseSprite.AdjustedMeshBounds(this.mesh.bounds, this.data.renderLayer);
			}
			if (!this.font.isPacked && (this.updateFlags & tk2dTextMesh.UpdateFlags.UpdateColors) != tk2dTextMesh.UpdateFlags.UpdateNone)
			{
				Color32 color = this.data.color;
				Color32 color2 = ((!this.data.useGradient) ? this.data.color : this.data.color2);
				for (int num9 = 0; num9 < this.colors.Length; num9++)
				{
					Color32 color3 = ((num9 % 4 >= 2) ? color2 : color);
					byte b = this.untintedColors[num9].r * color3.r / byte.MaxValue;
					byte b2 = this.untintedColors[num9].g * color3.g / byte.MaxValue;
					byte b3 = this.untintedColors[num9].b * color3.b / byte.MaxValue;
					byte b4 = this.untintedColors[num9].a * color3.a / byte.MaxValue;
					if (this._fontInst.premultipliedAlpha)
					{
						b = b * b4 / byte.MaxValue;
						b2 = b2 * b4 / byte.MaxValue;
						b3 = b3 * b4 / byte.MaxValue;
					}
					this.colors[num9] = new Color32(b, b2, b3, b4);
				}
				this.mesh.colors32 = this.colors;
			}
		}
		this.updateFlags = tk2dTextMesh.UpdateFlags.UpdateNone;
	}

	// Token: 0x06003E2D RID: 15917 RVA: 0x0013AA8C File Offset: 0x00138C8C
	public void MakePixelPerfect()
	{
		float num = 1f;
		tk2dCamera tk2dCamera = tk2dCamera.CameraForLayer(base.gameObject.layer);
		if (tk2dCamera != null)
		{
			if (this._fontInst.version < 1)
			{
				Debug.LogError("Need to rebuild font.");
			}
			float num2 = base.transform.position.z - tk2dCamera.transform.position.z;
			float num3 = this._fontInst.invOrthoSize * this._fontInst.halfTargetHeight;
			num = tk2dCamera.GetSizeAtDistance(num2) * num3;
		}
		else if (Camera.main)
		{
			if (Camera.main.orthographic)
			{
				num = Camera.main.orthographicSize;
			}
			else
			{
				float num4 = base.transform.position.z - Camera.main.transform.position.z;
				num = tk2dPixelPerfectHelper.CalculateScaleForPerspectiveCamera(Camera.main.fieldOfView, num4);
			}
			num *= this._fontInst.invOrthoSize;
		}
		this.scale = new Vector3(Mathf.Sign(this.scale.x) * num, Mathf.Sign(this.scale.y) * num, Mathf.Sign(this.scale.z) * num);
	}

	// Token: 0x06003E2E RID: 15918 RVA: 0x0013ABF4 File Offset: 0x00138DF4
	public bool UsesSpriteCollection(tk2dSpriteCollectionData spriteCollection)
	{
		return !(this.data.font != null) || !(this.data.font.spriteCollection != null) || this.data.font.spriteCollection == spriteCollection;
	}

	// Token: 0x06003E2F RID: 15919 RVA: 0x0013AC4C File Offset: 0x00138E4C
	private void UpdateMaterial()
	{
		if (base.GetComponent<Renderer>().sharedMaterial != this._fontInst.materialInst)
		{
			base.GetComponent<Renderer>().material = this._fontInst.materialInst;
		}
	}

	// Token: 0x06003E30 RID: 15920 RVA: 0x0013AC84 File Offset: 0x00138E84
	public void ForceBuild()
	{
		if (this.data.font != null)
		{
			this._fontInst = this.data.font.inst;
			this.UpdateMaterial();
		}
		this.Init(true);
	}

	// Token: 0x0400309D RID: 12445
	private tk2dFontData _fontInst;

	// Token: 0x0400309E RID: 12446
	private string _formattedText = string.Empty;

	// Token: 0x0400309F RID: 12447
	[SerializeField]
	private tk2dFontData _font;

	// Token: 0x040030A0 RID: 12448
	[SerializeField]
	private string _text = string.Empty;

	// Token: 0x040030A1 RID: 12449
	[SerializeField]
	private Color _color = Color.white;

	// Token: 0x040030A2 RID: 12450
	[SerializeField]
	private Color _color2 = Color.white;

	// Token: 0x040030A3 RID: 12451
	[SerializeField]
	private bool _useGradient;

	// Token: 0x040030A4 RID: 12452
	[SerializeField]
	private int _textureGradient;

	// Token: 0x040030A5 RID: 12453
	[SerializeField]
	private TextAnchor _anchor = TextAnchor.LowerLeft;

	// Token: 0x040030A6 RID: 12454
	[SerializeField]
	private Vector3 _scale = new Vector3(1f, 1f, 1f);

	// Token: 0x040030A7 RID: 12455
	[SerializeField]
	private bool _kerning;

	// Token: 0x040030A8 RID: 12456
	[SerializeField]
	private int _maxChars = 16;

	// Token: 0x040030A9 RID: 12457
	[SerializeField]
	private bool _inlineStyling;

	// Token: 0x040030AA RID: 12458
	[SerializeField]
	public bool supportsWooblyText;

	// Token: 0x040030AB RID: 12459
	[SerializeField]
	public int[] woobleStartIndices;

	// Token: 0x040030AC RID: 12460
	[SerializeField]
	public int[] woobleEndIndices;

	// Token: 0x040030AD RID: 12461
	[SerializeField]
	public tk2dTextMesh.WoobleStyle[] woobleStyles;

	// Token: 0x040030AE RID: 12462
	public int visibleCharacters = int.MaxValue;

	// Token: 0x040030AF RID: 12463
	[SerializeField]
	private bool _formatting;

	// Token: 0x040030B0 RID: 12464
	[SerializeField]
	private int _wordWrapWidth;

	// Token: 0x040030B1 RID: 12465
	[SerializeField]
	private float spacing;

	// Token: 0x040030B2 RID: 12466
	[SerializeField]
	private float lineSpacing;

	// Token: 0x040030B3 RID: 12467
	[SerializeField]
	private tk2dTextMeshData data = new tk2dTextMeshData();

	// Token: 0x040030B4 RID: 12468
	private Vector3[] vertices;

	// Token: 0x040030B5 RID: 12469
	private Vector2[] uvs;

	// Token: 0x040030B6 RID: 12470
	private Vector2[] uv2;

	// Token: 0x040030B7 RID: 12471
	private Color32[] colors;

	// Token: 0x040030B8 RID: 12472
	private Color32[] untintedColors;

	// Token: 0x040030B9 RID: 12473
	private tk2dTextMesh.UpdateFlags updateFlags = tk2dTextMesh.UpdateFlags.UpdateBuffers;

	// Token: 0x040030BA RID: 12474
	private Mesh mesh;

	// Token: 0x040030BB RID: 12475
	private MeshFilter meshFilter;

	// Token: 0x040030BC RID: 12476
	private Renderer _cachedRenderer;

	// Token: 0x040030BD RID: 12477
	protected Vector2[] woobleBuffer;

	// Token: 0x040030BE RID: 12478
	protected bool[] woobleRainbowBuffer;

	// Token: 0x040030BF RID: 12479
	protected float[] woobleTimes;

	// Token: 0x040030C0 RID: 12480
	protected List<int> indices;

	// Token: 0x040030C1 RID: 12481
	private List<GameObject> m_inlineSprites = new List<GameObject>();

	// Token: 0x040030C2 RID: 12482
	private List<int> m_inlineSpriteIndices = new List<int>();

	// Token: 0x040030C3 RID: 12483
	private tk2dFontData m_defaultAssignedFont;

	// Token: 0x02000B96 RID: 2966
	public enum WoobleStyle
	{
		// Token: 0x040030C5 RID: 12485
		SEQUENTIAL,
		// Token: 0x040030C6 RID: 12486
		SIMULTANEOUS,
		// Token: 0x040030C7 RID: 12487
		RANDOM_JITTER,
		// Token: 0x040030C8 RID: 12488
		RANDOM_SEQUENTIAL,
		// Token: 0x040030C9 RID: 12489
		SEQUENTIAL_RAINBOW
	}

	// Token: 0x02000B97 RID: 2967
	internal class WoobleDefinition
	{
		// Token: 0x040030CA RID: 12490
		public int startIndex = -1;

		// Token: 0x040030CB RID: 12491
		public int endIndex = -1;

		// Token: 0x040030CC RID: 12492
		public tk2dTextMesh.WoobleStyle style;
	}

	// Token: 0x02000B98 RID: 2968
	[Flags]
	private enum UpdateFlags
	{
		// Token: 0x040030CE RID: 12494
		UpdateNone = 0,
		// Token: 0x040030CF RID: 12495
		UpdateText = 1,
		// Token: 0x040030D0 RID: 12496
		UpdateColors = 2,
		// Token: 0x040030D1 RID: 12497
		UpdateBuffers = 4
	}
}
