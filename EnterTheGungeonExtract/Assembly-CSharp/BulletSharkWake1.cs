using System;
using System.Collections;
using Brave.BulletScript;
using FullInspector;
using UnityEngine;

// Token: 0x0200010F RID: 271
[InspectorDropdownName("BulletShark/Wake1")]
public class BulletSharkWake1 : Script
{
	// Token: 0x0600040B RID: 1035 RVA: 0x00013A10 File Offset: 0x00011C10
	protected override IEnumerator Top()
	{
		for (int i = 0; i < 27; i++)
		{
			float startSpeed = UnityEngine.Random.Range(4.5f, 7.5f);
			float endSpeed = UnityEngine.Random.Range(-1.25f, -0.25f);
			int deathTimer = UnityEngine.Random.Range(10, 60);
			if (UnityEngine.Random.value < 0.22f)
			{
				base.Fire(new Direction(-90f, DirectionType.Relative, -1f), new Speed(startSpeed, SpeedType.Absolute), new SpeedChangingBullet("tellBullet", 9f, 60, -1, false));
				base.Fire(new Direction(90f, DirectionType.Relative, -1f), new Speed(startSpeed, SpeedType.Absolute), new SpeedChangingBullet("tellBullet", 9f, 60, -1, false));
			}
			else
			{
				base.Fire(new Direction(-90f, DirectionType.Relative, -1f), new Speed(startSpeed, SpeedType.Absolute), new SpeedChangingBullet(endSpeed, 60, deathTimer));
				base.Fire(new Direction(90f, DirectionType.Relative, -1f), new Speed(startSpeed, SpeedType.Absolute), new SpeedChangingBullet(endSpeed, 60, deathTimer));
			}
			if (i % 3 == 1)
			{
				endSpeed = BraveUtility.RandomSign() * UnityEngine.Random.Range(1f, 2f);
				deathTimer = UnityEngine.Random.Range(10, 60);
				base.Fire(new Direction(90f, DirectionType.Relative, -1f), new Speed(0f, SpeedType.Absolute), new SpeedChangingBullet(endSpeed, 60, deathTimer));
			}
			yield return base.Wait(5);
		}
		yield break;
	}
}
