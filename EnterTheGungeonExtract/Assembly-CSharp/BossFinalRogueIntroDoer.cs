using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000FE6 RID: 4070
[RequireComponent(typeof(GenericIntroDoer))]
public class BossFinalRogueIntroDoer : SpecificIntroDoer
{
	// Token: 0x060058C5 RID: 22725 RVA: 0x0021E938 File Offset: 0x0021CB38
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x17000CB8 RID: 3256
	// (get) Token: 0x060058C6 RID: 22726 RVA: 0x0021E940 File Offset: 0x0021CB40
	public override Vector2? OverrideIntroPosition
	{
		get
		{
			GameManager.Instance.MainCameraController.OverrideZoomScale = 0.6666f;
			return new Vector2?(base.GetComponent<BossFinalRogueController>().CameraPos);
		}
	}

	// Token: 0x060058C7 RID: 22727 RVA: 0x0021E968 File Offset: 0x0021CB68
	public override void StartIntro(List<tk2dSpriteAnimator> animators)
	{
		base.StartCoroutine(this.DoIntro());
	}

	// Token: 0x060058C8 RID: 22728 RVA: 0x0021E978 File Offset: 0x0021CB78
	public IEnumerator DoIntro()
	{
		yield return base.TimeInvariantWait(1f);
		this.m_isFinished = true;
		yield break;
	}

	// Token: 0x17000CB9 RID: 3257
	// (get) Token: 0x060058C9 RID: 22729 RVA: 0x0021E994 File Offset: 0x0021CB94
	public override bool IsIntroFinished
	{
		get
		{
			return this.m_isFinished;
		}
	}

	// Token: 0x17000CBA RID: 3258
	// (get) Token: 0x060058CA RID: 22730 RVA: 0x0021E99C File Offset: 0x0021CB9C
	public override Vector2? OverrideOutroPosition
	{
		get
		{
			BossFinalRogueController component = base.GetComponent<BossFinalRogueController>();
			component.InitCamera();
			return new Vector2?(component.CameraPos);
		}
	}

	// Token: 0x060058CB RID: 22731 RVA: 0x0021E9C4 File Offset: 0x0021CBC4
	public override void EndIntro()
	{
		BossFinalRogueController component = base.GetComponent<BossFinalRogueController>();
		component.InitCamera();
		GameManager.Instance.MainCameraController.SetManualControl(true, true);
		GameManager.Instance.MainCameraController.OverrideZoomScale = 0.6666f;
	}

	// Token: 0x040051ED RID: 20973
	private bool m_isFinished;
}
