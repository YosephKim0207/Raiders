using System;
using UnityEngine;

// Token: 0x0200125D RID: 4701
[ExecuteInEditMode]
public class BulletCurtainParticleController : MonoBehaviour
{
	// Token: 0x0600695D RID: 26973 RVA: 0x00293DDC File Offset: 0x00291FDC
	private void Start()
	{
		this.m_system = base.GetComponent<ParticleSystem>();
		if (this.m_particles == null)
		{
			this.m_particles = new ParticleSystem.Particle[this.m_system.maxParticles];
		}
	}

	// Token: 0x0600695E RID: 26974 RVA: 0x00293E0C File Offset: 0x0029200C
	private void Update()
	{
		this.ProcessParticles();
	}

	// Token: 0x0600695F RID: 26975 RVA: 0x00293E14 File Offset: 0x00292014
	private void ProcessParticles()
	{
		int particles = this.m_system.GetParticles(this.m_particles);
		Transform transform = this.m_system.transform;
		float num = 0f;
		if (Application.isPlaying)
		{
			num = BraveTime.DeltaTime;
		}
		for (int i = 0; i < particles; i++)
		{
			Vector3 vector = transform.TransformPoint(this.m_particles[i].position);
			vector -= this.rootTransform.position;
			float num2 = this.m_particles[i].randomSeed % 30U / 30f * 0.5f;
			if ((vector.x < this.LocalXMin - num2 || vector.x > this.LocalXMax + num2) && vector.y > 1.25f)
			{
				this.m_particles[i].velocity = this.m_particles[i].velocity.WithX(0f);
			}
			else if (vector.x > this.LocalXMin && vector.x < this.LocalXMax && vector.y < this.LocalYMax)
			{
				if (vector.x > (this.LocalXMin + this.LocalXMax) / 2f)
				{
					ParticleSystem.Particle[] particles2 = this.m_particles;
					int num3 = i;
					particles2[num3].velocity = particles2[num3].velocity + Vector3.right * num * this.AccelFactor;
				}
				else
				{
					ParticleSystem.Particle[] particles3 = this.m_particles;
					int num4 = i;
					particles3[num4].velocity = particles3[num4].velocity + Vector3.right * -1f * num * this.AccelFactor;
				}
			}
		}
		this.m_system.SetParticles(this.m_particles, particles);
	}

	// Token: 0x040065BF RID: 26047
	public float LocalXMin = 3.5f;

	// Token: 0x040065C0 RID: 26048
	public float LocalXMax = 4.5f;

	// Token: 0x040065C1 RID: 26049
	public float LocalYMax = 2.5f;

	// Token: 0x040065C2 RID: 26050
	public float AccelFactor = 1f;

	// Token: 0x040065C3 RID: 26051
	public Transform rootTransform;

	// Token: 0x040065C4 RID: 26052
	private ParticleSystem m_system;

	// Token: 0x040065C5 RID: 26053
	private ParticleSystem.Particle[] m_particles;
}
