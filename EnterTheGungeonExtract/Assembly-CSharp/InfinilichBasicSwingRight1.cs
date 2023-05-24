using System;
using System.Collections;
using Brave.BulletScript;
using FullInspector;
using UnityEngine;

// Token: 0x020001F2 RID: 498
[InspectorDropdownName("Bosses/Infinilich/BasicSwingRight1")]
public class InfinilichBasicSwingRight1 : Script
{
	// Token: 0x0600076A RID: 1898 RVA: 0x00023970 File Offset: 0x00021B70
	protected override IEnumerator Top()
	{
		for (int i = 0; i < InfinilichBasicSwingRight1.ShootPoints.Length; i++)
		{
			string text = "bullet limb " + InfinilichBasicSwingRight1.ShootPoints[i];
			float num = Mathf.Lerp(0f, 2f, (float)i / ((float)InfinilichBasicSwingRight1.ShootPoints.Length - 1f));
			float aimDirection = base.GetAimDirection(text, num, 12f);
			base.Fire(new Offset(text), new Direction(aimDirection, DirectionType.Absolute, -1f), new Speed(12f, SpeedType.Absolute), null);
		}
		return null;
	}

	// Token: 0x0400073D RID: 1853
	private const float EnemyBulletSpeedItem = 12f;

	// Token: 0x0400073E RID: 1854
	private static int[] ShootPoints = new int[]
	{
		4, 9, 13, 18, 20, 21, 22, 23, 24, 25,
		26
	};
}
