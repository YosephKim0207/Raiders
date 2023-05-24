using System;
using System.Collections;
using Brave.BulletScript;

// Token: 0x020001EA RID: 490
public abstract class HighPriestSweepAttacks1 : Script
{
	// Token: 0x06000753 RID: 1875 RVA: 0x00023590 File Offset: 0x00021790
	public HighPriestSweepAttacks1(bool shootLeft, bool shootRight)
	{
		this.m_shootLeft = shootLeft;
		this.m_shootRight = shootRight;
	}

	// Token: 0x06000754 RID: 1876 RVA: 0x000235A8 File Offset: 0x000217A8
	protected override IEnumerator Top()
	{
		float angleDelta = 9f;
		for (int i = 0; i < 15; i++)
		{
			if (this.m_shootLeft)
			{
				base.Fire(new Offset(0f, -2.5f, -30f - (float)i * angleDelta, string.Empty, DirectionType.Absolute), new Direction((float)((8 - i) * 5), DirectionType.Aim, -1f), new Speed(0f, SpeedType.Absolute), new HighPriestSweepAttacks1.SweepBullet(i));
			}
			if (this.m_shootRight)
			{
				base.Fire(new Offset(0f, -2.5f, 30f + (float)i * angleDelta, string.Empty, DirectionType.Absolute), new Direction((float)((8 - i) * -5), DirectionType.Aim, -1f), new Speed(0f, SpeedType.Absolute), new HighPriestSweepAttacks1.SweepBullet(i));
			}
			yield return base.Wait(1);
		}
		yield break;
	}

	// Token: 0x0400072D RID: 1837
	private const int NumBullets = 15;

	// Token: 0x0400072E RID: 1838
	private bool m_shootLeft;

	// Token: 0x0400072F RID: 1839
	private bool m_shootRight;

	// Token: 0x020001EB RID: 491
	public class SweepBullet : Bullet
	{
		// Token: 0x06000755 RID: 1877 RVA: 0x000235C4 File Offset: 0x000217C4
		public SweepBullet(int delay)
			: base("sweep", false, false, false)
		{
			this.m_delay = delay;
		}

		// Token: 0x06000756 RID: 1878 RVA: 0x000235DC File Offset: 0x000217DC
		protected override IEnumerator Top()
		{
			yield return base.Wait(30 - this.m_delay);
			this.Speed = 12f;
			yield return base.Wait(270);
			base.Vanish(false);
			yield break;
		}

		// Token: 0x04000730 RID: 1840
		private int m_delay;
	}
}
