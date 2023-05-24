using System;
using System.Collections.Generic;
using UnityEngine;

namespace Dungeonator
{
	// Token: 0x02000F31 RID: 3889
	[Serializable]
	public class SemioticDungeonGenSettings
	{
		// Token: 0x06005366 RID: 21350 RVA: 0x001E6134 File Offset: 0x001E4334
		public DungeonFlow GetRandomFlow()
		{
			if (GameManager.Instance.BestGenerationDungeonPrefab != null && GameManager.Instance.BestGenerationDungeonPrefab.tileIndices.tilesetId == GlobalDungeonData.ValidTilesets.FORGEGEON && !GameStatsManager.Instance.GetFlag(GungeonFlags.BLACKSMITH_MET_PREVIOUSLY))
			{
				return this.flows[0];
			}
			float num = 0f;
			List<DungeonFlow> list = new List<DungeonFlow>();
			float num2 = 0f;
			List<DungeonFlow> list2 = new List<DungeonFlow>();
			for (int i = 0; i < this.flows.Count; i++)
			{
				if (GameStatsManager.Instance.QueryFlowDifferentiator(this.flows[i].name) > 0)
				{
					num += 1f;
					list.Add(this.flows[i]);
				}
				else
				{
					num2 += 1f;
					list2.Add(this.flows[i]);
				}
			}
			if (list2.Count <= 0 && list.Count > 0)
			{
				list2 = list;
				num2 = num;
			}
			if (list2.Count <= 0)
			{
				return null;
			}
			float num3 = BraveRandom.GenerationRandomValue() * num2;
			float num4 = 0f;
			for (int j = 0; j < list2.Count; j++)
			{
				num4 += 1f;
				if (num4 >= num3)
				{
					return list2[j];
				}
			}
			return this.flows[BraveRandom.GenerationRandomRange(0, this.flows.Count)];
		}

		// Token: 0x04004BD0 RID: 19408
		[SerializeField]
		public List<DungeonFlow> flows;

		// Token: 0x04004BD1 RID: 19409
		[SerializeField]
		public List<ExtraIncludedRoomData> mandatoryExtraRooms;

		// Token: 0x04004BD2 RID: 19410
		[SerializeField]
		public List<ExtraIncludedRoomData> optionalExtraRooms;

		// Token: 0x04004BD3 RID: 19411
		[SerializeField]
		public int MAX_GENERATION_ATTEMPTS = 25;

		// Token: 0x04004BD4 RID: 19412
		[SerializeField]
		public bool DEBUG_RENDER_CANVASES_SEPARATELY;
	}
}
