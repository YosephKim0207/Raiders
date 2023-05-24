using System;
using UnityEngine;

// Token: 0x02001010 RID: 4112
public class DimensionFogController : BraveBehaviour
{
	// Token: 0x17000D05 RID: 3333
	// (get) Token: 0x06005A01 RID: 23041 RVA: 0x00226408 File Offset: 0x00224608
	public float ApparentRadius
	{
		get
		{
			return (this.m_state != DimensionFogController.State.Growing) ? this.radius : Mathf.Max(0f, this.radius - 6f);
		}
	}

	// Token: 0x06005A02 RID: 23042 RVA: 0x00226438 File Offset: 0x00224638
	public void Start()
	{
		BraveUtility.EnableEmission(this.particleSystem, false);
		BraveUtility.EnableEmission(this.bitsParticleSystem, false);
	}

	// Token: 0x06005A03 RID: 23043 RVA: 0x00226454 File Offset: 0x00224654
	public void Update()
	{
		if (this.m_state == DimensionFogController.State.Growing)
		{
			this.radius = Mathf.MoveTowards(this.radius, this.targetRadius, this.growSpeed * BraveTime.DeltaTime);
			if (this.radius >= this.targetRadius)
			{
				this.targetRadius = 0f;
				this.m_state = DimensionFogController.State.Contracting;
			}
		}
		else if (this.m_state == DimensionFogController.State.Contracting)
		{
			this.radius = Mathf.MoveTowards(this.radius, this.minRadius, this.contractSpeed * BraveTime.DeltaTime);
			if (this.radius <= this.minRadius)
			{
				this.radius = 0f;
				this.targetRadius = 0f;
				this.m_state = DimensionFogController.State.Stable;
			}
		}
		else if (this.m_state == DimensionFogController.State.Stable && this.targetRadius > 0f)
		{
			this.radius = this.minRadius;
			this.m_state = DimensionFogController.State.Growing;
		}
		this.UpdateParticleSystem();
		this.UpdateBitsParticleSystem();
	}

	// Token: 0x06005A04 RID: 23044 RVA: 0x00226554 File Offset: 0x00224754
	private void UpdateParticleSystem()
	{
		float num = 3.1415927f * this.radius * this.radius;
		float num2 = this.emissionRatePerArea * num;
		BraveUtility.SetEmissionRate(this.particleSystem, num2);
		this.particleSystem.startSpeed = this.speedPerRadius * this.radius;
		Vector3 vector = Quaternion.Euler(0f, 0f, (float)UnityEngine.Random.Range(0, 360)) * new Vector3(this.radius, 0f);
		ParticleSystem.EmitParams emitParams = new ParticleSystem.EmitParams
		{
			position = vector,
			velocity = this.particleSystem.startSpeed * -vector.normalized,
			startSize = this.particleSystem.startSize,
			startLifetime = this.particleSystem.startLifetime,
			startColor = this.particleSystem.startColor
		};
		this.particleSystem.Emit(emitParams, (int)(BraveTime.DeltaTime * num2));
	}

	// Token: 0x06005A05 RID: 23045 RVA: 0x00226658 File Offset: 0x00224858
	private void UpdateBitsParticleSystem()
	{
		if (!this.bitsParticleSystem)
		{
			return;
		}
		float num = this.bitsEmissionRatePerRadius * this.radius;
		BraveUtility.SetEmissionRate(this.bitsParticleSystem, num);
		Vector3 vector = Quaternion.Euler(0f, 0f, (float)UnityEngine.Random.Range(0, 360)) * new Vector3(this.radius, 0f);
		ParticleSystem.EmitParams emitParams = new ParticleSystem.EmitParams
		{
			position = vector,
			velocity = this.bitsParticleSystem.startSpeed * vector.normalized,
			startSize = this.bitsParticleSystem.startSize,
			startLifetime = this.bitsParticleSystem.startLifetime,
			startColor = this.bitsParticleSystem.startColor
		};
		this.particleSystem.Emit(emitParams, (int)(BraveTime.DeltaTime * num));
	}

	// Token: 0x04005364 RID: 21348
	public float radius;

	// Token: 0x04005365 RID: 21349
	public float minRadius = 4f;

	// Token: 0x04005366 RID: 21350
	public float growSpeed = 8f;

	// Token: 0x04005367 RID: 21351
	public float contractSpeed = 1f;

	// Token: 0x04005368 RID: 21352
	public float targetRadius;

	// Token: 0x04005369 RID: 21353
	[Header("Main Particle System")]
	public new ParticleSystem particleSystem;

	// Token: 0x0400536A RID: 21354
	public float emissionRatePerArea = 0.2f;

	// Token: 0x0400536B RID: 21355
	public float speedPerRadius = 0.33f;

	// Token: 0x0400536C RID: 21356
	[Header("Bits Particle System")]
	public ParticleSystem bitsParticleSystem;

	// Token: 0x0400536D RID: 21357
	public float bitsEmissionRatePerRadius = 5f;

	// Token: 0x0400536E RID: 21358
	private DimensionFogController.State m_state = DimensionFogController.State.Contracting;

	// Token: 0x02001011 RID: 4113
	private enum State
	{
		// Token: 0x04005370 RID: 21360
		Growing,
		// Token: 0x04005371 RID: 21361
		Contracting,
		// Token: 0x04005372 RID: 21362
		Stable
	}
}
