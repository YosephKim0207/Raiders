using System;
using UnityEngine;

// Token: 0x02001613 RID: 5651
public abstract class ProjectileMotionModule
{
	// Token: 0x060083BA RID: 33722
	public abstract void UpdateDataOnBounce(float angleDiff);

	// Token: 0x060083BB RID: 33723
	public abstract void Move(Projectile source, Transform projectileTransform, tk2dBaseSprite projectileSprite, SpeculativeRigidbody specRigidbody, ref float m_timeElapsed, ref Vector2 m_currentDirection, bool Inverted, bool shouldRotate);

	// Token: 0x060083BC RID: 33724 RVA: 0x0035F9F4 File Offset: 0x0035DBF4
	public virtual void AdjustRightVector(float angleDiff)
	{
	}

	// Token: 0x060083BD RID: 33725 RVA: 0x0035F9F8 File Offset: 0x0035DBF8
	public virtual void SentInDirection(ProjectileData baseData, Transform projectileTransform, tk2dBaseSprite projectileSprite, SpeculativeRigidbody specRigidbody, ref float m_timeElapsed, ref Vector2 m_currentDirection, bool shouldRotate, Vector2 dirVec, bool resetDistance, bool updateRotation)
	{
	}
}
