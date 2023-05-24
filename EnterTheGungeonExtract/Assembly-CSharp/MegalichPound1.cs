using System;
using System.Collections;
using Brave.BulletScript;
using UnityEngine;

// Token: 0x02000251 RID: 593
public abstract class MegalichPound1 : Script
{
	// Token: 0x1700020D RID: 525
	// (get) Token: 0x060008F5 RID: 2293
	protected abstract float FireDirection { get; }

	// Token: 0x060008F6 RID: 2294 RVA: 0x0002BBF8 File Offset: 0x00029DF8
	protected override IEnumerator Top()
	{
		for (int j = 0; j < 12; j++)
		{
			base.Fire(new Direction(base.SubdivideArc(90f + this.FireDirection * 80f, this.FireDirection * 45f, 12, j, false), DirectionType.Absolute, -1f), new Speed(20f, SpeedType.Absolute), new Bullet("poundLarge", false, false, false));
		}
		for (int k = 0; k < 30; k++)
		{
			base.Fire(new Direction(UnityEngine.Random.Range(90f + this.FireDirection * 115f, 90f + this.FireDirection * 270f), DirectionType.Absolute, -1f), new Speed((float)UnityEngine.Random.Range(7, 14), SpeedType.Absolute), new Bullet("poundSmall", false, false, false));
		}
		yield return base.Wait(60);
		for (int i = 0; i < 60; i++)
		{
			MegalichPound1.DyingBullet bullet;
			if (UnityEngine.Random.value < 0.33f)
			{
				bullet = new MegalichPound1.DyingBullet("poundLarge", false);
			}
			else
			{
				bullet = new MegalichPound1.DyingBullet("poundSmall", true);
			}
			base.Fire(new Offset(this.FireDirection * -19.5f, UnityEngine.Random.Range(0f, -12f), 0f, string.Empty, DirectionType.Absolute), new Direction((float)((this.FireDirection <= 0f) ? 180 : 0), DirectionType.Absolute, -1f), new Speed(14f, SpeedType.Absolute), bullet);
			yield return base.Wait(1);
		}
		yield return base.Wait(60);
		yield break;
	}

	// Token: 0x04000911 RID: 2321
	private const int NumBurstBullets = 12;

	// Token: 0x04000912 RID: 2322
	private const int NumOtherBullets = 30;

	// Token: 0x04000913 RID: 2323
	private const int NumWallBullets = 60;

	// Token: 0x02000252 RID: 594
	public class DyingBullet : Bullet
	{
		// Token: 0x060008F7 RID: 2295 RVA: 0x0002BC14 File Offset: 0x00029E14
		public DyingBullet(string name, bool disappear)
			: base(name, false, false, false)
		{
			this.m_disappear = disappear;
		}

		// Token: 0x060008F8 RID: 2296 RVA: 0x0002BC28 File Offset: 0x00029E28
		protected override IEnumerator Top()
		{
			if (!this.m_disappear)
			{
				yield break;
			}
			Vector2 startPosition = base.Position;
			float deathDistance = UnityEngine.Random.Range(7f, 19.5f);
			for (;;)
			{
				if (Mathf.Abs(base.Position.x - startPosition.x) > deathDistance)
				{
					base.Vanish(false);
				}
				yield return base.Wait(1);
			}
			yield break;
		}

		// Token: 0x04000914 RID: 2324
		private bool m_disappear;
	}
}
