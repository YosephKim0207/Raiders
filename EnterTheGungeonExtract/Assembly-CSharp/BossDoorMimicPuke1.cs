using System;
using System.Collections;
using Brave.BulletScript;
using FullInspector;
using UnityEngine;

// Token: 0x02000056 RID: 86
[InspectorDropdownName("Bosses/BossDoorMimic/Puke1")]
public class BossDoorMimicPuke1 : Script
{
	// Token: 0x0600014D RID: 333 RVA: 0x0000707C File Offset: 0x0000527C
	protected override IEnumerator Top()
	{
		float pulseStartAngle = base.RandomAngle();
		for (int j = 0; j < 32; j++)
		{
			float num = base.SubdivideCircle(pulseStartAngle, 32, j, 1f, false);
			base.Fire(new Direction(num, DirectionType.Absolute, -1f), new Speed(4.5f, SpeedType.Absolute), new BossDoorMimicPuke1.PulseBullet((float)(2 * j) / 32f));
		}
		for (int k = 0; k < 8; k++)
		{
			float num2 = base.RandomAngle();
			Vector2 predictedTargetPosition = base.GetPredictedTargetPosition((float)(((double)UnityEngine.Random.value >= 0.5) ? 1 : 0), 8f);
			int num3 = UnityEngine.Random.Range(0, 10);
			for (int l = 0; l < 5; l++)
			{
				base.Fire(new Offset(new Vector2(0f, 1f), num2, string.Empty, DirectionType.Absolute), new Direction(num2, DirectionType.Absolute, -1f), new Speed(8f, SpeedType.Absolute), new BossDoorMimicPuke1.SnakeBullet(num3 + l * 3, predictedTargetPosition, false));
			}
		}
		yield return base.Wait(40);
		for (int m = 0; m < 32; m++)
		{
			float num4 = base.SubdivideCircle(pulseStartAngle, 32, m, 1f, true);
			base.Fire(new Direction(num4, DirectionType.Absolute, -1f), new Speed(4.5f, SpeedType.Absolute), new BossDoorMimicPuke1.PulseBullet((float)(2 * m) / 32f));
		}
		for (int i = 0; i < 6; i++)
		{
			float direction = base.RandomAngle();
			Vector2 targetPos = base.GetPredictedTargetPosition((float)(((double)UnityEngine.Random.value >= 0.5) ? 1 : 0), 8f);
			bool shouldHome = UnityEngine.Random.value < 0.33f;
			for (int n = 0; n < 5; n++)
			{
				base.Fire(new Offset(new Vector2(0f, 1f), direction, string.Empty, DirectionType.Absolute), new Direction(direction, DirectionType.Absolute, -1f), new Speed(8f, SpeedType.Absolute), new BossDoorMimicPuke1.SnakeBullet(n * 3, targetPos, shouldHome));
			}
			yield return base.Wait(10);
		}
		yield return base.Wait(20);
		yield break;
	}

	// Token: 0x04000149 RID: 329
	private const int NumPulseBullets = 32;

	// Token: 0x0400014A RID: 330
	private const float PulseBulletSpeed = 4.5f;

	// Token: 0x0400014B RID: 331
	private const int NumInitialSnakes = 8;

	// Token: 0x0400014C RID: 332
	private const int NumLateSnakes = 6;

	// Token: 0x0400014D RID: 333
	private const int NumBulletsInSnake = 5;

	// Token: 0x0400014E RID: 334
	private const int SnakeBulletSpeed = 8;

	// Token: 0x02000057 RID: 87
	public class PulseBullet : Bullet
	{
		// Token: 0x0600014E RID: 334 RVA: 0x00007098 File Offset: 0x00005298
		public PulseBullet(float initialOffest)
			: base("puke_burst", false, false, false)
		{
			this.m_initialOffest = initialOffest;
		}

		// Token: 0x0600014F RID: 335 RVA: 0x000070B0 File Offset: 0x000052B0
		protected override IEnumerator Top()
		{
			base.ManualControl = true;
			Vector2 truePosition = base.Position;
			Vector2 offsetDir = BraveMathCollege.DegreesToVector(this.Direction, 1f);
			for (int i = 0; i < 600; i++)
			{
				base.UpdateVelocity();
				truePosition += this.Velocity / 60f;
				float mag = Mathf.Sin((this.m_initialOffest + (float)base.Tick / 60f / 0.75f) * 3.1415927f) * 0.75f;
				if (i < 60)
				{
					mag *= (float)i / 60f;
				}
				base.Position = truePosition + offsetDir * mag;
				yield return base.Wait(1);
			}
			base.Vanish(false);
			yield break;
		}

		// Token: 0x0400014F RID: 335
		private const float SinPeriod = 0.75f;

		// Token: 0x04000150 RID: 336
		private const float SinMagnitude = 0.75f;

		// Token: 0x04000151 RID: 337
		private float m_initialOffest;
	}

	// Token: 0x02000059 RID: 89
	public class SnakeBullet : Bullet
	{
		// Token: 0x06000156 RID: 342 RVA: 0x00007288 File Offset: 0x00005488
		public SnakeBullet(int delay, Vector2 target, bool shouldHome)
			: base("puke_snake", false, false, false)
		{
			this.m_delay = delay;
			this.m_target = target;
			this.m_shouldHome = shouldHome;
		}

		// Token: 0x06000157 RID: 343 RVA: 0x000072B0 File Offset: 0x000054B0
		protected override IEnumerator Top()
		{
			base.ManualControl = true;
			yield return base.Wait(this.m_delay);
			Vector2 truePosition = base.Position;
			for (int i = 0; i < 360; i++)
			{
				float offsetMagnitude = Mathf.SmoothStep(-0.75f, 0.75f, Mathf.PingPong(0.5f + (float)i / 60f * 3f, 1f));
				if (this.m_shouldHome && i > 20 && i < 60)
				{
					float num = (this.m_target - truePosition).ToAngle();
					float num2 = BraveMathCollege.ClampAngle180(num - this.Direction);
					this.Direction += Mathf.Clamp(num2, -6f, 6f);
				}
				truePosition += BraveMathCollege.DegreesToVector(this.Direction, this.Speed / 60f);
				base.Position = truePosition + BraveMathCollege.DegreesToVector(this.Direction - 90f, offsetMagnitude);
				yield return base.Wait(1);
			}
			base.Vanish(false);
			yield break;
		}

		// Token: 0x0400015A RID: 346
		private const float SnakeMagnitude = 0.75f;

		// Token: 0x0400015B RID: 347
		private const float SnakePeriod = 3f;

		// Token: 0x0400015C RID: 348
		private int m_delay;

		// Token: 0x0400015D RID: 349
		private Vector2 m_target;

		// Token: 0x0400015E RID: 350
		private bool m_shouldHome;
	}
}
