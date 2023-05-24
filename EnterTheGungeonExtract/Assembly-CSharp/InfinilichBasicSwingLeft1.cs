using System;
using System.Collections;
using Brave.BulletScript;
using FullInspector;
using UnityEngine;

// Token: 0x020001F1 RID: 497
[InspectorDropdownName("Bosses/Infinilich/BasicSwingLeft1")]
public class InfinilichBasicSwingLeft1 : Script
{
	// Token: 0x06000767 RID: 1895 RVA: 0x000238BC File Offset: 0x00021ABC
	protected override IEnumerator Top()
	{
		for (int i = 0; i < InfinilichBasicSwingLeft1.ShootPoints.Length; i++)
		{
			string text = "bullet limb " + InfinilichBasicSwingLeft1.ShootPoints[i];
			float num = Mathf.Lerp(0f, 2f, (float)i / ((float)InfinilichBasicSwingLeft1.ShootPoints.Length - 1f));
			float aimDirection = base.GetAimDirection(text, num, 12f);
			base.Fire(new Offset(text), new Direction(aimDirection, DirectionType.Absolute, -1f), new Speed(12f, SpeedType.Absolute), null);
		}
		return null;
	}

	// Token: 0x0400073B RID: 1851
	private const float EnemyBulletSpeedItem = 12f;

	// Token: 0x0400073C RID: 1852
	private static int[] ShootPoints = new int[]
	{
		4, 9, 13, 18, 20, 21, 22, 23, 24, 25,
		26
	};
}
