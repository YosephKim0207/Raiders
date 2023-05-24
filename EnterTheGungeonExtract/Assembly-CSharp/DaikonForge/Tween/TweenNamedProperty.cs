using System;
using System.Linq;
using System.Reflection;
using DaikonForge.Tween.Interpolation;

namespace DaikonForge.Tween
{
	// Token: 0x0200051E RID: 1310
	public class TweenNamedProperty<T>
	{
		// Token: 0x06001F7A RID: 8058 RVA: 0x0008D80C File Offset: 0x0008BA0C
		public static Tween<T> Obtain(object target, string propertyName)
		{
			return TweenNamedProperty<T>.Obtain(target, propertyName, Interpolators.Get<T>());
		}

		// Token: 0x06001F7B RID: 8059 RVA: 0x0008D81C File Offset: 0x0008BA1C
		public static Tween<T> Obtain(object target, string propertyName, Interpolator<T> interpolator)
		{
			if (target == null)
			{
				throw new ArgumentException("Target object cannot be NULL");
			}
			Type type = target.GetType();
			MemberInfo member = TweenNamedProperty<T>.getMember(type, propertyName);
			if (member == null)
			{
				throw new ArgumentException(string.Format("Failed to find property {0}.{1}", type.Name, propertyName));
			}
			bool flag = false;
			if (member is FieldInfo)
			{
				if (((FieldInfo)member).FieldType != typeof(T))
				{
					flag = true;
				}
			}
			else if (((PropertyInfo)member).PropertyType != typeof(T))
			{
				flag = true;
			}
			if (flag)
			{
				throw new InvalidCastException(string.Format("{0}.{1} cannot be cast to type {2}", type.Name, member.Name, typeof(T).Name));
			}
			T t = TweenNamedProperty<T>.get(target, type, member);
			return Tween<T>.Obtain().SetStartValue(t).SetEndValue(t)
				.SetInterpolator(interpolator)
				.OnExecute(TweenNamedProperty<T>.set(target, type, member));
		}

		// Token: 0x06001F7C RID: 8060 RVA: 0x0008D914 File Offset: 0x0008BB14
		public static T GetCurrentValue(object target, string propertyName)
		{
			Type type = target.GetType();
			MemberInfo member = TweenNamedProperty<T>.getMember(type, propertyName);
			if (member == null)
			{
				throw new ArgumentException(string.Format("Failed to find property {0}.{1}", type.Name, propertyName));
			}
			return TweenNamedProperty<T>.get(target, type, member);
		}

		// Token: 0x06001F7D RID: 8061 RVA: 0x0008D958 File Offset: 0x0008BB58
		private static MethodInfo getGetMethod(PropertyInfo property)
		{
			return property.GetGetMethod();
		}

		// Token: 0x06001F7E RID: 8062 RVA: 0x0008D960 File Offset: 0x0008BB60
		private static MethodInfo getSetMethod(PropertyInfo property)
		{
			return property.GetSetMethod();
		}

		// Token: 0x06001F7F RID: 8063 RVA: 0x0008D968 File Offset: 0x0008BB68
		private static MemberInfo getMember(Type type, string propertyName)
		{
			return type.GetMember(propertyName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).FirstOrDefault<MemberInfo>();
		}

		// Token: 0x06001F80 RID: 8064 RVA: 0x0008D978 File Offset: 0x0008BB78
		private static T get(object target, Type type, MemberInfo member)
		{
			if (member is PropertyInfo)
			{
				PropertyInfo propertyInfo = (PropertyInfo)member;
				MethodInfo getMethod = TweenNamedProperty<T>.getGetMethod(propertyInfo);
				if (getMethod == null)
				{
					throw new ArgumentException(string.Format("Property {0}.{1} cannot be read", type.Name, member.Name));
				}
				return (T)((object)getMethod.Invoke(target, null));
			}
			else
			{
				if (member is FieldInfo)
				{
					FieldInfo fieldInfo = (FieldInfo)member;
					return (T)((object)fieldInfo.GetValue(target));
				}
				throw new ArgumentException(string.Format("Failed to find property {0}.{1}", type.Name, member.Name));
			}
		}

		// Token: 0x06001F81 RID: 8065 RVA: 0x0008DA08 File Offset: 0x0008BC08
		private static TweenAssignmentCallback<T> set(object target, Type type, MemberInfo member)
		{
			if (member is PropertyInfo)
			{
				return TweenNamedProperty<T>.setProperty(target, type, (PropertyInfo)member);
			}
			if (member is FieldInfo)
			{
				return TweenNamedProperty<T>.setField(target, type, (FieldInfo)member);
			}
			throw new ArgumentException(string.Format("Failed to find property {0}.{1}", type.Name, member.Name));
		}

		// Token: 0x06001F82 RID: 8066 RVA: 0x0008DA64 File Offset: 0x0008BC64
		private static TweenAssignmentCallback<T> setField(object target, Type type, FieldInfo field)
		{
			if (field.IsLiteral)
			{
				throw new ArgumentException(string.Format("Property {0}.{1} cannot be assigned", type.Name, field.Name));
			}
			return delegate(T result)
			{
				field.SetValue(target, result);
			};
		}

		// Token: 0x06001F83 RID: 8067 RVA: 0x0008DAC4 File Offset: 0x0008BCC4
		private static TweenAssignmentCallback<T> setProperty(object target, Type type, PropertyInfo property)
		{
			MethodInfo setter = TweenNamedProperty<T>.getSetMethod(property);
			if (setter == null)
			{
				throw new ArgumentException(string.Format("Property {0}.{1} cannot be assigned", type.Name, property.Name));
			}
			object[] paramArray = new object[1];
			return delegate(T result)
			{
				paramArray[0] = result;
				setter.Invoke(target, paramArray);
			};
		}
	}
}
