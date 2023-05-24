using System;
using System.Collections;
using Brave.BulletScript;
using FullInspector;
using UnityEngine;

// Token: 0x02000257 RID: 599
[InspectorDropdownName("Bosses/Megalich/SmokeRings1")]
public class MegalichSmokeRings1 : Script
{
	// Token: 0x0600090A RID: 2314 RVA: 0x0002C034 File Offset: 0x0002A234
	protected override IEnumerator Top()
	{
		base.EndOnBlank = true;
		float startDirection = base.RandomAngle();
		float delta = 15f;
		for (int i = 0; i < 4; i++)
		{
			for (int j = 0; j < 24; j++)
			{
				base.Fire(new Direction(-90f, DirectionType.Absolute, -1f), new Speed(0f, SpeedType.Absolute), new MegalichSmokeRings1.SmokeBullet(this, startDirection + (float)j * delta));
			}
			if (i < 3)
			{
				yield return base.Wait(45);
			}
		}
		yield break;
	}

	// Token: 0x04000921 RID: 2337
	private const int NumRings = 4;

	// Token: 0x04000922 RID: 2338
	private const int NumBullets = 24;

	// Token: 0x02000258 RID: 600
	public class SmokeBullet : Bullet
	{
		// Token: 0x0600090B RID: 2315 RVA: 0x0002C050 File Offset: 0x0002A250
		public SmokeBullet(MegalichSmokeRings1 parent, float angle = 0f)
			: base("ring", false, false, false)
		{
			this.m_parent = parent;
			this.m_angle = angle;
		}

		// Token: 0x0600090C RID: 2316 RVA: 0x0002C070 File Offset: 0x0002A270
		protected override IEnumerator Top()
		{
			base.ManualControl = true;
			Vector2 centerPosition = base.Position;
			float radius = 0f;
			this.m_spinSpeed = 40f;
			for (int i = 0; i < 300; i++)
			{
				if (i == 40)
				{
					base.ChangeSpeed(new Speed(18f, SpeedType.Absolute), 120);
					base.ChangeDirection(new Direction(this.m_parent.GetAimDirection(1f, 10f), DirectionType.Absolute, -1f), 20);
					base.StartTask(this.ChangeSpinSpeedTask(180f, 240));
				}
				if (i > 50 && i < 120 && UnityEngine.Random.value < 0.002f)
				{
					this.Direction = base.AimDirection;
					this.Speed = 12f;
					base.ManualControl = false;
					yield break;
				}
				base.UpdateVelocity();
				centerPosition += this.Velocity / 60f;
				if (i < 40)
				{
					radius += 0.075f;
				}
				this.m_angle += this.m_spinSpeed / 60f;
				base.Position = centerPosition + BraveMathCollege.DegreesToVector(this.m_angle, radius);
				yield return base.Wait(1);
			}
			base.Vanish(false);
			yield break;
		}

		// Token: 0x0600090D RID: 2317 RVA: 0x0002C08C File Offset: 0x0002A28C
		private IEnumerator ChangeSpinSpeedTask(float newSpinSpeed, int term)
		{
			float delta = (newSpinSpeed - this.m_spinSpeed) / (float)term;
			for (int i = 0; i < term; i++)
			{
				this.m_spinSpeed += delta;
				yield return base.Wait(1);
			}
			yield break;
		}

		// Token: 0x04000923 RID: 2339
		private const float ExpandSpeed = 4.5f;

		// Token: 0x04000924 RID: 2340
		private const float SpinSpeed = 40f;

		// Token: 0x04000925 RID: 2341
		private MegalichSmokeRings1 m_parent;

		// Token: 0x04000926 RID: 2342
		private float m_angle;

		// Token: 0x04000927 RID: 2343
		private float m_spinSpeed;
	}
}
