using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace FullSerializer.Internal
{
	// Token: 0x020005B7 RID: 1463
	public static class fsPortableReflection
	{
		// Token: 0x060022BF RID: 8895 RVA: 0x0009929C File Offset: 0x0009749C
		public static bool HasAttribute(MemberInfo element, Type attributeType)
		{
			return fsPortableReflection.GetAttribute(element, attributeType) != null;
		}

		// Token: 0x060022C0 RID: 8896 RVA: 0x000992AC File Offset: 0x000974AC
		public static bool HasAttribute<TAttribute>(MemberInfo element)
		{
			return fsPortableReflection.HasAttribute(element, typeof(TAttribute));
		}

		// Token: 0x060022C1 RID: 8897 RVA: 0x000992C0 File Offset: 0x000974C0
		public static Attribute GetAttribute(MemberInfo element, Type attributeType)
		{
			fsPortableReflection.AttributeQuery attributeQuery = new fsPortableReflection.AttributeQuery
			{
				MemberInfo = element,
				AttributeType = attributeType
			};
			Attribute attribute;
			if (!fsPortableReflection._cachedAttributeQueries.TryGetValue(attributeQuery, out attribute))
			{
				object[] customAttributes = element.GetCustomAttributes(attributeType, true);
				attribute = (Attribute)customAttributes.FirstOrDefault<object>();
				fsPortableReflection._cachedAttributeQueries[attributeQuery] = attribute;
			}
			return attribute;
		}

		// Token: 0x060022C2 RID: 8898 RVA: 0x0009931C File Offset: 0x0009751C
		public static TAttribute GetAttribute<TAttribute>(MemberInfo element) where TAttribute : Attribute
		{
			return (TAttribute)((object)fsPortableReflection.GetAttribute(element, typeof(TAttribute)));
		}

		// Token: 0x060022C3 RID: 8899 RVA: 0x00099334 File Offset: 0x00097534
		public static PropertyInfo GetDeclaredProperty(this Type type, string propertyName)
		{
			PropertyInfo[] declaredProperties = type.GetDeclaredProperties();
			for (int i = 0; i < declaredProperties.Length; i++)
			{
				if (declaredProperties[i].Name == propertyName)
				{
					return declaredProperties[i];
				}
			}
			return null;
		}

		// Token: 0x060022C4 RID: 8900 RVA: 0x00099374 File Offset: 0x00097574
		public static MethodInfo GetDeclaredMethod(this Type type, string methodName)
		{
			MethodInfo[] declaredMethods = type.GetDeclaredMethods();
			for (int i = 0; i < declaredMethods.Length; i++)
			{
				if (declaredMethods[i].Name == methodName)
				{
					return declaredMethods[i];
				}
			}
			return null;
		}

		// Token: 0x060022C5 RID: 8901 RVA: 0x000993B4 File Offset: 0x000975B4
		public static ConstructorInfo GetDeclaredConstructor(this Type type, Type[] parameters)
		{
			foreach (ConstructorInfo constructorInfo in type.GetDeclaredConstructors())
			{
				ParameterInfo[] parameters2 = constructorInfo.GetParameters();
				if (parameters.Length == parameters2.Length)
				{
					for (int j = 0; j < parameters2.Length; j++)
					{
						if (parameters2[j].ParameterType != parameters[j])
						{
						}
					}
					return constructorInfo;
				}
			}
			return null;
		}

		// Token: 0x060022C6 RID: 8902 RVA: 0x0009942C File Offset: 0x0009762C
		public static ConstructorInfo[] GetDeclaredConstructors(this Type type)
		{
			return type.GetConstructors(fsPortableReflection.DeclaredFlags);
		}

		// Token: 0x060022C7 RID: 8903 RVA: 0x0009943C File Offset: 0x0009763C
		public static MemberInfo[] GetFlattenedMember(this Type type, string memberName)
		{
			List<MemberInfo> list = new List<MemberInfo>();
			while (type != null)
			{
				MemberInfo[] declaredMembers = type.GetDeclaredMembers();
				for (int i = 0; i < declaredMembers.Length; i++)
				{
					if (declaredMembers[i].Name == memberName)
					{
						list.Add(declaredMembers[i]);
					}
				}
				type = type.Resolve().BaseType;
			}
			return list.ToArray();
		}

		// Token: 0x060022C8 RID: 8904 RVA: 0x000994A4 File Offset: 0x000976A4
		public static MethodInfo GetFlattenedMethod(this Type type, string methodName)
		{
			while (type != null)
			{
				MethodInfo[] declaredMethods = type.GetDeclaredMethods();
				for (int i = 0; i < declaredMethods.Length; i++)
				{
					if (declaredMethods[i].Name == methodName)
					{
						return declaredMethods[i];
					}
				}
				type = type.Resolve().BaseType;
			}
			return null;
		}

		// Token: 0x060022C9 RID: 8905 RVA: 0x000994FC File Offset: 0x000976FC
		public static IEnumerable<MethodInfo> GetFlattenedMethods(this Type type, string methodName)
		{
			while (type != null)
			{
				MethodInfo[] methods = type.GetDeclaredMethods();
				for (int i = 0; i < methods.Length; i++)
				{
					if (methods[i].Name == methodName)
					{
						yield return methods[i];
					}
				}
				type = type.Resolve().BaseType;
			}
			yield break;
		}

		// Token: 0x060022CA RID: 8906 RVA: 0x00099530 File Offset: 0x00097730
		public static PropertyInfo GetFlattenedProperty(this Type type, string propertyName)
		{
			while (type != null)
			{
				PropertyInfo[] declaredProperties = type.GetDeclaredProperties();
				for (int i = 0; i < declaredProperties.Length; i++)
				{
					if (declaredProperties[i].Name == propertyName)
					{
						return declaredProperties[i];
					}
				}
				type = type.Resolve().BaseType;
			}
			return null;
		}

		// Token: 0x060022CB RID: 8907 RVA: 0x00099588 File Offset: 0x00097788
		public static MemberInfo GetDeclaredMember(this Type type, string memberName)
		{
			MemberInfo[] declaredMembers = type.GetDeclaredMembers();
			for (int i = 0; i < declaredMembers.Length; i++)
			{
				if (declaredMembers[i].Name == memberName)
				{
					return declaredMembers[i];
				}
			}
			return null;
		}

		// Token: 0x060022CC RID: 8908 RVA: 0x000995C8 File Offset: 0x000977C8
		public static MethodInfo[] GetDeclaredMethods(this Type type)
		{
			return type.GetMethods(fsPortableReflection.DeclaredFlags);
		}

		// Token: 0x060022CD RID: 8909 RVA: 0x000995D8 File Offset: 0x000977D8
		public static PropertyInfo[] GetDeclaredProperties(this Type type)
		{
			return type.GetProperties(fsPortableReflection.DeclaredFlags);
		}

		// Token: 0x060022CE RID: 8910 RVA: 0x000995E8 File Offset: 0x000977E8
		public static FieldInfo[] GetDeclaredFields(this Type type)
		{
			return type.GetFields(fsPortableReflection.DeclaredFlags);
		}

		// Token: 0x060022CF RID: 8911 RVA: 0x000995F8 File Offset: 0x000977F8
		public static MemberInfo[] GetDeclaredMembers(this Type type)
		{
			return type.GetMembers(fsPortableReflection.DeclaredFlags);
		}

		// Token: 0x060022D0 RID: 8912 RVA: 0x00099608 File Offset: 0x00097808
		public static MemberInfo AsMemberInfo(Type type)
		{
			return type;
		}

		// Token: 0x060022D1 RID: 8913 RVA: 0x0009960C File Offset: 0x0009780C
		public static bool IsType(MemberInfo member)
		{
			return member is Type;
		}

		// Token: 0x060022D2 RID: 8914 RVA: 0x00099618 File Offset: 0x00097818
		public static Type AsType(MemberInfo member)
		{
			return (Type)member;
		}

		// Token: 0x060022D3 RID: 8915 RVA: 0x00099620 File Offset: 0x00097820
		public static Type Resolve(this Type type)
		{
			return type;
		}

		// Token: 0x04001871 RID: 6257
		public static Type[] EmptyTypes = new Type[0];

		// Token: 0x04001872 RID: 6258
		private static IDictionary<fsPortableReflection.AttributeQuery, Attribute> _cachedAttributeQueries = new Dictionary<fsPortableReflection.AttributeQuery, Attribute>(new fsPortableReflection.AttributeQueryComparator());

		// Token: 0x04001873 RID: 6259
		private static BindingFlags DeclaredFlags = BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;

		// Token: 0x020005B8 RID: 1464
		private struct AttributeQuery
		{
			// Token: 0x04001874 RID: 6260
			public MemberInfo MemberInfo;

			// Token: 0x04001875 RID: 6261
			public Type AttributeType;
		}

		// Token: 0x020005B9 RID: 1465
		private class AttributeQueryComparator : IEqualityComparer<fsPortableReflection.AttributeQuery>
		{
			// Token: 0x060022D6 RID: 8918 RVA: 0x00099650 File Offset: 0x00097850
			public bool Equals(fsPortableReflection.AttributeQuery x, fsPortableReflection.AttributeQuery y)
			{
				return x.MemberInfo == y.MemberInfo && x.AttributeType == y.AttributeType;
			}

			// Token: 0x060022D7 RID: 8919 RVA: 0x00099678 File Offset: 0x00097878
			public int GetHashCode(fsPortableReflection.AttributeQuery obj)
			{
				return obj.MemberInfo.GetHashCode() + 17 * obj.AttributeType.GetHashCode();
			}
		}
	}
}
