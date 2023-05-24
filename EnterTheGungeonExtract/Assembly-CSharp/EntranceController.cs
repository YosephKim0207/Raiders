using System;
using Dungeonator;
using UnityEngine;

// Token: 0x020010DC RID: 4316
public class EntranceController : DungeonPlaceableBehaviour, IPlaceConfigurable
{
	// Token: 0x06005F11 RID: 24337 RVA: 0x00248490 File Offset: 0x00246690
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
				cellData.cellVisualData.shouldIgnoreWallDrawing = true;
			}
		}
	}

	// Token: 0x06005F12 RID: 24338 RVA: 0x00248544 File Offset: 0x00246744
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x04005949 RID: 22857
	public int wallClearanceXStart;

	// Token: 0x0400594A RID: 22858
	public int wallClearanceYStart;

	// Token: 0x0400594B RID: 22859
	public int wallClearanceWidth = 4;

	// Token: 0x0400594C RID: 22860
	public int wallClearanceHeight = 2;

	// Token: 0x0400594D RID: 22861
	public Transform spawnTransform;
}
