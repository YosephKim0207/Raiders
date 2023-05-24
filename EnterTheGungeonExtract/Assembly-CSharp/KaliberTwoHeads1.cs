using System;
using System.Collections;
using Brave.BulletScript;
using FullInspector;

// Token: 0x02000218 RID: 536
[InspectorDropdownName("Kaliber/TwoHeads1")]
public class KaliberTwoHeads1 : Script
{
	// Token: 0x0600080C RID: 2060 RVA: 0x00027388 File Offset: 0x00025588
	protected override IEnumerator Top()
	{
		for (int i = 0; i < 6; i++)
		{
			yield return base.Wait(24);
			bool offset = i % 4 > 1;
			if (i % 2 == 0)
			{
				this.FireArc("2 top left", 70f, 150f, 8, 4, offset);
				this.FireArc("2 bottom left", 220f, 80f, 5, 2, offset);
			}
			else
			{
				this.FireArc("2 top right", 110f, -150f, 8, 4, offset);
				this.FireArc("2 bottom right", 320f, -80f, 5, 2, offset);
			}
		}
		yield return base.Wait(18);
		yield break;
	}

	// Token: 0x0600080D RID: 2061 RVA: 0x000273A4 File Offset: 0x000255A4
	private void FireArc(string transform, float startAngle, float sweepAngle, int numBullets, int muzzleIndex, bool offset)
	{
		float num = (float)((!offset) ? numBullets : (numBullets - 1));
		int num2 = 0;
		while ((float)num2 < num)
		{
			Offset offset2 = new Offset(transform);
			Direction direction = new Direction(base.SubdivideArc(startAngle, sweepAngle, numBullets, num2, offset), DirectionType.Absolute, -1f);
			Speed speed = new Speed(9f, SpeedType.Absolute);
			bool flag = num2 != muzzleIndex;
			base.Fire(offset2, direction, speed, new Bullet(null, flag, false, false));
			num2++;
		}
	}

	// Token: 0x04000818 RID: 2072
	private const int NumShots = 6;
}
