using System;
using UnityEngine;

namespace DaikonForge.Tween
{
	// Token: 0x02000526 RID: 1318
	public class CatmullRomSpline : ISplineInterpolator
	{
		// Token: 0x06001FB2 RID: 8114 RVA: 0x0008E144 File Offset: 0x0008C344
		public Vector3 Evaluate(Vector3 a, Vector3 b, Vector3 c, Vector3 d, float t)
		{
			return 0.5f * (2f * b + (-a + c) * t + (2f * a - 5f * b + 4f * c - d) * t * t + (-a + 3f * b - 3f * c + d) * t * t * t);
		}
	}
}
