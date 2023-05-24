using System;
using UnityEngine;

// Token: 0x0200036D RID: 877
[Serializable]
public struct IntVector2
{
	// Token: 0x06000E45 RID: 3653 RVA: 0x00043B08 File Offset: 0x00041D08
	public IntVector2(int x, int y)
	{
		this.x = x;
		this.y = y;
	}

	// Token: 0x17000331 RID: 817
	// (get) Token: 0x06000E46 RID: 3654 RVA: 0x00043B18 File Offset: 0x00041D18
	public static IntVector2[] Cardinals
	{
		get
		{
			if (IntVector2.m_cachedCardinals == null)
			{
				IntVector2.m_cachedCardinals = new IntVector2[]
				{
					IntVector2.North,
					IntVector2.East,
					IntVector2.South,
					IntVector2.West
				};
			}
			return IntVector2.m_cachedCardinals;
		}
	}

	// Token: 0x17000332 RID: 818
	// (get) Token: 0x06000E47 RID: 3655 RVA: 0x00043B84 File Offset: 0x00041D84
	public static IntVector2[] Ordinals
	{
		get
		{
			if (IntVector2.m_cachedOrdinals == null)
			{
				IntVector2.m_cachedOrdinals = new IntVector2[]
				{
					IntVector2.NorthEast,
					IntVector2.SouthEast,
					IntVector2.SouthWest,
					IntVector2.NorthWest
				};
			}
			return IntVector2.m_cachedOrdinals;
		}
	}

	// Token: 0x17000333 RID: 819
	// (get) Token: 0x06000E48 RID: 3656 RVA: 0x00043BF0 File Offset: 0x00041DF0
	public static IntVector2[] CardinalsAndOrdinals
	{
		get
		{
			if (IntVector2.m_cachedCardinalsAndOrdinals == null)
			{
				IntVector2.m_cachedCardinalsAndOrdinals = new IntVector2[]
				{
					IntVector2.North,
					IntVector2.NorthEast,
					IntVector2.East,
					IntVector2.SouthEast,
					IntVector2.South,
					IntVector2.SouthWest,
					IntVector2.West,
					IntVector2.NorthWest
				};
			}
			return IntVector2.m_cachedCardinalsAndOrdinals;
		}
	}

	// Token: 0x17000334 RID: 820
	// (get) Token: 0x06000E49 RID: 3657 RVA: 0x00043CA0 File Offset: 0x00041EA0
	public int X
	{
		get
		{
			return this.x;
		}
	}

	// Token: 0x17000335 RID: 821
	// (get) Token: 0x06000E4A RID: 3658 RVA: 0x00043CA8 File Offset: 0x00041EA8
	public int Y
	{
		get
		{
			return this.y;
		}
	}

	// Token: 0x17000336 RID: 822
	// (get) Token: 0x06000E4B RID: 3659 RVA: 0x00043CB0 File Offset: 0x00041EB0
	public IntVector2 MajorAxis
	{
		get
		{
			if (this.x == 0 && this.y == 0)
			{
				return IntVector2.Zero;
			}
			return (Mathf.Abs(this.x) <= Mathf.Abs(this.y)) ? new IntVector2(0, Math.Sign(this.y)) : new IntVector2(Math.Sign(this.x), 0);
		}
	}

	// Token: 0x06000E4C RID: 3660 RVA: 0x00043D1C File Offset: 0x00041F1C
	public Vector2 ToVector2()
	{
		return new Vector2((float)this.x, (float)this.y);
	}

	// Token: 0x06000E4D RID: 3661 RVA: 0x00043D34 File Offset: 0x00041F34
	public Vector2 ToVector2(float xOffset, float yOffset)
	{
		return new Vector2((float)this.x + xOffset, (float)this.y + yOffset);
	}

	// Token: 0x06000E4E RID: 3662 RVA: 0x00043D50 File Offset: 0x00041F50
	public Vector2 ToCenterVector2()
	{
		return new Vector3((float)this.x + 0.5f, (float)this.y + 0.5f);
	}

	// Token: 0x06000E4F RID: 3663 RVA: 0x00043D78 File Offset: 0x00041F78
	public Vector3 ToVector3()
	{
		return new Vector3((float)this.x, (float)this.y, 0f);
	}

	// Token: 0x06000E50 RID: 3664 RVA: 0x00043D94 File Offset: 0x00041F94
	public Vector3 ToVector3(float height)
	{
		return new Vector3((float)this.x, (float)this.y, height);
	}

	// Token: 0x06000E51 RID: 3665 RVA: 0x00043DAC File Offset: 0x00041FAC
	public Vector3 ToCenterVector3(float height)
	{
		return new Vector3((float)this.x + 0.5f, (float)this.y + 0.5f, height);
	}

	// Token: 0x17000337 RID: 823
	// (get) Token: 0x06000E52 RID: 3666 RVA: 0x00043DD0 File Offset: 0x00041FD0
	public int sqrMagnitude
	{
		get
		{
			return this.x * this.x + this.y * this.y;
		}
	}

	// Token: 0x06000E53 RID: 3667 RVA: 0x00043DF0 File Offset: 0x00041FF0
	public bool IsWithin(IntVector2 min, IntVector2 max)
	{
		return this.x >= min.x && this.x <= max.x && this.y >= min.y && this.y <= max.y;
	}

	// Token: 0x17000338 RID: 824
	// (get) Token: 0x06000E54 RID: 3668 RVA: 0x00043E48 File Offset: 0x00042048
	public int ComponentSum
	{
		get
		{
			return Math.Abs(this.x) + Math.Abs(this.y);
		}
	}

	// Token: 0x06000E55 RID: 3669 RVA: 0x00043E64 File Offset: 0x00042064
	public override string ToString()
	{
		return string.Format("{0},{1}", this.x, this.y);
	}

	// Token: 0x06000E56 RID: 3670 RVA: 0x00043E88 File Offset: 0x00042088
	public bool Equals(IntVector2 other)
	{
		return this.x == other.x && this.y == other.y;
	}

	// Token: 0x06000E57 RID: 3671 RVA: 0x00043EB0 File Offset: 0x000420B0
	public override bool Equals(object obj)
	{
		if (obj is IntVector2)
		{
			return this == (IntVector2)obj;
		}
		return base.Equals(obj);
	}

	// Token: 0x06000E58 RID: 3672 RVA: 0x00043EE0 File Offset: 0x000420E0
	public override int GetHashCode()
	{
		return 100267 * this.x + 200233 * this.y;
	}

	// Token: 0x06000E59 RID: 3673 RVA: 0x00043EFC File Offset: 0x000420FC
	public float GetHashedRandomValue()
	{
		int num = 0;
		num += this.x;
		num += num << 10;
		num ^= num >> 6;
		num += this.y;
		num += num << 10;
		num ^= num >> 6;
		num += num << 3;
		num ^= num >> 11;
		num += num << 15;
		uint num2 = (uint)num;
		return num2 * 1f / 4.2949673E+09f;
	}

	// Token: 0x06000E5A RID: 3674 RVA: 0x00043F5C File Offset: 0x0004215C
	public IntVector2 WithX(int newX)
	{
		return new IntVector2(newX, this.y);
	}

	// Token: 0x06000E5B RID: 3675 RVA: 0x00043F6C File Offset: 0x0004216C
	public IntVector2 WithY(int newY)
	{
		return new IntVector2(this.x, newY);
	}

	// Token: 0x06000E5C RID: 3676 RVA: 0x00043F7C File Offset: 0x0004217C
	public static IntVector2 operator +(IntVector2 a, IntVector2 b)
	{
		return new IntVector2(a.x + b.x, a.y + b.y);
	}

	// Token: 0x06000E5D RID: 3677 RVA: 0x00043FA4 File Offset: 0x000421A4
	public static IntVector2 operator -(IntVector2 a, IntVector2 b)
	{
		return new IntVector2(a.x - b.x, a.y - b.y);
	}

	// Token: 0x06000E5E RID: 3678 RVA: 0x00043FCC File Offset: 0x000421CC
	public static IntVector2 operator *(IntVector2 a, int b)
	{
		return new IntVector2(a.x * b, a.y * b);
	}

	// Token: 0x06000E5F RID: 3679 RVA: 0x00043FE8 File Offset: 0x000421E8
	public static IntVector2 operator *(int a, IntVector2 b)
	{
		return new IntVector2(a * b.x, a * b.y);
	}

	// Token: 0x06000E60 RID: 3680 RVA: 0x00044004 File Offset: 0x00042204
	public static IntVector2 operator /(IntVector2 a, int b)
	{
		return new IntVector2(a.x / b, a.y / b);
	}

	// Token: 0x06000E61 RID: 3681 RVA: 0x00044020 File Offset: 0x00042220
	public static IntVector2 operator -(IntVector2 a)
	{
		return new IntVector2(-a.x, -a.y);
	}

	// Token: 0x06000E62 RID: 3682 RVA: 0x00044038 File Offset: 0x00042238
	public static bool operator ==(IntVector2 a, IntVector2 b)
	{
		return a.x == b.x && a.y == b.y;
	}

	// Token: 0x06000E63 RID: 3683 RVA: 0x00044060 File Offset: 0x00042260
	public static bool operator !=(IntVector2 a, IntVector2 b)
	{
		return a.x != b.x || a.y != b.y;
	}

	// Token: 0x06000E64 RID: 3684 RVA: 0x0004408C File Offset: 0x0004228C
	public static int ManhattanDistance(IntVector2 a, IntVector2 b)
	{
		return Math.Abs(a.x - b.x) + Math.Abs(a.y - b.y);
	}

	// Token: 0x06000E65 RID: 3685 RVA: 0x000440B8 File Offset: 0x000422B8
	public static int ManhattanDistance(IntVector2 a, int bx, int by)
	{
		return Math.Abs(a.x - bx) + Math.Abs(a.y - by);
	}

	// Token: 0x06000E66 RID: 3686 RVA: 0x000440D8 File Offset: 0x000422D8
	public static int ManhattanDistance(int ax, int ay, int bx, int by)
	{
		return Math.Abs(ax - bx) + Math.Abs(ay - by);
	}

	// Token: 0x06000E67 RID: 3687 RVA: 0x000440EC File Offset: 0x000422EC
	public static float Distance(IntVector2 a, IntVector2 b)
	{
		return Mathf.Sqrt((float)((b.y - a.y) * (b.y - a.y) + (b.x - a.x) * (b.x - a.x)));
	}

	// Token: 0x06000E68 RID: 3688 RVA: 0x00044140 File Offset: 0x00042340
	public static float Distance(IntVector2 a, int bx, int by)
	{
		return Mathf.Sqrt((float)((by - a.y) * (by - a.y) + (bx - a.x) * (bx - a.x)));
	}

	// Token: 0x06000E69 RID: 3689 RVA: 0x00044170 File Offset: 0x00042370
	public static float Distance(int ax, int ay, int bx, int by)
	{
		return Mathf.Sqrt((float)((by - ay) * (by - ay) + (bx - ax) * (bx - ax)));
	}

	// Token: 0x06000E6A RID: 3690 RVA: 0x00044188 File Offset: 0x00042388
	public static float DistanceSquared(IntVector2 a, IntVector2 b)
	{
		return (float)((b.y - a.y) * (b.y - a.y) + (b.x - a.x) * (b.x - a.x));
	}

	// Token: 0x06000E6B RID: 3691 RVA: 0x000441D8 File Offset: 0x000423D8
	public static float DistanceSquared(IntVector2 a, int bx, int by)
	{
		return (float)((by - a.y) * (by - a.y) + (bx - a.x) * (bx - a.x));
	}

	// Token: 0x06000E6C RID: 3692 RVA: 0x00044204 File Offset: 0x00042404
	public static float DistanceSquared(int ax, int ay, int bx, int by)
	{
		return (float)((by - ay) * (by - ay) + (bx - ax) * (bx - ax));
	}

	// Token: 0x06000E6D RID: 3693 RVA: 0x00044218 File Offset: 0x00042418
	public void Clamp(IntVector2 min, IntVector2 max)
	{
		this.x = Math.Max(this.x, min.x);
		this.y = Math.Max(this.y, min.y);
		this.x = Math.Min(this.x, max.x);
		this.y = Math.Min(this.y, max.y);
	}

	// Token: 0x06000E6E RID: 3694 RVA: 0x00044288 File Offset: 0x00042488
	public static IntVector2 Scale(IntVector2 lhs, IntVector2 rhs)
	{
		return new IntVector2(lhs.x * rhs.x, lhs.y * rhs.y);
	}

	// Token: 0x06000E6F RID: 3695 RVA: 0x000442B0 File Offset: 0x000424B0
	public static IntVector2 Min(IntVector2 lhs, IntVector2 rhs)
	{
		return new IntVector2(Mathf.Min(lhs.x, rhs.x), Mathf.Min(lhs.y, rhs.y));
	}

	// Token: 0x06000E70 RID: 3696 RVA: 0x000442E0 File Offset: 0x000424E0
	public static IntVector2 Max(IntVector2 lhs, IntVector2 rhs)
	{
		return new IntVector2(Mathf.Max(lhs.x, rhs.x), Mathf.Max(lhs.y, rhs.y));
	}

	// Token: 0x06000E71 RID: 3697 RVA: 0x00044310 File Offset: 0x00042510
	public static bool AABBOverlap(IntVector2 posA, IntVector2 dimensionsA, IntVector2 posB, IntVector2 dimensionsB)
	{
		return posA.x + dimensionsA.x - 1 >= posB.x && posA.x <= posB.x + dimensionsB.x - 1 && posA.y + dimensionsA.y - 1 >= posB.y && posA.y <= posB.y + dimensionsB.y - 1;
	}

	// Token: 0x06000E72 RID: 3698 RVA: 0x00044394 File Offset: 0x00042594
	public static bool AABBOverlapWithArea(IntVector2 posA, IntVector2 dimensionsA, IntVector2 posB, IntVector2 dimensionsB, out int cellsOverlapping)
	{
		if (posA.x + dimensionsA.x - 1 < posB.x || posA.x > posB.x + dimensionsB.x - 1 || posA.y + dimensionsA.y - 1 < posB.y || posA.y > posB.y + dimensionsB.y - 1)
		{
			cellsOverlapping = 0;
			return false;
		}
		int num = Mathf.Max(0, Mathf.Min(posA.x + dimensionsA.x, posB.x + dimensionsB.x) - Mathf.Max(posA.x, posB.x));
		int num2 = Mathf.Max(0, Mathf.Min(posA.y + dimensionsA.y, posB.y + dimensionsB.y) - Mathf.Max(posA.y, posB.y));
		cellsOverlapping = num * num2;
		return true;
	}

	// Token: 0x06000E73 RID: 3699 RVA: 0x000444A0 File Offset: 0x000426A0
	public static explicit operator Vector2(IntVector2 v)
	{
		return new Vector2((float)v.x, (float)v.y);
	}

	// Token: 0x04000E31 RID: 3633
	public static IntVector2 Zero = new IntVector2(0, 0);

	// Token: 0x04000E32 RID: 3634
	public static IntVector2 One = new IntVector2(1, 1);

	// Token: 0x04000E33 RID: 3635
	public static IntVector2 NegOne = new IntVector2(-1, -1);

	// Token: 0x04000E34 RID: 3636
	public static IntVector2 Up = new IntVector2(0, 1);

	// Token: 0x04000E35 RID: 3637
	public static IntVector2 Right = new IntVector2(1, 0);

	// Token: 0x04000E36 RID: 3638
	public static IntVector2 Down = new IntVector2(0, -1);

	// Token: 0x04000E37 RID: 3639
	public static IntVector2 Left = new IntVector2(-1, 0);

	// Token: 0x04000E38 RID: 3640
	public static IntVector2 UpRight = new IntVector2(1, 1);

	// Token: 0x04000E39 RID: 3641
	public static IntVector2 DownRight = new IntVector2(1, -1);

	// Token: 0x04000E3A RID: 3642
	public static IntVector2 DownLeft = new IntVector2(-1, -1);

	// Token: 0x04000E3B RID: 3643
	public static IntVector2 UpLeft = new IntVector2(-1, 1);

	// Token: 0x04000E3C RID: 3644
	public static IntVector2 North = new IntVector2(0, 1);

	// Token: 0x04000E3D RID: 3645
	public static IntVector2 East = new IntVector2(1, 0);

	// Token: 0x04000E3E RID: 3646
	public static IntVector2 South = new IntVector2(0, -1);

	// Token: 0x04000E3F RID: 3647
	public static IntVector2 West = new IntVector2(-1, 0);

	// Token: 0x04000E40 RID: 3648
	public static IntVector2 NorthEast = new IntVector2(1, 1);

	// Token: 0x04000E41 RID: 3649
	public static IntVector2 SouthEast = new IntVector2(1, -1);

	// Token: 0x04000E42 RID: 3650
	public static IntVector2 SouthWest = new IntVector2(-1, -1);

	// Token: 0x04000E43 RID: 3651
	public static IntVector2 NorthWest = new IntVector2(-1, 1);

	// Token: 0x04000E44 RID: 3652
	public static IntVector2 MinValue = new IntVector2(int.MinValue, int.MinValue);

	// Token: 0x04000E45 RID: 3653
	public static IntVector2 MaxValue = new IntVector2(int.MaxValue, int.MaxValue);

	// Token: 0x04000E46 RID: 3654
	public static IntVector2[] m_cachedCardinals;

	// Token: 0x04000E47 RID: 3655
	public static IntVector2[] m_cachedOrdinals;

	// Token: 0x04000E48 RID: 3656
	public static IntVector2[] m_cachedCardinalsAndOrdinals;

	// Token: 0x04000E49 RID: 3657
	public int x;

	// Token: 0x04000E4A RID: 3658
	public int y;
}
