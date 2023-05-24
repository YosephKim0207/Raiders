using System;
using UnityEngine;

// Token: 0x020003DB RID: 987
[ExecuteInEditMode]
[Serializable]
public class dfInteractiveBase : dfControl
{
	// Token: 0x1700043F RID: 1087
	// (get) Token: 0x06001394 RID: 5012 RVA: 0x0005A904 File Offset: 0x00058B04
	// (set) Token: 0x06001395 RID: 5013 RVA: 0x0005A94C File Offset: 0x00058B4C
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

	// Token: 0x17000440 RID: 1088
	// (get) Token: 0x06001396 RID: 5014 RVA: 0x0005A96C File Offset: 0x00058B6C
	// (set) Token: 0x06001397 RID: 5015 RVA: 0x0005A974 File Offset: 0x00058B74
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
				this.setDefaultSize(value);
				this.Invalidate();
			}
		}
	}

	// Token: 0x17000441 RID: 1089
	// (get) Token: 0x06001398 RID: 5016 RVA: 0x0005A99C File Offset: 0x00058B9C
	// (set) Token: 0x06001399 RID: 5017 RVA: 0x0005A9A4 File Offset: 0x00058BA4
	public string DisabledSprite
	{
		get
		{
			return this.disabledSprite;
		}
		set
		{
			if (value != this.disabledSprite)
			{
				this.disabledSprite = value;
				this.Invalidate();
			}
		}
	}

	// Token: 0x17000442 RID: 1090
	// (get) Token: 0x0600139A RID: 5018 RVA: 0x0005A9C4 File Offset: 0x00058BC4
	// (set) Token: 0x0600139B RID: 5019 RVA: 0x0005A9CC File Offset: 0x00058BCC
	public string FocusSprite
	{
		get
		{
			return this.focusSprite;
		}
		set
		{
			if (value != this.focusSprite)
			{
				this.focusSprite = value;
				this.Invalidate();
			}
		}
	}

	// Token: 0x17000443 RID: 1091
	// (get) Token: 0x0600139C RID: 5020 RVA: 0x0005A9EC File Offset: 0x00058BEC
	// (set) Token: 0x0600139D RID: 5021 RVA: 0x0005A9F4 File Offset: 0x00058BF4
	public string HoverSprite
	{
		get
		{
			return this.hoverSprite;
		}
		set
		{
			if (value != this.hoverSprite)
			{
				this.hoverSprite = value;
				this.Invalidate();
			}
		}
	}

	// Token: 0x17000444 RID: 1092
	// (get) Token: 0x0600139E RID: 5022 RVA: 0x0005AA14 File Offset: 0x00058C14
	public override bool CanFocus
	{
		get
		{
			return (base.IsEnabled && base.IsVisible) || base.CanFocus;
		}
	}

	// Token: 0x0600139F RID: 5023 RVA: 0x0005AA34 File Offset: 0x00058C34
	protected internal override void OnGotFocus(dfFocusEventArgs args)
	{
		base.OnGotFocus(args);
		this.Invalidate();
	}

	// Token: 0x060013A0 RID: 5024 RVA: 0x0005AA44 File Offset: 0x00058C44
	protected internal override void OnLostFocus(dfFocusEventArgs args)
	{
		base.OnLostFocus(args);
		this.Invalidate();
	}

	// Token: 0x060013A1 RID: 5025 RVA: 0x0005AA54 File Offset: 0x00058C54
	protected internal override void OnMouseEnter(dfMouseEventArgs args)
	{
		base.OnMouseEnter(args);
		this.Invalidate();
	}

	// Token: 0x060013A2 RID: 5026 RVA: 0x0005AA64 File Offset: 0x00058C64
	protected internal override void OnMouseLeave(dfMouseEventArgs args)
	{
		base.OnMouseLeave(args);
		this.Invalidate();
	}

	// Token: 0x060013A3 RID: 5027 RVA: 0x0005AA74 File Offset: 0x00058C74
	public override Vector2 CalculateMinimumSize()
	{
		dfAtlas.ItemInfo itemInfo = this.getBackgroundSprite();
		if (itemInfo == null)
		{
			return base.CalculateMinimumSize();
		}
		RectOffset border = itemInfo.border;
		if (border.horizontal > 0 || border.vertical > 0)
		{
			return Vector2.Max(base.CalculateMinimumSize(), new Vector2((float)border.horizontal, (float)border.vertical));
		}
		return base.CalculateMinimumSize();
	}

	// Token: 0x060013A4 RID: 5028 RVA: 0x0005AAE0 File Offset: 0x00058CE0
	protected internal virtual void renderBackground()
	{
		if (this.Atlas == null)
		{
			return;
		}
		dfAtlas.ItemInfo itemInfo = this.getBackgroundSprite();
		if (itemInfo == null)
		{
			return;
		}
		Color32 color = base.ApplyOpacity(this.getActiveColor());
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

	// Token: 0x060013A5 RID: 5029 RVA: 0x0005ABCC File Offset: 0x00058DCC
	protected virtual Color32 getActiveColor()
	{
		if (base.IsEnabled)
		{
			return this.color;
		}
		if (!string.IsNullOrEmpty(this.disabledSprite) && this.Atlas != null && this.Atlas[this.DisabledSprite] != null)
		{
			return this.color;
		}
		return this.disabledColor;
	}

	// Token: 0x060013A6 RID: 5030 RVA: 0x0005AC38 File Offset: 0x00058E38
	protected internal virtual dfAtlas.ItemInfo getBackgroundSprite()
	{
		if (this.Atlas == null)
		{
			return null;
		}
		if (!base.IsEnabled)
		{
			dfAtlas.ItemInfo itemInfo = this.atlas[this.DisabledSprite];
			if (itemInfo != null)
			{
				return itemInfo;
			}
			return this.atlas[this.BackgroundSprite];
		}
		else
		{
			if (!this.HasFocus)
			{
				if (this.isMouseHovering)
				{
					dfAtlas.ItemInfo itemInfo2 = this.atlas[this.HoverSprite];
					if (itemInfo2 != null)
					{
						return itemInfo2;
					}
				}
				return this.Atlas[this.BackgroundSprite];
			}
			dfAtlas.ItemInfo itemInfo3 = this.atlas[this.FocusSprite];
			if (itemInfo3 != null)
			{
				return itemInfo3;
			}
			return this.atlas[this.BackgroundSprite];
		}
	}

	// Token: 0x060013A7 RID: 5031 RVA: 0x0005AD10 File Offset: 0x00058F10
	private void setDefaultSize(string spriteName)
	{
		if (this.Atlas == null)
		{
			return;
		}
		dfAtlas.ItemInfo itemInfo = this.Atlas[spriteName];
		if (this.size == Vector2.zero && itemInfo != null)
		{
			base.Size = itemInfo.sizeInPixels;
		}
	}

	// Token: 0x040010D6 RID: 4310
	[SerializeField]
	protected dfAtlas atlas;

	// Token: 0x040010D7 RID: 4311
	[SerializeField]
	protected string backgroundSprite;

	// Token: 0x040010D8 RID: 4312
	[SerializeField]
	protected string hoverSprite;

	// Token: 0x040010D9 RID: 4313
	[SerializeField]
	protected string disabledSprite;

	// Token: 0x040010DA RID: 4314
	[SerializeField]
	protected string focusSprite;
}
