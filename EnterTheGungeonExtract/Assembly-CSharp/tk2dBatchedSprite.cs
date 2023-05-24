using System;
using UnityEngine;

// Token: 0x02000BE0 RID: 3040
[Serializable]
public class tk2dBatchedSprite
{
	// Token: 0x06004063 RID: 16483 RVA: 0x00149554 File Offset: 0x00147754
	public tk2dBatchedSprite()
	{
		this.parentId = -1;
	}

	// Token: 0x170009C2 RID: 2498
	// (get) Token: 0x06004064 RID: 16484 RVA: 0x00149608 File Offset: 0x00147808
	// (set) Token: 0x06004065 RID: 16485 RVA: 0x00149618 File Offset: 0x00147818
	public float BoxColliderOffsetZ
	{
		get
		{
			return this.colliderData.x;
		}
		set
		{
			this.colliderData.x = value;
		}
	}

	// Token: 0x170009C3 RID: 2499
	// (get) Token: 0x06004066 RID: 16486 RVA: 0x00149628 File Offset: 0x00147828
	// (set) Token: 0x06004067 RID: 16487 RVA: 0x00149638 File Offset: 0x00147838
	public float BoxColliderExtentZ
	{
		get
		{
			return this.colliderData.y;
		}
		set
		{
			this.colliderData.y = value;
		}
	}

	// Token: 0x170009C4 RID: 2500
	// (get) Token: 0x06004068 RID: 16488 RVA: 0x00149648 File Offset: 0x00147848
	// (set) Token: 0x06004069 RID: 16489 RVA: 0x00149650 File Offset: 0x00147850
	public string FormattedText
	{
		get
		{
			return this.formattedText;
		}
		set
		{
			this.formattedText = value;
		}
	}

	// Token: 0x170009C5 RID: 2501
	// (get) Token: 0x0600406A RID: 16490 RVA: 0x0014965C File Offset: 0x0014785C
	// (set) Token: 0x0600406B RID: 16491 RVA: 0x00149664 File Offset: 0x00147864
	public Vector2 ClippedSpriteRegionBottomLeft
	{
		get
		{
			return this.internalData0;
		}
		set
		{
			this.internalData0 = value;
		}
	}

	// Token: 0x170009C6 RID: 2502
	// (get) Token: 0x0600406C RID: 16492 RVA: 0x00149670 File Offset: 0x00147870
	// (set) Token: 0x0600406D RID: 16493 RVA: 0x00149678 File Offset: 0x00147878
	public Vector2 ClippedSpriteRegionTopRight
	{
		get
		{
			return this.internalData1;
		}
		set
		{
			this.internalData1 = value;
		}
	}

	// Token: 0x170009C7 RID: 2503
	// (get) Token: 0x0600406E RID: 16494 RVA: 0x00149684 File Offset: 0x00147884
	// (set) Token: 0x0600406F RID: 16495 RVA: 0x0014968C File Offset: 0x0014788C
	public Vector2 SlicedSpriteBorderBottomLeft
	{
		get
		{
			return this.internalData0;
		}
		set
		{
			this.internalData0 = value;
		}
	}

	// Token: 0x170009C8 RID: 2504
	// (get) Token: 0x06004070 RID: 16496 RVA: 0x00149698 File Offset: 0x00147898
	// (set) Token: 0x06004071 RID: 16497 RVA: 0x001496A0 File Offset: 0x001478A0
	public Vector2 SlicedSpriteBorderTopRight
	{
		get
		{
			return this.internalData1;
		}
		set
		{
			this.internalData1 = value;
		}
	}

	// Token: 0x170009C9 RID: 2505
	// (get) Token: 0x06004072 RID: 16498 RVA: 0x001496AC File Offset: 0x001478AC
	// (set) Token: 0x06004073 RID: 16499 RVA: 0x001496B4 File Offset: 0x001478B4
	public Vector2 Dimensions
	{
		get
		{
			return this.internalData2;
		}
		set
		{
			this.internalData2 = value;
		}
	}

	// Token: 0x170009CA RID: 2506
	// (get) Token: 0x06004074 RID: 16500 RVA: 0x001496C0 File Offset: 0x001478C0
	public bool IsDrawn
	{
		get
		{
			return this.type != tk2dBatchedSprite.Type.EmptyGameObject;
		}
	}

	// Token: 0x06004075 RID: 16501 RVA: 0x001496D0 File Offset: 0x001478D0
	public bool CheckFlag(tk2dBatchedSprite.Flags mask)
	{
		return (this.flags & mask) != tk2dBatchedSprite.Flags.None;
	}

	// Token: 0x06004076 RID: 16502 RVA: 0x001496E0 File Offset: 0x001478E0
	public void SetFlag(tk2dBatchedSprite.Flags mask, bool value)
	{
		if (value)
		{
			this.flags |= mask;
		}
		else
		{
			this.flags &= ~mask;
		}
	}

	// Token: 0x170009CB RID: 2507
	// (get) Token: 0x06004077 RID: 16503 RVA: 0x0014970C File Offset: 0x0014790C
	// (set) Token: 0x06004078 RID: 16504 RVA: 0x00149714 File Offset: 0x00147914
	public Vector3 CachedBoundsCenter
	{
		get
		{
			return this.cachedBoundsCenter;
		}
		set
		{
			this.cachedBoundsCenter = value;
		}
	}

	// Token: 0x170009CC RID: 2508
	// (get) Token: 0x06004079 RID: 16505 RVA: 0x00149720 File Offset: 0x00147920
	// (set) Token: 0x0600407A RID: 16506 RVA: 0x00149728 File Offset: 0x00147928
	public Vector3 CachedBoundsExtents
	{
		get
		{
			return this.cachedBoundsExtents;
		}
		set
		{
			this.cachedBoundsExtents = value;
		}
	}

	// Token: 0x0600407B RID: 16507 RVA: 0x00149734 File Offset: 0x00147934
	public tk2dSpriteDefinition GetSpriteDefinition()
	{
		if (this.spriteCollection != null && this.spriteId != -1)
		{
			return this.spriteCollection.inst.spriteDefinitions[this.spriteId];
		}
		return null;
	}

	// Token: 0x0400334C RID: 13132
	public tk2dBatchedSprite.Type type = tk2dBatchedSprite.Type.Sprite;

	// Token: 0x0400334D RID: 13133
	public string name = string.Empty;

	// Token: 0x0400334E RID: 13134
	public int parentId = -1;

	// Token: 0x0400334F RID: 13135
	public int spriteId;

	// Token: 0x04003350 RID: 13136
	public int xRefId = -1;

	// Token: 0x04003351 RID: 13137
	public tk2dSpriteCollectionData spriteCollection;

	// Token: 0x04003352 RID: 13138
	public Quaternion rotation = Quaternion.identity;

	// Token: 0x04003353 RID: 13139
	public Vector3 position = Vector3.zero;

	// Token: 0x04003354 RID: 13140
	public Vector3 localScale = Vector3.one;

	// Token: 0x04003355 RID: 13141
	public Color color = Color.white;

	// Token: 0x04003356 RID: 13142
	public Vector3 baseScale = Vector3.one;

	// Token: 0x04003357 RID: 13143
	public int renderLayer;

	// Token: 0x04003358 RID: 13144
	[SerializeField]
	private Vector2 internalData0;

	// Token: 0x04003359 RID: 13145
	[SerializeField]
	private Vector2 internalData1;

	// Token: 0x0400335A RID: 13146
	[SerializeField]
	private Vector2 internalData2;

	// Token: 0x0400335B RID: 13147
	[SerializeField]
	private Vector2 colliderData = new Vector2(0f, 1f);

	// Token: 0x0400335C RID: 13148
	[SerializeField]
	private string formattedText = string.Empty;

	// Token: 0x0400335D RID: 13149
	[SerializeField]
	private tk2dBatchedSprite.Flags flags;

	// Token: 0x0400335E RID: 13150
	public tk2dBaseSprite.Anchor anchor;

	// Token: 0x0400335F RID: 13151
	public Matrix4x4 relativeMatrix = Matrix4x4.identity;

	// Token: 0x04003360 RID: 13152
	private Vector3 cachedBoundsCenter = Vector3.zero;

	// Token: 0x04003361 RID: 13153
	private Vector3 cachedBoundsExtents = Vector3.zero;

	// Token: 0x02000BE1 RID: 3041
	public enum Type
	{
		// Token: 0x04003363 RID: 13155
		EmptyGameObject,
		// Token: 0x04003364 RID: 13156
		Sprite,
		// Token: 0x04003365 RID: 13157
		TiledSprite,
		// Token: 0x04003366 RID: 13158
		SlicedSprite,
		// Token: 0x04003367 RID: 13159
		ClippedSprite,
		// Token: 0x04003368 RID: 13160
		TextMesh
	}

	// Token: 0x02000BE2 RID: 3042
	[Flags]
	public enum Flags
	{
		// Token: 0x0400336A RID: 13162
		None = 0,
		// Token: 0x0400336B RID: 13163
		Sprite_CreateBoxCollider = 1,
		// Token: 0x0400336C RID: 13164
		SlicedSprite_BorderOnly = 2
	}
}
