using System;
using System.Reflection;

namespace FullSerializer.Internal
{
	// Token: 0x020005BF RID: 1471
	public class fsMetaProperty
	{
		// Token: 0x060022F1 RID: 8945 RVA: 0x00099E68 File Offset: 0x00098068
		internal fsMetaProperty(FieldInfo field)
		{
			this._memberInfo = field;
			this.StorageType = field.FieldType;
			this.JsonName = fsMetaProperty.GetJsonName(field);
			this.JsonDeserializeOnly = fsMetaProperty.GetJsonDeserializeOnly(field);
			this.MemberName = field.Name;
			this.IsPublic = field.IsPublic;
			this.CanRead = true;
			this.CanWrite = true;
		}

		// Token: 0x060022F2 RID: 8946 RVA: 0x00099ECC File Offset: 0x000980CC
		internal fsMetaProperty(PropertyInfo property)
		{
			this._memberInfo = property;
			this.StorageType = property.PropertyType;
			this.JsonName = fsMetaProperty.GetJsonName(property);
			this.JsonDeserializeOnly = fsMetaProperty.GetJsonDeserializeOnly(property);
			this.MemberName = property.Name;
			this.IsPublic = property.GetGetMethod() != null && property.GetGetMethod().IsPublic && property.GetSetMethod() != null && property.GetSetMethod().IsPublic;
			this.CanRead = property.CanRead;
			this.CanWrite = property.CanWrite;
		}

		// Token: 0x17000691 RID: 1681
		// (get) Token: 0x060022F3 RID: 8947 RVA: 0x00099F6C File Offset: 0x0009816C
		// (set) Token: 0x060022F4 RID: 8948 RVA: 0x00099F74 File Offset: 0x00098174
		public Type StorageType { get; private set; }

		// Token: 0x17000692 RID: 1682
		// (get) Token: 0x060022F5 RID: 8949 RVA: 0x00099F80 File Offset: 0x00098180
		// (set) Token: 0x060022F6 RID: 8950 RVA: 0x00099F88 File Offset: 0x00098188
		public bool CanRead { get; private set; }

		// Token: 0x17000693 RID: 1683
		// (get) Token: 0x060022F7 RID: 8951 RVA: 0x00099F94 File Offset: 0x00098194
		// (set) Token: 0x060022F8 RID: 8952 RVA: 0x00099F9C File Offset: 0x0009819C
		public bool CanWrite { get; private set; }

		// Token: 0x17000694 RID: 1684
		// (get) Token: 0x060022F9 RID: 8953 RVA: 0x00099FA8 File Offset: 0x000981A8
		// (set) Token: 0x060022FA RID: 8954 RVA: 0x00099FB0 File Offset: 0x000981B0
		public string JsonName { get; private set; }

		// Token: 0x17000695 RID: 1685
		// (get) Token: 0x060022FB RID: 8955 RVA: 0x00099FBC File Offset: 0x000981BC
		// (set) Token: 0x060022FC RID: 8956 RVA: 0x00099FC4 File Offset: 0x000981C4
		public bool JsonDeserializeOnly { get; set; }

		// Token: 0x17000696 RID: 1686
		// (get) Token: 0x060022FD RID: 8957 RVA: 0x00099FD0 File Offset: 0x000981D0
		// (set) Token: 0x060022FE RID: 8958 RVA: 0x00099FD8 File Offset: 0x000981D8
		public string MemberName { get; private set; }

		// Token: 0x17000697 RID: 1687
		// (get) Token: 0x060022FF RID: 8959 RVA: 0x00099FE4 File Offset: 0x000981E4
		// (set) Token: 0x06002300 RID: 8960 RVA: 0x00099FEC File Offset: 0x000981EC
		public bool IsPublic { get; private set; }

		// Token: 0x06002301 RID: 8961 RVA: 0x00099FF8 File Offset: 0x000981F8
		public void Write(object context, object value)
		{
			FieldInfo fieldInfo = this._memberInfo as FieldInfo;
			PropertyInfo propertyInfo = this._memberInfo as PropertyInfo;
			if (fieldInfo != null)
			{
				fieldInfo.SetValue(context, value);
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

		// Token: 0x06002302 RID: 8962 RVA: 0x0009A058 File Offset: 0x00098258
		public object Read(object context)
		{
			if (this._memberInfo is PropertyInfo)
			{
				return ((PropertyInfo)this._memberInfo).GetValue(context, new object[0]);
			}
			return ((FieldInfo)this._memberInfo).GetValue(context);
		}

		// Token: 0x06002303 RID: 8963 RVA: 0x0009A094 File Offset: 0x00098294
		private static string GetJsonName(MemberInfo member)
		{
			fsPropertyAttribute attribute = fsPortableReflection.GetAttribute<fsPropertyAttribute>(member);
			if (attribute != null && !string.IsNullOrEmpty(attribute.Name))
			{
				return attribute.Name;
			}
			return member.Name;
		}

		// Token: 0x06002304 RID: 8964 RVA: 0x0009A0CC File Offset: 0x000982CC
		private static bool GetJsonDeserializeOnly(MemberInfo member)
		{
			fsPropertyAttribute attribute = fsPortableReflection.GetAttribute<fsPropertyAttribute>(member);
			return attribute != null && attribute.DeserializeOnly;
		}

		// Token: 0x04001883 RID: 6275
		private MemberInfo _memberInfo;
	}
}
