using System;
using UnityEngine;

// Token: 0x0200134C RID: 4940
public class AuraOnReloadModifier : MonoBehaviour
{
	// Token: 0x06007003 RID: 28675 RVA: 0x002C62F0 File Offset: 0x002C44F0
	private void Awake()
	{
		this.m_gun = base.GetComponent<Gun>();
		Gun gun = this.m_gun;
		gun.OnDropped = (Action)Delegate.Combine(gun.OnDropped, new Action(this.OnDropped));
		PlayerController playerController = this.m_gun.CurrentOwner as PlayerController;
		if (playerController)
		{
			playerController.inventory.OnGunChanged += this.OnGunChanged;
		}
	}

	// Token: 0x06007004 RID: 28676 RVA: 0x002C6364 File Offset: 0x002C4564
	private void Update()
	{
		if (this.m_gun.IsReloading && this.m_gun.CurrentOwner is PlayerController)
		{
			this.DoAura();
			if (this.IgnitesEnemies || this.DoRadialIndicatorAnyway)
			{
				this.HandleRadialIndicator();
			}
		}
		else
		{
			this.UnhandleRadialIndicator();
		}
	}

	// Token: 0x06007005 RID: 28677 RVA: 0x002C63C4 File Offset: 0x002C45C4
	private void HandleRadialIndicator()
	{
		if (!this.m_radialIndicatorActive)
		{
			this.m_radialIndicatorActive = true;
			this.m_radialIndicator = ((GameObject)UnityEngine.Object.Instantiate(ResourceCache.Acquire("Global VFX/HeatIndicator"), this.m_gun.CurrentOwner.CenterPosition.ToVector3ZisY(0f), Quaternion.identity, this.m_gun.CurrentOwner.transform)).GetComponent<HeatIndicatorController>();
			if (!this.IgnitesEnemies)
			{
				this.m_radialIndicator.CurrentColor = new Color(0f, 0f, 1f);
				this.m_radialIndicator.IsFire = false;
			}
		}
	}

	// Token: 0x06007006 RID: 28678 RVA: 0x002C6468 File Offset: 0x002C4668
	private void UnhandleRadialIndicator()
	{
		if (this.m_radialIndicatorActive)
		{
			this.m_radialIndicatorActive = false;
			if (this.m_radialIndicator)
			{
				this.m_radialIndicator.EndEffect();
			}
			this.m_radialIndicator = null;
		}
	}

	// Token: 0x06007007 RID: 28679 RVA: 0x002C64A0 File Offset: 0x002C46A0
	protected virtual void DoAura()
	{
		bool didDamageEnemies = false;
		PlayerController playerController = this.m_gun.CurrentOwner as PlayerController;
		if (this.AuraAction == null)
		{
			this.AuraAction = delegate(AIActor actor, float dist)
			{
				float num2 = this.DamagePerSecond * BraveTime.DeltaTime;
				if (this.IgnitesEnemies || num2 > 0f)
				{
					didDamageEnemies = true;
				}
				if (this.IgnitesEnemies)
				{
					actor.ApplyEffect(this.IgniteEffect, 1f, null);
				}
				actor.healthHaver.ApplyDamage(num2, Vector2.zero, "Aura", this.damageTypes, DamageCategory.Normal, false, null, false);
			};
		}
		if (playerController != null && playerController.CurrentRoom != null)
		{
			float num = this.AuraRadius;
			if (this.HasRadiusSynergy && playerController.HasActiveBonusSynergy(this.RadiusSynergy, false))
			{
				num = this.RadiusSynergyRadius;
			}
			if (this.m_radialIndicator)
			{
				this.m_radialIndicator.CurrentRadius = num;
			}
			playerController.CurrentRoom.ApplyActionToNearbyEnemies(playerController.CenterPosition, num, this.AuraAction);
		}
		if (didDamageEnemies)
		{
			playerController.DidUnstealthyAction();
		}
	}

	// Token: 0x06007008 RID: 28680 RVA: 0x002C6578 File Offset: 0x002C4778
	private void OnDropped()
	{
		this.UnhandleRadialIndicator();
		PlayerController playerController = this.m_gun.CurrentOwner as PlayerController;
		if (playerController)
		{
			playerController.inventory.OnGunChanged -= this.OnGunChanged;
		}
	}

	// Token: 0x06007009 RID: 28681 RVA: 0x002C65C0 File Offset: 0x002C47C0
	private void OnGunChanged(Gun previous, Gun current, Gun previoussecondary, Gun currentsecondary, bool newgun)
	{
		if (current != this && currentsecondary != this)
		{
			this.UnhandleRadialIndicator();
		}
	}

	// Token: 0x0600700A RID: 28682 RVA: 0x002C65E4 File Offset: 0x002C47E4
	private void OnDestroy()
	{
		Gun gun = this.m_gun;
		gun.OnDropped = (Action)Delegate.Remove(gun.OnDropped, new Action(this.OnDropped));
		PlayerController playerController = this.m_gun.CurrentOwner as PlayerController;
		if (playerController)
		{
			playerController.inventory.OnGunChanged -= this.OnGunChanged;
		}
	}

	// Token: 0x04006F52 RID: 28498
	public float AuraRadius = 5f;

	// Token: 0x04006F53 RID: 28499
	public CoreDamageTypes damageTypes;

	// Token: 0x04006F54 RID: 28500
	public float DamagePerSecond = 5f;

	// Token: 0x04006F55 RID: 28501
	public bool IgnitesEnemies;

	// Token: 0x04006F56 RID: 28502
	public GameActorFireEffect IgniteEffect;

	// Token: 0x04006F57 RID: 28503
	public bool DoRadialIndicatorAnyway;

	// Token: 0x04006F58 RID: 28504
	public bool HasRadiusSynergy;

	// Token: 0x04006F59 RID: 28505
	[LongNumericEnum]
	public CustomSynergyType RadiusSynergy;

	// Token: 0x04006F5A RID: 28506
	public float RadiusSynergyRadius = 10f;

	// Token: 0x04006F5B RID: 28507
	private Gun m_gun;

	// Token: 0x04006F5C RID: 28508
	private Action<AIActor, float> AuraAction;

	// Token: 0x04006F5D RID: 28509
	private bool m_radialIndicatorActive;

	// Token: 0x04006F5E RID: 28510
	private HeatIndicatorController m_radialIndicator;
}
