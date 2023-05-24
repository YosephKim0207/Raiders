using System;

namespace FullInspector.BackupService
{
	// Token: 0x020005F2 RID: 1522
	[Serializable]
	public class fiSerializedMember
	{
		// Token: 0x040018EB RID: 6379
		public string Name;

		// Token: 0x040018EC RID: 6380
		public string Value;

		// Token: 0x040018ED RID: 6381
		public fiEnableRestore ShouldRestore = new fiEnableRestore
		{
			Enabled = true
		};
	}
}
