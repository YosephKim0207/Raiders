using System;
using System.Collections;
using Brave.BulletScript;

// Token: 0x020000CD RID: 205
public class BullatGiantSoundBurst1 : Script
{
	// Token: 0x06000320 RID: 800 RVA: 0x00010260 File Offset: 0x0000E460
	protected override IEnumerator Top()
	{
		float num = base.RandomAngle();
		float num2 = 30f;
		for (int i = 0; i < 12; i++)
		{
			base.Fire(new Direction(num + (float)i * num2, DirectionType.Absolute, -1f), new Speed(6f, SpeedType.Absolute), null);
		}
		return null;
	}

	// Token: 0x04000348 RID: 840
	private const int NumBullets = 12;
}
