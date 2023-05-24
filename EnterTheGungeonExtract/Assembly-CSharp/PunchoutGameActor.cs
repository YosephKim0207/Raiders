using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001591 RID: 5521
public abstract class PunchoutGameActor : BraveBehaviour
{
	// Token: 0x170012BA RID: 4794
	// (get) Token: 0x06007E92 RID: 32402 RVA: 0x00332B54 File Offset: 0x00330D54
	// (set) Token: 0x06007E93 RID: 32403 RVA: 0x00332B5C File Offset: 0x00330D5C
	public int Stars { get; set; }

	// Token: 0x170012BB RID: 4795
	// (get) Token: 0x06007E94 RID: 32404 RVA: 0x00332B68 File Offset: 0x00330D68
	// (set) Token: 0x06007E95 RID: 32405 RVA: 0x00332B70 File Offset: 0x00330D70
	public PunchoutGameActor.State LastHitBy { get; set; }

	// Token: 0x170012BC RID: 4796
	// (get) Token: 0x06007E96 RID: 32406 RVA: 0x00332B7C File Offset: 0x00330D7C
	public int CurrentFrame
	{
		get
		{
			return base.spriteAnimator.CurrentFrame;
		}
	}

	// Token: 0x170012BD RID: 4797
	// (get) Token: 0x06007E97 RID: 32407 RVA: 0x00332B8C File Offset: 0x00330D8C
	public float CurrentFrameFloat
	{
		get
		{
			return (float)base.spriteAnimator.CurrentFrame + base.spriteAnimator.clipTime % 1f;
		}
	}

	// Token: 0x170012BE RID: 4798
	// (get) Token: 0x06007E98 RID: 32408 RVA: 0x00332BAC File Offset: 0x00330DAC
	// (set) Token: 0x06007E99 RID: 32409 RVA: 0x00332BB4 File Offset: 0x00330DB4
	public Vector2 CameraOffset { get; set; }

	// Token: 0x170012BF RID: 4799
	// (get) Token: 0x06007E9A RID: 32410 RVA: 0x00332BC0 File Offset: 0x00330DC0
	// (set) Token: 0x06007E9B RID: 32411 RVA: 0x00332BC8 File Offset: 0x00330DC8
	public bool IsYellow { get; set; }

	// Token: 0x170012C0 RID: 4800
	// (get) Token: 0x06007E9C RID: 32412 RVA: 0x00332BD4 File Offset: 0x00330DD4
	public bool IsFarAway
	{
		get
		{
			return this.state != null && this.state.IsFarAway();
		}
	}

	// Token: 0x170012C1 RID: 4801
	// (get) Token: 0x06007E9D RID: 32413
	public abstract bool IsDead { get; }

	// Token: 0x06007E9E RID: 32414 RVA: 0x00332BF4 File Offset: 0x00330DF4
	public virtual void Start()
	{
		SpriteOutlineManager.AddOutlineToSprite(base.sprite, Color.black, 0.1f, 0f, SpriteOutlineManager.OutlineType.NORMAL);
	}

	// Token: 0x06007E9F RID: 32415 RVA: 0x00332C14 File Offset: 0x00330E14
	public virtual void ManualUpdate()
	{
		if (this is PunchoutPlayerController && this.Health <= 0f && !(this.state is PunchoutPlayerController.DeathState))
		{
			this.Health = 100f;
			(this as PunchoutPlayerController).UpdateUI();
		}
		if (this.HealthBarUI)
		{
			this.HealthBarUI.FillAmount = Mathf.Max(0f, this.Health / 100f);
		}
		for (int i = 0; i < this.StarsUI.Length; i++)
		{
			this.StarsUI[i].IsVisible = i < this.Stars;
		}
		this.CameraOffset = Vector2.SmoothDamp(this.CameraOffset, this.m_cameraTarget, ref this.m_cameraVelocity, this.m_cameraTime, 100f, BraveTime.DeltaTime);
	}

	// Token: 0x06007EA0 RID: 32416 RVA: 0x00332CF0 File Offset: 0x00330EF0
	public virtual void Hit(bool isLeft, float damage, int starsUsed = 0, bool skipProcessing = false)
	{
	}

	// Token: 0x06007EA1 RID: 32417 RVA: 0x00332CF4 File Offset: 0x00330EF4
	public void Play(string name)
	{
		base.aiAnimator.FacingDirection = 90f;
		base.aiAnimator.PlayUntilFinished(name, false, null, -1f, false);
	}

	// Token: 0x06007EA2 RID: 32418 RVA: 0x00332D1C File Offset: 0x00330F1C
	public void Play(string name, bool isLeft)
	{
		base.aiAnimator.FacingDirection = (float)((!isLeft) ? 0 : 180);
		base.aiAnimator.PlayUntilFinished(name, false, null, -1f, false);
	}

	// Token: 0x06007EA3 RID: 32419 RVA: 0x00332D50 File Offset: 0x00330F50
	public void FlashDamage(float flashTime = 0.04f)
	{
		this.StopFlash();
		this.m_flashCoroutine = this.FlashColor(Color.white, flashTime, false);
		base.StartCoroutine(this.m_flashCoroutine);
	}

	// Token: 0x06007EA4 RID: 32420 RVA: 0x00332D78 File Offset: 0x00330F78
	public void FlashWarn(float flashFrames)
	{
		float num = flashFrames / base.spriteAnimator.ClipFps;
		this.StopFlash();
		this.IsYellow = true;
		this.m_flashCoroutine = this.FlashColor(Color.yellow, num, false);
		base.StartCoroutine(this.m_flashCoroutine);
		AkSoundEngine.PostEvent("Play_BOSS_RatPunchout_Flash_01", base.gameObject);
	}

	// Token: 0x06007EA5 RID: 32421 RVA: 0x00332DD4 File Offset: 0x00330FD4
	public void PulseColor(Color overrideColor, float flashFrames)
	{
		float num = flashFrames / base.spriteAnimator.ClipFps;
		this.StopFlash();
		this.m_flashCoroutine = this.FlashColor(overrideColor, num, true);
		base.StartCoroutine(this.m_flashCoroutine);
	}

	// Token: 0x06007EA6 RID: 32422 RVA: 0x00332E14 File Offset: 0x00331014
	protected IEnumerator FlashColor(Color overrideColor, float flashTime, bool roundtrip = false)
	{
		if (this is PunchoutPlayerController && (this as PunchoutPlayerController).IsEevee)
		{
			yield break;
		}
		this.m_isFlashing = true;
		overrideColor.a = 1f;
		if (base.sprite)
		{
			base.sprite.usesOverrideMaterial = true;
		}
		this.materialsToEnableBrightnessClampOn.Clear();
		this.materialsToFlash.Clear();
		this.outlineMaterialsToFlash.Clear();
		Material bodyMaterial = base.sprite.renderer.material;
		this.materialsToFlash.Add(bodyMaterial);
		for (int i = 0; i < bodyMaterial.shaderKeywords.Length; i++)
		{
			if (bodyMaterial.shaderKeywords[i] == "BRIGHTNESS_CLAMP_ON")
			{
				bodyMaterial.DisableKeyword("BRIGHTNESS_CLAMP_ON");
				bodyMaterial.EnableKeyword("BRIGHTNESS_CLAMP_OFF");
				this.materialsToEnableBrightnessClampOn.Add(bodyMaterial);
				break;
			}
		}
		tk2dSprite[] outlineSprites = SpriteOutlineManager.GetOutlineSprites(base.sprite);
		for (int j = 0; j < outlineSprites.Length; j++)
		{
			if (outlineSprites[j])
			{
				if (outlineSprites[j].renderer)
				{
					if (outlineSprites[j].renderer.material)
					{
						this.outlineMaterialsToFlash.Add(outlineSprites[j].renderer.material);
					}
				}
			}
		}
		this.sourceColors.Clear();
		for (int k = 0; k < this.materialsToFlash.Count; k++)
		{
			this.materialsToFlash[k].SetColor("_OverrideColor", overrideColor);
		}
		for (int l = 0; l < this.outlineMaterialsToFlash.Count; l++)
		{
			this.sourceColors.Add(this.outlineMaterialsToFlash[l].GetColor("_OverrideColor"));
			this.outlineMaterialsToFlash[l].SetColor("_OverrideColor", overrideColor);
		}
		for (float elapsed = 0f; elapsed < flashTime; elapsed += BraveTime.DeltaTime)
		{
			float t;
			Color baseColor;
			if (roundtrip)
			{
				t = Mathf.SmoothStep(0f, 1f, Mathf.PingPong(elapsed * 2f / flashTime, 1f));
				baseColor = new Color(0f, 0f, 0f, 0f);
			}
			else
			{
				t = 1f - elapsed / flashTime;
				baseColor = new Color(1f, 1f, 1f, 0f);
			}
			for (int m = 0; m < this.materialsToFlash.Count; m++)
			{
				this.materialsToFlash[m].SetColor("_OverrideColor", Color.Lerp(baseColor, overrideColor, t));
			}
			for (int n = 0; n < this.outlineMaterialsToFlash.Count; n++)
			{
				this.outlineMaterialsToFlash[n].SetColor("_OverrideColor", Color.Lerp(this.sourceColors[n], overrideColor, t));
			}
			yield return null;
		}
		this.StopFlash();
		yield break;
	}

	// Token: 0x06007EA7 RID: 32423 RVA: 0x00332E44 File Offset: 0x00331044
	private void StopFlash()
	{
		if (this is PunchoutPlayerController && (this as PunchoutPlayerController).IsEevee)
		{
			return;
		}
		for (int i = 0; i < this.materialsToFlash.Count; i++)
		{
			this.materialsToFlash[i].SetColor("_OverrideColor", new Color(1f, 1f, 1f, 0f));
		}
		for (int j = 0; j < this.outlineMaterialsToFlash.Count; j++)
		{
			this.outlineMaterialsToFlash[j].SetColor("_OverrideColor", this.sourceColors[j]);
		}
		for (int k = 0; k < this.materialsToEnableBrightnessClampOn.Count; k++)
		{
			this.materialsToEnableBrightnessClampOn[k].DisableKeyword("BRIGHTNESS_CLAMP_OFF");
			this.materialsToEnableBrightnessClampOn[k].EnableKeyword("BRIGHTNESS_CLAMP_ON");
		}
		this.m_isFlashing = false;
		this.IsYellow = false;
		if (this.m_flashCoroutine != null)
		{
			base.StopCoroutine(this.m_flashCoroutine);
		}
		this.m_flashCoroutine = null;
	}

	// Token: 0x06007EA8 RID: 32424 RVA: 0x00332F6C File Offset: 0x0033116C
	public void MoveCamera(Vector2 offset, float time)
	{
		this.m_cameraTarget = offset;
		this.m_cameraTime = time;
	}

	// Token: 0x170012C2 RID: 4802
	// (get) Token: 0x06007EA9 RID: 32425 RVA: 0x00332F7C File Offset: 0x0033117C
	// (set) Token: 0x06007EAA RID: 32426 RVA: 0x00332F84 File Offset: 0x00331184
	public PunchoutGameActor.State state
	{
		get
		{
			return this.m_state;
		}
		set
		{
			if (this.m_state != null)
			{
				this.m_state.Stop();
			}
			this.m_state = value;
			if (this.m_state != null)
			{
				this.m_state.Actor = this;
				this.m_state.Start();
			}
		}
	}

	// Token: 0x04008192 RID: 33170
	public dfSprite HealthBarUI;

	// Token: 0x04008193 RID: 33171
	public dfSprite[] StarsUI;

	// Token: 0x04008194 RID: 33172
	public PunchoutGameActor Opponent;

	// Token: 0x04008195 RID: 33173
	[NonSerialized]
	public float Health = 100f;

	// Token: 0x0400819A RID: 33178
	private IEnumerator m_flashCoroutine;

	// Token: 0x0400819B RID: 33179
	private PunchoutGameActor.State m_state;

	// Token: 0x0400819C RID: 33180
	private bool m_isFlashing;

	// Token: 0x0400819D RID: 33181
	protected List<Material> materialsToFlash = new List<Material>();

	// Token: 0x0400819E RID: 33182
	protected List<Material> outlineMaterialsToFlash = new List<Material>();

	// Token: 0x0400819F RID: 33183
	protected List<Material> materialsToEnableBrightnessClampOn = new List<Material>();

	// Token: 0x040081A0 RID: 33184
	protected List<Color> sourceColors = new List<Color>();

	// Token: 0x040081A1 RID: 33185
	private Vector2 m_cameraVelocity;

	// Token: 0x040081A2 RID: 33186
	private Vector2 m_cameraTarget;

	// Token: 0x040081A3 RID: 33187
	private float m_cameraTime;

	// Token: 0x02001592 RID: 5522
	public abstract class State
	{
		// Token: 0x06007EAB RID: 32427 RVA: 0x00332FD0 File Offset: 0x003311D0
		public State()
		{
		}

		// Token: 0x06007EAC RID: 32428 RVA: 0x00332FE0 File Offset: 0x003311E0
		public State(bool isLeft)
		{
			this.IsLeft = isLeft;
		}

		// Token: 0x170012C3 RID: 4803
		// (get) Token: 0x06007EAD RID: 32429 RVA: 0x00332FF8 File Offset: 0x003311F8
		// (set) Token: 0x06007EAE RID: 32430 RVA: 0x00333000 File Offset: 0x00331200
		public bool IsDone { get; set; }

		// Token: 0x170012C4 RID: 4804
		// (get) Token: 0x06007EAF RID: 32431 RVA: 0x0033300C File Offset: 0x0033120C
		// (set) Token: 0x06007EB0 RID: 32432 RVA: 0x00333014 File Offset: 0x00331214
		public PunchoutGameActor Actor { get; set; }

		// Token: 0x170012C5 RID: 4805
		// (get) Token: 0x06007EB1 RID: 32433 RVA: 0x00333020 File Offset: 0x00331220
		public PunchoutPlayerController ActorPlayer
		{
			get
			{
				return (PunchoutPlayerController)this.Actor;
			}
		}

		// Token: 0x170012C6 RID: 4806
		// (get) Token: 0x06007EB2 RID: 32434 RVA: 0x00333030 File Offset: 0x00331230
		public PunchoutAIActor ActorEnemy
		{
			get
			{
				return (PunchoutAIActor)this.Actor;
			}
		}

		// Token: 0x170012C7 RID: 4807
		// (get) Token: 0x06007EB3 RID: 32435 RVA: 0x00333040 File Offset: 0x00331240
		public virtual string AnimName
		{
			get
			{
				return null;
			}
		}

		// Token: 0x170012C8 RID: 4808
		// (get) Token: 0x06007EB4 RID: 32436 RVA: 0x00333044 File Offset: 0x00331244
		public virtual float PunishTime
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x170012C9 RID: 4809
		// (get) Token: 0x06007EB5 RID: 32437 RVA: 0x0033304C File Offset: 0x0033124C
		// (set) Token: 0x06007EB6 RID: 32438 RVA: 0x00333054 File Offset: 0x00331254
		public bool WasBlocked { get; set; }

		// Token: 0x06007EB7 RID: 32439 RVA: 0x00333060 File Offset: 0x00331260
		public virtual void Start()
		{
			if (this.AnimName != null)
			{
				this.Actor.Play(this.AnimName, this.IsLeft);
			}
			tk2dSpriteAnimator spriteAnimator = this.Actor.spriteAnimator;
			spriteAnimator.AnimationCompleted = (Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip>)Delegate.Combine(spriteAnimator.AnimationCompleted, new Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip>(this.AnimationCompleted));
		}

		// Token: 0x06007EB8 RID: 32440 RVA: 0x003330BC File Offset: 0x003312BC
		public virtual void Update()
		{
			if (this.AnimName != null && this.Actor.aiAnimator.IsIdle())
			{
				this.IsDone = true;
			}
			int i = this.Actor.spriteAnimator.CurrentFrame;
			if (i < this.m_lastReportedFrame)
			{
				if (this.Actor.spriteAnimator.CurrentClip.wrapMode == tk2dSpriteAnimationClip.WrapMode.LoopSection)
				{
					this.m_lastReportedFrame = i - 1;
				}
				else
				{
					this.m_lastReportedFrame = -1;
				}
			}
			while (i > this.m_lastReportedFrame)
			{
				this.m_lastReportedFrame++;
				this.OnFrame(this.m_lastReportedFrame);
			}
		}

		// Token: 0x06007EB9 RID: 32441 RVA: 0x00333168 File Offset: 0x00331368
		public virtual void Stop()
		{
			tk2dSpriteAnimator spriteAnimator = this.Actor.spriteAnimator;
			spriteAnimator.AnimationCompleted = (Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip>)Delegate.Remove(spriteAnimator.AnimationCompleted, new Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip>(this.AnimationCompleted));
		}

		// Token: 0x06007EBA RID: 32442 RVA: 0x00333198 File Offset: 0x00331398
		private void AnimationCompleted(tk2dSpriteAnimator tk2DSpriteAnimator, tk2dSpriteAnimationClip tk2DSpriteAnimationClip)
		{
			this.OnAnimationCompleted();
		}

		// Token: 0x06007EBB RID: 32443 RVA: 0x003331A0 File Offset: 0x003313A0
		public virtual void OnFrame(int currentFrame)
		{
		}

		// Token: 0x06007EBC RID: 32444 RVA: 0x003331A4 File Offset: 0x003313A4
		public virtual void OnHit(ref bool preventDamage, bool isLeft, int starsUsed)
		{
		}

		// Token: 0x06007EBD RID: 32445 RVA: 0x003331A8 File Offset: 0x003313A8
		public virtual void OnAnimationCompleted()
		{
		}

		// Token: 0x06007EBE RID: 32446 RVA: 0x003331AC File Offset: 0x003313AC
		public virtual bool CanBeHit(bool isLeft)
		{
			return true;
		}

		// Token: 0x06007EBF RID: 32447 RVA: 0x003331B0 File Offset: 0x003313B0
		public virtual bool IsFarAway()
		{
			return false;
		}

		// Token: 0x06007EC0 RID: 32448 RVA: 0x003331B4 File Offset: 0x003313B4
		public virtual bool ShouldInstantKO(int starsUsed)
		{
			return false;
		}

		// Token: 0x040081A6 RID: 33190
		public bool IsLeft;

		// Token: 0x040081A8 RID: 33192
		private int m_lastReportedFrame = -1;
	}

	// Token: 0x02001593 RID: 5523
	public class DuckState : PunchoutGameActor.State
	{
		// Token: 0x170012CA RID: 4810
		// (get) Token: 0x06007EC2 RID: 32450 RVA: 0x003331C0 File Offset: 0x003313C0
		public override string AnimName
		{
			get
			{
				return "duck";
			}
		}

		// Token: 0x06007EC3 RID: 32451 RVA: 0x003331C8 File Offset: 0x003313C8
		public override void Start()
		{
			base.Start();
			base.Actor.MoveCamera(new Vector2(0f, -0.4f), 0.2f);
		}

		// Token: 0x06007EC4 RID: 32452 RVA: 0x003331F0 File Offset: 0x003313F0
		public override void Stop()
		{
			base.Stop();
			base.Actor.MoveCamera(new Vector2(0f, 0f), 0.2f);
		}
	}

	// Token: 0x02001594 RID: 5524
	public class DodgeState : PunchoutGameActor.State
	{
		// Token: 0x06007EC5 RID: 32453 RVA: 0x00333218 File Offset: 0x00331418
		public DodgeState(bool isLeft)
			: base(isLeft)
		{
		}

		// Token: 0x170012CB RID: 4811
		// (get) Token: 0x06007EC6 RID: 32454 RVA: 0x00333224 File Offset: 0x00331424
		public override string AnimName
		{
			get
			{
				return "dodge";
			}
		}

		// Token: 0x06007EC7 RID: 32455 RVA: 0x0033322C File Offset: 0x0033142C
		public override void Start()
		{
			base.Start();
			base.Actor.MoveCamera(new Vector2(0.7f * (float)((!this.IsLeft) ? 1 : (-1)), 0f), 0.15f);
		}

		// Token: 0x06007EC8 RID: 32456 RVA: 0x00333268 File Offset: 0x00331468
		public override void Stop()
		{
			base.Stop();
			base.Actor.MoveCamera(new Vector2(0f, 0f), 0.25f);
		}
	}

	// Token: 0x02001595 RID: 5525
	public class HitState : PunchoutGameActor.State
	{
		// Token: 0x06007EC9 RID: 32457 RVA: 0x00333290 File Offset: 0x00331490
		public HitState(bool isLeft)
			: base(isLeft)
		{
		}

		// Token: 0x170012CC RID: 4812
		// (get) Token: 0x06007ECA RID: 32458 RVA: 0x0033329C File Offset: 0x0033149C
		public override string AnimName
		{
			get
			{
				return "hit";
			}
		}
	}

	// Token: 0x02001596 RID: 5526
	public class BlockState : PunchoutGameActor.State
	{
		// Token: 0x170012CD RID: 4813
		// (get) Token: 0x06007ECC RID: 32460 RVA: 0x003332AC File Offset: 0x003314AC
		public override string AnimName
		{
			get
			{
				return "block";
			}
		}

		// Token: 0x06007ECD RID: 32461 RVA: 0x003332B4 File Offset: 0x003314B4
		public virtual void Bonk()
		{
		}
	}

	// Token: 0x02001597 RID: 5527
	public abstract class BasicAttackState : PunchoutGameActor.State
	{
		// Token: 0x06007ECE RID: 32462 RVA: 0x003332B8 File Offset: 0x003314B8
		public BasicAttackState()
		{
		}

		// Token: 0x06007ECF RID: 32463 RVA: 0x003332C0 File Offset: 0x003314C0
		public BasicAttackState(bool isLeft)
			: base(isLeft)
		{
		}

		// Token: 0x170012CE RID: 4814
		// (get) Token: 0x06007ED0 RID: 32464
		public abstract int DamageFrame { get; }

		// Token: 0x170012CF RID: 4815
		// (get) Token: 0x06007ED1 RID: 32465
		public abstract float Damage { get; }

		// Token: 0x06007ED2 RID: 32466 RVA: 0x003332CC File Offset: 0x003314CC
		public override void OnFrame(int currentFrame)
		{
			base.OnFrame(currentFrame);
			if (currentFrame == this.DamageFrame && this.CanHitOpponent(base.Actor.Opponent.state))
			{
				base.Actor.Opponent.Hit(this.IsLeft, this.Damage, 0, false);
			}
		}

		// Token: 0x06007ED3 RID: 32467
		public abstract bool CanHitOpponent(PunchoutGameActor.State state);
	}

	// Token: 0x02001598 RID: 5528
	public abstract class BasicComboState : PunchoutGameActor.State
	{
		// Token: 0x06007ED4 RID: 32468 RVA: 0x00333328 File Offset: 0x00331528
		public BasicComboState()
		{
			this.States = new PunchoutGameActor.State[0];
		}

		// Token: 0x06007ED5 RID: 32469 RVA: 0x0033333C File Offset: 0x0033153C
		public BasicComboState(PunchoutGameActor.State[] states)
		{
			this.States = states;
		}

		// Token: 0x170012D0 RID: 4816
		// (get) Token: 0x06007ED6 RID: 32470 RVA: 0x0033334C File Offset: 0x0033154C
		public PunchoutGameActor.State CurrentState
		{
			get
			{
				return this.States[this.m_index];
			}
		}

		// Token: 0x06007ED7 RID: 32471 RVA: 0x0033335C File Offset: 0x0033155C
		public override void Start()
		{
			this.CurrentState.Actor = base.Actor;
			this.CurrentState.Start();
		}

		// Token: 0x06007ED8 RID: 32472 RVA: 0x0033337C File Offset: 0x0033157C
		public override void Update()
		{
			this.CurrentState.Update();
			this.CurrentState.WasBlocked = base.WasBlocked;
			if (this.CurrentState.IsDone)
			{
				this.CurrentState.Stop();
				this.m_index++;
				if (this.m_index >= this.States.Length || base.Actor.Opponent.IsDead)
				{
					base.IsDone = true;
					return;
				}
				this.CurrentState.Actor = base.Actor;
				this.CurrentState.Start();
				base.WasBlocked = false;
			}
		}

		// Token: 0x06007ED9 RID: 32473 RVA: 0x00333424 File Offset: 0x00331624
		public override void OnHit(ref bool preventDamage, bool isLeft, int starsUsed)
		{
			if (this.m_index < this.States.Length)
			{
				this.CurrentState.OnHit(ref preventDamage, isLeft, starsUsed);
			}
		}

		// Token: 0x06007EDA RID: 32474 RVA: 0x00333448 File Offset: 0x00331648
		public override bool CanBeHit(bool isLeft)
		{
			return this.m_index >= this.States.Length || (!base.WasBlocked && this.CurrentState.CanBeHit(isLeft));
		}

		// Token: 0x040081A9 RID: 33193
		public PunchoutGameActor.State[] States;

		// Token: 0x040081AA RID: 33194
		private int m_index;
	}
}
