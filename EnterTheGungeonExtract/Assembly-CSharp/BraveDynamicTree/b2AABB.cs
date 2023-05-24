using System;
using UnityEngine;

namespace BraveDynamicTree
{
	// Token: 0x0200035D RID: 861
	public struct b2AABB
	{
		// Token: 0x06000D98 RID: 3480 RVA: 0x00040A50 File Offset: 0x0003EC50
		public b2AABB(float lowX, float lowY, float upperX, float upperY)
		{
			this.lowerBound.x = lowX;
			this.lowerBound.y = lowY;
			this.upperBound.x = upperX;
			this.upperBound.y = upperY;
		}

		// Token: 0x06000D99 RID: 3481 RVA: 0x00040A84 File Offset: 0x0003EC84
		public b2AABB(Vector2 lowerBound, Vector2 upperBound)
		{
			this.lowerBound = lowerBound;
			this.upperBound = upperBound;
		}

		// Token: 0x06000D9A RID: 3482 RVA: 0x00040A94 File Offset: 0x0003EC94
		public bool IsValid()
		{
			Vector2 vector = this.upperBound - this.lowerBound;
			return vector.x >= 0f && vector.y >= 0f;
		}

		// Token: 0x06000D9B RID: 3483 RVA: 0x00040ADC File Offset: 0x0003ECDC
		public Vector2 GetCenter()
		{
			return 0.5f * (this.lowerBound + this.upperBound);
		}

		// Token: 0x06000D9C RID: 3484 RVA: 0x00040AFC File Offset: 0x0003ECFC
		public Vector2 GetExtents()
		{
			return 0.5f * (this.upperBound - this.lowerBound);
		}

		// Token: 0x06000D9D RID: 3485 RVA: 0x00040B1C File Offset: 0x0003ED1C
		public float GetPerimeter()
		{
			float num = this.upperBound.x - this.lowerBound.x;
			float num2 = this.upperBound.y - this.lowerBound.y;
			return 2f * (num + num2);
		}

		// Token: 0x06000D9E RID: 3486 RVA: 0x00040B64 File Offset: 0x0003ED64
		public void Combine(b2AABB aabb)
		{
			this.lowerBound = Vector2.Min(this.lowerBound, aabb.lowerBound);
			this.upperBound = Vector2.Max(this.upperBound, aabb.upperBound);
		}

		// Token: 0x06000D9F RID: 3487 RVA: 0x00040B98 File Offset: 0x0003ED98
		public void Combine(b2AABB aabb1, b2AABB aabb2)
		{
			this.lowerBound = Vector2.Min(aabb1.lowerBound, aabb2.lowerBound);
			this.upperBound = Vector2.Max(aabb1.upperBound, aabb2.upperBound);
		}

		// Token: 0x06000DA0 RID: 3488 RVA: 0x00040BCC File Offset: 0x0003EDCC
		public bool Contains(b2AABB aabb)
		{
			return this.lowerBound.x <= aabb.lowerBound.x && this.lowerBound.y <= aabb.lowerBound.y && aabb.upperBound.x <= this.upperBound.x && aabb.upperBound.y <= this.upperBound.y;
		}

		// Token: 0x06000DA1 RID: 3489 RVA: 0x00040C4C File Offset: 0x0003EE4C
		public bool RayCast(ref b2RayCastOutput output, b2RayCastInput input)
		{
			float num = float.MinValue;
			float num2 = float.MaxValue;
			Vector2 p = input.p1;
			Vector2 vector = input.p2 - input.p1;
			Vector2 vector2 = vector.Abs();
			Vector2 vector3 = Vector2.zero;
			for (int i = 0; i < 2; i++)
			{
				if (vector2[i] < 1E-45f)
				{
					if (p[i] < this.lowerBound[i] || this.upperBound[i] < p[i])
					{
						return false;
					}
				}
				else
				{
					float num3 = 1f / vector[i];
					float num4 = (this.lowerBound[i] - p[i]) * num3;
					float num5 = (this.upperBound[i] - p[i]) * num3;
					float num6 = -1f;
					if (num4 > num5)
					{
						float num7 = num4;
						num4 = num5;
						num5 = num7;
						num6 = 1f;
					}
					if (num4 > num)
					{
						vector3 = Vector2.zero;
						vector3[i] = num6;
						num = num4;
					}
					num2 = Mathf.Min(num2, num5);
					if (num > num2)
					{
						return false;
					}
				}
			}
			if (num < 0f || input.maxFraction < num)
			{
				return false;
			}
			output.fraction = num;
			output.normal = vector3;
			return true;
		}

		// Token: 0x06000DA2 RID: 3490 RVA: 0x00040DBC File Offset: 0x0003EFBC
		public static bool b2TestOverlap(ref b2AABB a, ref b2AABB b)
		{
			return b.lowerBound.x <= a.upperBound.x && a.lowerBound.x <= b.upperBound.x && b.lowerBound.y <= a.upperBound.y && a.lowerBound.y <= b.upperBound.y;
		}

		// Token: 0x04000DFB RID: 3579
		public Vector2 lowerBound;

		// Token: 0x04000DFC RID: 3580
		public Vector2 upperBound;
	}
}
