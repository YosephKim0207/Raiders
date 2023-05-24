using System;
using System.Collections.ObjectModel;
using FullInspector;
using UnityEngine;

// Token: 0x02000D21 RID: 3361
public class DeflectBulletsBehavior : BasicAttackBehavior
{
	// Token: 0x060046FA RID: 18170 RVA: 0x00173064 File Offset: 0x00171264
	public override void Start()
	{
		base.Start();
		if (!string.IsNullOrEmpty(this.TellAnimation))
		{
			tk2dSpriteAnimator spriteAnimator = this.m_aiAnimator.spriteAnimator;
			spriteAnimator.AnimationEventTriggered = (Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip, int>)Delegate.Combine(spriteAnimator.AnimationEventTriggered, new Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip, int>(this.AnimEventTriggered));
		}
	}

	// Token: 0x060046FB RID: 18171 RVA: 0x001730B4 File Offset: 0x001712B4
	public override void Upkeep()
	{
		base.Upkeep();
		base.DecrementTimer(ref this.m_timer, false);
	}

	// Token: 0x060046FC RID: 18172 RVA: 0x001730CC File Offset: 0x001712CC
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
		if (!this.m_aiActor.TargetRigidbody)
		{
			return BehaviorResult.Continue;
		}
		if (!string.IsNullOrEmpty(this.TellAnimation))
		{
			if (!string.IsNullOrEmpty(this.TellAnimation))
			{
				this.m_aiAnimator.PlayUntilFinished(this.TellAnimation, false, null, -1f, false);
			}
			this.m_state = DeflectBulletsBehavior.State.WaitingForTell;
		}
		else
		{
			this.StartDeflecting();
		}
		this.m_aiActor.ClearPath();
		if (this.m_aiActor && this.m_aiActor.knockbackDoer)
		{
			this.m_aiActor.knockbackDoer.SetImmobile(true, "DeflectBulletsBehavior");
		}
		this.m_updateEveryFrame = true;
		return BehaviorResult.RunContinuous;
	}

	// Token: 0x060046FD RID: 18173 RVA: 0x001731A8 File Offset: 0x001713A8
	public override ContinuousBehaviorResult ContinuousUpdate()
	{
		if (this.m_state == DeflectBulletsBehavior.State.WaitingForTell)
		{
			if (!this.m_aiAnimator.IsPlaying(this.TellAnimation))
			{
				this.StartDeflecting();
				return ContinuousBehaviorResult.Continue;
			}
		}
		else if (this.m_state == DeflectBulletsBehavior.State.Deflecting)
		{
			Vector2 unitCenter = this.m_aiActor.specRigidbody.HitboxPixelCollider.UnitCenter;
			float num = this.RadiusCurve.Evaluate(Mathf.InverseLerp(this.DeflectTime, 0f, this.m_timer)) * this.Radius;
			ReadOnlyCollection<Projectile> allProjectiles = StaticReferenceManager.AllProjectiles;
			for (int i = 0; i < allProjectiles.Count; i++)
			{
				Projectile projectile = allProjectiles[i];
				if (projectile.Owner is PlayerController)
				{
					if (projectile.specRigidbody)
					{
						if (Vector2.Distance(unitCenter, projectile.specRigidbody.UnitCenter) <= num)
						{
							this.AdjustProjectileVelocity(projectile, unitCenter, num);
						}
					}
				}
			}
			if (this.m_timer <= 0f)
			{
				return ContinuousBehaviorResult.Finished;
			}
		}
		return ContinuousBehaviorResult.Continue;
	}

	// Token: 0x060046FE RID: 18174 RVA: 0x001732BC File Offset: 0x001714BC
	public override void EndContinuousUpdate()
	{
		base.EndContinuousUpdate();
		if (!string.IsNullOrEmpty(this.TellAnimation))
		{
			this.m_aiAnimator.EndAnimationIf(this.TellAnimation);
		}
		if (!string.IsNullOrEmpty(this.DeflectAnimation))
		{
			this.m_aiAnimator.EndAnimationIf(this.DeflectAnimation);
		}
		if (!string.IsNullOrEmpty(this.DeflectVfx))
		{
			this.m_aiAnimator.StopVfx(this.DeflectVfx);
		}
		if (this.m_aiActor && this.m_aiActor.knockbackDoer)
		{
			this.m_aiActor.knockbackDoer.SetImmobile(false, "DeflectBulletsBehavior");
		}
		this.m_state = DeflectBulletsBehavior.State.Idle;
		this.m_updateEveryFrame = false;
		this.UpdateCooldowns();
	}

	// Token: 0x060046FF RID: 18175 RVA: 0x00173384 File Offset: 0x00171584
	private void AnimEventTriggered(tk2dSpriteAnimator sprite, tk2dSpriteAnimationClip clip, int frameNum)
	{
		tk2dSpriteAnimationFrame frame = clip.GetFrame(frameNum);
		if (this.m_state == DeflectBulletsBehavior.State.WaitingForTell && frame.eventInfo == "deflect")
		{
			this.StartDeflecting();
		}
	}

	// Token: 0x06004700 RID: 18176 RVA: 0x001733C0 File Offset: 0x001715C0
	private void StartDeflecting()
	{
		if (!string.IsNullOrEmpty(this.DeflectAnimation))
		{
			this.m_aiAnimator.PlayUntilFinished(this.DeflectAnimation, false, null, -1f, false);
		}
		if (!string.IsNullOrEmpty(this.DeflectVfx))
		{
			this.m_aiAnimator.PlayVfx(this.DeflectVfx, null, null, null);
		}
		this.m_timer = this.DeflectTime;
		this.m_state = DeflectBulletsBehavior.State.Deflecting;
	}

	// Token: 0x06004701 RID: 18177 RVA: 0x00173448 File Offset: 0x00171648
	private void AdjustProjectileVelocity(Projectile p, Vector2 deflectCenter, float deflectRadius)
	{
		Vector2 vector = p.specRigidbody.UnitCenter - deflectCenter;
		float num = Vector2.SqrMagnitude(vector);
		Vector2 velocity = p.specRigidbody.Velocity;
		if (velocity == Vector2.zero)
		{
			return;
		}
		float num2 = Mathf.Lerp(1f, 0.5f, Mathf.Sqrt(num) / deflectRadius);
		Vector2 vector2 = vector.normalized * (this.force * velocity.magnitude * num2 * num2);
		Vector2 vector3 = vector2 * Mathf.Clamp(BraveTime.DeltaTime, 0f, 0.02f);
		Vector2 vector4 = velocity + vector3;
		if (BraveTime.DeltaTime > 0.02f)
		{
			vector4 *= 0.02f / BraveTime.DeltaTime;
		}
		p.specRigidbody.Velocity = vector4;
		if (vector4 != Vector2.zero)
		{
			p.Direction = vector4.normalized;
			p.Speed = velocity.magnitude;
			p.specRigidbody.Velocity = p.Direction * p.Speed;
			if (p.shouldRotate && (vector4.x != 0f || vector4.y != 0f))
			{
				float num3 = BraveMathCollege.Atan2Degrees(p.Direction);
				if (!float.IsNaN(num3) && !float.IsInfinity(num3))
				{
					Quaternion quaternion = Quaternion.Euler(0f, 0f, num3);
					if (!float.IsNaN(quaternion.x) && !float.IsNaN(quaternion.y))
					{
						p.transform.rotation = quaternion;
					}
				}
			}
		}
	}

	// Token: 0x040039D4 RID: 14804
	public float Radius;

	// Token: 0x040039D5 RID: 14805
	public float DeflectTime;

	// Token: 0x040039D6 RID: 14806
	public AnimationCurve RadiusCurve;

	// Token: 0x040039D7 RID: 14807
	public float force = 10f;

	// Token: 0x040039D8 RID: 14808
	[InspectorCategory("Visuals")]
	public string TellAnimation;

	// Token: 0x040039D9 RID: 14809
	[InspectorCategory("Visuals")]
	public string DeflectAnimation;

	// Token: 0x040039DA RID: 14810
	[InspectorCategory("Visuals")]
	public string DeflectVfx;

	// Token: 0x040039DB RID: 14811
	private DeflectBulletsBehavior.State m_state;

	// Token: 0x040039DC RID: 14812
	private float m_timer;

	// Token: 0x02000D22 RID: 3362
	private enum State
	{
		// Token: 0x040039DE RID: 14814
		Idle,
		// Token: 0x040039DF RID: 14815
		WaitingForTell,
		// Token: 0x040039E0 RID: 14816
		Deflecting
	}
}
