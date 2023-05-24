using System;
using System.Collections;
using Brave.BulletScript;
using FullInspector;

// Token: 0x02000222 RID: 546
[InspectorDropdownName("Bosses/Lich/Burst1")]
public class LichBurst1 : Script
{
	// Token: 0x06000837 RID: 2103 RVA: 0x00027F28 File Offset: 0x00026128
	protected override IEnumerator Top()
	{
		float num = base.RandomAngle();
		for (int i = 0; i < 24; i++)
		{
			base.Fire(new Direction(base.SubdivideCircle(num, 24, i, 1f, false), DirectionType.Absolute, -1f), new Speed(9f, SpeedType.Absolute), new LichBurst1.BurstBullet());
		}
		for (int j = 0; j < 24; j++)
		{
			base.Fire(new Direction(base.SubdivideCircle(num, 24, j, 1f, true), DirectionType.Absolute, -1f), new Speed(6f, SpeedType.Absolute), null);
		}
		return null;
	}

	// Token: 0x0400083E RID: 2110
	private const int NumBounceBullets = 24;

	// Token: 0x0400083F RID: 2111
	private const int NumNormalBullets = 24;

	// Token: 0x02000223 RID: 547
	public class BurstBullet : Bullet
	{
		// Token: 0x06000838 RID: 2104 RVA: 0x00027FC4 File Offset: 0x000261C4
		public BurstBullet()
			: base("burst", false, false, false)
		{
		}

		// Token: 0x06000839 RID: 2105 RVA: 0x00027FD4 File Offset: 0x000261D4
		protected override IEnumerator Top()
		{
			this.Projectile.GetComponent<BounceProjModifier>().OnBounce += this.OnBounce;
			base.ChangeSpeed(new Speed(16f, SpeedType.Absolute), 180);
			return null;
		}

		// Token: 0x0600083A RID: 2106 RVA: 0x0002800C File Offset: 0x0002620C
		public override void OnBulletDestruction(Bullet.DestroyType destroyType, SpeculativeRigidbody hitRigidbody, bool preventSpawningProjectiles)
		{
			if (this.Projectile)
			{
				this.Projectile.GetComponent<BounceProjModifier>().OnBounce -= this.OnBounce;
			}
		}

		// Token: 0x0600083B RID: 2107 RVA: 0x0002803C File Offset: 0x0002623C
		private void OnBounce()
		{
			this.Direction = this.Projectile.Direction.ToAngle();
		}
	}
}
