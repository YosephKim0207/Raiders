using System;
using System.Collections;
using System.Collections.Generic;
using Dungeonator;
using UnityEngine;

// Token: 0x020013C1 RID: 5057
public class KthuliberProjectileController : MonoBehaviour
{
	// Token: 0x060072A5 RID: 29349 RVA: 0x002D9338 File Offset: 0x002D7538
	private void Start()
	{
		this.m_projectile = base.GetComponent<Projectile>();
		Projectile projectile = this.m_projectile;
		projectile.OnHitEnemy = (Action<Projectile, SpeculativeRigidbody, bool>)Delegate.Combine(projectile.OnHitEnemy, new Action<Projectile, SpeculativeRigidbody, bool>(this.HandleHitEnemy));
	}

	// Token: 0x060072A6 RID: 29350 RVA: 0x002D9370 File Offset: 0x002D7570
	private void Update()
	{
		if (this.m_soulEnemy && this.m_projectile && this.m_projectile.OverrideMotionModule != null && this.m_projectile.OverrideMotionModule is OrbitProjectileMotionModule)
		{
			this.m_projectile.DieInAir(false, true, true, false);
		}
	}

	// Token: 0x060072A7 RID: 29351 RVA: 0x002D93D4 File Offset: 0x002D75D4
	private void HandleHitEnemy(Projectile source, SpeculativeRigidbody target, bool fatal)
	{
		if (!fatal && target)
		{
			AIActor component = target.GetComponent<AIActor>();
			if (component && component.IsNormalEnemy)
			{
				if (this.SuckVFX)
				{
					Vector2 vector = Vector2.Lerp(this.m_projectile.specRigidbody.UnitCenter, target.UnitCenter, 0.5f);
					SpawnManager.SpawnVFX(this.SuckVFX, vector, Quaternion.identity);
					AkSoundEngine.PostEvent("Play_WPN_kthulu_soul_01", base.gameObject);
				}
				if (this.OverheadVFX)
				{
					this.m_overheadVFX = component.PlayEffectOnActor(this.OverheadVFX, new Vector3(0.0625f, component.specRigidbody.HitboxPixelCollider.UnitDimensions.y, 0f), true, false, true);
				}
				this.m_soulEnemy = target;
				this.m_projectile.allowSelfShooting = true;
				this.m_projectile.collidesWithEnemies = false;
				this.m_projectile.collidesWithPlayer = true;
				this.m_projectile.UpdateCollisionMask();
				this.m_projectile.SetNewShooter(target);
				this.m_projectile.spriteAnimator.Play("kthuliber_full_projectile");
				this.m_projectile.specRigidbody.PrimaryPixelCollider.ColliderGenerationMode = PixelCollider.PixelColliderGeneration.Circle;
				this.m_projectile.specRigidbody.PrimaryPixelCollider.ManualOffsetX = -8;
				this.m_projectile.specRigidbody.PrimaryPixelCollider.ManualOffsetY = -8;
				this.m_projectile.specRigidbody.PrimaryPixelCollider.ManualDiameter = 16;
				this.m_projectile.specRigidbody.PrimaryPixelCollider.Regenerate(this.m_projectile.transform, true, true);
				this.m_projectile.specRigidbody.Reinitialize();
				int num = -1;
				if (PlayerController.AnyoneHasActiveBonusSynergy(CustomSynergyType.KALIBER_KPOW, out num))
				{
					Projectile projectile = this.m_projectile;
					projectile.ModifyVelocity = (Func<Vector2, Vector2>)Delegate.Combine(projectile.ModifyVelocity, new Func<Vector2, Vector2>(this.HomeTowardPlayer));
				}
				SpeculativeRigidbody specRigidbody = this.m_projectile.specRigidbody;
				specRigidbody.OnPreRigidbodyCollision = (SpeculativeRigidbody.OnPreRigidbodyCollisionDelegate)Delegate.Combine(specRigidbody.OnPreRigidbodyCollision, new SpeculativeRigidbody.OnPreRigidbodyCollisionDelegate(this.HandlePreRigidbodySoulCollision));
				base.StartCoroutine(this.FrameDelayedPostProcessing(source));
				base.StartCoroutine(this.SlowDownOverTime(source));
			}
		}
	}

	// Token: 0x060072A8 RID: 29352 RVA: 0x002D961C File Offset: 0x002D781C
	private Vector2 HomeTowardPlayer(Vector2 inVel)
	{
		PlayerController activePlayerClosestToPoint = GameManager.Instance.GetActivePlayerClosestToPoint(this.m_projectile.LastPosition.XY(), true);
		if (activePlayerClosestToPoint)
		{
			float num = Vector2.Distance(activePlayerClosestToPoint.CenterPosition, this.m_projectile.LastPosition.XY());
			if (num < 10f)
			{
				Vector2 vector = activePlayerClosestToPoint.CenterPosition - this.m_projectile.LastPosition.XY();
				return inVel.magnitude * vector.normalized;
			}
		}
		return inVel;
	}

	// Token: 0x060072A9 RID: 29353 RVA: 0x002D96A8 File Offset: 0x002D78A8
	private void HandlePreRigidbodySoulCollision(SpeculativeRigidbody myRigidbody, PixelCollider myPixelCollider, SpeculativeRigidbody otherRigidbody, PixelCollider otherPixelCollider)
	{
		if (otherRigidbody)
		{
			PlayerController component = otherRigidbody.GetComponent<PlayerController>();
			if (component)
			{
				PhysicsEngine.SkipCollision = true;
				if (this.m_soulEnemy)
				{
					HealthHaver component2 = this.m_soulEnemy.GetComponent<HealthHaver>();
					if (component2 && !component2.IsBoss)
					{
						component2.ApplyDamage(component2.GetMaxHealth(), Vector2.zero, "Soul Burn", CoreDamageTypes.Void, DamageCategory.Unstoppable, true, null, false);
					}
					else if (component2 && component2.IsBoss)
					{
						component2.ApplyDamage(this.BossDamage, Vector2.zero, "Soul Burn", CoreDamageTypes.Void, DamageCategory.Unstoppable, false, null, true);
						if (this.m_overheadVFX)
						{
							UnityEngine.Object.Destroy(this.m_overheadVFX);
							this.m_overheadVFX = null;
						}
					}
					if (component.CurrentRoom != null)
					{
						List<AIActor> activeEnemies = component.CurrentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All);
						if (activeEnemies != null)
						{
							for (int i = 0; i < activeEnemies.Count; i++)
							{
								if (activeEnemies[i] && activeEnemies[i].healthHaver)
								{
									activeEnemies[i].healthHaver.ApplyDamage(this.DamageToRoom, Vector2.zero, "Soul Burn", CoreDamageTypes.Void, DamageCategory.Unstoppable, false, null, false);
								}
							}
						}
					}
					if (this.ExplodeVFX)
					{
						AIActor component3 = this.m_soulEnemy.GetComponent<AIActor>();
						if (component3)
						{
							component3.PlayEffectOnActor(this.ExplodeVFX, Vector3.zero, false, true, false);
							AkSoundEngine.PostEvent("Play_WPN_kthulu_blast_01", base.gameObject);
						}
					}
				}
				if (this.PickupVFX)
				{
					component.PlayEffectOnActor(this.PickupVFX, Vector3.zero, false, true, false);
				}
				this.m_projectile.DieInAir(false, true, true, false);
			}
		}
	}

	// Token: 0x060072AA RID: 29354 RVA: 0x002D9884 File Offset: 0x002D7A84
	private IEnumerator SlowDownOverTime(Projectile p)
	{
		float elapsed = 0f;
		float duration = this.SlowDuration;
		float startSpeed = p.baseData.speed;
		while (elapsed < duration)
		{
			elapsed += BraveTime.DeltaTime;
			float t = elapsed / duration;
			if (!p)
			{
				yield break;
			}
			p.baseData.speed = Mathf.Lerp(startSpeed, this.SoulSpeed, t);
			p.UpdateSpeed();
			yield return null;
		}
		yield break;
	}

	// Token: 0x060072AB RID: 29355 RVA: 0x002D98A8 File Offset: 0x002D7AA8
	private IEnumerator FrameDelayedPostProcessing(Projectile p)
	{
		yield return null;
		if (p)
		{
			PierceProjModifier component = p.GetComponent<PierceProjModifier>();
			if (component)
			{
				component.BeastModeLevel = PierceProjModifier.BeastModeStatus.NOT_BEAST_MODE;
				component.penetration = 0;
				UnityEngine.Object.Destroy(component);
			}
			HomingModifier component2 = p.GetComponent<HomingModifier>();
			if (component2)
			{
				UnityEngine.Object.Destroy(component2);
			}
		}
		yield break;
	}

	// Token: 0x040073F8 RID: 29688
	public float BossDamage = 50f;

	// Token: 0x040073F9 RID: 29689
	public float SoulSpeed = 3f;

	// Token: 0x040073FA RID: 29690
	public float SlowDuration = 0.3f;

	// Token: 0x040073FB RID: 29691
	public float DamageToRoom = 5f;

	// Token: 0x040073FC RID: 29692
	public GameObject SuckVFX;

	// Token: 0x040073FD RID: 29693
	public GameObject PickupVFX;

	// Token: 0x040073FE RID: 29694
	public GameObject ExplodeVFX;

	// Token: 0x040073FF RID: 29695
	public GameObject OverheadVFX;

	// Token: 0x04007400 RID: 29696
	private Projectile m_projectile;

	// Token: 0x04007401 RID: 29697
	private SpeculativeRigidbody m_soulEnemy;

	// Token: 0x04007402 RID: 29698
	private GameObject m_overheadVFX;
}
