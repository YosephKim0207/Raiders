using System;
using System.Collections;
using Brave.BulletScript;
using FullInspector;

// Token: 0x0200001A RID: 26
[InspectorDropdownName("Bosses/Bashellisk/CircleBursts1")]
public class BashelliskCircleBursts1 : Script
{
	// Token: 0x06000064 RID: 100 RVA: 0x0000363C File Offset: 0x0000183C
	protected override IEnumerator Top()
	{
		float num = base.RandomAngle();
		float num2 = 21.17647f;
		for (int i = 0; i < 17; i++)
		{
			base.Fire(new Direction(num + (float)i * num2, DirectionType.Absolute, -1f), new Speed(9f, SpeedType.Absolute), new Bullet("CircleBurst", false, false, false));
		}
		return null;
	}

	// Token: 0x04000065 RID: 101
	private const int NumBullets = 17;
}
