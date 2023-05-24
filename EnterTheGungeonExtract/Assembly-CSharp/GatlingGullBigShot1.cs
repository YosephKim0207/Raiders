using System;
using System.Collections;
using Brave.BulletScript;
using FullInspector;

// Token: 0x020001A6 RID: 422
[InspectorDropdownName("Bosses/GatlingGull/BigShot1")]
public class GatlingGullBigShot1 : Script
{
	// Token: 0x0600064A RID: 1610 RVA: 0x0001E594 File Offset: 0x0001C794
	protected override IEnumerator Top()
	{
		base.Fire(new Direction(0f, DirectionType.Aim, -1f), new Speed(10f, SpeedType.Absolute), new GatlingGullBigShot1.BigBullet());
		return null;
	}

	// Token: 0x04000616 RID: 1558
	private const int NumDeathBullets = 32;

	// Token: 0x020001A7 RID: 423
	private class BigBullet : Bullet
	{
		// Token: 0x0600064B RID: 1611 RVA: 0x0001E5C0 File Offset: 0x0001C7C0
		public BigBullet()
			: base("bigBullet", false, false, false)
		{
		}

		// Token: 0x0600064C RID: 1612 RVA: 0x0001E5D0 File Offset: 0x0001C7D0
		public override void OnBulletDestruction(Bullet.DestroyType destroyType, SpeculativeRigidbody hitRigidbody, bool preventSpawningProjectiles)
		{
			if (preventSpawningProjectiles)
			{
				return;
			}
			float num = base.RandomAngle();
			float num2 = 11.25f;
			for (int i = 0; i < 32; i++)
			{
				base.Fire(new Direction(num + num2 * (float)i, DirectionType.Absolute, -1f), new Speed(10f, SpeedType.Absolute), null);
			}
		}
	}
}
