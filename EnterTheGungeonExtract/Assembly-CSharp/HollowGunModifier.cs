using System;
using UnityEngine;

// Token: 0x020016F1 RID: 5873
public class HollowGunModifier : MonoBehaviour
{
	// Token: 0x06008881 RID: 34945 RVA: 0x00389400 File Offset: 0x00387600
	public void Awake()
	{
		this.m_gun = base.GetComponent<Gun>();
		Gun gun = this.m_gun;
		gun.PostProcessProjectile = (Action<Projectile>)Delegate.Combine(gun.PostProcessProjectile, new Action<Projectile>(this.HandleProcessProjectile));
	}

	// Token: 0x06008882 RID: 34946 RVA: 0x00389438 File Offset: 0x00387638
	private void HandleProcessProjectile(Projectile obj)
	{
		if (this.m_gun.CurrentOwner is PlayerController)
		{
			PlayerController playerController = this.m_gun.CurrentOwner as PlayerController;
			if (playerController.IsDarkSoulsHollow)
			{
				obj.baseData.damage *= this.DamageMultiplier;
			}
		}
	}

	// Token: 0x04008DE6 RID: 36326
	public float DamageMultiplier = 1.5f;

	// Token: 0x04008DE7 RID: 36327
	private Gun m_gun;
}
