using System;
using System.Collections;
using Brave.BulletScript;
using FullInspector;
using UnityEngine;

// Token: 0x02000307 RID: 775
[InspectorDropdownName("Bosses/TankTreader/AreaDenial1")]
public class TankTreaderAreaDenial1 : Script
{
	// Token: 0x06000BFF RID: 3071 RVA: 0x0003A620 File Offset: 0x00038820
	protected override IEnumerator Top()
	{
		base.Fire(new Direction(0f, DirectionType.Aim, -1f), new Speed(TankTreaderAreaDenial1.HugeBulletStartSpeed, SpeedType.Absolute), new TankTreaderAreaDenial1.HugeBullet());
		return null;
	}

	// Token: 0x04000CC3 RID: 3267
	public static float HugeBulletStartSpeed = 6f;

	// Token: 0x04000CC4 RID: 3268
	public static int HugeBulletDecelerationTime = 180;

	// Token: 0x04000CC5 RID: 3269
	public static float HugeBulletHangTime = 300f;

	// Token: 0x04000CC6 RID: 3270
	public static float SpinningBulletSpinSpeed = 180f;

	// Token: 0x02000308 RID: 776
	public class HugeBullet : Bullet
	{
		// Token: 0x06000C01 RID: 3073 RVA: 0x0003A678 File Offset: 0x00038878
		public HugeBullet()
			: base("hugeBullet", false, false, false)
		{
		}

		// Token: 0x06000C02 RID: 3074 RVA: 0x0003A688 File Offset: 0x00038888
		protected override IEnumerator Top()
		{
			this.m_fireSemicircles = true;
			base.StartTask(this.FireSemicircles());
			base.ChangeSpeed(new Speed(0f, SpeedType.Absolute), TankTreaderAreaDenial1.HugeBulletDecelerationTime);
			yield return base.Wait(TankTreaderAreaDenial1.HugeBulletDecelerationTime);
			Vector2 truePosition = base.Position;
			base.ManualControl = true;
			int i = 0;
			while ((float)i < TankTreaderAreaDenial1.HugeBulletHangTime)
			{
				if (this.m_fireSemicircles && (float)i > TankTreaderAreaDenial1.HugeBulletHangTime - 45f)
				{
					this.m_fireSemicircles = false;
				}
				base.Position = truePosition + new Vector2(0.12f * ((float)i / TankTreaderAreaDenial1.HugeBulletHangTime), 0f) * Mathf.Sin((float)i / 5f * 3.1415927f);
				yield return base.Wait(1);
				i++;
			}
			for (int j = 0; j < 36; j++)
			{
				base.Fire(new Direction((float)(j * 10), DirectionType.Absolute, -1f), new Speed(12f, SpeedType.Absolute), null);
			}
			for (int k = 0; k < 36; k++)
			{
				base.Fire(new Direction((float)(5 + k * 10), DirectionType.Absolute, -1f), new Speed(8f, SpeedType.Absolute), new SpeedChangingBullet(12f, 30, -1));
			}
			base.Vanish(false);
			yield break;
		}

		// Token: 0x06000C03 RID: 3075 RVA: 0x0003A6A4 File Offset: 0x000388A4
		private IEnumerator FireSemicircles()
		{
			yield return base.Wait(60);
			int phase = 0;
			while (this.m_fireSemicircles)
			{
				for (int i = 0; i < 36; i++)
				{
					if (i / 4 % 3 == phase)
					{
						base.Fire(new Direction((float)(i * 10), DirectionType.Absolute, -1f), new Speed(9f, SpeedType.Absolute), null);
					}
				}
				yield return base.Wait(45);
				phase = (phase + 1) % 3;
			}
			yield break;
		}

		// Token: 0x04000CC7 RID: 3271
		private const int SemiCircleNumBullets = 4;

		// Token: 0x04000CC8 RID: 3272
		private const int SemiCirclePhases = 3;

		// Token: 0x04000CC9 RID: 3273
		private bool m_fireSemicircles;
	}
}
