using System;
using System.Collections;
using Brave.BulletScript;

// Token: 0x020002ED RID: 749
public class ShockwaveChallengeCircleBurst : Script
{
	// Token: 0x06000BA1 RID: 2977 RVA: 0x00039114 File Offset: 0x00037314
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

	// Token: 0x04000C7A RID: 3194
	private const int NumBullets = 12;
}
