using System;
using System.Collections.Generic;

// Token: 0x02000F6D RID: 3949
[Serializable]
public class WeightedRoomCollection
{
	// Token: 0x06005521 RID: 21793 RVA: 0x00205330 File Offset: 0x00203530
	public WeightedRoomCollection()
	{
		this.elements = new List<WeightedRoom>();
	}

	// Token: 0x06005522 RID: 21794 RVA: 0x00205344 File Offset: 0x00203544
	public void Add(WeightedRoom w)
	{
		this.elements.Add(w);
	}

	// Token: 0x06005523 RID: 21795 RVA: 0x00205354 File Offset: 0x00203554
	public WeightedRoom SelectByWeight()
	{
		List<WeightedRoom> list = new List<WeightedRoom>();
		float num = 0f;
		for (int i = 0; i < this.elements.Count; i++)
		{
			WeightedRoom weightedRoom = this.elements[i];
			bool flag = true;
			for (int j = 0; j < weightedRoom.additionalPrerequisites.Length; j++)
			{
				if (!weightedRoom.additionalPrerequisites[j].CheckConditionsFulfilled())
				{
					flag = false;
					break;
				}
			}
			if (!(weightedRoom.room != null) || weightedRoom.room.CheckPrerequisites())
			{
				if (flag)
				{
					list.Add(weightedRoom);
					num += weightedRoom.weight;
				}
			}
		}
		if (list.Count == 0)
		{
			return null;
		}
		float num2 = BraveRandom.GenerationRandomValue() * num;
		float num3 = 0f;
		for (int k = 0; k < list.Count; k++)
		{
			num3 += list[k].weight;
			if (num3 > num2)
			{
				return list[k];
			}
		}
		return list[list.Count - 1];
	}

	// Token: 0x06005524 RID: 21796 RVA: 0x0020547C File Offset: 0x0020367C
	public WeightedRoom SelectByWeightWithoutDuplicates(List<PrototypeDungeonRoom> extant)
	{
		List<WeightedRoom> list = new List<WeightedRoom>();
		float num = 0f;
		for (int i = 0; i < this.elements.Count; i++)
		{
			WeightedRoom weightedRoom = this.elements[i];
			if (!extant.Contains(weightedRoom.room))
			{
				bool flag = true;
				for (int j = 0; j < weightedRoom.additionalPrerequisites.Length; j++)
				{
					if (!weightedRoom.additionalPrerequisites[j].CheckConditionsFulfilled())
					{
						flag = false;
						break;
					}
				}
				if (flag)
				{
					list.Add(weightedRoom);
					num += weightedRoom.weight;
				}
			}
		}
		float num2 = BraveRandom.GenerationRandomValue() * num;
		float num3 = 0f;
		for (int k = 0; k < list.Count; k++)
		{
			num3 += list[k].weight;
			if (num3 > num2)
			{
				return list[k];
			}
		}
		return list[list.Count - 1];
	}

	// Token: 0x04004E09 RID: 19977
	[TrimElementTags]
	public List<WeightedRoom> elements;
}
