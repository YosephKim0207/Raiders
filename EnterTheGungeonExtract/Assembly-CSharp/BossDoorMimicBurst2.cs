using System;
using System.Collections;
using Brave.BulletScript;
using FullInspector;
using UnityEngine;

// Token: 0x02000050 RID: 80
[InspectorDropdownName("Bosses/BossDoorMimic/Burst2")]
public class BossDoorMimicBurst2 : Script
{
	// Token: 0x06000135 RID: 309 RVA: 0x00006C20 File Offset: 0x00004E20
	protected override IEnumerator Top()
	{
		float floatDirection = base.RandomAngle();
		for (int i = 0; i < 5; i++)
		{
			float startDirection = base.RandomAngle();
			Vector2 floatVelocity = BraveMathCollege.DegreesToVector(floatDirection, 4f);
			for (int j = 0; j < 36; j++)
			{
				base.Fire(new Direction(base.SubdivideCircle(startDirection, 36, j, 1f, false), DirectionType.Absolute, -1f), new Speed(8f, SpeedType.Absolute), new BossDoorMimicBurst2.BurstBullet(floatVelocity));
			}
			floatDirection = floatDirection + 180f + UnityEngine.Random.Range(-60f, 60f);
			yield return base.Wait(90);
		}
		yield return base.Wait(75);
		yield break;
	}

	// Token: 0x04000133 RID: 307
	private const int NumBursts = 5;

	// Token: 0x04000134 RID: 308
	private const int NumBullets = 36;

	// Token: 0x02000051 RID: 81
	public class BurstBullet : Bullet
	{
		// Token: 0x06000136 RID: 310 RVA: 0x00006C3C File Offset: 0x00004E3C
		public BurstBullet(Vector2 additionalVelocity)
			: base("burst", false, false, false)
		{
			this.m_addtionalVelocity = additionalVelocity;
		}

		// Token: 0x06000137 RID: 311 RVA: 0x00006C54 File Offset: 0x00004E54
		protected override IEnumerator Top()
		{
			base.ManualControl = true;
			for (int i = 0; i < 300; i++)
			{
				base.UpdateVelocity();
				this.Velocity += this.m_addtionalVelocity * Mathf.Min(9f, (float)i / 30f);
				base.UpdatePosition();
				yield return base.Wait(1);
			}
			base.Vanish(false);
			yield break;
		}

		// Token: 0x04000135 RID: 309
		private Vector2 m_addtionalVelocity;
	}
}
