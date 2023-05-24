using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001601 RID: 5633
public class ProceduralChestContents : ScriptableObject
{
	// Token: 0x060082E8 RID: 33512 RVA: 0x003586DC File Offset: 0x003568DC
	public PickupObject GetItem(float val)
	{
		float num = 0f;
		for (int i = 0; i < this.items.Count; i++)
		{
			num += this.items[i].chance;
		}
		float num2 = 0f;
		for (int j = 0; j < this.items.Count; j++)
		{
			num2 += this.items[j].chance;
			if (num2 / num > val)
			{
				return this.items[j].item;
			}
		}
		return this.items[this.items.Count - 1].item;
	}

	// Token: 0x04008600 RID: 34304
	[BetterList]
	public List<ProceduralChestItem> items;
}
