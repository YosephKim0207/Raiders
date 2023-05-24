using System;
using UnityEngine;

// Token: 0x02000BBD RID: 3005
[Serializable]
public class BagelCollider
{
	// Token: 0x06003FE6 RID: 16358 RVA: 0x00144204 File Offset: 0x00142404
	public BagelCollider(BagelCollider source)
	{
		this.width = source.width;
		this.height = source.height;
		this.minX = source.minX;
		this.minY = source.minY;
		this.actualWidth = source.actualWidth;
		this.actualHeight = source.actualHeight;
		this.bagelCollider = new bool[source.bagelCollider.Length];
		for (int i = 0; i < source.bagelCollider.Length; i++)
		{
			this.bagelCollider[i] = source.bagelCollider[i];
		}
	}

	// Token: 0x06003FE7 RID: 16359 RVA: 0x0014429C File Offset: 0x0014249C
	public BagelCollider(int width, int height)
	{
		this.width = width;
		this.height = height;
		this.actualWidth = 0;
		this.actualHeight = 0;
		this.bagelCollider = new bool[0];
	}

	// Token: 0x170009AD RID: 2477
	public bool this[int x, int y]
	{
		get
		{
			if (this.actualWidth == 0 && this.bagelCollider != null && this.bagelCollider.Length > 0)
			{
				this.actualWidth = this.width;
				this.actualHeight = this.height;
			}
			return x >= this.minX && x < this.minX + this.actualWidth && y >= this.minY && y < this.minY + this.actualHeight && this.bagelCollider[(y - this.minY) * this.actualWidth + (x - this.minX)];
		}
	}

	// Token: 0x0400320E RID: 12814
	public int width;

	// Token: 0x0400320F RID: 12815
	public int height;

	// Token: 0x04003210 RID: 12816
	[SerializeField]
	private int minX;

	// Token: 0x04003211 RID: 12817
	[SerializeField]
	private int minY;

	// Token: 0x04003212 RID: 12818
	[SerializeField]
	private int actualWidth;

	// Token: 0x04003213 RID: 12819
	[SerializeField]
	private int actualHeight;

	// Token: 0x04003214 RID: 12820
	[SerializeField]
	private bool[] bagelCollider;
}
