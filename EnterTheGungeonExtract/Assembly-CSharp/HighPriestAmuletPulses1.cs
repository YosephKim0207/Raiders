using System;
using System.Collections;
using Brave.BulletScript;
using FullInspector;
using UnityEngine;

// Token: 0x020001D8 RID: 472
[InspectorDropdownName("Bosses/HighPriest/AmuletPulses1")]
public class HighPriestAmuletPulses1 : Script
{
	// Token: 0x06000710 RID: 1808 RVA: 0x0002269C File Offset: 0x0002089C
	protected override IEnumerator Top()
	{
		float angleDelta = 14.4f;
		for (int j = 0; j < 25; j++)
		{
			base.Fire(new Offset(2.5f, 0f, (float)j * angleDelta, string.Empty, DirectionType.Absolute), new Direction((float)j * angleDelta, DirectionType.Absolute, -1f), new Speed(0f, SpeedType.Absolute), new HighPriestAmuletPulses1.VibratingBullet());
		}
		for (int k = 0; k < 25; k++)
		{
			base.Fire(new Offset(3.25f, 0f, ((float)k + 0.5f) * angleDelta, string.Empty, DirectionType.Absolute), new Direction(((float)k + 0.5f) * angleDelta, DirectionType.Absolute, -1f), new Speed(0f, SpeedType.Absolute), new HighPriestAmuletPulses1.VibratingBullet());
		}
		yield return base.Wait(60);
		for (int i = 0; i < 12; i++)
		{
			base.Fire(new Direction(base.RandomAngle(), DirectionType.Absolute, -1f), new Bullet("homing", false, false, false));
			yield return base.Wait(10);
		}
		yield return base.Wait(220);
		yield break;
	}

	// Token: 0x040006F8 RID: 1784
	private const int NumBullets = 25;

	// Token: 0x020001D9 RID: 473
	public class VibratingBullet : Bullet
	{
		// Token: 0x06000711 RID: 1809 RVA: 0x000226B8 File Offset: 0x000208B8
		public VibratingBullet()
			: base("amuletRing", false, false, false)
		{
		}

		// Token: 0x06000712 RID: 1810 RVA: 0x000226C8 File Offset: 0x000208C8
		protected override IEnumerator Top()
		{
			this.Speed = 1f;
			yield return base.Wait(1);
			for (int i = 0; i < 20; i++)
			{
				float randWait = (float)UnityEngine.Random.Range(1, 11);
				yield return base.Wait(randWait);
				this.Direction += 180f;
				yield return base.Wait(10);
				this.Direction -= 180f;
				yield return base.Wait(10f - randWait);
				this.Direction += 180f;
			}
			this.Speed = 12f;
			yield return base.Wait(90);
			base.Vanish(false);
			yield break;
		}
	}
}
