using System;
using System.Collections;
using System.Collections.Generic;
using Dungeonator;
using UnityEngine;

// Token: 0x02001036 RID: 4150
public abstract class SpecificIntroDoer : BraveBehaviour
{
	// Token: 0x06005B08 RID: 23304 RVA: 0x0022EC30 File Offset: 0x0022CE30
	public virtual IntVector2 OverrideExitBasePosition(DungeonData.Direction directionToWalk, IntVector2 exitBaseCenter)
	{
		return exitBaseCenter;
	}

	// Token: 0x17000D3D RID: 3389
	// (get) Token: 0x06005B09 RID: 23305 RVA: 0x0022EC34 File Offset: 0x0022CE34
	public virtual Vector2? OverrideIntroPosition
	{
		get
		{
			return null;
		}
	}

	// Token: 0x17000D3E RID: 3390
	// (get) Token: 0x06005B0A RID: 23306 RVA: 0x0022EC4C File Offset: 0x0022CE4C
	public virtual Vector2? OverrideOutroPosition
	{
		get
		{
			return null;
		}
	}

	// Token: 0x17000D3F RID: 3391
	// (get) Token: 0x06005B0B RID: 23307 RVA: 0x0022EC64 File Offset: 0x0022CE64
	public virtual string OverrideBossMusicEvent
	{
		get
		{
			return null;
		}
	}

	// Token: 0x06005B0C RID: 23308 RVA: 0x0022EC68 File Offset: 0x0022CE68
	public virtual void PlayerWalkedIn(PlayerController player, List<tk2dSpriteAnimator> animators)
	{
	}

	// Token: 0x06005B0D RID: 23309 RVA: 0x0022EC6C File Offset: 0x0022CE6C
	public virtual void OnCameraIntro()
	{
	}

	// Token: 0x06005B0E RID: 23310 RVA: 0x0022EC70 File Offset: 0x0022CE70
	public virtual void StartIntro(List<tk2dSpriteAnimator> animators)
	{
	}

	// Token: 0x17000D40 RID: 3392
	// (get) Token: 0x06005B0F RID: 23311 RVA: 0x0022EC74 File Offset: 0x0022CE74
	public virtual bool IsIntroFinished
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06005B10 RID: 23312 RVA: 0x0022EC78 File Offset: 0x0022CE78
	public virtual void OnBossCard()
	{
	}

	// Token: 0x06005B11 RID: 23313 RVA: 0x0022EC7C File Offset: 0x0022CE7C
	public virtual void OnCameraOutro()
	{
	}

	// Token: 0x06005B12 RID: 23314 RVA: 0x0022EC80 File Offset: 0x0022CE80
	public virtual void OnCleanup()
	{
	}

	// Token: 0x06005B13 RID: 23315 RVA: 0x0022EC84 File Offset: 0x0022CE84
	public virtual void EndIntro()
	{
	}

	// Token: 0x06005B14 RID: 23316 RVA: 0x0022EC88 File Offset: 0x0022CE88
	public IEnumerator TimeInvariantWait(float duration)
	{
		for (float elapsed = 0f; elapsed < duration; elapsed += GameManager.INVARIANT_DELTA_TIME)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x06005B15 RID: 23317 RVA: 0x0022ECA4 File Offset: 0x0022CEA4
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}
}
