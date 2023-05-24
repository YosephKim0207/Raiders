using System;
using UnityEngine;

// Token: 0x02000EDD RID: 3805
public class DungeonDoorSubsidiaryBlocker : BraveBehaviour
{
	// Token: 0x0600510D RID: 20749 RVA: 0x001CAB88 File Offset: 0x001C8D88
	public void ToggleRenderers(bool visible)
	{
		if (this.sealAnimator != null)
		{
			this.sealAnimator.GetComponent<Renderer>().enabled = visible;
		}
		if (this.chainAnimator != null)
		{
			this.chainAnimator.GetComponent<Renderer>().enabled = visible;
		}
	}

	// Token: 0x0600510E RID: 20750 RVA: 0x001CABDC File Offset: 0x001C8DDC
	private void Update()
	{
		if (this.parentDoor != null && this.parentDoor.northSouth && this.isSealed && !string.IsNullOrEmpty(this.playerNearSealedAnimationName))
		{
			Vector2 unitCenter = this.sealAnimator.GetComponent<SpeculativeRigidbody>().UnitCenter;
			if (Vector2.Distance(unitCenter, GameManager.Instance.PrimaryPlayer.specRigidbody.UnitCenter) < 4f)
			{
				if (!this.sealAnimator.IsPlaying(this.playerNearSealedAnimationName) && !this.sealAnimator.IsPlaying(this.unsealAnimationName) && !this.sealAnimator.IsPlaying(this.sealAnimationName))
				{
					this.sealAnimator.Play(this.playerNearSealedAnimationName);
				}
			}
			else if (this.sealAnimator.IsPlaying(this.playerNearSealedAnimationName))
			{
				this.sealAnimator.Stop();
				tk2dSpriteAnimationClip clipByName = this.sealAnimator.GetClipByName(this.sealAnimationName);
				this.sealAnimator.Sprite.SetSprite(clipByName.frames[clipByName.frames.Length - 1].spriteId);
			}
		}
	}

	// Token: 0x0600510F RID: 20751 RVA: 0x001CAD0C File Offset: 0x001C8F0C
	public void OnSealAnimationEvent(tk2dSpriteAnimator animator, tk2dSpriteAnimationClip clip, int frameNo)
	{
		if (clip.GetFrame(frameNo).eventInfo == "SealVFX" && this.sealVFX != null)
		{
			this.sealVFX.gameObject.SetActive(true);
			this.sealVFX.Play();
		}
	}

	// Token: 0x06005110 RID: 20752 RVA: 0x001CAD64 File Offset: 0x001C8F64
	public void OnUnsealAnimationCompleted(tk2dSpriteAnimator a, tk2dSpriteAnimationClip c)
	{
		if (this.hideSealAnimators)
		{
			a.gameObject.SetActive(false);
		}
		if (a.GetComponent<SpeculativeRigidbody>() != null)
		{
			a.GetComponent<SpeculativeRigidbody>().enabled = false;
		}
		if (this.unsealedVFXOverride != null)
		{
			this.unsealedVFXOverride.SetActive(true);
		}
	}

	// Token: 0x06005111 RID: 20753 RVA: 0x001CADC4 File Offset: 0x001C8FC4
	public void Seal()
	{
		if (!string.IsNullOrEmpty(this.sealAnimationName))
		{
			this.sealAnimator.alwaysUpdateOffscreen = true;
			this.sealAnimator.AnimationCompleted = null;
			tk2dSpriteAnimator tk2dSpriteAnimator = this.sealAnimator;
			tk2dSpriteAnimator.AnimationEventTriggered = (Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip, int>)Delegate.Combine(tk2dSpriteAnimator.AnimationEventTriggered, new Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip, int>(this.OnSealAnimationEvent));
			this.sealAnimator.gameObject.SetActive(true);
			this.sealAnimator.Play(this.sealAnimationName);
			AkSoundEngine.PostEvent("Play_OBJ_gate_slam_01", base.gameObject);
		}
		if (!string.IsNullOrEmpty(this.sealChainAnimationName))
		{
			this.chainAnimator.Play(this.sealChainAnimationName);
		}
		if (this.sealAnimator.GetComponent<SpeculativeRigidbody>() != null)
		{
			this.sealAnimator.GetComponent<SpeculativeRigidbody>().enabled = true;
		}
		this.isSealed = true;
	}

	// Token: 0x06005112 RID: 20754 RVA: 0x001CAEA4 File Offset: 0x001C90A4
	public void Unseal()
	{
		if (!this.isSealed)
		{
			return;
		}
		if (!string.IsNullOrEmpty(this.unsealAnimationName))
		{
			this.sealAnimator.alwaysUpdateOffscreen = true;
			this.sealAnimator.Play(this.unsealAnimationName);
			tk2dSpriteAnimator tk2dSpriteAnimator = this.sealAnimator;
			tk2dSpriteAnimator.AnimationCompleted = (Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip>)Delegate.Combine(tk2dSpriteAnimator.AnimationCompleted, new Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip>(this.OnUnsealAnimationCompleted));
			this.sealAnimator.AnimationEventTriggered = null;
			AkSoundEngine.PostEvent("Play_OBJ_gate_open_01", base.gameObject);
		}
		if (!string.IsNullOrEmpty(this.unsealChainAnimationName))
		{
			this.chainAnimator.Play(this.unsealChainAnimationName);
		}
		if (this.usesUnsealScreenShake)
		{
			GameManager.Instance.MainCameraController.DoScreenShake(this.unsealScreenShake, new Vector2?(base.transform.position), false);
		}
		this.isSealed = false;
	}

	// Token: 0x06005113 RID: 20755 RVA: 0x001CAF8C File Offset: 0x001C918C
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x0400490D RID: 18701
	public bool hideSealAnimators = true;

	// Token: 0x0400490E RID: 18702
	public tk2dSpriteAnimator sealAnimator;

	// Token: 0x0400490F RID: 18703
	public tk2dSpriteAnimator chainAnimator;

	// Token: 0x04004910 RID: 18704
	public tk2dSpriteAnimator sealVFX;

	// Token: 0x04004911 RID: 18705
	public float unsealDistanceMaximum = -1f;

	// Token: 0x04004912 RID: 18706
	public GameObject unsealedVFXOverride;

	// Token: 0x04004913 RID: 18707
	public string sealAnimationName;

	// Token: 0x04004914 RID: 18708
	public string sealChainAnimationName;

	// Token: 0x04004915 RID: 18709
	public string unsealAnimationName;

	// Token: 0x04004916 RID: 18710
	public string unsealChainAnimationName;

	// Token: 0x04004917 RID: 18711
	public string playerNearSealedAnimationName;

	// Token: 0x04004918 RID: 18712
	public string playerNearChainAnimationName;

	// Token: 0x04004919 RID: 18713
	[NonSerialized]
	public bool isSealed;

	// Token: 0x0400491A RID: 18714
	public bool northSouth;

	// Token: 0x0400491B RID: 18715
	public bool usesUnsealScreenShake;

	// Token: 0x0400491C RID: 18716
	public ScreenShakeSettings unsealScreenShake;

	// Token: 0x0400491D RID: 18717
	[HideInInspector]
	public DungeonDoorController parentDoor;
}
