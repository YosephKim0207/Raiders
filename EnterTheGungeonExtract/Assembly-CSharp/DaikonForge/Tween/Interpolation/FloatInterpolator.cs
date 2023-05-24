using System;

namespace DaikonForge.Tween.Interpolation
{
	// Token: 0x020004F1 RID: 1265
	public class FloatInterpolator : Interpolator<float>
	{
		// Token: 0x06001E57 RID: 7767 RVA: 0x0008AC04 File Offset: 0x00088E04
		public override float Add(float lhs, float rhs)
		{
			return lhs + rhs;
		}

		// Token: 0x06001E58 RID: 7768 RVA: 0x0008AC0C File Offset: 0x00088E0C
		public override float Interpolate(float startValue, float endValue, float time)
		{
			return startValue + (endValue - startValue) * time;
		}

		// Token: 0x17000614 RID: 1556
		// (get) Token: 0x06001E59 RID: 7769 RVA: 0x0008AC18 File Offset: 0x00088E18
		public static Interpolator<float> Default
		{
			get
			{
				if (FloatInterpolator.singleton == null)
				{
					FloatInterpolator.singleton = new FloatInterpolator();
				}
				return FloatInterpolator.singleton;
			}
		}

		// Token: 0x040016C5 RID: 5829
		protected static FloatInterpolator singleton;
	}
}
