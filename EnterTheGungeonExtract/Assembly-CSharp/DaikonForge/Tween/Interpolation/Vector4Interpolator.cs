using System;
using UnityEngine;

namespace DaikonForge.Tween.Interpolation
{
	// Token: 0x020004F6 RID: 1270
	public class Vector4Interpolator : Interpolator<Vector4>
	{
		// Token: 0x06001E6C RID: 7788 RVA: 0x0008AEBC File Offset: 0x000890BC
		public override Vector4 Add(Vector4 lhs, Vector4 rhs)
		{
			return lhs + rhs;
		}

		// Token: 0x06001E6D RID: 7789 RVA: 0x0008AEC8 File Offset: 0x000890C8
		public override Vector4 Interpolate(Vector4 startValue, Vector4 endValue, float time)
		{
			return startValue + endValue * time;
		}

		// Token: 0x17000619 RID: 1561
		// (get) Token: 0x06001E6E RID: 7790 RVA: 0x0008AED8 File Offset: 0x000890D8
		public static Interpolator<Vector4> Default
		{
			get
			{
				if (Vector4Interpolator.singleton == null)
				{
					Vector4Interpolator.singleton = new Vector4Interpolator();
				}
				return Vector4Interpolator.singleton;
			}
		}

		// Token: 0x040016CA RID: 5834
		protected static Vector4Interpolator singleton;
	}
}
