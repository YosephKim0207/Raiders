using System;
using System.Collections;
using Brave.BulletScript;

// Token: 0x0200013F RID: 319
public class CircleBurst12 : Script
{
	// Token: 0x060004BF RID: 1215 RVA: 0x0001711C File Offset: 0x0001531C
	protected override IEnumerator Top()
	{
		float num = base.RandomAngle();
		float num2 = 30f;
		for (int i = 0; i < 12; i++)
		{
			base.Fire(new Direction(num + (float)i * num2, DirectionType.Absolute, -1f), new Speed(9f, SpeedType.Absolute), null);
		}
		return null;
	}

	// Token: 0x0400049C RID: 1180
	private const int NumBullets = 12;
}
