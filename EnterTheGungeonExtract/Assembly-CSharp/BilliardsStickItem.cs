using System;
using UnityEngine;

// Token: 0x020013A2 RID: 5026
public class BilliardsStickItem : PassiveItem
{
	// Token: 0x060071E0 RID: 29152 RVA: 0x002D40B4 File Offset: 0x002D22B4
	public override void Pickup(PlayerController player)
	{
		base.Pickup(player);
		player.PostProcessProjectile += this.HandlePostProcessProjectile;
		player.PostProcessBeam += this.HandlePostProcessBeam;
		player.PostProcessBeamTick += this.HandlePostProcessBeamTick;
	}

	// Token: 0x060071E1 RID: 29153 RVA: 0x002D40F4 File Offset: 0x002D22F4
	private void HandlePostProcessProjectile(Projectile targetProjectile, float effectChanceScalar)
	{
		targetProjectile.OnHitEnemy = (Action<Projectile, SpeculativeRigidbody, bool>)Delegate.Combine(targetProjectile.OnHitEnemy, new Action<Projectile, SpeculativeRigidbody, bool>(this.HandleHitEnemy));
		targetProjectile.AdjustPlayerProjectileTint(this.TintColor, this.TintPriority, 0f);
	}

	// Token: 0x060071E2 RID: 29154 RVA: 0x002D4130 File Offset: 0x002D2330
	private void HandleHitEnemy(Projectile sourceProjectile, SpeculativeRigidbody hitRigidbody, bool fatal)
	{
		if (fatal && hitRigidbody)
		{
			if (sourceProjectile)
			{
				sourceProjectile.baseData.force = 0f;
			}
			AIActor aiActor = hitRigidbody.aiActor;
			KnockbackDoer knockbackDoer = hitRigidbody.knockbackDoer;
			if (aiActor)
			{
				if (aiActor.GetComponent<ExplodeOnDeath>())
				{
					UnityEngine.Object.Destroy(aiActor.GetComponent<ExplodeOnDeath>());
				}
				hitRigidbody.AddCollisionLayerOverride(CollisionMask.LayerToMask(CollisionLayer.EnemyHitBox));
				hitRigidbody.OnPreRigidbodyCollision = (SpeculativeRigidbody.OnPreRigidbodyCollisionDelegate)Delegate.Combine(hitRigidbody.OnPreRigidbodyCollision, new SpeculativeRigidbody.OnPreRigidbodyCollisionDelegate(this.HandleHitEnemyHitEnemy));
			}
			if (knockbackDoer && sourceProjectile)
			{
				float num = -1f;
				AIActor nearestEnemyInDirection = aiActor.ParentRoom.GetNearestEnemyInDirection(aiActor.CenterPosition, sourceProjectile.Direction, this.AngleTolerance, out num, true);
				Vector2 vector = sourceProjectile.Direction;
				if (nearestEnemyInDirection)
				{
					vector = nearestEnemyInDirection.CenterPosition - aiActor.CenterPosition;
				}
				knockbackDoer.ApplyKnockback(vector, this.KnockbackForce, true);
			}
		}
	}

	// Token: 0x060071E3 RID: 29155 RVA: 0x002D4240 File Offset: 0x002D2440
	private void HandleHitEnemyHitEnemy(SpeculativeRigidbody myRigidbody, PixelCollider myPixelCollider, SpeculativeRigidbody otherRigidbody, PixelCollider otherPixelCollider)
	{
		if (otherRigidbody && otherRigidbody.aiActor && myRigidbody && myRigidbody.healthHaver)
		{
			AIActor aiActor = otherRigidbody.aiActor;
			myRigidbody.OnPreRigidbodyCollision = (SpeculativeRigidbody.OnPreRigidbodyCollisionDelegate)Delegate.Remove(myRigidbody.OnPreRigidbodyCollision, new SpeculativeRigidbody.OnPreRigidbodyCollisionDelegate(this.HandleHitEnemyHitEnemy));
			if (aiActor.IsNormalEnemy && aiActor.healthHaver)
			{
				aiActor.healthHaver.ApplyDamage(myRigidbody.healthHaver.GetMaxHealth() * 2f, myRigidbody.Velocity, "Pinball", CoreDamageTypes.None, DamageCategory.Normal, false, null, false);
			}
		}
	}

	// Token: 0x060071E4 RID: 29156 RVA: 0x002D42F4 File Offset: 0x002D24F4
	private void HandlePostProcessBeam(BeamController targetBeam)
	{
	}

	// Token: 0x060071E5 RID: 29157 RVA: 0x002D42F8 File Offset: 0x002D24F8
	private void HandlePostProcessBeamTick(BeamController arg1, SpeculativeRigidbody arg2, float arg3)
	{
	}

	// Token: 0x060071E6 RID: 29158 RVA: 0x002D42FC File Offset: 0x002D24FC
	public override DebrisObject Drop(PlayerController player)
	{
		if (player)
		{
			player.PostProcessProjectile -= this.HandlePostProcessProjectile;
			player.PostProcessBeam -= this.HandlePostProcessBeam;
			player.PostProcessBeamTick -= this.HandlePostProcessBeamTick;
		}
		return base.Drop(player);
	}

	// Token: 0x060071E7 RID: 29159 RVA: 0x002D4354 File Offset: 0x002D2554
	protected override void OnDestroy()
	{
		if (base.Owner)
		{
			base.Owner.PostProcessProjectile -= this.HandlePostProcessProjectile;
			base.Owner.PostProcessBeam -= this.HandlePostProcessBeam;
			base.Owner.PostProcessBeamTick -= this.HandlePostProcessBeamTick;
		}
		base.OnDestroy();
	}

	// Token: 0x04007352 RID: 29522
	public float KnockbackForce = 800f;

	// Token: 0x04007353 RID: 29523
	public float AngleTolerance = 30f;

	// Token: 0x04007354 RID: 29524
	public Color TintColor = Color.white;

	// Token: 0x04007355 RID: 29525
	public int TintPriority = 2;
}
