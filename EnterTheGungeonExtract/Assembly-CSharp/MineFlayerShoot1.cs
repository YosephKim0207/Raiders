using System;
using System.Collections;
using Brave.BulletScript;
using FullInspector;
using UnityEngine;

// Token: 0x0200029F RID: 671
[InspectorDropdownName("Bosses/MineFlayer/Shoot1")]
public class MineFlayerShoot1 : Script
{
	// Token: 0x06000A47 RID: 2631 RVA: 0x00031DC8 File Offset: 0x0002FFC8
	protected override IEnumerator Top()
	{
		float floatSign = BraveUtility.RandomSign();
		for (int i = 0; i < 5; i++)
		{
			float floatDirection = 90f + floatSign * 90f + UnityEngine.Random.Range(25f, -25f);
			Vector2 floatVelocity = BraveMathCollege.DegreesToVector(floatDirection, 4f);
			float startDirection = base.RandomAngle();
			for (int j = 0; j < 36; j++)
			{
				base.Fire(new Direction(base.SubdivideCircle(startDirection, 36, j, 1f, false), DirectionType.Absolute, -1f), new Speed(8f, SpeedType.Absolute), new MineFlayerShoot1.BurstBullet(floatVelocity));
			}
			yield return base.Wait(30);
			floatSign *= -1f;
		}
		yield break;
	}

	// Token: 0x04000AB6 RID: 2742
	private const int NumBursts = 5;

	// Token: 0x04000AB7 RID: 2743
	private const int NumBullets = 36;

	// Token: 0x020002A0 RID: 672
	public class BurstBullet : Bullet
	{
		// Token: 0x06000A48 RID: 2632 RVA: 0x00031DE4 File Offset: 0x0002FFE4
		public BurstBullet(Vector2 additionalVelocity)
			: base("burst", false, false, false)
		{
			this.m_addtionalVelocity = additionalVelocity;
		}

		// Token: 0x06000A49 RID: 2633 RVA: 0x00031DFC File Offset: 0x0002FFFC
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

		// Token: 0x04000AB8 RID: 2744
		private Vector2 m_addtionalVelocity;
	}
}
