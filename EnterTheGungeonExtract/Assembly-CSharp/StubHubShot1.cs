using System;
using System.Collections;
using Brave.BulletScript;

// Token: 0x020002FF RID: 767
public class StubHubShot1 : Script
{
	// Token: 0x06000BE7 RID: 3047 RVA: 0x0003A2A8 File Offset: 0x000384A8
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

	// Token: 0x04000CB9 RID: 3257
	private const int NumBullets = 12;
}
