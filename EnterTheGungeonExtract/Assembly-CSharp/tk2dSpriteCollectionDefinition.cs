using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000BBE RID: 3006
[Serializable]
public class tk2dSpriteCollectionDefinition
{
	// Token: 0x06003FEA RID: 16362 RVA: 0x001443E8 File Offset: 0x001425E8
	public void CopyFrom(tk2dSpriteCollectionDefinition src)
	{
		this.name = src.name;
		this.disableTrimming = src.disableTrimming;
		this.additive = src.additive;
		this.scale = src.scale;
		this.texture = src.texture;
		this.materialId = src.materialId;
		this.anchor = src.anchor;
		this.anchorX = src.anchorX;
		this.anchorY = src.anchorY;
		this.overrideMesh = src.overrideMesh;
		this.doubleSidedSprite = src.doubleSidedSprite;
		this.customSpriteGeometry = src.customSpriteGeometry;
		this.geometryIslands = src.geometryIslands;
		this.dice = src.dice;
		this.diceUnitX = src.diceUnitX;
		this.diceUnitY = src.diceUnitY;
		this.diceFilter = src.diceFilter;
		this.pad = src.pad;
		this.source = src.source;
		this.fromSpriteSheet = src.fromSpriteSheet;
		this.hasSpriteSheetId = src.hasSpriteSheetId;
		this.spriteSheetX = src.spriteSheetX;
		this.spriteSheetY = src.spriteSheetY;
		this.spriteSheetId = src.spriteSheetId;
		this.extractRegion = src.extractRegion;
		this.regionX = src.regionX;
		this.regionY = src.regionY;
		this.regionW = src.regionW;
		this.regionH = src.regionH;
		this.regionId = src.regionId;
		this.colliderType = src.colliderType;
		this.collisionLayer = src.collisionLayer;
		if (src.bagelColliders != null)
		{
			this.bagelColliders = new BagelCollider[src.bagelColliders.Length];
			for (int i = 0; i < src.bagelColliders.Length; i++)
			{
				this.bagelColliders[i] = new BagelCollider(src.bagelColliders[i]);
			}
		}
		if (src.metadata == null)
		{
			this.metadata = new TilesetIndexMetadata();
		}
		else
		{
			if (this.metadata == null)
			{
				this.metadata = new TilesetIndexMetadata();
			}
			this.metadata.CopyFrom(src.metadata);
		}
		this.boxColliderMin = src.boxColliderMin;
		this.boxColliderMax = src.boxColliderMax;
		this.polyColliderCap = src.polyColliderCap;
		this.colliderColor = src.colliderColor;
		this.colliderConvex = src.colliderConvex;
		this.colliderSmoothSphereCollisions = src.colliderSmoothSphereCollisions;
		this.extraPadding = src.extraPadding;
		if (src.polyColliderIslands != null)
		{
			this.polyColliderIslands = new tk2dSpriteColliderIsland[src.polyColliderIslands.Length];
			for (int j = 0; j < this.polyColliderIslands.Length; j++)
			{
				this.polyColliderIslands[j] = new tk2dSpriteColliderIsland();
				this.polyColliderIslands[j].CopyFrom(src.polyColliderIslands[j]);
			}
		}
		else
		{
			this.polyColliderIslands = new tk2dSpriteColliderIsland[0];
		}
		if (src.geometryIslands != null)
		{
			this.geometryIslands = new tk2dSpriteColliderIsland[src.geometryIslands.Length];
			for (int k = 0; k < this.geometryIslands.Length; k++)
			{
				this.geometryIslands[k] = new tk2dSpriteColliderIsland();
				this.geometryIslands[k].CopyFrom(src.geometryIslands[k]);
			}
		}
		else
		{
			this.geometryIslands = new tk2dSpriteColliderIsland[0];
		}
		this.attachPoints = new List<tk2dSpriteDefinition.AttachPoint>(src.attachPoints.Count);
		foreach (tk2dSpriteDefinition.AttachPoint attachPoint in src.attachPoints)
		{
			tk2dSpriteDefinition.AttachPoint attachPoint2 = new tk2dSpriteDefinition.AttachPoint();
			attachPoint2.CopyFrom(attachPoint);
			this.attachPoints.Add(attachPoint2);
		}
	}

	// Token: 0x06003FEB RID: 16363 RVA: 0x001447A8 File Offset: 0x001429A8
	public void Clear()
	{
		tk2dSpriteCollectionDefinition tk2dSpriteCollectionDefinition = new tk2dSpriteCollectionDefinition();
		this.CopyFrom(tk2dSpriteCollectionDefinition);
	}

	// Token: 0x06003FEC RID: 16364 RVA: 0x001447C4 File Offset: 0x001429C4
	public bool CompareTo(tk2dSpriteCollectionDefinition src)
	{
		if (this.name != src.name)
		{
			return false;
		}
		if (this.additive != src.additive)
		{
			return false;
		}
		if (this.scale != src.scale)
		{
			return false;
		}
		if (this.texture != src.texture)
		{
			return false;
		}
		if (this.materialId != src.materialId)
		{
			return false;
		}
		if (this.anchor != src.anchor)
		{
			return false;
		}
		if (this.anchorX != src.anchorX)
		{
			return false;
		}
		if (this.anchorY != src.anchorY)
		{
			return false;
		}
		if (this.overrideMesh != src.overrideMesh)
		{
			return false;
		}
		if (this.dice != src.dice)
		{
			return false;
		}
		if (this.diceUnitX != src.diceUnitX)
		{
			return false;
		}
		if (this.diceUnitY != src.diceUnitY)
		{
			return false;
		}
		if (this.diceFilter != src.diceFilter)
		{
			return false;
		}
		if (this.pad != src.pad)
		{
			return false;
		}
		if (this.extraPadding != src.extraPadding)
		{
			return false;
		}
		if (this.doubleSidedSprite != src.doubleSidedSprite)
		{
			return false;
		}
		if (this.customSpriteGeometry != src.customSpriteGeometry)
		{
			return false;
		}
		if (this.geometryIslands != src.geometryIslands)
		{
			return false;
		}
		if (this.geometryIslands != null && src.geometryIslands != null)
		{
			if (this.geometryIslands.Length != src.geometryIslands.Length)
			{
				return false;
			}
			for (int i = 0; i < this.geometryIslands.Length; i++)
			{
				if (!this.geometryIslands[i].CompareTo(src.geometryIslands[i]))
				{
					return false;
				}
			}
		}
		if (this.source != src.source)
		{
			return false;
		}
		if (this.fromSpriteSheet != src.fromSpriteSheet)
		{
			return false;
		}
		if (this.hasSpriteSheetId != src.hasSpriteSheetId)
		{
			return false;
		}
		if (this.spriteSheetId != src.spriteSheetId)
		{
			return false;
		}
		if (this.spriteSheetX != src.spriteSheetX)
		{
			return false;
		}
		if (this.spriteSheetY != src.spriteSheetY)
		{
			return false;
		}
		if (this.extractRegion != src.extractRegion)
		{
			return false;
		}
		if (this.regionX != src.regionX)
		{
			return false;
		}
		if (this.regionY != src.regionY)
		{
			return false;
		}
		if (this.regionW != src.regionW)
		{
			return false;
		}
		if (this.regionH != src.regionH)
		{
			return false;
		}
		if (this.regionId != src.regionId)
		{
			return false;
		}
		if (this.colliderType != src.colliderType)
		{
			return false;
		}
		if (this.collisionLayer != src.collisionLayer)
		{
			return false;
		}
		if (this.bagelColliders != src.bagelColliders)
		{
			return false;
		}
		if (this.metadata != src.metadata)
		{
			return false;
		}
		if (this.boxColliderMin != src.boxColliderMin)
		{
			return false;
		}
		if (this.boxColliderMax != src.boxColliderMax)
		{
			return false;
		}
		if (this.polyColliderIslands != src.polyColliderIslands)
		{
			return false;
		}
		if (this.polyColliderIslands != null && src.polyColliderIslands != null)
		{
			if (this.polyColliderIslands.Length != src.polyColliderIslands.Length)
			{
				return false;
			}
			for (int j = 0; j < this.polyColliderIslands.Length; j++)
			{
				if (!this.polyColliderIslands[j].CompareTo(src.polyColliderIslands[j]))
				{
					return false;
				}
			}
		}
		if (this.polyColliderCap != src.polyColliderCap)
		{
			return false;
		}
		if (this.colliderColor != src.colliderColor)
		{
			return false;
		}
		if (this.colliderSmoothSphereCollisions != src.colliderSmoothSphereCollisions)
		{
			return false;
		}
		if (this.colliderConvex != src.colliderConvex)
		{
			return false;
		}
		if (this.attachPoints.Count != src.attachPoints.Count)
		{
			return false;
		}
		for (int k = 0; k < this.attachPoints.Count; k++)
		{
			if (!this.attachPoints[k].CompareTo(src.attachPoints[k]))
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x04003215 RID: 12821
	public string name = string.Empty;

	// Token: 0x04003216 RID: 12822
	public bool disableTrimming;

	// Token: 0x04003217 RID: 12823
	public bool additive;

	// Token: 0x04003218 RID: 12824
	public Vector3 scale = new Vector3(1f, 1f, 1f);

	// Token: 0x04003219 RID: 12825
	public Texture2D texture;

	// Token: 0x0400321A RID: 12826
	[NonSerialized]
	public Texture2D thumbnailTexture;

	// Token: 0x0400321B RID: 12827
	public int materialId;

	// Token: 0x0400321C RID: 12828
	public tk2dSpriteCollectionDefinition.Anchor anchor = tk2dSpriteCollectionDefinition.Anchor.MiddleCenter;

	// Token: 0x0400321D RID: 12829
	public float anchorX;

	// Token: 0x0400321E RID: 12830
	public float anchorY;

	// Token: 0x0400321F RID: 12831
	public UnityEngine.Object overrideMesh;

	// Token: 0x04003220 RID: 12832
	public bool doubleSidedSprite;

	// Token: 0x04003221 RID: 12833
	public bool customSpriteGeometry;

	// Token: 0x04003222 RID: 12834
	public tk2dSpriteColliderIsland[] geometryIslands = new tk2dSpriteColliderIsland[0];

	// Token: 0x04003223 RID: 12835
	public bool dice;

	// Token: 0x04003224 RID: 12836
	public int diceUnitX = 64;

	// Token: 0x04003225 RID: 12837
	public int diceUnitY = 64;

	// Token: 0x04003226 RID: 12838
	public tk2dSpriteCollectionDefinition.DiceFilter diceFilter;

	// Token: 0x04003227 RID: 12839
	public tk2dSpriteCollectionDefinition.Pad pad;

	// Token: 0x04003228 RID: 12840
	public int extraPadding;

	// Token: 0x04003229 RID: 12841
	public tk2dSpriteCollectionDefinition.Source source;

	// Token: 0x0400322A RID: 12842
	public bool fromSpriteSheet;

	// Token: 0x0400322B RID: 12843
	public bool hasSpriteSheetId;

	// Token: 0x0400322C RID: 12844
	public int spriteSheetId;

	// Token: 0x0400322D RID: 12845
	public int spriteSheetX;

	// Token: 0x0400322E RID: 12846
	public int spriteSheetY;

	// Token: 0x0400322F RID: 12847
	public bool extractRegion;

	// Token: 0x04003230 RID: 12848
	public int regionX;

	// Token: 0x04003231 RID: 12849
	public int regionY;

	// Token: 0x04003232 RID: 12850
	public int regionW;

	// Token: 0x04003233 RID: 12851
	public int regionH;

	// Token: 0x04003234 RID: 12852
	public int regionId;

	// Token: 0x04003235 RID: 12853
	public tk2dSpriteCollectionDefinition.ColliderType colliderType;

	// Token: 0x04003236 RID: 12854
	public CollisionLayer collisionLayer = CollisionLayer.HighObstacle;

	// Token: 0x04003237 RID: 12855
	public BagelCollider[] bagelColliders;

	// Token: 0x04003238 RID: 12856
	public TilesetIndexMetadata metadata;

	// Token: 0x04003239 RID: 12857
	public Vector2 boxColliderMin;

	// Token: 0x0400323A RID: 12858
	public Vector2 boxColliderMax;

	// Token: 0x0400323B RID: 12859
	public tk2dSpriteColliderIsland[] polyColliderIslands;

	// Token: 0x0400323C RID: 12860
	public tk2dSpriteCollectionDefinition.PolygonColliderCap polyColliderCap = tk2dSpriteCollectionDefinition.PolygonColliderCap.FrontAndBack;

	// Token: 0x0400323D RID: 12861
	public bool colliderConvex;

	// Token: 0x0400323E RID: 12862
	public bool colliderSmoothSphereCollisions;

	// Token: 0x0400323F RID: 12863
	public tk2dSpriteCollectionDefinition.ColliderColor colliderColor;

	// Token: 0x04003240 RID: 12864
	public List<tk2dSpriteDefinition.AttachPoint> attachPoints = new List<tk2dSpriteDefinition.AttachPoint>();

	// Token: 0x02000BBF RID: 3007
	public enum Anchor
	{
		// Token: 0x04003242 RID: 12866
		UpperLeft,
		// Token: 0x04003243 RID: 12867
		UpperCenter,
		// Token: 0x04003244 RID: 12868
		UpperRight,
		// Token: 0x04003245 RID: 12869
		MiddleLeft,
		// Token: 0x04003246 RID: 12870
		MiddleCenter,
		// Token: 0x04003247 RID: 12871
		MiddleRight,
		// Token: 0x04003248 RID: 12872
		LowerLeft,
		// Token: 0x04003249 RID: 12873
		LowerCenter,
		// Token: 0x0400324A RID: 12874
		LowerRight,
		// Token: 0x0400324B RID: 12875
		Custom
	}

	// Token: 0x02000BC0 RID: 3008
	public enum Pad
	{
		// Token: 0x0400324D RID: 12877
		Default,
		// Token: 0x0400324E RID: 12878
		BlackZeroAlpha,
		// Token: 0x0400324F RID: 12879
		Extend,
		// Token: 0x04003250 RID: 12880
		TileXY
	}

	// Token: 0x02000BC1 RID: 3009
	public enum ColliderType
	{
		// Token: 0x04003252 RID: 12882
		UserDefined,
		// Token: 0x04003253 RID: 12883
		ForceNone,
		// Token: 0x04003254 RID: 12884
		BoxTrimmed,
		// Token: 0x04003255 RID: 12885
		BoxCustom,
		// Token: 0x04003256 RID: 12886
		Polygon
	}

	// Token: 0x02000BC2 RID: 3010
	public enum PolygonColliderCap
	{
		// Token: 0x04003258 RID: 12888
		None,
		// Token: 0x04003259 RID: 12889
		FrontAndBack,
		// Token: 0x0400325A RID: 12890
		Front,
		// Token: 0x0400325B RID: 12891
		Back
	}

	// Token: 0x02000BC3 RID: 3011
	public enum ColliderColor
	{
		// Token: 0x0400325D RID: 12893
		Default,
		// Token: 0x0400325E RID: 12894
		Red,
		// Token: 0x0400325F RID: 12895
		White,
		// Token: 0x04003260 RID: 12896
		Black
	}

	// Token: 0x02000BC4 RID: 3012
	public enum Source
	{
		// Token: 0x04003262 RID: 12898
		Sprite,
		// Token: 0x04003263 RID: 12899
		SpriteSheet,
		// Token: 0x04003264 RID: 12900
		Font
	}

	// Token: 0x02000BC5 RID: 3013
	public enum DiceFilter
	{
		// Token: 0x04003266 RID: 12902
		Complete,
		// Token: 0x04003267 RID: 12903
		SolidOnly,
		// Token: 0x04003268 RID: 12904
		TransparentOnly
	}
}
