using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

// Token: 0x020003DC RID: 988
[AddComponentMenu("Daikon Forge/User Interface/Label")]
[dfHelp("http://www.daikonforge.com/docs/df-gui/classdf_label.html")]
[dfTooltip("Displays text information, optionally allowing the use of markup to specify colors and embedded sprites")]
[ExecuteInEditMode]
[dfCategory("Basic Controls")]
[Serializable]
public class dfLabel : dfControl, IDFMultiRender, IRendersText
{
	// Token: 0x14000036 RID: 54
	// (add) Token: 0x060013A9 RID: 5033 RVA: 0x0005AE34 File Offset: 0x00059034
	// (remove) Token: 0x060013AA RID: 5034 RVA: 0x0005AE6C File Offset: 0x0005906C
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event PropertyChangedEventHandler<string> TextChanged;

	// Token: 0x17000445 RID: 1093
	// (get) Token: 0x060013AB RID: 5035 RVA: 0x0005AEA4 File Offset: 0x000590A4
	public dfFontBase DefaultAssignedFont
	{
		get
		{
			return this.m_defaultAssignedFont;
		}
	}

	// Token: 0x17000446 RID: 1094
	// (get) Token: 0x060013AC RID: 5036 RVA: 0x0005AEAC File Offset: 0x000590AC
	// (set) Token: 0x060013AD RID: 5037 RVA: 0x0005AEF4 File Offset: 0x000590F4
	public dfAtlas Atlas
	{
		get
		{
			if (this.atlas == null)
			{
				dfGUIManager manager = base.GetManager();
				if (manager != null)
				{
					return this.atlas = manager.DefaultAtlas;
				}
			}
			return this.atlas;
		}
		set
		{
			if (!dfAtlas.Equals(value, this.atlas))
			{
				this.atlas = value;
				this.Invalidate();
			}
		}
	}

	// Token: 0x17000447 RID: 1095
	// (get) Token: 0x060013AE RID: 5038 RVA: 0x0005AF14 File Offset: 0x00059114
	// (set) Token: 0x060013AF RID: 5039 RVA: 0x0005AF58 File Offset: 0x00059158
	public dfFontBase Font
	{
		get
		{
			if (this.font == null)
			{
				dfGUIManager manager = base.GetManager();
				if (manager != null)
				{
					this.font = manager.DefaultFont;
				}
			}
			return this.font;
		}
		set
		{
			if (value != this.font)
			{
				this.unbindTextureRebuildCallback();
				this.font = value;
				this.bindTextureRebuildCallback();
				this.Invalidate();
			}
		}
	}

	// Token: 0x17000448 RID: 1096
	// (get) Token: 0x060013B0 RID: 5040 RVA: 0x0005AF84 File Offset: 0x00059184
	// (set) Token: 0x060013B1 RID: 5041 RVA: 0x0005AF8C File Offset: 0x0005918C
	public string BackgroundSprite
	{
		get
		{
			return this.backgroundSprite;
		}
		set
		{
			if (value != this.backgroundSprite)
			{
				this.backgroundSprite = value;
				this.Invalidate();
			}
		}
	}

	// Token: 0x17000449 RID: 1097
	// (get) Token: 0x060013B2 RID: 5042 RVA: 0x0005AFAC File Offset: 0x000591AC
	// (set) Token: 0x060013B3 RID: 5043 RVA: 0x0005AFB4 File Offset: 0x000591B4
	public Color32 BackgroundColor
	{
		get
		{
			return this.backgroundColor;
		}
		set
		{
			if (!object.Equals(value, this.backgroundColor))
			{
				this.backgroundColor = value;
				this.Invalidate();
			}
		}
	}

	// Token: 0x1700044A RID: 1098
	// (get) Token: 0x060013B4 RID: 5044 RVA: 0x0005AFE0 File Offset: 0x000591E0
	// (set) Token: 0x060013B5 RID: 5045 RVA: 0x0005AFE8 File Offset: 0x000591E8
	public float TextScale
	{
		get
		{
			return this.textScale;
		}
		set
		{
			value = Mathf.Max(0.1f, value);
			if (!Mathf.Approximately(this.textScale, value))
			{
				dfFontManager.Invalidate(this.Font);
				this.textScale = value;
				this.Invalidate();
			}
		}
	}

	// Token: 0x1700044B RID: 1099
	// (get) Token: 0x060013B6 RID: 5046 RVA: 0x0005B020 File Offset: 0x00059220
	// (set) Token: 0x060013B7 RID: 5047 RVA: 0x0005B028 File Offset: 0x00059228
	public dfTextScaleMode TextScaleMode
	{
		get
		{
			return this.textScaleMode;
		}
		set
		{
			this.textScaleMode = value;
			this.Invalidate();
		}
	}

	// Token: 0x1700044C RID: 1100
	// (get) Token: 0x060013B8 RID: 5048 RVA: 0x0005B038 File Offset: 0x00059238
	// (set) Token: 0x060013B9 RID: 5049 RVA: 0x0005B040 File Offset: 0x00059240
	public int CharacterSpacing
	{
		get
		{
			return this.charSpacing;
		}
		set
		{
			value = Mathf.Max(0, value);
			if (value != this.charSpacing)
			{
				this.charSpacing = value;
				this.Invalidate();
			}
		}
	}

	// Token: 0x1700044D RID: 1101
	// (get) Token: 0x060013BA RID: 5050 RVA: 0x0005B064 File Offset: 0x00059264
	// (set) Token: 0x060013BB RID: 5051 RVA: 0x0005B06C File Offset: 0x0005926C
	public bool ColorizeSymbols
	{
		get
		{
			return this.colorizeSymbols;
		}
		set
		{
			if (value != this.colorizeSymbols)
			{
				this.colorizeSymbols = value;
				this.Invalidate();
			}
		}
	}

	// Token: 0x1700044E RID: 1102
	// (get) Token: 0x060013BC RID: 5052 RVA: 0x0005B088 File Offset: 0x00059288
	// (set) Token: 0x060013BD RID: 5053 RVA: 0x0005B090 File Offset: 0x00059290
	public bool ProcessMarkup
	{
		get
		{
			return this.processMarkup;
		}
		set
		{
			if (value != this.processMarkup)
			{
				this.processMarkup = value;
				this.Invalidate();
			}
		}
	}

	// Token: 0x1700044F RID: 1103
	// (get) Token: 0x060013BE RID: 5054 RVA: 0x0005B0AC File Offset: 0x000592AC
	// (set) Token: 0x060013BF RID: 5055 RVA: 0x0005B0B4 File Offset: 0x000592B4
	public bool ShowGradient
	{
		get
		{
			return this.enableGradient;
		}
		set
		{
			if (value != this.enableGradient)
			{
				this.enableGradient = value;
				this.Invalidate();
			}
		}
	}

	// Token: 0x17000450 RID: 1104
	// (get) Token: 0x060013C0 RID: 5056 RVA: 0x0005B0D0 File Offset: 0x000592D0
	// (set) Token: 0x060013C1 RID: 5057 RVA: 0x0005B0D8 File Offset: 0x000592D8
	public Color32 BottomColor
	{
		get
		{
			return this.bottomColor;
		}
		set
		{
			if (!this.bottomColor.EqualsNonAlloc(value))
			{
				this.bottomColor = value;
				this.OnColorChanged();
			}
		}
	}

	// Token: 0x060013C2 RID: 5058 RVA: 0x0005B0F8 File Offset: 0x000592F8
	public void ModifyLocalizedText(string text)
	{
		dfFontManager.Invalidate(this.Font);
		this.text = text;
		this.OnTextChanged();
	}

	// Token: 0x17000451 RID: 1105
	// (get) Token: 0x060013C3 RID: 5059 RVA: 0x0005B114 File Offset: 0x00059314
	// (set) Token: 0x060013C4 RID: 5060 RVA: 0x0005B11C File Offset: 0x0005931C
	public string Text
	{
		get
		{
			return this.text;
		}
		set
		{
			if (value == null)
			{
				value = string.Empty;
			}
			else
			{
				value = value.Replace("\\t", "\t").Replace("\\n", "\n");
			}
			if (!string.Equals(value, this.text))
			{
				dfFontManager.Invalidate(this.Font);
				this.localizationKey = value;
				this.text = base.getLocalizedValue(value);
				this.OnTextChanged();
			}
		}
	}

	// Token: 0x17000452 RID: 1106
	// (get) Token: 0x060013C5 RID: 5061 RVA: 0x0005B194 File Offset: 0x00059394
	// (set) Token: 0x060013C6 RID: 5062 RVA: 0x0005B1BC File Offset: 0x000593BC
	public bool AutoSize
	{
		get
		{
			if (this.autoSize && this.autoHeight)
			{
				this.autoHeight = false;
			}
			return this.autoSize;
		}
		set
		{
			if (value != this.autoSize)
			{
				if (value)
				{
					this.autoHeight = false;
				}
				this.autoSize = value;
				this.Invalidate();
			}
		}
	}

	// Token: 0x17000453 RID: 1107
	// (get) Token: 0x060013C7 RID: 5063 RVA: 0x0005B1E4 File Offset: 0x000593E4
	// (set) Token: 0x060013C8 RID: 5064 RVA: 0x0005B200 File Offset: 0x00059400
	public bool AutoHeight
	{
		get
		{
			return this.autoHeight && !this.autoSize;
		}
		set
		{
			if (value != this.autoHeight)
			{
				if (value)
				{
					this.autoSize = false;
				}
				this.autoHeight = value;
				this.Invalidate();
			}
		}
	}

	// Token: 0x17000454 RID: 1108
	// (get) Token: 0x060013C9 RID: 5065 RVA: 0x0005B228 File Offset: 0x00059428
	// (set) Token: 0x060013CA RID: 5066 RVA: 0x0005B230 File Offset: 0x00059430
	public bool WordWrap
	{
		get
		{
			return this.wordWrap;
		}
		set
		{
			if (value != this.wordWrap)
			{
				this.wordWrap = value;
				this.Invalidate();
			}
		}
	}

	// Token: 0x17000455 RID: 1109
	// (get) Token: 0x060013CB RID: 5067 RVA: 0x0005B24C File Offset: 0x0005944C
	// (set) Token: 0x060013CC RID: 5068 RVA: 0x0005B254 File Offset: 0x00059454
	public TextAlignment TextAlignment
	{
		get
		{
			return this.align;
		}
		set
		{
			if (value != this.align)
			{
				this.align = value;
				this.Invalidate();
			}
		}
	}

	// Token: 0x17000456 RID: 1110
	// (get) Token: 0x060013CD RID: 5069 RVA: 0x0005B270 File Offset: 0x00059470
	// (set) Token: 0x060013CE RID: 5070 RVA: 0x0005B278 File Offset: 0x00059478
	public dfVerticalAlignment VerticalAlignment
	{
		get
		{
			return this.vertAlign;
		}
		set
		{
			if (value != this.vertAlign)
			{
				this.vertAlign = value;
				this.Invalidate();
			}
		}
	}

	// Token: 0x17000457 RID: 1111
	// (get) Token: 0x060013CF RID: 5071 RVA: 0x0005B294 File Offset: 0x00059494
	// (set) Token: 0x060013D0 RID: 5072 RVA: 0x0005B29C File Offset: 0x0005949C
	public bool Outline
	{
		get
		{
			return this.outline;
		}
		set
		{
			if (value != this.outline)
			{
				this.outline = value;
				this.Invalidate();
			}
		}
	}

	// Token: 0x17000458 RID: 1112
	// (get) Token: 0x060013D1 RID: 5073 RVA: 0x0005B2B8 File Offset: 0x000594B8
	// (set) Token: 0x060013D2 RID: 5074 RVA: 0x0005B2C0 File Offset: 0x000594C0
	public int OutlineSize
	{
		get
		{
			return this.outlineWidth;
		}
		set
		{
			value = Mathf.Max(0, value);
			if (value != this.outlineWidth)
			{
				this.outlineWidth = value;
				this.Invalidate();
			}
		}
	}

	// Token: 0x17000459 RID: 1113
	// (get) Token: 0x060013D3 RID: 5075 RVA: 0x0005B2E4 File Offset: 0x000594E4
	// (set) Token: 0x060013D4 RID: 5076 RVA: 0x0005B2EC File Offset: 0x000594EC
	public Color32 OutlineColor
	{
		get
		{
			return this.outlineColor;
		}
		set
		{
			if (!value.Equals(this.outlineColor))
			{
				this.outlineColor = value;
				this.Invalidate();
			}
		}
	}

	// Token: 0x1700045A RID: 1114
	// (get) Token: 0x060013D5 RID: 5077 RVA: 0x0005B318 File Offset: 0x00059518
	// (set) Token: 0x060013D6 RID: 5078 RVA: 0x0005B320 File Offset: 0x00059520
	public bool Shadow
	{
		get
		{
			return this.shadow;
		}
		set
		{
			if (value != this.shadow)
			{
				this.shadow = value;
				this.Invalidate();
			}
		}
	}

	// Token: 0x1700045B RID: 1115
	// (get) Token: 0x060013D7 RID: 5079 RVA: 0x0005B33C File Offset: 0x0005953C
	// (set) Token: 0x060013D8 RID: 5080 RVA: 0x0005B344 File Offset: 0x00059544
	public Color32 ShadowColor
	{
		get
		{
			return this.shadowColor;
		}
		set
		{
			if (!value.Equals(this.shadowColor))
			{
				this.shadowColor = value;
				this.Invalidate();
			}
		}
	}

	// Token: 0x1700045C RID: 1116
	// (get) Token: 0x060013D9 RID: 5081 RVA: 0x0005B370 File Offset: 0x00059570
	// (set) Token: 0x060013DA RID: 5082 RVA: 0x0005B378 File Offset: 0x00059578
	public Vector2 ShadowOffset
	{
		get
		{
			return this.shadowOffset;
		}
		set
		{
			if (value != this.shadowOffset)
			{
				this.shadowOffset = value;
				this.Invalidate();
			}
		}
	}

	// Token: 0x1700045D RID: 1117
	// (get) Token: 0x060013DB RID: 5083 RVA: 0x0005B398 File Offset: 0x00059598
	// (set) Token: 0x060013DC RID: 5084 RVA: 0x0005B3B8 File Offset: 0x000595B8
	public RectOffset Padding
	{
		get
		{
			if (this.padding == null)
			{
				this.padding = new RectOffset();
			}
			return this.padding;
		}
		set
		{
			if (!object.Equals(value, this.padding))
			{
				this.padding = value;
				this.Invalidate();
			}
		}
	}

	// Token: 0x1700045E RID: 1118
	// (get) Token: 0x060013DD RID: 5085 RVA: 0x0005B3D8 File Offset: 0x000595D8
	// (set) Token: 0x060013DE RID: 5086 RVA: 0x0005B3E0 File Offset: 0x000595E0
	public int TabSize
	{
		get
		{
			return this.tabSize;
		}
		set
		{
			value = Mathf.Max(0, value);
			if (value != this.tabSize)
			{
				this.tabSize = value;
				this.Invalidate();
			}
		}
	}

	// Token: 0x1700045F RID: 1119
	// (get) Token: 0x060013DF RID: 5087 RVA: 0x0005B404 File Offset: 0x00059604
	public List<int> TabStops
	{
		get
		{
			return this.tabStops;
		}
	}

	// Token: 0x060013E0 RID: 5088 RVA: 0x0005B40C File Offset: 0x0005960C
	protected void CheckFontsForLanguage()
	{
		if (!Application.isPlaying)
		{
			return;
		}
		StringTableManager.GungeonSupportedLanguages gungeonSupportedLanguages = GameManager.Options.CurrentLanguage;
		if (this.PreventFontChanges)
		{
			gungeonSupportedLanguages = StringTableManager.GungeonSupportedLanguages.ENGLISH;
		}
		if (this.m_cachedLanguage == gungeonSupportedLanguages)
		{
			return;
		}
		if (this.m_defaultAssignedFont == null)
		{
			this.m_defaultAssignedFontTextScale = this.textScale;
			this.m_defaultAssignedFont = this.font;
		}
		if (this.m_defaultAssignedFont == null)
		{
			this.font = base.GUIManager.DefaultFont;
			this.m_defaultAssignedFont = this.font;
		}
		if (this.m_cachedPadding == null)
		{
			this.m_cachedPadding = this.padding;
		}
		float num = this.m_defaultAssignedFontTextScale;
		if (Pixelator.Instance)
		{
			dfLabel.m_cachedScaleTileScale = Pixelator.Instance.ScaleTileScale;
		}
		dfFontBase dfFontBase;
		if (gungeonSupportedLanguages == StringTableManager.GungeonSupportedLanguages.JAPANESE && !this.MaintainJapaneseFont)
		{
			dfFontBase = (ResourceCache.Acquire("Alternate Fonts/JackeyFont12_DF") as GameObject).GetComponent<dfFont>();
			float num2 = Mathf.Max(1f, (float)this.m_defaultAssignedFont.FontSize / 14f);
			num = (float)Mathf.CeilToInt(this.m_defaultAssignedFontTextScale * num2);
			if (this.m_defaultAssignedFont.LineHeight < 16)
			{
				int num3 = ((base.GetManager().FixedHeight <= 1000) ? 3 : 4);
				RectOffset rectOffset = new RectOffset(this.m_cachedPadding.left, this.m_cachedPadding.right, this.m_cachedPadding.top + num3 * 2, this.m_cachedPadding.bottom);
				this.padding = rectOffset;
			}
			else
			{
				this.padding = this.m_cachedPadding;
			}
		}
		else if (gungeonSupportedLanguages == StringTableManager.GungeonSupportedLanguages.CHINESE && !this.MaintainJapaneseFont)
		{
			dfFontBase = (ResourceCache.Acquire("Alternate Fonts/SimSun12_DF") as GameObject).GetComponent<dfFont>();
			float num4 = Mathf.Max(1f, (float)this.m_defaultAssignedFont.FontSize / 14f);
			num = (float)Mathf.CeilToInt(this.m_defaultAssignedFontTextScale * num4);
			if (this.m_defaultAssignedFont.LineHeight < 16)
			{
				int num5 = ((base.GetManager().FixedHeight <= 1000) ? 3 : 4);
				RectOffset rectOffset2 = new RectOffset(this.m_cachedPadding.left, this.m_cachedPadding.right, this.m_cachedPadding.top + num5 * 2, this.m_cachedPadding.bottom);
				this.padding = rectOffset2;
			}
			else
			{
				this.padding = this.m_cachedPadding;
			}
		}
		else if (gungeonSupportedLanguages == StringTableManager.GungeonSupportedLanguages.KOREAN && !this.MaintainKoreanFont)
		{
			dfFontBase = (ResourceCache.Acquire("Alternate Fonts/NanumGothic16_DF") as GameObject).GetComponent<dfFont>();
			float num6 = (float)this.m_defaultAssignedFont.FontSize / (float)dfFontBase.FontSize;
			float num7 = Mathf.Max(3f, dfLabel.m_cachedScaleTileScale);
			if (num6 < 1f)
			{
				num6 = (num7 - 1f) / num7;
			}
			num = ((num6 <= 1f) ? (this.m_defaultAssignedFontTextScale * num6) : ((float)Mathf.CeilToInt(this.m_defaultAssignedFontTextScale * num6)));
			if (this.m_defaultAssignedFont.LineHeight < 16)
			{
				int num8 = ((base.GetManager().FixedHeight <= 1000) ? 3 : 4);
				RectOffset rectOffset3 = new RectOffset(this.m_cachedPadding.left, this.m_cachedPadding.right, this.m_cachedPadding.top + num8 * 2, this.m_cachedPadding.bottom);
				this.padding = rectOffset3;
			}
			else
			{
				this.padding = this.m_cachedPadding;
			}
		}
		else if (gungeonSupportedLanguages == StringTableManager.GungeonSupportedLanguages.RUSSIAN && !this.MaintainRussianFont)
		{
			dfFontBase = (ResourceCache.Acquire("Alternate Fonts/PixelaCYR_15_DF") as GameObject).GetComponent<dfFont>();
			float num9 = (float)this.m_defaultAssignedFont.FontSize / (float)dfFontBase.FontSize;
			if (num9 < 1f)
			{
				num9 = 1f;
			}
			num = ((num9 <= 1f) ? (this.m_defaultAssignedFontTextScale * num9) : ((float)Mathf.CeilToInt(this.m_defaultAssignedFontTextScale * num9)));
			this.padding = this.m_cachedPadding;
		}
		else if (gungeonSupportedLanguages == StringTableManager.GungeonSupportedLanguages.POLISH && !this.MaintainRussianFont && this.m_defaultAssignedFont != null && base.GUIManager.DefaultFont != null && this.m_defaultAssignedFont.name.StartsWith("04b03"))
		{
			dfFontBase = base.GUIManager.DefaultFont;
			float num10 = (float)this.m_defaultAssignedFont.FontSize / (float)dfFontBase.FontSize;
			if (num10 < 1f)
			{
				num10 = 1f;
			}
			num = ((num10 <= 1f) ? (this.m_defaultAssignedFontTextScale * num10) : ((float)Mathf.CeilToInt(this.m_defaultAssignedFontTextScale * num10)));
			this.padding = this.m_cachedPadding;
		}
		else
		{
			dfFontBase = this.m_defaultAssignedFont;
			this.padding = this.m_cachedPadding;
		}
		if (dfFontBase != null && this.Font != dfFontBase)
		{
			this.Font = dfFontBase;
			if (dfFontBase is dfFont)
			{
				this.Atlas = (dfFontBase as dfFont).Atlas;
			}
			this.TextScale = num;
		}
		this.m_cachedLanguage = gungeonSupportedLanguages;
	}

	// Token: 0x060013E1 RID: 5089 RVA: 0x0005B95C File Offset: 0x00059B5C
	protected internal override void OnLocalize()
	{
		base.OnLocalize();
		this.CheckFontsForLanguage();
		if (this.text.StartsWith("#"))
		{
			this.localizationKey = this.text;
		}
		if (!string.IsNullOrEmpty(this.localizationKey) && this.localizationKey.StartsWith("#"))
		{
			this.text = base.getLocalizedValue(this.localizationKey);
		}
		else
		{
			this.Text = base.getLocalizedValue(this.text);
		}
		if (this.text.StartsWith("#") && this.text.Contains("ENCNAME"))
		{
			this.ModifyLocalizedText(StringTableManager.GetItemsString(this.localizationKey, -1));
		}
		base.PerformLayout();
	}

	// Token: 0x060013E2 RID: 5090 RVA: 0x0005BA28 File Offset: 0x00059C28
	protected internal void OnTextChanged()
	{
		this.CheckFontsForLanguage();
		this.Invalidate();
		base.Signal("OnTextChanged", this, this.text);
		if (this.TextChanged != null)
		{
			this.TextChanged(this, this.text);
		}
	}

	// Token: 0x060013E3 RID: 5091 RVA: 0x0005BA68 File Offset: 0x00059C68
	public override void Start()
	{
		base.Start();
	}

	// Token: 0x060013E4 RID: 5092 RVA: 0x0005BA70 File Offset: 0x00059C70
	public override void OnEnable()
	{
		base.OnEnable();
		bool flag = this.Font != null && this.Font.IsValid;
		if (Application.isPlaying && !flag)
		{
			this.Font = base.GetManager().DefaultFont;
		}
		this.bindTextureRebuildCallback();
		if (this.size.sqrMagnitude <= 1E-45f)
		{
			base.Size = new Vector2(150f, 25f);
		}
	}

	// Token: 0x060013E5 RID: 5093 RVA: 0x0005BAF4 File Offset: 0x00059CF4
	public override void OnDisable()
	{
		base.OnDisable();
		this.unbindTextureRebuildCallback();
	}

	// Token: 0x060013E6 RID: 5094 RVA: 0x0005BB04 File Offset: 0x00059D04
	public override void Awake()
	{
		this.localizationKey = this.Text;
		base.Awake();
		this.startSize = ((!Application.isPlaying) ? Vector2.zero : base.Size);
	}

	// Token: 0x060013E7 RID: 5095 RVA: 0x0005BB38 File Offset: 0x00059D38
	public override Vector2 CalculateMinimumSize()
	{
		if (this.Font != null)
		{
			float num = (float)this.Font.FontSize * this.TextScale * 0.75f;
			return Vector2.Max(base.CalculateMinimumSize(), new Vector2(num, num));
		}
		return base.CalculateMinimumSize();
	}

	// Token: 0x060013E8 RID: 5096 RVA: 0x0005BB8C File Offset: 0x00059D8C
	public float GetAutosizeWidth()
	{
		float num;
		using (dfFontRendererBase dfFontRendererBase = this.obtainRenderer())
		{
			num = dfFontRendererBase.MeasureString(this.text).RoundToInt().x + (float)this.padding.horizontal;
		}
		return num;
	}

	// Token: 0x060013E9 RID: 5097 RVA: 0x0005BBEC File Offset: 0x00059DEC
	[HideInInspector]
	public override void Invalidate()
	{
		base.Invalidate();
		if (this.m_cachedLanguage != GameManager.Options.CurrentLanguage)
		{
			this.CheckFontsForLanguage();
		}
		if (this.Font == null || !this.Font.IsValid || base.GetManager() == null)
		{
			return;
		}
		bool flag = this.size.sqrMagnitude <= float.Epsilon;
		if (!this.AutoSize && !this.autoHeight && !flag)
		{
			return;
		}
		if (string.IsNullOrEmpty(this.Text))
		{
			Vector2 size = this.size;
			Vector2 vector = size;
			if (flag)
			{
				vector = new Vector2(150f, 24f);
			}
			if (this.AutoSize || this.AutoHeight)
			{
				vector.y = (float)Mathf.CeilToInt((float)this.Font.LineHeight * this.TextScale);
			}
			if (size != vector)
			{
				base.SuspendLayout();
				base.Size = vector;
				base.ResumeLayout();
			}
			return;
		}
		Vector2 size2 = this.size;
		using (dfFontRendererBase dfFontRendererBase = this.obtainRenderer())
		{
			Vector2 vector2 = dfFontRendererBase.MeasureString(this.text).RoundToInt();
			if (this.AutoSize || flag)
			{
				this.size = vector2 + new Vector2((float)this.padding.horizontal, (float)this.padding.vertical);
			}
			else if (this.AutoHeight)
			{
				this.size = new Vector2(this.size.x, vector2.y + (float)this.padding.vertical);
			}
		}
		if ((this.size - size2).sqrMagnitude >= 1f)
		{
			base.raiseSizeChangedEvent();
		}
	}

	// Token: 0x060013EA RID: 5098 RVA: 0x0005BDE8 File Offset: 0x00059FE8
	private dfFontRendererBase obtainRenderer()
	{
		bool flag = base.Size.sqrMagnitude <= float.Epsilon;
		Vector2 vector = base.Size - new Vector2((float)this.padding.horizontal, (float)this.padding.vertical);
		Vector2 vector2 = ((!this.AutoSize && !flag) ? vector : this.getAutoSizeDefault());
		if (this.autoHeight)
		{
			vector2 = new Vector2(vector.x, 2.1474836E+09f);
		}
		float num = base.PixelsToUnits();
		Vector3 vector3 = (this.pivot.TransformToUpperLeft(base.Size) + new Vector3((float)this.padding.left, (float)(-(float)this.padding.top))) * num;
		float num2 = this.TextScale * this.getTextScaleMultiplier();
		dfFontRendererBase dfFontRendererBase = this.Font.ObtainRenderer();
		dfFontRendererBase.WordWrap = this.WordWrap;
		dfFontRendererBase.MaxSize = vector2;
		dfFontRendererBase.PixelRatio = num;
		dfFontRendererBase.TextScale = num2;
		dfFontRendererBase.CharacterSpacing = this.CharacterSpacing;
		dfFontRendererBase.VectorOffset = vector3.Quantize(num);
		dfFontRendererBase.PerCharacterAccumulatedOffset = this.PerCharacterOffset * num;
		dfFontRendererBase.MultiLine = true;
		dfFontRendererBase.TabSize = this.TabSize;
		dfFontRendererBase.TabStops = this.TabStops;
		dfFontRendererBase.TextAlign = ((!this.AutoSize) ? this.TextAlignment : TextAlignment.Left);
		dfFontRendererBase.ColorizeSymbols = this.ColorizeSymbols;
		dfFontRendererBase.ProcessMarkup = this.ProcessMarkup;
		dfFontRendererBase.DefaultColor = ((!base.IsEnabled) ? base.DisabledColor : base.Color);
		dfFontRendererBase.BottomColor = ((!this.enableGradient) ? null : new Color32?(this.BottomColor));
		dfFontRendererBase.OverrideMarkupColors = !base.IsEnabled;
		dfFontRendererBase.Opacity = base.CalculateOpacity();
		dfFontRendererBase.Outline = this.Outline;
		dfFontRendererBase.OutlineSize = this.OutlineSize;
		dfFontRendererBase.OutlineColor = this.OutlineColor;
		dfFontRendererBase.Shadow = this.Shadow;
		dfFontRendererBase.ShadowColor = this.ShadowColor;
		dfFontRendererBase.ShadowOffset = this.ShadowOffset;
		dfDynamicFont.DynamicFontRenderer dynamicFontRenderer = dfFontRendererBase as dfDynamicFont.DynamicFontRenderer;
		if (dynamicFontRenderer != null)
		{
			dynamicFontRenderer.SpriteAtlas = this.Atlas;
			dynamicFontRenderer.SpriteBuffer = this.renderData;
		}
		if (this.vertAlign != dfVerticalAlignment.Top)
		{
			dfFontRendererBase.VectorOffset = this.getVertAlignOffset(dfFontRendererBase);
		}
		return dfFontRendererBase;
	}

	// Token: 0x060013EB RID: 5099 RVA: 0x0005C088 File Offset: 0x0005A288
	private float getTextScaleMultiplier()
	{
		if (this.textScaleMode == dfTextScaleMode.None || !Application.isPlaying)
		{
			return 1f;
		}
		if (this.textScaleMode == dfTextScaleMode.ScreenResolution)
		{
			return base.GetManager().GetScreenSize().y / (float)base.GetManager().FixedHeight;
		}
		if (this.AutoSize)
		{
			return 1f;
		}
		return base.Size.y / this.startSize.y;
	}

	// Token: 0x060013EC RID: 5100 RVA: 0x0005C10C File Offset: 0x0005A30C
	private Vector2 getAutoSizeDefault()
	{
		float num = ((this.maxSize.x <= float.Epsilon) ? 2.1474836E+09f : this.maxSize.x);
		float num2 = ((this.maxSize.y <= float.Epsilon) ? 2.1474836E+09f : this.maxSize.y);
		Vector2 vector = new Vector2(num, num2);
		return vector;
	}

	// Token: 0x060013ED RID: 5101 RVA: 0x0005C17C File Offset: 0x0005A37C
	private Vector3 getVertAlignOffset(dfFontRendererBase textRenderer)
	{
		float num = base.PixelsToUnits();
		Vector2 vector = textRenderer.MeasureString(this.text) * num;
		Vector3 vectorOffset = textRenderer.VectorOffset;
		float num2 = (base.Height - (float)this.padding.vertical) * num;
		if (vector.y >= num2)
		{
			return vectorOffset;
		}
		dfVerticalAlignment dfVerticalAlignment = this.vertAlign;
		if (dfVerticalAlignment != dfVerticalAlignment.Middle)
		{
			if (dfVerticalAlignment == dfVerticalAlignment.Bottom)
			{
				vectorOffset.y -= num2 - vector.y;
			}
		}
		else
		{
			vectorOffset.y -= (num2 - vector.y) * 0.5f;
		}
		return vectorOffset;
	}

	// Token: 0x060013EE RID: 5102 RVA: 0x0005C22C File Offset: 0x0005A42C
	protected internal virtual void renderBackground()
	{
		if (this.Atlas == null)
		{
			return;
		}
		dfAtlas.ItemInfo itemInfo = this.Atlas[this.backgroundSprite];
		if (itemInfo == null)
		{
			return;
		}
		Color32 color = base.ApplyOpacity(this.BackgroundColor);
		dfSprite.RenderOptions renderOptions = new dfSprite.RenderOptions
		{
			atlas = this.atlas,
			color = color,
			fillAmount = 1f,
			flip = dfSpriteFlip.None,
			offset = this.pivot.TransformToUpperLeft(base.Size),
			pixelsToUnits = base.PixelsToUnits(),
			size = base.Size,
			spriteInfo = itemInfo
		};
		if (itemInfo.border.horizontal == 0 && itemInfo.border.vertical == 0)
		{
			dfSprite.renderSprite(this.renderData, renderOptions);
		}
		else
		{
			dfSlicedSprite.renderSprite(this.renderData, renderOptions);
		}
	}

	// Token: 0x060013EF RID: 5103 RVA: 0x0005C324 File Offset: 0x0005A524
	public dfList<dfRenderData> RenderMultiple()
	{
		dfList<dfRenderData> dfList;
		try
		{
			if (!this.isControlInvalidated && (this.textRenderData != null || this.renderData != null))
			{
				Matrix4x4 localToWorldMatrix = base.transform.localToWorldMatrix;
				for (int i = 0; i < this.renderDataBuffers.Count; i++)
				{
					this.renderDataBuffers[i].Transform = localToWorldMatrix;
				}
				dfList = this.renderDataBuffers;
			}
			else if (this.Atlas == null || this.Font == null || !this.isVisible)
			{
				dfList = null;
			}
			else
			{
				if (this.renderData == null)
				{
					this.renderData = dfRenderData.Obtain();
					this.textRenderData = dfRenderData.Obtain();
					this.isControlInvalidated = true;
				}
				this.resetRenderBuffers();
				this.renderBackground();
				if (string.IsNullOrEmpty(this.Text))
				{
					if (this.AutoSize || this.AutoHeight)
					{
						base.Height = (float)Mathf.CeilToInt((float)this.Font.LineHeight * this.TextScale);
					}
					dfList = this.renderDataBuffers;
				}
				else
				{
					bool flag = this.size.sqrMagnitude <= float.Epsilon;
					using (dfFontRendererBase dfFontRendererBase = this.obtainRenderer())
					{
						this.textRenderData.Glitchy = this.Glitchy;
						dfFontRendererBase.Render(this.text, this.textRenderData);
						if (this.AutoSize || flag)
						{
							base.Size = (dfFontRendererBase.RenderedSize + new Vector2((float)this.padding.horizontal, (float)this.padding.vertical)).CeilToInt();
						}
						else if (this.AutoHeight)
						{
							base.Size = new Vector2(this.size.x, dfFontRendererBase.RenderedSize.y + (float)this.padding.vertical).CeilToInt();
						}
					}
					base.updateCollider();
					dfList = this.renderDataBuffers;
				}
			}
		}
		finally
		{
			this.isControlInvalidated = false;
		}
		return dfList;
	}

	// Token: 0x060013F0 RID: 5104 RVA: 0x0005C584 File Offset: 0x0005A784
	private void resetRenderBuffers()
	{
		this.renderDataBuffers.Clear();
		Matrix4x4 localToWorldMatrix = base.transform.localToWorldMatrix;
		if (this.renderData != null)
		{
			this.renderData.Clear();
			this.renderData.Material = this.Atlas.Material;
			this.renderData.Transform = localToWorldMatrix;
			this.renderDataBuffers.Add(this.renderData);
		}
		if (this.textRenderData != null)
		{
			this.textRenderData.Clear();
			this.textRenderData.Material = this.Atlas.Material;
			this.textRenderData.Transform = localToWorldMatrix;
			this.renderDataBuffers.Add(this.textRenderData);
		}
	}

	// Token: 0x060013F1 RID: 5105 RVA: 0x0005C63C File Offset: 0x0005A83C
	private void bindTextureRebuildCallback()
	{
		if (this.isFontCallbackAssigned || this.Font == null)
		{
			return;
		}
		if (this.Font is dfDynamicFont)
		{
			UnityEngine.Font.textureRebuilt += this.onFontTextureRebuilt;
			this.isFontCallbackAssigned = true;
		}
	}

	// Token: 0x060013F2 RID: 5106 RVA: 0x0005C690 File Offset: 0x0005A890
	private void unbindTextureRebuildCallback()
	{
		if (!this.isFontCallbackAssigned || this.Font == null)
		{
			return;
		}
		if (this.Font is dfDynamicFont)
		{
			UnityEngine.Font.textureRebuilt -= this.onFontTextureRebuilt;
		}
		this.isFontCallbackAssigned = false;
	}

	// Token: 0x060013F3 RID: 5107 RVA: 0x0005C6E4 File Offset: 0x0005A8E4
	private void requestCharacterInfo()
	{
		dfDynamicFont dfDynamicFont = this.Font as dfDynamicFont;
		if (dfDynamicFont == null)
		{
			return;
		}
		if (!dfFontManager.IsDirty(this.Font))
		{
			return;
		}
		if (string.IsNullOrEmpty(this.text))
		{
			return;
		}
		float num = this.TextScale * this.getTextScaleMultiplier();
		int num2 = Mathf.CeilToInt((float)this.font.FontSize * num);
		dfDynamicFont.AddCharacterRequest(this.text, num2, FontStyle.Normal);
	}

	// Token: 0x060013F4 RID: 5108 RVA: 0x0005C75C File Offset: 0x0005A95C
	private void onFontTextureRebuilt(Font font)
	{
		if (this.Font is dfDynamicFont && font == (this.Font as dfDynamicFont).BaseFont)
		{
			this.requestCharacterInfo();
			this.Invalidate();
		}
	}

	// Token: 0x060013F5 RID: 5109 RVA: 0x0005C798 File Offset: 0x0005A998
	public void UpdateFontInfo()
	{
		this.requestCharacterInfo();
	}

	// Token: 0x040010DC RID: 4316
	public Vector3 PerCharacterOffset;

	// Token: 0x040010DD RID: 4317
	[NonSerialized]
	protected dfFontBase m_defaultAssignedFont;

	// Token: 0x040010DE RID: 4318
	protected float m_defaultAssignedFontTextScale;

	// Token: 0x040010DF RID: 4319
	[SerializeField]
	public bool PreventFontChanges;

	// Token: 0x040010E0 RID: 4320
	[SerializeField]
	protected dfAtlas atlas;

	// Token: 0x040010E1 RID: 4321
	[SerializeField]
	protected dfFontBase font;

	// Token: 0x040010E2 RID: 4322
	[SerializeField]
	protected string backgroundSprite;

	// Token: 0x040010E3 RID: 4323
	[SerializeField]
	protected Color32 backgroundColor = UnityEngine.Color.white;

	// Token: 0x040010E4 RID: 4324
	[SerializeField]
	protected bool autoSize;

	// Token: 0x040010E5 RID: 4325
	[SerializeField]
	protected bool autoHeight;

	// Token: 0x040010E6 RID: 4326
	[SerializeField]
	protected bool wordWrap;

	// Token: 0x040010E7 RID: 4327
	[SerializeField]
	protected string text = "Label";

	// Token: 0x040010E8 RID: 4328
	[SerializeField]
	protected Color32 bottomColor = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);

	// Token: 0x040010E9 RID: 4329
	[SerializeField]
	protected TextAlignment align;

	// Token: 0x040010EA RID: 4330
	[SerializeField]
	protected dfVerticalAlignment vertAlign;

	// Token: 0x040010EB RID: 4331
	[SerializeField]
	protected float textScale = 1f;

	// Token: 0x040010EC RID: 4332
	[SerializeField]
	protected dfTextScaleMode textScaleMode;

	// Token: 0x040010ED RID: 4333
	[SerializeField]
	protected int charSpacing;

	// Token: 0x040010EE RID: 4334
	[SerializeField]
	protected bool colorizeSymbols;

	// Token: 0x040010EF RID: 4335
	[SerializeField]
	protected bool processMarkup;

	// Token: 0x040010F0 RID: 4336
	[SerializeField]
	protected bool outline;

	// Token: 0x040010F1 RID: 4337
	[SerializeField]
	protected int outlineWidth = 1;

	// Token: 0x040010F2 RID: 4338
	[SerializeField]
	protected bool enableGradient;

	// Token: 0x040010F3 RID: 4339
	[SerializeField]
	protected Color32 outlineColor = UnityEngine.Color.black;

	// Token: 0x040010F4 RID: 4340
	[SerializeField]
	protected bool shadow;

	// Token: 0x040010F5 RID: 4341
	[SerializeField]
	protected Color32 shadowColor = UnityEngine.Color.black;

	// Token: 0x040010F6 RID: 4342
	[SerializeField]
	protected Vector2 shadowOffset = new Vector2(1f, -1f);

	// Token: 0x040010F7 RID: 4343
	[SerializeField]
	protected RectOffset padding = new RectOffset();

	// Token: 0x040010F8 RID: 4344
	[SerializeField]
	protected int tabSize = 48;

	// Token: 0x040010F9 RID: 4345
	[SerializeField]
	protected List<int> tabStops = new List<int>();

	// Token: 0x040010FA RID: 4346
	private Vector2 startSize = Vector2.zero;

	// Token: 0x040010FB RID: 4347
	private bool isFontCallbackAssigned;

	// Token: 0x040010FC RID: 4348
	private StringTableManager.GungeonSupportedLanguages m_cachedLanguage;

	// Token: 0x040010FD RID: 4349
	private RectOffset m_cachedPadding;

	// Token: 0x040010FE RID: 4350
	private static float m_cachedScaleTileScale = 3f;

	// Token: 0x040010FF RID: 4351
	public bool MaintainJapaneseFont;

	// Token: 0x04001100 RID: 4352
	public bool MaintainKoreanFont;

	// Token: 0x04001101 RID: 4353
	public bool MaintainRussianFont;

	// Token: 0x04001102 RID: 4354
	private dfRenderData textRenderData;

	// Token: 0x04001103 RID: 4355
	private dfList<dfRenderData> renderDataBuffers = dfList<dfRenderData>.Obtain();

	// Token: 0x04001104 RID: 4356
	[NonSerialized]
	public bool Glitchy;
}
