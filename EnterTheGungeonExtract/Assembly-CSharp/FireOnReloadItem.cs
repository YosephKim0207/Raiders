using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001404 RID: 5124
public class FireOnReloadItem : PassiveItem
{
	// Token: 0x0600744C RID: 29772 RVA: 0x002E43F4 File Offset: 0x002E25F4
	private void Awake()
	{
		if (this.IsHipHolster)
		{
			RadialBurstInterface radialBurstSettings = this.RadialBurstSettings;
			radialBurstSettings.CustomPostProcessProjectile = (Action<Projectile>)Delegate.Combine(radialBurstSettings.CustomPostProcessProjectile, new Action<Projectile>(this.HandleHipHolsterProcessing));
		}
	}

	// Token: 0x0600744D RID: 29773 RVA: 0x002E4428 File Offset: 0x002E2628
	private void HandleHipHolsterProcessing(Projectile proj)
	{
		if (base.Owner)
		{
			if (base.Owner.HasActiveBonusSynergy(CustomSynergyType.DOUBLE_HOLSTER, false))
			{
				HomingModifier homingModifier = proj.gameObject.GetComponent<HomingModifier>();
				if (homingModifier == null)
				{
					homingModifier = proj.gameObject.AddComponent<HomingModifier>();
					homingModifier.HomingRadius = 0f;
					homingModifier.AngularVelocity = 0f;
				}
				homingModifier.HomingRadius += 20f;
				homingModifier.AngularVelocity += 1080f;
			}
			if (base.Owner.HasActiveBonusSynergy(CustomSynergyType.EXPLOSIVE_HOLSTER, false))
			{
				ExplosiveModifier explosiveModifier = proj.gameObject.GetComponent<ExplosiveModifier>();
				if (explosiveModifier == null)
				{
					explosiveModifier = proj.gameObject.AddComponent<ExplosiveModifier>();
					explosiveModifier.explosionData = new ExplosionData();
					explosiveModifier.explosionData.ignoreList = new List<SpeculativeRigidbody>();
					explosiveModifier.explosionData.CopyFrom(GameManager.Instance.Dungeon.sharedSettingsPrefab.DefaultSmallExplosionData);
					explosiveModifier.explosionData.damageToPlayer = 0f;
					explosiveModifier.explosionData.useDefaultExplosion = false;
				}
			}
		}
	}

	// Token: 0x0600744E RID: 29774 RVA: 0x002E4548 File Offset: 0x002E2748
	public override void Pickup(PlayerController player)
	{
		if (this.m_pickedUp)
		{
			return;
		}
		base.Pickup(player);
		player.OnReloadedGun = (Action<PlayerController, Gun>)Delegate.Combine(player.OnReloadedGun, new Action<PlayerController, Gun>(this.DoEffect));
	}

	// Token: 0x0600744F RID: 29775 RVA: 0x002E4580 File Offset: 0x002E2780
	private void DoEffect(PlayerController usingPlayer, Gun usedGun)
	{
		if (Time.realtimeSinceStartup - this.m_lastUsedTime < this.InternalCooldown)
		{
			return;
		}
		if (usedGun && usedGun.HasFiredHolsterShot)
		{
			return;
		}
		usedGun.HasFiredHolsterShot = true;
		this.m_lastUsedTime = Time.realtimeSinceStartup;
		if (UnityEngine.Random.value < this.ActivationChance && this.TriggersRadialBulletBurst)
		{
			this.RadialBurstSettings.DoBurst(usingPlayer, null, null);
		}
	}

	// Token: 0x06007450 RID: 29776 RVA: 0x002E4608 File Offset: 0x002E2808
	public override DebrisObject Drop(PlayerController player)
	{
		DebrisObject debrisObject = base.Drop(player);
		FireOnReloadItem component = debrisObject.GetComponent<FireOnReloadItem>();
		player.OnReloadedGun = (Action<PlayerController, Gun>)Delegate.Remove(player.OnReloadedGun, new Action<PlayerController, Gun>(this.DoEffect));
		component.m_pickedUpThisRun = true;
		return debrisObject;
	}

	// Token: 0x06007451 RID: 29777 RVA: 0x002E4650 File Offset: 0x002E2850
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x040075F0 RID: 30192
	public float ActivationChance = 1f;

	// Token: 0x040075F1 RID: 30193
	public float InternalCooldown = 1f;

	// Token: 0x040075F2 RID: 30194
	public bool TriggersRadialBulletBurst;

	// Token: 0x040075F3 RID: 30195
	[ShowInInspectorIf("TriggersRadialBulletBurst", false)]
	public RadialBurstInterface RadialBurstSettings;

	// Token: 0x040075F4 RID: 30196
	private float m_lastUsedTime;

	// Token: 0x040075F5 RID: 30197
	public bool IsHipHolster;
}
