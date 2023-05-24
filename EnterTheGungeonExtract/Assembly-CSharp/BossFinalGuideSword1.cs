using System;
using System.Collections;
using Brave.BulletScript;
using FullInspector;
using UnityEngine;

// Token: 0x0200008C RID: 140
[InspectorDropdownName("Bosses/BossFinalGuide/Sword1")]
public class BossFinalGuideSword1 : Script
{
	// Token: 0x06000226 RID: 550 RVA: 0x0000ADC8 File Offset: 0x00008FC8
	protected override IEnumerator Top()
	{
		base.EndOnBlank = true;
		this.m_sign = BraveUtility.RandomSign();
		this.m_doubleSwing = BraveUtility.RandomBool();
		Vector2 leftOrigin = new Vector2(this.m_sign * 2f, -0.2f);
		this.FireLine(leftOrigin, new Vector2(this.m_sign * 3.8f, 0.2f), new Vector2(this.m_sign * 11f, 0.2f), 14);
		this.FireLine(leftOrigin, new Vector2(this.m_sign * 11.6f, -0.2f), new Vector2(this.m_sign * 11.6f, -0.2f), 2);
		this.FireLine(leftOrigin, new Vector2(this.m_sign * 3.8f, -0.6f), new Vector2(this.m_sign * 11f, -0.6f), 14);
		this.FireLine(leftOrigin, new Vector2(this.m_sign * 3.1f, -1.2f), new Vector2(this.m_sign * 3.1f, 0.8f), 4);
		this.FireLine(leftOrigin, new Vector2(this.m_sign * 2.2f, -0.2f), new Vector2(this.m_sign * 2.7f, -0.2f), 2);
		yield return base.Wait(75);
		yield break;
	}

	// Token: 0x06000227 RID: 551 RVA: 0x0000ADE4 File Offset: 0x00008FE4
	private void FireLine(Vector2 spawnPoint, Vector2 start, Vector2 end, int numBullets)
	{
		Vector2 vector = (end - start) / (float)Mathf.Max(1, numBullets - 1);
		float num = 0.33333334f;
		for (int i = 0; i < numBullets; i++)
		{
			Vector2 vector2 = ((numBullets != 1) ? (start + vector * (float)i) : end);
			float num2 = Vector2.Distance(vector2, spawnPoint) / num;
			base.Fire(new Offset(spawnPoint, 0f, string.Empty, DirectionType.Absolute), new Direction((vector2 - spawnPoint).ToAngle(), DirectionType.Absolute, -1f), new Speed(num2, SpeedType.Absolute), new BossFinalGuideSword1.SwordBullet(base.Position, this.m_sign, this.m_doubleSwing));
		}
	}

	// Token: 0x0400023D RID: 573
	private const int SetupTime = 20;

	// Token: 0x0400023E RID: 574
	private const int HoldTime = 30;

	// Token: 0x0400023F RID: 575
	private const int SwingTime = 25;

	// Token: 0x04000240 RID: 576
	private float m_sign;

	// Token: 0x04000241 RID: 577
	private bool m_doubleSwing;

	// Token: 0x0200008D RID: 141
	public class SwordBullet : Bullet
	{
		// Token: 0x06000228 RID: 552 RVA: 0x0000AE98 File Offset: 0x00009098
		public SwordBullet(Vector2 origin, float sign, bool doubleSwing)
			: base(null, false, false, false)
		{
			this.m_origin = origin;
			this.m_sign = sign;
			this.m_doubleSwing = doubleSwing;
		}

		// Token: 0x06000229 RID: 553 RVA: 0x0000AEBC File Offset: 0x000090BC
		protected override IEnumerator Top()
		{
			yield return base.Wait(20);
			float angle = (base.Position - this.m_origin).ToAngle();
			float dist = Vector2.Distance(base.Position, this.m_origin);
			this.Speed = 0f;
			yield return base.Wait(30);
			base.ManualControl = true;
			int swingtime = ((!this.m_doubleSwing) ? 25 : 100);
			float swingDegrees = (float)((!this.m_doubleSwing) ? 180 : 540);
			for (int i = 0; i < swingtime; i++)
			{
				float newAngle = angle - Mathf.SmoothStep(0f, this.m_sign * swingDegrees, (float)i / (float)swingtime);
				base.Position = this.m_origin + BraveMathCollege.DegreesToVector(newAngle, dist);
				yield return base.Wait(1);
			}
			yield return base.Wait(5);
			base.Vanish(false);
			yield break;
		}

		// Token: 0x04000242 RID: 578
		private Vector2 m_origin;

		// Token: 0x04000243 RID: 579
		private float m_sign;

		// Token: 0x04000244 RID: 580
		private bool m_doubleSwing;
	}
}
