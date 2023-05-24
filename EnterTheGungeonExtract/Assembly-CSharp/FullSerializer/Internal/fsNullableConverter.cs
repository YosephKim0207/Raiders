using System;

namespace FullSerializer.Internal
{
	// Token: 0x0200058F RID: 1423
	public class fsNullableConverter : fsConverter
	{
		// Token: 0x060021BE RID: 8638 RVA: 0x00094BD4 File Offset: 0x00092DD4
		public override bool CanProcess(Type type)
		{
			return type.Resolve().IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
		}

		// Token: 0x060021BF RID: 8639 RVA: 0x00094BFC File Offset: 0x00092DFC
		public override fsResult TrySerialize(object instance, out fsData serialized, Type storageType)
		{
			return this.Serializer.TrySerialize(Nullable.GetUnderlyingType(storageType), instance, out serialized);
		}

		// Token: 0x060021C0 RID: 8640 RVA: 0x00094C14 File Offset: 0x00092E14
		public override fsResult TryDeserialize(fsData data, ref object instance, Type storageType)
		{
			return this.Serializer.TryDeserialize(data, Nullable.GetUnderlyingType(storageType), ref instance);
		}

		// Token: 0x060021C1 RID: 8641 RVA: 0x00094C2C File Offset: 0x00092E2C
		public override object CreateInstance(fsData data, Type storageType)
		{
			return storageType;
		}
	}
}
