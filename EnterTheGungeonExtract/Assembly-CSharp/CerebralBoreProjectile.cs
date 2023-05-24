using System;
using System.Collections.Generic;
using Dungeonator;
using UnityEngine;

// Token: 0x0200162C RID: 5676
public class CerebralBoreProjectile : Projectile
{
	// Token: 0x06008484 RID: 33924 RVA: 0x00368E8C File Offset: 0x0036708C
	public override void Start()
	{
		base.Start();
		SpeculativeRigidbody specRigidbody = base.specRigidbody;
		specRigidbody.OnPreRigidbodyCollision = (SpeculativeRigidbody.OnPreRigidbodyCollisionDelegate)Delegate.Combine(specRigidbody.OnPreRigidbodyCollision, new SpeculativeRigidbody.OnPreRigidbodyCollisionDelegate(this.ProcessPreCollision));
		this.AcquireTarget();
	}

	// Token: 0x06008485 RID: 33925 RVA: 0x00368EC4 File Offset: 0x003670C4
	private void ProcessPreCollision(SpeculativeRigidbody myRigidbody, PixelCollider myPixelCollider, SpeculativeRigidbody otherRigidbody, PixelCollider otherPixelCollider)
	{
		if (this.m_currentMotionType == CerebralBoreProjectile.BoreMotionType.TRACKING)
		{
			if (otherRigidbody.aiActor != null)
			{
				if (otherRigidbody.aiActor != this.m_targetEnemy && !this.m_rigidbodiesDamagedInFlight.Contains(otherRigidbody))
				{
					bool flag = false;
					this.HandleDamage(otherRigidbody, otherPixelCollider, out flag, null, false);
					this.m_rigidbodiesDamagedInFlight.Add(otherRigidbody);
				}
				PhysicsEngine.SkipCollision = true;
			}
		}
		else if (this.m_currentMotionType == CerebralBoreProjectile.BoreMotionType.BORING)
		{
			PhysicsEngine.SkipCollision = true;
		}
	}

	// Token: 0x06008486 RID: 33926 RVA: 0x00368F50 File Offset: 0x00367150
	protected void AcquireTarget()
	{
		this.m_startPosition = base.transform.position.XY();
		this.m_initialAimPoint = this.m_startPosition + base.transform.right.XY() * 3f;
		this.m_currentBezierPoint = 0f;
		Func<AIActor, bool> func = (AIActor targ) => !targ.UniquePlayerTargetFlag;
		if (base.Owner is PlayerController)
		{
			PlayerController playerController = base.Owner as PlayerController;
			if (playerController.CurrentRoom == null)
			{
				return;
			}
			List<AIActor> activeEnemies = playerController.CurrentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All);
			if (activeEnemies == null)
			{
				return;
			}
			this.m_targetEnemy = BraveUtility.GetClosestToPosition<AIActor>(activeEnemies, playerController.unadjustedAimPoint.XY(), func, new AIActor[0]);
		}
		else if (base.Owner)
		{
			List<AIActor> activeEnemies2 = base.Owner.GetAbsoluteParentRoom().GetActiveEnemies(RoomHandler.ActiveEnemyType.All);
			if (activeEnemies2 == null)
			{
				return;
			}
			this.m_targetEnemy = BraveUtility.GetClosestToPosition<AIActor>(activeEnemies2, base.transform.position.XY(), func, new AIActor[0]);
		}
		if (this.m_targetEnemy != null)
		{
			this.m_targetEnemy.UniquePlayerTargetFlag = true;
		}
	}

	// Token: 0x06008487 RID: 33927 RVA: 0x00369090 File Offset: 0x00367290
	protected override void Move()
	{
		if (this.m_targetEnemy == null || !this.m_targetEnemy)
		{
			this.AcquireTarget();
		}
		CerebralBoreProjectile.BoreMotionType currentMotionType = this.m_currentMotionType;
		if (currentMotionType != CerebralBoreProjectile.BoreMotionType.TRACKING)
		{
			if (currentMotionType == CerebralBoreProjectile.BoreMotionType.BORING)
			{
				this.HandleBoring();
			}
		}
		else
		{
			this.HandleTracking();
		}
	}

	// Token: 0x06008488 RID: 33928 RVA: 0x003690F4 File Offset: 0x003672F4
	protected void HandleBoring()
	{
		if (this.m_targetEnemy != null)
		{
			this.m_targetEnemy.UniquePlayerTargetFlag = false;
		}
		this.m_elapsedBore += BraveTime.DeltaTime;
		float num = Mathf.Clamp01(this.m_elapsedBore / this.boreTime);
		if (this.m_elapsedBore < this.boreTime && this.m_targetEnemy != null && this.m_targetEnemy && this.m_targetEnemy.healthHaver.IsAlive && this.m_targetEnemy.specRigidbody.CollideWithOthers)
		{
			if (!this.m_targetEnemy.healthHaver.IsBoss)
			{
				this.m_targetEnemy.ClearPath();
				if (this.m_targetEnemy.behaviorSpeculator.IsInterruptable)
				{
					this.m_targetEnemy.behaviorSpeculator.Interrupt();
				}
				this.m_targetEnemy.behaviorSpeculator.Stun(1f, true);
			}
			Vector2 vector = this.m_targetEnemy.specRigidbody.HitboxPixelCollider.UnitTopCenter;
			vector += new Vector2(0f, this.boreCurve.Evaluate(num));
			Vector2 vector2 = vector - base.transform.PositionVector2();
			base.specRigidbody.Velocity = vector2 / BraveTime.DeltaTime;
			base.LastVelocity = base.specRigidbody.Velocity;
			float num2 = BraveMathCollege.Atan2Degrees(Vector2.down);
			base.transform.rotation = Quaternion.Euler(0f, 0f, num2);
		}
		else
		{
			if (this.m_targetEnemy && this.m_targetEnemy.healthHaver.IsAlive)
			{
				this.m_targetEnemy.healthHaver.ApplyDamage(base.ModifiedDamage, Vector2.down, base.OwnerName, CoreDamageTypes.None, DamageCategory.Normal, false, null, false);
				Exploder.Explode(base.specRigidbody.UnitCenter.ToVector3ZUp(0f), this.explosionData, Vector2.up, null, false, CoreDamageTypes.None, false);
				this.m_hasExploded = true;
			}
			base.DieInAir(false, true, true, false);
		}
	}

	// Token: 0x06008489 RID: 33929 RVA: 0x00369318 File Offset: 0x00367518
	protected void HandleTracking()
	{
		float num = this.baseData.speed;
		if (this.m_targetEnemy != null)
		{
			this.m_bezierUpdateTimer -= BraveTime.DeltaTime;
			if (this.m_bezierUpdateTimer <= 0f)
			{
				this.m_bezierUpdateTimer = 0.1f;
				this.m_targetTrackingPoint = this.m_targetEnemy.sprite.WorldTopCenter;
			}
			Vector2 vector = this.m_startPosition + (this.m_initialAimPoint - this.m_startPosition).normalized * 5f;
			Vector2 vector2 = this.m_targetTrackingPoint + Vector2.up * 4f;
			IntVector2 intVector = vector2.ToIntVector2(VectorConversions.Floor);
			Func<IntVector2, bool> func = (IntVector2 pos) => GameManager.Instance.Dungeon.data[pos] == null || GameManager.Instance.Dungeon.data[pos].type == CellType.WALL;
			if (func(intVector + IntVector2.Down) || func(intVector + IntVector2.Down * 2))
			{
				vector2 = this.m_targetTrackingPoint + Vector2.down;
			}
			float num2 = BraveMathCollege.EstimateBezierPathLength(this.m_startPosition, vector, vector2, this.m_targetTrackingPoint, 20);
			float num3 = num2 / this.baseData.speed;
			float num4 = this.m_currentBezierPoint + BraveTime.DeltaTime * 2f / num3;
			this.m_currentBezierPoint += BraveTime.DeltaTime / num3;
			if (this.m_currentBezierPoint >= 1f)
			{
				base.specRigidbody.CollideWithTileMap = false;
				this.SparksSystem.gameObject.SetActive(true);
				this.m_currentMotionType = CerebralBoreProjectile.BoreMotionType.BORING;
			}
			Vector2 vector3 = BraveMathCollege.CalculateBezierPoint(num4, this.m_startPosition, vector, vector2, this.m_targetTrackingPoint);
			Vector2 vector4 = vector3 - base.transform.PositionVector2();
			num = Mathf.Min(num, vector4.magnitude / BraveTime.DeltaTime);
			float num5 = BraveMathCollege.Atan2Degrees(vector4);
			num5 = num5.Quantize(3f);
			base.transform.rotation = Quaternion.Euler(0f, 0f, num5);
		}
		base.specRigidbody.Velocity = base.transform.right * num;
		base.LastVelocity = base.specRigidbody.Velocity;
	}

	// Token: 0x0600848A RID: 33930 RVA: 0x0036956C File Offset: 0x0036776C
	protected override void OnDestroy()
	{
		if (!this.m_hasExploded && !GameManager.Instance.IsLoadingLevel)
		{
			Exploder.Explode(base.specRigidbody.UnitCenter.ToVector3ZUp(0f), this.explosionData, Vector2.up, null, false, CoreDamageTypes.None, false);
		}
		base.OnDestroy();
		AkSoundEngine.PostEvent("Stop_WPN_cerebralbore_loop_01", base.gameObject);
		if (this.m_targetEnemy != null)
		{
			this.m_targetEnemy.UniquePlayerTargetFlag = false;
		}
	}

	// Token: 0x04008821 RID: 34849
	public ExplosionData explosionData;

	// Token: 0x04008822 RID: 34850
	public float boreTime = 2f;

	// Token: 0x04008823 RID: 34851
	public AnimationCurve boreCurve;

	// Token: 0x04008824 RID: 34852
	public ParticleSystem SparksSystem;

	// Token: 0x04008825 RID: 34853
	private AIActor m_targetEnemy;

	// Token: 0x04008826 RID: 34854
	private bool m_hasExploded;

	// Token: 0x04008827 RID: 34855
	private CerebralBoreProjectile.BoreMotionType m_currentMotionType;

	// Token: 0x04008828 RID: 34856
	private Vector2 m_startPosition;

	// Token: 0x04008829 RID: 34857
	private Vector2 m_initialAimPoint;

	// Token: 0x0400882A RID: 34858
	private float m_currentBezierPoint;

	// Token: 0x0400882B RID: 34859
	private HashSet<SpeculativeRigidbody> m_rigidbodiesDamagedInFlight = new HashSet<SpeculativeRigidbody>();

	// Token: 0x0400882C RID: 34860
	private Vector2 m_targetTrackingPoint;

	// Token: 0x0400882D RID: 34861
	private float m_bezierUpdateTimer;

	// Token: 0x0400882E RID: 34862
	private float m_elapsedBore;

	// Token: 0x0200162D RID: 5677
	private enum BoreMotionType
	{
		// Token: 0x04008832 RID: 34866
		TRACKING,
		// Token: 0x04008833 RID: 34867
		BORING
	}
}
