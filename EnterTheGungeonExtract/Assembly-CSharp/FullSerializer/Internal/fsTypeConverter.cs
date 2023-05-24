using System;

namespace FullSerializer.Internal
{
	// Token: 0x02000592 RID: 1426
	public class fsTypeConverter : fsConverter
	{
		// Token: 0x060021D2 RID: 8658 RVA: 0x0009520C File Offset: 0x0009340C
		public override bool CanProcess(Type type)
		{
			return typeof(Type).IsAssignableFrom(type);
		}

		// Token: 0x060021D3 RID: 8659 RVA: 0x00095220 File Offset: 0x00093420
		public override bool RequestCycleSupport(Type type)
		{
			return false;
		}

		// Token: 0x060021D4 RID: 8660 RVA: 0x00095224 File Offset: 0x00093424
		public override bool RequestInheritanceSupport(Type type)
		{
			return false;
		}

		// Token: 0x060021D5 RID: 8661 RVA: 0x00095228 File Offset: 0x00093428
		public override fsResult TrySerialize(object instance, out fsData serialized, Type storageType)
		{
			Type type = (Type)instance;
			serialized = new fsData(type.FullName);
			return fsResult.Success;
		}

		// Token: 0x060021D6 RID: 8662 RVA: 0x00095250 File Offset: 0x00093450
		public override fsResult TryDeserialize(fsData data, ref object instance, Type storageType)
		{
			if (!data.IsString)
			{
				return fsResult.Fail("Type converter requires a string");
			}
			instance = fsTypeLookup.GetType(data.AsString);
			if (instance == null)
			{
				return fsResult.Fail("Unable to find type " + data.AsString);
			}
			return fsResult.Success;
		}

		// Token: 0x060021D7 RID: 8663 RVA: 0x000952A4 File Offset: 0x000934A4
		public override object CreateInstance(fsData data, Type storageType)
		{
			return storageType;
		}
	}
}
