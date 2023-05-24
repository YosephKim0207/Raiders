using System;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000C4D RID: 3149
	[Serializable]
	public class DesiredItem
	{
		// Token: 0x040035F8 RID: 13816
		public GungeonFlags flagToSet;

		// Token: 0x040035F9 RID: 13817
		public DesiredItem.DetectType type;

		// Token: 0x040035FA RID: 13818
		public int specificItemId;

		// Token: 0x040035FB RID: 13819
		public int amount;

		// Token: 0x02000C4E RID: 3150
		public enum DetectType
		{
			// Token: 0x040035FD RID: 13821
			SPECIFIC_ITEM,
			// Token: 0x040035FE RID: 13822
			CURRENCY,
			// Token: 0x040035FF RID: 13823
			META_CURRENCY,
			// Token: 0x04003600 RID: 13824
			KEYS
		}
	}
}
