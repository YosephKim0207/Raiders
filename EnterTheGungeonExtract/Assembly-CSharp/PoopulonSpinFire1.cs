using System;
using System.Collections;
using Brave.BulletScript;
using UnityEngine;

// Token: 0x020002C1 RID: 705
public class PoopulonSpinFire1 : Script
{
	// Token: 0x06000AD3 RID: 2771 RVA: 0x00033FE4 File Offset: 0x000321E4
	protected override IEnumerator Top()
	{
		for (int i = 0; i < 100; i++)
		{
			float angle = base.RandomAngle();
			base.Fire(new Offset(0.75f, 0f, angle, string.Empty, DirectionType.Absolute), new Direction(angle, DirectionType.Absolute, -1f), new Speed(7f, SpeedType.Absolute), new PoopulonSpinFire1.RotatingBullet(base.Position));
			yield return base.Wait(3);
		}
		yield break;
	}

	// Token: 0x04000B5D RID: 2909
	private const int NumBullets = 100;

	// Token: 0x020002C2 RID: 706
	public class RotatingBullet : Bullet
	{
		// Token: 0x06000AD4 RID: 2772 RVA: 0x00034000 File Offset: 0x00032200
		public RotatingBullet(Vector2 origin)
			: base(null, false, false, false)
		{
			this.m_origin = origin;
		}

		// Token: 0x06000AD5 RID: 2773 RVA: 0x00034014 File Offset: 0x00032214
		protected override IEnumerator Top()
		{
			Vector2 originToPos = base.Position - this.m_origin;
			float dist = originToPos.magnitude;
			float angle = originToPos.ToAngle();
			base.ManualControl = true;
			for (int i = 0; i < 300; i++)
			{
				angle -= 0.4f;
				dist += this.Speed / 60f;
				base.Position = this.m_origin + BraveMathCollege.DegreesToVector(angle, dist);
				yield return base.Wait(1);
			}
			base.Vanish(false);
			yield break;
		}

		// Token: 0x04000B5E RID: 2910
		private Vector2 m_origin;
	}
}
