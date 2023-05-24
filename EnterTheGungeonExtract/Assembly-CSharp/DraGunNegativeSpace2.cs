using System;
using System.Collections.Generic;
using Brave.BulletScript;
using FullInspector;
using UnityEngine;

// Token: 0x02000183 RID: 387
[InspectorDropdownName("Bosses/DraGun/NegativeSpace2")]
public class DraGunNegativeSpace2 : ScriptLite
{
	// Token: 0x060005CF RID: 1487 RVA: 0x0001BDA0 File Offset: 0x00019FA0
	public override void Start()
	{
		this.ActivePlatformRadius = 4f;
		int num = 8;
		this.m_platformCenters = new List<Vector2>(10);
		this.m_platformCenters.Add(new Vector2(UnityEngine.Random.Range(-17f, 17f) / 2f, 0f));
		for (int i = 1; i < 10; i++)
		{
			Vector2 vector = this.m_platformCenters[this.m_platformCenters.Count - 1];
			DraGunNegativeSpace2.s_validPlatformIndices.Clear();
			for (int j = 0; j < DraGunNegativeSpace2.PlatformAngles.Length; j++)
			{
				Vector2 vector2 = vector + BraveMathCollege.DegreesToVector(DraGunNegativeSpace2.PlatformAngles[j], 2f * this.ActivePlatformRadius + DraGunNegativeSpace2.PlatformDistances[j]);
				if (vector2.x > -17f && vector2.x < 17f && Vector2.Distance(vector2, this.m_platformCenters[this.m_platformCenters.Count - 1]) > (float)num && (i < 2 || Vector2.Distance(vector2, this.m_platformCenters[this.m_platformCenters.Count - 2]) > (float)num) && (i < 3 || Vector2.Distance(vector2, this.m_platformCenters[this.m_platformCenters.Count - 3]) > (float)num))
				{
					DraGunNegativeSpace2.s_validPlatformIndices.Add(j);
				}
			}
			if (DraGunNegativeSpace2.s_validPlatformIndices.Count == 0)
			{
				DraGunNegativeSpace2.s_validPlatformIndices.Add(2);
			}
			int num2 = BraveUtility.RandomElement<int>(DraGunNegativeSpace2.s_validPlatformIndices);
			this.m_platformCenters.Add(vector + BraveMathCollege.DegreesToVector(DraGunNegativeSpace2.PlatformAngles[num2], 2f * this.ActivePlatformRadius + DraGunNegativeSpace2.PlatformDistances[num2]));
			if (i % 2 == 1)
			{
				DraGunNegativeSpace2.s_validPlatformIndices.Remove(num2);
				for (int k = DraGunNegativeSpace2.s_validPlatformIndices.Count - 1; k >= 0; k--)
				{
					int num3 = DraGunNegativeSpace2.s_validPlatformIndices[k];
					Vector2 vector3 = vector + BraveMathCollege.DegreesToVector(DraGunNegativeSpace2.PlatformAngles[num3], 2f * this.ActivePlatformRadius + DraGunNegativeSpace2.PlatformDistances[num3]);
					if (Vector2.Distance(vector3, this.m_platformCenters[this.m_platformCenters.Count - 1]) < (float)num || Vector2.Distance(vector3, this.m_platformCenters[this.m_platformCenters.Count - 2]) < (float)num)
					{
						DraGunNegativeSpace2.s_validPlatformIndices.RemoveAt(k);
					}
				}
				if (DraGunNegativeSpace2.s_validPlatformIndices.Count > 0)
				{
					num2 = BraveUtility.RandomElement<int>(DraGunNegativeSpace2.s_validPlatformIndices);
					this.m_platformCenters.Add(vector + BraveMathCollege.DegreesToVector(DraGunNegativeSpace2.PlatformAngles[num2], 2f * this.ActivePlatformRadius + DraGunNegativeSpace2.PlatformDistances[num2]));
				}
			}
		}
		this.m_verticalGap = 1.6f;
		this.m_lastCenterHeight = this.m_platformCenters[this.m_platformCenters.Count - 1].y;
		this.m_rowHeight = 0f;
		this.m_centerBullets.Clear();
		for (int l = 0; l < this.m_platformCenters.Count; l++)
		{
			this.m_centerBullets.Add(false);
		}
	}

	// Token: 0x060005D0 RID: 1488 RVA: 0x0001C0F8 File Offset: 0x0001A2F8
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
				base.Fire(new Offset(num, 18f, 0f, string.Empty, DirectionType.Absolute), new Direction(-90f, DirectionType.Absolute, -1f), new Speed(6f, SpeedType.Absolute), new DraGunNegativeSpace2.WiggleBullet(flag));
			}
			this.m_rowHeight += this.m_verticalGap;
			for (int k = 0; k < this.m_platformCenters.Count; k++)
			{
				if (!this.m_centerBullets[k] && this.m_platformCenters[k].y < this.m_rowHeight - 2f)
				{
					if (BraveUtility.RandomBool())
					{
						base.Fire(new Offset(this.m_platformCenters[k].x, 20.5f - this.m_rowHeight + this.m_platformCenters[k].y, 0f, string.Empty, DirectionType.Absolute), new Direction(-90f, DirectionType.Absolute, -1f), new Speed(6f, SpeedType.Absolute), new DraGunNegativeSpace2.WiggleBullet(true));
					}
					this.m_centerBullets[k] = true;
				}
			}
			return base.Wait(16);
		}
		state++;
		return base.Wait(120);
	}

	// Token: 0x0400059D RID: 1437
	private const int NumPlatforms = 10;

	// Token: 0x0400059E RID: 1438
	private const int NumBullets = 19;

	// Token: 0x0400059F RID: 1439
	private const int RowDelay = 16;

	// Token: 0x040005A0 RID: 1440
	private const float HalfRoomWidth = 17f;

	// Token: 0x040005A1 RID: 1441
	private const int PlatformRadius = 4;

	// Token: 0x040005A2 RID: 1442
	private static float[] PlatformAngles = new float[] { 155f, 125f, 90f, 55f, 25f };

	// Token: 0x040005A3 RID: 1443
	private static float[] PlatformDistances = new float[] { 1f, 2.5f, 3f, 2.5f, 1f };

	// Token: 0x040005A4 RID: 1444
	private static List<int> s_validPlatformIndices = new List<int>();

	// Token: 0x040005A5 RID: 1445
	private float ActivePlatformRadius;

	// Token: 0x040005A6 RID: 1446
	private List<Vector2> m_platformCenters;

	// Token: 0x040005A7 RID: 1447
	private List<bool> m_centerBullets = new List<bool>();

	// Token: 0x040005A8 RID: 1448
	private float m_verticalGap;

	// Token: 0x040005A9 RID: 1449
	private float m_lastCenterHeight;

	// Token: 0x040005AA RID: 1450
	private float m_rowHeight;

	// Token: 0x02000184 RID: 388
	public class WiggleBullet : BulletLite
	{
		// Token: 0x060005D2 RID: 1490 RVA: 0x0001C3A4 File Offset: 0x0001A5A4
		public WiggleBullet(bool suppressOffset)
			: base("default_novfx", false, false)
		{
			this.m_suppressOffset = suppressOffset;
		}

		// Token: 0x060005D3 RID: 1491 RVA: 0x0001C3BC File Offset: 0x0001A5BC
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

		// Token: 0x060005D4 RID: 1492 RVA: 0x0001C458 File Offset: 0x0001A658
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

		// Token: 0x040005AB RID: 1451
		private bool m_suppressOffset;

		// Token: 0x040005AC RID: 1452
		private Vector2 m_truePosition;

		// Token: 0x040005AD RID: 1453
		private Vector2 m_offset;

		// Token: 0x040005AE RID: 1454
		private float m_xMagnitude;

		// Token: 0x040005AF RID: 1455
		private float m_xPeriod;

		// Token: 0x040005B0 RID: 1456
		private float m_yMagnitude;

		// Token: 0x040005B1 RID: 1457
		private float m_yPeriod;

		// Token: 0x040005B2 RID: 1458
		private Vector2 m_delta;
	}
}
