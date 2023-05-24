using System;
using UnityEngine;

// Token: 0x0200132F RID: 4911
[RequireComponent(typeof(ParticleSystem))]
public class InvariantParticleSystem : BraveBehaviour
{
	// Token: 0x06006F59 RID: 28505 RVA: 0x002C25A0 File Offset: 0x002C07A0
	public void Awake()
	{
		this.m_particleSystem = base.GetComponent<ParticleSystem>();
	}

	// Token: 0x06006F5A RID: 28506 RVA: 0x002C25B0 File Offset: 0x002C07B0
	public void Update()
	{
		this.m_particleSystem.Simulate(GameManager.INVARIANT_DELTA_TIME, true, false);
	}

	// Token: 0x04006EA3 RID: 28323
	private ParticleSystem m_particleSystem;
}
