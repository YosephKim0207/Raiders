using System;
using System.Collections;
using Brave.BulletScript;
using FullInspector;
using UnityEngine;

// Token: 0x02000232 RID: 562
[InspectorDropdownName("ManfredsRival/ShieldSlam1")]
public class ManfredsRivalShieldSlam1 : Script
{
	// Token: 0x06000872 RID: 2162 RVA: 0x00028DAC File Offset: 0x00026FAC
	protected override IEnumerator Top()
	{
		base.EndOnBlank = true;
		this.FireExpandingLine(new Vector2(-0.6f, -1f), new Vector2(0.6f, -1f), 10);
		this.FireExpandingLine(new Vector2(-0.7f, -1f), new Vector2(-0.8f, -0.9f), 3);
		this.FireExpandingLine(new Vector2(0.7f, -1f), new Vector2(0.8f, -0.9f), 3);
		this.FireExpandingLine(new Vector2(-0.8f, -0.9f), new Vector2(-0.8f, 0.2f), 12);
		this.FireExpandingLine(new Vector2(0.8f, -0.9f), new Vector2(0.8f, 0.2f), 12);
		this.FireExpandingLine(new Vector2(-0.8f, 0.2f), new Vector2(-0.15f, 1f), 10);
		this.FireExpandingLine(new Vector2(0.8f, 0.2f), new Vector2(0.15f, 1f), 10);
		this.FireExpandingLine(new Vector2(-0.15f, 1f), new Vector2(0.15f, 1f), 5);
		this.FireSpinningLine(new Vector2(0f, -1.5f), new Vector2(0f, 1.5f), 4);
		this.FireSpinningLine(new Vector2(-0.6f, -0.4f), new Vector2(0.6f, -0.4f), 2);
		yield return base.Wait(60);
		yield break;
	}

	// Token: 0x06000873 RID: 2163 RVA: 0x00028DC8 File Offset: 0x00026FC8
	protected void FireExpandingLine(Vector2 start, Vector2 end, int numBullets)
	{
		start *= 0.5f;
		end *= 0.5f;
		for (int i = 0; i < numBullets; i++)
		{
			Vector2 vector = Vector2.Lerp(start, end, (float)i / ((float)numBullets - 1f));
			base.Fire(new Offset(vector, 0f, string.Empty, DirectionType.Absolute), new Direction(vector.ToAngle(), DirectionType.Absolute, -1f), new ManfredsRivalShieldSlam1.ExpandingBullet(base.Position));
		}
	}

	// Token: 0x06000874 RID: 2164 RVA: 0x00028E48 File Offset: 0x00027048
	protected void FireSpinningLine(Vector2 start, Vector2 end, int numBullets)
	{
		start *= 0.5f;
		end *= 0.5f;
		float num = (this.BulletManager.PlayerPosition() - base.Position).ToAngle();
		float num2 = (float)((BraveMathCollege.AbsAngleBetween(0f, num) >= 90f) ? 1 : (-1));
		float aimDirection = base.GetAimDirection(1f, 9f);
		for (int i = 0; i < numBullets; i++)
		{
			Vector2 vector = Vector2.Lerp(start, end, (float)i / ((float)numBullets - 1f));
			base.Fire(new Offset(vector, 0f, string.Empty, DirectionType.Absolute), new Direction(aimDirection, DirectionType.Absolute, -1f), new ManfredsRivalShieldSlam1.SpinningBullet(base.Position, num2));
		}
	}

	// Token: 0x02000233 RID: 563
	public class ExpandingBullet : Bullet
	{
		// Token: 0x06000875 RID: 2165 RVA: 0x00028F10 File Offset: 0x00027110
		public ExpandingBullet(Vector2 origin)
			: base("shield", false, false, false)
		{
			this.m_origin = origin;
			base.SuppressVfx = true;
		}

		// Token: 0x06000876 RID: 2166 RVA: 0x00028F30 File Offset: 0x00027130
		protected override IEnumerator Top()
		{
			Vector2 offset = base.Position - this.m_origin;
			float multiplier = 1f;
			base.ManualControl = true;
			for (int i = 0; i < 180; i++)
			{
				multiplier += 0.3f;
				base.Position = this.m_origin + offset * multiplier;
				yield return base.Wait(1);
			}
			base.Vanish(false);
			yield break;
		}

		// Token: 0x04000870 RID: 2160
		private Vector2 m_origin;
	}

	// Token: 0x02000235 RID: 565
	public class SpinningBullet : Bullet
	{
		// Token: 0x0600087D RID: 2173 RVA: 0x0002908C File Offset: 0x0002728C
		public SpinningBullet(Vector2 origin, float rotationSign)
			: base("sword", false, false, false)
		{
			this.m_origin = origin;
			this.m_rotationSign = rotationSign;
			base.SuppressVfx = true;
		}

		// Token: 0x0600087E RID: 2174 RVA: 0x000290B4 File Offset: 0x000272B4
		protected override IEnumerator Top()
		{
			this.Speed = 9f;
			base.ManualControl = true;
			float angle = 0f;
			Vector2 centerOfMass = this.m_origin;
			Vector2 centerOfMassOffset = this.m_origin - base.Position;
			for (int i = 0; i < 120; i++)
			{
				base.UpdateVelocity();
				centerOfMass += this.Velocity / 60f;
				angle += this.m_rotationSign * 8f;
				base.Position = centerOfMass + (Quaternion.Euler(0f, 0f, angle) * centerOfMassOffset).XY();
				yield return base.Wait(1);
			}
			base.Vanish(false);
			yield break;
		}

		// Token: 0x04000878 RID: 2168
		private const float RotationSpeed = 8f;

		// Token: 0x04000879 RID: 2169
		private Vector2 m_origin;

		// Token: 0x0400087A RID: 2170
		private float m_rotationSign;
	}
}
