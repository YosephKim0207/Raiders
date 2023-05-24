using System;
using Dungeonator;
using UnityEngine;

// Token: 0x02001290 RID: 4752
public class CrowdOfFansSystemController : MonoBehaviour
{
	// Token: 0x06006A58 RID: 27224 RVA: 0x0029B310 File Offset: 0x00299510
	private void Start()
	{
		this.m_system = base.GetComponent<ParticleSystem>();
		if (this.m_particles == null)
		{
			this.m_particles = new ParticleSystem.Particle[this.m_system.maxParticles];
		}
		this.m_offsets = new Vector2[this.MaxFans];
		for (int i = 0; i < this.MaxFans; i++)
		{
			this.m_offsets[i] = UnityEngine.Random.insideUnitCircle * 3f;
		}
		this.m_system.Play();
	}

	// Token: 0x06006A59 RID: 27225 RVA: 0x0029B3A0 File Offset: 0x002995A0
	public void Initialize(PlayerController p)
	{
		this.m_initialized = true;
		this.Target = p;
	}

	// Token: 0x06006A5A RID: 27226 RVA: 0x0029B3B0 File Offset: 0x002995B0
	private void Update()
	{
		if (Dungeon.IsGenerating || !this.m_initialized)
		{
			return;
		}
		this.ProcessParticles();
	}

	// Token: 0x06006A5B RID: 27227 RVA: 0x0029B3D0 File Offset: 0x002995D0
	private void ProcessParticles()
	{
		int num = 10;
		if (this.m_numEmitted < num)
		{
			ParticleSystem.EmitParams emitParams = default(ParticleSystem.EmitParams);
			emitParams.position = (this.Target.CenterPosition + this.m_offsets[this.m_numEmitted]).ToVector3ZisY(0f);
			emitParams.velocity = Vector3.zero;
			emitParams.startSize = this.m_system.startSize;
			emitParams.startLifetime = this.m_system.startLifetime;
			emitParams.startColor = this.m_system.startColor;
			this.m_system.Emit(emitParams, 1);
			Debug.LogError("emitting particle");
			this.m_numEmitted++;
		}
		int particles = this.m_system.GetParticles(this.m_particles);
		for (int i = 0; i < particles; i++)
		{
			Vector3 position = this.m_particles[i].position;
			Vector3 velocity = this.m_particles[i].velocity;
			Vector3 vector = (this.Target.CenterPosition + this.m_offsets[i]).ToVector3ZisY(0f);
			this.m_particles[i].position = vector;
			this.m_particles[i].velocity = Vector3.zero;
		}
		this.m_system.SetParticles(this.m_particles, particles);
	}

	// Token: 0x040066D6 RID: 26326
	public PlayerController Target;

	// Token: 0x040066D7 RID: 26327
	public int MaxFans = 100;

	// Token: 0x040066D8 RID: 26328
	private ParticleSystem m_system;

	// Token: 0x040066D9 RID: 26329
	private ParticleSystem.Particle[] m_particles;

	// Token: 0x040066DA RID: 26330
	private Vector2[] m_offsets;

	// Token: 0x040066DB RID: 26331
	private bool m_initialized;

	// Token: 0x040066DC RID: 26332
	private int m_numEmitted;
}
