using System;
using System.Collections;
using Brave.BulletScript;
using FullInspector;

// Token: 0x020000D4 RID: 212
[InspectorDropdownName("Bosses/BulletBros/TridentAttack1")]
public class BulletBrosTridentAttack1 : Script
{
	// Token: 0x06000336 RID: 822 RVA: 0x00010774 File Offset: 0x0000E974
	protected override IEnumerator Top()
	{
		for (int i = 0; i < 3; i++)
		{
			float aim = ((i % 2 != 0) ? base.GetAimDirection(1f, 10f) : base.AimDirection);
			base.Fire(new Direction(aim - 10f, DirectionType.Absolute, -1f), new Speed(9.5f, SpeedType.Absolute), null);
			base.Fire(new Direction(aim, DirectionType.Absolute, -1f), new Speed(9f, SpeedType.Absolute), null);
			base.Fire(new Direction(aim + 10f, DirectionType.Absolute, -1f), new Speed(9.5f, SpeedType.Absolute), null);
			yield return base.Wait(35);
		}
		yield break;
	}
}
