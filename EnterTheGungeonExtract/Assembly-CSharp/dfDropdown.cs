using System;
using System.Diagnostics;
using UnityEngine;

// Token: 0x0200039C RID: 924
[dfTooltip("Implements a drop-down list control")]
[dfHelp("http://www.daikonforge.com/docs/df-gui/classdf_dropdown.html")]
[ExecuteInEditMode]
[dfCategory("Basic Controls")]
[AddComponentMenu("Daikon Forge/User Interface/Dropdown List")]
[Serializable]
public class dfDropdown : dfInteractiveBase, IDFMultiRender, IRendersText
{
	// Token: 0x14000031 RID: 49
	// (add) Token: 0x06001163 RID: 4451 RVA: 0x000513D8 File Offset: 0x0004F5D8
	// (remove) Token: 0x06001164 RID: 4452 RVA: 0x00051410 File Offset: 0x0004F610
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event dfDropdown.PopupEventHandler DropdownOpen;

	// Token: 0x14000032 RID: 50
	// (add) Token: 0x06001165 RID: 4453 RVA: 0x00051448 File Offset: 0x0004F648
	// (remove) Token: 0x06001166 RID: 4454 RVA: 0x00051480 File Offset: 0x0004F680
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event dfDropdown.PopupEventHandler DropdownClose;

	// Token: 0x14000033 RID: 51
	// (add) Token: 0x06001167 RID: 4455 RVA: 0x000514B8 File Offset: 0x0004F6B8
	// (remove) Token: 0x06001168 RID: 4456 RVA: 0x000514F0 File Offset: 0x0004F6F0
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event PropertyChangedEventHandler<int> SelectedIndexChanged;

	// Token: 0x170003B4 RID: 948
	// (get) Token: 0x06001169 RID: 4457 RVA: 0x00051528 File Offset: 0x0004F728
	// (set) Token: 0x0600116A RID: 4458 RVA: 0x00051530 File Offset: 0x0004F730
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

	// Token: 0x170003B5 RID: 949
	// (get) Token: 0x0600116B RID: 4459 RVA: 0x0005153C File Offset: 0x0004F73C
	// (set) Token: 0x0600116C RID: 4460 RVA: 0x00051580 File Offset: 0x0004F780
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
				this.ClosePopup();
				this.unbindTextureRebuildCallback();
				this.font = value;
				this.bindTextureRebuildCallback();
				this.Invalidate();
			}
		}
	}

	// Token: 0x170003B6 RID: 950
	// (get) Token: 0x0600116D RID: 4461 RVA: 0x000515B4 File Offset: 0x0004F7B4
	// (set) Token: 0x0600116E RID: 4462 RVA: 0x000515BC File Offset: 0x0004F7BC
	public dfScrollbar ListScrollbar
	{
		get
		{
			return this.listScrollbar;
		}
		set
		{
			if (value != this.listScrollbar)
			{
				this.listScrollbar = value;
				this.Invalidate();
			}
		}
	}

	// Token: 0x170003B7 RID: 951
	// (get) Token: 0x0600116F RID: 4463 RVA: 0x000515DC File Offset: 0x0004F7DC
	// (set) Token: 0x06001170 RID: 4464 RVA: 0x000515E4 File Offset: 0x0004F7E4
	public Vector2 ListOffset
	{
		get
		{
			return this.listOffset;
		}
		set
		{
			if (Vector2.Distance(this.listOffset, value) > 1f)
			{
				this.listOffset = value;
				this.Invalidate();
			}
		}
	}

	// Token: 0x170003B8 RID: 952
	// (get) Token: 0x06001171 RID: 4465 RVA: 0x0005160C File Offset: 0x0004F80C
	// (set) Token: 0x06001172 RID: 4466 RVA: 0x00051614 File Offset: 0x0004F814
	public string ListBackground
	{
		get
		{
			return this.listBackground;
		}
		set
		{
			if (value != this.listBackground)
			{
				this.ClosePopup();
				this.listBackground = value;
				this.Invalidate();
			}
		}
	}

	// Token: 0x170003B9 RID: 953
	// (get) Token: 0x06001173 RID: 4467 RVA: 0x0005163C File Offset: 0x0004F83C
	// (set) Token: 0x06001174 RID: 4468 RVA: 0x00051644 File Offset: 0x0004F844
	public string ItemHover
	{
		get
		{
			return this.itemHover;
		}
		set
		{
			if (value != this.itemHover)
			{
				this.itemHover = value;
				this.Invalidate();
			}
		}
	}

	// Token: 0x170003BA RID: 954
	// (get) Token: 0x06001175 RID: 4469 RVA: 0x00051664 File Offset: 0x0004F864
	// (set) Token: 0x06001176 RID: 4470 RVA: 0x0005166C File Offset: 0x0004F86C
	public string ItemHighlight
	{
		get
		{
			return this.itemHighlight;
		}
		set
		{
			if (value != this.itemHighlight)
			{
				this.ClosePopup();
				this.itemHighlight = value;
				this.Invalidate();
			}
		}
	}

	// Token: 0x170003BB RID: 955
	// (get) Token: 0x06001177 RID: 4471 RVA: 0x00051694 File Offset: 0x0004F894
	// (set) Token: 0x06001178 RID: 4472 RVA: 0x000516A4 File Offset: 0x0004F8A4
	public string SelectedValue
	{
		get
		{
			return this.items[this.selectedIndex];
		}
		set
		{
			this.selectedIndex = -1;
			for (int i = 0; i < this.items.Length; i++)
			{
				if (this.items[i] == value)
				{
					this.selectedIndex = i;
					break;
				}
			}
			this.Invalidate();
		}
	}

	// Token: 0x170003BC RID: 956
	// (get) Token: 0x06001179 RID: 4473 RVA: 0x000516F8 File Offset: 0x0004F8F8
	// (set) Token: 0x0600117A RID: 4474 RVA: 0x00051700 File Offset: 0x0004F900
	public int SelectedIndex
	{
		get
		{
			return this.selectedIndex;
		}
		set
		{
			value = Mathf.Max(-1, value);
			value = Mathf.Min(this.items.Length - 1, value);
			if (value != this.selectedIndex)
			{
				if (this.popup != null)
				{
					this.popup.SelectedIndex = value;
				}
				this.selectedIndex = value;
				this.OnSelectedIndexChanged();
				this.Invalidate();
			}
		}
	}

	// Token: 0x170003BD RID: 957
	// (get) Token: 0x0600117B RID: 4475 RVA: 0x00051764 File Offset: 0x0004F964
	// (set) Token: 0x0600117C RID: 4476 RVA: 0x00051784 File Offset: 0x0004F984
	public RectOffset TextFieldPadding
	{
		get
		{
			if (this.textFieldPadding == null)
			{
				this.textFieldPadding = new RectOffset();
			}
			return this.textFieldPadding;
		}
		set
		{
			value = value.ConstrainPadding();
			if (!object.Equals(value, this.textFieldPadding))
			{
				this.textFieldPadding = value;
				this.Invalidate();
			}
		}
	}

	// Token: 0x170003BE RID: 958
	// (get) Token: 0x0600117D RID: 4477 RVA: 0x000517AC File Offset: 0x0004F9AC
	// (set) Token: 0x0600117E RID: 4478 RVA: 0x000517B4 File Offset: 0x0004F9B4
	public Color32 TextColor
	{
		get
		{
			return this.textColor;
		}
		set
		{
			this.ClosePopup();
			this.textColor = value;
			this.Invalidate();
		}
	}

	// Token: 0x170003BF RID: 959
	// (get) Token: 0x0600117F RID: 4479 RVA: 0x000517CC File Offset: 0x0004F9CC
	// (set) Token: 0x06001180 RID: 4480 RVA: 0x000517D4 File Offset: 0x0004F9D4
	public Color32 DisabledTextColor
	{
		get
		{
			return this.disabledTextColor;
		}
		set
		{
			this.ClosePopup();
			this.disabledTextColor = value;
			this.Invalidate();
		}
	}

	// Token: 0x170003C0 RID: 960
	// (get) Token: 0x06001181 RID: 4481 RVA: 0x000517EC File Offset: 0x0004F9EC
	// (set) Token: 0x06001182 RID: 4482 RVA: 0x000517F4 File Offset: 0x0004F9F4
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
				this.ClosePopup();
				dfFontManager.Invalidate(this.Font);
				this.textScale = value;
				this.Invalidate();
			}
		}
	}

	// Token: 0x170003C1 RID: 961
	// (get) Token: 0x06001183 RID: 4483 RVA: 0x00051834 File Offset: 0x0004FA34
	// (set) Token: 0x06001184 RID: 4484 RVA: 0x0005183C File Offset: 0x0004FA3C
	public int ItemHeight
	{
		get
		{
			return this.itemHeight;
		}
		set
		{
			value = Mathf.Max(1, value);
			if (value != this.itemHeight)
			{
				this.ClosePopup();
				this.itemHeight = value;
				this.Invalidate();
			}
		}
	}

	// Token: 0x170003C2 RID: 962
	// (get) Token: 0x06001185 RID: 4485 RVA: 0x00051868 File Offset: 0x0004FA68
	// (set) Token: 0x06001186 RID: 4486 RVA: 0x00051888 File Offset: 0x0004FA88
	public string[] Items
	{
		get
		{
			if (this.items == null)
			{
				this.items = new string[0];
			}
			return this.items;
		}
		set
		{
			this.ClosePopup();
			if (value == null)
			{
				value = new string[0];
			}
			this.items = value;
			this.Invalidate();
		}
	}

	// Token: 0x170003C3 RID: 963
	// (get) Token: 0x06001187 RID: 4487 RVA: 0x000518AC File Offset: 0x0004FAAC
	// (set) Token: 0x06001188 RID: 4488 RVA: 0x000518CC File Offset: 0x0004FACC
	public RectOffset ListPadding
	{
		get
		{
			if (this.listPadding == null)
			{
				this.listPadding = new RectOffset();
			}
			return this.listPadding;
		}
		set
		{
			value = value.ConstrainPadding();
			if (!object.Equals(value, this.listPadding))
			{
				this.listPadding = value;
				this.Invalidate();
			}
		}
	}

	// Token: 0x170003C4 RID: 964
	// (get) Token: 0x06001189 RID: 4489 RVA: 0x000518F4 File Offset: 0x0004FAF4
	// (set) Token: 0x0600118A RID: 4490 RVA: 0x000518FC File Offset: 0x0004FAFC
	public dfDropdown.PopupListPosition ListPosition
	{
		get
		{
			return this.listPosition;
		}
		set
		{
			if (value != this.ListPosition)
			{
				this.ClosePopup();
				this.listPosition = value;
				this.Invalidate();
			}
		}
	}

	// Token: 0x170003C5 RID: 965
	// (get) Token: 0x0600118B RID: 4491 RVA: 0x00051920 File Offset: 0x0004FB20
	// (set) Token: 0x0600118C RID: 4492 RVA: 0x00051928 File Offset: 0x0004FB28
	public int MaxListWidth
	{
		get
		{
			return this.listWidth;
		}
		set
		{
			this.listWidth = value;
		}
	}

	// Token: 0x170003C6 RID: 966
	// (get) Token: 0x0600118D RID: 4493 RVA: 0x00051934 File Offset: 0x0004FB34
	// (set) Token: 0x0600118E RID: 4494 RVA: 0x0005193C File Offset: 0x0004FB3C
	public int MaxListHeight
	{
		get
		{
			return this.listHeight;
		}
		set
		{
			this.listHeight = value;
			this.Invalidate();
		}
	}

	// Token: 0x170003C7 RID: 967
	// (get) Token: 0x0600118F RID: 4495 RVA: 0x0005194C File Offset: 0x0004FB4C
	// (set) Token: 0x06001190 RID: 4496 RVA: 0x00051954 File Offset: 0x0004FB54
	public dfControl TriggerButton
	{
		get
		{
			return this.triggerButton;
		}
		set
		{
			if (value != this.triggerButton)
			{
				this.detachChildEvents();
				this.triggerButton = value;
				this.attachChildEvents();
				this.Invalidate();
			}
		}
	}

	// Token: 0x170003C8 RID: 968
	// (get) Token: 0x06001191 RID: 4497 RVA: 0x00051980 File Offset: 0x0004FB80
	// (set) Token: 0x06001192 RID: 4498 RVA: 0x00051988 File Offset: 0x0004FB88
	public bool OpenOnMouseDown
	{
		get
		{
			return this.openOnMouseDown;
		}
		set
		{
			this.openOnMouseDown = value;
		}
	}

	// Token: 0x170003C9 RID: 969
	// (get) Token: 0x06001193 RID: 4499 RVA: 0x00051994 File Offset: 0x0004FB94
	// (set) Token: 0x06001194 RID: 4500 RVA: 0x0005199C File Offset: 0x0004FB9C
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

	// Token: 0x170003CA RID: 970
	// (get) Token: 0x06001195 RID: 4501 RVA: 0x000519B8 File Offset: 0x0004FBB8
	// (set) Token: 0x06001196 RID: 4502 RVA: 0x000519C0 File Offset: 0x0004FBC0
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

	// Token: 0x170003CB RID: 971
	// (get) Token: 0x06001197 RID: 4503 RVA: 0x000519EC File Offset: 0x0004FBEC
	// (set) Token: 0x06001198 RID: 4504 RVA: 0x000519F4 File Offset: 0x0004FBF4
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

	// Token: 0x06001199 RID: 4505 RVA: 0x00051A14 File Offset: 0x0004FC14
	protected internal override void OnMouseWheel(dfMouseEventArgs args)
	{
		this.SelectedIndex = Mathf.Max(0, this.SelectedIndex - Mathf.RoundToInt(args.WheelDelta));
		args.Use();
		base.OnMouseWheel(args);
	}

	// Token: 0x0600119A RID: 4506 RVA: 0x00051A44 File Offset: 0x0004FC44
	protected internal override void OnMouseDown(dfMouseEventArgs args)
	{
		if (this.openOnMouseDown && !args.Used && args.Buttons == dfMouseButtons.Left && args.Source == this)
		{
			args.Use();
			base.OnMouseDown(args);
			if (this.popup == null)
			{
				this.OpenPopup();
			}
			else
			{
				this.ClosePopup();
			}
		}
		else
		{
			base.OnMouseDown(args);
		}
	}

	// Token: 0x0600119B RID: 4507 RVA: 0x00051AC0 File Offset: 0x0004FCC0
	protected internal override void OnKeyDown(dfKeyEventArgs args)
	{
		KeyCode keyCode = args.KeyCode;
		switch (keyCode)
		{
		case KeyCode.UpArrow:
			this.SelectedIndex = Mathf.Max(0, this.selectedIndex - 1);
			break;
		case KeyCode.DownArrow:
			this.SelectedIndex = Mathf.Min(this.items.Length - 1, this.selectedIndex + 1);
			break;
		default:
			if (keyCode == KeyCode.Return || keyCode == KeyCode.Space)
			{
				if (this.ClickWhenSpacePressed && this.IsInteractive)
				{
					this.OpenPopup();
				}
			}
			break;
		case KeyCode.Home:
			this.SelectedIndex = 0;
			break;
		case KeyCode.End:
			this.SelectedIndex = this.items.Length - 1;
			break;
		}
		base.OnKeyDown(args);
	}

	// Token: 0x0600119C RID: 4508 RVA: 0x00051B98 File Offset: 0x0004FD98
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

	// Token: 0x0600119D RID: 4509 RVA: 0x00051BF4 File Offset: 0x0004FDF4
	public override void OnDisable()
	{
		base.OnDisable();
		this.unbindTextureRebuildCallback();
		this.ClosePopup(false);
	}

	// Token: 0x0600119E RID: 4510 RVA: 0x00051C0C File Offset: 0x0004FE0C
	public override void OnDestroy()
	{
		base.OnDestroy();
		this.ClosePopup(false);
		this.detachChildEvents();
	}

	// Token: 0x0600119F RID: 4511 RVA: 0x00051C24 File Offset: 0x0004FE24
	public override void Update()
	{
		base.Update();
		this.checkForPopupClose();
	}

	// Token: 0x060011A0 RID: 4512 RVA: 0x00051C34 File Offset: 0x0004FE34
	private void checkForPopupClose()
	{
		if (this.popup == null || !Input.GetMouseButtonDown(0))
		{
			return;
		}
		Camera camera = base.GetCamera();
		Ray ray = camera.ScreenPointToRay(Input.mousePosition);
		RaycastHit raycastHit;
		if (this.triggerButton != null && this.triggerButton.GetComponent<Collider>().Raycast(ray, out raycastHit, camera.farClipPlane))
		{
			return;
		}
		if (this.popup.GetComponent<Collider>().Raycast(ray, out raycastHit, camera.farClipPlane))
		{
			return;
		}
		if (this.popup.Scrollbar != null && this.popup.Scrollbar.GetComponent<Collider>().Raycast(ray, out raycastHit, camera.farClipPlane))
		{
			return;
		}
		if (base.GetComponent<Collider>().Raycast(ray, out raycastHit, camera.farClipPlane))
		{
			return;
		}
		this.ClosePopup();
	}

	// Token: 0x060011A1 RID: 4513 RVA: 0x00051D1C File Offset: 0x0004FF1C
	public override void LateUpdate()
	{
		base.LateUpdate();
		if (!Application.isPlaying)
		{
			return;
		}
		if (!this.eventsAttached)
		{
			this.attachChildEvents();
		}
		if (this.popup != null && !this.popup.ContainsFocus)
		{
			this.ClosePopup();
		}
	}

	// Token: 0x060011A2 RID: 4514 RVA: 0x00051D74 File Offset: 0x0004FF74
	private void attachChildEvents()
	{
		if (this.triggerButton != null && !this.eventsAttached)
		{
			this.eventsAttached = true;
			this.triggerButton.Click += this.trigger_Click;
		}
	}

	// Token: 0x060011A3 RID: 4515 RVA: 0x00051DB0 File Offset: 0x0004FFB0
	private void detachChildEvents()
	{
		if (this.triggerButton != null && this.eventsAttached)
		{
			this.triggerButton.Click -= this.trigger_Click;
			this.eventsAttached = false;
		}
	}

	// Token: 0x060011A4 RID: 4516 RVA: 0x00051DEC File Offset: 0x0004FFEC
	private void trigger_Click(dfControl control, dfMouseEventArgs mouseEvent)
	{
		if (mouseEvent.Source == this.triggerButton && !mouseEvent.Used)
		{
			mouseEvent.Use();
			if (this.popup == null)
			{
				this.OpenPopup();
			}
			else
			{
				this.ClosePopup();
			}
		}
	}

	// Token: 0x060011A5 RID: 4517 RVA: 0x00051E44 File Offset: 0x00050044
	protected internal virtual void OnSelectedIndexChanged()
	{
		base.SignalHierarchy("OnSelectedIndexChanged", new object[] { this, this.selectedIndex });
		if (this.SelectedIndexChanged != null)
		{
			this.SelectedIndexChanged(this, this.selectedIndex);
		}
	}

	// Token: 0x060011A6 RID: 4518 RVA: 0x00051E94 File Offset: 0x00050094
	protected internal override void OnLocalize()
	{
		base.OnLocalize();
		bool flag = false;
		for (int i = 0; i < this.items.Length; i++)
		{
			string localizedValue = base.getLocalizedValue(this.items[i]);
			if (localizedValue != this.items[i])
			{
				flag = true;
				this.items[i] = localizedValue;
			}
		}
		if (flag)
		{
			this.Invalidate();
		}
	}

	// Token: 0x060011A7 RID: 4519 RVA: 0x00051EFC File Offset: 0x000500FC
	private void renderText(dfRenderData buffer)
	{
		if (this.selectedIndex < 0 || this.selectedIndex >= this.items.Length)
		{
			return;
		}
		string text = this.items[this.selectedIndex];
		float num = base.PixelsToUnits();
		Vector2 vector = new Vector2(this.size.x - (float)this.textFieldPadding.horizontal, this.size.y - (float)this.textFieldPadding.vertical);
		Vector3 vector2 = this.pivot.TransformToUpperLeft(base.Size);
		Vector3 vector3 = new Vector3(vector2.x + (float)this.textFieldPadding.left, vector2.y - (float)this.textFieldPadding.top, 0f) * num;
		Color32 color = ((!base.IsEnabled) ? this.DisabledTextColor : this.TextColor);
		using (dfFontRendererBase dfFontRendererBase = this.font.ObtainRenderer())
		{
			dfFontRendererBase.WordWrap = false;
			dfFontRendererBase.MaxSize = vector;
			dfFontRendererBase.PixelRatio = num;
			dfFontRendererBase.TextScale = this.TextScale;
			dfFontRendererBase.VectorOffset = vector3;
			dfFontRendererBase.MultiLine = false;
			dfFontRendererBase.TextAlign = TextAlignment.Left;
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
				dynamicFontRenderer.SpriteBuffer = buffer;
			}
			dfFontRendererBase.Render(text, buffer);
		}
	}

	// Token: 0x060011A8 RID: 4520 RVA: 0x000520CC File Offset: 0x000502CC
	public void AddItem(string item)
	{
		string[] array = new string[this.items.Length + 1];
		Array.Copy(this.items, array, this.items.Length);
		array[this.items.Length] = item;
		this.items = array;
	}

	// Token: 0x060011A9 RID: 4521 RVA: 0x00052110 File Offset: 0x00050310
	public void OpenPopup()
	{
		if (this.popup != null || this.items.Length == 0)
		{
			return;
		}
		Vector2 vector = this.calculatePopupSize();
		this.popup = base.GetManager().AddControl<dfListbox>();
		this.popup.name = base.name + " - Dropdown List";
		this.popup.gameObject.hideFlags = HideFlags.DontSave;
		this.popup.Atlas = base.Atlas;
		this.popup.Anchor = dfAnchorStyle.Top | dfAnchorStyle.Left;
		this.popup.Color = base.Color;
		this.popup.Font = this.Font;
		this.popup.Pivot = dfPivotPoint.TopLeft;
		this.popup.Size = vector;
		this.popup.Font = this.Font;
		this.popup.ItemHeight = this.ItemHeight;
		this.popup.ItemHighlight = this.ItemHighlight;
		this.popup.ItemHover = this.ItemHover;
		this.popup.ItemPadding = this.TextFieldPadding;
		this.popup.ItemTextColor = this.TextColor;
		this.popup.ItemTextScale = this.TextScale;
		this.popup.Items = this.Items;
		this.popup.ListPadding = this.ListPadding;
		this.popup.BackgroundSprite = this.ListBackground;
		this.popup.Shadow = this.Shadow;
		this.popup.ShadowColor = this.ShadowColor;
		this.popup.ShadowOffset = this.ShadowOffset;
		this.popup.BringToFront();
		if (dfGUIManager.GetModalControl() != null)
		{
			dfGUIManager.PushModal(this.popup);
		}
		if (vector.y >= (float)this.MaxListHeight && this.listScrollbar != null)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.listScrollbar.gameObject);
			dfScrollbar activeScrollbar = gameObject.GetComponent<dfScrollbar>();
			float num = base.PixelsToUnits();
			Vector3 vector2 = this.popup.transform.TransformDirection(Vector3.right);
			Vector3 vector3 = this.popup.transform.position + vector2 * (vector.x - activeScrollbar.Width) * num;
			this.popup.AddControl(activeScrollbar);
			this.popup.Width -= activeScrollbar.Width;
			this.popup.Scrollbar = activeScrollbar;
			this.popup.SizeChanged += delegate(dfControl control, Vector2 size)
			{
				activeScrollbar.Height = control.Height;
			};
			activeScrollbar.transform.parent = this.popup.transform;
			activeScrollbar.transform.position = vector3;
			activeScrollbar.Anchor = dfAnchorStyle.Top | dfAnchorStyle.Bottom;
			activeScrollbar.Height = this.popup.Height;
		}
		Vector3 vector4 = this.calculatePopupPosition((int)this.popup.Size.y);
		this.popup.transform.position = vector4;
		this.popup.transform.rotation = base.transform.rotation;
		this.popup.SelectedIndexChanged += this.popup_SelectedIndexChanged;
		this.popup.LeaveFocus += this.popup_LostFocus;
		this.popup.ItemClicked += this.popup_ItemClicked;
		this.popup.KeyDown += this.popup_KeyDown;
		this.popup.SelectedIndex = Mathf.Max(0, this.SelectedIndex);
		this.popup.EnsureVisible(this.popup.SelectedIndex);
		this.popup.Focus(true);
		if (this.DropdownOpen != null)
		{
			bool flag = false;
			this.DropdownOpen(this, this.popup, ref flag);
		}
		base.Signal("OnDropdownOpen", this, this.popup);
	}

	// Token: 0x060011AA RID: 4522 RVA: 0x00052530 File Offset: 0x00050730
	public void ClosePopup()
	{
		this.ClosePopup(true);
	}

	// Token: 0x060011AB RID: 4523 RVA: 0x0005253C File Offset: 0x0005073C
	public void ClosePopup(bool allowOverride)
	{
		if (this.popup == null)
		{
			return;
		}
		if (dfGUIManager.GetModalControl() == this.popup)
		{
			dfGUIManager.PopModal();
		}
		this.popup.LostFocus -= this.popup_LostFocus;
		this.popup.SelectedIndexChanged -= this.popup_SelectedIndexChanged;
		this.popup.ItemClicked -= this.popup_ItemClicked;
		this.popup.KeyDown -= this.popup_KeyDown;
		if (!allowOverride)
		{
			UnityEngine.Object.Destroy(this.popup.gameObject);
			this.popup = null;
			return;
		}
		bool flag = false;
		if (this.DropdownClose != null)
		{
			this.DropdownClose(this, this.popup, ref flag);
		}
		if (!flag)
		{
			base.Signal("OnDropdownClose", this, this.popup);
		}
		if (!flag)
		{
			UnityEngine.Object.Destroy(this.popup.gameObject);
		}
		this.popup = null;
	}

	// Token: 0x060011AC RID: 4524 RVA: 0x00052648 File Offset: 0x00050848
	private Vector3 calculatePopupPosition(int height)
	{
		float num = base.PixelsToUnits();
		Vector3 vector = base.transform.position + this.pivot.TransformToUpperLeft(this.size) * num;
		Vector3 scaledDirection = base.getScaledDirection(Vector3.down);
		Vector3 vector2 = base.transformOffset(this.listOffset);
		Vector3 vector3 = vector + (vector2 + scaledDirection * base.Size.y) * num;
		Vector3 vector4 = vector + (vector2 - scaledDirection * this.popup.Size.y) * num;
		if (this.listPosition == dfDropdown.PopupListPosition.Above)
		{
			return vector4;
		}
		if (this.listPosition == dfDropdown.PopupListPosition.Below)
		{
			return vector3;
		}
		Vector2 screenSize = base.GetManager().GetScreenSize();
		float num2 = base.GetAbsolutePosition().y + base.Height + (float)height;
		if (num2 >= screenSize.y)
		{
			return vector4;
		}
		return vector3;
	}

	// Token: 0x060011AD RID: 4525 RVA: 0x00052754 File Offset: 0x00050954
	private Vector2 calculatePopupSize()
	{
		float num = ((this.MaxListWidth <= 0) ? this.size.x : ((float)this.MaxListWidth));
		int num2 = this.items.Length * this.itemHeight + this.listPadding.vertical;
		if (this.items.Length == 0)
		{
			num2 = this.itemHeight / 2 + this.listPadding.vertical;
		}
		return new Vector2(num, (float)Mathf.Min(this.MaxListHeight, num2));
	}

	// Token: 0x060011AE RID: 4526 RVA: 0x000527D8 File Offset: 0x000509D8
	private void popup_KeyDown(dfControl control, dfKeyEventArgs args)
	{
		if (args.KeyCode == KeyCode.Escape || args.KeyCode == KeyCode.Return)
		{
			this.ClosePopup();
			base.Focus(true);
		}
	}

	// Token: 0x060011AF RID: 4527 RVA: 0x00052804 File Offset: 0x00050A04
	private void popup_ItemClicked(dfControl control, int selectedIndex)
	{
		base.Focus(true);
	}

	// Token: 0x060011B0 RID: 4528 RVA: 0x00052810 File Offset: 0x00050A10
	private void popup_LostFocus(dfControl control, dfFocusEventArgs args)
	{
		if (this.popup != null && !this.popup.ContainsFocus)
		{
			this.ClosePopup();
		}
	}

	// Token: 0x060011B1 RID: 4529 RVA: 0x0005283C File Offset: 0x00050A3C
	private void popup_SelectedIndexChanged(dfControl control, int selectedIndex)
	{
		this.SelectedIndex = selectedIndex;
		this.Invalidate();
	}

	// Token: 0x060011B2 RID: 4530 RVA: 0x0005284C File Offset: 0x00050A4C
	public dfList<dfRenderData> RenderMultiple()
	{
		if (base.Atlas == null || this.Font == null)
		{
			return null;
		}
		if (!this.isVisible)
		{
			return null;
		}
		if (this.renderData == null)
		{
			this.renderData = dfRenderData.Obtain();
			this.textRenderData = dfRenderData.Obtain();
			this.isControlInvalidated = true;
		}
		if (!this.isControlInvalidated)
		{
			for (int i = 0; i < this.buffers.Count; i++)
			{
				this.buffers[i].Transform = base.transform.localToWorldMatrix;
			}
			return this.buffers;
		}
		this.buffers.Clear();
		this.renderData.Clear();
		this.renderData.Material = base.Atlas.Material;
		this.renderData.Transform = base.transform.localToWorldMatrix;
		this.buffers.Add(this.renderData);
		this.textRenderData.Clear();
		this.textRenderData.Material = base.Atlas.Material;
		this.textRenderData.Transform = base.transform.localToWorldMatrix;
		this.buffers.Add(this.textRenderData);
		this.renderBackground();
		this.renderText(this.textRenderData);
		this.isControlInvalidated = false;
		base.updateCollider();
		return this.buffers;
	}

	// Token: 0x060011B3 RID: 4531 RVA: 0x000529BC File Offset: 0x00050BBC
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

	// Token: 0x060011B4 RID: 4532 RVA: 0x00052A10 File Offset: 0x00050C10
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

	// Token: 0x060011B5 RID: 4533 RVA: 0x00052A64 File Offset: 0x00050C64
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
		string selectedValue = this.SelectedValue;
		if (string.IsNullOrEmpty(selectedValue))
		{
			return;
		}
		float num = this.TextScale;
		int num2 = Mathf.CeilToInt((float)this.font.FontSize * num);
		dfDynamicFont.AddCharacterRequest(selectedValue, num2, FontStyle.Normal);
	}

	// Token: 0x060011B6 RID: 4534 RVA: 0x00052AD4 File Offset: 0x00050CD4
	private void onFontTextureRebuilt(Font font)
	{
		if (this.Font is dfDynamicFont && font == (this.Font as dfDynamicFont).BaseFont)
		{
			this.requestCharacterInfo();
			this.Invalidate();
		}
	}

	// Token: 0x060011B7 RID: 4535 RVA: 0x00052B10 File Offset: 0x00050D10
	public void UpdateFontInfo()
	{
		this.requestCharacterInfo();
	}

	// Token: 0x04000F96 RID: 3990
	[SerializeField]
	protected dfFontBase font;

	// Token: 0x04000F97 RID: 3991
	[SerializeField]
	protected int selectedIndex = -1;

	// Token: 0x04000F98 RID: 3992
	[SerializeField]
	protected dfControl triggerButton;

	// Token: 0x04000F99 RID: 3993
	[SerializeField]
	protected Color32 disabledTextColor = UnityEngine.Color.gray;

	// Token: 0x04000F9A RID: 3994
	[SerializeField]
	protected Color32 textColor = UnityEngine.Color.white;

	// Token: 0x04000F9B RID: 3995
	[SerializeField]
	protected float textScale = 1f;

	// Token: 0x04000F9C RID: 3996
	[SerializeField]
	protected RectOffset textFieldPadding = new RectOffset();

	// Token: 0x04000F9D RID: 3997
	[SerializeField]
	protected dfDropdown.PopupListPosition listPosition;

	// Token: 0x04000F9E RID: 3998
	[SerializeField]
	protected int listWidth;

	// Token: 0x04000F9F RID: 3999
	[SerializeField]
	protected int listHeight = 200;

	// Token: 0x04000FA0 RID: 4000
	[SerializeField]
	protected RectOffset listPadding = new RectOffset();

	// Token: 0x04000FA1 RID: 4001
	[SerializeField]
	protected dfScrollbar listScrollbar;

	// Token: 0x04000FA2 RID: 4002
	[SerializeField]
	protected int itemHeight = 25;

	// Token: 0x04000FA3 RID: 4003
	[SerializeField]
	protected string itemHighlight = string.Empty;

	// Token: 0x04000FA4 RID: 4004
	[SerializeField]
	protected string itemHover = string.Empty;

	// Token: 0x04000FA5 RID: 4005
	[SerializeField]
	protected string listBackground = string.Empty;

	// Token: 0x04000FA6 RID: 4006
	[SerializeField]
	protected Vector2 listOffset = Vector2.zero;

	// Token: 0x04000FA7 RID: 4007
	[SerializeField]
	protected string[] items = new string[0];

	// Token: 0x04000FA8 RID: 4008
	[SerializeField]
	protected bool shadow;

	// Token: 0x04000FA9 RID: 4009
	[SerializeField]
	protected Color32 shadowColor = UnityEngine.Color.black;

	// Token: 0x04000FAA RID: 4010
	[SerializeField]
	protected Vector2 shadowOffset = new Vector2(1f, -1f);

	// Token: 0x04000FAB RID: 4011
	[SerializeField]
	protected bool openOnMouseDown = true;

	// Token: 0x04000FAC RID: 4012
	[SerializeField]
	protected bool clickWhenSpacePressed = true;

	// Token: 0x04000FAD RID: 4013
	private bool eventsAttached;

	// Token: 0x04000FAE RID: 4014
	private bool isFontCallbackAssigned;

	// Token: 0x04000FAF RID: 4015
	private dfListbox popup;

	// Token: 0x04000FB0 RID: 4016
	private dfRenderData textRenderData;

	// Token: 0x04000FB1 RID: 4017
	private dfList<dfRenderData> buffers = dfList<dfRenderData>.Obtain();

	// Token: 0x0200039D RID: 925
	public enum PopupListPosition
	{
		// Token: 0x04000FB3 RID: 4019
		Below,
		// Token: 0x04000FB4 RID: 4020
		Above,
		// Token: 0x04000FB5 RID: 4021
		Automatic
	}

	// Token: 0x0200039E RID: 926
	// (Invoke) Token: 0x060011B9 RID: 4537
	[dfEventCategory("Popup")]
	public delegate void PopupEventHandler(dfDropdown dropdown, dfListbox popup, ref bool overridden);
}
