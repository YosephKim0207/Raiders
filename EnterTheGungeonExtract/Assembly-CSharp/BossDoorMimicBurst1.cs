using System;
using System.Collections;
using Brave.BulletScript;
using FullInspector;

// Token: 0x0200004C RID: 76
[InspectorDropdownName("Bosses/BossDoorMimic/Burst1")]
public class BossDoorMimicBurst1 : Script
{
	// Token: 0x06000125 RID: 293 RVA: 0x000069FC File Offset: 0x00004BFC
	protected override IEnumerator Top()
	{
		float startDirection = -60f;
		float delta = 10f;
		for (int i = 0; i < 36; i++)
		{
			base.Fire(new Direction(startDirection + (float)i * delta, DirectionType.Absolute, -1f), new Speed(6f, SpeedType.Absolute), new BossDoorMimicBurst1.SpinBullet());
		}
		yield return base.Wait(30);
		yield break;
	}

	// Token: 0x04000128 RID: 296
	private const int NumBullets = 36;

	// Token: 0x0200004D RID: 77
	public class SpinBullet : Bullet
	{
		// Token: 0x06000126 RID: 294 RVA: 0x00006A18 File Offset: 0x00004C18
		public SpinBullet()
			: base("teleport_burst", false, false, false)
		{
		}

		// Token: 0x06000127 RID: 295 RVA: 0x00006A28 File Offset: 0x00004C28
		protected override IEnumerator Top()
		{
			base.ChangeDirection(new Direction(179f, DirectionType.Relative, -1f), 180);
			base.ChangeSpeed(new Speed(10f, SpeedType.Absolute), 180);
			yield return base.Wait(600);
			base.Vanish(false);
			yield break;
		}
	}
}
