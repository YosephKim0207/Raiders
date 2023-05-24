using System;
using UnityEngine;

namespace DaikonForge.Tween.Interpolation
{
	// Token: 0x020004F7 RID: 1271
	public class ColorInterpolator : Interpolator<Color>
	{
		// Token: 0x06001E70 RID: 7792 RVA: 0x0008AEFC File Offset: 0x000890FC
		public override Color Add(Color lhs, Color rhs)
		{
			return lhs + rhs;
		}

		// Token: 0x06001E71 RID: 7793 RVA: 0x0008AF08 File Offset: 0x00089108
		public override Color Interpolate(Color startValue, Color endValue, float time)
		{
			return Color.Lerp(startValue, endValue, time);
		}

		// Token: 0x1700061A RID: 1562
		// (get) Token: 0x06001E72 RID: 7794 RVA: 0x0008AF14 File Offset: 0x00089114
		public static Interpolator<Color> Default
		{
			get
			{
				if (ColorInterpolator.singleton == null)
				{
					ColorInterpolator.singleton = new ColorInterpolator();
				}
				return ColorInterpolator.singleton;
			}
		}

		// Token: 0x040016CB RID: 5835
		protected static ColorInterpolator singleton;
	}
}
