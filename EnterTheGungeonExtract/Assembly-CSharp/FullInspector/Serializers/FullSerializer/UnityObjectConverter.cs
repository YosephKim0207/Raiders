using System;
using FullSerializer;
using FullSerializer.Internal;
using UnityEngine;

namespace FullInspector.Serializers.FullSerializer
{
	// Token: 0x02000585 RID: 1413
	public class UnityObjectConverter : fsConverter
	{
		// Token: 0x06002177 RID: 8567 RVA: 0x0009363C File Offset: 0x0009183C
		public override bool CanProcess(Type type)
		{
			return typeof(UnityEngine.Object).Resolve().IsAssignableFrom(type.Resolve());
		}

		// Token: 0x06002178 RID: 8568 RVA: 0x00093658 File Offset: 0x00091858
		public override bool RequestCycleSupport(Type storageType)
		{
			return false;
		}

		// Token: 0x06002179 RID: 8569 RVA: 0x0009365C File Offset: 0x0009185C
		public override bool RequestInheritanceSupport(Type storageType)
		{
			return false;
		}

		// Token: 0x0600217A RID: 8570 RVA: 0x00093660 File Offset: 0x00091860
		public override fsResult TrySerialize(object instance, out fsData serialized, Type storageType)
		{
			UnityEngine.Object @object = (UnityEngine.Object)instance;
			ISerializationOperator serializationOperator = this.Serializer.Context.Get<ISerializationOperator>();
			int num = serializationOperator.StoreObjectReference(@object);
			return this.Serializer.TrySerialize<int>(num, out serialized);
		}

		// Token: 0x0600217B RID: 8571 RVA: 0x0009369C File Offset: 0x0009189C
		public override fsResult TryDeserialize(fsData data, ref object instance, Type storageType)
		{
			ISerializationOperator serializationOperator = this.Serializer.Context.Get<ISerializationOperator>();
			int num = 0;
			fsResult fsResult = this.Serializer.TryDeserialize<int>(data, ref num);
			if (fsResult.Failed)
			{
				return fsResult;
			}
			instance = serializationOperator.RetrieveObjectReference(num);
			return fsResult.Success;
		}

		// Token: 0x0600217C RID: 8572 RVA: 0x000936E8 File Offset: 0x000918E8
		public override object CreateInstance(fsData data, Type storageType)
		{
			return storageType;
		}
	}
}
