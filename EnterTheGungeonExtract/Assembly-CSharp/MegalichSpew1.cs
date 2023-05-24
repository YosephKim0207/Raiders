using System;
using System.Collections;
using Brave.BulletScript;
using FullInspector;
using UnityEngine;

// Token: 0x0200025C RID: 604
[InspectorDropdownName("Bosses/Megalich/Spew1")]
public class MegalichSpew1 : Script
{
	// Token: 0x06000921 RID: 2337 RVA: 0x0002C574 File Offset: 0x0002A774
	protected override IEnumerator Top()
	{
		for (int j = 0; j < 20; j++)
		{
			float num = base.SubdivideArc(-165f, 150f, 20, j, false) + UnityEngine.Random.Range(-3f, 3f);
			Vector2 predictedTargetPosition = base.GetPredictedTargetPosition((float)(((double)UnityEngine.Random.value >= 0.5) ? 1 : 0), 12f);
			int num2 = UnityEngine.Random.Range(0, 10);
			for (int k = 0; k < 5; k++)
			{
				base.Fire(new Direction(num, DirectionType.Absolute, -1f), new Speed(12f, SpeedType.Absolute), new MegalichSpew1.SnakeBullet(num2 + k * 3, predictedTargetPosition, false));
			}
		}
		yield return base.Wait(40);
		for (int i = 0; i < 20; i++)
		{
			float startingDirection = UnityEngine.Random.Range(-165f, -15f);
			Vector2 offset = UnityEngine.Random.insideUnitCircle;
			Vector2 targetPos = base.GetPredictedTargetPosition((float)(((double)UnityEngine.Random.value >= 0.5) ? 1 : 0), 12f);
			bool shouldHome = UnityEngine.Random.value < 0.33f;
			for (int l = 0; l < 5; l++)
			{
				base.Fire(new Offset(offset, 0f, string.Empty, DirectionType.Absolute), new Direction(startingDirection, DirectionType.Absolute, -1f), new Speed(12f, SpeedType.Absolute), new MegalichSpew1.SnakeBullet(l * 3, targetPos, shouldHome));
			}
			yield return base.Wait(10);
		}
		yield return base.Wait(20);
		yield break;
	}

	// Token: 0x0400093E RID: 2366
	private const int NumInitialSnakes = 20;

	// Token: 0x0400093F RID: 2367
	private const int NumLateSnakes = 20;

	// Token: 0x04000940 RID: 2368
	private const int NumBullets = 5;

	// Token: 0x04000941 RID: 2369
	private const int BulletSpeed = 12;

	// Token: 0x04000942 RID: 2370
	private const float SnakeMagnitude = 0.75f;

	// Token: 0x04000943 RID: 2371
	private const float SnakePeriod = 3f;

	// Token: 0x0200025D RID: 605
	public class SnakeBullet : Bullet
	{
		// Token: 0x06000922 RID: 2338 RVA: 0x0002C590 File Offset: 0x0002A790
		public SnakeBullet(int delay, Vector2 target, bool shouldHome)
			: base("spew", false, false, false)
		{
			this.m_delay = delay;
			this.m_target = target;
			this.m_shouldHome = shouldHome;
		}

		// Token: 0x06000923 RID: 2339 RVA: 0x0002C5B8 File Offset: 0x0002A7B8
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

		// Token: 0x04000944 RID: 2372
		private int m_delay;

		// Token: 0x04000945 RID: 2373
		private Vector2 m_target;

		// Token: 0x04000946 RID: 2374
		private bool m_shouldHome;
	}
}
