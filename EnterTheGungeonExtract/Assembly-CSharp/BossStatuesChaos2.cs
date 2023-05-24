using System;
using System.Collections;
using Brave.BulletScript;
using FullInspector;

// Token: 0x020000B0 RID: 176
[InspectorDropdownName("Bosses/BossStatues/Chaos2")]
public class BossStatuesChaos2 : Script
{
	// Token: 0x060002B0 RID: 688 RVA: 0x0000E208 File Offset: 0x0000C408
	protected override IEnumerator Top()
	{
		base.Fire(new Offset("top 0"), new Direction(135f, DirectionType.Absolute, -1f), new Speed(7.5f, SpeedType.Absolute), new Bullet("egg", false, false, false));
		base.Fire(new Offset("top 2"), new Direction(45f, DirectionType.Absolute, -1f), new Speed(7.5f, SpeedType.Absolute), new Bullet("egg", false, false, false));
		base.Fire(new Offset("right 0"), new Direction(45f, DirectionType.Absolute, -1f), new Speed(7.5f, SpeedType.Absolute), new Bullet("egg", false, false, false));
		base.Fire(new Offset("right 2"), new Direction(-45f, DirectionType.Absolute, -1f), new Speed(7.5f, SpeedType.Absolute), new Bullet("egg", false, false, false));
		base.Fire(new Offset("bottom 0"), new Direction(-45f, DirectionType.Absolute, -1f), new Speed(7.5f, SpeedType.Absolute), new Bullet("egg", false, false, false));
		base.Fire(new Offset("bottom 2"), new Direction(-135f, DirectionType.Absolute, -1f), new Speed(7.5f, SpeedType.Absolute), new Bullet("egg", false, false, false));
		base.Fire(new Offset("left 0"), new Direction(-135f, DirectionType.Absolute, -1f), new Speed(7.5f, SpeedType.Absolute), new Bullet("egg", false, false, false));
		base.Fire(new Offset("left 2"), new Direction(135f, DirectionType.Absolute, -1f), new Speed(7.5f, SpeedType.Absolute), new Bullet("egg", false, false, false));
		BossStatuesChaos1.AntiCornerShot(this);
		return null;
	}
}
