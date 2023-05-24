using System;
using System.Collections;
using Brave.BulletScript;
using FullInspector;

// Token: 0x02000217 RID: 535
[InspectorDropdownName("Kaliber/ThreeHeads1")]
public class KaliberThreeHeads1 : Script
{
	// Token: 0x0600080A RID: 2058 RVA: 0x00027320 File Offset: 0x00025520
	protected override IEnumerator Top()
	{
		float num = base.RandomAngle();
		float num2 = 12.857142f;
		for (int i = 0; i < 28; i++)
		{
			base.Fire(new Direction(num + (float)i * num2, DirectionType.Absolute, -1f), new Speed(9f, SpeedType.Absolute), new Bullet("burst", false, false, false));
		}
		return null;
	}

	// Token: 0x04000817 RID: 2071
	private const int NumBullets = 28;
}
