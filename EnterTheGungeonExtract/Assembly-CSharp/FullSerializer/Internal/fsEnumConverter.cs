using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace FullSerializer.Internal
{
	// Token: 0x0200058B RID: 1419
	public class fsEnumConverter : fsConverter
	{
		// Token: 0x0600219F RID: 8607 RVA: 0x000943AC File Offset: 0x000925AC
		public override bool CanProcess(Type type)
		{
			return type.Resolve().IsEnum;
		}

		// Token: 0x060021A0 RID: 8608 RVA: 0x000943BC File Offset: 0x000925BC
		public override bool RequestCycleSupport(Type storageType)
		{
			return false;
		}

		// Token: 0x060021A1 RID: 8609 RVA: 0x000943C0 File Offset: 0x000925C0
		public override bool RequestInheritanceSupport(Type storageType)
		{
			return false;
		}

		// Token: 0x060021A2 RID: 8610 RVA: 0x000943C4 File Offset: 0x000925C4
		public override object CreateInstance(fsData data, Type storageType)
		{
			return Enum.ToObject(storageType, 0);
		}

		// Token: 0x060021A3 RID: 8611 RVA: 0x000943D4 File Offset: 0x000925D4
		public override fsResult TrySerialize(object instance, out fsData serialized, Type storageType)
		{
			if (fsConfig.SerializeEnumsAsInteger)
			{
				serialized = new fsData(Convert.ToInt64(instance));
			}
			else if (fsPortableReflection.GetAttribute<FlagsAttribute>(storageType) != null)
			{
				long num = Convert.ToInt64(instance);
				StringBuilder stringBuilder = new StringBuilder();
				bool flag = true;
				IEnumerator enumerator = Enum.GetValues(storageType).GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						object obj = enumerator.Current;
						int num2 = (int)obj;
						bool flag2 = (num & (long)num2) != 0L;
						if (flag2)
						{
							if (!flag)
							{
								stringBuilder.Append(",");
							}
							flag = false;
							stringBuilder.Append(obj.ToString());
						}
					}
				}
				finally
				{
					IDisposable disposable;
					if ((disposable = enumerator as IDisposable) != null)
					{
						disposable.Dispose();
					}
				}
				serialized = new fsData(stringBuilder.ToString());
			}
			else
			{
				serialized = new fsData(Enum.GetName(storageType, instance));
			}
			return fsResult.Success;
		}

		// Token: 0x060021A4 RID: 8612 RVA: 0x000944CC File Offset: 0x000926CC
		public override fsResult TryDeserialize(fsData data, ref object instance, Type storageType)
		{
			if (data.IsString)
			{
				string[] array = data.AsString.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
				long num = 0L;
				foreach (string text in array)
				{
					if (!fsEnumConverter.ArrayContains<string>(Enum.GetNames(storageType), text))
					{
						return fsResult.Fail(string.Concat(new object[] { "Cannot find enum name ", text, " on type ", storageType }));
					}
					long num2 = (long)Convert.ChangeType(Enum.Parse(storageType, text), typeof(long));
					num |= num2;
				}
				instance = Enum.ToObject(storageType, num);
				return fsResult.Success;
			}
			if (data.IsInt64)
			{
				int num3 = (int)data.AsInt64;
				instance = Enum.ToObject(storageType, num3);
				return fsResult.Success;
			}
			return fsResult.Fail("EnumConverter encountered an unknown JSON data type");
		}

		// Token: 0x060021A5 RID: 8613 RVA: 0x000945BC File Offset: 0x000927BC
		private static bool ArrayContains<T>(T[] values, T value)
		{
			for (int i = 0; i < values.Length; i++)
			{
				if (EqualityComparer<T>.Default.Equals(values[i], value))
				{
					return true;
				}
			}
			return false;
		}
	}
}
