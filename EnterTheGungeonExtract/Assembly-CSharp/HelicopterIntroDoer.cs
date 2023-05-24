using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001048 RID: 4168
[RequireComponent(typeof(GenericIntroDoer))]
public class HelicopterIntroDoer : SpecificIntroDoer
{
	// Token: 0x17000D58 RID: 3416
	// (get) Token: 0x06005B7D RID: 23421 RVA: 0x00230804 File Offset: 0x0022EA04
	// (set) Token: 0x06005B7E RID: 23422 RVA: 0x0023080C File Offset: 0x0022EA0C
	public bool IsCameraModified { get; set; }

	// Token: 0x17000D59 RID: 3417
	// (get) Token: 0x06005B7F RID: 23423 RVA: 0x00230818 File Offset: 0x0022EA18
	public override bool IsIntroFinished
	{
		get
		{
			return this.m_isFinished && base.IsIntroFinished;
		}
	}

	// Token: 0x06005B80 RID: 23424 RVA: 0x00230830 File Offset: 0x0022EA30
	protected override void OnDestroy()
	{
		this.ModifyCamera(false);
		base.OnDestroy();
	}

	// Token: 0x06005B81 RID: 23425 RVA: 0x00230840 File Offset: 0x0022EA40
	public override void StartIntro(List<tk2dSpriteAnimator> animators)
	{
		base.StartIntro(animators);
		base.StartCoroutine(this.DoIntro());
	}

	// Token: 0x06005B82 RID: 23426 RVA: 0x00230858 File Offset: 0x0022EA58
	public IEnumerator DoIntro()
	{
		TextBoxManager.TIME_INVARIANT = true;
		yield return base.StartCoroutine(base.GetComponent<VoiceOverer>().HandleIntroVO());
		TextBoxManager.TIME_INVARIANT = false;
		this.m_isFinished = true;
		yield break;
	}

	// Token: 0x06005B83 RID: 23427 RVA: 0x00230874 File Offset: 0x0022EA74
	public override void PlayerWalkedIn(PlayerController player, List<tk2dSpriteAnimator> animators)
	{
		CameraController mainCameraController = GameManager.Instance.MainCameraController;
		mainCameraController.SetZoomScaleImmediate(0.75f);
		AkSoundEngine.PostEvent("Play_boss_helicopter_loop_01", base.gameObject);
		AkSoundEngine.PostEvent("Play_State_Volume_Lower_01", base.gameObject);
		base.aiActor.ParentRoom.CompletelyPreventLeaving = true;
	}

	// Token: 0x17000D5A RID: 3418
	// (get) Token: 0x06005B84 RID: 23428 RVA: 0x002308CC File Offset: 0x0022EACC
	public override Vector2? OverrideOutroPosition
	{
		get
		{
			this.ModifyCamera(true);
			return null;
		}
	}

	// Token: 0x06005B85 RID: 23429 RVA: 0x002308EC File Offset: 0x0022EAEC
	public void ModifyCamera(bool value)
	{
		if (!GameManager.HasInstance || GameManager.Instance.IsLoadingLevel || GameManager.IsReturningToBreach)
		{
			return;
		}
		if (this.IsCameraModified == value)
		{
			return;
		}
		CameraController mainCameraController = GameManager.Instance.MainCameraController;
		if (value)
		{
			mainCameraController.OverrideZoomScale = 0.75f;
			mainCameraController.LockToRoom = true;
			mainCameraController.controllerCamera.isTransitioning = false;
		}
		else
		{
			mainCameraController.SetZoomScaleImmediate(1f);
			mainCameraController.LockToRoom = false;
			AkSoundEngine.PostEvent("Stop_State_Volume_Lower_01", base.gameObject);
		}
		this.IsCameraModified = value;
	}

	// Token: 0x04005514 RID: 21780
	private bool m_isFinished;
}
