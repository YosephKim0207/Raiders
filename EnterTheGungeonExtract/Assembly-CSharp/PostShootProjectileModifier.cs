using System;
using UnityEngine;

// Token: 0x02001469 RID: 5225
public class PostShootProjectileModifier : MonoBehaviour
{
	// Token: 0x060076D2 RID: 30418 RVA: 0x002F5D14 File Offset: 0x002F3F14
	private void Start()
	{
		Gun component = base.GetComponent<Gun>();
		Gun gun = component;
		gun.PostProcessProjectile = (Action<Projectile>)Delegate.Combine(gun.PostProcessProjectile, new Action<Projectile>(this.PostProcessProjectile));
	}

	// Token: 0x060076D3 RID: 30419 RVA: 0x002F5D4C File Offset: 0x002F3F4C
	private void PostProcessProjectile(Projectile obj)
	{
		BounceProjModifier component = obj.GetComponent<BounceProjModifier>();
		if (component)
		{
			component.numberOfBounces = this.NumberBouncesToSet;
		}
	}

	// Token: 0x040078C8 RID: 30920
	public int NumberBouncesToSet;
}
