using System;
using System.Collections;
using Brave.BulletScript;
using UnityEngine;

// Token: 0x020002FD RID: 765
public class SpectreGroupShot : Script
{
	// Token: 0x06000BDE RID: 3038 RVA: 0x0003A0FC File Offset: 0x000382FC
	protected override IEnumerator Top()
	{
		for (int i = 0; i < 4; i++)
		{
			yield return base.Wait(30);
			this.FireFrom("eyes " + UnityEngine.Random.Range(1, 4));
		}
		yield break;
	}

	// Token: 0x06000BDF RID: 3039 RVA: 0x0003A118 File Offset: 0x00038318
	private void FireFrom(string transform)
	{
		float aimDirection = base.GetAimDirection(transform, (float)UnityEngine.Random.Range(0, 2), 8f);
		Vector2 vector = PhysicsEngine.PixelToUnit(new IntVector2(4, 0));
		Vector2 vector2 = vector;
		base.Fire(new Offset(vector2, 0f, transform, DirectionType.Absolute), new Direction(aimDirection, DirectionType.Absolute, -1f), new Speed(8f, SpeedType.Absolute), new Bullet("eyeBullet", false, false, false));
		vector2 = -vector;
		base.Fire(new Offset(vector2, 0f, transform, DirectionType.Absolute), new Direction(aimDirection, DirectionType.Absolute, -1f), new Speed(8f, SpeedType.Absolute), new Bullet("eyeBullet", false, false, false));
	}

	// Token: 0x04000CB3 RID: 3251
	private const int NumBullets = 4;
}
