using System;
using System.Collections;
using Brave.BulletScript;
using FullInspector;
using UnityEngine;

// Token: 0x020001D1 RID: 465
[InspectorDropdownName("Bosses/Helicopter/RandomBurstsSimple1")]
public class HelicopterRandomSimple1 : Script
{
	// Token: 0x060006F5 RID: 1781 RVA: 0x00021EB0 File Offset: 0x000200B0
	protected override IEnumerator Top()
	{
		if (UnityEngine.Random.value < 0.5f)
		{
			int numBullets = 8;
			float startDirection = base.RandomAngle();
			string transform = "shoot point 1";
			string transform2 = "shoot point 4";
			if (BraveUtility.RandomBool())
			{
				BraveUtility.Swap<string>(ref transform, ref transform2);
			}
			for (int i = 0; i < numBullets; i++)
			{
				base.Fire(new Offset(transform), new Direction(base.SubdivideCircle(startDirection, numBullets, i, 1f, false), DirectionType.Absolute, -1f), new Speed(9f, SpeedType.Absolute), new HelicopterRandomSimple1.BigBullet());
			}
			yield return base.Wait(15);
			for (int j = 0; j < numBullets; j++)
			{
				base.Fire(new Offset(transform2), new Direction(base.SubdivideCircle(startDirection, numBullets, j, 1f, true), DirectionType.Absolute, -1f), new Speed(9f, SpeedType.Absolute), new HelicopterRandomSimple1.BigBullet());
			}
		}
		else
		{
			int numBullets2 = 4;
			float arc = 35f;
			string transform3 = "shoot point 2";
			string transform4 = "shoot point 3";
			if (BraveUtility.RandomBool())
			{
				BraveUtility.Swap<string>(ref transform3, ref transform4);
			}
			float aimDirection = base.GetAimDirection(transform3);
			for (int k = 0; k < numBullets2; k++)
			{
				base.Fire(new Offset(transform3), new Direction(base.SubdivideArc(aimDirection - arc, arc * 2f, numBullets2, k, false), DirectionType.Absolute, -1f), new Speed(9f, SpeedType.Absolute), new HelicopterRandomSimple1.BigBullet());
			}
			yield return base.Wait(15);
			aimDirection = base.GetAimDirection(transform4);
			for (int l = 0; l < numBullets2; l++)
			{
				base.Fire(new Offset(transform4), new Direction(base.SubdivideArc(aimDirection - arc, arc * 2f, numBullets2, l, false), DirectionType.Absolute, -1f), new Speed(9f, SpeedType.Absolute), new HelicopterRandomSimple1.BigBullet());
			}
		}
		yield break;
	}

	// Token: 0x020001D2 RID: 466
	public class BigBullet : Bullet
	{
		// Token: 0x060006F6 RID: 1782 RVA: 0x00021ECC File Offset: 0x000200CC
		public BigBullet()
			: base("big", false, false, false)
		{
		}

		// Token: 0x060006F7 RID: 1783 RVA: 0x00021EDC File Offset: 0x000200DC
		protected override IEnumerator Top()
		{
			this.Projectile.Ramp(UnityEngine.Random.Range(2f, 3f), 2f);
			return null;
		}
	}
}
