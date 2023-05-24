using System;
using System.Collections;
using UnityEngine;

// Token: 0x020017B0 RID: 6064
public class CharacterSelectFacecardIdleDoer : BraveBehaviour
{
	// Token: 0x06008DF7 RID: 36343 RVA: 0x003BB7B0 File Offset: 0x003B99B0
	private void OnEnable()
	{
		if (this.EeveeTex)
		{
			base.sprite.usesOverrideMaterial = true;
			base.sprite.renderer.material.shader = Shader.Find("Brave/Internal/GlitchEevee");
			base.sprite.renderer.sharedMaterial.SetTexture("_EeveeTex", this.EeveeTex);
		}
		base.spriteAnimator.Play(this.appearAnimation);
		base.StartCoroutine(this.HandleCoreIdle());
	}

	// Token: 0x06008DF8 RID: 36344 RVA: 0x003BB838 File Offset: 0x003B9A38
	private void OnDisable()
	{
		base.spriteAnimator.StopAndResetFrame();
		base.StopAllCoroutines();
	}

	// Token: 0x06008DF9 RID: 36345 RVA: 0x003BB84C File Offset: 0x003B9A4C
	private IEnumerator HandleCoreIdle()
	{
		while (base.spriteAnimator.IsPlaying(this.appearAnimation))
		{
			yield return null;
		}
		if (!this.usesMultipleIdleAnimations)
		{
			base.spriteAnimator.Play(this.coreIdleAnimation);
			yield break;
		}
		for (;;)
		{
			float duration = UnityEngine.Random.Range(this.idleMin, this.idleMax);
			float elapsed = 0f;
			base.spriteAnimator.Play(this.multipleIdleAnimations[UnityEngine.Random.Range(0, this.multipleIdleAnimations.Length)]);
			while (elapsed < duration)
			{
				elapsed += BraveTime.DeltaTime;
				yield return null;
			}
		}
	}

	// Token: 0x06008DFA RID: 36346 RVA: 0x003BB868 File Offset: 0x003B9A68
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x040095CD RID: 38349
	public string appearAnimation = "_appear";

	// Token: 0x040095CE RID: 38350
	public string coreIdleAnimation;

	// Token: 0x040095CF RID: 38351
	public float idleMin = 4f;

	// Token: 0x040095D0 RID: 38352
	public float idleMax = 10f;

	// Token: 0x040095D1 RID: 38353
	public bool usesMultipleIdleAnimations;

	// Token: 0x040095D2 RID: 38354
	public string[] multipleIdleAnimations;

	// Token: 0x040095D3 RID: 38355
	public Texture2D EeveeTex;

	// Token: 0x040095D4 RID: 38356
	protected int lastPhase = -1;
}
