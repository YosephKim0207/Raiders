using System;
using System.Collections;
using Brave.BulletScript;
using FullInspector;
using UnityEngine;

// Token: 0x0200020D RID: 525
[InspectorDropdownName("Bosses/Infinilich/SpinBeams1")]
public class InfinilichSpinBeams1 : Script
{
	// Token: 0x060007DA RID: 2010 RVA: 0x00026758 File Offset: 0x00024958
	protected override IEnumerator Top()
	{
		for (;;)
		{
			yield return base.Wait(3);
			base.StartTask(this.FireBeam(base.Position + new Vector2(1f, 0f), 0f));
			yield return base.Wait(9);
			base.StartTask(this.FireBeam(base.Position + new Vector2(-1f, 0f), 180f));
			yield return base.Wait(6);
		}
		yield break;
	}

	// Token: 0x060007DB RID: 2011 RVA: 0x00026774 File Offset: 0x00024974
	private IEnumerator FireBeam(Vector2 pos, float direction)
	{
		AkSoundEngine.PostEvent("Play_BOSS_lichC_zap_01", base.BulletBank.gameObject);
		for (int i = 0; i < 7; i++)
		{
			for (int j = 0; j < 3; j++)
			{
				float num = (float)((i * 3 + j) * ((direction <= 90f) ? 1 : (-1)));
				base.Fire(Offset.OverridePosition(pos + new Vector2(num, 0.0625f)), new Direction(direction, DirectionType.Absolute, -1f), new Speed(0f, SpeedType.Absolute), new InfinilichSpinBeams1.BeamBullet(i));
				base.Fire(Offset.OverridePosition(pos + new Vector2(num, -0.375f)), new Direction(direction, DirectionType.Absolute, -1f), new Speed(0f, SpeedType.Absolute), new InfinilichSpinBeams1.BeamBullet(i));
			}
			yield return base.Wait(1);
		}
		yield break;
	}

	// Token: 0x040007EB RID: 2027
	private const int TurnSpeed = 3;

	// Token: 0x040007EC RID: 2028
	private const int TurnDelay = 3;

	// Token: 0x040007ED RID: 2029
	private const int BeamSetupTime = 7;

	// Token: 0x040007EE RID: 2030
	private const int BeamLifeTime = 12;

	// Token: 0x0200020E RID: 526
	public class BeamBullet : Bullet
	{
		// Token: 0x060007DC RID: 2012 RVA: 0x000267A0 File Offset: 0x000249A0
		public BeamBullet(int spawnDelay)
			: base(null, false, false, false)
		{
			this.m_spawnDelay = spawnDelay;
		}

		// Token: 0x060007DD RID: 2013 RVA: 0x000267B4 File Offset: 0x000249B4
		protected override IEnumerator Top()
		{
			yield return base.Wait(12 - this.m_spawnDelay);
			base.Vanish(false);
			yield break;
		}

		// Token: 0x040007EF RID: 2031
		private int m_spawnDelay;
	}
}
