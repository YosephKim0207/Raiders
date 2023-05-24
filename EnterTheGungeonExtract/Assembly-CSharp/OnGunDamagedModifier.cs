using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200144A RID: 5194
public class OnGunDamagedModifier : MonoBehaviour, IGunInheritable
{
	// Token: 0x17001198 RID: 4504
	// (get) Token: 0x060075EA RID: 30186 RVA: 0x002EEEF4 File Offset: 0x002ED0F4
	// (set) Token: 0x060075EB RID: 30187 RVA: 0x002EEEFC File Offset: 0x002ED0FC
	public bool Broken
	{
		get
		{
			return this.m_gunBroken;
		}
		set
		{
			this.m_gunBroken = value;
		}
	}

	// Token: 0x060075EC RID: 30188 RVA: 0x002EEF08 File Offset: 0x002ED108
	private void Awake()
	{
		this.m_hasAwoken = true;
		this.m_gun = base.GetComponent<Gun>();
		this.m_cachedIdleAnimation = this.m_gun.idleAnimation;
		this.m_cachedEmptyAnimation = this.m_gun.emptyAnimation;
		this.m_cachedChargeAnimation = this.m_gun.chargeAnimation;
		this.m_cachedIntroAnimation = this.m_gun.introAnimation;
		this.m_cachedDefaultID = this.m_gun.DefaultSpriteID;
		if (this.m_gunBroken && !string.IsNullOrEmpty(this.BrokenAnimation))
		{
			this.SetBrokenAnims();
		}
		Gun gun = this.m_gun;
		gun.OnInitializedWithOwner = (Action<GameActor>)Delegate.Combine(gun.OnInitializedWithOwner, new Action<GameActor>(this.OnGunInitialized));
		Gun gun2 = this.m_gun;
		gun2.OnDropped = (Action)Delegate.Combine(gun2.OnDropped, new Action(this.OnGunDroppedOrDestroyed));
		Gun gun3 = this.m_gun;
		gun3.OnAmmoChanged = (Action<PlayerController, Gun>)Delegate.Combine(gun3.OnAmmoChanged, new Action<PlayerController, Gun>(this.HandleAmmoChanged));
		Gun gun4 = this.m_gun;
		gun4.OnPostFired = (Action<PlayerController, Gun>)Delegate.Combine(gun4.OnPostFired, new Action<PlayerController, Gun>(this.HandleAmmoChanged));
		if (this.m_gun.CurrentOwner != null)
		{
			this.OnGunInitialized(this.m_gun.CurrentOwner);
		}
	}

	// Token: 0x060075ED RID: 30189 RVA: 0x002EF064 File Offset: 0x002ED264
	private void Start()
	{
		this.m_cachedDefaultID = this.m_gun.DefaultSpriteID;
	}

	// Token: 0x060075EE RID: 30190 RVA: 0x002EF078 File Offset: 0x002ED278
	private void Update()
	{
		if (this.m_playerOwner && this.PreventDepleteWithSynergy && this.m_playerOwner.HasActiveBonusSynergy(this.PreventDepleteSynergy, false))
		{
			this.m_lastFramePlayerHadSynergy = Time.frameCount;
		}
	}

	// Token: 0x060075EF RID: 30191 RVA: 0x002EF0B8 File Offset: 0x002ED2B8
	private void SetBrokenAnims()
	{
		this.m_gun.CanBeDropped = false;
		this.m_gun.idleAnimation = this.BrokenAnimation;
		this.m_gun.emptyAnimation = this.BrokenAnimation;
		this.m_gun.chargeAnimation = string.Empty;
		this.m_gun.introAnimation = string.Empty;
		tk2dSpriteAnimationClip clipByName = this.m_gun.spriteAnimator.GetClipByName(this.BrokenAnimation);
		this.m_gun.DefaultSpriteID = clipByName.frames[clipByName.frames.Length - 1].spriteId;
	}

	// Token: 0x060075F0 RID: 30192 RVA: 0x002EF14C File Offset: 0x002ED34C
	private void HandleAmmoChanged(PlayerController player, Gun ammoGun)
	{
		if (this.m_playerOwner)
		{
			if (ammoGun == this.m_gun && ammoGun.ammo >= 1 && this.m_gunBroken)
			{
				this.m_gunBroken = false;
				if (this.DisableHandsOnDepletion)
				{
					this.m_gun.additionalHandState = AdditionalHandState.None;
					player.ToggleHandRenderers(true, string.Empty);
					player.ProcessHandAttachment();
					GameManager.Instance.Dungeon.StartCoroutine(this.FrameDelayedProcessing(player));
				}
				if (!string.IsNullOrEmpty(this.BrokenAnimation))
				{
					this.m_gun.CanBeDropped = true;
					this.m_gun.idleAnimation = this.m_cachedIdleAnimation;
					this.m_gun.emptyAnimation = this.m_cachedEmptyAnimation;
					this.m_gun.chargeAnimation = this.m_cachedChargeAnimation;
					this.m_gun.introAnimation = this.m_cachedIntroAnimation;
					this.m_gun.DefaultSpriteID = this.m_cachedDefaultID;
					this.m_gun.PlayIdleAnimation();
				}
			}
			this.CheckFlightStatus(this.m_playerOwner.CurrentGun);
		}
	}

	// Token: 0x060075F1 RID: 30193 RVA: 0x002EF268 File Offset: 0x002ED468
	private IEnumerator FrameDelayedProcessing(PlayerController p)
	{
		yield return null;
		if (p && p.CurrentGun == this.m_gun && this.m_gun)
		{
			p.ToggleHandRenderers(true, string.Empty);
			p.ProcessHandAttachment();
		}
		yield break;
	}

	// Token: 0x060075F2 RID: 30194 RVA: 0x002EF28C File Offset: 0x002ED48C
	private void OnGunInitialized(GameActor actor)
	{
		if (this.m_playerOwner != null)
		{
			this.OnGunDroppedOrDestroyed();
		}
		if (actor == null)
		{
			return;
		}
		if (actor is PlayerController)
		{
			this.m_playerOwner = actor as PlayerController;
			this.m_playerOwner.OnReceivedDamage += this.OnReceivedDamage;
			this.m_playerOwner.GunChanged += this.HandleGunChanged;
		}
		if (this.m_playerOwner)
		{
			this.CheckFlightStatus(this.m_playerOwner.CurrentGun);
		}
	}

	// Token: 0x060075F3 RID: 30195 RVA: 0x002EF324 File Offset: 0x002ED524
	private void CheckFlightStatus(Gun currentGun)
	{
		if (this.m_gun)
		{
			this.m_gun.overrideOutOfAmmoHandedness = GunHandedness.NoHanded;
		}
		if (this.NondepletedGunGrantsFlight && this.m_playerOwner && currentGun)
		{
			OnGunDamagedModifier component = currentGun.GetComponent<OnGunDamagedModifier>();
			if (component && component.NondepletedGunGrantsFlight)
			{
				this.m_playerOwner.SetIsFlying(!component.m_gunBroken, "balloon gun", false, false);
				this.m_playerOwner.AdditionalCanDodgeRollWhileFlying.SetOverride("balloon gun", true, null);
			}
			else
			{
				this.m_playerOwner.SetIsFlying(false, "balloon gun", false, false);
				this.m_playerOwner.AdditionalCanDodgeRollWhileFlying.RemoveOverride("balloon gun");
			}
		}
	}

	// Token: 0x060075F4 RID: 30196 RVA: 0x002EF3F8 File Offset: 0x002ED5F8
	private void HandleGunChanged(Gun previous, Gun current, bool isNew)
	{
		this.CheckFlightStatus(current);
	}

	// Token: 0x060075F5 RID: 30197 RVA: 0x002EF404 File Offset: 0x002ED604
	private void OnReceivedDamage(PlayerController player)
	{
		if (player && player.CurrentGun == this.m_gun)
		{
			if (this.PreventDepleteWithSynergy && player.HasActiveBonusSynergy(this.PreventDepleteSynergy, false))
			{
				return;
			}
			if (this.PreventDepleteWithSynergy && this.m_lastFramePlayerHadSynergy == Time.frameCount)
			{
				return;
			}
			if (!this.m_gunBroken)
			{
				this.m_gunBroken = true;
				if (!string.IsNullOrEmpty(this.BrokenAnimation))
				{
					this.SetBrokenAnims();
					this.m_gun.PlayIdleAnimation();
				}
			}
			if (this.DepleteAmmoOnDamage && (!this.PreventDepleteWithSynergy || !player.HasActiveBonusSynergy(this.PreventDepleteSynergy, false)))
			{
				this.m_gun.ammo = 0;
				if (this.DisableHandsOnDepletion)
				{
					this.m_gun.additionalHandState = AdditionalHandState.HideBoth;
				}
			}
			this.CheckFlightStatus(player.CurrentGun);
		}
	}

	// Token: 0x060075F6 RID: 30198 RVA: 0x002EF4F8 File Offset: 0x002ED6F8
	private void OnDestroy()
	{
		this.OnGunDroppedOrDestroyed();
	}

	// Token: 0x060075F7 RID: 30199 RVA: 0x002EF500 File Offset: 0x002ED700
	private void OnGunDroppedOrDestroyed()
	{
		if (this.m_playerOwner != null)
		{
			this.m_playerOwner.OnReceivedDamage -= this.OnReceivedDamage;
			this.m_playerOwner.GunChanged -= this.HandleGunChanged;
			this.m_playerOwner = null;
		}
	}

	// Token: 0x060075F8 RID: 30200 RVA: 0x002EF554 File Offset: 0x002ED754
	public void InheritData(Gun sourceGun)
	{
		if (sourceGun)
		{
			if (!this.m_hasAwoken)
			{
				this.m_gun = base.GetComponent<Gun>();
				this.m_cachedIdleAnimation = this.m_gun.idleAnimation;
				this.m_cachedEmptyAnimation = this.m_gun.emptyAnimation;
				this.m_cachedChargeAnimation = this.m_gun.chargeAnimation;
				this.m_cachedIntroAnimation = this.m_gun.introAnimation;
				this.m_cachedDefaultID = this.m_gun.DefaultSpriteID;
			}
			OnGunDamagedModifier component = sourceGun.GetComponent<OnGunDamagedModifier>();
			if (component)
			{
				this.m_gunBroken = component.m_gunBroken;
				if (!string.IsNullOrEmpty(component.m_cachedEmptyAnimation))
				{
					this.m_cachedEmptyAnimation = component.m_cachedEmptyAnimation;
				}
				if (!string.IsNullOrEmpty(component.m_cachedIdleAnimation))
				{
					this.m_cachedIdleAnimation = component.m_cachedIdleAnimation;
				}
				if (!string.IsNullOrEmpty(component.m_cachedChargeAnimation))
				{
					this.m_cachedChargeAnimation = component.m_cachedChargeAnimation;
				}
				if (!string.IsNullOrEmpty(component.m_cachedIntroAnimation))
				{
					this.m_cachedIntroAnimation = component.m_cachedIntroAnimation;
				}
				if (component.m_cachedDefaultID != -1)
				{
					this.m_cachedDefaultID = component.m_cachedDefaultID;
				}
				base.GetComponent<Gun>().idleAnimation = this.m_cachedIdleAnimation;
				base.GetComponent<Gun>().emptyAnimation = this.m_cachedEmptyAnimation;
				base.GetComponent<Gun>().chargeAnimation = this.m_cachedChargeAnimation;
				base.GetComponent<Gun>().introAnimation = this.m_cachedIntroAnimation;
			}
		}
	}

	// Token: 0x060075F9 RID: 30201 RVA: 0x002EF6C4 File Offset: 0x002ED8C4
	public void MidGameSerialize(List<object> data, int dataIndex)
	{
		data.Add(this.Broken);
	}

	// Token: 0x060075FA RID: 30202 RVA: 0x002EF6D8 File Offset: 0x002ED8D8
	public void MidGameDeserialize(List<object> data, ref int dataIndex)
	{
		this.Broken = (bool)data[dataIndex];
		if (this.m_gunBroken && !string.IsNullOrEmpty(this.BrokenAnimation))
		{
			this.SetBrokenAnims();
			this.m_gun.PlayIdleAnimation();
		}
		dataIndex++;
	}

	// Token: 0x040077A3 RID: 30627
	[CheckAnimation(null)]
	public string BrokenAnimation;

	// Token: 0x040077A4 RID: 30628
	public bool DepleteAmmoOnDamage;

	// Token: 0x040077A5 RID: 30629
	public bool NondepletedGunGrantsFlight;

	// Token: 0x040077A6 RID: 30630
	public bool DisableHandsOnDepletion;

	// Token: 0x040077A7 RID: 30631
	public bool PreventDepleteWithSynergy;

	// Token: 0x040077A8 RID: 30632
	[LongNumericEnum]
	public CustomSynergyType PreventDepleteSynergy;

	// Token: 0x040077A9 RID: 30633
	private Gun m_gun;

	// Token: 0x040077AA RID: 30634
	private PlayerController m_playerOwner;

	// Token: 0x040077AB RID: 30635
	private string m_cachedIdleAnimation;

	// Token: 0x040077AC RID: 30636
	private string m_cachedEmptyAnimation;

	// Token: 0x040077AD RID: 30637
	private string m_cachedChargeAnimation;

	// Token: 0x040077AE RID: 30638
	private string m_cachedIntroAnimation;

	// Token: 0x040077AF RID: 30639
	private int m_cachedDefaultID = -1;

	// Token: 0x040077B0 RID: 30640
	private bool m_hasAwoken;

	// Token: 0x040077B1 RID: 30641
	private bool m_gunBroken;

	// Token: 0x040077B2 RID: 30642
	private int m_lastFramePlayerHadSynergy = -1;
}
