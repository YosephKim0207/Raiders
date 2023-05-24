using System;
using Dungeonator;
using UnityEngine;

// Token: 0x020011DA RID: 4570
public class PlacedWallDecorator : MonoBehaviour, IPlaceConfigurable
{
	// Token: 0x06006604 RID: 26116 RVA: 0x0027A508 File Offset: 0x00278708
	public void ConfigureOnPlacement(RoomHandler room)
	{
		IntVector2 intVector = base.transform.position.IntXY(VectorConversions.Floor);
		for (int i = this.wallClearanceXStart; i < this.wallClearanceWidth + this.wallClearanceXStart; i++)
		{
			for (int j = this.wallClearanceYStart; j < this.wallClearanceHeight + this.wallClearanceYStart; j++)
			{
				IntVector2 intVector2 = intVector + new IntVector2(i, j);
				CellData cellData = GameManager.Instance.Dungeon.data[intVector2];
				cellData.cellVisualData.containsObjectSpaceStamp = true;
				cellData.cellVisualData.containsWallSpaceStamp = true;
				cellData.cellVisualData.shouldIgnoreWallDrawing = this.ignoreWallDrawing;
				cellData.cellVisualData.shouldIgnoreBorders = this.ignoresBorders;
			}
		}
	}

	// Token: 0x040061C7 RID: 25031
	public int wallClearanceXStart;

	// Token: 0x040061C8 RID: 25032
	public int wallClearanceYStart = 1;

	// Token: 0x040061C9 RID: 25033
	public int wallClearanceWidth = 1;

	// Token: 0x040061CA RID: 25034
	public int wallClearanceHeight = 2;

	// Token: 0x040061CB RID: 25035
	public bool ignoreWallDrawing;

	// Token: 0x040061CC RID: 25036
	public bool ignoresBorders;
}
