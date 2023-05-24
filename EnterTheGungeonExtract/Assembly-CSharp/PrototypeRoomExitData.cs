using System;
using System.Collections.Generic;
using Dungeonator;
using UnityEngine;

// Token: 0x02000F57 RID: 3927
[Serializable]
public class PrototypeRoomExitData
{
	// Token: 0x06005490 RID: 21648 RVA: 0x001FC1EC File Offset: 0x001FA3EC
	public void MirrorData(PrototypeRoomExitData source, IntVector2 sourceDimensions)
	{
		this.exits = new List<PrototypeRoomExit>();
		for (int i = 0; i < source.exits.Count; i++)
		{
			this.exits.Add(PrototypeRoomExit.CreateMirror(source.exits[i], sourceDimensions));
		}
	}

	// Token: 0x06005491 RID: 21649 RVA: 0x001FC240 File Offset: 0x001FA440
	public bool HasDefinedExitGroups()
	{
		return this.GetDefinedExitGroups().Count > 1;
	}

	// Token: 0x06005492 RID: 21650 RVA: 0x001FC250 File Offset: 0x001FA450
	public List<PrototypeRoomExit.ExitGroup> GetDefinedExitGroups()
	{
		List<PrototypeRoomExit.ExitGroup> list = new List<PrototypeRoomExit.ExitGroup>();
		for (int i = 0; i < this.exits.Count; i++)
		{
			if (!list.Contains(this.exits[i].exitGroup))
			{
				list.Add(this.exits[i].exitGroup);
			}
		}
		return list;
	}

	// Token: 0x06005493 RID: 21651 RVA: 0x001FC2B4 File Offset: 0x001FA4B4
	public bool ProcessExitPosition(int ix, int iy, PrototypeDungeonRoom parent)
	{
		if (this[ix + 1, iy + 1] == null)
		{
			this.AddExitPosition(ix, iy, parent);
			return true;
		}
		return this.RemoveExitPosition(ix, iy, parent);
	}

	// Token: 0x06005494 RID: 21652 RVA: 0x001FC2E8 File Offset: 0x001FA4E8
	public void AddExitPosition(int ix, int iy, PrototypeDungeonRoom parent)
	{
		IntVector2 intVector = new IntVector2(ix, iy);
		HashSet<IntVector2> hashSet = this.ExitToCellRepresentation();
		hashSet.Add(intVector + IntVector2.One);
		this.exits = this.CellToExitRepresentation(hashSet, parent);
		foreach (PrototypeDungeonRoomCellData prototypeDungeonRoomCellData in parent.FullCellData)
		{
			prototypeDungeonRoomCellData.conditionalOnParentExit = false;
			prototypeDungeonRoomCellData.parentExitIndex = -1;
		}
	}

	// Token: 0x06005495 RID: 21653 RVA: 0x001FC358 File Offset: 0x001FA558
	public bool RemoveExitPosition(int ix, int iy, PrototypeDungeonRoom parent)
	{
		IntVector2 intVector = new IntVector2(ix + 1, iy + 1);
		HashSet<IntVector2> hashSet = this.ExitToCellRepresentation();
		bool flag = hashSet.Remove(intVector);
		this.exits = this.CellToExitRepresentation(hashSet, parent);
		foreach (PrototypeDungeonRoomCellData prototypeDungeonRoomCellData in parent.FullCellData)
		{
			prototypeDungeonRoomCellData.conditionalOnParentExit = false;
			prototypeDungeonRoomCellData.parentExitIndex = -1;
		}
		return flag;
	}

	// Token: 0x06005496 RID: 21654 RVA: 0x001FC3C8 File Offset: 0x001FA5C8
	public void TranslateAllExits(int xOffset, int yOffset, PrototypeDungeonRoom parent)
	{
		IntVector2 intVector = new IntVector2(xOffset, yOffset);
		HashSet<IntVector2> hashSet = this.ExitToCellRepresentation();
		List<IntVector2> list = new List<IntVector2>(hashSet);
		for (int i = 0; i < list.Count; i++)
		{
			list[i] += intVector;
		}
		hashSet = new HashSet<IntVector2>(list);
		this.exits = this.CellToExitRepresentation(hashSet, parent);
	}

	// Token: 0x06005497 RID: 21655 RVA: 0x001FC42C File Offset: 0x001FA62C
	public void HandleRowColumnShift(int rowXCoord, int xShift, int columnYCoord, int yShift, PrototypeDungeonRoom parent)
	{
		IntVector2 intVector = new IntVector2(xShift, yShift);
		HashSet<IntVector2> hashSet = this.ExitToCellRepresentation();
		if (hashSet.Count == 0)
		{
			return;
		}
		List<IntVector2> list = new List<IntVector2>(hashSet);
		for (int i = list.Count - 1; i >= 0; i--)
		{
			IntVector2 intVector2 = list[i];
			if (intVector2.x > rowXCoord && intVector2.y > columnYCoord)
			{
				list[i] = intVector2 + intVector;
			}
			else if (intVector2.x == rowXCoord || intVector2.y == columnYCoord)
			{
				if (xShift == -1 || yShift == -1)
				{
					list.RemoveAt(i);
				}
				else
				{
					list[i] = intVector2 + intVector;
				}
			}
		}
		hashSet = new HashSet<IntVector2>(list);
		this.exits = this.CellToExitRepresentation(hashSet, parent);
	}

	// Token: 0x06005498 RID: 21656 RVA: 0x001FC514 File Offset: 0x001FA714
	private List<PrototypeRoomExit> CellToExitRepresentation(HashSet<IntVector2> cells, PrototypeDungeonRoom parent)
	{
		int num = parent.Width + 2;
		int num2 = parent.Height + 2;
		bool[,] array = new bool[num, num2];
		foreach (IntVector2 intVector in cells)
		{
			array[intVector.x, intVector.y] = true;
		}
		HashSet<IntVector2> hashSet = new HashSet<IntVector2>();
		List<PrototypeRoomExit> list = new List<PrototypeRoomExit>();
		for (int i = 0; i < num; i++)
		{
			for (int j = 0; j < num2; j++)
			{
				IntVector2 intVector2 = new IntVector2(i, j);
				if (!hashSet.Contains(intVector2))
				{
					hashSet.Add(intVector2);
					if (array[i, j])
					{
						DungeonData.Direction floorDirection = parent.GetFloorDirection(i - 1, j - 1);
						if (floorDirection == DungeonData.Direction.SOUTHWEST)
						{
							Debug.LogError("An exit was defined with no nearby floor tile. This is unsupported behavior.");
						}
						else
						{
							DungeonData.Direction direction = (floorDirection + 4) % (DungeonData.Direction)8;
							PrototypeRoomExit prototypeRoomExit = new PrototypeRoomExit(direction, intVector2.ToVector2());
							this.RecurseFindExits(array, intVector2 + IntVector2.Up, hashSet, prototypeRoomExit);
							this.RecurseFindExits(array, intVector2 + IntVector2.Right, hashSet, prototypeRoomExit);
							this.RecurseFindExits(array, intVector2 + IntVector2.Down, hashSet, prototypeRoomExit);
							this.RecurseFindExits(array, intVector2 + IntVector2.Left, hashSet, prototypeRoomExit);
							PrototypeRoomExit prototypeRoomExit2 = this.FindPreviouslyDefinedExit(prototypeRoomExit);
							if (prototypeRoomExit2 != null)
							{
								prototypeRoomExit.exitGroup = prototypeRoomExit2.exitGroup;
								prototypeRoomExit.exitType = prototypeRoomExit2.exitType;
								prototypeRoomExit.containsDoor = prototypeRoomExit2.containsDoor;
								prototypeRoomExit.specifiedDoor = prototypeRoomExit2.specifiedDoor;
							}
							list.Add(prototypeRoomExit);
						}
					}
				}
			}
		}
		return list;
	}

	// Token: 0x06005499 RID: 21657 RVA: 0x001FC6FC File Offset: 0x001FA8FC
	private PrototypeRoomExit FindPreviouslyDefinedExit(PrototypeRoomExit newExit)
	{
		for (int i = 0; i < newExit.containedCells.Count; i++)
		{
			for (int j = 0; j < this.exits.Count; j++)
			{
				if (this.exits[j].containedCells.Contains(newExit.containedCells[i]))
				{
					return this.exits[j];
				}
			}
		}
		return null;
	}

	// Token: 0x0600549A RID: 21658 RVA: 0x001FC778 File Offset: 0x001FA978
	private void RecurseFindExits(bool[,] exitMatrix, IntVector2 coords, HashSet<IntVector2> closedSet, PrototypeRoomExit currentExit)
	{
		if (coords.x < 0 || coords.y < 0 || coords.x >= exitMatrix.GetLength(0) || coords.y >= exitMatrix.GetLength(1))
		{
			return;
		}
		if (closedSet.Contains(coords))
		{
			return;
		}
		bool flag = exitMatrix[coords.x, coords.y];
		if (flag)
		{
			currentExit.containedCells.Add(coords.ToVector2());
			closedSet.Add(coords);
			this.RecurseFindExits(exitMatrix, coords + IntVector2.Up, closedSet, currentExit);
			this.RecurseFindExits(exitMatrix, coords + IntVector2.Right, closedSet, currentExit);
			this.RecurseFindExits(exitMatrix, coords + IntVector2.Down, closedSet, currentExit);
			this.RecurseFindExits(exitMatrix, coords + IntVector2.Left, closedSet, currentExit);
		}
		else
		{
			closedSet.Add(coords);
		}
	}

	// Token: 0x0600549B RID: 21659 RVA: 0x001FC86C File Offset: 0x001FAA6C
	private HashSet<IntVector2> ExitToCellRepresentation()
	{
		HashSet<IntVector2> hashSet = new HashSet<IntVector2>();
		for (int i = 0; i < this.exits.Count; i++)
		{
			PrototypeRoomExit prototypeRoomExit = this.exits[i];
			for (int j = 0; j < prototypeRoomExit.containedCells.Count; j++)
			{
				Vector2 vector = prototypeRoomExit.containedCells[j];
				IntVector2 intVector = new IntVector2(Mathf.RoundToInt(vector.x), Mathf.RoundToInt(vector.y));
				hashSet.Add(intVector);
			}
		}
		return hashSet;
	}

	// Token: 0x17000BDF RID: 3039
	public PrototypeRoomExit this[int xOffset, int yOffset]
	{
		get
		{
			if (this.exits == null)
			{
				return null;
			}
			IntVector2 intVector = new IntVector2(xOffset, yOffset);
			for (int i = this.exits.Count - 1; i >= 0; i--)
			{
				PrototypeRoomExit prototypeRoomExit = this.exits[i];
				if (prototypeRoomExit == null || prototypeRoomExit.containedCells == null)
				{
					this.exits.RemoveAt(i);
				}
				else if (prototypeRoomExit.containedCells.Contains(intVector.ToVector2()))
				{
					return prototypeRoomExit;
				}
			}
			return null;
		}
	}

	// Token: 0x0600549D RID: 21661 RVA: 0x001FC988 File Offset: 0x001FAB88
	public List<PrototypeRoomExit> GetUnusedExitsFromInstance(CellArea instance)
	{
		List<PrototypeRoomExit> list = new List<PrototypeRoomExit>();
		for (int i = 0; i < this.exits.Count; i++)
		{
			if (this.exits[i].exitType != PrototypeRoomExit.ExitType.ENTRANCE_ONLY)
			{
				if (!instance.instanceUsedExits.Contains(this.exits[i]))
				{
					list.Add(this.exits[i]);
				}
			}
		}
		return list;
	}

	// Token: 0x0600549E RID: 21662 RVA: 0x001FCA04 File Offset: 0x001FAC04
	public List<PrototypeRoomExit> GetUnusedExitsOnSide(DungeonData.Direction exitDir)
	{
		List<PrototypeRoomExit> list = new List<PrototypeRoomExit>();
		for (int i = 0; i < this.exits.Count; i++)
		{
			if (this.exits[i].exitDirection == exitDir)
			{
				list.Add(this.exits[i]);
			}
		}
		return list;
	}

	// Token: 0x04004D7F RID: 19839
	[SerializeField]
	public List<PrototypeRoomExit> exits;
}
