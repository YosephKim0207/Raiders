using System;
using UnityEngine;

// Token: 0x0200143C RID: 5180
public class MindControlProjectileModifier : MonoBehaviour
{
	// Token: 0x06007592 RID: 30098 RVA: 0x002ED4B8 File Offset: 0x002EB6B8
	private void Start()
	{
		Projectile component = base.GetComponent<Projectile>();
		if (component)
		{
			Projectile projectile = component;
			projectile.OnHitEnemy = (Action<Projectile, SpeculativeRigidbody, bool>)Delegate.Combine(projectile.OnHitEnemy, new Action<Projectile, SpeculativeRigidbody, bool>(this.HandleHitEnemy));
		}
	}

	// Token: 0x06007593 RID: 30099 RVA: 0x002ED4FC File Offset: 0x002EB6FC
	private void HandleHitEnemy(Projectile arg1, SpeculativeRigidbody arg2, bool arg3)
	{
		if (arg2 && arg2.aiActor)
		{
			AIActor aiActor = arg2.aiActor;
			if (aiActor.IsNormalEnemy && !aiActor.healthHaver.IsBoss && !aiActor.IsHarmlessEnemy && !aiActor.gameObject.GetComponent<MindControlEffect>())
			{
				MindControlEffect orAddComponent = aiActor.gameObject.GetOrAddComponent<MindControlEffect>();
				orAddComponent.owner = arg1.Owner as PlayerController;
			}
		}
	}
}
