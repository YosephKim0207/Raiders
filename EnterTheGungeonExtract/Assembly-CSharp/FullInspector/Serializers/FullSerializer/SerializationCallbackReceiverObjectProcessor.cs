using System;
using FullSerializer;
using FullSerializer.Internal;
using UnityEngine;

namespace FullInspector.Serializers.FullSerializer
{
	// Token: 0x02000584 RID: 1412
	public class SerializationCallbackReceiverObjectProcessor : fsObjectProcessor
	{
		// Token: 0x06002171 RID: 8561 RVA: 0x00093580 File Offset: 0x00091780
		public override bool CanProcess(Type type)
		{
			return !typeof(UnityEngine.Object).Resolve().IsAssignableFrom(type.Resolve()) && typeof(ISerializationCallbackReceiver).Resolve().IsAssignableFrom(type.Resolve()) && !typeof(BaseObject).Resolve().IsAssignableFrom(type.Resolve());
		}

		// Token: 0x06002172 RID: 8562 RVA: 0x000935EC File Offset: 0x000917EC
		public override void OnBeforeSerialize(Type storageType, object instance)
		{
			ISerializationCallbackReceiver serializationCallbackReceiver = (ISerializationCallbackReceiver)instance;
			if (serializationCallbackReceiver != null)
			{
				serializationCallbackReceiver.OnBeforeSerialize();
			}
		}

		// Token: 0x06002173 RID: 8563 RVA: 0x0009360C File Offset: 0x0009180C
		public override void OnAfterSerialize(Type storageType, object instance, ref fsData data)
		{
		}

		// Token: 0x06002174 RID: 8564 RVA: 0x00093610 File Offset: 0x00091810
		public override void OnBeforeDeserialize(Type storageType, ref fsData data)
		{
		}

		// Token: 0x06002175 RID: 8565 RVA: 0x00093614 File Offset: 0x00091814
		public override void OnAfterDeserialize(Type storageType, object instance)
		{
			ISerializationCallbackReceiver serializationCallbackReceiver = (ISerializationCallbackReceiver)instance;
			if (serializationCallbackReceiver != null)
			{
				serializationCallbackReceiver.OnAfterDeserialize();
			}
		}
	}
}
