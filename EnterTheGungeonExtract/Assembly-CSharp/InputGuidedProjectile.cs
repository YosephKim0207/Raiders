using System;
using UnityEngine;

// Token: 0x0200164E RID: 5710
public class InputGuidedProjectile : Projectile
{
	// Token: 0x06008552 RID: 34130 RVA: 0x0037034C File Offset: 0x0036E54C
	protected override void Move()
	{
		bool flag = true;
		if (this.dumbfireTime > 0f && this.m_dumbfireTimer < this.dumbfireTime)
		{
			this.m_dumbfireTimer += BraveTime.DeltaTime;
			flag = false;
		}
		if (flag && base.Owner is PlayerController)
		{
			BraveInput instanceForPlayer = BraveInput.GetInstanceForPlayer((base.Owner as PlayerController).PlayerIDX);
			Vector2 vector = Vector2.zero;
			if (instanceForPlayer.IsKeyboardAndMouse(false))
			{
				vector = (base.Owner as PlayerController).unadjustedAimPoint.XY() - base.specRigidbody.UnitCenter;
			}
			else
			{
				vector = instanceForPlayer.ActiveActions.Aim.Vector;
			}
			float num = vector.ToAngle();
			float z = base.transform.eulerAngles.z;
			float num2 = Mathf.MoveTowardsAngle(z, num, this.trackingSpeed * BraveTime.DeltaTime);
			base.transform.rotation = Quaternion.Euler(0f, 0f, num2);
		}
		base.specRigidbody.Velocity = base.transform.right * this.baseData.speed;
		base.LastVelocity = base.specRigidbody.Velocity;
	}

	// Token: 0x06008553 RID: 34131 RVA: 0x00370498 File Offset: 0x0036E698
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x0400897F RID: 35199
	[Header("Input Guiding")]
	public float trackingSpeed = 45f;

	// Token: 0x04008980 RID: 35200
	public float dumbfireTime;

	// Token: 0x04008981 RID: 35201
	private float m_dumbfireTimer;
}
