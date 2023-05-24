using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200165E RID: 5726
public class ProjectileTrailRendererController : BraveBehaviour
{
	// Token: 0x06008591 RID: 34193 RVA: 0x003718F0 File Offset: 0x0036FAF0
	public void Awake()
	{
		this.m_previousTrailSpeed = null;
		base.projectile.TrailRendererController = this;
	}

	// Token: 0x06008592 RID: 34194 RVA: 0x00371918 File Offset: 0x0036FB18
	public void Start()
	{
		this.TryUpdateTrailLength();
	}

	// Token: 0x06008593 RID: 34195 RVA: 0x00371920 File Offset: 0x0036FB20
	public void LateUpdate()
	{
		this.TryUpdateTrailLength();
	}

	// Token: 0x06008594 RID: 34196 RVA: 0x00371928 File Offset: 0x0036FB28
	public void OnSpawned()
	{
		if (this.customTrailRenderer)
		{
			this.customTrailRenderer.Reenable();
		}
		this.TryUpdateTrailLength();
	}

	// Token: 0x06008595 RID: 34197 RVA: 0x0037194C File Offset: 0x0036FB4C
	public void OnDespawned()
	{
		this.m_previousTrailSpeed = null;
		if (this.customTrailRenderer)
		{
			this.customTrailRenderer.Clear();
			this.isStopping = false;
			base.StopAllCoroutines();
		}
	}

	// Token: 0x06008596 RID: 34198 RVA: 0x00371990 File Offset: 0x0036FB90
	public void Stop()
	{
		if (this.customTrailRenderer)
		{
			base.StartCoroutine(this.StopGracefully());
		}
	}

	// Token: 0x06008597 RID: 34199 RVA: 0x003719B0 File Offset: 0x0036FBB0
	private IEnumerator StopGracefully()
	{
		this.isStopping = true;
		float startLifetime = this.customTrailRenderer.lifeTime;
		float endLifetime = 0f;
		float timer = 0f;
		float duration = 1f;
		while (timer < duration)
		{
			this.customTrailRenderer.lifeTime = Mathf.Lerp(startLifetime, endLifetime, timer / duration);
			yield return null;
			timer += BraveTime.DeltaTime;
		}
		this.customTrailRenderer.lifeTime = endLifetime;
		this.customTrailRenderer.emit = false;
		yield break;
	}

	// Token: 0x06008598 RID: 34200 RVA: 0x003719CC File Offset: 0x0036FBCC
	private void TryUpdateTrailLength()
	{
		if (this.isStopping)
		{
			return;
		}
		float? num = null;
		if (num == null && base.projectile.braveBulletScript && base.projectile.braveBulletScript.bullet != null && !base.projectile.braveBulletScript.bullet.ManualControl)
		{
			num = new float?(base.projectile.braveBulletScript.bullet.Speed);
		}
		if (num == null && base.specRigidbody)
		{
			num = new float?(base.specRigidbody.Velocity.magnitude);
		}
		if (num == null)
		{
			return;
		}
		float? previousTrailSpeed = this.m_previousTrailSpeed;
		if (previousTrailSpeed == null || num != this.m_previousTrailSpeed.Value)
		{
			this.m_previousTrailSpeed = num;
			if (this.trailRenderer)
			{
				this.trailRenderer.time = ((!(num == 0f)) ? (this.desiredLength / num.Value) : this.desiredLength);
			}
			if (this.customTrailRenderer)
			{
				this.customTrailRenderer.lifeTime = ((!(num == 0f)) ? (this.desiredLength / num.Value) : this.desiredLength);
			}
		}
	}

	// Token: 0x040089CF RID: 35279
	public TrailRenderer trailRenderer;

	// Token: 0x040089D0 RID: 35280
	public CustomTrailRenderer customTrailRenderer;

	// Token: 0x040089D1 RID: 35281
	public float desiredLength;

	// Token: 0x040089D2 RID: 35282
	private float? m_previousTrailSpeed;

	// Token: 0x040089D3 RID: 35283
	private bool isStopping;
}
