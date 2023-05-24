using System;
using System.Collections.Generic;
using Dungeonator;
using UnityEngine;

// Token: 0x0200164C RID: 5708
public class HomingModifier : BraveBehaviour
{
	// Token: 0x0600854A RID: 34122 RVA: 0x0036FEDC File Offset: 0x0036E0DC
	private void Start()
	{
		if (!this.m_projectile)
		{
			this.m_projectile = base.GetComponent<Projectile>();
		}
		Projectile projectile = this.m_projectile;
		projectile.ModifyVelocity = (Func<Vector2, Vector2>)Delegate.Combine(projectile.ModifyVelocity, new Func<Vector2, Vector2>(this.ModifyVelocity));
	}

	// Token: 0x0600854B RID: 34123 RVA: 0x0036FF2C File Offset: 0x0036E12C
	public void AssignProjectile(Projectile source)
	{
		this.m_projectile = source;
	}

	// Token: 0x0600854C RID: 34124 RVA: 0x0036FF38 File Offset: 0x0036E138
	private Vector2 ModifyVelocity(Vector2 inVel)
	{
		Vector2 vector = inVel;
		RoomHandler absoluteRoomFromPosition = GameManager.Instance.Dungeon.data.GetAbsoluteRoomFromPosition(this.m_projectile.LastPosition.IntXY(VectorConversions.Floor));
		List<AIActor> activeEnemies = absoluteRoomFromPosition.GetActiveEnemies(RoomHandler.ActiveEnemyType.All);
		if (activeEnemies == null || activeEnemies.Count == 0)
		{
			return inVel;
		}
		float num = float.MaxValue;
		Vector2 vector2 = Vector2.zero;
		AIActor aiactor = null;
		Vector2 vector3 = ((!base.sprite) ? base.transform.position.XY() : base.sprite.WorldCenter);
		for (int i = 0; i < activeEnemies.Count; i++)
		{
			AIActor aiactor2 = activeEnemies[i];
			if (aiactor2 && aiactor2.IsWorthShootingAt && !aiactor2.IsGone)
			{
				Vector2 vector4 = aiactor2.CenterPosition - vector3;
				float sqrMagnitude = vector4.sqrMagnitude;
				if (sqrMagnitude < num)
				{
					vector2 = vector4;
					num = sqrMagnitude;
					aiactor = aiactor2;
				}
			}
		}
		num = Mathf.Sqrt(num);
		if (num < this.HomingRadius && aiactor != null)
		{
			float num2 = 1f - num / this.HomingRadius;
			float num3 = vector2.ToAngle();
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
		if (vector == Vector2.zero || float.IsNaN(vector.x) || float.IsNaN(vector.y))
		{
			return inVel;
		}
		return vector;
	}

	// Token: 0x0600854D RID: 34125 RVA: 0x0037016C File Offset: 0x0036E36C
	protected override void OnDestroy()
	{
		if (this.m_projectile)
		{
			Projectile projectile = this.m_projectile;
			projectile.ModifyVelocity = (Func<Vector2, Vector2>)Delegate.Remove(projectile.ModifyVelocity, new Func<Vector2, Vector2>(this.ModifyVelocity));
		}
		base.OnDestroy();
	}

	// Token: 0x04008978 RID: 35192
	public float HomingRadius = 2f;

	// Token: 0x04008979 RID: 35193
	public float AngularVelocity = 180f;

	// Token: 0x0400897A RID: 35194
	protected Projectile m_projectile;
}
