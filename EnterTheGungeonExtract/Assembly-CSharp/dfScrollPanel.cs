using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using InControl;
using UnityEngine;

// Token: 0x020003F2 RID: 1010
[dfTooltip("Implements a scrollable control container")]
[dfHelp("http://www.daikonforge.com/docs/df-gui/classdf_scroll_panel.html")]
[ExecuteInEditMode]
[AddComponentMenu("Daikon Forge/User Interface/Containers/Scrollable Panel")]
[dfCategory("Basic Controls")]
[Serializable]
public class dfScrollPanel : dfControl
{
	// Token: 0x1400003B RID: 59
	// (add) Token: 0x06001597 RID: 5527 RVA: 0x00064CB8 File Offset: 0x00062EB8
	// (remove) Token: 0x06001598 RID: 5528 RVA: 0x00064CF0 File Offset: 0x00062EF0
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event PropertyChangedEventHandler<Vector2> ScrollPositionChanged;

	// Token: 0x170004B0 RID: 1200
	// (get) Token: 0x06001599 RID: 5529 RVA: 0x00064D28 File Offset: 0x00062F28
	// (set) Token: 0x0600159A RID: 5530 RVA: 0x00064D30 File Offset: 0x00062F30
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

	// Token: 0x170004B1 RID: 1201
	// (get) Token: 0x0600159B RID: 5531 RVA: 0x00064D44 File Offset: 0x00062F44
	// (set) Token: 0x0600159C RID: 5532 RVA: 0x00064D4C File Offset: 0x00062F4C
	public bool ScrollWithArrowKeys
	{
		get
		{
			return this.scrollWithArrowKeys;
		}
		set
		{
			this.scrollWithArrowKeys = value;
		}
	}

	// Token: 0x170004B2 RID: 1202
	// (get) Token: 0x0600159D RID: 5533 RVA: 0x00064D58 File Offset: 0x00062F58
	// (set) Token: 0x0600159E RID: 5534 RVA: 0x00064DA0 File Offset: 0x00062FA0
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

	// Token: 0x170004B3 RID: 1203
	// (get) Token: 0x0600159F RID: 5535 RVA: 0x00064DC0 File Offset: 0x00062FC0
	// (set) Token: 0x060015A0 RID: 5536 RVA: 0x00064DC8 File Offset: 0x00062FC8
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

	// Token: 0x170004B4 RID: 1204
	// (get) Token: 0x060015A1 RID: 5537 RVA: 0x00064DE8 File Offset: 0x00062FE8
	// (set) Token: 0x060015A2 RID: 5538 RVA: 0x00064DF0 File Offset: 0x00062FF0
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

	// Token: 0x170004B5 RID: 1205
	// (get) Token: 0x060015A3 RID: 5539 RVA: 0x00064E1C File Offset: 0x0006301C
	// (set) Token: 0x060015A4 RID: 5540 RVA: 0x00064E24 File Offset: 0x00063024
	public bool AutoReset
	{
		get
		{
			return this.autoReset;
		}
		set
		{
			if (value != this.autoReset)
			{
				this.autoReset = value;
				this.Reset();
			}
		}
	}

	// Token: 0x170004B6 RID: 1206
	// (get) Token: 0x060015A5 RID: 5541 RVA: 0x00064E40 File Offset: 0x00063040
	// (set) Token: 0x060015A6 RID: 5542 RVA: 0x00064E60 File Offset: 0x00063060
	public RectOffset ScrollPadding
	{
		get
		{
			if (this.scrollPadding == null)
			{
				this.scrollPadding = new RectOffset();
			}
			return this.scrollPadding;
		}
		set
		{
			value = value.ConstrainPadding();
			if (!object.Equals(value, this.scrollPadding))
			{
				this.scrollPadding = value;
				if (this.AutoReset || this.AutoLayout)
				{
					this.Reset();
				}
			}
		}
	}

	// Token: 0x170004B7 RID: 1207
	// (get) Token: 0x060015A7 RID: 5543 RVA: 0x00064EA0 File Offset: 0x000630A0
	// (set) Token: 0x060015A8 RID: 5544 RVA: 0x00064EC0 File Offset: 0x000630C0
	public RectOffset AutoScrollPadding
	{
		get
		{
			if (this.autoScrollPadding == null)
			{
				this.autoScrollPadding = new RectOffset();
			}
			return this.autoScrollPadding;
		}
		set
		{
			value = value.ConstrainPadding();
			if (!object.Equals(value, this.autoScrollPadding))
			{
				this.autoScrollPadding = value;
				if (this.AutoReset || this.AutoLayout)
				{
					this.Reset();
				}
			}
		}
	}

	// Token: 0x170004B8 RID: 1208
	// (get) Token: 0x060015A9 RID: 5545 RVA: 0x00064F00 File Offset: 0x00063100
	// (set) Token: 0x060015AA RID: 5546 RVA: 0x00064F08 File Offset: 0x00063108
	public bool AutoLayout
	{
		get
		{
			return this.autoLayout;
		}
		set
		{
			if (value != this.autoLayout)
			{
				this.autoLayout = value;
				if (this.AutoReset || this.AutoLayout)
				{
					this.Reset();
				}
			}
		}
	}

	// Token: 0x170004B9 RID: 1209
	// (get) Token: 0x060015AB RID: 5547 RVA: 0x00064F3C File Offset: 0x0006313C
	// (set) Token: 0x060015AC RID: 5548 RVA: 0x00064F44 File Offset: 0x00063144
	public bool WrapLayout
	{
		get
		{
			return this.wrapLayout;
		}
		set
		{
			if (value != this.wrapLayout)
			{
				this.wrapLayout = value;
				this.Reset();
			}
		}
	}

	// Token: 0x170004BA RID: 1210
	// (get) Token: 0x060015AD RID: 5549 RVA: 0x00064F60 File Offset: 0x00063160
	// (set) Token: 0x060015AE RID: 5550 RVA: 0x00064F68 File Offset: 0x00063168
	public dfScrollPanel.LayoutDirection FlowDirection
	{
		get
		{
			return this.flowDirection;
		}
		set
		{
			if (value != this.flowDirection)
			{
				this.flowDirection = value;
				this.Reset();
			}
		}
	}

	// Token: 0x170004BB RID: 1211
	// (get) Token: 0x060015AF RID: 5551 RVA: 0x00064F84 File Offset: 0x00063184
	// (set) Token: 0x060015B0 RID: 5552 RVA: 0x00064FA4 File Offset: 0x000631A4
	public RectOffset FlowPadding
	{
		get
		{
			if (this.flowPadding == null)
			{
				this.flowPadding = new RectOffset();
			}
			return this.flowPadding;
		}
		set
		{
			value = value.ConstrainPadding();
			if (!object.Equals(value, this.flowPadding))
			{
				this.flowPadding = value;
				this.Reset();
			}
		}
	}

	// Token: 0x060015B1 RID: 5553 RVA: 0x00064FCC File Offset: 0x000631CC
	public Vector2 GetMaxScrollPositionDimensions()
	{
		Vector2 vector = this.calculateViewSize();
		Vector2 vector2 = new Vector2(this.size.x - (float)this.scrollPadding.horizontal, this.size.y - (float)this.scrollPadding.vertical);
		return vector - vector2;
	}

	// Token: 0x170004BC RID: 1212
	// (get) Token: 0x060015B2 RID: 5554 RVA: 0x00065020 File Offset: 0x00063220
	// (set) Token: 0x060015B3 RID: 5555 RVA: 0x00065028 File Offset: 0x00063228
	public Vector2 ScrollPosition
	{
		get
		{
			return this.scrollPosition;
		}
		set
		{
			Vector2 vector = this.calculateViewSize();
			Vector2 vector2 = new Vector2(this.size.x - (float)this.scrollPadding.horizontal, this.size.y - (float)this.scrollPadding.vertical);
			value = Vector2.Min(vector - vector2, value);
			value = Vector2.Max(Vector2.zero, value);
			value = value.RoundToInt();
			if ((value - this.scrollPosition).sqrMagnitude > 1E-45f)
			{
				Vector2 vector3 = value - this.scrollPosition;
				this.scrollPosition = value;
				this.scrollChildControls(vector3);
				this.updateScrollbars();
			}
			this.OnScrollPositionChanged();
		}
	}

	// Token: 0x170004BD RID: 1213
	// (get) Token: 0x060015B4 RID: 5556 RVA: 0x000650E4 File Offset: 0x000632E4
	// (set) Token: 0x060015B5 RID: 5557 RVA: 0x000650EC File Offset: 0x000632EC
	public int ScrollWheelAmount
	{
		get
		{
			return this.scrollWheelAmount;
		}
		set
		{
			this.scrollWheelAmount = value;
		}
	}

	// Token: 0x170004BE RID: 1214
	// (get) Token: 0x060015B6 RID: 5558 RVA: 0x000650F8 File Offset: 0x000632F8
	// (set) Token: 0x060015B7 RID: 5559 RVA: 0x00065100 File Offset: 0x00063300
	public dfScrollbar HorzScrollbar
	{
		get
		{
			return this.horzScroll;
		}
		set
		{
			this.horzScroll = value;
			this.updateScrollbars();
		}
	}

	// Token: 0x170004BF RID: 1215
	// (get) Token: 0x060015B8 RID: 5560 RVA: 0x00065110 File Offset: 0x00063310
	// (set) Token: 0x060015B9 RID: 5561 RVA: 0x00065118 File Offset: 0x00063318
	public dfScrollbar VertScrollbar
	{
		get
		{
			return this.vertScroll;
		}
		set
		{
			this.vertScroll = value;
			this.updateScrollbars();
		}
	}

	// Token: 0x170004C0 RID: 1216
	// (get) Token: 0x060015BA RID: 5562 RVA: 0x00065128 File Offset: 0x00063328
	// (set) Token: 0x060015BB RID: 5563 RVA: 0x00065130 File Offset: 0x00063330
	public dfControlOrientation WheelScrollDirection
	{
		get
		{
			return this.wheelDirection;
		}
		set
		{
			this.wheelDirection = value;
		}
	}

	// Token: 0x170004C1 RID: 1217
	// (get) Token: 0x060015BC RID: 5564 RVA: 0x0006513C File Offset: 0x0006333C
	// (set) Token: 0x060015BD RID: 5565 RVA: 0x00065144 File Offset: 0x00063344
	public bool UseVirtualScrolling
	{
		get
		{
			return this.useVirtualScrolling;
		}
		set
		{
			this.useVirtualScrolling = value;
			if (!value)
			{
				this.VirtualScrollingTile = null;
			}
		}
	}

	// Token: 0x170004C2 RID: 1218
	// (get) Token: 0x060015BE RID: 5566 RVA: 0x0006515C File Offset: 0x0006335C
	// (set) Token: 0x060015BF RID: 5567 RVA: 0x00065164 File Offset: 0x00063364
	public bool AutoFitVirtualTiles
	{
		get
		{
			return this.autoFitVirtualTiles;
		}
		set
		{
			this.autoFitVirtualTiles = value;
		}
	}

	// Token: 0x170004C3 RID: 1219
	// (get) Token: 0x060015C0 RID: 5568 RVA: 0x00065170 File Offset: 0x00063370
	// (set) Token: 0x060015C1 RID: 5569 RVA: 0x0006518C File Offset: 0x0006338C
	public dfControl VirtualScrollingTile
	{
		get
		{
			return (!this.useVirtualScrolling) ? null : this.virtualScrollingTile;
		}
		set
		{
			this.virtualScrollingTile = ((!this.useVirtualScrolling) ? null : value);
		}
	}

	// Token: 0x060015C2 RID: 5570 RVA: 0x000651A8 File Offset: 0x000633A8
	protected internal override RectOffset GetClipPadding()
	{
		return this.scrollPadding ?? dfRectOffsetExtensions.Empty;
	}

	// Token: 0x060015C3 RID: 5571 RVA: 0x000651BC File Offset: 0x000633BC
	protected internal override Plane[] GetClippingPlanes()
	{
		if (!base.ClipChildren)
		{
			return null;
		}
		Vector3[] corners = base.GetCorners();
		Vector3 vector = base.transform.TransformDirection(Vector3.right);
		Vector3 vector2 = base.transform.TransformDirection(Vector3.left);
		Vector3 vector3 = base.transform.TransformDirection(Vector3.up);
		Vector3 vector4 = base.transform.TransformDirection(Vector3.down);
		float num = base.PixelsToUnits();
		RectOffset rectOffset = this.ScrollPadding;
		corners[0] += vector * (float)rectOffset.left * num + vector4 * (float)rectOffset.top * num;
		corners[1] += vector2 * (float)rectOffset.right * num + vector4 * (float)rectOffset.top * num;
		corners[2] += vector * (float)rectOffset.left * num + vector3 * (float)rectOffset.bottom * num;
		return new Plane[]
		{
			new Plane(vector, corners[0]),
			new Plane(vector2, corners[1]),
			new Plane(vector3, corners[2]),
			new Plane(vector4, corners[0])
		};
	}

	// Token: 0x170004C4 RID: 1220
	// (get) Token: 0x060015C4 RID: 5572 RVA: 0x00065388 File Offset: 0x00063588
	public override bool CanFocus
	{
		get
		{
			return (base.IsEnabled && base.IsVisible) || base.CanFocus;
		}
	}

	// Token: 0x060015C5 RID: 5573 RVA: 0x000653A8 File Offset: 0x000635A8
	public override void OnDestroy()
	{
		if (this.horzScroll != null)
		{
			this.horzScroll.ValueChanged -= this.horzScroll_ValueChanged;
		}
		if (this.vertScroll != null)
		{
			this.vertScroll.ValueChanged -= this.vertScroll_ValueChanged;
		}
		this.horzScroll = null;
		this.vertScroll = null;
	}

	// Token: 0x060015C6 RID: 5574 RVA: 0x00065414 File Offset: 0x00063614
	public override void Update()
	{
		base.Update();
		if (this.useScrollMomentum && !this.isMouseDown && this.scrollMomentum.magnitude > 0.25f)
		{
			this.ScrollPosition += this.scrollMomentum;
			this.scrollMomentum *= 0.95f - BraveTime.DeltaTime;
		}
		if (this.isControlInvalidated && this.autoLayout && base.IsVisible)
		{
			this.AutoArrange();
			this.updateScrollbars();
		}
	}

	// Token: 0x060015C7 RID: 5575 RVA: 0x000654B4 File Offset: 0x000636B4
	public override void LateUpdate()
	{
		base.LateUpdate();
		if (this.LockScrollPanelToZero)
		{
			this.ScrollPosition = Vector2.zero;
		}
		this.initialize();
		if (this.resetNeeded && base.IsVisible)
		{
			this.resetNeeded = false;
			if (this.autoReset || this.autoLayout)
			{
				this.Reset();
			}
		}
	}

	// Token: 0x060015C8 RID: 5576 RVA: 0x0006551C File Offset: 0x0006371C
	public override void OnEnable()
	{
		base.OnEnable();
		if (this.size == Vector2.zero)
		{
			base.SuspendLayout();
			Camera camera = base.GetCamera();
			base.Size = new Vector3((float)(camera.pixelWidth / 2), (float)(camera.pixelHeight / 2));
			base.ResumeLayout();
		}
		if (this.autoLayout)
		{
			this.AutoArrange();
		}
		this.updateScrollbars();
	}

	// Token: 0x060015C9 RID: 5577 RVA: 0x00065590 File Offset: 0x00063790
	protected internal override void OnIsVisibleChanged()
	{
		base.OnIsVisibleChanged();
		if (base.IsVisible && (this.autoReset || this.autoLayout))
		{
			this.Reset();
			this.updateScrollbars();
		}
	}

	// Token: 0x060015CA RID: 5578 RVA: 0x000655C8 File Offset: 0x000637C8
	protected internal override void OnSizeChanged()
	{
		base.OnSizeChanged();
		if (this.autoReset || this.autoLayout)
		{
			this.Reset();
			return;
		}
		Vector2 vector = this.calculateMinChildPosition();
		if (vector.x > (float)this.scrollPadding.left || vector.y > (float)this.scrollPadding.top)
		{
			vector -= new Vector2((float)this.scrollPadding.left, (float)this.scrollPadding.top);
			vector = Vector2.Max(vector, Vector2.zero);
			this.scrollChildControls(vector);
		}
		this.updateScrollbars();
	}

	// Token: 0x060015CB RID: 5579 RVA: 0x00065674 File Offset: 0x00063874
	protected internal override void OnResolutionChanged(Vector2 previousResolution, Vector2 currentResolution)
	{
		base.OnResolutionChanged(previousResolution, currentResolution);
		this.resetNeeded = this.AutoLayout || this.AutoReset;
	}

	// Token: 0x060015CC RID: 5580 RVA: 0x00065698 File Offset: 0x00063898
	protected internal override void OnGotFocus(dfFocusEventArgs args)
	{
		if (args.Source != this && args.AllowScrolling && InputManager.ActiveDevice != null)
		{
			this.ScrollIntoView(args.Source);
		}
		base.OnGotFocus(args);
	}

	// Token: 0x060015CD RID: 5581 RVA: 0x000656D4 File Offset: 0x000638D4
	protected internal override void OnKeyDown(dfKeyEventArgs args)
	{
		if (!this.scrollWithArrowKeys || args.Used)
		{
			base.OnKeyDown(args);
			return;
		}
		float num = ((!(this.horzScroll != null)) ? 1f : this.horzScroll.IncrementAmount);
		float num2 = ((!(this.vertScroll != null)) ? 1f : this.vertScroll.IncrementAmount);
		if (args.KeyCode == KeyCode.LeftArrow)
		{
			this.ScrollPosition += new Vector2(-num, 0f);
			args.Use();
		}
		else if (args.KeyCode == KeyCode.RightArrow)
		{
			this.ScrollPosition += new Vector2(num, 0f);
			args.Use();
		}
		else if (args.KeyCode == KeyCode.UpArrow)
		{
			this.ScrollPosition += new Vector2(0f, -num2);
			args.Use();
		}
		else if (args.KeyCode == KeyCode.DownArrow)
		{
			this.ScrollPosition += new Vector2(0f, num2);
			args.Use();
		}
		base.OnKeyDown(args);
	}

	// Token: 0x060015CE RID: 5582 RVA: 0x00065830 File Offset: 0x00063A30
	protected internal override void OnMouseEnter(dfMouseEventArgs args)
	{
		base.OnMouseEnter(args);
		this.touchStartPosition = args.Position;
	}

	// Token: 0x060015CF RID: 5583 RVA: 0x00065848 File Offset: 0x00063A48
	protected internal override void OnMouseDown(dfMouseEventArgs args)
	{
		base.OnMouseDown(args);
		this.scrollMomentum = Vector2.zero;
		this.touchStartPosition = args.Position;
		this.isMouseDown = this.IsInteractive;
	}

	// Token: 0x060015D0 RID: 5584 RVA: 0x00065874 File Offset: 0x00063A74
	internal override void OnDragStart(dfDragEventArgs args)
	{
		base.OnDragStart(args);
		this.scrollMomentum = Vector2.zero;
		if (args.Used)
		{
			this.isMouseDown = false;
		}
	}

	// Token: 0x060015D1 RID: 5585 RVA: 0x0006589C File Offset: 0x00063A9C
	protected internal override void OnMouseUp(dfMouseEventArgs args)
	{
		base.OnMouseUp(args);
		this.isMouseDown = false;
	}

	// Token: 0x060015D2 RID: 5586 RVA: 0x000658AC File Offset: 0x00063AAC
	protected internal override void OnMouseMove(dfMouseEventArgs args)
	{
		if ((args is dfTouchEventArgs || this.isMouseDown) && !args.Used && (args.Position - this.touchStartPosition).magnitude > 5f)
		{
			Vector2 vector = args.MoveDelta.Scale(-1f, 1f);
			dfGUIManager manager = base.GetManager();
			Vector2 screenSize = manager.GetScreenSize();
			Camera renderCamera = manager.RenderCamera;
			vector.x = screenSize.x * (vector.x / (float)renderCamera.pixelWidth);
			vector.y = screenSize.y * (vector.y / (float)renderCamera.pixelHeight);
			this.ScrollPosition += vector;
			this.scrollMomentum = (this.scrollMomentum + vector) * 0.5f;
			args.Use();
		}
		base.OnMouseMove(args);
	}

	// Token: 0x060015D3 RID: 5587 RVA: 0x000659A4 File Offset: 0x00063BA4
	protected internal override void OnMouseWheel(dfMouseEventArgs args)
	{
		try
		{
			if (!args.Used)
			{
				float num = ((this.wheelDirection != dfControlOrientation.Horizontal) ? ((!(this.vertScroll != null)) ? ((float)this.scrollWheelAmount) : this.vertScroll.IncrementAmount) : ((!(this.horzScroll != null)) ? ((float)this.scrollWheelAmount) : this.horzScroll.IncrementAmount));
				if (this.wheelDirection == dfControlOrientation.Horizontal)
				{
					this.ScrollPosition = new Vector2(this.scrollPosition.x - num * args.WheelDelta, this.scrollPosition.y);
					this.scrollMomentum = new Vector2(-num * args.WheelDelta, 0f);
				}
				else
				{
					this.ScrollPosition = new Vector2(this.scrollPosition.x, this.scrollPosition.y - num * args.WheelDelta);
					this.scrollMomentum = new Vector2(0f, -num * args.WheelDelta);
				}
				args.Use();
				base.Signal("OnMouseWheel", this, args);
			}
		}
		finally
		{
			base.OnMouseWheel(args);
		}
	}

	// Token: 0x060015D4 RID: 5588 RVA: 0x00065AF8 File Offset: 0x00063CF8
	protected internal override void OnControlAdded(dfControl child)
	{
		base.OnControlAdded(child);
		this.attachEvents(child);
		if (this.autoLayout)
		{
			this.AutoArrange();
		}
	}

	// Token: 0x060015D5 RID: 5589 RVA: 0x00065B1C File Offset: 0x00063D1C
	protected internal override void OnControlRemoved(dfControl child)
	{
		if (GameManager.IsShuttingDown)
		{
			return;
		}
		base.OnControlRemoved(child);
		if (child != null)
		{
			this.detachEvents(child);
		}
		if (this.autoLayout)
		{
			this.AutoArrange();
		}
		else
		{
			this.updateScrollbars();
		}
	}

	// Token: 0x060015D6 RID: 5590 RVA: 0x00065B6C File Offset: 0x00063D6C
	protected override void OnRebuildRenderData()
	{
		if (this.Atlas == null || string.IsNullOrEmpty(this.backgroundSprite))
		{
			return;
		}
		dfAtlas.ItemInfo itemInfo = this.Atlas[this.backgroundSprite];
		if (itemInfo == null)
		{
			return;
		}
		this.renderData.Material = this.Atlas.Material;
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

	// Token: 0x060015D7 RID: 5591 RVA: 0x00065C88 File Offset: 0x00063E88
	protected internal void OnScrollPositionChanged()
	{
		this.Invalidate();
		base.SignalHierarchy("OnScrollPositionChanged", new object[] { this, this.ScrollPosition });
		if (this.ScrollPositionChanged != null)
		{
			this.ScrollPositionChanged(this, this.ScrollPosition);
		}
	}

	// Token: 0x060015D8 RID: 5592 RVA: 0x00065CDC File Offset: 0x00063EDC
	public void FitToContents()
	{
		if (this.controls.Count == 0)
		{
			return;
		}
		Vector2 vector = Vector2.zero;
		for (int i = 0; i < this.controls.Count; i++)
		{
			dfControl dfControl = this.controls[i];
			Vector2 vector2 = dfControl.RelativePosition + dfControl.Size;
			vector = Vector2.Max(vector, vector2);
		}
		base.Size = vector + new Vector2((float)this.scrollPadding.right, (float)this.scrollPadding.bottom);
	}

	// Token: 0x060015D9 RID: 5593 RVA: 0x00065D74 File Offset: 0x00063F74
	public void CenterChildControls()
	{
		if (this.controls.Count == 0)
		{
			return;
		}
		Vector2 vector = Vector2.one * float.MaxValue;
		Vector2 vector2 = Vector2.one * float.MinValue;
		for (int i = 0; i < this.controls.Count; i++)
		{
			dfControl dfControl = this.controls[i];
			Vector2 vector3 = dfControl.RelativePosition;
			Vector2 vector4 = vector3 + dfControl.Size;
			vector = Vector2.Min(vector, vector3);
			vector2 = Vector2.Max(vector2, vector4);
		}
		Vector2 vector5 = vector2 - vector;
		Vector2 vector6 = (base.Size - vector5) * 0.5f;
		for (int j = 0; j < this.controls.Count; j++)
		{
			dfControl dfControl2 = this.controls[j];
			dfControl2.RelativePosition = dfControl2.RelativePosition - vector + vector6;
		}
	}

	// Token: 0x060015DA RID: 5594 RVA: 0x00065E80 File Offset: 0x00064080
	public void ScrollToTop()
	{
		this.scrollMomentum = Vector2.zero;
		this.ScrollPosition = new Vector2(this.scrollPosition.x, 0f);
	}

	// Token: 0x060015DB RID: 5595 RVA: 0x00065EA8 File Offset: 0x000640A8
	public void ScrollToBottom()
	{
		this.scrollMomentum = Vector2.zero;
		this.ScrollPosition = new Vector2(this.scrollPosition.x, 2.1474836E+09f);
	}

	// Token: 0x060015DC RID: 5596 RVA: 0x00065ED0 File Offset: 0x000640D0
	public void ScrollToLeft()
	{
		this.scrollMomentum = Vector2.zero;
		this.ScrollPosition = new Vector2(0f, this.scrollPosition.y);
	}

	// Token: 0x060015DD RID: 5597 RVA: 0x00065EF8 File Offset: 0x000640F8
	public void ScrollToRight()
	{
		this.scrollMomentum = Vector2.zero;
		this.ScrollPosition = new Vector2(2.1474836E+09f, this.scrollPosition.y);
	}

	// Token: 0x060015DE RID: 5598 RVA: 0x00065F20 File Offset: 0x00064120
	public void ScrollToPrimeViewingPosition(dfControl control)
	{
		this.scrollMomentum = Vector2.zero;
		Rect rect = new Rect(this.scrollPosition.x + (float)this.scrollPadding.left, this.scrollPosition.y + (float)this.scrollPadding.top, this.size.x - (float)this.scrollPadding.horizontal, this.size.y - (float)this.scrollPadding.vertical).RoundToInt();
		Vector3 vector = control.RelativePosition;
		Vector2 size = control.Size;
		while (!this.controls.Contains(control))
		{
			control = control.Parent;
			vector += control.RelativePosition;
		}
		Rect rect2 = new Rect(this.scrollPosition.x + vector.x, this.scrollPosition.y + vector.y, size.x, size.y).RoundToInt();
		Vector2 vector2 = this.scrollPosition;
		if (rect2.center.y < rect.height / 2f)
		{
			this.ScrollToTop();
		}
		else
		{
			vector2.y = rect2.center.y - rect.height;
			this.ScrollPosition = vector2;
			this.scrollMomentum = Vector2.zero;
		}
	}

	// Token: 0x060015DF RID: 5599 RVA: 0x00066084 File Offset: 0x00064284
	public void ScrollIntoView(Vector2 positionRelativeToScrollPanelPx, Vector2 sizePx)
	{
		this.scrollMomentum = Vector2.zero;
		Rect rect = new Rect(this.scrollPosition.x + (float)this.scrollPadding.left, this.scrollPosition.y + (float)this.scrollPadding.top, this.size.x - (float)this.scrollPadding.horizontal, this.size.y - (float)this.scrollPadding.vertical).RoundToInt();
		Vector2 vector = positionRelativeToScrollPanelPx;
		Vector2 vector2 = sizePx;
		Rect rect2 = new Rect(this.scrollPosition.x + vector.x, this.scrollPosition.y + vector.y, vector2.x, vector2.y).RoundToInt();
		if (rect.Contains(rect2))
		{
			return;
		}
		Vector2 vector3 = this.scrollPosition;
		if (rect2.xMin < rect.xMin)
		{
			vector3.x = rect2.xMin - (float)this.scrollPadding.left;
		}
		else if (rect2.xMax > rect.xMax)
		{
			vector3.x = rect2.xMax - Mathf.Max(this.size.x, vector2.x) + (float)this.scrollPadding.horizontal;
		}
		if (rect2.y < rect.y)
		{
			vector3.y = rect2.yMin - (float)this.scrollPadding.top;
		}
		else if (rect2.yMax > rect.yMax)
		{
			vector3.y = rect2.yMax - Mathf.Max(this.size.y, vector2.y) + (float)this.scrollPadding.vertical;
		}
		this.ScrollPosition = vector3;
		this.scrollMomentum = Vector2.zero;
	}

	// Token: 0x060015E0 RID: 5600 RVA: 0x00066264 File Offset: 0x00064464
	public void ScrollIntoView(dfControl control)
	{
		this.scrollMomentum = Vector2.zero;
		Rect rect = new Rect(this.scrollPosition.x + (float)this.scrollPadding.left, this.scrollPosition.y, this.size.x - (float)this.scrollPadding.horizontal, this.size.y).RoundToInt();
		Vector3 vector = control.RelativePosition;
		Vector2 size = control.Size;
		while (!this.controls.Contains(control))
		{
			control = control.Parent;
			vector += control.RelativePosition;
		}
		Rect rect2 = new Rect(this.scrollPosition.x + vector.x, this.scrollPosition.y + vector.y, size.x, size.y).RoundToInt();
		if (rect.Contains(rect2))
		{
			return;
		}
		Vector2 vector2 = this.scrollPosition;
		if (rect2.xMin < rect.xMin)
		{
			vector2.x = rect2.xMin - (float)this.scrollPadding.left;
		}
		else if (rect2.xMax > rect.xMax)
		{
			vector2.x = rect2.xMax - Mathf.Max(this.size.x, size.x) + (float)this.scrollPadding.horizontal;
		}
		if (rect2.y < rect.y)
		{
			vector2.y = rect2.yMin - (float)this.autoScrollPadding.top;
		}
		else if (rect2.yMax > rect.yMax)
		{
			vector2.y = rect2.yMax - Mathf.Max(this.size.y, size.y) + (float)this.autoScrollPadding.vertical;
		}
		this.ScrollPosition = vector2;
		this.scrollMomentum = Vector2.zero;
	}

	// Token: 0x060015E1 RID: 5601 RVA: 0x00066460 File Offset: 0x00064660
	public void Reset()
	{
		try
		{
			base.SuspendLayout();
			if (this.autoLayout)
			{
				Vector2 vector = this.ScrollPosition;
				this.ScrollPosition = Vector2.zero;
				this.AutoArrange();
				this.ScrollPosition = vector;
			}
			else
			{
				this.scrollPadding = this.ScrollPadding.ConstrainPadding();
				Vector3 vector2 = this.calculateMinChildPosition();
				vector2 -= new Vector3((float)this.scrollPadding.left, (float)this.scrollPadding.top);
				for (int i = 0; i < this.controls.Count; i++)
				{
					this.controls[i].RelativePosition -= vector2;
				}
				this.scrollPosition = Vector2.zero;
			}
			this.Invalidate();
			this.updateScrollbars();
		}
		finally
		{
			base.ResumeLayout();
		}
	}

	// Token: 0x060015E2 RID: 5602 RVA: 0x00066550 File Offset: 0x00064750
	private void Virtualize<T>(IList<T> backingList, int startIndex)
	{
		if (!this.useVirtualScrolling)
		{
			UnityEngine.Debug.LogError("Virtual scrolling not enabled for this dfScrollPanel: " + base.name);
			return;
		}
		if (this.virtualScrollingTile == null)
		{
			UnityEngine.Debug.LogError("Virtual scrolling cannot be started without assigning VirtualScrollingTile: " + base.name);
			return;
		}
		if (backingList.Count == 0)
		{
		}
		dfVirtualScrollData<T> dfVirtualScrollData = this.GetVirtualScrollData<T>() ?? this.initVirtualScrollData<T>(backingList);
		bool flag = this.isVerticalFlow();
		RectOffset rectOffset = (dfVirtualScrollData.ItemPadding = new RectOffset(this.FlowPadding.left, this.FlowPadding.right, this.FlowPadding.top, this.FlowPadding.bottom));
		int num = ((!flag) ? rectOffset.horizontal : rectOffset.vertical);
		int num2 = ((!flag) ? rectOffset.left : rectOffset.top);
		float num3 = ((!flag) ? base.Width : base.Height);
		this.AutoLayout = false;
		this.AutoReset = false;
		IDFVirtualScrollingTile idfvirtualScrollingTile;
		if ((idfvirtualScrollingTile = dfVirtualScrollData.DummyTop) == null)
		{
			idfvirtualScrollingTile = (dfVirtualScrollData.DummyTop = this.initTile(rectOffset));
		}
		IDFVirtualScrollingTile idfvirtualScrollingTile2 = idfvirtualScrollingTile;
		dfPanel dfPanel = idfvirtualScrollingTile2.GetDfPanel();
		float num4 = ((!flag) ? idfvirtualScrollingTile2.GetDfPanel().Width : idfvirtualScrollingTile2.GetDfPanel().Height);
		dfPanel.IsEnabled = false;
		dfPanel.Opacity = 0f;
		dfPanel.gameObject.hideFlags = HideFlags.HideInHierarchy;
		dfScrollbar dfScrollbar;
		if ((dfScrollbar = this.VertScrollbar) || (dfScrollbar = this.HorzScrollbar))
		{
			IDFVirtualScrollingTile idfvirtualScrollingTile3;
			if ((idfvirtualScrollingTile3 = dfVirtualScrollData.DummyBottom) == null)
			{
				idfvirtualScrollingTile3 = (dfVirtualScrollData.DummyBottom = this.initTile(rectOffset));
			}
			IDFVirtualScrollingTile idfvirtualScrollingTile4 = idfvirtualScrollingTile3;
			dfPanel dfPanel2 = idfvirtualScrollingTile4.GetDfPanel();
			float num5 = ((!flag) ? dfPanel.RelativePosition.x : dfPanel.RelativePosition.y);
			float num6 = num5 + ((float)(backingList.Count - 1) * (num4 + (float)num) + (float)num2);
			dfPanel2.RelativePosition = ((!flag) ? new Vector3(num6, dfPanel.RelativePosition.y) : new Vector3(dfPanel.RelativePosition.x, num6));
			dfPanel2.IsEnabled = dfPanel.IsEnabled;
			dfPanel2.gameObject.hideFlags = dfPanel.hideFlags;
			dfPanel2.Opacity = dfPanel.Opacity;
			if (startIndex == 0 && dfScrollbar.MaxValue != 0f)
			{
				float num7 = dfScrollbar.Value / dfScrollbar.MaxValue;
				startIndex = Mathf.RoundToInt(num7 * (float)(backingList.Count - 1));
			}
			dfScrollbar.Value = (float)startIndex * (num4 + (float)num);
		}
		float num8 = num3 / (num4 + (float)num);
		int num9 = Mathf.RoundToInt(num8);
		int num10 = (((float)num9 <= num8) ? (num9 + 2) : (num9 + 1));
		float num11 = (float)num2;
		float num12 = (float)startIndex;
		int num13 = 0;
		while (num13 < num10 && num13 < backingList.Count && startIndex <= backingList.Count)
		{
			try
			{
				IDFVirtualScrollingTile idfvirtualScrollingTile5 = ((!dfVirtualScrollData.IsInitialized || dfVirtualScrollData.Tiles.Count < num13 + 1) ? this.initTile(rectOffset) : dfVirtualScrollData.Tiles[num13]);
				dfPanel dfPanel3 = idfvirtualScrollingTile5.GetDfPanel();
				float num14 = num11;
				dfPanel3.RelativePosition = ((!flag) ? new Vector2(num14, (float)rectOffset.top) : new Vector2((float)rectOffset.left, num14));
				num11 = num14 + num4 + (float)num;
				if (!dfVirtualScrollData.Tiles.Contains(idfvirtualScrollingTile5))
				{
					dfVirtualScrollData.Tiles.Add(idfvirtualScrollingTile5);
				}
				idfvirtualScrollingTile5.VirtualScrollItemIndex = startIndex;
				idfvirtualScrollingTile5.OnScrollPanelItemVirtualize(backingList[startIndex]);
				startIndex++;
			}
			catch
			{
				foreach (IDFVirtualScrollingTile idfvirtualScrollingTile6 in dfVirtualScrollData.Tiles)
				{
					int num15 = idfvirtualScrollingTile6.VirtualScrollItemIndex - 1;
					idfvirtualScrollingTile6.VirtualScrollItemIndex = num15;
					idfvirtualScrollingTile6.OnScrollPanelItemVirtualize(backingList[num15]);
				}
			}
			num13++;
		}
		if (num12 != 0f && this.ScrollPositionChanged != null)
		{
			this.ScrollPositionChanged -= this.virtualScrollPositionChanged<T>;
		}
		dfVirtualScrollData.IsInitialized = true;
		this.ScrollPositionChanged += this.virtualScrollPositionChanged<T>;
	}

	// Token: 0x060015E3 RID: 5603 RVA: 0x00066A18 File Offset: 0x00064C18
	public void Virtualize<T>(IList<T> backingList, dfPanel tile)
	{
		MonoBehaviour monoBehaviour = tile.GetComponents<MonoBehaviour>().FirstOrDefault((MonoBehaviour t) => t is IDFVirtualScrollingTile);
		if (!monoBehaviour)
		{
			UnityEngine.Debug.LogError("The tile you've chosen does not implement IDFVirtualScrollingTile!");
			return;
		}
		this.UseVirtualScrolling = true;
		this.VirtualScrollingTile = tile;
		this.Virtualize<T>(backingList, 0);
	}

	// Token: 0x060015E4 RID: 5604 RVA: 0x00066A6C File Offset: 0x00064C6C
	public void Virtualize<T>(IList<T> backingList)
	{
		this.Virtualize<T>(backingList, 0);
	}

	// Token: 0x060015E5 RID: 5605 RVA: 0x00066A78 File Offset: 0x00064C78
	public void ResetVirtualScrollingData()
	{
		this.virtualScrollData = null;
		foreach (dfControl dfControl in this.controls.ToArray())
		{
			base.RemoveControl(dfControl);
			UnityEngine.Object.Destroy(dfControl.gameObject);
		}
		this.ScrollPosition = Vector2.zero;
	}

	// Token: 0x060015E6 RID: 5606 RVA: 0x00066AD0 File Offset: 0x00064CD0
	public dfVirtualScrollData<T> GetVirtualScrollData<T>()
	{
		return (dfVirtualScrollData<T>)this.virtualScrollData;
	}

	// Token: 0x060015E7 RID: 5607 RVA: 0x00066AE0 File Offset: 0x00064CE0
	[HideInInspector]
	private void AutoArrange()
	{
		base.SuspendLayout();
		try
		{
			this.scrollPadding = this.ScrollPadding.ConstrainPadding();
			this.flowPadding = this.FlowPadding.ConstrainPadding();
			float num = (float)this.scrollPadding.left + (float)this.flowPadding.left - this.scrollPosition.x;
			float num2 = (float)this.scrollPadding.top + (float)this.flowPadding.top - this.scrollPosition.y;
			float num3 = 0f;
			float num4 = 0f;
			for (int i = 0; i < this.controls.Count; i++)
			{
				dfControl dfControl = this.controls[i];
				if (dfControl && dfControl.GetIsVisibleRaw() && dfControl.enabled && dfControl.gameObject.activeSelf)
				{
					if (!(dfControl == this.horzScroll) && !(dfControl == this.vertScroll))
					{
						if (this.wrapLayout)
						{
							if (this.flowDirection == dfScrollPanel.LayoutDirection.Horizontal)
							{
								if (num + dfControl.Width >= this.size.x - (float)this.scrollPadding.right)
								{
									num = (float)this.scrollPadding.left + (float)this.flowPadding.left;
									num2 += num4;
									num4 = 0f;
								}
							}
							else if (num2 + dfControl.Height + (float)this.flowPadding.vertical >= this.size.y - (float)this.scrollPadding.bottom)
							{
								num2 = (float)this.scrollPadding.top + (float)this.flowPadding.top;
								num += num3;
								num3 = 0f;
							}
						}
						Vector2 vector = new Vector2(num, num2);
						dfControl.RelativePosition = vector;
						float num5 = dfControl.Width + (float)this.flowPadding.horizontal;
						float num6 = dfControl.Height + (float)this.flowPadding.vertical;
						num3 = Mathf.Max(num5, num3);
						num4 = Mathf.Max(num6, num4);
						if (this.flowDirection == dfScrollPanel.LayoutDirection.Horizontal)
						{
							num += num5;
						}
						else
						{
							num2 += num6;
						}
					}
				}
			}
			this.updateScrollbars();
		}
		finally
		{
			base.ResumeLayout();
		}
	}

	// Token: 0x060015E8 RID: 5608 RVA: 0x00066D58 File Offset: 0x00064F58
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
			if (this.horzScroll != null)
			{
				this.horzScroll.ValueChanged += this.horzScroll_ValueChanged;
			}
			if (this.vertScroll != null)
			{
				this.vertScroll.ValueChanged += this.vertScroll_ValueChanged;
			}
		}
		if (this.resetNeeded || this.autoLayout || this.autoReset)
		{
			this.Reset();
		}
		this.Invalidate();
		this.ScrollPosition = Vector2.zero;
		this.updateScrollbars();
	}

	// Token: 0x060015E9 RID: 5609 RVA: 0x00066E10 File Offset: 0x00065010
	private void vertScroll_ValueChanged(dfControl control, float value)
	{
		this.ScrollPosition = new Vector2(this.scrollPosition.x, value);
		this.ScrollPosition = new Vector2(this.scrollPosition.x, value);
	}

	// Token: 0x060015EA RID: 5610 RVA: 0x00066E40 File Offset: 0x00065040
	private void horzScroll_ValueChanged(dfControl control, float value)
	{
		this.ScrollPosition = new Vector2(value, this.ScrollPosition.y);
	}

	// Token: 0x060015EB RID: 5611 RVA: 0x00066E68 File Offset: 0x00065068
	private void scrollChildControls(Vector3 delta)
	{
		try
		{
			this.scrolling = true;
			delta = delta.Scale(1f, -1f, 1f);
			for (int i = 0; i < this.controls.Count; i++)
			{
				dfControl dfControl = this.controls[i];
				dfControl.Position = (dfControl.Position - delta).RoundToInt();
			}
		}
		finally
		{
			this.scrolling = false;
		}
	}

	// Token: 0x060015EC RID: 5612 RVA: 0x00066EF0 File Offset: 0x000650F0
	private Vector2 calculateMinChildPosition()
	{
		float num = float.MaxValue;
		float num2 = float.MaxValue;
		for (int i = 0; i < this.controls.Count; i++)
		{
			dfControl dfControl = this.controls[i];
			if (dfControl.enabled && dfControl.gameObject.activeSelf)
			{
				Vector3 vector = dfControl.RelativePosition.FloorToInt();
				num = Mathf.Min(num, vector.x);
				num2 = Mathf.Min(num2, vector.y);
			}
		}
		return new Vector2(num, num2);
	}

	// Token: 0x060015ED RID: 5613 RVA: 0x00066F84 File Offset: 0x00065184
	private Vector2 calculateViewSize()
	{
		Vector2 vector = new Vector2((float)this.scrollPadding.horizontal, (float)this.scrollPadding.vertical).RoundToInt();
		Vector2 vector2 = base.Size.RoundToInt() - vector;
		if (!base.IsVisible || this.controls.Count == 0)
		{
			return vector2;
		}
		Vector2 vector3 = Vector2.one * float.MaxValue;
		Vector2 vector4 = Vector2.one * float.MinValue;
		for (int i = 0; i < this.controls.Count; i++)
		{
			dfControl dfControl = this.controls[i];
			if (!Application.isPlaying || dfControl.IsVisible)
			{
				Vector2 vector5 = dfControl.RelativePosition.CeilToInt();
				Vector2 vector6 = vector5 + dfControl.Size.CeilToInt();
				vector3 = Vector2.Min(vector5, vector3);
				vector4 = Vector2.Max(vector6, vector4);
			}
		}
		Vector2 vector7 = Vector2.Max(Vector2.zero, vector3 - new Vector2((float)this.scrollPadding.left, (float)this.scrollPadding.top));
		vector4 = Vector2.Max(vector4 + vector7, vector2);
		return vector4 - vector3 + vector7;
	}

	// Token: 0x060015EE RID: 5614 RVA: 0x000670D4 File Offset: 0x000652D4
	[HideInInspector]
	private void updateScrollbars()
	{
		Vector2 vector = this.calculateViewSize();
		Vector2 vector2 = base.Size - new Vector2((float)this.scrollPadding.horizontal, (float)this.scrollPadding.vertical);
		vector2.x = Mathf.Abs(vector2.x);
		vector2.y = Mathf.Abs(vector2.y);
		if (this.horzScroll != null)
		{
			this.horzScroll.MinValue = 0f;
			this.horzScroll.MaxValue = vector.x;
			this.horzScroll.ScrollSize = vector2.x;
			this.horzScroll.Value = Mathf.Max(0f, this.scrollPosition.x);
		}
		if (this.vertScroll != null)
		{
			this.vertScroll.MinValue = 0f;
			this.vertScroll.MaxValue = vector.y;
			this.vertScroll.ScrollSize = vector2.y;
			this.vertScroll.Value = Mathf.Max(0f, this.scrollPosition.y);
		}
	}

	// Token: 0x060015EF RID: 5615 RVA: 0x00067204 File Offset: 0x00065404
	private void virtualScrollPositionChanged<T>(dfControl control, Vector2 value)
	{
		dfVirtualScrollData<T> dfVirtualScrollData = this.GetVirtualScrollData<T>();
		if (dfVirtualScrollData == null)
		{
			return;
		}
		IList<T> backingList = dfVirtualScrollData.BackingList;
		RectOffset itemPadding = dfVirtualScrollData.ItemPadding;
		List<IDFVirtualScrollingTile> tiles = dfVirtualScrollData.Tiles;
		bool flag = this.isVerticalFlow();
		float num = ((!flag) ? (value.x - dfVirtualScrollData.LastScrollPosition.x) : (value.y - dfVirtualScrollData.LastScrollPosition.y));
		dfVirtualScrollData.LastScrollPosition = value;
		bool flag2 = Mathf.Abs(num) > base.Height;
		if (flag2 && (this.VertScrollbar || this.HorzScrollbar))
		{
			float num2 = ((!flag) ? (value.x / this.HorzScrollbar.MaxValue) : (value.y / this.VertScrollbar.MaxValue));
			int num3 = Mathf.RoundToInt(num2 * (float)(backingList.Count - 1));
			this.Virtualize<T>(backingList, num3);
			return;
		}
		foreach (IDFVirtualScrollingTile idfvirtualScrollingTile in tiles)
		{
			int num4 = 0;
			float num5 = 0f;
			bool flag3 = false;
			dfPanel dfPanel = idfvirtualScrollingTile.GetDfPanel();
			float num6 = ((!flag) ? dfPanel.RelativePosition.x : dfPanel.RelativePosition.y);
			float num7 = ((!flag) ? dfPanel.Width : dfPanel.Height);
			float num8 = ((!flag) ? base.Width : base.Height);
			if (num > 0f)
			{
				if (num6 + num7 > 0f)
				{
					continue;
				}
				dfVirtualScrollData.GetNewLimits(flag, true, out num4, out num5);
				if (num4 >= backingList.Count)
				{
					continue;
				}
				flag3 = true;
				dfPanel.RelativePosition = ((!flag) ? new Vector3(num5 + num7 + (float)itemPadding.horizontal, dfPanel.RelativePosition.y) : new Vector3(dfPanel.RelativePosition.x, num5 + num7 + (float)itemPadding.vertical));
			}
			else if (num < 0f)
			{
				if (num6 < num8)
				{
					continue;
				}
				dfVirtualScrollData.GetNewLimits(flag, false, out num4, out num5);
				if (num4 < 0)
				{
					continue;
				}
				flag3 = true;
				dfPanel.RelativePosition = ((!flag) ? new Vector3(num5 - (num7 + (float)itemPadding.horizontal), dfPanel.RelativePosition.y) : new Vector3(dfPanel.RelativePosition.x, num5 - (num7 + (float)itemPadding.vertical)));
			}
			if (flag3)
			{
				idfvirtualScrollingTile.VirtualScrollItemIndex = num4;
				idfvirtualScrollingTile.OnScrollPanelItemVirtualize(backingList[num4]);
			}
		}
	}

	// Token: 0x060015F0 RID: 5616 RVA: 0x00067520 File Offset: 0x00065720
	private dfVirtualScrollData<T> initVirtualScrollData<T>(IList<T> backingList)
	{
		dfVirtualScrollData<T> dfVirtualScrollData = new dfVirtualScrollData<T>
		{
			BackingList = backingList
		};
		this.virtualScrollData = dfVirtualScrollData;
		return dfVirtualScrollData;
	}

	// Token: 0x060015F1 RID: 5617 RVA: 0x00067544 File Offset: 0x00065744
	private IDFVirtualScrollingTile initTile(RectOffset padding)
	{
		MonoBehaviour monoBehaviour = this.virtualScrollingTile.GetComponents<MonoBehaviour>().FirstOrDefault((MonoBehaviour p) => p is IDFVirtualScrollingTile);
		IDFVirtualScrollingTile idfvirtualScrollingTile = (IDFVirtualScrollingTile)UnityEngine.Object.Instantiate<MonoBehaviour>(monoBehaviour);
		dfPanel dfPanel = idfvirtualScrollingTile.GetDfPanel();
		bool flag = this.isVerticalFlow();
		base.AddControl(dfPanel);
		if (this.AutoFitVirtualTiles)
		{
			if (flag)
			{
				dfPanel.Width = base.Width - (float)padding.horizontal;
			}
			else
			{
				dfPanel.Height = base.Height - (float)padding.vertical;
			}
		}
		dfPanel.RelativePosition = new Vector3((float)padding.left, (float)padding.top);
		return idfvirtualScrollingTile;
	}

	// Token: 0x060015F2 RID: 5618 RVA: 0x000675F8 File Offset: 0x000657F8
	private bool isVerticalFlow()
	{
		return this.FlowDirection == dfScrollPanel.LayoutDirection.Vertical;
	}

	// Token: 0x060015F3 RID: 5619 RVA: 0x00067604 File Offset: 0x00065804
	private void attachEvents(dfControl control)
	{
		control.IsVisibleChanged += this.childIsVisibleChanged;
		control.PositionChanged += this.childControlInvalidated;
		control.SizeChanged += this.childControlInvalidated;
		control.ZOrderChanged += this.childOrderChanged;
	}

	// Token: 0x060015F4 RID: 5620 RVA: 0x0006765C File Offset: 0x0006585C
	private void detachEvents(dfControl control)
	{
		control.IsVisibleChanged -= this.childIsVisibleChanged;
		control.PositionChanged -= this.childControlInvalidated;
		control.SizeChanged -= this.childControlInvalidated;
		control.ZOrderChanged -= this.childOrderChanged;
	}

	// Token: 0x060015F5 RID: 5621 RVA: 0x000676B4 File Offset: 0x000658B4
	private void childOrderChanged(dfControl control, int value)
	{
		this.onChildControlInvalidatedLayout();
	}

	// Token: 0x060015F6 RID: 5622 RVA: 0x000676BC File Offset: 0x000658BC
	private void childIsVisibleChanged(dfControl control, bool value)
	{
		this.onChildControlInvalidatedLayout();
	}

	// Token: 0x060015F7 RID: 5623 RVA: 0x000676C4 File Offset: 0x000658C4
	private void childControlInvalidated(dfControl control, Vector2 value)
	{
		this.onChildControlInvalidatedLayout();
	}

	// Token: 0x060015F8 RID: 5624 RVA: 0x000676CC File Offset: 0x000658CC
	[HideInInspector]
	private void onChildControlInvalidatedLayout()
	{
		if (this.scrolling || base.IsLayoutSuspended)
		{
			return;
		}
		if (this.autoLayout)
		{
			this.AutoArrange();
		}
		this.updateScrollbars();
		this.Invalidate();
	}

	// Token: 0x04001247 RID: 4679
	[SerializeField]
	protected dfAtlas atlas;

	// Token: 0x04001248 RID: 4680
	[SerializeField]
	protected string backgroundSprite;

	// Token: 0x04001249 RID: 4681
	[SerializeField]
	protected Color32 backgroundColor = UnityEngine.Color.white;

	// Token: 0x0400124A RID: 4682
	[SerializeField]
	protected bool autoReset = true;

	// Token: 0x0400124B RID: 4683
	[SerializeField]
	protected bool autoLayout;

	// Token: 0x0400124C RID: 4684
	[SerializeField]
	protected RectOffset scrollPadding = new RectOffset();

	// Token: 0x0400124D RID: 4685
	[SerializeField]
	protected RectOffset autoScrollPadding = new RectOffset();

	// Token: 0x0400124E RID: 4686
	[SerializeField]
	protected RectOffset flowPadding = new RectOffset();

	// Token: 0x0400124F RID: 4687
	[SerializeField]
	protected dfScrollPanel.LayoutDirection flowDirection;

	// Token: 0x04001250 RID: 4688
	[SerializeField]
	protected bool wrapLayout;

	// Token: 0x04001251 RID: 4689
	[SerializeField]
	protected Vector2 scrollPosition = Vector2.zero;

	// Token: 0x04001252 RID: 4690
	[SerializeField]
	protected int scrollWheelAmount = 10;

	// Token: 0x04001253 RID: 4691
	[SerializeField]
	protected dfScrollbar horzScroll;

	// Token: 0x04001254 RID: 4692
	[SerializeField]
	protected dfScrollbar vertScroll;

	// Token: 0x04001255 RID: 4693
	[SerializeField]
	protected dfControlOrientation wheelDirection;

	// Token: 0x04001256 RID: 4694
	[SerializeField]
	protected bool scrollWithArrowKeys;

	// Token: 0x04001257 RID: 4695
	[SerializeField]
	protected bool useScrollMomentum;

	// Token: 0x04001258 RID: 4696
	[SerializeField]
	protected bool useVirtualScrolling;

	// Token: 0x04001259 RID: 4697
	[SerializeField]
	protected bool autoFitVirtualTiles = true;

	// Token: 0x0400125A RID: 4698
	[SerializeField]
	protected dfControl virtualScrollingTile;

	// Token: 0x0400125B RID: 4699
	public bool LockScrollPanelToZero;

	// Token: 0x0400125C RID: 4700
	private bool initialized;

	// Token: 0x0400125D RID: 4701
	private bool resetNeeded;

	// Token: 0x0400125E RID: 4702
	private bool scrolling;

	// Token: 0x0400125F RID: 4703
	private bool isMouseDown;

	// Token: 0x04001260 RID: 4704
	private Vector2 touchStartPosition = Vector2.zero;

	// Token: 0x04001261 RID: 4705
	private Vector2 scrollMomentum = Vector2.zero;

	// Token: 0x04001262 RID: 4706
	private object virtualScrollData;

	// Token: 0x020003F3 RID: 1011
	public enum LayoutDirection
	{
		// Token: 0x04001265 RID: 4709
		Horizontal,
		// Token: 0x04001266 RID: 4710
		Vertical
	}
}
