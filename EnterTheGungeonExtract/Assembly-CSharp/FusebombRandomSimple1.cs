using System;
using System.Collections;
using Brave.BulletScript;
using FullInspector;
using UnityEngine;

// Token: 0x020001A4 RID: 420
[InspectorDropdownName("Bosses/Fusebomb/RandomBurstsSimple1")]
public class FusebombRandomSimple1 : Script
{
	// Token: 0x06000645 RID: 1605 RVA: 0x0001E450 File Offset: 0x0001C650
	protected override IEnumerator Top()
	{
		if (UnityEngine.Random.value < 0.5f)
		{
			int num = 10;
			float num2 = base.RandomAngle();
			for (int i = 0; i < num; i++)
			{
				base.Fire(new Direction(base.SubdivideArc(num2, 360f, num, i, false), DirectionType.Absolute, -1f), new Speed(9f, SpeedType.Absolute), null);
			}
		}
		else
		{
			int num3 = 5;
			float aimDirection = base.AimDirection;
			float num4 = 35f;
			bool flag = BraveUtility.RandomBool();
			for (int j = 0; j < num3 + ((!flag) ? 0 : (-1)); j++)
			{
				base.Fire(new Direction(base.SubdivideArc(aimDirection - num4, num4 * 2f, num3, j, flag), DirectionType.Absolute, -1f), new Speed(9f, SpeedType.Absolute), null);
			}
		}
		return null;
	}
}
