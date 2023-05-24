using System;
using System.Collections;
using UnityEngine;

// Token: 0x02001074 RID: 4212
public class BossFinalConvictHegemonyReinforceDoer : CustomReinforceDoer
{
	// Token: 0x06005CB1 RID: 23729 RVA: 0x00237DC4 File Offset: 0x00235FC4
	public override void StartIntro()
	{
		base.StartCoroutine(this.DoIntro());
	}

	// Token: 0x06005CB2 RID: 23730 RVA: 0x00237DD4 File Offset: 0x00235FD4
	private IEnumerator DoIntro()
	{
		Vector2 spawnPos = base.transform.position;
		bool faceRight = BraveUtility.RandomBool();
		base.specRigidbody.Initialize();
		SpawnManager.SpawnVFX(this.ropeVfx, base.specRigidbody.UnitCenter + new Vector2((float)((!faceRight) ? (-1) : (-2)), 0f), Quaternion.identity);
		base.aiActor.invisibleUntilAwaken = true;
		bool cachedCollideWithOthers = base.specRigidbody.CollideWithOthers;
		base.specRigidbody.CollideWithOthers = false;
		base.aiActor.IsGone = true;
		if (base.behaviorSpeculator)
		{
			base.behaviorSpeculator.enabled = false;
		}
		base.healthHaver.IsVulnerable = false;
		base.aiActor.ToggleRenderers(false);
		if (base.aiShooter)
		{
			base.aiShooter.ToggleGunAndHandRenderers(false, "BossFinalConvictHegemonyReinforceDoer");
		}
		float elapsed = 0f;
		float duration = 0.5f;
		while (elapsed < duration)
		{
			if (base.aiShooter)
			{
				base.aiShooter.ToggleGunAndHandRenderers(false, "BossFinalConvictHegemonyReinforceDoer");
			}
			yield return null;
			elapsed += GameManager.INVARIANT_DELTA_TIME;
		}
		yield return new WaitForSeconds(0.5f);
		base.aiActor.invisibleUntilAwaken = false;
		base.aiActor.ToggleRenderers(true);
		base.aiAnimator.LockFacingDirection = true;
		base.aiAnimator.FacingDirection = (float)((!faceRight) ? 180 : 0);
		base.aiAnimator.PlayUntilCancelled("rappel", false, null, -1f, false);
		elapsed = 0f;
		duration = 2f;
		while (elapsed < duration)
		{
			base.transform.position = spawnPos + new Vector2(0f, BraveMathCollege.LinearToSmoothStepInterpolate(7f, 0f, elapsed / duration));
			if (base.aiShooter)
			{
				base.aiShooter.ToggleGunAndHandRenderers(false, "BossFinalConvictHegemonyReinforceDoer");
			}
			yield return null;
			elapsed += GameManager.INVARIANT_DELTA_TIME;
		}
		base.transform.position = spawnPos;
		base.aiAnimator.LockFacingDirection = false;
		base.aiAnimator.EndAnimationIf("rappel");
		base.specRigidbody.CollideWithOthers = cachedCollideWithOthers;
		base.aiActor.IsGone = false;
		base.aiActor.State = AIActor.ActorState.Normal;
		if (base.behaviorSpeculator)
		{
			base.behaviorSpeculator.enabled = true;
		}
		base.healthHaver.IsVulnerable = true;
		if (base.aiShooter)
		{
			base.aiShooter.ToggleGunAndHandRenderers(true, "BossFinalConvictHegemonyReinforceDoer");
		}
		base.aiActor.HasBeenEngaged = true;
		this.m_isFinished = true;
		yield break;
	}

	// Token: 0x17000D9E RID: 3486
	// (get) Token: 0x06005CB3 RID: 23731 RVA: 0x00237DF0 File Offset: 0x00235FF0
	public override bool IsFinished
	{
		get
		{
			return this.m_isFinished;
		}
	}

	// Token: 0x04005653 RID: 22099
	public GameObject ropeVfx;

	// Token: 0x04005654 RID: 22100
	private bool m_isFinished;
}
