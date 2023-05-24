using System;
using System.Collections.Generic;
using System.Linq;

namespace FullSerializer
{
	// Token: 0x020005BB RID: 1467
	public static class fsTypeExtensions
	{
		// Token: 0x060022E0 RID: 8928 RVA: 0x000997F8 File Offset: 0x000979F8
		public static string CSharpName(this Type type)
		{
			return type.CSharpName(false);
		}

		// Token: 0x060022E1 RID: 8929 RVA: 0x00099804 File Offset: 0x00097A04
		public static string CSharpName(this Type type, bool includeNamespace, bool ensureSafeDeclarationName)
		{
			string text = type.CSharpName(includeNamespace);
			if (ensureSafeDeclarationName)
			{
				text = text.Replace('>', '_').Replace('<', '_').Replace('.', '_');
			}
			return text;
		}

		// Token: 0x060022E2 RID: 8930 RVA: 0x00099840 File Offset: 0x00097A40
		public static string CSharpName(this Type type, bool includeNamespace)
		{
			if (type == typeof(void))
			{
				return "void";
			}
			if (type == typeof(int))
			{
				return "int";
			}
			if (type == typeof(float))
			{
				return "float";
			}
			if (type == typeof(bool))
			{
				return "bool";
			}
			if (type == typeof(double))
			{
				return "double";
			}
			if (type == typeof(string))
			{
				return "string";
			}
			if (type.IsGenericParameter)
			{
				return type.ToString();
			}
			string text = string.Empty;
			IEnumerable<Type> enumerable = type.GetGenericArguments();
			if (type.IsNested)
			{
				text = text + type.DeclaringType.CSharpName() + ".";
				if (type.DeclaringType.GetGenericArguments().Length > 0)
				{
					enumerable = enumerable.Skip(type.DeclaringType.GetGenericArguments().Length);
				}
			}
			if (!enumerable.Any<Type>())
			{
				text += type.Name;
			}
			else
			{
				text += type.Name.Substring(0, type.Name.IndexOf('`'));
				text = text + "<" + string.Join(",", enumerable.Select((Type t) => t.CSharpName(includeNamespace)).ToArray<string>()) + ">";
			}
			if (includeNamespace && type.Namespace != null)
			{
				text = type.Namespace + "." + text;
			}
			return text;
		}
	}
}
