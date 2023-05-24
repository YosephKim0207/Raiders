using System;
using System.Collections;

namespace FullSerializer.Internal
{
	// Token: 0x02000591 RID: 1425
	public class fsReflectedConverter : fsConverter
	{
		// Token: 0x060021CD RID: 8653 RVA: 0x00095018 File Offset: 0x00093218
		public override bool CanProcess(Type type)
		{
			return !type.Resolve().IsArray && !typeof(ICollection).IsAssignableFrom(type);
		}

		// Token: 0x060021CE RID: 8654 RVA: 0x00095044 File Offset: 0x00093244
		public override fsResult TrySerialize(object instance, out fsData serialized, Type storageType)
		{
			serialized = fsData.CreateDictionary();
			fsResult success = fsResult.Success;
			fsMetaType fsMetaType = fsMetaType.Get(instance.GetType());
			fsMetaType.EmitAotData();
			for (int i = 0; i < fsMetaType.Properties.Length; i++)
			{
				fsMetaProperty fsMetaProperty = fsMetaType.Properties[i];
				if (fsMetaProperty.CanRead)
				{
					if (!fsMetaProperty.JsonDeserializeOnly)
					{
						fsData fsData;
						fsResult fsResult = this.Serializer.TrySerialize(fsMetaProperty.StorageType, fsMetaProperty.Read(instance), out fsData);
						success.AddMessages(fsResult);
						if (!fsResult.Failed)
						{
							serialized.AsDictionary[fsMetaProperty.JsonName] = fsData;
						}
					}
				}
			}
			return success;
		}

		// Token: 0x060021CF RID: 8655 RVA: 0x00095100 File Offset: 0x00093300
		public override fsResult TryDeserialize(fsData data, ref object instance, Type storageType)
		{
			fsResult fsResult = fsResult.Success;
			fsResult fsResult2;
			fsResult = (fsResult2 = fsResult + base.CheckType(data, fsDataType.Object));
			if (fsResult2.Failed)
			{
				return fsResult;
			}
			fsMetaType fsMetaType = fsMetaType.Get(storageType);
			fsMetaType.EmitAotData();
			for (int i = 0; i < fsMetaType.Properties.Length; i++)
			{
				fsMetaProperty fsMetaProperty = fsMetaType.Properties[i];
				if (fsMetaProperty.CanWrite)
				{
					fsData fsData;
					if (data.AsDictionary.TryGetValue(fsMetaProperty.JsonName, out fsData))
					{
						object obj = null;
						if (fsMetaProperty.CanRead)
						{
							obj = fsMetaProperty.Read(instance);
						}
						fsResult fsResult3 = this.Serializer.TryDeserialize(fsData, fsMetaProperty.StorageType, ref obj);
						fsResult.AddMessages(fsResult3);
						if (!fsResult3.Failed)
						{
							fsMetaProperty.Write(instance, obj);
						}
					}
				}
			}
			return fsResult;
		}

		// Token: 0x060021D0 RID: 8656 RVA: 0x000951E8 File Offset: 0x000933E8
		public override object CreateInstance(fsData data, Type storageType)
		{
			fsMetaType fsMetaType = fsMetaType.Get(storageType);
			return fsMetaType.CreateInstance();
		}
	}
}
