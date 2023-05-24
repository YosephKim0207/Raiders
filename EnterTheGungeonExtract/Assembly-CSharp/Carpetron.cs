using System;
using System.Collections.Generic;
using Dungeonator;
using UnityEngine;

// Token: 0x02000EA9 RID: 3753
public class Carpetron
{
	// Token: 0x06004F6E RID: 20334 RVA: 0x001B87F8 File Offset: 0x001B69F8
	public static HashSet<IntVector2> PostprocessFullRoom(HashSet<IntVector2> set)
	{
		HashSet<IntVector2> hashSet = new HashSet<IntVector2>();
		IntVector2[] cardinalsAndOrdinals = IntVector2.CardinalsAndOrdinals;
		foreach (IntVector2 intVector in set)
		{
			bool flag = false;
			for (int i = 0; i < cardinalsAndOrdinals.Length; i++)
			{
				if (!set.Contains(intVector + cardinalsAndOrdinals[i]))
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				hashSet.Add(intVector);
				for (int j = 0; j < cardinalsAndOrdinals.Length; j++)
				{
					hashSet.Add(intVector + cardinalsAndOrdinals[j]);
				}
			}
		}
		return hashSet;
	}

	// Token: 0x06004F6F RID: 20335 RVA: 0x001B88D8 File Offset: 0x001B6AD8
	public static Tuple<IntVector2, IntVector2> PostprocessSubmatrix(Tuple<IntVector2, IntVector2> rect, out Tuple<IntVector2, IntVector2> bonusRect)
	{
		bonusRect = null;
		IntVector2 intVector = rect.Second - rect.First;
		IntVector2 first = rect.First;
		IntVector2 second = rect.Second;
		bool flag = intVector.x > 12 && intVector.y > 12;
		if (flag)
		{
			if (UnityEngine.Random.value < 1.4f)
			{
				int num = intVector.x / 3;
				int num2 = intVector.y / 3;
				first.x += num;
				second.x -= num;
				IntVector2 first2 = rect.First;
				IntVector2 second2 = rect.Second;
				first2.y += num2;
				second2.y -= num2;
				bonusRect = new Tuple<IntVector2, IntVector2>(first2, second2);
			}
			else if (intVector.x > intVector.y)
			{
				while (intVector.y > 4 && UnityEngine.Random.value > 0.3f)
				{
					intVector.y -= 2;
					first.y++;
					second.y--;
				}
			}
			else
			{
				while (intVector.x > 4 && UnityEngine.Random.value > 0.3f)
				{
					intVector.x -= 2;
					first.x++;
					second.x--;
				}
			}
		}
		else if (intVector.x > intVector.y && intVector.x > 12)
		{
			while (intVector.x > 12 && UnityEngine.Random.value > 0.3f)
			{
				intVector.x -= 2;
				first.x++;
				second.x--;
			}
		}
		else if (intVector.y > intVector.x && intVector.y > 12)
		{
			while (intVector.y > 12 && UnityEngine.Random.value > 0.3f)
			{
				intVector.y -= 2;
				first.y++;
				second.y--;
			}
		}
		return new Tuple<IntVector2, IntVector2>(first, second);
	}

	// Token: 0x06004F70 RID: 20336 RVA: 0x001B8B58 File Offset: 0x001B6D58
	public static Tuple<IntVector2, IntVector2> RawMaxSubmatrix(CellData[][] matrix, IntVector2 basePosition, IntVector2 dimensions, Func<CellData, bool> isInvalidFunction)
	{
		List<IntRect> list = new List<IntRect>();
		int y = dimensions.y;
		int x = dimensions.x;
		int num = -1;
		int[] array = new int[x];
		for (int i = 0; i < x; i++)
		{
			array[i] = -1;
		}
		int[] array2 = new int[x];
		int[] array3 = new int[x];
		Stack<int> stack = new Stack<int>();
		for (int j = 0; j < y; j++)
		{
			for (int k = 0; k < x; k++)
			{
				CellData cellData = matrix[basePosition.x + k][basePosition.y + j];
				if (isInvalidFunction(cellData))
				{
					array[k] = j;
				}
			}
			stack.Clear();
			for (int l = 0; l < x; l++)
			{
				while (stack.Count > 0 && array[stack.Peek()] <= array[l])
				{
					stack.Pop();
				}
				array2[l] = ((stack.Count != 0) ? stack.Peek() : (-1));
				stack.Push(l);
			}
			stack.Clear();
			for (int m = x - 1; m >= 0; m--)
			{
				while (stack.Count > 0 && array[stack.Peek()] <= array[m])
				{
					stack.Pop();
				}
				array3[m] = ((stack.Count != 0) ? stack.Peek() : x);
				stack.Push(m);
			}
			for (int n = 0; n < x; n++)
			{
				int num2 = (j - array[n]) * (array3[n] - array2[n] - 1);
				if (num2 > num)
				{
					num = num2;
					int num3 = array2[n] + 1;
					int num4 = array[n] + 1;
					int num5 = array3[n] - 1;
					int num6 = j;
					list.Add(new IntRect(num3, num4, num5 - num3, num6 - num4));
				}
			}
		}
		IntVector2 dimensions2 = list[list.Count - 1].Dimensions;
		for (int num7 = list.Count - 2; num7 >= 0; num7--)
		{
			if (list[num7].Dimensions != dimensions2)
			{
				list.RemoveAt(num7);
				num7++;
			}
		}
		int num8 = Mathf.FloorToInt((float)list.Count / 2f);
		return new Tuple<IntVector2, IntVector2>(new IntVector2(list[num8].Left, list[num8].Bottom), new IntVector2(list[num8].Right, list[num8].Top));
	}

	// Token: 0x06004F71 RID: 20337 RVA: 0x001B8E28 File Offset: 0x001B7028
	public static Tuple<IntVector2, IntVector2> MaxSubmatrix(CellData[][] matrix, IntVector2 basePosition, IntVector2 dimensions, bool includePits = false, bool includeOverrideFloors = false, bool includeWallNeighbors = false, int visualSubtype = -1)
	{
		DungeonData data = GameManager.Instance.Dungeon.data;
		List<IntRect> list = new List<IntRect>();
		int y = dimensions.y;
		int x = dimensions.x;
		int num = -1;
		int[] array = new int[x];
		for (int i = 0; i < x; i++)
		{
			array[i] = -1;
		}
		int[] array2 = new int[x];
		int[] array3 = new int[x];
		Stack<int> stack = new Stack<int>();
		for (int j = 0; j < y; j++)
		{
			for (int k = 0; k < x; k++)
			{
				CellData cellData = matrix[basePosition.x + k][basePosition.y + j];
				if (cellData == null)
				{
					array[k] = j;
				}
				else
				{
					bool flag = (!includeWallNeighbors && cellData.HasWallNeighbor(true, false)) || (!includePits && cellData.HasPitNeighbor(data));
					if (cellData.type == CellType.WALL || cellData.cellVisualData.floorType == CellVisualData.CellFloorType.Ice || cellData.cellVisualData.pathTilesetGridIndex > -1 || (!includeOverrideFloors && cellData.doesDamage) || (!includePits && cellData.type == CellType.PIT) || (flag || (!includeOverrideFloors && cellData.cellVisualData.floorTileOverridden)) || (!includeOverrideFloors && cellData.HasPhantomCarpetNeighbor(true)) || (visualSubtype > -1 && cellData.cellVisualData.roomVisualTypeIndex != visualSubtype))
					{
						array[k] = j;
					}
				}
			}
			stack.Clear();
			for (int l = 0; l < x; l++)
			{
				while (stack.Count > 0 && array[stack.Peek()] <= array[l])
				{
					stack.Pop();
				}
				array2[l] = ((stack.Count != 0) ? stack.Peek() : (-1));
				stack.Push(l);
			}
			stack.Clear();
			for (int m = x - 1; m >= 0; m--)
			{
				while (stack.Count > 0 && array[stack.Peek()] <= array[m])
				{
					stack.Pop();
				}
				array3[m] = ((stack.Count != 0) ? stack.Peek() : x);
				stack.Push(m);
			}
			for (int n = 0; n < x; n++)
			{
				int num2 = (j - array[n]) * (array3[n] - array2[n] - 1);
				if (num2 > num)
				{
					num = num2;
					int num3 = array2[n] + 1;
					int num4 = array[n] + 1;
					int num5 = array3[n] - 1;
					int num6 = j;
					list.Add(new IntRect(num3, num4, num5 - num3, num6 - num4));
				}
			}
		}
		IntVector2 dimensions2 = list[list.Count - 1].Dimensions;
		for (int num7 = list.Count - 2; num7 >= 0; num7--)
		{
			if (list[num7].Dimensions != dimensions2)
			{
				list.RemoveAt(num7);
				num7++;
			}
		}
		int num8 = Mathf.FloorToInt((float)list.Count / 2f);
		return new Tuple<IntVector2, IntVector2>(new IntVector2(list[num8].Left, list[num8].Bottom), new IntVector2(list[num8].Right, list[num8].Top));
	}
}
