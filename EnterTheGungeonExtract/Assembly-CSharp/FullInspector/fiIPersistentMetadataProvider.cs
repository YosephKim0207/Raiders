using System;
using UnityEngine;

namespace FullInspector
{
	// Token: 0x02000617 RID: 1559
	public interface fiIPersistentMetadataProvider
	{
		// Token: 0x0600245D RID: 9309
		void RestoreData(UnityEngine.Object target);

		// Token: 0x0600245E RID: 9310
		void Reset(UnityEngine.Object target);

		// Token: 0x170006DE RID: 1758
		// (get) Token: 0x0600245F RID: 9311
		Type MetadataType { get; }
	}
}
