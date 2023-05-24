using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Dungeonator;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x02001319 RID: 4889
public class Gun : PickupObject, IPlayerInteractable
{
	// Token: 0x06006E18 RID: 28184 RVA: 0x002B4694 File Offset: 0x002B2894
	public IntVector2 GetCarryPixelOffset(PlayableCharacters id)
	{
		IntVector2 intVector = this.carryPixelOffset;
		if (this.UsesPerCharacterCarryPixelOffsets)
		{
			for (int i = 0; i < this.PerCharacterPixelOffsets.Length; i++)
			{
				if (this.PerCharacterPixelOffsets[i].character == id)
				{
					intVector += this.PerCharacterPixelOffsets[i].carryPixelOffset;
				}
			}
		}
		return intVector;
	}

	// Token: 0x1700109A RID: 4250
	// (get) Token: 0x06006E19 RID: 28185 RVA: 0x002B46FC File Offset: 0x002B28FC
	public GameActor CurrentOwner
	{
		get
		{
			return this.m_owner;
		}
	}

	// Token: 0x06006E1A RID: 28186 RVA: 0x002B4704 File Offset: 0x002B2904
	public bool OwnerHasSynergy(CustomSynergyType synergyToCheck)
	{
		return this.m_owner && this.m_owner is PlayerController && (this.m_owner as PlayerController).HasActiveBonusSynergy(synergyToCheck, false);
	}

	// Token: 0x1700109B RID: 4251
	// (get) Token: 0x06006E1B RID: 28187 RVA: 0x002B473C File Offset: 0x002B293C
	// (set) Token: 0x06006E1C RID: 28188 RVA: 0x002B4744 File Offset: 0x002B2944
	public ProjectileVolleyData RawSourceVolley
	{
		get
		{
			return this.rawVolley;
		}
		set
		{
			this.rawVolley = value;
		}
	}

	// Token: 0x1700109C RID: 4252
	// (get) Token: 0x06006E1D RID: 28189 RVA: 0x002B4750 File Offset: 0x002B2950
	// (set) Token: 0x06006E1E RID: 28190 RVA: 0x002B4768 File Offset: 0x002B2968
	public ProjectileVolleyData Volley
	{
		get
		{
			return this.modifiedVolley ?? this.rawVolley;
		}
		set
		{
			this.rawVolley = value;
		}
	}

	// Token: 0x1700109D RID: 4253
	// (get) Token: 0x06006E1F RID: 28191 RVA: 0x002B4774 File Offset: 0x002B2974
	public ProjectileVolleyData OptionalReloadVolley
	{
		get
		{
			return this.modifiedOptionalReloadVolley ?? this.rawOptionalReloadVolley;
		}
	}

	// Token: 0x1700109E RID: 4254
	// (get) Token: 0x06006E20 RID: 28192 RVA: 0x002B478C File Offset: 0x002B298C
	// (set) Token: 0x06006E21 RID: 28193 RVA: 0x002B47B8 File Offset: 0x002B29B8
	public int CurrentAmmo
	{
		get
		{
			if (this.RequiresFundsToShoot && this.m_owner is PlayerController)
			{
				return this.ClipShotsRemaining;
			}
			return this.ammo;
		}
		set
		{
			this.ammo = value;
		}
	}

	// Token: 0x1700109F RID: 4255
	// (get) Token: 0x06006E22 RID: 28194 RVA: 0x002B47C4 File Offset: 0x002B29C4
	// (set) Token: 0x06006E23 RID: 28195 RVA: 0x002B481C File Offset: 0x002B2A1C
	public bool InfiniteAmmo
	{
		get
		{
			if (this.m_owner && this.m_owner is PlayerController)
			{
				return this.LocalInfiniteAmmo || (this.m_owner as PlayerController).InfiniteAmmo.Value;
			}
			return this.LocalInfiniteAmmo;
		}
		set
		{
			this.LocalInfiniteAmmo = value;
		}
	}

	// Token: 0x06006E24 RID: 28196 RVA: 0x002B4828 File Offset: 0x002B2A28
	public int GetBaseMaxAmmo()
	{
		return this.maxAmmo;
	}

	// Token: 0x170010A0 RID: 4256
	// (get) Token: 0x06006E25 RID: 28197 RVA: 0x002B4830 File Offset: 0x002B2A30
	public int AdjustedMaxAmmo
	{
		get
		{
			if (this.InfiniteAmmo)
			{
				return int.MaxValue;
			}
			if (this.m_owner == null)
			{
				return this.maxAmmo;
			}
			if (!(this.m_owner is PlayerController))
			{
				return this.maxAmmo;
			}
			if (this.RequiresFundsToShoot)
			{
				return this.ClipShotsRemaining;
			}
			if ((this.m_owner as PlayerController).stats != null)
			{
				float statValue = (this.m_owner as PlayerController).stats.GetStatValue(PlayerStats.StatType.AmmoCapacityMultiplier);
				return Mathf.RoundToInt(statValue * (float)this.maxAmmo);
			}
			return this.maxAmmo;
		}
	}

	// Token: 0x06006E26 RID: 28198 RVA: 0x002B48D8 File Offset: 0x002B2AD8
	public void SetBaseMaxAmmo(int a)
	{
		this.maxAmmo = a;
	}

	// Token: 0x170010A1 RID: 4257
	// (get) Token: 0x06006E27 RID: 28199 RVA: 0x002B48E4 File Offset: 0x002B2AE4
	public float AdjustedReloadTime
	{
		get
		{
			float num = 1f;
			if (this.m_owner is PlayerController)
			{
				PlayerController playerController = this.m_owner as PlayerController;
				if (playerController.CurrentGun && playerController.CurrentSecondaryGun && playerController.CurrentSecondaryGun == this && playerController.CurrentGun != this)
				{
					return playerController.CurrentGun.AdjustedReloadTime;
				}
				num = playerController.stats.GetStatValue(PlayerStats.StatType.ReloadSpeed);
			}
			return this.reloadTime * num * this.AdditionalReloadMultiplier;
		}
	}

	// Token: 0x170010A2 RID: 4258
	// (get) Token: 0x06006E28 RID: 28200 RVA: 0x002B4980 File Offset: 0x002B2B80
	public bool UnswitchableGun
	{
		get
		{
			return this.m_unswitchableGun;
		}
	}

	// Token: 0x170010A3 RID: 4259
	// (get) Token: 0x06006E29 RID: 28201 RVA: 0x002B4988 File Offset: 0x002B2B88
	public bool LuteCompanionBuffActive
	{
		get
		{
			return this.IsLuteCompanionBuff && this.IsFiring;
		}
	}

	// Token: 0x170010A4 RID: 4260
	// (get) Token: 0x06006E2A RID: 28202 RVA: 0x002B49A0 File Offset: 0x002B2BA0
	// (set) Token: 0x06006E2B RID: 28203 RVA: 0x002B49A8 File Offset: 0x002B2BA8
	public float RemainingActiveCooldownAmount
	{
		get
		{
			return this.m_remainingActiveCooldownAmount;
		}
		set
		{
			if (this.m_remainingActiveCooldownAmount > 0f && value <= 0f && this.m_owner)
			{
				AkSoundEngine.PostEvent("Play_UI_cooldown_ready_01", this.m_owner.gameObject);
			}
			this.m_remainingActiveCooldownAmount = value;
		}
	}

	// Token: 0x170010A5 RID: 4261
	// (get) Token: 0x06006E2C RID: 28204 RVA: 0x002B4A00 File Offset: 0x002B2C00
	public float CurrentActiveItemChargeAmount
	{
		get
		{
			return Mathf.Clamp01(1f - this.m_remainingActiveCooldownAmount / this.ActiveItemStyleRechargeAmount);
		}
	}

	// Token: 0x170010A6 RID: 4262
	// (get) Token: 0x06006E2D RID: 28205 RVA: 0x002B4A1C File Offset: 0x002B2C1C
	public float CurrentAngle
	{
		get
		{
			return this.gunAngle;
		}
	}

	// Token: 0x170010A7 RID: 4263
	// (get) Token: 0x06006E2E RID: 28206 RVA: 0x002B4A24 File Offset: 0x002B2C24
	public ProjectileModule DefaultModule
	{
		get
		{
			if (this.Volley)
			{
				if (this.Volley.ModulesAreTiers)
				{
					for (int i = 0; i < this.Volley.projectiles.Count; i++)
					{
						ProjectileModule projectileModule = this.Volley.projectiles[i];
						if (projectileModule != null)
						{
							int num = ((projectileModule.CloneSourceIndex < 0) ? i : projectileModule.CloneSourceIndex);
							if (num == this.CurrentStrengthTier)
							{
								return projectileModule;
							}
						}
					}
				}
				return this.Volley.projectiles[0];
			}
			return this.singleModule;
		}
	}

	// Token: 0x170010A8 RID: 4264
	// (get) Token: 0x06006E2F RID: 28207 RVA: 0x002B4ACC File Offset: 0x002B2CCC
	public bool IsAutomatic
	{
		get
		{
			return this.DefaultModule.shootStyle == ProjectileModule.ShootStyle.Automatic;
		}
	}

	// Token: 0x170010A9 RID: 4265
	// (get) Token: 0x06006E30 RID: 28208 RVA: 0x002B4ADC File Offset: 0x002B2CDC
	public bool HasChargedProjectileReady
	{
		get
		{
			if (!this.m_isCurrentlyFiring)
			{
				return false;
			}
			if (this.Volley == null)
			{
				if (this.singleModule.shootStyle == ProjectileModule.ShootStyle.Charged)
				{
					ModuleShootData moduleShootData = this.m_moduleData[this.singleModule];
					ProjectileModule.ChargeProjectile chargeProjectile = this.singleModule.GetChargeProjectile(moduleShootData.chargeTime);
					if (chargeProjectile != null && chargeProjectile.Projectile)
					{
						return true;
					}
				}
				return false;
			}
			ProjectileVolleyData volley = this.Volley;
			for (int i = 0; i < volley.projectiles.Count; i++)
			{
				ProjectileModule projectileModule = volley.projectiles[i];
				if (projectileModule.shootStyle == ProjectileModule.ShootStyle.Charged)
				{
					ModuleShootData moduleShootData2 = this.m_moduleData[projectileModule];
					ProjectileModule.ChargeProjectile chargeProjectile2 = projectileModule.GetChargeProjectile(moduleShootData2.chargeTime);
					if (chargeProjectile2 != null && chargeProjectile2.Projectile)
					{
						return true;
					}
				}
			}
			return false;
		}
	}

	// Token: 0x170010AA RID: 4266
	// (get) Token: 0x06006E31 RID: 28209 RVA: 0x002B4BD0 File Offset: 0x002B2DD0
	public GameUIAmmoType.AmmoType AmmoType
	{
		get
		{
			return this.DefaultModule.ammoType;
		}
	}

	// Token: 0x170010AB RID: 4267
	// (get) Token: 0x06006E32 RID: 28210 RVA: 0x002B4BE0 File Offset: 0x002B2DE0
	public string CustomAmmoType
	{
		get
		{
			return this.DefaultModule.customAmmoType;
		}
	}

	// Token: 0x170010AC RID: 4268
	// (get) Token: 0x06006E33 RID: 28211 RVA: 0x002B4BF0 File Offset: 0x002B2DF0
	public override string DisplayName
	{
		get
		{
			EncounterTrackable component = base.GetComponent<EncounterTrackable>();
			if (component)
			{
				return component.GetModifiedDisplayName();
			}
			return this.gunName;
		}
	}

	// Token: 0x170010AD RID: 4269
	// (get) Token: 0x06006E34 RID: 28212 RVA: 0x002B4C1C File Offset: 0x002B2E1C
	// (set) Token: 0x06006E35 RID: 28213 RVA: 0x002B4D3C File Offset: 0x002B2F3C
	public int ClipShotsRemaining
	{
		get
		{
			if (this.RequiresFundsToShoot && this.m_owner is PlayerController)
			{
				return Mathf.FloorToInt((float)(this.m_owner as PlayerController).carriedConsumables.Currency / (float)this.CurrencyCostPerShot);
			}
			int num = this.ammo;
			if (this.m_moduleData == null || !this.m_moduleData.ContainsKey(this.DefaultModule))
			{
				num = ((this.DefaultModule.GetModNumberOfShotsInClip(this.CurrentOwner) > 0) ? this.DefaultModule.GetModNumberOfShotsInClip(this.CurrentOwner) : this.ammo);
			}
			else
			{
				num = ((this.DefaultModule.GetModNumberOfShotsInClip(this.CurrentOwner) > 0) ? (this.DefaultModule.GetModNumberOfShotsInClip(this.CurrentOwner) - this.m_moduleData[this.DefaultModule].numberShotsFired) : this.ammo);
			}
			if (num > this.ammo)
			{
				this.ClipShotsRemaining = this.ammo;
			}
			return Mathf.Min(num, this.ammo);
		}
		set
		{
			if (!this.m_moduleData.ContainsKey(this.DefaultModule))
			{
				return;
			}
			this.m_moduleData[this.DefaultModule].numberShotsFired = this.DefaultModule.GetModNumberOfShotsInClip(this.CurrentOwner) - value;
		}
	}

	// Token: 0x170010AE RID: 4270
	// (get) Token: 0x06006E36 RID: 28214 RVA: 0x002B4D8C File Offset: 0x002B2F8C
	public bool IsEmpty
	{
		get
		{
			return (!(this.Volley != null)) ? (!this.CheckHasLoadedModule(this.singleModule)) : (!this.CheckHasLoadedModule(this.Volley));
		}
	}

	// Token: 0x170010AF RID: 4271
	// (get) Token: 0x06006E37 RID: 28215 RVA: 0x002B4DC4 File Offset: 0x002B2FC4
	public int ClipCapacity
	{
		get
		{
			return (this.DefaultModule.GetModNumberOfShotsInClip(this.CurrentOwner) > 0) ? this.DefaultModule.GetModNumberOfShotsInClip(this.CurrentOwner) : this.AdjustedMaxAmmo;
		}
	}

	// Token: 0x170010B0 RID: 4272
	// (get) Token: 0x06006E38 RID: 28216 RVA: 0x002B4DFC File Offset: 0x002B2FFC
	private Vector3 ClipLaunchPoint
	{
		get
		{
			return (!(this.m_clipLaunchAttachPoint == null)) ? this.m_clipLaunchAttachPoint.position : Vector3.zero;
		}
	}

	// Token: 0x170010B1 RID: 4273
	// (get) Token: 0x06006E39 RID: 28217 RVA: 0x002B4E24 File Offset: 0x002B3024
	private Vector3 CasingLaunchPoint
	{
		get
		{
			return (!(this.m_casingLaunchAttachPoint == null)) ? this.m_casingLaunchAttachPoint.position : this.barrelOffset.position;
		}
	}

	// Token: 0x170010B2 RID: 4274
	// (get) Token: 0x06006E3A RID: 28218 RVA: 0x002B4E54 File Offset: 0x002B3054
	public GunHandedness Handedness
	{
		get
		{
			bool flag = this.m_owner is PlayerController && (this.m_owner as PlayerController).inventory != null && (this.m_owner as PlayerController).inventory.DualWielding;
			if (this.ammo == 0 && this.overrideOutOfAmmoHandedness != GunHandedness.AutoDetect)
			{
				return this.overrideOutOfAmmoHandedness;
			}
			if (this.IsPreppedForThrow)
			{
				return GunHandedness.OneHanded;
			}
			if (this.m_cachedGunHandedness == null)
			{
				if (this.gunHandedness == GunHandedness.AutoDetect)
				{
					Transform transform = base.transform.Find("SecondaryHand");
					bool flag2 = transform != null;
					if (this.IsTrickGun && this.TrickGunAlternatesHandedness)
					{
						flag2 = transform != null && transform.gameObject.activeSelf;
					}
					this.m_cachedGunHandedness = new GunHandedness?((!flag2) ? GunHandedness.OneHanded : GunHandedness.TwoHanded);
				}
				else
				{
					this.m_cachedGunHandedness = new GunHandedness?(this.gunHandedness);
				}
			}
			if (this.m_cachedGunHandedness == GunHandedness.TwoHanded && flag)
			{
				return GunHandedness.OneHanded;
			}
			return this.m_cachedGunHandedness.Value;
		}
	}

	// Token: 0x170010B3 RID: 4275
	// (get) Token: 0x06006E3B RID: 28219 RVA: 0x002B4F90 File Offset: 0x002B3190
	public bool IsForwardPosition
	{
		get
		{
			switch (this.gunPosition)
			{
			case GunPositionOverride.AutoDetect:
				return this.Handedness == GunHandedness.OneHanded || this.Handedness == GunHandedness.HiddenOneHanded;
			case GunPositionOverride.Forward:
				return true;
			case GunPositionOverride.Back:
				return false;
			default:
				Debug.LogWarning("Unhandled GunPositionOverride type: " + this.gunPosition);
				return true;
			}
		}
	}

	// Token: 0x170010B4 RID: 4276
	// (get) Token: 0x06006E3C RID: 28220 RVA: 0x002B4FF4 File Offset: 0x002B31F4
	public Transform PrimaryHandAttachPoint
	{
		get
		{
			return this.m_localAttachPoint;
		}
	}

	// Token: 0x170010B5 RID: 4277
	// (get) Token: 0x06006E3D RID: 28221 RVA: 0x002B4FFC File Offset: 0x002B31FC
	public Transform SecondaryHandAttachPoint
	{
		get
		{
			if (this.IsTrickGun && this.TrickGunAlternatesHandedness && this.m_offhandAttachPoint == null)
			{
				this.m_offhandAttachPoint = base.transform.Find("SecondaryHand");
			}
			return this.m_offhandAttachPoint;
		}
	}

	// Token: 0x170010B6 RID: 4278
	// (get) Token: 0x06006E3E RID: 28222 RVA: 0x002B504C File Offset: 0x002B324C
	public bool IsFiring
	{
		get
		{
			return this.m_isCurrentlyFiring;
		}
	}

	// Token: 0x170010B7 RID: 4279
	// (get) Token: 0x06006E3F RID: 28223 RVA: 0x002B5054 File Offset: 0x002B3254
	public bool IsReloading
	{
		get
		{
			return this.m_isReloading;
		}
	}

	// Token: 0x170010B8 RID: 4280
	// (get) Token: 0x06006E40 RID: 28224 RVA: 0x002B505C File Offset: 0x002B325C
	public bool IsCharging
	{
		get
		{
			if (!this.m_isCurrentlyFiring)
			{
				return false;
			}
			if (this.Volley != null)
			{
				for (int i = 0; i < this.Volley.projectiles.Count; i++)
				{
					ProjectileModule projectileModule = this.Volley.projectiles[i];
					if (projectileModule.shootStyle == ProjectileModule.ShootStyle.Charged && !this.m_moduleData[projectileModule].chargeFired)
					{
						return true;
					}
				}
			}
			else if (this.singleModule.shootStyle == ProjectileModule.ShootStyle.Charged && !this.m_moduleData[this.singleModule].chargeFired)
			{
				return true;
			}
			return false;
		}
	}

	// Token: 0x170010B9 RID: 4281
	// (get) Token: 0x06006E41 RID: 28225 RVA: 0x002B5114 File Offset: 0x002B3314
	// (set) Token: 0x06006E42 RID: 28226 RVA: 0x002B511C File Offset: 0x002B331C
	public bool NoOwnerOverride { get; set; }

	// Token: 0x170010BA RID: 4282
	// (get) Token: 0x06006E43 RID: 28227 RVA: 0x002B5128 File Offset: 0x002B3328
	// (set) Token: 0x06006E44 RID: 28228 RVA: 0x002B5130 File Offset: 0x002B3330
	public Projectile LastProjectile { get; set; }

	// Token: 0x170010BB RID: 4283
	// (get) Token: 0x06006E45 RID: 28229 RVA: 0x002B513C File Offset: 0x002B333C
	// (set) Token: 0x06006E46 RID: 28230 RVA: 0x002B5144 File Offset: 0x002B3344
	public int DefaultSpriteID
	{
		get
		{
			return this.m_defaultSpriteID;
		}
		set
		{
			this.m_defaultSpriteID = value;
		}
	}

	// Token: 0x170010BC RID: 4284
	// (get) Token: 0x06006E47 RID: 28231 RVA: 0x002B5150 File Offset: 0x002B3350
	public bool IsInWorld
	{
		get
		{
			return this.m_isThrown;
		}
	}

	// Token: 0x170010BD RID: 4285
	// (get) Token: 0x06006E48 RID: 28232 RVA: 0x002B5158 File Offset: 0x002B3358
	// (set) Token: 0x06006E49 RID: 28233 RVA: 0x002B5160 File Offset: 0x002B3360
	public bool LaserSightIsGreen { get; set; }

	// Token: 0x170010BE RID: 4286
	// (get) Token: 0x06006E4A RID: 28234 RVA: 0x002B516C File Offset: 0x002B336C
	// (set) Token: 0x06006E4B RID: 28235 RVA: 0x002B5174 File Offset: 0x002B3374
	public bool DoubleWideLaserSight { get; set; }

	// Token: 0x170010BF RID: 4287
	// (get) Token: 0x06006E4C RID: 28236 RVA: 0x002B5180 File Offset: 0x002B3380
	// (set) Token: 0x06006E4D RID: 28237 RVA: 0x002B5188 File Offset: 0x002B3388
	public bool ForceLaserSight { get; set; }

	// Token: 0x170010C0 RID: 4288
	// (get) Token: 0x06006E4E RID: 28238 RVA: 0x002B5194 File Offset: 0x002B3394
	public tk2dBaseSprite LaserSight
	{
		get
		{
			return this.m_extantLaserSight;
		}
	}

	// Token: 0x170010C1 RID: 4289
	// (get) Token: 0x06006E4F RID: 28239 RVA: 0x002B519C File Offset: 0x002B339C
	// (set) Token: 0x06006E50 RID: 28240 RVA: 0x002B51A4 File Offset: 0x002B33A4
	public bool IsMinusOneGun { get; set; }

	// Token: 0x06006E51 RID: 28241 RVA: 0x002B51B0 File Offset: 0x002B33B0
	public void TransformToTargetGun(Gun targetGun)
	{
		int clipShotsRemaining = this.ClipShotsRemaining;
		if (this.m_currentlyPlayingChargeVFX != null)
		{
			this.m_currentlyPlayingChargeVFX.DestroyAll();
			this.m_currentlyPlayingChargeVFX = null;
		}
		ProjectileVolleyData volley = this.Volley;
		this.rawVolley = targetGun.rawVolley;
		this.singleModule = targetGun.singleModule;
		this.modifiedVolley = null;
		if (targetGun.sprite)
		{
			this.m_defaultSpriteID = targetGun.sprite.spriteId;
			this.m_sprite.SetSprite(targetGun.sprite.Collection, this.m_defaultSpriteID);
			if (base.spriteAnimator && targetGun.spriteAnimator)
			{
				base.spriteAnimator.Library = targetGun.spriteAnimator.Library;
			}
			tk2dSpriteDefinition.AttachPoint[] attachPoints = this.m_sprite.Collection.GetAttachPoints(this.m_defaultSpriteID);
			tk2dSpriteDefinition.AttachPoint attachPoint;
			if (attachPoints != null)
			{
				attachPoint = Array.Find<tk2dSpriteDefinition.AttachPoint>(attachPoints, (tk2dSpriteDefinition.AttachPoint a) => a.name == "PrimaryHand");
			}
			else
			{
				attachPoint = null;
			}
			tk2dSpriteDefinition.AttachPoint attachPoint2 = attachPoint;
			if (attachPoint2 != null)
			{
				this.m_defaultLocalPosition = -attachPoint2.position;
			}
		}
		if (targetGun.maxAmmo != this.maxAmmo && targetGun.maxAmmo > 0)
		{
			int num = ((!this.InfiniteAmmo) ? this.AdjustedMaxAmmo : this.maxAmmo);
			this.maxAmmo = targetGun.maxAmmo;
			if (this.AdjustedMaxAmmo > 0 && num > 0 && this.ammo > 0 && !this.InfiniteAmmo)
			{
				this.ammo = Mathf.FloorToInt((float)this.ammo / (float)num * (float)this.AdjustedMaxAmmo);
				this.ammo = Mathf.Min(this.ammo, this.AdjustedMaxAmmo);
			}
			else
			{
				this.ammo = Mathf.Min(this.ammo, this.maxAmmo);
			}
		}
		this.gunSwitchGroup = targetGun.gunSwitchGroup;
		this.isAudioLoop = targetGun.isAudioLoop;
		this.gunClass = targetGun.gunClass;
		if (!string.IsNullOrEmpty(this.gunSwitchGroup))
		{
			AkSoundEngine.SetSwitch("WPN_Guns", this.gunSwitchGroup, base.gameObject);
		}
		this.currentGunDamageTypeModifiers = targetGun.currentGunDamageTypeModifiers;
		this.carryPixelOffset = targetGun.carryPixelOffset;
		this.carryPixelUpOffset = targetGun.carryPixelUpOffset;
		this.carryPixelDownOffset = targetGun.carryPixelDownOffset;
		this.leftFacingPixelOffset = targetGun.leftFacingPixelOffset;
		this.UsesPerCharacterCarryPixelOffsets = targetGun.UsesPerCharacterCarryPixelOffsets;
		this.PerCharacterPixelOffsets = targetGun.PerCharacterPixelOffsets;
		this.gunPosition = targetGun.gunPosition;
		this.forceFlat = targetGun.forceFlat;
		if (targetGun.GainsRateOfFireAsContinueAttack != this.GainsRateOfFireAsContinueAttack)
		{
			this.GainsRateOfFireAsContinueAttack = targetGun.GainsRateOfFireAsContinueAttack;
			this.RateOfFireMultiplierAdditionPerSecond = targetGun.RateOfFireMultiplierAdditionPerSecond;
		}
		if (this.barrelOffset && targetGun.barrelOffset)
		{
			this.barrelOffset.localPosition = targetGun.barrelOffset.localPosition;
			this.m_originalBarrelOffsetPosition = targetGun.barrelOffset.localPosition;
		}
		if (this.muzzleOffset && targetGun.muzzleOffset)
		{
			this.muzzleOffset.localPosition = targetGun.muzzleOffset.localPosition;
			this.m_originalMuzzleOffsetPosition = targetGun.muzzleOffset.localPosition;
		}
		if (this.chargeOffset && targetGun.chargeOffset)
		{
			this.chargeOffset.localPosition = targetGun.chargeOffset.localPosition;
			this.m_originalChargeOffsetPosition = targetGun.chargeOffset.localPosition;
		}
		this.reloadTime = targetGun.reloadTime;
		this.blankDuringReload = targetGun.blankDuringReload;
		this.blankReloadRadius = targetGun.blankReloadRadius;
		this.reflectDuringReload = targetGun.reflectDuringReload;
		this.blankKnockbackPower = targetGun.blankKnockbackPower;
		this.blankDamageToEnemies = targetGun.blankDamageToEnemies;
		this.blankDamageScalingOnEmptyClip = targetGun.blankDamageScalingOnEmptyClip;
		this.doesScreenShake = targetGun.doesScreenShake;
		this.gunScreenShake = targetGun.gunScreenShake;
		this.directionlessScreenShake = targetGun.directionlessScreenShake;
		this.AppliesHoming = targetGun.AppliesHoming;
		this.AppliedHomingAngularVelocity = targetGun.AppliedHomingAngularVelocity;
		this.AppliedHomingDetectRadius = targetGun.AppliedHomingDetectRadius;
		this.GoopReloadsFree = targetGun.GoopReloadsFree;
		this.gunHandedness = targetGun.gunHandedness;
		this.m_cachedGunHandedness = null;
		this.shootAnimation = targetGun.shootAnimation;
		this.usesContinuousFireAnimation = targetGun.usesContinuousFireAnimation;
		this.reloadAnimation = targetGun.reloadAnimation;
		this.emptyReloadAnimation = targetGun.emptyReloadAnimation;
		this.idleAnimation = targetGun.idleAnimation;
		this.chargeAnimation = targetGun.chargeAnimation;
		this.dischargeAnimation = targetGun.dischargeAnimation;
		this.emptyAnimation = targetGun.emptyAnimation;
		this.introAnimation = targetGun.introAnimation;
		this.finalShootAnimation = targetGun.finalShootAnimation;
		this.enemyPreFireAnimation = targetGun.enemyPreFireAnimation;
		this.dodgeAnimation = targetGun.dodgeAnimation;
		this.muzzleFlashEffects = targetGun.muzzleFlashEffects;
		this.usesContinuousMuzzleFlash = targetGun.usesContinuousMuzzleFlash;
		this.finalMuzzleFlashEffects = targetGun.finalMuzzleFlashEffects;
		this.reloadEffects = targetGun.reloadEffects;
		this.emptyReloadEffects = targetGun.emptyReloadEffects;
		this.activeReloadSuccessEffects = targetGun.activeReloadSuccessEffects;
		this.activeReloadFailedEffects = targetGun.activeReloadFailedEffects;
		this.shellCasing = targetGun.shellCasing;
		this.shellsToLaunchOnFire = targetGun.shellsToLaunchOnFire;
		this.shellCasingOnFireFrameDelay = targetGun.shellCasingOnFireFrameDelay;
		this.shellsToLaunchOnReload = targetGun.shellsToLaunchOnReload;
		this.reloadShellLaunchFrame = targetGun.reloadShellLaunchFrame;
		this.clipObject = targetGun.clipObject;
		this.clipsToLaunchOnReload = targetGun.clipsToLaunchOnReload;
		this.reloadClipLaunchFrame = targetGun.reloadClipLaunchFrame;
		this.IsTrickGun = targetGun.IsTrickGun;
		this.TrickGunAlternatesHandedness = targetGun.TrickGunAlternatesHandedness;
		this.alternateVolley = targetGun.alternateVolley;
		this.alternateShootAnimation = targetGun.alternateShootAnimation;
		this.alternateReloadAnimation = targetGun.alternateReloadAnimation;
		this.alternateIdleAnimation = targetGun.alternateIdleAnimation;
		this.alternateSwitchGroup = targetGun.alternateSwitchGroup;
		this.rampBullets = targetGun.rampBullets;
		this.rampStartHeight = targetGun.rampStartHeight;
		this.rampTime = targetGun.rampTime;
		this.usesDirectionalAnimator = targetGun.usesDirectionalAnimator;
		this.usesDirectionalIdleAnimations = targetGun.usesDirectionalIdleAnimations;
		if (base.aiAnimator)
		{
			UnityEngine.Object.Destroy(base.aiAnimator);
			base.aiAnimator = null;
		}
		if (targetGun.aiAnimator)
		{
			AIAnimator aianimator = base.gameObject.AddComponent<AIAnimator>();
			AIAnimator aiAnimator = targetGun.aiAnimator;
			aianimator.facingType = aiAnimator.facingType;
			aianimator.DirectionParent = aiAnimator.DirectionParent;
			aianimator.faceSouthWhenStopped = aiAnimator.faceSouthWhenStopped;
			aianimator.faceTargetWhenStopped = aiAnimator.faceTargetWhenStopped;
			aianimator.directionalType = aiAnimator.directionalType;
			aianimator.RotationQuantizeTo = aiAnimator.RotationQuantizeTo;
			aianimator.RotationOffset = aiAnimator.RotationOffset;
			aianimator.ForceKillVfxOnPreDeath = aiAnimator.ForceKillVfxOnPreDeath;
			aianimator.SuppressAnimatorFallback = aiAnimator.SuppressAnimatorFallback;
			aianimator.IsBodySprite = aiAnimator.IsBodySprite;
			aianimator.IdleAnimation = aiAnimator.IdleAnimation;
			aianimator.MoveAnimation = aiAnimator.MoveAnimation;
			aianimator.FlightAnimation = aiAnimator.FlightAnimation;
			aianimator.HitAnimation = aiAnimator.HitAnimation;
			aianimator.OtherAnimations = aiAnimator.OtherAnimations;
			aianimator.OtherVFX = aiAnimator.OtherVFX;
			aianimator.OtherScreenShake = aiAnimator.OtherScreenShake;
			aianimator.IdleFidgetAnimations = aiAnimator.IdleFidgetAnimations;
			base.aiAnimator = aianimator;
		}
		MultiTemporaryOrbitalSynergyProcessor component = targetGun.GetComponent<MultiTemporaryOrbitalSynergyProcessor>();
		MultiTemporaryOrbitalSynergyProcessor component2 = base.GetComponent<MultiTemporaryOrbitalSynergyProcessor>();
		if (!component && component2)
		{
			UnityEngine.Object.Destroy(component2);
		}
		else if (component && !component2)
		{
			MultiTemporaryOrbitalSynergyProcessor multiTemporaryOrbitalSynergyProcessor = base.gameObject.AddComponent<MultiTemporaryOrbitalSynergyProcessor>();
			multiTemporaryOrbitalSynergyProcessor.RequiredSynergy = component.RequiredSynergy;
			multiTemporaryOrbitalSynergyProcessor.OrbitalPrefab = component.OrbitalPrefab;
		}
		if (this.rawVolley != null)
		{
			for (int i = 0; i < this.rawVolley.projectiles.Count; i++)
			{
				this.rawVolley.projectiles[i].ResetRuntimeData();
			}
		}
		else
		{
			this.singleModule.ResetRuntimeData();
		}
		if (volley != null)
		{
			this.RawSourceVolley = DuctTapeItem.TransferDuctTapeModules(volley, this.RawSourceVolley, this);
		}
		if (this.m_owner is PlayerController)
		{
			PlayerController playerController = this.m_owner as PlayerController;
			if (playerController.stats != null)
			{
				playerController.stats.RecalculateStats(playerController, false, false);
			}
		}
		if (base.gameObject.activeSelf)
		{
			base.StartCoroutine(this.HandleFrameDelayedTransformation());
		}
		this.DidTransformGunThisFrame = true;
	}

	// Token: 0x06006E52 RID: 28242 RVA: 0x002B5A88 File Offset: 0x002B3C88
	private IEnumerator HandleFrameDelayedTransformation()
	{
		yield return null;
		if (!string.IsNullOrEmpty(this.gunSwitchGroup))
		{
			AkSoundEngine.SetSwitch("WPN_Guns", this.gunSwitchGroup, base.gameObject);
		}
		if (base.spriteAnimator && !string.IsNullOrEmpty(this.introAnimation))
		{
			base.spriteAnimator.Play(this.introAnimation);
		}
		else
		{
			this.PlayIdleAnimation();
		}
		if (this && base.enabled && this.CurrentOwner)
		{
			this.HandleSpriteFlip(false);
			this.HandleSpriteFlip(true);
			this.HandleSpriteFlip(this.CurrentOwner.SpriteFlipped);
		}
		yield break;
	}

	// Token: 0x06006E53 RID: 28243 RVA: 0x002B5AA4 File Offset: 0x002B3CA4
	public void Initialize(GameActor owner)
	{
		if (!this.m_sprite)
		{
			this.Awake();
		}
		this.m_owner = owner;
		this.m_transform = base.transform;
		this.m_attachTransform = base.transform.parent;
		this.m_anim = base.GetComponent<tk2dSpriteAnimator>();
		this.m_anim.AnimationCompleted = new Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip>(this.AnimationCompleteDelegate);
		this.m_sprite.automaticallyManagesDepth = false;
		this.m_sprite.IsPerpendicular = !this.forceFlat;
		this.m_sprite.independentOrientation = true;
		if (this.forceFlat)
		{
			owner.sprite.AttachRenderer(this.m_sprite);
			this.m_sprite.HeightOffGround = 0.25f;
			this.m_sprite.UpdateZDepth();
		}
		this.m_moduleData = new Dictionary<ProjectileModule, ModuleShootData>();
		if (this.Volley != null)
		{
			for (int i = 0; i < this.Volley.projectiles.Count; i++)
			{
				ModuleShootData moduleShootData = new ModuleShootData();
				if (this.ammo < this.Volley.projectiles[i].GetModNumberOfShotsInClip(this.CurrentOwner))
				{
					moduleShootData.numberShotsFired = this.Volley.projectiles[i].GetModNumberOfShotsInClip(this.CurrentOwner) - this.ammo;
				}
				this.m_moduleData.Add(this.Volley.projectiles[i], moduleShootData);
			}
		}
		else
		{
			ModuleShootData moduleShootData2 = new ModuleShootData();
			if (this.ammo < this.singleModule.GetModNumberOfShotsInClip(this.CurrentOwner))
			{
				moduleShootData2.numberShotsFired = this.singleModule.GetModNumberOfShotsInClip(this.CurrentOwner) - this.ammo;
			}
			this.m_moduleData.Add(this.singleModule, moduleShootData2);
		}
		if (this.modifiedFinalVolley)
		{
			for (int j = 0; j < this.modifiedFinalVolley.projectiles.Count; j++)
			{
				ModuleShootData moduleShootData3 = new ModuleShootData();
				if (this.ammo < this.modifiedFinalVolley.projectiles[j].GetModNumberOfShotsInClip(this.CurrentOwner))
				{
					moduleShootData3.numberShotsFired = this.modifiedFinalVolley.projectiles[j].GetModNumberOfShotsInClip(this.CurrentOwner) - this.ammo;
				}
				this.m_moduleData.Add(this.modifiedFinalVolley.projectiles[j], moduleShootData3);
			}
		}
		if (this.modifiedOptionalReloadVolley)
		{
			for (int k = 0; k < this.modifiedOptionalReloadVolley.projectiles.Count; k++)
			{
				ModuleShootData moduleShootData4 = new ModuleShootData();
				if (this.ammo < this.modifiedOptionalReloadVolley.projectiles[k].GetModNumberOfShotsInClip(this.CurrentOwner))
				{
					moduleShootData4.numberShotsFired = this.modifiedOptionalReloadVolley.projectiles[k].GetModNumberOfShotsInClip(this.CurrentOwner) - this.ammo;
				}
				this.m_moduleData.Add(this.modifiedOptionalReloadVolley.projectiles[k], moduleShootData4);
			}
		}
		if (this.procGunData != null)
		{
			this.ApplyProcGunData(this.procGunData);
		}
		if (this.m_childTransformsToFlip == null)
		{
			this.m_childTransformsToFlip = new List<Transform>();
		}
		for (int l = 0; l < this.m_transform.childCount; l++)
		{
			Transform child = this.m_transform.GetChild(l);
			if (child.GetComponent<Light>() != null)
			{
				this.m_childTransformsToFlip.Add(child);
			}
		}
		tk2dSpriteDefinition.AttachPoint[] attachPoints = this.m_sprite.Collection.GetAttachPoints(this.m_defaultSpriteID);
		tk2dSpriteDefinition.AttachPoint attachPoint;
		if (attachPoints != null)
		{
			attachPoint = Array.Find<tk2dSpriteDefinition.AttachPoint>(attachPoints, (tk2dSpriteDefinition.AttachPoint a) => a.name == "PrimaryHand");
		}
		else
		{
			attachPoint = null;
		}
		tk2dSpriteDefinition.AttachPoint attachPoint2 = attachPoint;
		this.m_defaultLocalPosition = -attachPoint2.position;
		if (this.AppliesHoming)
		{
			this.PostProcessProjectile = (Action<Projectile>)Delegate.Combine(this.PostProcessProjectile, new Action<Projectile>(this.ApplyHomingToProjectile));
		}
		if (this.m_owner != null)
		{
			SpeculativeRigidbody specRigidbody = this.m_owner.specRigidbody;
			specRigidbody.OnPostRigidbodyMovement = (Action<SpeculativeRigidbody, Vector2, IntVector2>)Delegate.Combine(specRigidbody.OnPostRigidbodyMovement, new Action<SpeculativeRigidbody, Vector2, IntVector2>(this.PostRigidbodyMovement));
		}
		if (this.OnInitializedWithOwner != null)
		{
			this.OnInitializedWithOwner(this.m_owner);
		}
	}

	// Token: 0x06006E54 RID: 28244 RVA: 0x002B5F34 File Offset: 0x002B4134
	public void UpdateAttachTransform()
	{
		this.m_attachTransform = base.transform.parent;
	}

	// Token: 0x06006E55 RID: 28245 RVA: 0x002B5F48 File Offset: 0x002B4148
	private void ApplyHomingToProjectile(Projectile obj)
	{
		if (obj is HomingProjectile)
		{
			return;
		}
		HomingModifier homingModifier = obj.GetComponent<HomingModifier>();
		if (homingModifier)
		{
			homingModifier.AngularVelocity = Mathf.Max(homingModifier.AngularVelocity, this.AppliedHomingAngularVelocity);
			homingModifier.HomingRadius = Mathf.Max(homingModifier.HomingRadius, this.AppliedHomingDetectRadius);
		}
		else
		{
			homingModifier = obj.gameObject.AddComponent<HomingModifier>();
			homingModifier.AngularVelocity = this.AppliedHomingAngularVelocity;
			homingModifier.HomingRadius = this.AppliedHomingDetectRadius;
		}
	}

	// Token: 0x06006E56 RID: 28246 RVA: 0x002B5FCC File Offset: 0x002B41CC
	private void InitializeDefaultFrame()
	{
		if (this.m_defaultSpriteID == 0)
		{
			PickupObject pickupObject = PickupObjectDatabase.Instance.InternalGetById(this.PickupObjectId);
			if (pickupObject != null)
			{
				this.m_defaultSpriteID = pickupObject.sprite.spriteId;
			}
			else
			{
				this.m_defaultSpriteID = base.sprite.spriteId;
			}
		}
	}

	// Token: 0x06006E57 RID: 28247 RVA: 0x002B6028 File Offset: 0x002B4228
	public void ReinitializeModuleData(ProjectileVolleyData originalSourceVolley)
	{
		Dictionary<ProjectileModule, ModuleShootData> moduleData = this.m_moduleData;
		if (this.m_moduleData == null)
		{
			this.m_moduleData = new Dictionary<ProjectileModule, ModuleShootData>();
		}
		if (this.Volley != null)
		{
			for (int i = 0; i < this.Volley.projectiles.Count; i++)
			{
				ProjectileModule projectileModule = this.Volley.projectiles[i];
				if (!this.m_moduleData.ContainsKey(projectileModule))
				{
					ModuleShootData moduleShootData = new ModuleShootData();
					if (this.ammo < projectileModule.GetModNumberOfShotsInClip(this.CurrentOwner))
					{
						moduleShootData.numberShotsFired = projectileModule.GetModNumberOfShotsInClip(this.CurrentOwner) - this.ammo;
					}
					ModuleShootData moduleShootData2;
					if (moduleData != null && originalSourceVolley != null && originalSourceVolley.projectiles != null && i < originalSourceVolley.projectiles.Count && moduleData.TryGetValue(originalSourceVolley.projectiles[i], out moduleShootData2) && moduleShootData2.beam)
					{
						moduleShootData.alternateAngleSign = moduleShootData2.alternateAngleSign;
						moduleShootData.beam = moduleShootData2.beam;
						moduleShootData.beamKnockbackID = moduleShootData2.beamKnockbackID;
						moduleShootData.angleForShot = moduleShootData2.angleForShot;
						this.m_activeBeams.Remove(moduleShootData2);
						this.m_activeBeams.Add(moduleShootData);
					}
					this.m_moduleData.Add(projectileModule, moduleShootData);
				}
			}
		}
		else
		{
			ModuleShootData moduleShootData3 = new ModuleShootData();
			if (this.ammo < this.singleModule.GetModNumberOfShotsInClip(this.CurrentOwner))
			{
				moduleShootData3.numberShotsFired = this.singleModule.GetModNumberOfShotsInClip(this.CurrentOwner) - this.ammo;
			}
			this.m_moduleData.Add(this.singleModule, moduleShootData3);
		}
		if (this.modifiedFinalVolley)
		{
			for (int j = 0; j < this.modifiedFinalVolley.projectiles.Count; j++)
			{
				if (!this.m_moduleData.ContainsKey(this.modifiedFinalVolley.projectiles[j]))
				{
					ModuleShootData moduleShootData4 = new ModuleShootData();
					if (this.ammo < this.modifiedFinalVolley.projectiles[j].GetModNumberOfShotsInClip(this.CurrentOwner))
					{
						moduleShootData4.numberShotsFired = this.modifiedFinalVolley.projectiles[j].GetModNumberOfShotsInClip(this.CurrentOwner) - this.ammo;
					}
					this.m_moduleData.Add(this.modifiedFinalVolley.projectiles[j], moduleShootData4);
				}
			}
		}
		if (this.modifiedOptionalReloadVolley)
		{
			for (int k = 0; k < this.modifiedOptionalReloadVolley.projectiles.Count; k++)
			{
				if (!this.m_moduleData.ContainsKey(this.modifiedOptionalReloadVolley.projectiles[k]))
				{
					ModuleShootData moduleShootData5 = new ModuleShootData();
					if (this.ammo < this.modifiedOptionalReloadVolley.projectiles[k].GetModNumberOfShotsInClip(this.CurrentOwner))
					{
						moduleShootData5.numberShotsFired = this.modifiedOptionalReloadVolley.projectiles[k].GetModNumberOfShotsInClip(this.CurrentOwner) - this.ammo;
					}
					this.m_moduleData.Add(this.modifiedOptionalReloadVolley.projectiles[k], moduleShootData5);
				}
			}
		}
		if (originalSourceVolley != null)
		{
			for (int l = 0; l < originalSourceVolley.projectiles.Count; l++)
			{
				if (!string.IsNullOrEmpty(originalSourceVolley.projectiles[l].runtimeGuid) && moduleData.ContainsKey(originalSourceVolley.projectiles[l]))
				{
					for (int m = 0; m < this.Volley.projectiles.Count; m++)
					{
						if (originalSourceVolley.projectiles[l].runtimeGuid == this.Volley.projectiles[m].runtimeGuid)
						{
							this.m_activeBeams.Remove(this.m_moduleData[this.Volley.projectiles[m]]);
							this.m_activeBeams.Add(moduleData[originalSourceVolley.projectiles[l]]);
							this.m_moduleData[this.Volley.projectiles[m]] = moduleData[originalSourceVolley.projectiles[l]];
						}
					}
				}
			}
		}
	}

	// Token: 0x06006E58 RID: 28248 RVA: 0x002B64C8 File Offset: 0x002B46C8
	public void Awake()
	{
		this.m_sprite = base.GetComponent<tk2dSprite>();
		this.AwakeAudio();
		this.m_clipLaunchAttachPoint = base.transform.Find("Clip");
		this.m_casingLaunchAttachPoint = base.transform.Find("Casing");
		this.m_localAttachPoint = base.transform.Find("PrimaryHand");
		this.m_offhandAttachPoint = base.transform.Find("SecondaryHand");
		this.m_meshRenderer = base.transform.GetComponent<MeshRenderer>();
		if (!string.IsNullOrEmpty(this.gunSwitchGroup))
		{
			AkSoundEngine.SetSwitch("WPN_Guns", this.gunSwitchGroup, base.gameObject);
		}
		this.InitializeDefaultFrame();
		if (this.rawVolley != null)
		{
			for (int i = 0; i < this.rawVolley.projectiles.Count; i++)
			{
				this.rawVolley.projectiles[i].ResetRuntimeData();
			}
		}
		else
		{
			this.singleModule.ResetRuntimeData();
		}
		if (this.alternateVolley != null)
		{
			for (int j = 0; j < this.alternateVolley.projectiles.Count; j++)
			{
				this.alternateVolley.projectiles[j].ResetRuntimeData();
			}
		}
		if (this.barrelOffset)
		{
			this.m_originalBarrelOffsetPosition = this.barrelOffset.localPosition;
		}
		if (this.muzzleOffset)
		{
			this.m_originalMuzzleOffsetPosition = this.muzzleOffset.localPosition;
		}
		if (this.chargeOffset)
		{
			this.m_originalChargeOffsetPosition = this.chargeOffset.localPosition;
		}
		this.weaponPanelSpriteOverride = base.GetComponent<GunWeaponPanelSpriteOverride>();
	}

	// Token: 0x06006E59 RID: 28249 RVA: 0x002B668C File Offset: 0x002B488C
	private void AwakeAudio()
	{
		AkGameObj akGameObj = base.GetComponent<AkGameObj>();
		if (!akGameObj)
		{
			akGameObj = base.gameObject.AddComponent<AkGameObj>();
		}
		akGameObj.Register();
	}

	// Token: 0x06006E5A RID: 28250 RVA: 0x002B66C0 File Offset: 0x002B48C0
	public void OnEnable()
	{
		if (this.m_isThrown)
		{
			return;
		}
		if (!this.NoOwnerOverride && !this.m_isThrown && (this.m_owner == null || this.m_owner.CurrentGun != this))
		{
			if (!(this.m_owner is PlayerController) || (this.m_owner as PlayerController).inventory == null || !((this.m_owner as PlayerController).CurrentSecondaryGun == this))
			{
				base.gameObject.SetActive(false);
				return;
			}
		}
		if (!this.NoOwnerOverride)
		{
			this.HandleSpriteFlip(this.m_owner.SpriteFlipped);
		}
		if (!this.m_owner)
		{
			return;
		}
		base.gameObject.GetOrAddComponent<AkGameObj>();
		this.m_transform.localPosition = BraveUtility.QuantizeVector(this.m_transform.localPosition, 16f);
		this.m_transform.localRotation = Quaternion.identity;
		if (this.ClearsCooldownsLikeAWP)
		{
			this.ClearCooldowns();
		}
		this.m_isReloading = false;
		this.m_reloadWhenDoneFiring = false;
		if (!this.m_isThrown)
		{
			if (!string.IsNullOrEmpty(this.introAnimation) && this.m_anim.GetClipByName(this.introAnimation) != null)
			{
				this.Play(this.introAnimation);
			}
			else
			{
				this.PlayIdleAnimation();
			}
		}
		base.gameObject.SetLayerRecursively(LayerMask.NameToLayer("FG_Reflection"));
	}

	// Token: 0x06006E5B RID: 28251 RVA: 0x002B6850 File Offset: 0x002B4A50
	public void Update()
	{
		this.m_isCritting = false;
		if (this.HeroSwordCooldown > 0f)
		{
			this.HeroSwordCooldown -= BraveTime.DeltaTime;
		}
		if (this.m_owner == null)
		{
			base.HandlePickupCurseParticles();
			if (!this.m_isBeingEyedByRat && Time.frameCount % 50 == 0 && base.ShouldBeTakenByRat(base.sprite.WorldCenter))
			{
				GameManager.Instance.Dungeon.StartCoroutine(base.HandleRatTheft());
			}
		}
		else if (this.UsesRechargeLikeActiveItem && this.m_owner is PlayerController && (this.m_owner as PlayerController).CharacterUsesRandomGuns)
		{
			this.RemainingActiveCooldownAmount = Mathf.Max(0f, this.m_remainingActiveCooldownAmount - 25f * BraveTime.DeltaTime);
		}
		if (this.m_reloadWhenDoneFiring && (string.IsNullOrEmpty(this.shootAnimation) || !base.spriteAnimator.IsPlaying(this.shootAnimation)) && (string.IsNullOrEmpty(this.finalShootAnimation) || !base.spriteAnimator.IsPlaying(this.finalShootAnimation)) && (string.IsNullOrEmpty(this.criticalFireAnimation) || !base.spriteAnimator.IsPlaying(this.criticalFireAnimation)))
		{
			this.Reload();
			if (this.OnReloadPressed != null)
			{
				this.OnReloadPressed(this.CurrentOwner as PlayerController, this, false);
			}
		}
		if (this.m_continueBurstInUpdate)
		{
			this.ContinueAttack(true, null);
			if (!this.m_midBurstFire)
			{
				this.CeaseAttack(false, null);
			}
		}
		if (this.m_owner is PlayerController && this.m_sprite && this.m_sprite.FlipX)
		{
			tk2dSprite[] outlineSprites = SpriteOutlineManager.GetOutlineSprites(this.m_sprite);
			if (outlineSprites != null)
			{
				for (int i = 0; i < outlineSprites.Length; i++)
				{
					if (outlineSprites[i])
					{
						outlineSprites[i].scale = this.m_sprite.scale;
					}
				}
			}
		}
		if (this.m_owner != null && this.m_instanceMinimapIcon != null)
		{
			this.GetRidOfMinimapIcon();
		}
		if (this.IsReloading && this.blankDuringReload)
		{
			this.m_reloadElapsed += BraveTime.DeltaTime;
			bool flag = base.spriteAnimator == null || base.spriteAnimator.IsPlaying(this.reloadAnimation);
			if (flag)
			{
				Vector2 unitCenter = this.m_owner.specRigidbody.GetUnitCenter(ColliderType.HitBox);
				if (this.reflectDuringReload)
				{
					float num = 1f;
					if (this.OnReflectedBulletDamageModifier != null)
					{
						num = this.OnReflectedBulletDamageModifier(num);
					}
					float num2 = 1f;
					if (this.OnReflectedBulletScaleModifier != null)
					{
						num2 = this.OnReflectedBulletScaleModifier(num2);
					}
					PassiveReflectItem.ReflectBulletsInRange(unitCenter, this.blankReloadRadius, true, this.m_owner, 10f, num2, num, false);
				}
				else
				{
					SilencerInstance.DestroyBulletsInRange(unitCenter, this.blankReloadRadius, true, false, null, false, null, false, null);
				}
				float num3 = this.blankDamageToEnemies;
				if (this.blankDamageScalingOnEmptyClip > 1f)
				{
					float num4 = (float)this.ClipShotsRemaining / (float)this.ClipCapacity;
					float num5 = Mathf.Clamp01(1f - num4);
					num3 = Mathf.Lerp(num3, num3 * this.blankDamageScalingOnEmptyClip, num5);
				}
				if (num3 > 0f)
				{
					if (this.m_reloadElapsed > 0.125f && !this.m_hasDoneSingleReloadBlankEffect)
					{
						this.m_hasDoneSingleReloadBlankEffect = true;
						Vector2 vector = this.PrimaryHandAttachPoint.position.XY();
						float num6 = this.blankReloadRadius * 2f;
						float num7 = 45f;
						this.DealDamageToEnemiesInArc(vector, num7, num6, num3, this.blankKnockbackPower, null);
					}
				}
				else
				{
					Exploder.DoRadialKnockback(unitCenter, this.blankKnockbackPower, this.blankReloadRadius + 1.25f);
				}
			}
		}
		if (this.m_isPreppedForThrow)
		{
			bool flag2 = this.m_prepThrowTime < 1.2f;
			bool flag3 = this.m_prepThrowTime < 0f;
			this.m_prepThrowTime += BraveTime.DeltaTime;
			PlayerController playerController = this.CurrentOwner as PlayerController;
			if (this.m_prepThrowTime < 1.2f)
			{
				this.HandleSpriteFlip(this.m_sprite.FlipY);
				if (playerController)
				{
					playerController.DoSustainedVibration(Vibration.Strength.UltraLight);
				}
			}
			else
			{
				if (flag2)
				{
					this.DoChargeCompletePoof();
				}
				if (playerController)
				{
					playerController.DoSustainedVibration(Vibration.Strength.Light);
				}
			}
			if (flag3 && this.m_prepThrowTime >= 0f)
			{
				playerController.ProcessHandAttachment();
			}
		}
		if (this.m_isThrown && this.m_sprite.FlipY)
		{
			this.m_sprite.FlipY = false;
		}
	}

	// Token: 0x06006E5C RID: 28252 RVA: 0x002B6D58 File Offset: 0x002B4F58
	public void OnWillRenderObject()
	{
		if (Pixelator.IsRenderingReflectionMap)
		{
			Bounds bounds = base.sprite.GetBounds();
			float num = bounds.min.y * 2f;
			if (this.m_owner != null && this.m_owner.CurrentGun == this)
			{
				bool flipY = base.sprite.FlipY;
				int num2 = ((!flipY) ? 1 : (-1));
				if (flipY)
				{
					num += 2f * bounds.size.y;
				}
				float num3 = 0f;
				float num4 = 1f - Mathf.Abs(90f - (this.gunAngle + 540f) % 180f) / 90f;
				if (this.CurrentOwner != null)
				{
					num3 = (float)(-1 * num2) * (base.transform.position.y - this.CurrentOwner.transform.position.y);
				}
				num3 = Mathf.Lerp(num3, (float)num2 * bounds.size.y, num4);
				num3 += -0.1875f * (float)num2 * (1f - num4);
				num += num3;
			}
			base.sprite.renderer.material.SetFloat("_ReflectionYOffset", num);
		}
	}

	// Token: 0x06006E5D RID: 28253 RVA: 0x002B6EC4 File Offset: 0x002B50C4
	public void OnDisable()
	{
		if (this.m_activeBeams.Count > 0 && this.doesScreenShake && GameManager.Instance.MainCameraController != null)
		{
			GameManager.Instance.MainCameraController.StopContinuousScreenShake(this);
		}
		if (this.m_extantLockOnSprite)
		{
			SpawnManager.Despawn(this.m_extantLockOnSprite);
		}
		this.DespawnVFX();
		base.sprite.SetSprite(this.m_defaultSpriteID);
		this.m_reloadWhenDoneFiring = false;
	}

	// Token: 0x06006E5E RID: 28254 RVA: 0x002B6F4C File Offset: 0x002B514C
	protected override void OnDestroy()
	{
		if (Minimap.HasInstance)
		{
			this.GetRidOfMinimapIcon();
		}
		base.OnDestroy();
	}

	// Token: 0x06006E5F RID: 28255 RVA: 0x002B6F64 File Offset: 0x002B5164
	public void ToggleRenderers(bool value)
	{
		this.m_meshRenderer.enabled = value;
		SpriteOutlineManager.ToggleOutlineRenderers(this.m_sprite, value);
		if (this.m_extantLaserSight)
		{
			this.m_extantLaserSight.renderer.enabled = value;
		}
		if (this.m_currentlyPlayingChargeVFX != null)
		{
			this.m_currentlyPlayingChargeVFX.ToggleRenderers(value);
			if (this.DefaultModule != null && this.m_moduleData.ContainsKey(this.DefaultModule))
			{
				ModuleShootData moduleShootData = this.m_moduleData[this.DefaultModule];
				if (moduleShootData != null && moduleShootData.lastChargeProjectile != null)
				{
					this.TogglePreviousChargeEffectsIfNecessary(moduleShootData.lastChargeProjectile, value);
				}
			}
		}
	}

	// Token: 0x06006E60 RID: 28256 RVA: 0x002B7014 File Offset: 0x002B5214
	public tk2dBaseSprite GetSprite()
	{
		if (this.m_sprite == null)
		{
			this.m_sprite = base.GetComponent<tk2dSprite>();
		}
		return this.m_sprite;
	}

	// Token: 0x06006E61 RID: 28257 RVA: 0x002B703C File Offset: 0x002B523C
	public void DespawnVFX()
	{
		if (this.m_extantLaserSight != null)
		{
			UnityEngine.Object.Destroy(this.m_extantLaserSight.gameObject);
			this.m_extantLaserSight = null;
		}
		this.muzzleFlashEffects.DestroyAll();
		this.m_isContinuousMuzzleFlashOut = false;
		this.finalMuzzleFlashEffects.DestroyAll();
		if (this.Volley != null)
		{
			for (int i = 0; i < this.Volley.projectiles.Count; i++)
			{
				if (this.Volley.projectiles[i].chargeProjectiles != null)
				{
					for (int j = 0; j < this.Volley.projectiles[i].chargeProjectiles.Count; j++)
					{
						if (this.Volley.projectiles[i].chargeProjectiles[j].UsesOverrideMuzzleFlashVfxPool)
						{
							this.Volley.projectiles[i].chargeProjectiles[j].OverrideMuzzleFlashVfxPool.DestroyAll();
						}
					}
				}
			}
		}
		else if (this.singleModule.chargeProjectiles != null)
		{
			for (int k = 0; k < this.singleModule.chargeProjectiles.Count; k++)
			{
				if (this.singleModule.chargeProjectiles[k].UsesOverrideMuzzleFlashVfxPool)
				{
					this.singleModule.chargeProjectiles[k].OverrideMuzzleFlashVfxPool.DestroyAll();
				}
			}
		}
		this.reloadEffects.DestroyAll();
		this.emptyReloadEffects.DestroyAll();
		this.activeReloadSuccessEffects.DestroyAll();
		this.activeReloadFailedEffects.DestroyAll();
	}

	// Token: 0x06006E62 RID: 28258 RVA: 0x002B71EC File Offset: 0x002B53EC
	public void ApplyProcGunData(ProceduralGunData data)
	{
		int randomIntValue = data.ammoData.GetRandomIntValue();
		this.ammo = randomIntValue;
		this.maxAmmo = randomIntValue;
		this.damageModifier = data.damageData.GetRandomIntValue();
		this.gunCooldownModifier = data.cooldownData.GetRandomValue();
	}

	// Token: 0x06006E63 RID: 28259 RVA: 0x002B7238 File Offset: 0x002B5438
	public void HandleSpriteFlip(bool flipped)
	{
		if (this.m_isThrown)
		{
			flipped = false;
		}
		if (this.usesDirectionalIdleAnimations || this.preventRotation)
		{
			flipped = false;
		}
		if (flipped && !this.forceFlat)
		{
			if (!this.m_sprite.FlipY)
			{
				this.barrelOffset.localPosition = this.m_originalBarrelOffsetPosition.WithY(-this.m_originalBarrelOffsetPosition.y);
				if (this.muzzleOffset)
				{
					this.muzzleOffset.localPosition = this.m_originalMuzzleOffsetPosition.WithY(-this.m_originalMuzzleOffsetPosition.y);
				}
				if (this.chargeOffset)
				{
					this.chargeOffset.localPosition = this.m_originalChargeOffsetPosition.WithY(-this.m_originalChargeOffsetPosition.y);
				}
				if (this.reloadOffset)
				{
					this.reloadOffset.localPosition = this.reloadOffset.localPosition.WithY(-this.reloadOffset.localPosition.y);
				}
				for (int i = 0; i < this.m_childTransformsToFlip.Count; i++)
				{
					this.m_childTransformsToFlip[i].localPosition = this.m_childTransformsToFlip[i].localPosition.WithY(-this.m_childTransformsToFlip[i].localPosition.y);
				}
				this.m_sprite.FlipY = true;
			}
		}
		else if (this.m_sprite.FlipY)
		{
			this.barrelOffset.localPosition = this.m_originalBarrelOffsetPosition;
			if (this.muzzleOffset)
			{
				this.muzzleOffset.localPosition = this.m_originalMuzzleOffsetPosition;
			}
			if (this.chargeOffset)
			{
				this.chargeOffset.localPosition = this.m_originalChargeOffsetPosition;
			}
			if (this.reloadOffset)
			{
				this.reloadOffset.localPosition = this.reloadOffset.localPosition.WithY(-this.reloadOffset.localPosition.y);
			}
			for (int j = 0; j < this.m_childTransformsToFlip.Count; j++)
			{
				this.m_childTransformsToFlip[j].localPosition = this.m_childTransformsToFlip[j].localPosition.WithY(-this.m_childTransformsToFlip[j].localPosition.y);
			}
			this.m_sprite.FlipY = false;
		}
		if (this.m_isPreppedForThrow)
		{
			Vector3 vector = this.m_defaultLocalPosition.WithZ(0f);
			if (flipped)
			{
				vector = Vector3.Scale(vector, new Vector3(1f, -1f, 1f));
			}
			base.transform.localPosition = Vector3.Lerp(vector.WithZ(0f), this.ThrowPrepPosition, Mathf.Clamp01(this.m_prepThrowTime / 1.2f));
		}
		else
		{
			this.m_transform.localPosition = this.m_defaultLocalPosition.WithZ(0f);
			if (flipped)
			{
				this.m_transform.localPosition = Vector3.Scale(this.m_transform.localPosition, new Vector3(1f, -1f, 1f));
			}
		}
		this.m_transform.localPosition = BraveUtility.QuantizeVector(this.m_transform.localPosition, 16f);
	}

	// Token: 0x06006E64 RID: 28260 RVA: 0x002B75BC File Offset: 0x002B57BC
	private bool ShouldDoLaserSight()
	{
		return !this.m_isPreppedForThrow && !this.m_isReloading && !this.SuppressLaserSight && (this.ForceLaserSight || this.PickupObjectId == GlobalItemIds.ArtfulDodgerChallengeGun || (this.CurrentOwner is PlayerController && PassiveItem.ActiveFlagItems.ContainsKey(this.CurrentOwner as PlayerController) && PassiveItem.ActiveFlagItems[this.CurrentOwner as PlayerController].ContainsKey(typeof(LaserSightItem))));
	}

	// Token: 0x06006E65 RID: 28261 RVA: 0x002B7668 File Offset: 0x002B5868
	public float GetChargeFraction()
	{
		bool flag = false;
		float num = 1f;
		if (this.IsFiring)
		{
			if (this.Volley != null)
			{
				for (int i = 0; i < this.Volley.projectiles.Count; i++)
				{
					ProjectileModule projectileModule = this.Volley.projectiles[i];
					if (projectileModule.shootStyle == ProjectileModule.ShootStyle.Charged)
					{
						ModuleShootData moduleShootData = this.m_moduleData[projectileModule];
						if (projectileModule.LongestChargeTime > 0f)
						{
							num = Mathf.Min(num, Mathf.Clamp01(moduleShootData.chargeTime / projectileModule.LongestChargeTime));
							flag = true;
						}
					}
				}
			}
			else
			{
				ProjectileModule projectileModule2 = this.singleModule;
				if (projectileModule2.shootStyle == ProjectileModule.ShootStyle.Charged)
				{
					ModuleShootData moduleShootData2 = this.m_moduleData[projectileModule2];
					if (projectileModule2.LongestChargeTime > 0f)
					{
						num = Mathf.Min(num, Mathf.Clamp01(moduleShootData2.chargeTime / projectileModule2.LongestChargeTime));
						flag = true;
					}
				}
			}
		}
		if (!flag)
		{
			num = 0f;
		}
		return num;
	}

	// Token: 0x06006E66 RID: 28262 RVA: 0x002B7778 File Offset: 0x002B5978
	public float HandleAimRotation(Vector3 ownerAimPoint, bool limitAimSpeed = false, float aimTimeScale = 1f)
	{
		if (this.m_isThrown)
		{
			return this.prevGunAngleUnmodified;
		}
		Vector2 vector;
		if (this.usesDirectionalIdleAnimations)
		{
			vector = (this.m_transform.position + Quaternion.Euler(0f, 0f, -this.m_attachTransform.localRotation.z) * this.barrelOffset.localPosition).XY();
			vector = this.m_owner.specRigidbody.HitboxPixelCollider.UnitCenter;
		}
		else if (this.LockedHorizontalOnCharge)
		{
			vector = this.m_owner.specRigidbody.HitboxPixelCollider.UnitCenter;
		}
		else
		{
			vector = (this.m_transform.position + Quaternion.Euler(0f, 0f, this.gunAngle) * Quaternion.Euler(0f, 0f, -this.m_attachTransform.localRotation.z) * this.barrelOffset.localPosition).XY();
		}
		float num = Vector2.Distance(ownerAimPoint.XY(), vector);
		float num2 = Mathf.Clamp01((num - 2f) / 3f);
		vector = Vector2.Lerp(this.m_owner.specRigidbody.HitboxPixelCollider.UnitCenter, vector, num2);
		this.m_localAimPoint = ownerAimPoint.XY();
		Vector2 vector2 = this.m_localAimPoint - vector;
		float num3 = Mathf.Atan2(vector2.y, vector2.x) * 57.29578f;
		if (this.OverrideAngleSnap != null)
		{
			num3 = BraveMathCollege.QuantizeFloat(num3, this.OverrideAngleSnap.Value);
		}
		if ((limitAimSpeed && aimTimeScale != 1f) || this.m_activeBeams.Count > 0)
		{
			float num4 = float.MaxValue * BraveTime.DeltaTime * aimTimeScale;
			if (this.m_activeBeams.Count > 0 && this.Volley && this.Volley.UsesBeamRotationLimiter)
			{
				num4 = this.Volley.BeamRotationDegreesPerSecond * BraveTime.DeltaTime * aimTimeScale;
			}
			float num5 = BraveMathCollege.ClampAngle180(num3 - this.prevGunAngleUnmodified);
			num3 = BraveMathCollege.ClampAngle180(this.prevGunAngleUnmodified + Mathf.Clamp(num5, -num4, num4));
			this.m_localAimPoint = (base.transform.position + (Quaternion.Euler(0f, 0f, num3) * Vector3.right).normalized * vector2.magnitude).XY();
		}
		this.prevGunAngleUnmodified = num3;
		this.gunAngle = num3;
		this.m_attachTransform.localRotation = Quaternion.Euler(this.m_attachTransform.localRotation.x, this.m_attachTransform.localRotation.y, num3);
		this.m_unroundedBarrelPosition = this.barrelOffset.position;
		float num6 = (float)((!this.forceFlat) ? (Mathf.RoundToInt(num3 / 10f) * 10) : (Mathf.RoundToInt(num3 / 3f) * 3));
		if (this.IgnoresAngleQuantization)
		{
			num6 = num3;
		}
		bool flag = base.sprite.FlipY;
		float num7 = 75f;
		float num8 = 105f;
		if (num6 <= 155f && num6 >= 25f)
		{
			num7 = 75f;
			num8 = 105f;
		}
		if (!base.sprite.FlipY && Mathf.Abs(num6) > num8)
		{
			flag = true;
		}
		else if (base.sprite.FlipY && Mathf.Abs(num6) < num7)
		{
			flag = false;
		}
		if (this.LockedHorizontalOnCharge)
		{
			float chargeFraction = this.GetChargeFraction();
			this.LockedHorizontalCachedAngle = num3;
			num6 = Mathf.LerpAngle(num6, (float)((!flag) ? 0 : 180), chargeFraction);
		}
		if (this.LockedHorizontalOnReload && this.IsReloading)
		{
			num6 = (float)((!flag) ? 0 : 180);
		}
		if (this.m_isPreppedForThrow)
		{
			if (this.m_prepThrowTime < 1.2f)
			{
				num6 = (float)Mathf.FloorToInt(Mathf.LerpAngle(num6, -90f, Mathf.Clamp01(this.m_prepThrowTime / 1.2f)));
			}
			else
			{
				num6 = (float)Mathf.FloorToInt(Mathf.PingPong(this.m_prepThrowTime * 15f, 10f) + -95f);
			}
		}
		if (this.preventRotation)
		{
			num6 = 0f;
		}
		if (this.usesDirectionalIdleAnimations)
		{
			int num9 = BraveMathCollege.AngleToOctant(num6 + 90f);
			float num10 = (float)(num9 * -45);
			Debug.Log(num10);
			float num11 = (num6 + 360f) % 360f - num10;
			this.m_attachTransform.localRotation = Quaternion.Euler(this.m_attachTransform.localRotation.x, this.m_attachTransform.localRotation.y, num11);
		}
		else
		{
			this.m_attachTransform.localRotation = Quaternion.Euler(this.m_attachTransform.localRotation.x, this.m_attachTransform.localRotation.y, num6);
		}
		if (this.m_currentlyPlayingChargeVFX != null)
		{
			this.UpdateChargeEffectZDepth(vector2);
		}
		if (this.m_sprite != null)
		{
			this.m_sprite.ForceRotationRebuild();
		}
		if (this.ShouldDoLaserSight())
		{
			if (this.m_extantLaserSight == null)
			{
				string text = "Global VFX/VFX_LaserSight";
				if (!(this.m_owner is PlayerController))
				{
					text = ((!this.LaserSightIsGreen) ? "Global VFX/VFX_LaserSight_Enemy" : "Global VFX/VFX_LaserSight_Enemy_Green");
				}
				this.m_extantLaserSight = SpawnManager.SpawnVFX((GameObject)BraveResources.Load(text, ".prefab"), false).GetComponent<tk2dTiledSprite>();
				this.m_extantLaserSight.IsPerpendicular = false;
				this.m_extantLaserSight.HeightOffGround = this.CustomLaserSightHeight;
				this.m_extantLaserSight.renderer.enabled = this.m_meshRenderer.enabled;
				this.m_extantLaserSight.transform.parent = this.barrelOffset;
				if (this.m_owner is AIActor)
				{
					this.m_extantLaserSight.renderer.enabled = false;
				}
			}
			this.m_extantLaserSight.transform.localPosition = Vector3.zero;
			this.m_extantLaserSight.transform.rotation = Quaternion.Euler(0f, 0f, num3);
			if (this.m_extantLaserSight.renderer.enabled)
			{
				Func<SpeculativeRigidbody, bool> func = (SpeculativeRigidbody otherRigidbody) => otherRigidbody.minorBreakable && !otherRigidbody.minorBreakable.stopsBullets;
				bool flag2 = false;
				float num12 = float.MaxValue;
				if (this.DoubleWideLaserSight)
				{
					CollisionLayer collisionLayer = ((!(this.m_owner is PlayerController)) ? CollisionLayer.PlayerHitBox : CollisionLayer.EnemyHitBox);
					int num13 = CollisionMask.LayerToMask(CollisionLayer.HighObstacle, CollisionLayer.BulletBlocker, collisionLayer, CollisionLayer.BulletBreakable);
					Vector2 vector3 = BraveMathCollege.DegreesToVector(vector2.ToAngle() + 90f, 0.0625f);
					RaycastResult raycastResult;
					if (PhysicsEngine.Instance.Raycast(this.barrelOffset.position.XY() + vector3, vector2, this.CustomLaserSightDistance, out raycastResult, true, true, num13, null, false, func, null))
					{
						flag2 = true;
						num12 = Mathf.Min(num12, raycastResult.Distance);
					}
					RaycastResult.Pool.Free(ref raycastResult);
					if (PhysicsEngine.Instance.Raycast(this.barrelOffset.position.XY() - vector3, vector2, this.CustomLaserSightDistance, out raycastResult, true, true, num13, null, false, func, null))
					{
						flag2 = true;
						num12 = Mathf.Min(num12, raycastResult.Distance);
					}
					RaycastResult.Pool.Free(ref raycastResult);
				}
				else
				{
					CollisionLayer collisionLayer2 = ((!(this.m_owner is PlayerController)) ? CollisionLayer.PlayerHitBox : CollisionLayer.EnemyHitBox);
					int num14 = CollisionMask.LayerToMask(CollisionLayer.HighObstacle, CollisionLayer.BulletBlocker, collisionLayer2, CollisionLayer.BulletBreakable);
					RaycastResult raycastResult2;
					if (PhysicsEngine.Instance.Raycast(this.barrelOffset.position.XY(), vector2, this.CustomLaserSightDistance, out raycastResult2, true, true, num14, null, false, func, null))
					{
						flag2 = true;
						num12 = raycastResult2.Distance;
						if (raycastResult2.SpeculativeRigidbody && raycastResult2.SpeculativeRigidbody.aiActor)
						{
							this.HandleEnemyHitByLaserSight(raycastResult2.SpeculativeRigidbody.aiActor);
						}
					}
					RaycastResult.Pool.Free(ref raycastResult2);
				}
				this.m_extantLaserSight.dimensions = new Vector2((!flag2) ? 480f : (num12 / 0.0625f), 1f);
				this.m_extantLaserSight.ForceRotationRebuild();
				this.m_extantLaserSight.UpdateZDepth();
			}
		}
		else if (this.m_extantLaserSight != null)
		{
			SpawnManager.Despawn(this.m_extantLaserSight.gameObject);
			this.m_extantLaserSight = null;
		}
		if (!this.OwnerHasSynergy(CustomSynergyType.PLASMA_LASER) && this.m_extantLockOnSprite)
		{
			SpawnManager.Despawn(this.m_extantLockOnSprite);
		}
		if (this.usesDirectionalAnimator)
		{
			base.aiAnimator.LockFacingDirection = true;
			base.aiAnimator.FacingDirection = num3;
		}
		return num3;
	}

	// Token: 0x06006E67 RID: 28263 RVA: 0x002B80DC File Offset: 0x002B62DC
	protected void HandleEnemyHitByLaserSight(AIActor hitEnemy)
	{
		if (hitEnemy && this.LastLaserSightEnemy != hitEnemy && this.OwnerHasSynergy(CustomSynergyType.PLASMA_LASER))
		{
			if (this.m_extantLockOnSprite)
			{
				SpawnManager.Despawn(this.m_extantLockOnSprite);
			}
			this.m_extantLockOnSprite = hitEnemy.PlayEffectOnActor((GameObject)BraveResources.Load("Global VFX/VFX_LockOn", ".prefab"), Vector3.zero, true, true, true);
			this.LastLaserSightEnemy = hitEnemy;
		}
	}

	// Token: 0x06006E68 RID: 28264 RVA: 0x002B8160 File Offset: 0x002B6360
	protected void UpdateChargeEffectZDepth(Vector2 currentAimDirection)
	{
		float num = (currentAimDirection.normalized.y + 1f) / 2f;
		float num2 = Mathf.Lerp(1.6f, 0.9f, num);
		this.m_currentlyPlayingChargeVFX.SetHeightOffGround(num2);
	}

	// Token: 0x06006E69 RID: 28265 RVA: 0x002B81A8 File Offset: 0x002B63A8
	protected void UpdatePerpendicularity(Vector2 gunToAim)
	{
		if (this.forceFlat)
		{
			return;
		}
		int num = BraveMathCollege.VectorToQuadrant(gunToAim);
		if (num == 2)
		{
			this.m_sprite.IsPerpendicular = false;
		}
		else
		{
			this.m_sprite.IsPerpendicular = true;
		}
	}

	// Token: 0x06006E6A RID: 28266 RVA: 0x002B81EC File Offset: 0x002B63EC
	protected float DealSwordDamageToEnemy(AIActor targetEnemy, Vector2 arcOrigin, Vector2 contact, float angle, float overrideDamage = -1f, float overrideForce = -1f)
	{
		Projectile currentProjectile = this.DefaultModule.GetCurrentProjectile();
		float num = ((overrideDamage <= 0f) ? currentProjectile.baseData.damage : overrideDamage);
		float num2 = ((overrideForce <= 0f) ? currentProjectile.baseData.force : overrideForce);
		if (targetEnemy.healthHaver)
		{
			targetEnemy.healthHaver.ApplyDamage(num, contact - arcOrigin, this.m_owner.ActorName, CoreDamageTypes.None, DamageCategory.Normal, false, null, false);
		}
		if (targetEnemy.knockbackDoer)
		{
			targetEnemy.knockbackDoer.ApplyKnockback(contact - arcOrigin, num2, false);
		}
		currentProjectile.hitEffects.HandleEnemyImpact(contact, angle, targetEnemy.transform, Vector2.zero, Vector2.zero, true, false);
		return num;
	}

	// Token: 0x06006E6B RID: 28267 RVA: 0x002B82C4 File Offset: 0x002B64C4
	protected void DealDamageToEnemiesInArc(Vector2 arcOrigin, float arcAngle, float arcRadius, float overrideDamage = -1f, float overrideForce = -1f, List<SpeculativeRigidbody> alreadyHit = null)
	{
		RoomHandler roomHandler = null;
		if (this.m_owner is PlayerController)
		{
			roomHandler = ((PlayerController)this.m_owner).CurrentRoom;
		}
		else if (this.m_owner is AIActor)
		{
			roomHandler = ((AIActor)this.m_owner).ParentRoom;
		}
		if (roomHandler == null)
		{
			return;
		}
		List<AIActor> activeEnemies = roomHandler.GetActiveEnemies(RoomHandler.ActiveEnemyType.All);
		if (activeEnemies == null)
		{
			return;
		}
		for (int i = 0; i < activeEnemies.Count; i++)
		{
			AIActor aiactor = activeEnemies[i];
			if (aiactor && aiactor.specRigidbody && aiactor.IsNormalEnemy && !aiactor.IsGone && aiactor.healthHaver)
			{
				if (alreadyHit == null || !alreadyHit.Contains(aiactor.specRigidbody))
				{
					for (int j = 0; j < aiactor.healthHaver.NumBodyRigidbodies; j++)
					{
						SpeculativeRigidbody bodyRigidbody = aiactor.healthHaver.GetBodyRigidbody(j);
						PixelCollider hitboxPixelCollider = bodyRigidbody.HitboxPixelCollider;
						if (hitboxPixelCollider != null)
						{
							Vector2 vector = BraveMathCollege.ClosestPointOnRectangle(arcOrigin, hitboxPixelCollider.UnitBottomLeft, hitboxPixelCollider.UnitDimensions);
							float num = Vector2.Distance(vector, arcOrigin);
							float num2 = BraveMathCollege.Atan2Degrees(vector - arcOrigin);
							if (num < arcRadius && Mathf.DeltaAngle(this.CurrentAngle, num2) < arcAngle)
							{
								bool flag = true;
								int num3 = CollisionMask.LayerToMask(CollisionLayer.HighObstacle, CollisionLayer.BulletBlocker, CollisionLayer.BulletBreakable);
								RaycastResult raycastResult;
								if (PhysicsEngine.Instance.Raycast(arcOrigin, vector - arcOrigin, num, out raycastResult, true, true, num3, null, false, null, null) && raycastResult.SpeculativeRigidbody != bodyRigidbody)
								{
									flag = false;
								}
								RaycastResult.Pool.Free(ref raycastResult);
								if (flag)
								{
									float num4 = this.DealSwordDamageToEnemy(aiactor, arcOrigin, vector, arcAngle, overrideDamage, overrideForce);
									if (alreadyHit != null)
									{
										if (alreadyHit.Count == 0)
										{
											StickyFrictionManager.Instance.RegisterSwordDamageStickyFriction(num4);
										}
										alreadyHit.Add(aiactor.specRigidbody);
									}
									break;
								}
							}
						}
					}
				}
			}
		}
	}

	// Token: 0x06006E6C RID: 28268 RVA: 0x002B84E8 File Offset: 0x002B66E8
	protected void HandleHeroSwordSlash(List<SpeculativeRigidbody> alreadyHit, Vector2 arcOrigin, int slashId)
	{
		float num = (this.m_casingLaunchAttachPoint.position.XY() - this.PrimaryHandAttachPoint.position.XY()).magnitude * 1.85f;
		float num2 = 45f;
		float num3 = num * num;
		if (this.HeroSwordDoesntBlank)
		{
			ReadOnlyCollection<Projectile> allProjectiles = StaticReferenceManager.AllProjectiles;
			for (int i = allProjectiles.Count - 1; i >= 0; i--)
			{
				Projectile projectile = allProjectiles[i];
				if (projectile && !(projectile.Owner is PlayerController) && projectile.IsReflectedBySword && projectile.LastReflectedSlashId != slashId)
				{
					if (!(projectile.Owner is AIActor) || (projectile.Owner as AIActor).IsNormalEnemy)
					{
						Vector2 worldCenter = projectile.sprite.WorldCenter;
						float num4 = Vector2.SqrMagnitude(worldCenter - arcOrigin);
						if (num4 < num3)
						{
							float num5 = BraveMathCollege.Atan2Degrees(worldCenter - arcOrigin);
							if (Mathf.DeltaAngle(this.CurrentAngle, num5) < num2)
							{
								PassiveReflectItem.ReflectBullet(projectile, true, this.m_owner, 2f, 1f, 1f, 0f);
								projectile.LastReflectedSlashId = slashId;
							}
						}
					}
				}
			}
		}
		else
		{
			ReadOnlyCollection<Projectile> allProjectiles2 = StaticReferenceManager.AllProjectiles;
			for (int j = allProjectiles2.Count - 1; j >= 0; j--)
			{
				Projectile projectile2 = allProjectiles2[j];
				if (projectile2 && (!(projectile2.Owner is PlayerController) || projectile2.ForcePlayerBlankable))
				{
					if (!(projectile2.Owner is AIActor) || (projectile2.Owner as AIActor).IsNormalEnemy)
					{
						Vector2 worldCenter2 = projectile2.sprite.WorldCenter;
						float num6 = Vector2.SqrMagnitude(worldCenter2 - arcOrigin);
						if (num6 < num3)
						{
							float num7 = BraveMathCollege.Atan2Degrees(worldCenter2 - arcOrigin);
							if (Mathf.DeltaAngle(this.CurrentAngle, num7) < num2)
							{
								projectile2.DieInAir(false, true, true, true);
							}
						}
					}
				}
			}
		}
		this.DealDamageToEnemiesInArc(arcOrigin, num2, num, -1f, -1f, alreadyHit);
		Projectile currentProjectile = this.DefaultModule.GetCurrentProjectile();
		float damage = currentProjectile.baseData.damage;
		float num8 = num * num;
		List<MinorBreakable> allMinorBreakables = StaticReferenceManager.AllMinorBreakables;
		for (int k = allMinorBreakables.Count - 1; k >= 0; k--)
		{
			MinorBreakable minorBreakable = allMinorBreakables[k];
			if (minorBreakable && minorBreakable.specRigidbody)
			{
				if (!minorBreakable.IsBroken && minorBreakable.sprite && (minorBreakable.sprite.WorldCenter - arcOrigin).sqrMagnitude < num8)
				{
					minorBreakable.Break();
				}
			}
		}
		List<MajorBreakable> allMajorBreakables = StaticReferenceManager.AllMajorBreakables;
		for (int l = allMajorBreakables.Count - 1; l >= 0; l--)
		{
			MajorBreakable majorBreakable = allMajorBreakables[l];
			if (majorBreakable && majorBreakable.specRigidbody)
			{
				if (!alreadyHit.Contains(majorBreakable.specRigidbody))
				{
					if (!majorBreakable.IsSecretDoor)
					{
						if (!majorBreakable.IsDestroyed && (majorBreakable.specRigidbody.UnitCenter - arcOrigin).sqrMagnitude <= num8)
						{
							float num9 = damage;
							if (majorBreakable.healthHaver)
							{
								num9 *= 0.2f;
							}
							majorBreakable.ApplyDamage(num9, majorBreakable.specRigidbody.UnitCenter - arcOrigin, false, false, false);
							alreadyHit.Add(majorBreakable.specRigidbody);
						}
					}
				}
			}
		}
	}

	// Token: 0x06006E6D RID: 28269 RVA: 0x002B88E0 File Offset: 0x002B6AE0
	private IEnumerator HandleSlash()
	{
		int slashId = Time.frameCount;
		List<SpeculativeRigidbody> alreadyHit = new List<SpeculativeRigidbody>();
		this.m_owner.knockbackDoer.ApplyKnockback(BraveMathCollege.DegreesToVector(this.CurrentAngle, 1f), 40f, 0.25f, false);
		this.DoScreenShake();
		this.HandleShootEffects(this.DefaultModule);
		if (!this.HeroSwordDoesntBlank && (this.m_owner.healthHaver.GetCurrentHealthPercentage() >= 1f || (this.m_owner as PlayerController).HasActiveBonusSynergy(CustomSynergyType.HERO_OF_CHICKEN, false)))
		{
			if (this.Volley)
			{
				for (int i = 0; i < this.Volley.projectiles.Count; i++)
				{
					this.ShootSingleProjectile(this.Volley.projectiles[i], null, null);
				}
			}
			else
			{
				this.ShootSingleProjectile(this.DefaultModule, null, null);
			}
		}
		Vector2 cachedSlashOffset = this.PrimaryHandAttachPoint.position.XY() - this.m_owner.CenterPosition;
		float ela = 0f;
		while (ela < 0.2f)
		{
			ela += BraveTime.DeltaTime;
			this.HandleHeroSwordSlash(alreadyHit, this.m_owner.CenterPosition + cachedSlashOffset, slashId);
			yield return null;
			if (!this)
			{
				yield break;
			}
		}
		yield break;
	}

	// Token: 0x06006E6E RID: 28270 RVA: 0x002B88FC File Offset: 0x002B6AFC
	public bool IsGunBlocked()
	{
		if (this.RequiresFundsToShoot && this.m_owner is PlayerController && (this.m_owner as PlayerController).carriedConsumables.Currency < this.CurrencyCostPerShot)
		{
			return true;
		}
		bool flag = false;
		Vector2 unitCenter = this.m_owner.specRigidbody.GetUnitCenter(ColliderType.HitBox);
		Vector2 vector = this.barrelOffset.transform.position.XY();
		Vector2 vector2 = vector - unitCenter;
		float magnitude = vector2.magnitude;
		int num = CollisionMask.LayerToMask(CollisionLayer.HighObstacle, CollisionLayer.BulletBlocker, CollisionLayer.BulletBreakable);
		if (this.m_owner && !(this.m_owner is PlayerController))
		{
			num |= CollisionMask.LayerToMask(CollisionLayer.EnemyBulletBlocker);
		}
		SpeculativeRigidbody speculativeRigidbody;
		if (PhysicsEngine.Instance.Pointcast(vector, out speculativeRigidbody, false, true, CollisionMask.LayerToMask(CollisionLayer.BeamBlocker), null, false, new SpeculativeRigidbody[0]))
		{
			UltraFortunesFavor ultraFortunesFavor = speculativeRigidbody.ultraFortunesFavor;
			if (ultraFortunesFavor)
			{
				speculativeRigidbody.ultraFortunesFavor.HitFromPoint(vector);
				return true;
			}
		}
		if (this.CanAttackThroughObjects)
		{
			return false;
		}
		PhysicsEngine.Instance.Pointcast(unitCenter, out speculativeRigidbody, false, true, num, null, false, new SpeculativeRigidbody[] { this.m_owner.specRigidbody });
		bool flag2 = false;
		if (this.Volley == null && this.singleModule.shootStyle == ProjectileModule.ShootStyle.Charged)
		{
			flag2 = true;
		}
		if (this.Volley != null && this.Volley.projectiles.Count == 1 && this.Volley.projectiles[0].shootStyle == ProjectileModule.ShootStyle.Charged)
		{
			flag2 = true;
		}
		int num2 = 100;
		RaycastResult raycastResult;
		while (PhysicsEngine.Instance.Raycast(unitCenter, vector2, magnitude, out raycastResult, true, true, num, null, false, null, speculativeRigidbody))
		{
			num2--;
			if (num2 <= 0)
			{
				flag = true;
				break;
			}
			SpeculativeRigidbody speculativeRigidbody2 = raycastResult.SpeculativeRigidbody;
			RaycastResult.Pool.Free(ref raycastResult);
			if (speculativeRigidbody2 != null)
			{
				MinorBreakable component = speculativeRigidbody2.GetComponent<MinorBreakable>();
				if (component != null && (!flag2 || this.m_currentlyPlayingChargeVFX != null) && !component.OnlyBrokenByCode)
				{
					component.Break(vector2.normalized * 3f);
					continue;
				}
			}
			if (!GameManager.Instance.InTutorial || !(speculativeRigidbody2 != null) || !speculativeRigidbody2.GetComponent<Chest>() || !speculativeRigidbody2.majorBreakable)
			{
				flag = true;
				break;
			}
			speculativeRigidbody2.majorBreakable.Break(vector2);
		}
		return flag;
	}

	// Token: 0x06006E6F RID: 28271 RVA: 0x002B8BC4 File Offset: 0x002B6DC4
	public void ForceThrowGun()
	{
		this.ThrowGun();
	}

	// Token: 0x06006E70 RID: 28272 RVA: 0x002B8BCC File Offset: 0x002B6DCC
	public DebrisObject DropGun(float dropHeight = 0.5f)
	{
		this.m_isThrown = true;
		this.m_thrownOnGround = true;
		if (this.m_sprite == null)
		{
			this.m_sprite = base.sprite;
		}
		base.gameObject.SetActive(true);
		this.m_owner = null;
		Vector3 vector = base.transform.position;
		if (this.PrimaryHandAttachPoint)
		{
			vector = this.PrimaryHandAttachPoint.position;
		}
		GameObject gameObject = SpawnManager.SpawnProjectile("ThrownGunProjectile", vector, Quaternion.identity);
		Projectile component = gameObject.GetComponent<Projectile>();
		this.LastProjectile = component;
		component.Shooter = ((!(this.m_owner != null)) ? null : this.m_owner.specRigidbody);
		component.DestroyMode = Projectile.ProjectileDestroyMode.BecomeDebris;
		component.shouldRotate = false;
		if (component)
		{
			TrailRenderer componentInChildren = component.GetComponentInChildren<TrailRenderer>();
			if (componentInChildren)
			{
				UnityEngine.Object.Destroy(componentInChildren);
			}
		}
		SpeculativeRigidbody component2 = gameObject.GetComponent<SpeculativeRigidbody>();
		component2.sprite = base.sprite;
		base.transform.parent = gameObject.transform;
		if (this.m_sprite.FlipY)
		{
			this.HandleSpriteFlip(false);
		}
		base.transform.localPosition = Vector3.zero;
		base.transform.localRotation = Quaternion.identity;
		if (this.PrimaryHandAttachPoint)
		{
			base.transform.localPosition -= this.PrimaryHandAttachPoint.localPosition;
		}
		if (this.m_defaultSpriteID >= 0)
		{
			base.spriteAnimator.StopAndResetFrame();
			this.m_sprite.SetSprite(this.m_defaultSpriteID);
		}
		if (!RoomHandler.unassignedInteractableObjects.Contains(this))
		{
			RoomHandler.unassignedInteractableObjects.Add(this);
		}
		DebrisObject debrisObject = component.BecomeDebris(Vector3.zero, dropHeight);
		debrisObject.Priority = EphemeralObject.EphemeralPriority.Critical;
		debrisObject.FlagAsPickup();
		debrisObject.inertialMass = 10f;
		debrisObject.canRotate = false;
		UnityEngine.Object.Destroy(component.GetComponentInChildren<SimpleSpriteRotator>());
		UnityEngine.Object.Destroy(component);
		component.ForceDestruction();
		SpriteOutlineManager.AddOutlineToSprite(this.m_sprite, Color.black, 0.1f, 0.05f, SpriteOutlineManager.OutlineType.NORMAL);
		this.m_sprite.ForceRotationRebuild();
		if (this.m_anim != null)
		{
			this.PlayIdleAnimation();
		}
		if (this.OnDropped != null)
		{
			this.OnDropped();
		}
		this.RegisterMinimapIcon();
		return debrisObject;
	}

	// Token: 0x06006E71 RID: 28273 RVA: 0x002B8E2C File Offset: 0x002B702C
	public void PrepGunForThrow()
	{
		if (!this.m_isPreppedForThrow && this.CurrentOwner is PlayerController)
		{
			this.m_isPreppedForThrow = true;
			this.m_prepThrowTime = -0.3f;
			this.HandleSpriteFlip(this.m_sprite.FlipY);
			(this.CurrentOwner as PlayerController).ProcessHandAttachment();
		}
		AkSoundEngine.PostEvent("Play_BOSS_doormimic_turn_01", base.gameObject);
	}

	// Token: 0x06006E72 RID: 28274 RVA: 0x002B8E98 File Offset: 0x002B7098
	public void UnprepGunForThrow()
	{
		if (this.m_isPreppedForThrow)
		{
			this.m_isPreppedForThrow = false;
			this.m_prepThrowTime = -0.3f;
			this.HandleSpriteFlip(this.m_sprite.FlipY);
			(this.CurrentOwner as PlayerController).ProcessHandAttachment();
		}
		AkSoundEngine.PostEvent("Stop_BOSS_doormimic_turn_01", base.gameObject);
	}

	// Token: 0x06006E73 RID: 28275 RVA: 0x002B8EF4 File Offset: 0x002B70F4
	private void ThrowGun()
	{
		this.m_isThrown = true;
		this.m_thrownOnGround = false;
		base.gameObject.SetActive(true);
		AkSoundEngine.PostEvent("Play_OBJ_item_throw_01", base.gameObject);
		Vector3 vector = this.ThrowPrepTransform.parent.TransformPoint((this.ThrowPrepPosition * -1f).WithX(0f));
		Vector2 vector2 = this.m_localAimPoint - vector.XY();
		float num = BraveMathCollege.Atan2Degrees(vector2);
		GameObject gameObject = SpawnManager.SpawnProjectile("ThrownGunProjectile", vector, Quaternion.Euler(0f, 0f, num));
		Projectile component = gameObject.GetComponent<Projectile>();
		component.Shooter = this.m_owner.specRigidbody;
		component.DestroyMode = Projectile.ProjectileDestroyMode.BecomeDebris;
		component.baseData.damage *= (this.m_owner as PlayerController).stats.GetStatValue(PlayerStats.StatType.ThrownGunDamage);
		SpeculativeRigidbody component2 = gameObject.GetComponent<SpeculativeRigidbody>();
		SpeculativeRigidbody speculativeRigidbody = component2;
		speculativeRigidbody.OnTriggerCollision = (SpeculativeRigidbody.OnTriggerDelegate)Delegate.Combine(speculativeRigidbody.OnTriggerCollision, new SpeculativeRigidbody.OnTriggerDelegate(this.OnTrigger));
		component2.sprite = base.sprite;
		this.m_sprite.scale = Vector3.one;
		base.transform.parent = gameObject.transform;
		base.transform.localRotation = Quaternion.identity;
		this.m_sprite.PlaceAtLocalPositionByAnchor(Vector3.zero, tk2dBaseSprite.Anchor.MiddleCenter);
		if (this.m_sprite.FlipY)
		{
			base.transform.localPosition = Vector3.Scale(new Vector3(-1f, 1f, 1f), base.transform.localPosition);
		}
		Bounds bounds = base.sprite.GetBounds();
		component2.PrimaryPixelCollider.ColliderGenerationMode = PixelCollider.PixelColliderGeneration.Manual;
		component2.PrimaryPixelCollider.ManualOffsetX = -Mathf.RoundToInt(bounds.extents.x / 0.0625f);
		component2.PrimaryPixelCollider.ManualOffsetY = -Mathf.RoundToInt(bounds.extents.y / 0.0625f);
		component2.PrimaryPixelCollider.ManualWidth = Mathf.RoundToInt(bounds.size.x / 0.0625f);
		component2.PrimaryPixelCollider.ManualHeight = Mathf.RoundToInt(bounds.size.y / 0.0625f);
		component2.UpdateCollidersOnRotation = true;
		component2.UpdateCollidersOnScale = true;
		component.Reawaken();
		component.Owner = this.CurrentOwner;
		component.Start();
		component.SendInDirection(vector2, true, false);
		Projectile projectile = component;
		projectile.OnBecameDebris = (Action<DebrisObject>)Delegate.Combine(projectile.OnBecameDebris, new Action<DebrisObject>(delegate(DebrisObject a)
		{
			if (this.barrelOffset)
			{
				this.barrelOffset.localPosition = this.m_originalBarrelOffsetPosition;
			}
			if (this.muzzleOffset)
			{
				this.muzzleOffset.localPosition = this.m_originalMuzzleOffsetPosition;
			}
			if (this.chargeOffset)
			{
				this.chargeOffset.localPosition = this.m_originalChargeOffsetPosition;
			}
			if (a)
			{
				a.FlagAsPickup();
				a.Priority = EphemeralObject.EphemeralPriority.Critical;
				TrailRenderer componentInChildren = a.gameObject.GetComponentInChildren<TrailRenderer>();
				if (componentInChildren)
				{
					UnityEngine.Object.Destroy(componentInChildren);
				}
				SpeculativeRigidbody component3 = a.GetComponent<SpeculativeRigidbody>();
				if (component3)
				{
					component3.AddCollisionLayerIgnoreOverride(CollisionMask.LayerToMask(CollisionLayer.Projectile, CollisionLayer.EnemyHitBox));
				}
			}
		}));
		Projectile projectile2 = component;
		projectile2.OnBecameDebrisGrounded = (Action<DebrisObject>)Delegate.Combine(projectile2.OnBecameDebrisGrounded, new Action<DebrisObject>(this.HandleThrownGunGrounded));
		component.angularVelocity = (float)((vector2.x <= 0f) ? 1080 : (-1080));
		if (!RoomHandler.unassignedInteractableObjects.Contains(this))
		{
			RoomHandler.unassignedInteractableObjects.Add(this);
		}
		component2.ForceRegenerate(null, null);
		if (this.m_owner)
		{
			(this.m_owner as PlayerController).DoPostProcessThrownGun(component);
		}
		this.m_owner = null;
	}

	// Token: 0x06006E74 RID: 28276 RVA: 0x002B924C File Offset: 0x002B744C
	private void HandleThrownGunGrounded(DebrisObject obj)
	{
		obj.OnGrounded = (Action<DebrisObject>)Delegate.Remove(obj.OnGrounded, new Action<DebrisObject>(this.HandleThrownGunGrounded));
		obj.inertialMass = 10f;
		if (this.barrelOffset)
		{
			this.barrelOffset.localPosition = this.m_originalBarrelOffsetPosition;
		}
		if (this.muzzleOffset)
		{
			this.muzzleOffset.localPosition = this.m_originalMuzzleOffsetPosition;
		}
		if (this.chargeOffset)
		{
			this.chargeOffset.localPosition = this.m_originalChargeOffsetPosition;
		}
		SpriteOutlineManager.RemoveOutlineFromSprite(this.m_sprite, true);
		SpriteOutlineManager.AddOutlineToSprite(this.m_sprite, Color.black, 0.1f, 0.05f, SpriteOutlineManager.OutlineType.NORMAL);
		this.m_sprite.UpdateZDepth();
		this.m_thrownOnGround = true;
	}

	// Token: 0x06006E75 RID: 28277 RVA: 0x002B9324 File Offset: 0x002B7524
	public void RegisterNewCustomAmmunition(ActiveAmmunitionData ammodata)
	{
		if (ammodata == null || ammodata.ShotsRemaining <= 0)
		{
			return;
		}
		if (!this.m_customAmmunitions.Contains(ammodata))
		{
			this.m_customAmmunitions.Add(ammodata);
		}
	}

	// Token: 0x06006E76 RID: 28278 RVA: 0x002B9358 File Offset: 0x002B7558
	public void RegisterMinimapIcon()
	{
		if (base.transform.position.y < -300f)
		{
			return;
		}
		GameObject gameObject = (GameObject)BraveResources.Load("Global Prefabs/Minimap_Gun_Icon", ".prefab");
		if (gameObject != null && this.m_owner == null)
		{
			this.m_minimapIconRoom = GameManager.Instance.Dungeon.data.GetAbsoluteRoomFromPosition(base.transform.position.IntXY(VectorConversions.Floor));
			this.m_instanceMinimapIcon = Minimap.Instance.RegisterRoomIcon(this.m_minimapIconRoom, gameObject, false);
		}
	}

	// Token: 0x06006E77 RID: 28279 RVA: 0x002B93F8 File Offset: 0x002B75F8
	public void GetRidOfMinimapIcon()
	{
		if (this.m_instanceMinimapIcon != null)
		{
			Minimap.Instance.DeregisterRoomIcon(this.m_minimapIconRoom, this.m_instanceMinimapIcon);
			this.m_instanceMinimapIcon = null;
		}
	}

	// Token: 0x06006E78 RID: 28280 RVA: 0x002B9428 File Offset: 0x002B7628
	public void GainAmmo(int amt)
	{
		if (!this.CanGainAmmo || this.InfiniteAmmo)
		{
			return;
		}
		if (amt > 0)
		{
			this.UnprepGunForThrow();
		}
		this.ammo += amt;
		if (this.AdjustedMaxAmmo > 0)
		{
			this.ammo = Math.Min(this.AdjustedMaxAmmo, this.ammo);
		}
		this.ammo = Mathf.Clamp(this.ammo, 0, 100000000);
		if (this.OnAmmoChanged != null)
		{
			this.OnAmmoChanged(this.m_owner as PlayerController, this);
		}
	}

	// Token: 0x06006E79 RID: 28281 RVA: 0x002B94C4 File Offset: 0x002B76C4
	public void LoseAmmo(int amt)
	{
		this.ammo -= amt;
		if (this.ammo < 0)
		{
			this.ammo = 0;
		}
		if (this.ClipShotsRemaining > this.ammo)
		{
			this.ClipShotsRemaining = this.ammo;
		}
		if (this.OnAmmoChanged != null)
		{
			this.OnAmmoChanged(this.m_owner as PlayerController, this);
		}
	}

	// Token: 0x06006E7A RID: 28282 RVA: 0x002B9534 File Offset: 0x002B7734
	public void GainAmmo(Gun g)
	{
		if (!this.CanGainAmmo)
		{
			return;
		}
		this.ammo += g.ammo;
		if (this.AdjustedMaxAmmo > 0)
		{
			this.ammo = Math.Min(this.AdjustedMaxAmmo, this.ammo);
		}
		if (this.OnAmmoChanged != null)
		{
			this.OnAmmoChanged(this.m_owner as PlayerController, this);
		}
	}

	// Token: 0x06006E7B RID: 28283 RVA: 0x002B95A8 File Offset: 0x002B77A8
	public float GetPrimaryCooldown()
	{
		if (this.Volley != null)
		{
			return this.Volley.projectiles[0].cooldownTime;
		}
		return this.singleModule.cooldownTime;
	}

	// Token: 0x06006E7C RID: 28284 RVA: 0x002B95E0 File Offset: 0x002B77E0
	public void ClearOptionalReloadVolleyCooldownAndReloadData()
	{
		if (this.OptionalReloadVolley != null)
		{
			for (int i = 0; i < this.OptionalReloadVolley.projectiles.Count; i++)
			{
				if (this.m_moduleData.ContainsKey(this.OptionalReloadVolley.projectiles[i]))
				{
					this.m_moduleData[this.OptionalReloadVolley.projectiles[i]].onCooldown = false;
					this.m_moduleData[this.OptionalReloadVolley.projectiles[i]].needsReload = false;
				}
			}
		}
	}

	// Token: 0x06006E7D RID: 28285 RVA: 0x002B9684 File Offset: 0x002B7884
	public void ClearCooldowns()
	{
		if (this.Volley != null)
		{
			for (int i = 0; i < this.Volley.projectiles.Count; i++)
			{
				this.m_moduleData[this.Volley.projectiles[i]].onCooldown = false;
			}
		}
		else
		{
			this.m_moduleData[this.singleModule].onCooldown = false;
		}
		if (this.UsesRechargeLikeActiveItem)
		{
			this.RemainingActiveCooldownAmount = 0f;
		}
	}

	// Token: 0x06006E7E RID: 28286 RVA: 0x002B9718 File Offset: 0x002B7918
	public void ClearReloadData()
	{
		if (this.Volley != null)
		{
			for (int i = 0; i < this.Volley.projectiles.Count; i++)
			{
				this.m_moduleData[this.Volley.projectiles[i]].needsReload = false;
			}
			this.m_isReloading = false;
		}
		else
		{
			this.m_moduleData[this.singleModule].needsReload = false;
			this.m_isReloading = false;
		}
	}

	// Token: 0x06006E7F RID: 28287 RVA: 0x002B97A4 File Offset: 0x002B79A4
	public Gun.AttackResult Attack(ProjectileData overrideProjectileData = null, GameObject overrideBulletObject = null)
	{
		if (this.m_isCurrentlyFiring && this.m_midBurstFire)
		{
			return Gun.AttackResult.Fail;
		}
		if (!this.m_hasReinitializedAudioSwitch)
		{
			this.m_hasReinitializedAudioSwitch = true;
			if (!string.IsNullOrEmpty(this.gunSwitchGroup))
			{
				AkSoundEngine.SetSwitch("WPN_Guns", this.gunSwitchGroup, base.gameObject);
			}
		}
		this.m_playedEmptyClipSound = false;
		this.m_continuousAttackTime = 0f;
		if (this.m_isReloading)
		{
			this.Reload();
			return Gun.AttackResult.Reload;
		}
		if (this.CurrentAmmo < 0)
		{
			this.CurrentAmmo = 0;
		}
		if (this.CurrentAmmo == 0)
		{
			if (!this.InfiniteAmmo)
			{
				this.HandleOutOfAmmo();
				return Gun.AttackResult.Empty;
			}
			this.GainAmmo(this.maxAmmo);
		}
		this.m_cachedIsGunBlocked = this.IsGunBlocked();
		this.m_isCurrentlyFiring = true;
		bool flag = false;
		if (this.CanCriticalFire)
		{
			float num = (float)PlayerStats.GetTotalCoolness() / 100f;
			if (this.m_owner.IsStealthed)
			{
				num = 10f;
			}
			if (UnityEngine.Random.value < this.CriticalChance + num)
			{
				this.m_isCritting = true;
			}
			if (this.ForceNextShotCritical)
			{
				this.ForceNextShotCritical = false;
				this.m_isCritting = true;
			}
		}
		if (this.IsHeroSword)
		{
			flag = true;
			if (!this.m_anim.IsPlaying(this.shootAnimation) && !this.m_anim.IsPlaying(this.reloadAnimation) && this.HeroSwordCooldown <= 0f)
			{
				this.HeroSwordCooldown = 0.5f;
				base.StartCoroutine(this.HandleSlash());
				this.HandleShootAnimation(null);
			}
		}
		else if (this.Volley != null)
		{
			bool flag2 = this.CheckHasLoadedModule(this.Volley);
			if (!flag2)
			{
				this.AttemptedFireNeedReload();
			}
			if (flag2 || this.reloadTime <= 0f)
			{
				ProjectileVolleyData volley = this.Volley;
				if (this.modifiedFinalVolley != null && this.DefaultModule.HasFinalVolleyOverride() && this.DefaultModule.IsFinalShot(this.m_moduleData[this.DefaultModule], this.CurrentOwner))
				{
					volley = this.modifiedFinalVolley;
				}
				flag = this.HandleInitialGunShoot(volley, overrideProjectileData, overrideBulletObject);
				this.m_midBurstFire = false;
				for (int i = 0; i < this.Volley.projectiles.Count; i++)
				{
					ProjectileModule projectileModule = this.Volley.projectiles[i];
					if (projectileModule.shootStyle == ProjectileModule.ShootStyle.Burst && this.m_moduleData[projectileModule].numberShotsFiredThisBurst < projectileModule.burstShotCount)
					{
						this.m_midBurstFire = true;
						break;
					}
				}
			}
		}
		else
		{
			bool flag3 = this.CheckHasLoadedModule(this.singleModule);
			if (!flag3)
			{
				this.AttemptedFireNeedReload();
			}
			if (flag3 || this.reloadTime <= 0f)
			{
				flag = this.HandleInitialGunShoot(this.singleModule, overrideProjectileData, overrideBulletObject);
				this.m_midBurstFire = false;
				if (this.singleModule.shootStyle == ProjectileModule.ShootStyle.Burst && this.m_moduleData[this.singleModule].numberShotsFiredThisBurst < this.singleModule.burstShotCount)
				{
					this.m_midBurstFire = true;
				}
			}
		}
		this.m_isCurrentlyFiring = flag;
		if (this.m_isCurrentlyFiring && this.lowersAudioWhileFiring)
		{
			AkSoundEngine.PostEvent("play_state_volume_lower_01", GameManager.Instance.gameObject);
		}
		if (flag && this.OnPostFired != null && this.m_owner is PlayerController)
		{
			this.OnPostFired(this.m_owner as PlayerController, this);
		}
		return (!flag) ? Gun.AttackResult.OnCooldown : Gun.AttackResult.Success;
	}

	// Token: 0x06006E80 RID: 28288 RVA: 0x002B9B68 File Offset: 0x002B7D68
	public bool ContinueAttack(bool canAttack = true, ProjectileData overrideProjectileData = null)
	{
		if (!this.m_isCurrentlyFiring)
		{
			if (!this.HasShootStyle(ProjectileModule.ShootStyle.Charged) && !this.HasShootStyle(ProjectileModule.ShootStyle.Automatic) && !this.HasShootStyle(ProjectileModule.ShootStyle.Burst))
			{
				return false;
			}
			if (this.IsEmpty)
			{
				return false;
			}
			if (this.m_isReloading)
			{
				return false;
			}
			if (this.CurrentAmmo < 0)
			{
				this.CurrentAmmo = 0;
			}
			return this.CurrentAmmo != 0 && canAttack && this.Attack(overrideProjectileData, null) == Gun.AttackResult.Success;
		}
		else
		{
			if (this.m_isReloading)
			{
				return false;
			}
			if (this.CurrentAmmo < 0)
			{
				this.CurrentAmmo = 0;
			}
			if (this.CurrentAmmo == 0)
			{
				this.CeaseAttack(false, null);
				return false;
			}
			if (!this.m_playedEmptyClipSound && this.ClipShotsRemaining == 0)
			{
				if (GameManager.AUDIO_ENABLED)
				{
					AkSoundEngine.PostEvent("Play_WPN_gun_empty_01", base.gameObject);
				}
				this.m_playedEmptyClipSound = true;
			}
			this.m_cachedIsGunBlocked = this.IsGunBlocked();
			this.m_isCurrentlyFiring = true;
			this.m_continuousAttackTime += BraveTime.DeltaTime;
			bool flag = false;
			if (!canAttack || this.m_cachedIsGunBlocked)
			{
				if (this.m_activeBeams.Count > 0)
				{
					this.ClearBeams();
				}
				else if (this.isAudioLoop && this.m_isAudioLooping)
				{
					if (GameManager.AUDIO_ENABLED)
					{
						AkSoundEngine.PostEvent("Stop_WPN_gun_loop_01", base.gameObject);
					}
					this.m_isAudioLooping = false;
				}
				this.ClearBurstState();
				if (this.usesContinuousMuzzleFlash)
				{
					this.muzzleFlashEffects.DestroyAll();
					this.m_isContinuousMuzzleFlashOut = false;
				}
				this.m_continuousAttackTime = 0f;
			}
			if (this.m_activeBeams.Count > 0 && this.m_owner is PlayerController)
			{
				GameStatsManager.Instance.RegisterStatChange(TrackedStats.BEAM_WEAPON_FIRE_TIME, BraveTime.DeltaTime);
			}
			if (this.CanCriticalFire)
			{
				float num = (float)PlayerStats.GetTotalCoolness() / 100f;
				if (this.m_owner.IsStealthed)
				{
					num = 10f;
				}
				if (UnityEngine.Random.value < this.CriticalChance + num)
				{
					this.m_isCritting = true;
				}
				if (this.ForceNextShotCritical)
				{
					this.ForceNextShotCritical = false;
					this.m_isCritting = true;
				}
			}
			if (this.Volley != null)
			{
				if (this.CheckHasLoadedModule(this.Volley))
				{
					ProjectileVolleyData volley = this.Volley;
					if (this.modifiedFinalVolley != null && this.DefaultModule.HasFinalVolleyOverride() && this.DefaultModule.IsFinalShot(this.m_moduleData[this.DefaultModule], this.CurrentOwner))
					{
						volley = this.modifiedFinalVolley;
					}
					flag = this.HandleContinueGunShoot(volley, canAttack, overrideProjectileData);
					this.m_midBurstFire = false;
					for (int i = 0; i < this.Volley.projectiles.Count; i++)
					{
						ProjectileModule projectileModule = this.Volley.projectiles[i];
						if (projectileModule.shootStyle == ProjectileModule.ShootStyle.Burst && this.m_moduleData[projectileModule].numberShotsFiredThisBurst < projectileModule.burstShotCount)
						{
							this.m_midBurstFire = true;
							break;
						}
					}
				}
				else
				{
					this.CeaseAttack(false, null);
				}
			}
			else if (this.CheckHasLoadedModule(this.singleModule))
			{
				flag = this.HandleContinueGunShoot(this.singleModule, canAttack, overrideProjectileData);
			}
			else
			{
				this.CeaseAttack(false, null);
				this.m_midBurstFire = false;
				if (this.singleModule.shootStyle == ProjectileModule.ShootStyle.Burst && this.m_moduleData[this.singleModule].numberShotsFiredThisBurst < this.singleModule.burstShotCount)
				{
					this.m_midBurstFire = true;
				}
			}
			if (flag && this.OnPostFired != null && this.m_owner is PlayerController)
			{
				this.OnPostFired(this.m_owner as PlayerController, this);
			}
			return flag;
		}
	}

	// Token: 0x06006E81 RID: 28289 RVA: 0x002B9F60 File Offset: 0x002B8160
	public void OnPrePlayerChange()
	{
		if (this.m_isPreppedForThrow)
		{
			this.UnprepGunForThrow();
		}
	}

	// Token: 0x06006E82 RID: 28290 RVA: 0x002B9F74 File Offset: 0x002B8174
	public bool CeaseAttack(bool canAttack = true, ProjectileData overrideProjectileData = null)
	{
		if (this.m_isPreppedForThrow && this.m_prepThrowTime < 1.2f)
		{
			this.UnprepGunForThrow();
		}
		else if (this.m_isPreppedForThrow)
		{
			(this.m_owner as PlayerController).inventory.RemoveGunFromInventory(this);
			this.ThrowGun();
			return true;
		}
		if (!this.m_isCurrentlyFiring)
		{
			return false;
		}
		if (this.m_midBurstFire && canAttack)
		{
			this.m_continueBurstInUpdate = true;
			return true;
		}
		this.m_continueBurstInUpdate = false;
		if (this.m_isCurrentlyFiring && this.lowersAudioWhileFiring)
		{
			AkSoundEngine.PostEvent("stop_state_volume_lower_01", GameManager.Instance.gameObject);
		}
		this.m_isCurrentlyFiring = false;
		this.m_hasDecrementedFunds = false;
		this.m_continuousAttackTime = 0f;
		this.m_cachedIsGunBlocked = this.IsGunBlocked();
		if (this.CanCriticalFire)
		{
			float num = (float)PlayerStats.GetTotalCoolness() / 100f;
			if (this.m_owner.IsStealthed)
			{
				num = 10f;
			}
			if (UnityEngine.Random.value < this.CriticalChance + num)
			{
				this.m_isCritting = true;
			}
			if (this.ForceNextShotCritical)
			{
				this.ForceNextShotCritical = false;
				this.m_isCritting = true;
			}
		}
		if (this.LockedHorizontalOnCharge)
		{
			this.m_attachTransform.localRotation = Quaternion.Euler(this.m_attachTransform.localRotation.x, this.m_attachTransform.localRotation.y, this.LockedHorizontalCachedAngle);
			this.gunAngle = this.LockedHorizontalCachedAngle;
		}
		bool flag;
		if (this.Volley != null)
		{
			flag = this.HandleEndGunShoot(this.Volley, canAttack, overrideProjectileData);
		}
		else
		{
			flag = this.HandleEndGunShoot(this.singleModule, canAttack, overrideProjectileData);
		}
		if (this.MovesPlayerForwardOnChargeFire && flag && this.m_owner && this.m_owner is PlayerController)
		{
			this.m_owner.knockbackDoer.ApplyKnockback(BraveMathCollege.DegreesToVector(this.CurrentAngle, 1f), 40f, 0.25f, false);
		}
		if (GameManager.AUDIO_ENABLED)
		{
			AkSoundEngine.PostEvent("Stop_WPN_gun_loop_01", base.gameObject);
		}
		this.m_isAudioLooping = false;
		this.ClearBeams();
		if (this.usesContinuousFireAnimation)
		{
			this.m_anim.StopAndResetFrame();
			this.AnimationCompleteDelegate(this.m_anim, null);
		}
		if (this.usesContinuousMuzzleFlash)
		{
			this.muzzleFlashEffects.DestroyAll();
			this.m_isContinuousMuzzleFlashOut = false;
		}
		if (!this.m_isReloading && this.DefaultModule.GetModNumberOfShotsInClip(this.CurrentOwner) == 1)
		{
			this.m_reloadWhenDoneFiring = true;
		}
		if (this.Volley)
		{
			ProjectileVolleyData volley = this.Volley;
			if (volley)
			{
				for (int i = 0; i < volley.projectiles.Count; i++)
				{
					this.m_moduleData[volley.projectiles[i]].numberShotsFiredThisBurst = 0;
				}
			}
		}
		else
		{
			this.m_moduleData[this.singleModule].numberShotsFiredThisBurst = 0;
		}
		if (this.CurrentOwner is PlayerController && this.OnFinishAttack != null)
		{
			this.OnFinishAttack(this.CurrentOwner as PlayerController, this);
		}
		return flag;
	}

	// Token: 0x06006E83 RID: 28291 RVA: 0x002BA2D8 File Offset: 0x002B84D8
	public void AttemptedFireNeedReload()
	{
		PlayerController playerController = this.m_owner as PlayerController;
		this.Reload();
		if (this.OnReloadPressed != null && playerController)
		{
			this.OnReloadPressed(playerController, this, false);
		}
	}

	// Token: 0x06006E84 RID: 28292 RVA: 0x002BA31C File Offset: 0x002B851C
	protected void OnActiveReloadSuccess()
	{
		this.FinishReload(true, false, false);
		float num = 1f;
		if (Gun.ActiveReloadActivated && this.m_owner is PlayerController && (this.m_owner as PlayerController).IsPrimaryPlayer)
		{
			num *= CogOfBattleItem.ACTIVE_RELOAD_DAMAGE_MULTIPLIER;
		}
		if (Gun.ActiveReloadActivatedPlayerTwo && this.m_owner is PlayerController && !(this.m_owner as PlayerController).IsPrimaryPlayer)
		{
			num *= CogOfBattleItem.ACTIVE_RELOAD_DAMAGE_MULTIPLIER;
		}
		if (this.LocalActiveReload)
		{
			num *= Mathf.Pow(this.activeReloadData.damageMultiply, (float)(this.SequentialActiveReloads + 1));
		}
		Debug.LogError("total damage multiplier: " + num);
		if (this.Volley != null)
		{
			for (int i = 0; i < this.Volley.projectiles.Count; i++)
			{
				this.m_moduleData[this.Volley.projectiles[i]].activeReloadDamageModifier = num;
			}
		}
		else
		{
			this.m_moduleData[this.singleModule].activeReloadDamageModifier = num;
		}
	}

	// Token: 0x06006E85 RID: 28293 RVA: 0x002BA454 File Offset: 0x002B8654
	private void HandleOutOfAmmo()
	{
		if (this.m_owner is PlayerController)
		{
			this.PrepGunForThrow();
		}
		else
		{
			this.m_owner.aiShooter.Inventory.RemoveGunFromInventory(this);
		}
	}

	// Token: 0x06006E86 RID: 28294 RVA: 0x002BA488 File Offset: 0x002B8688
	public void HandleShootAnimation(ProjectileModule module)
	{
		if (this.m_anim != null)
		{
			string overrideShootAnimation = this.shootAnimation;
			if (module != null && !string.IsNullOrEmpty(this.finalShootAnimation) && module.IsFinalShot(this.m_moduleData[module], this.CurrentOwner))
			{
				overrideShootAnimation = this.finalShootAnimation;
			}
			if (this.m_isCritting && !string.IsNullOrEmpty(this.criticalFireAnimation))
			{
				overrideShootAnimation = this.criticalFireAnimation;
			}
			if (module != null && module.shootStyle == ProjectileModule.ShootStyle.Charged)
			{
				ProjectileModule.ChargeProjectile chargeProjectile = module.GetChargeProjectile(this.m_moduleData[module].chargeTime);
				if (chargeProjectile != null && chargeProjectile.UsesOverrideShootAnimation)
				{
					overrideShootAnimation = chargeProjectile.OverrideShootAnimation;
				}
			}
			this.PlayIfExists(overrideShootAnimation, true);
		}
	}

	// Token: 0x06006E87 RID: 28295 RVA: 0x002BA554 File Offset: 0x002B8754
	public void HandleShootEffects(ProjectileModule module)
	{
		Transform transform = ((!this.muzzleOffset) ? this.barrelOffset : this.muzzleOffset);
		Vector3 vector = transform.position - new Vector3(0f, 0f, 0.1f);
		VFXPool vfxpool = this.muzzleFlashEffects;
		if (module != null && this.finalMuzzleFlashEffects.type != VFXPoolType.None && module.IsFinalShot(this.m_moduleData[module], this.CurrentOwner))
		{
			vfxpool = this.finalMuzzleFlashEffects;
		}
		if (this.m_isCritting && this.CriticalMuzzleFlashEffects.type != VFXPoolType.None)
		{
			vfxpool = this.CriticalMuzzleFlashEffects;
		}
		if (module != null && module.shootStyle == ProjectileModule.ShootStyle.Charged)
		{
			ProjectileModule.ChargeProjectile chargeProjectile = module.GetChargeProjectile(this.m_moduleData[module].chargeTime);
			if (chargeProjectile != null && chargeProjectile.UsesOverrideMuzzleFlashVfxPool)
			{
				vfxpool = chargeProjectile.OverrideMuzzleFlashVfxPool;
			}
		}
		if (!this.usesContinuousMuzzleFlash || !this.m_isContinuousMuzzleFlashOut)
		{
			vfxpool.SpawnAtPosition(vector, (!this.preventRotation) ? this.gunAngle : 0f, transform, new Vector2?(Vector2.zero), new Vector2?(Vector2.zero), new float?(-0.05f), true, null, null, false);
		}
		if (this.usesContinuousMuzzleFlash)
		{
			this.m_isContinuousMuzzleFlashOut = true;
		}
		if (this.shellsToLaunchOnFire > 0)
		{
			if (this.shellCasingOnFireFrameDelay <= 0)
			{
				for (int i = 0; i < this.shellsToLaunchOnFire; i++)
				{
					this.SpawnShellCasingAtPosition(this.CasingLaunchPoint);
				}
			}
			else
			{
				base.StartCoroutine(this.HandleShellCasingFireDelay());
			}
		}
	}

	// Token: 0x06006E88 RID: 28296 RVA: 0x002BA708 File Offset: 0x002B8908
	private void TogglePreviousChargeEffectsIfNecessary(ProjectileModule.ChargeProjectile cp, bool visible)
	{
		if (cp == null)
		{
			return;
		}
		if (cp.previousChargeProjectile != null && cp.previousChargeProjectile.DelayedVFXDestruction)
		{
			this.TogglePreviousChargeEffectsIfNecessary(cp.previousChargeProjectile, visible);
		}
		if (cp.UsesVfx && cp.VfxPool != null)
		{
			cp.VfxPool.ToggleRenderers(visible);
		}
	}

	// Token: 0x06006E89 RID: 28297 RVA: 0x002BA768 File Offset: 0x002B8968
	private void DestroyPreviousChargeEffectsIfNecessary(ProjectileModule.ChargeProjectile cp)
	{
		if (cp.previousChargeProjectile != null && cp.previousChargeProjectile.DelayedVFXDestruction)
		{
			this.DestroyPreviousChargeEffectsIfNecessary(cp.previousChargeProjectile);
		}
		if (cp.UsesVfx)
		{
			cp.VfxPool.DestroyAll();
		}
	}

	// Token: 0x06006E8A RID: 28298 RVA: 0x002BA7A8 File Offset: 0x002B89A8
	private void HandleChargeEffects(ProjectileModule.ChargeProjectile oldChargeProjectile, ProjectileModule.ChargeProjectile newChargeProjectile)
	{
		Transform transform = ((!this.chargeOffset) ? ((!this.muzzleOffset) ? this.barrelOffset : this.muzzleOffset) : this.chargeOffset);
		Vector3 vector = transform.position - new Vector3(0f, 0f, 0.1f);
		if (oldChargeProjectile != null)
		{
			if (!oldChargeProjectile.DelayedVFXDestruction || newChargeProjectile == null)
			{
				this.DestroyPreviousChargeEffectsIfNecessary(oldChargeProjectile);
			}
			if (oldChargeProjectile.UsesVfx && oldChargeProjectile.VfxPool == this.m_currentlyPlayingChargeVFX)
			{
				this.m_currentlyPlayingChargeVFX = null;
			}
			if (oldChargeProjectile.UsesScreenShake)
			{
				GameManager.Instance.MainCameraController.StopContinuousScreenShake(this);
			}
		}
		if (newChargeProjectile != null)
		{
			newChargeProjectile.previousChargeProjectile = oldChargeProjectile;
			if (newChargeProjectile.UsesVfx)
			{
				newChargeProjectile.VfxPool.SpawnAtPosition(vector, this.gunAngle, transform, new Vector2?(Vector2.zero), new Vector2?(Vector2.zero), new float?(2f), true, null, null, false);
				this.m_currentlyPlayingChargeVFX = newChargeProjectile.VfxPool;
				if (!this.m_meshRenderer.enabled)
				{
					this.m_currentlyPlayingChargeVFX.ToggleRenderers(false);
				}
				else
				{
					this.m_currentlyPlayingChargeVFX.ToggleRenderers(true);
				}
			}
			if (newChargeProjectile.ShouldDoChargePoof && this.m_owner is PlayerController)
			{
				this.DoChargeCompletePoof();
			}
			if (newChargeProjectile.UsesScreenShake)
			{
				GameManager.Instance.MainCameraController.DoContinuousScreenShake(newChargeProjectile.ScreenShake, this, this.m_owner is PlayerController);
			}
		}
	}

	// Token: 0x06006E8B RID: 28299 RVA: 0x002BA944 File Offset: 0x002B8B44
	private void DoChargeCompletePoof()
	{
		GameObject gameObject = SpawnManager.SpawnVFX(BraveResources.Load<GameObject>("Global VFX/VFX_DBZ_Charge", ".prefab"), false);
		gameObject.transform.parent = this.m_owner.transform;
		gameObject.transform.position = this.m_owner.specRigidbody.UnitCenter;
		tk2dBaseSprite component = gameObject.GetComponent<tk2dBaseSprite>();
		component.HeightOffGround = -1f;
		component.UpdateZDepth();
		(this.CurrentOwner as PlayerController).DoVibration(Vibration.Time.Quick, Vibration.Strength.Medium);
	}

	// Token: 0x06006E8C RID: 28300 RVA: 0x002BA9CC File Offset: 0x002B8BCC
	private void HandleChargeIntensity(ProjectileModule module, ModuleShootData shootData)
	{
		if (!this.light)
		{
			return;
		}
		ProjectileModule.ChargeProjectile chargeProjectile = module.GetChargeProjectile(shootData.chargeTime);
		if (chargeProjectile == null)
		{
			return;
		}
		float num = ((!chargeProjectile.UsesLightIntensity) ? this.baseLightIntensity : chargeProjectile.LightIntensity);
		float chargeTime = chargeProjectile.ChargeTime;
		float num2 = num;
		float num3 = chargeTime;
		int num4 = module.chargeProjectiles.IndexOf(chargeProjectile);
		if (num4 < module.chargeProjectiles.Count - 1)
		{
			num2 = ((!module.chargeProjectiles[num4 + 1].UsesLightIntensity) ? this.baseLightIntensity : module.chargeProjectiles[num4 + 1].LightIntensity);
			num3 = module.chargeProjectiles[num4 + 1].ChargeTime;
		}
		this.light.intensity = Mathf.Lerp(num, num2, Mathf.InverseLerp(chargeTime, num3, shootData.chargeTime));
	}

	// Token: 0x06006E8D RID: 28301 RVA: 0x002BAAB8 File Offset: 0x002B8CB8
	private void EndChargeIntensity()
	{
		if (this.light)
		{
			this.light.intensity = this.baseLightIntensity;
		}
	}

	// Token: 0x06006E8E RID: 28302 RVA: 0x002BAADC File Offset: 0x002B8CDC
	private IEnumerator HandleShellCasingFireDelay()
	{
		if (this.m_anim != null && this.m_anim.CurrentClip != null)
		{
			float frameLength = 1f / this.m_anim.CurrentClip.fps;
			yield return new WaitForSeconds(frameLength * (float)this.shellCasingOnFireFrameDelay);
		}
		if (!this || !this.m_owner)
		{
			yield break;
		}
		for (int i = 0; i < this.shellsToLaunchOnFire; i++)
		{
			this.SpawnShellCasingAtPosition(this.CasingLaunchPoint);
		}
		yield break;
	}

	// Token: 0x06006E8F RID: 28303 RVA: 0x002BAAF8 File Offset: 0x002B8CF8
	private void SpawnShellCasingAtPosition(Vector3 position)
	{
		if (this.shellCasing != null && this.m_transform)
		{
			GameObject gameObject = SpawnManager.SpawnDebris(this.shellCasing, position.WithZ(this.m_transform.position.z), Quaternion.Euler(0f, 0f, this.gunAngle));
			ShellCasing component = gameObject.GetComponent<ShellCasing>();
			if (component != null)
			{
				component.Trigger();
			}
			DebrisObject component2 = gameObject.GetComponent<DebrisObject>();
			if (component2 != null)
			{
				int num = ((component2.transform.right.x <= 0f) ? (-1) : 1);
				Vector3 vector = Vector3.up * (UnityEngine.Random.value * 1.5f + 1f) + -1.5f * Vector3.right * (float)num * (UnityEngine.Random.value + 1.5f);
				Vector3 vector2 = new Vector3(vector.x, 0f, vector.y);
				if (this.m_owner is PlayerController)
				{
					PlayerController playerController = this.m_owner as PlayerController;
					if (playerController.CurrentRoom != null && playerController.CurrentRoom.area.PrototypeRoomSpecialSubcategory == PrototypeDungeonRoom.RoomSpecialSubCategory.CATACOMBS_BRIDGE_ROOM)
					{
						vector2 = (vector.x * (float)num * -1f * (this.barrelOffset.position.XY() - this.m_localAimPoint).normalized).ToVector3ZUp(vector.y);
					}
				}
				float y = this.m_owner.transform.position.y;
				float num2 = position.y - this.m_owner.transform.position.y + 0.2f;
				float num3 = component2.transform.position.y - y + UnityEngine.Random.value * 0.5f;
				component2.additionalHeightBoost = num2 - num3;
				if (this.gunAngle > 25f && this.gunAngle < 155f)
				{
					component2.additionalHeightBoost += -0.25f;
				}
				else
				{
					component2.additionalHeightBoost += 0.25f;
				}
				component2.Trigger(vector2, num3, 1f);
			}
		}
	}

	// Token: 0x06006E90 RID: 28304 RVA: 0x002BAD6C File Offset: 0x002B8F6C
	private void SpawnClipAtPosition(Vector3 position)
	{
		if (this.clipObject != null)
		{
			GameObject gameObject = SpawnManager.SpawnDebris(this.clipObject, position.WithZ(-0.05f), Quaternion.Euler(0f, 0f, this.gunAngle));
			DebrisObject component = gameObject.GetComponent<DebrisObject>();
			if (component)
			{
				float num = 0.25f;
				int num2 = ((component.transform.right.x <= 0f) ? (-1) : 1);
				Vector3 vector = new Vector3(0f, -1f, 0f);
				if (this.m_owner is PlayerController)
				{
					PlayerController playerController = this.m_owner as PlayerController;
					if (playerController.CurrentRoom != null && playerController.CurrentRoom.area.PrototypeRoomSpecialSubcategory == PrototypeDungeonRoom.RoomSpecialSubCategory.CATACOMBS_BRIDGE_ROOM)
					{
						vector = new Vector3((float)num2 * 0.5f * (this.barrelOffset.position.XY() - this.m_localAimPoint).x, 0f, 1f);
						num = 0.5f;
					}
				}
				component.Trigger(vector, num, 1f);
			}
		}
	}

	// Token: 0x06006E91 RID: 28305 RVA: 0x002BAE9C File Offset: 0x002B909C
	private void DoScreenShake()
	{
		Vector2 vector = Quaternion.Euler(0f, 0f, this.gunAngle) * Vector3.right;
		if (GameManager.Instance.MainCameraController == null)
		{
			return;
		}
		if (this.directionlessScreenShake)
		{
			vector = Vector2.zero;
		}
		GameManager.Instance.MainCameraController.DoGunScreenShake(this.gunScreenShake, vector, null, this.m_owner as PlayerController);
	}

	// Token: 0x06006E92 RID: 28306 RVA: 0x002BAF20 File Offset: 0x002B9120
	public override void Pickup(PlayerController player)
	{
		if (RoomHandler.unassignedInteractableObjects.Contains(this))
		{
			RoomHandler.unassignedInteractableObjects.Remove(this);
		}
		if (GameManager.Instance.InTutorial)
		{
			GameManager.BroadcastRoomTalkDoerFsmEvent("playerAcquiredGun");
		}
		this.m_isThrown = false;
		this.m_isBeingEyedByRat = false;
		base.OnSharedPickup();
		if (!this.HasEverBeenAcquiredByPlayer)
		{
			player.HasReceivedNewGunThisFloor = true;
		}
		this.HasEverBeenAcquiredByPlayer = true;
		if (!this.ShouldBeDestroyedOnExistence(false))
		{
			GameStatsManager.Instance.RegisterStatChange(TrackedStats.GUNS_PICKED_UP, 1f);
			if (!PileOfDarkSoulsPickup.IsPileOfDarkSoulsPickup)
			{
				player.PlayEffectOnActor(ResourceCache.Acquire("Global VFX/VFX_Ammo_Sparkles_001") as GameObject, Vector3.zero, true, false, false);
			}
			base.HandleEncounterable(player);
			this.GetRidOfMinimapIcon();
			if (GameManager.AUDIO_ENABLED)
			{
				AkSoundEngine.PostEvent("Play_OBJ_weapon_pickup_01", base.gameObject);
			}
			if (player.CharacterUsesRandomGuns)
			{
				player.ChangeToRandomGun();
			}
			else
			{
				Gun gun = player.inventory.AddGunToInventory(this, true);
				if (gun.AdjustedMaxAmmo > 0)
				{
					gun.ammo = Math.Min(gun.AdjustedMaxAmmo, gun.ammo);
				}
			}
			PlatformInterface.SetAlienFXColor(this.m_alienPickupColor, 1f);
		}
		if (base.transform.parent != null)
		{
			UnityEngine.Object.Destroy(base.transform.parent.gameObject);
		}
		else
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x06006E93 RID: 28307 RVA: 0x002BB098 File Offset: 0x002B9298
	public bool HasShootStyle(ProjectileModule.ShootStyle shootStyle)
	{
		ProjectileVolleyData volley = this.Volley;
		if (this.Volley == null)
		{
			return this.singleModule.shootStyle == shootStyle;
		}
		for (int i = 0; i < volley.projectiles.Count; i++)
		{
			if (volley.projectiles[i] != null && volley.projectiles[i].shootStyle == shootStyle)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06006E94 RID: 28308 RVA: 0x002BB114 File Offset: 0x002B9314
	protected void AnimationCompleteDelegate(tk2dSpriteAnimator anima, tk2dSpriteAnimationClip clippy)
	{
		if (clippy != null)
		{
			if (!this.DisablesRendererOnCooldown || !this.m_reloadWhenDoneFiring)
			{
				this.PlayIdleAnimation();
			}
		}
	}

	// Token: 0x06006E95 RID: 28309 RVA: 0x002BB140 File Offset: 0x002B9340
	public void OnTrigger(SpeculativeRigidbody otherRigidbody, SpeculativeRigidbody source, CollisionData collisionData)
	{
		PlayerController component = otherRigidbody.GetComponent<PlayerController>();
		if (component != null && BraveInput.WasSelectPressed(null))
		{
			this.Pickup(component);
		}
	}

	// Token: 0x06006E96 RID: 28310 RVA: 0x002BB174 File Offset: 0x002B9374
	private void PostRigidbodyMovement(SpeculativeRigidbody specRigidbody, Vector2 unitDelta, IntVector2 pixelDelta)
	{
		if (this && base.enabled)
		{
			for (int i = this.m_activeBeams.Count - 1; i >= 0; i--)
			{
				if (i >= 0 && i < this.m_activeBeams.Count)
				{
					ModuleShootData moduleShootData = this.m_activeBeams[i];
					if (!moduleShootData.beam)
					{
						if (moduleShootData.beamKnockbackID >= 0)
						{
							if (this.m_owner && this.m_owner.knockbackDoer)
							{
								this.m_owner.knockbackDoer.EndContinuousKnockback(moduleShootData.beamKnockbackID);
							}
							moduleShootData.beamKnockbackID = -1;
						}
						this.m_activeBeams.RemoveAt(i);
					}
					else
					{
						moduleShootData.beam.LateUpdatePosition(this.m_unroundedBarrelPosition + unitDelta);
					}
				}
			}
		}
	}

	// Token: 0x06006E97 RID: 28311 RVA: 0x002BB26C File Offset: 0x002B946C
	private bool CheckHasLoadedModule(ProjectileModule module)
	{
		if (this.RequiresFundsToShoot && this.m_owner is PlayerController)
		{
			this.m_moduleData[module].numberShotsFired = 0;
			this.m_moduleData[module].needsReload = false;
			return (this.m_owner as PlayerController).carriedConsumables.Currency > 0;
		}
		return !module.ignoredForReloadPurposes && !this.m_moduleData[module].needsReload;
	}

	// Token: 0x06006E98 RID: 28312 RVA: 0x002BB2F8 File Offset: 0x002B94F8
	private bool CheckHasLoadedModule(ProjectileVolleyData Volley)
	{
		if (this.RequiresFundsToShoot && this.m_owner is PlayerController)
		{
			for (int i = 0; i < Volley.projectiles.Count; i++)
			{
				this.m_moduleData[Volley.projectiles[i]].numberShotsFired = 0;
				this.m_moduleData[Volley.projectiles[i]].needsReload = false;
			}
			return (this.m_owner as PlayerController).carriedConsumables.Currency > 0;
		}
		for (int j = 0; j < Volley.projectiles.Count; j++)
		{
			ProjectileModule projectileModule = Volley.projectiles[j];
			if (!projectileModule.ignoredForReloadPurposes)
			{
				if (!this.m_moduleData[projectileModule].needsReload)
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x06006E99 RID: 28313 RVA: 0x002BB3E8 File Offset: 0x002B95E8
	private void CreateAmp()
	{
		if (this.ObjectToInstantiateOnReload && this.m_owner && this.m_owner is PlayerController)
		{
			if (this.m_extantAmp != null)
			{
				if (this.m_extantAmp as ShootProjectileOnGunfireDoer)
				{
					this.m_extantAmp.Deactivate();
					this.m_extantAmp = null;
				}
				else if (this.m_extantAmp as BreakableShieldController && !(this.m_extantAmp as BreakableShieldController).majorBreakable.IsDestroyed)
				{
					return;
				}
			}
			PlayerController playerController = this.m_owner as PlayerController;
			IntVector2 bestRewardLocation = playerController.CurrentRoom.GetBestRewardLocation(IntVector2.One, RoomHandler.RewardLocationStyle.PlayerCenter, false);
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.ObjectToInstantiateOnReload, bestRewardLocation.ToVector3(), Quaternion.identity);
			if (gameObject)
			{
				this.m_extantAmp = gameObject.GetInterface<SingleSpawnableGunPlacedObject>();
				if (this.m_extantAmp != null)
				{
					this.m_extantAmp.Initialize(this);
				}
			}
		}
	}

	// Token: 0x06006E9A RID: 28314 RVA: 0x002BB4F4 File Offset: 0x002B96F4
	private bool HandleInitialGunShoot(ProjectileModule module, ProjectileData overrideProjectileData = null, GameObject overrideBulletObject = null)
	{
		if (this.m_moduleData[module].needsReload)
		{
			Debug.LogError("Trying to shoot a gun without being loaded, should never happen.");
			return false;
		}
		return !this.m_moduleData[module].onCooldown && (!this.UsesRechargeLikeActiveItem || this.m_remainingActiveCooldownAmount <= 0f) && this.HandleSpecificInitialGunShoot(module, overrideProjectileData, overrideBulletObject, true);
	}

	// Token: 0x06006E9B RID: 28315 RVA: 0x002BB564 File Offset: 0x002B9764
	private void IncrementModuleFireCountAndMarkReload(ProjectileModule mod, ProjectileModule.ChargeProjectile currentChargeProjectile)
	{
		this.m_moduleData[mod].numberShotsFired++;
		this.m_moduleData[mod].numberShotsFiredThisBurst++;
		if (this.m_moduleData[mod].numberShotsActiveReload > 0)
		{
			this.m_moduleData[mod].numberShotsActiveReload--;
		}
		if (currentChargeProjectile != null && currentChargeProjectile.DepleteAmmo)
		{
			foreach (ProjectileModule projectileModule in this.m_moduleData.Keys)
			{
				if (!projectileModule.IsDuctTapeModule)
				{
					this.m_moduleData[projectileModule].numberShotsFired = projectileModule.GetModNumberOfShotsInClip(this.CurrentOwner);
					this.m_moduleData[projectileModule].needsReload = true;
				}
			}
		}
		if (mod.GetModNumberOfShotsInClip(this.CurrentOwner) > 0 && this.m_moduleData[mod].numberShotsFired >= mod.GetModNumberOfShotsInClip(this.CurrentOwner))
		{
			this.m_moduleData[mod].needsReload = true;
		}
		if (mod.shootStyle != ProjectileModule.ShootStyle.Charged)
		{
			mod.IncrementShootCount();
		}
	}

	// Token: 0x06006E9C RID: 28316 RVA: 0x002BB6C8 File Offset: 0x002B98C8
	private bool RawFireVolley(ProjectileVolleyData Volley)
	{
		bool flag = false;
		bool flag2 = true;
		for (int i = 0; i < Volley.projectiles.Count; i++)
		{
			ProjectileModule projectileModule = Volley.projectiles[i];
			if (!this.m_moduleData[projectileModule].needsReload)
			{
				if (!this.m_moduleData[projectileModule].onCooldown)
				{
					if (!this.UsesRechargeLikeActiveItem || this.m_remainingActiveCooldownAmount <= 0f)
					{
						if (Volley.ModulesAreTiers)
						{
							int num = ((projectileModule.CloneSourceIndex < 0) ? i : projectileModule.CloneSourceIndex);
							flag2 = num == this.m_currentStrengthTier;
						}
						if (flag2)
						{
							flag |= this.HandleSpecificInitialGunShoot(projectileModule, null, null, false);
						}
						else if (!this.m_cachedIsGunBlocked)
						{
							this.IncrementModuleFireCountAndMarkReload(projectileModule, null);
						}
					}
				}
			}
		}
		return flag;
	}

	// Token: 0x06006E9D RID: 28317 RVA: 0x002BB7C0 File Offset: 0x002B99C0
	private bool HandleInitialGunShoot(ProjectileVolleyData Volley, ProjectileData overrideProjectileData = null, GameObject overrideBulletObject = null)
	{
		bool flag = true;
		bool flag2 = false;
		bool flag3 = false;
		bool flag4 = true;
		bool flag5 = false;
		for (int i = 0; i < Volley.projectiles.Count; i++)
		{
			ProjectileModule projectileModule = Volley.projectiles[i];
			if (!this.m_moduleData[projectileModule].needsReload)
			{
				flag2 = true;
				if (!this.m_moduleData[projectileModule].onCooldown)
				{
					if (!this.UsesRechargeLikeActiveItem || this.m_remainingActiveCooldownAmount <= 0f)
					{
						if (Volley.ModulesAreTiers)
						{
							if (projectileModule.IsDuctTapeModule)
							{
								flag4 = true;
							}
							else
							{
								int num = ((projectileModule.CloneSourceIndex < 0) ? i : projectileModule.CloneSourceIndex);
								if (num == this.m_currentStrengthTier)
								{
									flag = !flag5;
									flag4 = true;
									flag5 = true;
								}
								else
								{
									flag = false;
									flag4 = false;
								}
							}
						}
						if (flag4)
						{
							flag3 |= this.HandleSpecificInitialGunShoot(projectileModule, overrideProjectileData, overrideBulletObject, flag);
						}
						else if (!this.m_cachedIsGunBlocked)
						{
							this.IncrementModuleFireCountAndMarkReload(projectileModule, null);
						}
						flag = false;
					}
				}
			}
		}
		if (!flag2)
		{
			Debug.LogError("Trying to shoot a gun without being loaded, should never happen.");
		}
		return flag3;
	}

	// Token: 0x06006E9E RID: 28318 RVA: 0x002BB900 File Offset: 0x002B9B00
	private bool HandleSpecificInitialGunShoot(ProjectileModule module, ProjectileData overrideProjectileData = null, GameObject overrideBulletObject = null, bool playEffects = true)
	{
		if (module.shootStyle == ProjectileModule.ShootStyle.SemiAutomatic || module.shootStyle == ProjectileModule.ShootStyle.Burst || module.shootStyle == ProjectileModule.ShootStyle.Automatic)
		{
			if (this.m_cachedIsGunBlocked)
			{
				return false;
			}
			if (playEffects)
			{
				this.HandleShootAnimation(module);
				this.HandleShootEffects(module);
				if (this.doesScreenShake)
				{
					this.DoScreenShake();
				}
			}
			if (playEffects || (module.runtimeGuid != null && this.AdditionalShootSoundsByModule.ContainsKey(module.runtimeGuid)))
			{
				if (module.runtimeGuid != null && this.AdditionalShootSoundsByModule.ContainsKey(module.runtimeGuid))
				{
					AkSoundEngine.SetSwitch("WPN_Guns", this.AdditionalShootSoundsByModule[module.runtimeGuid], base.gameObject);
				}
				if (GameManager.AUDIO_ENABLED && (!this.isAudioLoop || !this.m_isAudioLooping))
				{
					string text = ((!module.IsFinalShot(this.m_moduleData[module], this.m_owner) || this.OverrideFinaleAudio) ? "Play_WPN_gun_shot_01" : "Play_WPN_gun_finale_01");
					if (!this.PreventNormalFireAudio)
					{
						AkSoundEngine.PostEvent(text, base.gameObject);
					}
					else
					{
						AkSoundEngine.PostEvent(this.OverrideNormalFireAudioEvent, base.gameObject);
					}
					this.m_isAudioLooping = true;
				}
				if (!string.IsNullOrEmpty(this.gunSwitchGroup))
				{
					AkSoundEngine.SetSwitch("WPN_Guns", this.gunSwitchGroup, base.gameObject);
				}
			}
			this.ShootSingleProjectile(module, overrideProjectileData, overrideBulletObject);
			this.DecrementAmmoCost(module);
			this.TriggerModuleCooldown(module);
			return true;
		}
		else if (module.shootStyle == ProjectileModule.ShootStyle.Beam)
		{
			if (this.m_cachedIsGunBlocked)
			{
				return false;
			}
			if (playEffects)
			{
				if (this.m_anim != null)
				{
					this.PlayIfExists(this.shootAnimation, false);
				}
				this.HandleShootEffects(module);
			}
			if (playEffects || (module.runtimeGuid != null && this.AdditionalShootSoundsByModule.ContainsKey(module.runtimeGuid)))
			{
				if (module.runtimeGuid != null && this.AdditionalShootSoundsByModule.ContainsKey(module.runtimeGuid))
				{
					AkSoundEngine.SetSwitch("WPN_Guns", this.AdditionalShootSoundsByModule[module.runtimeGuid], base.gameObject);
				}
				if (GameManager.AUDIO_ENABLED && (!this.isAudioLoop || !this.m_isAudioLooping))
				{
					string text2 = ((!module.IsFinalShot(this.m_moduleData[module], this.m_owner) || this.OverrideFinaleAudio) ? "Play_WPN_gun_shot_01" : "Play_WPN_gun_finale_01");
					if (!this.PreventNormalFireAudio)
					{
						AkSoundEngine.PostEvent(text2, base.gameObject);
					}
					this.m_isAudioLooping = true;
				}
				if (!string.IsNullOrEmpty(this.gunSwitchGroup))
				{
					AkSoundEngine.SetSwitch("WPN_Guns", this.gunSwitchGroup, base.gameObject);
				}
			}
			this.BeginFiringBeam(module);
			return true;
		}
		else
		{
			if (module.shootStyle == ProjectileModule.ShootStyle.Charged)
			{
				ModuleShootData moduleShootData = this.m_moduleData[module];
				moduleShootData.chargeTime = 0f;
				moduleShootData.chargeFired = false;
				if (playEffects)
				{
					this.PlayIfExists(this.chargeAnimation, false);
					ProjectileModule.ChargeProjectile chargeProjectile = module.GetChargeProjectile(moduleShootData.chargeTime);
					this.HandleChargeEffects(null, chargeProjectile);
					this.HandleChargeIntensity(module, moduleShootData);
					moduleShootData.lastChargeProjectile = chargeProjectile;
					if (GameManager.AUDIO_ENABLED)
					{
						AkSoundEngine.PostEvent("Play_WPN_gun_charge_01", base.gameObject);
					}
				}
				return true;
			}
			return false;
		}
	}

	// Token: 0x06006E9F RID: 28319 RVA: 0x002BBC74 File Offset: 0x002B9E74
	private bool HandleContinueGunShoot(ProjectileModule module, bool canAttack = true, ProjectileData overrideProjectileData = null)
	{
		if (this.m_moduleData[module].needsReload)
		{
			Debug.LogError("Attempting to continue fire on an unloaded gun. This should never happen.");
			return false;
		}
		return !this.m_moduleData[module].onCooldown && (!this.UsesRechargeLikeActiveItem || this.m_remainingActiveCooldownAmount <= 0f) && this.HandleSpecificContinueGunShoot(module, canAttack, overrideProjectileData, true);
	}

	// Token: 0x06006EA0 RID: 28320 RVA: 0x002BBCE4 File Offset: 0x002B9EE4
	private bool HandleContinueGunShoot(ProjectileVolleyData Volley, bool canAttack = true, ProjectileData overrideProjectileData = null)
	{
		bool flag = true;
		bool flag2 = false;
		bool flag3 = false;
		bool flag4 = true;
		for (int i = 0; i < Volley.projectiles.Count; i++)
		{
			ProjectileModule projectileModule = Volley.projectiles[i];
			if (!this.m_moduleData[projectileModule].needsReload)
			{
				flag2 = true;
				if (!this.m_moduleData[projectileModule].onCooldown)
				{
					if (!this.UsesRechargeLikeActiveItem || this.m_remainingActiveCooldownAmount <= 0f)
					{
						if (Volley.ModulesAreTiers)
						{
							if (projectileModule.IsDuctTapeModule)
							{
								flag4 = true;
							}
							else
							{
								int num = ((projectileModule.CloneSourceIndex < 0) ? i : projectileModule.CloneSourceIndex);
								if (num == this.m_currentStrengthTier)
								{
									flag = true;
									flag4 = true;
								}
								else
								{
									flag = false;
									flag4 = false;
								}
							}
						}
						if (projectileModule.isExternalAddedModule)
						{
							flag = false;
						}
						if (flag4)
						{
							flag3 |= this.HandleSpecificContinueGunShoot(projectileModule, canAttack, overrideProjectileData, flag);
						}
						else if ((projectileModule.shootStyle == ProjectileModule.ShootStyle.Automatic || projectileModule.shootStyle == ProjectileModule.ShootStyle.Burst) && !this.m_cachedIsGunBlocked && canAttack)
						{
							this.IncrementModuleFireCountAndMarkReload(projectileModule, null);
						}
						if (flag3)
						{
							flag = false;
						}
					}
				}
			}
		}
		if (!flag2)
		{
			Debug.LogError("Attempting to continue fire without being loaded. This should never happen.");
		}
		return flag3;
	}

	// Token: 0x06006EA1 RID: 28321 RVA: 0x002BBE4C File Offset: 0x002BA04C
	private bool HandleSpecificContinueGunShoot(ProjectileModule module, bool canAttack = true, ProjectileData overrideProjectileData = null, bool playEffects = true)
	{
		if (module.shootStyle == ProjectileModule.ShootStyle.Automatic || module.shootStyle == ProjectileModule.ShootStyle.Burst)
		{
			if (this.m_cachedIsGunBlocked)
			{
				return false;
			}
			if (!canAttack)
			{
				return false;
			}
			if (module.shootStyle == ProjectileModule.ShootStyle.Burst && this.m_moduleData[module].numberShotsFiredThisBurst >= module.burstShotCount)
			{
				this.m_moduleData[module].numberShotsFiredThisBurst = 0;
				if (this.OnBurstContinued != null)
				{
					this.OnBurstContinued(this.CurrentOwner as PlayerController, this);
				}
			}
			if (playEffects)
			{
				if (!this.usesContinuousFireAnimation)
				{
					string text = ((string.IsNullOrEmpty(this.finalShootAnimation) || !module.IsFinalShot(this.m_moduleData[module], this.CurrentOwner)) ? this.shootAnimation : this.finalShootAnimation);
					this.Play(text);
				}
				this.HandleShootEffects(module);
				if (this.doesScreenShake)
				{
					this.DoScreenShake();
				}
			}
			if (playEffects || (module.runtimeGuid != null && this.AdditionalShootSoundsByModule.ContainsKey(module.runtimeGuid)))
			{
				if (module.runtimeGuid != null && this.AdditionalShootSoundsByModule.ContainsKey(module.runtimeGuid))
				{
					AkSoundEngine.SetSwitch("WPN_Guns", this.AdditionalShootSoundsByModule[module.runtimeGuid], base.gameObject);
				}
				if (GameManager.AUDIO_ENABLED && (!this.isAudioLoop || !this.m_isAudioLooping))
				{
					string text2 = ((!module.IsFinalShot(this.m_moduleData[module], this.m_owner) || this.OverrideFinaleAudio) ? "Play_WPN_gun_shot_01" : "Play_WPN_gun_finale_01");
					if (!this.PreventNormalFireAudio)
					{
						AkSoundEngine.PostEvent(text2, base.gameObject);
					}
					this.m_isAudioLooping = true;
				}
				if (!string.IsNullOrEmpty(this.gunSwitchGroup))
				{
					AkSoundEngine.SetSwitch("WPN_Guns", this.gunSwitchGroup, base.gameObject);
				}
			}
			this.ShootSingleProjectile(module, overrideProjectileData, null);
			this.DecrementAmmoCost(module);
			this.TriggerModuleCooldown(module);
			return true;
		}
		else if (module.shootStyle == ProjectileModule.ShootStyle.Charged)
		{
			ModuleShootData moduleShootData = this.m_moduleData[module];
			if (moduleShootData.chargeFired)
			{
				return true;
			}
			float num = 1f;
			if (this.m_owner is PlayerController)
			{
				num = (this.m_owner as PlayerController).stats.GetStatValue(PlayerStats.StatType.ChargeAmountMultiplier);
			}
			moduleShootData.chargeTime += BraveTime.DeltaTime * num;
			if (module.maxChargeTime > 0f && moduleShootData.chargeTime >= module.maxChargeTime && canAttack && !this.m_cachedIsGunBlocked)
			{
				if (playEffects)
				{
					if (!this.usesContinuousFireAnimation)
					{
						this.Play(this.shootAnimation);
					}
					this.HandleShootEffects(module);
					if (moduleShootData.lastChargeProjectile != null)
					{
						if (GameManager.AUDIO_ENABLED)
						{
							int num2 = module.chargeProjectiles.IndexOf(moduleShootData.lastChargeProjectile);
							string text3 = ((!module.IsFinalShot(this.m_moduleData[module], this.m_owner) || this.OverrideFinaleAudio) ? "Play_WPN_gun_shot_" : "Play_WPN_gun_finale_");
							if (GameManager.AUDIO_ENABLED && (!this.isAudioLoop || !this.m_isAudioLooping))
							{
								AkSoundEngine.PostEvent(string.Format("{0}{1:D2}", text3, num2 + 1), base.gameObject);
								this.m_isAudioLooping = true;
							}
							if (moduleShootData.lastChargeProjectile.UsesAdditionalWwiseEvent)
							{
								AkSoundEngine.PostEvent(moduleShootData.lastChargeProjectile.AdditionalWwiseEvent, base.gameObject);
							}
						}
						this.HandleChargeEffects(moduleShootData.lastChargeProjectile, null);
						this.EndChargeIntensity();
						moduleShootData.lastChargeProjectile = null;
					}
					if (this.doesScreenShake)
					{
						this.DoScreenShake();
					}
				}
				this.ShootSingleProjectile(module, overrideProjectileData, null);
				this.DecrementAmmoCost(module);
				this.TriggerModuleCooldown(module);
				moduleShootData.chargeFired = true;
				return true;
			}
			if (playEffects)
			{
				ProjectileModule.ChargeProjectile chargeProjectile = module.GetChargeProjectile(moduleShootData.chargeTime);
				this.PlayIfExistsAndNotPlaying(this.chargeAnimation);
				if (chargeProjectile != moduleShootData.lastChargeProjectile)
				{
					if (GameManager.AUDIO_ENABLED)
					{
						int num3 = module.chargeProjectiles.IndexOf(chargeProjectile);
						if (GameManager.AUDIO_ENABLED)
						{
							AkSoundEngine.PostEvent(string.Format("Play_WPN_gun_charge_{0:D2}", num3 + 2), base.gameObject);
						}
					}
					this.HandleChargeEffects(moduleShootData.lastChargeProjectile, chargeProjectile);
					moduleShootData.lastChargeProjectile = chargeProjectile;
				}
				this.HandleChargeIntensity(module, moduleShootData);
				if (this.CurrentOwner is PlayerController)
				{
					bool flag = chargeProjectile != null && chargeProjectile.Projectile;
					(this.CurrentOwner as PlayerController).DoSustainedVibration((!flag) ? Vibration.Strength.UltraLight : Vibration.Strength.Light);
				}
			}
			return false;
		}
		else
		{
			if (module.shootStyle != ProjectileModule.ShootStyle.Beam)
			{
				return false;
			}
			if (this.m_cachedIsGunBlocked)
			{
				return false;
			}
			ModuleShootData moduleShootData2 = this.m_moduleData[module];
			if (canAttack && !this.m_activeBeams.Contains(moduleShootData2))
			{
				this.HandleSpecificInitialGunShoot(module, overrideProjectileData, null, playEffects);
			}
			else if (moduleShootData2 != null && moduleShootData2.beam)
			{
				BeamController beam = moduleShootData2.beam;
				beam.Direction = this.GetBeamAimDirection(moduleShootData2.angleForShot);
				beam.Origin = this.m_unroundedBarrelPosition;
				if (beam.knocksShooterBack && moduleShootData2.beamKnockbackID >= 0)
				{
					this.m_owner.knockbackDoer.UpdateContinuousKnockback(-beam.Direction, beam.knockbackStrength, moduleShootData2.beamKnockbackID);
				}
				if (beam.ShouldUseAmmo)
				{
					float num4 = ((!(this.m_owner is PlayerController)) ? 1f : (this.m_owner as PlayerController).stats.GetStatValue(PlayerStats.StatType.RateOfFire));
					this.m_fractionalAmmoUsage += BraveTime.DeltaTime * (float)module.ammoCost * num4;
					if (this.m_fractionalAmmoUsage > 1f)
					{
						this.ammo = Math.Max(0, this.ammo - (int)(this.m_fractionalAmmoUsage / 1f));
						if (module.numberOfShotsInClip > 0)
						{
							moduleShootData2.numberShotsFired += (int)(this.m_fractionalAmmoUsage / 1f);
							if (module.GetModNumberOfShotsInClip(this.CurrentOwner) > 0 && moduleShootData2.numberShotsFired >= module.GetModNumberOfShotsInClip(this.CurrentOwner))
							{
								moduleShootData2.needsReload = true;
							}
						}
						this.DecrementCustomAmmunitions((int)(this.m_fractionalAmmoUsage / 1f));
						this.m_fractionalAmmoUsage %= 1f;
					}
				}
			}
			return true;
		}
	}

	// Token: 0x06006EA2 RID: 28322 RVA: 0x002BC51C File Offset: 0x002BA71C
	private bool HandleEndGunShoot(ProjectileModule module, bool canAttack = true, ProjectileData overrideProjectileData = null)
	{
		return !this.m_moduleData[module].needsReload && !this.m_moduleData[module].onCooldown && (!this.UsesRechargeLikeActiveItem || this.m_remainingActiveCooldownAmount <= 0f) && this.HandleSpecificEndGunShoot(module, canAttack, overrideProjectileData, true);
	}

	// Token: 0x06006EA3 RID: 28323 RVA: 0x002BC580 File Offset: 0x002BA780
	private bool HandleEndGunShoot(ProjectileVolleyData Volley, bool canAttack = true, ProjectileData overrideProjectileData = null)
	{
		bool flag = true;
		bool flag2 = false;
		foreach (ProjectileModule projectileModule in Volley.projectiles)
		{
			if (!this.m_moduleData[projectileModule].needsReload)
			{
				if (!this.m_moduleData[projectileModule].onCooldown)
				{
					if (!this.UsesRechargeLikeActiveItem || this.m_remainingActiveCooldownAmount <= 0f)
					{
						flag2 |= this.HandleSpecificEndGunShoot(projectileModule, canAttack, overrideProjectileData, flag);
						if (flag2)
						{
							flag = false;
						}
					}
				}
			}
		}
		return flag2;
	}

	// Token: 0x06006EA4 RID: 28324 RVA: 0x002BC648 File Offset: 0x002BA848
	private bool HandleSpecificEndGunShoot(ProjectileModule module, bool canAttack = true, ProjectileData overrideProjectileData = null, bool playEffects = true)
	{
		if (module.shootStyle == ProjectileModule.ShootStyle.Charged)
		{
			ModuleShootData moduleShootData = this.m_moduleData[module];
			if (!moduleShootData.chargeFired)
			{
				float num = 1f;
				if (this.m_owner is PlayerController)
				{
					num = (this.m_owner as PlayerController).stats.GetStatValue(PlayerStats.StatType.ChargeAmountMultiplier);
				}
				moduleShootData.chargeTime += BraveTime.DeltaTime * num;
				ProjectileModule.ChargeProjectile chargeProjectile = module.GetChargeProjectile(moduleShootData.chargeTime);
				if (chargeProjectile != null && chargeProjectile.Projectile != null && canAttack && !this.m_cachedIsGunBlocked)
				{
					if (playEffects)
					{
						if (!this.usesContinuousFireAnimation)
						{
							this.HandleShootAnimation(module);
						}
						if (GameManager.AUDIO_ENABLED)
						{
							int num2 = module.chargeProjectiles.IndexOf(moduleShootData.lastChargeProjectile);
							string text = ((!module.IsFinalShot(this.m_moduleData[module], this.m_owner) || this.OverrideFinaleAudio) ? "Play_WPN_gun_shot_" : "Play_WPN_gun_finale_");
							if (GameManager.AUDIO_ENABLED && (!this.isAudioLoop || !this.m_isAudioLooping) && !this.PreventNormalFireAudio)
							{
								if (this.PickupObjectId == GlobalItemIds.Starpew && text == "Play_WPN_gun_shot_" && moduleShootData.chargeTime >= 2f)
								{
									AkSoundEngine.PostEvent("Play_WPN_Starpew_Blast_01", base.gameObject);
								}
								else
								{
									AkSoundEngine.PostEvent(string.Format("{0}{1:D2}", text, num2 + 1), base.gameObject);
								}
								this.m_isAudioLooping = true;
							}
							if (moduleShootData.lastChargeProjectile != null && moduleShootData.lastChargeProjectile.UsesAdditionalWwiseEvent)
							{
								AkSoundEngine.PostEvent(moduleShootData.lastChargeProjectile.AdditionalWwiseEvent, base.gameObject);
							}
						}
						this.HandleShootEffects(module);
						if (moduleShootData.lastChargeProjectile != null)
						{
							this.HandleChargeEffects(moduleShootData.lastChargeProjectile, null);
							this.EndChargeIntensity();
							moduleShootData.lastChargeProjectile = null;
						}
						if (this.doesScreenShake)
						{
							this.DoScreenShake();
						}
					}
					else if (moduleShootData.lastChargeProjectile != null)
					{
						this.HandleChargeEffects(moduleShootData.lastChargeProjectile, null);
						this.EndChargeIntensity();
						moduleShootData.lastChargeProjectile = null;
					}
					this.ShootSingleProjectile(module, overrideProjectileData, null);
					this.DecrementAmmoCost(module);
					this.TriggerModuleCooldown(module);
					moduleShootData.chargeFired = true;
					return true;
				}
				if (playEffects)
				{
					if (!string.IsNullOrEmpty(this.dischargeAnimation))
					{
						this.Play(this.dischargeAnimation);
					}
					else
					{
						this.PlayIdleAnimation();
					}
					if (moduleShootData.lastChargeProjectile != null)
					{
						this.HandleChargeEffects(moduleShootData.lastChargeProjectile, null);
						this.EndChargeIntensity();
						moduleShootData.lastChargeProjectile = null;
					}
				}
				if (module.triggerCooldownForAnyChargeAmount)
				{
					this.TriggerModuleCooldown(module);
				}
				return false;
			}
		}
		else if (module.shootStyle == ProjectileModule.ShootStyle.Beam)
		{
			if (playEffects)
			{
				this.PlayIdleAnimation();
			}
			ModuleShootData moduleShootData2 = this.m_moduleData[module];
			if (moduleShootData2.beam)
			{
				if (moduleShootData2.beam.knocksShooterBack && moduleShootData2.beamKnockbackID >= 0)
				{
					this.m_owner.knockbackDoer.EndContinuousKnockback(moduleShootData2.beamKnockbackID);
					moduleShootData2.beamKnockbackID = -1;
				}
				if (this.doesScreenShake)
				{
					GameManager.Instance.MainCameraController.StopContinuousScreenShake(this);
				}
				moduleShootData2.beam.CeaseAttack();
				moduleShootData2.beam = null;
				this.m_activeBeams.Remove(moduleShootData2);
			}
			return true;
		}
		return false;
	}

	// Token: 0x06006EA5 RID: 28325 RVA: 0x002BC9D4 File Offset: 0x002BABD4
	public void ForceFireProjectile(Projectile targetProjectile)
	{
		ProjectileModule projectileModule = null;
		if (this.Volley != null)
		{
			for (int i = 0; i < this.Volley.projectiles.Count; i++)
			{
				for (int j = 0; j < this.Volley.projectiles[j].projectiles.Count; j++)
				{
					if (targetProjectile.name.Contains(this.Volley.projectiles[j].projectiles[j].name))
					{
						projectileModule = this.Volley.projectiles[j];
						break;
					}
				}
				if (projectileModule != null)
				{
					break;
				}
			}
		}
		else
		{
			for (int k = 0; k < this.singleModule.projectiles.Count; k++)
			{
				if (targetProjectile.name.Contains(this.singleModule.projectiles[k].name))
				{
					projectileModule = this.singleModule;
					break;
				}
			}
		}
		if (projectileModule != null)
		{
			this.ShootSingleProjectile(projectileModule, null, null);
		}
	}

	// Token: 0x06006EA6 RID: 28326 RVA: 0x002BCAFC File Offset: 0x002BACFC
	private void DecrementCustomAmmunitions(int ammoCost)
	{
		for (int i = 0; i < this.m_customAmmunitions.Count; i++)
		{
			this.m_customAmmunitions[i].ShotsRemaining -= ammoCost;
			if (this.m_customAmmunitions[i].ShotsRemaining <= 0)
			{
				this.m_customAmmunitions.RemoveAt(i);
				i--;
			}
		}
	}

	// Token: 0x06006EA7 RID: 28327 RVA: 0x002BCB68 File Offset: 0x002BAD68
	private void ApplyCustomAmmunitionsToProjectile(Projectile target)
	{
		for (int i = 0; i < this.m_customAmmunitions.Count; i++)
		{
			ActiveAmmunitionData activeAmmunitionData = this.m_customAmmunitions[i];
			activeAmmunitionData.HandleAmmunition(target, this);
		}
	}

	// Token: 0x06006EA8 RID: 28328 RVA: 0x002BCBA8 File Offset: 0x002BADA8
	private void ShootSingleProjectile(ProjectileModule mod, ProjectileData overrideProjectileData = null, GameObject overrideBulletObject = null)
	{
		PlayerController playerController = this.m_owner as PlayerController;
		AIActor aiactor = this.m_owner as AIActor;
		Projectile projectile = null;
		ProjectileModule.ChargeProjectile chargeProjectile = null;
		if (overrideBulletObject)
		{
			projectile = overrideBulletObject.GetComponent<Projectile>();
		}
		else if (mod.shootStyle == ProjectileModule.ShootStyle.Charged)
		{
			chargeProjectile = mod.GetChargeProjectile(this.m_moduleData[mod].chargeTime);
			if (chargeProjectile != null)
			{
				projectile = chargeProjectile.Projectile;
				projectile.pierceMinorBreakables = true;
			}
		}
		else
		{
			projectile = mod.GetCurrentProjectile(this.m_moduleData[mod], this.CurrentOwner);
		}
		if (!projectile)
		{
			this.m_moduleData[mod].numberShotsFired++;
			this.m_moduleData[mod].numberShotsFiredThisBurst++;
			if (this.m_moduleData[mod].numberShotsActiveReload > 0)
			{
				this.m_moduleData[mod].numberShotsActiveReload--;
			}
			if (mod.GetModNumberOfShotsInClip(this.CurrentOwner) > 0 && this.m_moduleData[mod].numberShotsFired >= mod.GetModNumberOfShotsInClip(this.CurrentOwner))
			{
				this.m_moduleData[mod].needsReload = true;
			}
			if (mod.shootStyle != ProjectileModule.ShootStyle.Charged)
			{
				mod.IncrementShootCount();
			}
			return;
		}
		if (playerController && playerController.OnPreFireProjectileModifier != null)
		{
			projectile = playerController.OnPreFireProjectileModifier(this, projectile);
		}
		if (this.m_isCritting && this.CriticalReplacementProjectile)
		{
			projectile = this.CriticalReplacementProjectile;
		}
		if (this.OnPreFireProjectileModifier != null)
		{
			projectile = this.OnPreFireProjectileModifier(this, projectile, mod);
		}
		if (GameManager.Instance.InTutorial && playerController != null)
		{
			GameManager.BroadcastRoomTalkDoerFsmEvent("playerFiredGun");
		}
		Vector3 vector = this.barrelOffset.position;
		vector = new Vector3(vector.x, vector.y, -1f);
		float num = ((!(playerController != null)) ? 1f : playerController.stats.GetStatValue(PlayerStats.StatType.Accuracy));
		num = ((!(this.m_owner is DumbGunShooter) || !(this.m_owner as DumbGunShooter).overridesInaccuracy) ? num : (this.m_owner as DumbGunShooter).inaccuracyFraction);
		float num2 = mod.GetAngleForShot(this.m_moduleData[mod].alternateAngleSign, num, null);
		if (this.m_moduleData[mod].numberShotsActiveReload > 0 && this.activeReloadData.usesOverrideAngleVariance)
		{
			ProjectileModule projectileModule = mod;
			float num3 = num;
			num2 = projectileModule.GetAngleForShot(1f, num3, new float?(this.activeReloadData.overrideAngleVariance));
		}
		if (mod.alternateAngle)
		{
			this.m_moduleData[mod].alternateAngleSign *= -1f;
		}
		if (this.LockedHorizontalOnCharge && this.LockedHorizontalCenterFireOffset >= 0f)
		{
			vector = this.m_owner.specRigidbody.HitboxPixelCollider.UnitCenter + BraveMathCollege.DegreesToVector(this.gunAngle, this.LockedHorizontalCenterFireOffset);
		}
		GameObject gameObject = SpawnManager.SpawnProjectile(projectile.gameObject, vector + Quaternion.Euler(0f, 0f, this.gunAngle) * mod.positionOffset, Quaternion.Euler(0f, 0f, this.gunAngle + num2), true);
		Projectile component = gameObject.GetComponent<Projectile>();
		this.LastProjectile = component;
		component.Owner = this.m_owner;
		component.Shooter = this.m_owner.specRigidbody;
		component.baseData.damage += (float)this.damageModifier;
		component.Inverted = mod.inverted;
		if (this.m_owner is PlayerController && (this.LocalActiveReload || (playerController.IsPrimaryPlayer && Gun.ActiveReloadActivated) || (!playerController.IsPrimaryPlayer && Gun.ActiveReloadActivatedPlayerTwo)))
		{
			component.baseData.damage *= this.m_moduleData[mod].activeReloadDamageModifier;
		}
		if (this.m_owner.aiShooter)
		{
			component.collidesWithEnemies = this.m_owner.aiShooter.CanShootOtherEnemies;
		}
		if (this.rampBullets)
		{
			component.Ramp(this.rampStartHeight, this.rampTime);
			TrailController componentInChildren = gameObject.GetComponentInChildren<TrailController>();
			if (componentInChildren)
			{
				componentInChildren.rampHeight = true;
				componentInChildren.rampStartHeight = this.rampStartHeight;
				componentInChildren.rampTime = this.rampTime;
			}
		}
		if (this.m_owner is PlayerController)
		{
			PlayerStats stats = playerController.stats;
			component.baseData.damage *= stats.GetStatValue(PlayerStats.StatType.Damage);
			component.baseData.speed *= stats.GetStatValue(PlayerStats.StatType.ProjectileSpeed);
			component.baseData.force *= stats.GetStatValue(PlayerStats.StatType.KnockbackMultiplier);
			component.baseData.range *= stats.GetStatValue(PlayerStats.StatType.RangeMultiplier);
			if (playerController.inventory.DualWielding)
			{
				component.baseData.damage *= Gun.s_DualWieldFactor;
			}
			if (this.CanSneakAttack && playerController.IsStealthed)
			{
				component.baseData.damage *= this.SneakAttackDamageMultiplier;
			}
			if (this.m_isCritting)
			{
				component.baseData.damage *= this.CriticalDamageMultiplier;
				component.IsCritical = true;
			}
			if (this.UsesBossDamageModifier)
			{
				if (this.CustomBossDamageModifier >= 0f)
				{
					component.BossDamageMultiplier = this.CustomBossDamageModifier;
				}
				else
				{
					component.BossDamageMultiplier = 0.8f;
				}
			}
		}
		if (this.Volley != null && this.Volley.UsesShotgunStyleVelocityRandomizer)
		{
			component.baseData.speed *= this.Volley.GetVolleySpeedMod();
		}
		if (aiactor != null && aiactor.IsBlackPhantom)
		{
			component.baseData.speed *= aiactor.BlackPhantomProperties.BulletSpeedMultiplier;
		}
		if (this.m_moduleData[mod].numberShotsActiveReload > 0)
		{
			if (!this.activeReloadData.ActiveReloadStacks)
			{
				component.baseData.damage *= this.activeReloadData.damageMultiply;
			}
			component.baseData.force *= this.activeReloadData.knockbackMultiply;
		}
		if (overrideProjectileData != null)
		{
			component.baseData.SetAll(overrideProjectileData);
		}
		this.LastShotIndex = this.m_moduleData[mod].numberShotsFired;
		component.PlayerProjectileSourceGameTimeslice = Time.time;
		if (!this.IsMinusOneGun)
		{
			this.ApplyCustomAmmunitionsToProjectile(component);
			if (this.m_owner is PlayerController)
			{
				playerController.DoPostProcessProjectile(component);
			}
			if (this.PostProcessProjectile != null)
			{
				this.PostProcessProjectile(component);
			}
		}
		if (mod.mirror)
		{
			gameObject = SpawnManager.SpawnProjectile(projectile.gameObject, vector + Quaternion.Euler(0f, 0f, this.gunAngle) * mod.InversePositionOffset, Quaternion.Euler(0f, 0f, this.gunAngle - num2), true);
			Projectile component2 = gameObject.GetComponent<Projectile>();
			this.LastProjectile = component2;
			component2.Inverted = true;
			component2.Owner = this.m_owner;
			component2.Shooter = this.m_owner.specRigidbody;
			if (this.m_owner.aiShooter)
			{
				component2.collidesWithEnemies = this.m_owner.aiShooter.CanShootOtherEnemies;
			}
			if (this.rampBullets)
			{
				component2.Ramp(this.rampStartHeight, this.rampTime);
				TrailController componentInChildren2 = gameObject.GetComponentInChildren<TrailController>();
				if (componentInChildren2)
				{
					componentInChildren2.rampHeight = true;
					componentInChildren2.rampStartHeight = this.rampStartHeight;
					componentInChildren2.rampTime = this.rampTime;
				}
			}
			component2.PlayerProjectileSourceGameTimeslice = Time.time;
			if (!this.IsMinusOneGun)
			{
				this.ApplyCustomAmmunitionsToProjectile(component2);
				if (this.m_owner is PlayerController)
				{
					playerController.DoPostProcessProjectile(component2);
				}
				if (this.PostProcessProjectile != null)
				{
					this.PostProcessProjectile(component2);
				}
			}
			component2.baseData.SetAll(component.baseData);
			component2.IsCritical = component.IsCritical;
		}
		if (this.modifiedFinalVolley != null && mod == this.modifiedFinalVolley.projectiles[0])
		{
			mod = this.DefaultModule;
		}
		if (chargeProjectile != null && chargeProjectile.ReflectsIncomingBullets && this.barrelOffset)
		{
			if (chargeProjectile.MegaReflection)
			{
				int num4 = PassiveReflectItem.ReflectBulletsInRange(this.barrelOffset.position.XY(), 2.66f, true, this.m_owner, 30f, 1.25f, 1.5f, true);
				if (num4 > 0)
				{
					AkSoundEngine.PostEvent("Play_WPN_duelingpistol_impact_01", base.gameObject);
					AkSoundEngine.PostEvent("Play_PET_junk_punch_01", base.gameObject);
				}
			}
			else
			{
				int num5 = PassiveReflectItem.ReflectBulletsInRange(this.barrelOffset.position.XY(), 2.66f, true, this.m_owner, 30f, 1f, 1f, true);
				if (num5 > 0)
				{
					AkSoundEngine.PostEvent("Play_WPN_duelingpistol_impact_01", base.gameObject);
					AkSoundEngine.PostEvent("Play_PET_junk_punch_01", base.gameObject);
				}
			}
		}
		this.IncrementModuleFireCountAndMarkReload(mod, chargeProjectile);
		if (this.m_owner is PlayerController)
		{
			GameStatsManager.Instance.RegisterStatChange(TrackedStats.BULLETS_FIRED, 1f);
			if (projectile != null && projectile.AppliesKnockbackToPlayer)
			{
				playerController.knockbackDoer.ApplyKnockback(-1f * BraveMathCollege.DegreesToVector(this.gunAngle, 1f), projectile.PlayerKnockbackForce, false);
			}
		}
	}

	// Token: 0x06006EA9 RID: 28329 RVA: 0x002BD620 File Offset: 0x002BB820
	public void TriggerActiveCooldown()
	{
		if (!this.UsesRechargeLikeActiveItem)
		{
			return;
		}
		this.RemainingActiveCooldownAmount = this.ActiveItemStyleRechargeAmount;
	}

	// Token: 0x06006EAA RID: 28330 RVA: 0x002BD63C File Offset: 0x002BB83C
	public void ApplyActiveCooldownDamage(PlayerController Owner, float damageDone)
	{
		if (!this.UsesRechargeLikeActiveItem)
		{
			return;
		}
		if (Owner.CurrentGun == this && !PlayerItem.AllowDamageCooldownOnActive)
		{
			return;
		}
		float num = 1f;
		GameLevelDefinition lastLoadedLevelDefinition = GameManager.Instance.GetLastLoadedLevelDefinition();
		if (lastLoadedLevelDefinition != null)
		{
			num /= lastLoadedLevelDefinition.enemyHealthMultiplier;
		}
		damageDone *= num;
		if (this.ModifyActiveCooldownDamage != null)
		{
			damageDone = this.ModifyActiveCooldownDamage(damageDone);
		}
		this.RemainingActiveCooldownAmount = Mathf.Max(0f, this.m_remainingActiveCooldownAmount - damageDone);
	}

	// Token: 0x06006EAB RID: 28331 RVA: 0x002BD6C8 File Offset: 0x002BB8C8
	private void TriggerModuleCooldown(ProjectileModule mod)
	{
		if (this.UsesRechargeLikeActiveItem)
		{
			this.TriggerActiveCooldown();
		}
		GameManager.Instance.StartCoroutine(this.HandleModuleCooldown(mod));
	}

	// Token: 0x06006EAC RID: 28332 RVA: 0x002BD6F0 File Offset: 0x002BB8F0
	private IEnumerator HandleModuleCooldown(ProjectileModule mod)
	{
		this.m_moduleData[mod].onCooldown = true;
		float elapsed = 0f;
		float fireMultiplier = ((!(this.m_owner is PlayerController)) ? 1f : (this.m_owner as PlayerController).stats.GetStatValue(PlayerStats.StatType.RateOfFire));
		if (this.GainsRateOfFireAsContinueAttack)
		{
			float num = this.RateOfFireMultiplierAdditionPerSecond * this.m_continuousAttackTime;
			fireMultiplier += num;
		}
		float cooldownTime;
		if (mod.shootStyle == ProjectileModule.ShootStyle.Burst && this.m_moduleData[mod].numberShotsFiredThisBurst < mod.burstShotCount)
		{
			cooldownTime = mod.burstCooldownTime;
		}
		else
		{
			cooldownTime = mod.cooldownTime + this.gunCooldownModifier;
		}
		cooldownTime *= 1f / fireMultiplier;
		while (elapsed < cooldownTime)
		{
			elapsed += BraveTime.DeltaTime;
			yield return null;
		}
		if (this.m_moduleData != null && this.m_moduleData.ContainsKey(mod))
		{
			this.m_moduleData[mod].onCooldown = false;
			this.m_moduleData[mod].chargeTime = 0f;
			this.m_moduleData[mod].chargeFired = false;
		}
		yield break;
	}

	// Token: 0x06006EAD RID: 28333 RVA: 0x002BD714 File Offset: 0x002BB914
	private void BeginFiringBeam(ProjectileModule mod)
	{
		GameObject gameObject = SpawnManager.SpawnProjectile(mod.GetCurrentProjectile(this.m_moduleData[mod], this.CurrentOwner).gameObject, this.m_unroundedBarrelPosition, Quaternion.identity, true);
		Projectile component = gameObject.GetComponent<Projectile>();
		component.Owner = this.CurrentOwner;
		this.LastProjectile = component;
		BeamController component2 = gameObject.GetComponent<BeamController>();
		component2.Owner = this.m_owner;
		component2.Gun = this;
		component2.HitsPlayers = this.m_owner is AIActor;
		component2.HitsEnemies = this.m_owner is PlayerController;
		if (this.m_owner is PlayerController)
		{
			PlayerStats stats = (this.m_owner as PlayerController).stats;
			component.baseData.damage *= stats.GetStatValue(PlayerStats.StatType.Damage);
			component.baseData.speed *= stats.GetStatValue(PlayerStats.StatType.ProjectileSpeed);
			component.baseData.force *= stats.GetStatValue(PlayerStats.StatType.KnockbackMultiplier);
			component.baseData.range *= stats.GetStatValue(PlayerStats.StatType.RangeMultiplier);
			if ((this.m_owner as PlayerController).inventory.DualWielding)
			{
				component.baseData.damage *= Gun.s_DualWieldFactor;
			}
			if (this.UsesBossDamageModifier)
			{
				if (this.CustomBossDamageModifier >= 0f)
				{
					component.BossDamageMultiplier = this.CustomBossDamageModifier;
				}
				else
				{
					component.BossDamageMultiplier = 0.8f;
				}
			}
		}
		if (this.doesScreenShake && GameManager.Instance.MainCameraController != null)
		{
			GameManager.Instance.MainCameraController.DoContinuousScreenShake(this.gunScreenShake, this, false);
		}
		float num = ((!(this.m_owner is PlayerController)) ? 1f : (this.m_owner as PlayerController).stats.GetStatValue(PlayerStats.StatType.Accuracy));
		float angleForShot = mod.GetAngleForShot(this.m_moduleData[mod].alternateAngleSign, num, null);
		Vector3 beamAimDirection = this.GetBeamAimDirection(angleForShot);
		component2.Direction = beamAimDirection;
		component2.Origin = this.m_unroundedBarrelPosition;
		ModuleShootData moduleShootData = this.m_moduleData[mod];
		moduleShootData.beam = component2;
		moduleShootData.angleForShot = angleForShot;
		KnockbackDoer knockbackDoer = this.m_owner.knockbackDoer;
		moduleShootData.beamKnockbackID = -1;
		if (component2.knocksShooterBack)
		{
			moduleShootData.beamKnockbackID = knockbackDoer.ApplyContinuousKnockback(-beamAimDirection, component2.knockbackStrength);
		}
		this.m_activeBeams.Add(moduleShootData);
	}

	// Token: 0x06006EAE RID: 28334 RVA: 0x002BD9C0 File Offset: 0x002BBBC0
	private void ClearBeams()
	{
		if (this.m_activeBeams.Count > 0)
		{
			for (int i = 0; i < this.m_activeBeams.Count; i++)
			{
				BeamController beam = this.m_activeBeams[i].beam;
				if (beam && beam.knocksShooterBack)
				{
					this.m_owner.knockbackDoer.EndContinuousKnockback(this.m_activeBeams[i].beamKnockbackID);
					this.m_activeBeams[i].beamKnockbackID = -1;
				}
				if (this.doesScreenShake && GameManager.Instance.MainCameraController != null)
				{
					GameManager.Instance.MainCameraController.StopContinuousScreenShake(this);
				}
				if (beam)
				{
					beam.CeaseAttack();
				}
			}
			this.m_activeBeams.Clear();
			if (GameManager.AUDIO_ENABLED)
			{
				AkSoundEngine.PostEvent("Stop_WPN_gun_loop_01", base.gameObject);
			}
			this.m_isAudioLooping = false;
		}
	}

	// Token: 0x06006EAF RID: 28335 RVA: 0x002BDAC4 File Offset: 0x002BBCC4
	public void ForceImmediateReload(bool forceImmediate = false)
	{
		if (base.gameObject.activeSelf)
		{
			this.ClearBeams();
		}
		if (this.IsReloading)
		{
			this.FinishReload(false, false, forceImmediate);
		}
		else if (this.HaveAmmoToReloadWith())
		{
			this.FinishReload(false, true, forceImmediate);
		}
	}

	// Token: 0x06006EB0 RID: 28336 RVA: 0x002BDB14 File Offset: 0x002BBD14
	private void OnActiveReloadPressed(PlayerController p, Gun g, bool actualPress)
	{
		if (this.m_isReloading || this.reloadTime < 0f)
		{
			PlayerController playerController = this.m_owner as PlayerController;
			if (playerController && (actualPress || true))
			{
				bool flag = this.LocalActiveReload || (playerController.IsPrimaryPlayer && Gun.ActiveReloadActivated) || (!playerController.IsPrimaryPlayer && Gun.ActiveReloadActivatedPlayerTwo);
				if (flag && this.m_canAttemptActiveReload && !GameUIRoot.Instance.GetReloadBarForPlayer(this.m_owner as PlayerController).IsActiveReloadGracePeriod())
				{
					bool flag2 = GameUIRoot.Instance.AttemptActiveReload(this.m_owner as PlayerController);
					if (flag2)
					{
						this.OnActiveReloadSuccess();
						GunFormeSynergyProcessor component = base.GetComponent<GunFormeSynergyProcessor>();
						if (component)
						{
							component.JustActiveReloaded = true;
						}
						ChamberGunProcessor component2 = base.GetComponent<ChamberGunProcessor>();
						if (component2)
						{
							component2.JustActiveReloaded = true;
						}
					}
					this.m_canAttemptActiveReload = false;
					this.OnReloadPressed = (Action<PlayerController, Gun, bool>)Delegate.Remove(this.OnReloadPressed, new Action<PlayerController, Gun, bool>(this.OnActiveReloadPressed));
				}
			}
		}
	}

	// Token: 0x06006EB1 RID: 28337 RVA: 0x002BDC48 File Offset: 0x002BBE48
	private bool ReloadIsFree()
	{
		return this.GoopReloadsFree && this.m_owner.CurrentGoop != null;
	}

	// Token: 0x06006EB2 RID: 28338 RVA: 0x002BDC70 File Offset: 0x002BBE70
	public bool Reload()
	{
		if (this.IsHeroSword && !this.HeroSwordDoesntBlank && !this.m_isCurrentlyFiring && !this.m_anim.IsPlaying(this.reloadAnimation))
		{
			Vector2 unitCenter = this.m_owner.specRigidbody.GetUnitCenter(ColliderType.HitBox);
			SilencerInstance.DestroyBulletsInRange(unitCenter, this.blankReloadRadius, true, false, null, false, null, false, null);
			this.Play(this.reloadAnimation);
			return false;
		}
		this.m_continuousAttackTime = 0f;
		this.ClearBurstState();
		if (this.m_isReloading || this.reloadTime < 0f)
		{
			if (this.m_canAttemptActiveReload)
			{
				this.OnActiveReloadPressed(this.m_owner as PlayerController, this, true);
			}
			return false;
		}
		bool flag = false;
		bool flag2 = this.ReloadIsFree();
		if (this.Volley != null)
		{
			for (int i = 0; i < this.Volley.projectiles.Count; i++)
			{
				if (this.m_moduleData[this.Volley.projectiles[i]].numberShotsFired != 0)
				{
					flag = true;
					break;
				}
			}
			if (this.ammo == 0 && !flag2)
			{
				flag = false;
			}
		}
		else
		{
			if (this.m_moduleData[this.singleModule].numberShotsFired != 0)
			{
				flag = true;
			}
			if (this.ClipShotsRemaining == this.ammo && !flag2)
			{
				flag = false;
			}
		}
		if (flag)
		{
			flag = flag2 || this.HaveAmmoToReloadWith();
		}
		if (flag2)
		{
			this.GainAmmo(Mathf.Max(0, this.ClipCapacity - this.ClipShotsRemaining));
			DeadlyDeadlyGoopManager.DelayedClearGoopsInRadius(this.m_owner.CenterPosition, 2f);
		}
		if (flag)
		{
			if (!this.m_isReloading && this.IsCharging)
			{
				this.CeaseAttack(false, null);
			}
			this.m_isReloading = true;
			this.m_canAttemptActiveReload = true;
			this.m_reloadElapsed = 0f;
			this.m_hasDoneSingleReloadBlankEffect = false;
			this.OnReloadPressed = (Action<PlayerController, Gun, bool>)Delegate.Combine(this.OnReloadPressed, new Action<PlayerController, Gun, bool>(this.OnActiveReloadPressed));
			if (this.ClipShotsRemaining == 0 && this.OnAutoReload != null)
			{
				this.OnAutoReload(this.CurrentOwner as PlayerController, this);
			}
			if (GameManager.AUDIO_ENABLED)
			{
				AkSoundEngine.PostEvent("Play_WPN_gun_reload_01", base.gameObject);
			}
			if (this.reloadOffset)
			{
				float num = ((!this.m_owner.SpriteFlipped) ? (this.gunAngle + this.reloadOffset.transform.localEulerAngles.z) : (this.gunAngle - 180f - this.reloadOffset.transform.localEulerAngles.z));
				this.reloadOffset.localScale = Vector3.one;
				VFXPool vfxpool = ((!this.IsEmpty || this.emptyReloadEffects.type == VFXPoolType.None) ? this.reloadEffects : this.emptyReloadEffects);
				vfxpool.SpawnAtPosition(this.reloadOffset.position, num, this.reloadOffset, new Vector2?(Vector2.zero), new Vector2?(Vector2.zero), new float?(0.0375f), true, null, null, false);
				if (this.m_owner.SpriteFlipped)
				{
					this.reloadOffset.localScale = new Vector3(-1f, 1f, 1f);
				}
			}
			if (this.m_owner is PlayerController)
			{
				PlayerController playerController = this.m_owner as PlayerController;
				if (this.OptionalReloadVolley != null)
				{
					this.RawFireVolley(this.OptionalReloadVolley);
					this.ClearOptionalReloadVolleyCooldownAndReloadData();
				}
				if (playerController.OnReloadedGun != null)
				{
					playerController.OnReloadedGun(playerController, this);
				}
				if (this.ObjectToInstantiateOnReload)
				{
					this.CreateAmp();
				}
				if (this.AdjustedReloadTime > 0.1f)
				{
					Vector3 vector = new Vector3(0.1f, this.m_owner.SpriteDimensions.y / 2f + 0.25f, 0f);
					GameUIRoot.Instance.StartPlayerReloadBar(playerController, vector, this.AdjustedReloadTime);
				}
			}
			if (this.m_isReloading)
			{
				if (this.AdjustedReloadTime > 0f)
				{
					base.StartCoroutine(this.HandleReload());
				}
				else
				{
					this.FinishReload(false, false, true);
				}
			}
			this.m_reloadWhenDoneFiring = false;
			return true;
		}
		return false;
	}

	// Token: 0x06006EB3 RID: 28339 RVA: 0x002BE10C File Offset: 0x002BC30C
	public void HandleDodgeroll(float fullDodgeTime)
	{
		if (!string.IsNullOrEmpty(this.dodgeAnimation))
		{
			if (this.usesDirectionalAnimator)
			{
				AIAnimator aiAnimator = base.aiAnimator;
				string text = this.dodgeAnimation;
				aiAnimator.PlayUntilFinished(text, false, null, fullDodgeTime, false);
			}
			else if (this.m_anim != null)
			{
				tk2dSpriteAnimationClip clipByName = this.m_anim.GetClipByName(this.dodgeAnimation);
				if (clipByName != null)
				{
					float num = (float)clipByName.frames.Length / fullDodgeTime;
					this.m_anim.Play(clipByName, 0f, num, false);
				}
			}
		}
	}

	// Token: 0x06006EB4 RID: 28340 RVA: 0x002BE19C File Offset: 0x002BC39C
	private void ClearBurstState()
	{
		this.m_midBurstFire = false;
		this.m_continueBurstInUpdate = false;
		if (this.Volley != null)
		{
			for (int i = 0; i < this.Volley.projectiles.Count; i++)
			{
				if (this.Volley.projectiles[i].shootStyle == ProjectileModule.ShootStyle.Burst)
				{
					this.m_moduleData[this.Volley.projectiles[i]].numberShotsFiredThisBurst = 0;
				}
			}
		}
		else if (this.singleModule.shootStyle == ProjectileModule.ShootStyle.Burst)
		{
			this.m_moduleData[this.singleModule].numberShotsFiredThisBurst = 0;
		}
	}

	// Token: 0x06006EB5 RID: 28341 RVA: 0x002BE254 File Offset: 0x002BC454
	private IEnumerator HandleReload()
	{
		this.m_isReloading = true;
		string currentReloadAnim = ((string.IsNullOrEmpty(this.emptyReloadAnimation) || !this.IsEmpty) ? this.reloadAnimation : this.emptyReloadAnimation);
		if (this.IsTrickGun && !this.m_hasSwappedTrickGunsThisCycle)
		{
			this.m_hasSwappedTrickGunsThisCycle = true;
			if (!string.IsNullOrEmpty(this.gunSwitchGroup) && !string.IsNullOrEmpty(this.alternateSwitchGroup))
			{
				BraveUtility.Swap<string>(ref this.gunSwitchGroup, ref this.alternateSwitchGroup);
				AkSoundEngine.SetSwitch("WPN_Guns", this.gunSwitchGroup, base.gameObject);
			}
			tk2dSpriteAnimationClip clipByName = this.m_anim.GetClipByName(currentReloadAnim);
			this.m_defaultSpriteID = clipByName.frames[clipByName.frames.Length - 1].spriteId;
		}
		if (this.Volley != null)
		{
			for (int i = 0; i < this.Volley.projectiles.Count; i++)
			{
				this.m_moduleData[this.Volley.projectiles[i]].needsReload = true;
			}
		}
		float elapsed = 0f;
		if (this.m_anim != null)
		{
			this.PlayIfExists(currentReloadAnim, false);
		}
		bool hasLaunchedShellCasings = false;
		bool hasLaunchedClip = false;
		while (elapsed < this.AdjustedReloadTime)
		{
			elapsed += BraveTime.DeltaTime;
			if (this.shellsToLaunchOnReload > 0 && !hasLaunchedShellCasings && this.m_anim.IsPlaying(currentReloadAnim) && this.m_anim.CurrentFrame == this.reloadShellLaunchFrame)
			{
				for (int j = 0; j < this.shellsToLaunchOnReload; j++)
				{
					this.SpawnShellCasingAtPosition(this.CasingLaunchPoint);
				}
				hasLaunchedShellCasings = true;
			}
			bool animGoForClip = (this.m_anim.IsPlaying(currentReloadAnim) && this.m_anim.CurrentFrame == this.reloadClipLaunchFrame) || this.m_anim.GetClipByName(currentReloadAnim) == null;
			if (this.clipsToLaunchOnReload > 0 && !hasLaunchedClip && animGoForClip)
			{
				for (int k = 0; k < this.clipsToLaunchOnReload; k++)
				{
					this.SpawnClipAtPosition(this.ClipLaunchPoint);
				}
				hasLaunchedClip = true;
			}
			if (this.m_owner is PlayerController)
			{
				this.HandleSpriteFlip(this.m_owner.SpriteFlipped);
			}
			if (!this.m_isReloading)
			{
				break;
			}
			yield return null;
		}
		if (this.m_isReloading)
		{
			this.FinishReload(false, false, false);
		}
		yield break;
	}

	// Token: 0x06006EB6 RID: 28342 RVA: 0x002BE270 File Offset: 0x002BC470
	public void MoveBulletsIntoClip(int numBullets)
	{
		if (this.Volley != null)
		{
			for (int i = 0; i < this.Volley.projectiles.Count; i++)
			{
				int num = Mathf.Min(numBullets, this.m_moduleData[this.Volley.projectiles[i]].numberShotsFired);
				int num2 = this.Volley.projectiles[i].GetModNumberOfShotsInClip(this.CurrentOwner) - this.m_moduleData[this.Volley.projectiles[i]].numberShotsFired;
				num = Mathf.Min(num, this.ammo - num2);
				if (num > 0)
				{
					this.m_moduleData[this.Volley.projectiles[i]].numberShotsFired -= num;
					this.m_moduleData[this.Volley.projectiles[i]].needsReload = false;
				}
			}
		}
		else
		{
			int num3 = Mathf.Min(numBullets, this.m_moduleData[this.singleModule].numberShotsFired);
			int num4 = this.singleModule.GetModNumberOfShotsInClip(this.CurrentOwner) - this.m_moduleData[this.singleModule].numberShotsFired;
			num3 = Mathf.Min(num3, this.ammo - num4);
			if (num3 > 0)
			{
				this.m_moduleData[this.singleModule].numberShotsFired -= num3;
				this.m_moduleData[this.singleModule].needsReload = false;
			}
		}
	}

	// Token: 0x06006EB7 RID: 28343 RVA: 0x002BE410 File Offset: 0x002BC610
	private void FinishReload(bool activeReload = false, bool silent = false, bool isImmediate = false)
	{
		if (isImmediate)
		{
			string text = ((string.IsNullOrEmpty(this.emptyReloadAnimation) || !this.IsEmpty) ? this.reloadAnimation : this.emptyReloadAnimation);
			if (this.IsTrickGun && !this.m_hasSwappedTrickGunsThisCycle)
			{
				this.m_hasSwappedTrickGunsThisCycle = true;
				if (!string.IsNullOrEmpty(this.gunSwitchGroup) && !string.IsNullOrEmpty(this.alternateSwitchGroup))
				{
					BraveUtility.Swap<string>(ref this.gunSwitchGroup, ref this.alternateSwitchGroup);
					AkSoundEngine.SetSwitch("WPN_Guns", this.gunSwitchGroup, base.gameObject);
				}
				tk2dSpriteAnimationClip clipByName = this.m_anim.GetClipByName(text);
				this.m_defaultSpriteID = clipByName.frames[clipByName.frames.Length - 1].spriteId;
			}
		}
		if (!silent)
		{
			if (this.IsTrickGun)
			{
				BraveUtility.Swap<string>(ref this.reloadAnimation, ref this.alternateReloadAnimation);
				BraveUtility.Swap<string>(ref this.shootAnimation, ref this.alternateShootAnimation);
				if (!string.IsNullOrEmpty(this.alternateIdleAnimation))
				{
					BraveUtility.Swap<string>(ref this.idleAnimation, ref this.alternateIdleAnimation);
				}
				BraveUtility.Swap<ProjectileVolleyData>(ref this.rawVolley, ref this.alternateVolley);
				(this.CurrentOwner as PlayerController).stats.RecalculateStats(this.CurrentOwner as PlayerController, false, false);
			}
			if (this.IsTrickGun && this.TrickGunAlternatesHandedness)
			{
				if (this.Handedness == GunHandedness.OneHanded)
				{
					this.m_cachedGunHandedness = new GunHandedness?(GunHandedness.TwoHanded);
					this.carryPixelOffset = new IntVector2(10, 0);
				}
				else if (this.Handedness == GunHandedness.TwoHanded)
				{
					this.m_cachedGunHandedness = new GunHandedness?(GunHandedness.OneHanded);
					this.carryPixelOffset = new IntVector2(0, 0);
				}
				(this.m_owner as PlayerController).ProcessHandAttachment();
			}
		}
		this.m_hasSwappedTrickGunsThisCycle = false;
		this.HasFiredHolsterShot = false;
		this.HasFiredReloadSynergy = false;
		this.OnReloadPressed = (Action<PlayerController, Gun, bool>)Delegate.Remove(this.OnReloadPressed, new Action<PlayerController, Gun, bool>(this.OnActiveReloadPressed));
		if (this.Volley != null)
		{
			for (int i = 0; i < this.Volley.projectiles.Count; i++)
			{
				int num = this.Volley.projectiles[i].GetModNumberOfShotsInClip(this.CurrentOwner) - this.m_moduleData[this.Volley.projectiles[i]].numberShotsFired;
				int num2 = Math.Max(this.Volley.projectiles[i].GetModNumberOfShotsInClip(this.CurrentOwner) - this.ammo, 0);
				this.m_moduleData[this.Volley.projectiles[i]].numberShotsFired = num2;
				this.m_moduleData[this.Volley.projectiles[i]].needsReload = false;
				this.m_moduleData[this.Volley.projectiles[i]].activeReloadDamageModifier = 1f;
				int num3 = this.Volley.projectiles[i].GetModNumberOfShotsInClip(this.CurrentOwner) - num;
				if (activeReload)
				{
					this.m_moduleData[this.Volley.projectiles[i]].numberShotsActiveReload = num3;
				}
			}
		}
		else
		{
			int num4 = this.singleModule.GetModNumberOfShotsInClip(this.CurrentOwner) - this.m_moduleData[this.singleModule].numberShotsFired;
			int num5 = Math.Max(this.singleModule.GetModNumberOfShotsInClip(this.CurrentOwner) - this.ammo, 0);
			this.m_moduleData[this.singleModule].numberShotsFired = num5;
			this.m_moduleData[this.singleModule].needsReload = false;
			this.m_moduleData[this.singleModule].activeReloadDamageModifier = 1f;
			int num6 = this.singleModule.GetModNumberOfShotsInClip(this.CurrentOwner) - num4;
			if (activeReload)
			{
				this.m_moduleData[this.singleModule].numberShotsActiveReload = num6;
			}
		}
		if (!silent)
		{
			this.PlayIdleAnimation();
			this.SequentialActiveReloads = ((!activeReload) ? 0 : (this.SequentialActiveReloads + 1));
			if (this.LocalActiveReload && this.activeReloadData.ActiveReloadStacks)
			{
				if (activeReload)
				{
					if (this.activeReloadData.ActiveReloadIncrementsTier)
					{
						this.CurrentStrengthTier = Mathf.Min(this.CurrentStrengthTier + 1, this.activeReloadData.MaxTier - 1);
					}
					this.AdditionalReloadMultiplier /= this.activeReloadData.reloadSpeedMultiplier;
				}
				else
				{
					if (this.activeReloadData.ActiveReloadIncrementsTier)
					{
						this.CurrentStrengthTier = 0;
					}
					this.AdditionalReloadMultiplier = 1f;
				}
			}
			this.HandleActiveReloadEffects(activeReload);
		}
		this.m_isReloading = false;
	}

	// Token: 0x06006EB8 RID: 28344 RVA: 0x002BE8F4 File Offset: 0x002BCAF4
	private void HandleActiveReloadEffects(bool activeReload)
	{
		if (this.CurrentOwner && this.CurrentOwner.CurrentGun == this)
		{
			VFXPool vfxpool = null;
			if (activeReload)
			{
				if (this.activeReloadSuccessEffects.type != VFXPoolType.None)
				{
					vfxpool = this.activeReloadSuccessEffects;
				}
			}
			else if (this.activeReloadFailedEffects.type != VFXPoolType.None)
			{
				vfxpool = this.activeReloadFailedEffects;
			}
			if (this.CurrentOwner && vfxpool != null)
			{
				vfxpool.SpawnAtPosition(this.CurrentOwner.CenterPosition + new Vector2(0f, 2f), 0f, this.CurrentOwner.transform, new Vector2?(Vector2.zero), new Vector2?(Vector2.zero), new float?(5f), true, null, null, false);
			}
		}
	}

	// Token: 0x06006EB9 RID: 28345 RVA: 0x002BE9D4 File Offset: 0x002BCBD4
	private void PotentialShuffleAmmoForLargeClipGuns()
	{
		bool flag = false;
		int num = 0;
		if (this.Volley != null && this.Volley.projectiles.Count > 1)
		{
			for (int i = 0; i < this.Volley.projectiles.Count; i++)
			{
				if (!this.Volley.projectiles[i].ignoredForReloadPurposes && this.Volley.projectiles[i].GetModNumberOfShotsInClip(this.CurrentOwner) > 100)
				{
					num++;
					flag = true;
				}
			}
		}
		if (num < 2)
		{
			flag = false;
		}
		if (flag)
		{
			for (int j = 0; j < this.Volley.projectiles.Count; j++)
			{
				if (this.Volley.projectiles[j].GetModNumberOfShotsInClip(this.CurrentOwner) > 100)
				{
					int num2 = this.Volley.projectiles[j].GetModNumberOfShotsInClip(this.CurrentOwner) - 100;
					if (this.m_moduleData.ContainsKey(this.Volley.projectiles[j]) && num2 > this.m_moduleData[this.Volley.projectiles[j]].numberShotsFired)
					{
						this.m_moduleData[this.Volley.projectiles[j]].numberShotsFired = num2;
					}
				}
			}
		}
	}

	// Token: 0x06006EBA RID: 28346 RVA: 0x002BEB54 File Offset: 0x002BCD54
	private bool HaveAmmoToReloadWith()
	{
		this.PotentialShuffleAmmoForLargeClipGuns();
		if (this.CanReloadNoMatterAmmo)
		{
			return true;
		}
		if (this.Volley != null)
		{
			for (int i = 0; i < this.Volley.projectiles.Count; i++)
			{
				if (!this.Volley.projectiles[i].ignoredForReloadPurposes)
				{
					if (!this.Volley.projectiles[i].IsDuctTapeModule)
					{
						if (this.Volley.projectiles[i].GetModNumberOfShotsInClip(this.CurrentOwner) - this.m_moduleData[this.Volley.projectiles[i]].numberShotsFired >= this.ammo)
						{
							return false;
						}
					}
				}
			}
		}
		else if (this.singleModule.GetModifiedNumberOfFinalProjectiles(this.CurrentOwner) - this.m_moduleData[this.singleModule].numberShotsFired >= this.ammo)
		{
			return false;
		}
		return true;
	}

	// Token: 0x170010C2 RID: 4290
	// (get) Token: 0x06006EBB RID: 28347 RVA: 0x002BEC6C File Offset: 0x002BCE6C
	public List<ModuleShootData> ActiveBeams
	{
		get
		{
			return this.m_activeBeams;
		}
	}

	// Token: 0x06006EBC RID: 28348 RVA: 0x002BEC74 File Offset: 0x002BCE74
	private Vector3 GetBeamAimDirection(float angleForShot)
	{
		Vector3 vector = Quaternion.Euler(0f, 0f, this.gunAngle) * Vector3.right;
		return Quaternion.Euler(0f, 0f, angleForShot) * vector;
	}

	// Token: 0x06006EBD RID: 28349 RVA: 0x002BECB8 File Offset: 0x002BCEB8
	public void PlayIdleAnimation()
	{
		if (this.m_preventIdleLoop)
		{
			return;
		}
		this.m_preventIdleLoop = true;
		if (!string.IsNullOrEmpty(this.outOfAmmoAnimation) && this.ammo == 0)
		{
			this.Play(this.outOfAmmoAnimation);
		}
		else if (!string.IsNullOrEmpty(this.emptyAnimation) && this.ClipShotsRemaining <= 0)
		{
			this.Play(this.emptyAnimation);
		}
		else
		{
			if (this.m_anim == null)
			{
				this.m_anim = base.GetComponent<tk2dSpriteAnimator>();
			}
			if (this.usesDirectionalIdleAnimations)
			{
				if (this.m_directionalIdleNames == null)
				{
					this.m_directionalIdleNames = new string[]
					{
						this.idleAnimation + "_E",
						this.idleAnimation + "_SE",
						this.idleAnimation + "_S",
						this.idleAnimation + "_SW",
						this.idleAnimation + "_W",
						this.idleAnimation + "_NW",
						this.idleAnimation + "_N",
						this.idleAnimation + "_NE"
					};
				}
				float num = this.gunAngle;
				if (this.CurrentOwner is PlayerController)
				{
					PlayerController playerController = this.CurrentOwner as PlayerController;
					num = BraveMathCollege.Atan2Degrees(playerController.unadjustedAimPoint.XY() - this.m_attachTransform.position.XY());
				}
				int num2 = BraveMathCollege.AngleToOctant(num + 90f);
				if (!this.m_anim.IsPlaying(this.m_directionalIdleNames[num2]))
				{
					this.Play(this.m_directionalIdleNames[num2]);
				}
			}
			else if (!string.IsNullOrEmpty(this.idleAnimation) && this.m_anim.GetClipByName(this.idleAnimation) != null)
			{
				this.Play(this.idleAnimation);
			}
			else
			{
				this.m_anim.Stop();
				this.m_sprite.spriteId = this.m_defaultSpriteID;
			}
		}
		this.m_preventIdleLoop = false;
	}

	// Token: 0x06006EBE RID: 28350 RVA: 0x002BEEE4 File Offset: 0x002BD0E4
	private void DecrementAmmoCost(ProjectileModule module)
	{
		if (this.modifiedFinalVolley != null && module == this.modifiedFinalVolley.projectiles[0])
		{
			module = this.DefaultModule;
		}
		int num = module.ammoCost;
		if (module.shootStyle == ProjectileModule.ShootStyle.Charged)
		{
			ProjectileModule.ChargeProjectile chargeProjectile = module.GetChargeProjectile(this.m_moduleData[module].chargeTime);
			if (chargeProjectile.UsesAmmo)
			{
				num = chargeProjectile.AmmoCost;
			}
		}
		if (this.InfiniteAmmo)
		{
			num = 0;
		}
		if (this.RequiresFundsToShoot && !this.m_hasDecrementedFunds)
		{
			this.m_hasDecrementedFunds = true;
			(this.m_owner as PlayerController).carriedConsumables.Currency -= this.CurrencyCostPerShot;
		}
		this.ammo = Math.Max(0, this.ammo - num);
		this.DecrementCustomAmmunitions(num);
	}

	// Token: 0x170010C3 RID: 4291
	// (get) Token: 0x06006EBF RID: 28351 RVA: 0x002BEFC8 File Offset: 0x002BD1C8
	// (set) Token: 0x06006EC0 RID: 28352 RVA: 0x002BEFD0 File Offset: 0x002BD1D0
	public bool OverrideAnimations { get; set; }

	// Token: 0x06006EC1 RID: 28353 RVA: 0x002BEFDC File Offset: 0x002BD1DC
	private void Play(string animName)
	{
		if (this.OverrideAnimations)
		{
			return;
		}
		if (this.usesDirectionalAnimator)
		{
			base.aiAnimator.PlayUntilFinished(animName, false, null, -1f, false);
			return;
		}
		this.m_anim.Play(animName);
	}

	// Token: 0x06006EC2 RID: 28354 RVA: 0x002BF018 File Offset: 0x002BD218
	private void PlayIfExists(string name, bool restartIfPlaying = false)
	{
		if (this.OverrideAnimations)
		{
			return;
		}
		if (this.usesDirectionalAnimator && base.aiAnimator.HasDirectionalAnimation(name))
		{
			base.aiAnimator.PlayUntilFinished(name, false, null, -1f, false);
			return;
		}
		tk2dSpriteAnimationClip clipByName = this.m_anim.GetClipByName(name);
		if (clipByName != null)
		{
			if (restartIfPlaying && this.m_anim.IsPlaying(name))
			{
				this.m_anim.PlayFromFrame(0);
			}
			else
			{
				this.m_anim.Play(clipByName);
			}
		}
	}

	// Token: 0x06006EC3 RID: 28355 RVA: 0x002BF0AC File Offset: 0x002BD2AC
	private void PlayIfExistsAndNotPlaying(string name)
	{
		if (this.OverrideAnimations)
		{
			return;
		}
		if (this.usesDirectionalAnimator && base.aiAnimator.HasDirectionalAnimation(name) && !base.aiAnimator.IsPlaying(name))
		{
			base.aiAnimator.PlayUntilFinished(name, false, null, -1f, false);
			return;
		}
		tk2dSpriteAnimationClip clipByName = this.m_anim.GetClipByName(name);
		if (clipByName != null && !this.m_anim.IsPlaying(clipByName))
		{
			this.m_anim.Play(clipByName);
		}
	}

	// Token: 0x06006EC4 RID: 28356 RVA: 0x002BF138 File Offset: 0x002BD338
	public float GetDistanceToPoint(Vector2 point)
	{
		if (!base.gameObject.activeSelf)
		{
			return 10000f;
		}
		if (this.CurrentOwner != null)
		{
			return 10000f;
		}
		if (this.IsBeingSold)
		{
			return 1000f;
		}
		if (!this.m_sprite)
		{
			return 1000f;
		}
		if (this.m_isThrown)
		{
			if (!this.m_thrownOnGround)
			{
				return 1000f;
			}
			if (this.m_transform != null && this.m_transform.parent != null && this.m_transform.parent.GetComponent<Projectile>() != null)
			{
				return 1000f;
			}
		}
		Bounds bounds = this.m_sprite.GetBounds();
		Vector2 vector = base.transform.position.XY() + (base.transform.rotation * bounds.min).XY();
		Vector2 vector2 = vector + (base.transform.rotation * bounds.size).XY();
		return BraveMathCollege.DistToRectangle(point, vector, vector2 - vector);
	}

	// Token: 0x06006EC5 RID: 28357 RVA: 0x002BF26C File Offset: 0x002BD46C
	public float GetOverrideMaxDistance()
	{
		return -1f;
	}

	// Token: 0x06006EC6 RID: 28358 RVA: 0x002BF274 File Offset: 0x002BD474
	public void OnEnteredRange(PlayerController interactor)
	{
		if (!this)
		{
			return;
		}
		if (!RoomHandler.unassignedInteractableObjects.Contains(this))
		{
			return;
		}
		SpriteOutlineManager.RemoveOutlineFromSprite(this.m_sprite, false);
		SpriteOutlineManager.AddOutlineToSprite(this.m_sprite, Color.white, 0.1f, 0f, SpriteOutlineManager.OutlineType.NORMAL);
		this.m_sprite.UpdateZDepth();
	}

	// Token: 0x06006EC7 RID: 28359 RVA: 0x002BF2D0 File Offset: 0x002BD4D0
	public void OnExitRange(PlayerController interactor)
	{
		if (!this)
		{
			return;
		}
		SpriteOutlineManager.RemoveOutlineFromSprite(this.m_sprite, true);
		SpriteOutlineManager.AddOutlineToSprite(this.m_sprite, Color.black, 0.1f, 0.05f, SpriteOutlineManager.OutlineType.NORMAL);
		this.m_sprite.UpdateZDepth();
	}

	// Token: 0x06006EC8 RID: 28360 RVA: 0x002BF310 File Offset: 0x002BD510
	public void Interact(PlayerController interactor)
	{
		if (!base.gameObject.activeSelf)
		{
			return;
		}
		if (GameStatsManager.HasInstance && GameStatsManager.Instance.IsRainbowRun)
		{
			if (interactor && interactor.CurrentRoom != null && interactor.CurrentRoom == GameManager.Instance.Dungeon.data.Entrance && Time.frameCount == PickupObject.s_lastRainbowPickupFrame)
			{
				return;
			}
			PickupObject.s_lastRainbowPickupFrame = Time.frameCount;
		}
		SpriteOutlineManager.RemoveOutlineFromSprite(this.m_sprite, true);
		this.Pickup(interactor);
	}

	// Token: 0x06006EC9 RID: 28361 RVA: 0x002BF3AC File Offset: 0x002BD5AC
	public string GetAnimationState(PlayerController interactor, out bool shouldBeFlipped)
	{
		shouldBeFlipped = false;
		return string.Empty;
	}

	// Token: 0x170010C4 RID: 4292
	// (get) Token: 0x06006ECA RID: 28362 RVA: 0x002BF3B8 File Offset: 0x002BD5B8
	public Transform ThrowPrepTransform
	{
		get
		{
			if (this.m_throwPrepTransform == null)
			{
				this.m_throwPrepTransform = base.transform.Find("throw point");
				if (this.m_throwPrepTransform == null)
				{
					this.m_throwPrepTransform = new GameObject("throw point").transform;
					this.m_throwPrepTransform.parent = base.transform;
				}
			}
			this.m_throwPrepTransform.localPosition = this.ThrowPrepPosition * -1f;
			return this.m_throwPrepTransform;
		}
	}

	// Token: 0x170010C5 RID: 4293
	// (get) Token: 0x06006ECB RID: 28363 RVA: 0x002BF444 File Offset: 0x002BD644
	public Vector3 ThrowPrepPosition
	{
		get
		{
			Vector3 vector = base.sprite.WorldTopRight;
			Vector3 vector2 = this.barrelOffset.transform.parent.InverseTransformPoint(vector);
			Vector3 vector3 = this.barrelOffset.localPosition.WithX(vector2.x) * -1f;
			if (this.m_throwPrepTransform != null)
			{
				this.m_throwPrepTransform.localPosition = vector3 * -1f;
			}
			return vector3;
		}
	}

	// Token: 0x06006ECC RID: 28364 RVA: 0x002BF4C4 File Offset: 0x002BD6C4
	public void TriggerTemporaryBoost(float damageMultiplier, float scaleMultiplier, float duration, bool oneShot)
	{
		base.StartCoroutine(this.HandleTemporaryBoost(damageMultiplier, scaleMultiplier, duration, oneShot));
	}

	// Token: 0x06006ECD RID: 28365 RVA: 0x002BF4D8 File Offset: 0x002BD6D8
	private IEnumerator HandleTemporaryBoost(float damageMultiplier, float scaleMultiplier, float duration, bool oneShot)
	{
		float startTime = Time.time;
		int numberFired = 0;
		Action<Projectile> processTemporaryBoost = delegate(Projectile p)
		{
			p.AdditionalScaleMultiplier *= scaleMultiplier;
			p.baseData.damage *= damageMultiplier;
			numberFired++;
		};
		this.PostProcessProjectile = (Action<Projectile>)Delegate.Combine(this.PostProcessProjectile, processTemporaryBoost);
		while (Time.time - startTime < duration && (!oneShot || numberFired <= 0))
		{
			yield return null;
		}
		this.PostProcessProjectile = (Action<Projectile>)Delegate.Remove(this.PostProcessProjectile, processTemporaryBoost);
		yield break;
	}

	// Token: 0x06006ECE RID: 28366 RVA: 0x002BF510 File Offset: 0x002BD710
	public override void MidGameSerialize(List<object> data)
	{
		base.MidGameSerialize(data);
		int num = 0;
		if (this.UsesRechargeLikeActiveItem)
		{
			num++;
			data.Add(this.m_remainingActiveCooldownAmount);
		}
		IGunInheritable[] interfaces = base.gameObject.GetInterfaces<IGunInheritable>();
		for (int i = 0; i < interfaces.Length; i++)
		{
			interfaces[i].MidGameSerialize(data, i + num);
		}
	}

	// Token: 0x06006ECF RID: 28367 RVA: 0x002BF574 File Offset: 0x002BD774
	public override void MidGameDeserialize(List<object> data)
	{
		base.MidGameDeserialize(data);
		int num = 0;
		if (this.UsesRechargeLikeActiveItem)
		{
			this.m_remainingActiveCooldownAmount = (float)data[num];
			num++;
		}
		IGunInheritable[] interfaces = base.gameObject.GetInterfaces<IGunInheritable>();
		for (int i = 0; i < interfaces.Length; i++)
		{
			interfaces[i].MidGameDeserialize(data, ref num);
		}
	}

	// Token: 0x06006ED0 RID: 28368 RVA: 0x002BF5D8 File Offset: 0x002BD7D8
	public void CopyStateFrom(Gun other)
	{
		if (other && other.UsesRechargeLikeActiveItem)
		{
			this.m_remainingActiveCooldownAmount = other.m_remainingActiveCooldownAmount;
		}
	}

	// Token: 0x06006ED1 RID: 28369 RVA: 0x002BF5FC File Offset: 0x002BD7FC
	public void AddAdditionalFlipTransform(Transform t)
	{
		if (this.m_childTransformsToFlip == null)
		{
			this.m_childTransformsToFlip = new List<Transform>();
		}
		this.m_childTransformsToFlip.Add(t);
	}

	// Token: 0x170010C6 RID: 4294
	// (get) Token: 0x06006ED2 RID: 28370 RVA: 0x002BF620 File Offset: 0x002BD820
	public bool MidBurstFire
	{
		get
		{
			return this.m_midBurstFire;
		}
	}

	// Token: 0x170010C7 RID: 4295
	// (get) Token: 0x06006ED3 RID: 28371 RVA: 0x002BF628 File Offset: 0x002BD828
	public Dictionary<ProjectileModule, ModuleShootData> RuntimeModuleData
	{
		get
		{
			return this.m_moduleData;
		}
	}

	// Token: 0x170010C8 RID: 4296
	// (get) Token: 0x06006ED4 RID: 28372 RVA: 0x002BF630 File Offset: 0x002BD830
	// (set) Token: 0x06006ED5 RID: 28373 RVA: 0x002BF638 File Offset: 0x002BD838
	public int CurrentStrengthTier
	{
		get
		{
			return this.m_currentStrengthTier;
		}
		set
		{
			this.m_currentStrengthTier = value;
			if (this.CurrentOwner && this.CurrentOwner is PlayerController)
			{
				PlayerController playerController = this.CurrentOwner as PlayerController;
				if (playerController.stats != null)
				{
					playerController.stats.RecalculateStats(playerController, false, false);
				}
			}
		}
	}

	// Token: 0x170010C9 RID: 4297
	// (get) Token: 0x06006ED6 RID: 28374 RVA: 0x002BF698 File Offset: 0x002BD898
	public bool IsPreppedForThrow
	{
		get
		{
			return this.m_isPreppedForThrow && this.m_prepThrowTime > 0f;
		}
	}

	// Token: 0x04006B82 RID: 27522
	public static bool ActiveReloadActivated;

	// Token: 0x04006B83 RID: 27523
	public static bool ActiveReloadActivatedPlayerTwo;

	// Token: 0x04006B84 RID: 27524
	public static float s_DualWieldFactor = 0.75f;

	// Token: 0x04006B85 RID: 27525
	public string gunName = "gun";

	// Token: 0x04006B86 RID: 27526
	[FormerlySerializedAs("overrideAudioGunName")]
	public string gunSwitchGroup = string.Empty;

	// Token: 0x04006B87 RID: 27527
	public bool isAudioLoop;

	// Token: 0x04006B88 RID: 27528
	public bool lowersAudioWhileFiring;

	// Token: 0x04006B89 RID: 27529
	public GunClass gunClass;

	// Token: 0x04006B8A RID: 27530
	[SerializeField]
	public StatModifier[] currentGunStatModifiers;

	// Token: 0x04006B8B RID: 27531
	[SerializeField]
	public StatModifier[] passiveStatModifiers;

	// Token: 0x04006B8C RID: 27532
	[SerializeField]
	public DamageTypeModifier[] currentGunDamageTypeModifiers;

	// Token: 0x04006B8D RID: 27533
	[SerializeField]
	public int ArmorToGainOnPickup;

	// Token: 0x04006B8E RID: 27534
	public Transform barrelOffset;

	// Token: 0x04006B8F RID: 27535
	public Transform muzzleOffset;

	// Token: 0x04006B90 RID: 27536
	public Transform chargeOffset;

	// Token: 0x04006B91 RID: 27537
	public Transform reloadOffset;

	// Token: 0x04006B92 RID: 27538
	public IntVector2 carryPixelOffset;

	// Token: 0x04006B93 RID: 27539
	public IntVector2 carryPixelUpOffset;

	// Token: 0x04006B94 RID: 27540
	public IntVector2 carryPixelDownOffset;

	// Token: 0x04006B95 RID: 27541
	public bool UsesPerCharacterCarryPixelOffsets;

	// Token: 0x04006B96 RID: 27542
	public CharacterCarryPixelOffset[] PerCharacterPixelOffsets;

	// Token: 0x04006B97 RID: 27543
	public IntVector2 leftFacingPixelOffset;

	// Token: 0x04006B98 RID: 27544
	public GunHandedness gunHandedness;

	// Token: 0x04006B99 RID: 27545
	public GunHandedness overrideOutOfAmmoHandedness;

	// Token: 0x04006B9A RID: 27546
	public AdditionalHandState additionalHandState;

	// Token: 0x04006B9B RID: 27547
	public GunPositionOverride gunPosition;

	// Token: 0x04006B9C RID: 27548
	public bool forceFlat;

	// Token: 0x04006B9D RID: 27549
	[FormerlySerializedAs("volley")]
	[SerializeField]
	private ProjectileVolleyData rawVolley;

	// Token: 0x04006B9E RID: 27550
	public ProjectileModule singleModule;

	// Token: 0x04006B9F RID: 27551
	[SerializeField]
	public ProjectileVolleyData rawOptionalReloadVolley;

	// Token: 0x04006BA0 RID: 27552
	[NonSerialized]
	public bool OverrideFinaleAudio;

	// Token: 0x04006BA1 RID: 27553
	[NonSerialized]
	public bool HasFiredHolsterShot;

	// Token: 0x04006BA2 RID: 27554
	[NonSerialized]
	public bool HasFiredReloadSynergy;

	// Token: 0x04006BA3 RID: 27555
	[NonSerialized]
	public ProjectileVolleyData modifiedVolley;

	// Token: 0x04006BA4 RID: 27556
	[NonSerialized]
	public ProjectileVolleyData modifiedFinalVolley;

	// Token: 0x04006BA5 RID: 27557
	[NonSerialized]
	public ProjectileVolleyData modifiedOptionalReloadVolley;

	// Token: 0x04006BA6 RID: 27558
	[NonSerialized]
	public List<int> DuctTapeMergedGunIDs;

	// Token: 0x04006BA7 RID: 27559
	[NonSerialized]
	public bool PreventNormalFireAudio;

	// Token: 0x04006BA8 RID: 27560
	[NonSerialized]
	public string OverrideNormalFireAudioEvent;

	// Token: 0x04006BA9 RID: 27561
	public int ammo = 25;

	// Token: 0x04006BAA RID: 27562
	public bool CanGainAmmo = true;

	// Token: 0x04006BAB RID: 27563
	[FormerlySerializedAs("InfiniteAmmo")]
	public bool LocalInfiniteAmmo;

	// Token: 0x04006BAC RID: 27564
	public const float c_FallbackBossDamageModifier = 0.8f;

	// Token: 0x04006BAD RID: 27565
	public const float c_LuteCompanionDamageMultiplier = 2f;

	// Token: 0x04006BAE RID: 27566
	public const float c_LuteCompanionScaleMultiplier = 1.75f;

	// Token: 0x04006BAF RID: 27567
	public const float c_LuteCompanionFireRateMultiplier = 1.5f;

	// Token: 0x04006BB0 RID: 27568
	public bool UsesBossDamageModifier;

	// Token: 0x04006BB1 RID: 27569
	public float CustomBossDamageModifier = -1f;

	// Token: 0x04006BB2 RID: 27570
	[SerializeField]
	private int maxAmmo = -1;

	// Token: 0x04006BB3 RID: 27571
	public float reloadTime;

	// Token: 0x04006BB4 RID: 27572
	[NonSerialized]
	public bool CanReloadNoMatterAmmo;

	// Token: 0x04006BB5 RID: 27573
	public bool blankDuringReload;

	// Token: 0x04006BB6 RID: 27574
	[ShowInInspectorIf("blankDuringReload", false)]
	public float blankReloadRadius = 1f;

	// Token: 0x04006BB7 RID: 27575
	[ShowInInspectorIf("blankDuringReload", false)]
	public bool reflectDuringReload;

	// Token: 0x04006BB8 RID: 27576
	[ShowInInspectorIf("blankDuringReload", false)]
	public float blankKnockbackPower = 20f;

	// Token: 0x04006BB9 RID: 27577
	[ShowInInspectorIf("blankDuringReload", false)]
	public float blankDamageToEnemies;

	// Token: 0x04006BBA RID: 27578
	[ShowInInspectorIf("blankDuringReload", false)]
	public float blankDamageScalingOnEmptyClip = 1f;

	// Token: 0x04006BBB RID: 27579
	[NonSerialized]
	private float AdditionalReloadMultiplier = 1f;

	// Token: 0x04006BBC RID: 27580
	[NonSerialized]
	private int SequentialActiveReloads;

	// Token: 0x04006BBD RID: 27581
	public bool doesScreenShake = true;

	// Token: 0x04006BBE RID: 27582
	public ScreenShakeSettings gunScreenShake = new ScreenShakeSettings(1f, 1f, 0.5f, 0.5f);

	// Token: 0x04006BBF RID: 27583
	public bool directionlessScreenShake;

	// Token: 0x04006BC0 RID: 27584
	public int damageModifier;

	// Token: 0x04006BC1 RID: 27585
	public GameObject thrownObject;

	// Token: 0x04006BC2 RID: 27586
	public ProceduralGunData procGunData;

	// Token: 0x04006BC3 RID: 27587
	public ActiveReloadData activeReloadData;

	// Token: 0x04006BC4 RID: 27588
	public bool ClearsCooldownsLikeAWP;

	// Token: 0x04006BC5 RID: 27589
	public bool AppliesHoming;

	// Token: 0x04006BC6 RID: 27590
	public float AppliedHomingAngularVelocity = 180f;

	// Token: 0x04006BC7 RID: 27591
	public float AppliedHomingDetectRadius = 4f;

	// Token: 0x04006BC8 RID: 27592
	[SerializeField]
	private bool m_unswitchableGun;

	// Token: 0x04006BC9 RID: 27593
	[CheckAnimation(null)]
	public string shootAnimation = string.Empty;

	// Token: 0x04006BCA RID: 27594
	[ShowInInspectorIf("shootAnimation", false)]
	public bool usesContinuousFireAnimation;

	// Token: 0x04006BCB RID: 27595
	[CheckAnimation(null)]
	public string reloadAnimation = string.Empty;

	// Token: 0x04006BCC RID: 27596
	[CheckAnimation(null)]
	public string emptyReloadAnimation = string.Empty;

	// Token: 0x04006BCD RID: 27597
	[CheckAnimation(null)]
	public string idleAnimation = string.Empty;

	// Token: 0x04006BCE RID: 27598
	[CheckAnimation(null)]
	public string chargeAnimation = string.Empty;

	// Token: 0x04006BCF RID: 27599
	[CheckAnimation(null)]
	public string dischargeAnimation = string.Empty;

	// Token: 0x04006BD0 RID: 27600
	[CheckAnimation(null)]
	public string emptyAnimation = string.Empty;

	// Token: 0x04006BD1 RID: 27601
	[CheckAnimation(null)]
	public string introAnimation = string.Empty;

	// Token: 0x04006BD2 RID: 27602
	[CheckAnimation(null)]
	public string finalShootAnimation = string.Empty;

	// Token: 0x04006BD3 RID: 27603
	[CheckAnimation(null)]
	public string enemyPreFireAnimation = string.Empty;

	// Token: 0x04006BD4 RID: 27604
	[CheckAnimation(null)]
	public string outOfAmmoAnimation = string.Empty;

	// Token: 0x04006BD5 RID: 27605
	[CheckAnimation(null)]
	public string criticalFireAnimation = string.Empty;

	// Token: 0x04006BD6 RID: 27606
	[CheckAnimation(null)]
	public string dodgeAnimation = string.Empty;

	// Token: 0x04006BD7 RID: 27607
	public bool usesDirectionalIdleAnimations;

	// Token: 0x04006BD8 RID: 27608
	public bool usesDirectionalAnimator;

	// Token: 0x04006BD9 RID: 27609
	public bool preventRotation;

	// Token: 0x04006BDA RID: 27610
	public VFXPool muzzleFlashEffects;

	// Token: 0x04006BDB RID: 27611
	[ShowInInspectorIf("muzzleFlashEffects", false)]
	public bool usesContinuousMuzzleFlash;

	// Token: 0x04006BDC RID: 27612
	public VFXPool finalMuzzleFlashEffects;

	// Token: 0x04006BDD RID: 27613
	public VFXPool reloadEffects;

	// Token: 0x04006BDE RID: 27614
	public VFXPool emptyReloadEffects;

	// Token: 0x04006BDF RID: 27615
	public VFXPool activeReloadSuccessEffects;

	// Token: 0x04006BE0 RID: 27616
	public VFXPool activeReloadFailedEffects;

	// Token: 0x04006BE1 RID: 27617
	public Light light;

	// Token: 0x04006BE2 RID: 27618
	public float baseLightIntensity;

	// Token: 0x04006BE3 RID: 27619
	public GameObject shellCasing;

	// Token: 0x04006BE4 RID: 27620
	public int shellsToLaunchOnFire = 1;

	// Token: 0x04006BE5 RID: 27621
	public int shellCasingOnFireFrameDelay;

	// Token: 0x04006BE6 RID: 27622
	public int shellsToLaunchOnReload;

	// Token: 0x04006BE7 RID: 27623
	public int reloadShellLaunchFrame;

	// Token: 0x04006BE8 RID: 27624
	public GameObject clipObject;

	// Token: 0x04006BE9 RID: 27625
	public int clipsToLaunchOnReload;

	// Token: 0x04006BEA RID: 27626
	public int reloadClipLaunchFrame;

	// Token: 0x04006BEB RID: 27627
	[HideInInspector]
	public string prefabName = string.Empty;

	// Token: 0x04006BEC RID: 27628
	public bool rampBullets;

	// Token: 0x04006BED RID: 27629
	public float rampStartHeight = 2f;

	// Token: 0x04006BEE RID: 27630
	public float rampTime = 1f;

	// Token: 0x04006BEF RID: 27631
	public bool IgnoresAngleQuantization;

	// Token: 0x04006BF0 RID: 27632
	public bool IsTrickGun;

	// Token: 0x04006BF1 RID: 27633
	public bool TrickGunAlternatesHandedness;

	// Token: 0x04006BF2 RID: 27634
	public bool PreventOutlines;

	// Token: 0x04006BF3 RID: 27635
	public ProjectileVolleyData alternateVolley;

	// Token: 0x04006BF4 RID: 27636
	[CheckAnimation(null)]
	public string alternateShootAnimation;

	// Token: 0x04006BF5 RID: 27637
	[CheckAnimation(null)]
	public string alternateReloadAnimation;

	// Token: 0x04006BF6 RID: 27638
	[CheckAnimation(null)]
	public string alternateIdleAnimation;

	// Token: 0x04006BF7 RID: 27639
	public string alternateSwitchGroup;

	// Token: 0x04006BF8 RID: 27640
	public bool IsHeroSword;

	// Token: 0x04006BF9 RID: 27641
	public bool HeroSwordDoesntBlank;

	// Token: 0x04006BFA RID: 27642
	public bool StarterGunForAchievement;

	// Token: 0x04006BFB RID: 27643
	private float HeroSwordCooldown;

	// Token: 0x04006BFC RID: 27644
	public bool CanSneakAttack;

	// Token: 0x04006BFD RID: 27645
	public float SneakAttackDamageMultiplier = 3f;

	// Token: 0x04006BFE RID: 27646
	public bool SuppressLaserSight;

	// Token: 0x04006BFF RID: 27647
	public bool RequiresFundsToShoot;

	// Token: 0x04006C00 RID: 27648
	public int CurrencyCostPerShot = 1;

	// Token: 0x04006C01 RID: 27649
	public GunWeaponPanelSpriteOverride weaponPanelSpriteOverride;

	// Token: 0x04006C02 RID: 27650
	public bool IsLuteCompanionBuff;

	// Token: 0x04006C03 RID: 27651
	public bool MovesPlayerForwardOnChargeFire;

	// Token: 0x04006C04 RID: 27652
	public bool LockedHorizontalOnCharge;

	// Token: 0x04006C05 RID: 27653
	public float LockedHorizontalCenterFireOffset = -1f;

	// Token: 0x04006C06 RID: 27654
	[NonSerialized]
	public bool LockedHorizontalOnReload;

	// Token: 0x04006C07 RID: 27655
	private float LockedHorizontalCachedAngle = -1f;

	// Token: 0x04006C08 RID: 27656
	public bool GoopReloadsFree;

	// Token: 0x04006C09 RID: 27657
	public bool IsUndertaleGun;

	// Token: 0x04006C0A RID: 27658
	public bool LocalActiveReload;

	// Token: 0x04006C0B RID: 27659
	public bool UsesRechargeLikeActiveItem;

	// Token: 0x04006C0C RID: 27660
	public float ActiveItemStyleRechargeAmount = 100f;

	// Token: 0x04006C0D RID: 27661
	public bool CanAttackThroughObjects;

	// Token: 0x04006C0E RID: 27662
	private float m_remainingActiveCooldownAmount;

	// Token: 0x04006C0F RID: 27663
	public bool CanCriticalFire;

	// Token: 0x04006C10 RID: 27664
	public float CriticalChance = 0.1f;

	// Token: 0x04006C11 RID: 27665
	public float CriticalDamageMultiplier = 3f;

	// Token: 0x04006C12 RID: 27666
	public VFXPool CriticalMuzzleFlashEffects;

	// Token: 0x04006C13 RID: 27667
	public Projectile CriticalReplacementProjectile;

	// Token: 0x04006C14 RID: 27668
	public bool GainsRateOfFireAsContinueAttack;

	// Token: 0x04006C15 RID: 27669
	public float RateOfFireMultiplierAdditionPerSecond;

	// Token: 0x04006C16 RID: 27670
	public bool OnlyUsesIdleInWeaponBox;

	// Token: 0x04006C17 RID: 27671
	public bool DisablesRendererOnCooldown;

	// Token: 0x04006C18 RID: 27672
	[FormerlySerializedAs("ObjectToInstantiatedOnClipDepleted")]
	[SerializeField]
	public GameObject ObjectToInstantiateOnReload;

	// Token: 0x04006C19 RID: 27673
	[NonSerialized]
	public int AdditionalClipCapacity;

	// Token: 0x04006C1A RID: 27674
	private RoomHandler m_minimapIconRoom;

	// Token: 0x04006C1B RID: 27675
	private GameObject m_instanceMinimapIcon;

	// Token: 0x04006C1C RID: 27676
	private GunHandedness? m_cachedGunHandedness;

	// Token: 0x04006C23 RID: 27683
	public Action<GameActor> OnInitializedWithOwner;

	// Token: 0x04006C24 RID: 27684
	public Action<Projectile> PostProcessProjectile;

	// Token: 0x04006C25 RID: 27685
	public Action<ProjectileVolleyData> PostProcessVolley;

	// Token: 0x04006C26 RID: 27686
	public Action OnDropped;

	// Token: 0x04006C27 RID: 27687
	public Action<PlayerController, Gun> OnAutoReload;

	// Token: 0x04006C28 RID: 27688
	public Action<PlayerController, Gun, bool> OnReloadPressed;

	// Token: 0x04006C29 RID: 27689
	public Action<PlayerController, Gun> OnFinishAttack;

	// Token: 0x04006C2A RID: 27690
	public Action<PlayerController, Gun> OnPostFired;

	// Token: 0x04006C2B RID: 27691
	public Action<PlayerController, Gun> OnAmmoChanged;

	// Token: 0x04006C2C RID: 27692
	public Action<PlayerController, Gun> OnBurstContinued;

	// Token: 0x04006C2D RID: 27693
	public Func<float, float> OnReflectedBulletDamageModifier;

	// Token: 0x04006C2E RID: 27694
	public Func<float, float> OnReflectedBulletScaleModifier;

	// Token: 0x04006C2F RID: 27695
	[NonSerialized]
	private tk2dTiledSprite m_extantLaserSight;

	// Token: 0x04006C30 RID: 27696
	[NonSerialized]
	public int LastShotIndex = -1;

	// Token: 0x04006C31 RID: 27697
	[NonSerialized]
	public bool DidTransformGunThisFrame;

	// Token: 0x04006C32 RID: 27698
	[NonSerialized]
	public float CustomLaserSightDistance = 30f;

	// Token: 0x04006C33 RID: 27699
	[NonSerialized]
	public float CustomLaserSightHeight = 0.25f;

	// Token: 0x04006C34 RID: 27700
	[NonSerialized]
	public AIActor LastLaserSightEnemy;

	// Token: 0x04006C35 RID: 27701
	private GameObject m_extantLockOnSprite;

	// Token: 0x04006C36 RID: 27702
	private bool m_hasReinitializedAudioSwitch;

	// Token: 0x04006C37 RID: 27703
	[NonSerialized]
	public bool HasEverBeenAcquiredByPlayer;

	// Token: 0x04006C38 RID: 27704
	private SingleSpawnableGunPlacedObject m_extantAmp;

	// Token: 0x04006C39 RID: 27705
	[NonSerialized]
	public bool ForceNextShotCritical;

	// Token: 0x04006C3A RID: 27706
	private bool m_isCritting;

	// Token: 0x04006C3B RID: 27707
	public Func<Gun, Projectile, ProjectileModule, Projectile> OnPreFireProjectileModifier;

	// Token: 0x04006C3C RID: 27708
	public Func<float, float> ModifyActiveCooldownDamage;

	// Token: 0x04006C3D RID: 27709
	private const bool c_clickingCanActiveReload = true;

	// Token: 0x04006C3E RID: 27710
	private const bool c_DUAL_WIELD_PARALLEL_RELOAD = false;

	// Token: 0x04006C3F RID: 27711
	private bool m_hasSwappedTrickGunsThisCycle;

	// Token: 0x04006C40 RID: 27712
	protected List<ModuleShootData> m_activeBeams = new List<ModuleShootData>();

	// Token: 0x04006C41 RID: 27713
	private string[] m_directionalIdleNames;

	// Token: 0x04006C42 RID: 27714
	private bool m_preventIdleLoop;

	// Token: 0x04006C43 RID: 27715
	private bool m_hasDecrementedFunds;

	// Token: 0x04006C45 RID: 27717
	private Transform m_throwPrepTransform;

	// Token: 0x04006C46 RID: 27718
	private tk2dBaseSprite m_sprite;

	// Token: 0x04006C47 RID: 27719
	private tk2dSpriteAnimator m_anim;

	// Token: 0x04006C48 RID: 27720
	private GameActor m_owner;

	// Token: 0x04006C49 RID: 27721
	private int m_defaultSpriteID;

	// Token: 0x04006C4A RID: 27722
	private Transform m_transform;

	// Token: 0x04006C4B RID: 27723
	private Transform m_attachTransform;

	// Token: 0x04006C4C RID: 27724
	private List<Transform> m_childTransformsToFlip;

	// Token: 0x04006C4D RID: 27725
	private Vector3 m_defaultLocalPosition;

	// Token: 0x04006C4E RID: 27726
	private MeshRenderer m_meshRenderer;

	// Token: 0x04006C4F RID: 27727
	private Transform m_clipLaunchAttachPoint;

	// Token: 0x04006C50 RID: 27728
	private Transform m_localAttachPoint;

	// Token: 0x04006C51 RID: 27729
	private Transform m_offhandAttachPoint;

	// Token: 0x04006C52 RID: 27730
	private Transform m_casingLaunchAttachPoint;

	// Token: 0x04006C53 RID: 27731
	private float gunAngle;

	// Token: 0x04006C54 RID: 27732
	private float prevGunAngleUnmodified;

	// Token: 0x04006C55 RID: 27733
	private float gunCooldownModifier;

	// Token: 0x04006C56 RID: 27734
	private Vector2 m_localAimPoint;

	// Token: 0x04006C57 RID: 27735
	private Vector3 m_unroundedBarrelPosition;

	// Token: 0x04006C58 RID: 27736
	private Vector3 m_originalBarrelOffsetPosition;

	// Token: 0x04006C59 RID: 27737
	private Vector3 m_originalMuzzleOffsetPosition;

	// Token: 0x04006C5A RID: 27738
	private Vector3 m_originalChargeOffsetPosition;

	// Token: 0x04006C5B RID: 27739
	private float m_fractionalAmmoUsage;

	// Token: 0x04006C5C RID: 27740
	public bool HasBeenPickedUp;

	// Token: 0x04006C5D RID: 27741
	private bool m_reloadWhenDoneFiring;

	// Token: 0x04006C5E RID: 27742
	private bool m_isReloading;

	// Token: 0x04006C5F RID: 27743
	private bool m_isThrown;

	// Token: 0x04006C60 RID: 27744
	private bool m_thrownOnGround = true;

	// Token: 0x04006C61 RID: 27745
	private bool m_canAttemptActiveReload;

	// Token: 0x04006C62 RID: 27746
	private bool m_isCurrentlyFiring;

	// Token: 0x04006C63 RID: 27747
	private bool m_isAudioLooping;

	// Token: 0x04006C64 RID: 27748
	private float m_continuousAttackTime;

	// Token: 0x04006C65 RID: 27749
	private float m_reloadElapsed;

	// Token: 0x04006C66 RID: 27750
	private bool m_hasDoneSingleReloadBlankEffect;

	// Token: 0x04006C67 RID: 27751
	private bool m_cachedIsGunBlocked;

	// Token: 0x04006C68 RID: 27752
	private bool m_playedEmptyClipSound;

	// Token: 0x04006C69 RID: 27753
	private VFXPool m_currentlyPlayingChargeVFX;

	// Token: 0x04006C6A RID: 27754
	private bool m_midBurstFire;

	// Token: 0x04006C6B RID: 27755
	private bool m_continueBurstInUpdate;

	// Token: 0x04006C6C RID: 27756
	private bool m_isContinuousMuzzleFlashOut;

	// Token: 0x04006C6D RID: 27757
	private Dictionary<ProjectileModule, ModuleShootData> m_moduleData;

	// Token: 0x04006C6E RID: 27758
	[NonSerialized]
	private List<ActiveAmmunitionData> m_customAmmunitions = new List<ActiveAmmunitionData>();

	// Token: 0x04006C6F RID: 27759
	private int m_currentStrengthTier;

	// Token: 0x04006C70 RID: 27760
	[NonSerialized]
	public Dictionary<string, string> AdditionalShootSoundsByModule = new Dictionary<string, string>();

	// Token: 0x04006C71 RID: 27761
	[NonSerialized]
	public float? OverrideAngleSnap;

	// Token: 0x04006C72 RID: 27762
	private bool m_isPreppedForThrow;

	// Token: 0x04006C73 RID: 27763
	private float m_prepThrowTime = -0.3f;

	// Token: 0x04006C74 RID: 27764
	private const float c_prepTime = 1.2f;

	// Token: 0x04006C75 RID: 27765
	private const bool c_attackingCanReload = true;

	// Token: 0x04006C76 RID: 27766
	private const bool c_throwGunOnFire = true;

	// Token: 0x0200131A RID: 4890
	public enum AttackResult
	{
		// Token: 0x04006C7B RID: 27771
		Success,
		// Token: 0x04006C7C RID: 27772
		OnCooldown,
		// Token: 0x04006C7D RID: 27773
		Reload,
		// Token: 0x04006C7E RID: 27774
		Empty,
		// Token: 0x04006C7F RID: 27775
		Fail
	}
}
