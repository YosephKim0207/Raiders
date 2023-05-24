using System;
using System.Collections;
using Brave.BulletScript;
using FullInspector;
using UnityEngine;

// Token: 0x020000E0 RID: 224
[InspectorDropdownName("Bosses/BulletKing/QuadShot1")]
public class BulletKingQuadShot1 : Script
{
	// Token: 0x170000C8 RID: 200
	// (get) Token: 0x06000365 RID: 869 RVA: 0x00011074 File Offset: 0x0000F274
	public bool IsHard
	{
		get
		{
			return this is BulletKingQuadShotHard1;
		}
	}

	// Token: 0x06000366 RID: 870 RVA: 0x00011080 File Offset: 0x0000F280
	protected override IEnumerator Top()
	{
		yield return base.Wait(10);
		this.QuadShot(-1.25f, -0.75f, 0f);
		this.QuadShot(-1.3125f, -0.4375f, -15f);
		this.QuadShot(-1.5f, -0.1875f, -30f);
		this.QuadShot(-1.75f, 0.25f, -45f);
		this.QuadShot(-2.125f, 1.3125f, -67.5f);
		this.QuadShot(-2.125f, 1.3125f, -90f);
		this.QuadShot(-2.125f, 1.3125f, -112.5f);
		this.QuadShot(-2.0625f, 2.375f, -135f);
		this.QuadShot(-0.8125f, 3.1875f, -157.5f);
		this.QuadShot(0.0625f, 3.5625f, 180f);
		this.QuadShot(0.9375f, 3.1875f, 157.5f);
		this.QuadShot(2.125f, 2.375f, 135f);
		this.QuadShot(2.1875f, 1.3125f, 112.5f);
		this.QuadShot(2.1875f, 1.3125f, 90f);
		this.QuadShot(2.1875f, 1.3125f, 67.5f);
		this.QuadShot(1.875f, 0.25f, 45f);
		this.QuadShot(1.625f, -0.1875f, 30f);
		this.QuadShot(1.4275f, -0.4375f, 15f);
		this.QuadShot(1.375f, -0.75f, 0f);
		yield break;
	}

	// Token: 0x06000367 RID: 871 RVA: 0x0001109C File Offset: 0x0000F29C
	private void QuadShot(float x, float y, float direction)
	{
		for (int i = 0; i < 4; i++)
		{
			base.Fire(new Offset(x, y, 0f, string.Empty, DirectionType.Absolute), new Direction(direction - 90f, DirectionType.Absolute, -1f), new Speed(9f - (float)i * 1.5f, SpeedType.Absolute), new BulletKingQuadShot1.QuadBullet(this.IsHard, i));
		}
	}

	// Token: 0x020000E1 RID: 225
	public class QuadBullet : Bullet
	{
		// Token: 0x06000368 RID: 872 RVA: 0x00011108 File Offset: 0x0000F308
		public QuadBullet(bool isHard, int index)
			: base("quad", false, false, false)
		{
			this.m_isHard = isHard;
			this.m_index = index;
		}

		// Token: 0x06000369 RID: 873 RVA: 0x00011128 File Offset: 0x0000F328
		protected override IEnumerator Top()
		{
			base.ChangeSpeed(new Speed(10f, SpeedType.Absolute), 120);
			if (this.m_isHard)
			{
				yield return base.Wait(30);
				Vector2 velocity = BraveMathCollege.DegreesToVector(this.Direction, this.Speed);
				float sign = (float)((this.m_index % 2 != 0) ? 1 : (-1));
				velocity += new Vector2(0f, sign * 1.75f).Rotate(this.Direction);
				this.Direction = velocity.ToAngle();
				this.Speed = velocity.magnitude;
				yield return base.Wait(90);
				base.Vanish(false);
			}
			else
			{
				yield return base.Wait(120);
				base.Vanish(false);
			}
			yield break;
		}

		// Token: 0x0400037A RID: 890
		private bool m_isHard;

		// Token: 0x0400037B RID: 891
		private int m_index;
	}
}
