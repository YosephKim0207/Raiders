using System;
using System.Collections;
using Brave.BulletScript;
using UnityEngine;

// Token: 0x02000315 RID: 789
public class TombstoneCrossAttack1 : Script
{
	// Token: 0x06000C36 RID: 3126 RVA: 0x0003AFC0 File Offset: 0x000391C0
	protected override IEnumerator Top()
	{
		float aimDirection = base.GetAimDirection((float)((UnityEngine.Random.value >= 0.25f) ? 0 : 1), 10f);
		base.Fire(new Direction(aimDirection, DirectionType.Absolute, -1f), new Speed(10f, SpeedType.Absolute), new TombstoneCrossAttack1.CrossBullet(new Vector2(0.7f, 0f), 0, 20));
		base.Fire(new Direction(aimDirection, DirectionType.Absolute, -1f), new Speed(10f, SpeedType.Absolute), new TombstoneCrossAttack1.CrossBullet(new Vector2(0f, 0f), 0, 20));
		base.Fire(new Direction(aimDirection, DirectionType.Absolute, -1f), new Speed(10f, SpeedType.Absolute), new TombstoneCrossAttack1.CrossBullet(new Vector2(-0.7f, 0f), 0, 20));
		base.Fire(new Direction(aimDirection, DirectionType.Absolute, -1f), new Speed(10f, SpeedType.Absolute), new TombstoneCrossAttack1.CrossBullet(new Vector2(-1.4f, 0f), 0, 20));
		base.Fire(new Direction(aimDirection, DirectionType.Absolute, -1f), new Speed(10f, SpeedType.Absolute), new TombstoneCrossAttack1.CrossBullet(new Vector2(0f, 0.7f), 18, 15));
		base.Fire(new Direction(aimDirection, DirectionType.Absolute, -1f), new Speed(10f, SpeedType.Absolute), new TombstoneCrossAttack1.CrossBullet(new Vector2(0f, -0.7f), 18, 15));
		return null;
	}

	// Token: 0x04000CEE RID: 3310
	private const int BulletSpeed = 10;

	// Token: 0x04000CEF RID: 3311
	private const float GapDist = 0.7f;

	// Token: 0x02000316 RID: 790
	public class CrossBullet : Bullet
	{
		// Token: 0x06000C37 RID: 3127 RVA: 0x0003B12C File Offset: 0x0003932C
		public CrossBullet(Vector2 offset, int setupDelay, int setupTime)
			: base(null, false, false, false)
		{
			this.m_offset = offset;
			this.m_setupDelay = setupDelay;
			this.m_setupTime = setupTime;
		}

		// Token: 0x06000C38 RID: 3128 RVA: 0x0003B150 File Offset: 0x00039350
		protected override IEnumerator Top()
		{
			base.ManualControl = true;
			this.m_offset = this.m_offset.Rotate(this.Direction);
			for (int i = 0; i < 360; i++)
			{
				if (i > this.m_setupDelay && i < this.m_setupDelay + this.m_setupTime)
				{
					base.Position += this.m_offset / (float)this.m_setupTime;
				}
				base.Position += BraveMathCollege.DegreesToVector(this.Direction, this.Speed / 60f);
				yield return base.Wait(1);
			}
			base.Vanish(false);
			yield break;
		}

		// Token: 0x04000CF0 RID: 3312
		private Vector2 m_offset;

		// Token: 0x04000CF1 RID: 3313
		private int m_setupDelay;

		// Token: 0x04000CF2 RID: 3314
		private int m_setupTime;
	}
}
