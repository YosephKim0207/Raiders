using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000E04 RID: 3588
public class DodgeRollBehavior : OverrideBehaviorBase
{
	// Token: 0x06004BF9 RID: 19449 RVA: 0x0019E710 File Offset: 0x0019C910
	public override void Start()
	{
		base.Start();
		this.m_updateEveryFrame = true;
		this.m_ignoreGlobalCooldown = true;
		StaticReferenceManager.ProjectileAdded += this.ProjectileAdded;
		StaticReferenceManager.ProjectileRemoved += this.ProjectileRemoved;
	}

	// Token: 0x06004BFA RID: 19450 RVA: 0x0019E748 File Offset: 0x0019C948
	public override void Upkeep()
	{
		base.Upkeep();
		base.DecrementTimer(ref this.m_cooldownTimer, false);
		this.m_cachedShouldDodge = null;
		this.m_cachedRollDirection = null;
	}

	// Token: 0x06004BFB RID: 19451 RVA: 0x0019E788 File Offset: 0x0019C988
	public override bool OverrideOtherBehaviors()
	{
		Vector2 vector;
		return this.m_cooldownTimer <= 0f && this.ShouldDodgeroll(out vector);
	}

	// Token: 0x06004BFC RID: 19452 RVA: 0x0019E7B0 File Offset: 0x0019C9B0
	public override BehaviorResult Update()
	{
		base.Update();
		if (this.m_cooldownTimer > 0f)
		{
			return BehaviorResult.Continue;
		}
		Vector2 vector;
		if (this.ShouldDodgeroll(out vector))
		{
			this.m_aiAnimator.LockFacingDirection = true;
			this.m_aiAnimator.FacingDirection = vector.ToAngle();
			this.m_aiAnimator.PlayUntilFinished(this.dodgeAnim, false, null, -1f, false);
			float currentClipLength = this.m_aiAnimator.CurrentClipLength;
			this.m_aiActor.BehaviorOverridesVelocity = true;
			this.m_aiActor.BehaviorVelocity = vector * (this.rollDistance / currentClipLength);
			return BehaviorResult.RunContinuous;
		}
		return BehaviorResult.Continue;
	}

	// Token: 0x06004BFD RID: 19453 RVA: 0x0019E850 File Offset: 0x0019CA50
	public override ContinuousBehaviorResult ContinuousUpdate()
	{
		base.ContinuousUpdate();
		if (!this.m_aiAnimator.IsPlaying(this.dodgeAnim))
		{
			this.m_aiActor.BehaviorOverridesVelocity = false;
			this.m_aiAnimator.LockFacingDirection = false;
			return ContinuousBehaviorResult.Finished;
		}
		return ContinuousBehaviorResult.Continue;
	}

	// Token: 0x06004BFE RID: 19454 RVA: 0x0019E88C File Offset: 0x0019CA8C
	public override void Destroy()
	{
		StaticReferenceManager.ProjectileAdded -= this.ProjectileAdded;
		StaticReferenceManager.ProjectileRemoved -= this.ProjectileRemoved;
		base.Destroy();
	}

	// Token: 0x06004BFF RID: 19455 RVA: 0x0019E8B8 File Offset: 0x0019CAB8
	private void ProjectileAdded(Projectile p)
	{
		if (!p)
		{
			return;
		}
		if (!p.specRigidbody)
		{
			return;
		}
		if (p.specRigidbody.CanCollideWith(this.m_aiActor.specRigidbody))
		{
			this.m_consideredProjectiles.Add(p);
		}
	}

	// Token: 0x06004C00 RID: 19456 RVA: 0x0019E90C File Offset: 0x0019CB0C
	private void ProjectileRemoved(Projectile p)
	{
		this.m_consideredProjectiles.Remove(p);
	}

	// Token: 0x06004C01 RID: 19457 RVA: 0x0019E91C File Offset: 0x0019CB1C
	private bool ShouldDodgeroll(out Vector2 rollDirection)
	{
		if (this.m_cachedShouldDodge != null)
		{
			rollDirection = this.m_cachedRollDirection.Value;
			return this.m_cachedShouldDodge.Value;
		}
		PixelCollider hitboxPixelCollider = this.m_aiActor.specRigidbody.HitboxPixelCollider;
		for (int i = 0; i < this.m_consideredProjectiles.Count; i++)
		{
			Projectile projectile = this.m_consideredProjectiles[i];
			float num = Vector2.Distance(projectile.specRigidbody.UnitCenter, hitboxPixelCollider.UnitCenter) / projectile.Speed;
			if (num <= this.timeToHitThreshold)
			{
				IntVector2 intVector = PhysicsEngine.UnitToPixel(projectile.specRigidbody.Velocity * this.timeToHitThreshold * 1.1f);
				CollisionData collisionData;
				bool flag = PhysicsEngine.Instance.RigidbodyCast(projectile.specRigidbody, intVector, out collisionData, true, true, null, false);
				CollisionData.Pool.Free(ref collisionData);
				if (flag)
				{
					if (!(collisionData.OtherRigidbody == null) && !(collisionData.OtherRigidbody != this.m_aiActor.specRigidbody))
					{
						if (UnityEngine.Random.value <= this.dodgeChance)
						{
							List<Vector2> list = new List<Vector2>();
							Vector2 unitCenter = this.m_aiActor.specRigidbody.UnitCenter;
							for (int j = 0; j < 8; j++)
							{
								bool flag2 = false;
								Vector2 normalized = IntVector2.CardinalsAndOrdinals[j].ToVector2().normalized;
								RaycastResult raycastResult;
								bool flag3 = PhysicsEngine.Instance.Raycast(unitCenter, normalized, 3f, out raycastResult, true, true, int.MaxValue, new CollisionLayer?(CollisionLayer.EnemyCollider), false, null, this.m_aiActor.specRigidbody);
								RaycastResult.Pool.Free(ref raycastResult);
								float num2 = 0.25f;
								float num3 = num2;
								while (num3 <= this.rollDistance && !flag2 && !flag3)
								{
									if (GameManager.Instance.Dungeon.ShouldReallyFall(unitCenter + num3 * normalized))
									{
										flag2 = true;
									}
									num3 += num2;
								}
								if (!flag3 && !flag2)
								{
									list.Add(normalized);
								}
							}
							if (list.Count != 0)
							{
								this.m_cachedShouldDodge = new bool?(true);
								this.m_cachedRollDirection = new Vector2?(BraveUtility.RandomElement<Vector2>(list));
								rollDirection = this.m_cachedRollDirection.Value;
								return this.m_cachedShouldDodge.Value;
							}
						}
					}
				}
				this.m_consideredProjectiles.Remove(projectile);
			}
		}
		this.m_cachedShouldDodge = new bool?(false);
		this.m_cachedRollDirection = new Vector2?(Vector2.zero);
		rollDirection = this.m_cachedRollDirection.Value;
		return this.m_cachedShouldDodge.Value;
	}

	// Token: 0x040041D2 RID: 16850
	public float Cooldown = 1f;

	// Token: 0x040041D3 RID: 16851
	public float timeToHitThreshold = 0.25f;

	// Token: 0x040041D4 RID: 16852
	public float dodgeChance = 0.5f;

	// Token: 0x040041D5 RID: 16853
	public string dodgeAnim = "dodgeroll";

	// Token: 0x040041D6 RID: 16854
	public float rollDistance = 3f;

	// Token: 0x040041D7 RID: 16855
	private float m_cooldownTimer;

	// Token: 0x040041D8 RID: 16856
	private List<Projectile> m_consideredProjectiles = new List<Projectile>();

	// Token: 0x040041D9 RID: 16857
	private bool? m_cachedShouldDodge;

	// Token: 0x040041DA RID: 16858
	private Vector2? m_cachedRollDirection;
}
