using System;
using System.Collections;
using Brave.BulletScript;

// Token: 0x02000319 RID: 793
public class TutorialKnightBurstAttack1 : Script
{
	// Token: 0x06000C42 RID: 3138 RVA: 0x0003B38C File Offset: 0x0003958C
	protected override IEnumerator Top()
	{
		yield return base.Wait(15);
		for (int i = 0; i < 36; i++)
		{
			base.Fire(new Direction((float)(i * 10), DirectionType.Absolute, -1f), new Speed(5f, SpeedType.Absolute), new Bullet("burst", true, false, false));
		}
		yield return base.Wait(90);
		for (int j = 0; j < 36; j++)
		{
			base.Fire(new Direction((float)(j * 10), DirectionType.Absolute, -1f), new Speed(5f, SpeedType.Absolute), new Bullet("burst", true, false, false));
		}
		yield return base.Wait(30);
		yield break;
	}
}
