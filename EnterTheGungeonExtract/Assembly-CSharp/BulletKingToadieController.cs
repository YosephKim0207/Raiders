using System;
using System.Collections.Generic;
using Dungeonator;
using UnityEngine;

// Token: 0x02001006 RID: 4102
public class BulletKingToadieController : BraveBehaviour
{
	// Token: 0x17000CF9 RID: 3321
	// (get) Token: 0x060059BB RID: 22971 RVA: 0x00224468 File Offset: 0x00222668
	// (set) Token: 0x060059BC RID: 22972 RVA: 0x00224470 File Offset: 0x00222670
	public AIActor MyKing { get; set; }

	// Token: 0x060059BD RID: 22973 RVA: 0x0022447C File Offset: 0x0022267C
	public void Start()
	{
		SpeculativeRigidbody specRigidbody = base.specRigidbody;
		specRigidbody.OnPreRigidbodyCollision = (SpeculativeRigidbody.OnPreRigidbodyCollisionDelegate)Delegate.Combine(specRigidbody.OnPreRigidbodyCollision, new SpeculativeRigidbody.OnPreRigidbodyCollisionDelegate(this.PreRigidbodyCollision));
		base.healthHaver.OnDamaged += this.OnDamaged;
		if (this.ShouldCry)
		{
			base.aiActor.PreventAutoKillOnBossDeath = true;
		}
		if (this.CanReturnAngry)
		{
			base.healthHaver.OnDeath += this.OnDeath;
		}
		List<AIActor> activeEnemies = base.aiActor.ParentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All);
		for (int i = 0; i < activeEnemies.Count; i++)
		{
			if (activeEnemies[i] && activeEnemies[i].healthHaver && activeEnemies[i].healthHaver.IsBoss)
			{
				this.MyKing = activeEnemies[i];
				break;
			}
		}
	}

	// Token: 0x060059BE RID: 22974 RVA: 0x00224578 File Offset: 0x00222778
	public void Update()
	{
		if (this && base.healthHaver && base.healthHaver.IsDead)
		{
			return;
		}
		if (this.ShouldCry && this.MyKing && this.MyKing.healthHaver.IsDead)
		{
			if (!this.m_isCrazed && this.scepterAnimator)
			{
				this.scepterAnimator.Play("scepter_drop");
				this.scepterAnimator.transform.parent = SpawnManager.Instance.VFX;
			}
			base.aiAnimator.LockFacingDirection = true;
			base.aiAnimator.FacingDirection = (this.MyKing.CenterPosition - base.aiActor.CenterPosition).ToAngle();
			base.aiAnimator.PlayUntilCancelled("cry", false, null, -1f, false);
			base.aiActor.DiesOnCollison = true;
			base.aiActor.CollisionDamage = 0f;
			base.aiActor.healthHaver.ForceSetCurrentHealth(this.PostCrazeHealth);
			base.aiActor.ClearPath();
			base.aiActor.BehaviorVelocity = Vector2.zero;
			base.behaviorSpeculator.InterruptAndDisable();
			if (this.CanReturnAngry && GameManager.HasInstance && this && base.healthHaver && base.healthHaver.IsAlive)
			{
				GameManager.Instance.RunData.SpawnAngryToadie = true;
			}
			this.MyKing = null;
			SpeculativeRigidbody specRigidbody = base.specRigidbody;
			specRigidbody.OnPreRigidbodyCollision = (SpeculativeRigidbody.OnPreRigidbodyCollisionDelegate)Delegate.Remove(specRigidbody.OnPreRigidbodyCollision, new SpeculativeRigidbody.OnPreRigidbodyCollisionDelegate(this.PreRigidbodyCollision));
			this.m_isCrying = true;
		}
		if (!this.m_isCrazed && !this.m_isCrying && this.AutoCrazeTime > 0f)
		{
			this.m_timer += BraveTime.DeltaTime;
			if (this.m_timer > this.AutoCrazeTime)
			{
				this.LoseHat(null);
				base.aiAnimator.SetBaseAnim("crazed", false);
				base.behaviorSpeculator.enabled = true;
				this.MakeVulnerable();
				this.m_isCrazed = true;
				SpeculativeRigidbody specRigidbody2 = base.specRigidbody;
				specRigidbody2.OnPreRigidbodyCollision = (SpeculativeRigidbody.OnPreRigidbodyCollisionDelegate)Delegate.Remove(specRigidbody2.OnPreRigidbodyCollision, new SpeculativeRigidbody.OnPreRigidbodyCollisionDelegate(this.PreRigidbodyCollision));
				base.healthHaver.OnDamaged -= this.OnDamaged;
			}
		}
	}

	// Token: 0x060059BF RID: 22975 RVA: 0x00224814 File Offset: 0x00222A14
	protected override void OnDestroy()
	{
		SpeculativeRigidbody specRigidbody = base.specRigidbody;
		specRigidbody.OnPreRigidbodyCollision = (SpeculativeRigidbody.OnPreRigidbodyCollisionDelegate)Delegate.Remove(specRigidbody.OnPreRigidbodyCollision, new SpeculativeRigidbody.OnPreRigidbodyCollisionDelegate(this.PreRigidbodyCollision));
		base.healthHaver.OnDamaged -= this.OnDamaged;
		if (this.CanReturnAngry)
		{
			base.healthHaver.OnDeath -= this.OnDeath;
		}
		base.OnDestroy();
	}

	// Token: 0x060059C0 RID: 22976 RVA: 0x00224888 File Offset: 0x00222A88
	private void PreRigidbodyCollision(SpeculativeRigidbody myRigidbody, PixelCollider myPixelCollider, SpeculativeRigidbody otherRigidbody, PixelCollider otherPixelCollider)
	{
		Projectile projectile = otherRigidbody.projectile;
		if (projectile && projectile.Owner is PlayerController)
		{
			this.WasHit(otherRigidbody.Velocity);
		}
		else if (otherRigidbody.gameActor)
		{
			PhysicsEngine.SkipCollision = true;
		}
	}

	// Token: 0x060059C1 RID: 22977 RVA: 0x002248E0 File Offset: 0x00222AE0
	private void OnDamaged(float resultvalue, float maxvalue, CoreDamageTypes damagetypes, DamageCategory damagecategory, Vector2 damagedirection)
	{
		this.WasHit(damagedirection);
	}

	// Token: 0x060059C2 RID: 22978 RVA: 0x002248EC File Offset: 0x00222AEC
	private void OnDeath(Vector2 vector2)
	{
		if (this.CanReturnAngry && GameManager.HasInstance)
		{
			GameManager.Instance.RunData.SpawnAngryToadie = false;
		}
	}

	// Token: 0x060059C3 RID: 22979 RVA: 0x00224914 File Offset: 0x00222B14
	private void WasHit(Vector2 hatDirection)
	{
		if (this.m_isCrazed)
		{
			return;
		}
		this.DropScepter();
		this.LoseHat(new Vector2?(hatDirection));
		base.aiAnimator.SetBaseAnim("crazed", false);
		base.behaviorSpeculator.enabled = true;
		this.m_isCrazed = true;
		base.Invoke("MakeVulnerable", 1f);
		SpeculativeRigidbody specRigidbody = base.specRigidbody;
		specRigidbody.OnPreRigidbodyCollision = (SpeculativeRigidbody.OnPreRigidbodyCollisionDelegate)Delegate.Remove(specRigidbody.OnPreRigidbodyCollision, new SpeculativeRigidbody.OnPreRigidbodyCollisionDelegate(this.PreRigidbodyCollision));
		base.healthHaver.OnDamaged -= this.OnDamaged;
	}

	// Token: 0x060059C4 RID: 22980 RVA: 0x002249B4 File Offset: 0x00222BB4
	private void DropScepter()
	{
		if (!this.scepterAnimator)
		{
			return;
		}
		this.scepterAnimator.Play("scepter_drop");
		this.scepterAnimator.transform.parent = SpawnManager.Instance.VFX;
	}

	// Token: 0x060059C5 RID: 22981 RVA: 0x002249F4 File Offset: 0x00222BF4
	private void LoseHat(Vector2? hatDirection = null)
	{
		if (!this.hatPoint || !this.hatVfx)
		{
			return;
		}
		if (hatDirection == null)
		{
			hatDirection = new Vector2?(Vector2.one);
		}
		GameObject gameObject = SpawnManager.SpawnVFX(this.hatVfx, this.hatPoint.position, Quaternion.identity);
		gameObject.transform.parent = SpawnManager.Instance.VFX;
		DebrisObject orAddComponent = gameObject.GetOrAddComponent<DebrisObject>();
		orAddComponent.angularVelocity = 270f;
		orAddComponent.angularVelocityVariance = 0f;
		orAddComponent.decayOnBounce = 0.5f;
		orAddComponent.bounceCount = 3;
		orAddComponent.canRotate = true;
		orAddComponent.Trigger(hatDirection.Value.normalized * 10f, 1f, 1f);
	}

	// Token: 0x060059C6 RID: 22982 RVA: 0x00224AD4 File Offset: 0x00222CD4
	private void MakeVulnerable()
	{
		base.healthHaver.IsVulnerable = true;
		base.healthHaver.ForceSetCurrentHealth(this.PostCrazeHealth);
	}

	// Token: 0x04005314 RID: 21268
	public bool ShouldCry;

	// Token: 0x04005315 RID: 21269
	public bool CanReturnAngry;

	// Token: 0x04005316 RID: 21270
	public float AutoCrazeTime = -1f;

	// Token: 0x04005317 RID: 21271
	public float PostCrazeHealth = 1f;

	// Token: 0x04005318 RID: 21272
	public tk2dSpriteAnimator scepterAnimator;

	// Token: 0x04005319 RID: 21273
	public Transform hatPoint;

	// Token: 0x0400531A RID: 21274
	public GameObject hatVfx;

	// Token: 0x0400531C RID: 21276
	private bool m_isCrazed;

	// Token: 0x0400531D RID: 21277
	private bool m_isCrying;

	// Token: 0x0400531E RID: 21278
	private float m_timer;
}
