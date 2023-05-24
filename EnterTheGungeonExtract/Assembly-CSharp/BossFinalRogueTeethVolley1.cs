using System;
using System.Collections;
using Brave.BulletScript;
using FullInspector;

// Token: 0x020000AB RID: 171
[InspectorDropdownName("Bosses/BossFinalRogue/TeethVolley1")]
public class BossFinalRogueTeethVolley1 : Script
{
	// Token: 0x0600029D RID: 669 RVA: 0x0000D984 File Offset: 0x0000BB84
	protected override IEnumerator Top()
	{
		yield return base.Wait(10);
		base.Fire(new Offset("teeth mid 1"), new Direction(-90f, DirectionType.Absolute, -1f), new Speed(10f, SpeedType.Absolute), new Bullet("teeth_default", false, false, false));
		base.Fire(new Offset("teeth mid 9"), new Direction(-90f, DirectionType.Absolute, -1f), new Speed(10f, SpeedType.Absolute), new Bullet("teeth_default", false, false, false));
		yield return base.Wait(10);
		float dir = base.GetAimDirection("teeth mid 3", 1f, 10f);
		base.Fire(new Offset("teeth mid 2"), new Direction(dir, DirectionType.Absolute, -1f), new Speed(10f, SpeedType.Absolute), new Bullet("teeth_default", false, false, false));
		base.Fire(new Offset("teeth mid 3"), new Direction(dir, DirectionType.Absolute, -1f), new Speed(10f, SpeedType.Absolute), new Bullet("teeth_default", false, false, false));
		base.Fire(new Offset("teeth mid 4"), new Direction(dir, DirectionType.Absolute, -1f), new Speed(10f, SpeedType.Absolute), new Bullet("teeth_default", false, false, false));
		dir = base.GetAimDirection("teeth mid 7", 1f, 10f);
		base.Fire(new Offset("teeth mid 6"), new Direction(dir, DirectionType.Absolute, -1f), new Speed(10f, SpeedType.Absolute), new Bullet("teeth_default", false, false, false));
		base.Fire(new Offset("teeth mid 7"), new Direction(dir, DirectionType.Absolute, -1f), new Speed(10f, SpeedType.Absolute), new Bullet("teeth_default", false, false, false));
		base.Fire(new Offset("teeth mid 8"), new Direction(dir, DirectionType.Absolute, -1f), new Speed(10f, SpeedType.Absolute), new Bullet("teeth_default", false, false, false));
		yield return base.Wait(10);
		base.Fire(new Offset("teeth top 1"), new Direction(-90f, DirectionType.Absolute, -1f), new Speed(10f, SpeedType.Absolute), new Bullet("teeth_large", false, false, false));
		base.Fire(new Offset("teeth top 2"), new Direction(-90f, DirectionType.Absolute, -1f), new Speed(10f, SpeedType.Absolute), new Bullet("teeth_large", false, false, false));
		base.Fire(new Offset("teeth top 3"), new Direction(-90f, DirectionType.Absolute, -1f), new Speed(10f, SpeedType.Absolute), new Bullet("teeth_large", false, false, false));
		base.Fire(new Offset("teeth mid 5"), new Direction(-90f, DirectionType.Absolute, -1f), new Speed(10f, SpeedType.Absolute), new Bullet("teeth_default", false, false, false));
		yield return base.Wait(10);
		dir = base.GetAimDirection("teeth bottom 1", 1f, 10f);
		base.Fire(new Offset("teeth bottom 1"), new Direction(dir, DirectionType.Absolute, -1f), new Speed(10f, SpeedType.Absolute), new Bullet("teeth_football", false, false, false));
		dir = base.GetAimDirection("teeth bottom 4", 1f, 10f);
		base.Fire(new Offset("teeth bottom 4"), new Direction(dir, DirectionType.Absolute, -1f), new Speed(10f, SpeedType.Absolute), new Bullet("teeth_football", false, false, false));
		yield return base.Wait(10);
		yield break;
	}
}
