using System;
using System.Collections;
using Brave.BulletScript;
using FullInspector;

// Token: 0x0200012A RID: 298
[InspectorDropdownName("Bosses/Bunker/TriangleWave1")]
public class BunkerTriangleWave1 : Script
{
	// Token: 0x06000468 RID: 1128 RVA: 0x00015098 File Offset: 0x00013298
	protected override IEnumerator Top()
	{
		for (int i = 0; i < 6; i++)
		{
			base.Fire(new Offset(0f, -3.25f + (float)i * 1.3f, 0f, string.Empty, DirectionType.Relative), new Direction((float)(-40 + 16 * i), DirectionType.Relative, -1f), new Speed(9f, SpeedType.Absolute), new Bullet("default2", false, false, false));
			yield return base.Wait(3);
		}
		for (int j = 0; j < 5; j++)
		{
			base.Fire(new Offset(0f, 2.6f - (float)j * 1.3f, 0f, string.Empty, DirectionType.Relative), new Direction((float)(24 - 16 * j), DirectionType.Relative, -1f), new Speed(9f, SpeedType.Absolute), new Bullet("default2", false, false, false));
			yield return base.Wait(3);
		}
		for (int k = 0; k < 5; k++)
		{
			base.Fire(new Offset(0f, -2.6f + (float)k * 1.3f, 0f, string.Empty, DirectionType.Relative), new Direction((float)(-24 + 16 * k), DirectionType.Relative, -1f), new Speed(9f, SpeedType.Absolute), new Bullet("default2", false, false, false));
			yield return base.Wait(3);
		}
		yield break;
	}
}
