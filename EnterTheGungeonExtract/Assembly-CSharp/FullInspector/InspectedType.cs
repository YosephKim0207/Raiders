using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using FullInspector.Internal;
using FullSerializer;
using FullSerializer.Internal;
using UnityEngine;
using UnityEngine.Serialization;

namespace FullInspector
{
	// Token: 0x020005DC RID: 1500
	public sealed class InspectedType
	{
		// Token: 0x06002386 RID: 9094 RVA: 0x0009B70C File Offset: 0x0009990C
		static InspectedType()
		{
			InspectedType.InitializePropertyRemoval();
		}

		// Token: 0x06002387 RID: 9095 RVA: 0x0009B720 File Offset: 0x00099920
		internal InspectedType(Type type)
		{
			this.ReflectedType = type;
			this._isArray = type.IsArray;
			this.IsCollection = this._isArray || type.IsImplementationOf(typeof(ICollection<>));
			if (!this.IsCollection)
			{
				this._cachedMembers = new Dictionary<IInspectedMemberFilter, List<InspectedMember>>();
				this._cachedProperties = new Dictionary<IInspectedMemberFilter, List<InspectedProperty>>();
				this._cachedMethods = new Dictionary<IInspectedMemberFilter, List<InspectedMethod>>();
				this._allMembers = new List<InspectedMember>();
				if (this.ReflectedType.Resolve().BaseType != null)
				{
					InspectedType inspectedType = InspectedType.Get(this.ReflectedType.Resolve().BaseType);
					this._allMembers.AddRange(inspectedType._allMembers);
				}
				List<InspectedMember> list = InspectedType.CollectUnorderedLocalMembers(type).ToList<InspectedMember>();
				InspectedType.StableSort<InspectedMember>(list, (InspectedMember a, InspectedMember b) => Math.Sign(InspectorOrderAttribute.GetInspectorOrder(a.MemberInfo) - InspectorOrderAttribute.GetInspectorOrder(b.MemberInfo)));
				this._allMembers.AddRange(list);
				this._nameToProperty = new Dictionary<string, InspectedProperty>();
				this._formerlySerializedAsPropertyNames = new Dictionary<string, InspectedProperty>();
				foreach (InspectedMember inspectedMember in this._allMembers)
				{
					if (inspectedMember.IsProperty)
					{
						if (fiSettings.EmitWarnings && this._nameToProperty.ContainsKey(inspectedMember.Name))
						{
							Debug.LogWarning("Duplicate property with name=" + inspectedMember.Name + " detected on " + this.ReflectedType.CSharpName());
						}
						this._nameToProperty[inspectedMember.Name] = inspectedMember.Property;
						foreach (FormerlySerializedAsAttribute formerlySerializedAsAttribute in inspectedMember.MemberInfo.GetCustomAttributes(typeof(FormerlySerializedAsAttribute), true))
						{
							this._nameToProperty[formerlySerializedAsAttribute.oldName] = inspectedMember.Property;
						}
					}
				}
			}
		}

		// Token: 0x06002388 RID: 9096 RVA: 0x0009B94C File Offset: 0x00099B4C
		public static InspectedType Get(Type type)
		{
			InspectedType inspectedType;
			if (!InspectedType._cachedMetadata.TryGetValue(type, out inspectedType))
			{
				inspectedType = new InspectedType(type);
				InspectedType._cachedMetadata[type] = inspectedType;
			}
			return inspectedType;
		}

		// Token: 0x170006B9 RID: 1721
		// (get) Token: 0x06002389 RID: 9097 RVA: 0x0009B980 File Offset: 0x00099B80
		public bool HasDefaultConstructor
		{
			get
			{
				if (this._hasDefaultConstructorCache == null)
				{
					if (this._isArray)
					{
						this._hasDefaultConstructorCache = new bool?(true);
					}
					else if (this.ReflectedType.Resolve().IsValueType)
					{
						this._hasDefaultConstructorCache = new bool?(true);
					}
					else
					{
						ConstructorInfo declaredConstructor = this.ReflectedType.GetDeclaredConstructor(fsPortableReflection.EmptyTypes);
						this._hasDefaultConstructorCache = new bool?(declaredConstructor != null);
					}
				}
				return this._hasDefaultConstructorCache.Value;
			}
		}

		// Token: 0x0600238A RID: 9098 RVA: 0x0009BA10 File Offset: 0x00099C10
		public object CreateInstance()
		{
			if (typeof(ScriptableObject).IsAssignableFrom(this.ReflectedType))
			{
				return ScriptableObject.CreateInstance(this.ReflectedType);
			}
			if (typeof(Component).IsAssignableFrom(this.ReflectedType))
			{
				GameObject gameObject = fiLateBindings.Selection.activeObject as GameObject;
				if (!(gameObject != null))
				{
					Debug.LogWarning("No selected game object; constructing an unformatted instance (which will be null) for " + this.ReflectedType);
					return FormatterServices.GetSafeUninitializedObject(this.ReflectedType);
				}
				Component component = gameObject.GetComponent(this.ReflectedType);
				if (component != null)
				{
					return component;
				}
				return gameObject.AddComponent(this.ReflectedType);
			}
			else
			{
				if (!this.HasDefaultConstructor)
				{
					return FormatterServices.GetSafeUninitializedObject(this.ReflectedType);
				}
				if (this._isArray)
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
		}

		// Token: 0x0600238B RID: 9099 RVA: 0x0009BB84 File Offset: 0x00099D84
		public List<InspectedMember> GetMembers(IInspectedMemberFilter filter)
		{
			this.VerifyNotCollection();
			List<InspectedMember> list;
			if (!this._cachedMembers.TryGetValue(filter, out list))
			{
				list = new List<InspectedMember>();
				for (int i = 0; i < this._allMembers.Count; i++)
				{
					InspectedMember inspectedMember = this._allMembers[i];
					bool flag;
					if (inspectedMember.IsProperty)
					{
						flag = filter.IsInterested(inspectedMember.Property);
					}
					else
					{
						flag = filter.IsInterested(inspectedMember.Method);
					}
					if (flag)
					{
						list.Add(inspectedMember);
					}
				}
				this._cachedMembers[filter] = list;
			}
			return list;
		}

		// Token: 0x0600238C RID: 9100 RVA: 0x0009BC24 File Offset: 0x00099E24
		public List<InspectedProperty> GetProperties(IInspectedMemberFilter filter)
		{
			this.VerifyNotCollection();
			List<InspectedProperty> list;
			if (!this._cachedProperties.TryGetValue(filter, out list))
			{
				List<InspectedMember> members = this.GetMembers(filter);
				list = (from member in members
					where member.IsProperty
					select member.Property).ToList<InspectedProperty>();
				this._cachedProperties[filter] = list;
			}
			return list;
		}

		// Token: 0x0600238D RID: 9101 RVA: 0x0009BCAC File Offset: 0x00099EAC
		public List<InspectedMethod> GetMethods(IInspectedMemberFilter filter)
		{
			this.VerifyNotCollection();
			List<InspectedMethod> list;
			if (!this._cachedMethods.TryGetValue(filter, out list))
			{
				List<InspectedMember> members = this.GetMembers(filter);
				list = (from member in members
					where member.IsMethod
					select member.Method).ToList<InspectedMethod>();
				this._cachedMethods[filter] = list;
			}
			return list;
		}

		// Token: 0x0600238E RID: 9102 RVA: 0x0009BD34 File Offset: 0x00099F34
		private void VerifyNotCollection()
		{
			if (this.IsCollection)
			{
				throw new InvalidOperationException("Operation not valid -- " + this.ReflectedType + " is a collection");
			}
		}

		// Token: 0x0600238F RID: 9103 RVA: 0x0009BD5C File Offset: 0x00099F5C
		public static void StableSort<T>(IList<T> list, Func<T, T, int> comparator)
		{
			for (int i = 1; i < list.Count; i++)
			{
				T t = list[i];
				int num = i - 1;
				while (num >= 0 && comparator(list[num], t) > 0)
				{
					list[num + 1] = list[num];
					num--;
				}
				list[num + 1] = t;
			}
		}

		// Token: 0x06002390 RID: 9104 RVA: 0x0009BDCC File Offset: 0x00099FCC
		private static List<InspectedMember> CollectUnorderedLocalMembers(Type reflectedType)
		{
			List<InspectedMember> list = new List<InspectedMember>();
			foreach (MemberInfo memberInfo in reflectedType.GetDeclaredMembers())
			{
				PropertyInfo propertyInfo = memberInfo as PropertyInfo;
				FieldInfo fieldInfo = memberInfo as FieldInfo;
				if (propertyInfo != null)
				{
					MethodInfo getMethod = propertyInfo.GetGetMethod(true);
					MethodInfo setMethod = propertyInfo.GetSetMethod(true);
					if ((getMethod == null || getMethod == getMethod.GetBaseDefinition()) && (setMethod == null || setMethod == setMethod.GetBaseDefinition()))
					{
						list.Add(new InspectedMember(new InspectedProperty(propertyInfo)));
					}
				}
				else if (fieldInfo != null)
				{
					list.Add(new InspectedMember(new InspectedProperty(fieldInfo)));
				}
			}
			foreach (MethodInfo methodInfo in reflectedType.GetDeclaredMethods())
			{
				if (methodInfo == methodInfo.GetBaseDefinition())
				{
					list.Add(new InspectedMember(new InspectedMethod(methodInfo)));
				}
			}
			return list;
		}

		// Token: 0x170006BA RID: 1722
		// (get) Token: 0x06002391 RID: 9105 RVA: 0x0009BED8 File Offset: 0x0009A0D8
		// (set) Token: 0x06002392 RID: 9106 RVA: 0x0009BEE0 File Offset: 0x0009A0E0
		public Type ReflectedType { get; private set; }

		// Token: 0x170006BB RID: 1723
		// (get) Token: 0x06002393 RID: 9107 RVA: 0x0009BEEC File Offset: 0x0009A0EC
		// (set) Token: 0x06002394 RID: 9108 RVA: 0x0009BEF4 File Offset: 0x0009A0F4
		public bool IsCollection { get; private set; }

		// Token: 0x06002395 RID: 9109 RVA: 0x0009BF00 File Offset: 0x0009A100
		public Dictionary<string, List<InspectedMember>> GetCategories(IInspectedMemberFilter filter)
		{
			this.VerifyNotCollection();
			Dictionary<string, List<InspectedMember>> dictionary;
			if (!this._categoryCache.TryGetValue(filter, out dictionary))
			{
				dictionary = new Dictionary<string, List<InspectedMember>>();
				this._categoryCache[filter] = dictionary;
				foreach (InspectedMember inspectedMember in this.GetMembers(filter))
				{
					bool flag = false;
					foreach (InspectorCategoryAttribute inspectorCategoryAttribute in inspectedMember.MemberInfo.GetCustomAttributes(typeof(InspectorCategoryAttribute), true))
					{
						if (!dictionary.ContainsKey(inspectorCategoryAttribute.Category))
						{
							dictionary[inspectorCategoryAttribute.Category] = new List<InspectedMember>();
							if (inspectorCategoryAttribute.Category == "Conditions")
							{
								dictionary["Cond. (All)"] = new List<InspectedMember>();
							}
						}
						dictionary[inspectorCategoryAttribute.Category].Add(inspectedMember);
						if (inspectorCategoryAttribute.Category == "Conditions")
						{
							dictionary["Cond. (All)"].Add(inspectedMember);
						}
						flag = true;
					}
					if (!flag)
					{
						if (!dictionary.ContainsKey("Default"))
						{
							dictionary["Default"] = new List<InspectedMember>();
						}
						dictionary["Default"].Add(inspectedMember);
					}
				}
			}
			if (dictionary.Count == 1 && dictionary.ContainsKey("Default"))
			{
				dictionary.Clear();
			}
			return dictionary;
		}

		// Token: 0x06002396 RID: 9110 RVA: 0x0009C0B0 File Offset: 0x0009A2B0
		public InspectedProperty GetPropertyByName(string name)
		{
			this.VerifyNotCollection();
			InspectedProperty inspectedProperty;
			if (!this._nameToProperty.TryGetValue(name, out inspectedProperty))
			{
				return null;
			}
			return inspectedProperty;
		}

		// Token: 0x06002397 RID: 9111 RVA: 0x0009C0DC File Offset: 0x0009A2DC
		public InspectedProperty GetPropertyByFormerlySerializedName(string name)
		{
			this.VerifyNotCollection();
			InspectedProperty inspectedProperty;
			if (!this._formerlySerializedAsPropertyNames.TryGetValue(name, out inspectedProperty))
			{
				return null;
			}
			return inspectedProperty;
		}

		// Token: 0x06002398 RID: 9112 RVA: 0x0009C108 File Offset: 0x0009A308
		private static void InitializePropertyRemoval()
		{
			InspectedType.RemoveProperty<IntPtr>("m_value");
			InspectedType.RemoveProperty<UnityEngine.Object>("m_UnityRuntimeReferenceData");
			InspectedType.RemoveProperty<UnityEngine.Object>("m_UnityRuntimeErrorString");
			InspectedType.RemoveProperty<UnityEngine.Object>("name");
			InspectedType.RemoveProperty<UnityEngine.Object>("hideFlags");
			InspectedType.RemoveProperty<Component>("active");
			InspectedType.RemoveProperty<Component>("tag");
			InspectedType.RemoveProperty<Behaviour>("enabled");
			InspectedType.RemoveProperty<MonoBehaviour>("useGUILayout");
		}

		// Token: 0x06002399 RID: 9113 RVA: 0x0009C170 File Offset: 0x0009A370
		public static void RemoveProperty<T>(string propertyName)
		{
			InspectedType inspectedType = InspectedType.Get(typeof(T));
			inspectedType._nameToProperty.Remove(propertyName);
			inspectedType._cachedMembers = new Dictionary<IInspectedMemberFilter, List<InspectedMember>>();
			inspectedType._cachedMethods = new Dictionary<IInspectedMemberFilter, List<InspectedMethod>>();
			inspectedType._cachedProperties = new Dictionary<IInspectedMemberFilter, List<InspectedProperty>>();
			for (int i = 0; i < inspectedType._allMembers.Count; i++)
			{
				if (propertyName == inspectedType._allMembers[i].Name)
				{
					inspectedType._allMembers.RemoveAt(i);
					break;
				}
			}
		}

		// Token: 0x0600239A RID: 9114 RVA: 0x0009C208 File Offset: 0x0009A408
		private static bool IsSimpleTypeThatUnityCanSerialize(Type type)
		{
			return !InspectedType.IsPrimitiveSkippedByUnity(type) && (type.Resolve().IsPrimitive || type == typeof(string));
		}

		// Token: 0x0600239B RID: 9115 RVA: 0x0009C23C File Offset: 0x0009A43C
		private static bool IsPrimitiveSkippedByUnity(Type type)
		{
			return type == typeof(ushort) || type == typeof(uint) || type == typeof(ulong) || type == typeof(sbyte);
		}

		// Token: 0x0600239C RID: 9116 RVA: 0x0009C28C File Offset: 0x0009A48C
		public static bool IsSerializedByUnity(InspectedProperty property)
		{
			if (property.MemberInfo is PropertyInfo)
			{
				return false;
			}
			if (property.IsStatic)
			{
				return false;
			}
			FieldInfo fieldInfo = property.MemberInfo as FieldInfo;
			if (fieldInfo.IsInitOnly)
			{
				return false;
			}
			if (!property.IsPublic && !property.MemberInfo.IsDefined(typeof(SerializeField), true))
			{
				return false;
			}
			Type storageType = property.StorageType;
			return InspectedType.IsSimpleTypeThatUnityCanSerialize(storageType) || (typeof(UnityEngine.Object).IsAssignableFrom(storageType) && !storageType.Resolve().IsGenericType) || (storageType.IsArray && !storageType.GetElementType().IsArray && InspectedType.IsSimpleTypeThatUnityCanSerialize(storageType.GetElementType())) || (storageType.Resolve().IsGenericType && storageType.GetGenericTypeDefinition() == typeof(List<>) && InspectedType.IsSimpleTypeThatUnityCanSerialize(storageType.GetGenericArguments()[0]));
		}

		// Token: 0x0600239D RID: 9117 RVA: 0x0009C394 File Offset: 0x0009A594
		public static bool IsSerializedByFullInspector(InspectedProperty property)
		{
			if (property.IsStatic)
			{
				return false;
			}
			if (typeof(BaseObject).Resolve().IsAssignableFrom(property.StorageType.Resolve()))
			{
				return false;
			}
			MemberInfo memberInfo = property.MemberInfo;
			if (fsPortableReflection.HasAttribute<NonSerializedAttribute>(memberInfo) || fsPortableReflection.HasAttribute<NotSerializedAttribute>(memberInfo))
			{
				return false;
			}
			Type[] serializationOptOutAnnotations = fiInstalledSerializerManager.SerializationOptOutAnnotations;
			for (int i = 0; i < serializationOptOutAnnotations.Length; i++)
			{
				if (memberInfo.IsDefined(serializationOptOutAnnotations[i], true))
				{
					return false;
				}
			}
			if (fsPortableReflection.HasAttribute<SerializeField>(memberInfo) || fsPortableReflection.HasAttribute<SerializableAttribute>(memberInfo))
			{
				return true;
			}
			Type[] serializationOptInAnnotations = fiInstalledSerializerManager.SerializationOptInAnnotations;
			for (int j = 0; j < serializationOptInAnnotations.Length; j++)
			{
				if (memberInfo.IsDefined(serializationOptInAnnotations[j], true))
				{
					return true;
				}
			}
			if (property.MemberInfo is PropertyInfo)
			{
				if (!fiSettings.SerializeAutoProperties)
				{
					return false;
				}
				if (!property.IsAutoProperty)
				{
					return false;
				}
			}
			return property.IsPublic;
		}

		// Token: 0x040018AD RID: 6317
		private static Dictionary<Type, InspectedType> _cachedMetadata = new Dictionary<Type, InspectedType>();

		// Token: 0x040018AE RID: 6318
		private bool? _hasDefaultConstructorCache;

		// Token: 0x040018AF RID: 6319
		private List<InspectedMember> _allMembers;

		// Token: 0x040018B0 RID: 6320
		private Dictionary<IInspectedMemberFilter, List<InspectedMember>> _cachedMembers;

		// Token: 0x040018B1 RID: 6321
		private Dictionary<IInspectedMemberFilter, List<InspectedProperty>> _cachedProperties;

		// Token: 0x040018B2 RID: 6322
		private Dictionary<IInspectedMemberFilter, List<InspectedMethod>> _cachedMethods;

		// Token: 0x040018B5 RID: 6325
		private bool _isArray;

		// Token: 0x040018B6 RID: 6326
		private Dictionary<IInspectedMemberFilter, Dictionary<string, List<InspectedMember>>> _categoryCache = new Dictionary<IInspectedMemberFilter, Dictionary<string, List<InspectedMember>>>();

		// Token: 0x040018B7 RID: 6327
		private Dictionary<string, InspectedProperty> _nameToProperty;

		// Token: 0x040018B8 RID: 6328
		private Dictionary<string, InspectedProperty> _formerlySerializedAsPropertyNames;
	}
}
