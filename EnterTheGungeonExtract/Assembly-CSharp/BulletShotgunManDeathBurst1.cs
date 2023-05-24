using System;
using System.Collections;
using Brave.BulletScript;
using FullInspector;

// Token: 0x02000116 RID: 278
[InspectorDropdownName("BulletShotgunMan/DeathBurst1")]
public class BulletShotgunManDeathBurst1 : Script
{
	// Token: 0x06000425 RID: 1061 RVA: 0x00014118 File Offset: 0x00012318
	protected override IEnumerator Top()
	{
		for (int i = 0; i <= 6; i++)
		{
			base.Fire(new Direction((float)(i * 60), DirectionType.Absolute, -1f), new Speed(6.5f, SpeedType.Absolute), new Bullet("flashybullet", false, false, false));
		}
		return null;
	}
}
