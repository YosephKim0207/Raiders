using System;
using System.Collections;
using System.Collections.Generic;
using Dungeonator;
using UnityEngine;

// Token: 0x02001627 RID: 5671
public class BeeProjectile : Projectile
{
	// Token: 0x06008463 RID: 33891 RVA: 0x00367E44 File Offset: 0x00366044
	public override void Start()
	{
		base.Start();
		this.OnHitEnemy = (Action<Projectile, SpeculativeRigidbody, bool>)Delegate.Combine(this.OnHitEnemy, new Action<Projectile, SpeculativeRigidbody, bool>(this.HandleHitEnemy));
		this.ModifyVelocity = (Func<Vector2, Vector2>)Delegate.Combine(this.ModifyVelocity, new Func<Vector2, Vector2>(this.ModifyVelocityLocal));
	}

	// Token: 0x06008464 RID: 33892 RVA: 0x00367E9C File Offset: 0x0036609C
	private void HandleHitEnemy(Projectile arg1, SpeculativeRigidbody arg2, bool arg3)
	{
		this.m_previouslyHitEnemy = null;
		this.CurrentTarget = null;
		if (arg2 && arg2.aiActor)
		{
			this.m_previouslyHitEnemy = arg2.aiActor;
		}
	}

	// Token: 0x06008465 RID: 33893 RVA: 0x00367ED4 File Offset: 0x003660D4
	private IEnumerator FindTarget()
	{
		this.m_coroutineIsActive = true;
		for (;;)
		{
			if (base.Owner is PlayerController)
			{
				List<AIActor> activeEnemies = GameManager.Instance.Dungeon.data.GetAbsoluteRoomFromPosition(base.Owner.transform.position.IntXY(VectorConversions.Floor)).GetActiveEnemies(RoomHandler.ActiveEnemyType.All);
				if (activeEnemies != null)
				{
					float num = float.MaxValue;
					for (int i = 0; i < activeEnemies.Count; i++)
					{
						AIActor aiactor = activeEnemies[i];
						if (aiactor && aiactor.healthHaver && !aiactor.healthHaver.IsDead)
						{
							if (!aiactor.IsGone && aiactor.IsWorthShootingAt && aiactor.specRigidbody)
							{
								if (!(aiactor == this.m_previouslyHitEnemy))
								{
									float num2 = Vector2.Distance(aiactor.specRigidbody.GetUnitCenter(ColliderType.HitBox), base.Owner.specRigidbody.UnitCenter);
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
			}
			else
			{
				this.CurrentTarget = GameManager.Instance.GetPlayerClosestToPoint(base.transform.position.XY());
			}
			yield return new WaitForSeconds(1f);
		}
		yield break;
	}

	// Token: 0x06008466 RID: 33894 RVA: 0x00367EF0 File Offset: 0x003660F0
	private Vector2 ModifyVelocityLocal(Vector2 inVel)
	{
		if (!this.m_coroutineIsActive)
		{
			base.StartCoroutine(this.FindTarget());
		}
		float num = 1f;
		inVel = this.m_currentDirection;
		Vector2 vector = inVel;
		if (this.CurrentTarget != null && !this.CurrentTarget.IsGone)
		{
			Vector2 normalized = (this.CurrentTarget.specRigidbody.GetUnitCenter(ColliderType.HitBox) - base.specRigidbody.UnitCenter).normalized;
			vector = Vector3.RotateTowards(inVel, normalized, this.angularAcceleration * 0.017453292f * BraveTime.DeltaTime, 0f).XY().normalized;
			float num2 = Vector2.Angle(vector, normalized);
			num = 0.25f + (1f - Mathf.Clamp01(Mathf.Abs(num2) / 60f)) * 0.75f;
		}
		vector = vector * this.m_currentSpeed * num;
		if (this.OverrideMotionModule != null)
		{
			float num3 = BraveMathCollege.Atan2Degrees(inVel);
			float num4 = BraveMathCollege.Atan2Degrees(vector);
			this.OverrideMotionModule.AdjustRightVector(Mathf.DeltaAngle(num3, num4));
		}
		return vector;
	}

	// Token: 0x06008467 RID: 33895 RVA: 0x00368018 File Offset: 0x00366218
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x040087F8 RID: 34808
	public float angularAcceleration = 10f;

	// Token: 0x040087F9 RID: 34809
	public float searchRadius = 10f;

	// Token: 0x040087FA RID: 34810
	public GameActor CurrentTarget;

	// Token: 0x040087FB RID: 34811
	protected bool m_coroutineIsActive;

	// Token: 0x040087FC RID: 34812
	protected AIActor m_previouslyHitEnemy;
}
