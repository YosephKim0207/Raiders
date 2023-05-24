using System;
using System.Collections;
using System.Collections.Generic;
using Dungeonator;
using UnityEngine;

// Token: 0x02001161 RID: 4449
public class ForgeHammerController : DungeonPlaceableBehaviour, IPlaceConfigurable
{
	// Token: 0x17000E8F RID: 3727
	// (get) Token: 0x060062C2 RID: 25282 RVA: 0x00264B00 File Offset: 0x00262D00
	private float LocalDeltaTime
	{
		get
		{
			return BraveTime.DeltaTime * this.LocalTimeScale;
		}
	}

	// Token: 0x17000E90 RID: 3728
	// (get) Token: 0x060062C3 RID: 25283 RVA: 0x00264B10 File Offset: 0x00262D10
	// (set) Token: 0x060062C4 RID: 25284 RVA: 0x00264B18 File Offset: 0x00262D18
	public float LocalTimeScale
	{
		get
		{
			return this.m_localTimeScale;
		}
		set
		{
			base.spriteAnimator.OverrideTimeScale = value;
			this.m_localTimeScale = value;
		}
	}

	// Token: 0x060062C5 RID: 25285 RVA: 0x00264B30 File Offset: 0x00262D30
	public void Start()
	{
		PhysicsEngine.Instance.OnPostRigidbodyMovement += this.OnPostRigidbodyMovement;
	}

	// Token: 0x060062C6 RID: 25286 RVA: 0x00264B48 File Offset: 0x00262D48
	public void Update()
	{
		if (!this.m_isActive && this.State == ForgeHammerController.HammerState.Gone)
		{
			return;
		}
		if (this.m_isActive && this.State == ForgeHammerController.HammerState.Gone)
		{
			Vector2 unitBottomLeft = this.m_room.area.UnitBottomLeft;
			Vector2 unitTopRight = this.m_room.area.UnitTopRight;
			int num = 0;
			for (int i = 0; i < GameManager.Instance.AllPlayers.Length; i++)
			{
				PlayerController playerController = GameManager.Instance.AllPlayers[i];
				if (playerController && playerController.healthHaver.IsAlive)
				{
					Vector2 centerPosition = playerController.CenterPosition;
					if (BraveMathCollege.AABBContains(unitBottomLeft - Vector2.one, unitTopRight + Vector2.one, centerPosition))
					{
						num++;
					}
				}
			}
			if (num == 0)
			{
				this.Deactivate();
			}
		}
		this.m_timer = Mathf.Max(0f, this.m_timer - this.LocalDeltaTime);
		this.UpdateState(this.State);
	}

	// Token: 0x060062C7 RID: 25287 RVA: 0x00264C54 File Offset: 0x00262E54
	protected override void OnDestroy()
	{
		StaticReferenceManager.AllForgeHammers.Remove(this);
		base.OnDestroy();
	}

	// Token: 0x060062C8 RID: 25288 RVA: 0x00264C68 File Offset: 0x00262E68
	public void Activate()
	{
		if (this.m_isActive)
		{
			return;
		}
		if (this.DeactivateOnEnemiesCleared && !this.m_room.HasActiveEnemies(RoomHandler.ActiveEnemyType.RoomClear))
		{
			this.ForceStop();
			return;
		}
		this.m_isActive = true;
		if (this.State == ForgeHammerController.HammerState.Gone)
		{
			this.State = ForgeHammerController.HammerState.InitialDelay;
		}
	}

	// Token: 0x060062C9 RID: 25289 RVA: 0x00264CC0 File Offset: 0x00262EC0
	public void Deactivate()
	{
		if (!this.m_isActive)
		{
			return;
		}
		if (base.encounterTrackable)
		{
			GameStatsManager.Instance.HandleEncounteredObject(base.encounterTrackable);
		}
		this.m_isActive = false;
	}

	// Token: 0x17000E91 RID: 3729
	// (get) Token: 0x060062CA RID: 25290 RVA: 0x00264CF8 File Offset: 0x00262EF8
	// (set) Token: 0x060062CB RID: 25291 RVA: 0x00264D00 File Offset: 0x00262F00
	private ForgeHammerController.HammerState State
	{
		get
		{
			return this.m_state;
		}
		set
		{
			if (value != this.m_state)
			{
				this.EndState(this.m_state);
				this.m_state = value;
				this.BeginState(this.m_state);
			}
		}
	}

	// Token: 0x060062CC RID: 25292 RVA: 0x00264D30 File Offset: 0x00262F30
	private void BeginState(ForgeHammerController.HammerState state)
	{
		if (state == ForgeHammerController.HammerState.InitialDelay)
		{
			this.TargetAnimator.renderer.enabled = false;
			this.HitEffectAnimator.renderer.enabled = false;
			base.sprite.renderer.enabled = false;
			this.m_timer = this.InitialDelay;
		}
		else if (state == ForgeHammerController.HammerState.PreSwing)
		{
			this.m_targetPlayer = GameManager.Instance.GetRandomActivePlayer();
			if (GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER)
			{
				List<PlayerController> list = new List<PlayerController>(2);
				Vector2 unitBottomLeft = this.m_room.area.UnitBottomLeft;
				Vector2 unitTopRight = this.m_room.area.UnitTopRight;
				for (int i = 0; i < GameManager.Instance.AllPlayers.Length; i++)
				{
					PlayerController playerController = GameManager.Instance.AllPlayers[i];
					if (playerController && playerController.healthHaver.IsAlive)
					{
						Vector2 centerPosition = playerController.CenterPosition;
						if (BraveMathCollege.AABBContains(unitBottomLeft - Vector2.one, unitTopRight + Vector2.one, centerPosition))
						{
							list.Add(playerController);
						}
					}
				}
				if (list.Count > 0)
				{
					this.m_targetPlayer = BraveUtility.RandomElement<PlayerController>(list);
				}
			}
			IntVector2 intVector = this.m_targetPlayer.CenterPosition.ToIntVector2(VectorConversions.Floor);
			if (this.ForceLeft)
			{
				this.m_attackIsLeft = true;
			}
			else if (this.ForceRight)
			{
				this.m_attackIsLeft = false;
			}
			else
			{
				int num = 0;
				while (GameManager.Instance.Dungeon.data[intVector + IntVector2.Left * num].type != CellType.WALL)
				{
					num++;
				}
				int num2 = 0;
				while (GameManager.Instance.Dungeon.data[intVector + IntVector2.Right * num2].type != CellType.WALL)
				{
					num2++;
				}
				this.m_attackIsLeft = num < num2;
			}
			this.m_inAnim = ((!this.m_attackIsLeft) ? this.Hammer_Anim_In_Right : this.Hammer_Anim_In_Left);
			this.m_outAnim = ((!this.m_attackIsLeft) ? this.Hammer_Anim_Out_Right : this.Hammer_Anim_Out_Left);
			this.TargetAnimator.StopAndResetFrame();
			this.TargetAnimator.renderer.enabled = this.TracksPlayer;
			this.TargetAnimator.PlayAndDisableRenderer((!this.m_attackIsLeft) ? "hammer_right_target" : "hammer_left_target");
			this.m_targetOffset = ((!this.m_attackIsLeft) ? new Vector2(4.625f, 1.9375f) : new Vector2(1.9375f, 1.9375f));
			this.m_timer = this.FlashDurationBeforeAttack;
		}
		else if (state == ForgeHammerController.HammerState.Swing)
		{
			base.sprite.renderer.enabled = true;
			base.spriteAnimator.Play(this.m_inAnim);
			this.ShadowAnimator.renderer.enabled = true;
			this.ShadowAnimator.Play((!this.m_attackIsLeft) ? "hammer_right_slam_shadow" : "hammer_left_slam_shadow");
			base.sprite.HeightOffGround = -2.5f;
			base.sprite.UpdateZDepth();
			this.m_additionalTrackTimer = this.AdditionalTrackingTime;
		}
		else if (state == ForgeHammerController.HammerState.Grounded)
		{
			if (this.DoScreenShake)
			{
				GameManager.Instance.MainCameraController.DoScreenShake(this.ScreenShake, new Vector2?(base.specRigidbody.UnitCenter), false);
			}
			base.specRigidbody.enabled = true;
			base.specRigidbody.PixelColliders[0].ManualOffsetX = ((!this.m_attackIsLeft) ? 59 : 16);
			base.specRigidbody.PixelColliders[1].ManualOffsetX = ((!this.m_attackIsLeft) ? 59 : 16);
			base.specRigidbody.ForceRegenerate(null, null);
			base.specRigidbody.Reinitialize();
			Exploder.DoRadialMinorBreakableBreak(this.TargetAnimator.sprite.WorldCenter, 4f);
			this.HitEffectAnimator.renderer.enabled = true;
			this.HitEffectAnimator.PlayAndDisableRenderer((!this.m_attackIsLeft) ? "hammer_right_slam_vfx" : "hammer_left_slam_vfx");
			List<SpeculativeRigidbody> overlappingRigidbodies = PhysicsEngine.Instance.GetOverlappingRigidbodies(base.specRigidbody, null, false);
			for (int j = 0; j < overlappingRigidbodies.Count; j++)
			{
				if (overlappingRigidbodies[j].gameActor)
				{
					Vector2 vector = overlappingRigidbodies[j].UnitCenter - base.specRigidbody.UnitCenter;
					if (overlappingRigidbodies[j].gameActor is PlayerController)
					{
						PlayerController playerController2 = overlappingRigidbodies[j].gameActor as PlayerController;
						if (overlappingRigidbodies[j].CollideWithOthers)
						{
							if (!playerController2.DodgeRollIsBlink || !playerController2.IsDodgeRolling)
							{
								if (overlappingRigidbodies[j].healthHaver)
								{
									overlappingRigidbodies[j].healthHaver.ApplyDamage(0.5f, vector, StringTableManager.GetEnemiesString("#FORGE_HAMMER", -1), CoreDamageTypes.None, DamageCategory.Normal, false, null, false);
								}
								if (overlappingRigidbodies[j].knockbackDoer)
								{
									overlappingRigidbodies[j].knockbackDoer.ApplyKnockback(vector, this.KnockbackForcePlayers, false);
								}
							}
						}
					}
					else
					{
						if (overlappingRigidbodies[j].healthHaver)
						{
							overlappingRigidbodies[j].healthHaver.ApplyDamage(this.DamageToEnemies, vector, StringTableManager.GetEnemiesString("#FORGE_HAMMER", -1), CoreDamageTypes.None, DamageCategory.Normal, false, null, false);
						}
						if (overlappingRigidbodies[j].knockbackDoer)
						{
							overlappingRigidbodies[j].knockbackDoer.ApplyKnockback(vector, this.KnockbackForceEnemies, false);
						}
					}
				}
			}
			PhysicsEngine.Instance.RegisterOverlappingGhostCollisionExceptions(base.specRigidbody, null, false);
			if (this.DoGoopOnImpact)
			{
				DeadlyDeadlyGoopManager.GetGoopManagerForGoopType(this.GoopToDo).AddGoopRect(base.specRigidbody.UnitCenter + new Vector2(-1f, -1.25f), base.specRigidbody.UnitCenter + new Vector2(1f, 0.75f));
			}
			if (this.DoesBulletsOnImpact && this.m_isActive)
			{
				this.ShootPoint.transform.position = base.specRigidbody.UnitCenter;
				CellData cell = this.ShootPoint.transform.position.GetCell();
				if (cell != null && cell.type != CellType.WALL)
				{
					if (!this.m_bulletSource)
					{
						this.m_bulletSource = this.ShootPoint.gameObject.GetOrAddComponent<BulletScriptSource>();
					}
					this.m_bulletSource.BulletManager = base.bulletBank;
					this.m_bulletSource.BulletScript = this.BulletScript;
					this.m_bulletSource.Initialize();
				}
			}
			this.m_timer = UnityEngine.Random.Range(this.MinTimeToRestOnGround, this.MaxTimeToRestOnGround);
		}
		else if (state == ForgeHammerController.HammerState.UpSwing)
		{
			base.spriteAnimator.Play(this.m_outAnim);
			this.ShadowAnimator.PlayAndDisableRenderer((!this.m_attackIsLeft) ? "hammer_right_out_shadow" : "hammer_left_out_shadow");
		}
		else if (state == ForgeHammerController.HammerState.Gone)
		{
			base.sprite.renderer.enabled = false;
			this.m_timer = UnityEngine.Random.Range(this.MinTimeBetweenAttacks, this.MaxTimeBetweenAttacks);
		}
	}

	// Token: 0x060062CD RID: 25293 RVA: 0x00265524 File Offset: 0x00263724
	private void UpdateState(ForgeHammerController.HammerState state)
	{
		if (state == ForgeHammerController.HammerState.InitialDelay)
		{
			if (this.m_timer <= 0f)
			{
				this.State = ForgeHammerController.HammerState.PreSwing;
			}
		}
		else if (state == ForgeHammerController.HammerState.PreSwing)
		{
			if (this.m_timer <= 0f)
			{
				this.State = ForgeHammerController.HammerState.Swing;
			}
		}
		else if (state == ForgeHammerController.HammerState.Swing)
		{
			this.m_additionalTrackTimer -= this.LocalDeltaTime;
			if (!base.spriteAnimator.IsPlaying(this.m_inAnim))
			{
				this.State = ForgeHammerController.HammerState.Grounded;
			}
		}
		else if (state == ForgeHammerController.HammerState.Grounded)
		{
			if (this.m_timer <= 0f)
			{
				this.State = ForgeHammerController.HammerState.UpSwing;
			}
		}
		else if (state == ForgeHammerController.HammerState.UpSwing)
		{
			if (!base.spriteAnimator.IsPlaying(this.m_outAnim))
			{
				this.State = ForgeHammerController.HammerState.Gone;
			}
		}
		else if (state == ForgeHammerController.HammerState.Gone && this.m_timer <= 0f)
		{
			this.State = ForgeHammerController.HammerState.PreSwing;
		}
	}

	// Token: 0x060062CE RID: 25294 RVA: 0x0026561C File Offset: 0x0026381C
	private void EndState(ForgeHammerController.HammerState state)
	{
		if (state == ForgeHammerController.HammerState.Grounded)
		{
			base.specRigidbody.enabled = false;
		}
	}

	// Token: 0x060062CF RID: 25295 RVA: 0x00265634 File Offset: 0x00263834
	public void ConfigureOnPlacement(RoomHandler room)
	{
		StaticReferenceManager.AllForgeHammers.Add(this);
		this.m_room = room;
		if (room.visibility == RoomHandler.VisibilityStatus.CURRENT)
		{
			this.DoRealConfigure(true);
		}
		else
		{
			base.StartCoroutine(this.FrameDelayedConfigure());
		}
	}

	// Token: 0x060062D0 RID: 25296 RVA: 0x00265670 File Offset: 0x00263870
	private IEnumerator FrameDelayedConfigure()
	{
		yield return null;
		this.DoRealConfigure(false);
		yield break;
	}

	// Token: 0x060062D1 RID: 25297 RVA: 0x0026568C File Offset: 0x0026388C
	private void DoRealConfigure(bool activateNow)
	{
		if (this.ForceLeft)
		{
			base.transform.position += new Vector3(-1f, -1f, 0f);
		}
		else if (this.ForceRight)
		{
			base.transform.position += new Vector3(-3.5625f, -1f, 0f);
		}
		tk2dSpriteAnimator spriteAnimator = base.spriteAnimator;
		spriteAnimator.AnimationEventTriggered = (Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip, int>)Delegate.Combine(spriteAnimator.AnimationEventTriggered, new Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip, int>(this.HandleAnimationEvent));
		this.m_room.Entered += delegate(PlayerController a)
		{
			this.Activate();
		};
		if (activateNow)
		{
			this.Activate();
		}
		if (this.DeactivateOnEnemiesCleared)
		{
			RoomHandler room = this.m_room;
			room.OnEnemiesCleared = (Action)Delegate.Combine(room.OnEnemiesCleared, new Action(this.Deactivate));
		}
	}

	// Token: 0x060062D2 RID: 25298 RVA: 0x00265784 File Offset: 0x00263984
	private void HandleAnimationEvent(tk2dSpriteAnimator sourceAnimator, tk2dSpriteAnimationClip sourceClip, int sourceFrame)
	{
		if (this.State == ForgeHammerController.HammerState.Swing && sourceClip.frames[sourceFrame].eventInfo == "impact")
		{
			this.State = ForgeHammerController.HammerState.Grounded;
		}
	}

	// Token: 0x060062D3 RID: 25299 RVA: 0x002657B8 File Offset: 0x002639B8
	private void OnPostRigidbodyMovement()
	{
		if (this.TracksPlayer && (this.State == ForgeHammerController.HammerState.PreSwing || (this.m_additionalTrackTimer > 0f && this.State == ForgeHammerController.HammerState.Swing)))
		{
			this.UpdatePosition();
		}
	}

	// Token: 0x060062D4 RID: 25300 RVA: 0x002657F4 File Offset: 0x002639F4
	private void UpdatePosition()
	{
		Vector2 unitBottomLeft = this.m_room.area.UnitBottomLeft;
		Vector2 unitTopRight = this.m_room.area.UnitTopRight;
		Vector2 vector = this.m_targetPlayer.CenterPosition;
		vector = BraveMathCollege.ClampToBounds(vector, unitBottomLeft + Vector2.one, unitTopRight - Vector2.one);
		base.transform.position = (vector - this.m_targetOffset).Quantize(0.0625f);
		this.TargetAnimator.sprite.UpdateZDepth();
		base.sprite.UpdateZDepth();
	}

	// Token: 0x060062D5 RID: 25301 RVA: 0x00265890 File Offset: 0x00263A90
	private void ForceStop()
	{
		if (this.TargetAnimator)
		{
			this.TargetAnimator.renderer.enabled = false;
		}
		if (this.HitEffectAnimator)
		{
			this.HitEffectAnimator.renderer.enabled = false;
		}
		if (base.sprite)
		{
			base.sprite.renderer.enabled = false;
		}
		if (this.ShadowAnimator)
		{
			this.ShadowAnimator.renderer.enabled = false;
		}
		base.specRigidbody.enabled = false;
		if (this.m_bulletSource)
		{
			this.m_bulletSource.ForceStop();
		}
		this.State = ForgeHammerController.HammerState.Gone;
	}

	// Token: 0x04005DD9 RID: 24025
	[DwarfConfigurable]
	public bool TracksPlayer = true;

	// Token: 0x04005DDA RID: 24026
	[DwarfConfigurable]
	public bool DeactivateOnEnemiesCleared = true;

	// Token: 0x04005DDB RID: 24027
	[DwarfConfigurable]
	public bool ForceLeft;

	// Token: 0x04005DDC RID: 24028
	[DwarfConfigurable]
	public bool ForceRight;

	// Token: 0x04005DDD RID: 24029
	public float FlashDurationBeforeAttack = 0.5f;

	// Token: 0x04005DDE RID: 24030
	public float AdditionalTrackingTime = 0.25f;

	// Token: 0x04005DDF RID: 24031
	public float DamageToEnemies = 30f;

	// Token: 0x04005DE0 RID: 24032
	public float KnockbackForcePlayers = 50f;

	// Token: 0x04005DE1 RID: 24033
	public float KnockbackForceEnemies = 50f;

	// Token: 0x04005DE2 RID: 24034
	[DwarfConfigurable]
	public float InitialDelay = 1f;

	// Token: 0x04005DE3 RID: 24035
	[DwarfConfigurable]
	public float MinTimeBetweenAttacks = 2f;

	// Token: 0x04005DE4 RID: 24036
	[DwarfConfigurable]
	public float MaxTimeBetweenAttacks = 4f;

	// Token: 0x04005DE5 RID: 24037
	[DwarfConfigurable]
	public float MinTimeToRestOnGround = 1f;

	// Token: 0x04005DE6 RID: 24038
	[DwarfConfigurable]
	public float MaxTimeToRestOnGround = 1f;

	// Token: 0x04005DE7 RID: 24039
	public bool DoScreenShake;

	// Token: 0x04005DE8 RID: 24040
	public ScreenShakeSettings ScreenShake;

	// Token: 0x04005DE9 RID: 24041
	public string Hammer_Anim_In_Left;

	// Token: 0x04005DEA RID: 24042
	public string Hammer_Anim_Out_Left;

	// Token: 0x04005DEB RID: 24043
	public string Hammer_Anim_In_Right;

	// Token: 0x04005DEC RID: 24044
	public string Hammer_Anim_Out_Right;

	// Token: 0x04005DED RID: 24045
	public tk2dSpriteAnimator HitEffectAnimator;

	// Token: 0x04005DEE RID: 24046
	public tk2dSpriteAnimator TargetAnimator;

	// Token: 0x04005DEF RID: 24047
	public tk2dSpriteAnimator ShadowAnimator;

	// Token: 0x04005DF0 RID: 24048
	public bool DoGoopOnImpact;

	// Token: 0x04005DF1 RID: 24049
	[ShowInInspectorIf("DoGoopOnImpact", false)]
	public GoopDefinition GoopToDo;

	// Token: 0x04005DF2 RID: 24050
	[DwarfConfigurable]
	public bool DoesBulletsOnImpact;

	// Token: 0x04005DF3 RID: 24051
	[ShowInInspectorIf("DoGoopOnImpact", false)]
	public BulletScriptSelector BulletScript;

	// Token: 0x04005DF4 RID: 24052
	[ShowInInspectorIf("DoGoopOnImpact", false)]
	public Transform ShootPoint;

	// Token: 0x04005DF5 RID: 24053
	private float m_localTimeScale = 1f;

	// Token: 0x04005DF6 RID: 24054
	private ForgeHammerController.HammerState m_state = ForgeHammerController.HammerState.Gone;

	// Token: 0x04005DF7 RID: 24055
	private float m_timer;

	// Token: 0x04005DF8 RID: 24056
	private PlayerController m_targetPlayer;

	// Token: 0x04005DF9 RID: 24057
	private Vector2 m_targetOffset;

	// Token: 0x04005DFA RID: 24058
	private string m_inAnim;

	// Token: 0x04005DFB RID: 24059
	private string m_outAnim;

	// Token: 0x04005DFC RID: 24060
	private float m_additionalTrackTimer;

	// Token: 0x04005DFD RID: 24061
	private RoomHandler m_room;

	// Token: 0x04005DFE RID: 24062
	private bool m_isActive;

	// Token: 0x04005DFF RID: 24063
	private BulletScriptSource m_bulletSource;

	// Token: 0x04005E00 RID: 24064
	private bool m_attackIsLeft;

	// Token: 0x02001162 RID: 4450
	private enum HammerState
	{
		// Token: 0x04005E02 RID: 24066
		InitialDelay,
		// Token: 0x04005E03 RID: 24067
		PreSwing,
		// Token: 0x04005E04 RID: 24068
		Swing,
		// Token: 0x04005E05 RID: 24069
		Grounded,
		// Token: 0x04005E06 RID: 24070
		UpSwing,
		// Token: 0x04005E07 RID: 24071
		Gone
	}
}
