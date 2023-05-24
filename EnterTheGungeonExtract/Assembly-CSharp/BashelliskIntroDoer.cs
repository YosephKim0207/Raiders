using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000FC6 RID: 4038
[RequireComponent(typeof(GenericIntroDoer))]
public class BashelliskIntroDoer : SpecificIntroDoer
{
	// Token: 0x06005800 RID: 22528 RVA: 0x00219CA0 File Offset: 0x00217EA0
	private void Start()
	{
		this.m_head = base.GetComponent<BashelliskHeadController>();
	}

	// Token: 0x06005801 RID: 22529 RVA: 0x00219CB0 File Offset: 0x00217EB0
	private void Update()
	{
	}

	// Token: 0x06005802 RID: 22530 RVA: 0x00219CB4 File Offset: 0x00217EB4
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x06005803 RID: 22531 RVA: 0x00219CBC File Offset: 0x00217EBC
	public override void PlayerWalkedIn(PlayerController player, List<tk2dSpriteAnimator> animators)
	{
		AkSoundEngine.PostEvent("Play_BOSS_bashellisk_move_02", base.gameObject);
		this.m_head.aiAnimator.LockFacingDirection = true;
		this.m_head.aiAnimator.FacingDirection = -90f;
		this.m_head.aiAnimator.Update();
		animators.Add(this.m_head.spriteAnimator);
		base.GetComponent<GenericIntroDoer>().SkipFinalizeAnimation = true;
		base.StartCoroutine(this.PlayIntro());
	}

	// Token: 0x06005804 RID: 22532 RVA: 0x00219D3C File Offset: 0x00217F3C
	public override void StartIntro(List<tk2dSpriteAnimator> animators)
	{
	}

	// Token: 0x06005805 RID: 22533 RVA: 0x00219D40 File Offset: 0x00217F40
	public override void EndIntro()
	{
		base.StopAllCoroutines();
		this.m_head.specRigidbody.Reinitialize();
		this.m_head.ReinitMovementDirection = true;
	}

	// Token: 0x17000C95 RID: 3221
	// (get) Token: 0x06005806 RID: 22534 RVA: 0x00219D64 File Offset: 0x00217F64
	public override bool IsIntroFinished
	{
		get
		{
			return this.m_state == BashelliskIntroDoer.State.Finished;
		}
	}

	// Token: 0x06005807 RID: 22535 RVA: 0x00219D70 File Offset: 0x00217F70
	public override void OnCleanup()
	{
		base.behaviorSpeculator.enabled = true;
	}

	// Token: 0x06005808 RID: 22536 RVA: 0x00219D80 File Offset: 0x00217F80
	private IEnumerator PlayIntro()
	{
		this.m_state = BashelliskIntroDoer.State.Playing;
		Vector2 startPos = this.m_head.transform.position;
		this.m_head.aiAnimator.LockFacingDirection = true;
		float elapsed = 0f;
		float duration = 4f;
		while (elapsed < duration)
		{
			float angle = Mathf.Repeat(elapsed / 2.666f, 1f) * 360f - 90f;
			float r = 4f + 4f * Mathf.Cos(2f * angle * 0.017453292f);
			float drawAngle = ((angle <= 90f) ? angle : (360f - angle));
			Vector2 lastPos = this.m_head.transform.position;
			Vector2 newPos = startPos + BraveMathCollege.DegreesToVector(drawAngle, r);
			this.m_head.transform.position = newPos;
			this.m_head.aiAnimator.FacingDirection = (newPos - lastPos).ToAngle();
			this.m_head.aiAnimator.Update();
			this.m_head.OnPostRigidbodyMovement();
			yield return null;
			elapsed += GameManager.INVARIANT_DELTA_TIME;
		}
		this.m_head.aiAnimator.FacingDirection = -90f;
		this.m_head.aiAnimator.PlayUntilFinished("intro", false, null, -1f, false);
		this.m_head.aiAnimator.Update();
		elapsed = 0f;
		duration = 2f;
		while (elapsed < duration)
		{
			yield return null;
			elapsed += GameManager.INVARIANT_DELTA_TIME;
		}
		this.m_head.aiAnimator.EndAnimationIf("intro");
		this.m_state = BashelliskIntroDoer.State.Finished;
		yield break;
	}

	// Token: 0x040050FD RID: 20733
	private BashelliskIntroDoer.State m_state;

	// Token: 0x040050FE RID: 20734
	private BashelliskHeadController m_head;

	// Token: 0x02000FC7 RID: 4039
	private enum State
	{
		// Token: 0x04005100 RID: 20736
		Idle,
		// Token: 0x04005101 RID: 20737
		Playing,
		// Token: 0x04005102 RID: 20738
		Finished
	}
}
