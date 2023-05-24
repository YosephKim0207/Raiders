using System;
using System.Collections.Generic;

namespace Dungeonator
{
	// Token: 0x02000F0D RID: 3853
	[Serializable]
	public class RoomCreationStrategy
	{
		// Token: 0x06005255 RID: 21077 RVA: 0x001D9248 File Offset: 0x001D7448
		public RoomCreationStrategy()
		{
			this.rules = new List<RoomCreationRule>();
		}

		// Token: 0x04004AC5 RID: 19141
		public int minAreaSize;

		// Token: 0x04004AC6 RID: 19142
		public int maxAreaSize = 2;

		// Token: 0x04004AC7 RID: 19143
		public RoomCreationStrategy.RoomType roomType;

		// Token: 0x04004AC8 RID: 19144
		public List<RoomCreationRule> rules;

		// Token: 0x02000F0E RID: 3854
		public enum RoomType
		{
			// Token: 0x04004ACA RID: 19146
			SMOOTH_RECTILINEAR_ROOM,
			// Token: 0x04004ACB RID: 19147
			JAGGED_RECTILINEAR_ROOM,
			// Token: 0x04004ACC RID: 19148
			CIRCULAR_ROOM,
			// Token: 0x04004ACD RID: 19149
			SMOOTH_ANNEX,
			// Token: 0x04004ACE RID: 19150
			JAGGED_ANNEX,
			// Token: 0x04004ACF RID: 19151
			CAVE_ROOM,
			// Token: 0x04004AD0 RID: 19152
			PREDEFINED_ROOM
		}
	}
}
