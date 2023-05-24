using System;

// Token: 0x02001323 RID: 4899
public class ActiveAmmunitionData
{
	// Token: 0x06006F04 RID: 28420 RVA: 0x002C0568 File Offset: 0x002BE768
	public void HandleAmmunition(Projectile p, Gun g)
	{
		p.baseData.damage *= this.DamageModifier;
		p.baseData.speed *= this.SpeedModifier;
		p.baseData.range *= this.RangeModifier;
		if (this.OnAmmoModification != null)
		{
			this.OnAmmoModification(p, g);
		}
	}

	// Token: 0x04006CBE RID: 27838
	public int ShotsRemaining;

	// Token: 0x04006CBF RID: 27839
	public float DamageModifier = 1f;

	// Token: 0x04006CC0 RID: 27840
	public float SpeedModifier = 1f;

	// Token: 0x04006CC1 RID: 27841
	public float RangeModifier = 1f;

	// Token: 0x04006CC2 RID: 27842
	public Action<Projectile, Gun> OnAmmoModification;
}
