using System;
using UnityEngine;

namespace DaikonForge.Tween
{
	// Token: 0x0200052A RID: 1322
	public interface ISplineInterpolator
	{
		// Token: 0x06001FC8 RID: 8136
		Vector3 Evaluate(Vector3 a, Vector3 b, Vector3 c, Vector3 d, float t);
	}
}
