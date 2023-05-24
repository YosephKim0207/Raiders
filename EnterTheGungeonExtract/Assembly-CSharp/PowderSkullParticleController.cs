using System;
using UnityEngine;

// Token: 0x020012AA RID: 4778
public class PowderSkullParticleController : BraveBehaviour
{
	// Token: 0x06006AE7 RID: 27367 RVA: 0x0029EC4C File Offset: 0x0029CE4C
	public void Start()
	{
		this.m_lastPosition = base.transform.position;
		this.m_system = base.GetComponent<ParticleSystem>();
		if (this.m_particles == null)
		{
			this.m_particles = new ParticleSystem.Particle[this.m_system.maxParticles];
		}
		if (this.RotationChild != null)
		{
			this.m_rotationChildInitialRotation = this.RotationChild.localEulerAngles.x;
		}
	}

	// Token: 0x06006AE8 RID: 27368 RVA: 0x0029ECC4 File Offset: 0x0029CEC4
	public void LateUpdate()
	{
		this.m_curPosition = base.transform.position;
		if (this.RotationChild != null && this.ParentAnimator != null)
		{
			int num = BraveMathCollege.AngleToOctant(this.ParentAnimator.FacingDirection);
			this.RotationChild.localRotation = Quaternion.Euler(this.m_rotationChildInitialRotation + (float)(num * 45), 0f, 0f);
		}
		Vector3 vector = this.m_curPosition - this.m_lastPosition;
		if (BraveTime.DeltaTime > 0f && vector != Vector3.zero)
		{
			int particles = this.m_system.GetParticles(this.m_particles);
			for (int i = 0; i < particles; i++)
			{
				ParticleSystem.Particle[] particles2 = this.m_particles;
				int num2 = i;
				particles2[num2].position = particles2[num2].position + vector * this.VelocityFraction;
			}
			this.m_system.SetParticles(this.m_particles, particles);
		}
		this.m_lastPosition = this.m_curPosition;
	}

	// Token: 0x04006782 RID: 26498
	public AIAnimator ParentAnimator;

	// Token: 0x04006783 RID: 26499
	public Transform RotationChild;

	// Token: 0x04006784 RID: 26500
	public float VelocityFraction = 0.7f;

	// Token: 0x04006785 RID: 26501
	private float m_rotationChildInitialRotation;

	// Token: 0x04006786 RID: 26502
	private ParticleSystem m_system;

	// Token: 0x04006787 RID: 26503
	private ParticleSystem.Particle[] m_particles;

	// Token: 0x04006788 RID: 26504
	private Vector3 m_curPosition;

	// Token: 0x04006789 RID: 26505
	private Vector3 m_lastPosition;
}
