using System;
using System.Collections.Generic;
using Brave.BulletScript;
using FullInspector;
using UnityEngine;

// Token: 0x02000181 RID: 385
[InspectorDropdownName("Bosses/DraGun/NegativeSpace1")]
public class DraGunNegativeSpace1 : ScriptLite
{
	// Token: 0x060005C8 RID: 1480 RVA: 0x0001B7D8 File Offset: 0x000199D8
	public override void Start()
	{
		this.ActivePlatformRadius = ((!ChallengeManager.CHALLENGE_MODE_ACTIVE) ? 4f : 3.75f);
		int num = 10;
		this.m_platformCenters = new List<Vector2>(num);
		this.m_platformCenters.Add(new Vector2(UnityEngine.Random.Range(-17f, 17f), 0f));
		for (int i = 1; i < num; i++)
		{
			Vector2 vector = this.m_platformCenters[i - 1];
			DraGunNegativeSpace1.s_validPlatformIndices.Clear();
			if (i % 3 == 0 && !ChallengeManager.CHALLENGE_MODE_ACTIVE)
			{
				DraGunNegativeSpace1.s_validPlatformIndices.Add(2);
			}
			else
			{
				for (int j = 0; j < DraGunNegativeSpace1.PlatformAngles.Length; j++)
				{
					if (j != 2)
					{
						Vector2 vector2 = vector + BraveMathCollege.DegreesToVector(DraGunNegativeSpace1.PlatformAngles[j], 2f * this.ActivePlatformRadius + DraGunNegativeSpace1.PlatformDistances[j]);
						if (vector2.x > -17f && vector2.x < 17f)
						{
							DraGunNegativeSpace1.s_validPlatformIndices.Add(j);
						}
					}
				}
			}
			int num2 = BraveUtility.RandomElement<int>(DraGunNegativeSpace1.s_validPlatformIndices);
			this.m_platformCenters.Add(vector + BraveMathCollege.DegreesToVector(DraGunNegativeSpace1.PlatformAngles[num2], 2f * this.ActivePlatformRadius + DraGunNegativeSpace1.PlatformDistances[num2]));
		}
		this.m_verticalGap = 1.6f;
		this.m_lastCenterHeight = this.m_platformCenters[this.m_platformCenters.Count - 1].y;
		this.m_rowHeight = 0f;
	}

	// Token: 0x060005C9 RID: 1481 RVA: 0x0001B97C File Offset: 0x00019B7C
	public override int Update(ref int state)
	{
		if (state != 0)
		{
			return base.Done();
		}
		if (this.m_rowHeight < this.m_lastCenterHeight)
		{
			for (int i = 0; i < 19; i++)
			{
				float num = base.SubdivideRange(-17f, 17f, 19, i, false);
				Vector2 vector = new Vector2(num, this.m_rowHeight);
				bool flag = false;
				for (int j = 0; j < this.m_platformCenters.Count; j++)
				{
					if (Vector2.Distance(vector, this.m_platformCenters[j]) < this.ActivePlatformRadius)
					{
						Vector2 vector2;
						Vector2 vector3;
						int num2 = BraveMathCollege.LineCircleIntersections(this.m_platformCenters[j], this.ActivePlatformRadius, new Vector2(-17f, this.m_rowHeight), new Vector2(17f, this.m_rowHeight), out vector2, out vector3);
						if (num2 == 1)
						{
							num = vector2.x;
						}
						else
						{
							num = ((Mathf.Abs(num - vector2.x) >= Mathf.Abs(num - vector3.x)) ? vector3.x : vector2.x);
						}
						flag = true;
					}
				}
				base.Fire(new Offset(num, 18f, 0f, string.Empty, DirectionType.Absolute), new Direction(-90f, DirectionType.Absolute, -1f), new Speed(6f, SpeedType.Absolute), new DraGunNegativeSpace1.WiggleBullet(flag));
			}
			this.m_rowHeight += this.m_verticalGap;
			return base.Wait(16);
		}
		state++;
		return base.Wait(120);
	}

	// Token: 0x04000588 RID: 1416
	private const int NumPlatforms = 10;

	// Token: 0x04000589 RID: 1417
	private const int NumBullets = 19;

	// Token: 0x0400058A RID: 1418
	private const int RowDelay = 16;

	// Token: 0x0400058B RID: 1419
	private const float HalfRoomWidth = 17f;

	// Token: 0x0400058C RID: 1420
	private const int PlatformRadius = 4;

	// Token: 0x0400058D RID: 1421
	private static float[] PlatformAngles = new float[] { 155f, 125f, 90f, 55f, 25f };

	// Token: 0x0400058E RID: 1422
	private static float[] PlatformDistances = new float[] { 1f, 2.5f, 3f, 2.5f, 1f };

	// Token: 0x0400058F RID: 1423
	private static List<int> s_validPlatformIndices = new List<int>();

	// Token: 0x04000590 RID: 1424
	private float ActivePlatformRadius;

	// Token: 0x04000591 RID: 1425
	private List<Vector2> m_platformCenters;

	// Token: 0x04000592 RID: 1426
	private float m_verticalGap;

	// Token: 0x04000593 RID: 1427
	private float m_lastCenterHeight;

	// Token: 0x04000594 RID: 1428
	private float m_rowHeight;

	// Token: 0x02000182 RID: 386
	public class WiggleBullet : BulletLite
	{
		// Token: 0x060005CB RID: 1483 RVA: 0x0001BB48 File Offset: 0x00019D48
		public WiggleBullet(bool suppressOffset)
			: base("default_novfx", false, false)
		{
			this.m_suppressOffset = suppressOffset;
		}

		// Token: 0x060005CC RID: 1484 RVA: 0x0001BB60 File Offset: 0x00019D60
		public override void Start()
		{
			base.ManualControl = true;
			this.m_truePosition = base.Position;
			this.m_offset = Vector2.zero;
			this.m_xMagnitude = UnityEngine.Random.Range(0f, 0.6f);
			this.m_xPeriod = UnityEngine.Random.Range(1f, 2.5f);
			this.m_yMagnitude = UnityEngine.Random.Range(0f, 0.4f);
			this.m_yPeriod = UnityEngine.Random.Range(1f, 2.5f);
			this.m_delta = BraveMathCollege.DegreesToVector(this.Direction, this.Speed / 60f);
		}

		// Token: 0x060005CD RID: 1485 RVA: 0x0001BBFC File Offset: 0x00019DFC
		public override int Update(ref int state)
		{
			if (base.Tick >= 360)
			{
				base.Vanish(false);
				return base.Done();
			}
			if (!this.m_suppressOffset)
			{
				float num = 0.5f + (float)base.Tick / 60f * this.m_xPeriod;
				num = Mathf.Repeat(num, 2f);
				float num2 = 1f - Mathf.Abs(num - 1f);
				num2 = Mathf.Clamp01(num2);
				num2 = (float)(-2.0 * (double)num2 * (double)num2 * (double)num2 + 3.0 * (double)num2 * (double)num2);
				this.m_offset.x = (float)((double)this.m_xMagnitude * (double)num2 + (double)(-(double)this.m_xMagnitude) * (1.0 - (double)num2));
				float num3 = 0.5f + (float)base.Tick / 60f * this.m_yPeriod;
				num3 = Mathf.Repeat(num3, 2f);
				float num4 = 1f - Mathf.Abs(num3 - 1f);
				num4 = Mathf.Clamp01(num4);
				num4 = (float)(-2.0 * (double)num4 * (double)num4 * (double)num4 + 3.0 * (double)num4 * (double)num4);
				this.m_offset.y = (float)((double)this.m_yMagnitude * (double)num4 + (double)(-(double)this.m_yMagnitude) * (1.0 - (double)num4));
			}
			this.m_truePosition += this.m_delta;
			base.Position = this.m_truePosition + this.m_offset;
			return base.Wait(1);
		}

		// Token: 0x04000595 RID: 1429
		private bool m_suppressOffset;

		// Token: 0x04000596 RID: 1430
		private Vector2 m_truePosition;

		// Token: 0x04000597 RID: 1431
		private Vector2 m_offset;

		// Token: 0x04000598 RID: 1432
		private float m_xMagnitude;

		// Token: 0x04000599 RID: 1433
		private float m_xPeriod;

		// Token: 0x0400059A RID: 1434
		private float m_yMagnitude;

		// Token: 0x0400059B RID: 1435
		private float m_yPeriod;

		// Token: 0x0400059C RID: 1436
		private Vector2 m_delta;
	}
}
