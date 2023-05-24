using System;
using System.Collections;
using Brave.BulletScript;
using FullInspector;
using UnityEngine;

// Token: 0x020000AD RID: 173
[InspectorDropdownName("Bosses/BossStatues/Chaos1")]
public class BossStatuesChaos1 : Script
{
	// Token: 0x060002A5 RID: 677 RVA: 0x0000DEDC File Offset: 0x0000C0DC
	protected override IEnumerator Top()
	{
		base.Fire(new Offset("top 1"), new Direction(90f, DirectionType.Absolute, -1f), new Speed(7.5f, SpeedType.Absolute), new BossStatuesChaos1.EggBullet());
		base.Fire(new Offset("top 1"), new Direction(90f, DirectionType.Absolute, -1f), new Speed(6f, SpeedType.Absolute), new BossStatuesChaos1.EggBullet());
		base.Fire(new Offset("right 1"), new Direction(0f, DirectionType.Absolute, -1f), new Speed(7.5f, SpeedType.Absolute), new BossStatuesChaos1.EggBullet());
		base.Fire(new Offset("right 1"), new Direction(0f, DirectionType.Absolute, -1f), new Speed(6f, SpeedType.Absolute), new BossStatuesChaos1.EggBullet());
		base.Fire(new Offset("bottom 1"), new Direction(-90f, DirectionType.Absolute, -1f), new Speed(7.5f, SpeedType.Absolute), new BossStatuesChaos1.EggBullet());
		base.Fire(new Offset("bottom 1"), new Direction(-90f, DirectionType.Absolute, -1f), new Speed(6f, SpeedType.Absolute), new BossStatuesChaos1.EggBullet());
		base.Fire(new Offset("left 1"), new Direction(180f, DirectionType.Absolute, -1f), new Speed(7.5f, SpeedType.Absolute), new BossStatuesChaos1.EggBullet());
		base.Fire(new Offset("left 1"), new Direction(180f, DirectionType.Absolute, -1f), new Speed(6f, SpeedType.Absolute), new BossStatuesChaos1.EggBullet());
		BossStatuesChaos1.AntiCornerShot(this);
		return null;
	}

	// Token: 0x060002A6 RID: 678 RVA: 0x0000E070 File Offset: 0x0000C270
	public static void AntiCornerShot(Script parentScript)
	{
		if (UnityEngine.Random.value > 0.33f)
		{
			return;
		}
		float aimDirection = parentScript.AimDirection;
		string text = "top 1";
		switch (BraveMathCollege.AngleToQuadrant(aimDirection))
		{
		case 0:
			text = "top 1";
			break;
		case 1:
			text = "right 1";
			break;
		case 2:
			text = "bottom 1";
			break;
		case 3:
			text = "left 1";
			break;
		}
		parentScript.Fire(new Offset(text), new Direction(aimDirection, DirectionType.Absolute, -1f), new Speed(7.5f, SpeedType.Absolute), new Bullet("egg", false, false, false));
	}

	// Token: 0x020000AE RID: 174
	public class EggBullet : Bullet
	{
		// Token: 0x060002A7 RID: 679 RVA: 0x0000E118 File Offset: 0x0000C318
		public EggBullet()
			: base("egg", false, false, false)
		{
		}

		// Token: 0x060002A8 RID: 680 RVA: 0x0000E128 File Offset: 0x0000C328
		protected override IEnumerator Top()
		{
			base.ChangeSpeed(new Speed(7.5f, SpeedType.Absolute), 120);
			yield return base.Wait(600);
			base.Vanish(false);
			yield break;
		}
	}
}
