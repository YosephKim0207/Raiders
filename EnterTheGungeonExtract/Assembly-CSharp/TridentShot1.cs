using System;
using System.Collections;
using Brave.BulletScript;

// Token: 0x02000318 RID: 792
public class TridentShot1 : Script
{
	// Token: 0x06000C40 RID: 3136 RVA: 0x0003B310 File Offset: 0x00039510
	protected override IEnumerator Top()
	{
		base.Fire(new Direction(-12f, DirectionType.Aim, -1f), new Speed(10f, SpeedType.Absolute), null);
		base.Fire(new Direction(0f, DirectionType.Aim, -1f), new Speed(10f, SpeedType.Absolute), null);
		base.Fire(new Direction(12f, DirectionType.Aim, -1f), new Speed(10f, SpeedType.Absolute), null);
		return null;
	}
}
