using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200107A RID: 4218
[RequireComponent(typeof(GenericIntroDoer))]
public class TankTreaderIntroDoer : SpecificIntroDoer
{
	// Token: 0x06005CD2 RID: 23762 RVA: 0x00238E00 File Offset: 0x00237000
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x06005CD3 RID: 23763 RVA: 0x00238E08 File Offset: 0x00237008
	public override void PlayerWalkedIn(PlayerController player, List<tk2dSpriteAnimator> animators)
	{
		this.mainGun.enabled = false;
		this.mainGun.aiAnimator.LockFacingDirection = true;
		this.mainGun.aiAnimator.FacingDirection = -90f;
		this.mainGun.aiAnimator.Update();
		base.aiAnimator.LockFacingDirection = true;
		base.aiAnimator.FacingDirection = -90f;
		base.aiAnimator.Update();
		this.m_exhaustParticleSystems = base.GetComponentsInChildren<ParticleSystem>();
		foreach (ParticleSystem particleSystem in this.m_exhaustParticleSystems)
		{
			BraveUtility.EnableEmission(particleSystem, false);
			particleSystem.Clear();
			particleSystem.GetComponent<Renderer>().enabled = false;
		}
	}

	// Token: 0x06005CD4 RID: 23764 RVA: 0x00238EC4 File Offset: 0x002370C4
	public override void StartIntro(List<tk2dSpriteAnimator> animators)
	{
		animators.Add(this.guy.spriteAnimator);
		animators.Add(this.hatch);
		base.StartCoroutine(this.DoIntro());
		AkSoundEngine.PostEvent("Play_BOSS_tank_idle_01", base.gameObject);
	}

	// Token: 0x06005CD5 RID: 23765 RVA: 0x00238F04 File Offset: 0x00237104
	public override void OnCleanup()
	{
		this.mainGun.enabled = true;
		this.mainGun.aiAnimator.LockFacingDirection = false;
		this.guy.EndAnimationIf("intro");
		this.hatch.Play("hatch_closed");
		foreach (TankTreaderMiniTurretController tankTreaderMiniTurretController in base.GetComponentsInChildren<TankTreaderMiniTurretController>())
		{
			tankTreaderMiniTurretController.enabled = true;
		}
		foreach (ParticleSystem particleSystem in this.m_exhaustParticleSystems)
		{
			BraveUtility.EnableEmission(particleSystem, true);
			particleSystem.GetComponent<Renderer>().enabled = true;
		}
	}

	// Token: 0x17000DA5 RID: 3493
	// (get) Token: 0x06005CD6 RID: 23766 RVA: 0x00238FB0 File Offset: 0x002371B0
	public override bool IsIntroFinished
	{
		get
		{
			return this.m_finished;
		}
	}

	// Token: 0x06005CD7 RID: 23767 RVA: 0x00238FB8 File Offset: 0x002371B8
	public override void OnBossCard()
	{
	}

	// Token: 0x06005CD8 RID: 23768 RVA: 0x00238FBC File Offset: 0x002371BC
	public override void EndIntro()
	{
	}

	// Token: 0x06005CD9 RID: 23769 RVA: 0x00238FC0 File Offset: 0x002371C0
	private IEnumerator DoIntro()
	{
		this.hatch.Play("hatch_intro");
		float elapsed = 0f;
		float duration = 0.2f;
		while (elapsed < duration)
		{
			yield return null;
			elapsed += GameManager.INVARIANT_DELTA_TIME;
		}
		this.guy.gameObject.SetActive(true);
		this.guy.PlayUntilCancelled("intro", false, null, -1f, false);
		elapsed = 0f;
		duration = 2.4f;
		while (elapsed < duration)
		{
			yield return null;
			elapsed += GameManager.INVARIANT_DELTA_TIME;
		}
		this.m_finished = true;
		yield break;
	}

	// Token: 0x04005691 RID: 22161
	public BodyPartController mainGun;

	// Token: 0x04005692 RID: 22162
	public AIAnimator guy;

	// Token: 0x04005693 RID: 22163
	public tk2dSpriteAnimator hatch;

	// Token: 0x04005694 RID: 22164
	private bool m_finished;

	// Token: 0x04005695 RID: 22165
	private ParticleSystem[] m_exhaustParticleSystems;
}
