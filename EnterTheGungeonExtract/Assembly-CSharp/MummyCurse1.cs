using System;
using System.Collections;
using Brave.BulletScript;

// Token: 0x020002A7 RID: 679
public class MummyCurse1 : Script
{
	// Token: 0x06000A6A RID: 2666 RVA: 0x0003252C File Offset: 0x0003072C
	protected override IEnumerator Top()
	{
		base.Fire(new Direction(0f, DirectionType.Aim, -1f), new Speed(8f, SpeedType.Absolute), new MummyCurse1.SkullBullet(this));
		yield return base.Wait(180);
		yield break;
	}

	// Token: 0x04000AD6 RID: 2774
	private const int AirTime = 180;

	// Token: 0x020002A8 RID: 680
	public class SkullBullet : Bullet
	{
		// Token: 0x06000A6B RID: 2667 RVA: 0x00032548 File Offset: 0x00030748
		public SkullBullet(Script parentScript)
			: base("skull", false, false, false)
		{
			this.m_parentScript = parentScript;
		}

		// Token: 0x06000A6C RID: 2668 RVA: 0x00032560 File Offset: 0x00030760
		protected override IEnumerator Top()
		{
			HealthHaver owner = base.BulletBank.healthHaver;
			for (int i = 0; i < 180; i++)
			{
				base.ChangeDirection(new Direction(0f, DirectionType.Aim, 3f), 1);
				if (!owner || owner.IsDead)
				{
					base.Vanish(false);
					yield break;
				}
				yield return base.Wait(1);
			}
			base.Vanish(false);
			yield break;
		}

		// Token: 0x06000A6D RID: 2669 RVA: 0x0003257C File Offset: 0x0003077C
		public override void OnBulletDestruction(Bullet.DestroyType destroyType, SpeculativeRigidbody hitRigidbody, bool preventSpawningProjectiles)
		{
			if (this.m_parentScript != null)
			{
				this.m_parentScript.ForceEnd();
			}
		}

		// Token: 0x04000AD7 RID: 2775
		private Script m_parentScript;
	}
}
