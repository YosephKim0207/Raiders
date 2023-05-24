using System;
using Dungeonator;
using Pathfinding;
using UnityEngine;

// Token: 0x02001270 RID: 4720
public class CursePotChallengeModifier : ChallengeModifier
{
	// Token: 0x060069BD RID: 27069 RVA: 0x00296D90 File Offset: 0x00294F90
	private void Start()
	{
		RoomHandler currentRoom = GameManager.Instance.PrimaryPlayer.CurrentRoom;
		int num = currentRoom.CellsWithoutExits.Count / this.RoughAreaPerCursePot;
		num = Mathf.Max(1, num);
		CellValidator cellValidator = delegate(IntVector2 pos)
		{
			for (int j = 0; j < GameManager.Instance.AllPlayers.Length; j++)
			{
				if (Vector2.Distance(GameManager.Instance.AllPlayers[j].CenterPosition, pos.ToCenterVector2()) < 8f)
				{
					return false;
				}
			}
			return true;
		};
		for (int i = 0; i < num; i++)
		{
			IntVector2? randomAvailableCell = currentRoom.GetRandomAvailableCell(new IntVector2?(IntVector2.One), new CellTypes?(CellTypes.FLOOR), false, cellValidator);
			if (randomAvailableCell != null)
			{
				CellData cellData = GameManager.Instance.Dungeon.data[randomAvailableCell.Value];
				if (cellData.parentRoom == currentRoom && cellData.type == CellType.FLOOR && !cellData.isOccupied && !cellData.containsTrap && !cellData.isOccludedByTopWall)
				{
					cellData.containsTrap = true;
					this.CursePot.InstantiateObject(currentRoom, cellData.position - currentRoom.area.basePosition, false, false);
				}
			}
		}
	}

	// Token: 0x04006636 RID: 26166
	public DungeonPlaceable CursePot;

	// Token: 0x04006637 RID: 26167
	public int RoughAreaPerCursePot = 50;
}
