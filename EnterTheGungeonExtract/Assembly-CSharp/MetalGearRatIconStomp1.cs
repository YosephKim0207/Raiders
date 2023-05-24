using System;
using System.Collections;
using System.Collections.Generic;
using Brave.BulletScript;
using FullInspector;
using UnityEngine;

// Token: 0x02000264 RID: 612
[InspectorDropdownName("Bosses/MetalGearRat/IconStomp1")]
public class MetalGearRatIconStomp1 : Script
{
	// Token: 0x06000941 RID: 2369 RVA: 0x0002CDC0 File Offset: 0x0002AFC0
	protected override IEnumerator Top()
	{
		yield return base.Wait(160);
		List<MetalGearRatIconStomp1.LineDummy> lineDummies = new List<MetalGearRatIconStomp1.LineDummy>(this.delay.Length);
		base.StartTask(this.FireBullets(lineDummies));
		for (int i = 0; i < 180; i++)
		{
			for (int j = 0; j < lineDummies.Count; j++)
			{
				lineDummies[j].DoTick();
			}
			yield return base.Wait(1);
		}
		yield break;
	}

	// Token: 0x06000942 RID: 2370 RVA: 0x0002CDDC File Offset: 0x0002AFDC
	private IEnumerator FireBullets(List<MetalGearRatIconStomp1.LineDummy> lineDummies)
	{
		float direction = base.AimDirection;
		float flipScalar = (float)((BraveMathCollege.AbsAngleBetween(direction, 90f) >= 90f) ? 1 : (-1));
		for (int i = 0; i < this.delay.Length; i++)
		{
			Vector2 spawnOffset = PhysicsEngine.PixelToUnit(new IntVector2(this.xOffsets1[i], this.yOffsets1[i]));
			Vector2 spawnOffset2 = PhysicsEngine.PixelToUnit(new IntVector2(this.xOffsets2[i], this.yOffsets2[i]));
			MetalGearRatIconStomp1.LineDummy lineDummy = new MetalGearRatIconStomp1.LineDummy();
			lineDummy.Position = base.Position + (spawnOffset + spawnOffset2) / 2f;
			lineDummy.Direction = direction;
			lineDummy.Speed = 9f;
			lineDummy.BulletManager = this.BulletManager;
			lineDummy.Initialize();
			lineDummies.Add(lineDummy);
			for (int j = 0; j < 8; j++)
			{
				float num = base.SubdivideRange(0f, 10f, 8, j, false) + 1.75f;
				base.Fire(new Offset(spawnOffset, 0f, string.Empty, DirectionType.Absolute), new Direction(direction, DirectionType.Absolute, -1f), new MetalGearRatIconStomp1.IconBullet(lineDummy, -num * flipScalar));
				base.Fire(new Offset(spawnOffset2, 0f, string.Empty, DirectionType.Absolute), new Direction(direction, DirectionType.Absolute, -1f), new MetalGearRatIconStomp1.IconBullet(lineDummy, num * flipScalar));
			}
			yield return base.Wait(this.delay[i]);
			direction += (float)UnityEngine.Random.Range(-1, 2) * 8f;
		}
		yield break;
	}

	// Token: 0x04000962 RID: 2402
	public int[] delay = new int[] { 10, 15, 15, 15, 10, 15, 10, 10, 0 };

	// Token: 0x04000963 RID: 2403
	public int[] xOffsets1 = new int[] { 0, -4, -9, -13, -16, -19, -18, -15, -11 };

	// Token: 0x04000964 RID: 2404
	public int[] yOffsets1 = new int[] { 0, 3, 8, 13, 16, 21, 36, 39, 44 };

	// Token: 0x04000965 RID: 2405
	public int[] xOffsets2 = new int[] { 40, 33, 26, 19, 14, 8, 7, 11, 16 };

	// Token: 0x04000966 RID: 2406
	public int[] yOffsets2 = new int[] { -1, 2, 5, 8, 11, 18, 28, 38, 43 };

	// Token: 0x04000967 RID: 2407
	private const int SpreadTime = 60;

	// Token: 0x04000968 RID: 2408
	private const int HalfBulletsPerLine = 8;

	// Token: 0x04000969 RID: 2409
	private const float LineWidth = 20f;

	// Token: 0x0400096A RID: 2410
	private const float GapWidth = 3.5f;

	// Token: 0x0400096B RID: 2411
	private const float BulletSpeed = 9f;

	// Token: 0x0400096C RID: 2412
	private const float AngleVariance = 8f;

	// Token: 0x0400096D RID: 2413
	private const float PositionVariance = 0.4f;

	// Token: 0x02000265 RID: 613
	private class LineDummy : Bullet
	{
		// Token: 0x06000943 RID: 2371 RVA: 0x0002CE00 File Offset: 0x0002B000
		public LineDummy()
			: base(null, false, false, false)
		{
		}

		// Token: 0x06000944 RID: 2372 RVA: 0x0002CE0C File Offset: 0x0002B00C
		protected override IEnumerator Top()
		{
			for (;;)
			{
				yield return base.Wait(1);
			}
			yield break;
		}
	}

	// Token: 0x02000267 RID: 615
	private class IconBullet : Bullet
	{
		// Token: 0x0600094B RID: 2379 RVA: 0x0002CEC0 File Offset: 0x0002B0C0
		public IconBullet(MetalGearRatIconStomp1.LineDummy lineDummy, float scale)
			: base(null, false, false, false)
		{
			this.m_lineDummy = lineDummy;
			this.scale = scale;
		}

		// Token: 0x0600094C RID: 2380 RVA: 0x0002CEDC File Offset: 0x0002B0DC
		protected override IEnumerator Top()
		{
			Vector2 startingOffset = base.Position - this.m_lineDummy.Position;
			Vector2 desiredOffset = BraveMathCollege.DegreesToVector(this.Direction + 90f, this.scale) + new Vector2(0f, UnityEngine.Random.Range(-0.4f, 0.4f));
			base.ManualControl = true;
			for (int i = 0; i < 61; i++)
			{
				base.Position = this.m_lineDummy.Position + Vector2.Lerp(startingOffset, desiredOffset, (float)base.Tick / 60f);
				yield return base.Wait(1);
			}
			this.Speed = 9f;
			base.ManualControl = false;
			yield break;
		}

		// Token: 0x04000972 RID: 2418
		private MetalGearRatIconStomp1.LineDummy m_lineDummy;

		// Token: 0x04000973 RID: 2419
		private float scale;
	}
}
