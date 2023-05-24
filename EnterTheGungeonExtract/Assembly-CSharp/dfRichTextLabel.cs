using System;
using System.Diagnostics;
using UnityEngine;

// Token: 0x020004B5 RID: 1205
[AddComponentMenu("Daikon Forge/User Interface/Rich Text Label")]
[ExecuteInEditMode]
[Serializable]
public class dfRichTextLabel : dfControl, IDFMultiRender, IRendersText
{
	// Token: 0x1400005F RID: 95
	// (add) Token: 0x06001BED RID: 7149 RVA: 0x000841C8 File Offset: 0x000823C8
	// (remove) Token: 0x06001BEE RID: 7150 RVA: 0x00084200 File Offset: 0x00082400
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event PropertyChangedEventHandler<string> TextChanged;

	// Token: 0x14000060 RID: 96
	// (add) Token: 0x06001BEF RID: 7151 RVA: 0x00084238 File Offset: 0x00082438
	// (remove) Token: 0x06001BF0 RID: 7152 RVA: 0x00084270 File Offset: 0x00082470
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event PropertyChangedEventHandler<Vector2> ScrollPositionChanged;

	// Token: 0x14000061 RID: 97
	// (add) Token: 0x06001BF1 RID: 7153 RVA: 0x000842A8 File Offset: 0x000824A8
	// (remove) Token: 0x06001BF2 RID: 7154 RVA: 0x000842E0 File Offset: 0x000824E0
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event dfRichTextLabel.LinkClickEventHandler LinkClicked;

	// Token: 0x170005B2 RID: 1458
	// (get) Token: 0x06001BF3 RID: 7155 RVA: 0x00084318 File Offset: 0x00082518
	// (set) Token: 0x06001BF4 RID: 7156 RVA: 0x00084320 File Offset: 0x00082520
	public bool AutoHeight
	{
		get
		{
			return this.autoHeight;
		}
		set
		{
			if (this.autoHeight != value)
			{
				this.autoHeight = value;
				this.scrollPosition = Vector2.zero;
				this.Invalidate();
			}
		}
	}

	// Token: 0x170005B3 RID: 1459
	// (get) Token: 0x06001BF5 RID: 7157 RVA: 0x00084348 File Offset: 0x00082548
	// (set) Token: 0x06001BF6 RID: 7158 RVA: 0x00084390 File Offset: 0x00082590
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

	// Token: 0x170005B4 RID: 1460
	// (get) Token: 0x06001BF7 RID: 7159 RVA: 0x000843B0 File Offset: 0x000825B0
	// (set) Token: 0x06001BF8 RID: 7160 RVA: 0x000843B8 File Offset: 0x000825B8
	public dfDynamicFont Font
	{
		get
		{
			return this.font;
		}
		set
		{
			if (value != this.font)
			{
				this.unbindTextureRebuildCallback();
				this.font = value;
				this.bindTextureRebuildCallback();
				this.LineHeight = value.FontSize;
				dfFontManager.Invalidate(this.Font);
				this.Invalidate();
			}
		}
	}

	// Token: 0x170005B5 RID: 1461
	// (get) Token: 0x06001BF9 RID: 7161 RVA: 0x00084408 File Offset: 0x00082608
	// (set) Token: 0x06001BFA RID: 7162 RVA: 0x00084410 File Offset: 0x00082610
	public string BlankTextureSprite
	{
		get
		{
			return this.blankTextureSprite;
		}
		set
		{
			if (value != this.blankTextureSprite)
			{
				this.blankTextureSprite = value;
				this.Invalidate();
			}
		}
	}

	// Token: 0x170005B6 RID: 1462
	// (get) Token: 0x06001BFB RID: 7163 RVA: 0x00084430 File Offset: 0x00082630
	// (set) Token: 0x06001BFC RID: 7164 RVA: 0x00084438 File Offset: 0x00082638
	public string Text
	{
		get
		{
			return this.text;
		}
		set
		{
			value = base.getLocalizedValue(value);
			if (!string.Equals(this.text, value))
			{
				dfFontManager.Invalidate(this.Font);
				this.text = value;
				this.scrollPosition = Vector2.zero;
				this.Invalidate();
				this.OnTextChanged();
			}
		}
	}

	// Token: 0x170005B7 RID: 1463
	// (get) Token: 0x06001BFD RID: 7165 RVA: 0x00084488 File Offset: 0x00082688
	// (set) Token: 0x06001BFE RID: 7166 RVA: 0x00084490 File Offset: 0x00082690
	public int FontSize
	{
		get
		{
			return this.fontSize;
		}
		set
		{
			value = Mathf.Max(6, value);
			if (value != this.fontSize)
			{
				dfFontManager.Invalidate(this.Font);
				this.fontSize = value;
				this.Invalidate();
			}
			this.LineHeight = value;
		}
	}

	// Token: 0x170005B8 RID: 1464
	// (get) Token: 0x06001BFF RID: 7167 RVA: 0x000844C8 File Offset: 0x000826C8
	// (set) Token: 0x06001C00 RID: 7168 RVA: 0x000844D0 File Offset: 0x000826D0
	public int LineHeight
	{
		get
		{
			return this.lineheight;
		}
		set
		{
			value = Mathf.Max(this.FontSize, value);
			if (value != this.lineheight)
			{
				this.lineheight = value;
				this.Invalidate();
			}
		}
	}

	// Token: 0x170005B9 RID: 1465
	// (get) Token: 0x06001C01 RID: 7169 RVA: 0x000844FC File Offset: 0x000826FC
	// (set) Token: 0x06001C02 RID: 7170 RVA: 0x00084504 File Offset: 0x00082704
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

	// Token: 0x170005BA RID: 1466
	// (get) Token: 0x06001C03 RID: 7171 RVA: 0x00084514 File Offset: 0x00082714
	// (set) Token: 0x06001C04 RID: 7172 RVA: 0x0008451C File Offset: 0x0008271C
	public bool PreserveWhitespace
	{
		get
		{
			return this.preserveWhitespace;
		}
		set
		{
			if (value != this.preserveWhitespace)
			{
				this.preserveWhitespace = value;
				this.Invalidate();
			}
		}
	}

	// Token: 0x170005BB RID: 1467
	// (get) Token: 0x06001C05 RID: 7173 RVA: 0x00084538 File Offset: 0x00082738
	// (set) Token: 0x06001C06 RID: 7174 RVA: 0x00084540 File Offset: 0x00082740
	public FontStyle FontStyle
	{
		get
		{
			return this.fontStyle;
		}
		set
		{
			if (value != this.fontStyle)
			{
				this.fontStyle = value;
				this.Invalidate();
			}
		}
	}

	// Token: 0x170005BC RID: 1468
	// (get) Token: 0x06001C07 RID: 7175 RVA: 0x0008455C File Offset: 0x0008275C
	// (set) Token: 0x06001C08 RID: 7176 RVA: 0x00084564 File Offset: 0x00082764
	public dfMarkupTextAlign TextAlignment
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

	// Token: 0x170005BD RID: 1469
	// (get) Token: 0x06001C09 RID: 7177 RVA: 0x00084580 File Offset: 0x00082780
	// (set) Token: 0x06001C0A RID: 7178 RVA: 0x00084588 File Offset: 0x00082788
	public bool AllowScrolling
	{
		get
		{
			return this.allowScrolling;
		}
		set
		{
			this.allowScrolling = value;
			if (!value)
			{
				this.ScrollPosition = Vector2.zero;
			}
		}
	}

	// Token: 0x170005BE RID: 1470
	// (get) Token: 0x06001C0B RID: 7179 RVA: 0x000845A4 File Offset: 0x000827A4
	// (set) Token: 0x06001C0C RID: 7180 RVA: 0x000845AC File Offset: 0x000827AC
	public Vector2 ScrollPosition
	{
		get
		{
			return this.scrollPosition;
		}
		set
		{
			if (!this.allowScrolling || this.autoHeight)
			{
				value = Vector2.zero;
			}
			if (this.isMarkupInvalidated)
			{
				this.processMarkup();
			}
			Vector2 vector = this.ContentSize - base.Size;
			value = Vector2.Min(vector, value);
			value = Vector2.Max(Vector2.zero, value);
			value = value.RoundToInt();
			if ((value - this.scrollPosition).sqrMagnitude > 1E-45f)
			{
				this.scrollPosition = value;
				this.updateScrollbars();
				this.OnScrollPositionChanged();
			}
		}
	}

	// Token: 0x170005BF RID: 1471
	// (get) Token: 0x06001C0D RID: 7181 RVA: 0x00084648 File Offset: 0x00082848
	// (set) Token: 0x06001C0E RID: 7182 RVA: 0x00084650 File Offset: 0x00082850
	public dfScrollbar HorizontalScrollbar
	{
		get
		{
			return this.horzScrollbar;
		}
		set
		{
			this.horzScrollbar = value;
			this.updateScrollbars();
		}
	}

	// Token: 0x170005C0 RID: 1472
	// (get) Token: 0x06001C0F RID: 7183 RVA: 0x00084660 File Offset: 0x00082860
	// (set) Token: 0x06001C10 RID: 7184 RVA: 0x00084668 File Offset: 0x00082868
	public dfScrollbar VerticalScrollbar
	{
		get
		{
			return this.vertScrollbar;
		}
		set
		{
			this.vertScrollbar = value;
			this.updateScrollbars();
		}
	}

	// Token: 0x170005C1 RID: 1473
	// (get) Token: 0x06001C11 RID: 7185 RVA: 0x00084678 File Offset: 0x00082878
	public Vector2 ContentSize
	{
		get
		{
			if (this.viewportBox != null)
			{
				return this.viewportBox.Size;
			}
			return base.Size;
		}
	}

	// Token: 0x170005C2 RID: 1474
	// (get) Token: 0x06001C12 RID: 7186 RVA: 0x00084698 File Offset: 0x00082898
	// (set) Token: 0x06001C13 RID: 7187 RVA: 0x000846A0 File Offset: 0x000828A0
	public bool UseScrollMomentum
	{
		get
		{
			return this.useScrollMomentum;
		}
		set
		{
			this.useScrollMomentum = value;
			this.scrollMomentum = Vector2.zero;
		}
	}

	// Token: 0x06001C14 RID: 7188 RVA: 0x000846B4 File Offset: 0x000828B4
	protected internal override void OnLocalize()
	{
		base.OnLocalize();
		this.Text = base.getLocalizedValue(this.text);
	}

	// Token: 0x06001C15 RID: 7189 RVA: 0x000846D0 File Offset: 0x000828D0
	[HideInInspector]
	public override void Invalidate()
	{
		base.Invalidate();
		dfFontManager.Invalidate(this.Font);
		this.isMarkupInvalidated = true;
	}

	// Token: 0x06001C16 RID: 7190 RVA: 0x000846EC File Offset: 0x000828EC
	public override void Awake()
	{
		base.Awake();
		this.startSize = base.Size;
	}

	// Token: 0x06001C17 RID: 7191 RVA: 0x00084700 File Offset: 0x00082900
	public override void OnEnable()
	{
		base.OnEnable();
		this.bindTextureRebuildCallback();
		if (this.size.sqrMagnitude <= 1E-45f)
		{
			base.Size = new Vector2(320f, 200f);
			int num = 16;
			this.LineHeight = num;
			this.FontSize = num;
		}
	}

	// Token: 0x06001C18 RID: 7192 RVA: 0x00084754 File Offset: 0x00082954
	public override void OnDisable()
	{
		base.OnDisable();
		this.unbindTextureRebuildCallback();
	}

	// Token: 0x06001C19 RID: 7193 RVA: 0x00084764 File Offset: 0x00082964
	public override void Update()
	{
		base.Update();
		if (this.useScrollMomentum && !this.isMouseDown && this.scrollMomentum.magnitude > 0.5f)
		{
			this.ScrollPosition += this.scrollMomentum;
			this.scrollMomentum *= 0.95f - BraveTime.DeltaTime;
		}
	}

	// Token: 0x06001C1A RID: 7194 RVA: 0x000847D8 File Offset: 0x000829D8
	public override void LateUpdate()
	{
		base.LateUpdate();
		this.initialize();
	}

	// Token: 0x06001C1B RID: 7195 RVA: 0x000847E8 File Offset: 0x000829E8
	protected internal void OnTextChanged()
	{
		this.Invalidate();
		base.Signal("OnTextChanged", this, this.text);
		if (this.TextChanged != null)
		{
			this.TextChanged(this, this.text);
		}
	}

	// Token: 0x06001C1C RID: 7196 RVA: 0x00084820 File Offset: 0x00082A20
	protected internal void OnScrollPositionChanged()
	{
		base.Invalidate();
		base.SignalHierarchy("OnScrollPositionChanged", new object[] { this, this.ScrollPosition });
		if (this.ScrollPositionChanged != null)
		{
			this.ScrollPositionChanged(this, this.ScrollPosition);
		}
	}

	// Token: 0x06001C1D RID: 7197 RVA: 0x00084874 File Offset: 0x00082A74
	protected internal override void OnKeyDown(dfKeyEventArgs args)
	{
		if (args.Used)
		{
			base.OnKeyDown(args);
			return;
		}
		int num = this.FontSize;
		int num2 = this.FontSize;
		switch (args.KeyCode)
		{
		case KeyCode.UpArrow:
			this.ScrollPosition += new Vector2(0f, (float)(-(float)num2));
			args.Use();
			break;
		case KeyCode.DownArrow:
			this.ScrollPosition += new Vector2(0f, (float)num2);
			args.Use();
			break;
		case KeyCode.RightArrow:
			this.ScrollPosition += new Vector2((float)num, 0f);
			args.Use();
			break;
		case KeyCode.LeftArrow:
			this.ScrollPosition += new Vector2((float)(-(float)num), 0f);
			args.Use();
			break;
		case KeyCode.Home:
			this.ScrollToTop();
			args.Use();
			break;
		case KeyCode.End:
			this.ScrollToBottom();
			args.Use();
			break;
		}
		base.OnKeyDown(args);
	}

	// Token: 0x06001C1E RID: 7198 RVA: 0x000849A4 File Offset: 0x00082BA4
	internal override void OnDragEnd(dfDragEventArgs args)
	{
		base.OnDragEnd(args);
		this.isMouseDown = false;
	}

	// Token: 0x06001C1F RID: 7199 RVA: 0x000849B4 File Offset: 0x00082BB4
	protected internal override void OnMouseEnter(dfMouseEventArgs args)
	{
		base.OnMouseEnter(args);
		this.touchStartPosition = args.Position;
	}

	// Token: 0x06001C20 RID: 7200 RVA: 0x000849CC File Offset: 0x00082BCC
	protected internal override void OnMouseDown(dfMouseEventArgs args)
	{
		base.OnMouseDown(args);
		this.mouseDownTag = this.hitTestTag(args);
		this.mouseDownScrollPosition = this.scrollPosition;
		this.scrollMomentum = Vector2.zero;
		this.touchStartPosition = args.Position;
		this.isMouseDown = true;
	}

	// Token: 0x06001C21 RID: 7201 RVA: 0x00084A0C File Offset: 0x00082C0C
	protected internal override void OnMouseUp(dfMouseEventArgs args)
	{
		base.OnMouseUp(args);
		this.isMouseDown = false;
		if (Vector2.Distance(this.scrollPosition, this.mouseDownScrollPosition) <= 2f && this.hitTestTag(args) == this.mouseDownTag)
		{
			dfMarkupTag dfMarkupTag = this.mouseDownTag;
			while (dfMarkupTag != null && !(dfMarkupTag is dfMarkupTagAnchor))
			{
				dfMarkupTag = dfMarkupTag.Parent as dfMarkupTag;
			}
			if (dfMarkupTag is dfMarkupTagAnchor)
			{
				base.Signal("OnLinkClicked", this, dfMarkupTag);
				if (this.LinkClicked != null)
				{
					this.LinkClicked(this, dfMarkupTag as dfMarkupTagAnchor);
				}
			}
		}
		this.mouseDownTag = null;
		this.mouseDownScrollPosition = this.scrollPosition;
	}

	// Token: 0x06001C22 RID: 7202 RVA: 0x00084AC8 File Offset: 0x00082CC8
	protected internal override void OnMouseMove(dfMouseEventArgs args)
	{
		base.OnMouseMove(args);
		if (!this.allowScrolling || this.autoHeight)
		{
			return;
		}
		bool flag = args is dfTouchEventArgs || this.isMouseDown;
		if (flag && (args.Position - this.touchStartPosition).magnitude > 5f)
		{
			Vector2 vector = args.MoveDelta.Scale(-1f, 1f);
			dfGUIManager manager = base.GetManager();
			Vector2 screenSize = manager.GetScreenSize();
			Camera camera = Camera.main ?? base.GetCamera();
			vector.x = screenSize.x * (vector.x / (float)camera.pixelWidth);
			vector.y = screenSize.y * (vector.y / (float)camera.pixelHeight);
			this.ScrollPosition += vector;
			this.scrollMomentum = (this.scrollMomentum + vector) * 0.5f;
		}
	}

	// Token: 0x06001C23 RID: 7203 RVA: 0x00084BD8 File Offset: 0x00082DD8
	protected internal override void OnMouseWheel(dfMouseEventArgs args)
	{
		try
		{
			if (!args.Used && this.allowScrolling && !this.autoHeight)
			{
				int num = ((!this.UseScrollMomentum) ? 3 : 1);
				float num2 = ((!(this.vertScrollbar != null)) ? ((float)(this.FontSize * num)) : this.vertScrollbar.IncrementAmount);
				this.ScrollPosition = new Vector2(this.scrollPosition.x, this.scrollPosition.y - num2 * args.WheelDelta);
				this.scrollMomentum = new Vector2(0f, -num2 * args.WheelDelta);
				args.Use();
				base.Signal("OnMouseWheel", this, args);
			}
		}
		finally
		{
			base.OnMouseWheel(args);
		}
	}

	// Token: 0x06001C24 RID: 7204 RVA: 0x00084CC0 File Offset: 0x00082EC0
	public void ScrollToTop()
	{
		this.ScrollPosition = new Vector2(this.scrollPosition.x, 0f);
	}

	// Token: 0x06001C25 RID: 7205 RVA: 0x00084CE0 File Offset: 0x00082EE0
	public void ScrollToBottom()
	{
		this.ScrollPosition = new Vector2(this.scrollPosition.x, 2.1474836E+09f);
	}

	// Token: 0x06001C26 RID: 7206 RVA: 0x00084D00 File Offset: 0x00082F00
	public void ScrollToLeft()
	{
		this.ScrollPosition = new Vector2(0f, this.scrollPosition.y);
	}

	// Token: 0x06001C27 RID: 7207 RVA: 0x00084D20 File Offset: 0x00082F20
	public void ScrollToRight()
	{
		this.ScrollPosition = new Vector2(2.1474836E+09f, this.scrollPosition.y);
	}

	// Token: 0x06001C28 RID: 7208 RVA: 0x00084D40 File Offset: 0x00082F40
	public dfList<dfRenderData> RenderMultiple()
	{
		if (!this.isVisible || this.Font == null)
		{
			return null;
		}
		Matrix4x4 localToWorldMatrix = base.transform.localToWorldMatrix;
		if (!this.isControlInvalidated && this.viewportBox != null)
		{
			for (int i = 0; i < this.buffers.Count; i++)
			{
				this.buffers[i].Transform = localToWorldMatrix;
			}
			return this.buffers;
		}
		dfList<dfRenderData> dfList;
		try
		{
			this.isControlInvalidated = false;
			if (this.isMarkupInvalidated)
			{
				this.isMarkupInvalidated = false;
				this.processMarkup();
			}
			this.viewportBox.FitToContents();
			if (this.autoHeight)
			{
				base.Height = (float)this.viewportBox.Height;
			}
			this.updateScrollbars();
			this.buffers.Clear();
			this.gatherRenderBuffers(this.viewportBox, this.buffers);
			dfList = this.buffers;
		}
		finally
		{
			base.updateCollider();
		}
		return dfList;
	}

	// Token: 0x06001C29 RID: 7209 RVA: 0x00084E50 File Offset: 0x00083050
	private dfMarkupTag hitTestTag(dfMouseEventArgs args)
	{
		Vector2 vector = base.GetHitPosition(args) + this.scrollPosition;
		dfMarkupBox dfMarkupBox = this.viewportBox.HitTest(vector);
		if (dfMarkupBox != null)
		{
			dfMarkupElement dfMarkupElement = dfMarkupBox.Element;
			while (dfMarkupElement != null && !(dfMarkupElement is dfMarkupTag))
			{
				dfMarkupElement = dfMarkupElement.Parent;
			}
			return dfMarkupElement as dfMarkupTag;
		}
		return null;
	}

	// Token: 0x06001C2A RID: 7210 RVA: 0x00084EB0 File Offset: 0x000830B0
	private void processMarkup()
	{
		this.releaseMarkupReferences();
		this.elements = dfMarkupParser.Parse(this, this.text);
		float textScaleMultiplier = this.getTextScaleMultiplier();
		int num = Mathf.CeilToInt((float)this.FontSize * textScaleMultiplier);
		int num2 = Mathf.CeilToInt((float)this.LineHeight * textScaleMultiplier);
		dfMarkupStyle dfMarkupStyle = new dfMarkupStyle
		{
			Host = this,
			Atlas = this.Atlas,
			Font = this.Font,
			FontSize = num,
			FontStyle = this.FontStyle,
			LineHeight = num2,
			Color = base.ApplyOpacity(base.Color),
			Opacity = base.CalculateOpacity(),
			Align = this.TextAlignment,
			PreserveWhitespace = this.preserveWhitespace
		};
		this.viewportBox = new dfMarkupBox(null, dfMarkupDisplayType.block, dfMarkupStyle)
		{
			Size = base.Size
		};
		for (int i = 0; i < this.elements.Count; i++)
		{
			dfMarkupElement dfMarkupElement = this.elements[i];
			if (dfMarkupElement != null)
			{
				dfMarkupElement.PerformLayout(this.viewportBox, dfMarkupStyle);
			}
		}
	}

	// Token: 0x06001C2B RID: 7211 RVA: 0x00084FE8 File Offset: 0x000831E8
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
		return base.Size.y / this.startSize.y;
	}

	// Token: 0x06001C2C RID: 7212 RVA: 0x0008504C File Offset: 0x0008324C
	private void releaseMarkupReferences()
	{
		this.mouseDownTag = null;
		if (this.viewportBox != null)
		{
			this.viewportBox.Release();
		}
		if (this.elements != null)
		{
			for (int i = 0; i < this.elements.Count; i++)
			{
				this.elements[i].Release();
			}
			this.elements.Release();
		}
	}

	// Token: 0x06001C2D RID: 7213 RVA: 0x000850BC File Offset: 0x000832BC
	[HideInInspector]
	private void initialize()
	{
		if (this.initialized)
		{
			return;
		}
		this.initialized = true;
		if (Application.isPlaying)
		{
			if (this.horzScrollbar != null)
			{
				this.horzScrollbar.ValueChanged += this.horzScroll_ValueChanged;
			}
			if (this.vertScrollbar != null)
			{
				this.vertScrollbar.ValueChanged += this.vertScroll_ValueChanged;
			}
		}
		this.Invalidate();
		this.ScrollPosition = Vector2.zero;
		this.updateScrollbars();
	}

	// Token: 0x06001C2E RID: 7214 RVA: 0x00085150 File Offset: 0x00083350
	private void vertScroll_ValueChanged(dfControl control, float value)
	{
		this.ScrollPosition = new Vector2(this.scrollPosition.x, value);
	}

	// Token: 0x06001C2F RID: 7215 RVA: 0x0008516C File Offset: 0x0008336C
	private void horzScroll_ValueChanged(dfControl control, float value)
	{
		this.ScrollPosition = new Vector2(value, this.ScrollPosition.y);
	}

	// Token: 0x06001C30 RID: 7216 RVA: 0x00085194 File Offset: 0x00083394
	private void updateScrollbars()
	{
		if (this.horzScrollbar != null)
		{
			this.horzScrollbar.MinValue = 0f;
			this.horzScrollbar.MaxValue = this.ContentSize.x;
			this.horzScrollbar.ScrollSize = base.Size.x;
			this.horzScrollbar.Value = this.ScrollPosition.x;
		}
		if (this.vertScrollbar != null)
		{
			this.vertScrollbar.MinValue = 0f;
			this.vertScrollbar.MaxValue = this.ContentSize.y;
			this.vertScrollbar.ScrollSize = base.Size.y;
			this.vertScrollbar.Value = this.ScrollPosition.y;
		}
	}

	// Token: 0x06001C31 RID: 7217 RVA: 0x0008527C File Offset: 0x0008347C
	private void gatherRenderBuffers(dfMarkupBox box, dfList<dfRenderData> buffers)
	{
		dfIntersectionType viewportIntersection = this.getViewportIntersection(box);
		if (viewportIntersection == dfIntersectionType.None)
		{
			return;
		}
		dfRenderData dfRenderData = box.Render();
		if (dfRenderData != null)
		{
			if (dfRenderData.Material == null && this.atlas != null)
			{
				dfRenderData.Material = this.atlas.Material;
			}
			float num = base.PixelsToUnits();
			Vector2 vector = -this.scrollPosition.Scale(1f, -1f).RoundToInt();
			Vector3 vector2 = vector + box.GetOffset().Scale(1f, -1f) + this.pivot.TransformToUpperLeft(base.Size);
			dfList<Vector3> vertices = dfRenderData.Vertices;
			for (int i = 0; i < dfRenderData.Vertices.Count; i++)
			{
				vertices[i] = (vector2 + vertices[i]) * num;
			}
			if (viewportIntersection == dfIntersectionType.Intersecting)
			{
				this.clipToViewport(dfRenderData);
			}
			dfRenderData.Transform = base.transform.localToWorldMatrix;
			buffers.Add(dfRenderData);
		}
		for (int j = 0; j < box.Children.Count; j++)
		{
			this.gatherRenderBuffers(box.Children[j], buffers);
		}
	}

	// Token: 0x06001C32 RID: 7218 RVA: 0x000853D8 File Offset: 0x000835D8
	private dfIntersectionType getViewportIntersection(dfMarkupBox box)
	{
		if (box.Display == dfMarkupDisplayType.none)
		{
			return dfIntersectionType.None;
		}
		Vector2 size = base.Size;
		Vector2 vector = box.GetOffset() - this.scrollPosition;
		Vector2 vector2 = vector + box.Size;
		if (vector2.x <= 0f || vector2.y <= 0f)
		{
			return dfIntersectionType.None;
		}
		if (vector.x >= size.x || vector.y >= size.y)
		{
			return dfIntersectionType.None;
		}
		if (vector.x < 0f || vector.y < 0f || vector2.x > size.x || vector2.y > size.y)
		{
			return dfIntersectionType.Intersecting;
		}
		return dfIntersectionType.Inside;
	}

	// Token: 0x06001C33 RID: 7219 RVA: 0x000854B4 File Offset: 0x000836B4
	private void clipToViewport(dfRenderData renderData)
	{
		Plane[] viewportClippingPlanes = this.getViewportClippingPlanes();
		Material material = renderData.Material;
		Matrix4x4 transform = renderData.Transform;
		dfRichTextLabel.clipBuffer.Clear();
		dfClippingUtil.Clip(viewportClippingPlanes, renderData, dfRichTextLabel.clipBuffer);
		renderData.Clear();
		renderData.Merge(dfRichTextLabel.clipBuffer, false);
		renderData.Material = material;
		renderData.Transform = transform;
	}

	// Token: 0x06001C34 RID: 7220 RVA: 0x0008550C File Offset: 0x0008370C
	private Plane[] getViewportClippingPlanes()
	{
		Vector3[] corners = base.GetCorners();
		Matrix4x4 worldToLocalMatrix = base.transform.worldToLocalMatrix;
		for (int i = 0; i < corners.Length; i++)
		{
			corners[i] = worldToLocalMatrix.MultiplyPoint(corners[i]);
		}
		this.cachedClippingPlanes[0] = new Plane(Vector3.right, corners[0]);
		this.cachedClippingPlanes[1] = new Plane(Vector3.left, corners[1]);
		this.cachedClippingPlanes[2] = new Plane(Vector3.up, corners[2]);
		this.cachedClippingPlanes[3] = new Plane(Vector3.down, corners[0]);
		return this.cachedClippingPlanes;
	}

	// Token: 0x06001C35 RID: 7221 RVA: 0x00085604 File Offset: 0x00083804
	public void UpdateFontInfo()
	{
		if (!dfFontManager.IsDirty(this.Font))
		{
			return;
		}
		if (string.IsNullOrEmpty(this.text))
		{
			return;
		}
		this.updateFontInfo(this.viewportBox);
	}

	// Token: 0x06001C36 RID: 7222 RVA: 0x00085634 File Offset: 0x00083834
	private void updateFontInfo(dfMarkupBox box)
	{
		if (box == null)
		{
			return;
		}
		if (box != this.viewportBox && this.getViewportIntersection(box) == dfIntersectionType.None)
		{
			return;
		}
		dfMarkupBoxText dfMarkupBoxText = box as dfMarkupBoxText;
		if (dfMarkupBoxText != null)
		{
			this.font.AddCharacterRequest(dfMarkupBoxText.Text, dfMarkupBoxText.Style.FontSize, dfMarkupBoxText.Style.FontStyle);
		}
		for (int i = 0; i < box.Children.Count; i++)
		{
			this.updateFontInfo(box.Children[i]);
		}
	}

	// Token: 0x06001C37 RID: 7223 RVA: 0x000856CC File Offset: 0x000838CC
	private void onFontTextureRebuilt(Font font)
	{
		if (this.Font != null && font == this.Font.BaseFont)
		{
			this.Invalidate();
			this.updateFontInfo(this.viewportBox);
		}
	}

	// Token: 0x06001C38 RID: 7224 RVA: 0x00085704 File Offset: 0x00083904
	private void bindTextureRebuildCallback()
	{
		if (this.isFontCallbackAssigned || this.Font == null)
		{
			return;
		}
		UnityEngine.Font.textureRebuilt += this.onFontTextureRebuilt;
		this.isFontCallbackAssigned = true;
	}

	// Token: 0x06001C39 RID: 7225 RVA: 0x0008573C File Offset: 0x0008393C
	private void unbindTextureRebuildCallback()
	{
		if (!this.isFontCallbackAssigned || this.Font == null)
		{
			return;
		}
		UnityEngine.Font.textureRebuilt -= this.onFontTextureRebuilt;
		this.isFontCallbackAssigned = false;
	}

	// Token: 0x040015C5 RID: 5573
	[SerializeField]
	protected dfAtlas atlas;

	// Token: 0x040015C6 RID: 5574
	[SerializeField]
	protected dfDynamicFont font;

	// Token: 0x040015C7 RID: 5575
	[SerializeField]
	protected string text = "Rich Text Label";

	// Token: 0x040015C8 RID: 5576
	[SerializeField]
	protected int fontSize = 16;

	// Token: 0x040015C9 RID: 5577
	[SerializeField]
	protected int lineheight = 16;

	// Token: 0x040015CA RID: 5578
	[SerializeField]
	protected dfTextScaleMode textScaleMode;

	// Token: 0x040015CB RID: 5579
	[SerializeField]
	protected FontStyle fontStyle;

	// Token: 0x040015CC RID: 5580
	[SerializeField]
	protected bool preserveWhitespace;

	// Token: 0x040015CD RID: 5581
	[SerializeField]
	protected string blankTextureSprite;

	// Token: 0x040015CE RID: 5582
	[SerializeField]
	protected dfMarkupTextAlign align;

	// Token: 0x040015CF RID: 5583
	[SerializeField]
	protected bool allowScrolling;

	// Token: 0x040015D0 RID: 5584
	[SerializeField]
	protected dfScrollbar horzScrollbar;

	// Token: 0x040015D1 RID: 5585
	[SerializeField]
	protected dfScrollbar vertScrollbar;

	// Token: 0x040015D2 RID: 5586
	[SerializeField]
	protected bool useScrollMomentum;

	// Token: 0x040015D3 RID: 5587
	[SerializeField]
	protected bool autoHeight;

	// Token: 0x040015D4 RID: 5588
	private static dfRenderData clipBuffer = new dfRenderData();

	// Token: 0x040015D5 RID: 5589
	private dfList<dfRenderData> buffers = new dfList<dfRenderData>();

	// Token: 0x040015D6 RID: 5590
	private dfList<dfMarkupElement> elements;

	// Token: 0x040015D7 RID: 5591
	private dfMarkupBox viewportBox;

	// Token: 0x040015D8 RID: 5592
	private dfMarkupTag mouseDownTag;

	// Token: 0x040015D9 RID: 5593
	private Vector2 mouseDownScrollPosition = Vector2.zero;

	// Token: 0x040015DA RID: 5594
	private Vector2 scrollPosition = Vector2.zero;

	// Token: 0x040015DB RID: 5595
	private bool initialized;

	// Token: 0x040015DC RID: 5596
	private bool isMouseDown;

	// Token: 0x040015DD RID: 5597
	private Vector2 touchStartPosition = Vector2.zero;

	// Token: 0x040015DE RID: 5598
	private Vector2 scrollMomentum = Vector2.zero;

	// Token: 0x040015DF RID: 5599
	private bool isMarkupInvalidated = true;

	// Token: 0x040015E0 RID: 5600
	private Vector2 startSize = Vector2.zero;

	// Token: 0x040015E1 RID: 5601
	private bool isFontCallbackAssigned;

	// Token: 0x020004B6 RID: 1206
	// (Invoke) Token: 0x06001C3C RID: 7228
	[dfEventCategory("Markup")]
	public delegate void LinkClickEventHandler(dfRichTextLabel sender, dfMarkupTagAnchor tag);
}
