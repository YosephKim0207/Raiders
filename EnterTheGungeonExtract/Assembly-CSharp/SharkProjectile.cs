using System;
using System.Collections;
using System.Collections.Generic;
using Dungeonator;
using Pathfinding;
using UnityEngine;

// Token: 0x02001675 RID: 5749
public class SharkProjectile : Projectile
{
	// Token: 0x0600860B RID: 34315 RVA: 0x00376F6C File Offset: 0x0037516C
	public override void Start()
	{
		base.Start();
		base.StartCoroutine(this.AddLowObstacleCollider());
		SpeculativeRigidbody specRigidbody = base.specRigidbody;
		specRigidbody.OnPreRigidbodyCollision = (SpeculativeRigidbody.OnPreRigidbodyCollisionDelegate)Delegate.Combine(specRigidbody.OnPreRigidbodyCollision, new SpeculativeRigidbody.OnPreRigidbodyCollisionDelegate(this.PassThroughTables));
		RoomHandler absoluteRoomFromPosition = GameManager.Instance.Dungeon.data.GetAbsoluteRoomFromPosition(base.transform.position.XY().ToIntVector2(VectorConversions.Round));
		if (absoluteRoomFromPosition != null && absoluteRoomFromPosition.area.PrototypeRoomCategory == PrototypeDungeonRoom.RoomCategory.BOSS)
		{
			this.CanCrossPits = true;
		}
		if (!this.CanCrossPits)
		{
			SpeculativeRigidbody specRigidbody2 = base.specRigidbody;
			specRigidbody2.MovementRestrictor = (SpeculativeRigidbody.MovementRestrictorDelegate)Delegate.Combine(specRigidbody2.MovementRestrictor, new SpeculativeRigidbody.MovementRestrictorDelegate(this.NoPits));
		}
		this.OnHitEnemy = (Action<Projectile, SpeculativeRigidbody, bool>)Delegate.Combine(this.OnHitEnemy, new Action<Projectile, SpeculativeRigidbody, bool>(this.HandleHitEnemy));
		this.OnWillKillEnemy = (Action<Projectile, SpeculativeRigidbody>)Delegate.Combine(this.OnWillKillEnemy, new Action<Projectile, SpeculativeRigidbody>(this.MaybeEatEnemy));
	}

	// Token: 0x0600860C RID: 34316 RVA: 0x00377074 File Offset: 0x00375274
	private void PassThroughTables(SpeculativeRigidbody myRigidbody, PixelCollider myPixelCollider, SpeculativeRigidbody otherRigidbody, PixelCollider otherPixelCollider)
	{
		if (otherRigidbody.GetComponent<FlippableCover>())
		{
			PhysicsEngine.SkipCollision = true;
		}
	}

	// Token: 0x0600860D RID: 34317 RVA: 0x0037708C File Offset: 0x0037528C
	private IEnumerator AddLowObstacleCollider()
	{
		for (;;)
		{
			IntVector2 cellPos = base.transform.position.IntXY(VectorConversions.Round);
			IntVector2 aboveCellPos = cellPos + IntVector2.Up;
			bool shouldAddLayer = true;
			if (GameManager.Instance.Dungeon.data.CheckInBounds(cellPos) && GameManager.Instance.Dungeon.data[cellPos] != null && GameManager.Instance.Dungeon.data[cellPos].IsLowerFaceWall())
			{
				shouldAddLayer = false;
			}
			else if (GameManager.Instance.Dungeon.data.CheckInBounds(aboveCellPos) && GameManager.Instance.Dungeon.data[aboveCellPos] != null && GameManager.Instance.Dungeon.data[aboveCellPos].IsLowerFaceWall())
			{
				shouldAddLayer = false;
			}
			if (shouldAddLayer)
			{
				break;
			}
			yield return null;
		}
		base.specRigidbody.AddCollisionLayerIgnoreOverride(CollisionMask.LayerToMask(CollisionLayer.LowObstacle));
		yield break;
	}

	// Token: 0x0600860E RID: 34318 RVA: 0x003770A8 File Offset: 0x003752A8
	private void HandleHitEnemy(Projectile arg1, SpeculativeRigidbody arg2, bool arg3)
	{
		if (base.Owner && base.Owner is PlayerController && (base.Owner as PlayerController).HasActiveBonusSynergy(CustomSynergyType.EXPLOSIVE_SHARKS, false))
		{
			Exploder.DoDefaultExplosion(base.specRigidbody.UnitCenter.ToVector3ZisY(0f), Vector2.zero, null, false, CoreDamageTypes.None, false);
		}
		base.StartCoroutine(this.FrameDelayedDestruction());
	}

	// Token: 0x0600860F RID: 34319 RVA: 0x00377120 File Offset: 0x00375320
	private IEnumerator FrameDelayedDestruction()
	{
		yield return null;
		base.DieInAir(false, true, true, false);
		yield break;
	}

	// Token: 0x06008610 RID: 34320 RVA: 0x0037713C File Offset: 0x0037533C
	private void NoPits(SpeculativeRigidbody specRigidbody, IntVector2 prevPixelOffset, IntVector2 pixelOffset, ref bool validLocation)
	{
		if (!validLocation)
		{
			return;
		}
		Func<IntVector2, bool> func = delegate(IntVector2 pixel)
		{
			Vector2 vector = PhysicsEngine.PixelToUnitMidpoint(pixel);
			if (!GameManager.Instance.Dungeon.CellSupportsFalling(vector))
			{
				return false;
			}
			List<SpeculativeRigidbody> platformsAt = GameManager.Instance.Dungeon.GetPlatformsAt(vector);
			if (platformsAt != null)
			{
				for (int i = 0; i < platformsAt.Count; i++)
				{
					if (platformsAt[i].PrimaryPixelCollider.ContainsPixel(pixel))
					{
						return false;
					}
				}
			}
			return true;
		};
		PixelCollider primaryPixelCollider = specRigidbody.PrimaryPixelCollider;
		if (primaryPixelCollider != null)
		{
			IntVector2 intVector = pixelOffset - prevPixelOffset;
			if (intVector == IntVector2.Down && func(primaryPixelCollider.LowerLeft + pixelOffset) && func(primaryPixelCollider.LowerRight + pixelOffset) && (!func(primaryPixelCollider.UpperRight + prevPixelOffset) || !func(primaryPixelCollider.UpperLeft + prevPixelOffset)))
			{
				validLocation = false;
			}
			else if (intVector == IntVector2.Right && func(primaryPixelCollider.LowerRight + pixelOffset) && func(primaryPixelCollider.UpperRight + pixelOffset) && (!func(primaryPixelCollider.UpperLeft + prevPixelOffset) || !func(primaryPixelCollider.LowerLeft + prevPixelOffset)))
			{
				validLocation = false;
			}
			else if (intVector == IntVector2.Up && func(primaryPixelCollider.UpperRight + pixelOffset) && func(primaryPixelCollider.UpperLeft + pixelOffset) && (!func(primaryPixelCollider.LowerLeft + prevPixelOffset) || !func(primaryPixelCollider.LowerRight + prevPixelOffset)))
			{
				validLocation = false;
			}
			else if (intVector == IntVector2.Left && func(primaryPixelCollider.UpperLeft + pixelOffset) && func(primaryPixelCollider.LowerLeft + pixelOffset) && (!func(primaryPixelCollider.LowerRight + prevPixelOffset) || !func(primaryPixelCollider.UpperRight + prevPixelOffset)))
			{
				validLocation = false;
			}
		}
		if (!validLocation)
		{
			this.ForceBounce((pixelOffset - prevPixelOffset).ToVector2());
		}
	}

	// Token: 0x06008611 RID: 34321 RVA: 0x00377374 File Offset: 0x00375574
	private void ForceBounce(Vector2 normal)
	{
		BounceProjModifier component = base.GetComponent<BounceProjModifier>();
		float num = (-base.specRigidbody.Velocity).ToAngle();
		float num2 = normal.ToAngle();
		float num3 = BraveMathCollege.ClampAngle360(num + 2f * (num2 - num));
		if (this.shouldRotate)
		{
			base.transform.Rotate(0f, 0f, num3 - num);
		}
		this.m_currentDirection = BraveMathCollege.DegreesToVector(num3, 1f);
		this.m_currentSpeed *= 1f - component.percentVelocityToLoseOnBounce;
		if (base.braveBulletScript && base.braveBulletScript.bullet != null)
		{
			base.braveBulletScript.bullet.Direction = num3;
			base.braveBulletScript.bullet.Speed *= 1f - component.percentVelocityToLoseOnBounce;
		}
		if (component != null)
		{
			component.Bounce(this, base.specRigidbody.UnitCenter, null);
		}
	}

	// Token: 0x06008612 RID: 34322 RVA: 0x00377478 File Offset: 0x00375678
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

	// Token: 0x06008613 RID: 34323 RVA: 0x003774F0 File Offset: 0x003756F0
	private void EatEnemy(Vector2 dirVec)
	{
		if (this.m_cachedTargetToEat)
		{
			this.m_cachedTargetToEat.ForceDeath(Vector2.zero, false);
			this.m_cachedTargetToEat.StartCoroutine(this.HandleEat(this.m_cachedTargetToEat));
		}
		this.m_cachedTargetToEat = null;
	}

	// Token: 0x06008614 RID: 34324 RVA: 0x00377540 File Offset: 0x00375740
	private IEnumerator HandleEat(AIActor targetEat)
	{
		float ela = 0f;
		float duration = 0.75f;
		while (ela < duration)
		{
			ela += BraveTime.DeltaTime;
			if (targetEat && targetEat.behaviorSpeculator)
			{
				targetEat.behaviorSpeculator.Interrupt();
				targetEat.ClearPath();
				targetEat.behaviorSpeculator.Stun(1f, false);
			}
			yield return null;
		}
		UnityEngine.Object.Destroy(targetEat.gameObject);
		yield break;
	}

	// Token: 0x06008615 RID: 34325 RVA: 0x0037755C File Offset: 0x0037575C
	private IEnumerator FindTarget()
	{
		this.m_coroutineIsActive = true;
		bool ownerIsPlayer = base.Owner is PlayerController;
		while (this && base.Owner && base.Owner.specRigidbody)
		{
			if (!GameManager.HasInstance || GameManager.Instance.IsLoadingLevel)
			{
				yield break;
			}
			if (ownerIsPlayer)
			{
				List<AIActor> activeEnemies = GameManager.Instance.Dungeon.data.GetAbsoluteRoomFromPosition(base.Owner.transform.position.IntXY(VectorConversions.Floor)).GetActiveEnemies(RoomHandler.ActiveEnemyType.All);
				if (activeEnemies != null)
				{
					float num = float.MaxValue;
					for (int i = 0; i < activeEnemies.Count; i++)
					{
						AIActor aiactor = activeEnemies[i];
						if (aiactor && aiactor.healthHaver && !aiactor.healthHaver.IsDead && aiactor.specRigidbody)
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
			else
			{
				this.CurrentTarget = GameManager.Instance.GetPlayerClosestToPoint(base.transform.position.XY());
			}
			yield return new WaitForSeconds(1f);
		}
		yield break;
		yield break;
	}

	// Token: 0x06008616 RID: 34326 RVA: 0x00377578 File Offset: 0x00375778
	protected override void Move()
	{
		if (!this.m_coroutineIsActive)
		{
			base.StartCoroutine(this.FindTarget());
		}
		float num = 1f;
		this.m_pathTimer -= base.LocalDeltaTime;
		if (base.sprite && base.sprite.HeightOffGround != -1f)
		{
			base.sprite.HeightOffGround = -1f;
			base.sprite.UpdateZDepth();
		}
		if (this.CurrentTarget != null)
		{
			if (this.m_pathTimer <= 0f || this.m_currentPath == null)
			{
				CellTypes cellTypes = CellTypes.FLOOR;
				if (this.CanCrossPits)
				{
					cellTypes |= CellTypes.PIT;
				}
				Pathfinder.Instance.GetPath(base.specRigidbody.UnitCenter.ToIntVector2(VectorConversions.Floor), this.CurrentTarget.specRigidbody.UnitCenter.ToIntVector2(VectorConversions.Floor), out this.m_currentPath, new IntVector2?(IntVector2.One), cellTypes, null, null, false);
				this.m_pathTimer = 0.5f;
				if (this.m_currentPath != null && this.m_currentPath.Count > 0 && this.m_currentPath.WillReachFinalGoal)
				{
					this.m_currentPath.Smooth(base.specRigidbody.UnitCenter, new Vector2(0.25f, 0.25f), cellTypes, false, IntVector2.One);
				}
			}
			Vector2 vector = Vector2.zero;
			if (this.m_currentPath != null && this.m_currentPath.WillReachFinalGoal && this.m_currentPath.Count > 0)
			{
				vector = this.GetPathVelocityContribution();
			}
			else
			{
				this.CurrentTarget = null;
			}
			this.m_noTargetElapsed = 0f;
			this.m_currentDirection = Vector3.RotateTowards(this.m_currentDirection, vector, this.angularAcceleration * 0.017453292f * BraveTime.DeltaTime, 0f).XY().normalized;
			float num2 = Vector2.Angle(this.m_currentDirection, vector);
			num = 0.25f + (1f - Mathf.Clamp01(Mathf.Abs(num2) / 60f)) * 0.75f;
		}
		else
		{
			this.m_noTargetElapsed += BraveTime.DeltaTime;
		}
		if (this.m_noTargetElapsed > 5f)
		{
			base.DieInAir(true, true, true, false);
		}
		base.specRigidbody.Velocity = this.m_currentDirection * this.m_currentSpeed * num;
		DirectionalAnimation.Info info = this.animData.GetInfo(base.specRigidbody.Velocity, false);
		if (!base.sprite.spriteAnimator.IsPlaying(info.name))
		{
			base.sprite.spriteAnimator.Play(info.name);
		}
		if (this.m_lastGoopPosition != null)
		{
			DeadlyDeadlyGoopManager.GetGoopManagerForGoopType(this.waterGoop).AddGoopLine(this.m_lastGoopPosition.Value, base.specRigidbody.UnitCenter, this.goopRadius);
		}
		this.m_lastGoopPosition = new Vector2?(base.specRigidbody.UnitCenter);
		base.LastVelocity = base.specRigidbody.Velocity;
	}

	// Token: 0x06008617 RID: 34327 RVA: 0x003778A4 File Offset: 0x00375AA4
	private bool GetNextTargetPosition(out Vector2 targetPos)
	{
		if (this.m_currentPath != null && this.m_currentPath.Count > 0)
		{
			targetPos = this.m_currentPath.GetFirstCenterVector2();
			return true;
		}
		Vector2? overridePathEnd = this.m_overridePathEnd;
		if (overridePathEnd != null)
		{
			targetPos = this.m_overridePathEnd.Value;
			return true;
		}
		targetPos = Vector2.zero;
		return false;
	}

	// Token: 0x06008618 RID: 34328 RVA: 0x00377914 File Offset: 0x00375B14
	private Vector2 GetPathTarget()
	{
		Vector2 unitCenter = base.specRigidbody.UnitCenter;
		Vector2 vector = unitCenter;
		float num = this.baseData.speed * base.LocalDeltaTime;
		Vector2 vector2 = unitCenter;
		Vector2 vector3 = unitCenter;
		while (num > 0f)
		{
			if (this.GetNextTargetPosition(out vector3))
			{
				float num2 = Vector2.Distance(vector3, unitCenter);
				if (num2 < num)
				{
					num -= num2;
					vector2 = vector3;
					vector = vector2;
					if (this.m_currentPath != null && this.m_currentPath.Count > 0)
					{
						this.m_currentPath.RemoveFirst();
					}
					else
					{
						this.m_overridePathEnd = null;
					}
					continue;
				}
				vector = (vector3 - vector2).normalized * num + vector2;
			}
			return vector;
		}
		return vector;
	}

	// Token: 0x06008619 RID: 34329 RVA: 0x003779F4 File Offset: 0x00375BF4
	private Vector2 GetPathVelocityContribution()
	{
		if (this.m_currentPath == null || this.m_currentPath.Count == 0)
		{
			Vector2? overridePathEnd = this.m_overridePathEnd;
			if (overridePathEnd == null)
			{
				return Vector2.zero;
			}
		}
		Vector2 unitCenter = base.specRigidbody.UnitCenter;
		Vector2 pathTarget = this.GetPathTarget();
		Vector2 vector = pathTarget - unitCenter;
		if (this.baseData.speed * base.LocalDeltaTime > vector.magnitude)
		{
			return vector / base.LocalDeltaTime;
		}
		return this.baseData.speed * vector.normalized;
	}

	// Token: 0x0600861A RID: 34330 RVA: 0x00377A98 File Offset: 0x00375C98
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x0600861B RID: 34331 RVA: 0x00377AA0 File Offset: 0x00375CA0
	public static GameObject SpawnVFXBehind(GameObject prefab, Vector3 position, Quaternion rotation, bool ignoresPools)
	{
		GameObject gameObject = SpawnManager.SpawnVFX(prefab, position, rotation, ignoresPools);
		tk2dBaseSprite component = gameObject.GetComponent<tk2dBaseSprite>();
		component.scale = SharkProjectile.m_lastAssignedScale;
		component.depthUsesTrimmedBounds = true;
		component.HeightOffGround = -3f;
		component.UpdateZDepth();
		return gameObject;
	}

	// Token: 0x0600861C RID: 34332 RVA: 0x00377AE4 File Offset: 0x00375CE4
	protected override void HandleHitEffectsEnemy(SpeculativeRigidbody rigidbody, CollisionData lcr, bool playProjectileDeathVfx)
	{
		if (this.hitEffects.alwaysUseMidair)
		{
			base.HandleHitEffectsMidair(false);
			return;
		}
		if (rigidbody.gameActor == null)
		{
			return;
		}
		Vector3 vector = rigidbody.UnitBottomCenter.ToVector3ZUp(0f);
		tk2dSprite component = this.hitEffects.enemy.effects[0].effects[0].effect.GetComponent<tk2dSprite>();
		Vector3 vector2 = component.GetRelativePositionFromAnchor(tk2dBaseSprite.Anchor.MiddleCenter);
		vector -= vector2;
		float num = 0f;
		if (base.sprite)
		{
			SharkProjectile.m_lastAssignedScale = base.sprite.scale;
		}
		if (rigidbody.healthHaver && rigidbody.healthHaver.GetCurrentHealth() <= 0f)
		{
			if (lcr.Contact.x > rigidbody.UnitCenter.x)
			{
				vector += new Vector3(1.125f, 0.5f, 0f) - Vector3.Scale(new Vector3(2.25f, 1f, 0f), SharkProjectile.m_lastAssignedScale - Vector3.one);
				this.hitEffects.enemy.effects[0].SpawnAtPosition(vector, num, null, new Vector2?(lcr.Normal), new Vector2?(base.specRigidbody.Velocity), new float?(-3f), false, new VFXComplex.SpawnMethod(SharkProjectile.SpawnVFXBehind), null, false);
			}
			else
			{
				vector += new Vector3(1.125f, 0.5f, 0f) - Vector3.Scale(new Vector3(2.25f, 1f, 0f), SharkProjectile.m_lastAssignedScale - Vector3.one);
				this.hitEffects.enemy.effects[1].SpawnAtPosition(vector, num, null, new Vector2?(lcr.Normal), new Vector2?(base.specRigidbody.Velocity), new float?(-3f), false, new VFXComplex.SpawnMethod(SharkProjectile.SpawnVFXBehind), null, false);
			}
			if (this.ParticlesPrefab != null)
			{
				ParticleSystem component2 = SpawnManager.SpawnParticleSystem(this.ParticlesPrefab.gameObject, rigidbody.UnitBottomCenter.ToVector3ZUp(rigidbody.UnitBottomCenter.y), Quaternion.identity).GetComponent<ParticleSystem>();
				component2.Play();
			}
		}
		else
		{
			this.enemyNotKilledPool.SpawnAtPosition(rigidbody.UnitCenter.ToVector3ZUp(0f), num, null, new Vector2?(lcr.Normal), new Vector2?(base.specRigidbody.Velocity), null, false, null, null, false);
		}
	}

	// Token: 0x04008AD1 RID: 35537
	public VFXPool enemyNotKilledPool;

	// Token: 0x04008AD2 RID: 35538
	public DirectionalAnimation animData;

	// Token: 0x04008AD3 RID: 35539
	public GoopDefinition waterGoop;

	// Token: 0x04008AD4 RID: 35540
	public float goopRadius = 1f;

	// Token: 0x04008AD5 RID: 35541
	public float angularAcceleration = 10f;

	// Token: 0x04008AD6 RID: 35542
	public ParticleSystem ParticlesPrefab;

	// Token: 0x04008AD7 RID: 35543
	[NonSerialized]
	protected GameActor CurrentTarget;

	// Token: 0x04008AD8 RID: 35544
	[NonSerialized]
	protected bool m_coroutineIsActive;

	// Token: 0x04008AD9 RID: 35545
	protected bool CanCrossPits;

	// Token: 0x04008ADA RID: 35546
	private AIActor m_cachedTargetToEat;

	// Token: 0x04008ADB RID: 35547
	protected Vector2? m_lastGoopPosition;

	// Token: 0x04008ADC RID: 35548
	protected Path m_currentPath;

	// Token: 0x04008ADD RID: 35549
	protected float m_pathTimer;

	// Token: 0x04008ADE RID: 35550
	protected Vector2? m_overridePathEnd;

	// Token: 0x04008ADF RID: 35551
	private static Vector3 m_lastAssignedScale = Vector3.one;

	// Token: 0x04008AE0 RID: 35552
	private float m_noTargetElapsed;
}
