using System;
using System.Collections;
using Brave.BulletScript;
using FullInspector;

// Token: 0x020001E5 RID: 485
[InspectorDropdownName("Bosses/HighPriest/MergoWall1")]
public class HighPriestMergoWall1 : Script
{
	// Token: 0x06000742 RID: 1858 RVA: 0x00023144 File Offset: 0x00021344
	protected override IEnumerator Top()
	{
		for (int i = 0; i < 20; i++)
		{
			base.Fire(new Direction(0f, DirectionType.Relative, -1f), new Speed(4f, SpeedType.Absolute), new HighPriestMergoWall1.BigBullet());
		}
		return null;
	}

	// Token: 0x0400071F RID: 1823
	private const int NumBullets = 20;

	// Token: 0x020001E6 RID: 486
	public class BigBullet : Bullet
	{
		// Token: 0x06000743 RID: 1859 RVA: 0x0002318C File Offset: 0x0002138C
		public BigBullet()
			: base("mergoWall", false, false, false)
		{
		}

		// Token: 0x06000744 RID: 1860 RVA: 0x0002319C File Offset: 0x0002139C
		protected override IEnumerator Top()
		{
			this.Projectile.IgnoreTileCollisionsFor(1.5f);
			base.ChangeDirection(new Direction(30f, DirectionType.Relative, -1f), 30);
			yield return base.Wait(30);
			base.ChangeDirection(new Direction(-60f, DirectionType.Relative, -1f), 60);
			yield return base.Wait(30);
			base.ChangeSpeed(new Speed(8f, SpeedType.Absolute), 4);
			yield return base.Wait(60);
			for (int i = 0; i < 10; i++)
			{
				base.ChangeDirection(new Direction(60f, DirectionType.Relative, -1f), 60);
				yield return base.Wait(60);
				base.ChangeDirection(new Direction(-60f, DirectionType.Relative, -1f), 60);
				yield return base.Wait(60);
			}
			base.ChangeDirection(new Direction(30f, DirectionType.Relative, -1f), 30);
			yield return base.Wait(30);
			base.Vanish(false);
			yield break;
		}
	}
}
