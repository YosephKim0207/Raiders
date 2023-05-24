using System;
using UnityEngine;

// Token: 0x02001695 RID: 5781
public class WaveProjectile : Projectile
{
	// Token: 0x060086C6 RID: 34502 RVA: 0x0037DE38 File Offset: 0x0037C038
	protected override void Move()
	{
		this.m_timeElapsed += base.LocalDeltaTime;
		int num = ((!base.Inverted) ? 1 : (-1));
		float num2 = (float)num * this.amplitude * 2f * 3.1415927f * this.frequency * Mathf.Cos(this.m_timeElapsed * 2f * 3.1415927f * this.frequency);
		base.specRigidbody.Velocity = base.transform.right * this.baseData.speed + base.transform.up * num2;
	}

	// Token: 0x060086C7 RID: 34503 RVA: 0x0037DEE8 File Offset: 0x0037C0E8
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x04008BF5 RID: 35829
	public float amplitude = 1f;

	// Token: 0x04008BF6 RID: 35830
	public float frequency = 2f;
}
