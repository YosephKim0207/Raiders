using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020011B9 RID: 4537
public class MonsterHuntData : ScriptableObject
{
	// Token: 0x040060FB RID: 24827
	[SerializeField]
	public List<MonsterHuntQuest> OrderedQuests;

	// Token: 0x040060FC RID: 24828
	[SerializeField]
	public List<MonsterHuntQuest> ProceduralQuests;
}
