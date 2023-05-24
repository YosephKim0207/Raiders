using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200135E RID: 4958
[Serializable]
public class BossFloorEntry
{
	// Token: 0x06007065 RID: 28773 RVA: 0x002C92F0 File Offset: 0x002C74F0
	public IndividualBossFloorEntry SelectBoss()
	{
		List<IndividualBossFloorEntry> list = new List<IndividualBossFloorEntry>();
		float num = 0f;
		for (int i = 0; i < this.Bosses.Count; i++)
		{
			if (this.Bosses[i].GlobalPrereqsValid())
			{
				list.Add(this.Bosses[i]);
				Debug.LogWarning(string.Concat(new object[]
				{
					"Adding valid boss: ",
					this.Bosses[i].TargetRoomTable.name,
					"|",
					this.Bosses[i].GetWeightModifier()
				}));
				num += this.Bosses[i].GetWeightModifier() * this.Bosses[i].BossWeight;
			}
		}
		float num2 = BraveRandom.GenerationRandomValue() * num;
		float num3 = 0f;
		for (int j = 0; j < list.Count; j++)
		{
			num3 += this.Bosses[j].GetWeightModifier() * list[j].BossWeight;
			if (num3 >= num2)
			{
				Debug.LogWarning(string.Concat(new object[]
				{
					"Returning valid boss: ",
					list[j].TargetRoomTable.name,
					"|",
					list[j].GetWeightModifier()
				}));
				return list[j];
			}
		}
		Debug.LogWarning(string.Concat(new object[]
		{
			"Returning fallback boss boss: ",
			list[list.Count - 1].TargetRoomTable.name,
			"|",
			list[list.Count - 1].GetWeightModifier()
		}));
		return list[list.Count - 1];
	}

	// Token: 0x04006FD7 RID: 28631
	public string Annotation;

	// Token: 0x04006FD8 RID: 28632
	[EnumFlags]
	public GlobalDungeonData.ValidTilesets AssociatedTilesets;

	// Token: 0x04006FD9 RID: 28633
	[SerializeField]
	public List<IndividualBossFloorEntry> Bosses;
}
