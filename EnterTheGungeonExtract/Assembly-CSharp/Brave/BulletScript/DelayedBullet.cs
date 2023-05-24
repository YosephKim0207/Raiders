using System;
using System.Collections;

namespace Brave.BulletScript
{
	// Token: 0x0200034E RID: 846
	public class DelayedBullet : Bullet
	{
		// Token: 0x06000D4F RID: 3407 RVA: 0x0003F060 File Offset: 0x0003D260
		public DelayedBullet(int delayFrames)
			: base(null, false, false, false)
		{
			this.m_delayFrames = delayFrames;
		}

		// Token: 0x06000D50 RID: 3408 RVA: 0x0003F074 File Offset: 0x0003D274
		public DelayedBullet(string name, int delayFrames)
			: base(name, false, false, false)
		{
			this.m_delayFrames = delayFrames;
		}

		// Token: 0x06000D51 RID: 3409 RVA: 0x0003F088 File Offset: 0x0003D288
		protected override IEnumerator Top()
		{
			if (this.m_delayFrames == 0)
			{
				yield break;
			}
			float speed = this.Speed;
			this.Speed = 0f;
			yield return base.Wait(this.m_delayFrames);
			this.Speed = speed;
			yield break;
		}

		// Token: 0x04000DD3 RID: 3539
		private int m_delayFrames;
	}
}
