using System;

// Token: 0x02000E6B RID: 3691
[Serializable]
public class DungeonFlowSubtypeRestriction
{
	// Token: 0x040044EE RID: 17646
	public PrototypeDungeonRoom.RoomCategory baseCategoryRestriction = PrototypeDungeonRoom.RoomCategory.NORMAL;

	// Token: 0x040044EF RID: 17647
	public PrototypeDungeonRoom.RoomNormalSubCategory normalSubcategoryRestriction;

	// Token: 0x040044F0 RID: 17648
	public PrototypeDungeonRoom.RoomBossSubCategory bossSubcategoryRestriction;

	// Token: 0x040044F1 RID: 17649
	public PrototypeDungeonRoom.RoomSpecialSubCategory specialSubcategoryRestriction;

	// Token: 0x040044F2 RID: 17650
	public PrototypeDungeonRoom.RoomSecretSubCategory secretSubcategoryRestriction;

	// Token: 0x040044F3 RID: 17651
	public int maximumRoomsOfSubtype = 1;
}
