using System;
using System.Reflection;

namespace FullSerializer.Internal
{
	// Token: 0x020005C5 RID: 1477
	internal static class fsTypeLookup
	{
		// Token: 0x0600231B RID: 8987 RVA: 0x0009A870 File Offset: 0x00098A70
		public static Type GetType(string typeName)
		{
			Type type = Type.GetType(typeName);
			if (type != null)
			{
				return type;
			}
			foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
			{
				type = assembly.GetType(typeName);
				if (type != null)
				{
					return type;
				}
			}
			return null;
		}
	}
}
