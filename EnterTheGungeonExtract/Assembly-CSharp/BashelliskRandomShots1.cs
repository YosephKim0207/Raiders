using System;
using System.Collections;
using Brave.BulletScript;
using FullInspector;
using UnityEngine;

// Token: 0x02000020 RID: 32
[InspectorDropdownName("Bosses/Bashellisk/RandomShots1")]
public class BashelliskRandomShots1 : Script
{
	// Token: 0x06000078 RID: 120 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected override IEnumerator Top()
	{
		for (int i = 0; i < this.NumBullets; i++)
		{
			base.Fire(new Direction(base.GetAimDirection(1f, this.BulletSpeed) + (float)UnityEngine.Random.Range(-45, 45), DirectionType.Absolute, -1f), new Speed(this.BulletSpeed, SpeedType.Absolute), new Bullet("randomBullet", false, false, false));
			yield return base.Wait(12);
		}
		yield break;
	}

	// Token: 0x04000081 RID: 129
	public int NumBullets = 5;

	// Token: 0x04000082 RID: 130
	public float BulletSpeed = 10f;
}
