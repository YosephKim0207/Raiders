using System;
using System.Collections;
using Brave.BulletScript;
using FullInspector;
using UnityEngine;

// Token: 0x0200022E RID: 558
[InspectorDropdownName("ManfredsRival/Magic1")]
public class ManfredsRivalMagic1 : Script
{
	// Token: 0x06000861 RID: 2145 RVA: 0x0002880C File Offset: 0x00026A0C
	protected override IEnumerator Top()
	{
		yield return base.Wait(30);
		for (int i = 0; i < 4; i++)
		{
			float aim = base.GetAimDirection(1f, 12f);
			this.FireCluster(aim);
			yield return base.Wait(10);
			for (int j = 0; j < 16; j++)
			{
				float num = Mathf.Lerp(-30f, 30f, (float)j / 15f);
				base.Fire(new Offset(0.5f, 0f, this.Direction + num, string.Empty, DirectionType.Absolute), new Direction(aim + num, DirectionType.Absolute, -1f), new Speed(9f, SpeedType.Absolute), new Bullet(null, true, false, false));
			}
			this.FireCluster(aim);
			yield return base.Wait(10);
			this.FireCluster(aim);
			yield return base.Wait(60);
		}
		yield break;
	}

	// Token: 0x06000862 RID: 2146 RVA: 0x00028828 File Offset: 0x00026A28
	protected void FireCluster(float aim)
	{
		AkSoundEngine.PostEvent("Play_ENM_cannonarmor_blast_01", base.BulletBank.gameObject);
		base.Fire(new Offset(0.5f, 0f, aim, string.Empty, DirectionType.Absolute), new Direction(aim, DirectionType.Absolute, -1f), new Speed(11f, SpeedType.Absolute), null);
		base.Fire(new Offset(0.25f, 0.3f, aim, string.Empty, DirectionType.Absolute), new Direction(aim, DirectionType.Absolute, -1f), new Speed(11f, SpeedType.Absolute), null);
		base.Fire(new Offset(0.25f, -0.3f, aim, string.Empty, DirectionType.Absolute), new Direction(aim, DirectionType.Absolute, -1f), new Speed(11f, SpeedType.Absolute), null);
		base.Fire(new Offset(0f, 0.4f, aim, string.Empty, DirectionType.Absolute), new Direction(aim, DirectionType.Absolute, -1f), new Speed(11f, SpeedType.Absolute), null);
		base.Fire(new Offset(0f, -0.4f, aim, string.Empty, DirectionType.Absolute), new Direction(aim, DirectionType.Absolute, -1f), new Speed(11f, SpeedType.Absolute), null);
	}

	// Token: 0x04000860 RID: 2144
	private const int NumTimes = 4;

	// Token: 0x04000861 RID: 2145
	private const int NumBulletsMainWave = 16;
}
