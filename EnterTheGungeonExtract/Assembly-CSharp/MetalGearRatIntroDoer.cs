using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200106B RID: 4203
[RequireComponent(typeof(GenericIntroDoer))]
public class MetalGearRatIntroDoer : SpecificIntroDoer
{
	// Token: 0x06005C73 RID: 23667 RVA: 0x00236C00 File Offset: 0x00234E00
	private void Awake()
	{
		this.m_introDummy = base.transform.Find("intro dummy").gameObject;
		this.m_introLightsDummy = base.transform.Find("intro dummy lights").GetComponent<tk2dSpriteAnimator>();
	}

	// Token: 0x06005C74 RID: 23668 RVA: 0x00236C38 File Offset: 0x00234E38
	protected override void OnDestroy()
	{
		this.ModifyCamera(false);
		base.OnDestroy();
	}

	// Token: 0x06005C75 RID: 23669 RVA: 0x00236C48 File Offset: 0x00234E48
	public void Init()
	{
		if (this.m_initialized)
		{
			return;
		}
		GameManager.Instance.Dungeon.OverrideAmbientLight = true;
		GameManager.Instance.Dungeon.OverrideAmbientColor = base.aiActor.ParentRoom.area.runtimePrototypeData.customAmbient * 0.1f;
		base.aiActor.ParentRoom.BecomeTerrifyingDarkRoom(0f, 0.1f, 1f, "Play_Empty_Event_01");
		AkSoundEngine.PostEvent("Play_BOSS_RatMech_Ambience_01", base.gameObject);
		GameManager.Instance.Dungeon.OverrideAmbientLight = true;
		GameManager.Instance.Dungeon.OverrideAmbientColor = base.aiActor.ParentRoom.area.runtimePrototypeData.customAmbient * 0.1f;
		base.aiAnimator.PlayUntilCancelled("blank", false, null, -1f, false);
		base.aiAnimator.ChildAnimator.PlayUntilCancelled("blank", false, null, -1f, false);
		base.aiAnimator.ChildAnimator.ChildAnimator.PlayUntilCancelled("blank", false, null, -1f, false);
		this.m_introDummy.SetActive(true);
		GameManager.Instance.StartCoroutine(this.MusicStopperCR());
		this.m_initialized = true;
	}

	// Token: 0x06005C76 RID: 23670 RVA: 0x00236D9C File Offset: 0x00234F9C
	private IEnumerator MusicStopperCR()
	{
		for (int i = 0; i < 3; i++)
		{
			if (this && !this.m_musicStarted)
			{
				AkSoundEngine.PostEvent("Stop_MUS_All", base.gameObject);
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x06005C77 RID: 23671 RVA: 0x00236DB8 File Offset: 0x00234FB8
	public override void PlayerWalkedIn(PlayerController player, List<tk2dSpriteAnimator> animators)
	{
		base.PlayerWalkedIn(player, animators);
		this.Init();
	}

	// Token: 0x06005C78 RID: 23672 RVA: 0x00236DC8 File Offset: 0x00234FC8
	public override void StartIntro(List<tk2dSpriteAnimator> animators)
	{
		animators.Add(base.aiAnimator.ChildAnimator.ChildAnimator.spriteAnimator);
		base.aiAnimator.ChildAnimator.ChildAnimator.spriteAnimator.enabled = false;
		base.StartCoroutine(this.DoIntro());
	}

	// Token: 0x17000D8E RID: 3470
	// (get) Token: 0x06005C79 RID: 23673 RVA: 0x00236E18 File Offset: 0x00235018
	public override Vector2? OverrideOutroPosition
	{
		get
		{
			this.ModifyCamera(true);
			return null;
		}
	}

	// Token: 0x06005C7A RID: 23674 RVA: 0x00236E38 File Offset: 0x00235038
	public IEnumerator DoIntro()
	{
		this.m_introDummy.SetActive(true);
		this.m_introLightsDummy.gameObject.SetActive(true);
		this.m_introLightsDummy.Play("intro_light");
		float elapsed = 0f;
		float duration = this.m_introLightsDummy.CurrentClip.BaseClipLength;
		while (elapsed < duration)
		{
			yield return null;
			elapsed += GameManager.INVARIANT_DELTA_TIME;
		}
		Material material = this.m_introDummy.GetComponent<Renderer>().material;
		elapsed = 0f;
		duration = 1f;
		while (elapsed < duration)
		{
			yield return null;
			elapsed += GameManager.INVARIANT_DELTA_TIME;
			material.SetFloat("_ValueMinimum", elapsed / duration * 0.5f);
		}
		this.m_introDummy.SetActive(false);
		this.m_introLightsDummy.gameObject.SetActive(false);
		base.aiAnimator.EndAnimationIf("blank");
		base.aiAnimator.ChildAnimator.EndAnimationIf("blank");
		base.aiAnimator.ChildAnimator.ChildAnimator.EndAnimationIf("blank");
		base.aiAnimator.PlayUntilCancelled("intro", false, null, -1f, false);
		GameManager.Instance.DungeonMusicController.SwitchToBossMusic("Play_MUS_Rat_Theme_02", base.gameObject);
		this.m_musicStarted = true;
		elapsed = 0f;
		duration = base.aiAnimator.CurrentClipLength;
		while (elapsed < duration)
		{
			yield return null;
			elapsed += GameManager.INVARIANT_DELTA_TIME;
		}
		base.aiAnimator.EndAnimationIf("intro");
		this.m_isFinished = true;
		yield break;
	}

	// Token: 0x17000D8F RID: 3471
	// (get) Token: 0x06005C7B RID: 23675 RVA: 0x00236E54 File Offset: 0x00235054
	public override bool IsIntroFinished
	{
		get
		{
			return this.m_isFinished;
		}
	}

	// Token: 0x06005C7C RID: 23676 RVA: 0x00236E5C File Offset: 0x0023505C
	public override void OnCleanup()
	{
		base.aiAnimator.ChildAnimator.ChildAnimator.spriteAnimator.enabled = true;
		base.OnCleanup();
	}

	// Token: 0x06005C7D RID: 23677 RVA: 0x00236E80 File Offset: 0x00235080
	public override void EndIntro()
	{
		base.StopAllCoroutines();
		base.aiAnimator.EndAnimationIf("intro");
		this.m_introDummy.SetActive(false);
		this.m_introLightsDummy.gameObject.SetActive(false);
		base.aiAnimator.EndAnimationIf("blank");
		base.aiAnimator.ChildAnimator.EndAnimationIf("blank");
		base.aiAnimator.ChildAnimator.ChildAnimator.EndAnimationIf("blank");
		GameManager.Instance.Dungeon.OverrideAmbientLight = false;
		base.aiActor.ParentRoom.EndTerrifyingDarkRoom(1f, 0.1f, 1f, "Play_Empty_Event_01");
		GameManager.Instance.Dungeon.OverrideAmbientLight = false;
		if (!this.m_musicStarted)
		{
			GameManager.Instance.DungeonMusicController.SwitchToBossMusic("Play_MUS_Rat_Theme_02", base.gameObject);
			this.m_musicStarted = true;
		}
	}

	// Token: 0x06005C7E RID: 23678 RVA: 0x00236F74 File Offset: 0x00235174
	public void ModifyCamera(bool value)
	{
		if (!GameManager.HasInstance || GameManager.Instance.IsLoadingLevel || GameManager.IsReturningToBreach)
		{
			return;
		}
		if (this.m_isCameraModified == value)
		{
			return;
		}
		CameraController mainCameraController = GameManager.Instance.MainCameraController;
		if (value)
		{
			mainCameraController.OverrideZoomScale = 0.66f;
			mainCameraController.LockToRoom = true;
			mainCameraController.AddFocusPoint(this.head);
			mainCameraController.controllerCamera.isTransitioning = false;
			Projectile.SetGlobalProjectileDepth(4f);
			BasicBeamController.SetGlobalBeamHeight(4f);
		}
		else
		{
			mainCameraController.SetZoomScaleImmediate(1f);
			mainCameraController.LockToRoom = false;
			mainCameraController.RemoveFocusPoint(this.head);
			Projectile.ResetGlobalProjectileDepth();
			BasicBeamController.ResetGlobalBeamHeight();
		}
		this.m_isCameraModified = value;
	}

	// Token: 0x0400561E RID: 22046
	public GameObject head;

	// Token: 0x0400561F RID: 22047
	private bool m_initialized;

	// Token: 0x04005620 RID: 22048
	private GameObject m_introDummy;

	// Token: 0x04005621 RID: 22049
	private tk2dSpriteAnimator m_introLightsDummy;

	// Token: 0x04005622 RID: 22050
	private bool m_isFinished;

	// Token: 0x04005623 RID: 22051
	private bool m_isCameraModified;

	// Token: 0x04005624 RID: 22052
	private bool m_musicStarted;
}
