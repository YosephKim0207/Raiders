using System;

namespace FullInspector
{
	// Token: 0x020005EC RID: 1516
	[Flags]
	public enum VerifyPrefabTypeFlags
	{
		// Token: 0x040018D7 RID: 6359
		None = 1,
		// Token: 0x040018D8 RID: 6360
		Prefab = 2,
		// Token: 0x040018D9 RID: 6361
		ModelPrefab = 4,
		// Token: 0x040018DA RID: 6362
		PrefabInstance = 8,
		// Token: 0x040018DB RID: 6363
		ModelPrefabInstance = 16,
		// Token: 0x040018DC RID: 6364
		MissingPrefabInstance = 32,
		// Token: 0x040018DD RID: 6365
		DisconnectedPrefabInstance = 64,
		// Token: 0x040018DE RID: 6366
		DisconnectedModelPrefabInstance = 128
	}
}
