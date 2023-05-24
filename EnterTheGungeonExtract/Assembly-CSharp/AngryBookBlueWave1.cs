using System;
using System.Collections;
using Brave.BulletScript;
using FullInspector;
using UnityEngine;

// Token: 0x02000010 RID: 16
[InspectorDropdownName("AngryBook/BlueWave1")]
public class AngryBookBlueWave1 : Script
{
	// Token: 0x0600003C RID: 60 RVA: 0x00002F94 File Offset: 0x00001194
	protected override IEnumerator Top()
	{
		base.EndOnBlank = true;
		float startingDir = UnityEngine.Random.Range(0f, 360f);
		for (int i = 0; i < this.NumBullets; i++)
		{
			base.Fire(new Direction(startingDir + (float)i * 360f / (float)this.NumBullets, DirectionType.Absolute, -1f), new Speed(9f, SpeedType.Absolute), new AngryBookBlueWave1.WaveBullet());
		}
		yield return base.Wait(60);
		yield break;
	}

	// Token: 0x04000043 RID: 67
	public int NumBullets = 32;

	// Token: 0x02000011 RID: 17
	public class WaveBullet : Bullet
	{
		// Token: 0x0600003D RID: 61 RVA: 0x00002FB0 File Offset: 0x000011B0
		public WaveBullet()
			: base(null, false, false, false)
		{
		}

		// Token: 0x0600003E RID: 62 RVA: 0x00002FBC File Offset: 0x000011BC
		protected override IEnumerator Top()
		{
			yield return base.Wait(20);
			for (int i = 0; i < 2; i++)
			{
				base.ChangeSpeed(new Speed(-2f, SpeedType.Absolute), 20);
				yield return base.Wait(56);
				base.ChangeSpeed(new Speed(9f, SpeedType.Absolute), 20);
				yield return base.Wait(40);
			}
			base.Vanish(false);
			yield break;
		}
	}
}
