using System;
using System.Collections;
using Brave.BulletScript;
using FullInspector;
using UnityEngine;

// Token: 0x02000040 RID: 64
[InspectorDropdownName("Bosses/Blobulord/MoveSpray1")]
public class BlobulordMoveSpray1 : Script
{
	// Token: 0x060000F0 RID: 240 RVA: 0x00005CA4 File Offset: 0x00003EA4
	protected override IEnumerator Top()
	{
		int i = 0;
		while ((float)i < 30f)
		{
			base.Fire(new Direction(UnityEngine.Random.Range(-75f, 75f), DirectionType.Aim, -1f), new Speed(12f, SpeedType.Absolute), new Bullet("spew", false, false, false));
			yield return base.Wait(3);
			i++;
		}
		yield break;
	}

	// Token: 0x040000F7 RID: 247
	private const float NumBullets = 30f;

	// Token: 0x040000F8 RID: 248
	private const float ArcDegrees = 150f;
}
