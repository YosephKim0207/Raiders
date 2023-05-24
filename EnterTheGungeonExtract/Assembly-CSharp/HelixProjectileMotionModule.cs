using System;
using UnityEngine;

// Token: 0x0200164B RID: 5707
public class HelixProjectileMotionModule : ProjectileAndBeamMotionModule
{
	// Token: 0x06008544 RID: 34116 RVA: 0x0036FA68 File Offset: 0x0036DC68
	public override void UpdateDataOnBounce(float angleDiff)
	{
		if (!float.IsNaN(angleDiff))
		{
			this.m_initialUpVector = Quaternion.Euler(0f, 0f, angleDiff) * this.m_initialUpVector;
			this.m_initialRightVector = Quaternion.Euler(0f, 0f, angleDiff) * this.m_initialRightVector;
		}
	}

	// Token: 0x06008545 RID: 34117 RVA: 0x0036FAD8 File Offset: 0x0036DCD8
	public override void AdjustRightVector(float angleDiff)
	{
		if (!float.IsNaN(angleDiff))
		{
			this.m_initialUpVector = Quaternion.Euler(0f, 0f, angleDiff) * this.m_initialUpVector;
			this.m_initialRightVector = Quaternion.Euler(0f, 0f, angleDiff) * this.m_initialRightVector;
		}
	}

	// Token: 0x06008546 RID: 34118 RVA: 0x0036FB48 File Offset: 0x0036DD48
	public override void Move(Projectile source, Transform projectileTransform, tk2dBaseSprite projectileSprite, SpeculativeRigidbody specRigidbody, ref float m_timeElapsed, ref Vector2 m_currentDirection, bool Inverted, bool shouldRotate)
	{
		ProjectileData baseData = source.baseData;
		Vector2 vector = ((!projectileSprite) ? projectileTransform.position.XY() : projectileSprite.WorldCenter);
		if (!this.m_helixInitialized)
		{
			this.m_helixInitialized = true;
			this.m_initialRightVector = ((!shouldRotate) ? m_currentDirection : projectileTransform.right.XY());
			this.m_initialUpVector = ((!shouldRotate) ? (Quaternion.Euler(0f, 0f, 90f) * m_currentDirection) : projectileTransform.up);
			this.m_privateLastPosition = vector;
			this.m_displacement = 0f;
			this.m_yDisplacement = 0f;
		}
		m_timeElapsed += BraveTime.DeltaTime;
		int num = ((!(Inverted ^ this.ForceInvert)) ? 1 : (-1));
		float num2 = m_timeElapsed * baseData.speed;
		float num3 = (float)num * this.helixAmplitude * Mathf.Sin(m_timeElapsed * 3.1415927f * baseData.speed / this.helixWavelength);
		float num4 = num2 - this.m_displacement;
		float num5 = num3 - this.m_yDisplacement;
		Vector2 vector2 = this.m_privateLastPosition + this.m_initialRightVector * num4 + this.m_initialUpVector * num5;
		this.m_privateLastPosition = vector2;
		if (shouldRotate)
		{
			float num6 = (m_timeElapsed + 0.01f) * baseData.speed;
			float num7 = (float)num * this.helixAmplitude * Mathf.Sin((m_timeElapsed + 0.01f) * 3.1415927f * baseData.speed / this.helixWavelength);
			float num8 = BraveMathCollege.Atan2Degrees(num7 - num3, num6 - num2);
			projectileTransform.localRotation = Quaternion.Euler(0f, 0f, num8 + this.m_initialRightVector.ToAngle());
		}
		Vector2 vector3 = (vector2 - vector) / BraveTime.DeltaTime;
		float num9 = BraveMathCollege.Atan2Degrees(vector3);
		if (!float.IsNaN(num9))
		{
			m_currentDirection = vector3.normalized;
		}
		this.m_displacement = num2;
		this.m_yDisplacement = num3;
		specRigidbody.Velocity = vector3;
	}

	// Token: 0x06008547 RID: 34119 RVA: 0x0036FD7C File Offset: 0x0036DF7C
	public override void SentInDirection(ProjectileData baseData, Transform projectileTransform, tk2dBaseSprite projectileSprite, SpeculativeRigidbody specRigidbody, ref float m_timeElapsed, ref Vector2 m_currentDirection, bool shouldRotate, Vector2 dirVec, bool resetDistance, bool updateRotation)
	{
		Vector2 vector = ((!projectileSprite) ? projectileTransform.position.XY() : projectileSprite.WorldCenter);
		this.m_helixInitialized = true;
		this.m_initialRightVector = ((!shouldRotate) ? m_currentDirection : projectileTransform.right.XY());
		this.m_initialUpVector = ((!shouldRotate) ? (Quaternion.Euler(0f, 0f, 90f) * m_currentDirection) : projectileTransform.up);
		this.m_privateLastPosition = vector;
		this.m_displacement = 0f;
		this.m_yDisplacement = 0f;
		m_timeElapsed = 0f;
	}

	// Token: 0x06008548 RID: 34120 RVA: 0x0036FE40 File Offset: 0x0036E040
	public override Vector2 GetBoneOffset(BasicBeamController.BeamBone bone, BeamController sourceBeam, bool inverted)
	{
		int num = ((!(inverted ^ this.ForceInvert)) ? 1 : (-1));
		float num2 = bone.PosX - this.helixBeamOffsetPerSecond * (Time.timeSinceLevelLoad % 600000f);
		float num3 = (float)num * this.helixAmplitude * Mathf.Sin(num2 * 3.1415927f / this.helixWavelength);
		return BraveMathCollege.DegreesToVector(bone.RotationAngle + 90f, Mathf.SmoothStep(0f, num3, bone.PosX));
	}

	// Token: 0x0400896E RID: 35182
	public float helixWavelength = 3f;

	// Token: 0x0400896F RID: 35183
	public float helixAmplitude = 1f;

	// Token: 0x04008970 RID: 35184
	public float helixBeamOffsetPerSecond = 6f;

	// Token: 0x04008971 RID: 35185
	public bool ForceInvert;

	// Token: 0x04008972 RID: 35186
	private bool m_helixInitialized;

	// Token: 0x04008973 RID: 35187
	private Vector2 m_initialRightVector;

	// Token: 0x04008974 RID: 35188
	private Vector2 m_initialUpVector;

	// Token: 0x04008975 RID: 35189
	private Vector2 m_privateLastPosition;

	// Token: 0x04008976 RID: 35190
	private float m_displacement;

	// Token: 0x04008977 RID: 35191
	private float m_yDisplacement;
}
