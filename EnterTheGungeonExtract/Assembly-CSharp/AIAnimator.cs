using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x02000F91 RID: 3985
public class AIAnimator : BraveBehaviour
{
	// Token: 0x17000C3B RID: 3131
	// (get) Token: 0x06005651 RID: 22097 RVA: 0x0020E524 File Offset: 0x0020C724
	// (set) Token: 0x06005652 RID: 22098 RVA: 0x0020E52C File Offset: 0x0020C72C
	public bool UseAnimatedFacingDirection { get; set; }

	// Token: 0x17000C3C RID: 3132
	// (get) Token: 0x06005653 RID: 22099 RVA: 0x0020E538 File Offset: 0x0020C738
	protected float LocalDeltaTime
	{
		get
		{
			if (base.aiActor)
			{
				return base.aiActor.LocalDeltaTime;
			}
			if (base.behaviorSpeculator)
			{
				return base.behaviorSpeculator.LocalDeltaTime;
			}
			return BraveTime.DeltaTime;
		}
	}

	// Token: 0x06005654 RID: 22100 RVA: 0x0020E578 File Offset: 0x0020C778
	private bool ShowDirectionParent()
	{
		return this.facingType == AIAnimator.FacingType.SlaveDirection;
	}

	// Token: 0x06005655 RID: 22101 RVA: 0x0020E584 File Offset: 0x0020C784
	private bool ShowFaceSouthWhenStopped()
	{
		return this.facingType == AIAnimator.FacingType.Movement;
	}

	// Token: 0x06005656 RID: 22102 RVA: 0x0020E590 File Offset: 0x0020C790
	private bool ShowRotationOptions()
	{
		return this.directionalType != AIAnimator.DirectionalType.Sprite;
	}

	// Token: 0x06005657 RID: 22103 RVA: 0x0020E5A0 File Offset: 0x0020C7A0
	private bool ShowHitAnimationOptions()
	{
		return this.HitAnimation.Type != DirectionalAnimation.DirectionType.None;
	}

	// Token: 0x17000C3D RID: 3133
	// (get) Token: 0x06005658 RID: 22104 RVA: 0x0020E5B4 File Offset: 0x0020C7B4
	public bool SpriteFlipped
	{
		get
		{
			return this.m_currentBaseClip != null && (this.m_currentBaseClip.name == "move_back" || this.m_currentBaseClip.name == "move_left" || this.m_currentBaseClip.name == "move_forward_left" || this.m_currentBaseClip.name == "move_back_left");
		}
	}

	// Token: 0x17000C3E RID: 3134
	// (get) Token: 0x06005659 RID: 22105 RVA: 0x0020E634 File Offset: 0x0020C834
	// (set) Token: 0x0600565A RID: 22106 RVA: 0x0020E63C File Offset: 0x0020C83C
	public bool SuppressHitStates { get; set; }

	// Token: 0x17000C3F RID: 3135
	// (get) Token: 0x0600565B RID: 22107 RVA: 0x0020E648 File Offset: 0x0020C848
	// (set) Token: 0x0600565C RID: 22108 RVA: 0x0020E650 File Offset: 0x0020C850
	public bool LockFacingDirection { get; set; }

	// Token: 0x17000C40 RID: 3136
	// (get) Token: 0x0600565D RID: 22109 RVA: 0x0020E65C File Offset: 0x0020C85C
	// (set) Token: 0x0600565E RID: 22110 RVA: 0x0020E664 File Offset: 0x0020C864
	public float FacingDirection
	{
		get
		{
			return this.m_facingDirection;
		}
		set
		{
			if (!float.IsNaN(value))
			{
				this.m_facingDirection = value;
			}
		}
	}

	// Token: 0x17000C41 RID: 3137
	// (get) Token: 0x0600565F RID: 22111 RVA: 0x0020E678 File Offset: 0x0020C878
	// (set) Token: 0x06005660 RID: 22112 RVA: 0x0020E680 File Offset: 0x0020C880
	public float FpsScale
	{
		get
		{
			return this.m_fpsScale;
		}
		set
		{
			if (this.m_fpsScale != value)
			{
				this.m_fpsScale = value;
				bool flag = this.m_currentActionState != null && this.m_currentActionState.WarpClipDuration > 0f;
				if (base.spriteAnimator.Playing && !flag)
				{
					float num = base.spriteAnimator.CurrentClip.fps * this.m_fpsScale;
					if (num == 0f)
					{
						num = 0.001f;
					}
					base.spriteAnimator.ClipFps = num;
				}
			}
			if (this.ChildAnimator)
			{
				this.ChildAnimator.FpsScale = value;
			}
		}
	}

	// Token: 0x17000C42 RID: 3138
	// (get) Token: 0x06005661 RID: 22113 RVA: 0x0020E728 File Offset: 0x0020C928
	public float CurrentClipLength
	{
		get
		{
			return (float)base.spriteAnimator.CurrentClip.frames.Length / base.spriteAnimator.CurrentClip.fps;
		}
	}

	// Token: 0x17000C43 RID: 3139
	// (get) Token: 0x06005662 RID: 22114 RVA: 0x0020E750 File Offset: 0x0020C950
	public float CurrentClipProgress
	{
		get
		{
			return Mathf.Clamp01(base.spriteAnimator.ClipTimeSeconds / this.CurrentClipLength);
		}
	}

	// Token: 0x140000A9 RID: 169
	// (add) Token: 0x06005663 RID: 22115 RVA: 0x0020E76C File Offset: 0x0020C96C
	// (remove) Token: 0x06005664 RID: 22116 RVA: 0x0020E7A4 File Offset: 0x0020C9A4
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event Action OnSpawnCompleted;

	// Token: 0x17000C44 RID: 3140
	// (get) Token: 0x06005665 RID: 22117 RVA: 0x0020E7DC File Offset: 0x0020C9DC
	// (set) Token: 0x06005666 RID: 22118 RVA: 0x0020E7E4 File Offset: 0x0020C9E4
	public string OverrideIdleAnimation { get; set; }

	// Token: 0x17000C45 RID: 3141
	// (get) Token: 0x06005667 RID: 22119 RVA: 0x0020E7F0 File Offset: 0x0020C9F0
	// (set) Token: 0x06005668 RID: 22120 RVA: 0x0020E7F8 File Offset: 0x0020C9F8
	public string OverrideMoveAnimation { get; set; }

	// Token: 0x17000C46 RID: 3142
	// (get) Token: 0x06005669 RID: 22121 RVA: 0x0020E804 File Offset: 0x0020CA04
	public float CurrentArtAngle
	{
		get
		{
			if (this.m_currentActionState != null)
			{
				return this.m_currentActionState.ArtAngle;
			}
			return this.FacingDirection;
		}
	}

	// Token: 0x0600566A RID: 22122 RVA: 0x0020E824 File Offset: 0x0020CA24
	public void Awake()
	{
		base.spriteAnimator.playAutomatically = false;
		if (GameManager.Instance.InTutorial && base.name.Contains("turret", true))
		{
			this.FacingDirection = 180f;
			this.LockFacingDirection = true;
			base.specRigidbody.enabled = false;
		}
		if (this.SpecifyAiActor)
		{
			base.aiActor = this.SpecifyAiActor;
			base.specRigidbody = base.aiActor.specRigidbody;
		}
		if (this.ForceKillVfxOnPreDeath && base.healthHaver)
		{
			base.healthHaver.OnPreDeath += this.OnPreDeath;
		}
	}

	// Token: 0x0600566B RID: 22123 RVA: 0x0020E8E0 File Offset: 0x0020CAE0
	public void Start()
	{
		tk2dSpriteAnimator spriteAnimator = base.spriteAnimator;
		spriteAnimator.AnimationCompleted = (Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip>)Delegate.Combine(spriteAnimator.AnimationCompleted, new Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip>(this.AnimationCompleted));
		if (base.healthHaver && this.ChildAnimator && this.ChildAnimator.sprite && this.ChildAnimator.IsBodySprite)
		{
			base.healthHaver.RegisterBodySprite(this.ChildAnimator.sprite, false, 0);
		}
	}

	// Token: 0x0600566C RID: 22124 RVA: 0x0020E974 File Offset: 0x0020CB74
	private void UpdateTurboSettings()
	{
		if (this.m_cachedTurbo != TurboModeController.sEnemyAnimSpeed && GameManager.IsTurboMode)
		{
			if (this.m_cachedTurbo > 0f)
			{
				this.FpsScale /= this.m_cachedTurbo;
				this.m_cachedTurbo = -1f;
			}
			this.FpsScale *= TurboModeController.sEnemyAnimSpeed;
			this.m_cachedTurbo = TurboModeController.sEnemyAnimSpeed;
		}
		else if (this.m_cachedTurbo > 0f && !GameManager.IsTurboMode)
		{
			this.FpsScale /= this.m_cachedTurbo;
			this.m_cachedTurbo = -1f;
		}
	}

	// Token: 0x0600566D RID: 22125 RVA: 0x0020EA24 File Offset: 0x0020CC24
	public void Update()
	{
		this.m_suppressHitReactTimer = Mathf.Max(0f, this.m_suppressHitReactTimer - BraveTime.DeltaTime);
		this.UpdateTurboSettings();
		this.UpdateFacingDirection();
		this.UpdateCurrentBaseAnimation();
		if (this.m_currentActionState != null && this.m_currentActionState.DirectionalAnimation != null && this.m_currentActionState.AnimationClip == this.m_currentBaseClip)
		{
			this.m_currentActionState = null;
		}
		if (this.m_currentActionState != null)
		{
			this.m_currentActionState.Update(base.spriteAnimator, this.FacingDirection);
			if (!this.m_currentActionState.HasStarted)
			{
				base.spriteAnimator.Stop();
				this.PlayClip(this.m_currentActionState.AnimationClip, this.m_currentActionState.WarpClipDuration);
				this.m_currentActionState.HasStarted = true;
				this.m_playingHitEffect = false;
			}
			else if (!this.m_playingHitEffect && base.spriteAnimator.CurrentClip != this.m_currentActionState.AnimationClip)
			{
				base.spriteAnimator.Play(this.m_currentActionState.AnimationClip, base.spriteAnimator.ClipTimeSeconds, this.GetFps(this.m_currentActionState.AnimationClip, this.m_currentActionState.WarpClipDuration), true);
			}
			if (this.m_currentActionState.EndType == AIAnimator.AnimatorState.StateEndType.Duration || this.m_currentActionState.EndType == AIAnimator.AnimatorState.StateEndType.DurationOrFinished)
			{
				this.m_currentActionState.Timer = this.m_currentActionState.Timer - this.LocalDeltaTime;
				if (this.m_currentActionState.Timer <= 0f)
				{
					this.m_currentActionState = null;
					this.m_playingHitEffect = false;
				}
			}
		}
		else if (!this.m_playingHitEffect && this.m_baseDirectionalAnimationOverride != null && !base.spriteAnimator.IsPlaying(this.m_currentOverrideBaseClip))
		{
			this.PlayClip(this.m_currentOverrideBaseClip, -1f);
		}
		else if (!this.m_playingHitEffect && this.m_baseDirectionalAnimationOverride == null && this.m_currentBaseClip != null && !base.spriteAnimator.IsPlaying(this.m_currentBaseClip))
		{
			this.PlayClip(this.m_currentBaseClip, -1f);
		}
		this.UpdateFacingRotation();
		for (int i = 0; i < this.OtherVFX.Count; i++)
		{
			this.OtherVFX[i].vfxPool.RemoveDespawnedVfx();
		}
	}

	// Token: 0x0600566E RID: 22126 RVA: 0x0020EC98 File Offset: 0x0020CE98
	private string GetDebugString()
	{
		string text = string.Format("{0}: {1} ({2}) - {3} ({4})", new object[]
		{
			base.name,
			(this.m_currentActionState != null) ? this.m_currentActionState.Name : "null",
			(this.m_currentActionState != null) ? this.m_currentActionState.Timer.ToString() : "null",
			base.spriteAnimator.CurrentClip.name,
			base.spriteAnimator.ClipTimeSeconds
		});
		if (this.ChildAnimator && this.ChildAnimator.IsBodySprite)
		{
			text = text + " | " + this.ChildAnimator.GetDebugString();
		}
		return text;
	}

	// Token: 0x0600566F RID: 22127 RVA: 0x0020ED70 File Offset: 0x0020CF70
	protected override void OnDestroy()
	{
		if (base.spriteAnimator)
		{
			tk2dSpriteAnimator spriteAnimator = base.spriteAnimator;
			spriteAnimator.AnimationCompleted = (Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip>)Delegate.Remove(spriteAnimator.AnimationCompleted, new Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip>(this.AnimationCompleted));
		}
		base.OnDestroy();
		for (int i = 0; i < this.OtherVFX.Count; i++)
		{
			this.OtherVFX[i].vfxPool.DestroyAll();
		}
	}

	// Token: 0x06005670 RID: 22128 RVA: 0x0020EDEC File Offset: 0x0020CFEC
	private void AnimationCompleted(tk2dSpriteAnimator animator, tk2dSpriteAnimationClip clip)
	{
		if (!base.enabled)
		{
			return;
		}
		if (this.m_playingHitEffect)
		{
			this.m_playingHitEffect = false;
			if (this.m_currentActionState != null && this.m_currentActionState.HasStarted)
			{
				this.PlayClip(this.m_currentActionState.AnimationClip, this.m_currentActionState.WarpClipDuration);
			}
			else
			{
				this.PlayClip(this.m_currentBaseClip, -1f);
			}
		}
		else if (this.m_currentActionState != null && this.m_currentActionState.AnimationClip == clip && this.m_currentActionState.EndType != AIAnimator.AnimatorState.StateEndType.UntilCancelled)
		{
			this.m_currentActionState = null;
		}
	}

	// Token: 0x06005671 RID: 22129 RVA: 0x0020EE9C File Offset: 0x0020D09C
	public bool PlayUntilCancelled(string name, bool suppressHitStates = false, string overrideHitState = null, float warpClipDuration = -1f, bool skipChildAnimators = false)
	{
		bool flag = false;
		if (!skipChildAnimators && this.ChildAnimator)
		{
			flag = this.ChildAnimator.PlayUntilCancelled(name, suppressHitStates, overrideHitState, -1f, false);
		}
		if (this.HasDirectionalAnimation(name))
		{
			return this.Play(name, this.GetDirectionalAnimation(name), AIAnimator.AnimatorState.StateEndType.UntilCancelled, 0f, warpClipDuration, suppressHitStates, overrideHitState) || flag;
		}
		return this.Play(name, AIAnimator.AnimatorState.StateEndType.UntilCancelled, 0f, warpClipDuration, suppressHitStates, overrideHitState) || flag;
	}

	// Token: 0x06005672 RID: 22130 RVA: 0x0020EF20 File Offset: 0x0020D120
	public bool PlayUntilFinished(string name, bool suppressHitStates = false, string overrideHitState = null, float warpClipDuration = -1f, bool skipChildAnimators = false)
	{
		if (this.OnPlayUntilFinished != null)
		{
			this.OnPlayUntilFinished(name, suppressHitStates, overrideHitState, warpClipDuration, skipChildAnimators);
		}
		bool flag = false;
		if (!skipChildAnimators && this.ChildAnimator)
		{
			flag = this.ChildAnimator.PlayUntilFinished(name, suppressHitStates, overrideHitState, warpClipDuration, false);
		}
		if (this.HasDirectionalAnimation(name))
		{
			return this.Play(name, this.GetDirectionalAnimation(name), AIAnimator.AnimatorState.StateEndType.UntilFinished, 0f, warpClipDuration, suppressHitStates, overrideHitState) || flag;
		}
		return this.Play(name, AIAnimator.AnimatorState.StateEndType.UntilFinished, 0f, warpClipDuration, suppressHitStates, overrideHitState) || flag;
	}

	// Token: 0x06005673 RID: 22131 RVA: 0x0020EFC0 File Offset: 0x0020D1C0
	public bool PlayForDuration(string name, float duration, bool suppressHitStates = false, string overrideHitState = null, float warpClipDuration = -1f, bool skipChildAnimators = false)
	{
		bool flag = false;
		if (!skipChildAnimators && this.ChildAnimator)
		{
			flag = this.ChildAnimator.PlayForDuration(name, duration, suppressHitStates, overrideHitState, -1f, false);
		}
		if (this.HasDirectionalAnimation(name))
		{
			return this.Play(name, this.GetDirectionalAnimation(name), AIAnimator.AnimatorState.StateEndType.Duration, duration, warpClipDuration, suppressHitStates, overrideHitState) || flag;
		}
		return this.Play(name, AIAnimator.AnimatorState.StateEndType.Duration, duration, warpClipDuration, suppressHitStates, overrideHitState) || flag;
	}

	// Token: 0x06005674 RID: 22132 RVA: 0x0020F040 File Offset: 0x0020D240
	public bool PlayForDurationOrUntilFinished(string name, float duration, bool suppressHitStates = false, string overrideHitState = null, float warpClipDuration = -1f, bool skipChildAnimators = false)
	{
		bool flag = false;
		if (!skipChildAnimators && this.ChildAnimator)
		{
			flag = this.ChildAnimator.PlayForDurationOrUntilFinished(name, duration, suppressHitStates, overrideHitState, -1f, false);
		}
		if (this.HasDirectionalAnimation(name))
		{
			return this.Play(name, this.GetDirectionalAnimation(name), AIAnimator.AnimatorState.StateEndType.DurationOrFinished, duration, warpClipDuration, suppressHitStates, overrideHitState) || flag;
		}
		return this.Play(name, AIAnimator.AnimatorState.StateEndType.DurationOrFinished, duration, warpClipDuration, suppressHitStates, overrideHitState) || flag;
	}

	// Token: 0x06005675 RID: 22133 RVA: 0x0020F0C0 File Offset: 0x0020D2C0
	private bool Play(string name, AIAnimator.AnimatorState.StateEndType endType, float duration, float warpClipDuration, bool suppressHitStates, string overrideHitState)
	{
		return !this.SuppressAnimatorFallback && this.Play(new AIAnimator.AnimatorState
		{
			Name = name,
			AnimationClip = base.spriteAnimator.GetClipByName(name),
			EndType = endType,
			Timer = duration,
			WarpClipDuration = warpClipDuration,
			SuppressHitStates = suppressHitStates,
			OverrideHitStateName = overrideHitState
		});
	}

	// Token: 0x06005676 RID: 22134 RVA: 0x0020F128 File Offset: 0x0020D328
	private bool Play(string name, DirectionalAnimation directionalAnimation, AIAnimator.AnimatorState.StateEndType endType, float duration, float warpClipDuration, bool suppressHitStates, string overrideHitState)
	{
		return directionalAnimation.Type != DirectionalAnimation.DirectionType.None && this.Play(new AIAnimator.AnimatorState
		{
			Name = name,
			DirectionalAnimation = directionalAnimation,
			AnimationClip = base.spriteAnimator.GetClipByName(directionalAnimation.GetInfo(this.FacingDirection, true).name),
			EndType = endType,
			Timer = duration,
			WarpClipDuration = warpClipDuration,
			SuppressHitStates = suppressHitStates,
			OverrideHitStateName = overrideHitState
		});
	}

	// Token: 0x06005677 RID: 22135 RVA: 0x0020F1A8 File Offset: 0x0020D3A8
	private bool Play(AIAnimator.AnimatorState state)
	{
		if (state.DirectionalAnimation != null && state.DirectionalAnimation.Type == DirectionalAnimation.DirectionType.None)
		{
			return false;
		}
		if (state.AnimationClip != null)
		{
			this.m_currentActionState = state;
			base.spriteAnimator.Stop();
			this.PlayClip(this.m_currentActionState.AnimationClip, state.WarpClipDuration);
			this.m_currentActionState.HasStarted = true;
			this.m_playingHitEffect = false;
			return true;
		}
		return false;
	}

	// Token: 0x06005678 RID: 22136 RVA: 0x0020F21C File Offset: 0x0020D41C
	public void SetBaseAnim(string name, bool useFidgetTimer = false)
	{
		if (this.ChildAnimator)
		{
			this.ChildAnimator.SetBaseAnim(name, false);
		}
		if (!this.HasDirectionalAnimation(name))
		{
			return;
		}
		this.m_baseDirectionalAnimationOverride = this.GetDirectionalAnimation(name);
		if (useFidgetTimer)
		{
			tk2dSpriteAnimationClip clipByName = base.spriteAnimator.GetClipByName(this.m_baseDirectionalAnimationOverride.GetInfo(this.FacingDirection, false).name);
			this.m_currentOverrideBaseClip = clipByName;
			this.m_fidgetCooldown = 2f;
			this.m_fidgetTimer = clipByName.BaseClipLength;
		}
	}

	// Token: 0x06005679 RID: 22137 RVA: 0x0020F2A8 File Offset: 0x0020D4A8
	public void ClearBaseAnim()
	{
		if (this.ChildAnimator)
		{
			this.ChildAnimator.ClearBaseAnim();
		}
		this.m_baseDirectionalAnimationOverride = null;
	}

	// Token: 0x0600567A RID: 22138 RVA: 0x0020F2CC File Offset: 0x0020D4CC
	public bool IsIdle()
	{
		return this.m_currentActionState == null;
	}

	// Token: 0x0600567B RID: 22139 RVA: 0x0020F2D8 File Offset: 0x0020D4D8
	public bool IsPlaying(string animName)
	{
		return (this.m_currentActionState != null && this.m_currentActionState.Name == animName && base.spriteAnimator.Playing) || base.spriteAnimator.IsPlaying(animName) || (this.ChildAnimator && this.ChildAnimator.IsPlaying(animName));
	}

	// Token: 0x0600567C RID: 22140 RVA: 0x0020F34C File Offset: 0x0020D54C
	public bool GetWrapType(string animName, out tk2dSpriteAnimationClip.WrapMode wrapMode)
	{
		DirectionalAnimation directionalAnimation = this.GetDirectionalAnimation(animName);
		if (directionalAnimation != null)
		{
			tk2dSpriteAnimationClip clipByName = base.spriteAnimator.GetClipByName(directionalAnimation.GetInfo(0).name);
			if (clipByName != null)
			{
				wrapMode = clipByName.wrapMode;
				return true;
			}
		}
		if (this.ChildAnimator)
		{
			return this.ChildAnimator.GetWrapType(animName, out wrapMode);
		}
		wrapMode = tk2dSpriteAnimationClip.WrapMode.Single;
		return false;
	}

	// Token: 0x0600567D RID: 22141 RVA: 0x0020F3B4 File Offset: 0x0020D5B4
	public bool EndAnimation()
	{
		bool flag = false;
		if (this.ChildAnimator)
		{
			flag = this.ChildAnimator.EndAnimation();
		}
		if (this.m_currentActionState != null)
		{
			this.m_currentActionState = null;
			return true;
		}
		return flag;
	}

	// Token: 0x0600567E RID: 22142 RVA: 0x0020F3F4 File Offset: 0x0020D5F4
	public bool EndAnimationIf(string name)
	{
		if (this.OnEndAnimationIf != null)
		{
			this.OnEndAnimationIf(name);
		}
		bool flag = false;
		if (this.ChildAnimator)
		{
			flag = this.ChildAnimator.EndAnimationIf(name);
		}
		if (this.m_currentActionState != null && this.m_currentActionState.Name == name)
		{
			this.m_currentActionState = null;
			return true;
		}
		return flag;
	}

	// Token: 0x0600567F RID: 22143 RVA: 0x0020F464 File Offset: 0x0020D664
	public string PlayDefaultSpawnState()
	{
		bool flag;
		return this.PlayDefaultSpawnState(out flag);
	}

	// Token: 0x06005680 RID: 22144 RVA: 0x0020F47C File Offset: 0x0020D67C
	public string PlayDefaultSpawnState(out bool isPlayingAwaken)
	{
		isPlayingAwaken = false;
		if (this.ChildAnimator)
		{
			this.ChildAnimator.PlayDefaultSpawnState();
		}
		if (!base.enabled || this.m_hasPlayedAwaken)
		{
			return null;
		}
		tk2dSpriteAnimationClip clipByName = base.spriteAnimator.GetClipByName("spawn");
		string text;
		if (clipByName != null)
		{
			if (clipByName.wrapMode == tk2dSpriteAnimationClip.WrapMode.Loop || clipByName.wrapMode == tk2dSpriteAnimationClip.WrapMode.LoopSection || clipByName.wrapMode == tk2dSpriteAnimationClip.WrapMode.LoopFidget)
			{
				this.PlayForDuration(clipByName.name, 2f, true, null, -1f, false);
			}
			else
			{
				this.PlayUntilFinished(clipByName.name, true, null, -1f, false);
			}
			text = clipByName.name;
		}
		else
		{
			text = this.PlayDefaultAwakenedState();
			isPlayingAwaken = true;
		}
		this.m_hasPlayedAwaken = true;
		return text;
	}

	// Token: 0x06005681 RID: 22145 RVA: 0x0020F54C File Offset: 0x0020D74C
	public string PlayDefaultAwakenedState()
	{
		if (this.ChildAnimator)
		{
			this.ChildAnimator.PlayDefaultAwakenedState();
		}
		if (!base.enabled || this.m_hasPlayedAwaken)
		{
			return null;
		}
		this.m_hasPlayedAwaken = true;
		if (this.HasDirectionalAnimation("awaken"))
		{
			DirectionalAnimation directionalAnimation = this.GetDirectionalAnimation("awaken");
			if (directionalAnimation.Type == DirectionalAnimation.DirectionType.None)
			{
				return null;
			}
			tk2dSpriteAnimationClip tk2dSpriteAnimationClip = base.spriteAnimator.GetClipByName(directionalAnimation.GetInfo(this.FacingDirection, false).name);
			if (tk2dSpriteAnimationClip.wrapMode == tk2dSpriteAnimationClip.WrapMode.Loop || tk2dSpriteAnimationClip.wrapMode == tk2dSpriteAnimationClip.WrapMode.LoopSection || tk2dSpriteAnimationClip.wrapMode == tk2dSpriteAnimationClip.WrapMode.LoopFidget)
			{
				this.PlayForDuration("awaken", 2f, true, null, -1f, false);
			}
			else
			{
				this.PlayUntilFinished("awaken", true, null, -1f, false);
			}
			return "awaken";
		}
		else
		{
			tk2dSpriteAnimationClip tk2dSpriteAnimationClip = base.spriteAnimator.GetClipByName((this.FacingDirection >= 90f && this.FacingDirection <= 1270f) ? "awaken_left" : "awaken_right");
			if (tk2dSpriteAnimationClip == null)
			{
				tk2dSpriteAnimationClip = base.spriteAnimator.GetClipByName("awaken");
			}
			if (tk2dSpriteAnimationClip != null)
			{
				if (tk2dSpriteAnimationClip.wrapMode == tk2dSpriteAnimationClip.WrapMode.Loop || tk2dSpriteAnimationClip.wrapMode == tk2dSpriteAnimationClip.WrapMode.LoopSection || tk2dSpriteAnimationClip.wrapMode == tk2dSpriteAnimationClip.WrapMode.LoopFidget)
				{
					this.PlayForDuration(tk2dSpriteAnimationClip.name, 2f, true, null, -1f, false);
				}
				else
				{
					this.PlayUntilFinished(tk2dSpriteAnimationClip.name, true, null, -1f, false);
				}
				return tk2dSpriteAnimationClip.name;
			}
			return null;
		}
	}

	// Token: 0x06005682 RID: 22146 RVA: 0x0020F6F0 File Offset: 0x0020D8F0
	public void PlayHitState(Vector2 damageVector)
	{
		if (this.ChildAnimator)
		{
			this.ChildAnimator.PlayHitState(damageVector);
		}
		if (!base.enabled)
		{
			return;
		}
		if (this.SuppressHitStates)
		{
			return;
		}
		if (this.HitReactChance < 1f && UnityEngine.Random.value > this.HitReactChance)
		{
			return;
		}
		if (this.m_suppressHitReactTimer > 0f)
		{
			return;
		}
		if (this.FpsScale == 0f)
		{
			return;
		}
		DirectionalAnimation directionalAnimation = this.HitAnimation;
		if (this.m_currentActionState != null)
		{
			if (this.m_currentActionState.SuppressHitStates)
			{
				return;
			}
			string overrideHitStateName = this.m_currentActionState.OverrideHitStateName;
			if (!string.IsNullOrEmpty(overrideHitStateName))
			{
				if (this.HasDirectionalAnimation(overrideHitStateName))
				{
					directionalAnimation = this.GetDirectionalAnimation(overrideHitStateName);
				}
				else
				{
					UnityEngine.Debug.LogWarning("No override animation found with name " + overrideHitStateName);
				}
			}
		}
		if (directionalAnimation.Type != DirectionalAnimation.DirectionType.None)
		{
			if (this.HitType == AIAnimator.HitStateType.Basic)
			{
				Vector2 vector = ((!(damageVector == Vector2.zero)) ? damageVector : base.specRigidbody.Velocity);
				this.PlayClip(directionalAnimation.GetInfo(vector, false).name, -1f);
				this.m_playingHitEffect = true;
			}
			else
			{
				this.PlayClip(directionalAnimation.GetInfo(this.FacingDirection, false).name, -1f);
				this.m_playingHitEffect = true;
			}
		}
		if (this.MinTimeBetweenHitReacts > 0f)
		{
			this.m_suppressHitReactTimer = this.MinTimeBetweenHitReacts;
		}
	}

	// Token: 0x17000C47 RID: 3143
	// (get) Token: 0x06005683 RID: 22147 RVA: 0x0020F874 File Offset: 0x0020DA74
	public bool HasDefaultAnimation
	{
		get
		{
			return this.MoveAnimation.Type != DirectionalAnimation.DirectionType.None || this.IdleAnimation.Type != DirectionalAnimation.DirectionType.None || this.FlightAnimation.Type != DirectionalAnimation.DirectionType.None;
		}
	}

	// Token: 0x06005684 RID: 22148 RVA: 0x0020F8AC File Offset: 0x0020DAAC
	public bool HasDirectionalAnimation(string animName)
	{
		if (string.IsNullOrEmpty(animName))
		{
			return false;
		}
		if (animName.Equals("idle", StringComparison.OrdinalIgnoreCase))
		{
			return !string.IsNullOrEmpty(this.OverrideIdleAnimation) || this.IdleAnimation.Type != DirectionalAnimation.DirectionType.None;
		}
		if (animName.Equals("move", StringComparison.OrdinalIgnoreCase))
		{
			return !string.IsNullOrEmpty(this.OverrideMoveAnimation) || this.MoveAnimation.Type != DirectionalAnimation.DirectionType.None;
		}
		if (animName.Equals("talk", StringComparison.OrdinalIgnoreCase))
		{
			return this.TalkAnimation.Type != DirectionalAnimation.DirectionType.None;
		}
		if (animName.Equals("hit", StringComparison.OrdinalIgnoreCase))
		{
			return this.HitAnimation.Type != DirectionalAnimation.DirectionType.None;
		}
		if (animName.Equals("flight", StringComparison.OrdinalIgnoreCase))
		{
			return this.FlightAnimation.Type != DirectionalAnimation.DirectionType.None;
		}
		for (int i = 0; i < this.OtherAnimations.Count; i++)
		{
			if (animName.Equals(this.OtherAnimations[i].name, StringComparison.OrdinalIgnoreCase))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06005685 RID: 22149 RVA: 0x0020F9D8 File Offset: 0x0020DBD8
	public float GetDirectionalAnimationLength(string animName)
	{
		DirectionalAnimation directionalAnimation = this.GetDirectionalAnimation(animName);
		if (directionalAnimation == null)
		{
			return 0f;
		}
		return base.spriteAnimator.GetClipByName(directionalAnimation.GetInfo(-Vector2.up, false).name).BaseClipLength;
	}

	// Token: 0x06005686 RID: 22150 RVA: 0x0020FA20 File Offset: 0x0020DC20
	public void CopyStateFrom(AIAnimator other)
	{
		base.sprite.SetSprite(other.sprite.spriteId);
		base.spriteAnimator.PlayFrom(other.spriteAnimator.CurrentClip, other.spriteAnimator.clipTime);
		this.m_currentActionState = ((other.m_currentActionState != null) ? new AIAnimator.AnimatorState(other.m_currentActionState) : null);
		this.m_playingHitEffect = other.m_playingHitEffect;
		this.m_hasPlayedAwaken = other.m_hasPlayedAwaken;
		this.m_currentBaseClip = other.m_currentBaseClip;
		this.m_currentBaseArtAngle = other.m_currentBaseArtAngle;
		this.m_baseDirectionalAnimationOverride = other.m_baseDirectionalAnimationOverride;
		this.m_currentOverrideBaseClip = other.m_currentOverrideBaseClip;
		this.m_currentActionState = other.m_currentActionState;
		this.m_fidgetTimer = other.m_fidgetTimer;
		this.m_fidgetCooldown = other.m_fidgetCooldown;
		this.m_suppressHitReactTimer = other.m_suppressHitReactTimer;
		this.m_fpsScale = other.m_fpsScale;
	}

	// Token: 0x06005687 RID: 22151 RVA: 0x0020FB0C File Offset: 0x0020DD0C
	public void UpdateAnimation(float deltaTime)
	{
		AIAnimator aianimator = this;
		while (aianimator)
		{
			if (aianimator.spriteAnimator)
			{
				aianimator.spriteAnimator.UpdateAnimation(deltaTime);
			}
			aianimator = aianimator.ChildAnimator;
		}
	}

	// Token: 0x06005688 RID: 22152 RVA: 0x0020FB50 File Offset: 0x0020DD50
	public void PlayVfx(string name, Vector2? sourceNormal = null, Vector2? sourceVelocity = null, Vector2? position = null)
	{
		if (this.OnPlayVfx != null)
		{
			this.OnPlayVfx(name, sourceNormal, sourceVelocity, position);
		}
		if (this.ChildAnimator)
		{
			this.ChildAnimator.PlayVfx(name, null, null, null);
		}
		for (int i = 0; i < this.OtherVFX.Count; i++)
		{
			AIAnimator.NamedVFXPool namedVFXPool = this.OtherVFX[i];
			if (namedVFXPool.name == name)
			{
				if (position != null)
				{
					namedVFXPool.vfxPool.SpawnAtPosition(position.Value, 0f, base.transform, sourceNormal, sourceVelocity, null, true, null, null, false);
				}
				else if (namedVFXPool.anchorTransform)
				{
					if (sourceVelocity == null)
					{
						sourceVelocity = new Vector2?(new Vector2(1f, 0f).Rotate(namedVFXPool.anchorTransform.eulerAngles.z + 180f));
					}
					namedVFXPool.vfxPool.SpawnAtLocalPosition(Vector3.zero, 0f, namedVFXPool.anchorTransform, sourceNormal, sourceVelocity, true, null, false);
				}
				else
				{
					namedVFXPool.vfxPool.SpawnAtPosition(base.specRigidbody.UnitCenter, 0f, base.transform, sourceNormal, sourceVelocity, null, true, null, null, false);
				}
			}
		}
		for (int j = 0; j < this.OtherScreenShake.Count; j++)
		{
			AIAnimator.NamedScreenShake namedScreenShake = this.OtherScreenShake[j];
			if (namedScreenShake.name == name)
			{
				Vector2 vector;
				if (base.specRigidbody)
				{
					vector = base.specRigidbody.UnitCenter;
				}
				else
				{
					vector = base.sprite.WorldCenter;
				}
				GameManager.Instance.MainCameraController.DoScreenShake(namedScreenShake.screenShake, new Vector2?(vector), false);
			}
		}
	}

	// Token: 0x06005689 RID: 22153 RVA: 0x0020FD68 File Offset: 0x0020DF68
	public void StopVfx(string name)
	{
		if (this.OnStopVfx != null)
		{
			this.OnStopVfx(name);
		}
		if (this.ChildAnimator)
		{
			this.ChildAnimator.StopVfx(name);
		}
		for (int i = 0; i < this.OtherVFX.Count; i++)
		{
			AIAnimator.NamedVFXPool namedVFXPool = this.OtherVFX[i];
			if (namedVFXPool.name == name)
			{
				namedVFXPool.vfxPool.DestroyAll();
			}
		}
	}

	// Token: 0x0600568A RID: 22154 RVA: 0x0020FDF0 File Offset: 0x0020DFF0
	private void OnPreDeath(Vector2 deathDirection)
	{
		if (this.ForceKillVfxOnPreDeath)
		{
			for (int i = 0; i < this.OtherVFX.Count; i++)
			{
				this.OtherVFX[i].vfxPool.DestroyAll();
			}
		}
	}

	// Token: 0x0600568B RID: 22155 RVA: 0x0020FE3C File Offset: 0x0020E03C
	private void UpdateFacingDirection()
	{
		if (this.LockFacingDirection)
		{
			return;
		}
		if (this.UseAnimatedFacingDirection)
		{
			this.FacingDirection = this.AnimatedFacingDirection;
			return;
		}
		if (this.facingType == AIAnimator.FacingType.SlaveDirection)
		{
			this.FacingDirection = this.DirectionParent.FacingDirection;
			return;
		}
		if (this.facingType == AIAnimator.FacingType.Movement)
		{
			if (base.aiActor)
			{
				if (base.aiActor.VoluntaryMovementVelocity != Vector2.zero)
				{
					this.FacingDirection = base.aiActor.VoluntaryMovementVelocity.ToAngle();
				}
				else if (this.faceSouthWhenStopped)
				{
					this.FacingDirection = -90f;
				}
				else if (this.faceTargetWhenStopped && base.aiActor && base.aiActor.TargetRigidbody)
				{
					this.FacingDirection = BraveMathCollege.Atan2Degrees(base.aiActor.TargetRigidbody.UnitCenter - base.specRigidbody.UnitCenter);
				}
			}
			else if (base.specRigidbody.Velocity != Vector2.zero)
			{
				this.FacingDirection = base.specRigidbody.Velocity.ToAngle();
			}
		}
		else if (this.facingType == AIAnimator.FacingType.Target)
		{
			if (base.aiActor && base.aiActor.TargetRigidbody)
			{
				this.FacingDirection = BraveMathCollege.Atan2Degrees(base.aiActor.TargetRigidbody.UnitCenter - base.specRigidbody.UnitCenter);
			}
		}
		else if (base.talkDoer)
		{
			if (Mathf.Abs(base.specRigidbody.Velocity.x) > 0.0001f || Mathf.Abs(base.specRigidbody.Velocity.y) > 0.0001f)
			{
				this.FacingDirection = BraveMathCollege.Atan2Degrees(base.specRigidbody.Velocity);
			}
			else
			{
				PlayerController activePlayerClosestToPoint = GameManager.Instance.GetActivePlayerClosestToPoint(base.specRigidbody.UnitCenter, false);
				if (activePlayerClosestToPoint != null)
				{
					this.FacingDirection = BraveMathCollege.Atan2Degrees(activePlayerClosestToPoint.specRigidbody.UnitCenter - base.specRigidbody.UnitCenter);
				}
				else if (GameManager.Instance.PrimaryPlayer != null)
				{
					this.FacingDirection = BraveMathCollege.Atan2Degrees(GameManager.Instance.PrimaryPlayer.specRigidbody.UnitCenter - base.specRigidbody.UnitCenter);
				}
			}
		}
		else if (base.aiShooter && (base.aiShooter.CurrentGun != null || base.aiShooter.ManualGunAngle))
		{
			this.FacingDirection = base.aiShooter.GunAngle;
		}
		else if (base.aiActor && base.aiActor.TargetRigidbody)
		{
			this.FacingDirection = BraveMathCollege.Atan2Degrees(base.aiActor.TargetRigidbody.UnitCenter - base.specRigidbody.UnitCenter);
		}
		else if (base.specRigidbody && base.specRigidbody.Velocity != Vector2.zero)
		{
			this.FacingDirection = BraveMathCollege.Atan2Degrees(base.specRigidbody.Velocity);
		}
	}

	// Token: 0x0600568C RID: 22156 RVA: 0x002101D0 File Offset: 0x0020E3D0
	private void UpdateCurrentBaseAnimation()
	{
		bool flag = this.m_fidgetTimer > 0f;
		this.m_fidgetTimer -= this.LocalDeltaTime;
		if (flag && this.m_fidgetTimer <= 0f)
		{
			this.ClearBaseAnim();
		}
		if (this.m_fidgetTimer <= 0f && this.m_fidgetCooldown > 0f)
		{
			this.m_fidgetCooldown -= this.LocalDeltaTime;
		}
		bool flag2 = this.FlightAnimation.Type != DirectionalAnimation.DirectionType.None;
		bool flag3 = !string.IsNullOrEmpty(this.OverrideMoveAnimation) || this.MoveAnimation.Type != DirectionalAnimation.DirectionType.None;
		bool flag4 = !string.IsNullOrEmpty(this.OverrideIdleAnimation) || this.IdleAnimation.Type != DirectionalAnimation.DirectionType.None;
		DirectionalAnimation directionalAnimation = null;
		if (this.m_baseDirectionalAnimationOverride != null)
		{
			DirectionalAnimation.Info info = this.m_baseDirectionalAnimationOverride.GetInfo(this.FacingDirection, true);
			this.m_currentOverrideBaseClip = base.spriteAnimator.GetClipByName(info.name);
		}
		if (base.aiActor && flag2 && base.aiActor.IsFlying && base.aiActor.IsOverPit)
		{
			directionalAnimation = this.FlightAnimation;
		}
		else if (flag3 && base.specRigidbody && base.specRigidbody.Velocity != Vector2.zero)
		{
			if (!string.IsNullOrEmpty(this.OverrideMoveAnimation))
			{
				directionalAnimation = this.GetDirectionalAnimation(this.OverrideMoveAnimation);
			}
			else
			{
				directionalAnimation = this.MoveAnimation;
			}
		}
		else if (flag4)
		{
			if (!string.IsNullOrEmpty(this.OverrideIdleAnimation))
			{
				directionalAnimation = this.GetDirectionalAnimation(this.OverrideIdleAnimation);
			}
			else
			{
				directionalAnimation = this.IdleAnimation;
			}
			if (this.IdleFidgetAnimations.Count > 0 && this.m_fidgetTimer <= 0f && this.m_fidgetCooldown <= 0f)
			{
				float value = UnityEngine.Random.value;
				float num = BraveMathCollege.SliceProbability(0.2f, this.LocalDeltaTime);
				if (value < num)
				{
					this.SetBaseAnim(this.IdleFidgetAnimations[0].GetInfo(this.FacingDirection, true).name, true);
				}
			}
		}
		if (directionalAnimation == null && flag3)
		{
			directionalAnimation = this.MoveAnimation;
		}
		if (directionalAnimation != null && base.spriteAnimator)
		{
			DirectionalAnimation.Info info2 = directionalAnimation.GetInfo(this.FacingDirection, true);
			if (info2 != null)
			{
				this.m_currentBaseClip = base.spriteAnimator.GetClipByName(info2.name);
				this.m_currentBaseArtAngle = info2.artAngle;
			}
		}
	}

	// Token: 0x0600568D RID: 22157 RVA: 0x00210498 File Offset: 0x0020E698
	private DirectionalAnimation GetDirectionalAnimation(string animName)
	{
		if (string.IsNullOrEmpty(animName))
		{
			return null;
		}
		if (animName.Equals("idle", StringComparison.OrdinalIgnoreCase))
		{
			return this.IdleAnimation;
		}
		if (animName.Equals("move", StringComparison.OrdinalIgnoreCase))
		{
			return this.MoveAnimation;
		}
		if (animName.Equals("talk", StringComparison.OrdinalIgnoreCase))
		{
			return this.TalkAnimation;
		}
		if (animName.Equals("hit", StringComparison.OrdinalIgnoreCase))
		{
			return this.HitAnimation;
		}
		if (animName.Equals("flight", StringComparison.OrdinalIgnoreCase))
		{
			return this.FlightAnimation;
		}
		DirectionalAnimation directionalAnimation = null;
		int num = 0;
		for (int i = 0; i < this.OtherAnimations.Count; i++)
		{
			if (animName.Equals(this.OtherAnimations[i].name, StringComparison.OrdinalIgnoreCase))
			{
				num++;
				directionalAnimation = this.OtherAnimations[i].anim;
			}
		}
		if (num == 0)
		{
			return null;
		}
		if (num == 1)
		{
			return directionalAnimation;
		}
		int num2 = UnityEngine.Random.Range(0, num);
		num = 0;
		for (int j = 0; j < this.OtherAnimations.Count; j++)
		{
			if (animName.Equals(this.OtherAnimations[j].name, StringComparison.OrdinalIgnoreCase))
			{
				if (num == num2)
				{
					return this.OtherAnimations[j].anim;
				}
				num++;
			}
		}
		UnityEngine.Debug.LogError("GetDiretionalAnimation: THIS SHOULDN'T HAPPEN");
		return null;
	}

	// Token: 0x0600568E RID: 22158 RVA: 0x00210600 File Offset: 0x0020E800
	private void PlayClip(string clipName, float warpClipDuration)
	{
		this.PlayClip(base.spriteAnimator.GetClipByName(clipName), warpClipDuration);
	}

	// Token: 0x0600568F RID: 22159 RVA: 0x00210618 File Offset: 0x0020E818
	private void PlayClip(tk2dSpriteAnimationClip clip, float warpClipDuration)
	{
		base.spriteAnimator.Play(clip, 0f, this.GetFps(clip, warpClipDuration), false);
		this.UpdateFacingRotation();
	}

	// Token: 0x06005690 RID: 22160 RVA: 0x0021063C File Offset: 0x0020E83C
	private float GetFps(tk2dSpriteAnimationClip clip, float warpClipDuration = -1f)
	{
		if (warpClipDuration > 0f)
		{
			return (float)clip.frames.Length / warpClipDuration;
		}
		if (this.m_fpsScale != 1f)
		{
			return (this.m_fpsScale <= 0f) ? 1E-05f : (clip.fps * this.m_fpsScale);
		}
		return clip.fps;
	}

	// Token: 0x06005691 RID: 22161 RVA: 0x002106A0 File Offset: 0x0020E8A0
	private void UpdateFacingRotation()
	{
		if (this.directionalType == AIAnimator.DirectionalType.Sprite)
		{
			return;
		}
		float num = this.FacingDirection + this.RotationOffset;
		if (this.directionalType == AIAnimator.DirectionalType.SpriteAndRotation)
		{
			num -= ((this.m_currentActionState == null) ? this.m_currentBaseArtAngle : this.m_currentActionState.ArtAngle);
		}
		if (this.RotationQuantizeTo != 0f)
		{
			num = BraveMathCollege.QuantizeFloat(num, this.RotationQuantizeTo);
		}
		base.transform.rotation = Quaternion.Euler(0f, 0f, num);
		base.sprite.UpdateZDepth();
		base.sprite.ForceBuild();
	}

	// Token: 0x04004F45 RID: 20293
	public AIAnimator ChildAnimator;

	// Token: 0x04004F46 RID: 20294
	public AIActor SpecifyAiActor;

	// Token: 0x04004F47 RID: 20295
	public AIAnimator.FacingType facingType;

	// Token: 0x04004F48 RID: 20296
	[ShowInInspectorIf("ShowDirectionParent", true)]
	public AIAnimator DirectionParent;

	// Token: 0x04004F49 RID: 20297
	[ShowInInspectorIf("ShowFaceSouthWhenStopped", true)]
	public bool faceSouthWhenStopped;

	// Token: 0x04004F4A RID: 20298
	[ShowInInspectorIf("ShowFaceSouthWhenStopped", true)]
	public bool faceTargetWhenStopped;

	// Token: 0x04004F4B RID: 20299
	[HideInInspector]
	public float AnimatedFacingDirection = -90f;

	// Token: 0x04004F4D RID: 20301
	public AIAnimator.DirectionalType directionalType = AIAnimator.DirectionalType.Sprite;

	// Token: 0x04004F4E RID: 20302
	[ShowInInspectorIf("ShowRotationOptions", true)]
	public float RotationQuantizeTo;

	// Token: 0x04004F4F RID: 20303
	[ShowInInspectorIf("ShowRotationOptions", true)]
	public float RotationOffset;

	// Token: 0x04004F50 RID: 20304
	public bool ForceKillVfxOnPreDeath;

	// Token: 0x04004F51 RID: 20305
	public bool SuppressAnimatorFallback;

	// Token: 0x04004F52 RID: 20306
	public bool IsBodySprite = true;

	// Token: 0x04004F53 RID: 20307
	[Header("Animations")]
	public DirectionalAnimation IdleAnimation;

	// Token: 0x04004F54 RID: 20308
	[FormerlySerializedAs("BaseAnimation")]
	public DirectionalAnimation MoveAnimation;

	// Token: 0x04004F55 RID: 20309
	public DirectionalAnimation FlightAnimation;

	// Token: 0x04004F56 RID: 20310
	public DirectionalAnimation HitAnimation;

	// Token: 0x04004F57 RID: 20311
	[ShowInInspectorIf("ShowHitAnimationOptions", true)]
	public float HitReactChance = 1f;

	// Token: 0x04004F58 RID: 20312
	[ShowInInspectorIf("ShowHitAnimationOptions", true)]
	public float MinTimeBetweenHitReacts;

	// Token: 0x04004F59 RID: 20313
	[ShowInInspectorIf("ShowHitAnimationOptions", true)]
	public AIAnimator.HitStateType HitType;

	// Token: 0x04004F5A RID: 20314
	public DirectionalAnimation TalkAnimation;

	// Token: 0x04004F5B RID: 20315
	public List<AIAnimator.NamedDirectionalAnimation> OtherAnimations;

	// Token: 0x04004F5C RID: 20316
	public List<AIAnimator.NamedVFXPool> OtherVFX;

	// Token: 0x04004F5D RID: 20317
	public List<AIAnimator.NamedScreenShake> OtherScreenShake;

	// Token: 0x04004F5E RID: 20318
	public List<DirectionalAnimation> IdleFidgetAnimations;

	// Token: 0x04004F61 RID: 20321
	private float m_facingDirection;

	// Token: 0x04004F65 RID: 20325
	private float m_cachedTurbo = -1f;

	// Token: 0x04004F66 RID: 20326
	public AIAnimator.PlayUntilFinishedDelegate OnPlayUntilFinished;

	// Token: 0x04004F67 RID: 20327
	private const float c_FIDGET_COOLDOWN = 2f;

	// Token: 0x04004F68 RID: 20328
	public AIAnimator.EndAnimationIfDelegate OnEndAnimationIf;

	// Token: 0x04004F69 RID: 20329
	public AIAnimator.PlayVfxDelegate OnPlayVfx;

	// Token: 0x04004F6A RID: 20330
	public AIAnimator.StopVfxDelegate OnStopVfx;

	// Token: 0x04004F6B RID: 20331
	private bool m_playingHitEffect;

	// Token: 0x04004F6C RID: 20332
	private bool m_hasPlayedAwaken;

	// Token: 0x04004F6D RID: 20333
	private tk2dSpriteAnimationClip m_currentBaseClip;

	// Token: 0x04004F6E RID: 20334
	private float m_currentBaseArtAngle;

	// Token: 0x04004F6F RID: 20335
	private DirectionalAnimation m_baseDirectionalAnimationOverride;

	// Token: 0x04004F70 RID: 20336
	private tk2dSpriteAnimationClip m_currentOverrideBaseClip;

	// Token: 0x04004F71 RID: 20337
	private AIAnimator.AnimatorState m_currentActionState;

	// Token: 0x04004F72 RID: 20338
	private float m_fidgetTimer;

	// Token: 0x04004F73 RID: 20339
	private float m_fidgetCooldown;

	// Token: 0x04004F74 RID: 20340
	private float m_suppressHitReactTimer;

	// Token: 0x04004F75 RID: 20341
	private float m_fpsScale = 1f;

	// Token: 0x02000F92 RID: 3986
	public enum FacingType
	{
		// Token: 0x04004F77 RID: 20343
		Default,
		// Token: 0x04004F78 RID: 20344
		Target,
		// Token: 0x04004F79 RID: 20345
		Movement,
		// Token: 0x04004F7A RID: 20346
		SlaveDirection
	}

	// Token: 0x02000F93 RID: 3987
	public enum DirectionalType
	{
		// Token: 0x04004F7C RID: 20348
		Sprite = 10,
		// Token: 0x04004F7D RID: 20349
		Rotation = 20,
		// Token: 0x04004F7E RID: 20350
		SpriteAndRotation = 30
	}

	// Token: 0x02000F94 RID: 3988
	// (Invoke) Token: 0x06005693 RID: 22163
	public delegate void PlayUntilFinishedDelegate(string name, bool suppressHitStates = false, string overrideHitState = null, float warpClipDuration = -1f, bool skipChildAnimators = false);

	// Token: 0x02000F95 RID: 3989
	// (Invoke) Token: 0x06005697 RID: 22167
	public delegate void EndAnimationIfDelegate(string name);

	// Token: 0x02000F96 RID: 3990
	// (Invoke) Token: 0x0600569B RID: 22171
	public delegate void PlayVfxDelegate(string name, Vector2? sourceNormal, Vector2? sourceVelocity, Vector2? position);

	// Token: 0x02000F97 RID: 3991
	// (Invoke) Token: 0x0600569F RID: 22175
	public delegate void StopVfxDelegate(string name);

	// Token: 0x02000F98 RID: 3992
	private class AnimatorState
	{
		// Token: 0x060056A2 RID: 22178 RVA: 0x00210748 File Offset: 0x0020E948
		public AnimatorState()
		{
		}

		// Token: 0x060056A3 RID: 22179 RVA: 0x00210750 File Offset: 0x0020E950
		public AnimatorState(AIAnimator.AnimatorState other)
		{
			this.Name = other.Name;
			this.DirectionalAnimation = other.DirectionalAnimation;
			this.AnimationClip = other.AnimationClip;
			this.EndType = other.EndType;
			this.Timer = other.Timer;
			this.WarpClipDuration = other.WarpClipDuration;
			this.ArtAngle = other.ArtAngle;
			this.SuppressHitStates = other.SuppressHitStates;
			this.OverrideHitStateName = other.OverrideHitStateName;
			this.HasStarted = other.HasStarted;
		}

		// Token: 0x060056A4 RID: 22180 RVA: 0x002107DC File Offset: 0x0020E9DC
		public void Update(tk2dSpriteAnimator spriteAnimator, float facingDirection)
		{
			if (this.DirectionalAnimation != null)
			{
				DirectionalAnimation.Info info = this.DirectionalAnimation.GetInfo(facingDirection, true);
				if (info != null)
				{
					this.AnimationClip = spriteAnimator.GetClipByName(info.name);
					this.ArtAngle = info.artAngle;
				}
			}
		}

		// Token: 0x04004F7F RID: 20351
		public string Name;

		// Token: 0x04004F80 RID: 20352
		public DirectionalAnimation DirectionalAnimation;

		// Token: 0x04004F81 RID: 20353
		public tk2dSpriteAnimationClip AnimationClip;

		// Token: 0x04004F82 RID: 20354
		public AIAnimator.AnimatorState.StateEndType EndType;

		// Token: 0x04004F83 RID: 20355
		public float Timer;

		// Token: 0x04004F84 RID: 20356
		public float WarpClipDuration;

		// Token: 0x04004F85 RID: 20357
		public float ArtAngle;

		// Token: 0x04004F86 RID: 20358
		public bool SuppressHitStates;

		// Token: 0x04004F87 RID: 20359
		public string OverrideHitStateName;

		// Token: 0x04004F88 RID: 20360
		public bool HasStarted;

		// Token: 0x02000F99 RID: 3993
		public enum StateEndType
		{
			// Token: 0x04004F8A RID: 20362
			UntilCancelled,
			// Token: 0x04004F8B RID: 20363
			UntilFinished,
			// Token: 0x04004F8C RID: 20364
			Duration,
			// Token: 0x04004F8D RID: 20365
			DurationOrFinished
		}
	}

	// Token: 0x02000F9A RID: 3994
	public enum HitStateType
	{
		// Token: 0x04004F8F RID: 20367
		Basic,
		// Token: 0x04004F90 RID: 20368
		FacingDirection
	}

	// Token: 0x02000F9B RID: 3995
	[Serializable]
	public class NamedDirectionalAnimation
	{
		// Token: 0x04004F91 RID: 20369
		public string name;

		// Token: 0x04004F92 RID: 20370
		public DirectionalAnimation anim;
	}

	// Token: 0x02000F9C RID: 3996
	[Serializable]
	public class NamedVFXPool
	{
		// Token: 0x04004F93 RID: 20371
		public string name;

		// Token: 0x04004F94 RID: 20372
		public Transform anchorTransform;

		// Token: 0x04004F95 RID: 20373
		public VFXPool vfxPool;
	}

	// Token: 0x02000F9D RID: 3997
	[Serializable]
	public class NamedScreenShake
	{
		// Token: 0x04004F96 RID: 20374
		public string name;

		// Token: 0x04004F97 RID: 20375
		public ScreenShakeSettings screenShake;
	}
}
