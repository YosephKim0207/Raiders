using System;
using System.Collections;
using Brave.BulletScript;
using FullInspector;
using UnityEngine;

// Token: 0x0200017D RID: 381
[InspectorDropdownName("Bosses/DraGun/Mac10Burst2")]
public class DraGunMac10Burst2 : Script
{
	// Token: 0x060005B8 RID: 1464 RVA: 0x0001B41C File Offset: 0x0001961C
	protected override IEnumerator Top()
	{
		yield return base.Wait(1);
		Vector2 lastPosition = base.Position;
		base.PostWwiseEvent("Play_BOSS_Dragun_Uzi_01", null);
		for (;;)
		{
			if (Vector2.Distance(lastPosition, base.Position) > 1f)
			{
				base.Fire(new Offset((lastPosition - base.Position) * 0.33f, 0f, string.Empty, DirectionType.Absolute), new Direction(0f, DirectionType.Relative, -1f), new DraGunMac10Burst2.UziBullet());
				base.Fire(new Offset((lastPosition - base.Position) * 0.66f, 0f, string.Empty, DirectionType.Absolute), new Direction(0f, DirectionType.Relative, -1f), new DraGunMac10Burst2.UziBullet());
			}
			base.Fire(new Direction(0f, DirectionType.Relative, -1f), new DraGunMac10Burst2.UziBullet());
			lastPosition = base.Position;
			yield return base.Wait(UnityEngine.Random.Range(2, 4));
		}
		yield break;
	}

	// Token: 0x0200017E RID: 382
	public class UziBullet : Bullet
	{
		// Token: 0x060005B9 RID: 1465 RVA: 0x0001B438 File Offset: 0x00019638
		public UziBullet()
			: base("UziBullet", false, false, false)
		{
		}

		// Token: 0x060005BA RID: 1466 RVA: 0x0001B448 File Offset: 0x00019648
		protected override IEnumerator Top()
		{
			yield return base.Wait(60);
			base.Fire(new Direction(UnityEngine.Random.Range(-45f, 45f), DirectionType.Relative, -1f), new Speed(12f, SpeedType.Absolute), new Bullet("UziBurst", false, false, false));
			yield return base.Wait(60);
			base.Fire(new Direction(0f, DirectionType.Relative, -1f), new Speed(12f, SpeedType.Absolute), new Bullet("UziBurst", false, false, false));
			yield return base.Wait(60);
			this.Speed = 12f;
			this.Direction = base.RandomAngle();
			yield break;
		}
	}
}
