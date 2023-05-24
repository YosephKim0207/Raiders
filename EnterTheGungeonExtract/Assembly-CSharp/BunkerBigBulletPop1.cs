using System;
using System.Collections;
using Brave.BulletScript;
using FullInspector;

// Token: 0x02000125 RID: 293
[InspectorDropdownName("Bosses/Bunker/BigBulletPop1")]
public class BunkerBigBulletPop1 : Script
{
	// Token: 0x06000455 RID: 1109 RVA: 0x00014DF4 File Offset: 0x00012FF4
	protected override IEnumerator Top()
	{
		base.Fire(new Offset("left shooter"), new Direction(0f, DirectionType.Relative, -1f), new Speed(9f, SpeedType.Absolute), new BunkerBigBulletPop1.BigBullet());
		return null;
	}

	// Token: 0x02000126 RID: 294
	public class BigBullet : Bullet
	{
		// Token: 0x06000456 RID: 1110 RVA: 0x00014E28 File Offset: 0x00013028
		public BigBullet()
			: base("default_black", false, false, false)
		{
		}

		// Token: 0x06000457 RID: 1111 RVA: 0x00014E38 File Offset: 0x00013038
		protected override IEnumerator Top()
		{
			yield return base.Wait(40);
			base.Vanish(false);
			yield break;
		}

		// Token: 0x06000458 RID: 1112 RVA: 0x00014E54 File Offset: 0x00013054
		public override void OnBulletDestruction(Bullet.DestroyType destroyType, SpeculativeRigidbody hitRigidbody, bool preventSpawningProjectiles)
		{
			if (preventSpawningProjectiles)
			{
				return;
			}
			float num = base.RandomAngle();
			for (int i = 0; i < 8; i++)
			{
				base.Fire(new Direction(num + (float)(i * 45), DirectionType.Absolute, -1f), new Speed(9f, SpeedType.Absolute), new Bullet("default3", false, false, false));
			}
		}
	}
}
