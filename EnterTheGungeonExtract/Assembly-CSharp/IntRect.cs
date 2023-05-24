using System;
using UnityEngine;

// Token: 0x0200132E RID: 4910
public class IntRect
{
	// Token: 0x06006F44 RID: 28484 RVA: 0x002C2280 File Offset: 0x002C0480
	public IntRect(int left, int bottom, int width, int height)
	{
		this.x = left;
		this.y = bottom;
		this.dimX = width;
		this.dimY = height;
	}

	// Token: 0x06006F45 RID: 28485 RVA: 0x002C22A8 File Offset: 0x002C04A8
	public static IntRect FromTwoPoints(IntVector2 p1, IntVector2 p2)
	{
		IntVector2 intVector = IntVector2.Min(p1, p2);
		IntVector2 intVector2 = IntVector2.Max(p1, p2);
		IntVector2 intVector3 = intVector2 - intVector;
		return new IntRect(intVector.x, intVector.y, intVector3.x, intVector3.y);
	}

	// Token: 0x06006F46 RID: 28486 RVA: 0x002C22F0 File Offset: 0x002C04F0
	public static IntRect Intersection(IntRect a, IntRect b, int expand = 0)
	{
		int num = Math.Max(a.x - expand, b.x - expand);
		int num2 = Math.Max(a.y - expand, b.y - expand);
		int num3 = Math.Min(a.Right + expand, b.Right + expand) - num;
		int num4 = Math.Min(a.Top + expand, b.Top + expand) - num2;
		if (num4 <= 0 || num3 <= 0)
		{
			return null;
		}
		return new IntRect(num, num2, num3, num4);
	}

	// Token: 0x170010DE RID: 4318
	// (get) Token: 0x06006F47 RID: 28487 RVA: 0x002C2374 File Offset: 0x002C0574
	public IntVector2 Dimensions
	{
		get
		{
			return new IntVector2(this.Width, this.Height);
		}
	}

	// Token: 0x170010DF RID: 4319
	// (get) Token: 0x06006F48 RID: 28488 RVA: 0x002C2388 File Offset: 0x002C0588
	public float Aspect
	{
		get
		{
			return (float)this.Width / (float)this.Height;
		}
	}

	// Token: 0x170010E0 RID: 4320
	// (get) Token: 0x06006F49 RID: 28489 RVA: 0x002C239C File Offset: 0x002C059C
	public int Area
	{
		get
		{
			return this.Width * this.Height;
		}
	}

	// Token: 0x170010E1 RID: 4321
	// (get) Token: 0x06006F4A RID: 28490 RVA: 0x002C23AC File Offset: 0x002C05AC
	public IntVector2[] FourPoints
	{
		get
		{
			return new IntVector2[]
			{
				new IntVector2(this.x, this.y),
				new IntVector2(this.x, this.y + this.dimY),
				new IntVector2(this.x + this.dimX, this.y + this.dimY),
				new IntVector2(this.x + this.dimX, this.y)
			};
		}
	}

	// Token: 0x170010E2 RID: 4322
	// (get) Token: 0x06006F4B RID: 28491 RVA: 0x002C2454 File Offset: 0x002C0654
	public IntVector2 Min
	{
		get
		{
			return new IntVector2(this.x, this.y);
		}
	}

	// Token: 0x170010E3 RID: 4323
	// (get) Token: 0x06006F4C RID: 28492 RVA: 0x002C2468 File Offset: 0x002C0668
	public IntVector2 Max
	{
		get
		{
			return new IntVector2(this.x + this.dimX, this.y + this.dimY);
		}
	}

	// Token: 0x170010E4 RID: 4324
	// (get) Token: 0x06006F4D RID: 28493 RVA: 0x002C248C File Offset: 0x002C068C
	// (set) Token: 0x06006F4E RID: 28494 RVA: 0x002C2494 File Offset: 0x002C0694
	public int Left
	{
		get
		{
			return this.x;
		}
		set
		{
			this.x = value;
		}
	}

	// Token: 0x170010E5 RID: 4325
	// (get) Token: 0x06006F4F RID: 28495 RVA: 0x002C24A0 File Offset: 0x002C06A0
	public int Top
	{
		get
		{
			return this.y + this.dimY;
		}
	}

	// Token: 0x170010E6 RID: 4326
	// (get) Token: 0x06006F50 RID: 28496 RVA: 0x002C24B0 File Offset: 0x002C06B0
	public int Right
	{
		get
		{
			return this.x + this.dimX;
		}
	}

	// Token: 0x170010E7 RID: 4327
	// (get) Token: 0x06006F51 RID: 28497 RVA: 0x002C24C0 File Offset: 0x002C06C0
	// (set) Token: 0x06006F52 RID: 28498 RVA: 0x002C24C8 File Offset: 0x002C06C8
	public int Bottom
	{
		get
		{
			return this.y;
		}
		set
		{
			this.y = value;
		}
	}

	// Token: 0x170010E8 RID: 4328
	// (get) Token: 0x06006F53 RID: 28499 RVA: 0x002C24D4 File Offset: 0x002C06D4
	public int Width
	{
		get
		{
			return this.dimX;
		}
	}

	// Token: 0x170010E9 RID: 4329
	// (get) Token: 0x06006F54 RID: 28500 RVA: 0x002C24DC File Offset: 0x002C06DC
	public int Height
	{
		get
		{
			return this.dimY;
		}
	}

	// Token: 0x170010EA RID: 4330
	// (get) Token: 0x06006F55 RID: 28501 RVA: 0x002C24E4 File Offset: 0x002C06E4
	public int Metric
	{
		get
		{
			return Math.Max(this.Width, this.Height);
		}
	}

	// Token: 0x170010EB RID: 4331
	// (get) Token: 0x06006F56 RID: 28502 RVA: 0x002C24F8 File Offset: 0x002C06F8
	public Vector2 Center
	{
		get
		{
			return new Vector2((float)this.x + (float)this.dimX / 2f, (float)this.y + (float)this.dimY / 2f);
		}
	}

	// Token: 0x06006F57 RID: 28503 RVA: 0x002C252C File Offset: 0x002C072C
	public bool Contains(Vector2 vec)
	{
		return vec.x >= (float)this.x && vec.x <= (float)(this.x + this.dimX) && vec.y >= (float)this.y && vec.y <= (float)(this.y + this.dimY);
	}

	// Token: 0x04006E9F RID: 28319
	private int x;

	// Token: 0x04006EA0 RID: 28320
	private int y;

	// Token: 0x04006EA1 RID: 28321
	private int dimX;

	// Token: 0x04006EA2 RID: 28322
	private int dimY;
}
