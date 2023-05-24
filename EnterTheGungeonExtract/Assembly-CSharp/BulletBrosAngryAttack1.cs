using System;
using System.Collections;
using Brave.BulletScript;
using FullInspector;

// Token: 0x020000CE RID: 206
[InspectorDropdownName("Bosses/BulletBros/AngryAttack1")]
public class BulletBrosAngryAttack1 : Script
{
	// Token: 0x06000322 RID: 802 RVA: 0x000102BC File Offset: 0x0000E4BC
	protected override IEnumerator Top()
	{
		for (float num = -2f; num <= 2f; num += 1f)
		{
			base.Fire(new Direction(num * 20f, DirectionType.Aim, -1f), new Speed(10f, SpeedType.Absolute), new Bullet("angrybullet", false, false, false));
		}
		yield return base.Wait(40);
		float num2 = -1.5f;
		while ((double)num2 <= 1.5)
		{
			base.Fire(new Direction(num2 * 20f, DirectionType.Aim, -1f), new Speed(10f, SpeedType.Absolute), new Bullet("angrybullet", false, false, false));
			num2 += 1f;
		}
		yield return base.Wait(40);
		for (float num3 = -2f; num3 <= 2f; num3 += 0.5f)
		{
			base.Fire(new Direction(num3 * 20f, DirectionType.Aim, -1f), new Speed(10f, SpeedType.Absolute), new Bullet("angrybullet", false, false, false));
		}
		yield break;
	}
}
