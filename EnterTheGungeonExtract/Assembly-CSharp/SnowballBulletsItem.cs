using System;

// Token: 0x020013E7 RID: 5095
public class SnowballBulletsItem : PassiveItem
{
	// Token: 0x0600739F RID: 29599 RVA: 0x002DFE78 File Offset: 0x002DE078
	public override void Pickup(PlayerController player)
	{
		if (this.m_pickedUp)
		{
			return;
		}
		this.m_player = player;
		base.Pickup(player);
		player.PostProcessProjectile += this.HandleProjectile;
		player.PostProcessBeamChanceTick += this.HandleBeamFrame;
	}

	// Token: 0x060073A0 RID: 29600 RVA: 0x002DFEB8 File Offset: 0x002DE0B8
	private void HandleBeamFrame(BeamController sourceBeam)
	{
		if (sourceBeam is BasicBeamController)
		{
			BasicBeamController basicBeamController = sourceBeam as BasicBeamController;
			basicBeamController.ProjectileScale = (basicBeamController.Owner as PlayerController).BulletScaleModifier + basicBeamController.ApproximateDistance * (this.PercentScaleGainPerUnit / 100f);
		}
	}

	// Token: 0x060073A1 RID: 29601 RVA: 0x002DFF04 File Offset: 0x002DE104
	private void HandleProjectile(Projectile targetProjectile, float arg2)
	{
		ScalingProjectileModifier scalingProjectileModifier = targetProjectile.gameObject.AddComponent<ScalingProjectileModifier>();
		scalingProjectileModifier.ScaleToDamageRatio = this.PercentDamageGainPerUnit / this.PercentScaleGainPerUnit;
		scalingProjectileModifier.MaximumDamageMultiplier = this.DamageMultiplierCap;
		scalingProjectileModifier.IsSynergyContingent = false;
		if (base.Owner.HasActiveBonusSynergy(CustomSynergyType.SNOWBREAKERS, false))
		{
			scalingProjectileModifier.PercentGainPerUnit = this.PercentScaleGainPerUnit * 1.5f;
			scalingProjectileModifier.ScaleMultiplier = 2f;
		}
		else
		{
			scalingProjectileModifier.PercentGainPerUnit = this.PercentScaleGainPerUnit;
		}
	}

	// Token: 0x060073A2 RID: 29602 RVA: 0x002DFF88 File Offset: 0x002DE188
	public override DebrisObject Drop(PlayerController player)
	{
		DebrisObject debrisObject = base.Drop(player);
		this.m_player = null;
		debrisObject.GetComponent<SnowballBulletsItem>().m_pickedUpThisRun = true;
		player.PostProcessProjectile -= this.HandleProjectile;
		player.PostProcessBeamChanceTick -= this.HandleBeamFrame;
		return debrisObject;
	}

	// Token: 0x060073A3 RID: 29603 RVA: 0x002DFFD8 File Offset: 0x002DE1D8
	protected override void OnDestroy()
	{
		base.OnDestroy();
		if (this.m_player)
		{
			this.m_player.PostProcessProjectile -= this.HandleProjectile;
			this.m_player.PostProcessBeamChanceTick -= this.HandleBeamFrame;
		}
	}

	// Token: 0x04007538 RID: 30008
	public float PercentScaleGainPerUnit = 10f;

	// Token: 0x04007539 RID: 30009
	public float PercentDamageGainPerUnit = 2.5f;

	// Token: 0x0400753A RID: 30010
	public float DamageMultiplierCap = 2.5f;

	// Token: 0x0400753B RID: 30011
	private PlayerController m_player;
}
