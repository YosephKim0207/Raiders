using System;
using System.Collections;
using UnityEngine;

// Token: 0x020017B4 RID: 6068
public class CharacterSelectIdleDoer : BraveBehaviour
{
	// Token: 0x06008E03 RID: 36355 RVA: 0x003BBA8C File Offset: 0x003B9C8C
	private void Update()
	{
		if (this.IsEevee)
		{
			base.sprite.usesOverrideMaterial = true;
			base.sprite.renderer.material.shader = Shader.Find("Brave/PlayerShaderEevee");
			base.sprite.renderer.sharedMaterial.SetTexture("_EeveeTex", this.EeveeTex);
			this.m_lastEeveeSwitchTime += BraveTime.DeltaTime;
			if (this.m_lastEeveeSwitchTime > 2.5f)
			{
				this.m_lastEeveeSwitchTime -= 2.5f;
				int num = UnityEngine.Random.Range(0, this.AnimationLibraries.Length);
				base.spriteAnimator.Library = this.AnimationLibraries[num];
				base.spriteAnimator.Play(this.coreIdleAnimation);
			}
		}
	}

	// Token: 0x06008E04 RID: 36356 RVA: 0x003BBB58 File Offset: 0x003B9D58
	private void OnEnable()
	{
		if (this.IsEevee)
		{
			base.sprite.usesOverrideMaterial = true;
			base.sprite.renderer.material.shader = Shader.Find("Brave/PlayerShaderEevee");
			base.sprite.renderer.sharedMaterial.SetTexture("_EeveeTex", this.EeveeTex);
		}
		base.StartCoroutine(this.HandleCoreIdle());
	}

	// Token: 0x06008E05 RID: 36357 RVA: 0x003BBBC8 File Offset: 0x003B9DC8
	private void OnDisable()
	{
		base.spriteAnimator.StopAndResetFrame();
		base.StopAllCoroutines();
	}

	// Token: 0x06008E06 RID: 36358 RVA: 0x003BBBDC File Offset: 0x003B9DDC
	private IEnumerator HandleCoreIdle()
	{
		if (!CharacterSelectController.HasSelected)
		{
			base.spriteAnimator.Play(this.coreIdleAnimation);
		}
		yield return new WaitForSeconds(Mathf.Lerp(this.idleMin, this.idleMax, UnityEngine.Random.value));
		if (this.phases.Length != 0)
		{
			int num = UnityEngine.Random.Range(0, this.phases.Length);
			if (num == this.lastPhase && this.phases.Length > 1)
			{
				num = (num + 1) % this.phases.Length;
			}
			if (num < 0 || num >= this.phases.Length)
			{
				num = 0;
			}
			CharacterSelectIdlePhase characterSelectIdlePhase = this.phases[num];
			this.lastPhase = num;
			if (!CharacterSelectController.HasSelected)
			{
				base.StartCoroutine(this.HandlePhase(characterSelectIdlePhase));
			}
		}
		yield break;
	}

	// Token: 0x06008E07 RID: 36359 RVA: 0x003BBBF8 File Offset: 0x003B9DF8
	private void DeactivateVFX(tk2dSpriteAnimator s, tk2dSpriteAnimationClip c)
	{
		s.AnimationCompleted = (Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip>)Delegate.Remove(s.AnimationCompleted, new Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip>(this.DeactivateVFX));
		s.gameObject.SetActive(false);
	}

	// Token: 0x06008E08 RID: 36360 RVA: 0x003BBC28 File Offset: 0x003B9E28
	private void TriggerVFX(CharacterSelectIdlePhase phase)
	{
		phase.vfxSpriteAnimator.StopAndResetFrame();
		phase.vfxSpriteAnimator.gameObject.SetActive(true);
		phase.vfxSpriteAnimator.Play();
		tk2dSpriteAnimator vfxSpriteAnimator = phase.vfxSpriteAnimator;
		vfxSpriteAnimator.AnimationCompleted = (Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip>)Delegate.Combine(vfxSpriteAnimator.AnimationCompleted, new Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip>(this.DeactivateVFX));
	}

	// Token: 0x06008E09 RID: 36361 RVA: 0x003BBC84 File Offset: 0x003B9E84
	private void TriggerEndVFX(CharacterSelectIdlePhase phase)
	{
		if (phase.endVFXSpriteAnimator != null)
		{
			phase.endVFXSpriteAnimator.StopAndResetFrame();
			phase.endVFXSpriteAnimator.Play();
		}
	}

	// Token: 0x06008E0A RID: 36362 RVA: 0x003BBCB0 File Offset: 0x003B9EB0
	private IEnumerator HandlePhase(CharacterSelectIdlePhase phase)
	{
		if (!string.IsNullOrEmpty(phase.inAnimation))
		{
			tk2dSpriteAnimationClip clip = base.spriteAnimator.GetClipByName(phase.inAnimation);
			if (!CharacterSelectController.HasSelected && clip != null)
			{
				base.spriteAnimator.Play(clip);
			}
			if (phase.vfxTrigger == CharacterSelectIdlePhase.VFXPhaseTrigger.IN)
			{
				this.TriggerVFX(phase);
			}
			if (clip != null)
			{
				yield return new WaitForSeconds((float)clip.frames.Length / clip.fps);
			}
		}
		if (!string.IsNullOrEmpty(phase.holdAnimation) && !CharacterSelectController.HasSelected)
		{
			base.spriteAnimator.Play(phase.holdAnimation);
		}
		float elapsed = 0f;
		float vfxElapsed = 0f;
		float holdDuration = Mathf.Lerp(phase.holdMin, phase.holdMax, UnityEngine.Random.value);
		while (elapsed < holdDuration)
		{
			if (phase.vfxTrigger == CharacterSelectIdlePhase.VFXPhaseTrigger.HOLD && vfxElapsed > phase.vfxHoldPeriod)
			{
				vfxElapsed -= phase.vfxHoldPeriod;
				if (!phase.vfxSpriteAnimator.gameObject.activeSelf)
				{
					this.TriggerVFX(phase);
				}
			}
			elapsed += BraveTime.DeltaTime;
			vfxElapsed += BraveTime.DeltaTime;
			yield return null;
		}
		if (!string.IsNullOrEmpty(phase.optionalHoldIdleAnimation) && UnityEngine.Random.value < phase.optionalHoldChance)
		{
			if (!CharacterSelectController.HasSelected)
			{
				base.spriteAnimator.Play(phase.optionalHoldIdleAnimation);
			}
			yield return new WaitForSeconds(Mathf.Lerp(phase.holdMin, phase.holdMax, UnityEngine.Random.value));
		}
		if (!string.IsNullOrEmpty(phase.outAnimation))
		{
			tk2dSpriteAnimationClip clip2 = base.spriteAnimator.GetClipByName(phase.outAnimation);
			if (!CharacterSelectController.HasSelected)
			{
				base.spriteAnimator.Play(clip2);
			}
			if (phase.vfxTrigger == CharacterSelectIdlePhase.VFXPhaseTrigger.OUT)
			{
				this.TriggerVFX(phase);
			}
			yield return new WaitForSeconds((float)(clip2.frames.Length - 2) / clip2.fps);
			this.TriggerEndVFX(phase);
			yield return new WaitForSeconds(2f / clip2.fps);
		}
		base.StartCoroutine(this.HandleCoreIdle());
		yield break;
	}

	// Token: 0x06008E0B RID: 36363 RVA: 0x003BBCD4 File Offset: 0x003B9ED4
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x040095EB RID: 38379
	public string coreIdleAnimation = "select_idle";

	// Token: 0x040095EC RID: 38380
	public string onSelectedAnimation;

	// Token: 0x040095ED RID: 38381
	public float idleMin = 4f;

	// Token: 0x040095EE RID: 38382
	public float idleMax = 10f;

	// Token: 0x040095EF RID: 38383
	public CharacterSelectIdlePhase[] phases;

	// Token: 0x040095F0 RID: 38384
	public bool IsEevee;

	// Token: 0x040095F1 RID: 38385
	public Texture2D EeveeTex;

	// Token: 0x040095F2 RID: 38386
	public tk2dSpriteAnimation[] AnimationLibraries;

	// Token: 0x040095F3 RID: 38387
	protected int lastPhase = -1;

	// Token: 0x040095F4 RID: 38388
	protected float m_lastEeveeSwitchTime;
}
