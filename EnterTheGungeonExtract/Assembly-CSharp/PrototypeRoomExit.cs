using System;
using System.Collections.Generic;
using Dungeonator;
using UnityEngine;

// Token: 0x02000F59 RID: 3929
[Serializable]
public class PrototypeRoomExit
{
	// Token: 0x060054A3 RID: 21667 RVA: 0x001FCAAC File Offset: 0x001FACAC
	public PrototypeRoomExit(DungeonData.Direction d, Vector2 pos)
	{
		this.exitDirection = d;
		this.containedCells = new List<Vector2>();
		this.containedCells.Add(pos);
	}

	// Token: 0x060054A4 RID: 21668 RVA: 0x001FCAE0 File Offset: 0x001FACE0
	public IntVector2 GetHalfExitAttachPoint(int TotalExitLength)
	{
		Vector2 vector = new Vector2(float.MaxValue, float.MaxValue);
		for (int i = 0; i < this.containedCells.Count; i++)
		{
			Vector2 vector2 = this.containedCells[i];
			if (this.exitDirection == DungeonData.Direction.EAST || this.exitDirection == DungeonData.Direction.WEST)
			{
				if (vector2.y < vector.y)
				{
					vector = vector2;
				}
			}
			else if (vector2.x < vector.x)
			{
				vector = vector2;
			}
		}
		IntVector2 intVector = vector.ToIntVector2(VectorConversions.Round);
		if (this.exitDirection == DungeonData.Direction.SOUTH)
		{
			intVector += IntVector2.Down;
		}
		if (this.exitLength <= 2)
		{
			this.exitLength = 3;
		}
		return intVector + DungeonData.GetIntVector2FromDirection(this.exitDirection) * 2;
	}

	// Token: 0x060054A5 RID: 21669 RVA: 0x001FCBBC File Offset: 0x001FADBC
	public IntVector2 GetExitAttachPoint()
	{
		Vector2 vector = new Vector2(float.MaxValue, float.MaxValue);
		for (int i = 0; i < this.containedCells.Count; i++)
		{
			Vector2 vector2 = this.containedCells[i];
			if (this.exitDirection == DungeonData.Direction.EAST || this.exitDirection == DungeonData.Direction.WEST)
			{
				if (vector2.y < vector.y)
				{
					vector = vector2;
				}
			}
			else if (vector2.x < vector.x)
			{
				vector = vector2;
			}
		}
		IntVector2 intVector = vector.ToIntVector2(VectorConversions.Round);
		if (this.exitDirection == DungeonData.Direction.SOUTH)
		{
		}
		return intVector;
	}

	// Token: 0x060054A6 RID: 21670 RVA: 0x001FCC60 File Offset: 0x001FAE60
	public IntVector2 GetExitOrigin(int TotalExitLength)
	{
		Vector2 vector = new Vector2(float.MaxValue, float.MaxValue);
		for (int i = 0; i < this.containedCells.Count; i++)
		{
			Vector2 vector2 = this.containedCells[i];
			if (this.exitDirection == DungeonData.Direction.EAST || this.exitDirection == DungeonData.Direction.WEST)
			{
				if (vector2.y < vector.y)
				{
					vector = vector2;
				}
			}
			else if (vector2.x < vector.x)
			{
				vector = vector2;
			}
		}
		IntVector2 intVector = vector.ToIntVector2(VectorConversions.Round);
		if (this.exitDirection == DungeonData.Direction.SOUTH)
		{
			intVector += IntVector2.Down;
		}
		if (TotalExitLength <= 2)
		{
			this.exitLength = 3;
			TotalExitLength = 3;
		}
		return intVector + DungeonData.GetIntVector2FromDirection(this.exitDirection) * (TotalExitLength - 1);
	}

	// Token: 0x17000BE3 RID: 3043
	// (get) Token: 0x060054A7 RID: 21671 RVA: 0x001FCD3C File Offset: 0x001FAF3C
	public int ExitCellCount
	{
		get
		{
			return this.containedCells.Count;
		}
	}

	// Token: 0x060054A8 RID: 21672 RVA: 0x001FCD4C File Offset: 0x001FAF4C
	public static PrototypeRoomExit CreateMirror(PrototypeRoomExit source, IntVector2 sourceRoomDimensions)
	{
		PrototypeRoomExit prototypeRoomExit = new PrototypeRoomExit(source.exitDirection, Vector2.zero);
		prototypeRoomExit.containedCells.Clear();
		switch (source.exitDirection)
		{
		case DungeonData.Direction.NORTH:
			prototypeRoomExit.exitDirection = DungeonData.Direction.NORTH;
			goto IL_8B;
		case DungeonData.Direction.EAST:
			prototypeRoomExit.exitDirection = DungeonData.Direction.WEST;
			goto IL_8B;
		case DungeonData.Direction.SOUTH:
			prototypeRoomExit.exitDirection = DungeonData.Direction.SOUTH;
			goto IL_8B;
		case DungeonData.Direction.WEST:
			prototypeRoomExit.exitDirection = DungeonData.Direction.EAST;
			goto IL_8B;
		}
		prototypeRoomExit.exitDirection = source.exitDirection;
		IL_8B:
		prototypeRoomExit.exitType = source.exitType;
		prototypeRoomExit.exitGroup = source.exitGroup;
		prototypeRoomExit.containsDoor = source.containsDoor;
		prototypeRoomExit.specifiedDoor = source.specifiedDoor;
		prototypeRoomExit.exitLength = source.exitLength;
		for (int i = 0; i < source.containedCells.Count; i++)
		{
			Vector2 vector = source.containedCells[i];
			vector.x = (float)(sourceRoomDimensions.x + 2) - (vector.x + 1f);
			prototypeRoomExit.containedCells.Add(vector);
		}
		return prototypeRoomExit;
	}

	// Token: 0x04004D8A RID: 19850
	[SerializeField]
	public DungeonData.Direction exitDirection;

	// Token: 0x04004D8B RID: 19851
	[SerializeField]
	public PrototypeRoomExit.ExitType exitType;

	// Token: 0x04004D8C RID: 19852
	[SerializeField]
	public PrototypeRoomExit.ExitGroup exitGroup;

	// Token: 0x04004D8D RID: 19853
	[SerializeField]
	public bool containsDoor = true;

	// Token: 0x04004D8E RID: 19854
	[SerializeField]
	public DungeonPlaceable specifiedDoor;

	// Token: 0x04004D8F RID: 19855
	[SerializeField]
	public int exitLength = 1;

	// Token: 0x04004D90 RID: 19856
	[SerializeField]
	public List<Vector2> containedCells;

	// Token: 0x02000F5A RID: 3930
	public enum ExitType
	{
		// Token: 0x04004D92 RID: 19858
		NO_RESTRICTION,
		// Token: 0x04004D93 RID: 19859
		ENTRANCE_ONLY,
		// Token: 0x04004D94 RID: 19860
		EXIT_ONLY
	}

	// Token: 0x02000F5B RID: 3931
	public enum ExitGroup
	{
		// Token: 0x04004D96 RID: 19862
		A,
		// Token: 0x04004D97 RID: 19863
		B,
		// Token: 0x04004D98 RID: 19864
		C,
		// Token: 0x04004D99 RID: 19865
		D,
		// Token: 0x04004D9A RID: 19866
		E,
		// Token: 0x04004D9B RID: 19867
		F,
		// Token: 0x04004D9C RID: 19868
		G,
		// Token: 0x04004D9D RID: 19869
		H
	}
}
