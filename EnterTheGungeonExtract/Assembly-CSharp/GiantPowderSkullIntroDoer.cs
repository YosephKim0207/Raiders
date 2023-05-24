using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200103B RID: 4155
[RequireComponent(typeof(GenericIntroDoer))]
public class GiantPowderSkullIntroDoer : SpecificIntroDoer
{
	// Token: 0x06005B2F RID: 23343 RVA: 0x0022F1C8 File Offset: 0x0022D3C8
	public void Update()
	{
		if (!this.m_initialized && base.aiActor.ShadowObject && this.m_mainParticleSystem != null)
		{
			this.m_shadowSprite = base.aiActor.ShadowObject.GetComponent<tk2dBaseSprite>();
			SpriteOutlineManager.ToggleOutlineRenderers(base.sprite, false);
			base.aiActor.ToggleRenderers(false);
			this.m_shadowSprite.renderer.enabled = false;
			this.m_mainParticleSystem.GetComponent<Renderer>().enabled = true;
			this.m_initialized = true;
			Debug.Log("INITIALIZED!");
		}
	}

	// Token: 0x06005B30 RID: 23344 RVA: 0x0022F268 File Offset: 0x0022D468
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x06005B31 RID: 23345 RVA: 0x0022F270 File Offset: 0x0022D470
	public override void PlayerWalkedIn(PlayerController player, List<tk2dSpriteAnimator> animators)
	{
		base.StartCoroutine(this.RunParticleSystems());
		AkSoundEngine.PostEvent("Play_ENM_cannonball_intro_01", base.gameObject);
		base.aiAnimator.LockFacingDirection = true;
		base.aiAnimator.FacingDirection = -90f;
	}

	// Token: 0x06005B32 RID: 23346 RVA: 0x0022F2AC File Offset: 0x0022D4AC
	public override void StartIntro(List<tk2dSpriteAnimator> animators)
	{
		base.StartCoroutine(this.DoIntro());
	}

	// Token: 0x17000D47 RID: 3399
	// (get) Token: 0x06005B33 RID: 23347 RVA: 0x0022F2BC File Offset: 0x0022D4BC
	public override bool IsIntroFinished
	{
		get
		{
			return this.m_finished;
		}
	}

	// Token: 0x06005B34 RID: 23348 RVA: 0x0022F2C4 File Offset: 0x0022D4C4
	public override void OnBossCard()
	{
		this.m_shadowSprite.renderer.enabled = true;
	}

	// Token: 0x06005B35 RID: 23349 RVA: 0x0022F2D8 File Offset: 0x0022D4D8
	public override void EndIntro()
	{
		this.m_finished = true;
		base.StopAllCoroutines();
		SpriteOutlineManager.ToggleOutlineRenderers(base.sprite, true);
		base.aiActor.ToggleRenderers(true);
		this.m_shadowSprite.renderer.enabled = true;
		base.aiAnimator.LockFacingDirection = false;
		base.aiAnimator.EndAnimation();
		BraveUtility.SetEmissionRate(this.m_mainParticleSystem, this.m_startParticleRate);
		BraveUtility.EnableEmission(this.m_mainParticleSystem, true);
		this.m_mainParticleSystem.Play();
		BraveUtility.EnableEmission(this.m_trailParticleSystem, true);
	}

	// Token: 0x06005B36 RID: 23350 RVA: 0x0022F368 File Offset: 0x0022D568
	private IEnumerator RunParticleSystems()
	{
		PowderSkullParticleController particleController = base.aiActor.GetComponentInChildren<PowderSkullParticleController>();
		this.m_mainParticleSystem = particleController.GetComponent<ParticleSystem>();
		this.m_trailParticleSystem = particleController.RotationChild.GetComponentInChildren<ParticleSystem>();
		this.m_startParticleRate = this.m_mainParticleSystem.emission.rate.constant;
		this.m_mainParticleSystem.GetComponent<Renderer>().enabled = true;
		BraveUtility.SetEmissionRate(this.m_mainParticleSystem, 0f);
		this.m_mainParticleSystem.Clear();
		BraveUtility.EnableEmission(this.m_trailParticleSystem, false);
		this.m_trailParticleSystem.Clear();
		float t = 0f;
		float duration = 6f;
		while (!this.m_finished)
		{
			if (t < duration)
			{
				BraveUtility.SetEmissionRate(this.m_mainParticleSystem, Mathf.Lerp(0f, this.m_startParticleRate, this.emissionRate.Evaluate(t / duration)));
			}
			this.m_mainParticleSystem.Simulate(GameManager.INVARIANT_DELTA_TIME, false, false);
			yield return null;
			t += GameManager.INVARIANT_DELTA_TIME;
		}
		yield break;
	}

	// Token: 0x06005B37 RID: 23351 RVA: 0x0022F384 File Offset: 0x0022D584
	private IEnumerator DoIntro()
	{
		float elapsed = 0f;
		float duration = 2f;
		while (elapsed < duration)
		{
			yield return null;
			elapsed += GameManager.INVARIANT_DELTA_TIME;
		}
		SpriteOutlineManager.ToggleOutlineRenderers(base.sprite, true);
		base.aiActor.ToggleRenderers(true);
		this.m_shadowSprite.renderer.enabled = false;
		base.aiAnimator.PlayUntilFinished("intro", false, null, -1f, false);
		elapsed = 0f;
		duration = 4f;
		while (elapsed < duration)
		{
			yield return null;
			elapsed += GameManager.INVARIANT_DELTA_TIME;
		}
		this.m_finished = true;
		yield break;
	}

	// Token: 0x040054C7 RID: 21703
	[CurveRange(0f, 0f, 1f, 1f)]
	public AnimationCurve emissionRate;

	// Token: 0x040054C8 RID: 21704
	private bool m_initialized;

	// Token: 0x040054C9 RID: 21705
	private bool m_finished;

	// Token: 0x040054CA RID: 21706
	private ParticleSystem m_mainParticleSystem;

	// Token: 0x040054CB RID: 21707
	private ParticleSystem m_trailParticleSystem;

	// Token: 0x040054CC RID: 21708
	private float m_startParticleRate;

	// Token: 0x040054CD RID: 21709
	private tk2dBaseSprite m_shadowSprite;
}
