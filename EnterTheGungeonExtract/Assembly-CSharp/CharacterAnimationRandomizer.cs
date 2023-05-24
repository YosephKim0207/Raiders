using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200128A RID: 4746
public class CharacterAnimationRandomizer : MonoBehaviour
{
	// Token: 0x06006A3D RID: 27197 RVA: 0x0029A788 File Offset: 0x00298988
	public void Start()
	{
		this.m_player = base.GetComponent<PlayerController>();
		this.m_sprite = this.m_player.sprite;
		this.m_animator = this.m_player.spriteAnimator;
		this.m_material = this.m_sprite.renderer.sharedMaterial;
		this.m_shaderID = Shader.PropertyToID("_EeveeColor");
		this.m_material.SetTexture("_EeveeTex", this.CosmicTex);
		tk2dSpriteAnimator animator = this.m_animator;
		animator.AnimationCompleted = (Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip>)Delegate.Combine(animator.AnimationCompleted, new Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip>(this.HandleAnimationCompletedSwap));
	}

	// Token: 0x06006A3E RID: 27198 RVA: 0x0029A828 File Offset: 0x00298A28
	private void HandleAnimationCompletedSwap(tk2dSpriteAnimator arg1, tk2dSpriteAnimationClip arg2)
	{
		if (!this.m_player.IsVisible)
		{
			return;
		}
		int num = UnityEngine.Random.Range(0, this.AnimationLibraries.Count);
		this.m_animator.Library = this.AnimationLibraries[num];
		this.m_material.SetColor(this.m_shaderID, this.PrimaryColors[Mathf.Min(num, this.PrimaryColors.Length - 1)]);
		this.m_material.SetTexture("_EeveeTex", this.CosmicTex);
	}

	// Token: 0x06006A3F RID: 27199 RVA: 0x0029A8B8 File Offset: 0x00298AB8
	public void AddOverrideAnimLibrary(tk2dSpriteAnimation library)
	{
		if (!this.AnimationLibraries.Contains(library))
		{
			this.AnimationLibraries.Add(library);
		}
	}

	// Token: 0x06006A40 RID: 27200 RVA: 0x0029A8D8 File Offset: 0x00298AD8
	public void RemoveOverrideAnimLibrary(tk2dSpriteAnimation library)
	{
		if (this.AnimationLibraries.Contains(library))
		{
			this.AnimationLibraries.Remove(library);
		}
	}

	// Token: 0x040066B3 RID: 26291
	public List<tk2dSpriteAnimation> AnimationLibraries;

	// Token: 0x040066B4 RID: 26292
	public Color[] PrimaryColors;

	// Token: 0x040066B5 RID: 26293
	public Texture2D CosmicTex;

	// Token: 0x040066B6 RID: 26294
	private PlayerController m_player;

	// Token: 0x040066B7 RID: 26295
	private tk2dBaseSprite m_sprite;

	// Token: 0x040066B8 RID: 26296
	private tk2dSpriteAnimator m_animator;

	// Token: 0x040066B9 RID: 26297
	private Material m_material;

	// Token: 0x040066BA RID: 26298
	private int m_shaderID;
}
