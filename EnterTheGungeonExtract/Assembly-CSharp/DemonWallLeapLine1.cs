using System;
using System.Collections;
using Brave.BulletScript;
using FullInspector;
using UnityEngine;

// Token: 0x0200014D RID: 333
[InspectorDropdownName("Bosses/DemonWall/LeapLine1")]
public class DemonWallLeapLine1 : Script
{
	// Token: 0x060004F9 RID: 1273 RVA: 0x00018168 File Offset: 0x00016368
	protected override IEnumerator Top()
	{
		float num = 1f;
		for (int i = 0; i < 24; i++)
		{
			base.Fire(new Offset(-11.5f + (float)i * num, 0f, 0f, string.Empty, DirectionType.Absolute), new Direction(-90f, DirectionType.Absolute, -1f), new Speed(5f, SpeedType.Absolute), new DemonWallLeapLine1.WaveBullet());
		}
		return null;
	}

	// Token: 0x040004CF RID: 1231
	private const int NumBullets = 24;

	// Token: 0x0200014E RID: 334
	private class WaveBullet : Bullet
	{
		// Token: 0x060004FA RID: 1274 RVA: 0x000181D4 File Offset: 0x000163D4
		public WaveBullet()
			: base("leap", false, false, false)
		{
		}

		// Token: 0x060004FB RID: 1275 RVA: 0x000181E4 File Offset: 0x000163E4
		protected override IEnumerator Top()
		{
			base.ManualControl = true;
			Vector2 truePosition = base.Position;
			for (int i = 0; i < 600; i++)
			{
				base.UpdateVelocity();
				truePosition += this.Velocity / 60f;
				base.Position = truePosition + new Vector2(0f, Mathf.Sin((float)base.Tick / 60f / 0.75f * 3.1415927f) * 1.5f);
				yield return base.Wait(1);
			}
			base.Vanish(false);
			yield break;
		}

		// Token: 0x040004D0 RID: 1232
		private const float SinPeriod = 0.75f;

		// Token: 0x040004D1 RID: 1233
		private const float SinMagnitude = 1.5f;
	}
}
