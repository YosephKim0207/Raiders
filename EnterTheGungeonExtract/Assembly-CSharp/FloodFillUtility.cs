using System;
using System.Collections.Generic;
using Dungeonator;

// Token: 0x020012DD RID: 4829
public class FloodFillUtility
{
	// Token: 0x06006C3B RID: 27707 RVA: 0x002A98DC File Offset: 0x002A7ADC
	public static void PreprocessContiguousCells(RoomHandler room, IntVector2 myPos, int bufferCells = 0)
	{
		DungeonData data = GameManager.Instance.Dungeon.data;
		FloodFillUtility.m_areaMin = room.area.basePosition - new IntVector2(bufferCells, bufferCells);
		FloodFillUtility.m_areaDim = room.area.dimensions + new IntVector2(2 * bufferCells, 2 * bufferCells);
		int num = FloodFillUtility.m_areaDim.x * FloodFillUtility.m_areaDim.y;
		if (FloodFillUtility.s_reachable.Length < num)
		{
			FloodFillUtility.s_reachable = new bool[num];
		}
		for (int i = 0; i < num; i++)
		{
			FloodFillUtility.s_reachable[i] = false;
		}
		FloodFillUtility.s_openList.Clear();
		if (data.GetCellTypeSafe(myPos) == CellType.FLOOR)
		{
			int num2 = myPos.x - FloodFillUtility.m_areaMin.x + (myPos.y - FloodFillUtility.m_areaMin.y) * FloodFillUtility.m_areaDim.x;
			FloodFillUtility.s_openList.Add(num2);
			FloodFillUtility.s_reachable[num2] = true;
		}
		int num3 = 0;
		while (FloodFillUtility.s_openList.Count > 0 && num3 < 1000)
		{
			int num4 = FloodFillUtility.s_openList[0];
			int num5 = FloodFillUtility.s_openList[0] % FloodFillUtility.m_areaDim.x;
			int num6 = FloodFillUtility.s_openList[0] / FloodFillUtility.m_areaDim.x;
			int num7 = FloodFillUtility.m_areaMin.x + num5;
			int num8 = FloodFillUtility.m_areaMin.y + num6;
			FloodFillUtility.s_openList.RemoveAt(0);
			int num9 = -1;
			if (num5 > 0 && data.GetCellTypeSafe(num7 - 1, num8) == CellType.FLOOR && !FloodFillUtility.s_reachable[num4 + num9])
			{
				FloodFillUtility.s_reachable[num4 + num9] = true;
				FloodFillUtility.s_openList.Add(num4 + num9);
			}
			num9 = 1;
			if (num5 < FloodFillUtility.m_areaDim.x - 1 && data.GetCellTypeSafe(num7 + 1, num8) == CellType.FLOOR && !FloodFillUtility.s_reachable[num4 + num9])
			{
				FloodFillUtility.s_reachable[num4 + num9] = true;
				FloodFillUtility.s_openList.Add(num4 + num9);
			}
			num9 = -FloodFillUtility.m_areaDim.x;
			if (num6 > 0 && data.GetCellTypeSafe(num7, num8 - 1) == CellType.FLOOR && !FloodFillUtility.s_reachable[num4 + num9])
			{
				FloodFillUtility.s_reachable[num4 + num9] = true;
				FloodFillUtility.s_openList.Add(num4 + num9);
			}
			num9 = FloodFillUtility.m_areaDim.x;
			if (num6 < FloodFillUtility.m_areaDim.y - 1 && data.GetCellTypeSafe(num7, num8 + 1) == CellType.FLOOR && !FloodFillUtility.s_reachable[num4 + num9])
			{
				FloodFillUtility.s_reachable[num4 + num9] = true;
				FloodFillUtility.s_openList.Add(num4 + num9);
			}
			num3++;
		}
	}

	// Token: 0x06006C3C RID: 27708 RVA: 0x002A9BB4 File Offset: 0x002A7DB4
	public static bool WasFilled(IntVector2 c)
	{
		return FloodFillUtility.s_reachable[c.x - FloodFillUtility.m_areaMin.x + (c.y - FloodFillUtility.m_areaMin.y) * FloodFillUtility.m_areaDim.x];
	}

	// Token: 0x04006939 RID: 26937
	private static List<int> s_openList = new List<int>();

	// Token: 0x0400693A RID: 26938
	private static bool[] s_reachable = new bool[0];

	// Token: 0x0400693B RID: 26939
	private static IntVector2 m_areaMin;

	// Token: 0x0400693C RID: 26940
	private static IntVector2 m_areaDim;
}
