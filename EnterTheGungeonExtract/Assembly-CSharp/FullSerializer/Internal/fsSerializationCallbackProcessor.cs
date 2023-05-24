using System;

namespace FullSerializer.Internal
{
	// Token: 0x020005A9 RID: 1449
	public class fsSerializationCallbackProcessor : fsObjectProcessor
	{
		// Token: 0x06002248 RID: 8776 RVA: 0x000968F8 File Offset: 0x00094AF8
		public override bool CanProcess(Type type)
		{
			return typeof(fsISerializationCallbacks).IsAssignableFrom(type);
		}

		// Token: 0x06002249 RID: 8777 RVA: 0x0009690C File Offset: 0x00094B0C
		public override void OnBeforeSerialize(Type storageType, object instance)
		{
			((fsISerializationCallbacks)instance).OnBeforeSerialize(storageType);
		}

		// Token: 0x0600224A RID: 8778 RVA: 0x0009691C File Offset: 0x00094B1C
		public override void OnAfterSerialize(Type storageType, object instance, ref fsData data)
		{
			((fsISerializationCallbacks)instance).OnAfterSerialize(storageType, ref data);
		}

		// Token: 0x0600224B RID: 8779 RVA: 0x0009692C File Offset: 0x00094B2C
		public override void OnBeforeDeserializeAfterInstanceCreation(Type storageType, object instance, ref fsData data)
		{
			if (!(instance is fsISerializationCallbacks))
			{
				throw new InvalidCastException(string.Concat(new object[]
				{
					"Please ensure the converter for ",
					storageType,
					" actually returns an instance of it, not an instance of ",
					instance.GetType()
				}));
			}
			((fsISerializationCallbacks)instance).OnBeforeDeserialize(storageType, ref data);
		}

		// Token: 0x0600224C RID: 8780 RVA: 0x00096980 File Offset: 0x00094B80
		public override void OnAfterDeserialize(Type storageType, object instance)
		{
			((fsISerializationCallbacks)instance).OnAfterDeserialize(storageType);
		}
	}
}
