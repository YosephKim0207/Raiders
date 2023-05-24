using System;
using System.Collections;
using Brave.BulletScript;
using FullInspector;
using UnityEngine;

// Token: 0x02000230 RID: 560
[InspectorDropdownName("ManfredsRival/Magic2")]
public class ManfredsRivalMagic2 : ManfredsRivalMagic1
{
	// Token: 0x0600086A RID: 2154 RVA: 0x00028B70 File Offset: 0x00026D70
	protected override IEnumerator Top()
	{
		yield return base.Wait(30);
		for (int i = 0; i < 3; i++)
		{
			float aim = base.GetAimDirection(1f, 12f);
			base.FireCluster(aim);
			yield return base.Wait(10);
			for (int j = 0; j < 16; j++)
			{
				float num = Mathf.Lerp(-30f, 30f, (float)j / 15f);
				base.Fire(new Offset(0.5f, 0f, this.Direction + num, string.Empty, DirectionType.Absolute), new Direction(aim + num, DirectionType.Absolute, -1f), new Speed(9f, SpeedType.Absolute), new Bullet(null, true, false, false));
			}
			base.FireCluster(aim);
			yield return base.Wait(10);
			base.FireCluster(aim);
			yield return base.Wait(40);
		}
		yield break;
	}

	// Token: 0x04000868 RID: 2152
	private const int NumTimes = 3;

	// Token: 0x04000869 RID: 2153
	private const int NumBulletsMainWave = 16;
}
