using System;

// Token: 0x02000E5F RID: 3679
[Serializable]
public class DamageTypeModifier
{
	// Token: 0x06004E52 RID: 20050 RVA: 0x001B153C File Offset: 0x001AF73C
	public DamageTypeModifier()
	{
	}

	// Token: 0x06004E53 RID: 20051 RVA: 0x001B1550 File Offset: 0x001AF750
	public DamageTypeModifier(DamageTypeModifier other)
	{
		this.damageType = other.damageType;
		this.damageMultiplier = other.damageMultiplier;
	}

	// Token: 0x040044B8 RID: 17592
	public CoreDamageTypes damageType;

	// Token: 0x040044B9 RID: 17593
	public float damageMultiplier = 1f;
}
