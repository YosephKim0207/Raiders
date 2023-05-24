using System;
using System.Collections;
using Brave.BulletScript;
using FullInspector;

// Token: 0x020001D4 RID: 468
[InspectorDropdownName("Bosses/Helicopter/RandomBurstsRapid1")]
public class HelicopterRandomRapid1 : Script
{
	// Token: 0x060006FF RID: 1791 RVA: 0x00022254 File Offset: 0x00020454
	protected override IEnumerator Top()
	{
		float startDirection = base.RandomAngle();
		string transform = BraveUtility.RandomElement<string>(HelicopterRandomRapid1.Transforms);
		for (int i = 0; i < 6; i++)
		{
			base.Fire(new Offset(transform), new Direction(base.SubdivideCircle(startDirection, 6, i, 1f, false), DirectionType.Absolute, -1f), new Speed(9f, SpeedType.Absolute), new HelicopterRandomSimple1.BigBullet());
		}
		if (BraveUtility.RandomBool())
		{
			yield break;
		}
		yield return base.Wait(15);
		startDirection = base.RandomAngle();
		transform = BraveUtility.RandomElement<string>(HelicopterRandomRapid1.Transforms);
		for (int j = 0; j < 6; j++)
		{
			base.Fire(new Offset(transform), new Direction(base.SubdivideCircle(startDirection, 6, j, 1f, false), DirectionType.Absolute, -1f), new Speed(9f, SpeedType.Absolute), new HelicopterRandomSimple1.BigBullet());
		}
		yield break;
	}

	// Token: 0x040006EA RID: 1770
	private const int NumBullets = 6;

	// Token: 0x040006EB RID: 1771
	private static string[] Transforms = new string[] { "shoot point 1", "shoot point 2", "shoot point 3", "shoot point 4" };
}
