using System;
using System.Collections;
using Brave.BulletScript;
using FullInspector;
using UnityEngine;

// Token: 0x02000025 RID: 37
[InspectorDropdownName("Bosses/Bashellisk/SnakeBullets1")]
public class BashelliskSnakeBullets1 : Script
{
	// Token: 0x0600008A RID: 138 RVA: 0x00004010 File Offset: 0x00002210
	protected override IEnumerator Top()
	{
		float aimDirection = base.GetAimDirection((float)(((double)UnityEngine.Random.value >= 0.5) ? 1 : 0), 11f);
		for (int i = 0; i < 8; i++)
		{
			base.Fire(new Direction(aimDirection, DirectionType.Absolute, -1f), new Speed(11f, SpeedType.Absolute), new BashelliskSnakeBullets1.SnakeBullet(i * 3));
		}
		return null;
	}

	// Token: 0x0400008D RID: 141
	private const int NumBullets = 8;

	// Token: 0x0400008E RID: 142
	private const int BulletSpeed = 11;

	// Token: 0x0400008F RID: 143
	private const float SnakeMagnitude = 0.6f;

	// Token: 0x04000090 RID: 144
	private const float SnakePeriod = 3f;

	// Token: 0x02000026 RID: 38
	public class SnakeBullet : Bullet
	{
		// Token: 0x0600008B RID: 139 RVA: 0x00004080 File Offset: 0x00002280
		public SnakeBullet(int delay)
			: base("snakeBullet", false, false, false)
		{
			this.delay = delay;
		}

		// Token: 0x0600008C RID: 140 RVA: 0x00004098 File Offset: 0x00002298
		protected override IEnumerator Top()
		{
			base.ManualControl = true;
			yield return base.Wait(this.delay);
			Vector2 truePosition = base.Position;
			for (int i = 0; i < 360; i++)
			{
				float offsetMagnitude = Mathf.SmoothStep(-0.6f, 0.6f, Mathf.PingPong(0.5f + (float)i / 60f * 3f, 1f));
				truePosition += BraveMathCollege.DegreesToVector(this.Direction, this.Speed / 60f);
				base.Position = truePosition + BraveMathCollege.DegreesToVector(this.Direction - 90f, offsetMagnitude);
				yield return base.Wait(1);
			}
			base.Vanish(false);
			yield break;
		}

		// Token: 0x04000091 RID: 145
		private int delay;
	}
}
