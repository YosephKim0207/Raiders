using System;
using System.Collections;
using Brave.BulletScript;
using FullInspector;
using UnityEngine;

// Token: 0x020001E1 RID: 481
[InspectorDropdownName("Bosses/HighPriest/FaceShoot1")]
public class HighPriestFaceShoot1 : Script
{
	// Token: 0x06000732 RID: 1842 RVA: 0x00022EB0 File Offset: 0x000210B0
	protected override IEnumerator Top()
	{
		yield return base.Wait(60);
		float aim = base.GetAimDirection(1f, 16f);
		base.Fire(new Direction(aim, DirectionType.Absolute, -1f), new Speed(16f, SpeedType.Absolute), new HighPriestFaceShoot1.FastHomingShot());
		yield return base.Wait(30);
		yield break;
	}

	// Token: 0x020001E2 RID: 482
	public class FastHomingShot : Bullet
	{
		// Token: 0x06000733 RID: 1843 RVA: 0x00022ECC File Offset: 0x000210CC
		public FastHomingShot()
			: base("quickHoming", false, false, false)
		{
		}

		// Token: 0x06000734 RID: 1844 RVA: 0x00022EDC File Offset: 0x000210DC
		protected override IEnumerator Top()
		{
			for (int i = 0; i < 180; i++)
			{
				float aim = base.GetAimDirection(1f, 16f);
				float delta = BraveMathCollege.ClampAngle180(aim - this.Direction);
				if (Mathf.Abs(delta) > 100f)
				{
					yield break;
				}
				this.Direction += Mathf.MoveTowards(0f, delta, 3f);
				yield return base.Wait(1);
			}
			yield break;
		}
	}
}
