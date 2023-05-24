using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using FullSerializer.Internal;
using UnityEngine;

namespace FullSerializer
{
	// Token: 0x020005C0 RID: 1472
	public class fsMetaType
	{
		// Token: 0x06002305 RID: 8965 RVA: 0x0009A0F0 File Offset: 0x000982F0
		private fsMetaType(Type reflectedType)
		{
			this.ReflectedType = reflectedType;
			List<fsMetaProperty> list = new List<fsMetaProperty>();
			fsMetaType.CollectProperties(list, reflectedType);
			this.Properties = list.ToArray();
		}

		// Token: 0x06002306 RID: 8966 RVA: 0x0009A124 File Offset: 0x00098324
		public static fsMetaType Get(Type type)
		{
			fsMetaType fsMetaType;
			if (!fsMetaType._metaTypes.TryGetValue(type, out fsMetaType))
			{
				fsMetaType = new fsMetaType(type);
				fsMetaType._metaTypes[type] = fsMetaType;
			}
			return fsMetaType;
		}

		// Token: 0x06002307 RID: 8967 RVA: 0x0009A158 File Offset: 0x00098358
		public static void ClearCache()
		{
			fsMetaType._metaTypes = new Dictionary<Type, fsMetaType>();
		}

		// Token: 0x06002308 RID: 8968 RVA: 0x0009A164 File Offset: 0x00098364
		private static void CollectProperties(List<fsMetaProperty> properties, Type reflectedType)
		{
			bool flag = fsConfig.DefaultMemberSerialization == fsMemberSerialization.OptIn;
			bool flag2 = fsConfig.DefaultMemberSerialization == fsMemberSerialization.OptOut;
			fsObjectAttribute attribute = fsPortableReflection.GetAttribute<fsObjectAttribute>(reflectedType);
			if (attribute != null)
			{
				flag = attribute.MemberSerialization == fsMemberSerialization.OptIn;
				flag2 = attribute.MemberSerialization == fsMemberSerialization.OptOut;
			}
			MemberInfo[] declaredMembers = reflectedType.GetDeclaredMembers();
			MemberInfo[] array = declaredMembers;
			for (int i = 0; i < array.Length; i++)
			{
				MemberInfo member = array[i];
				if (!fsConfig.IgnoreSerializeAttributes.Any((Type t) => fsPortableReflection.HasAttribute(member, t)))
				{
					PropertyInfo propertyInfo = member as PropertyInfo;
					FieldInfo fieldInfo = member as FieldInfo;
					if (!flag || fsConfig.SerializeAttributes.Any((Type t) => fsPortableReflection.HasAttribute(member, t)))
					{
						if (!flag2 || !fsConfig.IgnoreSerializeAttributes.Any((Type t) => fsPortableReflection.HasAttribute(member, t)))
						{
							if (propertyInfo != null)
							{
								if (fsMetaType.CanSerializeProperty(propertyInfo, declaredMembers, flag2))
								{
									properties.Add(new fsMetaProperty(propertyInfo));
								}
							}
							else if (fieldInfo != null && fsMetaType.CanSerializeField(fieldInfo, flag2))
							{
								properties.Add(new fsMetaProperty(fieldInfo));
							}
						}
					}
				}
			}
			if (reflectedType.Resolve().BaseType != null)
			{
				fsMetaType.CollectProperties(properties, reflectedType.Resolve().BaseType);
			}
		}

		// Token: 0x06002309 RID: 8969 RVA: 0x0009A2D0 File Offset: 0x000984D0
		private static bool IsAutoProperty(PropertyInfo property, MemberInfo[] members)
		{
			if (!property.CanWrite || !property.CanRead)
			{
				return false;
			}
			string text = "<" + property.Name + ">k__BackingField";
			for (int i = 0; i < members.Length; i++)
			{
				if (members[i].Name == text)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600230A RID: 8970 RVA: 0x0009A338 File Offset: 0x00098538
		private static bool CanSerializeProperty(PropertyInfo property, MemberInfo[] members, bool annotationFreeValue)
		{
			if (typeof(Delegate).IsAssignableFrom(property.PropertyType))
			{
				return false;
			}
			MethodInfo getMethod = property.GetGetMethod(false);
			MethodInfo setMethod = property.GetSetMethod(false);
			return (getMethod == null || !getMethod.IsStatic) && (setMethod == null || !setMethod.IsStatic) && (fsConfig.SerializeAttributes.Any((Type t) => fsPortableReflection.HasAttribute(property, t)) || (property.CanRead && property.CanWrite && (((fsConfig.SerializeNonAutoProperties || fsMetaType.IsAutoProperty(property, members)) && getMethod != null && (fsConfig.SerializeNonPublicSetProperties || setMethod != null)) || annotationFreeValue)));
		}

		// Token: 0x0600230B RID: 8971 RVA: 0x0009A424 File Offset: 0x00098624
		private static bool CanSerializeField(FieldInfo field, bool annotationFreeValue)
		{
			return !typeof(Delegate).IsAssignableFrom(field.FieldType) && !field.IsDefined(typeof(CompilerGeneratedAttribute), false) && !field.IsStatic && (fsConfig.SerializeAttributes.Any((Type t) => fsPortableReflection.HasAttribute(field, t)) || annotationFreeValue || field.IsPublic);
		}

		// Token: 0x0600230C RID: 8972 RVA: 0x0009A4C4 File Offset: 0x000986C4
		public bool EmitAotData()
		{
			if (this._hasEmittedAotData)
			{
				return false;
			}
			this._hasEmittedAotData = true;
			for (int i = 0; i < this.Properties.Length; i++)
			{
				if (!this.Properties[i].IsPublic)
				{
					return false;
				}
			}
			if (!this.HasDefaultConstructor)
			{
				return false;
			}
			fsAotCompilationManager.AddAotCompilation(this.ReflectedType, this.Properties, this._isDefaultConstructorPublic);
			return true;
		}

		// Token: 0x17000698 RID: 1688
		// (get) Token: 0x0600230D RID: 8973 RVA: 0x0009A538 File Offset: 0x00098738
		// (set) Token: 0x0600230E RID: 8974 RVA: 0x0009A540 File Offset: 0x00098740
		public fsMetaProperty[] Properties { get; private set; }

		// Token: 0x17000699 RID: 1689
		// (get) Token: 0x0600230F RID: 8975 RVA: 0x0009A54C File Offset: 0x0009874C
		public bool HasDefaultConstructor
		{
			get
			{
				if (this._hasDefaultConstructorCache == null)
				{
					if (this.ReflectedType.Resolve().IsArray)
					{
						this._hasDefaultConstructorCache = new bool?(true);
						this._isDefaultConstructorPublic = true;
					}
					else if (this.ReflectedType.Resolve().IsValueType)
					{
						this._hasDefaultConstructorCache = new bool?(true);
						this._isDefaultConstructorPublic = true;
					}
					else
					{
						ConstructorInfo declaredConstructor = this.ReflectedType.GetDeclaredConstructor(fsPortableReflection.EmptyTypes);
						this._hasDefaultConstructorCache = new bool?(declaredConstructor != null);
						if (declaredConstructor != null)
						{
							this._isDefaultConstructorPublic = declaredConstructor.IsPublic;
						}
					}
				}
				return this._hasDefaultConstructorCache.Value;
			}
		}

		// Token: 0x06002310 RID: 8976 RVA: 0x0009A604 File Offset: 0x00098804
		public object CreateInstance()
		{
			if (this.ReflectedType.Resolve().IsInterface || this.ReflectedType.Resolve().IsAbstract)
			{
				throw new Exception("Cannot create an instance of an interface or abstract type for " + this.ReflectedType);
			}
			if (typeof(ScriptableObject).IsAssignableFrom(this.ReflectedType))
			{
				return ScriptableObject.CreateInstance(this.ReflectedType);
			}
			if (typeof(string) == this.ReflectedType)
			{
				return string.Empty;
			}
			if (!this.HasDefaultConstructor)
			{
				return FormatterServices.GetSafeUninitializedObject(this.ReflectedType);
			}
			if (this.ReflectedType.Resolve().IsArray)
			{
				return Array.CreateInstance(this.ReflectedType.GetElementType(), 0);
			}
			object obj;
			try
			{
				obj = Activator.CreateInstance(this.ReflectedType, true);
			}
			catch (MissingMethodException ex)
			{
				throw new InvalidOperationException("Unable to create instance of " + this.ReflectedType + "; there is no default constructor", ex);
			}
			catch (TargetInvocationException ex2)
			{
				throw new InvalidOperationException("Constructor of " + this.ReflectedType + " threw an exception when creating an instance", ex2);
			}
			catch (MemberAccessException ex3)
			{
				throw new InvalidOperationException("Unable to access constructor of " + this.ReflectedType, ex3);
			}
			return obj;
		}

		// Token: 0x0400188B RID: 6283
		private static Dictionary<Type, fsMetaType> _metaTypes = new Dictionary<Type, fsMetaType>();

		// Token: 0x0400188C RID: 6284
		public Type ReflectedType;

		// Token: 0x0400188D RID: 6285
		private bool _hasEmittedAotData;

		// Token: 0x0400188F RID: 6287
		private bool? _hasDefaultConstructorCache;

		// Token: 0x04001890 RID: 6288
		private bool _isDefaultConstructorPublic;
	}
}
