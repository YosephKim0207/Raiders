using System;
using Beebyte.Obfuscator;

namespace DaikonForge.Tween.Interpolation
{
	// Token: 0x020004EF RID: 1263
	public abstract class Interpolator<T>
	{
		// Token: 0x06001E50 RID: 7760
		[Skip]
		public abstract T Add(T lhs, T rhs);

		// Token: 0x06001E51 RID: 7761
		[Skip]
		public abstract T Interpolate(T startValue, T endValue, float time);
	}
}
