using System;
using FullInspector;
using UnityEngine;

// Token: 0x02000D6A RID: 3434
public class TransformBehavior : BasicAttackBehavior
{
	// Token: 0x0600488B RID: 18571 RVA: 0x001812DC File Offset: 0x0017F4DC
	private bool ShowShadowAnimationNames()
	{
		return this.shadowSupport == TransformBehavior.ShadowSupport.Animate;
	}

	// Token: 0x0600488C RID: 18572 RVA: 0x001812E8 File Offset: 0x0017F4E8
	private bool ShowReflectBullets()
	{
		return this.invulnerabilityMode != TransformBehavior.Invulnerability.None;
	}

	// Token: 0x0600488D RID: 18573 RVA: 0x001812F8 File Offset: 0x0017F4F8
	public override void Start()
	{
		base.Start();
		tk2dSpriteAnimator spriteAnimator = this.m_aiActor.spriteAnimator;
		spriteAnimator.AnimationEventTriggered = (Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip, int>)Delegate.Combine(spriteAnimator.AnimationEventTriggered, new Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip, int>(this.AnimationEventTriggered));
		if (this.invulnerabilityMode != TransformBehavior.Invulnerability.None)
		{
			for (int i = 0; i < this.m_aiActor.specRigidbody.PixelColliders.Count; i++)
			{
				PixelCollider pixelCollider = this.m_aiActor.specRigidbody.PixelColliders[i];
				if (pixelCollider.CollisionLayer == CollisionLayer.EnemyHitBox)
				{
					this.m_enemyHitbox = pixelCollider;
				}
				if (pixelCollider.CollisionLayer == CollisionLayer.BulletBlocker)
				{
					this.m_bulletBlocker = pixelCollider;
				}
			}
			if (this.invulnerabilityMode == TransformBehavior.Invulnerability.WhileTransformed)
			{
				this.Invulnerable = false;
			}
			else if (this.invulnerabilityMode == TransformBehavior.Invulnerability.WhileNotTransformed)
			{
				this.Invulnerable = true;
			}
		}
	}

	// Token: 0x0600488E RID: 18574 RVA: 0x001813D4 File Offset: 0x0017F5D4
	public override void Upkeep()
	{
		base.Upkeep();
		base.DecrementTimer(ref this.m_timer, false);
	}

	// Token: 0x0600488F RID: 18575 RVA: 0x001813EC File Offset: 0x0017F5EC
	public override BehaviorResult Update()
	{
		base.Update();
		if (this.m_shadowSprite == null)
		{
			this.m_shadowSprite = this.m_aiActor.ShadowObject.GetComponent<tk2dBaseSprite>();
		}
		if (!this.IsReady())
		{
			return BehaviorResult.Continue;
		}
		if (!this.m_aiActor.TargetRigidbody)
		{
			return BehaviorResult.Continue;
		}
		this.State = TransformBehavior.TransformState.InTrans;
		this.m_updateEveryFrame = true;
		return BehaviorResult.RunContinuous;
	}

	// Token: 0x06004890 RID: 18576 RVA: 0x0018145C File Offset: 0x0017F65C
	public override ContinuousBehaviorResult ContinuousUpdate()
	{
		base.ContinuousUpdate();
		if (this.State == TransformBehavior.TransformState.InTrans)
		{
			if (this.shadowSupport == TransformBehavior.ShadowSupport.Fade)
			{
				this.m_shadowSprite.color = this.m_shadowSprite.color.WithAlpha(1f - this.m_aiAnimator.CurrentClipProgress);
			}
			if (!this.m_hasTransitioned && this.m_aiAnimator.CurrentClipProgress > 0.5f)
			{
				if (this.invulnerabilityMode == TransformBehavior.Invulnerability.WhileTransformed)
				{
					this.Invulnerable = true;
				}
				else if (this.invulnerabilityMode == TransformBehavior.Invulnerability.WhileNotTransformed)
				{
					this.Invulnerable = false;
				}
				this.m_hasTransitioned = true;
			}
			if (!this.m_aiAnimator.IsPlaying(this.inAnim))
			{
				this.State = ((this.transformedTime <= 0f) ? TransformBehavior.TransformState.OutTrans : TransformBehavior.TransformState.Transformed);
			}
		}
		else if (this.State == TransformBehavior.TransformState.Transformed)
		{
			if (this.m_timer <= 0f)
			{
				this.State = TransformBehavior.TransformState.OutTrans;
			}
		}
		else if (this.State == TransformBehavior.TransformState.OutTrans)
		{
			if (this.shadowSupport == TransformBehavior.ShadowSupport.Fade)
			{
				this.m_shadowSprite.color = this.m_shadowSprite.color.WithAlpha(this.m_aiAnimator.CurrentClipProgress);
			}
			if (!this.m_hasTransitioned && this.m_aiAnimator.CurrentClipProgress > 0.5f)
			{
				if (this.invulnerabilityMode == TransformBehavior.Invulnerability.WhileTransformed)
				{
					this.Invulnerable = false;
				}
				else if (this.invulnerabilityMode == TransformBehavior.Invulnerability.WhileNotTransformed)
				{
					this.Invulnerable = true;
				}
				this.m_hasTransitioned = true;
			}
			if (this.m_aiShooter)
			{
				this.m_aiShooter.ToggleGunAndHandRenderers(false, "TransformBehavior");
			}
			if (!this.m_aiAnimator.IsPlaying(this.outAnim))
			{
				this.State = TransformBehavior.TransformState.None;
				return ContinuousBehaviorResult.Finished;
			}
		}
		return ContinuousBehaviorResult.Continue;
	}

	// Token: 0x06004891 RID: 18577 RVA: 0x00181640 File Offset: 0x0017F840
	public override void EndContinuousUpdate()
	{
		base.EndContinuousUpdate();
		if (this.invulnerabilityMode != TransformBehavior.Invulnerability.None)
		{
			if (this.invulnerabilityMode == TransformBehavior.Invulnerability.WhileTransformed)
			{
				this.Invulnerable = false;
			}
			else if (this.invulnerabilityMode == TransformBehavior.Invulnerability.WhileNotTransformed)
			{
				this.Invulnerable = true;
			}
		}
		if (this.m_aiShooter)
		{
			this.m_aiShooter.ToggleGunAndHandRenderers(true, "TransformBehavior");
		}
		if (this.m_bulletSource && !this.m_bulletSource.IsEnded)
		{
			this.m_bulletSource.ForceStop();
		}
		if (this.setTransformAnimAsBaseState && this.m_state == TransformBehavior.TransformState.Transformed)
		{
			this.m_aiAnimator.ClearBaseAnim();
		}
		this.m_aiAnimator.EndAnimationIf(this.inAnim);
		this.m_aiAnimator.EndAnimationIf(this.transformedAnim);
		this.m_aiAnimator.EndAnimationIf(this.outAnim);
		if (this.requiresTransparency && this.m_cachedShader)
		{
			this.m_aiActor.sprite.usesOverrideMaterial = false;
			this.m_aiActor.renderer.material.shader = this.m_cachedShader;
			this.m_cachedShader = null;
		}
		if (this.shadowSupport == TransformBehavior.ShadowSupport.Fade)
		{
			this.m_shadowSprite.color = this.m_shadowSprite.color.WithAlpha(1f);
		}
		else if (this.shadowSupport == TransformBehavior.ShadowSupport.Animate)
		{
			tk2dSpriteAnimationClip clipByName = this.m_shadowSprite.spriteAnimator.GetClipByName(this.shadowOutAnim);
			this.m_shadowSprite.spriteAnimator.Play(clipByName, (float)(clipByName.frames.Length - 1), clipByName.fps, false);
		}
		this.m_updateEveryFrame = false;
		this.UpdateCooldowns();
	}

	// Token: 0x06004892 RID: 18578 RVA: 0x00181808 File Offset: 0x0017FA08
	public override bool IsOverridable()
	{
		return !this.Uninterruptible;
	}

	// Token: 0x06004893 RID: 18579 RVA: 0x00181814 File Offset: 0x0017FA14
	public void AnimationEventTriggered(tk2dSpriteAnimator animator, tk2dSpriteAnimationClip clip, int frame)
	{
		string eventInfo = clip.GetFrame(frame).eventInfo;
		if (this.m_shouldFire && eventInfo == "fire")
		{
			if (this.State == TransformBehavior.TransformState.InTrans)
			{
				this.ShootBulletScript(this.inBulletScript);
			}
			else if (this.State == TransformBehavior.TransformState.Transformed)
			{
				this.ShootBulletScript(this.transformedBulletScript);
			}
			else if (this.State == TransformBehavior.TransformState.OutTrans)
			{
				this.ShootBulletScript(this.outBulletScript);
			}
			this.m_shouldFire = false;
		}
		if (this.m_state == TransformBehavior.TransformState.OutTrans || this.m_state == TransformBehavior.TransformState.InTrans)
		{
			if (eventInfo == "collider_on")
			{
				this.m_aiActor.IsGone = false;
				this.m_aiActor.specRigidbody.CollideWithOthers = true;
			}
			else if (eventInfo == "collider_off")
			{
				this.m_aiActor.IsGone = true;
				this.m_aiActor.specRigidbody.CollideWithOthers = false;
			}
		}
	}

	// Token: 0x06004894 RID: 18580 RVA: 0x0018191C File Offset: 0x0017FB1C
	private void ShootBulletScript(BulletScriptSelector script)
	{
		if (!this.m_bulletSource)
		{
			this.m_bulletSource = this.shootPoint.GetOrAddComponent<BulletScriptSource>();
		}
		this.m_bulletSource.BulletManager = this.m_aiActor.bulletBank;
		this.m_bulletSource.BulletScript = script;
		this.m_bulletSource.Initialize();
	}

	// Token: 0x17000A76 RID: 2678
	// (get) Token: 0x06004895 RID: 18581 RVA: 0x00181978 File Offset: 0x0017FB78
	// (set) Token: 0x06004896 RID: 18582 RVA: 0x00181980 File Offset: 0x0017FB80
	private TransformBehavior.TransformState State
	{
		get
		{
			return this.m_state;
		}
		set
		{
			this.EndState(this.m_state);
			this.m_state = value;
			this.BeginState(this.m_state);
		}
	}

	// Token: 0x06004897 RID: 18583 RVA: 0x001819A4 File Offset: 0x0017FBA4
	private void BeginState(TransformBehavior.TransformState state)
	{
		this.m_hasTransitioned = false;
		if (state == TransformBehavior.TransformState.InTrans)
		{
			if (this.inBulletScript != null && !this.inBulletScript.IsNull)
			{
				this.m_shouldFire = true;
			}
			if (this.requiresTransparency)
			{
				this.m_cachedShader = this.m_aiActor.renderer.material.shader;
				this.m_aiActor.sprite.usesOverrideMaterial = true;
				this.m_aiActor.renderer.material.shader = ShaderCache.Acquire("Brave/LitBlendUber");
			}
			this.m_aiAnimator.PlayUntilCancelled(this.inAnim, true, null, -1f, false);
			if (this.shadowSupport == TransformBehavior.ShadowSupport.Animate)
			{
				this.m_shadowSprite.spriteAnimator.PlayAndForceTime(this.shadowInAnim, this.m_aiAnimator.CurrentClipLength);
			}
			this.m_aiActor.ClearPath();
			if (this.m_aiShooter)
			{
				this.m_aiShooter.ToggleGunAndHandRenderers(false, "TransformBehavior");
			}
		}
		else if (state == TransformBehavior.TransformState.Transformed)
		{
			this.m_timer = this.transformedTime;
			if (this.transformedBulletScript != null && !this.transformedBulletScript.IsNull)
			{
				this.m_shouldFire = true;
			}
			if (!string.IsNullOrEmpty(this.transformedAnim))
			{
				this.m_aiAnimator.PlayUntilCancelled(this.transformedAnim, false, null, -1f, false);
			}
			if (this.setTransformAnimAsBaseState)
			{
				this.m_aiAnimator.SetBaseAnim(this.transformedAnim, false);
			}
			if (this.goneWhileTransformed)
			{
				this.m_aiActor.IsGone = true;
				this.m_aiActor.specRigidbody.CollideWithOthers = false;
			}
			if (this.transformFireImmediately && this.transformedBulletScript != null && !this.transformedBulletScript.IsNull)
			{
				this.ShootBulletScript(this.transformedBulletScript);
				this.m_shouldFire = false;
			}
		}
		else if (state == TransformBehavior.TransformState.OutTrans)
		{
			if (this.outBulletScript != null && !this.outBulletScript.IsNull)
			{
				this.m_shouldFire = true;
			}
			this.m_aiAnimator.PlayUntilFinished(this.outAnim, true, null, -1f, false);
			if (this.shadowSupport == TransformBehavior.ShadowSupport.Animate)
			{
				this.m_shadowSprite.spriteAnimator.PlayAndForceTime(this.shadowOutAnim, this.m_aiAnimator.CurrentClipLength);
			}
			if (this.m_aiShooter)
			{
				this.m_aiShooter.ToggleGunAndHandRenderers(false, "TransformBehavior");
			}
		}
	}

	// Token: 0x06004898 RID: 18584 RVA: 0x00181C24 File Offset: 0x0017FE24
	private void EndState(TransformBehavior.TransformState state)
	{
		if (state == TransformBehavior.TransformState.InTrans)
		{
			if (this.inBulletScript != null && !this.inBulletScript.IsNull && this.m_shouldFire)
			{
				this.ShootBulletScript(this.inBulletScript);
				this.m_shouldFire = false;
			}
			if (this.invulnerabilityMode == TransformBehavior.Invulnerability.WhileTransformed)
			{
				this.Invulnerable = true;
			}
			else if (this.invulnerabilityMode == TransformBehavior.Invulnerability.WhileNotTransformed)
			{
				this.Invulnerable = false;
			}
		}
		else if (state == TransformBehavior.TransformState.Transformed)
		{
			if (this.setTransformAnimAsBaseState)
			{
				this.m_aiAnimator.ClearBaseAnim();
			}
			if (!string.IsNullOrEmpty(this.transformedAnim))
			{
				this.m_aiAnimator.EndAnimationIf(this.transformedAnim);
			}
			if (this.transformedBulletScript != null && !this.transformedBulletScript.IsNull && this.m_shouldFire)
			{
				this.ShootBulletScript(this.transformedBulletScript);
				this.m_shouldFire = false;
			}
		}
		else if (state == TransformBehavior.TransformState.OutTrans)
		{
			if (this.requiresTransparency && this.m_cachedShader)
			{
				this.m_aiActor.sprite.usesOverrideMaterial = false;
				this.m_aiActor.renderer.material.shader = this.m_cachedShader;
				this.m_cachedShader = null;
			}
			if (this.shadowSupport == TransformBehavior.ShadowSupport.Fade)
			{
				this.m_shadowSprite.color = this.m_shadowSprite.color.WithAlpha(1f);
			}
			if (this.goneWhileTransformed)
			{
				this.m_aiActor.IsGone = false;
				this.m_aiActor.specRigidbody.CollideWithOthers = true;
			}
			if (this.m_aiShooter)
			{
				this.m_aiShooter.ToggleGunAndHandRenderers(true, "TransformBehavior");
			}
			if (this.outBulletScript != null && !this.outBulletScript.IsNull && this.m_shouldFire)
			{
				this.ShootBulletScript(this.outBulletScript);
				this.m_shouldFire = false;
			}
			if (this.invulnerabilityMode == TransformBehavior.Invulnerability.WhileTransformed)
			{
				this.Invulnerable = false;
			}
			else if (this.invulnerabilityMode == TransformBehavior.Invulnerability.WhileNotTransformed)
			{
				this.Invulnerable = true;
			}
		}
	}

	// Token: 0x17000A77 RID: 2679
	// (get) Token: 0x06004899 RID: 18585 RVA: 0x00181E54 File Offset: 0x00180054
	// (set) Token: 0x0600489A RID: 18586 RVA: 0x00181E5C File Offset: 0x0018005C
	private bool Invulnerable
	{
		get
		{
			return this.m_isInvulnerable;
		}
		set
		{
			if (value == this.m_isInvulnerable)
			{
				return;
			}
			this.m_enemyHitbox.Enabled = !value;
			this.m_aiActor.healthHaver.IsVulnerable = !value;
			if (this.m_bulletBlocker != null)
			{
				this.m_bulletBlocker.Enabled = value;
			}
			if (this.reflectBullets && this.m_aiActor.healthHaver.IsAlive)
			{
				this.m_aiActor.specRigidbody.ReflectProjectiles = value;
				this.m_aiActor.specRigidbody.ReflectBeams = value;
				if (value)
				{
					this.m_aiActor.specRigidbody.ReflectProjectilesNormalGenerator = new Func<Vector2, Vector2, Vector2>(this.GetNormal);
					this.m_aiActor.specRigidbody.ReflectBeamsNormalGenerator = new Func<Vector2, Vector2, Vector2>(this.GetNormal);
				}
			}
			this.m_isInvulnerable = value;
		}
	}

	// Token: 0x0600489B RID: 18587 RVA: 0x00181F38 File Offset: 0x00180138
	private Vector2 GetNormal(Vector2 contact, Vector2 normal)
	{
		return (contact - this.m_bulletBlocker.UnitCenter).normalized;
	}

	// Token: 0x04003C68 RID: 15464
	public TransformBehavior.Invulnerability invulnerabilityMode = TransformBehavior.Invulnerability.None;

	// Token: 0x04003C69 RID: 15465
	public bool reflectBullets;

	// Token: 0x04003C6A RID: 15466
	public float transformedTime = 1f;

	// Token: 0x04003C6B RID: 15467
	public bool goneWhileTransformed;

	// Token: 0x04003C6C RID: 15468
	public bool Uninterruptible;

	// Token: 0x04003C6D RID: 15469
	[InspectorCategory("Attack")]
	public GameObject shootPoint;

	// Token: 0x04003C6E RID: 15470
	[InspectorCategory("Attack")]
	public BulletScriptSelector inBulletScript;

	// Token: 0x04003C6F RID: 15471
	[InspectorCategory("Attack")]
	public BulletScriptSelector transformedBulletScript;

	// Token: 0x04003C70 RID: 15472
	[InspectorCategory("Attack")]
	public bool transformFireImmediately;

	// Token: 0x04003C71 RID: 15473
	[InspectorCategory("Attack")]
	public BulletScriptSelector outBulletScript;

	// Token: 0x04003C72 RID: 15474
	[InspectorCategory("Visuals")]
	public string inAnim;

	// Token: 0x04003C73 RID: 15475
	[InspectorCategory("Visuals")]
	public string transformedAnim;

	// Token: 0x04003C74 RID: 15476
	[InspectorCategory("Visuals")]
	public bool setTransformAnimAsBaseState;

	// Token: 0x04003C75 RID: 15477
	[InspectorCategory("Visuals")]
	public string outAnim;

	// Token: 0x04003C76 RID: 15478
	[InspectorCategory("Visuals")]
	public bool requiresTransparency;

	// Token: 0x04003C77 RID: 15479
	[InspectorCategory("Visuals")]
	public TransformBehavior.ShadowSupport shadowSupport = TransformBehavior.ShadowSupport.None;

	// Token: 0x04003C78 RID: 15480
	[InspectorCategory("Visuals")]
	[InspectorShowIf("ShowShadowAnimationNames")]
	public string shadowInAnim;

	// Token: 0x04003C79 RID: 15481
	[InspectorShowIf("ShowShadowAnimationNames")]
	[InspectorCategory("Visuals")]
	public string shadowOutAnim;

	// Token: 0x04003C7A RID: 15482
	private tk2dBaseSprite m_shadowSprite;

	// Token: 0x04003C7B RID: 15483
	private Shader m_cachedShader;

	// Token: 0x04003C7C RID: 15484
	private BulletScriptSource m_bulletSource;

	// Token: 0x04003C7D RID: 15485
	private PixelCollider m_enemyHitbox;

	// Token: 0x04003C7E RID: 15486
	private PixelCollider m_bulletBlocker;

	// Token: 0x04003C7F RID: 15487
	private float m_timer;

	// Token: 0x04003C80 RID: 15488
	private bool m_shouldFire;

	// Token: 0x04003C81 RID: 15489
	private bool m_isInvulnerable;

	// Token: 0x04003C82 RID: 15490
	private bool m_hasTransitioned;

	// Token: 0x04003C83 RID: 15491
	private TransformBehavior.TransformState m_state;

	// Token: 0x02000D6B RID: 3435
	public enum ShadowSupport
	{
		// Token: 0x04003C85 RID: 15493
		None = 10,
		// Token: 0x04003C86 RID: 15494
		Fade = 20,
		// Token: 0x04003C87 RID: 15495
		Animate = 30
	}

	// Token: 0x02000D6C RID: 3436
	public enum Invulnerability
	{
		// Token: 0x04003C89 RID: 15497
		None = 10,
		// Token: 0x04003C8A RID: 15498
		WhileTransformed = 20,
		// Token: 0x04003C8B RID: 15499
		WhileNotTransformed = 30
	}

	// Token: 0x02000D6D RID: 3437
	private enum TransformState
	{
		// Token: 0x04003C8D RID: 15501
		None,
		// Token: 0x04003C8E RID: 15502
		InTrans,
		// Token: 0x04003C8F RID: 15503
		Transformed,
		// Token: 0x04003C90 RID: 15504
		OutTrans
	}
}
