using System;
using UnityEngine;

// Token: 0x020012B2 RID: 4786
[ExecuteInEditMode]
public class RedMatterParticleController : MonoBehaviour
{
	// Token: 0x06006B17 RID: 27415 RVA: 0x002A0BA8 File Offset: 0x0029EDA8
	private void Awake()
	{
		this.m_system = base.GetComponent<ParticleSystem>();
		if (this.m_particles == null)
		{
			this.m_particles = new ParticleSystem.Particle[this.m_system.maxParticles];
		}
	}

	// Token: 0x06006B18 RID: 27416 RVA: 0x002A0BD8 File Offset: 0x0029EDD8
	public void ProcessParticles()
	{
		int particles = this.m_system.GetParticles(this.m_particles);
		float vortexScale = this.VortexScale;
		for (int i = 0; i < particles; i++)
		{
			Vector3 position = this.m_particles[i].position;
			float num = Mathf.Lerp(0f, 1f, 1f - (this.m_particles[i].remainingLifetime - (this.m_particles[i].startLifetime - 1f)));
			float num2 = 1f - (this.m_particles[i].remainingLifetime - 0.5f) / this.m_particles[i].startLifetime;
			Vector3 vector = ((!(this.target == null)) ? ((this.target.position - position).normalized * Mathf.Lerp(this.m_particles[i].velocity.magnitude, (this.target.position - position).magnitude * 10f, num2)) : this.m_particles[i].velocity);
			this.m_particles[i].velocity = Vector3.Lerp(this.m_particles[i].velocity, vector, num);
			if ((this.target.position - position).sqrMagnitude <= 1f)
			{
				this.m_particles[i].remainingLifetime = this.m_particles[i].remainingLifetime - 0.1f;
			}
		}
		this.m_system.SetParticles(this.m_particles, particles);
	}

	// Token: 0x040067E9 RID: 26601
	private ParticleSystem m_system;

	// Token: 0x040067EA RID: 26602
	private ParticleSystem.Particle[] m_particles;

	// Token: 0x040067EB RID: 26603
	public Transform target;

	// Token: 0x040067EC RID: 26604
	public float VortexScale = 1.5f;

	// Token: 0x040067ED RID: 26605
	public float VortexSpeed = 2f;
}
