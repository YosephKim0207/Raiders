using System;
using UnityEngine;

namespace FullInspector.Internal
{
	// Token: 0x02000613 RID: 1555
	public class fiGraphMetadataSerializer<TPersistentData> : fiIGraphMetadataStorage, ISerializationCallbackReceiver where TPersistentData : IGraphMetadataItemPersistent
	{
		// Token: 0x0600244F RID: 9295 RVA: 0x0009DF10 File Offset: 0x0009C110
		public void RestoreData(UnityEngine.Object target)
		{
			this._target = target;
			if (this._keys != null && this._values != null)
			{
				fiPersistentMetadata.GetMetadataFor(this._target).Deserialize<TPersistentData>(this._keys, this._values);
			}
		}

		// Token: 0x06002450 RID: 9296 RVA: 0x0009DF4C File Offset: 0x0009C14C
		void ISerializationCallbackReceiver.OnAfterDeserialize()
		{
		}

		// Token: 0x06002451 RID: 9297 RVA: 0x0009DF50 File Offset: 0x0009C150
		void ISerializationCallbackReceiver.OnBeforeSerialize()
		{
			if (this._target == null)
			{
				return;
			}
			fiGraphMetadata metadataFor = fiPersistentMetadata.GetMetadataFor(this._target);
			if (metadataFor.ShouldSerialize())
			{
				metadataFor.Serialize<TPersistentData>(out this._keys, out this._values);
			}
		}

		// Token: 0x04001921 RID: 6433
		[SerializeField]
		private string[] _keys;

		// Token: 0x04001922 RID: 6434
		[SerializeField]
		private TPersistentData[] _values;

		// Token: 0x04001923 RID: 6435
		[SerializeField]
		private UnityEngine.Object _target;
	}
}
