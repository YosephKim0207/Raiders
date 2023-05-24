using System;
using UnityEngine;

// Token: 0x0200149B RID: 5275
public class ScalingStatBoostItem : PassiveItem
{
	// Token: 0x06007809 RID: 30729 RVA: 0x002FF7F4 File Offset: 0x002FD9F4
	public override void Pickup(PlayerController player)
	{
		if (this.m_pickedUp)
		{
			return;
		}
		this.m_player = player;
		base.Pickup(player);
		player.PostProcessProjectile += this.PostProcessProjectile;
		player.PostProcessBeam += this.PostProcessBeam;
	}

	// Token: 0x0600780A RID: 30730 RVA: 0x002FF834 File Offset: 0x002FDA34
	private void PostProcessBeam(BeamController beam)
	{
		if (beam)
		{
			Projectile projectile = beam.projectile;
			if (projectile)
			{
				this.PostProcessProjectile(projectile, 1f);
			}
			beam.AdjustPlayerBeamTint(this.TintColor.WithAlpha(this.TintColor.a / 2f), this.TintPriority, 0f);
		}
	}

	// Token: 0x0600780B RID: 30731 RVA: 0x002FF898 File Offset: 0x002FDA98
	private void PostProcessProjectile(Projectile obj, float effectChanceScalar)
	{
		if (!this.m_player)
		{
			return;
		}
		float num = 0f;
		ScalingStatBoostItem.ScalingModeTarget scalingTarget = this.ScalingTarget;
		if (scalingTarget != ScalingStatBoostItem.ScalingModeTarget.CURRENCY)
		{
			if (scalingTarget == ScalingStatBoostItem.ScalingModeTarget.CURSE)
			{
				num = Mathf.Clamp01(Mathf.InverseLerp(this.ScalingTargetMin, this.ScalingTargetMax, this.m_player.stats.GetStatValue(PlayerStats.StatType.Curse)));
				num = this.ScalingCurve.Evaluate(num);
			}
		}
		else
		{
			num = Mathf.Clamp01(Mathf.InverseLerp(this.ScalingTargetMin, this.ScalingTargetMax, (float)this.m_player.carriedConsumables.Currency));
			num = this.ScalingCurve.Evaluate(num);
		}
		float num2 = Mathf.Lerp(this.MinScaling, this.MaxScaling, num);
		PlayerStats.StatType targetStat = this.TargetStat;
		if (targetStat == PlayerStats.StatType.Damage)
		{
			obj.baseData.damage *= num2;
		}
		if (this.TintBullets)
		{
			obj.AdjustPlayerProjectileTint(this.TintColor, this.TintPriority, 0f);
		}
		if (this.ScalingTarget == ScalingStatBoostItem.ScalingModeTarget.CURSE)
		{
			obj.CurseSparks = true;
		}
	}

	// Token: 0x0600780C RID: 30732 RVA: 0x002FF9C0 File Offset: 0x002FDBC0
	public override DebrisObject Drop(PlayerController player)
	{
		DebrisObject debrisObject = base.Drop(player);
		this.m_player = null;
		debrisObject.GetComponent<ScalingStatBoostItem>().m_pickedUpThisRun = true;
		player.PostProcessProjectile -= this.PostProcessProjectile;
		player.PostProcessBeam -= this.PostProcessBeam;
		return debrisObject;
	}

	// Token: 0x0600780D RID: 30733 RVA: 0x002FFA10 File Offset: 0x002FDC10
	protected override void OnDestroy()
	{
		base.OnDestroy();
		if (this.m_player)
		{
			this.m_player.PostProcessProjectile -= this.PostProcessProjectile;
			this.m_player.PostProcessBeam -= this.PostProcessBeam;
		}
	}

	// Token: 0x04007A27 RID: 31271
	public PlayerStats.StatType TargetStat = PlayerStats.StatType.Damage;

	// Token: 0x04007A28 RID: 31272
	public float MinScaling = 1f;

	// Token: 0x04007A29 RID: 31273
	public float MaxScaling = 2f;

	// Token: 0x04007A2A RID: 31274
	public float ScalingTargetMin;

	// Token: 0x04007A2B RID: 31275
	public float ScalingTargetMax = 500f;

	// Token: 0x04007A2C RID: 31276
	public bool TintBullets;

	// Token: 0x04007A2D RID: 31277
	public bool TintBeams;

	// Token: 0x04007A2E RID: 31278
	public Color TintColor = Color.yellow;

	// Token: 0x04007A2F RID: 31279
	public int TintPriority = 2;

	// Token: 0x04007A30 RID: 31280
	public AnimationCurve ScalingCurve;

	// Token: 0x04007A31 RID: 31281
	public ScalingStatBoostItem.ScalingModeTarget ScalingTarget;

	// Token: 0x04007A32 RID: 31282
	private PlayerController m_player;

	// Token: 0x0200149C RID: 5276
	public enum ScalingModeTarget
	{
		// Token: 0x04007A34 RID: 31284
		CURRENCY,
		// Token: 0x04007A35 RID: 31285
		CURSE
	}
}
