using System;
using UnityEngine;

// Token: 0x02001632 RID: 5682
public class ConvergeProjectile : Projectile
{
	// Token: 0x060084A5 RID: 33957 RVA: 0x0036A2C0 File Offset: 0x003684C0
	protected override void Move()
	{
		this.m_timeElapsed += BraveTime.DeltaTime;
		float num = this.convergeDistance / this.baseData.speed;
		if (this.m_timeElapsed < num)
		{
			int num2 = ((!base.Inverted) ? 1 : (-1));
			float num3 = (float)num2 * this.amplitude * 2f * 3.1415927f * (this.baseData.speed / (this.convergeDistance * 2f)) * Mathf.Cos(this.m_timeElapsed * 2f * 3.1415927f * (this.baseData.speed / (this.convergeDistance * 2f)));
			base.specRigidbody.Velocity = base.transform.right * this.baseData.speed + base.transform.up * num3;
		}
		base.LastVelocity = base.specRigidbody.Velocity;
	}

	// Token: 0x060084A6 RID: 33958 RVA: 0x0036A3C4 File Offset: 0x003685C4
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x04008855 RID: 34901
	public float convergeDistance = 10f;

	// Token: 0x04008856 RID: 34902
	public float amplitude = 1f;
}
