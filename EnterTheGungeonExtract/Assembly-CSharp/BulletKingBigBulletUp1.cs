using System;
using System.Collections;
using Brave.BulletScript;
using FullInspector;

// Token: 0x020000DA RID: 218
[InspectorDropdownName("Bosses/BulletKing/BigBulletUp1")]
public class BulletKingBigBulletUp1 : Script
{
	// Token: 0x170000C3 RID: 195
	// (get) Token: 0x0600034F RID: 847 RVA: 0x00010D74 File Offset: 0x0000EF74
	protected bool IsHard
	{
		get
		{
			return this is BulletKingBigBulletUpHard1;
		}
	}

	// Token: 0x06000350 RID: 848 RVA: 0x00010D80 File Offset: 0x0000EF80
	protected override IEnumerator Top()
	{
		base.Fire(new Offset(0.0625f, 3.5625f, 0f, string.Empty, DirectionType.Absolute), new Direction(90f, DirectionType.Absolute, -1f), new Speed(3f, SpeedType.Absolute), new BulletKingBigBulletUp1.BigBullet(this.IsHard));
		return null;
	}

	// Token: 0x0400036F RID: 879
	private const int NumMediumBullets = 8;

	// Token: 0x04000370 RID: 880
	private const int NumSmallBullets = 8;

	// Token: 0x020000DB RID: 219
	public class BigBullet : Bullet
	{
		// Token: 0x06000351 RID: 849 RVA: 0x00010DD4 File Offset: 0x0000EFD4
		public BigBullet(bool isHard)
			: base("bigBullet", false, false, false)
		{
			this.m_isHard = isHard;
		}

		// Token: 0x06000352 RID: 850 RVA: 0x00010DEC File Offset: 0x0000EFEC
		protected override IEnumerator Top()
		{
			yield return base.Wait(40);
			base.Vanish(false);
			yield break;
		}

		// Token: 0x06000353 RID: 851 RVA: 0x00010E08 File Offset: 0x0000F008
		public override void OnBulletDestruction(Bullet.DestroyType destroyType, SpeculativeRigidbody hitRigidbody, bool preventSpawningProjectiles)
		{
			if (preventSpawningProjectiles)
			{
				return;
			}
			float num = base.RandomAngle();
			for (int i = 0; i < 8; i++)
			{
				float num2;
				if (this.m_isHard)
				{
					num2 = base.SubdivideArc(base.AimDirection - 120f, 240f, 8, i, false);
				}
				else
				{
					num2 = base.SubdivideCircle(num, 8, i, 1f, false);
				}
				base.Fire(new Direction(num2, DirectionType.Absolute, -1f), new Speed(6f, SpeedType.Absolute), new BulletKingBigBulletUp1.MediumBullet());
			}
		}

		// Token: 0x04000371 RID: 881
		private bool m_isHard;
	}

	// Token: 0x020000DD RID: 221
	public class MediumBullet : Bullet
	{
		// Token: 0x0600035A RID: 858 RVA: 0x00010F34 File Offset: 0x0000F134
		public MediumBullet()
			: base("quad", false, false, false)
		{
		}

		// Token: 0x0600035B RID: 859 RVA: 0x00010F44 File Offset: 0x0000F144
		protected override IEnumerator Top()
		{
			yield return base.Wait(30);
			base.Vanish(false);
			yield break;
		}

		// Token: 0x0600035C RID: 860 RVA: 0x00010F60 File Offset: 0x0000F160
		public override void OnBulletDestruction(Bullet.DestroyType destroyType, SpeculativeRigidbody hitRigidbody, bool preventSpawningProjectiles)
		{
			if (preventSpawningProjectiles)
			{
				return;
			}
			float num = base.RandomAngle();
			float num2 = 45f;
			for (int i = 0; i < 8; i++)
			{
				base.Fire(new Direction(num + (float)i * num2, DirectionType.Absolute, -1f), new Speed(10f, SpeedType.Absolute), new Bullet("default_novfx", false, false, false));
			}
		}
	}
}
