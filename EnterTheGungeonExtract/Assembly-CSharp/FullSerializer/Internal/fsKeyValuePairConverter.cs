using System;
using System.Collections.Generic;
using System.Reflection;

namespace FullSerializer.Internal
{
	// Token: 0x0200058E RID: 1422
	public class fsKeyValuePairConverter : fsConverter
	{
		// Token: 0x060021B8 RID: 8632 RVA: 0x00094A10 File Offset: 0x00092C10
		public override bool CanProcess(Type type)
		{
			return type.Resolve().IsGenericType && type.GetGenericTypeDefinition() == typeof(KeyValuePair<, >);
		}

		// Token: 0x060021B9 RID: 8633 RVA: 0x00094A38 File Offset: 0x00092C38
		public override bool RequestCycleSupport(Type storageType)
		{
			return false;
		}

		// Token: 0x060021BA RID: 8634 RVA: 0x00094A3C File Offset: 0x00092C3C
		public override bool RequestInheritanceSupport(Type storageType)
		{
			return false;
		}

		// Token: 0x060021BB RID: 8635 RVA: 0x00094A40 File Offset: 0x00092C40
		public override fsResult TryDeserialize(fsData data, ref object instance, Type storageType)
		{
			fsResult fsResult = fsResult.Success;
			fsData fsData;
			fsResult fsResult2;
			fsResult = (fsResult2 = fsResult + base.CheckKey(data, "Key", out fsData));
			if (fsResult2.Failed)
			{
				return fsResult;
			}
			fsData fsData2;
			fsResult fsResult3;
			fsResult = (fsResult3 = fsResult + base.CheckKey(data, "Value", out fsData2));
			if (fsResult3.Failed)
			{
				return fsResult;
			}
			Type[] genericArguments = storageType.GetGenericArguments();
			Type type = genericArguments[0];
			Type type2 = genericArguments[1];
			object obj = null;
			object obj2 = null;
			fsResult.AddMessages(this.Serializer.TryDeserialize(fsData, type, ref obj));
			fsResult.AddMessages(this.Serializer.TryDeserialize(fsData2, type2, ref obj2));
			instance = Activator.CreateInstance(storageType, new object[] { obj, obj2 });
			return fsResult;
		}

		// Token: 0x060021BC RID: 8636 RVA: 0x00094B00 File Offset: 0x00092D00
		public override fsResult TrySerialize(object instance, out fsData serialized, Type storageType)
		{
			PropertyInfo declaredProperty = storageType.GetDeclaredProperty("Key");
			PropertyInfo declaredProperty2 = storageType.GetDeclaredProperty("Value");
			object value = declaredProperty.GetValue(instance, null);
			object value2 = declaredProperty2.GetValue(instance, null);
			Type[] genericArguments = storageType.GetGenericArguments();
			Type type = genericArguments[0];
			Type type2 = genericArguments[1];
			fsResult success = fsResult.Success;
			fsData fsData;
			success.AddMessages(this.Serializer.TrySerialize(type, value, out fsData));
			fsData fsData2;
			success.AddMessages(this.Serializer.TrySerialize(type2, value2, out fsData2));
			serialized = fsData.CreateDictionary();
			if (fsData != null)
			{
				serialized.AsDictionary["Key"] = fsData;
			}
			if (fsData2 != null)
			{
				serialized.AsDictionary["Value"] = fsData2;
			}
			return success;
		}
	}
}
