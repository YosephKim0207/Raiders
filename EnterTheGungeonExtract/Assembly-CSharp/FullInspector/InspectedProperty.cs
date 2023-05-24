using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using FullInspector.Internal;
using FullSerializer;
using FullSerializer.Internal;
using UnityEngine;

namespace FullInspector
{
	// Token: 0x020005DB RID: 1499
	public sealed class InspectedProperty
	{
		// Token: 0x06002375 RID: 9077 RVA: 0x0009B220 File Offset: 0x00099420
		public InspectedProperty(PropertyInfo property)
		{
			this.MemberInfo = property;
			this.StorageType = property.PropertyType;
			this.CanWrite = property.GetSetMethod(true) != null;
			this.IsStatic = (property.GetGetMethod(true) ?? property.GetSetMethod(true)).IsStatic;
			this.SetupNames();
		}

		// Token: 0x06002376 RID: 9078 RVA: 0x0009B280 File Offset: 0x00099480
		public InspectedProperty(FieldInfo field)
		{
			this.MemberInfo = field;
			this.StorageType = field.FieldType;
			this.CanWrite = !field.IsLiteral;
			this.IsStatic = field.IsStatic;
			this.SetupNames();
		}

		// Token: 0x170006B3 RID: 1715
		// (get) Token: 0x06002377 RID: 9079 RVA: 0x0009B2BC File Offset: 0x000994BC
		// (set) Token: 0x06002378 RID: 9080 RVA: 0x0009B2C4 File Offset: 0x000994C4
		public MemberInfo MemberInfo { get; private set; }

		// Token: 0x170006B4 RID: 1716
		// (get) Token: 0x06002379 RID: 9081 RVA: 0x0009B2D0 File Offset: 0x000994D0
		public bool IsPublic
		{
			get
			{
				bool? isPublicCache = this._isPublicCache;
				if (isPublicCache == null)
				{
					FieldInfo fieldInfo = this.MemberInfo as FieldInfo;
					if (fieldInfo != null)
					{
						this._isPublicCache = new bool?(fieldInfo.IsPublic);
					}
					PropertyInfo propertyInfo = this.MemberInfo as PropertyInfo;
					if (propertyInfo != null)
					{
						this._isPublicCache = new bool?(propertyInfo.GetGetMethod(false) != null || propertyInfo.GetSetMethod(false) != null);
					}
					if (this._isPublicCache == null)
					{
						this._isPublicCache = new bool?(false);
					}
				}
				return this._isPublicCache.Value;
			}
		}

		// Token: 0x170006B5 RID: 1717
		// (get) Token: 0x0600237A RID: 9082 RVA: 0x0009B378 File Offset: 0x00099578
		public bool IsAutoProperty
		{
			get
			{
				bool? isAutoPropertyCache = this._isAutoPropertyCache;
				if (isAutoPropertyCache == null)
				{
					if (!(this.MemberInfo is PropertyInfo))
					{
						this._isAutoPropertyCache = new bool?(false);
					}
					else
					{
						string text = string.Format("<{0}>k__BackingField", this.MemberInfo.Name);
						bool flag = false;
						foreach (FieldInfo fieldInfo in this.MemberInfo.DeclaringType.GetDeclaredFields())
						{
							if (fsPortableReflection.HasAttribute<CompilerGeneratedAttribute>(fieldInfo) && fieldInfo.Name == text)
							{
								flag = true;
								break;
							}
						}
						this._isAutoPropertyCache = new bool?(flag);
					}
				}
				return this._isAutoPropertyCache.Value;
			}
		}

		// Token: 0x170006B6 RID: 1718
		// (get) Token: 0x0600237B RID: 9083 RVA: 0x0009B440 File Offset: 0x00099640
		// (set) Token: 0x0600237C RID: 9084 RVA: 0x0009B448 File Offset: 0x00099648
		public bool IsStatic { get; private set; }

		// Token: 0x170006B7 RID: 1719
		// (get) Token: 0x0600237D RID: 9085 RVA: 0x0009B454 File Offset: 0x00099654
		// (set) Token: 0x0600237E RID: 9086 RVA: 0x0009B45C File Offset: 0x0009965C
		public bool CanWrite { get; private set; }

		// Token: 0x0600237F RID: 9087 RVA: 0x0009B468 File Offset: 0x00099668
		public void Write(object context, object value)
		{
			try
			{
				FieldInfo fieldInfo = this.MemberInfo as FieldInfo;
				PropertyInfo propertyInfo = this.MemberInfo as PropertyInfo;
				if (fieldInfo != null)
				{
					if (!fieldInfo.IsLiteral)
					{
						fieldInfo.SetValue(context, value);
					}
				}
				else if (propertyInfo != null)
				{
					MethodInfo setMethod = propertyInfo.GetSetMethod(true);
					if (setMethod != null)
					{
						setMethod.Invoke(context, new object[] { value });
					}
				}
			}
			catch (Exception ex)
			{
				Debug.LogWarning(string.Concat(new object[] { "Caught exception when writing property ", this.Name, " with context=", context, " and value=", value }));
				Debug.LogException(ex);
			}
		}

		// Token: 0x06002380 RID: 9088 RVA: 0x0009B530 File Offset: 0x00099730
		public object Read(object context)
		{
			object obj;
			try
			{
				if (this.MemberInfo is PropertyInfo)
				{
					obj = ((PropertyInfo)this.MemberInfo).GetValue(context, new object[0]);
				}
				else
				{
					obj = ((FieldInfo)this.MemberInfo).GetValue(context);
				}
			}
			catch (Exception ex)
			{
				Debug.LogWarning(string.Concat(new object[]
				{
					"Caught exception when reading property ",
					this.Name,
					" with  context=",
					context,
					"; returning default value for ",
					this.StorageType.CSharpName()
				}));
				Debug.LogException(ex);
				obj = this.DefaultValue;
			}
			return obj;
		}

		// Token: 0x170006B8 RID: 1720
		// (get) Token: 0x06002381 RID: 9089 RVA: 0x0009B5E8 File Offset: 0x000997E8
		public object DefaultValue
		{
			get
			{
				if (this.StorageType.Resolve().IsValueType)
				{
					return InspectedType.Get(this.StorageType).CreateInstance();
				}
				return null;
			}
		}

		// Token: 0x06002382 RID: 9090 RVA: 0x0009B614 File Offset: 0x00099814
		private void SetupNames()
		{
			this.Name = this.MemberInfo.Name;
			InspectorNameAttribute attribute = fsPortableReflection.GetAttribute<InspectorNameAttribute>(this.MemberInfo);
			if (attribute != null)
			{
				this.DisplayName = attribute.DisplayName;
			}
			if (string.IsNullOrEmpty(this.DisplayName))
			{
				this.DisplayName = fiDisplayNameMapper.Map(this.Name);
			}
		}

		// Token: 0x06002383 RID: 9091 RVA: 0x0009B674 File Offset: 0x00099874
		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}
			InspectedProperty inspectedProperty = obj as InspectedProperty;
			return inspectedProperty != null && this.StorageType == inspectedProperty.StorageType && this.Name == inspectedProperty.Name;
		}

		// Token: 0x06002384 RID: 9092 RVA: 0x0009B6C0 File Offset: 0x000998C0
		public bool Equals(InspectedProperty p)
		{
			return p != null && this.StorageType == p.StorageType && this.Name == p.Name;
		}

		// Token: 0x06002385 RID: 9093 RVA: 0x0009B6F0 File Offset: 0x000998F0
		public override int GetHashCode()
		{
			return this.StorageType.GetHashCode() ^ this.Name.GetHashCode();
		}

		// Token: 0x040018A6 RID: 6310
		public string Name;

		// Token: 0x040018A7 RID: 6311
		public string DisplayName;

		// Token: 0x040018A8 RID: 6312
		private bool? _isPublicCache;

		// Token: 0x040018A9 RID: 6313
		private bool? _isAutoPropertyCache;

		// Token: 0x040018AC RID: 6316
		public Type StorageType;
	}
}
