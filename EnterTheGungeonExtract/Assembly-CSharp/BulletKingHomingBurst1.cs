using System;
using System.Collections;
using Brave.BulletScript;
using FullInspector;

// Token: 0x020000F6 RID: 246
[InspectorDropdownName("Bosses/BulletKing/HomingBurst1")]
public class BulletKingHomingBurst1 : Script
{
	// Token: 0x060003A9 RID: 937 RVA: 0x00012348 File Offset: 0x00010548
	protected override IEnumerator Top()
	{
		yield return base.Wait(10);
		this.HomingShot(-1.25f, -0.75f, 0f);
		this.HomingShot(-1.3125f, -0.4375f, -15f);
		this.HomingShot(-1.5f, -0.1875f, -30f);
		this.HomingShot(-1.75f, 0.25f, -45f);
		this.HomingShot(-2.125f, 1.3125f, -67.5f);
		this.HomingShot(-2.125f, 1.3125f, -90f);
		this.HomingShot(-2.125f, 1.3125f, -112.5f);
		this.HomingShot(-2.0625f, 2.375f, -135f);
		this.HomingShot(-0.8125f, 3.1875f, -157.5f);
		this.HomingShot(0.0625f, 3.5625f, 180f);
		this.HomingShot(0.9375f, 3.1875f, 157.5f);
		this.HomingShot(2.125f, 2.375f, 135f);
		this.HomingShot(2.1875f, 1.3125f, 112.5f);
		this.HomingShot(2.1875f, 1.3125f, 90f);
		this.HomingShot(2.1875f, 1.3125f, 67.5f);
		this.HomingShot(1.875f, 0.25f, 45f);
		this.HomingShot(1.625f, -0.1875f, 30f);
		this.HomingShot(1.4275f, -0.4375f, 15f);
		this.HomingShot(1.375f, -0.75f, 0f);
		yield break;
	}

	// Token: 0x060003AA RID: 938 RVA: 0x00012364 File Offset: 0x00010564
	private void HomingShot(float x, float y, float direction)
	{
		base.Fire(new Offset(x, y, 0f, string.Empty, DirectionType.Absolute), new Direction(direction - 90f, DirectionType.Absolute, -1f), new Speed(9f, SpeedType.Absolute), new BulletKingHomingBurst1.HomingBullet());
	}

	// Token: 0x020000F7 RID: 247
	public class HomingBullet : Bullet
	{
		// Token: 0x060003AB RID: 939 RVA: 0x000123A0 File Offset: 0x000105A0
		public HomingBullet()
			: base("homing", false, false, false)
		{
		}

		// Token: 0x060003AC RID: 940 RVA: 0x000123B0 File Offset: 0x000105B0
		protected override IEnumerator Top()
		{
			yield return base.Wait(10);
			this.Direction = base.AimDirection;
			yield return base.Wait(90);
			this.Direction = base.AimDirection;
			base.Fire(new Direction(60f, DirectionType.Aim, -1f), new Speed(7f, SpeedType.Absolute), new Bullet("homingBurst", false, false, false));
			base.Fire(new Direction(-60f, DirectionType.Aim, -1f), new Speed(7f, SpeedType.Absolute), new Bullet("homingBurst", false, false, false));
			yield return base.Wait(600);
			base.Vanish(false);
			yield break;
		}
	}
}
