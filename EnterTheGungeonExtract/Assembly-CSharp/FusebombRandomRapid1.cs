using System;
using System.Collections;
using Brave.BulletScript;
using FullInspector;

// Token: 0x020001A5 RID: 421
[InspectorDropdownName("Bosses/Fusebomb/RandomBurstsRapid1")]
public class FusebombRandomRapid1 : Script
{
	// Token: 0x06000647 RID: 1607 RVA: 0x0001E534 File Offset: 0x0001C734
	protected override IEnumerator Top()
	{
		float num = base.RandomAngle();
		for (int i = 0; i < 8; i++)
		{
			base.Fire(new Direction(base.SubdivideArc(num, 360f, 8, i, false), DirectionType.Absolute, -1f), new Speed(9f, SpeedType.Absolute), null);
		}
		return null;
	}

	// Token: 0x04000614 RID: 1556
	private const int NumBullets = 8;

	// Token: 0x04000615 RID: 1557
	private static bool s_offset;
}
