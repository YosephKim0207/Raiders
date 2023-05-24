using System;

namespace FullInspector
{
	// Token: 0x020005CF RID: 1487
	public interface fiISerializerMetadata
	{
		// Token: 0x170006A6 RID: 1702
		// (get) Token: 0x06002347 RID: 9031
		Guid SerializerGuid { get; }

		// Token: 0x170006A7 RID: 1703
		// (get) Token: 0x06002348 RID: 9032
		Type SerializerType { get; }

		// Token: 0x170006A8 RID: 1704
		// (get) Token: 0x06002349 RID: 9033
		Type[] SerializationOptInAnnotationTypes { get; }

		// Token: 0x170006A9 RID: 1705
		// (get) Token: 0x0600234A RID: 9034
		Type[] SerializationOptOutAnnotationTypes { get; }
	}
}
