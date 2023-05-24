using System;
using System.Collections;
using Brave.BulletScript;
using FullInspector;
using UnityEngine;

// Token: 0x02000227 RID: 551
[InspectorDropdownName("Bosses/Lich/QuickDrawHomingShot1")]
public class LichQuickDrawHomingShot1 : Script
{
	// Token: 0x06000847 RID: 2119 RVA: 0x0002826C File Offset: 0x0002646C
	protected override IEnumerator Top()
	{
		float aimDirection = base.GetAimDirection(1f, 16f);
		base.Fire(new Direction(aimDirection, DirectionType.Absolute, -1f), new Speed(16f, SpeedType.Absolute), new LichQuickDrawHomingShot1.FastHomingShot());
		return null;
	}

	// Token: 0x02000228 RID: 552
	public class FastHomingShot : Bullet
	{
		// Token: 0x06000848 RID: 2120 RVA: 0x000282B0 File Offset: 0x000264B0
		public FastHomingShot()
			: base("quickHoming", false, false, false)
		{
		}

		// Token: 0x06000849 RID: 2121 RVA: 0x000282C0 File Offset: 0x000264C0
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
