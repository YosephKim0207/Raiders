using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000F55 RID: 3925
[Serializable]
public class PrototypeRoomPitEntry
{
	// Token: 0x06005489 RID: 21641 RVA: 0x001FBF90 File Offset: 0x001FA190
	public PrototypeRoomPitEntry(IEnumerable<Vector2> cells)
	{
		this.containedCells = new List<Vector2>(cells);
	}

	// Token: 0x0600548A RID: 21642 RVA: 0x001FBFA4 File Offset: 0x001FA1A4
	public PrototypeRoomPitEntry(Vector2 cell)
	{
		this.containedCells = new List<Vector2>();
		this.containedCells.Add(cell);
	}

	// Token: 0x0600548B RID: 21643 RVA: 0x001FBFC4 File Offset: 0x001FA1C4
	public PrototypeRoomPitEntry CreateMirror(IntVector2 roomDimensions)
	{
		PrototypeRoomPitEntry prototypeRoomPitEntry = new PrototypeRoomPitEntry(Vector2.zero);
		prototypeRoomPitEntry.containedCells.Clear();
		prototypeRoomPitEntry.borderType = this.borderType;
		for (int i = 0; i < this.containedCells.Count; i++)
		{
			Vector2 vector = this.containedCells[i];
			vector.x = (float)roomDimensions.x - (vector.x + 1f);
			prototypeRoomPitEntry.containedCells.Add(vector);
		}
		return prototypeRoomPitEntry;
	}

	// Token: 0x0600548C RID: 21644 RVA: 0x001FC048 File Offset: 0x001FA248
	public bool IsAdjoining(Vector2 cell)
	{
		foreach (Vector2 vector in this.containedCells)
		{
			if (Mathf.Approximately(Vector2.Distance(cell, vector), 1f))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x0600548D RID: 21645 RVA: 0x001FC0C0 File Offset: 0x001FA2C0
	public bool IsAdjoining(IEnumerable<Vector2> cells)
	{
		foreach (Vector2 vector in cells)
		{
			foreach (Vector2 vector2 in this.containedCells)
			{
				if (Mathf.Approximately(Vector2.Distance(vector, vector2), 1f))
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x0600548E RID: 21646 RVA: 0x001FC174 File Offset: 0x001FA374
	public void AddCells(IEnumerable<Vector2> cells)
	{
		foreach (Vector2 vector in cells)
		{
			if (!this.containedCells.Contains(vector))
			{
				this.containedCells.Add(vector);
			}
		}
	}

	// Token: 0x04004D79 RID: 19833
	public List<Vector2> containedCells;

	// Token: 0x04004D7A RID: 19834
	public PrototypeRoomPitEntry.PitBorderType borderType;

	// Token: 0x02000F56 RID: 3926
	public enum PitBorderType
	{
		// Token: 0x04004D7C RID: 19836
		FLAT,
		// Token: 0x04004D7D RID: 19837
		RAISED,
		// Token: 0x04004D7E RID: 19838
		NONE
	}
}
