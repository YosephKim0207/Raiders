using System;
using System.Collections.Generic;
using UnityEngine;

namespace Dungeonator
{
	// Token: 0x02000EF2 RID: 3826
	public class DungeonPlaceable : ScriptableObject
	{
		// Token: 0x0600517C RID: 20860 RVA: 0x001CE5F0 File Offset: 0x001CC7F0
		public int GetHeight()
		{
			return this.height;
		}

		// Token: 0x0600517D RID: 20861 RVA: 0x001CE5F8 File Offset: 0x001CC7F8
		public int GetWidth()
		{
			return this.width;
		}

		// Token: 0x17000B8A RID: 2954
		// (get) Token: 0x0600517E RID: 20862 RVA: 0x001CE600 File Offset: 0x001CC800
		public bool ContainsEnemy
		{
			get
			{
				for (int i = 0; i < this.variantTiers.Count; i++)
				{
					if (!string.IsNullOrEmpty(this.variantTiers[i].enemyPlaceableGuid))
					{
						EnemyDatabaseEntry entry = EnemyDatabase.GetEntry(this.variantTiers[i].enemyPlaceableGuid);
						if (entry != null)
						{
							return true;
						}
					}
				}
				return false;
			}
		}

		// Token: 0x17000B8B RID: 2955
		// (get) Token: 0x0600517F RID: 20863 RVA: 0x001CE664 File Offset: 0x001CC864
		public bool ContainsEnemylikeObjectForReinforcement
		{
			get
			{
				for (int i = 0; i < this.variantTiers.Count; i++)
				{
					if (this.variantTiers[i].nonDatabasePlaceable && this.variantTiers[i].nonDatabasePlaceable.GetComponent<ForgeHammerController>())
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x06005180 RID: 20864 RVA: 0x001CE6CC File Offset: 0x001CC8CC
		public bool IsValidMirrorPlaceable()
		{
			for (int i = 0; i < this.variantTiers.Count; i++)
			{
				if (this.variantTiers[i].nonDatabasePlaceable && !PrototypeDungeonRoom.GameObjectCanBeMirrored(this.variantTiers[i].nonDatabasePlaceable))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06005181 RID: 20865 RVA: 0x001CE730 File Offset: 0x001CC930
		public bool HasValidPlaceable()
		{
			for (int i = 0; i < this.variantTiers.Count; i++)
			{
				bool flag = true;
				if (this.variantTiers[i] != null)
				{
					if (this.variantTiers[i].prerequisites == null)
					{
						return true;
					}
					for (int j = 0; j < this.variantTiers[i].prerequisites.Length; j++)
					{
						if (!this.variantTiers[i].prerequisites[j].CheckConditionsFulfilled())
						{
							flag = false;
						}
					}
					if (flag)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06005182 RID: 20866 RVA: 0x001CE7D8 File Offset: 0x001CC9D8
		private GameObject InstantiateInternal(DungeonPlaceableVariant selectedVariant, RoomHandler targetRoom, IntVector2 location, bool deferConfiguration)
		{
			if (selectedVariant != null && selectedVariant.GetOrLoadPlaceableObject != null)
			{
				GameObject gameObject = DungeonPlaceableUtility.InstantiateDungeonPlaceable(selectedVariant.GetOrLoadPlaceableObject, targetRoom, location, deferConfiguration, AIActor.AwakenAnimationType.Default, false);
				if (gameObject != null && selectedVariant.unitOffset != Vector2.zero)
				{
					gameObject.transform.position += selectedVariant.unitOffset.ToVector3ZUp(0f);
					SpeculativeRigidbody componentInChildren = gameObject.GetComponentInChildren<SpeculativeRigidbody>();
					if (componentInChildren)
					{
						componentInChildren.Reinitialize();
					}
				}
				if (gameObject != null && this.UsePrefabTransformOffset)
				{
					gameObject.transform.position += selectedVariant.GetOrLoadPlaceableObject.transform.position;
				}
				if (selectedVariant.forceBlackPhantom && gameObject)
				{
					AIActor component = gameObject.GetComponent<AIActor>();
					if (component)
					{
						component.ForceBlackPhantom = true;
					}
				}
				if (selectedVariant.addDebrisObject && gameObject)
				{
					DebrisObject orAddComponent = gameObject.GetOrAddComponent<DebrisObject>();
					orAddComponent.shouldUseSRBMotion = true;
					orAddComponent.angularVelocity = 0f;
					orAddComponent.Priority = EphemeralObject.EphemeralPriority.Critical;
					orAddComponent.canRotate = false;
				}
				if (this.MarkSpawnedItemsAsRatIgnored && gameObject)
				{
					PickupObject component2 = gameObject.GetComponent<PickupObject>();
					if (component2)
					{
						component2.IgnoredByRat = true;
					}
				}
				return gameObject;
			}
			return null;
		}

		// Token: 0x06005183 RID: 20867 RVA: 0x001CE94C File Offset: 0x001CCB4C
		private GameObject InstantiateInternalOnlyActors(DungeonPlaceableVariant selectedVariant, RoomHandler targetRoom, IntVector2 location, bool deferConfiguration)
		{
			if (selectedVariant != null && selectedVariant.GetOrLoadPlaceableObject != null)
			{
				GameObject gameObject = DungeonPlaceableUtility.InstantiateDungeonPlaceableOnlyActors(selectedVariant.GetOrLoadPlaceableObject, targetRoom, location, deferConfiguration);
				if (selectedVariant.forceBlackPhantom && gameObject)
				{
					AIActor component = gameObject.GetComponent<AIActor>();
					if (component)
					{
						component.ForceBlackPhantom = true;
					}
				}
				return gameObject;
			}
			return null;
		}

		// Token: 0x06005184 RID: 20868 RVA: 0x001CE9B4 File Offset: 0x001CCBB4
		public GameObject InstantiateObjectDirectional(RoomHandler targetRoom, IntVector2 location, DungeonData.Direction direction)
		{
			List<DungeonPlaceableVariant> list = new List<DungeonPlaceableVariant>();
			if (this.variantTiers.Count == 4)
			{
				switch (direction)
				{
				case DungeonData.Direction.NORTH:
					list.Add(this.variantTiers[0]);
					break;
				case DungeonData.Direction.EAST:
					list.Add(this.variantTiers[1]);
					break;
				case DungeonData.Direction.SOUTH:
					list.Add(this.variantTiers[2]);
					break;
				case DungeonData.Direction.WEST:
					list.Add(this.variantTiers[3]);
					break;
				}
				return this.InstantiateInternal(this.SelectVariantByWeighting(list), targetRoom, location, false);
			}
			foreach (DungeonPlaceableVariant dungeonPlaceableVariant in this.variantTiers)
			{
				dungeonPlaceableVariant.percentChanceMultiplier = 1f;
				if (this.ProcessVariantPrerequisites(dungeonPlaceableVariant, new IntVector2?(location), targetRoom))
				{
					DungeonDoorController component = dungeonPlaceableVariant.nonDatabasePlaceable.GetComponent<DungeonDoorController>();
					DungeonDoorSubsidiaryBlocker component2 = dungeonPlaceableVariant.nonDatabasePlaceable.GetComponent<DungeonDoorSubsidiaryBlocker>();
					if (component != null)
					{
						if (component.northSouth && (direction == DungeonData.Direction.NORTH || direction == DungeonData.Direction.SOUTH))
						{
							list.Add(dungeonPlaceableVariant);
						}
						else if (!component.northSouth && (direction == DungeonData.Direction.EAST || direction == DungeonData.Direction.WEST))
						{
							list.Add(dungeonPlaceableVariant);
						}
					}
					else if (component2 != null)
					{
						if (component2.northSouth && (direction == DungeonData.Direction.NORTH || direction == DungeonData.Direction.SOUTH))
						{
							list.Add(dungeonPlaceableVariant);
						}
						else if (!component2.northSouth && (direction == DungeonData.Direction.EAST || direction == DungeonData.Direction.WEST))
						{
							list.Add(dungeonPlaceableVariant);
						}
					}
					else
					{
						list.Add(dungeonPlaceableVariant);
					}
				}
			}
			return this.InstantiateInternal(this.SelectVariantByWeighting(list), targetRoom, location, false);
		}

		// Token: 0x06005185 RID: 20869 RVA: 0x001CEBC8 File Offset: 0x001CCDC8
		public GameObject InstantiateObject(RoomHandler targetRoom, IntVector2 location, bool onlyActors = false, bool deferConfiguration = false)
		{
			int num = -1;
			return this.InstantiateObject(targetRoom, location, out num, -1, onlyActors, deferConfiguration);
		}

		// Token: 0x06005186 RID: 20870 RVA: 0x001CEBE8 File Offset: 0x001CCDE8
		public void ModifyWeightsByDifficulty(List<DungeonPlaceableVariant> validVariants)
		{
			if (GameManager.Instance.CurrentLevelOverrideState == GameManager.LevelOverrideState.FOYER)
			{
				return;
			}
			if (GameManager.Instance.PrimaryPlayer == null || GameManager.Instance.PrimaryPlayer.stats == null)
			{
				Debug.LogError("No player yet--can't check curse stat in DungeonPlaceable.");
				return;
			}
			Dictionary<DungeonPlaceableBehaviour.PlaceableDifficulty, float> dictionary = new Dictionary<DungeonPlaceableBehaviour.PlaceableDifficulty, float>();
			float num = 0f;
			for (int i = 0; i < validVariants.Count; i++)
			{
				DungeonPlaceableBehaviour.PlaceableDifficulty difficulty = validVariants[i].difficulty;
				if (!dictionary.ContainsKey(difficulty))
				{
					dictionary.Add(difficulty, 0f);
				}
				dictionary[difficulty] += validVariants[i].GetPercentChance();
				num += validVariants[i].GetPercentChance();
			}
			if (dictionary.Count <= 1)
			{
				return;
			}
			float num2 = (float)PlayerStats.GetTotalCurse();
			float num3 = Mathf.Clamp01(num2 / 10f);
			float num4 = ((!dictionary.ContainsKey(DungeonPlaceableBehaviour.PlaceableDifficulty.BASE)) ? 0f : dictionary[DungeonPlaceableBehaviour.PlaceableDifficulty.BASE]) / num;
			float num5 = ((!dictionary.ContainsKey(DungeonPlaceableBehaviour.PlaceableDifficulty.HARD)) ? 0f : dictionary[DungeonPlaceableBehaviour.PlaceableDifficulty.HARD]) / num;
			float num6 = ((!dictionary.ContainsKey(DungeonPlaceableBehaviour.PlaceableDifficulty.HARDER)) ? 0f : dictionary[DungeonPlaceableBehaviour.PlaceableDifficulty.HARDER]) / num;
			float num7 = ((!dictionary.ContainsKey(DungeonPlaceableBehaviour.PlaceableDifficulty.HARDEST)) ? 0f : dictionary[DungeonPlaceableBehaviour.PlaceableDifficulty.HARDEST]) / num;
			if (num4 > num3)
			{
				float num8 = num4 - num3;
				for (int j = 0; j < validVariants.Count; j++)
				{
					if (validVariants[j].difficultyRating == 0)
					{
						validVariants[j].percentChanceMultiplier = num8 / num4;
					}
				}
			}
			else if (num4 + num5 > num3)
			{
				float num9 = num4 + num5 - num3;
				for (int k = 0; k < validVariants.Count; k++)
				{
					if (validVariants[k].difficultyRating <= 0)
					{
						validVariants.RemoveAt(k);
						k--;
					}
					else if (validVariants[k].difficultyRating == 1)
					{
						validVariants[k].percentChanceMultiplier = num9 / num5;
					}
				}
			}
			else if (num4 + num5 + num6 > num3)
			{
				float num10 = num4 + num5 + num6 - num3;
				for (int l = 0; l < validVariants.Count; l++)
				{
					if (validVariants[l].difficultyRating <= 1)
					{
						validVariants.RemoveAt(l);
						l--;
					}
					else if (validVariants[l].difficultyRating == 2)
					{
						validVariants[l].percentChanceMultiplier = num10 / num6;
					}
				}
			}
			else
			{
				if (num4 + num5 + num6 + num7 < num3)
				{
					return;
				}
				for (int m = 0; m < validVariants.Count; m++)
				{
					if (validVariants[m].difficultyRating <= 2)
					{
						validVariants.RemoveAt(m);
						m--;
					}
				}
			}
		}

		// Token: 0x06005187 RID: 20871 RVA: 0x001CEF0C File Offset: 0x001CD10C
		public GameObject InstantiateObject(RoomHandler targetRoom, IntVector2 location, out int variantIndex, int forceVariant = -1, bool onlyActors = false, bool deferConfiguration = false)
		{
			variantIndex = -1;
			List<DungeonPlaceableVariant> list = new List<DungeonPlaceableVariant>();
			int num = int.MaxValue;
			for (int i = 0; i < this.variantTiers.Count; i++)
			{
				DungeonPlaceableVariant dungeonPlaceableVariant = this.variantTiers[i];
				dungeonPlaceableVariant.percentChanceMultiplier = 1f;
				if (this.ProcessVariantPrerequisites(dungeonPlaceableVariant, new IntVector2?(location), targetRoom))
				{
					if (this.respectsEncounterableDifferentiator)
					{
						int? num2 = null;
						if (dungeonPlaceableVariant.nonDatabasePlaceable != null)
						{
							EncounterTrackable component = dungeonPlaceableVariant.nonDatabasePlaceable.GetComponent<EncounterTrackable>();
							if (component != null)
							{
								num2 = new int?(GameStatsManager.Instance.QueryEncounterableDifferentiator(component));
							}
						}
						else if (!string.IsNullOrEmpty(dungeonPlaceableVariant.enemyPlaceableGuid))
						{
							EnemyDatabaseEntry entry = EnemyDatabase.GetEntry(dungeonPlaceableVariant.enemyPlaceableGuid);
							if (entry != null && !string.IsNullOrEmpty(entry.encounterGuid))
							{
								EncounterDatabaseEntry entry2 = EncounterDatabase.GetEntry(entry.encounterGuid);
								if (entry2 != null)
								{
									num2 = new int?(GameStatsManager.Instance.QueryEncounterableDifferentiator(entry2));
								}
							}
						}
						if (num2 != null)
						{
							if (num2.Value < num)
							{
								list.Clear();
								num = num2.Value;
							}
							else if (num2.Value > num)
							{
								goto IL_16A;
							}
						}
					}
					if (targetRoom == null || !this.roomSequential || i <= targetRoom.distanceFromEntrance / 2)
					{
						list.Add(dungeonPlaceableVariant);
					}
				}
				IL_16A:;
			}
			DungeonPlaceableVariant dungeonPlaceableVariant2 = null;
			this.ModifyWeightsByDifficulty(list);
			if (forceVariant == -1)
			{
				dungeonPlaceableVariant2 = this.SelectVariantByWeighting(list);
			}
			else if (list.Count > forceVariant)
			{
				dungeonPlaceableVariant2 = list[forceVariant];
			}
			if (dungeonPlaceableVariant2 != null)
			{
				variantIndex = this.variantTiers.IndexOf(dungeonPlaceableVariant2);
			}
			if (this.respectsEncounterableDifferentiator && dungeonPlaceableVariant2 != null && dungeonPlaceableVariant2.GetOrLoadPlaceableObject != null)
			{
				EncounterTrackable component2 = dungeonPlaceableVariant2.GetOrLoadPlaceableObject.GetComponent<EncounterTrackable>();
				if (component2 != null)
				{
					component2.HandleEncounter_GeneratedObjects();
				}
			}
			if (dungeonPlaceableVariant2 != null && dungeonPlaceableVariant2.GetOrLoadPlaceableObject != null)
			{
				DungeonPlaceableBehaviour component3 = dungeonPlaceableVariant2.GetOrLoadPlaceableObject.GetComponent<DungeonPlaceableBehaviour>();
				if (component3 != null)
				{
					GameObject gameObject;
					if (onlyActors)
					{
						gameObject = component3.InstantiateObjectOnlyActors(targetRoom, location, deferConfiguration);
					}
					else
					{
						gameObject = component3.InstantiateObject(targetRoom, location, deferConfiguration);
					}
					if (gameObject != null && dungeonPlaceableVariant2.unitOffset != Vector2.zero)
					{
						gameObject.transform.position += dungeonPlaceableVariant2.unitOffset.ToVector3ZUp(0f);
						SpeculativeRigidbody componentInChildren = gameObject.GetComponentInChildren<SpeculativeRigidbody>();
						if (componentInChildren)
						{
							componentInChildren.Reinitialize();
						}
					}
					if (dungeonPlaceableVariant2.forceBlackPhantom && gameObject)
					{
						AIActor component4 = gameObject.GetComponent<AIActor>();
						if (component4)
						{
							component4.ForceBlackPhantom = true;
						}
					}
					return gameObject;
				}
			}
			if (onlyActors)
			{
				return this.InstantiateInternalOnlyActors(dungeonPlaceableVariant2, targetRoom, location, deferConfiguration);
			}
			return this.InstantiateInternal(dungeonPlaceableVariant2, targetRoom, location, deferConfiguration);
		}

		// Token: 0x06005188 RID: 20872 RVA: 0x001CF248 File Offset: 0x001CD448
		private bool ProcessVariantPrerequisites(DungeonPlaceableVariant dpv, IntVector2? targetPosition = null, RoomHandler targetRoom = null)
		{
			if (targetRoom != null && targetPosition != null && dpv.materialRequirements != null && dpv.materialRequirements.Length > 0)
			{
				bool flag = true;
				for (int i = 0; i < dpv.materialRequirements.Length; i++)
				{
					if (dpv.materialRequirements[i].TargetTileset != GameManager.Instance.Dungeon.tileIndices.tilesetId)
					{
						if (dpv.materialRequirements[i].RequireMaterial)
						{
							return false;
						}
					}
					else
					{
						int roomVisualTypeIndex = GameManager.Instance.Dungeon.data[targetRoom.area.basePosition + targetPosition.Value].cellVisualData.roomVisualTypeIndex;
						if (!dpv.materialRequirements[i].RequireMaterial || dpv.materialRequirements[i].RoomMaterial != roomVisualTypeIndex)
						{
							if (dpv.materialRequirements[i].RequireMaterial || dpv.materialRequirements[i].RoomMaterial == roomVisualTypeIndex)
							{
								flag = false;
							}
						}
					}
				}
				if (!flag)
				{
					return false;
				}
			}
			if (dpv.prerequisites != null && dpv.prerequisites.Length > 0)
			{
				bool flag2 = true;
				for (int j = 0; j < dpv.prerequisites.Length; j++)
				{
					if (!dpv.prerequisites[j].CheckConditionsFulfilled())
					{
						flag2 = false;
						break;
					}
				}
				return flag2;
			}
			return true;
		}

		// Token: 0x06005189 RID: 20873 RVA: 0x001CF3C4 File Offset: 0x001CD5C4
		public DungeonPlaceableVariant SelectFromTiersFull()
		{
			List<DungeonPlaceableVariant> list = new List<DungeonPlaceableVariant>();
			int num = int.MaxValue;
			for (int i = 0; i < this.variantTiers.Count; i++)
			{
				DungeonPlaceableVariant dungeonPlaceableVariant = this.variantTiers[i];
				dungeonPlaceableVariant.percentChanceMultiplier = 1f;
				if (this.ProcessVariantPrerequisites(dungeonPlaceableVariant, null, null))
				{
					if (this.respectsEncounterableDifferentiator)
					{
						int? num2 = null;
						if (dungeonPlaceableVariant.nonDatabasePlaceable != null)
						{
							EncounterTrackable component = dungeonPlaceableVariant.nonDatabasePlaceable.GetComponent<EncounterTrackable>();
							if (component != null)
							{
								num2 = new int?(GameStatsManager.Instance.QueryEncounterableDifferentiator(component));
							}
						}
						else if (!string.IsNullOrEmpty(dungeonPlaceableVariant.enemyPlaceableGuid))
						{
							EnemyDatabaseEntry entry = EnemyDatabase.GetEntry(dungeonPlaceableVariant.enemyPlaceableGuid);
							if (entry != null && !string.IsNullOrEmpty(entry.encounterGuid))
							{
								EncounterDatabaseEntry entry2 = EncounterDatabase.GetEntry(entry.encounterGuid);
								if (entry2 != null)
								{
									num2 = new int?(GameStatsManager.Instance.QueryEncounterableDifferentiator(entry2));
								}
							}
						}
						if (num2 != null)
						{
							if (num2.Value < num)
							{
								list.Clear();
								num = num2.Value;
							}
							else if (num2.Value > num)
							{
								goto IL_147;
							}
						}
					}
					list.Add(dungeonPlaceableVariant);
				}
				IL_147:;
			}
			this.ModifyWeightsByDifficulty(list);
			return this.SelectVariantByWeighting(list);
		}

		// Token: 0x0600518A RID: 20874 RVA: 0x001CF544 File Offset: 0x001CD744
		private DungeonPlaceableVariant SelectVariantByWeighting(List<DungeonPlaceableVariant> variants)
		{
			float num = 0f;
			float num2 = 0f;
			bool flag = this.IsAnnexTable;
			if (flag)
			{
				flag = GameManager.Instance.BestGenerationDungeonPrefab.tileIndices.tilesetId == GlobalDungeonData.ValidTilesets.CATACOMBGEON && GameManager.Instance.PrimaryPlayer && GameManager.Instance.PrimaryPlayer.MasteryTokensCollectedThisRun > 0;
			}
			for (int i = 0; i < variants.Count; i++)
			{
				if (!flag || !variants[i].nonDatabasePlaceable || !(variants[i].nonDatabasePlaceable.name == "SellPit"))
				{
					num2 += variants[i].GetPercentChance() * variants[i].percentChanceMultiplier;
				}
			}
			float num3 = UnityEngine.Random.value * num2;
			DungeonPlaceableVariant dungeonPlaceableVariant = null;
			for (int j = 0; j < variants.Count; j++)
			{
				if (!flag || !variants[j].nonDatabasePlaceable || !(variants[j].nonDatabasePlaceable.name == "SellPit"))
				{
					num += variants[j].GetPercentChance() * variants[j].percentChanceMultiplier;
					if (num >= num3)
					{
						dungeonPlaceableVariant = variants[j];
						break;
					}
				}
			}
			return dungeonPlaceableVariant;
		}

		// Token: 0x0600518B RID: 20875 RVA: 0x001CF6C4 File Offset: 0x001CD8C4
		public int GetMinimumDifficulty()
		{
			int num = int.MaxValue;
			foreach (DungeonPlaceableVariant dungeonPlaceableVariant in this.variantTiers)
			{
				num = Math.Min(num, dungeonPlaceableVariant.difficultyRating);
			}
			return num;
		}

		// Token: 0x0600518C RID: 20876 RVA: 0x001CF730 File Offset: 0x001CD930
		public int GetMaximumDifficulty()
		{
			int num = int.MinValue;
			foreach (DungeonPlaceableVariant dungeonPlaceableVariant in this.variantTiers)
			{
				num = Math.Max(num, dungeonPlaceableVariant.difficultyRating);
			}
			return num;
		}

		// Token: 0x04004984 RID: 18820
		public int width;

		// Token: 0x04004985 RID: 18821
		public int height;

		// Token: 0x04004986 RID: 18822
		[SerializeField]
		public bool isPassable = true;

		// Token: 0x04004987 RID: 18823
		[SerializeField]
		public bool roomSequential;

		// Token: 0x04004988 RID: 18824
		[SerializeField]
		public bool respectsEncounterableDifferentiator;

		// Token: 0x04004989 RID: 18825
		[SerializeField]
		public bool UsePrefabTransformOffset;

		// Token: 0x0400498A RID: 18826
		[SerializeField]
		public bool MarkSpawnedItemsAsRatIgnored;

		// Token: 0x0400498B RID: 18827
		[SerializeField]
		public bool DebugThisPlaceable;

		// Token: 0x0400498C RID: 18828
		[SerializeField]
		public bool IsAnnexTable;

		// Token: 0x0400498D RID: 18829
		[SerializeField]
		public List<DungeonPlaceableVariant> variantTiers;
	}
}
