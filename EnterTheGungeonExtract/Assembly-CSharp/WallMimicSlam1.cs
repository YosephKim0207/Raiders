using System;
using System.Collections;
using Brave.BulletScript;
using FullInspector;
using UnityEngine;

// Token: 0x0200031E RID: 798
[InspectorDropdownName("MimicWall/Slam1")]
public class WallMimicSlam1 : Script
{
	// Token: 0x06000C54 RID: 3156 RVA: 0x0003B71C File Offset: 0x0003991C
	protected override IEnumerator Top()
	{
		float facingDirection = base.BulletBank.aiAnimator.CurrentArtAngle;
		this.FireLine(facingDirection - 90f, 5f, 45f, -15f, false);
		this.FireLine(facingDirection, 11f, -45f, 45f, false);
		this.FireLine(facingDirection + 90f, 5f, -45f, 15f, false);
		yield return base.Wait(10);
		this.FireLine(facingDirection - 90f, 4f, 45f, -15f, false);
		this.FireLine(facingDirection, 10f, -45f, 45f, false);
		this.FireLine(facingDirection + 90f, 4f, -45f, 15f, false);
		yield break;
	}

	// Token: 0x06000C55 RID: 3157 RVA: 0x0003B738 File Offset: 0x00039938
	protected void FireLine(float centralAngle, float numBullets, float minAngle, float maxAngle, bool addBlackBullets = false)
	{
		float num = (maxAngle - minAngle) / (numBullets - 1f);
		int num2 = 0;
		while ((float)num2 < numBullets)
		{
			float num3 = Mathf.Atan((minAngle + (float)num2 * num) / 45f) * 57.29578f;
			float num4 = Mathf.Cos(num3 * 0.017453292f);
			float num5 = (((double)Mathf.Abs(num4) >= 0.0001) ? (1f / num4) : 1f);
			Bullet bullet = new Bullet(null, false, false, false);
			if (addBlackBullets && num2 % 2 == 1)
			{
				bullet.ForceBlackBullet = true;
			}
			base.Fire(new Direction(num3 + centralAngle, DirectionType.Absolute, -1f), new Speed(num5 * 9f, SpeedType.Absolute), bullet);
			num2++;
		}
	}
}
