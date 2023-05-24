using System;

namespace FullSerializer.Internal
{
	// Token: 0x02000590 RID: 1424
	public class fsPrimitiveConverter : fsConverter
	{
		// Token: 0x060021C3 RID: 8643 RVA: 0x00094C38 File Offset: 0x00092E38
		public override bool CanProcess(Type type)
		{
			return type.Resolve().IsPrimitive || type == typeof(string) || type == typeof(decimal);
		}

		// Token: 0x060021C4 RID: 8644 RVA: 0x00094C6C File Offset: 0x00092E6C
		public override bool RequestCycleSupport(Type storageType)
		{
			return false;
		}

		// Token: 0x060021C5 RID: 8645 RVA: 0x00094C70 File Offset: 0x00092E70
		public override bool RequestInheritanceSupport(Type storageType)
		{
			return false;
		}

		// Token: 0x060021C6 RID: 8646 RVA: 0x00094C74 File Offset: 0x00092E74
		private static bool UseBool(Type type)
		{
			return type == typeof(bool);
		}

		// Token: 0x060021C7 RID: 8647 RVA: 0x00094C84 File Offset: 0x00092E84
		private static bool UseInt64(Type type)
		{
			return type == typeof(sbyte) || type == typeof(byte) || type == typeof(short) || type == typeof(ushort) || type == typeof(int) || type == typeof(uint) || type == typeof(long) || type == typeof(ulong);
		}

		// Token: 0x060021C8 RID: 8648 RVA: 0x00094D14 File Offset: 0x00092F14
		private static bool UseDouble(Type type)
		{
			return type == typeof(float) || type == typeof(double) || type == typeof(decimal);
		}

		// Token: 0x060021C9 RID: 8649 RVA: 0x00094D48 File Offset: 0x00092F48
		private static bool UseString(Type type)
		{
			return type == typeof(string) || type == typeof(char);
		}

		// Token: 0x060021CA RID: 8650 RVA: 0x00094D6C File Offset: 0x00092F6C
		public override fsResult TrySerialize(object instance, out fsData serialized, Type storageType)
		{
			Type type = instance.GetType();
			if (fsConfig.Serialize64BitIntegerAsString && (type == typeof(long) || type == typeof(ulong)))
			{
				serialized = new fsData((string)Convert.ChangeType(instance, typeof(string)));
				return fsResult.Success;
			}
			if (fsPrimitiveConverter.UseBool(type))
			{
				serialized = new fsData((bool)instance);
				return fsResult.Success;
			}
			if (fsPrimitiveConverter.UseInt64(type))
			{
				serialized = new fsData((long)Convert.ChangeType(instance, typeof(long)));
				return fsResult.Success;
			}
			if (fsPrimitiveConverter.UseDouble(type))
			{
				serialized = new fsData((double)Convert.ChangeType(instance, typeof(double)));
				return fsResult.Success;
			}
			if (fsPrimitiveConverter.UseString(type))
			{
				serialized = new fsData((string)Convert.ChangeType(instance, typeof(string)));
				return fsResult.Success;
			}
			serialized = null;
			return fsResult.Fail("Unhandled primitive type " + instance.GetType());
		}

		// Token: 0x060021CB RID: 8651 RVA: 0x00094E8C File Offset: 0x0009308C
		public override fsResult TryDeserialize(fsData storage, ref object instance, Type storageType)
		{
			fsResult fsResult = fsResult.Success;
			if (fsPrimitiveConverter.UseBool(storageType))
			{
				fsResult fsResult2;
				fsResult = (fsResult2 = fsResult + base.CheckType(storage, fsDataType.Boolean));
				if (fsResult2.Succeeded)
				{
					instance = storage.AsBool;
				}
				return fsResult;
			}
			if (fsPrimitiveConverter.UseDouble(storageType) || fsPrimitiveConverter.UseInt64(storageType))
			{
				if (storage.IsDouble)
				{
					instance = Convert.ChangeType(storage.AsDouble, storageType);
				}
				else if (storage.IsInt64)
				{
					instance = Convert.ChangeType(storage.AsInt64, storageType);
				}
				else
				{
					if (!fsConfig.Serialize64BitIntegerAsString || !storage.IsString || (storageType != typeof(long) && storageType != typeof(ulong)))
					{
						return fsResult.Fail(string.Concat(new object[]
						{
							base.GetType().Name,
							" expected number but got ",
							storage.Type,
							" in ",
							storage
						}));
					}
					instance = Convert.ChangeType(storage.AsString, storageType);
				}
				return fsResult.Success;
			}
			if (fsPrimitiveConverter.UseString(storageType))
			{
				fsResult fsResult3;
				fsResult = (fsResult3 = fsResult + base.CheckType(storage, fsDataType.String));
				if (fsResult3.Succeeded)
				{
					instance = storage.AsString;
				}
				return fsResult;
			}
			return fsResult.Fail(base.GetType().Name + ": Bad data; expected bool, number, string, but got " + storage);
		}
	}
}
