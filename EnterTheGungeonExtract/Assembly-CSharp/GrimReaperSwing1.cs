using System;
using System.Collections;
using Brave.BulletScript;
using UnityEngine;

// Token: 0x020001B7 RID: 439
public class GrimReaperSwing1 : Script
{
	// Token: 0x06000689 RID: 1673 RVA: 0x0001F72C File Offset: 0x0001D92C
	protected override IEnumerator Top()
	{
		Vector2 zero = Vector2.zero;
		Vector2 vector = zero + new Vector2(-1.5f, -1.5f);
		Vector2 vector2 = new Vector2(2f, -5f);
		Vector2 vector3 = vector2 + new Vector2(-1.5f, 0.4f);
		Vector2 vector4 = new Vector2(-0.5f, -1.5f);
		Vector2 vector5 = vector4 + new Vector2(0.75f, 0.75f);
		Vector2 vector6 = new Vector2(-0.5f, 1.5f);
		Vector2 vector7 = vector6 + new Vector2(0.75f, -0.75f);
		float num = BraveMathCollege.ClampAngle180((this.BulletManager.PlayerPosition() - base.BulletBank.specRigidbody.GetUnitCenter(ColliderType.HitBox)).ToAngle());
		bool flag = num > -45f && num < 120f;
		Vector2 vector8 = base.Position + BraveMathCollege.CalculateBezierPoint(0.5f, zero, vector, vector3, vector2);
		for (int i = 0; i < 12; i++)
		{
			float num2 = (float)i / 11f;
			Vector2 vector9 = BraveMathCollege.CalculateBezierPoint(num2, zero, vector, vector3, vector2);
			if (flag)
			{
				num2 = 1f - num2;
			}
			Vector2 vector10 = BraveMathCollege.CalculateBezierPoint(num2, vector4, vector5, vector7, vector6);
			base.Fire(new Offset(vector9, 0f, string.Empty, DirectionType.Absolute), new Direction(0f, DirectionType.Aim, -1f), new Speed(8f, SpeedType.Absolute), new GrimReaperSwing1.ReaperBullet(vector8, vector10));
		}
		return null;
	}

	// Token: 0x04000660 RID: 1632
	private const int NumBullets = 12;

	// Token: 0x020001B8 RID: 440
	public class ReaperBullet : Bullet
	{
		// Token: 0x0600068A RID: 1674 RVA: 0x0001F8C0 File Offset: 0x0001DAC0
		public ReaperBullet(Vector2 phantomBulletPoint, Vector2 offset)
			: base(null, false, false, false)
		{
			this.m_phantomBulletPoint = phantomBulletPoint;
			this.m_offset = offset;
		}

		// Token: 0x0600068B RID: 1675 RVA: 0x0001F8DC File Offset: 0x0001DADC
		protected override IEnumerator Top()
		{
			base.ManualControl = true;
			yield return base.Wait(5);
			for (int i = 0; i < 180; i++)
			{
				this.Projectile.ResetDistance();
				this.Direction = Mathf.MoveTowardsAngle(this.Direction, base.GetAimDirection(this.m_phantomBulletPoint, 0f, this.Speed), 2f);
				base.UpdateVelocity();
				this.m_phantomBulletPoint += this.Velocity / 60f;
				base.Position += this.Velocity / 60f;
				float rotation = this.Velocity.ToAngle();
				Vector2 goalPos = this.m_phantomBulletPoint + this.m_offset.Rotate(rotation);
				if (i < 30)
				{
					Vector2 vector = goalPos - base.Position;
					base.Position += vector / (float)(30 - i);
				}
				else
				{
					base.Position = goalPos;
				}
				yield return base.Wait(1);
			}
			base.Vanish(false);
			yield break;
		}

		// Token: 0x04000661 RID: 1633
		private Vector2 m_phantomBulletPoint;

		// Token: 0x04000662 RID: 1634
		private Vector2 m_offset;
	}
}
