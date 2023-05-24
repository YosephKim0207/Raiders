using System;
using UnityEngine;

// Token: 0x02001488 RID: 5256
public class RestoreAmmoToGunModifier : MonoBehaviour
{
	// Token: 0x06007782 RID: 30594 RVA: 0x002FA1C0 File Offset: 0x002F83C0
	private void Start()
	{
		this.m_projectile = base.GetComponent<Projectile>();
		Projectile projectile = this.m_projectile;
		projectile.OnHitEnemy = (Action<Projectile, SpeculativeRigidbody, bool>)Delegate.Combine(projectile.OnHitEnemy, new Action<Projectile, SpeculativeRigidbody, bool>(this.HandleHitEnemy));
	}

	// Token: 0x06007783 RID: 30595 RVA: 0x002FA1F8 File Offset: 0x002F83F8
	private void HandleHitEnemy(Projectile arg1, SpeculativeRigidbody arg2, bool arg3)
	{
		if (this.m_projectile.PossibleSourceGun && (!this.RequiresSynergy || arg1.PossibleSourceGun.OwnerHasSynergy(this.RequiredSynergy)) && UnityEngine.Random.value < this.ChanceToWork)
		{
			this.m_projectile.PossibleSourceGun.GainAmmo(this.AmmoToGain);
			if (!string.IsNullOrEmpty(this.RegainAmmoAnimation))
			{
				this.m_projectile.PossibleSourceGun.spriteAnimator.PlayForDuration(this.RegainAmmoAnimation, -1f, this.m_projectile.PossibleSourceGun.idleAnimation, false);
			}
		}
	}

	// Token: 0x04007973 RID: 31091
	public float ChanceToWork = 1f;

	// Token: 0x04007974 RID: 31092
	public int AmmoToGain = 1;

	// Token: 0x04007975 RID: 31093
	public bool RequiresSynergy;

	// Token: 0x04007976 RID: 31094
	[LongNumericEnum]
	public CustomSynergyType RequiredSynergy;

	// Token: 0x04007977 RID: 31095
	public string RegainAmmoAnimation;

	// Token: 0x04007978 RID: 31096
	private Projectile m_projectile;
}
