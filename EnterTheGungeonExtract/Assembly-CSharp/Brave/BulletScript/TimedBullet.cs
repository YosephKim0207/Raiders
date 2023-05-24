using System;
using System.Collections;

namespace Brave.BulletScript
{
	// Token: 0x02000352 RID: 850
	public class TimedBullet : Bullet
	{
		// Token: 0x06000D61 RID: 3425 RVA: 0x0003F2E0 File Offset: 0x0003D4E0
		public TimedBullet(int destroyTimer)
			: base(null, false, false, false)
		{
			this.m_destroyTimer = destroyTimer;
		}

		// Token: 0x06000D62 RID: 3426 RVA: 0x0003F2F4 File Offset: 0x0003D4F4
		public TimedBullet(string name, int destroyTimer)
			: base(name, false, false, false)
		{
			this.m_destroyTimer = destroyTimer;
		}

		// Token: 0x06000D63 RID: 3427 RVA: 0x0003F308 File Offset: 0x0003D508
		protected override IEnumerator Top()
		{
			yield return base.Wait(this.m_destroyTimer);
			base.Vanish(false);
			yield break;
		}

		// Token: 0x04000DE0 RID: 3552
		private int m_destroyTimer;
	}
}
