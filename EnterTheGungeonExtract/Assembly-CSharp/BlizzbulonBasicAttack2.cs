using System;
using System.Collections;
using Brave.BulletScript;
using FullInspector;

// Token: 0x02000036 RID: 54
[InspectorDropdownName("Blizzbulon/BasicAttack2")]
public class BlizzbulonBasicAttack2 : Script
{
	// Token: 0x060000C8 RID: 200 RVA: 0x000051F4 File Offset: 0x000033F4
	protected override IEnumerator Top()
	{
		base.Fire(new Direction(-28f, DirectionType.Aim, -1f), new Speed(9f, SpeedType.Absolute), null);
		base.Fire(new Direction(0f, DirectionType.Aim, -1f), new Speed(11f, SpeedType.Absolute), null);
		base.Fire(new Direction(28f, DirectionType.Aim, -1f), new Speed(9f, SpeedType.Absolute), null);
		yield return base.Wait(45);
		base.Fire(new Direction(-28f, DirectionType.Aim, -1f), new Speed(9f, SpeedType.Absolute), null);
		base.Fire(new Direction(0f, DirectionType.Aim, -1f), new Speed(11f, SpeedType.Absolute), null);
		base.Fire(new Direction(28f, DirectionType.Aim, -1f), new Speed(9f, SpeedType.Absolute), null);
		yield break;
	}
}
