using System;

// Token: 0x0200135F RID: 4959
[Serializable]
public class OverrideBossFloorEntry
{
	// Token: 0x06007067 RID: 28775 RVA: 0x002C94E8 File Offset: 0x002C76E8
	public bool GlobalPrereqsValid(GlobalDungeonData.ValidTilesets targetTileset)
	{
		if ((this.AssociatedTilesets | targetTileset) != this.AssociatedTilesets)
		{
			return false;
		}
		for (int i = 0; i < this.GlobalBossPrerequisites.Length; i++)
		{
			if (!this.GlobalBossPrerequisites[i].CheckConditionsFulfilled())
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x04006FDA RID: 28634
	public string Annotation;

	// Token: 0x04006FDB RID: 28635
	[EnumFlags]
	public GlobalDungeonData.ValidTilesets AssociatedTilesets;

	// Token: 0x04006FDC RID: 28636
	public DungeonPrerequisite[] GlobalBossPrerequisites;

	// Token: 0x04006FDD RID: 28637
	public float ChanceToOverride = 0.01f;

	// Token: 0x04006FDE RID: 28638
	public GenericRoomTable TargetRoomTable;
}
