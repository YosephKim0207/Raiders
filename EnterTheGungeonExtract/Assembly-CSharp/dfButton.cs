using System;
using System.Diagnostics;
using UnityEngine;

// Token: 0x0200038E RID: 910
[AddComponentMenu("Daikon Forge/User Interface/Button")]
[ExecuteInEditMode]
[dfCategory("Basic Controls")]
[dfHelp("http://www.daikonforge.com/docs/df-gui/classdf_button.html")]
[dfTooltip("Provides a basic Button implementation that allows the developer to specify individual sprite images to be used to represent common button states.")]
[Serializable]
public class dfButton : dfInteractiveBase, IDFMultiRender, IRendersText
{
	// Token: 0x14000008 RID: 8
	// (add) Token: 0x06000F9C RID: 3996 RVA: 0x00048768 File Offset: 0x00046968
	// (remove) Token: 0x06000F9D RID: 3997 RVA: 0x000487A0 File Offset: 0x000469A0
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event PropertyChangedEventHandler<dfButton.ButtonState> ButtonStateChanged;

	// Token: 0x17000368 RID: 872
	// (get) Token: 0x06000F9E RID: 3998 RVA: 0x000487D8 File Offset: 0x000469D8
	// (set) Token: 0x06000F9F RID: 3999 RVA: 0x000487E0 File Offset: 0x000469E0
	public bool ClickWhenSpacePressed
	{
		get
		{
			return this.clickWhenSpacePressed;
		}
		set
		{
			this.clickWhenSpacePressed = value;
		}
	}

	// Token: 0x17000369 RID: 873
	// (get) Token: 0x06000FA0 RID: 4000 RVA: 0x000487EC File Offset: 0x000469EC
	// (set) Token: 0x06000FA1 RID: 4001 RVA: 0x000487F4 File Offset: 0x000469F4
	public dfButton.ButtonState State
	{
		get
		{
			return this.state;
		}
		set
		{
			if (!this.manualStateControl && value != this.state)
			{
				this.OnButtonStateChanged(value);
				this.Invalidate();
			}
		}
	}

	// Token: 0x06000FA2 RID: 4002 RVA: 0x0004881C File Offset: 0x00046A1C
	public void ForceState(dfButton.ButtonState newState)
	{
		if (newState != this.state)
		{
			this.OnButtonStateChanged(newState);
			this.Invalidate();
		}
	}

	// Token: 0x1700036A RID: 874
	// (get) Token: 0x06000FA3 RID: 4003 RVA: 0x00048838 File Offset: 0x00046A38
	// (set) Token: 0x06000FA4 RID: 4004 RVA: 0x00048840 File Offset: 0x00046A40
	public string PressedSprite
	{
		get
		{
			return this.pressedSprite;
		}
		set
		{
			if (value != this.pressedSprite)
			{
				this.pressedSprite = value;
				this.Invalidate();
			}
		}
	}

	// Token: 0x1700036B RID: 875
	// (get) Token: 0x06000FA5 RID: 4005 RVA: 0x00048860 File Offset: 0x00046A60
	// (set) Token: 0x06000FA6 RID: 4006 RVA: 0x00048868 File Offset: 0x00046A68
	public dfControl ButtonGroup
	{
		get
		{
			return this.group;
		}
		set
		{
			if (value != this.group)
			{
				this.group = value;
				this.Invalidate();
			}
		}
	}

	// Token: 0x1700036C RID: 876
	// (get) Token: 0x06000FA7 RID: 4007 RVA: 0x00048888 File Offset: 0x00046A88
	// (set) Token: 0x06000FA8 RID: 4008 RVA: 0x00048890 File Offset: 0x00046A90
	public bool AutoSize
	{
		get
		{
			return this.autoSize;
		}
		set
		{
			if (value != this.autoSize)
			{
				this.autoSize = value;
				if (value)
				{
					this.textAlign = TextAlignment.Left;
				}
				this.Invalidate();
			}
		}
	}

	// Token: 0x1700036D RID: 877
	// (get) Token: 0x06000FA9 RID: 4009 RVA: 0x000488B8 File Offset: 0x00046AB8
	// (set) Token: 0x06000FAA RID: 4010 RVA: 0x000488D0 File Offset: 0x00046AD0
	public TextAlignment TextAlignment
	{
		get
		{
			if (this.autoSize)
			{
				return TextAlignment.Left;
			}
			return this.textAlign;
		}
		set
		{
			if (value != this.textAlign)
			{
				this.textAlign = value;
				this.Invalidate();
			}
		}
	}

	// Token: 0x1700036E RID: 878
	// (get) Token: 0x06000FAB RID: 4011 RVA: 0x000488EC File Offset: 0x00046AEC
	// (set) Token: 0x06000FAC RID: 4012 RVA: 0x000488F4 File Offset: 0x00046AF4
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

	// Token: 0x1700036F RID: 879
	// (get) Token: 0x06000FAD RID: 4013 RVA: 0x00048910 File Offset: 0x00046B10
	// (set) Token: 0x06000FAE RID: 4014 RVA: 0x00048930 File Offset: 0x00046B30
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
			value = value.ConstrainPadding();
			if (!object.Equals(value, this.padding))
			{
				this.padding = value;
				this.Invalidate();
			}
		}
	}

	// Token: 0x17000370 RID: 880
	// (get) Token: 0x06000FAF RID: 4015 RVA: 0x00048958 File Offset: 0x00046B58
	// (set) Token: 0x06000FB0 RID: 4016 RVA: 0x0004899C File Offset: 0x00046B9C
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
			}
			this.Invalidate();
		}
	}

	// Token: 0x17000371 RID: 881
	// (get) Token: 0x06000FB1 RID: 4017 RVA: 0x000489C8 File Offset: 0x00046BC8
	// (set) Token: 0x06000FB2 RID: 4018 RVA: 0x000489D0 File Offset: 0x00046BD0
	public string Text
	{
		get
		{
			return this.text;
		}
		set
		{
			if (value != this.text)
			{
				this.CheckFontsForLanguage();
				dfFontManager.Invalidate(this.Font);
				this.localizationKey = value;
				this.text = base.getLocalizedValue(value);
				this.Invalidate();
			}
		}
	}

	// Token: 0x06000FB3 RID: 4019 RVA: 0x00048A10 File Offset: 0x00046C10
	public void ModifyLocalizedText(string text)
	{
		this.CheckFontsForLanguage();
		dfFontManager.Invalidate(this.Font);
		this.text = text;
		this.Invalidate();
	}

	// Token: 0x06000FB4 RID: 4020 RVA: 0x00048A30 File Offset: 0x00046C30
	protected void CheckFontsForLanguage()
	{
		if (!Application.isPlaying)
		{
			return;
		}
		StringTableManager.GungeonSupportedLanguages currentLanguage = GameManager.Options.CurrentLanguage;
		if (this.m_cachedLanguage == currentLanguage)
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
			return;
		}
		float num = this.m_defaultAssignedFontTextScale;
		if (Pixelator.Instance)
		{
			dfButton.m_cachedScaleTileScale = Pixelator.Instance.ScaleTileScale;
		}
		dfFontBase dfFontBase;
		if (currentLanguage == StringTableManager.GungeonSupportedLanguages.JAPANESE)
		{
			dfFontBase = (ResourceCache.Acquire("Alternate Fonts/JackeyFont12_DF") as GameObject).GetComponent<dfFont>();
			float num2 = Mathf.Max(1f, (float)this.m_defaultAssignedFont.FontSize / 14f);
			num = (float)Mathf.CeilToInt(this.m_defaultAssignedFontTextScale * num2);
		}
		else if (currentLanguage == StringTableManager.GungeonSupportedLanguages.CHINESE)
		{
			dfFontBase = (ResourceCache.Acquire("Alternate Fonts/SimSun12_DF") as GameObject).GetComponent<dfFont>();
			float num3 = Mathf.Max(1f, (float)this.m_defaultAssignedFont.FontSize / 14f);
			num = (float)Mathf.CeilToInt(this.m_defaultAssignedFontTextScale * num3);
		}
		else if (currentLanguage == StringTableManager.GungeonSupportedLanguages.KOREAN)
		{
			dfFontBase = (ResourceCache.Acquire("Alternate Fonts/NanumGothic16_DF") as GameObject).GetComponent<dfFont>();
			float num4 = (float)this.m_defaultAssignedFont.FontSize / (float)dfFontBase.FontSize;
			float num5 = Mathf.Max(3f, dfButton.m_cachedScaleTileScale);
			if (num4 < 1f)
			{
				num4 = (num5 - 1f) / num5;
			}
			num = ((num4 <= 1f) ? (this.m_defaultAssignedFontTextScale * num4) : ((float)Mathf.CeilToInt(this.m_defaultAssignedFontTextScale * num4)));
		}
		else if (currentLanguage == StringTableManager.GungeonSupportedLanguages.RUSSIAN)
		{
			dfFontBase = (ResourceCache.Acquire("Alternate Fonts/PixelaCYR_15_DF") as GameObject).GetComponent<dfFont>();
			float num6 = (float)this.m_defaultAssignedFont.FontSize / (float)dfFontBase.FontSize;
			if (num6 < 1f)
			{
				num6 = 1f;
			}
			num = ((num6 <= 1f) ? (this.m_defaultAssignedFontTextScale * num6) : ((float)Mathf.CeilToInt(this.m_defaultAssignedFontTextScale * num6)));
		}
		else
		{
			dfFontBase = this.m_defaultAssignedFont;
		}
		if (dfFontBase != null && this.Font != dfFontBase)
		{
			this.Font = dfFontBase;
			if (dfFontBase is dfFont)
			{
				base.Atlas = (dfFontBase as dfFont).Atlas;
			}
			this.TextScale = num;
		}
		this.m_cachedLanguage = currentLanguage;
	}

	// Token: 0x17000372 RID: 882
	// (get) Token: 0x06000FB5 RID: 4021 RVA: 0x00048CB4 File Offset: 0x00046EB4
	// (set) Token: 0x06000FB6 RID: 4022 RVA: 0x00048CBC File Offset: 0x00046EBC
	public Color32 TextColor
	{
		get
		{
			return this.textColor;
		}
		set
		{
			this.textColor = value;
			this.Invalidate();
		}
	}

	// Token: 0x17000373 RID: 883
	// (get) Token: 0x06000FB7 RID: 4023 RVA: 0x00048CCC File Offset: 0x00046ECC
	// (set) Token: 0x06000FB8 RID: 4024 RVA: 0x00048CD4 File Offset: 0x00046ED4
	public Color32 HoverTextColor
	{
		get
		{
			return this.hoverText;
		}
		set
		{
			this.hoverText = value;
			this.Invalidate();
		}
	}

	// Token: 0x17000374 RID: 884
	// (get) Token: 0x06000FB9 RID: 4025 RVA: 0x00048CE4 File Offset: 0x00046EE4
	// (set) Token: 0x06000FBA RID: 4026 RVA: 0x00048CEC File Offset: 0x00046EEC
	public Color32 NormalBackgroundColor
	{
		get
		{
			return this.normalColor;
		}
		set
		{
			this.normalColor = value;
			this.Invalidate();
		}
	}

	// Token: 0x17000375 RID: 885
	// (get) Token: 0x06000FBB RID: 4027 RVA: 0x00048CFC File Offset: 0x00046EFC
	// (set) Token: 0x06000FBC RID: 4028 RVA: 0x00048D04 File Offset: 0x00046F04
	public Color32 HoverBackgroundColor
	{
		get
		{
			return this.hoverColor;
		}
		set
		{
			this.hoverColor = value;
			this.Invalidate();
		}
	}

	// Token: 0x17000376 RID: 886
	// (get) Token: 0x06000FBD RID: 4029 RVA: 0x00048D14 File Offset: 0x00046F14
	// (set) Token: 0x06000FBE RID: 4030 RVA: 0x00048D1C File Offset: 0x00046F1C
	public Color32 PressedTextColor
	{
		get
		{
			return this.pressedText;
		}
		set
		{
			this.pressedText = value;
			this.Invalidate();
		}
	}

	// Token: 0x17000377 RID: 887
	// (get) Token: 0x06000FBF RID: 4031 RVA: 0x00048D2C File Offset: 0x00046F2C
	// (set) Token: 0x06000FC0 RID: 4032 RVA: 0x00048D34 File Offset: 0x00046F34
	public Color32 PressedBackgroundColor
	{
		get
		{
			return this.pressedColor;
		}
		set
		{
			this.pressedColor = value;
			this.Invalidate();
		}
	}

	// Token: 0x17000378 RID: 888
	// (get) Token: 0x06000FC1 RID: 4033 RVA: 0x00048D44 File Offset: 0x00046F44
	// (set) Token: 0x06000FC2 RID: 4034 RVA: 0x00048D4C File Offset: 0x00046F4C
	public Color32 FocusTextColor
	{
		get
		{
			return this.focusText;
		}
		set
		{
			this.focusText = value;
			this.Invalidate();
		}
	}

	// Token: 0x17000379 RID: 889
	// (get) Token: 0x06000FC3 RID: 4035 RVA: 0x00048D5C File Offset: 0x00046F5C
	// (set) Token: 0x06000FC4 RID: 4036 RVA: 0x00048D64 File Offset: 0x00046F64
	public Color32 FocusBackgroundColor
	{
		get
		{
			return this.focusColor;
		}
		set
		{
			this.focusColor = value;
			this.Invalidate();
		}
	}

	// Token: 0x1700037A RID: 890
	// (get) Token: 0x06000FC5 RID: 4037 RVA: 0x00048D74 File Offset: 0x00046F74
	// (set) Token: 0x06000FC6 RID: 4038 RVA: 0x00048D7C File Offset: 0x00046F7C
	public Color32 DisabledTextColor
	{
		get
		{
			return this.disabledText;
		}
		set
		{
			this.disabledText = value;
			this.Invalidate();
		}
	}

	// Token: 0x1700037B RID: 891
	// (get) Token: 0x06000FC7 RID: 4039 RVA: 0x00048D8C File Offset: 0x00046F8C
	// (set) Token: 0x06000FC8 RID: 4040 RVA: 0x00048D94 File Offset: 0x00046F94
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

	// Token: 0x1700037C RID: 892
	// (get) Token: 0x06000FC9 RID: 4041 RVA: 0x00048DCC File Offset: 0x00046FCC
	// (set) Token: 0x06000FCA RID: 4042 RVA: 0x00048DD4 File Offset: 0x00046FD4
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

	// Token: 0x1700037D RID: 893
	// (get) Token: 0x06000FCB RID: 4043 RVA: 0x00048DE4 File Offset: 0x00046FE4
	// (set) Token: 0x06000FCC RID: 4044 RVA: 0x00048DEC File Offset: 0x00046FEC
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

	// Token: 0x1700037E RID: 894
	// (get) Token: 0x06000FCD RID: 4045 RVA: 0x00048E08 File Offset: 0x00047008
	// (set) Token: 0x06000FCE RID: 4046 RVA: 0x00048E10 File Offset: 0x00047010
	public bool Shadow
	{
		get
		{
			return this.textShadow;
		}
		set
		{
			if (value != this.textShadow)
			{
				this.textShadow = value;
				this.Invalidate();
			}
		}
	}

	// Token: 0x1700037F RID: 895
	// (get) Token: 0x06000FCF RID: 4047 RVA: 0x00048E2C File Offset: 0x0004702C
	// (set) Token: 0x06000FD0 RID: 4048 RVA: 0x00048E34 File Offset: 0x00047034
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

	// Token: 0x17000380 RID: 896
	// (get) Token: 0x06000FD1 RID: 4049 RVA: 0x00048E60 File Offset: 0x00047060
	// (set) Token: 0x06000FD2 RID: 4050 RVA: 0x00048E68 File Offset: 0x00047068
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

	// Token: 0x06000FD3 RID: 4051 RVA: 0x00048E88 File Offset: 0x00047088
	protected internal override void OnLocalize()
	{
		base.OnLocalize();
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
		if (this.forceUpperCase)
		{
			this.text = this.text.ToUpperInvariant();
		}
		this.CheckFontsForLanguage();
		base.PerformLayout();
	}

	// Token: 0x06000FD4 RID: 4052 RVA: 0x00048F34 File Offset: 0x00047134
	[HideInInspector]
	public override void Invalidate()
	{
		base.Invalidate();
		if (this.m_cachedLanguage != GameManager.Options.CurrentLanguage)
		{
			this.CheckFontsForLanguage();
		}
		if (this.AutoSize)
		{
			this.autoSizeToText();
		}
	}

	// Token: 0x06000FD5 RID: 4053 RVA: 0x00048F68 File Offset: 0x00047168
	public override void Start()
	{
		base.Start();
	}

	// Token: 0x06000FD6 RID: 4054 RVA: 0x00048F70 File Offset: 0x00047170
	public override void OnEnable()
	{
		base.OnEnable();
		bool flag = this.Font != null && this.Font.IsValid;
		if (Application.isPlaying && !flag)
		{
			this.Font = base.GetManager().DefaultFont;
		}
		this.bindTextureRebuildCallback();
	}

	// Token: 0x06000FD7 RID: 4055 RVA: 0x00048FCC File Offset: 0x000471CC
	public override void OnDisable()
	{
		base.OnDisable();
		this.unbindTextureRebuildCallback();
	}

	// Token: 0x06000FD8 RID: 4056 RVA: 0x00048FDC File Offset: 0x000471DC
	public override void Awake()
	{
		this.localizationKey = this.Text;
		base.Awake();
		this.startSize = base.Size;
	}

	// Token: 0x06000FD9 RID: 4057 RVA: 0x00048FFC File Offset: 0x000471FC
	public float GetAutosizeWidth()
	{
		float num;
		using (dfFontRendererBase dfFontRendererBase = this.obtainTextRenderer())
		{
			num = dfFontRendererBase.MeasureString(this.text).RoundToInt().x + (float)this.padding.horizontal;
		}
		return num;
	}

	// Token: 0x06000FDA RID: 4058 RVA: 0x0004905C File Offset: 0x0004725C
	protected internal override void OnEnterFocus(dfFocusEventArgs args)
	{
		if (this.State != dfButton.ButtonState.Pressed)
		{
			this.State = dfButton.ButtonState.Focus;
		}
		base.OnEnterFocus(args);
	}

	// Token: 0x06000FDB RID: 4059 RVA: 0x00049078 File Offset: 0x00047278
	protected internal override void OnLeaveFocus(dfFocusEventArgs args)
	{
		this.State = dfButton.ButtonState.Default;
		base.OnLeaveFocus(args);
	}

	// Token: 0x06000FDC RID: 4060 RVA: 0x00049088 File Offset: 0x00047288
	protected internal override void OnKeyPress(dfKeyEventArgs args)
	{
		if (this.ClickWhenSpacePressed && this.IsInteractive && args.KeyCode == KeyCode.Space)
		{
			this.OnClick(new dfMouseEventArgs(this, dfMouseButtons.Left, 1, default(Ray), Vector2.zero, 0f));
			return;
		}
		base.OnKeyPress(args);
	}

	// Token: 0x06000FDD RID: 4061 RVA: 0x000490E4 File Offset: 0x000472E4
	protected internal override void OnClick(dfMouseEventArgs args)
	{
		if (this.group != null)
		{
			foreach (dfButton dfButton in base.transform.parent.GetComponentsInChildren<dfButton>())
			{
				if (dfButton != this && dfButton.ButtonGroup == this.ButtonGroup && dfButton != this)
				{
					dfButton.State = dfButton.ButtonState.Default;
				}
			}
			if (!base.transform.IsChildOf(this.group.transform))
			{
				base.Signal(this.group.gameObject, "OnClick", args);
			}
		}
		base.OnClick(args);
	}

	// Token: 0x06000FDE RID: 4062 RVA: 0x0004919C File Offset: 0x0004739C
	protected internal override void OnMouseDown(dfMouseEventArgs args)
	{
		if (!(this.parent is dfTabstrip) || this.State != dfButton.ButtonState.Focus)
		{
			this.State = dfButton.ButtonState.Pressed;
		}
		base.OnMouseDown(args);
	}

	// Token: 0x06000FDF RID: 4063 RVA: 0x000491C8 File Offset: 0x000473C8
	protected internal override void OnMouseUp(dfMouseEventArgs args)
	{
		if (!base.IsEnabled)
		{
			this.State = dfButton.ButtonState.Disabled;
			base.OnMouseUp(args);
			return;
		}
		if (this.isMouseHovering)
		{
			if (this.parent is dfTabstrip && this.ContainsFocus)
			{
				this.State = dfButton.ButtonState.Focus;
			}
			else
			{
				this.State = dfButton.ButtonState.Hover;
			}
		}
		else if (this.HasFocus)
		{
			this.State = dfButton.ButtonState.Focus;
		}
		else
		{
			this.State = dfButton.ButtonState.Default;
		}
		base.OnMouseUp(args);
	}

	// Token: 0x06000FE0 RID: 4064 RVA: 0x00049254 File Offset: 0x00047454
	protected internal override void OnMouseEnter(dfMouseEventArgs args)
	{
		if (!(this.parent is dfTabstrip) || this.State != dfButton.ButtonState.Focus)
		{
			this.State = dfButton.ButtonState.Hover;
		}
		base.OnMouseEnter(args);
	}

	// Token: 0x06000FE1 RID: 4065 RVA: 0x00049280 File Offset: 0x00047480
	protected internal override void OnMouseLeave(dfMouseEventArgs args)
	{
		if (this.ContainsFocus)
		{
			this.State = dfButton.ButtonState.Focus;
		}
		else
		{
			this.State = dfButton.ButtonState.Default;
		}
		base.OnMouseLeave(args);
	}

	// Token: 0x06000FE2 RID: 4066 RVA: 0x000492A8 File Offset: 0x000474A8
	protected internal override void OnIsEnabledChanged()
	{
		if (!base.IsEnabled)
		{
			this.State = dfButton.ButtonState.Disabled;
		}
		else
		{
			this.State = dfButton.ButtonState.Default;
		}
		base.OnIsEnabledChanged();
	}

	// Token: 0x06000FE3 RID: 4067 RVA: 0x000492D0 File Offset: 0x000474D0
	protected virtual void OnButtonStateChanged(dfButton.ButtonState value)
	{
		if (value != dfButton.ButtonState.Disabled && !base.IsEnabled)
		{
			value = dfButton.ButtonState.Disabled;
		}
		this.state = value;
		base.Signal("OnButtonStateChanged", this, value);
		if (this.ButtonStateChanged != null)
		{
			this.ButtonStateChanged(this, value);
		}
		this.Invalidate();
	}

	// Token: 0x06000FE4 RID: 4068 RVA: 0x0004932C File Offset: 0x0004752C
	protected override Color32 getActiveColor()
	{
		switch (this.State)
		{
		case dfButton.ButtonState.Focus:
			return this.FocusBackgroundColor;
		case dfButton.ButtonState.Hover:
			return this.HoverBackgroundColor;
		case dfButton.ButtonState.Pressed:
			return this.PressedBackgroundColor;
		case dfButton.ButtonState.Disabled:
			return base.DisabledColor;
		default:
			return this.NormalBackgroundColor;
		}
	}

	// Token: 0x06000FE5 RID: 4069 RVA: 0x00049380 File Offset: 0x00047580
	private void autoSizeToText()
	{
		if (this.Font == null || !this.Font.IsValid || string.IsNullOrEmpty(this.Text))
		{
			return;
		}
		using (dfFontRendererBase dfFontRendererBase = this.obtainTextRenderer())
		{
			Vector2 vector = dfFontRendererBase.MeasureString(this.Text);
			Vector2 vector2 = new Vector2(vector.x + (float)this.padding.horizontal, vector.y + (float)this.padding.vertical);
			if (this.size != vector2)
			{
				base.SuspendLayout();
				base.Size = vector2;
				base.ResumeLayout();
			}
		}
	}

	// Token: 0x06000FE6 RID: 4070 RVA: 0x00049448 File Offset: 0x00047648
	private dfRenderData renderText()
	{
		if (this.Font == null || !this.Font.IsValid || string.IsNullOrEmpty(this.Text))
		{
			return null;
		}
		dfRenderData renderData = this.renderData;
		if (this.font is dfDynamicFont)
		{
			dfDynamicFont dfDynamicFont = (dfDynamicFont)this.font;
			renderData = this.textRenderData;
			renderData.Clear();
			renderData.Material = dfDynamicFont.Material;
		}
		using (dfFontRendererBase dfFontRendererBase = this.obtainTextRenderer())
		{
			dfFontRendererBase.Render(this.text, renderData);
		}
		return renderData;
	}

	// Token: 0x06000FE7 RID: 4071 RVA: 0x000494FC File Offset: 0x000476FC
	private dfFontRendererBase obtainTextRenderer()
	{
		Vector2 vector = base.Size - new Vector2((float)this.padding.horizontal, (float)this.padding.vertical);
		Vector2 vector2 = ((!this.autoSize) ? vector : (Vector2.one * 2.1474836E+09f));
		float num = base.PixelsToUnits();
		Vector3 vector3 = (this.pivot.TransformToUpperLeft(base.Size) + new Vector3((float)this.padding.left, (float)(-(float)this.padding.top))) * num;
		float num2 = this.TextScale * this.getTextScaleMultiplier();
		Color32 color = base.ApplyOpacity(this.getTextColorForState());
		dfFontRendererBase dfFontRendererBase = this.Font.ObtainRenderer();
		dfFontRendererBase.WordWrap = this.WordWrap;
		dfFontRendererBase.MultiLine = this.WordWrap;
		dfFontRendererBase.MaxSize = vector2;
		dfFontRendererBase.PixelRatio = num;
		dfFontRendererBase.TextScale = num2;
		dfFontRendererBase.CharacterSpacing = 0;
		int num3 = this.textPixelOffset;
		if (this.state == dfButton.ButtonState.Hover)
		{
			num3 = this.hoverTextPixelOffset;
		}
		if (this.state == dfButton.ButtonState.Pressed)
		{
			num3 = this.downTextPixelOffset;
		}
		if (this.state == dfButton.ButtonState.Disabled)
		{
			num3 = this.downTextPixelOffset;
		}
		dfFontRendererBase.VectorOffset = vector3.Quantize(num) + new Vector3(0f, (float)num3 * num, 0f);
		dfFontRendererBase.TabSize = 0;
		dfFontRendererBase.TextAlign = ((!this.autoSize) ? this.TextAlignment : TextAlignment.Left);
		dfFontRendererBase.ProcessMarkup = true;
		dfFontRendererBase.DefaultColor = color;
		dfFontRendererBase.OverrideMarkupColors = false;
		dfFontRendererBase.Opacity = base.CalculateOpacity();
		dfFontRendererBase.Shadow = this.Shadow;
		dfFontRendererBase.ShadowColor = this.ShadowColor;
		dfFontRendererBase.ShadowOffset = this.ShadowOffset;
		dfDynamicFont.DynamicFontRenderer dynamicFontRenderer = dfFontRendererBase as dfDynamicFont.DynamicFontRenderer;
		if (dynamicFontRenderer != null)
		{
			dynamicFontRenderer.SpriteAtlas = base.Atlas;
			dynamicFontRenderer.SpriteBuffer = this.renderData;
		}
		if (this.vertAlign != dfVerticalAlignment.Top)
		{
			dfFontRendererBase.VectorOffset = this.getVertAlignOffset(dfFontRendererBase);
		}
		return dfFontRendererBase;
	}

	// Token: 0x06000FE8 RID: 4072 RVA: 0x00049720 File Offset: 0x00047920
	private float getTextScaleMultiplier()
	{
		if (this.textScaleMode == dfTextScaleMode.None || !Application.isPlaying)
		{
			return 1f;
		}
		if (this.textScaleMode == dfTextScaleMode.ScreenResolution)
		{
			return (float)Screen.height / (float)this.cachedManager.FixedHeight;
		}
		if (this.autoSize)
		{
			return 1f;
		}
		return base.Size.y / this.startSize.y;
	}

	// Token: 0x06000FE9 RID: 4073 RVA: 0x00049794 File Offset: 0x00047994
	private Color32 getTextColorForState()
	{
		if (!base.IsEnabled)
		{
			return this.DisabledTextColor;
		}
		switch (this.state)
		{
		case dfButton.ButtonState.Default:
			return this.TextColor;
		case dfButton.ButtonState.Focus:
			return this.FocusTextColor;
		case dfButton.ButtonState.Hover:
			return this.HoverTextColor;
		case dfButton.ButtonState.Pressed:
			return this.PressedTextColor;
		case dfButton.ButtonState.Disabled:
			return this.DisabledTextColor;
		default:
			return UnityEngine.Color.white;
		}
	}

	// Token: 0x06000FEA RID: 4074 RVA: 0x00049808 File Offset: 0x00047A08
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

	// Token: 0x06000FEB RID: 4075 RVA: 0x000498B8 File Offset: 0x00047AB8
	protected internal override dfAtlas.ItemInfo getBackgroundSprite()
	{
		if (base.Atlas == null)
		{
			return null;
		}
		dfAtlas.ItemInfo itemInfo = null;
		switch (this.state)
		{
		case dfButton.ButtonState.Default:
			itemInfo = this.atlas[this.backgroundSprite];
			break;
		case dfButton.ButtonState.Focus:
			itemInfo = this.atlas[this.focusSprite];
			break;
		case dfButton.ButtonState.Hover:
			itemInfo = this.atlas[this.hoverSprite];
			break;
		case dfButton.ButtonState.Pressed:
			itemInfo = this.atlas[this.pressedSprite];
			break;
		case dfButton.ButtonState.Disabled:
			itemInfo = this.atlas[this.disabledSprite];
			break;
		}
		if (itemInfo == null)
		{
			itemInfo = this.atlas[this.backgroundSprite];
		}
		return itemInfo;
	}

	// Token: 0x06000FEC RID: 4076 RVA: 0x00049994 File Offset: 0x00047B94
	public dfList<dfRenderData> RenderMultiple()
	{
		if (this.renderData == null)
		{
			this.renderData = dfRenderData.Obtain();
			this.textRenderData = dfRenderData.Obtain();
			this.isControlInvalidated = true;
		}
		Matrix4x4 localToWorldMatrix = base.transform.localToWorldMatrix;
		if (!this.isControlInvalidated)
		{
			for (int i = 0; i < this.buffers.Count; i++)
			{
				this.buffers[i].Transform = localToWorldMatrix;
			}
			return this.buffers;
		}
		this.isControlInvalidated = false;
		this.buffers.Clear();
		this.renderData.Clear();
		if (base.Atlas != null)
		{
			this.renderData.Material = base.Atlas.Material;
			this.renderData.Transform = localToWorldMatrix;
			this.renderBackground();
			this.buffers.Add(this.renderData);
		}
		dfRenderData dfRenderData = this.renderText();
		if (dfRenderData != null && dfRenderData != this.renderData)
		{
			dfRenderData.Transform = localToWorldMatrix;
			this.buffers.Add(dfRenderData);
		}
		base.updateCollider();
		return this.buffers;
	}

	// Token: 0x06000FED RID: 4077 RVA: 0x00049AB4 File Offset: 0x00047CB4
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

	// Token: 0x06000FEE RID: 4078 RVA: 0x00049B08 File Offset: 0x00047D08
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

	// Token: 0x06000FEF RID: 4079 RVA: 0x00049B5C File Offset: 0x00047D5C
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

	// Token: 0x06000FF0 RID: 4080 RVA: 0x00049BD4 File Offset: 0x00047DD4
	private void onFontTextureRebuilt(Font font)
	{
		if (this.Font is dfDynamicFont && font == (this.Font as dfDynamicFont).BaseFont)
		{
			this.requestCharacterInfo();
			this.Invalidate();
		}
	}

	// Token: 0x06000FF1 RID: 4081 RVA: 0x00049C10 File Offset: 0x00047E10
	public void UpdateFontInfo()
	{
		this.requestCharacterInfo();
	}

	// Token: 0x04000EDE RID: 3806
	[SerializeField]
	protected dfFontBase font;

	// Token: 0x04000EDF RID: 3807
	[SerializeField]
	protected string pressedSprite;

	// Token: 0x04000EE0 RID: 3808
	[SerializeField]
	protected dfButton.ButtonState state;

	// Token: 0x04000EE1 RID: 3809
	[SerializeField]
	protected dfControl group;

	// Token: 0x04000EE2 RID: 3810
	[SerializeField]
	protected string text = string.Empty;

	// Token: 0x04000EE3 RID: 3811
	[SerializeField]
	public int textPixelOffset;

	// Token: 0x04000EE4 RID: 3812
	[SerializeField]
	public int hoverTextPixelOffset;

	// Token: 0x04000EE5 RID: 3813
	[SerializeField]
	public int downTextPixelOffset;

	// Token: 0x04000EE6 RID: 3814
	[SerializeField]
	protected TextAlignment textAlign = TextAlignment.Center;

	// Token: 0x04000EE7 RID: 3815
	[SerializeField]
	protected dfVerticalAlignment vertAlign = dfVerticalAlignment.Middle;

	// Token: 0x04000EE8 RID: 3816
	[SerializeField]
	protected Color32 normalColor = UnityEngine.Color.white;

	// Token: 0x04000EE9 RID: 3817
	[SerializeField]
	protected Color32 textColor = UnityEngine.Color.white;

	// Token: 0x04000EEA RID: 3818
	[SerializeField]
	protected Color32 hoverText = UnityEngine.Color.white;

	// Token: 0x04000EEB RID: 3819
	[SerializeField]
	protected Color32 pressedText = UnityEngine.Color.white;

	// Token: 0x04000EEC RID: 3820
	[SerializeField]
	protected Color32 focusText = UnityEngine.Color.white;

	// Token: 0x04000EED RID: 3821
	[SerializeField]
	protected Color32 disabledText = UnityEngine.Color.white;

	// Token: 0x04000EEE RID: 3822
	[SerializeField]
	protected Color32 hoverColor = UnityEngine.Color.white;

	// Token: 0x04000EEF RID: 3823
	[SerializeField]
	protected Color32 pressedColor = UnityEngine.Color.white;

	// Token: 0x04000EF0 RID: 3824
	[SerializeField]
	protected Color32 focusColor = UnityEngine.Color.white;

	// Token: 0x04000EF1 RID: 3825
	[SerializeField]
	protected float textScale = 1f;

	// Token: 0x04000EF2 RID: 3826
	[SerializeField]
	protected dfTextScaleMode textScaleMode;

	// Token: 0x04000EF3 RID: 3827
	[SerializeField]
	protected bool wordWrap;

	// Token: 0x04000EF4 RID: 3828
	[SerializeField]
	protected RectOffset padding = new RectOffset();

	// Token: 0x04000EF5 RID: 3829
	[SerializeField]
	protected bool textShadow;

	// Token: 0x04000EF6 RID: 3830
	[SerializeField]
	protected Color32 shadowColor = UnityEngine.Color.black;

	// Token: 0x04000EF7 RID: 3831
	[SerializeField]
	protected Vector2 shadowOffset = new Vector2(1f, -1f);

	// Token: 0x04000EF8 RID: 3832
	[SerializeField]
	protected bool autoSize;

	// Token: 0x04000EF9 RID: 3833
	[SerializeField]
	protected bool clickWhenSpacePressed = true;

	// Token: 0x04000EFA RID: 3834
	[SerializeField]
	public bool forceUpperCase;

	// Token: 0x04000EFB RID: 3835
	[SerializeField]
	public bool manualStateControl;

	// Token: 0x04000EFC RID: 3836
	protected dfFontBase m_defaultAssignedFont;

	// Token: 0x04000EFD RID: 3837
	protected float m_defaultAssignedFontTextScale;

	// Token: 0x04000EFE RID: 3838
	private StringTableManager.GungeonSupportedLanguages m_cachedLanguage;

	// Token: 0x04000EFF RID: 3839
	private static float m_cachedScaleTileScale = 3f;

	// Token: 0x04000F00 RID: 3840
	private Vector2 startSize = Vector2.zero;

	// Token: 0x04000F01 RID: 3841
	private bool isFontCallbackAssigned;

	// Token: 0x04000F02 RID: 3842
	private dfRenderData textRenderData;

	// Token: 0x04000F03 RID: 3843
	private dfList<dfRenderData> buffers = dfList<dfRenderData>.Obtain();

	// Token: 0x0200038F RID: 911
	public enum ButtonState
	{
		// Token: 0x04000F05 RID: 3845
		Default,
		// Token: 0x04000F06 RID: 3846
		Focus,
		// Token: 0x04000F07 RID: 3847
		Hover,
		// Token: 0x04000F08 RID: 3848
		Pressed,
		// Token: 0x04000F09 RID: 3849
		Disabled
	}
}
