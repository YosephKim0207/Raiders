using System;
using System.Collections;
using Brave.BulletScript;
using FullInspector;

// Token: 0x0200030D RID: 781
[InspectorDropdownName("Bosses/TankTreader/HomingShot1")]
public class TankTreaderHomingShot1 : Script
{
	// Token: 0x06000C19 RID: 3097 RVA: 0x0003ABB8 File Offset: 0x00038DB8
	protected override IEnumerator Top()
	{
		base.Fire(new Direction(0f, DirectionType.Aim, -1f), new Speed(7.5f, SpeedType.Absolute), new TankTreaderHomingShot1.HomingBullet());
		return null;
	}

	// Token: 0x04000CDD RID: 3293
	private const int AirTime = 75;

	// Token: 0x04000CDE RID: 3294
	private const int NumDeathBullets = 16;

	// Token: 0x0200030E RID: 782
	private class HomingBullet : Bullet
	{
		// Token: 0x06000C1A RID: 3098 RVA: 0x0003ABE4 File Offset: 0x00038DE4
		public HomingBullet()
			: base("homingBullet", false, false, false)
		{
		}

		// Token: 0x06000C1B RID: 3099 RVA: 0x0003ABF4 File Offset: 0x00038DF4
		protected override IEnumerator Top()
		{
			for (int i = 0; i < 75; i++)
			{
				base.ChangeDirection(new Direction(0f, DirectionType.Aim, 3f), 1);
				if (i == 45)
				{
					this.Projectile.spriteAnimator.Play("enemy_projectile_rocket_impact");
				}
				yield return base.Wait(1);
			}
			base.Vanish(false);
			yield break;
		}

		// Token: 0x06000C1C RID: 3100 RVA: 0x0003AC10 File Offset: 0x00038E10
		public override void OnBulletDestruction(Bullet.DestroyType destroyType, SpeculativeRigidbody hitRigidbody, bool preventSpawningProjectiles)
		{
			if (preventSpawningProjectiles)
			{
				return;
			}
			float num = base.RandomAngle();
			float num2 = 22.5f;
			for (int i = 0; i < 16; i++)
			{
				base.Fire(new Direction(num + num2 * (float)i, DirectionType.Absolute, -1f), new Speed(10f, SpeedType.Absolute), null);
			}
			AkSoundEngine.PostEvent("Play_WPN_golddoublebarrelshotgun_shot_01", this.Projectile.gameObject);
		}
	}
}
