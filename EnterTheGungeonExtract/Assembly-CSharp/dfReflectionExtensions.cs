using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using UnityEngine;

// Token: 0x02000401 RID: 1025
public static class dfReflectionExtensions
{
	// Token: 0x06001677 RID: 5751 RVA: 0x0006A93C File Offset: 0x00068B3C
	public static MemberTypes GetMemberType(this MemberInfo member)
	{
		return member.MemberType;
	}

	// Token: 0x06001678 RID: 5752 RVA: 0x0006A944 File Offset: 0x00068B44
	public static Type GetBaseType(this Type type)
	{
		return type.BaseType;
	}

	// Token: 0x06001679 RID: 5753 RVA: 0x0006A94C File Offset: 0x00068B4C
	public static Assembly GetAssembly(this Type type)
	{
		return type.Assembly;
	}

	// Token: 0x0600167A RID: 5754 RVA: 0x0006A954 File Offset: 0x00068B54
	[HideInInspector]
	internal static bool SignalHierarchy(this GameObject target, string messageName, params object[] args)
	{
		while (target != null)
		{
			if (target.Signal(messageName, args))
			{
				return true;
			}
			if (target.transform.parent == null)
			{
				break;
			}
			target = target.transform.parent.gameObject;
		}
		return false;
	}

	// Token: 0x0600167B RID: 5755 RVA: 0x0006A9B0 File Offset: 0x00068BB0
	[HideInInspector]
	internal static bool Signal(this GameObject target, string messageName, params object[] args)
	{
		Component[] components = target.GetComponents(typeof(MonoBehaviour));
		Type[] array = new Type[args.Length];
		for (int i = 0; i < array.Length; i++)
		{
			if (args[i] == null)
			{
				array[i] = typeof(object);
			}
			else
			{
				array[i] = args[i].GetType();
			}
		}
		bool flag = false;
		foreach (Component component in components)
		{
			if (!(component == null) && component.GetType() != null)
			{
				if (!(component is MonoBehaviour) || ((MonoBehaviour)component).enabled)
				{
					MethodInfo method = dfReflectionExtensions.getMethod(component.GetType(), messageName, array);
					if (method != null)
					{
						IEnumerator enumerator = method.Invoke(component, args) as IEnumerator;
						if (enumerator != null)
						{
							((MonoBehaviour)component).StartCoroutine(enumerator);
						}
						flag = true;
					}
					else if (args.Length != 0)
					{
						MethodInfo method2 = dfReflectionExtensions.getMethod(component.GetType(), messageName, dfReflectionExtensions.EmptyTypes);
						if (method2 != null)
						{
							IEnumerator enumerator = method2.Invoke(component, null) as IEnumerator;
							if (enumerator != null)
							{
								((MonoBehaviour)component).StartCoroutine(enumerator);
							}
							flag = true;
						}
					}
				}
			}
		}
		return flag;
	}

	// Token: 0x0600167C RID: 5756 RVA: 0x0006AB0C File Offset: 0x00068D0C
	private static MethodInfo getMethod(Type type, string name, Type[] paramTypes)
	{
		return type.GetMethod(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, paramTypes, null);
	}

	// Token: 0x0600167D RID: 5757 RVA: 0x0006AB28 File Offset: 0x00068D28
	private static bool matchesParameterTypes(MethodInfo method, Type[] types)
	{
		ParameterInfo[] parameters = method.GetParameters();
		if (parameters.Length != types.Length)
		{
			return false;
		}
		for (int i = 0; i < types.Length; i++)
		{
			if (!parameters[i].ParameterType.IsAssignableFrom(types[i]))
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x0600167E RID: 5758 RVA: 0x0006AB78 File Offset: 0x00068D78
	internal static FieldInfo[] GetAllFields(this Type type)
	{
		if (type == null)
		{
			return new FieldInfo[0];
		}
		BindingFlags bindingFlags = BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
		return (from f in type.GetFields(bindingFlags).Concat(type.GetBaseType().GetAllFields())
			where !f.IsDefined(typeof(HideInInspector), true)
			select f).ToArray<FieldInfo>();
	}

	// Token: 0x04001294 RID: 4756
	public static Type[] EmptyTypes = new Type[0];
}
