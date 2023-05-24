using System;
using System.Collections;
using Dungeonator;
using UnityEngine;

// Token: 0x02001194 RID: 4500
public class HologramFoyerController : BraveBehaviour
{
	// Token: 0x06006412 RID: 25618 RVA: 0x0026D24C File Offset: 0x0026B44C
	private IEnumerator Start()
	{
		while (Dungeon.IsGenerating || Foyer.DoIntroSequence || Foyer.DoMainMenu)
		{
			yield return null;
		}
		this.ArcRenderer.enabled = false;
		this.m_arcMaterial = this.ArcRenderer.material;
		base.StartCoroutine(this.Core());
		yield break;
	}

	// Token: 0x06006413 RID: 25619 RVA: 0x0026D268 File Offset: 0x0026B468
	private IEnumerator Core()
	{
		yield return null;
		base.StartCoroutine(this.ToggleAdditionalLight(true));
		base.StartCoroutine(this.HandleArcLerp(false));
		int animIndex = 0;
		this.TargetAnimator.Sprite.renderer.material.SetFloat("_IsGreen", -1f);
		for (;;)
		{
			this.TargetAnimator.Sprite.renderer.material.SetFloat("_IsGreen", -1f);
			string animName = this.animationCadence[animIndex];
			yield return base.StartCoroutine(this.CoreCycle(animName));
			animIndex = (animIndex + 1) % this.animationCadence.Length;
		}
		yield break;
	}

	// Token: 0x06006414 RID: 25620 RVA: 0x0026D284 File Offset: 0x0026B484
	private IEnumerator CoreCycle(string targetAnimation)
	{
		this.ChangeToAnimation(targetAnimation);
		int m_id = Shader.PropertyToID("_IsGreen");
		while (this.TargetAnimator.IsPlaying(targetAnimation))
		{
			this.TargetAnimator.Sprite.renderer.material.SetFloat(m_id, -1f);
			yield return null;
		}
		yield break;
	}

	// Token: 0x06006415 RID: 25621 RVA: 0x0026D2A8 File Offset: 0x0026B4A8
	private IEnumerator ToggleAdditionalLight(bool lightEnabled)
	{
		float elapsed = 0f;
		float duration = 0.25f;
		while (elapsed < duration)
		{
			elapsed += BraveTime.DeltaTime;
			float t = elapsed / duration;
			if (!lightEnabled)
			{
				t = 1f - t;
			}
			this.AttachedBraveLight.LightIntensity = Mathf.Lerp(0f, 3f, t);
			yield return null;
		}
		yield break;
	}

	// Token: 0x06006416 RID: 25622 RVA: 0x0026D2CC File Offset: 0x0026B4CC
	private IEnumerator HandleArcLerp(bool invert)
	{
		float ela = 0f;
		this.ArcRenderer.enabled = true;
		while (ela < 0.2f)
		{
			ela += BraveTime.DeltaTime;
			float t = ela / 0.2f;
			if (invert)
			{
				t = Mathf.Clamp01(1f - t);
			}
			float smoothT = Mathf.SmoothStep(0f, 1f, t);
			this.m_arcMaterial.SetFloat("_RevealAmount", smoothT);
			this.ArcRenderer.enabled = true;
			yield return null;
		}
		if (!invert)
		{
			this.m_arcMaterial.SetFloat("_BrightnessWarble", Mathf.PingPong(Time.realtimeSinceStartup, 1f) / 10f + 1f);
			yield return null;
		}
		yield break;
	}

	// Token: 0x06006417 RID: 25623 RVA: 0x0026D2F0 File Offset: 0x0026B4F0
	public void ChangeToAnimation(string animationName)
	{
		this.TargetAnimator.renderer.enabled = true;
		this.TargetAnimator.Play(animationName);
		this.TargetAnimator.Sprite.usesOverrideMaterial = true;
		this.TargetAnimator.renderer.material.shader = ShaderCache.Acquire("Brave/Internal/HologramShader");
	}

	// Token: 0x04005FAC RID: 24492
	public string[] animationCadence;

	// Token: 0x04005FAD RID: 24493
	public MeshRenderer ArcRenderer;

	// Token: 0x04005FAE RID: 24494
	private Material m_arcMaterial;

	// Token: 0x04005FAF RID: 24495
	public AdditionalBraveLight AttachedBraveLight;

	// Token: 0x04005FB0 RID: 24496
	public tk2dSpriteAnimator TargetAnimator;
}
