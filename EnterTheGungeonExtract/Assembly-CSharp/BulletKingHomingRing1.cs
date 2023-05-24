using System;
using System.Collections;
using Brave.BulletScript;
using FullInspector;
using UnityEngine;

// Token: 0x020000FA RID: 250
[InspectorDropdownName("Bosses/BulletKing/HomingRing1")]
public class BulletKingHomingRing1 : Script
{
	// Token: 0x060003BA RID: 954 RVA: 0x000127F0 File Offset: 0x000109F0
	protected override IEnumerator Top()
	{
		base.EndOnBlank = true;
		float startDirection = base.RandomAngle();
		float delta = 15f;
		for (int i = 0; i < 24; i++)
		{
			base.Fire(new Offset(0.0625f, 3.5625f, 0f, string.Empty, DirectionType.Absolute), new Direction(0f, DirectionType.Aim, -1f), new Speed(6f, SpeedType.Absolute), new BulletKingHomingRing1.SmokeBullet(startDirection + (float)i * delta));
		}
		yield return base.Wait(45);
		yield break;
	}

	// Token: 0x040003A7 RID: 935
	private const int NumBullets = 24;

	// Token: 0x020000FB RID: 251
	public class SmokeBullet : Bullet
	{
		// Token: 0x060003BB RID: 955 RVA: 0x0001280C File Offset: 0x00010A0C
		public SmokeBullet(float angle)
			: base("homingRing", false, false, true)
		{
			this.m_angle = angle;
		}

		// Token: 0x060003BC RID: 956 RVA: 0x00012824 File Offset: 0x00010A24
		protected override IEnumerator Top()
		{
			base.ManualControl = true;
			Vector2 centerPosition = base.Position;
			float radius = 0f;
			int i = 0;
			while ((float)i < 600f)
			{
				float desiredAngle = (this.BulletManager.PlayerPosition() - centerPosition).ToAngle();
				this.Direction = Mathf.MoveTowardsAngle(this.Direction, desiredAngle, 1f);
				float speedScale = 1f;
				if (i < 60)
				{
					speedScale = Mathf.SmoothStep(0f, 1f, (float)i / 60f);
				}
				base.UpdateVelocity();
				centerPosition += this.Velocity / 60f * speedScale;
				if (i < 60)
				{
					radius += 0.033333335f;
				}
				this.m_angle += 2f;
				base.Position = centerPosition + BraveMathCollege.DegreesToVector(this.m_angle, radius);
				yield return base.Wait(1);
				i++;
			}
			base.Vanish(false);
			yield break;
		}

		// Token: 0x040003A8 RID: 936
		private const float ExpandSpeed = 2f;

		// Token: 0x040003A9 RID: 937
		private const float SpinSpeed = 120f;

		// Token: 0x040003AA RID: 938
		private const float Lifetime = 600f;

		// Token: 0x040003AB RID: 939
		private float m_angle;
	}
}
