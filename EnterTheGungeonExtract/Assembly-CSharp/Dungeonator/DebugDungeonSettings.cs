using System;
using UnityEngine;

namespace Dungeonator
{
	// Token: 0x02000EBC RID: 3772
	[Serializable]
	public class DebugDungeonSettings
	{
		// Token: 0x040047CC RID: 18380
		public bool RAPID_DEBUG_DUNGEON_ITERATION_SEEKER;

		// Token: 0x040047CD RID: 18381
		public bool RAPID_DEBUG_DUNGEON_ITERATION;

		// Token: 0x040047CE RID: 18382
		public int RAPID_DEBUG_DUNGEON_COUNT = 50;

		// Token: 0x040047CF RID: 18383
		public bool GENERATION_VIEWER_MODE;

		// Token: 0x040047D0 RID: 18384
		public bool FULL_MINIMAP_VISIBILITY;

		// Token: 0x040047D1 RID: 18385
		public bool COOP_TEST;

		// Token: 0x040047D2 RID: 18386
		[Header("Generation Options")]
		public bool DISABLE_ENEMIES;

		// Token: 0x040047D3 RID: 18387
		public bool DISABLE_LOOPS;

		// Token: 0x040047D4 RID: 18388
		public bool DISABLE_SECRET_ROOM_COVERS;

		// Token: 0x040047D5 RID: 18389
		public bool DISABLE_OUTLINES;

		// Token: 0x040047D6 RID: 18390
		public bool WALLS_ARE_PITS;
	}
}
