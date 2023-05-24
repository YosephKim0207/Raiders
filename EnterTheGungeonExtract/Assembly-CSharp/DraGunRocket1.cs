using System;
using System.Collections;
using Brave.BulletScript;
using FullInspector;
using UnityEngine;

// Token: 0x02000187 RID: 391
[InspectorDropdownName("Bosses/DraGun/Rocket1")]
public class DraGunRocket1 : Script
{
	// Token: 0x060005DF RID: 1503 RVA: 0x0001C930 File Offset: 0x0001AB30
	protected override IEnumerator Top()
	{
		if (ChallengeManager.CHALLENGE_MODE_ACTIVE)
		{
			if (UnityEngine.Random.value < 0.5f)
			{
				base.Fire(new Direction(-60f, DirectionType.Absolute, -1f), new Speed(40f, SpeedType.Absolute), new DraGunRocket1.Rocket());
				base.Fire(new Direction(-120f, DirectionType.Absolute, -1f), new Speed(20f, SpeedType.Absolute), new DraGunRocket1.Rocket());
			}
			else
			{
				base.Fire(new Direction(-60f, DirectionType.Absolute, -1f), new Speed(20f, SpeedType.Absolute), new DraGunRocket1.Rocket());
				base.Fire(new Direction(-120f, DirectionType.Absolute, -1f), new Speed(40f, SpeedType.Absolute), new DraGunRocket1.Rocket());
			}
		}
		else
		{
			base.Fire(new Direction(-90f, DirectionType.Absolute, -1f), new Speed(40f, SpeedType.Absolute), new DraGunRocket1.Rocket());
		}
		return null;
	}

	// Token: 0x040005C3 RID: 1475
	private const int NumBullets = 42;

	// Token: 0x02000188 RID: 392
	public class Rocket : Bullet
	{
		// Token: 0x060005E0 RID: 1504 RVA: 0x0001CA20 File Offset: 0x0001AC20
		public Rocket()
			: base("rocket", false, false, false)
		{
		}

		// Token: 0x060005E1 RID: 1505 RVA: 0x0001CA30 File Offset: 0x0001AC30
		protected override IEnumerator Top()
		{
			return null;
		}

		// Token: 0x060005E2 RID: 1506 RVA: 0x0001CA34 File Offset: 0x0001AC34
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
			for (int j = 0; j < 12; j++)
			{
				base.Fire(new Direction(UnityEngine.Random.Range(20f, 160f), DirectionType.Absolute, -1f), new Speed(UnityEngine.Random.Range(10f, 16f), SpeedType.Absolute), new DraGunRocket1.ShrapnelBullet());
			}
		}
	}

	// Token: 0x02000189 RID: 393
	public class ShrapnelBullet : Bullet
	{
		// Token: 0x060005E3 RID: 1507 RVA: 0x0001CB6C File Offset: 0x0001AD6C
		public ShrapnelBullet()
			: base("shrapnel", false, false, false)
		{
		}

		// Token: 0x060005E4 RID: 1508 RVA: 0x0001CB7C File Offset: 0x0001AD7C
		protected override IEnumerator Top()
		{
			base.ManualControl = true;
			yield return base.Wait(UnityEngine.Random.Range(0, 10));
			Vector2 truePosition = base.Position;
			float trueDirection = this.Direction;
			for (int i = 0; i < 360; i++)
			{
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

		// Token: 0x040005C4 RID: 1476
		private const float WiggleMagnitude = 0.75f;

		// Token: 0x040005C5 RID: 1477
		private const float WigglePeriod = 3f;
	}
}
