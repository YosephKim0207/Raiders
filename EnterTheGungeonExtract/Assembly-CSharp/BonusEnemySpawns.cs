using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001490 RID: 5264
[Serializable]
public class BonusEnemySpawns
{
	// Token: 0x060077C9 RID: 30665 RVA: 0x002FD284 File Offset: 0x002FB484
	public void Select(string name, Dictionary<GlobalDungeonData.ValidTilesets, int> numAssignedToFloors)
	{
		if (!DungeonPrerequisite.CheckConditionsFulfilled(this.Prereqs))
		{
			return;
		}
		int num = this.NumSpawnedChances.SelectByWeight();
		float num2 = this.CastleChance;
		float num3 = this.SewerChance;
		float num4 = this.GungeonChance;
		float num5 = this.CathedralChance;
		float num6 = this.MinegeonChance;
		float num7 = this.CatacombgeonChance;
		float num8 = this.ForgegeonChance;
		float num9 = this.BulletHellChance;
		for (int i = 0; i < num; i++)
		{
			float num10 = UnityEngine.Random.value * (num2 + num3 + num4 + num5 + num6 + num7 + num8 + num9);
			GlobalDungeonData.ValidTilesets validTilesets;
			if (num10 < num2)
			{
				validTilesets = GlobalDungeonData.ValidTilesets.CASTLEGEON;
				num2 = 0.05f;
			}
			else if (num10 < num2 + num3)
			{
				validTilesets = GlobalDungeonData.ValidTilesets.SEWERGEON;
				num3 = 0.05f;
			}
			else if (num10 < num2 + num3 + num4)
			{
				validTilesets = GlobalDungeonData.ValidTilesets.GUNGEON;
				num4 = 0.05f;
			}
			else if (num10 < num2 + num3 + num4 + num5)
			{
				validTilesets = GlobalDungeonData.ValidTilesets.CATHEDRALGEON;
				num5 = 0.05f;
			}
			else if (num10 < num2 + num3 + num4 + num5 + num6)
			{
				validTilesets = GlobalDungeonData.ValidTilesets.MINEGEON;
				num6 = 0.05f;
			}
			else if (num10 < num2 + num3 + num4 + num5 + num6 + num7)
			{
				validTilesets = GlobalDungeonData.ValidTilesets.CATACOMBGEON;
				num7 = 0.05f;
			}
			else if (num10 < num2 + num3 + num4 + num5 + num6 + num7 + num8)
			{
				validTilesets = GlobalDungeonData.ValidTilesets.FORGEGEON;
				num8 = 0.05f;
			}
			else
			{
				validTilesets = GlobalDungeonData.ValidTilesets.HELLGEON;
				num9 = 0.05f;
			}
			if (numAssignedToFloors.ContainsKey(validTilesets))
			{
				numAssignedToFloors[validTilesets]++;
			}
			else
			{
				numAssignedToFloors.Add(validTilesets, 1);
			}
		}
	}

	// Token: 0x040079F1 RID: 31217
	public DungeonPrerequisite[] Prereqs;

	// Token: 0x040079F2 RID: 31218
	[EnemyIdentifier]
	public string EnemyGuid;

	// Token: 0x040079F3 RID: 31219
	public WeightedIntCollection NumSpawnedChances;

	// Token: 0x040079F4 RID: 31220
	public float CastleChance = 0.2f;

	// Token: 0x040079F5 RID: 31221
	public float SewerChance;

	// Token: 0x040079F6 RID: 31222
	public float GungeonChance = 0.175f;

	// Token: 0x040079F7 RID: 31223
	public float CathedralChance;

	// Token: 0x040079F8 RID: 31224
	public float MinegeonChance = 0.15f;

	// Token: 0x040079F9 RID: 31225
	public float CatacombgeonChance = 0.125f;

	// Token: 0x040079FA RID: 31226
	public float ForgegeonChance = 0.1f;

	// Token: 0x040079FB RID: 31227
	public float BulletHellChance;
}
