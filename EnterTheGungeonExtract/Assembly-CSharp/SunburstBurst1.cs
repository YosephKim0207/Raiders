using System;
using System.Collections;
using Brave.BulletScript;
using FullInspector;

// Token: 0x02000301 RID: 769
[InspectorDropdownName("Sunburst/Burst1")]
public class SunburstBurst1 : Script
{
	// Token: 0x06000BEB RID: 3051 RVA: 0x0003A3C0 File Offset: 0x000385C0
	protected override IEnumerator Top()
	{
		float num = base.RandomAngle();
		float num2 = 15f;
		for (int i = 0; i < 24; i++)
		{
			base.Fire(new Direction(num + (float)i * num2, DirectionType.Absolute, -1f), new Speed(9f, SpeedType.Absolute), new SunburstBurst1.BurstBullet());
		}
		return null;
	}

	// Token: 0x04000CBA RID: 3258
	private const int NumBullets = 24;

	// Token: 0x02000302 RID: 770
	public class BurstBullet : Bullet
	{
		// Token: 0x06000BEC RID: 3052 RVA: 0x0003A418 File Offset: 0x00038618
		public BurstBullet()
			: base(null, false, false, false)
		{
		}

		// Token: 0x06000BED RID: 3053 RVA: 0x0003A424 File Offset: 0x00038624
		protected override IEnumerator Top()
		{
			base.ChangeSpeed(new Speed(5f, SpeedType.Absolute), 40);
			return null;
		}
	}
}
