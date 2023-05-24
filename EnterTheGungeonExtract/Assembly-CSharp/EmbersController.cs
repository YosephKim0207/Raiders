using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001294 RID: 4756
[ExecuteInEditMode]
public class EmbersController : MonoBehaviour
{
	// Token: 0x06006A70 RID: 27248 RVA: 0x0029BAFC File Offset: 0x00299CFC
	private void Start()
	{
		this.m_system = base.GetComponent<ParticleSystem>();
		if (this.m_particles == null)
		{
			this.m_particles = new ParticleSystem.Particle[this.m_system.maxParticles];
		}
	}

	// Token: 0x06006A71 RID: 27249 RVA: 0x0029BB2C File Offset: 0x00299D2C
	private void Update()
	{
		this.ProcessParticles();
	}

	// Token: 0x06006A72 RID: 27250 RVA: 0x0029BB34 File Offset: 0x00299D34
	private void ProcessVortex(int particleIndex, Vector2 particlePos, Vector2 vortex, float vortexScale, float speed)
	{
		float num = particlePos.x - vortex.x;
		float num2 = particlePos.y - vortex.y;
		Vector2 vector = new Vector2(num2, -num);
		float num3 = Mathf.Clamp01(1f - vector.magnitude / vortexScale);
		Vector3 vector2 = vector.normalized.ToVector3ZUp(0f) * num3 * speed;
		ParticleSystem.Particle[] particles = this.m_particles;
		particles[particleIndex].velocity = particles[particleIndex].velocity + vector2;
	}

	// Token: 0x06006A73 RID: 27251 RVA: 0x0029BBC0 File Offset: 0x00299DC0
	private void ProcessParticles()
	{
		int particles = this.m_system.GetParticles(this.m_particles);
		float vortexScale = this.VortexScale;
		for (int i = 0; i < particles; i++)
		{
			Vector3 position = this.m_particles[i].position;
			Vector2 vector = position.XY().Quantize(2f);
			float num = position.x - vector.x;
			float num2 = position.y - vector.y;
			float num3 = Mathf.Sin(position.x + position.y);
			float num4 = vortexScale * Mathf.Lerp(0.75f, 1.75f, (Mathf.Cos(position.x + position.y) + 1f) / 2f);
			float num5 = this.VortexSpeed * Mathf.Lerp(0.75f, 1.75f, (num3 + 1f) / 2f);
			if (num3 > 0f)
			{
				num5 *= -1f;
			}
			float num6 = -num2 * num5;
			float num7 = num * num5;
			float num8 = 1f / (1f + (num * num + num2 * num2) / num4);
			Vector3 vector2 = new Vector3(num6 - this.m_particles[i].velocity.x, num7 - this.m_particles[i].velocity.y, 0f) * num8;
			ParticleSystem.Particle[] particles2 = this.m_particles;
			int num9 = i;
			particles2[num9].velocity = particles2[num9].velocity + vector2;
			if (this.AdditionalVortices.Count != 0)
			{
				for (int j = 0; j < this.AdditionalVortices.Count; j++)
				{
					this.ProcessVortex(i, position, new Vector2(this.AdditionalVortices[j].x, this.AdditionalVortices[j].y), this.AdditionalVortices[j].z, this.AdditionalVortices[j].w);
				}
			}
		}
		this.m_system.SetParticles(this.m_particles, particles);
	}

	// Token: 0x040066F2 RID: 26354
	private ParticleSystem m_system;

	// Token: 0x040066F3 RID: 26355
	private ParticleSystem.Particle[] m_particles;

	// Token: 0x040066F4 RID: 26356
	[NonSerialized]
	public List<Vector4> AdditionalVortices = new List<Vector4>();

	// Token: 0x040066F5 RID: 26357
	public float VortexScale = 1.5f;

	// Token: 0x040066F6 RID: 26358
	public float VortexSpeed = 2f;
}
