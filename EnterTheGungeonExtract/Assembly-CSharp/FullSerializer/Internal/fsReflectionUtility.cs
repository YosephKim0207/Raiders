using System;

namespace FullSerializer.Internal
{
	// Token: 0x020005C4 RID: 1476
	public static class fsReflectionUtility
	{
		// Token: 0x0600231A RID: 8986 RVA: 0x0009A7D4 File Offset: 0x000989D4
		public static Type GetInterface(Type type, Type interfaceType)
		{
			if (interfaceType.Resolve().IsGenericType && !interfaceType.Resolve().IsGenericTypeDefinition)
			{
				throw new ArgumentException("GetInterface requires that if the interface type is generic, then it must be the generic type definition, not a specific generic type instantiation");
			}
			while (type != null)
			{
				foreach (Type type2 in type.GetInterfaces())
				{
					if (type2.Resolve().IsGenericType)
					{
						if (interfaceType == type2.GetGenericTypeDefinition())
						{
							return type2;
						}
					}
					else if (interfaceType == type2)
					{
						return type2;
					}
				}
				type = type.Resolve().BaseType;
			}
			return null;
		}
	}
}
