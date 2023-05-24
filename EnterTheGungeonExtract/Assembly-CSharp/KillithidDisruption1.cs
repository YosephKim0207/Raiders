using System;
using System.Collections;
using Brave.BulletScript;
using UnityEngine;

// Token: 0x0200021A RID: 538
public class KillithidDisruption1 : Script
{
	// Token: 0x06000815 RID: 2069 RVA: 0x000275BC File Offset: 0x000257BC
	protected override IEnumerator Top()
	{
		KillithidDisruption1.AstralBullet astralBullet = new KillithidDisruption1.AstralBullet();
		base.Fire(astralBullet);
		while (!astralBullet.Destroyed)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x0200021B RID: 539
	public class AstralBullet : Bullet
	{
		// Token: 0x06000816 RID: 2070 RVA: 0x000275D8 File Offset: 0x000257D8
		public AstralBullet()
			: base("disruption", false, false, false)
		{
		}

		// Token: 0x06000817 RID: 2071 RVA: 0x000275E8 File Offset: 0x000257E8
		protected override IEnumerator Top()
		{
			yield return base.Wait(113);
			if (base.BulletBank.aiActor.healthHaver.IsDead)
			{
				base.Vanish(false);
			}
			this.Projectile.specRigidbody.CollideWithOthers = true;
			this.Direction = base.AimDirection;
			this.Speed = 0f;
			int numShots = UnityEngine.Random.Range(2, 6);
			for (int i = 0; i < numShots; i++)
			{
				yield return base.Wait(UnityEngine.Random.Range(20, 70));
				if (base.BulletBank.aiActor.healthHaver.IsDead)
				{
					base.Vanish(false);
				}
				this.Projectile.spriteAnimator.PlayFromFrame("killithid_disruption_attack", 0);
				yield return base.Wait(15);
				base.Fire(new Direction(0f, DirectionType.Aim, -1f), new Speed(12f, SpeedType.Absolute), null);
			}
			yield return base.Wait(30);
			base.Vanish(false);
			yield break;
		}
	}
}
