using System;
using System.Collections;
using System.Collections.Generic;
using Dungeonator;
using Pathfinding;
using UnityEngine;

// Token: 0x02001688 RID: 5768
public class TigerProjectile : Projectile
{
	// Token: 0x06008680 RID: 34432 RVA: 0x0037ADB0 File Offset: 0x00378FB0
	public override void Start()
	{
		base.Start();
		this.hitEffects.HasProjectileDeathVFX = false;
		base.specRigidbody.CollideWithTileMap = false;
		this.OnHitEnemy = (Action<Projectile, SpeculativeRigidbody, bool>)Delegate.Combine(this.OnHitEnemy, new Action<Projectile, SpeculativeRigidbody, bool>(this.HandleHitEnemy));
		this.OnWillKillEnemy = (Action<Projectile, SpeculativeRigidbody>)Delegate.Combine(this.OnWillKillEnemy, new Action<Projectile, SpeculativeRigidbody>(this.MaybeEatEnemy));
	}

	// Token: 0x06008681 RID: 34433 RVA: 0x0037AE20 File Offset: 0x00379020
	private void HandleHitEnemy(Projectile arg1, SpeculativeRigidbody arg2, bool arg3)
	{
		base.StartCoroutine(this.FrameDelayedDestruction());
	}

	// Token: 0x06008682 RID: 34434 RVA: 0x0037AE30 File Offset: 0x00379030
	private IEnumerator FrameDelayedDestruction()
	{
		yield return null;
		base.DieInAir(false, true, true, false);
		yield break;
	}

	// Token: 0x06008683 RID: 34435 RVA: 0x0037AE4C File Offset: 0x0037904C
	private void MaybeEatEnemy(Projectile selfProjectile, SpeculativeRigidbody enemyRigidbody)
	{
		if (!enemyRigidbody)
		{
			return;
		}
		AIActor aiActor = enemyRigidbody.aiActor;
		if (!aiActor || !aiActor.healthHaver || aiActor.healthHaver.IsBoss)
		{
			return;
		}
		this.m_cachedTargetToEat = aiActor;
		aiActor.healthHaver.ManualDeathHandling = true;
		aiActor.healthHaver.OnPreDeath += this.EatEnemy;
	}

	// Token: 0x06008684 RID: 34436 RVA: 0x0037AEC4 File Offset: 0x003790C4
	private void EatEnemy(Vector2 dirVec)
	{
		if (this.m_cachedTargetToEat && (!this.m_cachedTargetToEat.healthHaver || !this.m_cachedTargetToEat.healthHaver.IsBoss))
		{
			this.m_cachedTargetToEat.ForceDeath(Vector2.zero, false);
			this.m_cachedTargetToEat.StartCoroutine(this.HandleEat(this.m_cachedTargetToEat));
			this.m_cachedTargetToEat = null;
		}
	}

	// Token: 0x06008685 RID: 34437 RVA: 0x0037AF3C File Offset: 0x0037913C
	private IEnumerator HandleEat(AIActor targetEat)
	{
		float ela = 0f;
		float duration = 1f;
		while (ela < duration)
		{
			ela += BraveTime.DeltaTime;
			if (targetEat && targetEat.behaviorSpeculator)
			{
				targetEat.behaviorSpeculator.Interrupt();
				targetEat.ClearPath();
				targetEat.behaviorSpeculator.Stun(1f, true);
			}
			yield return null;
		}
		UnityEngine.Object.Destroy(targetEat.gameObject);
		yield break;
	}

	// Token: 0x06008686 RID: 34438 RVA: 0x0037AF58 File Offset: 0x00379158
	private IEnumerator FindTarget()
	{
		this.m_coroutineIsActive = true;
		bool ownerIsPlayer = base.Owner is PlayerController;
		for (;;)
		{
			if (ownerIsPlayer)
			{
				List<AIActor> activeEnemies = GameManager.Instance.Dungeon.data.GetAbsoluteRoomFromPosition(base.Owner.transform.position.IntXY(VectorConversions.Floor)).GetActiveEnemies(RoomHandler.ActiveEnemyType.All);
				if (activeEnemies != null)
				{
					float num = float.MaxValue;
					for (int i = 0; i < activeEnemies.Count; i++)
					{
						AIActor aiactor = activeEnemies[i];
						if (aiactor)
						{
							if (!aiactor.healthHaver.IsDead)
							{
								float num2 = Vector2.Distance(aiactor.specRigidbody.UnitCenter, base.Owner.specRigidbody.UnitCenter);
								if (num2 < num)
								{
									this.CurrentTarget = aiactor;
									num = num2;
								}
							}
						}
					}
				}
			}
			else
			{
				this.CurrentTarget = GameManager.Instance.GetPlayerClosestToPoint(base.transform.position.XY());
			}
			yield return new WaitForSeconds(1f);
		}
		yield break;
	}

	// Token: 0x06008687 RID: 34439 RVA: 0x0037AF74 File Offset: 0x00379174
	protected override void Move()
	{
		if (!this.m_coroutineIsActive)
		{
			base.StartCoroutine(this.FindTarget());
		}
		this.m_moveElapsed += BraveTime.DeltaTime;
		this.m_pathTimer -= BraveTime.DeltaTime;
		if (!base.specRigidbody.CollideWithTileMap && this.m_moveElapsed > 0.5f)
		{
			this.m_moveElapsed = 0f;
			base.specRigidbody.CollideWithTileMap = true;
			if (PhysicsEngine.Instance.OverlapCast(base.specRigidbody, null, true, false, null, null, false, null, null, new SpeculativeRigidbody[0]))
			{
				base.specRigidbody.CollideWithTileMap = false;
			}
		}
		float num = 1f;
		if (this.CurrentTarget != null)
		{
			this.m_noTargetElapsed = 0f;
			if (this.m_currentPath == null || this.m_pathTimer <= 0f)
			{
				bool path = Pathfinder.Instance.GetPath(base.specRigidbody.Position.UnitPosition.ToIntVector2(VectorConversions.Floor), this.CurrentTarget.specRigidbody.UnitCenter.ToIntVector2(VectorConversions.Ceil), out this.m_currentPath, new IntVector2?(base.specRigidbody.UnitDimensions.ToIntVector2(VectorConversions.Ceil)), CellTypes.FLOOR | CellTypes.PIT, null, null, false);
				this.m_pathTimer = 0.25f;
				if (!path)
				{
					this.m_currentPath = null;
				}
				else
				{
					this.m_currentPath.Smooth(base.specRigidbody.Position.UnitPosition, base.specRigidbody.UnitDimensions / 2f, CellTypes.FLOOR | CellTypes.PIT, true, base.specRigidbody.UnitDimensions.ToIntVector2(VectorConversions.Ceil));
				}
			}
			if (this.m_currentPath != null && this.m_currentPath.Positions.Count > 2)
			{
				Vector2 normalized = (this.m_currentPath.GetSecondCenterVector2() - base.specRigidbody.UnitCenter).normalized;
				this.m_currentDirection = Vector3.RotateTowards(this.m_currentDirection, normalized, this.angularAcceleration * 0.017453292f * BraveTime.DeltaTime, 0f).XY().normalized;
			}
			else
			{
				Vector2 normalized2 = (this.CurrentTarget.specRigidbody.UnitCenter - base.specRigidbody.UnitCenter).normalized;
				this.m_currentDirection = Vector3.RotateTowards(this.m_currentDirection, normalized2, this.angularAcceleration * 0.017453292f * BraveTime.DeltaTime, 0f).XY().normalized;
			}
		}
		else
		{
			this.m_noTargetElapsed += BraveTime.DeltaTime;
		}
		if (this.m_noTargetElapsed > 3f)
		{
			base.DieInAir(true, true, true, false);
		}
		base.specRigidbody.Velocity = this.m_currentDirection * this.baseData.speed * num;
		DirectionalAnimation.Info info = this.animData.GetInfo(base.specRigidbody.Velocity, false);
		if (!base.sprite.spriteAnimator.IsPlaying(info.name))
		{
			base.sprite.spriteAnimator.Play(info.name);
		}
		base.LastVelocity = base.specRigidbody.Velocity;
	}

	// Token: 0x06008688 RID: 34440 RVA: 0x0037B2E8 File Offset: 0x003794E8
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x06008689 RID: 34441 RVA: 0x0037B2F0 File Offset: 0x003794F0
	public static GameObject SpawnVFXBehind(GameObject prefab, Vector3 position, Quaternion rotation, bool ignoresPools)
	{
		GameObject gameObject = SpawnManager.SpawnVFX(prefab, position, rotation, ignoresPools);
		tk2dBaseSprite component = gameObject.GetComponent<tk2dBaseSprite>();
		component.scale = TigerProjectile.m_lastAssignedScale;
		component.UpdateZDepth();
		return gameObject;
	}

	// Token: 0x0600868A RID: 34442 RVA: 0x0037B320 File Offset: 0x00379520
	protected override void HandleHitEffectsEnemy(SpeculativeRigidbody rigidbody, CollisionData lcr, bool playProjectileDeathVfx)
	{
		if (!rigidbody || !rigidbody.gameActor)
		{
			return;
		}
		Vector3 vector = rigidbody.UnitBottomCenter.ToVector3ZUp(0f);
		tk2dSprite component = this.hitEffects.deathEnemy.effects[0].effects[0].effect.GetComponent<tk2dSprite>();
		Vector3 vector2 = component.GetRelativePositionFromAnchor(tk2dBaseSprite.Anchor.MiddleCenter);
		vector -= vector2;
		float num = 0f;
		if (base.sprite)
		{
			TigerProjectile.m_lastAssignedScale = base.sprite.scale;
		}
		if (lcr.Contact.x > rigidbody.UnitCenter.x)
		{
			vector += new Vector3(1f, 2f, 0f) - Vector3.Scale(new Vector3(2f, 4f, 0f), TigerProjectile.m_lastAssignedScale - Vector3.one);
			this.hitEffects.deathEnemy.effects[0].SpawnAtPosition(vector, num, null, new Vector2?(lcr.Normal), new Vector2?(base.specRigidbody.Velocity), new float?(-3f), false, new VFXComplex.SpawnMethod(TigerProjectile.SpawnVFXBehind), null, false);
		}
		else
		{
			vector += new Vector3(-1f, 2f, 0f) - Vector3.Scale(new Vector3(2f, 4f, 0f), TigerProjectile.m_lastAssignedScale - Vector3.one);
			this.hitEffects.deathEnemy.effects[1].SpawnAtPosition(vector, num, null, new Vector2?(lcr.Normal), new Vector2?(base.specRigidbody.Velocity), new float?(-3f), false, new VFXComplex.SpawnMethod(TigerProjectile.SpawnVFXBehind), null, false);
		}
	}

	// Token: 0x04008B7D RID: 35709
	public DirectionalAnimation animData;

	// Token: 0x04008B7E RID: 35710
	public float angularAcceleration = 10f;

	// Token: 0x04008B7F RID: 35711
	[NonSerialized]
	protected GameActor CurrentTarget;

	// Token: 0x04008B80 RID: 35712
	[NonSerialized]
	protected bool m_coroutineIsActive;

	// Token: 0x04008B81 RID: 35713
	private AIActor m_cachedTargetToEat;

	// Token: 0x04008B82 RID: 35714
	private float m_moveElapsed;

	// Token: 0x04008B83 RID: 35715
	private float m_pathTimer;

	// Token: 0x04008B84 RID: 35716
	private Path m_currentPath;

	// Token: 0x04008B85 RID: 35717
	private static Vector3 m_lastAssignedScale = Vector3.one;

	// Token: 0x04008B86 RID: 35718
	private float m_noTargetElapsed;
}
