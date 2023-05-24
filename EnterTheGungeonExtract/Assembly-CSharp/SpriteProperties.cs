using System;
using UnityEngine;

// Token: 0x0200047B RID: 1147
[AddComponentMenu("Daikon Forge/Examples/Sprites/Sprite Properties")]
public class SpriteProperties : MonoBehaviour
{
	// Token: 0x06001A56 RID: 6742 RVA: 0x0007AA10 File Offset: 0x00078C10
	private void Awake()
	{
		if (this.sprite == null)
		{
			this.sprite = base.GetComponent<dfSprite>();
		}
		this.ShowBorders = true;
		base.useGUILayout = false;
	}

	// Token: 0x06001A57 RID: 6743 RVA: 0x0007AA40 File Offset: 0x00078C40
	private void Start()
	{
	}

	// Token: 0x06001A58 RID: 6744 RVA: 0x0007AA44 File Offset: 0x00078C44
	private void OnGUI()
	{
		if (!this.ShowBorders || this.blankTexture == null)
		{
			return;
		}
		Rect screenRect = this.sprite.GetScreenRect();
		RectOffset border = this.sprite.SpriteInfo.border;
		float x = screenRect.x;
		float y = screenRect.y;
		float width = screenRect.width;
		float height = screenRect.height;
		int num = border.left;
		int num2 = border.right;
		int num3 = border.top;
		int num4 = border.bottom;
		if (this.sprite.Flip.IsSet(dfSpriteFlip.FlipHorizontal))
		{
			num = border.right;
			num2 = border.left;
		}
		if (this.sprite.Flip.IsSet(dfSpriteFlip.FlipVertical))
		{
			num3 = border.bottom;
			num4 = border.top;
		}
		GUI.color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
		GUI.DrawTexture(new Rect(x - 10f, y + (float)num3, width + 20f, 1f), this.blankTexture);
		GUI.DrawTexture(new Rect(x - 10f, y + height - (float)num4, width + 20f, 1f), this.blankTexture);
		GUI.DrawTexture(new Rect(x + (float)num, y - 10f, 1f, height + 20f), this.blankTexture);
		GUI.DrawTexture(new Rect(x + width - (float)num2, y - 10f, 1f, height + 20f), this.blankTexture);
	}

	// Token: 0x17000577 RID: 1399
	// (get) Token: 0x06001A59 RID: 6745 RVA: 0x0007ABE4 File Offset: 0x00078DE4
	// (set) Token: 0x06001A5A RID: 6746 RVA: 0x0007ABEC File Offset: 0x00078DEC
	public bool ShowBorders { get; set; }

	// Token: 0x17000578 RID: 1400
	// (get) Token: 0x06001A5B RID: 6747 RVA: 0x0007ABF8 File Offset: 0x00078DF8
	// (set) Token: 0x06001A5C RID: 6748 RVA: 0x0007AC20 File Offset: 0x00078E20
	public float PatternScaleX
	{
		get
		{
			return ((dfTiledSprite)this.sprite).TileScale.x;
		}
		set
		{
			dfTiledSprite dfTiledSprite = this.sprite as dfTiledSprite;
			dfTiledSprite.TileScale = new Vector2(value, dfTiledSprite.TileScale.y);
		}
	}

	// Token: 0x17000579 RID: 1401
	// (get) Token: 0x06001A5D RID: 6749 RVA: 0x0007AC54 File Offset: 0x00078E54
	// (set) Token: 0x06001A5E RID: 6750 RVA: 0x0007AC7C File Offset: 0x00078E7C
	public float PatternScaleY
	{
		get
		{
			return ((dfTiledSprite)this.sprite).TileScale.y;
		}
		set
		{
			dfTiledSprite dfTiledSprite = this.sprite as dfTiledSprite;
			dfTiledSprite.TileScale = new Vector2(dfTiledSprite.TileScale.x, value);
		}
	}

	// Token: 0x1700057A RID: 1402
	// (get) Token: 0x06001A5F RID: 6751 RVA: 0x0007ACB0 File Offset: 0x00078EB0
	// (set) Token: 0x06001A60 RID: 6752 RVA: 0x0007ACD8 File Offset: 0x00078ED8
	public float PatternOffsetX
	{
		get
		{
			return ((dfTiledSprite)this.sprite).TileScroll.x;
		}
		set
		{
			dfTiledSprite dfTiledSprite = this.sprite as dfTiledSprite;
			dfTiledSprite.TileScroll = new Vector2(value, dfTiledSprite.TileScroll.y);
		}
	}

	// Token: 0x1700057B RID: 1403
	// (get) Token: 0x06001A61 RID: 6753 RVA: 0x0007AD0C File Offset: 0x00078F0C
	// (set) Token: 0x06001A62 RID: 6754 RVA: 0x0007AD34 File Offset: 0x00078F34
	public float PatternOffsetY
	{
		get
		{
			return ((dfTiledSprite)this.sprite).TileScroll.y;
		}
		set
		{
			dfTiledSprite dfTiledSprite = this.sprite as dfTiledSprite;
			dfTiledSprite.TileScroll = new Vector2(dfTiledSprite.TileScroll.x, value);
		}
	}

	// Token: 0x1700057C RID: 1404
	// (get) Token: 0x06001A63 RID: 6755 RVA: 0x0007AD68 File Offset: 0x00078F68
	// (set) Token: 0x06001A64 RID: 6756 RVA: 0x0007AD7C File Offset: 0x00078F7C
	public int FillOrigin
	{
		get
		{
			return (int)((dfRadialSprite)this.sprite).FillOrigin;
		}
		set
		{
			((dfRadialSprite)this.sprite).FillOrigin = (dfPivotPoint)value;
		}
	}

	// Token: 0x1700057D RID: 1405
	// (get) Token: 0x06001A65 RID: 6757 RVA: 0x0007AD90 File Offset: 0x00078F90
	// (set) Token: 0x06001A66 RID: 6758 RVA: 0x0007ADA0 File Offset: 0x00078FA0
	public bool FillVertical
	{
		get
		{
			return this.sprite.FillDirection == dfFillDirection.Vertical;
		}
		set
		{
			if (value)
			{
				this.sprite.FillDirection = dfFillDirection.Vertical;
			}
			else
			{
				this.sprite.FillDirection = dfFillDirection.Horizontal;
			}
		}
	}

	// Token: 0x1700057E RID: 1406
	// (get) Token: 0x06001A67 RID: 6759 RVA: 0x0007ADC8 File Offset: 0x00078FC8
	// (set) Token: 0x06001A68 RID: 6760 RVA: 0x0007ADDC File Offset: 0x00078FDC
	public bool FlipHorizontal
	{
		get
		{
			return this.sprite.Flip.IsSet(dfSpriteFlip.FlipHorizontal);
		}
		set
		{
			this.sprite.Flip = this.sprite.Flip.SetFlag(dfSpriteFlip.FlipHorizontal, value);
		}
	}

	// Token: 0x1700057F RID: 1407
	// (get) Token: 0x06001A69 RID: 6761 RVA: 0x0007ADFC File Offset: 0x00078FFC
	// (set) Token: 0x06001A6A RID: 6762 RVA: 0x0007AE10 File Offset: 0x00079010
	public bool FlipVertical
	{
		get
		{
			return this.sprite.Flip.IsSet(dfSpriteFlip.FlipVertical);
		}
		set
		{
			this.sprite.Flip = this.sprite.Flip.SetFlag(dfSpriteFlip.FlipVertical, value);
		}
	}

	// Token: 0x040014A1 RID: 5281
	public Texture2D blankTexture;

	// Token: 0x040014A2 RID: 5282
	public dfSprite sprite;
}
