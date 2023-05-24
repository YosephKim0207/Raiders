using System;
using System.Collections;
using Brave.BulletScript;

// Token: 0x0200010E RID: 270
public class BulletManShroomed : Script
{
	// Token: 0x06000409 RID: 1033 RVA: 0x000139B4 File Offset: 0x00011BB4
	protected override IEnumerator Top()
	{
		base.Fire(new Direction(-20f, DirectionType.Aim, -1f), new Speed(9f, SpeedType.Absolute), null);
		base.Fire(new Direction(20f, DirectionType.Aim, -1f), new Speed(9f, SpeedType.Absolute), null);
		return null;
	}
}
