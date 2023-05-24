using System;

namespace FullSerializer
{
	// Token: 0x020005A8 RID: 1448
	public interface fsISerializationCallbacks
	{
		// Token: 0x06002243 RID: 8771
		void OnBeforeSerialize(Type storageType);

		// Token: 0x06002244 RID: 8772
		void OnAfterSerialize(Type storageType, ref fsData data);

		// Token: 0x06002245 RID: 8773
		void OnBeforeDeserialize(Type storageType, ref fsData data);

		// Token: 0x06002246 RID: 8774
		void OnAfterDeserialize(Type storageType);
	}
}
