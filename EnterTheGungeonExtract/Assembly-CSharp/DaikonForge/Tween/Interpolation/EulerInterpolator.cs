using System;
using UnityEngine;

namespace DaikonForge.Tween.Interpolation
{
	// Token: 0x020004F5 RID: 1269
	public class EulerInterpolator : Interpolator<Vector3>
	{
		// Token: 0x17000618 RID: 1560
		// (get) Token: 0x06001E67 RID: 7783 RVA: 0x0008ADC0 File Offset: 0x00088FC0
		public static Interpolator<Vector3> Default
		{
			get
			{
				if (EulerInterpolator.singleton == null)
				{
					EulerInterpolator.singleton = new EulerInterpolator();
				}
				return EulerInterpolator.singleton;
			}
		}

		// Token: 0x06001E68 RID: 7784 RVA: 0x0008ADDC File Offset: 0x00088FDC
		public override Vector3 Add(Vector3 lhs, Vector3 rhs)
		{
			return lhs + rhs;
		}

		// Token: 0x06001E69 RID: 7785 RVA: 0x0008ADE8 File Offset: 0x00088FE8
		public override Vector3 Interpolate(Vector3 startValue, Vector3 endValue, float time)
		{
			float num = EulerInterpolator.clerp(startValue.x, endValue.x, time);
			float num2 = EulerInterpolator.clerp(startValue.y, endValue.y, time);
			float num3 = EulerInterpolator.clerp(startValue.z, endValue.z, time);
			return new Vector3(num, num2, num3);
		}

		// Token: 0x06001E6A RID: 7786 RVA: 0x0008AE3C File Offset: 0x0008903C
		private static float clerp(float start, float end, float time)
		{
			float num = 0f;
			float num2 = 360f;
			float num3 = Mathf.Abs((num2 - num) / 2f);
			float num5;
			if (end - start < -num3)
			{
				float num4 = (num2 - start + end) * time;
				num5 = start + num4;
			}
			else if (end - start > num3)
			{
				float num4 = -(num2 - end + start) * time;
				num5 = start + num4;
			}
			else
			{
				num5 = start + (end - start) * time;
			}
			return num5;
		}

		// Token: 0x040016C9 RID: 5833
		protected static EulerInterpolator singleton;
	}
}
