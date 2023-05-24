using System;
using System.Diagnostics;
using UnityEngine;

// Token: 0x020003E2 RID: 994
[ExecuteInEditMode]
[AddComponentMenu("Daikon Forge/User Interface/Listbox")]
[dfHelp("http://www.daikonforge.com/docs/df-gui/classdf_listbox.html")]
[dfCategory("Basic Controls")]
[dfTooltip("Allows the user to select from a list of options")]
[Serializable]
public class dfListbox : dfInteractiveBase, IDFMultiRender, IRendersText
{
	// Token: 0x14000037 RID: 55
	// (add) Token: 0x06001459 RID: 5209 RVA: 0x0005E09C File Offset: 0x0005C29C
	// (remove) Token: 0x0600145A RID: 5210 RVA: 0x0005E0D4 File Offset: 0x0005C2D4
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event PropertyChangedEventHandler<int> SelectedIndexChanged;

	// Token: 0x14000038 RID: 56
	// (add) Token: 0x0600145B RID: 5211 RVA: 0x0005E10C File Offset: 0x0005C30C
	// (remove) Token: 0x0600145C RID: 5212 RVA: 0x0005E144 File Offset: 0x0005C344
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event PropertyChangedEventHandler<int> ItemClicked;

	// Token: 0x1700046A RID: 1130
	// (get) Token: 0x0600145D RID: 5213 RVA: 0x0005E17C File Offset: 0x0005C37C
	// (set) Token: 0x0600145E RID: 5214 RVA: 0x0005E1C0 File Offset: 0x0005C3C0
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

	// Token: 0x1700046B RID: 1131
	// (get) Token: 0x0600145F RID: 5215 RVA: 0x0005E1EC File Offset: 0x0005C3EC
	// (set) Token: 0x06001460 RID: 5216 RVA: 0x0005E1F4 File Offset: 0x0005C3F4
	public float ScrollPosition
	{
		get
		{
			return this.scrollPosition;
		}
		set
		{
			if (!Mathf.Approximately(value, this.scrollPosition))
			{
				this.scrollPosition = this.constrainScrollPosition(value);
				this.Invalidate();
			}
		}
	}

	// Token: 0x1700046C RID: 1132
	// (get) Token: 0x06001461 RID: 5217 RVA: 0x0005E21C File Offset: 0x0005C41C
	// (set) Token: 0x06001462 RID: 5218 RVA: 0x0005E224 File Offset: 0x0005C424
	public TextAlignment ItemAlignment
	{
		get
		{
			return this.itemAlignment;
		}
		set
		{
			if (value != this.itemAlignment)
			{
				this.itemAlignment = value;
				this.Invalidate();
			}
		}
	}

	// Token: 0x1700046D RID: 1133
	// (get) Token: 0x06001463 RID: 5219 RVA: 0x0005E240 File Offset: 0x0005C440
	// (set) Token: 0x06001464 RID: 5220 RVA: 0x0005E248 File Offset: 0x0005C448
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
				this.itemHighlight = value;
				this.Invalidate();
			}
		}
	}

	// Token: 0x1700046E RID: 1134
	// (get) Token: 0x06001465 RID: 5221 RVA: 0x0005E268 File Offset: 0x0005C468
	// (set) Token: 0x06001466 RID: 5222 RVA: 0x0005E270 File Offset: 0x0005C470
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

	// Token: 0x1700046F RID: 1135
	// (get) Token: 0x06001467 RID: 5223 RVA: 0x0005E290 File Offset: 0x0005C490
	public string SelectedItem
	{
		get
		{
			if (this.selectedIndex == -1)
			{
				return null;
			}
			return this.items[this.selectedIndex];
		}
	}

	// Token: 0x17000470 RID: 1136
	// (get) Token: 0x06001468 RID: 5224 RVA: 0x0005E2B0 File Offset: 0x0005C4B0
	// (set) Token: 0x06001469 RID: 5225 RVA: 0x0005E2C0 File Offset: 0x0005C4C0
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
		}
	}

	// Token: 0x17000471 RID: 1137
	// (get) Token: 0x0600146A RID: 5226 RVA: 0x0005E30C File Offset: 0x0005C50C
	// (set) Token: 0x0600146B RID: 5227 RVA: 0x0005E314 File Offset: 0x0005C514
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
				this.selectedIndex = value;
				this.EnsureVisible(value);
				this.OnSelectedIndexChanged();
				this.Invalidate();
			}
		}
	}

	// Token: 0x17000472 RID: 1138
	// (get) Token: 0x0600146C RID: 5228 RVA: 0x0005E364 File Offset: 0x0005C564
	// (set) Token: 0x0600146D RID: 5229 RVA: 0x0005E384 File Offset: 0x0005C584
	public RectOffset ItemPadding
	{
		get
		{
			if (this.itemPadding == null)
			{
				this.itemPadding = new RectOffset();
			}
			return this.itemPadding;
		}
		set
		{
			value = value.ConstrainPadding();
			if (!value.Equals(this.itemPadding))
			{
				this.itemPadding = value;
				this.Invalidate();
			}
		}
	}

	// Token: 0x17000473 RID: 1139
	// (get) Token: 0x0600146E RID: 5230 RVA: 0x0005E3AC File Offset: 0x0005C5AC
	// (set) Token: 0x0600146F RID: 5231 RVA: 0x0005E3B4 File Offset: 0x0005C5B4
	public Color32 ItemTextColor
	{
		get
		{
			return this.itemTextColor;
		}
		set
		{
			if (!value.Equals(this.itemTextColor))
			{
				this.itemTextColor = value;
				this.Invalidate();
			}
		}
	}

	// Token: 0x17000474 RID: 1140
	// (get) Token: 0x06001470 RID: 5232 RVA: 0x0005E3E0 File Offset: 0x0005C5E0
	// (set) Token: 0x06001471 RID: 5233 RVA: 0x0005E3E8 File Offset: 0x0005C5E8
	public float ItemTextScale
	{
		get
		{
			return this.itemTextScale;
		}
		set
		{
			value = Mathf.Max(0.1f, value);
			if (!Mathf.Approximately(this.itemTextScale, value))
			{
				this.itemTextScale = value;
				this.Invalidate();
			}
		}
	}

	// Token: 0x17000475 RID: 1141
	// (get) Token: 0x06001472 RID: 5234 RVA: 0x0005E418 File Offset: 0x0005C618
	// (set) Token: 0x06001473 RID: 5235 RVA: 0x0005E420 File Offset: 0x0005C620
	public int ItemHeight
	{
		get
		{
			return this.itemHeight;
		}
		set
		{
			this.scrollPosition = 0f;
			value = Mathf.Max(1, value);
			if (value != this.itemHeight)
			{
				this.itemHeight = value;
				this.Invalidate();
			}
		}
	}

	// Token: 0x17000476 RID: 1142
	// (get) Token: 0x06001474 RID: 5236 RVA: 0x0005E450 File Offset: 0x0005C650
	// (set) Token: 0x06001475 RID: 5237 RVA: 0x0005E470 File Offset: 0x0005C670
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
			if (value != this.items)
			{
				this.scrollPosition = 0f;
				if (value == null)
				{
					value = new string[0];
				}
				this.items = value;
				this.Invalidate();
			}
		}
	}

	// Token: 0x17000477 RID: 1143
	// (get) Token: 0x06001476 RID: 5238 RVA: 0x0005E4A4 File Offset: 0x0005C6A4
	// (set) Token: 0x06001477 RID: 5239 RVA: 0x0005E4AC File Offset: 0x0005C6AC
	public dfScrollbar Scrollbar
	{
		get
		{
			return this.scrollbar;
		}
		set
		{
			this.scrollPosition = 0f;
			if (value != this.scrollbar)
			{
				this.detachScrollbarEvents();
				this.scrollbar = value;
				this.attachScrollbarEvents();
				this.Invalidate();
			}
		}
	}

	// Token: 0x17000478 RID: 1144
	// (get) Token: 0x06001478 RID: 5240 RVA: 0x0005E4E4 File Offset: 0x0005C6E4
	// (set) Token: 0x06001479 RID: 5241 RVA: 0x0005E504 File Offset: 0x0005C704
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

	// Token: 0x17000479 RID: 1145
	// (get) Token: 0x0600147A RID: 5242 RVA: 0x0005E52C File Offset: 0x0005C72C
	// (set) Token: 0x0600147B RID: 5243 RVA: 0x0005E534 File Offset: 0x0005C734
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

	// Token: 0x1700047A RID: 1146
	// (get) Token: 0x0600147C RID: 5244 RVA: 0x0005E550 File Offset: 0x0005C750
	// (set) Token: 0x0600147D RID: 5245 RVA: 0x0005E558 File Offset: 0x0005C758
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

	// Token: 0x1700047B RID: 1147
	// (get) Token: 0x0600147E RID: 5246 RVA: 0x0005E584 File Offset: 0x0005C784
	// (set) Token: 0x0600147F RID: 5247 RVA: 0x0005E58C File Offset: 0x0005C78C
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

	// Token: 0x1700047C RID: 1148
	// (get) Token: 0x06001480 RID: 5248 RVA: 0x0005E5AC File Offset: 0x0005C7AC
	// (set) Token: 0x06001481 RID: 5249 RVA: 0x0005E5B4 File Offset: 0x0005C7B4
	public bool AnimateHover
	{
		get
		{
			return this.animateHover;
		}
		set
		{
			this.animateHover = value;
		}
	}

	// Token: 0x1700047D RID: 1149
	// (get) Token: 0x06001482 RID: 5250 RVA: 0x0005E5C0 File Offset: 0x0005C7C0
	// (set) Token: 0x06001483 RID: 5251 RVA: 0x0005E5C8 File Offset: 0x0005C7C8
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

	// Token: 0x06001484 RID: 5252 RVA: 0x0005E5D8 File Offset: 0x0005C7D8
	public override void Awake()
	{
		base.Awake();
		this.startSize = base.Size;
	}

	// Token: 0x06001485 RID: 5253 RVA: 0x0005E5EC File Offset: 0x0005C7EC
	public override void Update()
	{
		base.Update();
		if (this.size.magnitude == 0f)
		{
			this.size = new Vector2(200f, 150f);
		}
		if (this.animateHover && this.hoverIndex != -1)
		{
			float num = (float)(this.hoverIndex * this.itemHeight) * base.PixelsToUnits();
			if (Mathf.Abs(this.hoverTweenLocation - num) < 1f)
			{
				this.Invalidate();
			}
		}
		if (this.isControlInvalidated)
		{
			this.synchronizeScrollbar();
		}
	}

	// Token: 0x06001486 RID: 5254 RVA: 0x0005E684 File Offset: 0x0005C884
	public override void LateUpdate()
	{
		base.LateUpdate();
		if (!Application.isPlaying)
		{
			return;
		}
		this.attachScrollbarEvents();
	}

	// Token: 0x06001487 RID: 5255 RVA: 0x0005E6A0 File Offset: 0x0005C8A0
	public override void OnEnable()
	{
		base.OnEnable();
		this.bindTextureRebuildCallback();
	}

	// Token: 0x06001488 RID: 5256 RVA: 0x0005E6B0 File Offset: 0x0005C8B0
	public override void OnDestroy()
	{
		base.OnDestroy();
		this.detachScrollbarEvents();
	}

	// Token: 0x06001489 RID: 5257 RVA: 0x0005E6C0 File Offset: 0x0005C8C0
	public override void OnDisable()
	{
		base.OnDisable();
		this.unbindTextureRebuildCallback();
		this.detachScrollbarEvents();
	}

	// Token: 0x0600148A RID: 5258 RVA: 0x0005E6D4 File Offset: 0x0005C8D4
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

	// Token: 0x0600148B RID: 5259 RVA: 0x0005E73C File Offset: 0x0005C93C
	protected internal virtual void OnSelectedIndexChanged()
	{
		base.SignalHierarchy("OnSelectedIndexChanged", new object[] { this, this.selectedIndex });
		if (this.SelectedIndexChanged != null)
		{
			this.SelectedIndexChanged(this, this.selectedIndex);
		}
	}

	// Token: 0x0600148C RID: 5260 RVA: 0x0005E78C File Offset: 0x0005C98C
	protected internal virtual void OnItemClicked()
	{
		base.Signal("OnItemClicked", this, this.selectedIndex);
		if (this.ItemClicked != null)
		{
			this.ItemClicked(this, this.selectedIndex);
		}
	}

	// Token: 0x0600148D RID: 5261 RVA: 0x0005E7C4 File Offset: 0x0005C9C4
	protected internal override void OnMouseMove(dfMouseEventArgs args)
	{
		base.OnMouseMove(args);
		if (!(args is dfTouchEventArgs))
		{
			this.updateItemHover(args);
			return;
		}
		if (Mathf.Abs(args.Position.y - this.touchStartPosition.y) < (float)(this.itemHeight / 2))
		{
			return;
		}
		this.ScrollPosition = Mathf.Max(0f, this.ScrollPosition + args.MoveDelta.y);
		this.synchronizeScrollbar();
		this.hoverIndex = -1;
	}

	// Token: 0x0600148E RID: 5262 RVA: 0x0005E84C File Offset: 0x0005CA4C
	protected internal override void OnMouseEnter(dfMouseEventArgs args)
	{
		base.OnMouseEnter(args);
		this.touchStartPosition = args.Position;
	}

	// Token: 0x0600148F RID: 5263 RVA: 0x0005E864 File Offset: 0x0005CA64
	protected internal override void OnMouseLeave(dfMouseEventArgs args)
	{
		base.OnMouseLeave(args);
		this.hoverIndex = -1;
	}

	// Token: 0x06001490 RID: 5264 RVA: 0x0005E874 File Offset: 0x0005CA74
	protected internal override void OnMouseWheel(dfMouseEventArgs args)
	{
		base.OnMouseWheel(args);
		this.ScrollPosition = Mathf.Max(0f, this.ScrollPosition - (float)((int)args.WheelDelta * this.ItemHeight));
		this.synchronizeScrollbar();
		this.updateItemHover(args);
	}

	// Token: 0x06001491 RID: 5265 RVA: 0x0005E8B0 File Offset: 0x0005CAB0
	protected internal override void OnMouseUp(dfMouseEventArgs args)
	{
		this.hoverIndex = -1;
		base.OnMouseUp(args);
		if (args is dfTouchEventArgs && Mathf.Abs(args.Position.y - this.touchStartPosition.y) < (float)this.itemHeight)
		{
			this.selectItemUnderMouse(args);
		}
	}

	// Token: 0x06001492 RID: 5266 RVA: 0x0005E908 File Offset: 0x0005CB08
	protected internal override void OnMouseDown(dfMouseEventArgs args)
	{
		base.OnMouseDown(args);
		if (args is dfTouchEventArgs)
		{
			this.touchStartPosition = args.Position;
			return;
		}
		this.selectItemUnderMouse(args);
	}

	// Token: 0x06001493 RID: 5267 RVA: 0x0005E930 File Offset: 0x0005CB30
	protected internal override void OnKeyDown(dfKeyEventArgs args)
	{
		switch (args.KeyCode)
		{
		case KeyCode.UpArrow:
			this.SelectedIndex = Mathf.Max(0, this.selectedIndex - 1);
			break;
		case KeyCode.DownArrow:
			this.SelectedIndex++;
			break;
		case KeyCode.Home:
			this.SelectedIndex = 0;
			break;
		case KeyCode.End:
			this.SelectedIndex = this.items.Length;
			break;
		case KeyCode.PageUp:
		{
			int num = this.SelectedIndex - Mathf.FloorToInt((this.size.y - (float)this.listPadding.vertical) / (float)this.itemHeight);
			this.SelectedIndex = Mathf.Max(0, num);
			break;
		}
		case KeyCode.PageDown:
			this.SelectedIndex += Mathf.FloorToInt((this.size.y - (float)this.listPadding.vertical) / (float)this.itemHeight);
			break;
		}
		base.OnKeyDown(args);
	}

	// Token: 0x06001494 RID: 5268 RVA: 0x0005EA44 File Offset: 0x0005CC44
	public void AddItem(string item)
	{
		string[] array = new string[this.items.Length + 1];
		Array.Copy(this.items, array, this.items.Length);
		array[this.items.Length] = item;
		this.items = array;
		this.Invalidate();
	}

	// Token: 0x06001495 RID: 5269 RVA: 0x0005EA90 File Offset: 0x0005CC90
	public void EnsureVisible(int index)
	{
		int num = index * this.ItemHeight;
		if (this.scrollPosition > (float)num)
		{
			this.ScrollPosition = (float)num;
		}
		float num2 = this.size.y - (float)this.listPadding.vertical;
		if (this.scrollPosition + num2 < (float)(num + this.itemHeight))
		{
			this.ScrollPosition = (float)num - num2 + (float)this.itemHeight;
		}
	}

	// Token: 0x06001496 RID: 5270 RVA: 0x0005EAFC File Offset: 0x0005CCFC
	private void selectItemUnderMouse(dfMouseEventArgs args)
	{
		float num = this.pivot.TransformToUpperLeft(base.Size).y + ((float)(-(float)this.itemHeight) * ((float)this.selectedIndex - this.scrollPosition) - (float)this.listPadding.top);
		float num2 = ((float)this.selectedIndex - this.scrollPosition + 1f) * (float)this.itemHeight + (float)this.listPadding.vertical;
		float num3 = num2 - this.size.y;
		if (num3 > 0f)
		{
			num += num3;
		}
		float num4 = base.GetHitPosition(args).y - (float)this.listPadding.top;
		if (num4 < 0f || num4 > this.size.y - (float)this.listPadding.bottom)
		{
			return;
		}
		this.SelectedIndex = (int)((this.scrollPosition + num4) / (float)this.itemHeight);
		this.OnItemClicked();
	}

	// Token: 0x06001497 RID: 5271 RVA: 0x0005EBF8 File Offset: 0x0005CDF8
	private void renderHover()
	{
		if (!Application.isPlaying)
		{
			return;
		}
		bool flag = base.Atlas == null || !base.IsEnabled || this.hoverIndex < 0 || this.hoverIndex > this.items.Length - 1 || string.IsNullOrEmpty(this.ItemHover);
		if (flag)
		{
			return;
		}
		dfAtlas.ItemInfo itemInfo = base.Atlas[this.ItemHover];
		if (itemInfo == null)
		{
			return;
		}
		Vector3 vector = this.pivot.TransformToUpperLeft(base.Size);
		Vector3 vector2 = new Vector3(vector.x + (float)this.listPadding.left, vector.y - (float)this.listPadding.top + this.scrollPosition, 0f);
		float num = base.PixelsToUnits();
		int num2 = this.hoverIndex * this.itemHeight;
		if (this.animateHover)
		{
			float num3 = Mathf.Abs(this.hoverTweenLocation - (float)num2);
			float num4 = (this.size.y - (float)this.listPadding.vertical) * 0.5f;
			if (num3 > num4)
			{
				this.hoverTweenLocation = (float)num2 + Mathf.Sign(this.hoverTweenLocation - (float)num2) * num4;
			}
			float num5 = BraveTime.DeltaTime / num * 2f;
			this.hoverTweenLocation = Mathf.MoveTowards(this.hoverTweenLocation, (float)num2, num5);
		}
		else
		{
			this.hoverTweenLocation = (float)num2;
		}
		vector2.y -= this.hoverTweenLocation.Quantize(num);
		Color32 color = base.ApplyOpacity(this.color);
		dfSprite.RenderOptions renderOptions = new dfSprite.RenderOptions
		{
			atlas = this.atlas,
			color = color,
			fillAmount = 1f,
			flip = dfSpriteFlip.None,
			pixelsToUnits = base.PixelsToUnits(),
			size = new Vector3(this.size.x - (float)this.listPadding.horizontal, (float)this.itemHeight),
			spriteInfo = itemInfo,
			offset = vector2
		};
		if (itemInfo.border.horizontal > 0 || itemInfo.border.vertical > 0)
		{
			dfSlicedSprite.renderSprite(this.renderData, renderOptions);
		}
		else
		{
			dfSprite.renderSprite(this.renderData, renderOptions);
		}
		if ((float)num2 != this.hoverTweenLocation)
		{
			this.Invalidate();
		}
	}

	// Token: 0x06001498 RID: 5272 RVA: 0x0005EE80 File Offset: 0x0005D080
	private void renderSelection()
	{
		if (base.Atlas == null || this.selectedIndex < 0)
		{
			return;
		}
		dfAtlas.ItemInfo itemInfo = base.Atlas[this.ItemHighlight];
		if (itemInfo == null)
		{
			return;
		}
		float num = base.PixelsToUnits();
		Vector3 vector = this.pivot.TransformToUpperLeft(base.Size);
		Vector3 vector2 = new Vector3(vector.x + (float)this.listPadding.left, vector.y - (float)this.listPadding.top + this.scrollPosition, 0f);
		vector2.y -= (float)(this.selectedIndex * this.itemHeight);
		Color32 color = base.ApplyOpacity(this.color);
		dfSprite.RenderOptions renderOptions = new dfSprite.RenderOptions
		{
			atlas = this.atlas,
			color = color,
			fillAmount = 1f,
			flip = dfSpriteFlip.None,
			pixelsToUnits = num,
			size = new Vector3(this.size.x - (float)this.listPadding.horizontal, (float)this.itemHeight),
			spriteInfo = itemInfo,
			offset = vector2
		};
		if (itemInfo.border.horizontal > 0 || itemInfo.border.vertical > 0)
		{
			dfSlicedSprite.renderSprite(this.renderData, renderOptions);
		}
		else
		{
			dfSprite.renderSprite(this.renderData, renderOptions);
		}
	}

	// Token: 0x06001499 RID: 5273 RVA: 0x0005F008 File Offset: 0x0005D208
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

	// Token: 0x0600149A RID: 5274 RVA: 0x0005F06C File Offset: 0x0005D26C
	private void renderItems(dfRenderData buffer)
	{
		if (this.font == null || this.items == null || this.items.Length == 0)
		{
			return;
		}
		float num = base.PixelsToUnits();
		Vector2 vector = new Vector2(this.size.x - (float)this.itemPadding.horizontal - (float)this.listPadding.horizontal, (float)(this.itemHeight - this.itemPadding.vertical));
		Vector3 vector2 = this.pivot.TransformToUpperLeft(base.Size);
		Vector3 vector3 = new Vector3(vector2.x + (float)this.itemPadding.left + (float)this.listPadding.left, vector2.y - (float)this.itemPadding.top - (float)this.listPadding.top, 0f) * num;
		vector3.y += this.scrollPosition * num;
		Color32 color = ((!base.IsEnabled) ? base.DisabledColor : this.ItemTextColor);
		float num2 = vector2.y * num;
		float num3 = num2 - this.size.y * num;
		for (int i = 0; i < this.items.Length; i++)
		{
			using (dfFontRendererBase dfFontRendererBase = this.font.ObtainRenderer())
			{
				dfFontRendererBase.WordWrap = false;
				dfFontRendererBase.MaxSize = vector;
				dfFontRendererBase.PixelRatio = num;
				dfFontRendererBase.TextScale = this.ItemTextScale * this.getTextScaleMultiplier();
				dfFontRendererBase.VectorOffset = vector3;
				dfFontRendererBase.MultiLine = false;
				dfFontRendererBase.TextAlign = this.ItemAlignment;
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
				if (vector3.y - (float)this.itemHeight * num <= num2)
				{
					dfFontRendererBase.Render(this.items[i], buffer);
				}
				vector3.y -= (float)this.itemHeight * num;
				dfFontRendererBase.VectorOffset = vector3;
				if (vector3.y < num3)
				{
					break;
				}
			}
		}
	}

	// Token: 0x0600149B RID: 5275 RVA: 0x0005F314 File Offset: 0x0005D514
	private void clipQuads(dfRenderData buffer, int startIndex)
	{
		dfList<Vector3> vertices = buffer.Vertices;
		dfList<Vector2> uv = buffer.UV;
		float num = base.PixelsToUnits();
		float num2 = (base.Pivot.TransformToUpperLeft(base.Size).y - (float)this.listPadding.top) * num;
		float num3 = num2 - (this.size.y - (float)this.listPadding.vertical) * num;
		for (int i = startIndex; i < vertices.Count; i += 4)
		{
			Vector3 vector = vertices[i];
			Vector3 vector2 = vertices[i + 1];
			Vector3 vector3 = vertices[i + 2];
			Vector3 vector4 = vertices[i + 3];
			float num4 = vector.y - vector4.y;
			if (vector4.y < num3)
			{
				float num5 = 1f - Mathf.Abs(-num3 + vector.y) / num4;
				dfList<Vector3> dfList = vertices;
				int num6 = i;
				vector = new Vector3(vector.x, Mathf.Max(vector.y, num3), vector2.z);
				dfList[num6] = vector;
				dfList<Vector3> dfList2 = vertices;
				int num7 = i + 1;
				vector2 = new Vector3(vector2.x, Mathf.Max(vector2.y, num3), vector2.z);
				dfList2[num7] = vector2;
				dfList<Vector3> dfList3 = vertices;
				int num8 = i + 2;
				vector3 = new Vector3(vector3.x, Mathf.Max(vector3.y, num3), vector3.z);
				dfList3[num8] = vector3;
				dfList<Vector3> dfList4 = vertices;
				int num9 = i + 3;
				vector4 = new Vector3(vector4.x, Mathf.Max(vector4.y, num3), vector4.z);
				dfList4[num9] = vector4;
				float num10 = Mathf.Lerp(uv[i + 3].y, uv[i].y, num5);
				uv[i + 3] = new Vector2(uv[i + 3].x, num10);
				uv[i + 2] = new Vector2(uv[i + 2].x, num10);
				num4 = Mathf.Abs(vector4.y - vector.y);
			}
			if (vector.y > num2)
			{
				float num11 = Mathf.Abs(num2 - vector.y) / num4;
				vertices[i] = new Vector3(vector.x, Mathf.Min(num2, vector.y), vector.z);
				vertices[i + 1] = new Vector3(vector2.x, Mathf.Min(num2, vector2.y), vector2.z);
				vertices[i + 2] = new Vector3(vector3.x, Mathf.Min(num2, vector3.y), vector3.z);
				vertices[i + 3] = new Vector3(vector4.x, Mathf.Min(num2, vector4.y), vector4.z);
				float num12 = Mathf.Lerp(uv[i].y, uv[i + 3].y, num11);
				uv[i] = new Vector2(uv[i].x, num12);
				uv[i + 1] = new Vector2(uv[i + 1].x, num12);
			}
		}
	}

	// Token: 0x0600149C RID: 5276 RVA: 0x0005F680 File Offset: 0x0005D880
	private void updateItemHover(dfMouseEventArgs args)
	{
		if (!Application.isPlaying)
		{
			return;
		}
		Ray ray = args.Ray;
		RaycastHit raycastHit;
		if (!base.GetComponent<Collider>().Raycast(ray, out raycastHit, 1000f))
		{
			this.hoverIndex = -1;
			this.hoverTweenLocation = 0f;
			return;
		}
		Vector2 vector;
		base.GetHitPosition(ray, out vector);
		float num = base.Pivot.TransformToUpperLeft(base.Size).y + ((float)(-(float)this.itemHeight) * ((float)this.selectedIndex - this.scrollPosition) - (float)this.listPadding.top);
		float num2 = ((float)this.selectedIndex - this.scrollPosition + 1f) * (float)this.itemHeight + (float)this.listPadding.vertical;
		float num3 = num2 - this.size.y;
		if (num3 > 0f)
		{
			num += num3;
		}
		float num4 = vector.y - (float)this.listPadding.top;
		int num5 = (int)(this.scrollPosition + num4) / this.itemHeight;
		if (num5 != this.hoverIndex)
		{
			this.hoverIndex = num5;
			this.Invalidate();
		}
	}

	// Token: 0x0600149D RID: 5277 RVA: 0x0005F7A8 File Offset: 0x0005D9A8
	private float constrainScrollPosition(float value)
	{
		value = Mathf.Max(0f, value);
		int num = this.items.Length * this.itemHeight;
		float num2 = this.size.y - (float)this.listPadding.vertical;
		if ((float)num < num2)
		{
			return 0f;
		}
		return Mathf.Min(value, (float)num - num2);
	}

	// Token: 0x0600149E RID: 5278 RVA: 0x0005F804 File Offset: 0x0005DA04
	private void attachScrollbarEvents()
	{
		if (this.scrollbar == null || this.eventsAttached)
		{
			return;
		}
		this.eventsAttached = true;
		this.scrollbar.ValueChanged += this.scrollbar_ValueChanged;
		this.scrollbar.GotFocus += this.scrollbar_GotFocus;
	}

	// Token: 0x0600149F RID: 5279 RVA: 0x0005F864 File Offset: 0x0005DA64
	private void detachScrollbarEvents()
	{
		if (this.scrollbar == null || !this.eventsAttached)
		{
			return;
		}
		this.eventsAttached = false;
		this.scrollbar.ValueChanged -= this.scrollbar_ValueChanged;
		this.scrollbar.GotFocus -= this.scrollbar_GotFocus;
	}

	// Token: 0x060014A0 RID: 5280 RVA: 0x0005F8C4 File Offset: 0x0005DAC4
	private void scrollbar_GotFocus(dfControl control, dfFocusEventArgs args)
	{
		base.Focus(true);
	}

	// Token: 0x060014A1 RID: 5281 RVA: 0x0005F8D0 File Offset: 0x0005DAD0
	private void scrollbar_ValueChanged(dfControl control, float value)
	{
		this.ScrollPosition = value;
	}

	// Token: 0x060014A2 RID: 5282 RVA: 0x0005F8DC File Offset: 0x0005DADC
	private void synchronizeScrollbar()
	{
		if (this.scrollbar == null)
		{
			return;
		}
		int num = this.items.Length * this.itemHeight;
		float num2 = this.size.y - (float)this.listPadding.vertical;
		this.scrollbar.IncrementAmount = (float)this.itemHeight;
		this.scrollbar.MinValue = 0f;
		this.scrollbar.MaxValue = (float)num;
		this.scrollbar.ScrollSize = num2;
		this.scrollbar.Value = this.scrollPosition;
	}

	// Token: 0x060014A3 RID: 5283 RVA: 0x0005F970 File Offset: 0x0005DB70
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
		Matrix4x4 localToWorldMatrix = base.transform.localToWorldMatrix;
		if (!this.isControlInvalidated)
		{
			for (int i = 0; i < this.buffers.Count; i++)
			{
				this.buffers[i].Transform = localToWorldMatrix;
			}
			return this.buffers;
		}
		this.buffers.Clear();
		this.renderData.Clear();
		this.renderData.Material = base.Atlas.Material;
		this.renderData.Transform = localToWorldMatrix;
		this.buffers.Add(this.renderData);
		this.textRenderData.Clear();
		this.textRenderData.Material = base.Atlas.Material;
		this.textRenderData.Transform = localToWorldMatrix;
		this.buffers.Add(this.textRenderData);
		this.renderBackground();
		int count = this.renderData.Vertices.Count;
		this.renderHover();
		this.renderSelection();
		this.renderItems(this.textRenderData);
		this.clipQuads(this.renderData, count);
		this.clipQuads(this.textRenderData, 0);
		this.isControlInvalidated = false;
		base.updateCollider();
		return this.buffers;
	}

	// Token: 0x060014A4 RID: 5284 RVA: 0x0005FB08 File Offset: 0x0005DD08
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

	// Token: 0x060014A5 RID: 5285 RVA: 0x0005FB5C File Offset: 0x0005DD5C
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

	// Token: 0x060014A6 RID: 5286 RVA: 0x0005FBB0 File Offset: 0x0005DDB0
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
		if (this.items == null || this.items.Length == 0)
		{
			return;
		}
		float textScaleMultiplier = this.getTextScaleMultiplier();
		int num = Mathf.CeilToInt((float)this.font.FontSize * textScaleMultiplier);
		for (int i = 0; i < this.items.Length; i++)
		{
			dfDynamicFont.AddCharacterRequest(this.items[i], num, FontStyle.Normal);
		}
	}

	// Token: 0x060014A7 RID: 5287 RVA: 0x0005FC44 File Offset: 0x0005DE44
	private void onFontTextureRebuilt(Font font)
	{
		if (this.Font is dfDynamicFont && font == (this.Font as dfDynamicFont).BaseFont)
		{
			this.requestCharacterInfo();
			this.Invalidate();
		}
	}

	// Token: 0x060014A8 RID: 5288 RVA: 0x0005FC80 File Offset: 0x0005DE80
	public void UpdateFontInfo()
	{
		this.requestCharacterInfo();
	}

	// Token: 0x040011C9 RID: 4553
	[SerializeField]
	protected dfFontBase font;

	// Token: 0x040011CA RID: 4554
	[SerializeField]
	protected RectOffset listPadding = new RectOffset();

	// Token: 0x040011CB RID: 4555
	[SerializeField]
	protected int selectedIndex = -1;

	// Token: 0x040011CC RID: 4556
	[SerializeField]
	protected Color32 itemTextColor = UnityEngine.Color.white;

	// Token: 0x040011CD RID: 4557
	[SerializeField]
	protected float itemTextScale = 1f;

	// Token: 0x040011CE RID: 4558
	[SerializeField]
	protected int itemHeight = 25;

	// Token: 0x040011CF RID: 4559
	[SerializeField]
	protected RectOffset itemPadding = new RectOffset();

	// Token: 0x040011D0 RID: 4560
	[SerializeField]
	protected string[] items = new string[0];

	// Token: 0x040011D1 RID: 4561
	[SerializeField]
	protected string itemHighlight = string.Empty;

	// Token: 0x040011D2 RID: 4562
	[SerializeField]
	protected string itemHover = string.Empty;

	// Token: 0x040011D3 RID: 4563
	[SerializeField]
	protected dfScrollbar scrollbar;

	// Token: 0x040011D4 RID: 4564
	[SerializeField]
	protected bool animateHover;

	// Token: 0x040011D5 RID: 4565
	[SerializeField]
	protected bool shadow;

	// Token: 0x040011D6 RID: 4566
	[SerializeField]
	protected dfTextScaleMode textScaleMode;

	// Token: 0x040011D7 RID: 4567
	[SerializeField]
	protected Color32 shadowColor = UnityEngine.Color.black;

	// Token: 0x040011D8 RID: 4568
	[SerializeField]
	protected Vector2 shadowOffset = new Vector2(1f, -1f);

	// Token: 0x040011D9 RID: 4569
	[SerializeField]
	protected TextAlignment itemAlignment;

	// Token: 0x040011DA RID: 4570
	private bool isFontCallbackAssigned;

	// Token: 0x040011DB RID: 4571
	private bool eventsAttached;

	// Token: 0x040011DC RID: 4572
	private float scrollPosition;

	// Token: 0x040011DD RID: 4573
	private int hoverIndex = -1;

	// Token: 0x040011DE RID: 4574
	private float hoverTweenLocation;

	// Token: 0x040011DF RID: 4575
	private Vector2 touchStartPosition = Vector2.zero;

	// Token: 0x040011E0 RID: 4576
	private Vector2 startSize = Vector2.zero;

	// Token: 0x040011E1 RID: 4577
	private dfRenderData textRenderData;

	// Token: 0x040011E2 RID: 4578
	private dfList<dfRenderData> buffers = dfList<dfRenderData>.Obtain();
}
