using System;
using System.Collections;
using Brave.BulletScript;
using UnityEngine;

// Token: 0x020002F5 RID: 757
public class ShotgunCreecherUglyCircle1 : Script
{
	// Token: 0x06000BBD RID: 3005 RVA: 0x000396F4 File Offset: 0x000378F4
	protected override IEnumerator Top()
	{
		for (int i = 1; i <= 7; i++)
		{
			string text = string.Format("shoot point {0}", i);
			for (int j = 0; j < 2; j++)
			{
				base.Fire(new Offset(text), new Direction(base.RandomAngle(), DirectionType.Absolute, -1f), new Speed((float)UnityEngine.Random.Range(8, 12), SpeedType.Absolute), new ShotgunCreecherUglyCircle1.CreecherBullet());
			}
		}
		return null;
	}

	// Token: 0x04000C90 RID: 3216
	private const int NumBulletNodes = 7;

	// Token: 0x04000C91 RID: 3217
	private const int NumBulletsPerNode = 2;

	// Token: 0x020002F6 RID: 758
	public class CreecherBullet : Bullet
	{
		// Token: 0x06000BBE RID: 3006 RVA: 0x00039768 File Offset: 0x00037968
		public CreecherBullet()
			: base(null, false, false, false)
		{
		}

		// Token: 0x06000BBF RID: 3007 RVA: 0x00039774 File Offset: 0x00037974
		protected override IEnumerator Top()
		{
			base.ChangeSpeed(new Speed(12f, SpeedType.Absolute), 60);
			for (int i = 0; i < 60; i++)
			{
				if (this.Projectile)
				{
					this.Projectile.Speed = this.Speed;
					this.Projectile.UpdateSpeed();
				}
			}
			return null;
		}
	}
}
