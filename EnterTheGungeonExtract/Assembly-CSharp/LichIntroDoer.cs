using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200105A RID: 4186
[RequireComponent(typeof(GenericIntroDoer))]
public class LichIntroDoer : SpecificIntroDoer
{
	// Token: 0x06005C07 RID: 23559 RVA: 0x0023496C File Offset: 0x00232B6C
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x17000D79 RID: 3449
	// (get) Token: 0x06005C08 RID: 23560 RVA: 0x00234974 File Offset: 0x00232B74
	public override string OverrideBossMusicEvent
	{
		get
		{
			return (!GameManager.IsGunslingerPast) ? null : "Play_MUS_Lich_Double_01";
		}
	}

	// Token: 0x06005C09 RID: 23561 RVA: 0x0023498C File Offset: 0x00232B8C
	public override void PlayerWalkedIn(PlayerController player, List<tk2dSpriteAnimator> animators)
	{
		LichIntroDoer.DoubleLich = GameManager.IsGunslingerPast;
		if (!LichIntroDoer.DoubleLich)
		{
			return;
		}
		base.aiActor.PreventBlackPhantom = true;
		this.m_otherLich = AIActor.Spawn(EnemyDatabase.GetOrLoadByGuid(base.aiActor.EnemyGuid), base.specRigidbody.UnitBottomLeft, base.aiActor.ParentRoom, false, AIActor.AwakenAnimationType.Default, false);
		this.m_otherLich.transform.position = base.transform.position + new Vector3(0.25f, 0.25f, 0f);
		this.m_otherLich.specRigidbody.Reinitialize();
		this.m_otherLich.OverrideBlackPhantomShader = ShaderCache.Acquire("Brave/PlayerShaderEevee");
		this.m_otherLich.ForceBlackPhantom = true;
		this.m_otherLich.sprite.renderer.material.SetTexture("_EeveeTex", this.CosmicTex);
		this.m_otherLich.sprite.renderer.material.DisableKeyword("BRIGHTNESS_CLAMP_ON");
		this.m_otherLich.sprite.renderer.material.EnableKeyword("BRIGHTNESS_CLAMP_OFF");
		animators.Add(this.m_otherLich.spriteAnimator);
		this.m_otherLich.aiAnimator.PlayUntilCancelled("preintro", false, null, -1f, false);
		base.StartCoroutine(this.HandleDelayedTextureCR());
	}

	// Token: 0x06005C0A RID: 23562 RVA: 0x00234AF4 File Offset: 0x00232CF4
	private IEnumerator HandleDelayedTextureCR()
	{
		yield return null;
		this.m_otherLich.sprite.renderer.material.SetTexture("_EeveeTex", this.CosmicTex);
		yield break;
	}

	// Token: 0x06005C0B RID: 23563 RVA: 0x00234B10 File Offset: 0x00232D10
	public override void StartIntro(List<tk2dSpriteAnimator> animators)
	{
		if (!LichIntroDoer.DoubleLich)
		{
			return;
		}
		this.m_otherLich.aiAnimator.PlayUntilCancelled("intro", false, null, -1f, false);
	}

	// Token: 0x06005C0C RID: 23564 RVA: 0x00234B3C File Offset: 0x00232D3C
	public override void OnCameraOutro()
	{
		if (!LichIntroDoer.DoubleLich)
		{
			return;
		}
		base.aiAnimator.FacingDirection = -90f;
		base.aiAnimator.PlayUntilCancelled("idle", false, null, -1f, false);
		this.m_otherLich.aiAnimator.FacingDirection = -90f;
		this.m_otherLich.aiAnimator.PlayUntilCancelled("idle", false, null, -1f, false);
	}

	// Token: 0x040055BB RID: 21947
	public static bool DoubleLich;

	// Token: 0x040055BC RID: 21948
	public tk2dSprite HandSprite;

	// Token: 0x040055BD RID: 21949
	public Texture2D CosmicTex;

	// Token: 0x040055BE RID: 21950
	private AIActor m_otherLich;
}
