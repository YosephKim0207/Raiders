using System;
using System.Collections.Generic;
using Dungeonator;
using FullInspector;
using Pathfinding;
using UnityEngine;

// Token: 0x02000D61 RID: 3425
public class SummonEnemyBehavior : BasicAttackBehavior
{
	// Token: 0x0600485F RID: 18527 RVA: 0x0017F174 File Offset: 0x0017D374
	private bool ShowCraze()
	{
		return this.MaxToSpawn > 0;
	}

	// Token: 0x06004860 RID: 18528 RVA: 0x0017F180 File Offset: 0x0017D380
	public override void Start()
	{
		base.Start();
		this.m_crazeBehavior = this.m_aiActor.GetComponent<CrazedController>();
	}

	// Token: 0x06004861 RID: 18529 RVA: 0x0017F19C File Offset: 0x0017D39C
	public override void Upkeep()
	{
		base.Upkeep();
		base.DecrementTimer(ref this.m_timer, false);
		if (this.MaxToSpawn > 0 && this.m_lifetimeSpawnCount >= this.MaxToSpawn && this.CrazeAfterMaxSpawned && this.m_crazeBehavior != null)
		{
			this.m_crazeBehavior.GoCrazed();
		}
		for (int i = this.m_allSpawnedActors.Count - 1; i >= 0; i--)
		{
			if (!this.m_allSpawnedActors[i] || !this.m_allSpawnedActors[i].healthHaver || this.m_allSpawnedActors[i].healthHaver.IsDead)
			{
				this.m_allSpawnedActors.RemoveAt(i);
			}
		}
	}

	// Token: 0x06004862 RID: 18530 RVA: 0x0017F278 File Offset: 0x0017D478
	public override BehaviorResult Update()
	{
		BehaviorResult behaviorResult = base.Update();
		if (behaviorResult != BehaviorResult.Continue)
		{
			return behaviorResult;
		}
		if (!this.IsReady())
		{
			return BehaviorResult.Continue;
		}
		if (this.MaxRoomOccupancy >= 0 && this.m_aiActor.ParentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All).Count >= this.MaxRoomOccupancy)
		{
			return BehaviorResult.Continue;
		}
		this.PrepareSpawn();
		IntVector2? spawnCell = this.m_spawnCell;
		if (spawnCell == null)
		{
			return BehaviorResult.Continue;
		}
		if (!string.IsNullOrEmpty(this.SummonAnim))
		{
			this.m_aiAnimator.PlayUntilFinished(this.SummonAnim, true, null, -1f, false);
			if (this.StopDuringAnimation)
			{
				if (this.HideGun)
				{
					this.m_aiShooter.ToggleGunAndHandRenderers(false, "SummonEnemyBehavior");
				}
				this.m_aiActor.ClearPath();
			}
		}
		if (!string.IsNullOrEmpty(this.SummonVfx))
		{
			this.m_aiAnimator.PlayVfx(this.SummonVfx, null, null, null);
		}
		if (!string.IsNullOrEmpty(this.TargetVfx))
		{
			AIAnimator aiAnimator = this.m_aiAnimator;
			string targetVfx = this.TargetVfx;
			Vector2? vector = new Vector2?(Pathfinder.GetClearanceOffset(this.m_spawnCell.Value, this.m_enemyClearance));
			aiAnimator.PlayVfx(targetVfx, null, null, vector);
		}
		this.m_timer = this.SummonTime;
		this.m_spawnCount = 0;
		if (this.m_aiActor && this.m_aiActor.knockbackDoer)
		{
			this.m_aiActor.knockbackDoer.SetImmobile(true, "SummonEnemyBehavior");
		}
		this.m_numToSpawn = this.NumToSpawn;
		if (this.MaxRoomOccupancy >= 0)
		{
			this.m_numToSpawn = Mathf.Min(this.m_numToSpawn, this.MaxRoomOccupancy - this.m_aiActor.ParentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All).Count);
		}
		if (this.MaxSummonedAtOnce >= 0)
		{
			this.m_numToSpawn = Mathf.Min(this.m_numToSpawn, this.MaxSummonedAtOnce - this.m_allSpawnedActors.Count);
		}
		if (this.MaxToSpawn >= 0)
		{
			this.m_numToSpawn = Mathf.Min(this.m_numToSpawn, this.MaxToSpawn - this.m_lifetimeSpawnCount);
		}
		this.m_state = SummonEnemyBehavior.State.Summoning;
		this.m_updateEveryFrame = true;
		return BehaviorResult.RunContinuous;
	}

	// Token: 0x06004863 RID: 18531 RVA: 0x0017F4D8 File Offset: 0x0017D6D8
	public override ContinuousBehaviorResult ContinuousUpdate()
	{
		if (this.m_state == SummonEnemyBehavior.State.Summoning)
		{
			if (this.m_timer <= 0f)
			{
				this.m_spawnedActor = AIActor.Spawn(this.m_enemyPrefab, this.m_spawnCell.Value, this.m_aiActor.ParentRoom, false, AIActor.AwakenAnimationType.Spawn, true);
				this.m_spawnedActor.aiAnimator.PlayDefaultSpawnState();
				this.m_allSpawnedActors.Add(this.m_spawnedActor);
				this.m_spawnedActor.CanDropCurrency = false;
				if (this.OverrideCorpse != null)
				{
					this.m_spawnedActor.CorpseObject = this.OverrideCorpse;
				}
				if (this.BlackPhantomChance > 0f && (this.BlackPhantomChance >= 1f || UnityEngine.Random.value < this.BlackPhantomChance))
				{
					this.m_spawnedActor.ForceBlackPhantom = true;
				}
				this.m_spawnCount++;
				this.m_lifetimeSpawnCount++;
				if (this.m_spawnCount < this.m_numToSpawn)
				{
					this.PrepareSpawn();
					IntVector2? spawnCell = this.m_spawnCell;
					if (spawnCell != null)
					{
						if (!string.IsNullOrEmpty(this.TargetVfx))
						{
							if (this.TargetVfxLoops)
							{
								this.m_aiAnimator.StopVfx(this.TargetVfx);
							}
							AIAnimator aiAnimator = this.m_aiAnimator;
							string targetVfx = this.TargetVfx;
							Vector2? vector = new Vector2?(Pathfinder.GetClearanceOffset(this.m_spawnCell.Value, this.m_enemyClearance));
							aiAnimator.PlayVfx(targetVfx, null, null, vector);
						}
						this.m_timer = this.SummonTime;
						return ContinuousBehaviorResult.Continue;
					}
				}
				this.m_spawnClip = this.m_spawnedActor.spriteAnimator.CurrentClip;
				if (this.m_spawnClip != null && this.m_spawnClip.wrapMode != tk2dSpriteAnimationClip.WrapMode.Loop)
				{
					this.m_state = SummonEnemyBehavior.State.WaitingForSummonAnim;
					return ContinuousBehaviorResult.Continue;
				}
				if (!string.IsNullOrEmpty(this.PostSummonAnim))
				{
					this.m_state = SummonEnemyBehavior.State.WaitingForPostAnim;
					this.m_aiAnimator.PlayUntilFinished(this.PostSummonAnim, false, null, -1f, false);
					return ContinuousBehaviorResult.Continue;
				}
				return ContinuousBehaviorResult.Finished;
			}
		}
		else if (this.m_state == SummonEnemyBehavior.State.WaitingForSummonAnim)
		{
			if (!this.m_spawnedActor || !this.m_spawnedActor.healthHaver || this.m_spawnedActor.healthHaver.IsDead || !this.m_spawnedActor.spriteAnimator.IsPlaying(this.m_spawnClip))
			{
				if (!string.IsNullOrEmpty(this.PostSummonAnim))
				{
					this.m_state = SummonEnemyBehavior.State.WaitingForPostAnim;
					this.m_aiAnimator.PlayUntilFinished(this.PostSummonAnim, false, null, -1f, false);
					return ContinuousBehaviorResult.Continue;
				}
				return ContinuousBehaviorResult.Finished;
			}
		}
		else if (this.m_state == SummonEnemyBehavior.State.WaitingForPostAnim && !this.m_aiActor.spriteAnimator.IsPlaying(this.PostSummonAnim))
		{
			return ContinuousBehaviorResult.Finished;
		}
		return ContinuousBehaviorResult.Continue;
	}

	// Token: 0x06004864 RID: 18532 RVA: 0x0017F7B4 File Offset: 0x0017D9B4
	public override void EndContinuousUpdate()
	{
		base.EndContinuousUpdate();
		if (!string.IsNullOrEmpty(this.SummonAnim))
		{
			this.m_aiAnimator.EndAnimationIf(this.SummonAnim);
		}
		if (!string.IsNullOrEmpty(this.SummonVfx))
		{
			this.m_aiAnimator.StopVfx(this.SummonVfx);
		}
		if (!string.IsNullOrEmpty(this.TargetVfx) && this.TargetVfxLoops)
		{
			this.m_aiAnimator.StopVfx(this.TargetVfx);
		}
		if (!string.IsNullOrEmpty(this.PostSummonAnim))
		{
			this.m_aiAnimator.EndAnimationIf(this.PostSummonAnim);
		}
		if (this.HideGun)
		{
			this.m_aiShooter.ToggleGunAndHandRenderers(true, "SummonEnemyBehavior");
		}
		if (this.m_aiActor && this.m_aiActor.knockbackDoer)
		{
			this.m_aiActor.knockbackDoer.SetImmobile(false, "SummonEnemyBehavior");
		}
		this.m_state = SummonEnemyBehavior.State.Idle;
		this.m_updateEveryFrame = false;
		this.UpdateCooldowns();
	}

	// Token: 0x06004865 RID: 18533 RVA: 0x0017F8C4 File Offset: 0x0017DAC4
	public override bool IsReady()
	{
		return (this.MaxToSpawn <= 0 || this.m_lifetimeSpawnCount < this.MaxToSpawn) && (this.MaxSummonedAtOnce <= 0 || this.m_allSpawnedActors.Count < this.MaxSummonedAtOnce) && base.IsReady();
	}

	// Token: 0x06004866 RID: 18534 RVA: 0x0017F91C File Offset: 0x0017DB1C
	public override void OnActorPreDeath()
	{
		if (this.KillSpawnedOnDeath)
		{
			for (int i = 0; i < this.m_allSpawnedActors.Count; i++)
			{
				AIActor aiactor = this.m_allSpawnedActors[i];
				if (aiactor && aiactor.healthHaver && aiactor.healthHaver.IsAlive)
				{
					aiactor.healthHaver.ApplyDamage(10000f, Vector2.zero, "Summoner Death", CoreDamageTypes.None, DamageCategory.Unstoppable, false, null, false);
				}
			}
		}
	}

	// Token: 0x06004867 RID: 18535 RVA: 0x0017F9A8 File Offset: 0x0017DBA8
	private void PrepareSpawn()
	{
		this.m_enemyGuid = ((this.selectionType != SummonEnemyBehavior.SelectionType.Ordered) ? BraveUtility.RandomElement<string>(this.EnemeyGuids) : this.EnemeyGuids[this.m_lifetimeSpawnCount]);
		this.m_enemyPrefab = EnemyDatabase.GetOrLoadByGuid(this.m_enemyGuid);
		PixelCollider groundPixelCollider = this.m_enemyPrefab.specRigidbody.GroundPixelCollider;
		if (groundPixelCollider != null && groundPixelCollider.ColliderGenerationMode == PixelCollider.PixelColliderGeneration.Manual)
		{
			this.m_enemyClearance = new Vector2((float)groundPixelCollider.ManualWidth / 16f, (float)groundPixelCollider.ManualHeight / 16f).ToIntVector2(VectorConversions.Ceil);
		}
		else
		{
			Debug.LogFormat("Enemy type {0} does not have a manually defined ground collider!", new object[] { this.m_enemyPrefab.name });
			this.m_enemyClearance = IntVector2.One;
		}
		Vector2 vector = BraveUtility.ViewportToWorldpoint(new Vector2(0f, 0f), ViewportType.Gameplay);
		Vector2 vector2 = BraveUtility.ViewportToWorldpoint(new Vector2(1f, 1f), ViewportType.Gameplay);
		IntVector2 bottomLeft = vector.ToIntVector2(VectorConversions.Ceil);
		IntVector2 topRight = vector2.ToIntVector2(VectorConversions.Floor) - IntVector2.One;
		Vector2 center = this.m_aiActor.specRigidbody.GetUnitCenter(ColliderType.Ground);
		float minDistanceSquared = this.MinSpawnRadius * this.MinSpawnRadius;
		float maxDistanceSquared = this.MaxSpawnRadius * this.MaxSpawnRadius;
		CellValidator cellValidator = delegate(IntVector2 c)
		{
			for (int i = 0; i < this.m_enemyClearance.x; i++)
			{
				for (int j = 0; j < this.m_enemyClearance.y; j++)
				{
					if (GameManager.Instance.Dungeon.data.isTopWall(c.x + i, c.y + j))
					{
						return false;
					}
					if (this.ManuallyDefineRoom && ((float)(c.x + i) < this.roomMin.x || (float)(c.x + i) > this.roomMax.x || (float)(c.y + j) < this.roomMin.y || (float)(c.y + j) > this.roomMax.y))
					{
						return false;
					}
				}
			}
			if (this.DefineSpawnRadius)
			{
				float num = (float)c.x + 0.5f - center.x;
				float num2 = (float)c.y + 0.5f - center.y;
				float num3 = num * num + num2 * num2;
				if (num3 < minDistanceSquared || num3 > maxDistanceSquared)
				{
					return false;
				}
			}
			else if (c.x < bottomLeft.x || c.y < bottomLeft.y || c.x + this.m_aiActor.Clearance.x - 1 > topRight.x || c.y + this.m_aiActor.Clearance.y - 1 > topRight.y)
			{
				return false;
			}
			return true;
		};
		this.m_spawnCell = this.m_aiActor.ParentRoom.GetRandomAvailableCell(new IntVector2?(this.m_enemyClearance), new CellTypes?(this.m_enemyPrefab.PathableTiles), true, cellValidator);
	}

	// Token: 0x04003C00 RID: 15360
	public bool DefineSpawnRadius;

	// Token: 0x04003C01 RID: 15361
	[InspectorShowIf("DefineSpawnRadius")]
	[InspectorIndent]
	public float MinSpawnRadius;

	// Token: 0x04003C02 RID: 15362
	[InspectorShowIf("DefineSpawnRadius")]
	[InspectorIndent]
	public float MaxSpawnRadius;

	// Token: 0x04003C03 RID: 15363
	public int MaxRoomOccupancy = -1;

	// Token: 0x04003C04 RID: 15364
	public int MaxSummonedAtOnce = -1;

	// Token: 0x04003C05 RID: 15365
	public int MaxToSpawn = -1;

	// Token: 0x04003C06 RID: 15366
	public int NumToSpawn = 1;

	// Token: 0x04003C07 RID: 15367
	public bool KillSpawnedOnDeath;

	// Token: 0x04003C08 RID: 15368
	[InspectorShowIf("ShowCraze")]
	[InspectorIndent]
	public bool CrazeAfterMaxSpawned;

	// Token: 0x04003C09 RID: 15369
	public float BlackPhantomChance;

	// Token: 0x04003C0A RID: 15370
	public List<string> EnemeyGuids;

	// Token: 0x04003C0B RID: 15371
	public SummonEnemyBehavior.SelectionType selectionType;

	// Token: 0x04003C0C RID: 15372
	public GameObject OverrideCorpse;

	// Token: 0x04003C0D RID: 15373
	public float SummonTime;

	// Token: 0x04003C0E RID: 15374
	public bool DisableDrops = true;

	// Token: 0x04003C0F RID: 15375
	public bool HideGun;

	// Token: 0x04003C10 RID: 15376
	[InspectorCategory("Visuals")]
	public bool StopDuringAnimation = true;

	// Token: 0x04003C11 RID: 15377
	[InspectorCategory("Visuals")]
	public string SummonAnim;

	// Token: 0x04003C12 RID: 15378
	[InspectorCategory("Visuals")]
	public string SummonVfx;

	// Token: 0x04003C13 RID: 15379
	[InspectorCategory("Visuals")]
	public string TargetVfx;

	// Token: 0x04003C14 RID: 15380
	[InspectorCategory("Visuals")]
	public bool TargetVfxLoops = true;

	// Token: 0x04003C15 RID: 15381
	[InspectorCategory("Visuals")]
	public string PostSummonAnim;

	// Token: 0x04003C16 RID: 15382
	public bool ManuallyDefineRoom;

	// Token: 0x04003C17 RID: 15383
	[InspectorIndent]
	[InspectorShowIf("ManuallyDefineRoom")]
	public Vector2 roomMin;

	// Token: 0x04003C18 RID: 15384
	[InspectorIndent]
	[InspectorShowIf("ManuallyDefineRoom")]
	public Vector2 roomMax;

	// Token: 0x04003C19 RID: 15385
	private SummonEnemyBehavior.State m_state;

	// Token: 0x04003C1A RID: 15386
	private string m_enemyGuid;

	// Token: 0x04003C1B RID: 15387
	private AIActor m_enemyPrefab;

	// Token: 0x04003C1C RID: 15388
	private IntVector2? m_spawnCell;

	// Token: 0x04003C1D RID: 15389
	private float m_timer;

	// Token: 0x04003C1E RID: 15390
	private AIActor m_spawnedActor;

	// Token: 0x04003C1F RID: 15391
	private tk2dSpriteAnimationClip m_spawnClip;

	// Token: 0x04003C20 RID: 15392
	private IntVector2 m_enemyClearance;

	// Token: 0x04003C21 RID: 15393
	private List<AIActor> m_allSpawnedActors = new List<AIActor>();

	// Token: 0x04003C22 RID: 15394
	private int m_numToSpawn;

	// Token: 0x04003C23 RID: 15395
	private int m_spawnCount;

	// Token: 0x04003C24 RID: 15396
	private int m_lifetimeSpawnCount;

	// Token: 0x04003C25 RID: 15397
	private CrazedController m_crazeBehavior;

	// Token: 0x02000D62 RID: 3426
	public enum SelectionType
	{
		// Token: 0x04003C27 RID: 15399
		Random,
		// Token: 0x04003C28 RID: 15400
		Ordered
	}

	// Token: 0x02000D63 RID: 3427
	private enum State
	{
		// Token: 0x04003C2A RID: 15402
		Idle,
		// Token: 0x04003C2B RID: 15403
		Summoning,
		// Token: 0x04003C2C RID: 15404
		WaitingForSummonAnim,
		// Token: 0x04003C2D RID: 15405
		WaitingForPostAnim
	}
}
