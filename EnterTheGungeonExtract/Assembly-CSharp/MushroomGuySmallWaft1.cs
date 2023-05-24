using System;
using System.Collections;
using Brave.BulletScript;
using FullInspector;
using UnityEngine;

// Token: 0x020002AE RID: 686
[InspectorDropdownName("MushroomGuy/SmallWaft1")]
public class MushroomGuySmallWaft1 : Script
{
	// Token: 0x06000A85 RID: 2693 RVA: 0x00032BAC File Offset: 0x00030DAC
	protected override IEnumerator Top()
	{
		for (int i = 0; i < 30; i++)
		{
			string text = ((UnityEngine.Random.value > 0.33f) ? "spore2" : "spore1");
			base.Fire(new Direction(base.RandomAngle(), DirectionType.Absolute, -1f), new Speed(UnityEngine.Random.Range(1.2f, 6f), SpeedType.Absolute), new MushroomGuySmallWaft1.WaftBullet(text));
		}
		for (int j = 0; j < 10; j++)
		{
			string text2 = ((UnityEngine.Random.value > 0.33f) ? "spore2" : "spore1");
			Bullet bullet = new SpeedChangingBullet(text2, 9f, 75, 300, false);
			base.Fire(new Direction(base.RandomAngle(), DirectionType.Absolute, -1f), new Speed((float)UnityEngine.Random.Range(2, 16), SpeedType.Absolute), bullet);
			bullet.Projectile.spriteAnimator.Play();
		}
		return null;
	}

	// Token: 0x04000AF5 RID: 2805
	private const int NumWaftBullets = 30;

	// Token: 0x04000AF6 RID: 2806
	private const int NumFastBullets = 10;

	// Token: 0x04000AF7 RID: 2807
	private const float VerticalDriftVelocity = -0.5f;

	// Token: 0x04000AF8 RID: 2808
	private const float WaftXPeriod = 3f;

	// Token: 0x04000AF9 RID: 2809
	private const float WaftXMagnitude = 0.5f;

	// Token: 0x04000AFA RID: 2810
	private const float WaftYPeriod = 1f;

	// Token: 0x04000AFB RID: 2811
	private const float WaftYMagnitude = 0.125f;

	// Token: 0x04000AFC RID: 2812
	private const int WaftLifeTime = 300;

	// Token: 0x020002AF RID: 687
	public class WaftBullet : Bullet
	{
		// Token: 0x06000A86 RID: 2694 RVA: 0x00032CA0 File Offset: 0x00030EA0
		public WaftBullet(string bankName)
			: base(bankName, false, false, false)
		{
		}

		// Token: 0x06000A87 RID: 2695 RVA: 0x00032CAC File Offset: 0x00030EAC
		protected override IEnumerator Top()
		{
			base.ChangeSpeed(new Speed(0f, SpeedType.Absolute), 120);
			yield return base.Wait(120);
			base.ManualControl = true;
			Vector2 truePosition = base.Position;
			float xOffset = UnityEngine.Random.Range(0f, 3f);
			float yOffset = UnityEngine.Random.Range(0f, 1f);
			truePosition -= new Vector2(Mathf.Sin(xOffset * 3.1415927f / 3f) * 0.5f, Mathf.Sin(yOffset * 3.1415927f / 1f) * 0.125f);
			for (int i = 0; i < 300; i++)
			{
				if (base.IsOwnerAlive && UnityEngine.Random.value < 0.0005f)
				{
					this.Projectile.spriteAnimator.Play();
					yield return base.Wait(30);
					base.ManualControl = false;
					this.Direction = base.AimDirection;
					base.ChangeSpeed(new Speed(9f, SpeedType.Absolute), 30);
					yield break;
				}
				truePosition += new Vector2(0f, -0.008333334f);
				float t = (float)i / 60f;
				float waftXOffset = Mathf.Sin((t + xOffset) * 3.1415927f / 3f) * 0.5f;
				float waftYOffset = Mathf.Sin((t + yOffset) * 3.1415927f / 1f) * 0.125f;
				base.Position = truePosition + new Vector2(waftXOffset, waftYOffset);
				yield return base.Wait(1);
			}
			base.Vanish(false);
			yield break;
		}
	}
}
