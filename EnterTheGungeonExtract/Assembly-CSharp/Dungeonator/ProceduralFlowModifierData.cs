using System;
using System.Collections.Generic;
using UnityEngine;

namespace Dungeonator
{
	// Token: 0x02000F05 RID: 3845
	[Serializable]
	public class ProceduralFlowModifierData
	{
		// Token: 0x17000BA1 RID: 2977
		// (get) Token: 0x06005205 RID: 20997 RVA: 0x001D4240 File Offset: 0x001D2440
		public bool PrerequisitesMet
		{
			get
			{
				for (int i = 0; i < this.prerequisites.Length; i++)
				{
					if (!this.prerequisites[i].CheckConditionsFulfilled())
					{
						return false;
					}
				}
				return !this.RequiresMasteryToken || !GameManager.HasInstance || !GameManager.Instance.PrimaryPlayer || GameManager.Instance.PrimaryPlayer.MasteryTokensCollectedThisRun > 0;
			}
		}

		// Token: 0x06005206 RID: 20998 RVA: 0x001D42BC File Offset: 0x001D24BC
		public ProceduralFlowModifierData.FlowModifierPlacementType GetPlacementRule()
		{
			return this.placementRules[BraveRandom.GenerationRandomRange(0, this.placementRules.Count)];
		}

		// Token: 0x04004A36 RID: 18998
		public string annotation;

		// Token: 0x04004A37 RID: 18999
		public bool DEBUG_FORCE_SPAWN;

		// Token: 0x04004A38 RID: 19000
		public bool OncePerRun;

		// Token: 0x04004A39 RID: 19001
		public List<ProceduralFlowModifierData.FlowModifierPlacementType> placementRules;

		// Token: 0x04004A3A RID: 19002
		public GenericRoomTable roomTable;

		// Token: 0x04004A3B RID: 19003
		public PrototypeDungeonRoom exactRoom;

		// Token: 0x04004A3C RID: 19004
		public bool IsWarpWing;

		// Token: 0x04004A3D RID: 19005
		public bool RequiresMasteryToken;

		// Token: 0x04004A3E RID: 19006
		public float chanceToLock;

		// Token: 0x04004A3F RID: 19007
		public float selectionWeight = 1f;

		// Token: 0x04004A40 RID: 19008
		public float chanceToSpawn = 1f;

		// Token: 0x04004A41 RID: 19009
		[SerializeField]
		public DungeonPlaceable RequiredValidPlaceable;

		// Token: 0x04004A42 RID: 19010
		public DungeonPrerequisite[] prerequisites;

		// Token: 0x04004A43 RID: 19011
		public bool CanBeForcedSecret = true;

		// Token: 0x04004A44 RID: 19012
		[Header("For Random Node Child")]
		public int RandomNodeChildMinDistanceFromEntrance;

		// Token: 0x04004A45 RID: 19013
		[Header("For Combat Frame")]
		public PrototypeDungeonRoom exactSecondaryRoom;

		// Token: 0x04004A46 RID: 19014
		public int framedCombatNodes;

		// Token: 0x02000F06 RID: 3846
		public enum FlowModifierPlacementType
		{
			// Token: 0x04004A48 RID: 19016
			BEFORE_ANY_COMBAT_ROOM,
			// Token: 0x04004A49 RID: 19017
			END_OF_CHAIN,
			// Token: 0x04004A4A RID: 19018
			HUB_ADJACENT_CHAIN_START,
			// Token: 0x04004A4B RID: 19019
			HUB_ADJACENT_NO_LINK,
			// Token: 0x04004A4C RID: 19020
			RANDOM_NODE_CHILD,
			// Token: 0x04004A4D RID: 19021
			COMBAT_FRAME,
			// Token: 0x04004A4E RID: 19022
			NO_LINKS,
			// Token: 0x04004A4F RID: 19023
			AFTER_BOSS,
			// Token: 0x04004A50 RID: 19024
			BLACK_MARKET
		}
	}
}
