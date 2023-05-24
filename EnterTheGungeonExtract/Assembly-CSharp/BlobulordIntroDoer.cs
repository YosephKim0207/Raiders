using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000FD2 RID: 4050
[RequireComponent(typeof(GenericIntroDoer))]
public class BlobulordIntroDoer : SpecificIntroDoer
{
	// Token: 0x06005852 RID: 22610 RVA: 0x0021C538 File Offset: 0x0021A738
	public void Update()
	{
		if (!this.m_initialized && base.aiActor.enabled && base.aiActor.ShadowObject)
		{
			this.m_shadowSprite = base.aiActor.ShadowObject.GetComponent<tk2dBaseSprite>();
			this.m_shadowSprite.color = this.m_shadowSprite.color.WithAlpha(0f);
			this.m_initialized = true;
		}
	}

	// Token: 0x06005853 RID: 22611 RVA: 0x0021C5B4 File Offset: 0x0021A7B4
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x06005854 RID: 22612 RVA: 0x0021C5BC File Offset: 0x0021A7BC
	public override void PlayerWalkedIn(PlayerController player, List<tk2dSpriteAnimator> animators)
	{
		base.aiAnimator.LockFacingDirection = true;
		base.aiAnimator.FacingDirection = -90f;
		base.aiAnimator.PlayUntilCancelled("preintro", false, null, -1f, false);
		if (this.m_shadowSprite)
		{
			this.m_shadowSprite.color = this.m_shadowSprite.color.WithAlpha(0f);
		}
	}

	// Token: 0x06005855 RID: 22613 RVA: 0x0021C630 File Offset: 0x0021A830
	public override void StartIntro(List<tk2dSpriteAnimator> animators)
	{
		base.aiAnimator.PlayUntilFinished("intro", false, null, -1f, false);
		base.StartCoroutine(this.DoIntro());
	}

	// Token: 0x17000CA5 RID: 3237
	// (get) Token: 0x06005856 RID: 22614 RVA: 0x0021C658 File Offset: 0x0021A858
	public override bool IsIntroFinished
	{
		get
		{
			return this.m_finished;
		}
	}

	// Token: 0x06005857 RID: 22615 RVA: 0x0021C660 File Offset: 0x0021A860
	public override void EndIntro()
	{
		this.m_finished = true;
		base.StopAllCoroutines();
		this.m_shadowSprite.color = this.m_shadowSprite.color.WithAlpha(1f);
		base.aiAnimator.LockFacingDirection = false;
		base.aiAnimator.EndAnimation();
	}

	// Token: 0x06005858 RID: 22616 RVA: 0x0021C6B4 File Offset: 0x0021A8B4
	private IEnumerator DoIntro()
	{
		float elapsed = 0f;
		float duration = 0.33f;
		while (elapsed < duration)
		{
			yield return null;
			elapsed += GameManager.INVARIANT_DELTA_TIME;
		}
		elapsed = 0f;
		duration = 0.66f;
		while (elapsed < duration)
		{
			this.m_shadowSprite.color = this.m_shadowSprite.color.WithAlpha(elapsed / duration);
			yield return null;
			elapsed += GameManager.INVARIANT_DELTA_TIME;
		}
		this.m_shadowSprite.color = this.m_shadowSprite.color.WithAlpha(1f);
		elapsed = 0f;
		duration = 4.5f;
		while (elapsed < duration)
		{
			yield return null;
			elapsed += GameManager.INVARIANT_DELTA_TIME;
		}
		this.m_finished = true;
		yield break;
	}

	// Token: 0x04005184 RID: 20868
	public Transform particleTransform;

	// Token: 0x04005185 RID: 20869
	private bool m_initialized;

	// Token: 0x04005186 RID: 20870
	private bool m_finished;

	// Token: 0x04005187 RID: 20871
	private tk2dBaseSprite m_shadowSprite;
}
