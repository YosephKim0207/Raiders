using System;

namespace FullSerializer.Internal
{
	// Token: 0x0200058C RID: 1420
	public class fsGuidConverter : fsConverter
	{
		// Token: 0x060021A7 RID: 8615 RVA: 0x00094600 File Offset: 0x00092800
		public override bool CanProcess(Type type)
		{
			return type == typeof(Guid);
		}

		// Token: 0x060021A8 RID: 8616 RVA: 0x00094610 File Offset: 0x00092810
		public override bool RequestCycleSupport(Type storageType)
		{
			return false;
		}

		// Token: 0x060021A9 RID: 8617 RVA: 0x00094614 File Offset: 0x00092814
		public override bool RequestInheritanceSupport(Type storageType)
		{
			return false;
		}

		// Token: 0x060021AA RID: 8618 RVA: 0x00094618 File Offset: 0x00092818
		public override fsResult TrySerialize(object instance, out fsData serialized, Type storageType)
		{
			serialized = new fsData(((Guid)instance).ToString());
			return fsResult.Success;
		}

		// Token: 0x060021AB RID: 8619 RVA: 0x00094648 File Offset: 0x00092848
		public override fsResult TryDeserialize(fsData data, ref object instance, Type storageType)
		{
			if (data.IsString)
			{
				instance = new Guid(data.AsString);
				return fsResult.Success;
			}
			return fsResult.Fail("fsGuidConverter encountered an unknown JSON data type");
		}

		// Token: 0x060021AC RID: 8620 RVA: 0x00094678 File Offset: 0x00092878
		public override object CreateInstance(fsData data, Type storageType)
		{
			return default(Guid);
		}
	}
}
