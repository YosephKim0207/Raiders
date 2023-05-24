using System;
using System.Collections;
using Brave.BulletScript;
using FullInspector;

// Token: 0x02000123 RID: 291
[InspectorDropdownName("Bosses/Bunker/Arc1")]
public class BunkerArc1 : Script
{
	// Token: 0x0600044D RID: 1101 RVA: 0x00014C68 File Offset: 0x00012E68
	protected override IEnumerator Top()
	{
		for (int i = 0; i < 21; i++)
		{
			base.Fire(new Offset(0f, -3.25f + (float)i * 0.325f, 0f, string.Empty, DirectionType.Relative), new Direction((float)(-60 + i * 6), DirectionType.Relative, -1f), new Speed(7f, SpeedType.Absolute), null);
			base.Fire(new Offset(0f, 3.25f - (float)i * 0.325f, 0f, string.Empty, DirectionType.Relative), new Direction((float)(60 - i * 6), DirectionType.Relative, -1f), new Speed(7f, SpeedType.Absolute), null);
			yield return base.Wait(3);
		}
		yield break;
	}
}
