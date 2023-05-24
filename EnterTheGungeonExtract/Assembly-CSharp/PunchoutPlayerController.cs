using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200159A RID: 5530
public class PunchoutPlayerController : PunchoutGameActor
{
	// Token: 0x170012D3 RID: 4819
	// (get) Token: 0x06007EE2 RID: 32482 RVA: 0x003339B8 File Offset: 0x00331BB8
	// (set) Token: 0x06007EE3 RID: 32483 RVA: 0x003339C0 File Offset: 0x00331BC0
	public float CurrentExhaust { get; set; }

	// Token: 0x170012D4 RID: 4820
	// (get) Token: 0x06007EE4 RID: 32484 RVA: 0x003339CC File Offset: 0x00331BCC
	public int PlayerID
	{
		get
		{
			return this.m_playerId;
		}
	}

	// Token: 0x170012D5 RID: 4821
	// (get) Token: 0x06007EE5 RID: 32485 RVA: 0x003339D4 File Offset: 0x00331BD4
	public override bool IsDead
	{
		get
		{
			return base.state is PunchoutPlayerController.DeathState;
		}
	}

	// Token: 0x170012D6 RID: 4822
	// (get) Token: 0x06007EE6 RID: 32486 RVA: 0x003339E4 File Offset: 0x00331BE4
	public bool IsSlinger
	{
		get
		{
			return this.m_playerId == 6;
		}
	}

	// Token: 0x170012D7 RID: 4823
	// (get) Token: 0x06007EE7 RID: 32487 RVA: 0x003339F0 File Offset: 0x00331BF0
	// (set) Token: 0x06007EE8 RID: 32488 RVA: 0x003339F8 File Offset: 0x00331BF8
	public bool IsEevee { get; private set; }

	// Token: 0x170012D8 RID: 4824
	// (set) Token: 0x06007EE9 RID: 32489 RVA: 0x00333A04 File Offset: 0x00331C04
	public bool VfxIsAboveCharacter
	{
		set
		{
			tk2dBaseSprite sprite = base.aiAnimator.ChildAnimator.ChildAnimator.sprite;
			sprite.HeightOffGround = ((!value) ? (-0.05f) : 0.05f);
			sprite.UpdateZDepth();
		}
	}

	// Token: 0x06007EEA RID: 32490 RVA: 0x00333A48 File Offset: 0x00331C48
	public override void Start()
	{
		base.Start();
		this.m_actions = (PunchoutPlayerController.Action[])Enum.GetValues(typeof(PunchoutPlayerController.Action));
		this.m_inputLastPressed = new float[this.m_actions.Length];
		for (int i = 0; i < this.m_actions.Length; i++)
		{
			this.m_inputLastPressed[i] = 100f;
		}
		tk2dSpriteAnimator spriteAnimator = base.spriteAnimator;
		spriteAnimator.AnimationCompleted = (Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip>)Delegate.Combine(spriteAnimator.AnimationCompleted, new Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip>(this.HandleAnimationCompletedSwap));
	}

	// Token: 0x06007EEB RID: 32491 RVA: 0x00333AD8 File Offset: 0x00331CD8
	public override void ManualUpdate()
	{
		base.ManualUpdate();
		this.UpdateInput();
		this.CurrentExhaust = Mathf.Max(0f, this.CurrentExhaust - this.ExhauseRecoveryRate * BraveTime.DeltaTime);
		if (base.state != null)
		{
			base.state.Update();
			if (base.state.IsDone)
			{
				base.state = null;
			}
		}
		this.UpdateState();
	}

	// Token: 0x06007EEC RID: 32492 RVA: 0x00333B48 File Offset: 0x00331D48
	public void UpdateState()
	{
		if (base.state == null && this.CurrentExhaust >= this.MaxExhaust)
		{
			base.state = new PunchoutPlayerController.ExhaustState(null);
			return;
		}
		if (base.state == null || base.state is PunchoutGameActor.BlockState)
		{
			if (this.WasPressed(PunchoutPlayerController.Action.PunchLeft))
			{
				base.state = new PunchoutPlayerController.PlayerPunchState(true);
				return;
			}
			if (this.WasPressed(PunchoutPlayerController.Action.PunchRight))
			{
				base.state = new PunchoutPlayerController.PlayerPunchState(false);
				return;
			}
			if (this.WasPressed(PunchoutPlayerController.Action.Super) && base.Stars > 0)
			{
				base.state = new PunchoutPlayerController.PlayerSuperState(base.Stars);
				base.Stars = 0;
				return;
			}
		}
		if (base.state is PunchoutGameActor.DuckState && base.CurrentFrame >= 6)
		{
			if (this.WasPressed(PunchoutPlayerController.Action.PunchLeft))
			{
				base.state = new PunchoutPlayerController.PlayerPunchState(true);
				return;
			}
			if (this.WasPressed(PunchoutPlayerController.Action.PunchRight))
			{
				base.state = new PunchoutPlayerController.PlayerPunchState(false);
				return;
			}
		}
		if (base.state == null)
		{
			if (this.WasPressed(PunchoutPlayerController.Action.DodgeLeft))
			{
				base.state = new PunchoutPlayerController.PlayerDodgeState(true);
			}
			else if (this.WasPressed(PunchoutPlayerController.Action.DodgeRight))
			{
				base.state = new PunchoutPlayerController.PlayerDodgeState(false);
			}
			else if (this.WasPressed(PunchoutPlayerController.Action.Block))
			{
				base.state = new PunchoutPlayerController.PlayerBlockState();
			}
			else if (this.WasPressed(PunchoutPlayerController.Action.Duck))
			{
				base.state = new PunchoutPlayerController.PlayerDuckState();
			}
		}
	}

	// Token: 0x06007EED RID: 32493 RVA: 0x00333CCC File Offset: 0x00331ECC
	public override void Hit(bool isLeft, float damage, int starsUsed = 0, bool skipProcessing = false)
	{
		if (base.state is PunchoutPlayerController.DeathState)
		{
			return;
		}
		if (base.Stars > 0 && damage >= 4f)
		{
			this.RemoveStars();
		}
		bool flag = false;
		if (base.state != null)
		{
			base.state.OnHit(ref flag, isLeft, starsUsed);
		}
		AkSoundEngine.PostEvent("Play_CHR_general_hurt_01", base.gameObject);
		if (!this.CoopAnimator.IsPlaying("alarm"))
		{
			this.CoopAnimator.PlayUntilFinished("alarm", false, null, -1f, false);
		}
		if (this.Health - damage <= 0f)
		{
			this.Health = 0f;
			this.UpdateUI();
			BraveTime.RegisterTimeScaleMultiplier(0.25f, base.gameObject);
			base.FlashDamage((this.m_playerId != 5) ? 0.66f : 0.25f);
			base.aiAnimator.PlayVfx("death", null, null, null);
			base.state = new PunchoutPlayerController.DeathState(isLeft);
			return;
		}
		base.state = new PunchoutGameActor.HitState(isLeft);
		base.aiAnimator.PlayVfx("normal_hit", null, null, null);
		base.FlashDamage(0.04f);
		this.Health -= damage;
		GameManager.Instance.PrimaryPlayer.DoVibration(Vibration.Time.Normal, Vibration.Strength.Hard);
		this.UpdateUI();
		this.CurrentExhaust = 0f;
	}

	// Token: 0x06007EEE RID: 32494 RVA: 0x00333E64 File Offset: 0x00332064
	public void SwapPlayer(int? newPlayerIndex = null, bool keepEevee = false)
	{
		if (newPlayerIndex == null)
		{
			if (this.IsEevee && !keepEevee)
			{
				newPlayerIndex = new int?(0);
			}
			else
			{
				newPlayerIndex = new int?((this.m_playerId + 1) % (PunchoutPlayerController.PlayerNames.Length + 1));
			}
		}
		if (!keepEevee)
		{
			bool flag = newPlayerIndex.Value == 7;
			if (flag && !this.IsEevee)
			{
				this.IsEevee = true;
				base.sprite.usesOverrideMaterial = true;
				base.sprite.renderer.material.shader = Shader.Find("Brave/PlayerShaderEevee");
				base.sprite.renderer.sharedMaterial.SetTexture("_EeveeTex", this.CosmicTex);
				base.sprite.renderer.material.DisableKeyword("BRIGHTNESS_CLAMP_ON");
				base.sprite.renderer.material.EnableKeyword("BRIGHTNESS_CLAMP_OFF");
			}
			else if (!flag && this.IsEevee)
			{
				this.IsEevee = false;
				base.sprite.usesOverrideMaterial = false;
			}
		}
		if (this.IsEevee)
		{
			newPlayerIndex = new int?(UnityEngine.Random.Range(0, PunchoutPlayerController.PlayerNames.Length));
		}
		string text = PunchoutPlayerController.PlayerNames[this.m_playerId];
		string text2 = PunchoutPlayerController.PlayerNames[newPlayerIndex.Value];
		this.m_playerId = newPlayerIndex.Value;
		this.SwapAnim(base.aiAnimator.IdleAnimation, text, text2);
		this.SwapAnim(base.aiAnimator.HitAnimation, text, text2);
		for (int i = 0; i < base.aiAnimator.OtherAnimations.Count; i++)
		{
			this.SwapAnim(base.aiAnimator.OtherAnimations[i].anim, text, text2);
		}
		this.UpdateUI();
		List<AIAnimator.NamedDirectionalAnimation> otherAnimations = base.aiAnimator.ChildAnimator.OtherAnimations;
		otherAnimations[0].anim.Type = DirectionalAnimation.DirectionType.None;
		otherAnimations[1].anim.Type = DirectionalAnimation.DirectionType.None;
		otherAnimations[2].anim.Type = DirectionalAnimation.DirectionType.None;
		if (this.m_playerId == 4)
		{
			otherAnimations[0].anim.Type = DirectionalAnimation.DirectionType.Single;
			otherAnimations[0].anim.Prefix = "bullet_super_vfx";
			otherAnimations[1].anim.Type = DirectionalAnimation.DirectionType.Single;
			otherAnimations[1].anim.Prefix = "bullet_super_final_vfx";
		}
		else if (this.m_playerId == 5)
		{
			otherAnimations[0].anim.Type = DirectionalAnimation.DirectionType.Single;
			otherAnimations[0].anim.Prefix = "robot_super_vfx";
			otherAnimations[1].anim.Type = DirectionalAnimation.DirectionType.Single;
			otherAnimations[1].anim.Prefix = "robot_super_final_vfx";
			otherAnimations[2].anim.Type = DirectionalAnimation.DirectionType.Single;
			otherAnimations[2].anim.Prefix = "robot_knockout_vfx";
		}
		else if (this.m_playerId == 6)
		{
			otherAnimations[0].anim.Type = DirectionalAnimation.DirectionType.Single;
			otherAnimations[0].anim.Prefix = "slinger_super_vfx";
			otherAnimations[1].anim.Type = DirectionalAnimation.DirectionType.Single;
			otherAnimations[1].anim.Prefix = "slinger_super_final_vfx";
		}
	}

	// Token: 0x06007EEF RID: 32495 RVA: 0x003341DC File Offset: 0x003323DC
	private void SwapAnim(DirectionalAnimation directionalAnim, string oldName, string newName)
	{
		directionalAnim.Prefix = directionalAnim.Prefix.Replace(oldName, newName);
		for (int i = 0; i < directionalAnim.AnimNames.Length; i++)
		{
			directionalAnim.AnimNames[i] = directionalAnim.AnimNames[i].Replace(oldName, newName);
		}
	}

	// Token: 0x06007EF0 RID: 32496 RVA: 0x0033422C File Offset: 0x0033242C
	public void Win()
	{
		base.state = new PunchoutPlayerController.WinState();
		UnityEngine.Object.FindObjectOfType<PunchoutController>().DoWinFade(false);
	}

	// Token: 0x06007EF1 RID: 32497 RVA: 0x00334244 File Offset: 0x00332444
	public void AddStar()
	{
		base.Stars = Mathf.Min(base.Stars + 1, 3);
		AIAnimator childAnimator = base.aiAnimator.ChildAnimator.ChildAnimator;
		this.VfxIsAboveCharacter = true;
		if (base.Stars == 3)
		{
			childAnimator.PlayUntilFinished("get_star_three", false, null, -1f, false);
		}
		else if (base.Stars == 2)
		{
			childAnimator.PlayUntilFinished("get_star_two", false, null, -1f, false);
		}
		else
		{
			childAnimator.PlayUntilFinished("get_star_one", false, null, -1f, false);
		}
	}

	// Token: 0x06007EF2 RID: 32498 RVA: 0x003342DC File Offset: 0x003324DC
	public void RemoveStars()
	{
		for (int i = 0; i < base.Stars; i++)
		{
			dfSprite dfSprite = this.StarsUI[i];
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.starRemovedAnimationPrefab.gameObject);
			gameObject.transform.parent = dfSprite.transform.parent;
			gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
			gameObject.layer = dfSprite.gameObject.layer;
			dfSprite component = gameObject.GetComponent<dfSprite>();
			component.BringToFront();
			dfSprite.Parent.AddControl(component);
			dfSprite.Parent.BringToFront();
			component.ZOrder = dfSprite.ZOrder - 1;
			component.RelativePosition = dfSprite.RelativePosition + this.starRemovedOffset;
		}
		base.Stars = 0;
		AIAnimator childAnimator = base.aiAnimator.ChildAnimator.ChildAnimator;
		this.VfxIsAboveCharacter = true;
		childAnimator.PlayUntilFinished("lose_stars", false, null, -1f, false);
	}

	// Token: 0x06007EF3 RID: 32499 RVA: 0x003343E0 File Offset: 0x003325E0
	public void UpdateUI()
	{
		string text = PunchoutPlayerController.PlayerUiNames[this.m_playerId];
		this.HealthBarUI.SpriteName = "punch_health_bar_001";
		if (this.Health > 66f)
		{
			this.PlayerUiSprite.SpriteName = text + "1";
		}
		else if (this.Health > 33f)
		{
			this.PlayerUiSprite.SpriteName = text + "2";
		}
		else
		{
			this.PlayerUiSprite.SpriteName = text + "3";
		}
		if (this.IsEevee && this.PlayerUiSprite.OverrideMaterial == null)
		{
			Material material = UnityEngine.Object.Instantiate<Material>(this.PlayerUiSprite.Atlas.Material);
			material.shader = Shader.Find("Brave/Internal/GlitchEevee");
			material.SetTexture("_EeveeTex", this.CosmicTex);
			material.SetFloat("_WaveIntensity", 0.1f);
			material.SetFloat("_ColorIntensity", 0.015f);
			this.PlayerUiSprite.OverrideMaterial = material;
		}
		else if (!this.IsEevee && this.PlayerUiSprite.OverrideMaterial != null)
		{
			this.PlayerUiSprite.OverrideMaterial = null;
		}
	}

	// Token: 0x06007EF4 RID: 32500 RVA: 0x0033452C File Offset: 0x0033272C
	public void Exhaust(float? time = null)
	{
		base.state = new PunchoutPlayerController.ExhaustState(time);
	}

	// Token: 0x06007EF5 RID: 32501 RVA: 0x0033453C File Offset: 0x0033273C
	private void HandleAnimationCompletedSwap(tk2dSpriteAnimator arg1, tk2dSpriteAnimationClip arg2)
	{
		if (this.IsEevee)
		{
			this.SwapPlayer(new int?(UnityEngine.Random.Range(0, PunchoutPlayerController.PlayerNames.Length)), true);
			base.sprite.usesOverrideMaterial = true;
			base.sprite.renderer.material.shader = Shader.Find("Brave/PlayerShaderEevee");
			base.sprite.renderer.sharedMaterial.SetTexture("_EeveeTex", this.CosmicTex);
			base.sprite.renderer.material.DisableKeyword("BRIGHTNESS_CLAMP_ON");
			base.sprite.renderer.material.EnableKeyword("BRIGHTNESS_CLAMP_OFF");
		}
	}

	// Token: 0x06007EF6 RID: 32502 RVA: 0x003345EC File Offset: 0x003327EC
	private void UpdateInput()
	{
		if (this.m_inputLastPressed == null)
		{
			return;
		}
		IEnumerator enumerator = Enum.GetValues(typeof(PunchoutPlayerController.Action)).GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				PunchoutPlayerController.Action action = (PunchoutPlayerController.Action)obj;
				if (GameManager.HasInstance && !GameManager.Instance.IsPaused && this.WasPressedRaw(action))
				{
					this.m_inputLastPressed[(int)action] = 0f;
				}
				else
				{
					this.m_inputLastPressed[(int)action] += BraveTime.DeltaTime;
				}
			}
		}
		finally
		{
			IDisposable disposable;
			if ((disposable = enumerator as IDisposable) != null)
			{
				disposable.Dispose();
			}
		}
	}

	// Token: 0x06007EF7 RID: 32503 RVA: 0x003346AC File Offset: 0x003328AC
	private bool WasPressed(PunchoutPlayerController.Action action)
	{
		bool flag = this.m_inputLastPressed[(int)action] < this.InputBufferTime;
		if (flag)
		{
			this.m_inputLastPressed[(int)action] = 100f;
			if (PunchoutController.InTutorial)
			{
				PunchoutController.InputWasPressed((int)action);
			}
		}
		return flag;
	}

	// Token: 0x06007EF8 RID: 32504 RVA: 0x003346F0 File Offset: 0x003328F0
	private bool WasPressedRaw(PunchoutPlayerController.Action action)
	{
		BraveInput braveInput = ((!BraveInput.HasInstanceForPlayer(0)) ? null : BraveInput.GetInstanceForPlayer(0));
		if (braveInput)
		{
			switch (action)
			{
			case PunchoutPlayerController.Action.DodgeLeft:
				return braveInput.ActiveActions.PunchoutDodgeLeft.WasPressed;
			case PunchoutPlayerController.Action.DodgeRight:
				return braveInput.ActiveActions.PunchoutDodgeRight.WasPressed;
			case PunchoutPlayerController.Action.Block:
				return braveInput.ActiveActions.PunchoutBlock.WasPressed;
			case PunchoutPlayerController.Action.Duck:
				return braveInput.ActiveActions.PunchoutDuck.WasPressed;
			case PunchoutPlayerController.Action.PunchLeft:
				return braveInput.ActiveActions.PunchoutPunchLeft.WasPressed;
			case PunchoutPlayerController.Action.PunchRight:
				return braveInput.ActiveActions.PunchoutPunchRight.WasPressed;
			case PunchoutPlayerController.Action.Super:
				return braveInput.ActiveActions.PunchoutSuper.WasPressed;
			}
		}
		return false;
	}

	// Token: 0x040081B7 RID: 33207
	public dfSprite PlayerUiSprite;

	// Token: 0x040081B8 RID: 33208
	public AIAnimator CoopAnimator;

	// Token: 0x040081B9 RID: 33209
	public dfSprite starRemovedAnimationPrefab;

	// Token: 0x040081BA RID: 33210
	public Vector3 starRemovedOffset;

	// Token: 0x040081BB RID: 33211
	[Header("Constants")]
	public float InputBufferTime = 0.1f;

	// Token: 0x040081BC RID: 33212
	public float MaxExhaust = 2.9f;

	// Token: 0x040081BD RID: 33213
	public float ExhauseRecoveryRate = 0.2f;

	// Token: 0x040081BE RID: 33214
	public float BlockStickyFriction = 0.1f;

	// Token: 0x040081BF RID: 33215
	public float DuckCameraSway = 0.4f;

	// Token: 0x040081C0 RID: 33216
	public float DodgeCameraSway = 0.7f;

	// Token: 0x040081C1 RID: 33217
	public float PunchCameraSway = 0.4f;

	// Token: 0x040081C2 RID: 33218
	public float SuperBackCameraSway = 0.4f;

	// Token: 0x040081C3 RID: 33219
	public float SuperForwardCameraSway = 0.4f;

	// Token: 0x040081C4 RID: 33220
	[Header("Visuals")]
	public Texture2D CosmicTex;

	// Token: 0x040081C7 RID: 33223
	private PunchoutPlayerController.Action[] m_actions;

	// Token: 0x040081C8 RID: 33224
	private float[] m_inputLastPressed;

	// Token: 0x040081C9 RID: 33225
	private int m_playerId;

	// Token: 0x040081CA RID: 33226
	private static readonly string[] PlayerNames = new string[] { "convict", "hunter", "marine", "pilot", "bullet", "robot", "slinger" };

	// Token: 0x040081CB RID: 33227
	private static readonly string[] PlayerUiNames = new string[] { "punch_player_health_convict_00", "punch_player_health_hunter_00", "punch_player_health_marine_00", "punch_player_health_pilot_00", "punch_player_health_bullet_00", "punch_player_health_robot_00", "punch_player_health_slinger_00" };

	// Token: 0x0200159B RID: 5531
	public class PlayerBlockState : PunchoutGameActor.BlockState
	{
		// Token: 0x170012D9 RID: 4825
		// (get) Token: 0x06007EFB RID: 32507 RVA: 0x00334868 File Offset: 0x00332A68
		public override string AnimName
		{
			get
			{
				return "block";
			}
		}

		// Token: 0x06007EFC RID: 32508 RVA: 0x00334870 File Offset: 0x00332A70
		public override void Bonk()
		{
			base.ActorPlayer.VfxIsAboveCharacter = false;
			base.Actor.Play(this.HitAnimName);
			StickyFrictionManager.Instance.RegisterCustomStickyFriction(base.ActorPlayer.BlockStickyFriction, 0f, false, false);
			base.Actor.aiAnimator.PlayVfx("block_ss", null, null, null);
			GameManager.Instance.PrimaryPlayer.DoVibration(Vibration.Time.Quick, Vibration.Strength.Light);
			if (base.Actor.Opponent.state != null)
			{
				base.Actor.Opponent.state.WasBlocked = true;
			}
		}

		// Token: 0x06007EFD RID: 32509 RVA: 0x00334924 File Offset: 0x00332B24
		public override void OnHit(ref bool preventDamage, bool isLeft, int starsUsed)
		{
			base.OnHit(ref preventDamage, isLeft, starsUsed);
			base.ActorPlayer.VfxIsAboveCharacter = false;
			base.Actor.aiAnimator.ChildAnimator.ChildAnimator.PlayUntilFinished("block_break", false, null, -1f, false);
		}

		// Token: 0x040081CC RID: 33228
		public string HitAnimName = "block_hit";

		// Token: 0x040081CD RID: 33229
		private bool m_hasBonk;
	}

	// Token: 0x0200159C RID: 5532
	public class PlayerDuckState : PunchoutGameActor.DuckState
	{
		// Token: 0x06007EFF RID: 32511 RVA: 0x0033496C File Offset: 0x00332B6C
		public override void Start()
		{
			base.Start();
			base.Actor.MoveCamera(new Vector2(0f, -base.ActorPlayer.DuckCameraSway), 0.2f);
		}

		// Token: 0x06007F00 RID: 32512 RVA: 0x0033499C File Offset: 0x00332B9C
		public override void Stop()
		{
			base.Stop();
			base.Actor.MoveCamera(new Vector2(0f, 0f), 0.2f);
		}
	}

	// Token: 0x0200159D RID: 5533
	public class PlayerDodgeState : PunchoutGameActor.DodgeState
	{
		// Token: 0x06007F01 RID: 32513 RVA: 0x003349C4 File Offset: 0x00332BC4
		public PlayerDodgeState(bool isLeft)
			: base(isLeft)
		{
		}

		// Token: 0x06007F02 RID: 32514 RVA: 0x003349D0 File Offset: 0x00332BD0
		public override void Start()
		{
			base.Start();
			base.Actor.MoveCamera(new Vector2(base.ActorPlayer.DodgeCameraSway * (float)((!this.IsLeft) ? 1 : (-1)), 0f), 0.15f);
		}

		// Token: 0x06007F03 RID: 32515 RVA: 0x00334A1C File Offset: 0x00332C1C
		public override void OnFrame(int currentFrame)
		{
			base.OnFrame(currentFrame);
			if (currentFrame == 3)
			{
				base.Actor.MoveCamera(new Vector2(0f, 0f), 0.25f);
			}
		}

		// Token: 0x06007F04 RID: 32516 RVA: 0x00334A4C File Offset: 0x00332C4C
		public override void Stop()
		{
			base.Stop();
			base.Actor.MoveCamera(new Vector2(0f, 0f), 0.15f);
		}
	}

	// Token: 0x0200159E RID: 5534
	public class PlayerPunchState : PunchoutGameActor.BasicAttackState
	{
		// Token: 0x06007F05 RID: 32517 RVA: 0x00334A74 File Offset: 0x00332C74
		public PlayerPunchState(bool isLeft)
			: base(isLeft)
		{
		}

		// Token: 0x170012DA RID: 4826
		// (get) Token: 0x06007F06 RID: 32518 RVA: 0x00334A80 File Offset: 0x00332C80
		public override string AnimName
		{
			get
			{
				return "punch";
			}
		}

		// Token: 0x170012DB RID: 4827
		// (get) Token: 0x06007F07 RID: 32519 RVA: 0x00334A88 File Offset: 0x00332C88
		public override int DamageFrame
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x170012DC RID: 4828
		// (get) Token: 0x06007F08 RID: 32520 RVA: 0x00334A8C File Offset: 0x00332C8C
		public override float Damage
		{
			get
			{
				return 5f;
			}
		}

		// Token: 0x06007F09 RID: 32521 RVA: 0x00334A94 File Offset: 0x00332C94
		public override void Start()
		{
			base.Start();
			base.Actor.MoveCamera(new Vector2(0f, base.ActorPlayer.PunchCameraSway), 0.04f);
		}

		// Token: 0x06007F0A RID: 32522 RVA: 0x00334AC4 File Offset: 0x00332CC4
		public override bool CanHitOpponent(PunchoutGameActor.State state)
		{
			if (this.m_missed)
			{
				return false;
			}
			bool flag = !(state is PunchoutGameActor.BlockState) && (state == null || state.CanBeHit(this.IsLeft));
			if (PunchoutController.InTutorial)
			{
				base.ActorPlayer.CurrentExhaust = 0f;
				this.m_missed = true;
			}
			else if (!flag)
			{
				this.m_missed = true;
				base.ActorPlayer.CurrentExhaust += 1f;
				if (base.Actor.Opponent.IsFarAway)
				{
					base.ActorPlayer.Play("punch_miss_far", this.IsLeft);
					AIAnimator aiAnimator = base.Actor.aiAnimator;
					string text = "miss_alert";
					Vector2? vector = new Vector2?(base.Actor.transform.position.XY() + new Vector2(0.0625f, 4.25f));
					aiAnimator.PlayVfx(text, null, null, vector);
				}
				else
				{
					base.ActorPlayer.Play("punch_miss", this.IsLeft);
					Vector2 vector2 = new Vector2((!this.IsLeft) ? 0.3125f : (-0.1875f), 4.375f);
					if (base.ActorPlayer.PlayerID == 4)
					{
						vector2.x = 0.0625f;
					}
					else if (base.ActorPlayer.PlayerID == 5)
					{
						vector2.y += 0.3125f;
					}
					AIAnimator aiAnimator2 = base.Actor.Opponent.aiAnimator;
					string text = "block_poof";
					Vector2? vector = new Vector2?(base.Actor.transform.position.XY() + vector2);
					aiAnimator2.PlayVfx(text, null, null, vector);
					GameManager.Instance.PrimaryPlayer.DoVibration(Vibration.Time.Quick, Vibration.Strength.Light);
					if (base.ActorPlayer.IsSlinger)
					{
						base.ActorPlayer.aiAnimator.PlayVfx((!this.IsLeft) ? "shoot_right_miss" : "shoot_left_miss", null, null, null);
					}
				}
			}
			else
			{
				base.ActorPlayer.CurrentExhaust = 0f;
				if (base.Actor.Opponent.state is PunchoutAIActor.ThrowAmmoState)
				{
					this.m_missed = true;
					base.ActorPlayer.Play("punch_miss_far", this.IsLeft);
					base.Actor.Opponent.aiAnimator.PlayVfx("normal_hit", null, null, null);
				}
				if (base.ActorPlayer.IsSlinger)
				{
					base.ActorPlayer.aiAnimator.PlayVfx((!this.IsLeft) ? "shoot_right" : "shoot_left", null, null, null);
				}
			}
			return flag;
		}

		// Token: 0x06007F0B RID: 32523 RVA: 0x00334DE8 File Offset: 0x00332FE8
		public override void OnFrame(int currentFrame)
		{
			base.OnFrame(currentFrame);
			if ((!this.m_missed && currentFrame == 2) || (this.m_missed && currentFrame == 1))
			{
				base.Actor.MoveCamera(new Vector2(0f, 0f), 0.12f);
			}
		}

		// Token: 0x06007F0C RID: 32524 RVA: 0x00334E40 File Offset: 0x00333040
		public override void Stop()
		{
			base.Stop();
			base.Actor.MoveCamera(new Vector2(0f, 0f), 0.08f);
		}

		// Token: 0x170012DD RID: 4829
		// (get) Token: 0x06007F0D RID: 32525 RVA: 0x00334E68 File Offset: 0x00333068
		public int RealFrame
		{
			get
			{
				if (this.m_missed)
				{
					return base.Actor.CurrentFrame + 1;
				}
				return base.Actor.CurrentFrame;
			}
		}

		// Token: 0x040081CE RID: 33230
		private bool m_missed;
	}

	// Token: 0x0200159F RID: 5535
	public class PlayerSuperState : PunchoutGameActor.BasicAttackState
	{
		// Token: 0x06007F0E RID: 32526 RVA: 0x00334E90 File Offset: 0x00333090
		public PlayerSuperState(int starsUsed)
			: base(false)
		{
			this.m_starsUsed = starsUsed;
		}

		// Token: 0x170012DE RID: 4830
		// (get) Token: 0x06007F0F RID: 32527 RVA: 0x00334EA0 File Offset: 0x003330A0
		public override string AnimName
		{
			get
			{
				return "super";
			}
		}

		// Token: 0x170012DF RID: 4831
		// (get) Token: 0x06007F10 RID: 32528 RVA: 0x00334EA8 File Offset: 0x003330A8
		public override int DamageFrame
		{
			get
			{
				return 6;
			}
		}

		// Token: 0x170012E0 RID: 4832
		// (get) Token: 0x06007F11 RID: 32529 RVA: 0x00334EAC File Offset: 0x003330AC
		public override float Damage
		{
			get
			{
				return (float)(15 * this.m_starsUsed);
			}
		}

		// Token: 0x06007F12 RID: 32530 RVA: 0x00334EB8 File Offset: 0x003330B8
		public override void Start()
		{
			base.Start();
			float currentExhaust = base.ActorPlayer.CurrentExhaust;
			PunchoutAIActor punchoutAIActor = base.Actor.Opponent as PunchoutAIActor;
			if (punchoutAIActor.Phase == 2 && this.CanHitOpponent(punchoutAIActor.state) && (base.Actor.Opponent.Health <= this.Damage || punchoutAIActor.ShouldInstantKO(this.m_starsUsed)))
			{
				if (this.AnimName != null)
				{
					base.Actor.Play("super_final", this.IsLeft);
				}
				this.m_isFinal = true;
			}
			base.ActorPlayer.CurrentExhaust = currentExhaust;
			base.Actor.Opponent.spriteAnimator.Pause();
			base.Actor.Opponent.aiAnimator.ChildAnimator.spriteAnimator.Pause();
			base.Actor.Opponent.aiAnimator.ChildAnimator.ChildAnimator.spriteAnimator.Pause();
			base.Actor.MoveCamera(new Vector2(0f, -base.ActorPlayer.SuperBackCameraSway), 0.5f);
		}

		// Token: 0x06007F13 RID: 32531 RVA: 0x00334FE4 File Offset: 0x003331E4
		public override bool CanHitOpponent(PunchoutGameActor.State state)
		{
			if (this.m_isFinal)
			{
				return true;
			}
			bool flag = !base.Actor.Opponent.IsFarAway;
			if (!flag)
			{
				base.ActorPlayer.CurrentExhaust += 1f;
			}
			else
			{
				base.ActorPlayer.CurrentExhaust = 0f;
			}
			return flag;
		}

		// Token: 0x06007F14 RID: 32532 RVA: 0x00335048 File Offset: 0x00333248
		public override void OnFrame(int currentFrame)
		{
			if (currentFrame == this.DamageFrame)
			{
				base.Actor.Opponent.spriteAnimator.Resume();
				base.Actor.Opponent.aiAnimator.ChildAnimator.spriteAnimator.Resume();
				base.Actor.Opponent.aiAnimator.ChildAnimator.ChildAnimator.spriteAnimator.Resume();
				if (this.CanHitOpponent(base.Actor.Opponent.state))
				{
					base.Actor.Opponent.Hit(this.IsLeft, this.Damage, this.m_starsUsed, false);
				}
			}
			if (currentFrame == 6)
			{
				base.Actor.MoveCamera(new Vector2(0f, base.ActorPlayer.SuperForwardCameraSway), 0.08f);
			}
			else if (currentFrame == 7)
			{
				base.Actor.MoveCamera(new Vector2(0f, 0f), 0.5f);
			}
		}

		// Token: 0x06007F15 RID: 32533 RVA: 0x00335150 File Offset: 0x00333350
		public override void Stop()
		{
			base.Stop();
			base.Actor.MoveCamera(new Vector2(0f, 0f), 0.16f);
			if (base.Actor.Opponent.spriteAnimator.Paused)
			{
				base.Actor.Opponent.spriteAnimator.Resume();
				base.Actor.Opponent.aiAnimator.ChildAnimator.spriteAnimator.Resume();
				base.Actor.Opponent.aiAnimator.ChildAnimator.ChildAnimator.spriteAnimator.Resume();
			}
		}

		// Token: 0x040081CF RID: 33231
		private int m_starsUsed;

		// Token: 0x040081D0 RID: 33232
		private bool m_isFinal;
	}

	// Token: 0x020015A0 RID: 5536
	public class ExhaustState : PunchoutGameActor.State
	{
		// Token: 0x06007F16 RID: 32534 RVA: 0x003351F4 File Offset: 0x003333F4
		public ExhaustState(float? overrideExhaustTime = null)
		{
			this.m_overrideExhaustTime = overrideExhaustTime;
		}

		// Token: 0x170012E1 RID: 4833
		// (get) Token: 0x06007F17 RID: 32535 RVA: 0x00335204 File Offset: 0x00333404
		public override string AnimName
		{
			get
			{
				return "exhaust";
			}
		}

		// Token: 0x170012E2 RID: 4834
		// (get) Token: 0x06007F18 RID: 32536 RVA: 0x0033520C File Offset: 0x0033340C
		public int ExhaustCycles
		{
			get
			{
				return 3;
			}
		}

		// Token: 0x06007F19 RID: 32537 RVA: 0x00335210 File Offset: 0x00333410
		public override void Start()
		{
			base.Start();
			float? overrideExhaustTime = this.m_overrideExhaustTime;
			if (overrideExhaustTime != null)
			{
				base.Actor.aiAnimator.PlayForDurationOrUntilFinished(this.AnimName, this.m_overrideExhaustTime.Value, false, null, -1f, false);
				this.m_usesExhaustTime = true;
			}
		}

		// Token: 0x06007F1A RID: 32538 RVA: 0x00335268 File Offset: 0x00333468
		public override void OnFrame(int currentFrame)
		{
			base.OnFrame(currentFrame);
			if (currentFrame == base.Actor.spriteAnimator.CurrentClip.frames.Length - 1)
			{
				this.m_cycles++;
				if (!this.m_usesExhaustTime && this.m_cycles >= this.ExhaustCycles)
				{
					base.Actor.aiAnimator.EndAnimationIf(this.AnimName);
					base.IsDone = true;
				}
			}
		}

		// Token: 0x06007F1B RID: 32539 RVA: 0x003352E4 File Offset: 0x003334E4
		public override void Stop()
		{
			base.Stop();
			base.ActorPlayer.CurrentExhaust = 0f;
		}

		// Token: 0x040081D1 RID: 33233
		private int m_cycles;

		// Token: 0x040081D2 RID: 33234
		private bool m_usesExhaustTime;

		// Token: 0x040081D3 RID: 33235
		private float? m_overrideExhaustTime;
	}

	// Token: 0x020015A1 RID: 5537
	public class DeathState : PunchoutGameActor.State
	{
		// Token: 0x06007F1C RID: 32540 RVA: 0x003352FC File Offset: 0x003334FC
		public DeathState(bool isLeft)
			: base(isLeft)
		{
		}

		// Token: 0x06007F1D RID: 32541 RVA: 0x00335308 File Offset: 0x00333508
		public override void Start()
		{
			base.Start();
			base.Actor.aiAnimator.FacingDirection = (float)((!this.IsLeft) ? 0 : 180);
			base.ActorPlayer.VfxIsAboveCharacter = true;
			base.Actor.aiAnimator.PlayUntilCancelled("die", false, null, -1f, false);
		}

		// Token: 0x06007F1E RID: 32542 RVA: 0x0033536C File Offset: 0x0033356C
		public override void Update()
		{
			if (this.m_timer < 3f)
			{
				this.m_timer += Time.unscaledDeltaTime;
				if (this.m_timer > 2f)
				{
					BraveTime.SetTimeScaleMultiplier(Mathf.Lerp(0.25f, 1f, this.m_timer - 2f), base.Actor.gameObject);
				}
				if (this.m_timer >= 3f)
				{
					BraveTime.ClearMultiplier(base.Actor.gameObject);
					UnityEngine.Object.FindObjectOfType<PunchoutController>().DoLoseFade(false);
				}
			}
		}

		// Token: 0x06007F1F RID: 32543 RVA: 0x00335404 File Offset: 0x00333604
		public override bool CanBeHit(bool isLeft)
		{
			return false;
		}

		// Token: 0x040081D4 RID: 33236
		private float m_timer;
	}

	// Token: 0x020015A2 RID: 5538
	public class WinState : PunchoutGameActor.State
	{
		// Token: 0x06007F21 RID: 32545 RVA: 0x00335410 File Offset: 0x00333610
		public override void Start()
		{
			base.Start();
			base.Actor.aiAnimator.PlayUntilCancelled("win", false, null, -1f, false);
			base.ActorPlayer.CoopAnimator.PlayUntilCancelled("win", false, null, -1f, false);
		}

		// Token: 0x06007F22 RID: 32546 RVA: 0x00335460 File Offset: 0x00333660
		public override bool CanBeHit(bool isLeft)
		{
			return false;
		}
	}

	// Token: 0x020015A3 RID: 5539
	private enum Action
	{
		// Token: 0x040081D6 RID: 33238
		DodgeLeft,
		// Token: 0x040081D7 RID: 33239
		DodgeRight,
		// Token: 0x040081D8 RID: 33240
		Block,
		// Token: 0x040081D9 RID: 33241
		Duck,
		// Token: 0x040081DA RID: 33242
		PunchLeft,
		// Token: 0x040081DB RID: 33243
		PunchRight,
		// Token: 0x040081DC RID: 33244
		Super
	}
}
