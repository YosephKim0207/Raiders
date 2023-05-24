using System;

namespace FullInspector.Internal
{
	// Token: 0x02000662 RID: 1634
	[Serializable]
	public class tkFoldoutMetadata : IGraphMetadataItemPersistent
	{
		// Token: 0x0600256E RID: 9582 RVA: 0x000A0C10 File Offset: 0x0009EE10
		bool IGraphMetadataItemPersistent.ShouldSerialize()
		{
			return !this.IsExpanded;
		}

		// Token: 0x0400199A RID: 6554
		public bool IsExpanded;
	}
}
