using System;

namespace Dungeonator
{
	// Token: 0x02000EF3 RID: 3827
	[Serializable]
	public class DungeonPlaceableRoomMaterialRequirement
	{
		// Token: 0x0400498E RID: 18830
		public GlobalDungeonData.ValidTilesets TargetTileset = GlobalDungeonData.ValidTilesets.CASTLEGEON;

		// Token: 0x0400498F RID: 18831
		[PrettyDungeonMaterial("TargetTileset")]
		public int RoomMaterial;

		// Token: 0x04004990 RID: 18832
		public bool RequireMaterial = true;
	}
}
