using System;
using UnityEngine;

namespace DaikonForge.Tween.Interpolation
{
	// Token: 0x020004F4 RID: 1268
	public class Vector3Interpolator : Interpolator<Vector3>
	{
		// Token: 0x06001E63 RID: 7779 RVA: 0x0008AD78 File Offset: 0x00088F78
		public override Vector3 Add(Vector3 lhs, Vector3 rhs)
		{
			return lhs + rhs;
		}

		// Token: 0x06001E64 RID: 7780 RVA: 0x0008AD84 File Offset: 0x00088F84
		public override Vector3 Interpolate(Vector3 startValue, Vector3 endValue, float time)
		{
			return startValue + (endValue - startValue) * time;
		}

		// Token: 0x17000617 RID: 1559
		// (get) Token: 0x06001E65 RID: 7781 RVA: 0x0008AD9C File Offset: 0x00088F9C
		public static Interpolator<Vector3> Default
		{
			get
			{
				if (Vector3Interpolator.singleton == null)
				{
					Vector3Interpolator.singleton = new Vector3Interpolator();
				}
				return Vector3Interpolator.singleton;
			}
		}

		// Token: 0x040016C8 RID: 5832
		protected static Vector3Interpolator singleton;
	}
}
