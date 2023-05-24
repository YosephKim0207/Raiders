using System;
using System.Diagnostics;
using UnityEngine;

// Token: 0x02000403 RID: 1027
[dfTooltip("Used in conjunction with the dfTabContainer class to implement tabbed containers. This control maintains the tabs that are displayed for the user to select, and the dfTabContainer class manages the display of the tab pages themselves.")]
[dfCategory("Basic Controls")]
[AddComponentMenu("Daikon Forge/User Interface/Containers/Tab Control/Tab Strip")]
[ExecuteInEditMode]
[dfHelp("http://www.daikonforge.com/docs/df-gui/classdf_tabstrip.html")]
[Serializable]
public class dfTabstrip : dfControl
{
	// Token: 0x1400003F RID: 63
	// (add) Token: 0x0600169B RID: 5787 RVA: 0x0006B230 File Offset: 0x00069430
	// (remove) Token: 0x0600169C RID: 5788 RVA: 0x0006B268 File Offset: 0x00069468
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event PropertyChangedEventHandler<int> SelectedIndexChanged;

	// Token: 0x170004E0 RID: 1248
	// (get) Token: 0x0600169D RID: 5789 RVA: 0x0006B2A0 File Offset: 0x000694A0
	// (set) Token: 0x0600169E RID: 5790 RVA: 0x0006B2A8 File Offset: 0x000694A8
	public dfTabContainer TabPages
	{
		get
		{
			return this.pageContainer;
		}
		set
		{
			if (this.pageContainer != value)
			{
				this.pageContainer = value;
				if (value != null)
				{
					while (value.Controls.Count < this.controls.Count)
					{
						value.AddTabPage();
					}
				}
				this.pageContainer.SelectedIndex = this.SelectedIndex;
				this.Invalidate();
			}
		}
	}

	// Token: 0x170004E1 RID: 1249
	// (get) Token: 0x0600169F RID: 5791 RVA: 0x0006B318 File Offset: 0x00069518
	// (set) Token: 0x060016A0 RID: 5792 RVA: 0x0006B320 File Offset: 0x00069520
	public int SelectedIndex
	{
		get
		{
			return this.selectedIndex;
		}
		set
		{
			if (value != this.selectedIndex)
			{
				this.selectTabByIndex(value);
			}
		}
	}

	// Token: 0x170004E2 RID: 1250
	// (get) Token: 0x060016A1 RID: 5793 RVA: 0x0006B338 File Offset: 0x00069538
	// (set) Token: 0x060016A2 RID: 5794 RVA: 0x0006B380 File Offset: 0x00069580
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

	// Token: 0x170004E3 RID: 1251
	// (get) Token: 0x060016A3 RID: 5795 RVA: 0x0006B3A0 File Offset: 0x000695A0
	// (set) Token: 0x060016A4 RID: 5796 RVA: 0x0006B3A8 File Offset: 0x000695A8
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

	// Token: 0x170004E4 RID: 1252
	// (get) Token: 0x060016A5 RID: 5797 RVA: 0x0006B3C8 File Offset: 0x000695C8
	// (set) Token: 0x060016A6 RID: 5798 RVA: 0x0006B3E8 File Offset: 0x000695E8
	public RectOffset LayoutPadding
	{
		get
		{
			if (this.layoutPadding == null)
			{
				this.layoutPadding = new RectOffset();
			}
			return this.layoutPadding;
		}
		set
		{
			value = value.ConstrainPadding();
			if (!object.Equals(value, this.layoutPadding))
			{
				this.layoutPadding = value;
				this.arrangeTabs();
			}
		}
	}

	// Token: 0x170004E5 RID: 1253
	// (get) Token: 0x060016A7 RID: 5799 RVA: 0x0006B410 File Offset: 0x00069610
	// (set) Token: 0x060016A8 RID: 5800 RVA: 0x0006B418 File Offset: 0x00069618
	public bool AllowKeyboardNavigation
	{
		get
		{
			return this.allowKeyboardNavigation;
		}
		set
		{
			this.allowKeyboardNavigation = value;
		}
	}

	// Token: 0x060016A9 RID: 5801 RVA: 0x0006B424 File Offset: 0x00069624
	public void EnableTab(int index)
	{
		if (this.selectedIndex >= 0 && this.selectedIndex <= this.controls.Count - 1)
		{
			this.controls[index].Enable();
		}
	}

	// Token: 0x060016AA RID: 5802 RVA: 0x0006B45C File Offset: 0x0006965C
	public void DisableTab(int index)
	{
		if (this.selectedIndex >= 0 && this.selectedIndex <= this.controls.Count - 1)
		{
			this.controls[index].Disable();
		}
	}

	// Token: 0x060016AB RID: 5803 RVA: 0x0006B494 File Offset: 0x00069694
	public dfControl AddTab(string Text)
	{
		if (Text == null)
		{
			Text = string.Empty;
		}
		dfButton dfButton = this.controls.Where((dfControl i) => i is dfButton).FirstOrDefault() as dfButton;
		string text = "Tab " + (this.controls.Count + 1);
		if (string.IsNullOrEmpty(Text))
		{
			Text = text;
		}
		dfButton dfButton2 = base.AddControl<dfButton>();
		dfButton2.name = text;
		dfButton2.Atlas = this.Atlas;
		dfButton2.Text = Text;
		dfButton2.ButtonGroup = this;
		if (dfButton != null)
		{
			dfButton2.Atlas = dfButton.Atlas;
			dfButton2.Font = dfButton.Font;
			dfButton2.AutoSize = dfButton.AutoSize;
			dfButton2.Size = dfButton.Size;
			dfButton2.BackgroundSprite = dfButton.BackgroundSprite;
			dfButton2.DisabledSprite = dfButton.DisabledSprite;
			dfButton2.FocusSprite = dfButton.FocusSprite;
			dfButton2.HoverSprite = dfButton.HoverSprite;
			dfButton2.PressedSprite = dfButton.PressedSprite;
			dfButton2.Shadow = dfButton.Shadow;
			dfButton2.ShadowColor = dfButton.ShadowColor;
			dfButton2.ShadowOffset = dfButton.ShadowOffset;
			dfButton2.TextColor = dfButton.TextColor;
			dfButton2.TextAlignment = dfButton.TextAlignment;
			RectOffset padding = dfButton.Padding;
			dfButton2.Padding = new RectOffset(padding.left, padding.right, padding.top, padding.bottom);
		}
		if (this.pageContainer != null)
		{
			this.pageContainer.AddTabPage();
		}
		this.arrangeTabs();
		this.Invalidate();
		return dfButton2;
	}

	// Token: 0x060016AC RID: 5804 RVA: 0x0006B63C File Offset: 0x0006983C
	protected internal override void OnGotFocus(dfFocusEventArgs args)
	{
		if (this.controls.Contains(args.GotFocus))
		{
			this.SelectedIndex = args.GotFocus.ZOrder;
		}
		base.OnGotFocus(args);
	}

	// Token: 0x060016AD RID: 5805 RVA: 0x0006B66C File Offset: 0x0006986C
	protected internal override void OnLostFocus(dfFocusEventArgs args)
	{
		base.OnLostFocus(args);
		if (this.controls.Contains(args.LostFocus))
		{
			this.showSelectedTab();
		}
	}

	// Token: 0x060016AE RID: 5806 RVA: 0x0006B694 File Offset: 0x00069894
	protected internal override void OnClick(dfMouseEventArgs args)
	{
		if (this.controls.Contains(args.Source))
		{
			this.SelectedIndex = args.Source.ZOrder;
		}
		base.OnClick(args);
	}

	// Token: 0x060016AF RID: 5807 RVA: 0x0006B6C4 File Offset: 0x000698C4
	private void OnClick(dfControl sender, dfMouseEventArgs args)
	{
		if (!this.controls.Contains(args.Source))
		{
			return;
		}
		this.SelectedIndex = args.Source.ZOrder;
	}

	// Token: 0x060016B0 RID: 5808 RVA: 0x0006B6F0 File Offset: 0x000698F0
	protected internal override void OnKeyDown(dfKeyEventArgs args)
	{
		if (args.Used)
		{
			return;
		}
		if (this.allowKeyboardNavigation)
		{
			if (args.KeyCode == KeyCode.LeftArrow || (args.KeyCode == KeyCode.Tab && args.Shift))
			{
				this.SelectedIndex = Mathf.Max(0, this.SelectedIndex - 1);
				args.Use();
				return;
			}
			if (args.KeyCode == KeyCode.RightArrow || args.KeyCode == KeyCode.Tab)
			{
				this.SelectedIndex++;
				args.Use();
				return;
			}
		}
		base.OnKeyDown(args);
	}

	// Token: 0x060016B1 RID: 5809 RVA: 0x0006B790 File Offset: 0x00069990
	protected internal override void OnControlAdded(dfControl child)
	{
		base.OnControlAdded(child);
		this.attachEvents(child);
		this.arrangeTabs();
	}

	// Token: 0x060016B2 RID: 5810 RVA: 0x0006B7A8 File Offset: 0x000699A8
	protected internal override void OnControlRemoved(dfControl child)
	{
		base.OnControlRemoved(child);
		this.detachEvents(child);
		this.arrangeTabs();
	}

	// Token: 0x060016B3 RID: 5811 RVA: 0x0006B7C0 File Offset: 0x000699C0
	public override void OnEnable()
	{
		base.OnEnable();
		if (this.size.sqrMagnitude < 1E-45f)
		{
			base.Size = new Vector2(256f, 26f);
		}
		if (Application.isPlaying)
		{
			this.selectTabByIndex(Mathf.Max(this.selectedIndex, 0));
		}
	}

	// Token: 0x060016B4 RID: 5812 RVA: 0x0006B81C File Offset: 0x00069A1C
	public override void Update()
	{
		base.Update();
		if (this.isControlInvalidated)
		{
			this.arrangeTabs();
		}
		this.showSelectedTab();
	}

	// Token: 0x060016B5 RID: 5813 RVA: 0x0006B83C File Offset: 0x00069A3C
	protected internal virtual void OnSelectedIndexChanged()
	{
		base.SignalHierarchy("OnSelectedIndexChanged", new object[] { this, this.SelectedIndex });
		if (this.SelectedIndexChanged != null)
		{
			this.SelectedIndexChanged(this, this.SelectedIndex);
		}
	}

	// Token: 0x060016B6 RID: 5814 RVA: 0x0006B88C File Offset: 0x00069A8C
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
		Color32 color = base.ApplyOpacity((!base.IsEnabled) ? this.disabledColor : this.color);
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

	// Token: 0x060016B7 RID: 5815 RVA: 0x0006B9C0 File Offset: 0x00069BC0
	private void showSelectedTab()
	{
		if (this.selectedIndex >= 0 && this.selectedIndex <= this.controls.Count - 1)
		{
			dfButton dfButton = this.controls[this.selectedIndex] as dfButton;
			if (dfButton != null && !dfButton.ContainsMouse)
			{
				dfButton.State = dfButton.ButtonState.Focus;
			}
		}
	}

	// Token: 0x060016B8 RID: 5816 RVA: 0x0006BA28 File Offset: 0x00069C28
	private void selectTabByIndex(int value)
	{
		value = Mathf.Max(Mathf.Min(value, this.controls.Count - 1), -1);
		if (value == this.selectedIndex)
		{
			return;
		}
		this.selectedIndex = value;
		for (int i = 0; i < this.controls.Count; i++)
		{
			dfButton dfButton = this.controls[i] as dfButton;
			if (!(dfButton == null))
			{
				if (i == value)
				{
					dfButton.State = dfButton.ButtonState.Focus;
				}
				else
				{
					dfButton.State = dfButton.ButtonState.Default;
				}
			}
		}
		this.Invalidate();
		this.OnSelectedIndexChanged();
		if (this.pageContainer != null)
		{
			this.pageContainer.SelectedIndex = value;
		}
	}

	// Token: 0x060016B9 RID: 5817 RVA: 0x0006BAE8 File Offset: 0x00069CE8
	private void arrangeTabs()
	{
		base.SuspendLayout();
		try
		{
			this.layoutPadding = this.layoutPadding.ConstrainPadding();
			float num = (float)this.layoutPadding.left - this.scrollPosition.x;
			float num2 = (float)this.layoutPadding.top - this.scrollPosition.y;
			float num3 = 0f;
			float num4 = 0f;
			for (int i = 0; i < base.Controls.Count; i++)
			{
				dfControl dfControl = this.controls[i];
				if (dfControl.IsVisible && dfControl.enabled && dfControl.gameObject.activeSelf)
				{
					Vector2 vector = new Vector2(num, num2);
					dfControl.RelativePosition = vector;
					float num5 = dfControl.Width + (float)this.layoutPadding.horizontal;
					float num6 = dfControl.Height + (float)this.layoutPadding.vertical;
					num3 = Mathf.Max(num5, num3);
					num4 = Mathf.Max(num6, num4);
					num += num5;
				}
			}
		}
		finally
		{
			base.ResumeLayout();
		}
	}

	// Token: 0x060016BA RID: 5818 RVA: 0x0006BC2C File Offset: 0x00069E2C
	private void attachEvents(dfControl control)
	{
		control.IsVisibleChanged += this.control_IsVisibleChanged;
		control.PositionChanged += this.childControlInvalidated;
		control.SizeChanged += this.childControlInvalidated;
		control.ZOrderChanged += this.childControlZOrderChanged;
	}

	// Token: 0x060016BB RID: 5819 RVA: 0x0006BC84 File Offset: 0x00069E84
	private void detachEvents(dfControl control)
	{
		control.IsVisibleChanged -= this.control_IsVisibleChanged;
		control.PositionChanged -= this.childControlInvalidated;
		control.SizeChanged -= this.childControlInvalidated;
	}

	// Token: 0x060016BC RID: 5820 RVA: 0x0006BCBC File Offset: 0x00069EBC
	private void childControlZOrderChanged(dfControl control, int value)
	{
		this.onChildControlInvalidatedLayout();
	}

	// Token: 0x060016BD RID: 5821 RVA: 0x0006BCC4 File Offset: 0x00069EC4
	private void control_IsVisibleChanged(dfControl control, bool value)
	{
		this.onChildControlInvalidatedLayout();
	}

	// Token: 0x060016BE RID: 5822 RVA: 0x0006BCCC File Offset: 0x00069ECC
	private void childControlInvalidated(dfControl control, Vector2 value)
	{
		this.onChildControlInvalidatedLayout();
	}

	// Token: 0x060016BF RID: 5823 RVA: 0x0006BCD4 File Offset: 0x00069ED4
	private void onChildControlInvalidatedLayout()
	{
		if (base.IsLayoutSuspended)
		{
			return;
		}
		this.arrangeTabs();
		this.Invalidate();
	}

	// Token: 0x0400129D RID: 4765
	[SerializeField]
	protected dfAtlas atlas;

	// Token: 0x0400129E RID: 4766
	[SerializeField]
	protected string backgroundSprite;

	// Token: 0x0400129F RID: 4767
	[SerializeField]
	protected RectOffset layoutPadding = new RectOffset();

	// Token: 0x040012A0 RID: 4768
	[SerializeField]
	protected Vector2 scrollPosition = Vector2.zero;

	// Token: 0x040012A1 RID: 4769
	[SerializeField]
	protected int selectedIndex;

	// Token: 0x040012A2 RID: 4770
	[SerializeField]
	protected dfTabContainer pageContainer;

	// Token: 0x040012A3 RID: 4771
	[SerializeField]
	protected bool allowKeyboardNavigation = true;
}
