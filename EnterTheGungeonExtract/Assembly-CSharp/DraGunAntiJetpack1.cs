using System;
using System.Collections;
using Brave.BulletScript;
using FullInspector;

// Token: 0x02000157 RID: 343
[InspectorDropdownName("Bosses/DraGun/AntiJetpack1")]
public class DraGunAntiJetpack1 : Script
{
	// Token: 0x06000525 RID: 1317 RVA: 0x00018B50 File Offset: 0x00016D50
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
					base.Fire(new Offset(base.SubdivideRange(-17.5f, 17.5f, 30, j, offset), 18f, 0f, string.Empty, DirectionType.Absolute), new Direction(-90f, DirectionType.Absolute, -1f), new Speed(8f, SpeedType.Absolute), new SpeedChangingBullet(9f, 90, 60 - 6 * i));
				}
			}
			yield return base.Wait(10);
		}
		yield return base.Wait(80);
		yield break;
	}

	// Token: 0x040004F8 RID: 1272
	private const int NumBullets = 30;

	// Token: 0x040004F9 RID: 1273
	private const int NumLines = 4;

	// Token: 0x040004FA RID: 1274
	private const float RoomHalfWidth = 17.5f;
}
