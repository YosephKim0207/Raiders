using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000E38 RID: 3640
public class ThreeWishesBuff : AppliedEffectBase
{
	// Token: 0x06004D05 RID: 19717 RVA: 0x001A5ACC File Offset: 0x001A3CCC
	public override void Initialize(AppliedEffectBase source)
	{
		this.hh = base.GetComponent<HealthHaver>();
		this.m_extantCount = 1;
		if (source is ThreeWishesBuff)
		{
			ThreeWishesBuff threeWishesBuff = source as ThreeWishesBuff;
			this.NumRequired = threeWishesBuff.NumRequired;
			this.TriggersOnFrozenEnemy = threeWishesBuff.TriggersOnFrozenEnemy;
			this.OnlyOncePerEnemy = threeWishesBuff.OnlyOncePerEnemy;
			if (threeWishesBuff.OverheadVFX != null)
			{
				GameActor component = base.GetComponent<GameActor>();
				if (component && component.specRigidbody && component.specRigidbody.HitboxPixelCollider != null)
				{
					this.instantiatedVFX = component.PlayEffectOnActor(threeWishesBuff.OverheadVFX, new Vector3(0f, component.specRigidbody.HitboxPixelCollider.UnitDimensions.y, 0f), true, false, true);
				}
			}
		}
		else
		{
			UnityEngine.Object.Destroy(this);
		}
	}

	// Token: 0x06004D06 RID: 19718 RVA: 0x001A5BAC File Offset: 0x001A3DAC
	public void Increment(ThreeWishesBuff source)
	{
		this.m_extantCount++;
		this.DamageDelay = source.DamageDelay;
		this.DirectionalVFXOffset = source.DirectionalVFXOffset;
		bool flag = this.m_extantCount == this.NumRequired;
		if (this.TriggersOnFrozenEnemy)
		{
			flag = this.hh.gameActor.IsFrozen && this.m_extantCount > 0;
		}
		if (flag)
		{
			if (this.instantiatedVFX)
			{
				UnityEngine.Object.Destroy(this.instantiatedVFX);
			}
			if (this.instantiatedVFX2)
			{
				UnityEngine.Object.Destroy(this.instantiatedVFX2);
			}
			this.instantiatedVFX2 = null;
			this.instantiatedVFX = null;
			if (source.transform.position.x < this.hh.gameActor.CenterPosition.x)
			{
				GameObject gameObject = this.hh.gameActor.PlayEffectOnActor(source.FinalVFX_Right, this.DirectionalVFXOffset.WithX(this.DirectionalVFXOffset.x * -1f), true, false, false);
				tk2dBaseSprite component = gameObject.GetComponent<tk2dBaseSprite>();
				component.HeightOffGround = 3f;
				component.UpdateZDepth();
				GameManager.Instance.StartCoroutine(this.DelayedDamage(source.DamageDealt, gameObject, source.FinalVFX_Shared));
			}
			else
			{
				GameObject gameObject2 = this.hh.gameActor.PlayEffectOnActor(source.FinalVFX_Left, this.DirectionalVFXOffset, true, false, false);
				tk2dBaseSprite component2 = gameObject2.GetComponent<tk2dBaseSprite>();
				component2.HeightOffGround = 3f;
				component2.UpdateZDepth();
				GameManager.Instance.StartCoroutine(this.DelayedDamage(source.DamageDealt, gameObject2, source.FinalVFX_Shared));
			}
			if (source.doesExplosion)
			{
				Exploder.Explode(this.hh.gameActor.CenterPosition, source.explosionData, Vector2.zero, null, false, CoreDamageTypes.None, false);
			}
			if (!this.OnlyOncePerEnemy)
			{
				UnityEngine.Object.Destroy(this);
			}
			else
			{
				this.m_extantCount = -1000000;
			}
		}
		else if (source.OverheadVFX != null)
		{
			GameActor component3 = base.GetComponent<GameActor>();
			if (component3 && component3.specRigidbody && component3.specRigidbody.HitboxPixelCollider != null)
			{
				this.instantiatedVFX2 = component3.PlayEffectOnActor(source.OverheadVFX, new Vector3(0f, component3.specRigidbody.HitboxPixelCollider.UnitDimensions.y + 0.5f, 0f), true, false, true);
			}
		}
	}

	// Token: 0x06004D07 RID: 19719 RVA: 0x001A5E4C File Offset: 0x001A404C
	private IEnumerator DelayedDamage(float source, GameObject vfx, GameObject finalVfx)
	{
		float ela = 0f;
		while (ela < this.DamageDelay)
		{
			ela += BraveTime.DeltaTime;
			if (!this.hh || this.hh.IsDead)
			{
				if (vfx)
				{
					UnityEngine.Object.Destroy(vfx);
				}
				yield break;
			}
			yield return null;
		}
		if (this.hh)
		{
			this.hh.ApplyDamage(source, Vector2.zero, "Wish", CoreDamageTypes.None, DamageCategory.Normal, false, null, false);
			if (this.hh.gameActor && finalVfx)
			{
				this.hh.gameActor.PlayEffectOnActor(finalVfx, Vector3.zero, false, false, false);
			}
			StickyFrictionManager.Instance.RegisterCustomStickyFriction(0.1f, 0.1f, false, false);
			if (vfx)
			{
				vfx.transform.parent = null;
				GameManager.Instance.StartCoroutine(this.TimedDestroy(vfx, 2f));
			}
			if (this.hh.knockbackDoer)
			{
				this.hh.knockbackDoer.ApplyKnockback(Vector2.up, (float)((this.hh.GetCurrentHealth() > 0f) ? 30 : 100), false);
			}
		}
		else if (vfx)
		{
			UnityEngine.Object.Destroy(vfx);
		}
		yield break;
	}

	// Token: 0x06004D08 RID: 19720 RVA: 0x001A5E7C File Offset: 0x001A407C
	private IEnumerator TimedDestroy(GameObject target, float delay)
	{
		float ela = 0f;
		tk2dSpriteAnimator anim = target.GetComponent<tk2dSpriteAnimator>();
		SpriteAnimatorKiller killer = target.GetComponent<SpriteAnimatorKiller>();
		while (ela < delay)
		{
			ela += BraveTime.DeltaTime;
			if (anim && !anim.enabled)
			{
				anim.enabled = true;
			}
			if (killer && !killer.enabled)
			{
				killer.enabled = true;
			}
			yield return null;
		}
		yield return new WaitForSeconds(delay);
		UnityEngine.Object.Destroy(target);
		yield break;
	}

	// Token: 0x06004D09 RID: 19721 RVA: 0x001A5EA0 File Offset: 0x001A40A0
	public override void AddSelfToTarget(GameObject target)
	{
		if (this.SynergyContingent)
		{
			Projectile component = base.GetComponent<Projectile>();
			if (component && component.PossibleSourceGun && !component.PossibleSourceGun.OwnerHasSynergy(this.RequiredSynergy))
			{
				return;
			}
		}
		HealthHaver healthHaver = target.GetComponent<HealthHaver>();
		if (!healthHaver)
		{
			SpeculativeRigidbody component2 = target.GetComponent<SpeculativeRigidbody>();
			if (component2)
			{
				healthHaver = component2.healthHaver;
				if (healthHaver)
				{
					target = healthHaver.gameObject;
				}
			}
		}
		if (!healthHaver)
		{
			return;
		}
		ThreeWishesBuff[] components = target.GetComponents<ThreeWishesBuff>();
		if (components.Length > 0)
		{
			components[0].Increment(this);
			return;
		}
		ThreeWishesBuff threeWishesBuff = target.AddComponent<ThreeWishesBuff>();
		threeWishesBuff.Initialize(this);
	}

	// Token: 0x04004321 RID: 17185
	public bool SynergyContingent;

	// Token: 0x04004322 RID: 17186
	[LongNumericEnum]
	public CustomSynergyType RequiredSynergy;

	// Token: 0x04004323 RID: 17187
	public bool OnlyOncePerEnemy;

	// Token: 0x04004324 RID: 17188
	public bool TriggersOnFrozenEnemy;

	// Token: 0x04004325 RID: 17189
	public int NumRequired = 3;

	// Token: 0x04004326 RID: 17190
	public float DamageDelay = 0.4f;

	// Token: 0x04004327 RID: 17191
	public float DamageDealt = 50f;

	// Token: 0x04004328 RID: 17192
	public Vector3 DirectionalVFXOffset = new Vector3(3f, 0f, 0f);

	// Token: 0x04004329 RID: 17193
	public bool doesExplosion;

	// Token: 0x0400432A RID: 17194
	public ExplosionData explosionData;

	// Token: 0x0400432B RID: 17195
	public GameObject OverheadVFX;

	// Token: 0x0400432C RID: 17196
	public GameObject FinalVFX_Right;

	// Token: 0x0400432D RID: 17197
	public GameObject FinalVFX_Left;

	// Token: 0x0400432E RID: 17198
	public GameObject FinalVFX_Shared;

	// Token: 0x0400432F RID: 17199
	private int m_extantCount = 1;

	// Token: 0x04004330 RID: 17200
	private GameObject instantiatedVFX;

	// Token: 0x04004331 RID: 17201
	private GameObject instantiatedVFX2;

	// Token: 0x04004332 RID: 17202
	private HealthHaver hh;
}
