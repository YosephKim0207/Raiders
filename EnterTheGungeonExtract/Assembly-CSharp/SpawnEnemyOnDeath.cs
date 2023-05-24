using System;
using System.Collections.Generic;
using Dungeonator;
using Pathfinding;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

// Token: 0x020010C0 RID: 4288
public class SpawnEnemyOnDeath : OnDeathBehavior
{
	// Token: 0x06005E7F RID: 24191 RVA: 0x00244138 File Offset: 0x00242338
	private bool ShowRandomPrams()
	{
		return this.enemySelection == SpawnEnemyOnDeath.EnemySelection.Random;
	}

	// Token: 0x06005E80 RID: 24192 RVA: 0x00244144 File Offset: 0x00242344
	private bool ShowInsideColliderParams()
	{
		return this.spawnPosition == SpawnEnemyOnDeath.SpawnPosition.InsideCollider;
	}

	// Token: 0x06005E81 RID: 24193 RVA: 0x00244150 File Offset: 0x00242350
	private bool ShowInsideRadiusParams()
	{
		return this.spawnPosition == SpawnEnemyOnDeath.SpawnPosition.InsideRadius;
	}

	// Token: 0x06005E82 RID: 24194 RVA: 0x0024415C File Offset: 0x0024235C
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x06005E83 RID: 24195 RVA: 0x00244164 File Offset: 0x00242364
	protected override void OnTrigger(Vector2 damageDirection)
	{
		if (this.m_hasTriggered)
		{
			return;
		}
		this.m_hasTriggered = true;
		if (this.guaranteedSpawnGenerations <= 0f && this.chanceToSpawn < 1f && UnityEngine.Random.value > this.chanceToSpawn)
		{
			return;
		}
		if (!string.IsNullOrEmpty(this.spawnVfx))
		{
			base.aiAnimator.PlayVfx(this.spawnVfx, null, null, null);
		}
		string[] array = null;
		if (this.enemySelection == SpawnEnemyOnDeath.EnemySelection.All)
		{
			array = this.enemyGuidsToSpawn;
		}
		else if (this.enemySelection == SpawnEnemyOnDeath.EnemySelection.Random)
		{
			array = new string[UnityEngine.Random.Range(this.minSpawnCount, this.maxSpawnCount)];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = BraveUtility.RandomElement<string>(this.enemyGuidsToSpawn);
			}
		}
		this.SpawnEnemies(array);
	}

	// Token: 0x06005E84 RID: 24196 RVA: 0x0024425C File Offset: 0x0024245C
	public void ManuallyTrigger(Vector2 damageDirection)
	{
		this.OnTrigger(damageDirection);
	}

	// Token: 0x06005E85 RID: 24197 RVA: 0x00244268 File Offset: 0x00242468
	private void SpawnEnemies(string[] selectedEnemyGuids)
	{
		if (this.spawnPosition == SpawnEnemyOnDeath.SpawnPosition.InsideCollider)
		{
			IntVector2 intVector = base.specRigidbody.UnitCenter.ToIntVector2(VectorConversions.Floor);
			if (base.aiActor.IsFalling)
			{
				return;
			}
			if (GameManager.Instance.Dungeon.CellIsPit(base.specRigidbody.UnitCenter.ToVector3ZUp(0f)))
			{
				return;
			}
			RoomHandler roomFromPosition = GameManager.Instance.Dungeon.GetRoomFromPosition(intVector);
			List<SpeculativeRigidbody> list = new List<SpeculativeRigidbody>();
			list.Add(base.specRigidbody);
			Vector2 unitBottomLeft = base.specRigidbody.UnitBottomLeft;
			for (int i = 0; i < selectedEnemyGuids.Length; i++)
			{
				AIActor orLoadByGuid = EnemyDatabase.GetOrLoadByGuid(selectedEnemyGuids[i]);
				AIActor aiactor = AIActor.Spawn(orLoadByGuid, base.specRigidbody.UnitCenter.ToIntVector2(VectorConversions.Floor), roomFromPosition, false, AIActor.AwakenAnimationType.Default, true);
				if (base.aiActor.IsBlackPhantom)
				{
					aiactor.ForceBlackPhantom = true;
				}
				if (aiactor)
				{
					aiactor.specRigidbody.Initialize();
					Vector2 vector = unitBottomLeft - (aiactor.specRigidbody.UnitBottomLeft - aiactor.transform.position.XY());
					Vector2 vector2 = vector + new Vector2(Mathf.Max(0f, base.specRigidbody.UnitDimensions.x - aiactor.specRigidbody.UnitDimensions.x), 0f);
					aiactor.transform.position = Vector2.Lerp(vector, vector2, (selectedEnemyGuids.Length != 1) ? ((float)i / ((float)selectedEnemyGuids.Length - 1f)) : 0f);
					aiactor.specRigidbody.Reinitialize();
					vector -= new Vector2(PhysicsEngine.PixelToUnit(this.extraPixelWidth), 0f);
					vector2 += new Vector2(PhysicsEngine.PixelToUnit(this.extraPixelWidth), 0f);
					Vector2 vector3 = Vector2.Lerp(vector, vector2, (selectedEnemyGuids.Length != 1) ? ((float)i / ((float)selectedEnemyGuids.Length - 1f)) : 0.5f);
					IntVector2 intVector2 = PhysicsEngine.UnitToPixel(vector3 - aiactor.transform.position.XY());
					CollisionData collisionData = null;
					if (PhysicsEngine.Instance.RigidbodyCastWithIgnores(aiactor.specRigidbody, intVector2, out collisionData, true, true, null, false, list.ToArray()))
					{
						intVector2 = collisionData.NewPixelsToMove;
					}
					CollisionData.Pool.Free(ref collisionData);
					aiactor.transform.position += PhysicsEngine.PixelToUnit(intVector2);
					aiactor.specRigidbody.Reinitialize();
					if (i == 0)
					{
						aiactor.aiAnimator.FacingDirection = 180f;
					}
					else if (i == selectedEnemyGuids.Length - 1)
					{
						aiactor.aiAnimator.FacingDirection = 0f;
					}
					this.HandleSpawn(aiactor);
					list.Add(aiactor.specRigidbody);
				}
			}
			for (int j = 0; j < list.Count; j++)
			{
				for (int k = 0; k < list.Count; k++)
				{
					if (j != k)
					{
						list[j].RegisterGhostCollisionException(list[k]);
					}
				}
			}
		}
		else if (this.spawnPosition == SpawnEnemyOnDeath.SpawnPosition.ScreenEdge)
		{
			for (int l = 0; l < selectedEnemyGuids.Length; l++)
			{
				AIActor orLoadByGuid2 = EnemyDatabase.GetOrLoadByGuid(selectedEnemyGuids[l]);
				AIActor spawnedActor = AIActor.Spawn(orLoadByGuid2, base.specRigidbody.UnitCenter.ToIntVector2(VectorConversions.Floor), base.aiActor.ParentRoom, false, AIActor.AwakenAnimationType.Default, true);
				if (spawnedActor)
				{
					Vector2 cameraBottomLeft = BraveUtility.ViewportToWorldpoint(new Vector2(0f, 0f), ViewportType.Gameplay);
					Vector2 cameraTopRight = BraveUtility.ViewportToWorldpoint(new Vector2(1f, 1f), ViewportType.Gameplay);
					IntVector2 bottomLeft = cameraBottomLeft.ToIntVector2(VectorConversions.Ceil);
					IntVector2 topRight = cameraTopRight.ToIntVector2(VectorConversions.Floor) - IntVector2.One;
					CellValidator cellValidator = delegate(IntVector2 c)
					{
						for (int num2 = 0; num2 < spawnedActor.Clearance.x; num2++)
						{
							for (int num3 = 0; num3 < spawnedActor.Clearance.y; num3++)
							{
								if (GameManager.Instance.Dungeon.data.isTopWall(c.x + num2, c.y + num3))
								{
									return false;
								}
								if (GameManager.Instance.Dungeon.data[c.x + num2, c.y + num3].isExitCell)
								{
									return false;
								}
							}
						}
						return c.x >= bottomLeft.x && c.y >= bottomLeft.y && c.x + spawnedActor.Clearance.x - 1 <= topRight.x && c.y + spawnedActor.Clearance.y - 1 <= topRight.y;
					};
					Func<IntVector2, float> func = delegate(IntVector2 c)
					{
						float num2 = float.MaxValue;
						num2 = Mathf.Min(num2, (float)c.x - cameraBottomLeft.x);
						num2 = Mathf.Min(num2, (float)c.y - cameraBottomLeft.y);
						num2 = Mathf.Min(num2, cameraTopRight.x - (float)c.x + (float)spawnedActor.Clearance.x);
						return Mathf.Min(num2, cameraTopRight.y - (float)c.y + (float)spawnedActor.Clearance.y);
					};
					Vector2 vector4 = spawnedActor.specRigidbody.UnitCenter - spawnedActor.transform.position.XY();
					IntVector2? randomWeightedAvailableCell = spawnedActor.ParentRoom.GetRandomWeightedAvailableCell(new IntVector2?(spawnedActor.Clearance), new CellTypes?(spawnedActor.PathableTiles), false, cellValidator, func, 0.25f);
					if (randomWeightedAvailableCell == null)
					{
						Debug.LogError("Screen Edge Spawn FAILED!", spawnedActor);
						UnityEngine.Object.Destroy(spawnedActor);
					}
					else
					{
						spawnedActor.transform.position = Pathfinder.GetClearanceOffset(randomWeightedAvailableCell.Value, spawnedActor.Clearance) - vector4;
						spawnedActor.specRigidbody.Reinitialize();
						this.HandleSpawn(spawnedActor);
					}
				}
			}
		}
		else if (this.spawnPosition == SpawnEnemyOnDeath.SpawnPosition.InsideRadius)
		{
			Vector2 unitCenter = base.specRigidbody.GetUnitCenter(ColliderType.HitBox);
			List<SpeculativeRigidbody> list2 = new List<SpeculativeRigidbody>();
			list2.Add(base.specRigidbody);
			for (int m = 0; m < selectedEnemyGuids.Length; m++)
			{
				Vector2 vector5 = unitCenter + UnityEngine.Random.insideUnitCircle * this.spawnRadius;
				if (GameManager.Instance.CurrentLevelOverrideState == GameManager.LevelOverrideState.CHARACTER_PAST && SceneManager.GetActiveScene().name == "fs_robot")
				{
					RoomHandler entrance = GameManager.Instance.Dungeon.data.Entrance;
					Vector2 vector6 = entrance.area.basePosition.ToVector2() + new Vector2(7f, 7f);
					Vector2 vector7 = entrance.area.basePosition.ToVector2() + new Vector2(38f, 36f);
					vector5 = Vector2.Min(vector7, Vector2.Max(vector6, vector5));
				}
				AIActor orLoadByGuid3 = EnemyDatabase.GetOrLoadByGuid(selectedEnemyGuids[m]);
				AIActor aiactor2 = AIActor.Spawn(orLoadByGuid3, unitCenter.ToIntVector2(VectorConversions.Floor), base.aiActor.ParentRoom, true, AIActor.AwakenAnimationType.Default, true);
				if (aiactor2)
				{
					aiactor2.specRigidbody.Initialize();
					Vector2 vector8 = vector5 - aiactor2.specRigidbody.GetUnitCenter(ColliderType.HitBox);
					aiactor2.specRigidbody.ImpartedPixelsToMove = PhysicsEngine.UnitToPixel(vector8);
					this.HandleSpawn(aiactor2);
					list2.Add(aiactor2.specRigidbody);
				}
			}
			for (int n = 0; n < list2.Count; n++)
			{
				for (int num = 0; num < list2.Count; num++)
				{
					if (n != num)
					{
						list2[n].RegisterGhostCollisionException(list2[num]);
					}
				}
			}
		}
		else
		{
			Debug.LogError("Unknown spawn type: " + this.spawnPosition);
		}
	}

	// Token: 0x06005E86 RID: 24198 RVA: 0x002449CC File Offset: 0x00242BCC
	private void HandleSpawn(AIActor spawnedActor)
	{
		if (!string.IsNullOrEmpty(this.spawnAnim))
		{
			spawnedActor.aiAnimator.PlayUntilFinished(this.spawnAnim, false, null, -1f, false);
		}
		SpawnEnemyOnDeath component = spawnedActor.GetComponent<SpawnEnemyOnDeath>();
		if (component)
		{
			component.guaranteedSpawnGenerations = Mathf.Max(0f, this.guaranteedSpawnGenerations - 1f);
		}
		if (!this.spawnsCanDropLoot)
		{
			spawnedActor.CanDropCurrency = false;
			spawnedActor.CanDropItems = false;
		}
		if (this.DoNormalReinforcement)
		{
			spawnedActor.HandleReinforcementFallIntoRoom(0.1f);
		}
	}

	// Token: 0x04005898 RID: 22680
	public float chanceToSpawn = 1f;

	// Token: 0x04005899 RID: 22681
	public string spawnVfx;

	// Token: 0x0400589A RID: 22682
	[Header("Enemies to Spawn")]
	public SpawnEnemyOnDeath.EnemySelection enemySelection = SpawnEnemyOnDeath.EnemySelection.All;

	// Token: 0x0400589B RID: 22683
	[EnemyIdentifier]
	public string[] enemyGuidsToSpawn;

	// Token: 0x0400589C RID: 22684
	[ShowInInspectorIf("ShowRandomPrams", true)]
	public int minSpawnCount = 1;

	// Token: 0x0400589D RID: 22685
	[ShowInInspectorIf("ShowRandomPrams", true)]
	public int maxSpawnCount = 1;

	// Token: 0x0400589E RID: 22686
	[FormerlySerializedAs("spawnType")]
	[Header("Placement")]
	public SpawnEnemyOnDeath.SpawnPosition spawnPosition;

	// Token: 0x0400589F RID: 22687
	[ShowInInspectorIf("ShowInsideColliderParams", true)]
	public int extraPixelWidth;

	// Token: 0x040058A0 RID: 22688
	[ShowInInspectorIf("ShowInsideRadiusParams", true)]
	public float spawnRadius = 1f;

	// Token: 0x040058A1 RID: 22689
	[Header("Spawn Parameters")]
	public float guaranteedSpawnGenerations;

	// Token: 0x040058A2 RID: 22690
	public string spawnAnim = "awaken";

	// Token: 0x040058A3 RID: 22691
	public bool spawnsCanDropLoot = true;

	// Token: 0x040058A4 RID: 22692
	public bool DoNormalReinforcement;

	// Token: 0x040058A5 RID: 22693
	private bool m_hasTriggered;

	// Token: 0x020010C1 RID: 4289
	public enum EnemySelection
	{
		// Token: 0x040058A7 RID: 22695
		All = 10,
		// Token: 0x040058A8 RID: 22696
		Random = 20
	}

	// Token: 0x020010C2 RID: 4290
	public enum SpawnPosition
	{
		// Token: 0x040058AA RID: 22698
		InsideCollider,
		// Token: 0x040058AB RID: 22699
		ScreenEdge,
		// Token: 0x040058AC RID: 22700
		InsideRadius = 20
	}
}
