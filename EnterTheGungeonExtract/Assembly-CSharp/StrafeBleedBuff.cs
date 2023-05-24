using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000E33 RID: 3635
public class StrafeBleedBuff : AppliedEffectBase
{
	// Token: 0x06004CE9 RID: 19689 RVA: 0x001A4E18 File Offset: 0x001A3018
	private void InitializeSelf(StrafeBleedBuff source)
	{
		if (!source)
		{
			return;
		}
		this.m_initialized = true;
		this.explosionData = source.explosionData;
		this.PreventExplosion = source.PreventExplosion;
		this.hh = base.GetComponent<HealthHaver>();
		if (this.hh != null)
		{
			Projectile component = source.GetComponent<Projectile>();
			if (component.PossibleSourceGun != null)
			{
				this.m_attachedGun = component.PossibleSourceGun;
				Gun possibleSourceGun = component.PossibleSourceGun;
				possibleSourceGun.OnFinishAttack = (Action<PlayerController, Gun>)Delegate.Combine(possibleSourceGun.OnFinishAttack, new Action<PlayerController, Gun>(this.HandleCeaseAttack));
			}
			else if (component && component.Owner && component.Owner.CurrentGun)
			{
				this.m_attachedGun = component.Owner.CurrentGun;
				Gun currentGun = component.Owner.CurrentGun;
				currentGun.OnFinishAttack = (Action<PlayerController, Gun>)Delegate.Combine(currentGun.OnFinishAttack, new Action<PlayerController, Gun>(this.HandleCeaseAttack));
			}
		}
		else
		{
			UnityEngine.Object.Destroy(this);
		}
	}

	// Token: 0x06004CEA RID: 19690 RVA: 0x001A4F38 File Offset: 0x001A3138
	private void Update()
	{
		if (this.m_initialized)
		{
			this.m_elapsed += BraveTime.DeltaTime;
			if (this.m_elapsed > this.CascadeTime)
			{
				this.DoEffect();
				this.Disconnect();
			}
		}
	}

	// Token: 0x06004CEB RID: 19691 RVA: 0x001A4F74 File Offset: 0x001A3174
	private void HandleCeaseAttack(PlayerController arg1, Gun arg2)
	{
		this.DoEffect();
		this.Disconnect();
	}

	// Token: 0x06004CEC RID: 19692 RVA: 0x001A4F84 File Offset: 0x001A3184
	private void Disconnect()
	{
		this.m_initialized = false;
		if (this.m_attachedGun)
		{
			Gun attachedGun = this.m_attachedGun;
			attachedGun.OnFinishAttack = (Action<PlayerController, Gun>)Delegate.Remove(attachedGun.OnFinishAttack, new Action<PlayerController, Gun>(this.HandleCeaseAttack));
		}
	}

	// Token: 0x06004CED RID: 19693 RVA: 0x001A4FC4 File Offset: 0x001A31C4
	public override void Initialize(AppliedEffectBase source)
	{
		if (source is StrafeBleedBuff)
		{
			StrafeBleedBuff strafeBleedBuff = source as StrafeBleedBuff;
			if (base.GetComponent<StrafeBleedBuff>() == this && strafeBleedBuff.additionalVFX != null && base.GetComponent<SpeculativeRigidbody>())
			{
				SpeculativeRigidbody component = base.GetComponent<SpeculativeRigidbody>();
				GameObject gameObject = SpawnManager.SpawnVFX(strafeBleedBuff.additionalVFX, component.UnitCenter, Quaternion.identity, true);
				gameObject.transform.parent = base.transform;
			}
			this.InitializeSelf(strafeBleedBuff);
			if (strafeBleedBuff.vfx != null)
			{
				this.instantiatedVFX = SpawnManager.SpawnVFX(strafeBleedBuff.vfx, base.transform.position, Quaternion.identity, true);
				tk2dSprite component2 = this.instantiatedVFX.GetComponent<tk2dSprite>();
				tk2dSprite component3 = base.GetComponent<tk2dSprite>();
				if (component2 != null && component3 != null)
				{
					component3.AttachRenderer(component2);
					component2.HeightOffGround = 0.1f;
					component2.IsPerpendicular = true;
					component2.usesOverrideMaterial = true;
				}
				BuffVFXAnimator component4 = this.instantiatedVFX.GetComponent<BuffVFXAnimator>();
				if (component4 != null)
				{
					Projectile component5 = source.GetComponent<Projectile>();
					if (component5 && component5.LastVelocity != Vector2.zero)
					{
						this.m_cachedSourceVector = component5.LastVelocity;
						component4.InitializePierce(base.GetComponent<GameActor>(), component5.LastVelocity);
					}
					else
					{
						component4.Initialize(base.GetComponent<GameActor>());
					}
				}
			}
		}
		else
		{
			UnityEngine.Object.Destroy(this);
		}
	}

	// Token: 0x06004CEE RID: 19694 RVA: 0x001A5154 File Offset: 0x001A3354
	public override void AddSelfToTarget(GameObject target)
	{
		if (target.GetComponent<HealthHaver>() == null)
		{
			return;
		}
		StrafeBleedBuff strafeBleedBuff = target.AddComponent<StrafeBleedBuff>();
		strafeBleedBuff.Initialize(this);
	}

	// Token: 0x06004CEF RID: 19695 RVA: 0x001A5184 File Offset: 0x001A3384
	private void DoEffect()
	{
		if (this.hh && !this.PreventExplosion)
		{
			float num = this.explosionData.force / 4f;
			this.explosionData.force = 0f;
			if (this.hh.specRigidbody)
			{
				if (this.explosionData.ignoreList == null)
				{
					this.explosionData.ignoreList = new List<SpeculativeRigidbody>();
				}
				this.explosionData.ignoreList.Add(this.hh.specRigidbody);
				this.hh.ApplyDamage(this.explosionData.damage, this.m_cachedSourceVector.normalized, "Strafe", CoreDamageTypes.None, DamageCategory.Normal, false, null, false);
			}
			if (this.instantiatedVFX != null)
			{
				Exploder.Explode(this.instantiatedVFX.GetComponent<tk2dBaseSprite>().WorldCenter + this.m_cachedSourceVector.normalized * -0.5f, this.explosionData, Vector2.zero, null, true, CoreDamageTypes.None, false);
			}
			else
			{
				Exploder.Explode(this.hh.aiActor.CenterPosition, this.explosionData, Vector2.zero, null, true, CoreDamageTypes.None, false);
			}
			if (this.hh.knockbackDoer && this.m_cachedSourceVector != Vector2.zero)
			{
				this.hh.knockbackDoer.ApplyKnockback(this.m_cachedSourceVector.normalized, num, false);
			}
		}
		if (this.instantiatedVFX)
		{
			UnityEngine.Object.Destroy(this.instantiatedVFX);
		}
		UnityEngine.Object.Destroy(this);
	}

	// Token: 0x06004CF0 RID: 19696 RVA: 0x001A5334 File Offset: 0x001A3534
	private void OnDestroy()
	{
		this.Disconnect();
	}

	// Token: 0x040042FA RID: 17146
	public bool PreventExplosion;

	// Token: 0x040042FB RID: 17147
	public ExplosionData explosionData;

	// Token: 0x040042FC RID: 17148
	public GameObject vfx;

	// Token: 0x040042FD RID: 17149
	public GameObject additionalVFX;

	// Token: 0x040042FE RID: 17150
	public float CascadeTime = 3f;

	// Token: 0x040042FF RID: 17151
	private GameObject instantiatedVFX;

	// Token: 0x04004300 RID: 17152
	private Gun m_attachedGun;

	// Token: 0x04004301 RID: 17153
	private HealthHaver hh;

	// Token: 0x04004302 RID: 17154
	private bool m_initialized;

	// Token: 0x04004303 RID: 17155
	private float m_elapsed;

	// Token: 0x04004304 RID: 17156
	private Vector2 m_cachedSourceVector = Vector2.zero;
}
