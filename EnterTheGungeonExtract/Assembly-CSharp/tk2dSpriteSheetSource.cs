using System;
using UnityEngine;

// Token: 0x02000BC7 RID: 3015
[Serializable]
public class tk2dSpriteSheetSource
{
	// Token: 0x06003FEF RID: 16367 RVA: 0x00144C74 File Offset: 0x00142E74
	public void CopyFrom(tk2dSpriteSheetSource src)
	{
		this.texture = src.texture;
		this.tilesX = src.tilesX;
		this.tilesY = src.tilesY;
		this.numTiles = src.numTiles;
		this.anchor = src.anchor;
		this.pad = src.pad;
		this.scale = src.scale;
		this.colliderType = src.colliderType;
		this.version = src.version;
		this.active = src.active;
		this.tileWidth = src.tileWidth;
		this.tileHeight = src.tileHeight;
		this.tileSpacingX = src.tileSpacingX;
		this.tileSpacingY = src.tileSpacingY;
		this.tileMarginX = src.tileMarginX;
		this.tileMarginY = src.tileMarginY;
		this.splitMethod = src.splitMethod;
	}

	// Token: 0x06003FF0 RID: 16368 RVA: 0x00144D50 File Offset: 0x00142F50
	public bool CompareTo(tk2dSpriteSheetSource src)
	{
		return !(this.texture != src.texture) && this.tilesX == src.tilesX && this.tilesY == src.tilesY && this.numTiles == src.numTiles && this.anchor == src.anchor && this.pad == src.pad && !(this.scale != src.scale) && this.colliderType == src.colliderType && this.version == src.version && this.active == src.active && this.tileWidth == src.tileWidth && this.tileHeight == src.tileHeight && this.tileSpacingX == src.tileSpacingX && this.tileSpacingY == src.tileSpacingY && this.tileMarginX == src.tileMarginX && this.tileMarginY == src.tileMarginY && this.splitMethod == src.splitMethod;
	}

	// Token: 0x170009AE RID: 2478
	// (get) Token: 0x06003FF1 RID: 16369 RVA: 0x00144EAC File Offset: 0x001430AC
	public string Name
	{
		get
		{
			return (!(this.texture != null)) ? "New Sprite Sheet" : this.texture.name;
		}
	}

	// Token: 0x0400326E RID: 12910
	public Texture2D texture;

	// Token: 0x0400326F RID: 12911
	public int tilesX;

	// Token: 0x04003270 RID: 12912
	public int tilesY;

	// Token: 0x04003271 RID: 12913
	public int numTiles;

	// Token: 0x04003272 RID: 12914
	public tk2dSpriteSheetSource.Anchor anchor = tk2dSpriteSheetSource.Anchor.MiddleCenter;

	// Token: 0x04003273 RID: 12915
	public tk2dSpriteCollectionDefinition.Pad pad;

	// Token: 0x04003274 RID: 12916
	public Vector3 scale = new Vector3(1f, 1f, 1f);

	// Token: 0x04003275 RID: 12917
	public bool additive;

	// Token: 0x04003276 RID: 12918
	public bool active;

	// Token: 0x04003277 RID: 12919
	public int tileWidth;

	// Token: 0x04003278 RID: 12920
	public int tileHeight;

	// Token: 0x04003279 RID: 12921
	public int tileMarginX;

	// Token: 0x0400327A RID: 12922
	public int tileMarginY;

	// Token: 0x0400327B RID: 12923
	public int tileSpacingX;

	// Token: 0x0400327C RID: 12924
	public int tileSpacingY;

	// Token: 0x0400327D RID: 12925
	public tk2dSpriteSheetSource.SplitMethod splitMethod;

	// Token: 0x0400327E RID: 12926
	public int version;

	// Token: 0x0400327F RID: 12927
	public const int CURRENT_VERSION = 1;

	// Token: 0x04003280 RID: 12928
	public tk2dSpriteCollectionDefinition.ColliderType colliderType;

	// Token: 0x02000BC8 RID: 3016
	public enum Anchor
	{
		// Token: 0x04003282 RID: 12930
		UpperLeft,
		// Token: 0x04003283 RID: 12931
		UpperCenter,
		// Token: 0x04003284 RID: 12932
		UpperRight,
		// Token: 0x04003285 RID: 12933
		MiddleLeft,
		// Token: 0x04003286 RID: 12934
		MiddleCenter,
		// Token: 0x04003287 RID: 12935
		MiddleRight,
		// Token: 0x04003288 RID: 12936
		LowerLeft,
		// Token: 0x04003289 RID: 12937
		LowerCenter,
		// Token: 0x0400328A RID: 12938
		LowerRight
	}

	// Token: 0x02000BC9 RID: 3017
	public enum SplitMethod
	{
		// Token: 0x0400328C RID: 12940
		UniformDivision
	}
}
