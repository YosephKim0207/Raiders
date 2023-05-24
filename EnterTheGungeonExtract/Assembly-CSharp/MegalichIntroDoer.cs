using System;
using System.Collections;
using System.Collections.Generic;
using Dungeonator;
using UnityEngine;

// Token: 0x02001066 RID: 4198
[RequireComponent(typeof(GenericIntroDoer))]
public class MegalichIntroDoer : SpecificIntroDoer
{
	// Token: 0x06005C50 RID: 23632 RVA: 0x0023607C File Offset: 0x0023427C
	public void Start()
	{
		base.aiAnimator.SetBaseAnim("blank", false);
		base.spriteAnimator.Play("blank");
	}

	// Token: 0x06005C51 RID: 23633 RVA: 0x002360A0 File Offset: 0x002342A0
	protected override void OnDestroy()
	{
		this.ModifyCamera(false);
		this.BlockPitTiles(false);
		base.OnDestroy();
	}

	// Token: 0x06005C52 RID: 23634 RVA: 0x002360B8 File Offset: 0x002342B8
	public override void PlayerWalkedIn(PlayerController player, List<tk2dSpriteAnimator> animators)
	{
		base.spriteAnimator.Play("blank");
	}

	// Token: 0x06005C53 RID: 23635 RVA: 0x002360CC File Offset: 0x002342CC
	public override void StartIntro(List<tk2dSpriteAnimator> animators)
	{
		base.aiAnimator.ClearBaseAnim();
		base.StartCoroutine(this.DoIntro());
	}

	// Token: 0x17000D86 RID: 3462
	// (get) Token: 0x06005C54 RID: 23636 RVA: 0x002360E8 File Offset: 0x002342E8
	public override Vector2? OverrideOutroPosition
	{
		get
		{
			this.ModifyCamera(true);
			this.BlockPitTiles(true);
			return null;
		}
	}

	// Token: 0x06005C55 RID: 23637 RVA: 0x0023610C File Offset: 0x0023430C
	public IEnumerator DoIntro()
	{
		base.aiAnimator.PlayUntilCancelled("intro", false, null, -1f, false);
		float elapsed = 0f;
		float duration = 1f;
		while (elapsed < duration)
		{
			yield return null;
			elapsed += GameManager.INVARIANT_DELTA_TIME;
		}
		GameManager.Instance.MainCameraController.DoContinuousScreenShake(this.screenShake, this, false);
		while (base.aiAnimator.IsPlaying("intro"))
		{
			yield return null;
		}
		base.aiAnimator.EndAnimationIf("intro");
		GameManager.Instance.MainCameraController.StopContinuousScreenShake(this);
		this.m_isFinished = true;
		yield break;
	}

	// Token: 0x17000D87 RID: 3463
	// (get) Token: 0x06005C56 RID: 23638 RVA: 0x00236128 File Offset: 0x00234328
	public override bool IsIntroFinished
	{
		get
		{
			return this.m_isFinished;
		}
	}

	// Token: 0x06005C57 RID: 23639 RVA: 0x00236130 File Offset: 0x00234330
	public override void EndIntro()
	{
		base.StopAllCoroutines();
		base.aiAnimator.EndAnimationIf("intro");
		GameManager.Instance.MainCameraController.StopContinuousScreenShake(this);
		AkSoundEngine.PostEvent("Play_MUS_lich_phase_02", base.gameObject);
	}

	// Token: 0x06005C58 RID: 23640 RVA: 0x0023616C File Offset: 0x0023436C
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
			mainCameraController.OverrideZoomScale = 0.75f;
			mainCameraController.LockToRoom = true;
			mainCameraController.AddFocusPoint(this.head);
			mainCameraController.controllerCamera.isTransitioning = false;
		}
		else
		{
			mainCameraController.SetZoomScaleImmediate(1f);
			mainCameraController.LockToRoom = false;
			mainCameraController.RemoveFocusPoint(this.head);
		}
		this.m_isCameraModified = value;
	}

	// Token: 0x06005C59 RID: 23641 RVA: 0x00236210 File Offset: 0x00234410
	public void BlockPitTiles(bool value)
	{
		if (!GameManager.HasInstance || GameManager.Instance.IsLoadingLevel || GameManager.IsReturningToBreach || GameManager.Instance.Dungeon == null)
		{
			return;
		}
		IntVector2 basePosition = base.aiActor.ParentRoom.area.basePosition;
		IntVector2 intVector = base.aiActor.ParentRoom.area.basePosition + base.aiActor.ParentRoom.area.dimensions - IntVector2.One;
		DungeonData data = GameManager.Instance.Dungeon.data;
		for (int i = basePosition.x; i <= intVector.x; i++)
		{
			for (int j = basePosition.y; j <= intVector.y; j++)
			{
				CellData cellData = data[i, j];
				if (cellData != null && cellData.type == CellType.PIT)
				{
					cellData.IsPlayerInaccessible = value;
				}
			}
		}
	}

	// Token: 0x040055F7 RID: 22007
	public GameObject head;

	// Token: 0x040055F8 RID: 22008
	public ScreenShakeSettings screenShake;

	// Token: 0x040055F9 RID: 22009
	private bool m_isFinished;

	// Token: 0x040055FA RID: 22010
	private bool m_isCameraModified;
}
