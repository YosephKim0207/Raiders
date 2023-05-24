using System;
using System.Collections.Generic;
using System.Linq;
using Dungeonator;
using UnityEngine;

// Token: 0x020011E9 RID: 4585
public class ResourcefulRatRewardRoomController : DungeonPlaceableBehaviour
{
	// Token: 0x06006659 RID: 26201 RVA: 0x0027CA60 File Offset: 0x0027AC60
	public void Start()
	{
		RoomHandler absoluteRoom = base.transform.position.GetAbsoluteRoom();
		List<RewardPedestal> list = absoluteRoom.GetComponentsInRoom<RewardPedestal>();
		list = list.ttOrderByDescending((RewardPedestal a) => a.transform.position.x * 10000f + a.transform.position.y);
		this.m_pedestals = list.ToArray();
		this.m_pedestals[0].SpecificItemId = this.PedestalA1Items[UnityEngine.Random.Range(0, this.PedestalA1Items.Length)];
		this.m_pedestals[1].SpecificItemId = this.PedestalA2Items[UnityEngine.Random.Range(0, this.PedestalA2Items.Length)];
		this.m_pedestals[2].SpecificItemId = this.PedestalA3Items[UnityEngine.Random.Range(0, this.PedestalA3Items.Length)];
		this.m_pedestals[3].SpecificItemId = this.PedestalA4Items[UnityEngine.Random.Range(0, this.PedestalA4Items.Length)];
		this.m_pedestals[4].SpecificItemId = this.PedestalB1Items[UnityEngine.Random.Range(0, this.PedestalB1Items.Length)];
		this.m_pedestals[5].SpecificItemId = this.PedestalB2Items[UnityEngine.Random.Range(0, this.PedestalB2Items.Length)];
		this.m_pedestals[6].SpecificItemId = this.PedestalB3Items[UnityEngine.Random.Range(0, this.PedestalB3Items.Length)];
		this.m_pedestals[7].SpecificItemId = this.PedestalB4Items[UnityEngine.Random.Range(0, this.PedestalB4Items.Length)];
		for (int i = 0; i < this.m_pedestals.Length; i++)
		{
			this.m_pedestals[i].ForceConfiguration();
		}
		List<Chest> componentsInRoom = absoluteRoom.GetComponentsInRoom<Chest>();
		this.m_ratChests = new Chest[4];
		int num = 0;
		for (int j = 0; j < componentsInRoom.Count; j++)
		{
			if (componentsInRoom[j].ChestIdentifier == Chest.SpecialChestIdentifier.RAT)
			{
				this.m_ratChests[num] = componentsInRoom[j];
				num++;
				if (num >= this.m_ratChests.Length)
				{
					break;
				}
			}
		}
		List<int> list2 = Enumerable.Range(0, this.RatChestItems.Length).ToList<int>().Shuffle<int>();
		for (int k = 0; k < this.m_ratChests.Length; k++)
		{
			this.m_ratChests[k].forceContentIds = new List<int>();
			this.m_ratChests[k].forceContentIds.Add(this.RatChestItems[list2[k]]);
		}
	}

	// Token: 0x0400622A RID: 25130
	[PickupIdentifier]
	public int[] PedestalA1Items;

	// Token: 0x0400622B RID: 25131
	[PickupIdentifier]
	public int[] PedestalA2Items;

	// Token: 0x0400622C RID: 25132
	[PickupIdentifier]
	public int[] PedestalA3Items;

	// Token: 0x0400622D RID: 25133
	[PickupIdentifier]
	public int[] PedestalA4Items;

	// Token: 0x0400622E RID: 25134
	[PickupIdentifier]
	public int[] PedestalB1Items;

	// Token: 0x0400622F RID: 25135
	[PickupIdentifier]
	public int[] PedestalB2Items;

	// Token: 0x04006230 RID: 25136
	[PickupIdentifier]
	public int[] PedestalB3Items;

	// Token: 0x04006231 RID: 25137
	[PickupIdentifier]
	public int[] PedestalB4Items;

	// Token: 0x04006232 RID: 25138
	[PickupIdentifier]
	public int[] RatChestItems;

	// Token: 0x04006233 RID: 25139
	private RewardPedestal[] m_pedestals;

	// Token: 0x04006234 RID: 25140
	private Chest[] m_ratChests;
}
