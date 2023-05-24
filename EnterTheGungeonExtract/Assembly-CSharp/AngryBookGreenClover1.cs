using System;
using System.Collections;
using Brave.BulletScript;
using FullInspector;
using UnityEngine;

// Token: 0x02000014 RID: 20
[InspectorDropdownName("AngryBook/GreenClover1")]
public class AngryBookGreenClover1 : Script
{
	// Token: 0x0600004C RID: 76 RVA: 0x00003254 File Offset: 0x00001454
	protected override IEnumerator Top()
	{
		base.EndOnBlank = true;
		for (int i = 0; i < this.NumBullets; i++)
		{
			float angleRad = (float)i * (6.2831855f / (float)this.NumBullets);
			float radius = Mathf.Sin(2f * angleRad) + 0.25f * Mathf.Sin(6f * angleRad);
			float x = radius * Mathf.Cos(angleRad) * 2f;
			float y = radius * Mathf.Sin(angleRad) * 2f;
			float angleFromCenter = BraveMathCollege.Atan2Degrees(y, x);
			base.Fire(new Offset(x, y, 0f, string.Empty, DirectionType.Absolute), new Direction(angleFromCenter, DirectionType.Absolute, -1f), new AngryBookGreenClover1.WaveBullet(i));
			yield return base.Wait(1);
		}
		yield break;
	}

	// Token: 0x0400004E RID: 78
	public int NumBullets = 60;

	// Token: 0x02000015 RID: 21
	public class WaveBullet : Bullet
	{
		// Token: 0x0600004D RID: 77 RVA: 0x00003270 File Offset: 0x00001470
		public WaveBullet(int spawnTime)
			: base(null, false, false, false)
		{
			this.spawnTime = spawnTime;
		}

		// Token: 0x0600004E RID: 78 RVA: 0x00003284 File Offset: 0x00001484
		protected override IEnumerator Top()
		{
			yield return base.Wait(75 - this.spawnTime);
			base.ChangeSpeed(new Speed(8f, SpeedType.Absolute), 1);
			yield break;
		}

		// Token: 0x0400004F RID: 79
		public int spawnTime;
	}
}
