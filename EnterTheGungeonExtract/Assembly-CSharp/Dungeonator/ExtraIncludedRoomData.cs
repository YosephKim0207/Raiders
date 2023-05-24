using System;
using UnityEngine;

namespace Dungeonator
{
	// Token: 0x02000F32 RID: 3890
	[Serializable]
	public class ExtraIncludedRoomData
	{
		// Token: 0x04004BD5 RID: 19413
		[SerializeField]
		public PrototypeDungeonRoom room;

		// Token: 0x04004BD6 RID: 19414
		[NonSerialized]
		public bool hasBeenProcessed;
	}
}
