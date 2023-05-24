using System;
using System.Collections.Generic;

namespace Dungeonator
{
	// Token: 0x02000EB3 RID: 3763
	public class CorridorExitData
	{
		// Token: 0x06004FA2 RID: 20386 RVA: 0x001BA8C0 File Offset: 0x001B8AC0
		public CorridorExitData(List<CellData> c, RoomHandler rh)
		{
			this.cells = c;
			this.linkedRoom = rh;
		}

		// Token: 0x04004772 RID: 18290
		public List<CellData> cells;

		// Token: 0x04004773 RID: 18291
		public RoomHandler linkedRoom;
	}
}
