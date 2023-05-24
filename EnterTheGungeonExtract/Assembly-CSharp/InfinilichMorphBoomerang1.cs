using System;
using System.Collections;
using Brave.BulletScript;
using FullInspector;
using UnityEngine;

// Token: 0x020001FF RID: 511
[InspectorDropdownName("Bosses/Infinilich/MorphBoomerang1")]
public class InfinilichMorphBoomerang1 : Script
{
	// Token: 0x0600079F RID: 1951 RVA: 0x00024C70 File Offset: 0x00022E70
	protected override IEnumerator Top()
	{
		float num = BraveMathCollege.ClampAngle180(base.BulletBank.aiAnimator.FacingDirection);
		this.m_sign = (float)((num > 90f || num < -90f) ? (-1) : 1);
		Vector2 vector = base.Position + new Vector2(this.m_sign * 2.5f, 1f);
		float num2 = (this.BulletManager.PlayerPosition() - vector).ToAngle();
		for (int i = 1; i <= 43; i++)
		{
			string text = "morph bullet " + i;
			Vector2 vector2 = this.BulletManager.TransformOffset(Vector2.zero, text);
			base.Fire(new Offset(text), new Direction(num2, DirectionType.Absolute, -1f), new Speed(12f, SpeedType.Absolute), new InfinilichMorphBoomerang1.BoomerangBullet(vector - vector2, this.m_sign));
		}
		return null;
	}

	// Token: 0x04000783 RID: 1923
	private const float EnemyBulletSpeedItem = 12f;

	// Token: 0x04000784 RID: 1924
	private const float RotationSpeed = -5f;

	// Token: 0x04000785 RID: 1925
	private float m_sign;

	// Token: 0x02000200 RID: 512
	public class BoomerangBullet : Bullet
	{
		// Token: 0x060007A0 RID: 1952 RVA: 0x00024D64 File Offset: 0x00022F64
		public BoomerangBullet(Vector2 centerOfMassOffset, float sign)
			: base(null, false, false, false)
		{
			this.m_centerOfMassOffset = centerOfMassOffset;
			this.m_sign = sign;
		}

		// Token: 0x060007A1 RID: 1953 RVA: 0x00024D80 File Offset: 0x00022F80
		protected override IEnumerator Top()
		{
			Vector2 centerOfMass = base.Position + this.m_centerOfMassOffset;
			base.ManualControl = true;
			float angle = 0f;
			for (int i = 0; i < 120; i++)
			{
				this.Direction = Mathf.MoveTowardsAngle(this.Direction, (this.BulletManager.PlayerPosition() - centerOfMass).ToAngle(), 1.5f);
				base.UpdateVelocity();
				centerOfMass += this.Velocity / 60f;
				angle += this.m_sign * -5f;
				base.Position = centerOfMass + (Quaternion.Euler(0f, 0f, angle) * -this.m_centerOfMassOffset).XY();
				yield return base.Wait(1);
			}
			for (int j = 0; j < 120; j++)
			{
				Vector2 target = this.Projectile.Owner.specRigidbody.GetUnitCenter(ColliderType.HitBox);
				Vector2 toTarget = target - base.Position;
				if (toTarget.magnitude < 1f)
				{
					base.Vanish(false);
					yield break;
				}
				if ((target - centerOfMass).magnitude < 5f)
				{
					this.Direction = Mathf.MoveTowardsAngle(this.Direction, toTarget.ToAngle(), 5f);
				}
				else
				{
					this.Direction = Mathf.MoveTowardsAngle(this.Direction, (target - centerOfMass).ToAngle(), 2.5f);
					this.Speed += 0.1f;
				}
				base.UpdateVelocity();
				centerOfMass += this.Velocity / 60f;
				angle += this.m_sign * -5f;
				base.Position = centerOfMass + (Quaternion.Euler(0f, 0f, angle) * -this.m_centerOfMassOffset).XY();
				yield return base.Wait(1);
			}
			base.Vanish(false);
			yield break;
		}

		// Token: 0x04000786 RID: 1926
		private Vector2 m_centerOfMassOffset;

		// Token: 0x04000787 RID: 1927
		private float m_sign;
	}
}
