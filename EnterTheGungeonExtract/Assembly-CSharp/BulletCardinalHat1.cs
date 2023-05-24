using System;
using System.Collections;
using Brave.BulletScript;
using UnityEngine;

// Token: 0x020000D6 RID: 214
public class BulletCardinalHat1 : Script
{
	// Token: 0x0600033E RID: 830 RVA: 0x00010910 File Offset: 0x0000EB10
	protected override IEnumerator Top()
	{
		this.FireSpinningLine(new Vector2(-0.75f, 0f), new Vector2(0.75f, 0f), 3);
		this.FireSpinningLine(new Vector2(0f, -0.75f), new Vector2(0f, 0.75f), 3);
		yield return base.Wait(60);
		yield break;
	}

	// Token: 0x0600033F RID: 831 RVA: 0x0001092C File Offset: 0x0000EB2C
	private void FireSpinningLine(Vector2 start, Vector2 end, int numBullets)
	{
		start *= 0.5f;
		end *= 0.5f;
		float num = (this.BulletManager.PlayerPosition() - base.Position).ToAngle();
		for (int i = 0; i < numBullets; i++)
		{
			Vector2 vector = Vector2.Lerp(start, end, (float)i / ((float)numBullets - 1f));
			base.Fire(new Direction(num, DirectionType.Absolute, -1f), new BulletCardinalHat1.SpinningBullet(base.Position, base.Position + vector));
		}
	}

	// Token: 0x020000D7 RID: 215
	public class SpinningBullet : Bullet
	{
		// Token: 0x06000340 RID: 832 RVA: 0x000109C0 File Offset: 0x0000EBC0
		public SpinningBullet(Vector2 origin, Vector2 startPos)
			: base("hat", false, false, false)
		{
			this.m_origin = origin;
			this.m_startPos = startPos;
			base.SuppressVfx = true;
		}

		// Token: 0x06000341 RID: 833 RVA: 0x000109E8 File Offset: 0x0000EBE8
		protected override IEnumerator Top()
		{
			base.ManualControl = true;
			Vector2 delta = (this.m_startPos - base.Position) / 45f;
			for (int i = 0; i < 45; i++)
			{
				base.Position += delta;
				yield return base.Wait(1);
			}
			this.Speed = 9f;
			float angle = 0f;
			Vector2 centerOfMass = this.m_origin;
			Vector2 centerOfMassOffset = this.m_origin - base.Position;
			for (int j = 0; j < 120; j++)
			{
				this.Direction = Mathf.MoveTowardsAngle(this.Direction, (this.BulletManager.PlayerPosition() - centerOfMass).ToAngle(), 1.5f);
				base.UpdateVelocity();
				centerOfMass += this.Velocity / 60f;
				angle += 6f;
				base.Position = centerOfMass + (Quaternion.Euler(0f, 0f, angle) * centerOfMassOffset).XY();
				yield return base.Wait(1);
			}
			base.Vanish(false);
			yield break;
		}

		// Token: 0x0400035E RID: 862
		private const float RotationSpeed = 6f;

		// Token: 0x0400035F RID: 863
		private Vector2 m_origin;

		// Token: 0x04000360 RID: 864
		private Vector2 m_startPos;
	}
}
