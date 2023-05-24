using System;
using System.Collections;
using Brave.BulletScript;
using FullInspector;

// Token: 0x02000035 RID: 53
[InspectorDropdownName("Blizzbulon/BasicAttack1")]
public class BlizzbulonBasicAttack1 : Script
{
	// Token: 0x060000C6 RID: 198 RVA: 0x000051A4 File Offset: 0x000033A4
	protected override IEnumerator Top()
	{
		float num = 30f;
		for (int i = 0; i < 12; i++)
		{
			base.Fire(new Direction((float)i * num, DirectionType.Absolute, -1f), new Speed(6f, SpeedType.Absolute), null);
		}
		return null;
	}

	// Token: 0x040000CA RID: 202
	private const int NumBullets = 12;
}
