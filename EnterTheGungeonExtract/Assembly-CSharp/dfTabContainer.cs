using System;
using System.Diagnostics;
using UnityEngine;

// Token: 0x02000402 RID: 1026
[ExecuteInEditMode]
[AddComponentMenu("Daikon Forge/User Interface/Containers/Tab Control/Tab Page Container")]
[dfTooltip("Used in conjunction with the dfTabContainer class to implement tabbed containers. This control maintains the tabs that are displayed for the user to select, and the dfTabContainer class manages the display of the tab pages themselves.")]
[dfHelp("http://www.daikonforge.com/docs/df-gui/classdf_tab_container.html")]
[dfCategory("Basic Controls")]
[Serializable]
public class dfTabContainer : dfControl
{
	// Token: 0x1400003E RID: 62
	// (add) Token: 0x06001682 RID: 5762 RVA: 0x0006AC10 File Offset: 0x00068E10
	// (remove) Token: 0x06001683 RID: 5763 RVA: 0x0006AC48 File Offset: 0x00068E48
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event PropertyChangedEventHandler<int> SelectedIndexChanged;

	// Token: 0x170004DC RID: 1244
	// (get) Token: 0x06001684 RID: 5764 RVA: 0x0006AC80 File Offset: 0x00068E80
	// (set) Token: 0x06001685 RID: 5765 RVA: 0x0006ACC8 File Offset: 0x00068EC8
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

	// Token: 0x170004DD RID: 1245
	// (get) Token: 0x06001686 RID: 5766 RVA: 0x0006ACE8 File Offset: 0x00068EE8
	// (set) Token: 0x06001687 RID: 5767 RVA: 0x0006ACF0 File Offset: 0x00068EF0
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

	// Token: 0x170004DE RID: 1246
	// (get) Token: 0x06001688 RID: 5768 RVA: 0x0006AD10 File Offset: 0x00068F10
	// (set) Token: 0x06001689 RID: 5769 RVA: 0x0006AD30 File Offset: 0x00068F30
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
				this.arrangeTabPages();
			}
		}
	}

	// Token: 0x170004DF RID: 1247
	// (get) Token: 0x0600168A RID: 5770 RVA: 0x0006AD58 File Offset: 0x00068F58
	// (set) Token: 0x0600168B RID: 5771 RVA: 0x0006AD60 File Offset: 0x00068F60
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
				this.selectPageByIndex(value);
			}
		}
	}

	// Token: 0x0600168C RID: 5772 RVA: 0x0006AD78 File Offset: 0x00068F78
	public dfControl AddTabPage()
	{
		dfPanel dfPanel = this.controls.Where((dfControl i) => i is dfPanel).FirstOrDefault() as dfPanel;
		string text = "Tab Page " + (this.controls.Count + 1);
		dfPanel dfPanel2 = base.AddControl<dfPanel>();
		dfPanel2.name = text;
		dfPanel2.Atlas = this.Atlas;
		dfPanel2.Anchor = dfAnchorStyle.All;
		dfPanel2.ClipChildren = true;
		if (dfPanel != null)
		{
			dfPanel2.Atlas = dfPanel.Atlas;
			dfPanel2.BackgroundSprite = dfPanel.BackgroundSprite;
		}
		this.arrangeTabPages();
		this.Invalidate();
		return dfPanel2;
	}

	// Token: 0x0600168D RID: 5773 RVA: 0x0006AE30 File Offset: 0x00069030
	public override void OnEnable()
	{
		base.OnEnable();
		if (this.size.sqrMagnitude < 1E-45f)
		{
			base.Size = new Vector2(256f, 256f);
		}
	}

	// Token: 0x0600168E RID: 5774 RVA: 0x0006AE64 File Offset: 0x00069064
	protected internal override void OnControlAdded(dfControl child)
	{
		base.OnControlAdded(child);
		this.attachEvents(child);
		this.arrangeTabPages();
	}

	// Token: 0x0600168F RID: 5775 RVA: 0x0006AE7C File Offset: 0x0006907C
	protected internal override void OnControlRemoved(dfControl child)
	{
		base.OnControlRemoved(child);
		this.detachEvents(child);
		this.arrangeTabPages();
	}

	// Token: 0x06001690 RID: 5776 RVA: 0x0006AE94 File Offset: 0x00069094
	protected internal virtual void OnSelectedIndexChanged(int Index)
	{
		base.SignalHierarchy("OnSelectedIndexChanged", new object[] { this, Index });
		if (this.SelectedIndexChanged != null)
		{
			this.SelectedIndexChanged(this, Index);
		}
	}

	// Token: 0x06001691 RID: 5777 RVA: 0x0006AED0 File Offset: 0x000690D0
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

	// Token: 0x06001692 RID: 5778 RVA: 0x0006B004 File Offset: 0x00069204
	private void selectPageByIndex(int value)
	{
		value = Mathf.Max(Mathf.Min(value, this.controls.Count - 1), -1);
		if (value == this.selectedIndex)
		{
			return;
		}
		this.selectedIndex = value;
		for (int i = 0; i < this.controls.Count; i++)
		{
			dfControl dfControl = this.controls[i];
			if (!(dfControl == null))
			{
				dfControl.IsVisible = i == value;
			}
		}
		this.arrangeTabPages();
		this.Invalidate();
		this.OnSelectedIndexChanged(value);
	}

	// Token: 0x06001693 RID: 5779 RVA: 0x0006B098 File Offset: 0x00069298
	private void arrangeTabPages()
	{
		if (this.padding == null)
		{
			this.padding = new RectOffset(0, 0, 0, 0);
		}
		Vector3 vector = new Vector3((float)this.padding.left, (float)this.padding.top);
		Vector2 vector2 = new Vector2(this.size.x - (float)this.padding.horizontal, this.size.y - (float)this.padding.vertical);
		for (int i = 0; i < this.controls.Count; i++)
		{
			dfPanel dfPanel = this.controls[i] as dfPanel;
			if (dfPanel != null)
			{
				dfPanel.Size = vector2;
				dfPanel.RelativePosition = vector;
			}
		}
	}

	// Token: 0x06001694 RID: 5780 RVA: 0x0006B160 File Offset: 0x00069360
	private void attachEvents(dfControl control)
	{
		control.IsVisibleChanged += this.control_IsVisibleChanged;
		control.PositionChanged += this.childControlInvalidated;
		control.SizeChanged += this.childControlInvalidated;
	}

	// Token: 0x06001695 RID: 5781 RVA: 0x0006B198 File Offset: 0x00069398
	private void detachEvents(dfControl control)
	{
		control.IsVisibleChanged -= this.control_IsVisibleChanged;
		control.PositionChanged -= this.childControlInvalidated;
		control.SizeChanged -= this.childControlInvalidated;
	}

	// Token: 0x06001696 RID: 5782 RVA: 0x0006B1D0 File Offset: 0x000693D0
	private void control_IsVisibleChanged(dfControl control, bool value)
	{
		this.onChildControlInvalidatedLayout();
	}

	// Token: 0x06001697 RID: 5783 RVA: 0x0006B1D8 File Offset: 0x000693D8
	private void childControlInvalidated(dfControl control, Vector2 value)
	{
		this.onChildControlInvalidatedLayout();
	}

	// Token: 0x06001698 RID: 5784 RVA: 0x0006B1E0 File Offset: 0x000693E0
	private void onChildControlInvalidatedLayout()
	{
		if (base.IsLayoutSuspended)
		{
			return;
		}
		this.arrangeTabPages();
		this.Invalidate();
	}

	// Token: 0x04001297 RID: 4759
	[SerializeField]
	protected dfAtlas atlas;

	// Token: 0x04001298 RID: 4760
	[SerializeField]
	protected string backgroundSprite;

	// Token: 0x04001299 RID: 4761
	[SerializeField]
	protected RectOffset padding = new RectOffset();

	// Token: 0x0400129A RID: 4762
	[SerializeField]
	protected int selectedIndex;
}
