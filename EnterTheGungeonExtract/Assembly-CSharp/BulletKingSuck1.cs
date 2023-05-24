using System;
using System.Collections;
using Brave.BulletScript;
using FullInspector;
using UnityEngine;

// Token: 0x02000103 RID: 259
[InspectorDropdownName("Bosses/BulletKing/Suck1")]
public class BulletKingSuck1 : Script
{
	// Token: 0x060003DD RID: 989 RVA: 0x00012F94 File Offset: 0x00011194
	protected override IEnumerator Top()
	{
		base.EndOnBlank = true;
		float startAngle = base.RandomAngle();
		float radius = 1f;
		for (int j = 0; j < 20; j++)
		{
			for (int k = 0; k < 6; k++)
			{
				float num = base.SubdivideCircle(startAngle, 6, k, 1f, false);
				Vector2 vector = base.Position + BraveMathCollege.DegreesToVector(num, radius);
				base.Fire(Offset.OverridePosition(vector), new Direction(num, DirectionType.Absolute, -1f), new BulletKingSuck1.SuckBullet(base.Position, num, j));
			}
		}
		yield return base.Wait(110);
		for (int i = 159; i >= 0; i--)
		{
			if (i % 20 == 0)
			{
				float num2 = base.RandomAngle();
				base.Fire(new Offset(1f, 0f, num2, string.Empty, DirectionType.Absolute), new Direction(num2, DirectionType.Absolute, -1f), new Speed(7f, SpeedType.Absolute), new Bullet("suck", false, false, false));
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x040003D2 RID: 978
	private const int NumBulletRings = 20;

	// Token: 0x040003D3 RID: 979
	private const int BulletsPerRing = 6;

	// Token: 0x040003D4 RID: 980
	private const float AngleDeltaPerRing = 10f;

	// Token: 0x040003D5 RID: 981
	private const float StartRadius = 1f;

	// Token: 0x040003D6 RID: 982
	private const float RadiusPerRing = 1f;

	// Token: 0x040003D7 RID: 983
	public static float SpinningBulletSpinSpeed = 180f;

	// Token: 0x02000104 RID: 260
	public class SuckBullet : Bullet
	{
		// Token: 0x060003DF RID: 991 RVA: 0x00012FBC File Offset: 0x000111BC
		public SuckBullet(Vector2 centerPoint, float startAngle, int i)
			: base("suck", false, false, false)
		{
			this.m_centerPoint = centerPoint;
			this.m_startAngle = startAngle;
			this.m_index = i;
		}

		// Token: 0x060003E0 RID: 992 RVA: 0x00012FE4 File Offset: 0x000111E4
		protected override IEnumerator Top()
		{
			base.ManualControl = true;
			float radius = 1f;
			float angle = this.m_startAngle;
			int remainingWait = 110;
			for (int i = 1; i < this.m_index; i++)
			{
				int steps = Mathf.Max(4, 14 - i * 2);
				float deltaRadius = 1f / (float)steps;
				float deltaAngle = 10f / (float)steps;
				for (int j = 0; j < steps; j++)
				{
					radius += deltaRadius;
					angle += deltaAngle;
					base.Position = this.m_centerPoint + BraveMathCollege.DegreesToVector(angle, radius);
					yield return base.Wait(1);
					remainingWait--;
				}
			}
			bool isDoingTell = false;
			while (remainingWait > 0)
			{
				if (!isDoingTell && remainingWait <= 60)
				{
					this.Projectile.spriteAnimator.Play("enemy_projectile_small_fire_tell");
					isDoingTell = true;
				}
				remainingWait--;
				yield return base.Wait(1);
			}
			this.Direction = (this.m_centerPoint - base.Position).ToAngle();
			float distToTravel = (this.m_centerPoint - base.Position).magnitude;
			base.ManualControl = false;
			for (int k = 0; k < 240; k++)
			{
				if (k < 40)
				{
					this.Speed += 0.2f;
				}
				distToTravel -= this.Speed / 60f;
				if (distToTravel < 2f)
				{
					base.Vanish(false);
					break;
				}
				yield return null;
			}
			base.Vanish(false);
			yield break;
		}

		// Token: 0x040003D8 RID: 984
		private Vector2 m_centerPoint;

		// Token: 0x040003D9 RID: 985
		private float m_startAngle;

		// Token: 0x040003DA RID: 986
		private int m_index;
	}
}
