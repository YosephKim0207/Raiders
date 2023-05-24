using System;
using System.Collections;
using Brave.BulletScript;
using FullInspector;

// Token: 0x02000300 RID: 768
[InspectorDropdownName("Sunburst/BlueWave1")]
public class SunburstBlueWave1 : Script
{
	// Token: 0x06000BE9 RID: 3049 RVA: 0x0003A304 File Offset: 0x00038504
	protected override IEnumerator Top()
	{
		float aimDirection = base.AimDirection;
		base.Fire(new Offset(0f, 0.66f, aimDirection, string.Empty, DirectionType.Absolute), new Direction(aimDirection, DirectionType.Absolute, -1f), new Speed(9f, SpeedType.Absolute), null);
		base.Fire(new Offset(0.66f, 0f, aimDirection, string.Empty, DirectionType.Absolute), new Direction(aimDirection, DirectionType.Absolute, -1f), new Speed(9f, SpeedType.Absolute), null);
		base.Fire(new Offset(0f, -0.66f, aimDirection, string.Empty, DirectionType.Absolute), new Direction(aimDirection, DirectionType.Absolute, -1f), new Speed(9f, SpeedType.Absolute), null);
		return null;
	}
}
