using System;
using Dungeonator;
using FullInspector;
using UnityEngine;

// Token: 0x02000DC9 RID: 3529
[InspectorDropdownName("Bosses/Meduzi/UnderwaterBehavior")]
public class MeduziUnderwaterBehavior : BasicAttackBehavior
{
	// Token: 0x06004AD3 RID: 19155 RVA: 0x00192DF4 File Offset: 0x00190FF4
	private bool ShowShadowAnimationNames()
	{
		return this.shadowSupport == MeduziUnderwaterBehavior.ShadowSupport.Animate;
	}

	// Token: 0x06004AD4 RID: 19156 RVA: 0x00192E00 File Offset: 0x00191000
	public override void Start()
	{
		base.Start();
		if ((this.disappearBulletScript != null && !this.disappearBulletScript.IsNull) || (this.reappearInBulletScript != null && !this.reappearInBulletScript.IsNull))
		{
			tk2dSpriteAnimator spriteAnimator = this.m_aiActor.spriteAnimator;
			spriteAnimator.AnimationEventTriggered = (Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip, int>)Delegate.Combine(spriteAnimator.AnimationEventTriggered, new Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip, int>(this.AnimationEventTriggered));
		}
		this.crawlAnimator.sprite.OverrideMaterialMode = tk2dBaseSprite.SpriteMaterialOverrideMode.OVERRIDE_MATERIAL_SIMPLE;
		this.crawlAnimator.renderer.material.SetFloat("_ReflectionYOffset", 1000f);
		this.m_goopDoer = this.m_aiActor.GetComponent<GoopDoer>();
		SpeculativeRigidbody specRigidbody = this.m_aiActor.specRigidbody;
		specRigidbody.MovementRestrictor = (SpeculativeRigidbody.MovementRestrictorDelegate)Delegate.Combine(specRigidbody.MovementRestrictor, new SpeculativeRigidbody.MovementRestrictorDelegate(this.RoomMovementRestrictor));
	}

	// Token: 0x06004AD5 RID: 19157 RVA: 0x00192EE4 File Offset: 0x001910E4
	public override void Upkeep()
	{
		base.Upkeep();
		base.DecrementTimer(ref this.m_timer, false);
	}

	// Token: 0x06004AD6 RID: 19158 RVA: 0x00192EFC File Offset: 0x001910FC
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
		this.State = MeduziUnderwaterBehavior.UnderwaterState.Disappear;
		this.m_aiActor.healthHaver.minimumHealth = 1f;
		this.m_updateEveryFrame = true;
		return BehaviorResult.RunContinuous;
	}

	// Token: 0x06004AD7 RID: 19159 RVA: 0x00192F80 File Offset: 0x00191180
	public override ContinuousBehaviorResult ContinuousUpdate()
	{
		base.ContinuousUpdate();
		if (this.State == MeduziUnderwaterBehavior.UnderwaterState.Disappear)
		{
			if (this.shadowSupport == MeduziUnderwaterBehavior.ShadowSupport.Fade)
			{
				this.m_shadowSprite.color = this.m_shadowSprite.color.WithAlpha(1f - this.m_aiAnimator.CurrentClipProgress);
			}
			if (!this.m_aiAnimator.IsPlaying(this.disappearAnim))
			{
				this.State = ((this.GoneTime <= 0f) ? MeduziUnderwaterBehavior.UnderwaterState.Reappear : MeduziUnderwaterBehavior.UnderwaterState.Gone);
			}
		}
		else if (this.State == MeduziUnderwaterBehavior.UnderwaterState.Gone)
		{
			float num = ((this.m_aiActor.BehaviorVelocity.magnitude != 0f) ? this.m_aiActor.BehaviorVelocity.ToAngle() : 0f);
			if (this.m_aiActor.TargetRigidbody)
			{
				num = (this.m_aiActor.TargetRigidbody.GetUnitCenter(ColliderType.Ground) - this.m_aiActor.specRigidbody.UnitCenter).ToAngle();
			}
			this.m_direction = Mathf.SmoothDampAngle(this.m_direction, num, ref this.m_angularVelocity, this.crawlTurnTime);
			this.crawlSprite.transform.rotation = Quaternion.Euler(0f, 0f, BraveMathCollege.QuantizeFloat(this.m_direction, 11.25f));
			this.m_aiActor.BehaviorVelocity = BraveMathCollege.DegreesToVector(this.m_direction, this.crawlSpeed);
			if (this.m_timer <= 0f)
			{
				this.State = MeduziUnderwaterBehavior.UnderwaterState.Reappear;
			}
		}
		else if (this.State == MeduziUnderwaterBehavior.UnderwaterState.Reappear)
		{
			if (this.shadowSupport == MeduziUnderwaterBehavior.ShadowSupport.Fade)
			{
				this.m_shadowSprite.color = this.m_shadowSprite.color.WithAlpha(this.m_aiAnimator.CurrentClipProgress);
			}
			if (this.m_aiShooter)
			{
				this.m_aiShooter.ToggleGunAndHandRenderers(false, "MeduziUnderwaterBehavior");
			}
			if (!this.m_aiAnimator.IsPlaying(this.reappearAnim))
			{
				this.State = MeduziUnderwaterBehavior.UnderwaterState.None;
				return ContinuousBehaviorResult.Finished;
			}
		}
		return ContinuousBehaviorResult.Continue;
	}

	// Token: 0x06004AD8 RID: 19160 RVA: 0x00193198 File Offset: 0x00191398
	public override void EndContinuousUpdate()
	{
		base.EndContinuousUpdate();
		this.m_aiActor.healthHaver.minimumHealth = 0f;
		if (this.requiresTransparency && this.m_cachedShader)
		{
			this.m_aiActor.sprite.usesOverrideMaterial = false;
			this.m_aiActor.renderer.material.shader = this.m_cachedShader;
			this.m_cachedShader = null;
		}
		this.m_aiActor.sprite.renderer.enabled = true;
		if (this.m_aiActor.knockbackDoer)
		{
			this.m_aiActor.knockbackDoer.SetImmobile(false, "teleport");
		}
		this.m_aiActor.specRigidbody.CollideWithOthers = true;
		this.m_aiActor.IsGone = false;
		if (this.m_aiShooter)
		{
			this.m_aiShooter.ToggleGunAndHandRenderers(true, "MeduziUnderwaterBehavior");
		}
		this.m_aiAnimator.EndAnimationIf(this.disappearAnim);
		this.m_aiAnimator.EndAnimationIf(this.reappearAnim);
		if (this.shadowSupport == MeduziUnderwaterBehavior.ShadowSupport.Fade)
		{
			this.m_shadowSprite.color = this.m_shadowSprite.color.WithAlpha(1f);
		}
		else if (this.shadowSupport == MeduziUnderwaterBehavior.ShadowSupport.Animate)
		{
			tk2dSpriteAnimationClip clipByName = this.m_shadowSprite.spriteAnimator.GetClipByName(this.shadowReappearAnim);
			this.m_shadowSprite.spriteAnimator.Play(clipByName, (float)(clipByName.frames.Length - 1), clipByName.fps, false);
		}
		this.crawlSprite.SetActive(false);
		this.m_aiActor.BehaviorOverridesVelocity = false;
		SpriteOutlineManager.ToggleOutlineRenderers(this.m_aiActor.sprite, true);
		this.m_goopDoer.enabled = false;
		this.m_state = MeduziUnderwaterBehavior.UnderwaterState.None;
		this.m_updateEveryFrame = false;
		this.UpdateCooldowns();
	}

	// Token: 0x06004AD9 RID: 19161 RVA: 0x00193374 File Offset: 0x00191574
	public override void OnActorPreDeath()
	{
		SpeculativeRigidbody specRigidbody = this.m_aiActor.specRigidbody;
		specRigidbody.MovementRestrictor = (SpeculativeRigidbody.MovementRestrictorDelegate)Delegate.Remove(specRigidbody.MovementRestrictor, new SpeculativeRigidbody.MovementRestrictorDelegate(this.RoomMovementRestrictor));
		base.OnActorPreDeath();
	}

	// Token: 0x06004ADA RID: 19162 RVA: 0x001933A8 File Offset: 0x001915A8
	public override bool IsOverridable()
	{
		return false;
	}

	// Token: 0x06004ADB RID: 19163 RVA: 0x001933AC File Offset: 0x001915AC
	public void AnimationEventTriggered(tk2dSpriteAnimator animator, tk2dSpriteAnimationClip clip, int frame)
	{
		if (this.m_shouldFire && clip.GetFrame(frame).eventInfo == "fire")
		{
			if (this.State == MeduziUnderwaterBehavior.UnderwaterState.Reappear)
			{
				SpawnManager.SpawnBulletScript(this.m_aiActor, this.reappearInBulletScript, null, null, false, null);
			}
			else if (this.State == MeduziUnderwaterBehavior.UnderwaterState.Disappear)
			{
				SpawnManager.SpawnBulletScript(this.m_aiActor, this.disappearBulletScript, null, null, false, null);
			}
			this.m_shouldFire = false;
		}
	}

	// Token: 0x06004ADC RID: 19164 RVA: 0x00193450 File Offset: 0x00191650
	private void RoomMovementRestrictor(SpeculativeRigidbody specRigidbody, IntVector2 prevPixelOffset, IntVector2 pixelOffset, ref bool validLocation)
	{
		if (!validLocation)
		{
			return;
		}
		IntVector2 intVector = pixelOffset - prevPixelOffset;
		CellArea area = this.m_aiActor.ParentRoom.area;
		if (intVector.x < 0)
		{
			int num = specRigidbody.PixelColliders[0].MinX + pixelOffset.x;
			int num2 = area.basePosition.x * 16;
			if (num < num2)
			{
				validLocation = false;
				return;
			}
		}
		else if (intVector.x > 0)
		{
			int num3 = specRigidbody.PixelColliders[0].MaxX + pixelOffset.x;
			int num4 = (area.basePosition.x + area.dimensions.x) * 16 - 1;
			if (num3 > num4)
			{
				validLocation = false;
				return;
			}
		}
		else if (intVector.y < 0)
		{
			int num5 = specRigidbody.PixelColliders[0].MinY + pixelOffset.y;
			int num6 = area.basePosition.y * 16;
			if (num5 < num6)
			{
				validLocation = false;
				return;
			}
		}
		else if (intVector.y > 0)
		{
			int num7 = specRigidbody.PixelColliders[0].MaxY + pixelOffset.y;
			int num8 = (area.basePosition.y + area.dimensions.y) * 16 - 1;
			if (num7 > num8)
			{
				validLocation = false;
				return;
			}
		}
	}

	// Token: 0x17000A93 RID: 2707
	// (get) Token: 0x06004ADD RID: 19165 RVA: 0x001935C0 File Offset: 0x001917C0
	// (set) Token: 0x06004ADE RID: 19166 RVA: 0x001935C8 File Offset: 0x001917C8
	private MeduziUnderwaterBehavior.UnderwaterState State
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

	// Token: 0x06004ADF RID: 19167 RVA: 0x001935EC File Offset: 0x001917EC
	private void BeginState(MeduziUnderwaterBehavior.UnderwaterState state)
	{
		if (state == MeduziUnderwaterBehavior.UnderwaterState.Disappear)
		{
			if (this.disappearBulletScript != null && !this.disappearBulletScript.IsNull)
			{
				this.m_shouldFire = true;
			}
			if (this.requiresTransparency)
			{
				this.m_cachedShader = this.m_aiActor.renderer.material.shader;
				this.m_aiActor.sprite.usesOverrideMaterial = true;
				this.m_aiActor.renderer.material.shader = ShaderCache.Acquire("Brave/LitBlendUber");
			}
			this.m_aiAnimator.PlayUntilCancelled(this.disappearAnim, true, null, -1f, false);
			if (this.shadowSupport == MeduziUnderwaterBehavior.ShadowSupport.Animate)
			{
				this.m_shadowSprite.spriteAnimator.PlayAndForceTime(this.shadowDisappearAnim, this.m_aiAnimator.CurrentClipLength);
			}
			this.m_aiActor.ClearPath();
			if (!this.AttackableDuringAnimation)
			{
				this.m_aiActor.specRigidbody.CollideWithOthers = false;
				this.m_aiActor.IsGone = true;
			}
			if (this.m_aiShooter)
			{
				this.m_aiShooter.ToggleGunAndHandRenderers(false, "MeduziUnderwaterBehavior");
			}
		}
		else if (state == MeduziUnderwaterBehavior.UnderwaterState.Gone)
		{
			this.m_timer = this.GoneTime;
			this.m_aiActor.specRigidbody.CollideWithOthers = false;
			this.m_aiActor.IsGone = true;
			this.m_aiActor.sprite.renderer.enabled = false;
			this.crawlSprite.transform.rotation = Quaternion.identity;
			this.crawlSprite.SetActive(true);
			this.crawlAnimator.Play(this.crawlAnimator.DefaultClip, 0f, this.crawlAnimator.DefaultClip.fps, false);
			if (this.startingDirection == MeduziUnderwaterBehavior.StartingDirection.Player && this.m_aiActor.TargetRigidbody)
			{
				this.m_direction = (this.m_aiActor.TargetRigidbody.GetUnitCenter(ColliderType.Ground) - this.m_aiActor.specRigidbody.UnitCenter).ToAngle();
			}
			else
			{
				this.m_direction = UnityEngine.Random.Range(0f, 360f);
			}
			this.m_angularVelocity = 0f;
			this.crawlSprite.transform.rotation = Quaternion.Euler(0f, 0f, BraveMathCollege.QuantizeFloat(this.m_direction, 11.25f));
			this.m_aiActor.BehaviorOverridesVelocity = true;
			this.m_aiActor.BehaviorVelocity = BraveMathCollege.DegreesToVector(this.m_direction, this.crawlSpeed);
			this.m_goopDoer.enabled = true;
		}
		else if (state == MeduziUnderwaterBehavior.UnderwaterState.Reappear)
		{
			if (this.reappearInBulletScript != null && !this.reappearInBulletScript.IsNull)
			{
				this.m_shouldFire = true;
			}
			this.m_aiAnimator.PlayUntilFinished(this.reappearAnim, true, null, -1f, false);
			if (this.shadowSupport == MeduziUnderwaterBehavior.ShadowSupport.Animate)
			{
				this.m_shadowSprite.spriteAnimator.PlayAndForceTime(this.shadowReappearAnim, this.m_aiAnimator.CurrentClipLength);
			}
			this.m_shadowSprite.renderer.enabled = true;
			if (this.AttackableDuringAnimation)
			{
				this.m_aiActor.specRigidbody.CollideWithOthers = true;
				this.m_aiActor.IsGone = false;
			}
			this.m_aiActor.sprite.renderer.enabled = true;
			if (this.m_aiShooter)
			{
				this.m_aiShooter.ToggleGunAndHandRenderers(false, "MeduziUnderwaterBehavior");
			}
			SpriteOutlineManager.ToggleOutlineRenderers(this.m_aiActor.sprite, true);
		}
	}

	// Token: 0x06004AE0 RID: 19168 RVA: 0x0019397C File Offset: 0x00191B7C
	private void EndState(MeduziUnderwaterBehavior.UnderwaterState state)
	{
		if (state == MeduziUnderwaterBehavior.UnderwaterState.Disappear)
		{
			this.m_shadowSprite.renderer.enabled = false;
			SpriteOutlineManager.ToggleOutlineRenderers(this.m_aiActor.sprite, false);
			if (this.disappearBulletScript != null && !this.disappearBulletScript.IsNull && this.m_shouldFire)
			{
				SpawnManager.SpawnBulletScript(this.m_aiActor, this.disappearBulletScript, null, null, false, null);
				this.m_shouldFire = false;
			}
		}
		else if (state == MeduziUnderwaterBehavior.UnderwaterState.Gone)
		{
			this.crawlSprite.SetActive(false);
			this.m_aiActor.BehaviorOverridesVelocity = false;
			this.m_goopDoer.enabled = false;
		}
		else if (state == MeduziUnderwaterBehavior.UnderwaterState.Reappear)
		{
			if (this.requiresTransparency)
			{
				this.m_aiActor.sprite.usesOverrideMaterial = false;
				this.m_aiActor.renderer.material.shader = this.m_cachedShader;
			}
			if (this.shadowSupport == MeduziUnderwaterBehavior.ShadowSupport.Fade)
			{
				this.m_shadowSprite.color = this.m_shadowSprite.color.WithAlpha(1f);
			}
			this.m_aiActor.specRigidbody.CollideWithOthers = true;
			this.m_aiActor.IsGone = false;
			if (this.m_aiShooter)
			{
				this.m_aiShooter.ToggleGunAndHandRenderers(true, "MeduziUnderwaterBehavior");
			}
			if (this.reappearInBulletScript != null && !this.reappearInBulletScript.IsNull && this.m_shouldFire)
			{
				SpawnManager.SpawnBulletScript(this.m_aiActor, this.reappearInBulletScript, null, null, false, null);
				this.m_shouldFire = false;
			}
		}
	}

	// Token: 0x04003FD0 RID: 16336
	public bool AttackableDuringAnimation;

	// Token: 0x04003FD1 RID: 16337
	public bool AvoidWalls;

	// Token: 0x04003FD2 RID: 16338
	public MeduziUnderwaterBehavior.StartingDirection startingDirection;

	// Token: 0x04003FD3 RID: 16339
	public float GoneTime = 1f;

	// Token: 0x04003FD4 RID: 16340
	[InspectorCategory("Attack")]
	public BulletScriptSelector disappearBulletScript;

	// Token: 0x04003FD5 RID: 16341
	[InspectorCategory("Attack")]
	public BulletScriptSelector reappearInBulletScript;

	// Token: 0x04003FD6 RID: 16342
	[InspectorCategory("Visuals")]
	public string disappearAnim = "teleport_out";

	// Token: 0x04003FD7 RID: 16343
	[InspectorCategory("Visuals")]
	public string reappearAnim = "teleport_in";

	// Token: 0x04003FD8 RID: 16344
	[InspectorCategory("Visuals")]
	public bool requiresTransparency;

	// Token: 0x04003FD9 RID: 16345
	[InspectorCategory("Visuals")]
	public MeduziUnderwaterBehavior.ShadowSupport shadowSupport;

	// Token: 0x04003FDA RID: 16346
	[InspectorCategory("Visuals")]
	[InspectorShowIf("ShowShadowAnimationNames")]
	public string shadowDisappearAnim;

	// Token: 0x04003FDB RID: 16347
	[InspectorCategory("Visuals")]
	[InspectorShowIf("ShowShadowAnimationNames")]
	public string shadowReappearAnim;

	// Token: 0x04003FDC RID: 16348
	public GameObject crawlSprite;

	// Token: 0x04003FDD RID: 16349
	public tk2dSpriteAnimator crawlAnimator;

	// Token: 0x04003FDE RID: 16350
	public float crawlSpeed = 8f;

	// Token: 0x04003FDF RID: 16351
	public float crawlTurnTime = 1f;

	// Token: 0x04003FE0 RID: 16352
	private tk2dBaseSprite m_shadowSprite;

	// Token: 0x04003FE1 RID: 16353
	private Shader m_cachedShader;

	// Token: 0x04003FE2 RID: 16354
	private GoopDoer m_goopDoer;

	// Token: 0x04003FE3 RID: 16355
	private float m_timer;

	// Token: 0x04003FE4 RID: 16356
	private bool m_shouldFire;

	// Token: 0x04003FE5 RID: 16357
	private float m_direction;

	// Token: 0x04003FE6 RID: 16358
	private float m_angularVelocity;

	// Token: 0x04003FE7 RID: 16359
	private MeduziUnderwaterBehavior.UnderwaterState m_state;

	// Token: 0x02000DCA RID: 3530
	public enum ShadowSupport
	{
		// Token: 0x04003FE9 RID: 16361
		None,
		// Token: 0x04003FEA RID: 16362
		Fade,
		// Token: 0x04003FEB RID: 16363
		Animate
	}

	// Token: 0x02000DCB RID: 3531
	public enum StartingDirection
	{
		// Token: 0x04003FED RID: 16365
		Player,
		// Token: 0x04003FEE RID: 16366
		RandomAwayFromWalls
	}

	// Token: 0x02000DCC RID: 3532
	private enum UnderwaterState
	{
		// Token: 0x04003FF0 RID: 16368
		None,
		// Token: 0x04003FF1 RID: 16369
		Disappear,
		// Token: 0x04003FF2 RID: 16370
		Gone,
		// Token: 0x04003FF3 RID: 16371
		Reappear
	}
}
