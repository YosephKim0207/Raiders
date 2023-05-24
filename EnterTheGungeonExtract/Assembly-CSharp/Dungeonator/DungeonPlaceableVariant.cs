using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Dungeonator
{
	// Token: 0x02000EF4 RID: 3828
	[Serializable]
	public class DungeonPlaceableVariant
	{
		// Token: 0x0600518F RID: 20879 RVA: 0x001CF7E4 File Offset: 0x001CD9E4
		public float GetPercentChance()
		{
			return this.percentChance;
		}

		// Token: 0x17000B8C RID: 2956
		// (get) Token: 0x06005190 RID: 20880 RVA: 0x001CF7EC File Offset: 0x001CD9EC
		public GameObject GetOrLoadPlaceableObject
		{
			get
			{
				if (this.nonDatabasePlaceable)
				{
					return this.nonDatabasePlaceable;
				}
				if (!string.IsNullOrEmpty(this.enemyPlaceableGuid))
				{
					AIActor orLoadByGuid = EnemyDatabase.GetOrLoadByGuid(this.enemyPlaceableGuid);
					if (orLoadByGuid)
					{
						return orLoadByGuid.gameObject;
					}
				}
				if (this.pickupObjectPlaceableId >= 0)
				{
					PickupObject byId = PickupObjectDatabase.GetById(this.pickupObjectPlaceableId);
					if (byId)
					{
						return byId.gameObject;
					}
				}
				return null;
			}
		}

		// Token: 0x17000B8D RID: 2957
		// (get) Token: 0x06005191 RID: 20881 RVA: 0x001CF86C File Offset: 0x001CDA6C
		public DungeonPlaceableBehaviour.PlaceableDifficulty difficulty
		{
			get
			{
				if (this.nonDatabasePlaceable != null)
				{
					DungeonPlaceableBehaviour component = this.nonDatabasePlaceable.GetComponent<DungeonPlaceableBehaviour>();
					if (component != null)
					{
						return component.difficulty;
					}
				}
				if (string.IsNullOrEmpty(this.enemyPlaceableGuid))
				{
					return DungeonPlaceableBehaviour.PlaceableDifficulty.BASE;
				}
				EnemyDatabaseEntry entry = EnemyDatabase.GetEntry(this.enemyPlaceableGuid);
				if (entry == null)
				{
					return DungeonPlaceableBehaviour.PlaceableDifficulty.BASE;
				}
				return entry.difficulty;
			}
		}

		// Token: 0x17000B8E RID: 2958
		// (get) Token: 0x06005192 RID: 20882 RVA: 0x001CF8D8 File Offset: 0x001CDAD8
		public int difficultyRating
		{
			get
			{
				return (int)this.difficulty;
			}
		}

		// Token: 0x04004991 RID: 18833
		[SerializeField]
		public float percentChance = 0.1f;

		// Token: 0x04004992 RID: 18834
		[SerializeField]
		public Vector2 unitOffset = Vector2.zero;

		// Token: 0x04004993 RID: 18835
		[SerializeField]
		[FormerlySerializedAs("nonenemyPlaceable")]
		public GameObject nonDatabasePlaceable;

		// Token: 0x04004994 RID: 18836
		[FormerlySerializedAs("enemyGuid")]
		[EnemyIdentifier]
		public string enemyPlaceableGuid;

		// Token: 0x04004995 RID: 18837
		[PickupIdentifier]
		public int pickupObjectPlaceableId = -1;

		// Token: 0x04004996 RID: 18838
		[SerializeField]
		public bool forceBlackPhantom;

		// Token: 0x04004997 RID: 18839
		[SerializeField]
		public bool addDebrisObject;

		// Token: 0x04004998 RID: 18840
		[SerializeField]
		public DungeonPrerequisite[] prerequisites;

		// Token: 0x04004999 RID: 18841
		[SerializeField]
		public DungeonPlaceableRoomMaterialRequirement[] materialRequirements;

		// Token: 0x0400499A RID: 18842
		[NonSerialized]
		public float percentChanceMultiplier = 1f;
	}
}
