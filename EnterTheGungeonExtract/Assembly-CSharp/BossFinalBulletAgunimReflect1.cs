using System;
using System.Collections;
using Brave.BulletScript;
using FullInspector;
using UnityEngine;

// Token: 0x02000063 RID: 99
[InspectorDropdownName("Bosses/BossFinalBullet/AgunimReflect1")]
public class BossFinalBulletAgunimReflect1 : Script
{
	// Token: 0x0600017F RID: 383 RVA: 0x00008098 File Offset: 0x00006298
	protected override IEnumerator Top()
	{
		yield return base.Wait(48);
		if (!BossFinalBulletAgunimReflect1.WasLastShotFake && UnityEngine.Random.value < 0.33f)
		{
			BossFinalBulletAgunimReflect1.WasLastShotFake = true;
			for (int i = 0; i < 5; i++)
			{
				BossFinalBulletAgunimReflect1.RingBullet ringBullet = new BossFinalBulletAgunimReflect1.RingBullet(base.SubdivideCircle(0f, 5, i, 1f, false));
				base.Fire(new Direction(base.AimDirection, DirectionType.Absolute, -1f), new Speed(10f, SpeedType.Absolute), ringBullet);
				ringBullet.Projectile.IgnoreTileCollisionsFor(0.6f);
			}
			yield return base.Wait(60);
		}
		else
		{
			BossFinalBulletAgunimReflect1.WasLastShotFake = false;
			Bullet bullet = new Bullet("reflect", false, false, false);
			base.Fire(new Direction(0f, DirectionType.Aim, -1f), new Speed(12f, SpeedType.Absolute), bullet);
			bullet.Projectile.IsReflectedBySword = true;
			bullet.Projectile.IgnoreTileCollisionsFor(0.6f);
			do
			{
				yield return base.Wait(1);
			}
			while (bullet.Projectile && !bullet.Destroyed);
			yield return base.Wait(24);
		}
		yield break;
	}

	// Token: 0x04000198 RID: 408
	private const float FakeChance = 0.33f;

	// Token: 0x04000199 RID: 409
	private static bool WasLastShotFake;

	// Token: 0x0400019A RID: 410
	private const int FakeNumBullets = 5;

	// Token: 0x0400019B RID: 411
	private const float FakeRadius = 0.55f;

	// Token: 0x0400019C RID: 412
	private const float FakeSpinSpeed = 450f;

	// Token: 0x02000064 RID: 100
	public class RingBullet : Bullet
	{
		// Token: 0x06000181 RID: 385 RVA: 0x000080B8 File Offset: 0x000062B8
		public RingBullet(float angle = 0f)
			: base("ring", false, false, false)
		{
			this.m_angle = angle;
		}

		// Token: 0x06000182 RID: 386 RVA: 0x000080D0 File Offset: 0x000062D0
		protected override IEnumerator Top()
		{
			base.ManualControl = true;
			this.Projectile.IgnoreTileCollisionsFor(0.6f);
			Vector2 centerPosition = base.Position;
			for (int i = 0; i < 300; i++)
			{
				base.UpdateVelocity();
				centerPosition += this.Velocity / 60f;
				this.m_angle += 7.5f;
				base.Position = centerPosition + BraveMathCollege.DegreesToVector(this.m_angle, 0.55f);
				yield return base.Wait(1);
			}
			base.Vanish(false);
			yield break;
		}

		// Token: 0x0400019D RID: 413
		private float m_angle;
	}
}
