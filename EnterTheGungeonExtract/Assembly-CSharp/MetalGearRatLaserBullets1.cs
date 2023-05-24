using System;
using System.Collections;
using Brave.BulletScript;
using FullInspector;
using UnityEngine;

// Token: 0x02000260 RID: 608
[InspectorDropdownName("Bosses/MetalGearRat/LaserBullets1")]
public class MetalGearRatLaserBullets1 : Script
{
	// Token: 0x06000931 RID: 2353 RVA: 0x0002CAB8 File Offset: 0x0002ACB8
	protected override IEnumerator Top()
	{
		AIBeamShooter[] beams = base.BulletBank.GetComponents<AIBeamShooter>();
		for (;;)
		{
			yield return base.Wait(25);
			if (beams == null || beams.Length == 0)
			{
				break;
			}
			AIBeamShooter beam = beams[UnityEngine.Random.Range(1, beams.Length)];
			if (beam && beam.LaserBeam)
			{
				Vector2 vector = beam.LaserBeam.Origin + beam.LaserBeam.Direction.normalized * beam.MaxBeamLength;
				base.Fire(Offset.OverridePosition(vector), new MetalGearRatLaserBullets1.LaserBullet());
			}
		}
		yield break;
		yield break;
	}

	// Token: 0x04000957 RID: 2391
	private const int NumBullets = 12;

	// Token: 0x02000261 RID: 609
	public class LaserBullet : Bullet
	{
		// Token: 0x06000932 RID: 2354 RVA: 0x0002CAD4 File Offset: 0x0002ACD4
		public LaserBullet()
			: base(null, false, false, false)
		{
		}

		// Token: 0x06000933 RID: 2355 RVA: 0x0002CAE0 File Offset: 0x0002ACE0
		protected override IEnumerator Top()
		{
			this.Projectile.IgnoreTileCollisionsFor(1.25f);
			yield return base.Wait(60);
			this.Direction = base.AimDirection;
			base.ChangeSpeed(new Speed(11f, SpeedType.Absolute), 30);
			yield break;
		}
	}
}
