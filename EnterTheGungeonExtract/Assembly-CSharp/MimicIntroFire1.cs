using System;
using System.Collections;
using Brave.BulletScript;

// Token: 0x0200028D RID: 653
public class MimicIntroFire1 : Script
{
	// Token: 0x060009FF RID: 2559 RVA: 0x000306B8 File Offset: 0x0002E8B8
	protected override IEnumerator Top()
	{
		bool flag = base.BulletBank && base.BulletBank.aiActor && base.BulletBank.aiActor.IsBlackPhantom;
		base.Fire(new Direction(base.AimDirection, DirectionType.Absolute, -1f), new Speed(8f, SpeedType.Absolute), new MimicIntroFire1.BigBullet(flag));
		return null;
	}

	// Token: 0x0200028E RID: 654
	public class BigBullet : Bullet
	{
		// Token: 0x06000A00 RID: 2560 RVA: 0x00030728 File Offset: 0x0002E928
		public BigBullet(bool isBlackPhantom)
			: base("bigbullet", false, false, false)
		{
			base.ForceBlackBullet = true;
			this.m_isBlackPhantom = isBlackPhantom;
		}

		// Token: 0x06000A01 RID: 2561 RVA: 0x00030748 File Offset: 0x0002E948
		protected override IEnumerator Top()
		{
			yield return base.Wait(80);
			base.Vanish(false);
			yield break;
		}

		// Token: 0x06000A02 RID: 2562 RVA: 0x00030764 File Offset: 0x0002E964
		public override void OnBulletDestruction(Bullet.DestroyType destroyType, SpeculativeRigidbody hitRigidbody, bool preventSpawningProjectiles)
		{
			if (preventSpawningProjectiles)
			{
				return;
			}
			for (int i = 0; i < 8; i++)
			{
				Bullet bullet = new SpeedChangingBullet(10f, 120, 600);
				base.Fire(new Direction((float)(i * 45), DirectionType.Absolute, -1f), new Speed(8f, SpeedType.Absolute), bullet);
				if (!this.m_isBlackPhantom)
				{
					bullet.ForceBlackBullet = false;
					bullet.Projectile.ForceBlackBullet = false;
					bullet.Projectile.ReturnFromBlackBullet();
				}
			}
		}

		// Token: 0x04000A4F RID: 2639
		private bool m_isBlackPhantom;
	}
}
