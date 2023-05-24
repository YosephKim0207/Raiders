using System;
using System.Collections;
using Brave.BulletScript;
using FullInspector;

// Token: 0x020000BC RID: 188
[InspectorDropdownName("Bosses/BossStatues/DirectionalWaveAllSimple")]
public class BossStatuesDirectionalWaveAllSimple : Script
{
	// Token: 0x060002E0 RID: 736 RVA: 0x0000F2A4 File Offset: 0x0000D4A4
	protected override IEnumerator Top()
	{
		base.Fire(new Offset("top 0"), new Direction(100f, DirectionType.Absolute, -1f), new Speed(9f, SpeedType.Absolute), new BossStatuesDirectionalWaveAllSimple.EggBullet());
		base.Fire(new Offset("top 1"), new Direction(90f, DirectionType.Absolute, -1f), new Speed(9f, SpeedType.Absolute), new BossStatuesDirectionalWaveAllSimple.EggBullet());
		base.Fire(new Offset("top 2"), new Direction(80f, DirectionType.Absolute, -1f), new Speed(9f, SpeedType.Absolute), new BossStatuesDirectionalWaveAllSimple.EggBullet());
		base.Fire(new Offset("right 0"), new Direction(10f, DirectionType.Absolute, -1f), new Speed(9f, SpeedType.Absolute), new BossStatuesDirectionalWaveAllSimple.EggBullet());
		base.Fire(new Offset("right 1"), new Direction(0f, DirectionType.Absolute, -1f), new Speed(9f, SpeedType.Absolute), new BossStatuesDirectionalWaveAllSimple.EggBullet());
		base.Fire(new Offset("right 2"), new Direction(-10f, DirectionType.Absolute, -1f), new Speed(9f, SpeedType.Absolute), new BossStatuesDirectionalWaveAllSimple.EggBullet());
		base.Fire(new Offset("bottom 0"), new Direction(-80f, DirectionType.Absolute, -1f), new Speed(9f, SpeedType.Absolute), new BossStatuesDirectionalWaveAllSimple.EggBullet());
		base.Fire(new Offset("bottom 1"), new Direction(-90f, DirectionType.Absolute, -1f), new Speed(9f, SpeedType.Absolute), new BossStatuesDirectionalWaveAllSimple.EggBullet());
		base.Fire(new Offset("bottom 2"), new Direction(-100f, DirectionType.Absolute, -1f), new Speed(9f, SpeedType.Absolute), new BossStatuesDirectionalWaveAllSimple.EggBullet());
		base.Fire(new Offset("left 0"), new Direction(190f, DirectionType.Absolute, -1f), new Speed(9f, SpeedType.Absolute), new BossStatuesDirectionalWaveAllSimple.EggBullet());
		base.Fire(new Offset("left 1"), new Direction(180f, DirectionType.Absolute, -1f), new Speed(9f, SpeedType.Absolute), new BossStatuesDirectionalWaveAllSimple.EggBullet());
		base.Fire(new Offset("left 2"), new Direction(170f, DirectionType.Absolute, -1f), new Speed(9f, SpeedType.Absolute), new BossStatuesDirectionalWaveAllSimple.EggBullet());
		BossStatuesChaos1.AntiCornerShot(this);
		return null;
	}

	// Token: 0x020000BD RID: 189
	public class EggBullet : Bullet
	{
		// Token: 0x060002E1 RID: 737 RVA: 0x0000F4F8 File Offset: 0x0000D6F8
		public EggBullet()
			: base("egg", false, false, false)
		{
		}

		// Token: 0x060002E2 RID: 738 RVA: 0x0000F508 File Offset: 0x0000D708
		protected override IEnumerator Top()
		{
			base.ChangeSpeed(new Speed(10f, SpeedType.Absolute), 120);
			yield return base.Wait(600);
			base.Vanish(false);
			yield break;
		}
	}
}
