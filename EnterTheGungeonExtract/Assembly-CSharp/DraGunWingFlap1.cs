using System;
using System.Collections;
using Brave.BulletScript;
using FullInspector;
using UnityEngine;

// Token: 0x02000193 RID: 403
[InspectorDropdownName("Bosses/DraGun/WingFlap1")]
public class DraGunWingFlap1 : Script
{
	// Token: 0x06000609 RID: 1545 RVA: 0x0001D534 File Offset: 0x0001B734
	protected override IEnumerator Top()
	{
		base.EndOnBlank = true;
		for (int i = 0; i < 30; i++)
		{
			base.Fire(new Offset(-17f + UnityEngine.Random.Range(0f, 5f), 18f, 0f, string.Empty, DirectionType.Absolute), new Direction(-90f, DirectionType.Absolute, -1f), new Speed(12f, SpeedType.Absolute), new DraGunWingFlap1.WindProjectile(1f));
			base.Fire(new Offset(17f - UnityEngine.Random.Range(0f, 5f), 18f, 0f, string.Empty, DirectionType.Absolute), new Direction(-90f, DirectionType.Absolute, -1f), new Speed(12f, SpeedType.Absolute), new DraGunWingFlap1.WindProjectile(-1f));
			yield return base.Wait(8);
		}
		yield return base.Wait(60);
		yield break;
	}

	// Token: 0x040005E5 RID: 1509
	private const int NumBullets = 30;

	// Token: 0x02000194 RID: 404
	public class WindProjectile : Bullet
	{
		// Token: 0x0600060A RID: 1546 RVA: 0x0001D550 File Offset: 0x0001B750
		public WindProjectile(float sign)
			: base(null, false, false, false)
		{
			this.m_sign = sign;
		}

		// Token: 0x0600060B RID: 1547 RVA: 0x0001D564 File Offset: 0x0001B764
		protected override IEnumerator Top()
		{
			yield return base.Wait(UnityEngine.Random.Range(60, 126));
			base.ChangeDirection(new Direction(-90f + this.m_sign * 90f, DirectionType.Absolute, -1f), 30);
			yield break;
		}

		// Token: 0x040005E6 RID: 1510
		private float m_sign;
	}
}
