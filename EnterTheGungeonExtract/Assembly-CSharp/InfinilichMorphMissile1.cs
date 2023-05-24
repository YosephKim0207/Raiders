using System;
using System.Collections;
using Brave.BulletScript;
using FullInspector;
using UnityEngine;

// Token: 0x02000205 RID: 517
[InspectorDropdownName("Bosses/Infinilich/MorphMissile1")]
public class InfinilichMorphMissile1 : Script
{
	// Token: 0x060007B4 RID: 1972 RVA: 0x000254F0 File Offset: 0x000236F0
	protected override IEnumerator Top()
	{
		float num = BraveMathCollege.ClampAngle180(base.BulletBank.aiAnimator.FacingDirection);
		this.m_sign = (float)((num > 90f || num < -90f) ? (-1) : 1);
		Vector2 vector = base.Position + new Vector2(this.m_sign * 2.5f, 0.5f);
		for (int i = 1; i <= 37; i++)
		{
			string text = "morph bullet " + i;
			bool flag = Array.IndexOf<int>((this.m_sign <= 0f) ? InfinilichMorphMissile1.LeftBoosters : InfinilichMorphMissile1.RightBoosters, i) >= 0;
			Vector2 vector2 = this.BulletManager.TransformOffset(Vector2.zero, text);
			base.Fire(new Offset(text), new Direction((float)((this.m_sign <= 0f) ? 180 : 0), DirectionType.Absolute, -1f), new Speed(5f, SpeedType.Absolute), new InfinilichMorphMissile1.MissileBullet(vector - vector2, this.m_sign, flag));
		}
		return null;
	}

	// Token: 0x0400079A RID: 1946
	private const float EnemyBulletSpeedItem = 5f;

	// Token: 0x0400079B RID: 1947
	private static int[] RightBoosters = new int[] { 1, 2, 4, 7, 15, 24, 32, 35, 37 };

	// Token: 0x0400079C RID: 1948
	private static int[] LeftBoosters = new int[] { 1, 3, 6, 14, 23, 31, 34, 36, 37 };

	// Token: 0x0400079D RID: 1949
	private float m_sign;

	// Token: 0x02000206 RID: 518
	public class MissileBullet : Bullet
	{
		// Token: 0x060007B6 RID: 1974 RVA: 0x00025644 File Offset: 0x00023844
		public MissileBullet(Vector2 centerOfMassOffset, float sign, bool isBooster)
			: base(null, false, false, false)
		{
			this.m_centerOfMassOffset = centerOfMassOffset;
			this.m_sign = sign;
			this.m_isBooster = isBooster;
		}

		// Token: 0x060007B7 RID: 1975 RVA: 0x00025668 File Offset: 0x00023868
		protected override IEnumerator Top()
		{
			this.Speed *= 3f;
			for (int i = 0; i < 60; i++)
			{
				if (this.m_isBooster && UnityEngine.Random.value < 0.12f)
				{
					base.Fire(new Direction(UnityEngine.Random.Range(150f, 210f), DirectionType.Relative, -1f), new Speed(6f, SpeedType.Absolute), null);
				}
				yield return base.Wait(1);
			}
			this.Speed /= 3f;
			Vector2 centerOfMass = base.Position + this.m_centerOfMassOffset;
			float directionOffset = (float)((this.m_sign >= 0f) ? 0 : (-180));
			base.ManualControl = true;
			for (int j = 0; j < 150; j++)
			{
				float desiredDirection = (this.BulletManager.PlayerPosition() - centerOfMass).ToAngle();
				if (j <= 90 || BraveMathCollege.AbsAngleBetween(desiredDirection, this.Direction) >= 3f)
				{
					float maxDelta = Mathf.SmoothStep(0f, 2.5f, (float)j / 120f);
					this.Direction = Mathf.MoveTowardsAngle(this.Direction, desiredDirection, maxDelta);
					base.UpdateVelocity();
					centerOfMass += this.Velocity / 60f;
					base.Position = centerOfMass + (Quaternion.Euler(0f, 0f, this.Direction + directionOffset) * -this.m_centerOfMassOffset).XY();
					if (this.m_isBooster && UnityEngine.Random.value < 0.04f)
					{
						base.Fire(new Direction(UnityEngine.Random.Range(150f, 210f), DirectionType.Relative, -1f), new Speed(6f, SpeedType.Absolute), null);
					}
					yield return base.Wait(1);
				}
			}
			base.ManualControl = false;
			for (int k = 0; k < 240; k++)
			{
				this.Speed += 0.2f;
				if (this.m_isBooster && UnityEngine.Random.value < 0.12f)
				{
					base.Fire(new Direction(UnityEngine.Random.Range(130f, 230f), DirectionType.Relative, -1f), new Speed(8f, SpeedType.Absolute), null);
				}
				yield return base.Wait(1);
			}
			base.Vanish(false);
			yield break;
		}

		// Token: 0x0400079E RID: 1950
		private Vector2 m_centerOfMassOffset;

		// Token: 0x0400079F RID: 1951
		private float m_sign;

		// Token: 0x040007A0 RID: 1952
		private bool m_isBooster;
	}
}
