using System;
using Dungeonator;
using UnityEngine;

// Token: 0x02000D2F RID: 3375
public class LeapExplosion : AttackBehaviorBase
{
	// Token: 0x06004748 RID: 18248 RVA: 0x00175110 File Offset: 0x00173310
	public override void Start()
	{
		base.Start();
		for (int i = 0; i < this.m_aiActor.specRigidbody.PixelColliders.Count; i++)
		{
			PixelCollider pixelCollider = this.m_aiActor.specRigidbody.PixelColliders[i];
			if (pixelCollider.CollisionLayer == CollisionLayer.EnemyCollider)
			{
				this.m_enemyCollider = pixelCollider;
			}
		}
	}

	// Token: 0x06004749 RID: 18249 RVA: 0x00175174 File Offset: 0x00173374
	public override void Upkeep()
	{
		base.Upkeep();
	}

	// Token: 0x0600474A RID: 18250 RVA: 0x0017517C File Offset: 0x0017337C
	public override BehaviorResult Update()
	{
		base.Update();
		BehaviorResult behaviorResult = base.Update();
		if (behaviorResult != BehaviorResult.Continue)
		{
			return behaviorResult;
		}
		if (!this.m_aiActor.TargetRigidbody)
		{
			return BehaviorResult.Continue;
		}
		Vector2 vector = this.m_aiActor.TargetRigidbody.specRigidbody.GetUnitCenter(ColliderType.HitBox);
		if (this.leadAmount > 0f)
		{
			Vector2 vector2 = vector + this.m_aiActor.TargetRigidbody.specRigidbody.Velocity * 0.75f;
			vector = Vector2.Lerp(vector, vector2, this.leadAmount);
		}
		float num = Vector2.Distance(this.m_aiActor.specRigidbody.UnitCenter, vector);
		if (num > this.minLeapDistance && num < this.leapDistance)
		{
			this.m_state = LeapExplosion.State.Charging;
			this.m_aiAnimator.PlayUntilFinished(this.chargeAnim, true, null, -1f, false);
			this.m_aiActor.ClearPath();
			this.m_aiActor.BehaviorOverridesVelocity = true;
			this.m_aiActor.BehaviorVelocity = Vector2.zero;
			this.m_updateEveryFrame = true;
			return BehaviorResult.RunContinuous;
		}
		return BehaviorResult.Continue;
	}

	// Token: 0x0600474B RID: 18251 RVA: 0x00175294 File Offset: 0x00173494
	public override ContinuousBehaviorResult ContinuousUpdate()
	{
		if (this.m_state == LeapExplosion.State.Charging)
		{
			if (!this.m_aiAnimator.IsPlaying(this.chargeAnim))
			{
				this.m_state = LeapExplosion.State.Leaping;
				if (!this.m_aiActor.TargetRigidbody)
				{
					this.m_state = LeapExplosion.State.Idle;
					return ContinuousBehaviorResult.Finished;
				}
				Vector2 unitCenter = this.m_aiActor.specRigidbody.UnitCenter;
				Vector2 vector = this.m_aiActor.TargetRigidbody.specRigidbody.GetUnitCenter(ColliderType.HitBox);
				if (this.leadAmount > 0f)
				{
					Vector2 vector2 = vector + this.m_aiActor.TargetRigidbody.specRigidbody.Velocity * 0.75f;
					vector = Vector2.Lerp(vector, vector2, this.leadAmount);
				}
				float num = Vector2.Distance(unitCenter, vector);
				if (num > this.maxTravelDistance)
				{
					vector = unitCenter + (vector - unitCenter).normalized * this.maxTravelDistance;
					num = Vector2.Distance(unitCenter, vector);
				}
				this.m_aiActor.ClearPath();
				this.m_aiActor.BehaviorOverridesVelocity = true;
				this.m_aiActor.BehaviorVelocity = (vector - unitCenter).normalized * (num / this.leapTime);
				float num2 = this.m_aiActor.BehaviorVelocity.ToAngle();
				this.m_aiAnimator.LockFacingDirection = true;
				this.m_aiAnimator.FacingDirection = num2;
				this.m_aiActor.PathableTiles = CellTypes.FLOOR | CellTypes.PIT;
				this.m_enemyCollider.CollisionLayer = CollisionLayer.TileBlocker;
				this.m_aiActor.specRigidbody.AddCollisionLayerOverride(CollisionMask.LayerToMask(CollisionLayer.BulletBlocker, CollisionLayer.EnemyBulletBlocker));
				this.m_aiActor.DoDustUps = false;
				this.m_aiAnimator.PlayUntilCancelled(this.leapAnim, true, null, -1f, false);
			}
		}
		else if (this.m_state == LeapExplosion.State.Leaping)
		{
			this.m_elapsed += this.m_deltaTime;
			if (this.m_elapsed >= this.leapTime)
			{
				return ContinuousBehaviorResult.Finished;
			}
		}
		return ContinuousBehaviorResult.Continue;
	}

	// Token: 0x0600474C RID: 18252 RVA: 0x0017548C File Offset: 0x0017368C
	public override void EndContinuousUpdate()
	{
		base.EndContinuousUpdate();
		if (this.m_aiActor.healthHaver.IsAlive)
		{
			this.m_aiActor.healthHaver.ApplyDamage(float.MaxValue, Vector2.zero, "self-immolation", CoreDamageTypes.Fire, DamageCategory.Unstoppable, true, null, false);
		}
		this.m_updateEveryFrame = false;
	}

	// Token: 0x0600474D RID: 18253 RVA: 0x001754E0 File Offset: 0x001736E0
	public override bool IsReady()
	{
		return true;
	}

	// Token: 0x0600474E RID: 18254 RVA: 0x001754E4 File Offset: 0x001736E4
	public override float GetMinReadyRange()
	{
		return this.leapDistance;
	}

	// Token: 0x0600474F RID: 18255 RVA: 0x001754EC File Offset: 0x001736EC
	public override float GetMaxRange()
	{
		return this.leapDistance;
	}

	// Token: 0x04003A30 RID: 14896
	public float minLeapDistance = 1f;

	// Token: 0x04003A31 RID: 14897
	public float leapDistance = 4f;

	// Token: 0x04003A32 RID: 14898
	public float maxTravelDistance = 5f;

	// Token: 0x04003A33 RID: 14899
	public float leadAmount;

	// Token: 0x04003A34 RID: 14900
	public float leapTime = 0.75f;

	// Token: 0x04003A35 RID: 14901
	public string chargeAnim;

	// Token: 0x04003A36 RID: 14902
	public string leapAnim;

	// Token: 0x04003A37 RID: 14903
	private PixelCollider m_enemyCollider;

	// Token: 0x04003A38 RID: 14904
	private float m_elapsed;

	// Token: 0x04003A39 RID: 14905
	private LeapExplosion.State m_state;

	// Token: 0x02000D30 RID: 3376
	private enum State
	{
		// Token: 0x04003A3B RID: 14907
		Idle,
		// Token: 0x04003A3C RID: 14908
		Charging,
		// Token: 0x04003A3D RID: 14909
		Leaping
	}
}
