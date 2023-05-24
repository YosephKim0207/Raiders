using System;
using System.Collections.Generic;
using Dungeonator;
using UnityEngine;

// Token: 0x020012A2 RID: 4770
[ExecuteInEditMode]
public class HelicopterSnowController : MonoBehaviour
{
	// Token: 0x06006ABA RID: 27322 RVA: 0x0029D98C File Offset: 0x0029BB8C
	private void Start()
	{
		this.m_system = base.GetComponent<ParticleSystem>();
		if (this.m_particles == null)
		{
			this.m_particles = new ParticleSystem.Particle[this.m_system.main.maxParticles];
		}
	}

	// Token: 0x06006ABB RID: 27323 RVA: 0x0029D9D0 File Offset: 0x0029BBD0
	private void OnEnable()
	{
		RoomHandler absoluteRoom = base.transform.position.GetAbsoluteRoom();
		if (absoluteRoom != null)
		{
			List<AIActor> activeEnemies = absoluteRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All);
			if (activeEnemies != null && activeEnemies.Count > 0)
			{
				for (int i = 0; i < activeEnemies.Count; i++)
				{
					if (activeEnemies[i] && activeEnemies[i].healthHaver && activeEnemies[i].healthHaver.IsBoss)
					{
						this.m_helicopter = activeEnemies[i];
					}
				}
			}
		}
	}

	// Token: 0x06006ABC RID: 27324 RVA: 0x0029DA70 File Offset: 0x0029BC70
	private void Update()
	{
		this.ProcessParticles();
	}

	// Token: 0x06006ABD RID: 27325 RVA: 0x0029DA78 File Offset: 0x0029BC78
	private void ProcessParticles()
	{
		int particles = this.m_system.GetParticles(this.m_particles);
		if (this.m_helicopter)
		{
			this.WorldSpaceVortexCenter = this.m_helicopter.specRigidbody.UnitCenter + new Vector2(0f, 1.5f);
		}
		float num = this.VortexRadius * this.VortexRadius;
		if (!this.m_helicopter)
		{
			num = -1f;
		}
		for (int i = 0; i < particles; i++)
		{
			Vector3 position = this.m_particles[i].position;
			Vector3 worldSpaceVortexCenter = this.WorldSpaceVortexCenter;
			float num2 = position.x - worldSpaceVortexCenter.x;
			float num3 = position.y - worldSpaceVortexCenter.y;
			float num4 = num2 * num2 + num3 * num3;
			if (num4 < num)
			{
				float vortexSpeed = this.VortexSpeed;
				float num5 = -num3 * vortexSpeed;
				float num6 = num2 * vortexSpeed;
				float num7 = 1f / (1f + num4 / num);
				Vector3 vector = new Vector3(num5, num6, 0f) * num7;
				ParticleSystem.Particle[] particles2 = this.m_particles;
				int num8 = i;
				particles2[num8].velocity = particles2[num8].velocity + vector;
			}
		}
		this.m_system.SetParticles(this.m_particles, particles);
	}

	// Token: 0x04006759 RID: 26457
	private ParticleSystem m_system;

	// Token: 0x0400675A RID: 26458
	private ParticleSystem.Particle[] m_particles;

	// Token: 0x0400675B RID: 26459
	public Vector3 WorldSpaceVortexCenter;

	// Token: 0x0400675C RID: 26460
	public float VortexRadius = 5f;

	// Token: 0x0400675D RID: 26461
	public float VortexSpeed = 5f;

	// Token: 0x0400675E RID: 26462
	private AIActor m_helicopter;
}
