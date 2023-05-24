using System;
using UnityEngine;

namespace DaikonForge.Tween.Interpolation
{
	// Token: 0x020004F2 RID: 1266
	public class RectInterpolator : Interpolator<Rect>
	{
		// Token: 0x06001E5B RID: 7771 RVA: 0x0008AC3C File Offset: 0x00088E3C
		public override Rect Add(Rect lhs, Rect rhs)
		{
			return new Rect(lhs.xMin + rhs.xMin, lhs.yMin + rhs.yMin, lhs.width + rhs.width, lhs.height + rhs.height);
		}

		// Token: 0x06001E5C RID: 7772 RVA: 0x0008AC8C File Offset: 0x00088E8C
		public override Rect Interpolate(Rect startValue, Rect endValue, float time)
		{
			float num = startValue.xMin + (endValue.xMin - startValue.xMin) * time;
			float num2 = startValue.yMin + (endValue.yMin - startValue.yMin) * time;
			float num3 = startValue.width + (endValue.width - startValue.width) * time;
			float num4 = startValue.height + (endValue.height - startValue.height) * time;
			return new Rect(num, num2, num3, num4);
		}

		// Token: 0x17000615 RID: 1557
		// (get) Token: 0x06001E5D RID: 7773 RVA: 0x0008AD0C File Offset: 0x00088F0C
		public static Interpolator<Rect> Default
		{
			get
			{
				if (RectInterpolator.singleton == null)
				{
					RectInterpolator.singleton = new RectInterpolator();
				}
				return RectInterpolator.singleton;
			}
		}

		// Token: 0x040016C6 RID: 5830
		protected static RectInterpolator singleton;
	}
}
