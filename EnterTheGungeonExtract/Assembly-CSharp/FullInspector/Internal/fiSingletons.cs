using System;
using System.Collections.Generic;

namespace FullInspector.Internal
{
	// Token: 0x02000580 RID: 1408
	public static class fiSingletons
	{
		// Token: 0x06002157 RID: 8535 RVA: 0x00093208 File Offset: 0x00091408
		public static T Get<T>()
		{
			return (T)((object)fiSingletons.Get(typeof(T)));
		}

		// Token: 0x06002158 RID: 8536 RVA: 0x00093220 File Offset: 0x00091420
		public static object Get(Type type)
		{
			object obj;
			if (!fiSingletons._instances.TryGetValue(type, out obj))
			{
				obj = Activator.CreateInstance(type);
				fiSingletons._instances[type] = obj;
			}
			return obj;
		}

		// Token: 0x04001818 RID: 6168
		private static Dictionary<Type, object> _instances = new Dictionary<Type, object>();
	}
}
