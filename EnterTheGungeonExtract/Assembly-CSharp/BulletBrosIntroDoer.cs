using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001000 RID: 4096
[RequireComponent(typeof(GenericIntroDoer))]
public class BulletBrosIntroDoer : SpecificIntroDoer
{
	// Token: 0x06005996 RID: 22934 RVA: 0x00223510 File Offset: 0x00221710
	public void Update()
	{
		if (!this.m_initialized && base.aiActor.ShadowObject)
		{
			this.m_smiley = base.aiAnimator;
			for (int i = 0; i < StaticReferenceManager.AllBros.Count; i++)
			{
				if (StaticReferenceManager.AllBros[i].gameObject != base.gameObject)
				{
					this.m_shades = StaticReferenceManager.AllBros[i].aiAnimator;
					break;
				}
			}
			this.m_smiley.aiActor.ToggleRenderers(false);
			this.m_smiley.aiShooter.ToggleGunAndHandRenderers(false, "BulletBrosIntroDoer");
			this.m_smiley.transform.position += PhysicsEngine.PixelToUnit(new IntVector2(11, 0)).ToVector3ZUp(0f);
			this.m_smiley.specRigidbody.Reinitialize();
			this.m_shades.aiActor.ToggleRenderers(false);
			this.m_shades.aiShooter.ToggleGunAndHandRenderers(false, "BulletBrosIntroDoer");
			this.m_shades.transform.position += PhysicsEngine.PixelToUnit(new IntVector2(-11, 0)).ToVector3ZUp(0f);
			this.m_shades.specRigidbody.Reinitialize();
			this.m_smileyShadow = this.m_smiley.aiActor.ShadowObject.GetComponent<tk2dBaseSprite>();
			this.m_smileyShadow.renderer.enabled = false;
			this.m_shadesShadow = this.m_shades.aiActor.ShadowObject.GetComponent<tk2dBaseSprite>();
			this.m_shadesShadow.renderer.enabled = false;
			this.m_initialized = true;
		}
	}

	// Token: 0x06005997 RID: 22935 RVA: 0x002236D0 File Offset: 0x002218D0
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x17000CF1 RID: 3313
	// (get) Token: 0x06005998 RID: 22936 RVA: 0x002236D8 File Offset: 0x002218D8
	public override Vector2? OverrideIntroPosition
	{
		get
		{
			return new Vector2?(0.5f * (this.m_smiley.specRigidbody.GetUnitCenter(ColliderType.HitBox) + this.m_shades.specRigidbody.GetUnitCenter(ColliderType.HitBox)));
		}
	}

	// Token: 0x06005999 RID: 22937 RVA: 0x00223710 File Offset: 0x00221910
	public override void PlayerWalkedIn(PlayerController player, List<tk2dSpriteAnimator> animators)
	{
		base.aiAnimator.LockFacingDirection = true;
		base.aiAnimator.FacingDirection = -90f;
		if (this.m_smiley && this.m_shades)
		{
			this.m_smiley.aiActor.ToggleRenderers(false);
			SpriteOutlineManager.ToggleOutlineRenderers(this.m_smiley.sprite, false);
			this.m_smiley.aiShooter.ToggleGunAndHandRenderers(false, "BulletBrosIntroDoer");
			this.m_smileyShadow.renderer.enabled = false;
			this.m_shades.aiActor.ToggleRenderers(false);
			SpriteOutlineManager.ToggleOutlineRenderers(this.m_shades.sprite, false);
			this.m_shades.aiShooter.ToggleGunAndHandRenderers(false, "BulletBrosIntroDoer");
			this.m_shadesShadow.renderer.enabled = false;
		}
		base.StartCoroutine(this.FuckOutlines());
	}

	// Token: 0x0600599A RID: 22938 RVA: 0x002237F8 File Offset: 0x002219F8
	private IEnumerator FuckOutlines()
	{
		yield return null;
		SpriteOutlineManager.ToggleOutlineRenderers(this.m_smiley.sprite, false);
		SpriteOutlineManager.ToggleOutlineRenderers(this.m_shades.sprite, false);
		yield break;
	}

	// Token: 0x0600599B RID: 22939 RVA: 0x00223814 File Offset: 0x00221A14
	public override void StartIntro(List<tk2dSpriteAnimator> animators)
	{
		base.StartCoroutine(this.DoIntro());
		animators.Add(this.m_shades.spriteAnimator);
		animators.Add(this.shadowDummy);
	}

	// Token: 0x17000CF2 RID: 3314
	// (get) Token: 0x0600599C RID: 22940 RVA: 0x00223840 File Offset: 0x00221A40
	public override bool IsIntroFinished
	{
		get
		{
			return this.m_finished;
		}
	}

	// Token: 0x0600599D RID: 22941 RVA: 0x00223848 File Offset: 0x00221A48
	public override void OnBossCard()
	{
		this.m_smileyShadow.renderer.enabled = true;
		this.m_shadesShadow.renderer.enabled = true;
		this.m_smiley.aiShooter.ToggleGunAndHandRenderers(true, "BulletBrosIntroDoer");
		this.m_shades.aiShooter.ToggleGunAndHandRenderers(true, "BulletBrosIntroDoer");
	}

	// Token: 0x0600599E RID: 22942 RVA: 0x002238A4 File Offset: 0x00221AA4
	public override void EndIntro()
	{
		this.m_finished = true;
		base.StopAllCoroutines();
		this.m_smiley.aiActor.ToggleRenderers(true);
		SpriteOutlineManager.ToggleOutlineRenderers(this.m_smiley.sprite, true);
		this.m_smiley.sprite.renderer.enabled = true;
		this.m_smiley.EndAnimation();
		this.m_smiley.aiShooter.ToggleGunAndHandRenderers(true, "BulletBrosIntroDoer");
		this.m_smiley.specRigidbody.CollideWithOthers = true;
		this.m_smiley.aiActor.IsGone = false;
		this.m_smiley.aiActor.State = AIActor.ActorState.Normal;
		this.m_smiley.aiShooter.AimAtPoint(this.m_smiley.aiActor.CenterPosition + new Vector2(10f, -2f));
		this.m_smiley.FacingDirection = -90f;
		this.m_shades.aiActor.ToggleRenderers(true);
		SpriteOutlineManager.ToggleOutlineRenderers(this.m_shades.sprite, true);
		this.m_shades.sprite.renderer.enabled = true;
		this.m_shades.EndAnimation();
		this.m_shades.aiShooter.ToggleGunAndHandRenderers(true, "BulletBrosIntroDoer");
		this.m_shades.specRigidbody.CollideWithOthers = true;
		this.m_shades.aiActor.IsGone = false;
		this.m_shades.aiActor.State = AIActor.ActorState.Normal;
		this.m_shades.aiShooter.AimAtPoint(this.m_shades.aiActor.CenterPosition + new Vector2(-10f, -2f));
		this.m_shades.FacingDirection = -90f;
		this.shadowDummy.renderer.enabled = false;
	}

	// Token: 0x0600599F RID: 22943 RVA: 0x00223A74 File Offset: 0x00221C74
	private IEnumerator DoIntro()
	{
		SpriteOutlineManager.ToggleOutlineRenderers(this.m_smiley.sprite, false);
		SpriteOutlineManager.ToggleOutlineRenderers(this.m_shades.sprite, false);
		float elapsed = 0f;
		float duration = 1f;
		while (elapsed < duration)
		{
			this.m_smiley.aiShooter.ToggleGunAndHandRenderers(false, "BulletBrosIntroDoer");
			this.m_shades.aiShooter.ToggleGunAndHandRenderers(false, "BulletBrosIntroDoer");
			this.m_smileyShadow.renderer.enabled = false;
			this.m_shadesShadow.renderer.enabled = false;
			yield return null;
			elapsed += GameManager.INVARIANT_DELTA_TIME;
		}
		this.m_smiley.aiActor.ToggleRenderers(true);
		this.m_smiley.PlayUntilFinished("intro", false, null, -1f, false);
		SpriteOutlineManager.ToggleOutlineRenderers(this.m_smiley.sprite, true);
		this.m_shades.aiActor.ToggleRenderers(true);
		this.m_shades.PlayUntilFinished("intro", false, null, -1f, false);
		SpriteOutlineManager.ToggleOutlineRenderers(this.m_shades.sprite, true);
		this.shadowDummy.Play();
		while (this.m_smiley.IsPlaying("intro"))
		{
			this.m_smiley.aiShooter.ToggleGunAndHandRenderers(false, "BulletBrosIntroDoer");
			this.m_shades.aiShooter.ToggleGunAndHandRenderers(false, "BulletBrosIntroDoer");
			this.m_smileyShadow.renderer.enabled = false;
			this.m_shadesShadow.renderer.enabled = false;
			yield return null;
		}
		this.shadowDummy.renderer.enabled = false;
		this.m_smileyShadow.renderer.enabled = true;
		this.m_smiley.PlayUntilFinished("idle", false, null, -1f, false);
		this.m_smiley.aiShooter.ToggleGunAndHandRenderers(true, "BulletBrosIntroDoer");
		this.m_smiley.aiShooter.AimAtPoint(this.m_smiley.aiActor.CenterPosition + new Vector2(10f, -2f));
		this.m_shadesShadow.renderer.enabled = true;
		this.m_shades.PlayUntilFinished("idle", false, null, -1f, false);
		this.m_shades.aiShooter.ToggleGunAndHandRenderers(true, "BulletBrosIntroDoer");
		this.m_shades.aiShooter.AimAtPoint(this.m_shades.aiActor.CenterPosition + new Vector2(-10f, -2f));
		elapsed = 0f;
		duration = 1f;
		while (elapsed < duration)
		{
			yield return null;
			elapsed += GameManager.INVARIANT_DELTA_TIME;
		}
		this.m_finished = true;
		yield break;
	}

	// Token: 0x040052ED RID: 21229
	public tk2dSpriteAnimator shadowDummy;

	// Token: 0x040052EE RID: 21230
	private bool m_initialized;

	// Token: 0x040052EF RID: 21231
	private bool m_finished;

	// Token: 0x040052F0 RID: 21232
	private AIAnimator m_smiley;

	// Token: 0x040052F1 RID: 21233
	private tk2dBaseSprite m_smileyShadow;

	// Token: 0x040052F2 RID: 21234
	private AIAnimator m_shades;

	// Token: 0x040052F3 RID: 21235
	private tk2dBaseSprite m_shadesShadow;
}
