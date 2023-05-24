using System;
using System.Collections;
using Brave.BulletScript;
using FullInspector;

// Token: 0x02000303 RID: 771
[InspectorDropdownName("Sunburst/DeathBurst1")]
public class SunburstDeathBurst1 : Script
{
	// Token: 0x06000BEF RID: 3055 RVA: 0x0003A444 File Offset: 0x00038644
	protected override IEnumerator Top()
	{
		yield return base.Wait(20);
		for (int i = 0; i < 36; i++)
		{
			base.Fire(new Direction((float)(i * 10), DirectionType.Absolute, -1f), new Speed(9f, SpeedType.Absolute), new SunburstDeathBurst1.BurstBullet());
		}
		yield break;
	}

	// Token: 0x02000304 RID: 772
	public class BurstBullet : Bullet
	{
		// Token: 0x06000BF0 RID: 3056 RVA: 0x0003A460 File Offset: 0x00038660
		public BurstBullet()
			: base(null, false, false, false)
		{
		}

		// Token: 0x06000BF1 RID: 3057 RVA: 0x0003A46C File Offset: 0x0003866C
		protected override IEnumerator Top()
		{
			base.ChangeSpeed(new Speed(7f, SpeedType.Absolute), 40);
			yield return base.Wait(600);
			base.Vanish(false);
			yield break;
		}
	}
}
