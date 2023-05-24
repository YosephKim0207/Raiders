using System;

// Token: 0x02001412 RID: 5138
public class GunClassPassiveItem : PassiveItem
{
	// Token: 0x06007498 RID: 29848 RVA: 0x002E6BDC File Offset: 0x002E4DDC
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

	// Token: 0x06007499 RID: 29849 RVA: 0x002E6C1C File Offset: 0x002E4E1C
	private void PostProcessBeam(BeamController obj)
	{
		if (!this.m_player || !this.m_player.CurrentGun || this.damageModifiers == null || !obj || !obj.projectile)
		{
			return;
		}
		for (int i = 0; i < this.classesToModify.Length; i++)
		{
			if (this.m_player.CurrentGun.gunClass == this.classesToModify[i])
			{
				obj.projectile.baseData.damage *= this.damageModifiers[i];
			}
		}
	}

	// Token: 0x0600749A RID: 29850 RVA: 0x002E6CCC File Offset: 0x002E4ECC
	private void PostProcessProjectile(Projectile obj, float effectChanceScalar)
	{
		for (int i = 0; i < this.classesToModify.Length; i++)
		{
			if (this.m_player.CurrentGun != null && this.m_player.CurrentGun.gunClass == this.classesToModify[i])
			{
				obj.baseData.damage *= this.damageModifiers[i];
			}
		}
	}

	// Token: 0x0600749B RID: 29851 RVA: 0x002E6D40 File Offset: 0x002E4F40
	public override DebrisObject Drop(PlayerController player)
	{
		DebrisObject debrisObject = base.Drop(player);
		this.m_player = null;
		debrisObject.GetComponent<GunClassPassiveItem>().m_pickedUpThisRun = true;
		player.PostProcessProjectile -= this.PostProcessProjectile;
		return debrisObject;
	}

	// Token: 0x0600749C RID: 29852 RVA: 0x002E6D7C File Offset: 0x002E4F7C
	protected override void OnDestroy()
	{
		base.OnDestroy();
		if (this.m_player)
		{
			this.m_player.PostProcessProjectile -= this.PostProcessProjectile;
		}
	}

	// Token: 0x0400767C RID: 30332
	public GunClass[] classesToModify;

	// Token: 0x0400767D RID: 30333
	public float[] damageModifiers;

	// Token: 0x0400767E RID: 30334
	private PlayerController m_player;
}
