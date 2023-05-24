using System;
using FullSerializer.Internal;

namespace FullInspector.Internal
{
	// Token: 0x020005DF RID: 1503
	public static class TypeExtensions
	{
		// Token: 0x060023AB RID: 9131 RVA: 0x0009C6F4 File Offset: 0x0009A8F4
		public static bool IsNullableType(this Type type)
		{
			return type.Resolve().IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
		}

		// Token: 0x060023AC RID: 9132 RVA: 0x0009C71C File Offset: 0x0009A91C
		private static bool CompareTypes(Type a, Type b)
		{
			if (a.Resolve().IsGenericType && b.Resolve().IsGenericType && (a.Resolve().IsGenericTypeDefinition || b.Resolve().IsGenericTypeDefinition))
			{
				a = a.GetGenericTypeDefinition();
				b = b.GetGenericTypeDefinition();
			}
			return a == b;
		}

		// Token: 0x060023AD RID: 9133 RVA: 0x0009C780 File Offset: 0x0009A980
		public static bool HasParent(this Type type, Type parentType)
		{
			if (TypeExtensions.CompareTypes(type, parentType))
			{
				return false;
			}
			if (parentType.IsAssignableFrom(type))
			{
				return true;
			}
			while (type != null)
			{
				if (TypeExtensions.CompareTypes(type, parentType))
				{
					return true;
				}
				foreach (Type type2 in type.GetInterfaces())
				{
					if (TypeExtensions.CompareTypes(type2, parentType))
					{
						return true;
					}
				}
				type = type.Resolve().BaseType;
			}
			return false;
		}

		// Token: 0x060023AE RID: 9134 RVA: 0x0009C800 File Offset: 0x0009AA00
		public static Type GetInterface(this Type type, Type interfaceType)
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

		// Token: 0x060023AF RID: 9135 RVA: 0x0009C89C File Offset: 0x0009AA9C
		public static bool IsImplementationOf(this Type type, Type interfaceType)
		{
			if (interfaceType.Resolve().IsGenericType && !interfaceType.Resolve().IsGenericTypeDefinition)
			{
				throw new ArgumentException("IsImplementationOf requires that if the interface type is generic, then it must be the generic type definition, not a specific generic type instantiation");
			}
			if (type.Resolve().IsGenericType)
			{
				type = type.GetGenericTypeDefinition();
			}
			while (type != null)
			{
				foreach (Type type2 in type.GetInterfaces())
				{
					if (type2.Resolve().IsGenericType)
					{
						if (interfaceType == type2.GetGenericTypeDefinition())
						{
							return true;
						}
					}
					else if (interfaceType == type2)
					{
						return true;
					}
				}
				type = type.Resolve().BaseType;
			}
			return false;
		}
	}
}
