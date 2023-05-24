using System;
using UnityEngine;

// Token: 0x02001376 RID: 4982
public class ClipBulletModifierItem : PassiveItem
{
	// Token: 0x060070E4 RID: 28900 RVA: 0x002CCF10 File Offset: 0x002CB110
	public override void Pickup(PlayerController player)
	{
		if (this.m_pickedUp)
		{
			return;
		}
		this.m_player = player;
		base.Pickup(player);
		player.PostProcessProjectile += this.PostProcessProjectile;
	}

	// Token: 0x060070E5 RID: 28901 RVA: 0x002CCF40 File Offset: 0x002CB140
	private void PostProcessProjectile(Projectile obj, float effectChanceScalar)
	{
		float activationChance = this.ActivationChance;
		if (UnityEngine.Random.value < activationChance)
		{
			if (this.FirstShotBoost && this.m_player.CurrentGun.LastShotIndex == 0)
			{
				obj.baseData.damage *= this.FirstShotMultiplier;
			}
			if (this.LastShotBoost && this.m_player.CurrentGun.LastShotIndex == this.m_player.CurrentGun.ClipCapacity - 1)
			{
				obj.baseData.damage *= this.LastShotMultiplier;
			}
		}
	}

	// Token: 0x060070E6 RID: 28902 RVA: 0x002CCFE4 File Offset: 0x002CB1E4
	public override DebrisObject Drop(PlayerController player)
	{
		DebrisObject debrisObject = base.Drop(player);
		this.m_player = null;
		debrisObject.GetComponent<ClipBulletModifierItem>().m_pickedUpThisRun = true;
		player.PostProcessProjectile -= this.PostProcessProjectile;
		return debrisObject;
	}

	// Token: 0x060070E7 RID: 28903 RVA: 0x002CD020 File Offset: 0x002CB220
	protected override void OnDestroy()
	{
		base.OnDestroy();
		if (this.m_player)
		{
			this.m_player.PostProcessProjectile -= this.PostProcessProjectile;
		}
	}

	// Token: 0x04007065 RID: 28773
	public float ActivationChance = 1f;

	// Token: 0x04007066 RID: 28774
	public bool FirstShotBoost;

	// Token: 0x04007067 RID: 28775
	public float FirstShotMultiplier = 2f;

	// Token: 0x04007068 RID: 28776
	public bool LastShotBoost;

	// Token: 0x04007069 RID: 28777
	public float LastShotMultiplier = 2f;

	// Token: 0x0400706A RID: 28778
	private PlayerController m_player;
}
