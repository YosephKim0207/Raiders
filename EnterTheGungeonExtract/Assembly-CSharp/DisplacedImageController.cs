using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200108C RID: 4236
public class DisplacedImageController : BraveBehaviour
{
	// Token: 0x17000DAF RID: 3503
	// (get) Token: 0x06005D30 RID: 23856 RVA: 0x0023BCDC File Offset: 0x00239EDC
	// (set) Token: 0x06005D31 RID: 23857 RVA: 0x0023BCE4 File Offset: 0x00239EE4
	public float Fade
	{
		get
		{
			return this.m_fade;
		}
		set
		{
			if (this.m_fade != value)
			{
				this.m_fade = value;
				this.OnFadeChange(base.aiActor, Mathf.Clamp(this.m_fade, 0f, 0.85f), false);
				this.OnFadeChange(this.m_host, 1f - this.m_fade, true);
			}
		}
	}

	// Token: 0x06005D32 RID: 23858 RVA: 0x0023BD40 File Offset: 0x00239F40
	public void Update()
	{
		if (this.m_unfadeDelayTimer > 0f)
		{
			this.m_unfadeDelayTimer = Mathf.Max(0f, this.m_unfadeDelayTimer - BraveTime.DeltaTime);
		}
		else if (this.Fade > 0f)
		{
			this.Fade -= BraveTime.DeltaTime / this.FadeRecoveryTime;
		}
	}

	// Token: 0x06005D33 RID: 23859 RVA: 0x0023BDA8 File Offset: 0x00239FA8
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this.ClearHost();
		if (base.healthHaver)
		{
			base.healthHaver.OnPreDeath -= this.OnImagePreDeath;
			base.healthHaver.OnDeath -= this.OnImageDeath;
		}
	}

	// Token: 0x06005D34 RID: 23860 RVA: 0x0023BE00 File Offset: 0x0023A000
	public void Init()
	{
		if (this.m_initialized)
		{
			return;
		}
		base.aiActor.CanDropCurrency = false;
		base.aiActor.CanDropItems = false;
		base.aiActor.CollisionDamage = 0f;
		base.aiActor.MovementSpeed = 6f;
		base.aiActor.CorpseObject = null;
		base.aiActor.shadowDeathType = AIActor.ShadowDeathType.None;
		if (base.aiActor.encounterTrackable)
		{
			UnityEngine.Object.Destroy(base.aiActor.encounterTrackable);
		}
		base.healthHaver.OnPreDeath += this.OnImagePreDeath;
		this.m_lastImageHealth = base.healthHaver.GetMaxHealth();
		base.aiAnimator.OtherAnimations[3].anim.Prefix = "poof";
		base.RegenerateCache();
		SeekTargetBehavior seekTargetBehavior = new SeekTargetBehavior();
		seekTargetBehavior.StopWhenInRange = false;
		base.behaviorSpeculator.InstantFirstTick = true;
		base.behaviorSpeculator.PostAwakenDelay = 0f;
		base.behaviorSpeculator.MovementBehaviors[0] = seekTargetBehavior;
		AttackBehaviorGroup attackBehaviorGroup = base.behaviorSpeculator.AttackBehaviorGroup;
		if (attackBehaviorGroup != null)
		{
			attackBehaviorGroup.AttackBehaviors[0].Probability = 0f;
			attackBehaviorGroup.AttackBehaviors[1].Probability = 1f;
		}
		BulletLimbController[] componentsInChildren = base.GetComponentsInChildren<BulletLimbController>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].enabled = true;
		}
		this.Fade = 0f;
		base.aiActor.SetOutlines(true);
		this.UpdateOutlineMaterial(base.sprite);
		base.healthHaver.OnDeath += this.OnImageDeath;
		this.m_initialized = true;
	}

	// Token: 0x06005D35 RID: 23861 RVA: 0x0023BFBC File Offset: 0x0023A1BC
	public void SetHost(AIActor host)
	{
		this.m_host = host;
		if (!this.m_host)
		{
			return;
		}
		base.aiAnimator.CopyStateFrom(this.m_host.aiAnimator);
		this.m_lastHostHealth = host.healthHaver.GetMaxHealth();
		this.m_host.healthHaver.OnPreDeath += this.OnHostPreDeath;
		this.m_host.healthHaver.OnDamaged += this.OnHostDamaged;
		base.healthHaver.OnDamaged += this.OnImageDamaged;
		host.SetOutlines(true);
		this.UpdateOutlineMaterial(host.sprite);
		this.OnFadeChange(this.m_host, 1f - this.Fade, true);
	}

	// Token: 0x06005D36 RID: 23862 RVA: 0x0023C084 File Offset: 0x0023A284
	public void ClearHost()
	{
		if (this.m_host == null)
		{
			return;
		}
		this.m_host.healthHaver.OnPreDeath -= this.OnHostPreDeath;
		this.m_host.healthHaver.OnDamaged -= this.OnHostDamaged;
		base.healthHaver.OnDamaged -= this.OnImageDamaged;
		this.OnFadeChange(this.m_host, 0f, true);
		this.m_host = null;
	}

	// Token: 0x06005D37 RID: 23863 RVA: 0x0023C10C File Offset: 0x0023A30C
	private void OnHostPreDeath(Vector2 deathDir)
	{
		this.ClearHost();
		base.healthHaver.ApplyDamage(100000f, Vector2.zero, "Mirror Host Death", CoreDamageTypes.None, DamageCategory.Unstoppable, false, null, false);
	}

	// Token: 0x06005D38 RID: 23864 RVA: 0x0023C140 File Offset: 0x0023A340
	private void OnImagePreDeath(Vector2 deathDir)
	{
		this.OnFadeChange(base.aiActor, 0f, false);
		base.StartCoroutine(this.DeathFade());
	}

	// Token: 0x06005D39 RID: 23865 RVA: 0x0023C164 File Offset: 0x0023A364
	private void OnHostDamaged(float resultValue, float maxValue, CoreDamageTypes damagetypes, DamageCategory damagecategory, Vector2 damagedirection)
	{
		float num = this.m_lastHostHealth - resultValue;
		this.OnEitherDamaged(num, maxValue);
		this.m_lastHostHealth = resultValue;
	}

	// Token: 0x06005D3A RID: 23866 RVA: 0x0023C18C File Offset: 0x0023A38C
	private void OnImageDamaged(float resultValue, float maxValue, CoreDamageTypes damagetypes, DamageCategory damagecategory, Vector2 damagedirection)
	{
		float num = this.m_lastImageHealth - resultValue;
		this.OnEitherDamaged(num, maxValue);
		this.m_lastImageHealth = resultValue;
	}

	// Token: 0x06005D3B RID: 23867 RVA: 0x0023C1B4 File Offset: 0x0023A3B4
	private void OnImageDeath(Vector2 vector2)
	{
		base.aiAnimator.PlayVfx("death_poof", null, null, null);
	}

	// Token: 0x06005D3C RID: 23868 RVA: 0x0023C1EC File Offset: 0x0023A3EC
	private void OnFadeChange(AIActor aiActor, float fade, bool isHost)
	{
		if (!aiActor)
		{
			return;
		}
		aiActor.renderer.material.SetFloat("_DisplacerFade", fade * 1.5f);
		aiActor.sprite.usesOverrideMaterial = fade > 0f;
		if (isHost)
		{
			if (fade <= 0f)
			{
				aiActor.SetOutlines(true);
				this.UpdateOutlineMaterial(aiActor.sprite);
			}
			else
			{
				bool flag = false;
				for (int i = 0; i < GameManager.Instance.AllPlayers.Length; i++)
				{
					PlayerController playerController = GameManager.Instance.AllPlayers[i];
					if (playerController && playerController.CanDetectHiddenEnemies)
					{
						flag = true;
					}
				}
				if (!flag)
				{
					aiActor.SetOutlines(false);
				}
			}
		}
		tk2dSprite component = aiActor.ShadowObject.GetComponent<tk2dSprite>();
		component.color = component.color.WithAlpha(1f - fade);
	}

	// Token: 0x06005D3D RID: 23869 RVA: 0x0023C2D4 File Offset: 0x0023A4D4
	private void OnEitherDamaged(float damage, float maxHealth)
	{
		float num = damage / maxHealth / this.DamagePercentForMaxFade;
		this.Fade = Mathf.Clamp01(this.Fade + num);
		this.m_unfadeDelayTimer = this.UnfadeDelayTime;
	}

	// Token: 0x06005D3E RID: 23870 RVA: 0x0023C30C File Offset: 0x0023A50C
	private IEnumerator DeathFade()
	{
		tk2dSprite shadowSprite = base.aiActor.ShadowObject.GetComponent<tk2dSprite>();
		float startAlpha = shadowSprite.color.a;
		for (;;)
		{
			shadowSprite.color = shadowSprite.color.WithAlpha(startAlpha * (1f - base.aiAnimator.CurrentClipProgress));
			yield return null;
		}
		yield break;
	}

	// Token: 0x06005D3F RID: 23871 RVA: 0x0023C328 File Offset: 0x0023A528
	private void UpdateOutlineMaterial(tk2dBaseSprite sprite)
	{
		Material outlineMaterial = SpriteOutlineManager.GetOutlineMaterial(sprite);
		outlineMaterial.SetColor("_OverrideColor", new Color(0f, 11f, 33f));
		outlineMaterial.EnableKeyword("EXCLUDE_INTERIOR");
		outlineMaterial.DisableKeyword("INCLUDE_INTERIOR");
	}

	// Token: 0x0400571A RID: 22298
	public float DamagePercentForMaxFade = 0.7f;

	// Token: 0x0400571B RID: 22299
	public float UnfadeDelayTime = 1f;

	// Token: 0x0400571C RID: 22300
	public float FadeRecoveryTime = 1f;

	// Token: 0x0400571D RID: 22301
	private bool m_initialized;

	// Token: 0x0400571E RID: 22302
	private AIActor m_host;

	// Token: 0x0400571F RID: 22303
	private float m_lastHostHealth;

	// Token: 0x04005720 RID: 22304
	private float m_lastImageHealth;

	// Token: 0x04005721 RID: 22305
	private float m_fade = -1f;

	// Token: 0x04005722 RID: 22306
	private float m_unfadeDelayTimer;
}
