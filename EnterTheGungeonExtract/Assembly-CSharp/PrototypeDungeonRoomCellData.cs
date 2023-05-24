using System;
using System.Collections.Generic;
using Dungeonator;

// Token: 0x02000F52 RID: 3922
[Serializable]
public class PrototypeDungeonRoomCellData
{
	// Token: 0x0600547A RID: 21626 RVA: 0x001FB8C0 File Offset: 0x001F9AC0
	public PrototypeDungeonRoomCellData()
	{
	}

	// Token: 0x0600547B RID: 21627 RVA: 0x001FB8E4 File Offset: 0x001F9AE4
	public PrototypeDungeonRoomCellData(string s, CellType st)
	{
		this.str = s;
		this.state = st;
	}

	// Token: 0x17000BDE RID: 3038
	// (get) Token: 0x0600547C RID: 21628 RVA: 0x001FB914 File Offset: 0x001F9B14
	public bool IsOccupied
	{
		get
		{
			return this.placedObjectRUBELIndex >= 0;
		}
	}

	// Token: 0x0600547D RID: 21629 RVA: 0x001FB928 File Offset: 0x001F9B28
	public bool HasChanges()
	{
		return this.diagonalWallType != DiagonalWallType.NONE || this.breakable || !string.IsNullOrEmpty(this.str) || this.conditionalOnParentExit || this.conditionalCellIsPit || this.parentExitIndex != -1 || this.containsManuallyPlacedLight || this.lightStampIndex != 0 || this.lightPixelsOffsetY != 0 || this.doesDamage || this.damageDefinition.HasChanges() || this.placedObjectRUBELIndex != -1 || this.additionalPlacedObjectIndices.Count != 0 || (this.appearance != null && this.appearance.HasChanges()) || this.ForceTileNonDecorated;
	}

	// Token: 0x0600547E RID: 21630 RVA: 0x001FB9FC File Offset: 0x001F9BFC
	public bool IsOccupiedAtLayer(int layerIndex)
	{
		return this.state == CellType.WALL || (this.additionalPlacedObjectIndices.Count > layerIndex && this.additionalPlacedObjectIndices[layerIndex] >= 0);
	}

	// Token: 0x0600547F RID: 21631 RVA: 0x001FBA34 File Offset: 0x001F9C34
	public void MirrorData(PrototypeDungeonRoomCellData source)
	{
		this.state = source.state;
		switch (source.diagonalWallType)
		{
		case DiagonalWallType.NONE:
			this.diagonalWallType = DiagonalWallType.NONE;
			break;
		case DiagonalWallType.NORTHEAST:
			this.diagonalWallType = DiagonalWallType.NORTHWEST;
			break;
		case DiagonalWallType.SOUTHEAST:
			this.diagonalWallType = DiagonalWallType.SOUTHWEST;
			break;
		case DiagonalWallType.SOUTHWEST:
			this.diagonalWallType = DiagonalWallType.SOUTHEAST;
			break;
		case DiagonalWallType.NORTHWEST:
			this.diagonalWallType = DiagonalWallType.NORTHEAST;
			break;
		}
		this.breakable = source.breakable;
		this.str = source.str;
		this.conditionalOnParentExit = source.conditionalOnParentExit;
		this.conditionalCellIsPit = source.conditionalCellIsPit;
		this.parentExitIndex = source.parentExitIndex;
		this.containsManuallyPlacedLight = source.containsManuallyPlacedLight;
		this.lightStampIndex = source.lightStampIndex;
		this.lightPixelsOffsetY = source.lightPixelsOffsetY;
		this.doesDamage = source.doesDamage;
		this.damageDefinition = source.damageDefinition;
		this.placedObjectRUBELIndex = source.placedObjectRUBELIndex;
		this.additionalPlacedObjectIndices = new List<int>();
		for (int i = 0; i < source.additionalPlacedObjectIndices.Count; i++)
		{
			this.additionalPlacedObjectIndices.Add(source.additionalPlacedObjectIndices[i]);
		}
		this.appearance = new PrototypeDungeonRoomCellAppearance();
		this.appearance.MirrorData(source.appearance);
		this.ForceTileNonDecorated = source.ForceTileNonDecorated;
	}

	// Token: 0x04004D62 RID: 19810
	public CellType state;

	// Token: 0x04004D63 RID: 19811
	public DiagonalWallType diagonalWallType;

	// Token: 0x04004D64 RID: 19812
	public bool breakable;

	// Token: 0x04004D65 RID: 19813
	public string str;

	// Token: 0x04004D66 RID: 19814
	public bool conditionalOnParentExit;

	// Token: 0x04004D67 RID: 19815
	public bool conditionalCellIsPit;

	// Token: 0x04004D68 RID: 19816
	public int parentExitIndex = -1;

	// Token: 0x04004D69 RID: 19817
	public bool containsManuallyPlacedLight;

	// Token: 0x04004D6A RID: 19818
	public int lightStampIndex;

	// Token: 0x04004D6B RID: 19819
	public int lightPixelsOffsetY;

	// Token: 0x04004D6C RID: 19820
	public bool doesDamage;

	// Token: 0x04004D6D RID: 19821
	public CellDamageDefinition damageDefinition;

	// Token: 0x04004D6E RID: 19822
	public int placedObjectRUBELIndex = -1;

	// Token: 0x04004D6F RID: 19823
	public List<int> additionalPlacedObjectIndices = new List<int>();

	// Token: 0x04004D70 RID: 19824
	public PrototypeDungeonRoomCellAppearance appearance;

	// Token: 0x04004D71 RID: 19825
	public bool ForceTileNonDecorated;
}
