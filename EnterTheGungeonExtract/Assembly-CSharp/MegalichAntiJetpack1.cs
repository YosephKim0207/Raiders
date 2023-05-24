using System;
using System.Collections;
using Brave.BulletScript;
using FullInspector;

// Token: 0x0200024D RID: 589
[InspectorDropdownName("Bosses/Megalich/AntiJetpack1")]
public class MegalichAntiJetpack1 : Script
{
	// Token: 0x060008E4 RID: 2276 RVA: 0x0002B80C File Offset: 0x00029A0C
	protected override IEnumerator Top()
	{
		base.EndOnBlank = true;
		for (int i = 0; i < 4; i++)
		{
			bool offset = i % 2 == 1;
			for (int j = 0; j < 30; j++)
			{
				if (!offset || j != 29)
				{
					base.Fire(new Offset(base.SubdivideRange(-20f, 20f, 30, j, offset), 7.5f, 0f, string.Empty, DirectionType.Absolute), new Direction(-90f, DirectionType.Absolute, -1f), new Speed(7f, SpeedType.Absolute), new SpeedChangingBullet(8f, 60, 20 - 6 * i));
				}
			}
			yield return base.Wait(10);
		}
		yield return base.Wait(80);
		yield break;
	}

	// Token: 0x04000900 RID: 2304
	private const int NumBullets = 30;

	// Token: 0x04000901 RID: 2305
	private const int NumLines = 4;

	// Token: 0x04000902 RID: 2306
	private const float RoomHalfWidth = 20f;
}
