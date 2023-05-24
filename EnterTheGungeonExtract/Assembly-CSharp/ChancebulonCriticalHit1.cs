using System;
using System.Collections;
using Brave.BulletScript;
using FullInspector;
using UnityEngine;

// Token: 0x02000135 RID: 309
[InspectorDropdownName("Chancebulon/CriticalHit1")]
public class ChancebulonCriticalHit1 : Script
{
	// Token: 0x06000490 RID: 1168 RVA: 0x00015EFC File Offset: 0x000140FC
	protected override IEnumerator Top()
	{
		for (int i = 0; i < 12; i++)
		{
			float num = (float)i * 30f;
			base.Fire(new Offset(1f, 0f, num, string.Empty, DirectionType.Absolute), new Direction(num, DirectionType.Absolute, -1f), new Speed(UnityEngine.Random.Range(4f, 11f), SpeedType.Absolute), new ChancebulonBlobProjectileAttack1.BlobulonBullet((ChancebulonBlobProjectileAttack1.BlobType)UnityEngine.Random.Range(0, 3)));
		}
		float deltaAngle = 30f;
		for (int j = 0; j < 12; j++)
		{
			base.Fire(new Direction(((float)j + 0.5f) * deltaAngle, DirectionType.Absolute, -1f), new Speed(6f, SpeedType.Absolute), new Bullet("icicle", false, false, false));
		}
		yield return base.Wait(30);
		base.Fire(new Direction(-28f, DirectionType.Aim, -1f), new Speed(9f, SpeedType.Absolute), new Bullet("icicle", false, false, false));
		base.Fire(new Direction(0f, DirectionType.Aim, -1f), new Speed(11f, SpeedType.Absolute), new Bullet("icicle", false, false, false));
		base.Fire(new Direction(28f, DirectionType.Aim, -1f), new Speed(9f, SpeedType.Absolute), new Bullet("icicle", false, false, false));
		yield return base.Wait(30);
		base.Fire(new Direction(-28f, DirectionType.Aim, -1f), new Speed(9f, SpeedType.Absolute), new Bullet("icicle", false, false, false));
		base.Fire(new Direction(0f, DirectionType.Aim, -1f), new Speed(11f, SpeedType.Absolute), new Bullet("icicle", false, false, false));
		base.Fire(new Direction(28f, DirectionType.Aim, -1f), new Speed(9f, SpeedType.Absolute), new Bullet("icicle", false, false, false));
		yield break;
	}

	// Token: 0x04000475 RID: 1141
	private const int NumBullets = 12;
}
