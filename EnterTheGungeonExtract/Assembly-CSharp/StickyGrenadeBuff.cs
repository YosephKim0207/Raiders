using System;
using UnityEngine;

// Token: 0x02000E31 RID: 3633
public class StickyGrenadeBuff : AppliedEffectBase
{
	// Token: 0x06004CD9 RID: 19673 RVA: 0x001A4698 File Offset: 0x001A2898
	private void InitializeSelf(StickyGrenadeBuff source)
	{
		if (!source)
		{
			return;
		}
		this.explosionData = source.explosionData;
		this.hh = base.GetComponent<HealthHaver>();
		if (this.hh != null)
		{
			Projectile component = source.GetComponent<Projectile>();
			if (component.PossibleSourceGun != null)
			{
				this.m_attachedGun = component.PossibleSourceGun;
				this.m_player = component.PossibleSourceGun.CurrentOwner as PlayerController;
				Gun possibleSourceGun = component.PossibleSourceGun;
				possibleSourceGun.OnReloadPressed = (Action<PlayerController, Gun, bool>)Delegate.Combine(possibleSourceGun.OnReloadPressed, new Action<PlayerController, Gun, bool>(this.ExplodeOnReload));
				if (this.m_player)
				{
					this.m_player.GunChanged += this.GunChanged;
				}
			}
			else if (component && component.Owner && component.Owner.CurrentGun)
			{
				this.m_attachedGun = component.Owner.CurrentGun;
				this.m_player = component.Owner as PlayerController;
				Gun currentGun = component.Owner.CurrentGun;
				currentGun.OnReloadPressed = (Action<PlayerController, Gun, bool>)Delegate.Combine(currentGun.OnReloadPressed, new Action<PlayerController, Gun, bool>(this.ExplodeOnReload));
				if (this.m_player)
				{
					this.m_player.GunChanged += this.GunChanged;
				}
			}
		}
		else
		{
			UnityEngine.Object.Destroy(this);
		}
	}

	// Token: 0x06004CDA RID: 19674 RVA: 0x001A4818 File Offset: 0x001A2A18
	private void Disconnect()
	{
		if (this.m_player)
		{
			this.m_player.GunChanged -= this.GunChanged;
		}
		if (this.m_attachedGun)
		{
			Gun attachedGun = this.m_attachedGun;
			attachedGun.OnReloadPressed = (Action<PlayerController, Gun, bool>)Delegate.Remove(attachedGun.OnReloadPressed, new Action<PlayerController, Gun, bool>(this.ExplodeOnReload));
		}
	}

	// Token: 0x06004CDB RID: 19675 RVA: 0x001A4884 File Offset: 0x001A2A84
	private void GunChanged(Gun arg1, Gun arg2, bool newGun)
	{
		this.Disconnect();
		this.DoEffect();
	}

	// Token: 0x06004CDC RID: 19676 RVA: 0x001A4894 File Offset: 0x001A2A94
	private void ExplodeOnReload(PlayerController arg1, Gun arg2, bool actual)
	{
		this.Disconnect();
		this.DoEffect();
	}

	// Token: 0x06004CDD RID: 19677 RVA: 0x001A48A4 File Offset: 0x001A2AA4
	public override void Initialize(AppliedEffectBase source)
	{
		if (source is StickyGrenadeBuff)
		{
			StickyGrenadeBuff stickyGrenadeBuff = source as StickyGrenadeBuff;
			this.InitializeSelf(stickyGrenadeBuff);
			if (stickyGrenadeBuff.vfx != null)
			{
				this.instantiatedVFX = SpawnManager.SpawnVFX(stickyGrenadeBuff.vfx, base.transform.position, Quaternion.identity, true);
				tk2dSprite component = this.instantiatedVFX.GetComponent<tk2dSprite>();
				tk2dSprite component2 = base.GetComponent<tk2dSprite>();
				if (component != null && component2 != null)
				{
					component2.AttachRenderer(component);
					component.HeightOffGround = 0.1f;
					component.IsPerpendicular = true;
					component.usesOverrideMaterial = true;
				}
				BuffVFXAnimator component3 = this.instantiatedVFX.GetComponent<BuffVFXAnimator>();
				if (component3 != null)
				{
					Projectile component4 = source.GetComponent<Projectile>();
					if (component4 && component4.LastVelocity != Vector2.zero)
					{
						this.m_cachedSourceVector = component4.LastVelocity;
						component3.InitializePierce(base.GetComponent<GameActor>(), component4.LastVelocity);
					}
					else
					{
						component3.Initialize(base.GetComponent<GameActor>());
					}
				}
			}
		}
		else
		{
			UnityEngine.Object.Destroy(this);
		}
	}

	// Token: 0x06004CDE RID: 19678 RVA: 0x001A49C8 File Offset: 0x001A2BC8
	public override void AddSelfToTarget(GameObject target)
	{
		if (target.GetComponent<HealthHaver>() == null)
		{
			return;
		}
		if (this.IsSynergyContingent)
		{
			Projectile component = base.GetComponent<Projectile>();
			if (!component || !(component.Owner is PlayerController))
			{
				return;
			}
			if (!(component.Owner as PlayerController).HasActiveBonusSynergy(this.RequiredSynergy, false))
			{
				return;
			}
		}
		StickyGrenadeBuff stickyGrenadeBuff = target.AddComponent<StickyGrenadeBuff>();
		stickyGrenadeBuff.Initialize(this);
	}

	// Token: 0x06004CDF RID: 19679 RVA: 0x001A4A48 File Offset: 0x001A2C48
	private void DoEffect()
	{
		if (this.hh)
		{
			float num = this.explosionData.force / 4f;
			this.explosionData.force = 0f;
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

	// Token: 0x06004CE0 RID: 19680 RVA: 0x001A4B74 File Offset: 0x001A2D74
	private void OnDestroy()
	{
		this.Disconnect();
	}

	// Token: 0x040042EE RID: 17134
	public bool IsSynergyContingent;

	// Token: 0x040042EF RID: 17135
	[LongNumericEnum]
	public CustomSynergyType RequiredSynergy;

	// Token: 0x040042F0 RID: 17136
	public ExplosionData explosionData;

	// Token: 0x040042F1 RID: 17137
	public GameObject vfx;

	// Token: 0x040042F2 RID: 17138
	private GameObject instantiatedVFX;

	// Token: 0x040042F3 RID: 17139
	private PlayerController m_player;

	// Token: 0x040042F4 RID: 17140
	private Gun m_attachedGun;

	// Token: 0x040042F5 RID: 17141
	private HealthHaver hh;

	// Token: 0x040042F6 RID: 17142
	private Vector2 m_cachedSourceVector = Vector2.zero;
}
