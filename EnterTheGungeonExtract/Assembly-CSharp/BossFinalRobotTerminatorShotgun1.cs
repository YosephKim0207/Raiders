using System;
using System.Collections;
using Brave.BulletScript;
using FullInspector;
using UnityEngine;

// Token: 0x0200009A RID: 154
[InspectorDropdownName("Bosses/BossFinalRobot/TerminatorShotgun1")]
public class BossFinalRobotTerminatorShotgun1 : Script
{
	// Token: 0x0600025F RID: 607 RVA: 0x0000BE08 File Offset: 0x0000A008
	protected override IEnumerator Top()
	{
		switch (UnityEngine.Random.Range(0, 4))
		{
		case 0:
		{
			for (int i = -2; i <= 2; i++)
			{
				base.Fire(new Direction((float)(i * 6), DirectionType.Aim, -1f), new Speed(5f, SpeedType.Absolute), null);
			}
			break;
		}
		case 1:
		{
			for (int j = -2; j <= 2; j++)
			{
				base.Fire(new Direction((float)(j * 6), DirectionType.Aim, -1f), new Speed(9f, SpeedType.Absolute), null);
			}
			break;
		}
		case 2:
		{
			float aimDirection = base.GetAimDirection(1f, 9f);
			for (int k = -2; k <= 2; k++)
			{
				base.Fire(new Direction(aimDirection + (float)(k * 6), DirectionType.Absolute, -1f), new Speed(10f - (float)Mathf.Abs(k) * 0.5f, SpeedType.Absolute), null);
			}
			break;
		}
		case 3:
		{
			for (int l = -2; l <= 2; l++)
			{
				base.Fire(new Direction((float)(l * 6), DirectionType.Aim, -1f), new Speed(9f, SpeedType.Absolute), null);
			}
			break;
		}
		}
		return null;
	}
}
