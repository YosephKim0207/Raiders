using System;
using UnityEngine;

// Token: 0x02001432 RID: 5170
public class LockOnHomingModifier : BraveBehaviour
{
	// Token: 0x0600755B RID: 30043 RVA: 0x002EBD7C File Offset: 0x002E9F7C
	private void Start()
	{
		if (!this.m_projectile)
		{
			this.m_projectile = base.GetComponent<Projectile>();
			if (!this.lockOnTarget && this.m_projectile.PossibleSourceGun && this.m_projectile.PossibleSourceGun.LastLaserSightEnemy)
			{
				this.lockOnTarget = this.m_projectile.PossibleSourceGun.LastLaserSightEnemy;
			}
		}
		Projectile projectile = this.m_projectile;
		projectile.ModifyVelocity = (Func<Vector2, Vector2>)Delegate.Combine(projectile.ModifyVelocity, new Func<Vector2, Vector2>(this.ModifyVelocity));
	}

	// Token: 0x0600755C RID: 30044 RVA: 0x002EBE24 File Offset: 0x002EA024
	public void AssignTargetManually(AIActor enemy)
	{
		this.lockOnTarget = enemy;
	}

	// Token: 0x0600755D RID: 30045 RVA: 0x002EBE30 File Offset: 0x002EA030
	public void AssignProjectile(Projectile source)
	{
		this.m_projectile = source;
	}

	// Token: 0x0600755E RID: 30046 RVA: 0x002EBE3C File Offset: 0x002EA03C
	private Vector2 ModifyVelocity(Vector2 inVel)
	{
		Vector2 vector = inVel;
		if (!this.lockOnTarget)
		{
			return inVel;
		}
		Vector2 vector2 = ((!base.sprite) ? base.transform.position.XY() : base.sprite.WorldCenter);
		Vector2 vector3 = this.lockOnTarget.CenterPosition - vector2;
		AIActor aiactor = this.lockOnTarget;
		float num = vector3.sqrMagnitude;
		num = Mathf.Sqrt(num);
		if (num < this.HomingRadius && aiactor != null)
		{
			float num2 = 1f - num / this.HomingRadius;
			float num3 = vector3.ToAngle();
			float num4 = inVel.ToAngle();
			float num5 = this.AngularVelocity * num2 * this.m_projectile.LocalDeltaTime;
			float num6 = Mathf.MoveTowardsAngle(num4, num3, num5);
			if (this.m_projectile is HelixProjectile)
			{
				float num7 = num6 - num4;
				(this.m_projectile as HelixProjectile).AdjustRightVector(num7);
			}
			else
			{
				if (this.m_projectile.shouldRotate)
				{
					base.transform.rotation = Quaternion.Euler(0f, 0f, num6);
				}
				vector = BraveMathCollege.DegreesToVector(num6, inVel.magnitude);
			}
			if (this.m_projectile.OverrideMotionModule != null)
			{
				this.m_projectile.OverrideMotionModule.AdjustRightVector(num6 - num4);
			}
		}
		if (vector == Vector2.zero)
		{
			return inVel;
		}
		return vector;
	}

	// Token: 0x0600755F RID: 30047 RVA: 0x002EBFB8 File Offset: 0x002EA1B8
	protected override void OnDestroy()
	{
		if (this.m_projectile)
		{
			Projectile projectile = this.m_projectile;
			projectile.ModifyVelocity = (Func<Vector2, Vector2>)Delegate.Remove(projectile.ModifyVelocity, new Func<Vector2, Vector2>(this.ModifyVelocity));
		}
		base.OnDestroy();
	}

	// Token: 0x04007741 RID: 30529
	public float HomingRadius = 2f;

	// Token: 0x04007742 RID: 30530
	public float AngularVelocity = 180f;

	// Token: 0x04007743 RID: 30531
	public GameObject LockOnVFX;

	// Token: 0x04007744 RID: 30532
	[NonSerialized]
	public AIActor lockOnTarget;

	// Token: 0x04007745 RID: 30533
	protected Projectile m_projectile;
}
