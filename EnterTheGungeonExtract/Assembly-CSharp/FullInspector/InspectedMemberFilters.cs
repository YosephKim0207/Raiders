using System;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using FullInspector.Internal;
using FullSerializer.Internal;
using UnityEngine;

namespace FullInspector
{
	// Token: 0x020005D3 RID: 1491
	public static class InspectedMemberFilters
	{
		// Token: 0x06002358 RID: 9048 RVA: 0x0009AD74 File Offset: 0x00098F74
		private static bool ShouldDisplayProperty(InspectedProperty property)
		{
			MemberInfo memberInfo = property.MemberInfo;
			return memberInfo.IsDefined(typeof(ShowInInspectorAttribute), true) || (!memberInfo.IsDefined(typeof(HideInInspector), true) && !memberInfo.IsDefined(typeof(NotSerializedAttribute), true) && !fiInstalledSerializerManager.SerializationOptOutAnnotations.Any((Type t) => memberInfo.IsDefined(t, true)) && ((!property.IsStatic && fiInstalledSerializerManager.SerializationOptInAnnotations.Any((Type t) => memberInfo.IsDefined(t, true))) || ((!(property.MemberInfo is PropertyInfo) || !fiSettings.InspectorRequireShowInInspector) && (typeof(BaseObject).Resolve().IsAssignableFrom(property.StorageType.Resolve()) || InspectedType.IsSerializedByFullInspector(property) || InspectedType.IsSerializedByUnity(property)))));
		}

		// Token: 0x06002359 RID: 9049 RVA: 0x0009AE80 File Offset: 0x00099080
		private static bool IsPropertyTypeInspectable(InspectedProperty property)
		{
			if (typeof(Delegate).IsAssignableFrom(property.StorageType))
			{
				return false;
			}
			if (property.MemberInfo is FieldInfo)
			{
				if (property.MemberInfo.IsDefined(typeof(CompilerGeneratedAttribute), false))
				{
					return false;
				}
			}
			else if (property.MemberInfo is PropertyInfo)
			{
				PropertyInfo propertyInfo = (PropertyInfo)property.MemberInfo;
				if (!propertyInfo.CanRead)
				{
					return false;
				}
				string @namespace = propertyInfo.DeclaringType.Namespace;
				if (@namespace != null && (@namespace.StartsWith("UnityEngine") || @namespace.StartsWith("UnityEditor")) && !propertyInfo.CanWrite)
				{
					return false;
				}
				if (propertyInfo.Name.EndsWith("Item"))
				{
					ParameterInfo[] indexParameters = propertyInfo.GetIndexParameters();
					if (indexParameters.Length > 0)
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x0400189C RID: 6300
		public static IInspectedMemberFilter All = new InspectedMemberFilters.AllFilter();

		// Token: 0x0400189D RID: 6301
		public static IInspectedMemberFilter FullInspectorSerializedProperties = new InspectedMemberFilters.FullInspectorSerializedPropertiesFilter();

		// Token: 0x0400189E RID: 6302
		public static IInspectedMemberFilter InspectableMembers = new InspectedMemberFilters.InspectableMembersFilter();

		// Token: 0x0400189F RID: 6303
		public static IInspectedMemberFilter StaticInspectableMembers = new InspectedMemberFilters.StaticInspectableMembersFilter();

		// Token: 0x040018A0 RID: 6304
		public static IInspectedMemberFilter ButtonMembers = new InspectedMemberFilters.ButtonMembersFilter();

		// Token: 0x020005D4 RID: 1492
		private class AllFilter : IInspectedMemberFilter
		{
			// Token: 0x0600235C RID: 9052 RVA: 0x0009AFA8 File Offset: 0x000991A8
			public bool IsInterested(InspectedProperty property)
			{
				return true;
			}

			// Token: 0x0600235D RID: 9053 RVA: 0x0009AFAC File Offset: 0x000991AC
			public bool IsInterested(InspectedMethod method)
			{
				return true;
			}
		}

		// Token: 0x020005D5 RID: 1493
		private class FullInspectorSerializedPropertiesFilter : IInspectedMemberFilter
		{
			// Token: 0x0600235F RID: 9055 RVA: 0x0009AFB8 File Offset: 0x000991B8
			public bool IsInterested(InspectedProperty property)
			{
				return property.CanWrite && InspectedType.IsSerializedByFullInspector(property) && !InspectedType.IsSerializedByUnity(property);
			}

			// Token: 0x06002360 RID: 9056 RVA: 0x0009AFDC File Offset: 0x000991DC
			public bool IsInterested(InspectedMethod method)
			{
				return false;
			}
		}

		// Token: 0x020005D6 RID: 1494
		private class InspectableMembersFilter : IInspectedMemberFilter
		{
			// Token: 0x06002362 RID: 9058 RVA: 0x0009AFE8 File Offset: 0x000991E8
			public bool IsInterested(InspectedProperty property)
			{
				return InspectedMemberFilters.IsPropertyTypeInspectable(property) && InspectedMemberFilters.ShouldDisplayProperty(property);
			}

			// Token: 0x06002363 RID: 9059 RVA: 0x0009B000 File Offset: 0x00099200
			public bool IsInterested(InspectedMethod method)
			{
				return method.Method.IsDefined(typeof(InspectorButtonAttribute), true);
			}
		}

		// Token: 0x020005D7 RID: 1495
		private class StaticInspectableMembersFilter : IInspectedMemberFilter
		{
			// Token: 0x06002365 RID: 9061 RVA: 0x0009B020 File Offset: 0x00099220
			public bool IsInterested(InspectedProperty property)
			{
				return property.IsStatic && InspectedMemberFilters.IsPropertyTypeInspectable(property);
			}

			// Token: 0x06002366 RID: 9062 RVA: 0x0009B038 File Offset: 0x00099238
			public bool IsInterested(InspectedMethod method)
			{
				return method.Method.IsDefined(typeof(InspectorButtonAttribute), true);
			}
		}

		// Token: 0x020005D8 RID: 1496
		private class ButtonMembersFilter : IInspectedMemberFilter
		{
			// Token: 0x06002368 RID: 9064 RVA: 0x0009B058 File Offset: 0x00099258
			public bool IsInterested(InspectedProperty property)
			{
				return false;
			}

			// Token: 0x06002369 RID: 9065 RVA: 0x0009B05C File Offset: 0x0009925C
			public bool IsInterested(InspectedMethod method)
			{
				return method.Method.IsDefined(typeof(InspectorButtonAttribute), true);
			}
		}
	}
}
