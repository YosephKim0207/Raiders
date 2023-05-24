using System;
using UnityEngine;

// Token: 0x0200170B RID: 5899
public class ScalingProjectileModifier : MonoBehaviour
{
	// Token: 0x06008923 RID: 35107 RVA: 0x0038E3B4 File Offset: 0x0038C5B4
	public void Start()
	{
		this.m_projectile = base.GetComponent<Projectile>();
		if (this.IsSynergyContingent)
		{
			if (!this.m_projectile.PossibleSourceGun || !(this.m_projectile.PossibleSourceGun.CurrentOwner is PlayerController))
			{
				return;
			}
			if (!(this.m_projectile.PossibleSourceGun.CurrentOwner as PlayerController).HasActiveBonusSynergy(this.SynergyToTest, false))
			{
				return;
			}
		}
		this.m_projectile.specRigidbody.UpdateCollidersOnScale = true;
		this.m_projectile.OnPostUpdate += this.HandlePostUpdate;
	}

	// Token: 0x06008924 RID: 35108 RVA: 0x0038E458 File Offset: 0x0038C658
	public virtual void OnDespawned()
	{
		if (this.m_projectile)
		{
			this.m_projectile.RuntimeUpdateScale(1f / this.m_projectile.AdditionalScaleMultiplier);
			this.m_projectile.baseData.damage = this.m_projectile.baseData.damage / this.m_elapsedDamageGain;
		}
		UnityEngine.Object.Destroy(this);
	}

	// Token: 0x06008925 RID: 35109 RVA: 0x0038E4C0 File Offset: 0x0038C6C0
	private void HandlePostUpdate(Projectile proj)
	{
		if (!proj)
		{
			return;
		}
		float elapsedDistance = proj.GetElapsedDistance();
		if (elapsedDistance < this.m_lastElapsedDistance)
		{
			this.m_lastElapsedDistance = 0f;
			this.m_totalElapsedDistance = 0f;
		}
		this.m_totalElapsedDistance += elapsedDistance - this.m_lastElapsedDistance;
		this.m_lastElapsedDistance = elapsedDistance;
		this.m_totalElapsedDistance = Mathf.Clamp(this.m_totalElapsedDistance, 0f, 160f);
		float num = 1f + this.m_totalElapsedDistance / 100f * this.PercentGainPerUnit;
		float num2 = (num - 1f) * this.ScaleToDamageRatio + 1f;
		float num3 = ((this.MaximumDamageMultiplier <= 0f) ? (num2 * this.DamageMultiplier) : Mathf.Min(this.MaximumDamageMultiplier, num2 * this.DamageMultiplier));
		float num4 = num * this.ScaleMultiplier / this.m_elapsedSizeGain;
		if (num4 > 1.25f)
		{
			this.m_projectile.RuntimeUpdateScale(num * this.ScaleMultiplier / this.m_elapsedSizeGain);
			this.m_elapsedSizeGain = num * this.ScaleMultiplier;
		}
		this.m_projectile.baseData.damage *= num3 / this.m_elapsedDamageGain;
		this.m_elapsedDamageGain = num3;
	}

	// Token: 0x04008EF9 RID: 36601
	public bool IsSynergyContingent;

	// Token: 0x04008EFA RID: 36602
	[LongNumericEnum]
	public CustomSynergyType SynergyToTest;

	// Token: 0x04008EFB RID: 36603
	public float PercentGainPerUnit = 2f;

	// Token: 0x04008EFC RID: 36604
	[NonSerialized]
	public float ScaleMultiplier = 1f;

	// Token: 0x04008EFD RID: 36605
	[NonSerialized]
	public float DamageMultiplier = 1f;

	// Token: 0x04008EFE RID: 36606
	public float MaximumDamageMultiplier = -1f;

	// Token: 0x04008EFF RID: 36607
	[NonSerialized]
	public float ScaleToDamageRatio = 1f;

	// Token: 0x04008F00 RID: 36608
	private Projectile m_projectile;

	// Token: 0x04008F01 RID: 36609
	private float m_lastElapsedDistance;

	// Token: 0x04008F02 RID: 36610
	private float m_totalElapsedDistance;

	// Token: 0x04008F03 RID: 36611
	private float m_elapsedSizeGain = 1f;

	// Token: 0x04008F04 RID: 36612
	private float m_elapsedDamageGain = 1f;
}
