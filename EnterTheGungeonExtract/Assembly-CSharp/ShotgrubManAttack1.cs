using System;
using System.Collections;
using Brave.BulletScript;
using UnityEngine;

// Token: 0x020002F0 RID: 752
public class ShotgrubManAttack1 : Script
{
	// Token: 0x06000BAA RID: 2986 RVA: 0x00039370 File Offset: 0x00037570
	protected override IEnumerator Top()
	{
		float num = -22.5f;
		float num2 = 9f;
		for (int i = 0; i < 5; i++)
		{
			base.Fire(new Direction(0f, DirectionType.Aim, -1f), new Speed(9f, SpeedType.Absolute), new ShotgrubManAttack1.GrossBullet(num + (float)i * num2));
		}
		return null;
	}

	// Token: 0x04000C7E RID: 3198
	private const int NumBullets = 5;

	// Token: 0x04000C7F RID: 3199
	private const float Spread = 45f;

	// Token: 0x04000C80 RID: 3200
	private const int NumDeathBullets = 6;

	// Token: 0x04000C81 RID: 3201
	private const float GrubMagnitude = 0.75f;

	// Token: 0x04000C82 RID: 3202
	private const float GrubPeriod = 3f;

	// Token: 0x020002F1 RID: 753
	public class GrossBullet : Bullet
	{
		// Token: 0x06000BAB RID: 2987 RVA: 0x000393C8 File Offset: 0x000375C8
		public GrossBullet(float deltaAngle)
			: base("gross", false, false, false)
		{
			this.deltaAngle = deltaAngle;
		}

		// Token: 0x06000BAC RID: 2988 RVA: 0x000393E0 File Offset: 0x000375E0
		protected override IEnumerator Top()
		{
			yield return base.Wait(20);
			this.Direction += this.deltaAngle;
			this.Speed += UnityEngine.Random.Range(-1f, 1f);
			yield break;
		}

		// Token: 0x06000BAD RID: 2989 RVA: 0x000393FC File Offset: 0x000375FC
		public override void OnBulletDestruction(Bullet.DestroyType destroyType, SpeculativeRigidbody hitRigidbody, bool preventSpawningProjectiles)
		{
			if (preventSpawningProjectiles)
			{
				return;
			}
			float num = base.RandomAngle();
			float num2 = 60f;
			for (int i = 0; i < 6; i++)
			{
				base.Fire(new Direction(num + num2 * (float)i, DirectionType.Absolute, -1f), new Speed(8f, SpeedType.Absolute), new ShotgrubManAttack1.GrubBullet());
			}
		}

		// Token: 0x04000C83 RID: 3203
		private float deltaAngle;
	}

	// Token: 0x020002F3 RID: 755
	public class GrubBullet : Bullet
	{
		// Token: 0x06000BB4 RID: 2996 RVA: 0x0003952C File Offset: 0x0003772C
		public GrubBullet()
			: base(null, false, false, false)
		{
			base.SuppressVfx = true;
		}

		// Token: 0x06000BB5 RID: 2997 RVA: 0x00039540 File Offset: 0x00037740
		protected override IEnumerator Top()
		{
			base.ManualControl = true;
			Vector2 truePosition = base.Position;
			float startVal = UnityEngine.Random.value;
			for (int i = 0; i < 360; i++)
			{
				float offsetMagnitude = Mathf.SmoothStep(-0.75f, 0.75f, Mathf.PingPong(startVal + (float)i / 60f * 3f, 1f));
				truePosition += BraveMathCollege.DegreesToVector(this.Direction, this.Speed / 60f);
				base.Position = truePosition + BraveMathCollege.DegreesToVector(this.Direction - 90f, offsetMagnitude);
				yield return base.Wait(1);
			}
			base.Vanish(false);
			yield break;
		}
	}
}
