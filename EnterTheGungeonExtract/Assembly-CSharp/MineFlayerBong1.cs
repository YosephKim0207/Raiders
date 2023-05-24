using System;
using System.Collections;
using Brave.BulletScript;
using FullInspector;

// Token: 0x02000295 RID: 661
[InspectorDropdownName("Bosses/MineFlayer/Bong1")]
public class MineFlayerBong1 : Script
{
	// Token: 0x06000A1D RID: 2589 RVA: 0x00030D6C File Offset: 0x0002EF6C
	protected override IEnumerator Top()
	{
		float startDirection = base.RandomAngle();
		float delta = 9.75f;
		for (int i = 0; i < 90; i++)
		{
			Bullet bullet = new Bullet(null, false, false, false);
			Bullet bullet2 = new Bullet(null, false, false, false);
			base.Fire(new Direction(startDirection + (float)i * delta, DirectionType.Absolute, -1f), new Speed(12f, SpeedType.Absolute), bullet);
			base.Fire(new Direction(startDirection + (float)i * delta + 180f, DirectionType.Absolute, -1f), new Speed(12f, SpeedType.Absolute), bullet2);
			bullet.Projectile.IgnoreTileCollisionsFor(0.4f);
			bullet2.Projectile.IgnoreTileCollisionsFor(0.4f);
			yield return base.Wait(1);
		}
		yield break;
	}

	// Token: 0x04000A64 RID: 2660
	private const int NumBullets = 90;
}
