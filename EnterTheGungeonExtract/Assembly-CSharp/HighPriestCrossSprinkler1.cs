using System;
using System.Collections;
using Brave.BulletScript;
using FullInspector;

// Token: 0x020001DF RID: 479
[InspectorDropdownName("Bosses/HighPriest/CrossSprinkler1")]
public class HighPriestCrossSprinkler1 : Script
{
	// Token: 0x0600072A RID: 1834 RVA: 0x00022D1C File Offset: 0x00020F1C
	protected override IEnumerator Top()
	{
		for (int i = 0; i < 105; i++)
		{
			float d = (float)i / 105f;
			base.Fire(new Offset("left hand"), new Direction(-65f - d * 230f * 3.5f, DirectionType.Absolute, -1f), new Speed(12f, SpeedType.Absolute), new Bullet("cross", false, false, false));
			base.Fire(new Offset("right hand"), new Direction(-115f + d * 230f * 3.5f, DirectionType.Absolute, -1f), new Speed(12f, SpeedType.Absolute), new Bullet("cross", false, false, false));
			yield return base.Wait(2);
		}
		yield break;
	}

	// Token: 0x0400070C RID: 1804
	private const int NumBullets = 105;
}
