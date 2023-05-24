using System;
using System.Collections;
using Brave.BulletScript;
using UnityEngine;

// Token: 0x020002B1 RID: 689
public class PedestalMimicThrowStuff1 : Script
{
	// Token: 0x06000A8F RID: 2703 RVA: 0x00032FCC File Offset: 0x000311CC
	protected override IEnumerator Top()
	{
		int numBullets = 20;
		float deltaAngle = (float)(360 / (numBullets + 1) * 2);
		int i = UnityEngine.Random.RandomRange(0, PedestalMimicThrowStuff1.BulletNames.Length);
		for (int j = 0; j < numBullets; j++)
		{
			float angle = -180f + (float)j * deltaAngle;
			base.Fire(new Offset(1.5f, 0f, angle, string.Empty, DirectionType.Absolute), new Direction(angle, DirectionType.Absolute, -1f), new Speed(4f, SpeedType.Absolute), new PedestalMimicThrowStuff1.AcceleratingBullet());
			yield return base.Wait(4);
			if (j % 10 == 9)
			{
				base.Fire(new Direction(0f, DirectionType.Aim, -1f), new Speed(12f, SpeedType.Absolute), new PedestalMimicThrowStuff1.HomingShot(PedestalMimicThrowStuff1.BulletNames[i]));
				i = (i + 1) % PedestalMimicThrowStuff1.BulletNames.Length;
			}
		}
		yield break;
	}

	// Token: 0x04000B08 RID: 2824
	private static readonly string[] BulletNames = new string[] { "boot", "gun", "sponge" };

	// Token: 0x04000B09 RID: 2825
	private const float HomingSpeed = 12f;

	// Token: 0x020002B2 RID: 690
	public class AcceleratingBullet : Bullet
	{
		// Token: 0x06000A91 RID: 2705 RVA: 0x00033010 File Offset: 0x00031210
		public AcceleratingBullet()
			: base("default", false, false, false)
		{
		}

		// Token: 0x06000A92 RID: 2706 RVA: 0x00033020 File Offset: 0x00031220
		protected override IEnumerator Top()
		{
			base.ChangeSpeed(new Speed(12f, SpeedType.Absolute), 20);
			base.Wait(120);
			yield break;
		}
	}

	// Token: 0x020002B4 RID: 692
	public class HomingShot : Bullet
	{
		// Token: 0x06000A99 RID: 2713 RVA: 0x000330A0 File Offset: 0x000312A0
		public HomingShot(string bulletName)
			: base(bulletName, false, false, false)
		{
		}

		// Token: 0x06000A9A RID: 2714 RVA: 0x000330AC File Offset: 0x000312AC
		protected override IEnumerator Top()
		{
			for (int i = 0; i < 180; i++)
			{
				float aim = base.GetAimDirection(1f, 12f);
				float delta = BraveMathCollege.ClampAngle180(aim - this.Direction);
				if (Mathf.Abs(delta) > 100f)
				{
					yield break;
				}
				this.Direction += Mathf.MoveTowards(0f, delta, 1f);
				yield return base.Wait(1);
			}
			yield break;
		}

		// Token: 0x06000A9B RID: 2715 RVA: 0x000330C8 File Offset: 0x000312C8
		public override void OnBulletDestruction(Bullet.DestroyType destroyType, SpeculativeRigidbody hitRigidbody, bool preventSpawningProjectiles)
		{
			if (preventSpawningProjectiles)
			{
				return;
			}
			for (int i = 0; i < 8; i++)
			{
				base.Fire(new Direction((float)(i * 45), DirectionType.Absolute, -1f), new Speed(8f, SpeedType.Absolute), new SpeedChangingBullet(10f, 120, 600));
			}
		}
	}
}
