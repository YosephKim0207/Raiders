using System;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000CA1 RID: 3233
	[Serializable]
	public class MassiveSaveFlagEntry
	{
		// Token: 0x0400374A RID: 14154
		public GungeonFlags RequiredFlag;

		// Token: 0x0400374B RID: 14155
		public bool RequiredFlagState = true;

		// Token: 0x0400374C RID: 14156
		public GungeonFlags CompletedFlag;

		// Token: 0x0400374D RID: 14157
		public string mode;
	}
}
