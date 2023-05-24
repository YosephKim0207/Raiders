using System;
using UnityEngine;

namespace DaikonForge.Tween.Interpolation
{
	// Token: 0x020004F3 RID: 1267
	public class Vector2Interpolator : Interpolator<Vector2>
	{
		// Token: 0x06001E5F RID: 7775 RVA: 0x0008AD30 File Offset: 0x00088F30
		public override Vector2 Add(Vector2 lhs, Vector2 rhs)
		{
			return lhs + rhs;
		}

		// Token: 0x06001E60 RID: 7776 RVA: 0x0008AD3C File Offset: 0x00088F3C
		public override Vector2 Interpolate(Vector2 startValue, Vector2 endValue, float time)
		{
			return startValue + (endValue - startValue) * time;
		}

		// Token: 0x17000616 RID: 1558
		// (get) Token: 0x06001E61 RID: 7777 RVA: 0x0008AD54 File Offset: 0x00088F54
		public static Interpolator<Vector2> Default
		{
			get
			{
				if (Vector2Interpolator.singleton == null)
				{
					Vector2Interpolator.singleton = new Vector2Interpolator();
				}
				return Vector2Interpolator.singleton;
			}
		}

		// Token: 0x040016C7 RID: 5831
		protected static Vector2Interpolator singleton;
	}
}
