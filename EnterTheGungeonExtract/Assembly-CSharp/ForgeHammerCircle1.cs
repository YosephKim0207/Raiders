using System;
using System.Collections;
using Brave.BulletScript;

// Token: 0x02000198 RID: 408
public class ForgeHammerCircle1 : Script
{
	// Token: 0x0600061C RID: 1564 RVA: 0x0001D8DC File Offset: 0x0001BADC
	protected override IEnumerator Top()
	{
		int count = 0;
		float degDelta = 360f / (float)this.CircleBullets;
		for (int i = 0; i < this.CircleBullets; i++)
		{
			Offset offset = new Offset(0f, 1f, (float)i * degDelta, string.Empty, DirectionType.Absolute);
			Direction direction = new Direction(90f + (float)i * degDelta, DirectionType.Absolute, -1f);
			int num;
			count = (num = count) + 1;
			base.Fire(offset, direction, new ForgeHammerCircle1.DefaultBullet(num));
		}
		yield return null;
		yield break;
	}

	// Token: 0x040005F1 RID: 1521
	public int CircleBullets = 12;

	// Token: 0x02000199 RID: 409
	public class DefaultBullet : Bullet
	{
		// Token: 0x0600061D RID: 1565 RVA: 0x0001D8F8 File Offset: 0x0001BAF8
		public DefaultBullet(int spawnTime)
			: base(null, false, false, false)
		{
			this.spawnTime = spawnTime;
		}

		// Token: 0x0600061E RID: 1566 RVA: 0x0001D90C File Offset: 0x0001BB0C
		protected override IEnumerator Top()
		{
			base.ChangeSpeed(new Speed(8f, SpeedType.Absolute), 1);
			yield return null;
			yield break;
		}

		// Token: 0x040005F2 RID: 1522
		public int spawnTime;
	}
}
