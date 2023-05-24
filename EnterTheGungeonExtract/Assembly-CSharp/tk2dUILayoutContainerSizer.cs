using System;
using UnityEngine;

// Token: 0x02000C1D RID: 3101
[AddComponentMenu("2D Toolkit/UI/Core/tk2dUILayoutContainerSizer")]
public class tk2dUILayoutContainerSizer : tk2dUILayoutContainer
{
	// Token: 0x060042AF RID: 17071 RVA: 0x00158CFC File Offset: 0x00156EFC
	protected override void DoChildLayout()
	{
		int count = this.layoutItems.Count;
		if (count == 0)
		{
			return;
		}
		float num = this.bMax.x - this.bMin.x - 2f * this.margin.x;
		float num2 = this.bMax.y - this.bMin.y - 2f * this.margin.y;
		float num3 = ((!this.horizontal) ? num2 : num) - this.spacing * (float)(count - 1);
		float num4 = 1f;
		float num5 = num3;
		float num6 = 0f;
		float[] array = new float[count];
		for (int i = 0; i < count; i++)
		{
			tk2dUILayoutItem tk2dUILayoutItem = this.layoutItems[i];
			if (tk2dUILayoutItem.fixedSize)
			{
				array[i] = ((!this.horizontal) ? (tk2dUILayoutItem.layout.bMax.y - tk2dUILayoutItem.layout.bMin.y) : (tk2dUILayoutItem.layout.bMax.x - tk2dUILayoutItem.layout.bMin.x));
				num5 -= array[i];
			}
			else if (tk2dUILayoutItem.fillPercentage > 0f)
			{
				float num7 = num4 * tk2dUILayoutItem.fillPercentage / 100f;
				array[i] = num3 * num7;
				num5 -= array[i];
				num4 -= num7;
			}
			else
			{
				num6 += tk2dUILayoutItem.sizeProportion;
			}
		}
		for (int j = 0; j < count; j++)
		{
			tk2dUILayoutItem tk2dUILayoutItem2 = this.layoutItems[j];
			if (!tk2dUILayoutItem2.fixedSize && tk2dUILayoutItem2.fillPercentage <= 0f)
			{
				array[j] = num5 * tk2dUILayoutItem2.sizeProportion / num6;
			}
		}
		Vector3 zero = Vector3.zero;
		Vector3 zero2 = Vector3.zero;
		float num8 = 0f;
		Matrix4x4 localToWorldMatrix = base.transform.localToWorldMatrix;
		if (this.horizontal)
		{
			this.innerSize = new Vector2(2f * this.margin.x + this.spacing * (float)(count - 1), this.bMax.y - this.bMin.y);
		}
		else
		{
			this.innerSize = new Vector2(this.bMax.x - this.bMin.x, 2f * this.margin.y + this.spacing * (float)(count - 1));
		}
		for (int k = 0; k < count; k++)
		{
			tk2dUILayoutItem tk2dUILayoutItem3 = this.layoutItems[k];
			Matrix4x4 matrix4x = tk2dUILayoutItem3.gameObj.transform.localToWorldMatrix * base.transform.worldToLocalMatrix;
			if (this.horizontal)
			{
				if (this.expand)
				{
					zero.y = this.bMin.y + this.margin.y;
					zero2.y = this.bMax.y - this.margin.y;
				}
				else
				{
					zero.y = matrix4x.MultiplyPoint(tk2dUILayoutItem3.layout.bMin).y;
					zero2.y = matrix4x.MultiplyPoint(tk2dUILayoutItem3.layout.bMax).y;
				}
				zero.x = this.bMin.x + this.margin.x + num8;
				zero2.x = zero.x + array[k];
			}
			else
			{
				if (this.expand)
				{
					zero.x = this.bMin.x + this.margin.x;
					zero2.x = this.bMax.x - this.margin.x;
				}
				else
				{
					zero.x = matrix4x.MultiplyPoint(tk2dUILayoutItem3.layout.bMin).x;
					zero2.x = matrix4x.MultiplyPoint(tk2dUILayoutItem3.layout.bMax).x;
				}
				zero2.y = this.bMax.y - this.margin.y - num8;
				zero.y = zero2.y - array[k];
			}
			tk2dUILayoutItem3.layout.SetBounds(localToWorldMatrix.MultiplyPoint(zero), localToWorldMatrix.MultiplyPoint(zero2));
			num8 += array[k] + this.spacing;
			if (this.horizontal)
			{
				this.innerSize.x = this.innerSize.x + array[k];
			}
			else
			{
				this.innerSize.y = this.innerSize.y + array[k];
			}
		}
	}

	// Token: 0x040034F1 RID: 13553
	public bool horizontal;

	// Token: 0x040034F2 RID: 13554
	public bool expand = true;

	// Token: 0x040034F3 RID: 13555
	public Vector2 margin = Vector2.zero;

	// Token: 0x040034F4 RID: 13556
	public float spacing;
}
