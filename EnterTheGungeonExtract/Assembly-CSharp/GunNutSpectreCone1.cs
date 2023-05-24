using System;
using System.Collections;
using Brave.BulletScript;
using FullInspector;
using UnityEngine;

// Token: 0x020001C3 RID: 451
[InspectorDropdownName("GunNut/SpectreCone1")]
public class GunNutSpectreCone1 : Script
{
	// Token: 0x060006BF RID: 1727 RVA: 0x00020CC8 File Offset: 0x0001EEC8
	protected override IEnumerator Top()
	{
		for (int i = 0; i < 25; i++)
		{
			float num = -45f + (float)i * 3.75f;
			base.Fire(new Offset(0.5f, 0f, this.Direction + num, string.Empty, DirectionType.Absolute), new Direction(num, DirectionType.Relative, -1f), new Speed(10f, SpeedType.Absolute), null);
		}
		float desiredAngle = base.GetAimDirection(1f, 12f);
		float angle = Mathf.MoveTowardsAngle(this.Direction, desiredAngle, 40f);
		bool isBlackPhantom = base.BulletBank && base.BulletBank.aiActor && base.BulletBank.aiActor.IsBlackPhantom;
		Bullet bullet = new GunNutSpectreCone1.BurstingBullet(isBlackPhantom);
		base.Fire(new Direction(angle, DirectionType.Absolute, -1f), new Speed(12f, SpeedType.Absolute), bullet);
		yield return null;
		yield break;
	}

	// Token: 0x04000696 RID: 1686
	private const int NumBulletsMainWave = 25;

	// Token: 0x020001C4 RID: 452
	public class BurstingBullet : Bullet
	{
		// Token: 0x060006C0 RID: 1728 RVA: 0x00020CE4 File Offset: 0x0001EEE4
		public BurstingBullet(bool isBlackPhantom)
			: base("bigBullet", false, false, false)
		{
			base.ForceBlackBullet = true;
			this.m_isBlackPhantom = isBlackPhantom;
		}

		// Token: 0x060006C1 RID: 1729 RVA: 0x00020D04 File Offset: 0x0001EF04
		public override void OnBulletDestruction(Bullet.DestroyType destroyType, SpeculativeRigidbody hitRigidbody, bool preventSpawningProjectiles)
		{
			if (preventSpawningProjectiles)
			{
				return;
			}
			float num = base.RandomAngle();
			float num2 = 20f;
			for (int i = 0; i < 18; i++)
			{
				Bullet bullet = new Bullet(null, false, false, false);
				base.Fire(new Direction(num + (float)i * num2, DirectionType.Absolute, -1f), new Speed(7f, SpeedType.Absolute), bullet);
				if (!this.m_isBlackPhantom)
				{
					bullet.ForceBlackBullet = false;
					bullet.Projectile.ForceBlackBullet = false;
					bullet.Projectile.ReturnFromBlackBullet();
				}
			}
		}

		// Token: 0x04000697 RID: 1687
		private const int NumBullets = 18;

		// Token: 0x04000698 RID: 1688
		private bool m_isBlackPhantom;
	}
}
