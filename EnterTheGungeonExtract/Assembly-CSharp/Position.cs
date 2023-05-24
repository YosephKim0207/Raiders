using System;
using UnityEngine;

// Token: 0x0200086A RID: 2154
[Serializable]
public struct Position
{
	// Token: 0x06002FC3 RID: 12227 RVA: 0x000FCB1C File Offset: 0x000FAD1C
	public Position(int pixelX, int pixelY)
	{
		this.m_position.x = pixelX;
		this.m_position.y = pixelY;
		this.m_remainder = Vector2.zero;
	}

	// Token: 0x06002FC4 RID: 12228 RVA: 0x000FCB44 File Offset: 0x000FAD44
	public Position(float unitX, float unitY)
	{
		this.m_position.x = Mathf.RoundToInt(unitX * 16f);
		this.m_position.y = Mathf.RoundToInt(unitY * 16f);
		this.m_remainder.x = unitX - (float)this.m_position.x * 0.0625f;
		this.m_remainder.y = unitY - (float)this.m_position.y * 0.0625f;
	}

	// Token: 0x06002FC5 RID: 12229 RVA: 0x000FCBC0 File Offset: 0x000FADC0
	public Position(IntVector2 pixelPosition, Vector2 remainder)
	{
		this.m_position = pixelPosition;
		this.m_remainder = remainder;
	}

	// Token: 0x06002FC6 RID: 12230 RVA: 0x000FCBD0 File Offset: 0x000FADD0
	public Position(Position position)
	{
		this = new Position(position.m_position, position.m_remainder);
	}

	// Token: 0x06002FC7 RID: 12231 RVA: 0x000FCBE8 File Offset: 0x000FADE8
	public Position(Vector2 unitPosition)
	{
		this = new Position(unitPosition.x, unitPosition.y);
	}

	// Token: 0x06002FC8 RID: 12232 RVA: 0x000FCC00 File Offset: 0x000FAE00
	public Position(Vector3 unitPosition)
	{
		this = new Position(unitPosition.x, unitPosition.y);
	}

	// Token: 0x06002FC9 RID: 12233 RVA: 0x000FCC18 File Offset: 0x000FAE18
	public Position(IntVector2 pixelPosition)
	{
		this = new Position(pixelPosition.x, pixelPosition.y);
	}

	// Token: 0x170008EE RID: 2286
	// (get) Token: 0x06002FCA RID: 12234 RVA: 0x000FCC30 File Offset: 0x000FAE30
	// (set) Token: 0x06002FCB RID: 12235 RVA: 0x000FCC40 File Offset: 0x000FAE40
	public int X
	{
		get
		{
			return this.m_position.x;
		}
		set
		{
			this.m_position.x = value;
			this.m_remainder.x = 0f;
		}
	}

	// Token: 0x170008EF RID: 2287
	// (get) Token: 0x06002FCC RID: 12236 RVA: 0x000FCC60 File Offset: 0x000FAE60
	// (set) Token: 0x06002FCD RID: 12237 RVA: 0x000FCC70 File Offset: 0x000FAE70
	public int Y
	{
		get
		{
			return this.m_position.y;
		}
		set
		{
			this.m_position.y = value;
			this.m_remainder.y = 0f;
		}
	}

	// Token: 0x170008F0 RID: 2288
	// (get) Token: 0x06002FCE RID: 12238 RVA: 0x000FCC90 File Offset: 0x000FAE90
	// (set) Token: 0x06002FCF RID: 12239 RVA: 0x000FCCB0 File Offset: 0x000FAEB0
	public float UnitX
	{
		get
		{
			return (float)this.m_position.x * 0.0625f + this.m_remainder.x;
		}
		set
		{
			this.m_position.x = Mathf.RoundToInt(value * 16f);
			this.m_remainder.x = value - (float)this.m_position.x * 0.0625f;
		}
	}

	// Token: 0x170008F1 RID: 2289
	// (get) Token: 0x06002FD0 RID: 12240 RVA: 0x000FCCE8 File Offset: 0x000FAEE8
	// (set) Token: 0x06002FD1 RID: 12241 RVA: 0x000FCD08 File Offset: 0x000FAF08
	public float UnitY
	{
		get
		{
			return (float)this.m_position.y * 0.0625f + this.m_remainder.y;
		}
		set
		{
			this.m_position.y = Mathf.RoundToInt(value * 16f);
			this.m_remainder.y = value - (float)this.m_position.y * 0.0625f;
		}
	}

	// Token: 0x170008F2 RID: 2290
	// (get) Token: 0x06002FD2 RID: 12242 RVA: 0x000FCD40 File Offset: 0x000FAF40
	// (set) Token: 0x06002FD3 RID: 12243 RVA: 0x000FCD48 File Offset: 0x000FAF48
	public IntVector2 PixelPosition
	{
		get
		{
			return this.m_position;
		}
		set
		{
			this.X = value.x;
			this.Y = value.y;
		}
	}

	// Token: 0x170008F3 RID: 2291
	// (get) Token: 0x06002FD4 RID: 12244 RVA: 0x000FCD64 File Offset: 0x000FAF64
	// (set) Token: 0x06002FD5 RID: 12245 RVA: 0x000FCDB4 File Offset: 0x000FAFB4
	public Vector2 UnitPosition
	{
		get
		{
			return new Vector2((float)this.m_position.x * 0.0625f + this.m_remainder.x, (float)this.m_position.y * 0.0625f + this.m_remainder.y);
		}
		set
		{
			this.UnitX = value.x;
			this.UnitY = value.y;
		}
	}

	// Token: 0x170008F4 RID: 2292
	// (get) Token: 0x06002FD6 RID: 12246 RVA: 0x000FCDD0 File Offset: 0x000FAFD0
	// (set) Token: 0x06002FD7 RID: 12247 RVA: 0x000FCDD8 File Offset: 0x000FAFD8
	public Vector2 Remainder
	{
		get
		{
			return this.m_remainder;
		}
		set
		{
			this.m_remainder = value;
		}
	}

	// Token: 0x06002FD8 RID: 12248 RVA: 0x000FCDE4 File Offset: 0x000FAFE4
	public static Position operator +(Position lhs, Vector2 rhs)
	{
		return new Position(lhs.UnitPosition + rhs);
	}

	// Token: 0x06002FD9 RID: 12249 RVA: 0x000FCDF8 File Offset: 0x000FAFF8
	public static Position operator +(Position lhs, IntVector2 rhs)
	{
		return new Position(lhs.PixelPosition + rhs, lhs.Remainder);
	}

	// Token: 0x06002FDA RID: 12250 RVA: 0x000FCE14 File Offset: 0x000FB014
	public Vector2 GetPixelVector2()
	{
		return (Vector2)this.m_position * 0.0625f;
	}

	// Token: 0x06002FDB RID: 12251 RVA: 0x000FCE2C File Offset: 0x000FB02C
	public IntVector2 GetPixelDelta(Vector2 unitDelta)
	{
		IntVector2 zero = IntVector2.Zero;
		zero.x = Mathf.RoundToInt((this.UnitX + unitDelta.x) * 16f) - this.m_position.x;
		zero.y = Mathf.RoundToInt((this.UnitY + unitDelta.y) * 16f) - this.m_position.y;
		return zero;
	}

	// Token: 0x040020A3 RID: 8355
	public IntVector2 m_position;

	// Token: 0x040020A4 RID: 8356
	public Vector2 m_remainder;
}
