using System;
using System.Collections.Generic;
using FullInspector.Internal;

namespace FullInspector.BackupService
{
	// Token: 0x020005F1 RID: 1521
	[Serializable]
	public class fiSerializedObject
	{
		// Token: 0x040018E5 RID: 6373
		public fiUnityObjectReference Target;

		// Token: 0x040018E6 RID: 6374
		public string SavedAt;

		// Token: 0x040018E7 RID: 6375
		public bool ShowDeserialized;

		// Token: 0x040018E8 RID: 6376
		public fiDeserializedObject DeserializedState;

		// Token: 0x040018E9 RID: 6377
		public List<fiSerializedMember> Members = new List<fiSerializedMember>();

		// Token: 0x040018EA RID: 6378
		public List<fiUnityObjectReference> ObjectReferences = new List<fiUnityObjectReference>();
	}
}
