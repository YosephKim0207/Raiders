using System;
using UnityEngine;

// Token: 0x020013F9 RID: 5113
public class EnemyBulletSpeedItem : PassiveItem
{
	// Token: 0x0600740C RID: 29708 RVA: 0x002E306C File Offset: 0x002E126C
	public override void Pickup(PlayerController player)
	{
		if (this.m_pickedUp)
		{
			return;
		}
		base.Pickup(player);
		Projectile.BaseEnemyBulletSpeedMultiplier *= Mathf.Clamp01(1f - this.percentageSpeedReduction);
	}

	// Token: 0x0600740D RID: 29709 RVA: 0x002E30A0 File Offset: 0x002E12A0
	public override DebrisObject Drop(PlayerController player)
	{
		DebrisObject debrisObject = base.Drop(player);
		debrisObject.GetComponent<EnemyBulletSpeedItem>().m_pickedUpThisRun = true;
		Projectile.BaseEnemyBulletSpeedMultiplier /= Mathf.Clamp01(1f - this.percentageSpeedReduction);
		return debrisObject;
	}

	// Token: 0x0600740E RID: 29710 RVA: 0x002E30E0 File Offset: 0x002E12E0
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x040075A6 RID: 30118
	public float percentageSpeedReduction = 0.25f;
}
