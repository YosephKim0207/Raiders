using System;
using System.Collections;
using Brave.BulletScript;
using FullInspector;
using UnityEngine;

// Token: 0x0200018B RID: 395
[InspectorDropdownName("Bosses/DraGun/Rocket2")]
public class DraGunRocket2 : Script
{
	// Token: 0x060005EC RID: 1516 RVA: 0x0001CDBC File Offset: 0x0001AFBC
	protected override IEnumerator Top()
	{
		base.Fire(new Direction(-90f, DirectionType.Absolute, -1f), new Speed(40f, SpeedType.Absolute), new DraGunRocket2.Rocket());
		return null;
	}

	// Token: 0x040005CF RID: 1487
	private const int NumBullets = 42;

	// Token: 0x0200018C RID: 396
	public class Rocket : Bullet
	{
		// Token: 0x060005ED RID: 1517 RVA: 0x0001CDE8 File Offset: 0x0001AFE8
		public Rocket()
			: base("rocket", false, false, false)
		{
		}

		// Token: 0x060005EE RID: 1518 RVA: 0x0001CDF8 File Offset: 0x0001AFF8
		protected override IEnumerator Top()
		{
			return null;
		}

		// Token: 0x060005EF RID: 1519 RVA: 0x0001CDFC File Offset: 0x0001AFFC
		public override void OnBulletDestruction(Bullet.DestroyType destroyType, SpeculativeRigidbody hitRigidbody, bool preventSpawningProjectiles)
		{
			for (int i = 0; i < 42; i++)
			{
				base.Fire(new Direction(base.SubdivideArc(-10f, 200f, 42, i, false), DirectionType.Absolute, -1f), new Speed(12f, SpeedType.Absolute), new Bullet("default_novfx", false, false, false));
				if (i < 41)
				{
					base.Fire(new Direction(base.SubdivideArc(-10f, 200f, 42, i, true), DirectionType.Absolute, -1f), new Speed(8f, SpeedType.Absolute), new SpeedChangingBullet("default_novfx", 12f, 60, -1, false));
				}
				base.Fire(new Direction(base.SubdivideArc(-10f, 200f, 42, i, false), DirectionType.Absolute, -1f), new Speed(4f, SpeedType.Absolute), new SpeedChangingBullet("default_novfx", 12f, 60, -1, false));
			}
			for (int j = 0; j < 5; j++)
			{
				base.Fire(new Offset(new Vector2(0f, -1f), 0f, string.Empty, DirectionType.Absolute), new Direction(180f, DirectionType.Absolute, -1f), new Speed((float)(16 - j * 4), SpeedType.Absolute), new SpeedChangingBullet("default_novfx", 12f, 60, -1, false));
				base.Fire(new Offset(new Vector2(0f, -1f), 0f, string.Empty, DirectionType.Absolute), new Direction(0f, DirectionType.Absolute, -1f), new Speed((float)(16 - j * 4), SpeedType.Absolute), new SpeedChangingBullet("default_novfx", 12f, 60, -1, false));
			}
			for (int k = 0; k < 12; k++)
			{
				float num;
				if (k % 2 == 0)
				{
					num = UnityEngine.Random.Range(150f, 182f);
				}
				else
				{
					num = UnityEngine.Random.Range(0f, 35f);
				}
				base.Fire(new Direction(num, DirectionType.Absolute, -1f), new Speed(UnityEngine.Random.Range(4f, 12f), SpeedType.Absolute), new DraGunRocket2.ShrapnelBullet());
			}
		}
	}

	// Token: 0x0200018D RID: 397
	public class ShrapnelBullet : Bullet
	{
		// Token: 0x060005F0 RID: 1520 RVA: 0x0001D010 File Offset: 0x0001B210
		public ShrapnelBullet()
			: base("shrapnel", false, false, false)
		{
		}

		// Token: 0x060005F1 RID: 1521 RVA: 0x0001D020 File Offset: 0x0001B220
		protected override IEnumerator Top()
		{
			base.ChangeSpeed(new Speed(12f, SpeedType.Absolute), 60);
			BounceProjModifier bounce = this.Projectile.GetComponent<BounceProjModifier>();
			bool hasBounced = false;
			base.ManualControl = true;
			yield return base.Wait(UnityEngine.Random.Range(0, 10));
			Vector2 truePosition = base.Position;
			float trueDirection = this.Direction;
			for (int i = 0; i < 360; i++)
			{
				if (!hasBounced && bounce.numberOfBounces == 0)
				{
					trueDirection = BraveMathCollege.QuantizeFloat(trueDirection, 90f) + 180f;
					this.Speed = 18f;
					hasBounced = true;
				}
				float offsetMagnitude = Mathf.SmoothStep(-0.75f, 0.75f, Mathf.PingPong(0.5f + (float)i / 60f * 3f, 1f));
				Vector2 lastPosition = truePosition;
				truePosition += BraveMathCollege.DegreesToVector(trueDirection, this.Speed / 60f);
				base.Position = truePosition + BraveMathCollege.DegreesToVector(trueDirection - 90f, offsetMagnitude);
				this.Direction = (truePosition - lastPosition).ToAngle();
				this.Projectile.transform.rotation = Quaternion.Euler(0f, 0f, this.Direction);
				yield return base.Wait(1);
			}
			base.Vanish(false);
			yield break;
		}

		// Token: 0x040005D0 RID: 1488
		private const float WiggleMagnitude = 0.75f;

		// Token: 0x040005D1 RID: 1489
		private const float WigglePeriod = 3f;
	}
}
