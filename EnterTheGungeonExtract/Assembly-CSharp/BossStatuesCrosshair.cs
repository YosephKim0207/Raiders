using System;
using System.Collections;
using Brave.BulletScript;
using FullInspector;

// Token: 0x020000B1 RID: 177
[InspectorDropdownName("Bosses/BossStatues/Crosshair")]
public class BossStatuesCrosshair : Script
{
	// Token: 0x060002B2 RID: 690 RVA: 0x0000E3E4 File Offset: 0x0000C5E4
	protected override IEnumerator Top()
	{
		base.EndOnBlank = true;
		this.FireSpinningLine(90f);
		this.FireCircleSegment(90f);
		this.FireSpinningLine(0f);
		this.FireCircleSegment(0f);
		this.FireSpinningLine(-90f);
		this.FireCircleSegment(-90f);
		this.FireSpinningLine(180f);
		this.FireCircleSegment(180f);
		yield return base.Wait(BossStatuesCrosshair.SetupTime + BossStatuesCrosshair.PulseInitialDelay - BossStatuesCrosshair.PulseDelay);
		for (int i = 0; i < BossStatuesCrosshair.PulseCount; i++)
		{
			yield return base.Wait(BossStatuesCrosshair.PulseDelay);
			this.FirePulse();
		}
		yield return base.Wait(BossStatuesCrosshair.SpinTime - (BossStatuesCrosshair.PulseDelay * (BossStatuesCrosshair.PulseCount - 1) + BossStatuesCrosshair.PulseInitialDelay));
		yield break;
	}

	// Token: 0x060002B3 RID: 691 RVA: 0x0000E400 File Offset: 0x0000C600
	private void FireSpinningLine(float dir)
	{
		float num = (float)BossStatuesCrosshair.SkipSetupBulletNum * (BossStatuesCrosshair.Radius * 2f * (60f / (float)BossStatuesCrosshair.SetupTime) / (float)BossStatuesCrosshair.BulletCount);
		float num2 = BossStatuesCrosshair.Radius * 2f * (60f / (float)BossStatuesCrosshair.SetupTime) / (float)BossStatuesCrosshair.BulletCount;
		for (int i = 0; i < BossStatuesCrosshair.BulletCount + BossStatuesCrosshair.ExtraSetupBulletNum - BossStatuesCrosshair.SkipSetupBulletNum; i++)
		{
			base.Fire(new Direction(dir, DirectionType.Absolute, -1f), new Speed(num + num2 * (float)i, SpeedType.Absolute), new BossStatuesCrosshair.LineBullet(i + BossStatuesCrosshair.SkipSetupBulletNum));
		}
	}

	// Token: 0x060002B4 RID: 692 RVA: 0x0000E4A0 File Offset: 0x0000C6A0
	private void FireCircleSegment(float dir)
	{
		for (int i = 0; i < BossStatuesCrosshair.BulletCount; i++)
		{
			base.Fire(new Direction(dir, DirectionType.Absolute, -1f), new Speed(BossStatuesCrosshair.Radius * 2f * (60f / (float)BossStatuesCrosshair.SetupTime), SpeedType.Absolute), new BossStatuesCrosshair.CircleBullet(i));
		}
	}

	// Token: 0x060002B5 RID: 693 RVA: 0x0000E4FC File Offset: 0x0000C6FC
	private void FirePulse()
	{
		float num = 4.5f;
		for (int i = 0; i < 80; i++)
		{
			base.Fire(new Direction(((float)i + 0.5f) * num, DirectionType.Absolute, -1f), new Speed(BossStatuesCrosshair.Radius / ((float)BossStatuesCrosshair.PulseTravelTime / 60f), SpeedType.Absolute), new Bullet("defaultPulse", false, false, true));
		}
	}

	// Token: 0x040002ED RID: 749
	public static float QuarterPi = 0.785f;

	// Token: 0x040002EE RID: 750
	public static int SkipSetupBulletNum = 6;

	// Token: 0x040002EF RID: 751
	public static int ExtraSetupBulletNum;

	// Token: 0x040002F0 RID: 752
	public static int SetupTime = 90;

	// Token: 0x040002F1 RID: 753
	public static int BulletCount = 25;

	// Token: 0x040002F2 RID: 754
	public static float Radius = 11f;

	// Token: 0x040002F3 RID: 755
	public static int QuaterRotTime = 120;

	// Token: 0x040002F4 RID: 756
	public static int SpinTime = 600;

	// Token: 0x040002F5 RID: 757
	public static int PulseInitialDelay = 120;

	// Token: 0x040002F6 RID: 758
	public static int PulseDelay = 120;

	// Token: 0x040002F7 RID: 759
	public static int PulseCount = 4;

	// Token: 0x040002F8 RID: 760
	public static int PulseTravelTime = 100;

	// Token: 0x020000B2 RID: 178
	public class LineBullet : Bullet
	{
		// Token: 0x060002B7 RID: 695 RVA: 0x0000E5C8 File Offset: 0x0000C7C8
		public LineBullet(int spawnTime)
			: base("defaultLine", false, false, true)
		{
			this.spawnTime = spawnTime;
		}

		// Token: 0x060002B8 RID: 696 RVA: 0x0000E5E0 File Offset: 0x0000C7E0
		protected override IEnumerator Top()
		{
			base.ChangeSpeed(new Speed(0f, SpeedType.Absolute), BossStatuesCrosshair.SetupTime);
			yield return base.Wait(BossStatuesCrosshair.SetupTime);
			base.ChangeDirection(new Direction(90f, DirectionType.Relative, -1f), 1);
			yield return base.Wait(1);
			base.ChangeSpeed(new Speed((float)this.spawnTime / (float)BossStatuesCrosshair.BulletCount * (BossStatuesCrosshair.Radius * 2f) * BossStatuesCrosshair.QuarterPi * (60f / (float)BossStatuesCrosshair.QuaterRotTime), SpeedType.Relative), 1);
			base.ChangeDirection(new Direction(90f / (float)BossStatuesCrosshair.QuaterRotTime, DirectionType.Sequence, -1f), BossStatuesCrosshair.SpinTime);
			yield return base.Wait(BossStatuesCrosshair.SpinTime - 1);
			base.Vanish(false);
			yield break;
		}

		// Token: 0x040002F9 RID: 761
		public int spawnTime;
	}

	// Token: 0x020000B4 RID: 180
	public class CircleBullet : Bullet
	{
		// Token: 0x060002BF RID: 703 RVA: 0x0000E7A8 File Offset: 0x0000C9A8
		public CircleBullet(int spawnTime)
			: base("defaultCircle", false, false, false)
		{
			this.spawnTime = spawnTime;
		}

		// Token: 0x060002C0 RID: 704 RVA: 0x0000E7C0 File Offset: 0x0000C9C0
		protected override IEnumerator Top()
		{
			base.ChangeSpeed(new Speed(0f, SpeedType.Absolute), BossStatuesCrosshair.SetupTime);
			yield return base.Wait(BossStatuesCrosshair.SetupTime);
			base.ChangeDirection(new Direction(90f, DirectionType.Relative, -1f), 1);
			yield return base.Wait(1);
			base.ChangeSpeed(new Speed(BossStatuesCrosshair.Radius * 2f * BossStatuesCrosshair.QuarterPi * (60f / (float)BossStatuesCrosshair.QuaterRotTime), SpeedType.Relative), 1);
			base.ChangeDirection(new Direction(90f / (float)BossStatuesCrosshair.QuaterRotTime, DirectionType.Sequence, -1f), BossStatuesCrosshair.QuaterRotTime);
			yield return base.Wait((float)this.spawnTime * ((float)BossStatuesCrosshair.QuaterRotTime / (float)BossStatuesCrosshair.BulletCount));
			base.ChangeSpeed(new Speed(0f, SpeedType.Absolute), 1);
			for (int i = 1; i < 7; i++)
			{
				base.Fire(new Direction(-90f, DirectionType.Relative, -1f), new Speed((float)(i * 3), SpeedType.Absolute), new BossStatuesCrosshair.CircleExtraBullet(this.spawnTime));
			}
			yield return base.Wait((float)(BossStatuesCrosshair.BulletCount - this.spawnTime) * ((float)BossStatuesCrosshair.QuaterRotTime / (float)BossStatuesCrosshair.BulletCount));
			yield return base.Wait(BossStatuesCrosshair.SpinTime - BossStatuesCrosshair.QuaterRotTime);
			base.Vanish(false);
			yield break;
		}

		// Token: 0x040002FE RID: 766
		public int spawnTime;
	}

	// Token: 0x020000B6 RID: 182
	public class CircleExtraBullet : Bullet
	{
		// Token: 0x060002C7 RID: 711 RVA: 0x0000EA6C File Offset: 0x0000CC6C
		public CircleExtraBullet(int spawnTime)
			: base("defaultCircleExtra", false, false, false)
		{
			this.spawnTime = spawnTime;
		}

		// Token: 0x060002C8 RID: 712 RVA: 0x0000EA84 File Offset: 0x0000CC84
		protected override IEnumerator Top()
		{
			base.ChangeSpeed(new Speed(0f, SpeedType.Absolute), 60);
			yield return base.Wait((float)(BossStatuesCrosshair.BulletCount - this.spawnTime) * ((float)BossStatuesCrosshair.QuaterRotTime / (float)BossStatuesCrosshair.BulletCount));
			yield return base.Wait(BossStatuesCrosshair.SpinTime - BossStatuesCrosshair.QuaterRotTime);
			base.ChangeSpeed(new Speed(6f, SpeedType.Absolute), 1);
			yield return base.Wait(120);
			base.Vanish(false);
			yield break;
		}

		// Token: 0x04000303 RID: 771
		public int spawnTime;
	}
}
