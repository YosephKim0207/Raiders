using System;
using Dungeonator;
using UnityEngine.Serialization;

// Token: 0x0200115B RID: 4443
public class FloorTypeOverrideDoer : BraveBehaviour, IPlaceConfigurable
{
	// Token: 0x060062A4 RID: 25252 RVA: 0x00263C30 File Offset: 0x00261E30
	public void Start()
	{
		if (this.overrideMode == FloorTypeOverrideDoer.OverrideMode.Rigidbody)
		{
			this.DoFloorOverride(base.specRigidbody.UnitBottomLeft.ToIntVector2(VectorConversions.Round), base.specRigidbody.UnitTopRight.ToIntVector2(VectorConversions.Round) - IntVector2.One);
		}
	}

	// Token: 0x060062A5 RID: 25253 RVA: 0x00263C7C File Offset: 0x00261E7C
	public void ConfigureOnPlacement(RoomHandler room)
	{
		IntVector2 intVector = base.transform.position.IntXY(VectorConversions.Floor);
		this.DoFloorOverride(intVector + new IntVector2(this.xStartOffset, this.yStartOffset), intVector + new IntVector2(this.xStartOffset + this.width - 1, this.yStartOffset + this.height - 1));
	}

	// Token: 0x060062A6 RID: 25254 RVA: 0x00263CE4 File Offset: 0x00261EE4
	private void DoFloorOverride(IntVector2 lowerLeft, IntVector2 upperRight)
	{
		DungeonData data = GameManager.Instance.Dungeon.data;
		for (int i = lowerLeft.x; i <= upperRight.x; i++)
		{
			for (int j = lowerLeft.y; j <= upperRight.y; j++)
			{
				if (this.overrideCellFloorType)
				{
					data[i, j].cellVisualData.floorType = this.cellFloorType;
				}
				if (this.overrideTileIndex)
				{
					int num = Array.IndexOf<GlobalDungeonData.ValidTilesets>(this.TilesetsToOverrideFloorTile, GameManager.Instance.Dungeon.tileIndices.tilesetId);
					if (num >= 0)
					{
						int num2 = this.OverrideFloorTiles[num];
						data[i, j].cellVisualData.UsesCustomIndexOverride01 = true;
						data[i, j].cellVisualData.CustomIndexOverride01 = num2;
						data[i, j].cellVisualData.CustomIndexOverride01Layer = GlobalDungeonData.patternLayerIndex;
					}
				}
			}
		}
		if (this.preventsOtherFloorDecoration)
		{
			for (int k = lowerLeft.x - 1; k <= upperRight.x + 1; k++)
			{
				for (int l = lowerLeft.y - 1; l <= upperRight.y + 1; l++)
				{
					data[k, l].cellVisualData.floorTileOverridden = true;
					data[k, l].cellVisualData.preventFloorStamping = true;
					data[k, l].cellVisualData.containsObjectSpaceStamp = true;
					data[k, l].cellVisualData.containsWallSpaceStamp = !this.allowWallDecorationTho;
				}
			}
		}
	}

	// Token: 0x04005DA6 RID: 23974
	public FloorTypeOverrideDoer.OverrideMode overrideMode = FloorTypeOverrideDoer.OverrideMode.Placeable;

	// Token: 0x04005DA7 RID: 23975
	[ShowInInspectorIf("overrideMode", 0, true)]
	public int xStartOffset;

	// Token: 0x04005DA8 RID: 23976
	[ShowInInspectorIf("overrideMode", 0, true)]
	public int yStartOffset;

	// Token: 0x04005DA9 RID: 23977
	[ShowInInspectorIf("overrideMode", 0, true)]
	public int width = 1;

	// Token: 0x04005DAA RID: 23978
	[ShowInInspectorIf("overrideMode", 0, true)]
	public int height = 1;

	// Token: 0x04005DAB RID: 23979
	public bool overrideCellFloorType;

	// Token: 0x04005DAC RID: 23980
	[ShowInInspectorIf("overrideCellFloorType", true)]
	[FormerlySerializedAs("overrideType")]
	public CellVisualData.CellFloorType cellFloorType = CellVisualData.CellFloorType.Carpet;

	// Token: 0x04005DAD RID: 23981
	public bool overrideTileIndex;

	// Token: 0x04005DAE RID: 23982
	public GlobalDungeonData.ValidTilesets[] TilesetsToOverrideFloorTile;

	// Token: 0x04005DAF RID: 23983
	public int[] OverrideFloorTiles;

	// Token: 0x04005DB0 RID: 23984
	public bool preventsOtherFloorDecoration = true;

	// Token: 0x04005DB1 RID: 23985
	public bool allowWallDecorationTho;

	// Token: 0x0200115C RID: 4444
	public enum OverrideMode
	{
		// Token: 0x04005DB3 RID: 23987
		Placeable = 10,
		// Token: 0x04005DB4 RID: 23988
		Rigidbody = 20
	}
}
