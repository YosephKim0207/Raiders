using System;
using System.Collections;

namespace Brave.BulletScript
{
	// Token: 0x02000350 RID: 848
	public class SpeedChangingBullet : Bullet
	{
		// Token: 0x06000D58 RID: 3416 RVA: 0x0003F188 File Offset: 0x0003D388
		public SpeedChangingBullet(float newSpeed, int term, int destroyTimer = -1)
			: base(null, false, false, false)
		{
			this.m_newSpeed = newSpeed;
			this.m_term = term;
			this.m_destroyTimer = destroyTimer;
		}

		// Token: 0x06000D59 RID: 3417 RVA: 0x0003F1AC File Offset: 0x0003D3AC
		public SpeedChangingBullet(string name, float newSpeed, int term, int destroyTimer = -1, bool suppressVfx = false)
			: base(name, suppressVfx, false, false)
		{
			this.m_newSpeed = newSpeed;
			this.m_term = term;
			this.m_destroyTimer = destroyTimer;
		}

		// Token: 0x06000D5A RID: 3418 RVA: 0x0003F1D0 File Offset: 0x0003D3D0
		protected override IEnumerator Top()
		{
			base.ChangeSpeed(new Speed(this.m_newSpeed, SpeedType.Absolute), this.m_term);
			if (this.m_destroyTimer < 0)
			{
				yield break;
			}
			yield return base.Wait(this.m_term + this.m_destroyTimer);
			base.Vanish(false);
			yield break;
		}

		// Token: 0x04000DD9 RID: 3545
		private float m_newSpeed;

		// Token: 0x04000DDA RID: 3546
		private int m_term;

		// Token: 0x04000DDB RID: 3547
		private int m_destroyTimer;
	}
}
