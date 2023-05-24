using System;
using System.Collections;
using System.Collections.Generic;
using Dungeonator;
using UnityEngine;

// Token: 0x02000FDB RID: 4059
[RequireComponent(typeof(GenericIntroDoer))]
public class BossFinalGuideIntroDoer : SpecificIntroDoer
{
	// Token: 0x06005887 RID: 22663 RVA: 0x0021D79C File Offset: 0x0021B99C
	public void Start()
	{
		this.m_topAnimator = base.aiAnimator.ChildAnimator;
	}

	// Token: 0x06005888 RID: 22664 RVA: 0x0021D7B0 File Offset: 0x0021B9B0
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x06005889 RID: 22665 RVA: 0x0021D7B8 File Offset: 0x0021B9B8
	public override void PlayerWalkedIn(PlayerController player, List<tk2dSpriteAnimator> animators)
	{
		base.aiAnimator.FacingDirection = -90f;
		this.m_topAnimator.FacingDirection = -90f;
		List<HealthHaver> allHealthHavers = StaticReferenceManager.AllHealthHavers;
		for (int i = 0; i < allHealthHavers.Count; i++)
		{
			if (!allHealthHavers[i].IsBoss && allHealthHavers[i].name.Contains("DrWolf", true))
			{
				ObjectVisibilityManager component = allHealthHavers[i].GetComponent<ObjectVisibilityManager>();
				component.ChangeToVisibility(RoomHandler.VisibilityStatus.CURRENT, true);
				allHealthHavers[i].specRigidbody.CollideWithOthers = true;
				allHealthHavers[i].aiActor.IsGone = false;
				allHealthHavers[i].aiActor.State = AIActor.ActorState.Normal;
				this.m_drAnimator = allHealthHavers[i].aiAnimator;
				animators.Add(this.m_drAnimator.spriteAnimator);
				break;
			}
		}
		base.aiAnimator.renderer.enabled = false;
		SpriteOutlineManager.ToggleOutlineRenderers(base.aiAnimator.sprite, false);
		base.aiAnimator.ChildAnimator.renderer.enabled = false;
		SpriteOutlineManager.ToggleOutlineRenderers(base.aiAnimator.ChildAnimator.sprite, false);
		UnityEngine.Object.FindObjectOfType<DungeonDoorSubsidiaryBlocker>().Seal();
	}

	// Token: 0x0600588A RID: 22666 RVA: 0x0021D8FC File Offset: 0x0021BAFC
	public override void StartIntro(List<tk2dSpriteAnimator> animators)
	{
		this.m_topAnimator.PlayUntilFinished("intro", false, null, -1f, false);
		this.m_drAnimator.PlayUntilFinished("intro", false, null, -1f, false);
		base.StartCoroutine(this.DoIntro());
	}

	// Token: 0x17000CAF RID: 3247
	// (get) Token: 0x0600588B RID: 22667 RVA: 0x0021D948 File Offset: 0x0021BB48
	public override bool IsIntroFinished
	{
		get
		{
			return this.m_finished;
		}
	}

	// Token: 0x0600588C RID: 22668 RVA: 0x0021D950 File Offset: 0x0021BB50
	public override void EndIntro()
	{
		base.aiAnimator.renderer.enabled = true;
		SpriteOutlineManager.ToggleOutlineRenderers(base.aiAnimator.sprite, true);
		base.aiAnimator.ChildAnimator.renderer.enabled = true;
		SpriteOutlineManager.ToggleOutlineRenderers(base.aiAnimator.ChildAnimator.sprite, true);
		base.aiActor.ToggleShadowVisiblity(true);
		this.m_finished = true;
		base.StopAllCoroutines();
		this.m_topAnimator.EndAnimationIf("intro");
		this.m_drAnimator.EndAnimationIf("intro");
	}

	// Token: 0x0600588D RID: 22669 RVA: 0x0021D9E8 File Offset: 0x0021BBE8
	private IEnumerator DoIntro()
	{
		AkSoundEngine.PostEvent("Play_BOSS_cyborg_drop_01", base.gameObject);
		base.aiAnimator.renderer.enabled = true;
		SpriteOutlineManager.ToggleOutlineRenderers(base.aiAnimator.sprite, true);
		base.aiAnimator.ChildAnimator.renderer.enabled = true;
		SpriteOutlineManager.ToggleOutlineRenderers(base.aiAnimator.ChildAnimator.sprite, true);
		Vector3 startShadowPosition = base.aiActor.ShadowObject.transform.position;
		Vector3 startPosition = base.aiAnimator.transform.position;
		float elapsed = 0f;
		float duration = 1f;
		while (elapsed < duration)
		{
			elapsed += GameManager.INVARIANT_DELTA_TIME;
			base.aiAnimator.transform.position = new Vector3(startPosition.x, startPosition.y + Mathf.Lerp(14f, 0f, elapsed / duration), startPosition.z).Quantize(0.0625f);
			base.aiActor.ShadowObject.transform.position = startShadowPosition;
			yield return null;
		}
		GameManager.Instance.MainCameraController.DoScreenShake(1f, 8f, 0.25f, 0.125f, null);
		base.aiActor.ShadowObject.transform.position = startShadowPosition;
		base.aiActor.ToggleShadowVisiblity(true);
		elapsed = 0f;
		duration = 3f;
		while (elapsed < duration)
		{
			yield return null;
			elapsed += GameManager.INVARIANT_DELTA_TIME;
		}
		this.m_finished = true;
		yield break;
	}

	// Token: 0x040051AB RID: 20907
	private AIAnimator m_topAnimator;

	// Token: 0x040051AC RID: 20908
	private AIAnimator m_drAnimator;

	// Token: 0x040051AD RID: 20909
	private bool m_finished;
}
