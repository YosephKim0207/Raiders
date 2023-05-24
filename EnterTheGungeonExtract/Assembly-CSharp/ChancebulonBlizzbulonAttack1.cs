using System;
using System.Collections;
using Brave.BulletScript;
using FullInspector;

// Token: 0x0200012C RID: 300
[InspectorDropdownName("Chancebulon/BlizzbulonAttack1")]
public class ChancebulonBlizzbulonAttack1 : Script
{
	// Token: 0x06000470 RID: 1136 RVA: 0x00015348 File Offset: 0x00013548
	protected override IEnumerator Top()
	{
		float deltaAngle = 30f;
		for (int i = 0; i < 12; i++)
		{
			base.Fire(new Direction((float)i * deltaAngle, DirectionType.Absolute, -1f), new Speed(6f, SpeedType.Absolute), new Bullet("icicle", false, false, false));
		}
		yield return base.Wait(30);
		base.Fire(new Direction(-28f, DirectionType.Aim, -1f), new Speed(9f, SpeedType.Absolute), new Bullet("icicle", false, false, false));
		base.Fire(new Direction(0f, DirectionType.Aim, -1f), new Speed(11f, SpeedType.Absolute), new Bullet("icicle", false, false, false));
		base.Fire(new Direction(28f, DirectionType.Aim, -1f), new Speed(9f, SpeedType.Absolute), new Bullet("icicle", false, false, false));
		yield return base.Wait(30);
		base.Fire(new Direction(-28f, DirectionType.Aim, -1f), new Speed(9f, SpeedType.Absolute), new Bullet("icicle", false, false, false));
		base.Fire(new Direction(0f, DirectionType.Aim, -1f), new Speed(11f, SpeedType.Absolute), new Bullet("icicle", false, false, false));
		base.Fire(new Direction(28f, DirectionType.Aim, -1f), new Speed(9f, SpeedType.Absolute), new Bullet("icicle", false, false, false));
		yield break;
	}

	// Token: 0x04000450 RID: 1104
	private const int NumBullets = 12;
}
