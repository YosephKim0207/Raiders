using System;
using UnityEngine;

// Token: 0x0200164A RID: 5706
public class HelixProjectile : Projectile
{
	// Token: 0x06008540 RID: 34112 RVA: 0x0036F804 File Offset: 0x0036DA04
	public void AdjustRightVector(float angleDiff)
	{
		if (!float.IsNaN(angleDiff))
		{
			this.m_initialUpVector = Quaternion.Euler(0f, 0f, angleDiff) * this.m_initialUpVector;
			this.m_initialRightVector = Quaternion.Euler(0f, 0f, angleDiff) * this.m_initialRightVector;
		}
	}

	// Token: 0x06008541 RID: 34113 RVA: 0x0036F874 File Offset: 0x0036DA74
	protected override void Move()
	{
		if (!this.m_helixInitialized)
		{
			this.m_helixInitialized = true;
			this.m_initialRightVector = base.transform.right;
			this.m_initialUpVector = base.transform.up;
			this.m_privateLastPosition = base.sprite.WorldCenter;
			this.m_displacement = 0f;
			this.m_yDisplacement = 0f;
		}
		this.m_timeElapsed += BraveTime.DeltaTime;
		int num = ((!base.Inverted) ? 1 : (-1));
		float num2 = this.m_timeElapsed * this.baseData.speed;
		float num3 = (float)num * this.helixAmplitude * Mathf.Sin(this.m_timeElapsed * 3.1415927f * this.baseData.speed / this.helixWavelength);
		float num4 = num2 - this.m_displacement;
		float num5 = num3 - this.m_yDisplacement;
		Vector2 vector = this.m_privateLastPosition + this.m_initialRightVector * num4 + this.m_initialUpVector * num5;
		this.m_privateLastPosition = vector;
		Vector2 vector2 = (vector - base.sprite.WorldCenter) / BraveTime.DeltaTime;
		float num6 = BraveMathCollege.Atan2Degrees(vector2);
		if (this.shouldRotate && !float.IsNaN(num6))
		{
			base.transform.localRotation = Quaternion.Euler(0f, 0f, num6);
		}
		if (!float.IsNaN(num6))
		{
			this.m_currentDirection = vector2.normalized;
		}
		this.m_displacement = num2;
		this.m_yDisplacement = num3;
		base.specRigidbody.Velocity = vector2;
		base.LastVelocity = base.specRigidbody.Velocity;
	}

	// Token: 0x06008542 RID: 34114 RVA: 0x0036FA34 File Offset: 0x0036DC34
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x04008966 RID: 35174
	public float helixWavelength = 3f;

	// Token: 0x04008967 RID: 35175
	public float helixAmplitude = 1f;

	// Token: 0x04008968 RID: 35176
	private bool m_helixInitialized;

	// Token: 0x04008969 RID: 35177
	private Vector2 m_initialRightVector;

	// Token: 0x0400896A RID: 35178
	private Vector2 m_initialUpVector;

	// Token: 0x0400896B RID: 35179
	private Vector2 m_privateLastPosition;

	// Token: 0x0400896C RID: 35180
	private float m_displacement;

	// Token: 0x0400896D RID: 35181
	private float m_yDisplacement;
}
