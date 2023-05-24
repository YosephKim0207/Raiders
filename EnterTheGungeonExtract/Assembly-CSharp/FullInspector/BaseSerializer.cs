using System;
using System.Reflection;
using FullSerializer.Internal;

namespace FullInspector
{
	// Token: 0x02000543 RID: 1347
	public abstract class BaseSerializer
	{
		// Token: 0x0600200F RID: 8207
		public abstract string Serialize(MemberInfo storageType, object value, ISerializationOperator serializationOperator);

		// Token: 0x06002010 RID: 8208
		public abstract object Deserialize(MemberInfo storageType, string serializedState, ISerializationOperator serializationOperator);

		// Token: 0x17000636 RID: 1590
		// (get) Token: 0x06002011 RID: 8209 RVA: 0x0008EF24 File Offset: 0x0008D124
		public virtual bool SupportsMultithreading
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06002012 RID: 8210 RVA: 0x0008EF28 File Offset: 0x0008D128
		protected static Type GetStorageType(MemberInfo member)
		{
			if (member is FieldInfo)
			{
				return ((FieldInfo)member).FieldType;
			}
			if (member is PropertyInfo)
			{
				return ((PropertyInfo)member).PropertyType;
			}
			if (fsPortableReflection.IsType(member))
			{
				return fsPortableReflection.AsType(member);
			}
			throw new InvalidOperationException("Unknown member type " + member);
		}
	}
}
