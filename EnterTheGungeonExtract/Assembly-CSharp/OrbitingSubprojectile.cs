using System;
using UnityEngine;

// Token: 0x0200165A RID: 5722
public class OrbitingSubprojectile : Projectile
{
	// Token: 0x06008588 RID: 34184 RVA: 0x003716EC File Offset: 0x0036F8EC
	public void AssignProjectile(Projectile p)
	{
		this.TargetMainProjectile = p;
	}

	// Token: 0x06008589 RID: 34185 RVA: 0x003716F8 File Offset: 0x0036F8F8
	protected override void Move()
	{
		if (!this.TargetMainProjectile)
		{
			base.Move();
			return;
		}
		this.m_elapsed += BraveTime.DeltaTime;
		float num = Mathf.Lerp(0f, 360f, this.m_elapsed % this.RotationPeriod / this.RotationPeriod);
		Vector2 vector = (Quaternion.Euler(0f, 0f, num) * Vector2.right).normalized * this.RotationRadius;
		Vector2 vector2 = this.TargetMainProjectile.specRigidbody.UnitCenter + vector - (base.specRigidbody.UnitCenter - base.transform.position.XY());
		base.specRigidbody.Velocity = (vector2 - base.transform.position.XY()) / BraveTime.DeltaTime;
		base.LastVelocity = base.specRigidbody.Velocity;
	}

	// Token: 0x0600858A RID: 34186 RVA: 0x00371804 File Offset: 0x0036FA04
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x040089BE RID: 35262
	public float RotationPeriod = 1f;

	// Token: 0x040089BF RID: 35263
	public float RotationRadius = 2f;

	// Token: 0x040089C0 RID: 35264
	[NonSerialized]
	public Projectile TargetMainProjectile;

	// Token: 0x040089C1 RID: 35265
	private float m_elapsed;
}
