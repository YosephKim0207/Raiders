using System;
using System.Collections;
using System.Collections.Generic;

namespace FullSerializer.Internal
{
	// Token: 0x02000588 RID: 1416
	public class fsArrayConverter : fsConverter
	{
		// Token: 0x0600218C RID: 8588 RVA: 0x00093AC4 File Offset: 0x00091CC4
		public override bool CanProcess(Type type)
		{
			return type.IsArray;
		}

		// Token: 0x0600218D RID: 8589 RVA: 0x00093ACC File Offset: 0x00091CCC
		public override bool RequestCycleSupport(Type storageType)
		{
			return false;
		}

		// Token: 0x0600218E RID: 8590 RVA: 0x00093AD0 File Offset: 0x00091CD0
		public override bool RequestInheritanceSupport(Type storageType)
		{
			return false;
		}

		// Token: 0x0600218F RID: 8591 RVA: 0x00093AD4 File Offset: 0x00091CD4
		public override fsResult TrySerialize(object instance, out fsData serialized, Type storageType)
		{
			IList list = (Array)instance;
			Type elementType = storageType.GetElementType();
			fsResult success = fsResult.Success;
			serialized = fsData.CreateList(list.Count);
			List<fsData> asList = serialized.AsList;
			for (int i = 0; i < list.Count; i++)
			{
				object obj = list[i];
				fsData fsData;
				fsResult fsResult = this.Serializer.TrySerialize(elementType, obj, out fsData);
				success.AddMessages(fsResult);
				if (!fsResult.Failed)
				{
					asList.Add(fsData);
				}
			}
			return success;
		}

		// Token: 0x06002190 RID: 8592 RVA: 0x00093B64 File Offset: 0x00091D64
		public override fsResult TryDeserialize(fsData data, ref object instance, Type storageType)
		{
			fsResult fsResult = fsResult.Success;
			fsResult fsResult2;
			fsResult = (fsResult2 = fsResult + base.CheckType(data, fsDataType.Array));
			if (fsResult2.Failed)
			{
				return fsResult;
			}
			Type elementType = storageType.GetElementType();
			List<fsData> asList = data.AsList;
			ArrayList arrayList = new ArrayList(asList.Count);
			int count = arrayList.Count;
			for (int i = 0; i < asList.Count; i++)
			{
				fsData fsData = asList[i];
				object obj = null;
				if (i < count)
				{
					obj = arrayList[i];
				}
				fsResult fsResult3 = this.Serializer.TryDeserialize(fsData, elementType, ref obj);
				fsResult.AddMessages(fsResult3);
				if (!fsResult3.Failed)
				{
					if (i < count)
					{
						arrayList[i] = obj;
					}
					else
					{
						arrayList.Add(obj);
					}
				}
			}
			instance = arrayList.ToArray(elementType);
			return fsResult;
		}

		// Token: 0x06002191 RID: 8593 RVA: 0x00093C50 File Offset: 0x00091E50
		public override object CreateInstance(fsData data, Type storageType)
		{
			return fsMetaType.Get(storageType).CreateInstance();
		}
	}
}
