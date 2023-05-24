using System;

namespace Brave.BulletScript
{
	// Token: 0x0200034B RID: 843
	public class BulletLite : Bullet
	{
		// Token: 0x06000D48 RID: 3400 RVA: 0x0003F018 File Offset: 0x0003D218
		public BulletLite(string bankName = null, bool suppressVfx = false, bool firstBulletOfAttack = false)
			: base(bankName, suppressVfx, firstBulletOfAttack, false)
		{
		}

		// Token: 0x06000D49 RID: 3401 RVA: 0x0003F024 File Offset: 0x0003D224
		public override void Initialize()
		{
			this.m_tasks.Add(new Bullet.TaskLite(this));
		}

		// Token: 0x06000D4A RID: 3402 RVA: 0x0003F038 File Offset: 0x0003D238
		public virtual void Start()
		{
		}

		// Token: 0x06000D4B RID: 3403 RVA: 0x0003F03C File Offset: 0x0003D23C
		public virtual int Update(ref int state)
		{
			return this.Done();
		}

		// Token: 0x06000D4C RID: 3404 RVA: 0x0003F044 File Offset: 0x0003D244
		protected int Done()
		{
			return -1;
		}
	}
}
