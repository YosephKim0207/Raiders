using System;
using System.Collections;
using Brave.BulletScript;
using FullInspector;
using UnityEngine;

// Token: 0x0200001E RID: 30
[InspectorDropdownName("Bosses/Bashellisk/RandomLeadShots1")]
public class BashelliskRandomLeadShots1 : Script
{
	// Token: 0x06000070 RID: 112 RVA: 0x00003B80 File Offset: 0x00001D80
	protected override IEnumerator Top()
	{
		float leadAmount = UnityEngine.Random.Range(0f, 2f);
		for (int i = 0; i < this.NumBullets; i++)
		{
			float dir = base.GetAimDirection(leadAmount, this.BulletSpeed);
			base.Fire(new Direction(dir, DirectionType.Absolute, -1f), new Speed(this.BulletSpeed, SpeedType.Absolute), null);
			yield return base.Wait(10);
		}
		yield break;
	}

	// Token: 0x04000078 RID: 120
	public int NumBullets = 10;

	// Token: 0x04000079 RID: 121
	public float BulletSpeed = 14f;
}
