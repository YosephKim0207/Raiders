using System;
using System.Collections;
using Brave.BulletScript;
using FullInspector;
using UnityEngine;

// Token: 0x0200014B RID: 331
[InspectorDropdownName("Bosses/DemonWall/BasicWaves1")]
public class DemonWallBasicWaves1 : Script
{
	// Token: 0x060004EF RID: 1263 RVA: 0x00017EF8 File Offset: 0x000160F8
	protected override IEnumerator Top()
	{
		int group = 1;
		for (int i = 0; i < 10; i++)
		{
			group = BraveUtility.SequentialRandomRange(0, DemonWallBasicWaves1.shootPoints.Length, group, null, true);
			this.FireWave(BraveUtility.RandomElement<string>(DemonWallBasicWaves1.shootPoints[group]));
			yield return base.Wait(20);
		}
		yield break;
	}

	// Token: 0x060004F0 RID: 1264 RVA: 0x00017F14 File Offset: 0x00016114
	private void FireWave(string transform)
	{
		for (int i = 0; i < 7; i++)
		{
			base.Fire(new Offset(transform), new Direction(base.SubdivideArc(-125f, 70f, 7, i, false), DirectionType.Absolute, -1f), new Speed(7f, SpeedType.Absolute), new Bullet("wave", i != 3, false, false));
		}
		float aimDirection = base.GetAimDirection(transform);
		if (UnityEngine.Random.value < 0.333f && BraveMathCollege.AbsAngleBetween(-90f, aimDirection) < 45f)
		{
			base.Fire(new Offset(transform), new Direction(aimDirection, DirectionType.Absolute, -1f), new Speed(7f, SpeedType.Absolute), new Bullet("wave", true, false, false));
		}
	}

	// Token: 0x040004C7 RID: 1223
	public static string[][] shootPoints = new string[][]
	{
		new string[] { "sad bullet", "blobulon", "dopey bullet" },
		new string[] { "left eye", "right eye", "crashed bullet" },
		new string[] { "sideways bullet", "shotgun bullet", "cultist", "angry bullet" }
	};

	// Token: 0x040004C8 RID: 1224
	public const int NumBursts = 10;
}
