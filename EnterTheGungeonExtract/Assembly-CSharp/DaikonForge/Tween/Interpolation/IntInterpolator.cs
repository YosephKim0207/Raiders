using System;
using UnityEngine;

namespace DaikonForge.Tween.Interpolation
{
	// Token: 0x020004F0 RID: 1264
	public class IntInterpolator : Interpolator<int>
	{
		// Token: 0x06001E53 RID: 7763 RVA: 0x0008ABC8 File Offset: 0x00088DC8
		public override int Add(int lhs, int rhs)
		{
			return lhs + rhs;
		}

		// Token: 0x06001E54 RID: 7764 RVA: 0x0008ABD0 File Offset: 0x00088DD0
		public override int Interpolate(int startValue, int endValue, float time)
		{
			return Mathf.RoundToInt((float)startValue + (float)(endValue - startValue) * time);
		}

		// Token: 0x17000613 RID: 1555
		// (get) Token: 0x06001E55 RID: 7765 RVA: 0x0008ABE0 File Offset: 0x00088DE0
		public static Interpolator<int> Default
		{
			get
			{
				if (IntInterpolator.singleton == null)
				{
					IntInterpolator.singleton = new IntInterpolator();
				}
				return IntInterpolator.singleton;
			}
		}

		// Token: 0x040016C4 RID: 5828
		protected static IntInterpolator singleton;
	}
}
