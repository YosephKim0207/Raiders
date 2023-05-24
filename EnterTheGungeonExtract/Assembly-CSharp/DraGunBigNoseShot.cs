using System;
using System.Collections;
using Brave.BulletScript;
using FullInspector;

// Token: 0x02000159 RID: 345
[InspectorDropdownName("Bosses/DraGun/BigNoseShot")]
public class DraGunBigNoseShot : Script
{
	// Token: 0x0600052D RID: 1325 RVA: 0x00018D18 File Offset: 0x00016F18
	protected override IEnumerator Top()
	{
		base.Fire(new Direction(-90f, DirectionType.Absolute, -1f), new Speed(6f, SpeedType.Absolute), new Bullet("homing", false, false, false));
		base.Fire(new Direction(-110f, DirectionType.Absolute, -1f), new Speed(6f, SpeedType.Absolute), new Bullet("homing", false, false, false));
		base.Fire(new Direction(-130f, DirectionType.Absolute, -1f), new Speed(6f, SpeedType.Absolute), new Bullet("homing", false, false, false));
		base.Fire(new Direction(-70f, DirectionType.Absolute, -1f), new Speed(6f, SpeedType.Absolute), new Bullet("homing", false, false, false));
		base.Fire(new Direction(-50f, DirectionType.Absolute, -1f), new Speed(6f, SpeedType.Absolute), new Bullet("homing", false, false, false));
		if (ChallengeManager.CHALLENGE_MODE_ACTIVE)
		{
			base.Fire(new Direction(-60f, DirectionType.Absolute, -1f), new Speed(6f, SpeedType.Absolute), new Bullet("homing", false, false, false));
			base.Fire(new Direction(-80f, DirectionType.Absolute, -1f), new Speed(6f, SpeedType.Absolute), new Bullet("homing", false, false, false));
			base.Fire(new Direction(-100f, DirectionType.Absolute, -1f), new Speed(6f, SpeedType.Absolute), new Bullet("homing", false, false, false));
			base.Fire(new Direction(-120f, DirectionType.Absolute, -1f), new Speed(6f, SpeedType.Absolute), new Bullet("homing", false, false, false));
		}
		return null;
	}
}
