using System;
using UnityEngine;

// Token: 0x02001648 RID: 5704
public class GrenadeProjectile : Projectile
{
	// Token: 0x06008530 RID: 34096 RVA: 0x0036EC04 File Offset: 0x0036CE04
	public override void Start()
	{
		base.Start();
		this.m_currentHeight = this.startingHeight;
		this.m_current3DVelocity = (this.m_currentDirection * this.m_currentSpeed).ToVector3ZUp(0f);
	}

	// Token: 0x06008531 RID: 34097 RVA: 0x0036EC3C File Offset: 0x0036CE3C
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x06008532 RID: 34098 RVA: 0x0036EC44 File Offset: 0x0036CE44
	protected override void Move()
	{
		this.m_current3DVelocity.x = this.m_currentDirection.x;
		this.m_current3DVelocity.y = this.m_currentDirection.y;
		this.m_current3DVelocity.z = this.m_current3DVelocity.z + base.LocalDeltaTime * -10f;
		float num = this.m_currentHeight + this.m_current3DVelocity.z * base.LocalDeltaTime;
		if (num < 0f)
		{
			this.m_current3DVelocity.z = -this.m_current3DVelocity.z;
			num *= -1f;
		}
		this.m_currentHeight = num;
		this.m_currentDirection = this.m_current3DVelocity.XY();
		Vector2 vector = this.m_current3DVelocity.XY().normalized * this.m_currentSpeed;
		base.specRigidbody.Velocity = new Vector2(vector.x, vector.y + this.m_current3DVelocity.z);
		base.LastVelocity = this.m_current3DVelocity.XY();
	}

	// Token: 0x06008533 RID: 34099 RVA: 0x0036ED54 File Offset: 0x0036CF54
	protected override void DoModifyVelocity()
	{
		if (this.ModifyVelocity != null)
		{
			Vector2 vector = this.m_current3DVelocity.XY().normalized * this.m_currentSpeed;
			vector = this.ModifyVelocity(vector);
			base.specRigidbody.Velocity = new Vector2(vector.x, vector.y + this.m_current3DVelocity.z);
			if (vector.sqrMagnitude > 0f)
			{
				this.m_currentDirection = vector.normalized;
			}
		}
	}

	// Token: 0x04008948 RID: 35144
	public float startingHeight = 1f;

	// Token: 0x04008949 RID: 35145
	private float m_currentHeight;

	// Token: 0x0400894A RID: 35146
	private Vector3 m_current3DVelocity;
}
