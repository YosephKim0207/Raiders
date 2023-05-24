using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000861 RID: 2145
[Serializable]
public class PixelCollider
{
	// Token: 0x170008BF RID: 2239
	// (get) Token: 0x06002F50 RID: 12112 RVA: 0x000F9814 File Offset: 0x000F7A14
	public Vector2 UnitTopLeft
	{
		get
		{
			return new Vector2((float)this.Min.x, (float)this.Max.y) / 16f;
		}
	}

	// Token: 0x170008C0 RID: 2240
	// (get) Token: 0x06002F51 RID: 12113 RVA: 0x000F9850 File Offset: 0x000F7A50
	public Vector2 UnitTopRight
	{
		get
		{
			return this.Max.ToVector2() / 16f;
		}
	}

	// Token: 0x170008C1 RID: 2241
	// (get) Token: 0x06002F52 RID: 12114 RVA: 0x000F9878 File Offset: 0x000F7A78
	public Vector2 UnitTopCenter
	{
		get
		{
			return (this.Min.ToVector2() + new Vector2((float)this.Width / 2f, (float)this.Height)) / 16f;
		}
	}

	// Token: 0x170008C2 RID: 2242
	// (get) Token: 0x06002F53 RID: 12115 RVA: 0x000F98BC File Offset: 0x000F7ABC
	public Vector2 UnitCenterLeft
	{
		get
		{
			return (this.Min.ToVector2() + new Vector2(0f, (float)this.Height / 2f)) / 16f;
		}
	}

	// Token: 0x170008C3 RID: 2243
	// (get) Token: 0x06002F54 RID: 12116 RVA: 0x000F9900 File Offset: 0x000F7B00
	public Vector2 UnitCenter
	{
		get
		{
			return (this.Min.ToVector2() + new Vector2((float)this.Width / 2f, (float)this.Height / 2f)) / 16f;
		}
	}

	// Token: 0x170008C4 RID: 2244
	// (get) Token: 0x06002F55 RID: 12117 RVA: 0x000F994C File Offset: 0x000F7B4C
	public Vector2 UnitCenterRight
	{
		get
		{
			return (this.Min.ToVector2() + new Vector2((float)this.Width, (float)this.Height / 2f)) / 16f;
		}
	}

	// Token: 0x170008C5 RID: 2245
	// (get) Token: 0x06002F56 RID: 12118 RVA: 0x000F9990 File Offset: 0x000F7B90
	public Vector2 UnitBottomLeft
	{
		get
		{
			return this.Min.ToVector2() / 16f;
		}
	}

	// Token: 0x170008C6 RID: 2246
	// (get) Token: 0x06002F57 RID: 12119 RVA: 0x000F99B8 File Offset: 0x000F7BB8
	public Vector2 UnitBottomCenter
	{
		get
		{
			return (this.Min.ToVector2() + new Vector2((float)this.Width / 2f, 0f)) / 16f;
		}
	}

	// Token: 0x170008C7 RID: 2247
	// (get) Token: 0x06002F58 RID: 12120 RVA: 0x000F99FC File Offset: 0x000F7BFC
	public Vector2 UnitBottomRight
	{
		get
		{
			return new Vector2((float)this.Max.x, (float)this.Min.y) / 16f;
		}
	}

	// Token: 0x170008C8 RID: 2248
	// (get) Token: 0x06002F59 RID: 12121 RVA: 0x000F9A38 File Offset: 0x000F7C38
	public Vector2 UnitDimensions
	{
		get
		{
			return this.Dimensions.ToVector2() / 16f;
		}
	}

	// Token: 0x170008C9 RID: 2249
	// (get) Token: 0x06002F5A RID: 12122 RVA: 0x000F9A60 File Offset: 0x000F7C60
	public float UnitLeft
	{
		get
		{
			return (float)this.MinX / 16f;
		}
	}

	// Token: 0x170008CA RID: 2250
	// (get) Token: 0x06002F5B RID: 12123 RVA: 0x000F9A70 File Offset: 0x000F7C70
	public float UnitRight
	{
		get
		{
			return (float)(this.MaxX + 1) / 16f;
		}
	}

	// Token: 0x170008CB RID: 2251
	// (get) Token: 0x06002F5C RID: 12124 RVA: 0x000F9A84 File Offset: 0x000F7C84
	public float UnitBottom
	{
		get
		{
			return (float)this.MinY / 16f;
		}
	}

	// Token: 0x170008CC RID: 2252
	// (get) Token: 0x06002F5D RID: 12125 RVA: 0x000F9A94 File Offset: 0x000F7C94
	public float UnitTop
	{
		get
		{
			return (float)(this.MaxY + 1) / 16f;
		}
	}

	// Token: 0x170008CD RID: 2253
	// (get) Token: 0x06002F5E RID: 12126 RVA: 0x000F9AA8 File Offset: 0x000F7CA8
	public float UnitWidth
	{
		get
		{
			return (float)this.Dimensions.x / 16f;
		}
	}

	// Token: 0x170008CE RID: 2254
	// (get) Token: 0x06002F5F RID: 12127 RVA: 0x000F9ACC File Offset: 0x000F7CCC
	public float UnitHeight
	{
		get
		{
			return (float)this.Dimensions.y / 16f;
		}
	}

	// Token: 0x170008CF RID: 2255
	// (get) Token: 0x06002F60 RID: 12128 RVA: 0x000F9AF0 File Offset: 0x000F7CF0
	// (set) Token: 0x06002F61 RID: 12129 RVA: 0x000F9AF8 File Offset: 0x000F7CF8
	public IntVector2 Position
	{
		get
		{
			return this.m_position;
		}
		set
		{
			this.m_position = value;
		}
	}

	// Token: 0x170008D0 RID: 2256
	// (get) Token: 0x06002F62 RID: 12130 RVA: 0x000F9B04 File Offset: 0x000F7D04
	// (set) Token: 0x06002F63 RID: 12131 RVA: 0x000F9B0C File Offset: 0x000F7D0C
	public IntVector2 Dimensions
	{
		get
		{
			return this.m_dimensions;
		}
		set
		{
			this.m_dimensions = value;
		}
	}

	// Token: 0x170008D1 RID: 2257
	// (get) Token: 0x06002F64 RID: 12132 RVA: 0x000F9B18 File Offset: 0x000F7D18
	public IntVector2 Offset
	{
		get
		{
			return this.m_offset + this.m_transformOffset;
		}
	}

	// Token: 0x170008D2 RID: 2258
	// (get) Token: 0x06002F65 RID: 12133 RVA: 0x000F9B2C File Offset: 0x000F7D2C
	public Vector2 UnitOffset
	{
		get
		{
			return PhysicsEngine.PixelToUnit(this.Offset);
		}
	}

	// Token: 0x170008D3 RID: 2259
	// (get) Token: 0x06002F66 RID: 12134 RVA: 0x000F9B3C File Offset: 0x000F7D3C
	public IntVector2 Min
	{
		get
		{
			return this.m_position;
		}
	}

	// Token: 0x170008D4 RID: 2260
	// (get) Token: 0x06002F67 RID: 12135 RVA: 0x000F9B44 File Offset: 0x000F7D44
	public IntVector2 Max
	{
		get
		{
			return this.m_position + this.m_dimensions - IntVector2.One;
		}
	}

	// Token: 0x170008D5 RID: 2261
	// (get) Token: 0x06002F68 RID: 12136 RVA: 0x000F9B64 File Offset: 0x000F7D64
	public int MinX
	{
		get
		{
			return this.m_position.x;
		}
	}

	// Token: 0x170008D6 RID: 2262
	// (get) Token: 0x06002F69 RID: 12137 RVA: 0x000F9B74 File Offset: 0x000F7D74
	public int MaxX
	{
		get
		{
			return this.m_position.x + this.m_dimensions.x - 1;
		}
	}

	// Token: 0x170008D7 RID: 2263
	// (get) Token: 0x06002F6A RID: 12138 RVA: 0x000F9B90 File Offset: 0x000F7D90
	public int MinY
	{
		get
		{
			return this.m_position.y;
		}
	}

	// Token: 0x170008D8 RID: 2264
	// (get) Token: 0x06002F6B RID: 12139 RVA: 0x000F9BA0 File Offset: 0x000F7DA0
	public int MaxY
	{
		get
		{
			return this.m_position.y + this.m_dimensions.y - 1;
		}
	}

	// Token: 0x170008D9 RID: 2265
	// (get) Token: 0x06002F6C RID: 12140 RVA: 0x000F9BBC File Offset: 0x000F7DBC
	public IntVector2 LowerLeft
	{
		get
		{
			return this.m_position;
		}
	}

	// Token: 0x170008DA RID: 2266
	// (get) Token: 0x06002F6D RID: 12141 RVA: 0x000F9BC4 File Offset: 0x000F7DC4
	public IntVector2 LowerRight
	{
		get
		{
			return new IntVector2(this.m_position.x + this.m_dimensions.x - 1, this.m_position.y);
		}
	}

	// Token: 0x170008DB RID: 2267
	// (get) Token: 0x06002F6E RID: 12142 RVA: 0x000F9BF0 File Offset: 0x000F7DF0
	public IntVector2 UpperLeft
	{
		get
		{
			return new IntVector2(this.m_position.x, this.m_position.y + this.m_dimensions.y - 1);
		}
	}

	// Token: 0x170008DC RID: 2268
	// (get) Token: 0x06002F6F RID: 12143 RVA: 0x000F9C1C File Offset: 0x000F7E1C
	public IntVector2 UpperRight
	{
		get
		{
			return this.m_position + this.m_dimensions - IntVector2.One;
		}
	}

	// Token: 0x170008DD RID: 2269
	// (get) Token: 0x06002F70 RID: 12144 RVA: 0x000F9C3C File Offset: 0x000F7E3C
	public int X
	{
		get
		{
			return this.m_position.x;
		}
	}

	// Token: 0x170008DE RID: 2270
	// (get) Token: 0x06002F71 RID: 12145 RVA: 0x000F9C4C File Offset: 0x000F7E4C
	public int Y
	{
		get
		{
			return this.m_position.y;
		}
	}

	// Token: 0x170008DF RID: 2271
	// (get) Token: 0x06002F72 RID: 12146 RVA: 0x000F9C5C File Offset: 0x000F7E5C
	public int Width
	{
		get
		{
			return this.m_dimensions.x;
		}
	}

	// Token: 0x170008E0 RID: 2272
	// (get) Token: 0x06002F73 RID: 12147 RVA: 0x000F9C6C File Offset: 0x000F7E6C
	public int Height
	{
		get
		{
			return this.m_dimensions.y;
		}
	}

	// Token: 0x170008E1 RID: 2273
	// (get) Token: 0x06002F74 RID: 12148 RVA: 0x000F9C7C File Offset: 0x000F7E7C
	// (set) Token: 0x06002F75 RID: 12149 RVA: 0x000F9C84 File Offset: 0x000F7E84
	public bool IsSlope { get; set; }

	// Token: 0x170008E2 RID: 2274
	// (get) Token: 0x06002F76 RID: 12150 RVA: 0x000F9C90 File Offset: 0x000F7E90
	// (set) Token: 0x06002F77 RID: 12151 RVA: 0x000F9C98 File Offset: 0x000F7E98
	public float Slope { get; set; }

	// Token: 0x170008E3 RID: 2275
	// (get) Token: 0x06002F78 RID: 12152 RVA: 0x000F9CA4 File Offset: 0x000F7EA4
	// (set) Token: 0x06002F79 RID: 12153 RVA: 0x000F9CAC File Offset: 0x000F7EAC
	public IntVector2 UpslopeDirection { get; set; }

	// Token: 0x170008E4 RID: 2276
	// (get) Token: 0x06002F7A RID: 12154 RVA: 0x000F9CB8 File Offset: 0x000F7EB8
	public Vector2 SlopeStart
	{
		get
		{
			return this.m_slopeStart.Value;
		}
	}

	// Token: 0x170008E5 RID: 2277
	// (get) Token: 0x06002F7B RID: 12155 RVA: 0x000F9CC8 File Offset: 0x000F7EC8
	public Vector2 SlopeEnd
	{
		get
		{
			return this.m_slopeEnd.Value;
		}
	}

	// Token: 0x170008E6 RID: 2278
	// (get) Token: 0x06002F7C RID: 12156 RVA: 0x000F9CD8 File Offset: 0x000F7ED8
	// (set) Token: 0x06002F7D RID: 12157 RVA: 0x000F9CE0 File Offset: 0x000F7EE0
	public bool IsTileCollider { get; set; }

	// Token: 0x170008E7 RID: 2279
	// (get) Token: 0x06002F7E RID: 12158 RVA: 0x000F9CEC File Offset: 0x000F7EEC
	// (set) Token: 0x06002F7F RID: 12159 RVA: 0x000F9CF4 File Offset: 0x000F7EF4
	public float Rotation
	{
		get
		{
			return this.m_rotation;
		}
		set
		{
			this.SetRotationAndScale(value, this.m_scale);
		}
	}

	// Token: 0x170008E8 RID: 2280
	// (get) Token: 0x06002F80 RID: 12160 RVA: 0x000F9D04 File Offset: 0x000F7F04
	// (set) Token: 0x06002F81 RID: 12161 RVA: 0x000F9D0C File Offset: 0x000F7F0C
	public Vector2 Scale
	{
		get
		{
			return this.m_scale;
		}
		set
		{
			this.SetRotationAndScale(this.m_rotation, value);
		}
	}

	// Token: 0x170008E9 RID: 2281
	// (get) Token: 0x06002F82 RID: 12162 RVA: 0x000F9D1C File Offset: 0x000F7F1C
	// (set) Token: 0x06002F83 RID: 12163 RVA: 0x000F9D24 File Offset: 0x000F7F24
	public int CollisionLayerCollidableOverride { get; set; }

	// Token: 0x170008EA RID: 2282
	// (get) Token: 0x06002F84 RID: 12164 RVA: 0x000F9D30 File Offset: 0x000F7F30
	// (set) Token: 0x06002F85 RID: 12165 RVA: 0x000F9D38 File Offset: 0x000F7F38
	public int CollisionLayerIgnoreOverride { get; set; }

	// Token: 0x170008EB RID: 2283
	// (get) Token: 0x06002F86 RID: 12166 RVA: 0x000F9D44 File Offset: 0x000F7F44
	public List<TriggerCollisionData> TriggerCollisions
	{
		get
		{
			return this.m_triggerCollisions;
		}
	}

	// Token: 0x170008EC RID: 2284
	public bool this[int x, int y]
	{
		get
		{
			return this.m_bestPixels[x, y];
		}
	}

	// Token: 0x170008ED RID: 2285
	public bool this[IntVector2 pos]
	{
		get
		{
			return this.m_bestPixels[pos.x, pos.y];
		}
	}

	// Token: 0x06002F89 RID: 12169 RVA: 0x000F9D78 File Offset: 0x000F7F78
	public bool AABBOverlaps(PixelCollider otherCollider)
	{
		return IntVector2.AABBOverlap(this.m_position, this.m_dimensions, otherCollider.m_position, otherCollider.m_dimensions);
	}

	// Token: 0x06002F8A RID: 12170 RVA: 0x000F9D98 File Offset: 0x000F7F98
	public bool AABBOverlaps(PixelCollider otherCollider, IntVector2 pixelsToMove)
	{
		int num = Mathf.Min(this.m_position.x, this.m_position.x + pixelsToMove.x);
		int num2 = this.m_position.x + this.m_dimensions.x - 1;
		int num3 = Mathf.Max(num2, num2 + pixelsToMove.x) - num + 1;
		if (num + num3 - 1 < otherCollider.m_position.x)
		{
			return false;
		}
		if (num > otherCollider.m_position.x + otherCollider.m_dimensions.x - 1)
		{
			return false;
		}
		int num4 = this.m_position.y + this.m_dimensions.y - 1;
		int num5 = Mathf.Min(this.m_position.y, this.m_position.y + pixelsToMove.y);
		int num6 = Mathf.Max(num4, num4 + pixelsToMove.y) - num5 + 1;
		return num5 + num6 - 1 >= otherCollider.m_position.y && num5 <= otherCollider.m_position.y + otherCollider.m_dimensions.y - 1;
	}

	// Token: 0x06002F8B RID: 12171 RVA: 0x000F9EC0 File Offset: 0x000F80C0
	public bool AABBOverlaps(IntVector2 pos, IntVector2 dimensions)
	{
		return IntVector2.AABBOverlap(this.m_position, this.m_dimensions, pos, dimensions);
	}

	// Token: 0x06002F8C RID: 12172 RVA: 0x000F9ED8 File Offset: 0x000F80D8
	public bool Overlaps(PixelCollider otherCollider)
	{
		return this.Overlaps(otherCollider, IntVector2.Zero);
	}

	// Token: 0x06002F8D RID: 12173 RVA: 0x000F9EE8 File Offset: 0x000F80E8
	public bool Overlaps(PixelCollider otherCollider, IntVector2 otherColliderOffset)
	{
		IntVector2 intVector = otherCollider.m_position - this.m_position + otherColliderOffset;
		int num = Math.Max(0, intVector.x);
		int num2 = Math.Max(0, intVector.y);
		int num3 = Math.Min(this.m_bestPixels.Width - 1, otherCollider.m_bestPixels.Width - 1 + intVector.x);
		int num4 = Math.Min(this.m_bestPixels.Height - 1, otherCollider.m_bestPixels.Height - 1 + intVector.y);
		for (int i = num; i <= num3; i++)
		{
			for (int j = num2; j <= num4; j++)
			{
				if (this.m_bestPixels[i, j] && otherCollider.m_bestPixels[i - intVector.x, j - intVector.y])
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x06002F8E RID: 12174 RVA: 0x000F9FE4 File Offset: 0x000F81E4
	public bool CanCollideWith(int mask, CollisionLayer? sourceLayer = null)
	{
		if (!this.Enabled)
		{
			return false;
		}
		if ((mask & this.CollisionLayerCollidableOverride) > 0)
		{
			return true;
		}
		if (sourceLayer != null)
		{
			int num = CollisionMask.LayerToMask(sourceLayer.Value);
			if ((num & this.CollisionLayerCollidableOverride) == num)
			{
				return true;
			}
			if (this.IsTileCollider && sourceLayer.Value == CollisionLayer.TileBlocker)
			{
				return true;
			}
		}
		int num2 = CollisionMask.LayerToMask(this.CollisionLayer);
		return (mask & num2) == num2;
	}

	// Token: 0x06002F8F RID: 12175 RVA: 0x000FA064 File Offset: 0x000F8264
	public bool CanCollideWith(PixelCollider otherCollider, bool ignoreFrameSpecificExceptions = false)
	{
		if (!this.Enabled || !otherCollider.Enabled)
		{
			return false;
		}
		if (this.IsTileCollider && otherCollider.CollisionLayer == CollisionLayer.TileBlocker)
		{
			return true;
		}
		int num = CollisionMask.LayerToMask(this.CollisionLayer);
		int num2 = CollisionMask.LayerToMask(otherCollider.CollisionLayer);
		if ((num & otherCollider.CollisionLayerCollidableOverride) != num && (num2 & this.CollisionLayerCollidableOverride) != num2)
		{
			int num3 = CollisionLayerMatrix.GetMask(otherCollider.CollisionLayer);
			num3 &= ~otherCollider.CollisionLayerIgnoreOverride;
			if ((num3 & num) != num)
			{
				return false;
			}
			int num4 = CollisionLayerMatrix.GetMask(this.CollisionLayer);
			num4 &= ~this.CollisionLayerIgnoreOverride;
			if ((num4 & num2) != num2)
			{
				return false;
			}
		}
		if (!ignoreFrameSpecificExceptions)
		{
			if (this.m_frameSpecificCollisionExceptions.Count > 0 && this.m_frameSpecificCollisionExceptions.Contains(otherCollider))
			{
				return false;
			}
			if (otherCollider.m_frameSpecificCollisionExceptions.Count > 0 && otherCollider.m_frameSpecificCollisionExceptions.Contains(this))
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x06002F90 RID: 12176 RVA: 0x000FA174 File Offset: 0x000F8374
	public bool CanCollideWith(CollisionLayer collisionLayer)
	{
		if (!this.Enabled)
		{
			return false;
		}
		if (this.IsTileCollider && collisionLayer == CollisionLayer.TileBlocker)
		{
			return true;
		}
		int num = CollisionMask.LayerToMask(collisionLayer);
		int mask = CollisionLayerMatrix.GetMask(this.CollisionLayer);
		return (mask & num) == num;
	}

	// Token: 0x06002F91 RID: 12177 RVA: 0x000FA1BC File Offset: 0x000F83BC
	public bool Raycast(Vector2 origin, Vector2 direction, float distance, out RaycastResult result)
	{
		result = null;
		if (!this.Enabled)
		{
			return false;
		}
		direction.Normalize();
		IntVector2 intVector = PhysicsEngine.UnitToPixel(origin);
		IntVector2 intVector2 = PhysicsEngine.UnitToPixel(direction * distance);
		intVector2 += new IntVector2((int)Mathf.Sign(direction.x), (int)Mathf.Sign(direction.y));
		if (intVector2.x < 0)
		{
			intVector2.x *= -1;
			intVector.x -= intVector2.x;
		}
		if (intVector2.y < 0)
		{
			intVector2.y *= -1;
			intVector.y -= intVector2.y;
		}
		if (!IntVector2.AABBOverlap(intVector, intVector2, this.m_position, this.m_dimensions))
		{
			return false;
		}
		Vector2 vector = origin + distance * direction;
		Vector2 vector2 = this.m_position.ToVector2() / 16f;
		Vector2 vector3 = this.m_dimensions.ToVector2() / 16f;
		bool flag = origin.IsWithin(vector2, vector2 + vector3);
		Vector2 vector4;
		if (!BraveUtility.LineIntersectsAABB(origin, vector, vector2, vector3, out vector4) && !flag)
		{
			return false;
		}
		if (this.DirectionIgnorer != null && this.DirectionIgnorer(PhysicsEngine.UnitToPixel(direction * distance)))
		{
			return false;
		}
		IntVector2 intVector3 = IntVector2.NegOne;
		IntVector2 intVector4 = IntVector2.NegOne;
		float num = 0f;
		Vector2 vector5;
		if (flag)
		{
			vector5 = origin * 16f;
			intVector3 = new IntVector2((int)vector5.x, (int)vector5.y);
			intVector4 = IntVector2.Zero;
		}
		else
		{
			float num2 = Mathf.Abs(vector4.x - PhysicsEngine.PixelToUnit(this.Min.x));
			float num3 = Mathf.Abs(vector4.x - PhysicsEngine.PixelToUnit(this.Max.x + 1));
			float num4 = Mathf.Abs(vector4.y - PhysicsEngine.PixelToUnit(this.Min.y));
			float num5 = Mathf.Abs(vector4.y - PhysicsEngine.PixelToUnit(this.Max.y + 1));
			if (num2 <= num3 && num2 <= num5 && num2 <= num4 && direction.x > 0f)
			{
				intVector3 = new IntVector2(this.Min.X - 1, PhysicsEngine.UnitToPixel(vector4.y));
				intVector4 = IntVector2.Right;
				vector5 = new Vector2((float)this.Min.X, vector4.y * 16f);
			}
			else if (num3 <= num2 && num3 <= num5 && num3 <= num4 && direction.x < 0f)
			{
				intVector3 = new IntVector2(this.Max.X + 1, PhysicsEngine.UnitToPixel(vector4.y));
				intVector4 = IntVector2.Left;
				vector5 = new Vector2((float)(this.Max.X + 1), vector4.y * 16f);
			}
			else if (num4 <= num3 && num4 <= num5 && num4 <= num2 && direction.y > 0f)
			{
				intVector3 = new IntVector2(PhysicsEngine.UnitToPixel(vector4.x), this.Min.Y - 1);
				intVector4 = IntVector2.Up;
				vector5 = new Vector2(vector4.x * 16f, (float)this.Min.y);
			}
			else
			{
				if (num5 > num3 || num5 > num2 || num5 > num4 || direction.y >= 0f)
				{
					return false;
				}
				intVector3 = new IntVector2(PhysicsEngine.UnitToPixel(vector4.x), this.Max.y + 1);
				intVector4 = IntVector2.Down;
				vector5 = new Vector2(vector4.x * 16f, (float)(this.Max.y + 1));
			}
			num = Vector2.Distance(origin, vector4);
		}
		bool flag2 = false;
		int num6 = Math.Sign(direction.x);
		int num7 = Math.Sign(direction.y);
		while ((!flag2 || this.AABBContainsPixel(intVector3)) && num < distance)
		{
			IntVector2 intVector5 = intVector3 + intVector4;
			if (this.AABBContainsPixel(intVector5))
			{
				flag2 = true;
				if (this[intVector5 - this.Position])
				{
					result = RaycastResult.Pool.Allocate();
					result.Contact = vector5 / 16f;
					result.HitPixel = intVector5;
					result.LastRayPixel = intVector3;
					result.Distance = num;
					result.Normal = ((Vector2)(-intVector4)).normalized;
					if (this.NormalModifier != null)
					{
						result.Normal = this.NormalModifier(result.Normal);
					}
					result.OtherPixelCollider = this;
					return true;
				}
			}
			intVector3 = intVector5;
			float num8 = ((direction.x == 0f) ? float.PositiveInfinity : ((float)(intVector3.x + num6) - vector5.x));
			float num9 = ((direction.y == 0f) ? float.PositiveInfinity : ((float)(intVector3.y + num7) - vector5.y));
			if (num6 < 0)
			{
				num8 += 1f;
			}
			if (num7 < 0)
			{
				num9 += 1f;
			}
			float num10 = ((direction.x == 0f) ? float.PositiveInfinity : (num8 / direction.x));
			float num11 = ((direction.y == 0f) ? float.PositiveInfinity : (num9 / direction.y));
			Vector2 vector6 = vector5;
			if (num10 < num11)
			{
				intVector4 = new IntVector2(num6, 0);
				vector5.x += num8;
				if (direction.y != 0f && num10 != 0f)
				{
					vector5.y += direction.y * num10;
				}
				num += Vector2.Distance(vector6, vector5) / 16f;
			}
			else
			{
				intVector4 = new IntVector2(0, num7);
				if (direction.x != 0f && num11 != 0f)
				{
					vector5.x += direction.x * num11;
				}
				vector5.y += num9;
				num += Vector2.Distance(vector6, vector5) / 16f;
			}
		}
		return false;
	}

	// Token: 0x06002F92 RID: 12178 RVA: 0x000FA8C8 File Offset: 0x000F8AC8
	public bool LinearCast(PixelCollider otherCollider, IntVector2 pixelsToMove, out LinearCastResult result)
	{
		PhysicsEngine.PixelMovementGenerator(pixelsToMove, PixelCollider.m_stepList);
		return this.LinearCast(otherCollider, pixelsToMove, PixelCollider.m_stepList, out result, false, 0f);
	}

	// Token: 0x06002F93 RID: 12179 RVA: 0x000FA8EC File Offset: 0x000F8AEC
	public bool LinearCast(PixelCollider otherCollider, IntVector2 pixelsToMove, List<PixelCollider.StepData> stepList, out LinearCastResult result, bool traverseSlopes = false, float currentSlope = 0f)
	{
		if (!this.Enabled)
		{
			result = null;
			return false;
		}
		if (otherCollider.DirectionIgnorer != null && otherCollider.DirectionIgnorer(pixelsToMove))
		{
			result = null;
			return false;
		}
		IntVector2 intVector = IntVector2.Zero;
		IntVector2 intVector2 = otherCollider.m_position - this.m_position;
		result = LinearCastResult.Pool.Allocate();
		result.MyPixelCollider = this;
		result.OtherPixelCollider = null;
		result.TimeUsed = 0f;
		result.CollidedX = false;
		result.CollidedY = false;
		result.NewPixelsToMove.x = 0;
		result.NewPixelsToMove.y = 0;
		result.Overlap = false;
		float num = 0f;
		for (int i = 0; i < stepList.Count; i++)
		{
			IntVector2 deltaPos = stepList[i].deltaPos;
			float deltaTime = stepList[i].deltaTime;
			num += deltaTime;
			IntVector2 intVector3 = this.m_position + intVector + deltaPos;
			if (IntVector2.AABBOverlap(intVector3, this.m_dimensions, otherCollider.Position, otherCollider.Dimensions))
			{
				IntVector2 intVector4 = IntVector2.Max(IntVector2.Zero, otherCollider.Position - intVector3);
				IntVector2 intVector5 = IntVector2.Min(this.m_dimensions - IntVector2.One, otherCollider.UpperRight - intVector3);
				for (int j = intVector4.x; j <= intVector5.x; j++)
				{
					for (int k = intVector4.y; k <= intVector5.y; k++)
					{
						if (this.m_bestPixels[j, k])
						{
							IntVector2 intVector6 = new IntVector2(j, k) - intVector2 + intVector + deltaPos;
							if (intVector6.x >= 0 && intVector6.x < otherCollider.Dimensions.x && intVector6.y >= 0 && intVector6.y < otherCollider.Dimensions.y && otherCollider[intVector6])
							{
								if (!otherCollider.IsSlope || !traverseSlopes || otherCollider.Slope != currentSlope)
								{
									result.TimeUsed = num;
									result.CollidedX = deltaPos.x != 0;
									result.CollidedY = deltaPos.y != 0;
									result.NewPixelsToMove = intVector;
									if (!otherCollider.IsSlope || deltaPos.y == 1 || deltaPos.y < 0 || Math.Sign(deltaPos.x) == Math.Sign(otherCollider.SlopeEnd.y - otherCollider.SlopeStart.y))
									{
									}
									result.MyPixelCollider = this;
									result.OtherPixelCollider = otherCollider;
									IntVector2 intVector7 = this.Position + new IntVector2(j, k) + intVector + deltaPos;
									result.Contact = PixelCollider.FromCollisionVector(intVector7) + new Vector2(0.5f, 0.5f) / 16f;
									result.Normal = (Vector2)(-deltaPos);
									if (otherCollider.NormalModifier != null)
									{
										result.Normal = otherCollider.NormalModifier(result.Normal);
									}
									return true;
								}
							}
						}
					}
				}
			}
			intVector += deltaPos;
		}
		result.NewPixelsToMove = intVector;
		return false;
	}

	// Token: 0x06002F94 RID: 12180 RVA: 0x000FACA0 File Offset: 0x000F8EA0
	public bool AABBContainsPixel(IntVector2 pixel)
	{
		return pixel.x >= this.Min.x && pixel.x <= this.Max.x && pixel.y >= this.Min.y && pixel.y <= this.Max.y;
	}

	// Token: 0x06002F95 RID: 12181 RVA: 0x000FAD18 File Offset: 0x000F8F18
	public bool ContainsPixel(IntVector2 pixel)
	{
		return this.AABBContainsPixel(pixel) && this.m_bestPixels[pixel.x - this.m_position.x, pixel.y - this.m_position.y];
	}

	// Token: 0x06002F96 RID: 12182 RVA: 0x000FAD64 File Offset: 0x000F8F64
	public void SetRotationAndScale(float rotation, Vector2 scale)
	{
		BitArray2D bitArray2D = ((rotation != 0f || !(scale == Vector2.one)) ? this.m_modifiedPixels : this.m_basePixels);
		if (this.m_rotation == rotation && this.m_scale == scale && this.m_bestPixels == bitArray2D && this.m_bestPixels != null && this.m_bestPixels.IsValid)
		{
			return;
		}
		this.m_rotation = rotation;
		this.m_scale = scale;
		int width = this.m_basePixels.Width;
		int height = this.m_basePixels.Height;
		if (rotation == 0f && scale == Vector2.one)
		{
			this.m_bestPixels = this.m_basePixels;
			this.m_dimensions = new IntVector2(width, height);
			this.m_transformOffset = IntVector2.Zero;
			return;
		}
		if (this.m_modifiedPixels == null)
		{
			this.m_modifiedPixels = new BitArray2D(false);
		}
		Vector2 vector = -(Vector2)this.m_offset;
		Vector2 vector2 = this.TransformPixel(new Vector2(0.5f, 0.5f), vector, rotation, scale);
		Vector2 vector3 = this.TransformPixel(new Vector2((float)width - 0.5f, 0.5f), vector, rotation, scale);
		Vector2 vector4 = this.TransformPixel(new Vector2(0.5f, (float)height - 0.5f), vector, rotation, scale);
		Vector2 vector5 = this.TransformPixel(new Vector2((float)width - 0.5f, (float)height - 0.5f), vector, rotation, scale);
		int num = Mathf.FloorToInt(Mathf.Min(new float[] { vector2.x, vector3.x, vector4.x, vector5.x }));
		int num2 = Mathf.FloorToInt(Mathf.Min(new float[] { vector2.y, vector3.y, vector4.y, vector5.y }));
		int num3 = Mathf.CeilToInt(Mathf.Max(new float[] { vector2.x, vector3.x, vector4.x, vector5.x }));
		int num4 = Mathf.CeilToInt(Mathf.Max(new float[] { vector2.y, vector3.y, vector4.y, vector5.y }));
		this.m_transformOffset = new IntVector2(num, num2);
		Vector2 vector6 = vector - (Vector2)this.m_transformOffset;
		int num5 = num3 - num;
		int num6 = num4 - num2;
		this.m_modifiedPixels.ReinitializeWithDefault(num5, num6, false, 0f, false);
		if (this.m_basePixels.IsAabb)
		{
			int num7 = 4;
			Vector2[] array = new Vector2[]
			{
				vector2 - this.m_transformOffset.ToVector2(),
				vector3 - this.m_transformOffset.ToVector2(),
				vector5 - this.m_transformOffset.ToVector2(),
				vector4 - this.m_transformOffset.ToVector2()
			};
			int[] array2 = new int[4];
			for (int i = 0; i < num6; i++)
			{
				int num8 = 0;
				int num9 = num7 - 1;
				int j;
				for (j = 0; j < num7; j++)
				{
					if (((double)array[j].y < (double)i && (double)array[num9].y >= (double)i) || ((double)array[num9].y < (double)i && (double)array[j].y >= (double)i))
					{
						array2[num8++] = (int)(array[j].x + ((float)i - array[j].y) / (array[num9].y - array[j].y) * (array[num9].x - array[j].x));
					}
					num9 = j;
				}
				j = 0;
				while (j < num8 - 1)
				{
					if (array2[j] > array2[j + 1])
					{
						int num10 = array2[j];
						array2[j] = array2[j + 1];
						array2[j + 1] = num10;
						if (j != 0)
						{
							j--;
						}
					}
					else
					{
						j++;
					}
				}
				for (j = 0; j < num8; j += 2)
				{
					if (array2[j] >= num5 - 1)
					{
						break;
					}
					if (array2[j + 1] > 0)
					{
						if (array2[j] < 0)
						{
							array2[j] = 0;
						}
						if (array2[j + 1] > num5 - 1)
						{
							array2[j + 1] = num5 - 1;
						}
						for (int k = array2[j]; k < array2[j + 1]; k++)
						{
							this.m_modifiedPixels[k, i] = true;
						}
					}
				}
			}
		}
		else
		{
			float num11 = -rotation;
			Vector2 vector7 = new Vector2(1f / scale.x, 1f / scale.y);
			for (int l = 0; l < num5; l++)
			{
				for (int m = 0; m < num6; m++)
				{
					Vector2 vector8 = new Vector2((float)l + 0.5f, (float)m + 0.5f);
					Vector2 vector9 = this.TransformPixel(vector8, vector6, num11, vector7) + (Vector2)this.m_transformOffset;
					if (vector9.x < 0f || (int)vector9.x >= width || vector9.y < 0f || (int)vector9.y >= height)
					{
						this.m_modifiedPixels[l, m] = false;
					}
					else
					{
						this.m_modifiedPixels[l, m] = this.m_basePixels[(int)vector9.x, (int)vector9.y];
					}
				}
			}
		}
		this.m_dimensions = new IntVector2(num5, num6);
		this.m_bestPixels = this.m_modifiedPixels;
	}

	// Token: 0x06002F97 RID: 12183 RVA: 0x000FB3C4 File Offset: 0x000F95C4
	private Vector2 TransformPixel(Vector2 pixel, Vector2 pivot, float rotation, Vector2 scale)
	{
		Vector2 vector = pixel - pivot;
		Vector2 vector2;
		vector2.x = vector.x * Mathf.Cos(rotation * 0.017453292f) - vector.y * Mathf.Sin(rotation * 0.017453292f);
		vector2.y = vector.x * Mathf.Sin(rotation * 0.017453292f) + vector.y * Mathf.Cos(rotation * 0.017453292f);
		return Vector2.Scale(vector2, scale) + pivot;
	}

	// Token: 0x06002F98 RID: 12184 RVA: 0x000FB448 File Offset: 0x000F9648
	public void RegisterFrameSpecificCollisionException(SpeculativeRigidbody mySpecRigidbody, PixelCollider pixelCollider)
	{
		if (!this.m_frameSpecificCollisionExceptions.Contains(pixelCollider))
		{
			this.m_frameSpecificCollisionExceptions.Add(pixelCollider);
			mySpecRigidbody.HasFrameSpecificCollisionExceptions = true;
		}
	}

	// Token: 0x06002F99 RID: 12185 RVA: 0x000FB470 File Offset: 0x000F9670
	public void ClearFrameSpecificCollisionExceptions()
	{
		this.m_frameSpecificCollisionExceptions.Clear();
	}

	// Token: 0x06002F9A RID: 12186 RVA: 0x000FB480 File Offset: 0x000F9680
	public TriggerCollisionData RegisterTriggerCollision(SpeculativeRigidbody mySpecRigidbody, SpeculativeRigidbody otherSpecRigidbody, PixelCollider otherPixelCollider)
	{
		TriggerCollisionData triggerCollisionData = this.m_triggerCollisions.Find((TriggerCollisionData d) => d.PixelCollider == otherPixelCollider);
		if (triggerCollisionData == null)
		{
			triggerCollisionData = new TriggerCollisionData(otherSpecRigidbody, otherPixelCollider);
			this.m_triggerCollisions.Add(triggerCollisionData);
			mySpecRigidbody.HasTriggerCollisions = true;
		}
		else
		{
			triggerCollisionData.ContinuedCollision = true;
		}
		return triggerCollisionData;
	}

	// Token: 0x06002F9B RID: 12187 RVA: 0x000FB4E8 File Offset: 0x000F96E8
	public void ResetTriggerCollisionData()
	{
		for (int i = 0; i < this.m_triggerCollisions.Count; i++)
		{
			this.m_triggerCollisions[i].Reset();
		}
	}

	// Token: 0x06002F9C RID: 12188 RVA: 0x000FB524 File Offset: 0x000F9724
	public void Regenerate(Transform transform, bool allowRotation = true, bool allowScale = true)
	{
		if (!this.Sprite)
		{
			this.Sprite = transform.GetComponentInChildren<tk2dBaseSprite>();
		}
		float num = ((!allowRotation) ? 0f : transform.eulerAngles.z);
		Vector2 vector = ((!allowScale) ? Vector2.one : transform.localScale);
		if (allowScale && this.Sprite)
		{
			vector = Vector2.Scale(vector, this.Sprite.scale);
		}
		switch (this.ColliderGenerationMode)
		{
		case PixelCollider.PixelColliderGeneration.Manual:
			this.RegenerateFromManual(transform, new IntVector2(this.ManualOffsetX, this.ManualOffsetY), new IntVector2(this.ManualWidth, this.ManualHeight), num, new Vector2?(vector));
			break;
		case PixelCollider.PixelColliderGeneration.Tk2dPolygon:
			this.RegenerateFrom3dCollider(this.Sprite.GetTrueCurrentSpriteDef().colliderVertices, transform, num, new Vector2?(vector), this.Sprite.FlipX, this.Sprite.FlipY);
			break;
		case PixelCollider.PixelColliderGeneration.BagelCollider:
			this.RegenerateFromBagelCollider(this.Sprite, transform, num, new Vector2?(vector), this.Sprite.FlipX);
			break;
		case PixelCollider.PixelColliderGeneration.Circle:
			if (this.ManualDiameter <= 0 && this.ManualRadius > 0)
			{
				this.ManualDiameter = 2 * this.ManualRadius;
			}
			this.RegenerateFromCircle(transform, new IntVector2(this.ManualOffsetX, this.ManualOffsetY), this.ManualDiameter);
			break;
		case PixelCollider.PixelColliderGeneration.Line:
			this.RegenerateFromLine(transform, new IntVector2(this.ManualLeftX, this.ManualLeftY), new IntVector2(this.ManualRightX, this.ManualRightY));
			break;
		}
	}

	// Token: 0x06002F9D RID: 12189 RVA: 0x000FB6EC File Offset: 0x000F98EC
	public void RegenerateFromManual(Transform transform, IntVector2 offset, IntVector2 dimensions, float rotation = 0f, Vector2? scale = null)
	{
		this.RegenerateFromManual(transform.position, offset, dimensions, rotation, scale);
	}

	// Token: 0x06002F9E RID: 12190 RVA: 0x000FB708 File Offset: 0x000F9908
	public void RegenerateFromManual(Vector2 position, IntVector2 offset, IntVector2 dimensions, float rotation = 0f, Vector2? scale = null)
	{
		if (scale == null)
		{
			scale = new Vector2?(new Vector2(1f, 1f));
		}
		this.m_offset = offset;
		this.m_dimensions = dimensions;
		this.m_position = PixelCollider.ToCollisionVector(position) + this.m_offset;
		this.m_basePixels.ReinitializeWithDefault(this.m_dimensions.x, this.m_dimensions.y, true, 0f, true);
		this.m_bestPixels = this.m_basePixels;
		this.SetRotationAndScale(rotation, scale.Value);
	}

	// Token: 0x06002F9F RID: 12191 RVA: 0x000FB7A4 File Offset: 0x000F99A4
	public void RegenerateFrom3dCollider(Vector3[] allVertices, Transform transform, float rotation = 0f, Vector2? scale = null, bool flipX = false, bool flipY = false)
	{
		if (scale == null)
		{
			scale = new Vector2?(new Vector2(1f, 1f));
		}
		if (allVertices.Length == 2)
		{
			Vector2[] array = new Vector2[4];
			Vector2 vector = allVertices[0];
			Vector2 vector2 = allVertices[1];
			if (flipX)
			{
				vector.x *= -1f;
			}
			if (flipY)
			{
				vector.y *= -1f;
			}
			array[0] = vector + new Vector2(-vector2.x, vector2.y);
			array[1] = vector + new Vector2(-vector2.x, -vector2.y);
			array[2] = vector + new Vector2(vector2.x, -vector2.y);
			array[3] = vector + new Vector2(vector2.x, vector2.y);
			this.RegenerateFromVertices(array, transform, rotation, scale);
		}
		else
		{
			Vector2[] array2 = new Vector2[allVertices.Length / 2];
			int num = 0;
			int num2 = 0;
			while (num2 < allVertices.Length && num < array2.Length)
			{
				if (allVertices[num2].z < 0f)
				{
					Vector2 vector3 = allVertices[num2];
					bool flag = false;
					for (int i = 0; i < num; i++)
					{
						if (Mathf.Approximately(array2[i].x, vector3.x) && Mathf.Approximately(array2[i].y, vector3.y))
						{
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						array2[num++] = vector3;
					}
				}
				num2++;
			}
			array2 = BraveUtility.ResizeArray(array2, num);
			this.RegenerateFromVertices(array2, transform, rotation, scale);
		}
	}

	// Token: 0x06002FA0 RID: 12192 RVA: 0x000FB9D0 File Offset: 0x000F9BD0
	public void RegenerateFromBagelCollider(tk2dBaseSprite sprite, Transform transform, float rotation = 0f, Vector2? scale = null, bool flipX = false)
	{
		if (scale == null)
		{
			scale = new Vector2?(new Vector2(1f, 1f));
		}
		tk2dSpriteDefinition tk2dSpriteDefinition;
		if (this.BagleUseFirstFrameOnly && !string.IsNullOrEmpty(this.SpecifyBagelFrame))
		{
			tk2dSpriteDefinition = sprite.Collection.GetSpriteDefinition(this.SpecifyBagelFrame);
		}
		else
		{
			tk2dSpriteDefinition = sprite.GetTrueCurrentSpriteDef();
		}
		this.m_lastSpriteDef = tk2dSpriteDefinition;
		if (!this.BagleUseFirstFrameOnly && this.m_cachedBasePixels == null)
		{
			this.m_cachedBasePixels = new Dictionary<int, PixelCollider.PixelCache>();
		}
		int num = ((tk2dSpriteDefinition != null) ? sprite.GetSpriteIdByName(tk2dSpriteDefinition.name) : (-1));
		if (!this.BagleUseFirstFrameOnly && this.m_cachedBasePixels.ContainsKey(num))
		{
			PixelCollider.PixelCache pixelCache = this.m_cachedBasePixels[num];
			this.m_dimensions = pixelCache.dimensions;
			this.m_basePixels = pixelCache.basePixels;
			this.m_bestPixels = this.m_basePixels;
			this.m_offset = pixelCache.offset;
		}
		else
		{
			this.m_basePixels = new BitArray2D(false);
			BagelCollider[] bagelColliders = sprite.Collection.GetBagelColliders(num);
			int num2 = ((bagelColliders == null) ? 0 : bagelColliders.Length);
			BagelCollider bagelCollider = ((this.BagelColliderNumber >= num2) ? null : bagelColliders[this.BagelColliderNumber]);
			if (bagelCollider == null)
			{
				this.RegenerateEmptyCollider(transform);
				if (!this.BagleUseFirstFrameOnly)
				{
					PixelCollider.PixelCache pixelCache2 = new PixelCollider.PixelCache();
					pixelCache2.dimensions = this.m_dimensions;
					pixelCache2.basePixels = this.m_basePixels;
					pixelCache2.offset = this.m_offset;
					pixelCache2.basePixels.ReadOnly = true;
					this.m_cachedBasePixels.Add(num, pixelCache2);
				}
				return;
			}
			tk2dSlicedSprite tk2dSlicedSprite = this.Sprite as tk2dSlicedSprite;
			IntVector2 intVector;
			IntVector2 intVector2;
			if (tk2dSlicedSprite)
			{
				intVector = IntVector2.Zero;
				intVector2 = new IntVector2(Mathf.RoundToInt(tk2dSlicedSprite.dimensions.x) - 1, Mathf.RoundToInt(tk2dSlicedSprite.dimensions.y) - 1);
			}
			else
			{
				intVector = IntVector2.MaxValue;
				intVector2 = IntVector2.MinValue;
				for (int i = 0; i < bagelCollider.width; i++)
				{
					for (int j = 0; j < bagelCollider.height; j++)
					{
						if (bagelCollider[i, bagelCollider.height - j - 1])
						{
							intVector = IntVector2.Min(intVector, new IntVector2(i, j));
							intVector2 = IntVector2.Max(intVector2, new IntVector2(i, j));
						}
					}
				}
				if (intVector == IntVector2.MaxValue || intVector2 == IntVector2.MinValue)
				{
					this.RegenerateEmptyCollider(transform);
					return;
				}
			}
			this.m_dimensions = intVector2 - intVector + IntVector2.One;
			this.m_basePixels.Reinitialize(this.m_dimensions.x, this.m_dimensions.y, true);
			this.m_bestPixels = this.m_basePixels;
			if (tk2dSlicedSprite)
			{
				this.m_offset = intVector - tk2dSlicedSprite.anchorOffset.ToIntVector2(VectorConversions.Round);
				tk2dSpriteDefinition trueCurrentSpriteDef = tk2dSlicedSprite.GetTrueCurrentSpriteDef();
				float num3 = trueCurrentSpriteDef.position1.x - trueCurrentSpriteDef.position0.x;
				float num4 = trueCurrentSpriteDef.position2.y - trueCurrentSpriteDef.position0.y;
				float x = trueCurrentSpriteDef.texelSize.x;
				float y = trueCurrentSpriteDef.texelSize.y;
				IntVector2 intVector3 = new IntVector2(Mathf.RoundToInt(num3 / x), Mathf.RoundToInt(num4 / y));
				Vector3 boundsDataExtents = trueCurrentSpriteDef.boundsDataExtents;
				Vector3 vector = new Vector3(boundsDataExtents.x / trueCurrentSpriteDef.texelSize.x, boundsDataExtents.y / trueCurrentSpriteDef.texelSize.y, 1f);
				IntVector2 intVector4 = new IntVector2(Mathf.RoundToInt(tk2dSlicedSprite.dimensions.x), Mathf.RoundToInt(tk2dSlicedSprite.dimensions.y));
				int num5 = Mathf.RoundToInt(tk2dSlicedSprite.borderTop * vector.y);
				int num6 = Mathf.RoundToInt(tk2dSlicedSprite.borderBottom * vector.y);
				int num7 = Mathf.RoundToInt(tk2dSlicedSprite.borderLeft * vector.x);
				int num8 = Mathf.RoundToInt(tk2dSlicedSprite.borderRight * vector.x);
				int num9 = intVector3.x - num7 - num8;
				int num10 = intVector3.y - num5 - num6;
				for (int k = intVector.x; k <= intVector2.x; k++)
				{
					int num11;
					if (k < num7)
					{
						num11 = k;
					}
					else if (k < intVector4.x - num8)
					{
						num11 = (k - num7) % num9 + num7;
					}
					else
					{
						num11 = intVector3.x - (intVector4.x - k);
					}
					for (int l = intVector.y; l <= intVector2.y; l++)
					{
						int num12;
						if (l < num6)
						{
							num12 = l;
						}
						else if (l < intVector4.y - num5)
						{
							num12 = (l - num6) % num10 + num5;
						}
						else
						{
							num12 = intVector3.y - (intVector4.y - l);
						}
						this.m_basePixels[k, l] = bagelCollider[num11, bagelCollider.height - num12 - 1];
					}
				}
			}
			else
			{
				this.m_offset = intVector - this.Sprite.GetAnchorPixelOffset();
				for (int m = intVector.x; m <= intVector2.x; m++)
				{
					for (int n = intVector.y; n <= intVector2.y; n++)
					{
						this.m_basePixels[m - intVector.x, n - intVector.y] = bagelCollider[m, bagelCollider.height - n - 1];
					}
				}
			}
			if (!this.BagleUseFirstFrameOnly)
			{
				PixelCollider.PixelCache pixelCache3 = new PixelCollider.PixelCache();
				pixelCache3.dimensions = this.m_dimensions;
				pixelCache3.basePixels = this.m_basePixels;
				pixelCache3.offset = this.m_offset;
				pixelCache3.basePixels.ReadOnly = true;
				this.m_cachedBasePixels.Add(num, pixelCache3);
			}
		}
		this.m_position = PixelCollider.ToCollisionVector(transform.position) + this.m_offset;
		this.SetRotationAndScale(rotation, scale.Value);
	}

	// Token: 0x06002FA1 RID: 12193 RVA: 0x000FC068 File Offset: 0x000FA268
	public void RegenerateFromCircle(Transform transform, IntVector2 offset, int diameter)
	{
		this.RegenerateFromCircle(transform.position, offset, diameter);
	}

	// Token: 0x06002FA2 RID: 12194 RVA: 0x000FC080 File Offset: 0x000FA280
	public void RegenerateFromCircle(Vector2 position, IntVector2 offset, int diameter)
	{
		this.m_offset = offset;
		this.m_dimensions = new IntVector2(diameter, diameter);
		this.m_position = PixelCollider.ToCollisionVector(position) + this.m_offset;
		this.m_basePixels.Reinitialize(this.m_dimensions.x, this.m_dimensions.y, true);
		this.m_bestPixels = this.m_basePixels;
		float num = (float)diameter / 2f;
		for (int i = 0; i < this.m_dimensions.x; i++)
		{
			for (int j = 0; j < this.m_dimensions.y; j++)
			{
				this.m_basePixels[i, j] = Vector2.Distance(new Vector2((float)i, (float)j), new Vector2(num, num)) < num;
			}
		}
		this.SetRotationAndScale(0f, new Vector2(1f, 1f));
	}

	// Token: 0x06002FA3 RID: 12195 RVA: 0x000FC168 File Offset: 0x000FA368
	public void RegenerateFromLine(Transform transform, IntVector2 leftPoint, IntVector2 rightPoint)
	{
		this.RegenerateFromLine(transform.position, leftPoint, rightPoint);
	}

	// Token: 0x06002FA4 RID: 12196 RVA: 0x000FC180 File Offset: 0x000FA380
	public void RegenerateFromLine(Vector2 position, IntVector2 leftPoint, IntVector2 rightPoint)
	{
		this.m_offset = new IntVector2(Mathf.Min(leftPoint.x, rightPoint.x), Mathf.Min(leftPoint.y, rightPoint.y));
		this.m_dimensions = new IntVector2(Mathf.Abs(rightPoint.x - leftPoint.x) + 1, Mathf.Abs(rightPoint.y - leftPoint.y) + 1);
		this.m_position = PixelCollider.ToCollisionVector(position) + this.m_offset;
		this.m_basePixels.ReinitializeWithDefault(this.m_dimensions.x, this.m_dimensions.y, false, 0f, true);
		this.m_bestPixels = this.m_basePixels;
		this.PlotPixelLines(new Vector2[]
		{
			PhysicsEngine.PixelToUnit(leftPoint),
			PhysicsEngine.PixelToUnit(rightPoint)
		}, -PhysicsEngine.PixelToUnit(this.m_offset));
		this.SetRotationAndScale(0f, new Vector2(1f, 1f));
	}

	// Token: 0x06002FA5 RID: 12197 RVA: 0x000FC29C File Offset: 0x000FA49C
	public void RegenerateEmptyCollider(Transform transform)
	{
		this.m_offset = IntVector2.Zero;
		this.m_dimensions = IntVector2.Zero;
		this.m_position = PixelCollider.ToCollisionVector(transform.position) + this.m_offset;
		this.m_basePixels.Reinitialize(0, 0, true);
		this.m_bestPixels = this.m_basePixels;
		this.SetRotationAndScale(0f, new Vector2(1f, 1f));
	}

	// Token: 0x06002FA6 RID: 12198 RVA: 0x000FC314 File Offset: 0x000FA514
	public void RegenerateFromVertices(Vector2[] vertices, Transform transform, float rotation = 0f, Vector2? scale = null)
	{
		this.RegenerateFromVertices(vertices, PixelCollider.ToCollisionVector(transform.position), rotation, scale);
	}

	// Token: 0x06002FA7 RID: 12199 RVA: 0x000FC330 File Offset: 0x000FA530
	public void RegenerateFromVertices(Vector2[] vertices, IntVector2 position, float rotation = 0f, Vector2? scale = null)
	{
		if (scale == null)
		{
			scale = new Vector2?(new Vector2(1f, 1f));
		}
		this.m_position = position;
		this.m_bestPixels = this.m_basePixels;
		if (vertices.Length == 0)
		{
			this.m_dimensions = IntVector2.Zero;
			this.m_basePixels.Reinitialize(0, 0, true);
		}
		else
		{
			Vector2 vector = new Vector2(float.MaxValue, float.MaxValue);
			Vector2 vector2 = new Vector2(float.MinValue, float.MinValue);
			for (int i = 0; i < vertices.Length; i++)
			{
				Vector3 vector3 = vertices[i];
				vector = Vector2.Min(vector, vector3);
				vector2 = Vector2.Max(vector2, vector3);
			}
			this.m_offset = new IntVector2(Mathf.FloorToInt(vector.x * 16f), Mathf.FloorToInt(vector.y * 16f));
			this.m_position += this.m_offset;
			this.m_dimensions = new IntVector2(Mathf.CeilToInt(vector2.x * 16f), Mathf.CeilToInt(vector2.y * 16f)) - this.m_offset;
			this.m_basePixels.ReinitializeWithDefault(this.m_dimensions.x, this.m_dimensions.y, false, 0f, true);
			this.PlotPixelLines(vertices, -PhysicsEngine.PixelToUnit(this.m_offset));
			this.FillInternalPixels();
		}
		this.SetRotationAndScale(rotation, scale.Value);
	}

	// Token: 0x06002FA8 RID: 12200 RVA: 0x000FC4D8 File Offset: 0x000FA6D8
	private static int ToCollisionPixel(float value)
	{
		return Mathf.RoundToInt(value * 16f);
	}

	// Token: 0x06002FA9 RID: 12201 RVA: 0x000FC4E8 File Offset: 0x000FA6E8
	private static IntVector2 ToCollisionVector(Vector2 value)
	{
		return new IntVector2(PixelCollider.ToCollisionPixel(value.x), PixelCollider.ToCollisionPixel(value.y));
	}

	// Token: 0x06002FAA RID: 12202 RVA: 0x000FC508 File Offset: 0x000FA708
	private static float FromCollisionPixel(int value)
	{
		return (float)value / 16f;
	}

	// Token: 0x06002FAB RID: 12203 RVA: 0x000FC514 File Offset: 0x000FA714
	private static Vector2 FromCollisionVector(IntVector2 value)
	{
		return new Vector2(PixelCollider.FromCollisionPixel(value.x), PixelCollider.FromCollisionPixel(value.y));
	}

	// Token: 0x06002FAC RID: 12204 RVA: 0x000FC534 File Offset: 0x000FA734
	private void PlotPixelLines(Vector2[] vertices)
	{
		this.PlotPixelLines(vertices, Vector2.zero);
	}

	// Token: 0x06002FAD RID: 12205 RVA: 0x000FC544 File Offset: 0x000FA744
	private void PlotPixelLines(Vector2[] vertices, Vector2 offset)
	{
		for (int i = 0; i < vertices.Length; i++)
		{
			IntVector2 intVector = PixelCollider.ToCollisionVector(vertices[i] + offset);
			IntVector2 intVector2 = PixelCollider.ToCollisionVector(vertices[(i + 1) % vertices.Length] + offset);
			if (intVector.x == this.m_dimensions.x)
			{
				intVector.x--;
			}
			if (intVector.y == this.m_dimensions.y)
			{
				intVector.y--;
			}
			if (intVector2.x == this.m_dimensions.x)
			{
				intVector2.x--;
			}
			if (intVector2.y == this.m_dimensions.y)
			{
				intVector2.y--;
			}
			this.PlotPixelLine(intVector.x, intVector.y, intVector2.x, intVector2.y);
		}
	}

	// Token: 0x06002FAE RID: 12206 RVA: 0x000FC658 File Offset: 0x000FA858
	private void PlotPixelLine(int x0, int y0, int x1, int y1)
	{
		bool flag = Mathf.Abs(y1 - y0) > Mathf.Abs(x1 - x0);
		if (flag)
		{
			this.Swap(ref x0, ref y0);
			this.Swap(ref x1, ref y1);
		}
		if (x0 > x1)
		{
			this.Swap(ref x0, ref x1);
			this.Swap(ref y0, ref y1);
		}
		int num = x1 - x0;
		int num2 = Mathf.Abs(y1 - y0);
		int num3 = num / 2;
		int num4 = y0;
		int num5 = ((y0 >= y1) ? (-1) : 1);
		for (int i = x0; i <= x1; i++)
		{
			if (flag)
			{
				this.m_basePixels[num4, i] = true;
			}
			else
			{
				this.m_basePixels[i, num4] = true;
			}
			num3 -= num2;
			if (num3 < 0)
			{
				num4 += num5;
				num3 += num;
			}
		}
	}

	// Token: 0x06002FAF RID: 12207 RVA: 0x000FC72C File Offset: 0x000FA92C
	private void Swap(ref int a, ref int b)
	{
		int num = a;
		a = b;
		b = num;
	}

	// Token: 0x06002FB0 RID: 12208 RVA: 0x000FC744 File Offset: 0x000FA944
	private void FillInternalPixels()
	{
		for (int i = 0; i < this.Width; i++)
		{
			int num = -1;
			int num2 = -1;
			for (int j = 0; j < this.Height; j++)
			{
				if (this.m_basePixels[i, j])
				{
					num = j;
					break;
				}
			}
			if (num != -1)
			{
				for (int k = this.Height - 1; k >= 0; k--)
				{
					if (this.m_basePixels[i, k])
					{
						num2 = k;
						break;
					}
				}
				for (int l = num + 1; l < num2; l++)
				{
					this.m_basePixels[i, l] = true;
				}
			}
		}
	}

	// Token: 0x06002FB1 RID: 12209 RVA: 0x000FC808 File Offset: 0x000FAA08
	private void UpdateSlope()
	{
		if (this.m_slopeStart != null && this.m_slopeEnd != null)
		{
			this.IsSlope = true;
			this.Slope = (this.m_slopeEnd.Value.y - this.m_slopeStart.Value.y) / (this.m_slopeEnd.Value.x - this.m_slopeStart.Value.x);
		}
	}

	// Token: 0x06002FB2 RID: 12210 RVA: 0x000FC894 File Offset: 0x000FAA94
	public static PixelCollider CreateRectangle(CollisionLayer layer, int x, int y, int width, int height, bool enabled = true)
	{
		return new PixelCollider
		{
			CollisionLayer = layer,
			ColliderGenerationMode = PixelCollider.PixelColliderGeneration.Manual,
			ManualOffsetX = x,
			ManualOffsetY = y,
			ManualWidth = width,
			ManualHeight = height,
			Enabled = enabled
		};
	}

	// Token: 0x04002057 RID: 8279
	public bool Enabled = true;

	// Token: 0x04002058 RID: 8280
	public CollisionLayer CollisionLayer = CollisionLayer.LowObstacle;

	// Token: 0x04002059 RID: 8281
	public bool IsTrigger;

	// Token: 0x0400205A RID: 8282
	public PixelCollider.PixelColliderGeneration ColliderGenerationMode = PixelCollider.PixelColliderGeneration.Tk2dPolygon;

	// Token: 0x0400205B RID: 8283
	public bool BagleUseFirstFrameOnly = true;

	// Token: 0x0400205C RID: 8284
	[CheckSprite("Sprite")]
	public string SpecifyBagelFrame;

	// Token: 0x0400205D RID: 8285
	public int BagelColliderNumber;

	// Token: 0x0400205E RID: 8286
	public int ManualOffsetX;

	// Token: 0x0400205F RID: 8287
	public int ManualOffsetY;

	// Token: 0x04002060 RID: 8288
	public int ManualWidth;

	// Token: 0x04002061 RID: 8289
	public int ManualHeight;

	// Token: 0x04002062 RID: 8290
	[Obsolete("ManualRadius is deprecated, use ManualDiameter instead.")]
	public int ManualRadius;

	// Token: 0x04002063 RID: 8291
	public int ManualDiameter;

	// Token: 0x04002064 RID: 8292
	public int ManualLeftX;

	// Token: 0x04002065 RID: 8293
	public int ManualLeftY;

	// Token: 0x04002066 RID: 8294
	public int ManualRightX;

	// Token: 0x04002067 RID: 8295
	public int ManualRightY;

	// Token: 0x04002068 RID: 8296
	public tk2dBaseSprite Sprite;

	// Token: 0x04002069 RID: 8297
	public Func<IntVector2, bool> DirectionIgnorer;

	// Token: 0x0400206A RID: 8298
	public Func<Vector2, Vector2> NormalModifier;

	// Token: 0x04002071 RID: 8305
	public IntVector2 m_offset;

	// Token: 0x04002072 RID: 8306
	public IntVector2 m_transformOffset;

	// Token: 0x04002073 RID: 8307
	private IntVector2 m_position;

	// Token: 0x04002074 RID: 8308
	public IntVector2 m_dimensions;

	// Token: 0x04002075 RID: 8309
	public float m_rotation;

	// Token: 0x04002076 RID: 8310
	private Vector2 m_scale = new Vector2(1f, 1f);

	// Token: 0x04002077 RID: 8311
	private BitArray2D m_basePixels = new BitArray2D(false);

	// Token: 0x04002078 RID: 8312
	private BitArray2D m_modifiedPixels;

	// Token: 0x04002079 RID: 8313
	private BitArray2D m_bestPixels;

	// Token: 0x0400207A RID: 8314
	private Vector2? m_slopeStart;

	// Token: 0x0400207B RID: 8315
	private Vector2? m_slopeEnd;

	// Token: 0x0400207C RID: 8316
	[NonSerialized]
	public tk2dSpriteDefinition m_lastSpriteDef;

	// Token: 0x0400207D RID: 8317
	[NonSerialized]
	private List<PixelCollider> m_frameSpecificCollisionExceptions = new List<PixelCollider>();

	// Token: 0x0400207E RID: 8318
	[NonSerialized]
	private List<TriggerCollisionData> m_triggerCollisions = new List<TriggerCollisionData>();

	// Token: 0x0400207F RID: 8319
	private Dictionary<int, PixelCollider.PixelCache> m_cachedBasePixels;

	// Token: 0x04002080 RID: 8320
	public static List<PixelCollider.StepData> m_stepList = new List<PixelCollider.StepData>();

	// Token: 0x02000862 RID: 2146
	public enum PixelColliderGeneration
	{
		// Token: 0x04002082 RID: 8322
		Manual,
		// Token: 0x04002083 RID: 8323
		Tk2dPolygon,
		// Token: 0x04002084 RID: 8324
		BagelCollider,
		// Token: 0x04002085 RID: 8325
		Circle,
		// Token: 0x04002086 RID: 8326
		Line
	}

	// Token: 0x02000863 RID: 2147
	private class PixelCache
	{
		// Token: 0x04002087 RID: 8327
		public IntVector2 dimensions;

		// Token: 0x04002088 RID: 8328
		public BitArray2D basePixels;

		// Token: 0x04002089 RID: 8329
		public IntVector2 offset;
	}

	// Token: 0x02000864 RID: 2148
	public struct StepData
	{
		// Token: 0x06002FB5 RID: 12213 RVA: 0x000FC8F0 File Offset: 0x000FAAF0
		public StepData(IntVector2 deltaPos, float deltaTime)
		{
			this.deltaPos = deltaPos;
			this.deltaTime = deltaTime;
		}

		// Token: 0x0400208A RID: 8330
		public IntVector2 deltaPos;

		// Token: 0x0400208B RID: 8331
		public float deltaTime;
	}
}
