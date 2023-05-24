using System;
using System.Collections;
using Brave.BulletScript;
using FullInspector;
using UnityEngine;

// Token: 0x020002AB RID: 683
[InspectorDropdownName("MushroomGuy/GiantWaft1")]
public class MushroomGuyGiantWaft1 : Script
{
	// Token: 0x06000A7B RID: 2683 RVA: 0x00032790 File Offset: 0x00030990
	protected override IEnumerator Top()
	{
		for (int i = 0; i < 50; i++)
		{
			string text = ((UnityEngine.Random.value > 0.33f) ? "spore2" : "spore1");
			base.Fire(new Direction(base.RandomAngle(), DirectionType.Absolute, -1f), new Speed((float)UnityEngine.Random.Range(2, 14), SpeedType.Absolute), new MushroomGuyGiantWaft1.WaftBullet(text));
		}
		for (int j = 0; j < 25; j++)
		{
			string text2 = ((UnityEngine.Random.value > 0.33f) ? "spore2" : "spore1");
			Bullet bullet = new SpeedChangingBullet(text2, 10.2f, 75, 300, false);
			base.Fire(new Direction(base.RandomAngle(), DirectionType.Absolute, -1f), new Speed((float)UnityEngine.Random.Range(2, 16), SpeedType.Absolute), bullet);
			bullet.Projectile.spriteAnimator.Play();
		}
		return null;
	}

	// Token: 0x04000AE2 RID: 2786
	private const int NumWaftBullets = 50;

	// Token: 0x04000AE3 RID: 2787
	private const int NumFastBullets = 25;

	// Token: 0x04000AE4 RID: 2788
	private const float VerticalDriftVelocity = -0.5f;

	// Token: 0x04000AE5 RID: 2789
	private const float WaftXPeriod = 3f;

	// Token: 0x04000AE6 RID: 2790
	private const float WaftXMagnitude = 0.5f;

	// Token: 0x04000AE7 RID: 2791
	private const float WaftYPeriod = 1f;

	// Token: 0x04000AE8 RID: 2792
	private const float WaftYMagnitude = 0.125f;

	// Token: 0x04000AE9 RID: 2793
	private const int WaftLifeTime = 300;

	// Token: 0x020002AC RID: 684
	public class WaftBullet : Bullet
	{
		// Token: 0x06000A7C RID: 2684 RVA: 0x0003287C File Offset: 0x00030A7C
		public WaftBullet(string bankName)
			: base(bankName, false, false, false)
		{
		}

		// Token: 0x06000A7D RID: 2685 RVA: 0x00032888 File Offset: 0x00030A88
		protected override IEnumerator Top()
		{
			base.ChangeSpeed(new Speed(0f, SpeedType.Absolute), 150);
			yield return base.Wait(150);
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
