using System;

// Token: 0x020013BE RID: 5054
public class InfiniteBulletsModifier : BraveBehaviour
{
	// Token: 0x06007290 RID: 29328 RVA: 0x002D8CE8 File Offset: 0x002D6EE8
	public void Start()
	{
		Projectile projectile = base.projectile;
		projectile.OnDestruction += this.HandleDestruction;
	}

	// Token: 0x06007291 RID: 29329 RVA: 0x002D8D10 File Offset: 0x002D6F10
	private void HandleDestruction(Projectile p)
	{
		if (!p.HasImpactedEnemy && p.PossibleSourceGun && p.PossibleSourceGun.gameObject.activeSelf)
		{
			p.PossibleSourceGun.GainAmmo(1);
			p.PossibleSourceGun.ForceFireProjectile(p);
		}
	}
}
