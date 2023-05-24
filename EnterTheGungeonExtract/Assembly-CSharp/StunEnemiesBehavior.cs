using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000D5E RID: 3422
public class StunEnemiesBehavior : AttackBehaviorBase
{
	// Token: 0x06004849 RID: 18505 RVA: 0x0017E72C File Offset: 0x0017C92C
	public override void Start()
	{
		base.Start();
		this.m_minAttackDistance = this.minAttackDistance;
		this.m_maxAttackDistance = this.maxAttackDistance;
	}

	// Token: 0x0600484A RID: 18506 RVA: 0x0017E74C File Offset: 0x0017C94C
	public override BehaviorResult Update()
	{
		base.Update();
		bool flag = false;
		base.DecrementTimer(ref this.m_cooldownTimer, false);
		BehaviorResult behaviorResult = base.Update();
		if (behaviorResult != BehaviorResult.Continue)
		{
			return behaviorResult;
		}
		if (this.m_cooldownTimer > 0f)
		{
			return BehaviorResult.Continue;
		}
		if (!this.m_aiActor.TargetRigidbody)
		{
			return BehaviorResult.Continue;
		}
		SpeculativeRigidbody targetRigidbody = this.m_aiActor.TargetRigidbody;
		AIActor aiActor = targetRigidbody.aiActor;
		if (aiActor)
		{
			if (!aiActor.IsNormalEnemy)
			{
				return BehaviorResult.Continue;
			}
			HealthHaver healthHaver = targetRigidbody.healthHaver;
			if (healthHaver)
			{
				if (!healthHaver.IsVulnerable)
				{
					return BehaviorResult.Continue;
				}
				if (healthHaver.IsBoss)
				{
					flag = GameManager.Instance.Dungeon.CellSupportsFalling(targetRigidbody.UnitCenter);
				}
			}
		}
		this.m_minAttackDistance = this.minAttackDistance;
		this.m_maxAttackDistance = this.maxAttackDistance;
		if (flag)
		{
			this.m_minAttackDistance = this.minAttackDistance;
			this.m_maxAttackDistance = this.maxAttackDistance + 1f;
		}
		Vector2 unitCenter = this.m_aiActor.specRigidbody.UnitCenter;
		Vector2 targetPoint = this.GetTargetPoint(this.m_aiActor.TargetRigidbody, unitCenter);
		float num = Vector2.Distance(unitCenter, targetPoint);
		bool hasLineOfSightToTarget = this.m_aiActor.HasLineOfSightToTarget;
		if (num < this.maxAttackDistance && hasLineOfSightToTarget)
		{
			BehaviorSpeculator component = targetRigidbody.GetComponent<BehaviorSpeculator>();
			if (component)
			{
				if (!string.IsNullOrEmpty(this.AnimationName) && !this.m_aiAnimator.IsPlaying(this.AnimationName))
				{
					this.m_aiAnimator.PlayUntilFinished(this.AnimationName, false, null, -1f, false);
					if (this.StunVFX)
					{
						this.m_aiActor.StartCoroutine(this.HandleDelayedSpawnStunVFX(targetPoint));
					}
				}
				component.Stun(this.StunDuration, true);
				this.m_cooldownTimer = this.Cooldown;
				this.m_updateEveryFrame = true;
				return BehaviorResult.RunContinuous;
			}
		}
		return BehaviorResult.Continue;
	}

	// Token: 0x0600484B RID: 18507 RVA: 0x0017E948 File Offset: 0x0017CB48
	private IEnumerator HandleDelayedSpawnStunVFX(Vector2 targetPoint)
	{
		yield return new WaitForSeconds(0.75f);
		if (this.StunVFX)
		{
			bool flag = BraveMathCollege.AbsAngleBetween(this.m_aiAnimator.FacingDirection, 0f) < 90f;
			GameObject gameObject = SpawnManager.SpawnVFX(this.StunVFX, this.m_aiActor.CenterPosition + new Vector2(0.625f * (float)((!flag) ? (-1) : 1), -0.0625f), Quaternion.identity);
			gameObject.transform.parent = this.m_aiActor.transform;
		}
		yield break;
	}

	// Token: 0x0600484C RID: 18508 RVA: 0x0017E964 File Offset: 0x0017CB64
	public override ContinuousBehaviorResult ContinuousUpdate()
	{
		SpeculativeRigidbody targetRigidbody = this.m_aiActor.TargetRigidbody;
		if (!targetRigidbody)
		{
			return ContinuousBehaviorResult.Finished;
		}
		bool flag = false;
		if (targetRigidbody && targetRigidbody.aiActor && targetRigidbody.healthHaver && targetRigidbody.healthHaver.IsBoss)
		{
			flag = GameManager.Instance.Dungeon.CellSupportsFalling(targetRigidbody.UnitCenter);
		}
		this.m_minAttackDistance = this.minAttackDistance;
		this.m_maxAttackDistance = this.maxAttackDistance;
		if (flag)
		{
			this.m_minAttackDistance = this.minAttackDistance;
			this.m_maxAttackDistance = this.maxAttackDistance + 1f;
		}
		Vector2 unitCenter = this.m_aiActor.specRigidbody.UnitCenter;
		Vector2 targetPoint = this.GetTargetPoint(this.m_aiActor.TargetRigidbody, unitCenter);
		float num = Vector2.Distance(unitCenter, targetPoint);
		if (num > this.maxAttackDistance)
		{
			return ContinuousBehaviorResult.Finished;
		}
		this.m_aiActor.ClearPath();
		BehaviorSpeculator component = targetRigidbody.GetComponent<BehaviorSpeculator>();
		if (component)
		{
			if (component.healthHaver && !component.healthHaver.IsVulnerable)
			{
				return ContinuousBehaviorResult.Finished;
			}
			if (!string.IsNullOrEmpty(this.AnimationName) && !this.m_aiAnimator.IsPlaying(this.AnimationName))
			{
				this.m_aiAnimator.PlayUntilFinished(this.AnimationName, false, null, -1f, false);
				if (this.StunVFX)
				{
					this.m_aiActor.StartCoroutine(this.HandleDelayedSpawnStunVFX(targetPoint));
				}
			}
			if (component.IsStunned)
			{
				component.UpdateStun(this.StunDuration);
			}
			else
			{
				component.Stun(this.StunDuration, true);
			}
			this.m_cooldownTimer = this.Cooldown;
		}
		return ContinuousBehaviorResult.Continue;
	}

	// Token: 0x0600484D RID: 18509 RVA: 0x0017EB3C File Offset: 0x0017CD3C
	public override void EndContinuousUpdate()
	{
		base.EndContinuousUpdate();
		this.m_aiActor.BehaviorOverridesVelocity = false;
		this.m_aiAnimator.LockFacingDirection = false;
		this.m_updateEveryFrame = false;
	}

	// Token: 0x0600484E RID: 18510 RVA: 0x0017EB64 File Offset: 0x0017CD64
	private Vector2 GetTargetPoint(SpeculativeRigidbody targetRigidbody, Vector2 myCenter)
	{
		PixelCollider hitboxPixelCollider = targetRigidbody.HitboxPixelCollider;
		return BraveMathCollege.ClosestPointOnRectangle(myCenter, hitboxPixelCollider.UnitBottomLeft, hitboxPixelCollider.UnitDimensions);
	}

	// Token: 0x0600484F RID: 18511 RVA: 0x0017EB8C File Offset: 0x0017CD8C
	public override bool IsReady()
	{
		return true;
	}

	// Token: 0x06004850 RID: 18512 RVA: 0x0017EB90 File Offset: 0x0017CD90
	public override float GetMinReadyRange()
	{
		return this.m_maxAttackDistance;
	}

	// Token: 0x06004851 RID: 18513 RVA: 0x0017EB98 File Offset: 0x0017CD98
	public override float GetMaxRange()
	{
		return this.m_maxAttackDistance;
	}

	// Token: 0x04003BE8 RID: 15336
	public float StunDuration = 1f;

	// Token: 0x04003BE9 RID: 15337
	public float Cooldown;

	// Token: 0x04003BEA RID: 15338
	public float minAttackDistance = 0.1f;

	// Token: 0x04003BEB RID: 15339
	public float maxAttackDistance = 1f;

	// Token: 0x04003BEC RID: 15340
	public string AnimationName;

	// Token: 0x04003BED RID: 15341
	public GameObject StunVFX;

	// Token: 0x04003BEE RID: 15342
	private float m_cooldownTimer;

	// Token: 0x04003BEF RID: 15343
	private float m_minAttackDistance;

	// Token: 0x04003BF0 RID: 15344
	private float m_maxAttackDistance;
}
