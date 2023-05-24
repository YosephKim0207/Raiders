using System;
using UnityEngine;

// Token: 0x0200141E RID: 5150
public class HomingBulletsPassiveItem : PassiveItem
{
	// Token: 0x060074E2 RID: 29922 RVA: 0x002E8B54 File Offset: 0x002E6D54
	public override void Pickup(PlayerController player)
	{
		if (this.m_pickedUp)
		{
			return;
		}
		this.m_player = player;
		base.Pickup(player);
		player.PostProcessProjectile += this.PostProcessProjectile;
		player.PostProcessBeamChanceTick += this.PostProcessBeamChanceTick;
	}

	// Token: 0x060074E3 RID: 29923 RVA: 0x002E8B94 File Offset: 0x002E6D94
	private void PostProcessProjectile(Projectile obj, float effectChanceScalar)
	{
		if (UnityEngine.Random.value > this.ActivationChance * effectChanceScalar)
		{
			if (this.SynergyIncreasesDamageIfNotActive && this.m_player && this.m_player.HasActiveBonusSynergy(this.SynergyRequired, false))
			{
				obj.baseData.damage *= this.SynergyDamageMultiplier;
				obj.RuntimeUpdateScale(this.SynergyDamageMultiplier);
			}
			return;
		}
		HomingModifier homingModifier = obj.gameObject.GetComponent<HomingModifier>();
		if (homingModifier == null)
		{
			homingModifier = obj.gameObject.AddComponent<HomingModifier>();
			homingModifier.HomingRadius = 0f;
			homingModifier.AngularVelocity = 0f;
		}
		float num = ((!this.SynergyIncreasesDamageIfNotActive || !this.m_player || !this.m_player.HasActiveBonusSynergy(this.SynergyRequired, false)) ? 1f : 2f);
		homingModifier.HomingRadius += this.homingRadius * num;
		homingModifier.AngularVelocity += this.homingAngularVelocity * num;
	}

	// Token: 0x060074E4 RID: 29924 RVA: 0x002E8CB0 File Offset: 0x002E6EB0
	private void PostProcessBeamChanceTick(BeamController beam)
	{
		if (UnityEngine.Random.value > this.ActivationChance)
		{
			return;
		}
		beam.ChanceBasedHomingRadius += this.homingRadius;
		beam.ChanceBasedHomingAngularVelocity += this.homingAngularVelocity;
	}

	// Token: 0x060074E5 RID: 29925 RVA: 0x002E8CEC File Offset: 0x002E6EEC
	public override DebrisObject Drop(PlayerController player)
	{
		DebrisObject debrisObject = base.Drop(player);
		debrisObject.GetComponent<HomingBulletsPassiveItem>().m_pickedUpThisRun = true;
		this.m_player = null;
		player.PostProcessProjectile -= this.PostProcessProjectile;
		player.PostProcessBeamChanceTick -= this.PostProcessBeamChanceTick;
		return debrisObject;
	}

	// Token: 0x060074E6 RID: 29926 RVA: 0x002E8D3C File Offset: 0x002E6F3C
	protected override void OnDestroy()
	{
		base.OnDestroy();
		if (this.m_player)
		{
			this.m_player.PostProcessProjectile -= this.PostProcessProjectile;
			this.m_player.PostProcessBeamChanceTick -= this.PostProcessBeamChanceTick;
		}
	}

	// Token: 0x040076B7 RID: 30391
	public float ActivationChance = 1f;

	// Token: 0x040076B8 RID: 30392
	public float homingRadius = 5f;

	// Token: 0x040076B9 RID: 30393
	public float homingAngularVelocity = 360f;

	// Token: 0x040076BA RID: 30394
	public bool SynergyIncreasesDamageIfNotActive;

	// Token: 0x040076BB RID: 30395
	[LongNumericEnum]
	public CustomSynergyType SynergyRequired;

	// Token: 0x040076BC RID: 30396
	public float SynergyDamageMultiplier = 1.5f;

	// Token: 0x040076BD RID: 30397
	protected PlayerController m_player;
}
