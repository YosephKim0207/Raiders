using System;
using System.Collections.Generic;

namespace Dungeonator
{
	// Token: 0x02000F0F RID: 3855
	[Serializable]
	public class RoomCreationRule
	{
		// Token: 0x06005256 RID: 21078 RVA: 0x001D9264 File Offset: 0x001D7464
		public RoomCreationRule()
		{
			this.subrules = new List<Subrule>();
		}

		// Token: 0x04004AD1 RID: 19153
		public float percentChance;

		// Token: 0x04004AD2 RID: 19154
		public List<Subrule> subrules;

		// Token: 0x02000F10 RID: 3856
		public enum PlacementStrategy
		{
			// Token: 0x04004AD4 RID: 19156
			CENTERPIECE,
			// Token: 0x04004AD5 RID: 19157
			CORNERS,
			// Token: 0x04004AD6 RID: 19158
			WALLS,
			// Token: 0x04004AD7 RID: 19159
			BACK_WALL,
			// Token: 0x04004AD8 RID: 19160
			RANDOM_CENTER,
			// Token: 0x04004AD9 RID: 19161
			RANDOM
		}
	}
}
