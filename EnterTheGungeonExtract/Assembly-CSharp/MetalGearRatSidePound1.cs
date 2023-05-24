using System;
using System.Collections;
using Brave.BulletScript;
using UnityEngine;

// Token: 0x02000271 RID: 625
public abstract class MetalGearRatSidePound1 : Script
{
	// Token: 0x17000230 RID: 560
	// (get) Token: 0x0600097B RID: 2427
	protected abstract float StartAngle { get; }

	// Token: 0x17000231 RID: 561
	// (get) Token: 0x0600097C RID: 2428
	protected abstract float SweepAngle { get; }

	// Token: 0x0600097D RID: 2429 RVA: 0x0002DDCC File Offset: 0x0002BFCC
	protected override IEnumerator Top()
	{
		int i = 0;
		while ((float)i < 7f)
		{
			bool isOffset = i % 2 == 1;
			int numBullets = 9 - i;
			for (int j = 0; j < numBullets + ((!isOffset) ? 0 : (-1)); j++)
			{
				float num = base.SubdivideArc(this.StartAngle, this.SweepAngle, numBullets, j, isOffset);
				Vector2 ellipsePointSmooth = BraveMathCollege.GetEllipsePointSmooth(Vector2.zero, 2.5f, 1f, num);
				base.Fire(new Offset(ellipsePointSmooth, 0f, string.Empty, DirectionType.Absolute), new Direction(num, DirectionType.Absolute, -1f), new Speed((float)(14 - i * 2), SpeedType.Absolute), new MetalGearRatSidePound1.WaftBullet());
			}
			yield return base.Wait(6);
			i++;
		}
		yield break;
	}

	// Token: 0x040009B8 RID: 2488
	private const float NumWaves = 7f;

	// Token: 0x040009B9 RID: 2489
	private const int NumBullets = 9;

	// Token: 0x040009BA RID: 2490
	private const float EllipseA = 2.5f;

	// Token: 0x040009BB RID: 2491
	private const float EllipseB = 1f;

	// Token: 0x040009BC RID: 2492
	private const float VerticalDriftVelocity = 0.5f;

	// Token: 0x040009BD RID: 2493
	private const float WaftXPeriod = 3f;

	// Token: 0x040009BE RID: 2494
	private const float WaftXMagnitude = 0.65f;

	// Token: 0x040009BF RID: 2495
	private const float WaftYPeriod = 1f;

	// Token: 0x040009C0 RID: 2496
	private const float WaftYMagnitude = 0.25f;

	// Token: 0x040009C1 RID: 2497
	private const int WaftLifeTime = 300;

	// Token: 0x02000272 RID: 626
	public class WaftBullet : Bullet
	{
		// Token: 0x0600097E RID: 2430 RVA: 0x0002DDE8 File Offset: 0x0002BFE8
		public WaftBullet()
			: base("default_noramp", false, false, false)
		{
		}

		// Token: 0x0600097F RID: 2431 RVA: 0x0002DDF8 File Offset: 0x0002BFF8
		protected override IEnumerator Top()
		{
			base.ChangeSpeed(new Speed(0f, SpeedType.Absolute), 150);
			yield return base.Wait(150);
			base.ManualControl = true;
			Vector2 truePosition = base.Position;
			float xOffset = UnityEngine.Random.Range(0f, 3f);
			float yOffset = UnityEngine.Random.Range(0f, 1f);
			truePosition -= new Vector2(Mathf.Sin(xOffset * 3.1415927f / 3f) * 0.65f, Mathf.Sin(yOffset * 3.1415927f / 1f) * 0.25f);
			for (int i = 0; i < 300; i++)
			{
				truePosition += new Vector2(0f, 0.008333334f);
				float t = (float)i / 60f;
				float waftXOffset = Mathf.Sin((t + xOffset) * 3.1415927f / 3f) * 0.65f;
				float waftYOffset = Mathf.Sin((t + yOffset) * 3.1415927f / 1f) * 0.25f;
				base.Position = truePosition + new Vector2(waftXOffset, waftYOffset);
				yield return base.Wait(1);
			}
			base.Vanish(false);
			yield break;
		}
	}
}
