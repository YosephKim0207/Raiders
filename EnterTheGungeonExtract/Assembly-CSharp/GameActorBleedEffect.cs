using System;
using UnityEngine;

// Token: 0x02000E1E RID: 3614
[Serializable]
public class GameActorBleedEffect : GameActorEffect
{
	// Token: 0x06004C87 RID: 19591 RVA: 0x001A1BB8 File Offset: 0x0019FDB8
	public bool ShouldVanishOnDeath(GameActor actor)
	{
		return (!actor.healthHaver || !actor.healthHaver.IsBoss) && (!(actor is AIActor) || !(actor as AIActor).IsSignatureEnemy);
	}

	// Token: 0x06004C88 RID: 19592 RVA: 0x001A1C08 File Offset: 0x0019FE08
	public override void ApplyTint(GameActor actor)
	{
	}

	// Token: 0x06004C89 RID: 19593 RVA: 0x001A1C0C File Offset: 0x0019FE0C
	public override void OnEffectApplied(GameActor actor, RuntimeGameActorEffectData effectData, float partialAmount = 1f)
	{
		if (actor && actor.healthHaver && actor.healthHaver.IsDead)
		{
			effectData.accumulator = 0f;
			return;
		}
		this.m_isHammerOfDawn = this.vfxExplosion.GetComponent<HammerOfDawnController>() != null;
		effectData.accumulator += this.ChargeAmount * partialAmount;
	}

	// Token: 0x06004C8A RID: 19594 RVA: 0x001A1C7C File Offset: 0x0019FE7C
	public override void OnDarkSoulsAccumulate(GameActor actor, RuntimeGameActorEffectData effectData, float partialAmount = 1f, Projectile sourceProjectile = null)
	{
		if (actor && actor.healthHaver && actor.healthHaver.IsDead)
		{
			effectData.accumulator = 0f;
			return;
		}
		if (this.m_isHammerOfDawn && HammerOfDawnController.HasExtantHammer(sourceProjectile))
		{
			effectData.accumulator = 0f;
			return;
		}
		effectData.accumulator += this.ChargeAmount * partialAmount;
		if ((!this.m_isHammerOfDawn || !HammerOfDawnController.HasExtantHammer(sourceProjectile)) && !this.m_extantReticle)
		{
			this.m_extantReticle = UnityEngine.Object.Instantiate<GameObject>(this.vfxChargingReticle, actor.specRigidbody.HitboxPixelCollider.UnitBottomCenter, Quaternion.identity);
			this.m_extantReticle.transform.parent = actor.transform;
			RailgunChargeEffectController component = this.m_extantReticle.GetComponent<RailgunChargeEffectController>();
			if (component)
			{
				component.IsManuallyControlled = true;
			}
		}
		if (effectData.accumulator > 100f && actor.healthHaver.IsAlive)
		{
			effectData.accumulator = 0f;
			if (!this.m_isHammerOfDawn || !HammerOfDawnController.HasExtantHammer(sourceProjectile))
			{
				GameObject gameObject;
				if (this.m_isHammerOfDawn)
				{
					gameObject = UnityEngine.Object.Instantiate<GameObject>(this.vfxExplosion, actor.transform.position, Quaternion.identity);
				}
				else
				{
					gameObject = actor.PlayEffectOnActor(this.vfxExplosion, Vector3.zero, false, false, false);
				}
				tk2dBaseSprite component2 = gameObject.GetComponent<tk2dBaseSprite>();
				if (actor && actor.specRigidbody && component2)
				{
					component2.PlaceAtPositionByAnchor(actor.specRigidbody.HitboxPixelCollider.UnitBottomCenter, tk2dBaseSprite.Anchor.LowerCenter);
				}
				HammerOfDawnController component3 = gameObject.GetComponent<HammerOfDawnController>();
				if (component3 && sourceProjectile)
				{
					component3.AssignOwner(sourceProjectile.Owner as PlayerController, sourceProjectile);
				}
			}
			if (this.m_extantReticle)
			{
				UnityEngine.Object.Destroy(this.m_extantReticle.gameObject);
				this.m_extantReticle = null;
			}
		}
	}

	// Token: 0x06004C8B RID: 19595 RVA: 0x001A1EA8 File Offset: 0x001A00A8
	public override void EffectTick(GameActor actor, RuntimeGameActorEffectData effectData)
	{
		if (effectData.accumulator > 0f)
		{
			effectData.accumulator = Mathf.Max(0f, effectData.accumulator - BraveTime.DeltaTime * this.ChargeDispelFactor);
		}
		if (this.m_extantReticle)
		{
			RailgunChargeEffectController component = this.m_extantReticle.GetComponent<RailgunChargeEffectController>();
			if (component)
			{
				component.ManualCompletionPercentage = effectData.accumulator / 100f;
			}
		}
	}

	// Token: 0x06004C8C RID: 19596 RVA: 0x001A1F24 File Offset: 0x001A0124
	public override void OnEffectRemoved(GameActor actor, RuntimeGameActorEffectData effectData)
	{
		if (this.m_extantReticle)
		{
			UnityEngine.Object.Destroy(this.m_extantReticle.gameObject);
			this.m_extantReticle = null;
		}
	}

	// Token: 0x06004C8D RID: 19597 RVA: 0x001A1F50 File Offset: 0x001A0150
	public override bool IsFinished(GameActor actor, RuntimeGameActorEffectData effectData, float elapsedTime)
	{
		return effectData.accumulator <= 0f;
	}

	// Token: 0x04004275 RID: 17013
	public float ChargeAmount = 10f;

	// Token: 0x04004276 RID: 17014
	public float ChargeDispelFactor = 10f;

	// Token: 0x04004277 RID: 17015
	public GameObject vfxChargingReticle;

	// Token: 0x04004278 RID: 17016
	public GameObject vfxExplosion;

	// Token: 0x04004279 RID: 17017
	private GameObject m_extantReticle;

	// Token: 0x0400427A RID: 17018
	private bool m_isHammerOfDawn;
}
