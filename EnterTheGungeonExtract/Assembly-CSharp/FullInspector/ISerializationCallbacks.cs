using System;

namespace FullInspector
{
	// Token: 0x020005C7 RID: 1479
	public interface ISerializationCallbacks
	{
		// Token: 0x0600231D RID: 8989
		void OnBeforeSerialize();

		// Token: 0x0600231E RID: 8990
		void OnAfterSerialize();

		// Token: 0x0600231F RID: 8991
		void OnBeforeDeserialize();

		// Token: 0x06002320 RID: 8992
		void OnAfterDeserialize();
	}
}
