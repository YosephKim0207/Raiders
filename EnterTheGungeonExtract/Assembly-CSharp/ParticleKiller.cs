using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020015AF RID: 5551
public class ParticleKiller : MonoBehaviour
{
	// Token: 0x06007F58 RID: 32600 RVA: 0x003369EC File Offset: 0x00334BEC
	public void Awake()
	{
		if (base.transform.parent == null)
		{
			return;
		}
		this.ForceInit();
	}

	// Token: 0x06007F59 RID: 32601 RVA: 0x00336A0C File Offset: 0x00334C0C
	public void ForceInit()
	{
		this.m_particleSystem = base.GetComponent<ParticleSystem>();
		this.m_parentTransform = base.transform.parent;
		if (base.transform.parent)
		{
			DebrisObject component = base.transform.parent.GetComponent<DebrisObject>();
			if (component)
			{
				this.m_debrisParent = component;
				if (component.detachedParticleSystems == null)
				{
					component.detachedParticleSystems = new List<ParticleSystem>();
				}
				component.detachedParticleSystems.Add(this.m_particleSystem);
			}
		}
		if (this.destroyAfterStartLifetime)
		{
			base.StartCoroutine(this.TimedDespawn(this.m_particleSystem.startLifetime));
		}
		if (this.destroyAfterTimer)
		{
			base.StartCoroutine(this.TimedDespawn(this.destroyTimer));
		}
		if (this.deparent)
		{
			base.transform.parent = SpawnManager.Instance.VFX;
		}
		if (this.transferToSubEmitter)
		{
			base.StartCoroutine(this.TimedTransferToSubEmitter(this.transferToSubEmitterTimer));
		}
	}

	// Token: 0x06007F5A RID: 32602 RVA: 0x00336B14 File Offset: 0x00334D14
	private void Start()
	{
		if (this.m_debrisParent && this.disableEmitterOnParentGrounded)
		{
			DebrisObject debrisParent = this.m_debrisParent;
			debrisParent.OnGrounded = (Action<DebrisObject>)Delegate.Combine(debrisParent.OnGrounded, new Action<DebrisObject>(this.DisableOnParentGrounded));
		}
	}

	// Token: 0x06007F5B RID: 32603 RVA: 0x00336B64 File Offset: 0x00334D64
	public void Update()
	{
		if (this.deparent)
		{
			if (this.m_parentTransform)
			{
				if (this.parentPosition)
				{
					if (this.parentPositionYDepth)
					{
						base.transform.position = this.m_parentTransform.position.WithZ(this.m_parentTransform.position.y);
					}
					else
					{
						base.transform.position = this.m_parentTransform.position;
					}
				}
				if (this.parentRotation)
				{
					base.transform.rotation = this.m_parentTransform.rotation;
				}
				if (this.parentScale)
				{
					base.transform.localScale = this.m_parentTransform.localScale;
				}
			}
			else
			{
				BraveUtility.EnableEmission(this.m_particleSystem, false);
			}
		}
		if (this.destroyOnNoParticlesRemaining && this.m_particleSystem)
		{
			if (this.m_particleSystem.particleCount == 0)
			{
				this.m_noParticleCounter++;
				if (this.m_noParticleCounter >= this.c_framesOfNoParticlesToDestroy)
				{
					SpawnManager.Despawn(base.gameObject);
				}
			}
			else
			{
				this.m_noParticleCounter = 0;
			}
		}
	}

	// Token: 0x06007F5C RID: 32604 RVA: 0x00336CA0 File Offset: 0x00334EA0
	public void StopEmitting()
	{
		BraveUtility.EnableEmission(this.m_particleSystem, false);
	}

	// Token: 0x06007F5D RID: 32605 RVA: 0x00336CB0 File Offset: 0x00334EB0
	private IEnumerator TimedDespawn(float t)
	{
		yield return new WaitForSeconds(t);
		SpawnManager.Despawn(base.gameObject);
		yield break;
	}

	// Token: 0x06007F5E RID: 32606 RVA: 0x00336CD4 File Offset: 0x00334ED4
	private void DisableOnParentGrounded(DebrisObject obj)
	{
		BraveUtility.EnableEmission(this.m_particleSystem, false);
	}

	// Token: 0x06007F5F RID: 32607 RVA: 0x00336CE4 File Offset: 0x00334EE4
	private IEnumerator TimedTransferToSubEmitter(float t)
	{
		yield return new WaitForSeconds(t);
		this.TransferToSubEmitter();
		yield break;
	}

	// Token: 0x06007F60 RID: 32608 RVA: 0x00336D08 File Offset: 0x00334F08
	private void TransferToSubEmitter()
	{
		ParticleSystem.Particle[] array = new ParticleSystem.Particle[this.m_particleSystem.particleCount];
		int particles = this.m_particleSystem.GetParticles(array);
		for (int i = 0; i < particles; i++)
		{
			ParticleSystem.Particle particle = array[i];
			ParticleSystem.EmitParams emitParams = new ParticleSystem.EmitParams
			{
				position = particle.position,
				rotation = particle.rotation,
				startSize = particle.size,
				startColor = particle.color
			};
			this.subEmitter.Emit(emitParams, 1);
		}
		this.m_particleSystem.Clear(false);
		this.m_particleSystem.Stop();
	}

	// Token: 0x040081FD RID: 33277
	public bool deparent;

	// Token: 0x040081FE RID: 33278
	[ShowInInspectorIf("deparent", true)]
	public bool parentPosition = true;

	// Token: 0x040081FF RID: 33279
	[ShowInInspectorIf("deparent", true)]
	public bool parentRotation = true;

	// Token: 0x04008200 RID: 33280
	[ShowInInspectorIf("deparent", true)]
	public bool parentScale = true;

	// Token: 0x04008201 RID: 33281
	[ShowInInspectorIf("deparent", true)]
	public bool parentPositionYDepth;

	// Token: 0x04008202 RID: 33282
	public bool destroyAfterTimer;

	// Token: 0x04008203 RID: 33283
	public float destroyTimer = 5f;

	// Token: 0x04008204 RID: 33284
	public bool disableEmitterOnParentDeath;

	// Token: 0x04008205 RID: 33285
	public bool destroyOnNoParticlesRemaining;

	// Token: 0x04008206 RID: 33286
	public bool destroyAfterStartLifetime;

	// Token: 0x04008207 RID: 33287
	public bool disableEmitterOnParentGrounded;

	// Token: 0x04008208 RID: 33288
	public bool overrideXRotation;

	// Token: 0x04008209 RID: 33289
	[ShowInInspectorIf("overrideXRotation", false)]
	public float xRotation;

	// Token: 0x0400820A RID: 33290
	[Header("Manual Subemitter")]
	public bool transferToSubEmitter;

	// Token: 0x0400820B RID: 33291
	public float transferToSubEmitterTimer;

	// Token: 0x0400820C RID: 33292
	public ParticleSystem subEmitter;

	// Token: 0x0400820D RID: 33293
	private ParticleSystem m_particleSystem;

	// Token: 0x0400820E RID: 33294
	private DebrisObject m_debrisParent;

	// Token: 0x0400820F RID: 33295
	private Transform m_parentTransform;

	// Token: 0x04008210 RID: 33296
	private int m_noParticleCounter;

	// Token: 0x04008211 RID: 33297
	private int c_framesOfNoParticlesToDestroy = 30;
}
