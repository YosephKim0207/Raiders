using System;
using System.Collections;
using Brave.BulletScript;
using FullInspector;
using UnityEngine;

// Token: 0x02000090 RID: 144
[InspectorDropdownName("Bosses/BossFinalMarine/Belch1")]
public class BossFinalMarineBelch1 : Script
{
	// Token: 0x06000237 RID: 567 RVA: 0x0000B394 File Offset: 0x00009594
	protected override IEnumerator Top()
	{
		for (int i = 0; i < 10; i++)
		{
			float startingDirection = UnityEngine.Random.Range(-150f, -30f);
			Vector2 targetPos = base.GetPredictedTargetPosition((float)(((double)UnityEngine.Random.value >= 0.5) ? 1 : 0), 12f);
			for (int j = 0; j < 5; j++)
			{
				base.Fire(new Direction(startingDirection, DirectionType.Absolute, -1f), new Speed(12f, SpeedType.Absolute), new BossFinalMarineBelch1.SnakeBullet(j * 3, targetPos));
			}
			yield return base.Wait(6);
		}
		yield break;
	}

	// Token: 0x04000254 RID: 596
	private const int NumSnakes = 10;

	// Token: 0x04000255 RID: 597
	private const int NumBullets = 5;

	// Token: 0x04000256 RID: 598
	private const int BulletSpeed = 12;

	// Token: 0x04000257 RID: 599
	private const float SnakeMagnitude = 0.75f;

	// Token: 0x04000258 RID: 600
	private const float SnakePeriod = 3f;

	// Token: 0x02000091 RID: 145
	public class SnakeBullet : Bullet
	{
		// Token: 0x06000238 RID: 568 RVA: 0x0000B3B0 File Offset: 0x000095B0
		public SnakeBullet(int delay, Vector2 target)
			: base(null, false, false, false)
		{
			this.delay = delay;
			this.target = target;
		}

		// Token: 0x06000239 RID: 569 RVA: 0x0000B3CC File Offset: 0x000095CC
		protected override IEnumerator Top()
		{
			base.ManualControl = true;
			yield return base.Wait(this.delay);
			Vector2 truePosition = base.Position;
			for (int i = 0; i < 360; i++)
			{
				float offsetMagnitude = Mathf.SmoothStep(-0.75f, 0.75f, Mathf.PingPong(0.5f + (float)i / 60f * 3f, 1f));
				if (i > 20 && i < 60)
				{
					float num = (this.target - truePosition).ToAngle();
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

		// Token: 0x04000259 RID: 601
		private int delay;

		// Token: 0x0400025A RID: 602
		private Vector2 target;
	}
}
