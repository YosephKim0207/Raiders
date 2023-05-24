using System;
using System.Collections;
using Brave.BulletScript;
using FullInspector;
using UnityEngine;

// Token: 0x02000310 RID: 784
[InspectorDropdownName("Bosses/TankTreader/ScatterShot1")]
public class TankTreaderScatterShot1 : Script
{
	// Token: 0x06000C24 RID: 3108 RVA: 0x0003AD94 File Offset: 0x00038F94
	protected override IEnumerator Top()
	{
		base.Fire(new Direction(0f, DirectionType.Aim, -1f), new Speed(12f, SpeedType.Absolute), new TankTreaderScatterShot1.ScatterBullet());
		return null;
	}

	// Token: 0x04000CE4 RID: 3300
	private const int AirTime = 30;

	// Token: 0x04000CE5 RID: 3301
	private const int NumDeathBullets = 16;

	// Token: 0x02000311 RID: 785
	private class ScatterBullet : Bullet
	{
		// Token: 0x06000C25 RID: 3109 RVA: 0x0003ADC0 File Offset: 0x00038FC0
		public ScatterBullet()
			: base("scatterBullet", false, false, false)
		{
		}

		// Token: 0x06000C26 RID: 3110 RVA: 0x0003ADD0 File Offset: 0x00038FD0
		protected override IEnumerator Top()
		{
			yield return base.Wait(30);
			for (int i = 0; i < 16; i++)
			{
				base.Fire(new Direction((float)UnityEngine.Random.Range(-35, 35), DirectionType.Relative, -1f), new Speed((float)UnityEngine.Random.Range(3, 12), SpeedType.Absolute), new TankTreaderScatterShot1.LittleScatterBullet());
			}
			base.Vanish(false);
			yield break;
		}
	}

	// Token: 0x02000313 RID: 787
	private class LittleScatterBullet : Bullet
	{
		// Token: 0x06000C2D RID: 3117 RVA: 0x0003AED4 File Offset: 0x000390D4
		public LittleScatterBullet()
			: base(null, false, false, false)
		{
		}

		// Token: 0x06000C2E RID: 3118 RVA: 0x0003AEE0 File Offset: 0x000390E0
		protected override IEnumerator Top()
		{
			base.ChangeSpeed(new Speed(12f, SpeedType.Absolute), 40);
			yield return base.Wait(300);
			base.Vanish(false);
			yield break;
		}
	}
}
