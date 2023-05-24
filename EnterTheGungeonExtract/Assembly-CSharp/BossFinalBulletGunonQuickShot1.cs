using System;
using System.Collections;
using Brave.BulletScript;
using FullInspector;

// Token: 0x02000067 RID: 103
[InspectorDropdownName("Bosses/BossFinalBullet/GunonQuickShot1")]
public class BossFinalBulletGunonQuickShot1 : Script
{
	// Token: 0x06000190 RID: 400 RVA: 0x000084A8 File Offset: 0x000066A8
	protected override IEnumerator Top()
	{
		base.Fire(new Offset("left double shoot point"), new BossFinalBulletGunonQuickShot1.BatBullet());
		yield return base.Wait(6);
		base.Fire(new Offset("right double shoot point"), new BossFinalBulletGunonQuickShot1.BatBullet());
		yield break;
	}

	// Token: 0x02000068 RID: 104
	public class BatBullet : Bullet
	{
		// Token: 0x06000191 RID: 401 RVA: 0x000084C4 File Offset: 0x000066C4
		public BatBullet()
			: base("hipbat", false, false, false)
		{
		}

		// Token: 0x06000192 RID: 402 RVA: 0x000084D4 File Offset: 0x000066D4
		protected override IEnumerator Top()
		{
			this.Direction = base.GetAimDirection(base.Position, (float)((!BraveUtility.RandomBool()) ? 1 : 0), 12f);
			base.ChangeSpeed(new Speed(12f, SpeedType.Absolute), 3);
			while (base.Tick < 180)
			{
				if (base.Tick > 7 && base.Tick % 7 == 0)
				{
					base.Fire(new BossFinalBulletGunonQuickShot1.FireBullet());
				}
				yield return base.Wait(1);
			}
			base.Vanish(false);
			yield break;
		}
	}

	// Token: 0x0200006A RID: 106
	public class FireBullet : Bullet
	{
		// Token: 0x06000199 RID: 409 RVA: 0x0000862C File Offset: 0x0000682C
		public FireBullet()
			: base("fire", false, false, false)
		{
		}

		// Token: 0x0600019A RID: 410 RVA: 0x0000863C File Offset: 0x0000683C
		protected override IEnumerator Top()
		{
			yield return base.Wait(30);
			base.Vanish(false);
			yield break;
		}
	}
}
