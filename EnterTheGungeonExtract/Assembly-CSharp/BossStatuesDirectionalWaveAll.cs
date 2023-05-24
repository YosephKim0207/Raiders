using System;
using System.Collections;
using Brave.BulletScript;
using FullInspector;

// Token: 0x020000B9 RID: 185
[InspectorDropdownName("Bosses/BossStatues/DirectionalWaveAll")]
public class BossStatuesDirectionalWaveAll : Script
{
	// Token: 0x060002D6 RID: 726 RVA: 0x0000EDE0 File Offset: 0x0000CFE0
	protected override IEnumerator Top()
	{
		base.Fire(new Offset("top 0"), new Direction(120f, DirectionType.Absolute, -1f), new Speed(9f, SpeedType.Absolute), new BossStatuesDirectionalWaveAll.EggBullet());
		base.Fire(new Offset("top 0"), new Direction(100f, DirectionType.Absolute, -1f), new Speed(9f, SpeedType.Absolute), new BossStatuesDirectionalWaveAll.EggBullet());
		base.Fire(new Offset("top 1"), new Direction(90f, DirectionType.Absolute, -1f), new Speed(9f, SpeedType.Absolute), new BossStatuesDirectionalWaveAll.EggBullet());
		base.Fire(new Offset("top 2"), new Direction(80f, DirectionType.Absolute, -1f), new Speed(9f, SpeedType.Absolute), new BossStatuesDirectionalWaveAll.EggBullet());
		base.Fire(new Offset("top 2"), new Direction(60f, DirectionType.Absolute, -1f), new Speed(9f, SpeedType.Absolute), new BossStatuesDirectionalWaveAll.EggBullet());
		base.Fire(new Offset("right 0"), new Direction(30f, DirectionType.Absolute, -1f), new Speed(9f, SpeedType.Absolute), new BossStatuesDirectionalWaveAll.EggBullet());
		base.Fire(new Offset("right 0"), new Direction(10f, DirectionType.Absolute, -1f), new Speed(9f, SpeedType.Absolute), new BossStatuesDirectionalWaveAll.EggBullet());
		base.Fire(new Offset("right 1"), new Direction(0f, DirectionType.Absolute, -1f), new Speed(9f, SpeedType.Absolute), new BossStatuesDirectionalWaveAll.EggBullet());
		base.Fire(new Offset("right 2"), new Direction(-10f, DirectionType.Absolute, -1f), new Speed(9f, SpeedType.Absolute), new BossStatuesDirectionalWaveAll.EggBullet());
		base.Fire(new Offset("right 2"), new Direction(-30f, DirectionType.Absolute, -1f), new Speed(9f, SpeedType.Absolute), new BossStatuesDirectionalWaveAll.EggBullet());
		base.Fire(new Offset("bottom 0"), new Direction(-60f, DirectionType.Absolute, -1f), new Speed(9f, SpeedType.Absolute), new BossStatuesDirectionalWaveAll.EggBullet());
		base.Fire(new Offset("bottom 0"), new Direction(-80f, DirectionType.Absolute, -1f), new Speed(9f, SpeedType.Absolute), new BossStatuesDirectionalWaveAll.EggBullet());
		base.Fire(new Offset("bottom 1"), new Direction(-90f, DirectionType.Absolute, -1f), new Speed(9f, SpeedType.Absolute), new BossStatuesDirectionalWaveAll.EggBullet());
		base.Fire(new Offset("bottom 2"), new Direction(-100f, DirectionType.Absolute, -1f), new Speed(9f, SpeedType.Absolute), new BossStatuesDirectionalWaveAll.EggBullet());
		base.Fire(new Offset("bottom 2"), new Direction(-120f, DirectionType.Absolute, -1f), new Speed(9f, SpeedType.Absolute), new BossStatuesDirectionalWaveAll.EggBullet());
		base.Fire(new Offset("left 0"), new Direction(210f, DirectionType.Absolute, -1f), new Speed(9f, SpeedType.Absolute), new BossStatuesDirectionalWaveAll.EggBullet());
		base.Fire(new Offset("left 0"), new Direction(190f, DirectionType.Absolute, -1f), new Speed(9f, SpeedType.Absolute), new BossStatuesDirectionalWaveAll.EggBullet());
		base.Fire(new Offset("left 1"), new Direction(180f, DirectionType.Absolute, -1f), new Speed(9f, SpeedType.Absolute), new BossStatuesDirectionalWaveAll.EggBullet());
		base.Fire(new Offset("left 2"), new Direction(170f, DirectionType.Absolute, -1f), new Speed(9f, SpeedType.Absolute), new BossStatuesDirectionalWaveAll.EggBullet());
		base.Fire(new Offset("left 2"), new Direction(150f, DirectionType.Absolute, -1f), new Speed(9f, SpeedType.Absolute), new BossStatuesDirectionalWaveAll.EggBullet());
		BossStatuesChaos1.AntiCornerShot(this);
		return null;
	}

	// Token: 0x020000BA RID: 186
	public class EggBullet : Bullet
	{
		// Token: 0x060002D7 RID: 727 RVA: 0x0000F1B4 File Offset: 0x0000D3B4
		public EggBullet()
			: base("egg", false, false, false)
		{
		}

		// Token: 0x060002D8 RID: 728 RVA: 0x0000F1C4 File Offset: 0x0000D3C4
		protected override IEnumerator Top()
		{
			base.ChangeSpeed(new Speed(10f, SpeedType.Absolute), 120);
			yield return base.Wait(600);
			base.Vanish(false);
			yield break;
		}
	}
}
