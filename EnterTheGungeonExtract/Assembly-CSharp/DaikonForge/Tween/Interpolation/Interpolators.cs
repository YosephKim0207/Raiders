using System;
using System.Collections.Generic;
using UnityEngine;

namespace DaikonForge.Tween.Interpolation
{
	// Token: 0x020004EE RID: 1262
	public static class Interpolators
	{
		// Token: 0x06001E4B RID: 7755 RVA: 0x0008AAD8 File Offset: 0x00088CD8
		static Interpolators()
		{
			Interpolators.Register<int>(IntInterpolator.Default);
			Interpolators.Register<float>(FloatInterpolator.Default);
			Interpolators.Register<Rect>(RectInterpolator.Default);
			Interpolators.Register<Color>(ColorInterpolator.Default);
			Interpolators.Register<Vector2>(Vector2Interpolator.Default);
			Interpolators.Register<Vector3>(Vector3Interpolator.Default);
			Interpolators.Register<Vector4>(Vector4Interpolator.Default);
		}

		// Token: 0x06001E4C RID: 7756 RVA: 0x0008AB38 File Offset: 0x00088D38
		public static Interpolator<T> Get<T>()
		{
			return (Interpolator<T>)Interpolators.Get(typeof(T), true);
		}

		// Token: 0x06001E4D RID: 7757 RVA: 0x0008AB50 File Offset: 0x00088D50
		public static object Get(Type type, bool throwOnNotFound)
		{
			if (type == null)
			{
				throw new ArgumentNullException("You must provide a System.Type value");
			}
			object obj = null;
			if (!Interpolators.registry.TryGetValue(type, out obj) && throwOnNotFound)
			{
				throw new KeyNotFoundException(string.Format("There is no default interpolator defined for type '{0}'", type.Name));
			}
			return obj;
		}

		// Token: 0x06001E4E RID: 7758 RVA: 0x0008ABA0 File Offset: 0x00088DA0
		public static void Register<T>(Interpolator<T> interpolator)
		{
			Interpolators.registry[typeof(T)] = interpolator;
		}

		// Token: 0x040016C3 RID: 5827
		private static Dictionary<Type, object> registry = new Dictionary<Type, object>();
	}
}
