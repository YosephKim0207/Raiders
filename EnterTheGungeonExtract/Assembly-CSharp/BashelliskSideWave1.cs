using System;
using System.Collections;
using Brave.BulletScript;
using FullInspector;

// Token: 0x02000022 RID: 34
[InspectorDropdownName("Bosses/Bashellisk/SideWave1")]
public class BashelliskSideWave1 : Script
{
	// Token: 0x06000080 RID: 128 RVA: 0x00003E28 File Offset: 0x00002028
	protected override IEnumerator Top()
	{
		base.Fire(new Direction(-90f, DirectionType.Relative, -1f), new Speed(9f, SpeedType.Absolute), new BashelliskSideWave1.WaveBullet());
		base.Fire(new Direction(90f, DirectionType.Relative, -1f), new Speed(9f, SpeedType.Absolute), new BashelliskSideWave1.WaveBullet());
		return null;
	}

	// Token: 0x02000023 RID: 35
	public class WaveBullet : Bullet
	{
		// Token: 0x06000081 RID: 129 RVA: 0x00003E84 File Offset: 0x00002084
		public WaveBullet()
			: base("bigBullet", false, false, false)
		{
		}

		// Token: 0x06000082 RID: 130 RVA: 0x00003E94 File Offset: 0x00002094
		protected override IEnumerator Top()
		{
			yield return base.Wait(20);
			for (int i = 0; i < 2; i++)
			{
				base.ChangeSpeed(new Speed(-2f, SpeedType.Absolute), 20);
				yield return base.Wait(56);
				base.ChangeSpeed(new Speed(9f, SpeedType.Absolute), 20);
				yield return base.Wait(40);
			}
			base.Vanish(false);
			yield break;
		}
	}
}
