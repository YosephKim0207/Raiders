using System;
using System.Collections;
using Brave.BulletScript;
using FullInspector;
using UnityEngine;

// Token: 0x02000150 RID: 336
[InspectorDropdownName("Bosses/DemonWall/Spew1")]
public class DemonWallSpew1 : Script
{
	// Token: 0x06000503 RID: 1283 RVA: 0x00018368 File Offset: 0x00016568
	protected override IEnumerator Top()
	{
		for (int i = 0; i < 4; i++)
		{
			base.StartTask(this.FireWall((float)((i % 2 != 0) ? 1 : (-1))));
			base.StartTask(this.FireWaves((i + 1) % 2));
			yield return base.Wait(110);
		}
		yield break;
	}

	// Token: 0x06000504 RID: 1284 RVA: 0x00018384 File Offset: 0x00016584
	private IEnumerator FireWall(float sign)
	{
		for (int i = 0; i < 4; i++)
		{
			bool offset = i % 2 == 1;
			for (int j = 0; j < ((!offset) ? 12 : 11); j++)
			{
				base.Fire(new Offset(sign * base.SubdivideArc(2f, 9.5f, 12, j, offset), 0f, 0f, string.Empty, DirectionType.Absolute), new Direction(-90f, DirectionType.Absolute, -1f), new Speed(7f, SpeedType.Absolute), new Bullet("spew", false, false, false));
			}
			yield return base.Wait(12);
		}
		yield break;
	}

	// Token: 0x06000505 RID: 1285 RVA: 0x000183A8 File Offset: 0x000165A8
	private IEnumerator FireWaves(int index)
	{
		for (int i = 0; i < 3; i++)
		{
			string transform = BraveUtility.RandomElement<string>(DemonWallSpew1.shootPoints[index]);
			for (int j = 0; j < 5; j++)
			{
				base.Fire(new Offset(transform), new Direction(base.SubdivideArc(-115f, 50f, 5, j, false), DirectionType.Absolute, -1f), new Speed(7f, SpeedType.Absolute), new Bullet("wave", j != 2, false, false));
			}
			float aimDirection = base.GetAimDirection(transform);
			if (UnityEngine.Random.value < 0.333f && BraveMathCollege.AbsAngleBetween(-90f, aimDirection) < 45f)
			{
				base.Fire(new Offset(transform), new Direction(aimDirection, DirectionType.Absolute, -1f), new Speed(7f, SpeedType.Absolute), new Bullet("wave", true, false, false));
			}
			yield return base.Wait(40);
		}
		yield break;
	}

	// Token: 0x040004D8 RID: 1240
	public static string[][] shootPoints = new string[][]
	{
		new string[] { "sad bullet", "blobulon", "dopey bullet" },
		new string[] { "sideways bullet", "shotgun bullet", "cultist", "angry bullet" }
	};

	// Token: 0x040004D9 RID: 1241
	private const int NumBullets = 12;
}
