using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001657 RID: 5719
public class MaxNumberAliveModifier : MonoBehaviour
{
	// Token: 0x0600857D RID: 34173 RVA: 0x0037141C File Offset: 0x0036F61C
	private void Start()
	{
		Gun component = base.GetComponent<Gun>();
		this.m_aliveProjectiles = new List<Projectile>();
		Gun gun = component;
		gun.PostProcessProjectile = (Action<Projectile>)Delegate.Combine(gun.PostProcessProjectile, new Action<Projectile>(this.HandleProjectileFired));
	}

	// Token: 0x0600857E RID: 34174 RVA: 0x00371460 File Offset: 0x0036F660
	private void HandleProjectileFired(Projectile obj)
	{
		this.m_aliveProjectiles.Add(obj);
		this.CompactList();
	}

	// Token: 0x0600857F RID: 34175 RVA: 0x00371474 File Offset: 0x0036F674
	private void CompactList()
	{
		for (int i = 0; i < this.m_aliveProjectiles.Count; i++)
		{
			if (!this.m_aliveProjectiles[i])
			{
				this.m_aliveProjectiles.RemoveAt(i);
				i--;
			}
		}
		while (this.m_aliveProjectiles.Count > this.MaxNumberAlive)
		{
			Projectile projectile = this.m_aliveProjectiles[0];
			this.m_aliveProjectiles.RemoveAt(0);
			projectile.DieInAir(false, true, true, false);
		}
	}

	// Token: 0x040089BB RID: 35259
	private List<Projectile> m_aliveProjectiles;

	// Token: 0x040089BC RID: 35260
	public int MaxNumberAlive;
}
