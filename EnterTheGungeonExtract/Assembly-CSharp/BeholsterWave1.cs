using System;
using System.Collections;
using Brave.BulletScript;
using FullInspector;

// Token: 0x02000028 RID: 40
[InspectorDropdownName("Bosses/Beholster/Wave1")]
public class BeholsterWave1 : Script
{
	// Token: 0x06000094 RID: 148 RVA: 0x00004278 File Offset: 0x00002478
	protected override IEnumerator Top()
	{
		for (int i = -3; i <= 3; i++)
		{
			base.Fire(new Direction((float)(i * 20), DirectionType.Aim, -1f), new Speed(8f, SpeedType.Absolute), new Bullet("donut", false, false, false));
		}
		yield return base.Wait(35);
		for (float num = -2.5f; num <= 2.5f; num += 1f)
		{
			base.Fire(new Direction(num * 20f, DirectionType.Aim, -1f), new Speed(8f, SpeedType.Absolute), new Bullet("donut", false, false, false));
		}
		yield return base.Wait(35);
		for (int j = -9; j < 9; j++)
		{
			base.Fire(new Direction((float)(j * 20), DirectionType.Aim, -1f), new Speed(8f, SpeedType.Absolute), new Bullet("donut", false, false, false));
		}
		yield return base.Wait(35);
		for (float num2 = -8.5f; num2 <= 8.5f; num2 += 1f)
		{
			base.Fire(new Direction(num2 * 20f, DirectionType.Aim, -1f), new Speed(8f, SpeedType.Absolute), new Bullet("donut", false, false, false));
		}
		yield break;
	}
}
