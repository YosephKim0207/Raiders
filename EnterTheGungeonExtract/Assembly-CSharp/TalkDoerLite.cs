using System;
using System.Collections;
using System.Collections.Generic;
using Dungeonator;
using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;
using Pathfinding;
using UnityEngine;

// Token: 0x0200122F RID: 4655
public class TalkDoerLite : DungeonPlaceableBehaviour, IPlayerInteractable
{
	// Token: 0x17000F6D RID: 3949
	// (get) Token: 0x06006823 RID: 26659 RVA: 0x0028C418 File Offset: 0x0028A618
	// (set) Token: 0x06006824 RID: 26660 RVA: 0x0028C420 File Offset: 0x0028A620
	public TalkDoerLite.TalkingState State
	{
		get
		{
			return this.m_talkingState;
		}
		set
		{
			if (!this.SuppressReinteractDelay && this.m_talkingState != TalkDoerLite.TalkingState.None && value == TalkDoerLite.TalkingState.None && this.IsInteractable)
			{
				this.IsInteractable = false;
				this.m_setInteractableTimer = 0.5f;
			}
			this.m_talkingState = value;
			this.UpdateOutlines();
		}
	}

	// Token: 0x17000F6E RID: 3950
	// (get) Token: 0x06006825 RID: 26661 RVA: 0x0028C474 File Offset: 0x0028A674
	// (set) Token: 0x06006826 RID: 26662 RVA: 0x0028C484 File Offset: 0x0028A684
	public bool IsTalking
	{
		get
		{
			return this.State != TalkDoerLite.TalkingState.None;
		}
		set
		{
			this.State = ((!value) ? TalkDoerLite.TalkingState.None : TalkDoerLite.TalkingState.Conversation);
		}
	}

	// Token: 0x17000F6F RID: 3951
	// (get) Token: 0x06006827 RID: 26663 RVA: 0x0028C49C File Offset: 0x0028A69C
	// (set) Token: 0x06006828 RID: 26664 RVA: 0x0028C4A4 File Offset: 0x0028A6A4
	public bool IsPlayerInRange
	{
		get
		{
			return this.m_isPlayerInRange;
		}
		set
		{
			this.m_isPlayerInRange = value;
			this.UpdateOutlines();
		}
	}

	// Token: 0x17000F70 RID: 3952
	// (get) Token: 0x06006829 RID: 26665 RVA: 0x0028C4B4 File Offset: 0x0028A6B4
	// (set) Token: 0x0600682A RID: 26666 RVA: 0x0028C4BC File Offset: 0x0028A6BC
	public bool ShowOutlines
	{
		get
		{
			return this.m_showOutlines;
		}
		set
		{
			this.m_showOutlines = value;
			this.UpdateOutlines();
		}
	}

	// Token: 0x17000F71 RID: 3953
	// (get) Token: 0x0600682B RID: 26667 RVA: 0x0028C4CC File Offset: 0x0028A6CC
	// (set) Token: 0x0600682C RID: 26668 RVA: 0x0028C4D4 File Offset: 0x0028A6D4
	public bool AllowWalkAways
	{
		get
		{
			return this.m_allowWalkAways;
		}
		set
		{
			this.m_allowWalkAways = value;
		}
	}

	// Token: 0x17000F72 RID: 3954
	// (get) Token: 0x0600682D RID: 26669 RVA: 0x0028C4E0 File Offset: 0x0028A6E0
	// (set) Token: 0x0600682E RID: 26670 RVA: 0x0028C4E8 File Offset: 0x0028A6E8
	public bool IsInteractable
	{
		get
		{
			return this.m_isInteractable;
		}
		set
		{
			this.m_isInteractable = value;
			this.UpdateOutlines();
		}
	}

	// Token: 0x17000F73 RID: 3955
	// (get) Token: 0x0600682F RID: 26671 RVA: 0x0028C4F8 File Offset: 0x0028A6F8
	// (set) Token: 0x06006830 RID: 26672 RVA: 0x0028C500 File Offset: 0x0028A700
	public bool HasPlayerLocked
	{
		get
		{
			return this.m_hasPlayerLocked;
		}
		set
		{
			this.m_hasPlayerLocked = value;
			this.UpdateOutlines();
		}
	}

	// Token: 0x17000F74 RID: 3956
	// (get) Token: 0x06006831 RID: 26673 RVA: 0x0028C510 File Offset: 0x0028A710
	// (set) Token: 0x06006832 RID: 26674 RVA: 0x0028C518 File Offset: 0x0028A718
	public bool SuppressReinteractDelay { get; set; }

	// Token: 0x17000F75 RID: 3957
	// (get) Token: 0x06006833 RID: 26675 RVA: 0x0028C524 File Offset: 0x0028A724
	// (set) Token: 0x06006834 RID: 26676 RVA: 0x0028C52C File Offset: 0x0028A72C
	public bool SuppressRoomEnterExitEvents { get; set; }

	// Token: 0x17000F76 RID: 3958
	// (get) Token: 0x06006835 RID: 26677 RVA: 0x0028C538 File Offset: 0x0028A738
	// (set) Token: 0x06006836 RID: 26678 RVA: 0x0028C540 File Offset: 0x0028A740
	public PlayerInputState CachedPlayerInput { get; set; }

	// Token: 0x17000F77 RID: 3959
	// (get) Token: 0x06006837 RID: 26679 RVA: 0x0028C54C File Offset: 0x0028A74C
	// (set) Token: 0x06006838 RID: 26680 RVA: 0x0028C554 File Offset: 0x0028A754
	public PlayerController TalkingPlayer
	{
		get
		{
			return this.m_talkingPlayer;
		}
		set
		{
			if (value == null && this.m_talkingPlayer != null)
			{
				this.m_talkingPlayer.TalkPartner = null;
				this.m_talkingPlayer.IsTalking = false;
			}
			if (value != null && this.m_talkingPlayer == null)
			{
				value.TalkPartner = this;
				value.IsTalking = true;
			}
			if (value != null && this.m_talkingPlayer != null)
			{
				this.m_talkingPlayer.IsTalking = false;
				this.m_talkingPlayer.TalkPartner = null;
				value.IsTalking = true;
				value.TalkPartner = this;
			}
			this.m_talkingPlayer = value;
		}
	}

	// Token: 0x17000F78 RID: 3960
	// (get) Token: 0x06006839 RID: 26681 RVA: 0x0028C60C File Offset: 0x0028A80C
	// (set) Token: 0x0600683A RID: 26682 RVA: 0x0028C614 File Offset: 0x0028A814
	public PlayerController CompletedTalkingPlayer { get; set; }

	// Token: 0x17000F79 RID: 3961
	// (get) Token: 0x0600683B RID: 26683 RVA: 0x0028C620 File Offset: 0x0028A820
	public int NumTimesSpokenTo
	{
		get
		{
			return this.m_numTimesSpokenTo;
		}
	}

	// Token: 0x17000F7A RID: 3962
	// (get) Token: 0x0600683C RID: 26684 RVA: 0x0028C628 File Offset: 0x0028A828
	public RoomHandler ParentRoom
	{
		get
		{
			if (this.m_parentRoom == null)
			{
				this.m_parentRoom = GameManager.Instance.Dungeon.GetRoomFromPosition(base.transform.position.IntXY(VectorConversions.Floor));
			}
			return this.m_parentRoom;
		}
	}

	// Token: 0x17000F7B RID: 3963
	// (get) Token: 0x0600683D RID: 26685 RVA: 0x0028C664 File Offset: 0x0028A864
	// (set) Token: 0x0600683E RID: 26686 RVA: 0x0028C66C File Offset: 0x0028A86C
	public AIActor HostileObject { get; set; }

	// Token: 0x17000F7C RID: 3964
	// (get) Token: 0x0600683F RID: 26687 RVA: 0x0028C678 File Offset: 0x0028A878
	public bool IsPlayingZombieAnimation
	{
		get
		{
			return this.m_hasZombieTextBox && this.m_zombieBoxTimer > 0f && base.aiAnimator && base.aiAnimator.IsPlaying(this.m_zombieBoxTalkAnim);
		}
	}

	// Token: 0x06006840 RID: 26688 RVA: 0x0028C6C4 File Offset: 0x0028A8C4
	public static void ClearPerLevelData()
	{
		StaticReferenceManager.AllNpcs.Clear();
	}

	// Token: 0x06006841 RID: 26689 RVA: 0x0028C6D0 File Offset: 0x0028A8D0
	private void Start()
	{
		if (this.shadow != null)
		{
			tk2dBaseSprite component = this.shadow.GetComponent<tk2dBaseSprite>();
			if (component && component.HeightOffGround >= -1f && component.GetCurrentSpriteDef().name == "rogue_shadow" && this.shadow.layer == LayerMask.NameToLayer("FG_Critical"))
			{
				component.HeightOffGround = -5f;
				component.UpdateZDepth();
			}
		}
		this.m_overheadUIElementDelay = this.OverheadUIElementDelay;
		StaticReferenceManager.AllNpcs.Add(this);
		if (base.aiActor != null && !base.aiActor.IsNormalEnemy && !RoomHandler.unassignedInteractableObjects.Contains(this))
		{
			RoomHandler.unassignedInteractableObjects.Add(this);
		}
		if (base.specRigidbody != null)
		{
			SpeculativeRigidbody specRigidbody = base.specRigidbody;
			specRigidbody.OnRigidbodyCollision = (SpeculativeRigidbody.OnRigidbodyCollisionDelegate)Delegate.Combine(specRigidbody.OnRigidbodyCollision, new SpeculativeRigidbody.OnRigidbodyCollisionDelegate(this.OnRigidbodyCollision));
		}
		SpriteOutlineManager.AddOutlineToSprite(base.sprite, Color.black, this.OutlineDepth, this.OutlineLuminanceCutoff, SpriteOutlineManager.OutlineType.NORMAL);
		this.m_parentRoom = GameManager.Instance.Dungeon.GetRoomFromPosition(base.transform.position.IntXY(VectorConversions.Floor));
		if (this.AllowPlayerToPassEventually && base.specRigidbody != null && GameManager.Instance.CurrentLevelOverrideState != GameManager.LevelOverrideState.FOYER)
		{
			SpeculativeRigidbody specRigidbody2 = base.specRigidbody;
			specRigidbody2.OnRigidbodyCollision = (SpeculativeRigidbody.OnRigidbodyCollisionDelegate)Delegate.Combine(specRigidbody2.OnRigidbodyCollision, new SpeculativeRigidbody.OnRigidbodyCollisionDelegate(this.HandlePlayerTemporaryIncorporeality));
		}
		if (this.m_parentRoom != null)
		{
			this.m_parentRoom.Entered += this.PlayerEnteredRoom;
			this.m_parentRoom.Exited += this.PlayerExitedRoom;
			if (this.OptionalMinimapIcon)
			{
				Minimap.Instance.RegisterRoomIcon(this.m_parentRoom, this.OptionalMinimapIcon, false);
			}
		}
		if (this.IsPaletteSwapped)
		{
			base.sprite.usesOverrideMaterial = true;
			base.sprite.renderer.material.SetTexture("_PaletteTex", this.PaletteTexture);
		}
		if (this.DisableOnShortcutRun && GameManager.Instance.CurrentGameMode == GameManager.GameMode.SHORTCUT)
		{
			this.IsInteractable = false;
			SetNpcVisibility.SetVisible(this, false);
			this.ShowOutlines = false;
		}
		if (base.spriteAnimator)
		{
			tk2dSpriteAnimator spriteAnimator = base.spriteAnimator;
			spriteAnimator.AnimationEventTriggered = (Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip, int>)Delegate.Combine(spriteAnimator.AnimationEventTriggered, new Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip, int>(this.HandleAnimationEvent));
		}
		for (int i = 0; i < base.playmakerFsms.Length; i++)
		{
			PlayMakerFSM playMakerFSM = base.playmakerFsms[i];
			if (playMakerFSM && playMakerFSM.Fsm != null)
			{
				for (int j = 0; j < playMakerFSM.Fsm.States.Length; j++)
				{
					FsmState fsmState = playMakerFSM.Fsm.States[j];
					for (int k = 0; k < fsmState.Actions.Length; k++)
					{
						FsmStateAction fsmStateAction = fsmState.Actions[k];
						if (fsmStateAction is DialogueBox)
						{
							DialogueBox dialogueBox = fsmStateAction as DialogueBox;
							if (dialogueBox.AlternativeTalker != null)
							{
								TalkDoerLite instanceReference = this.GetInstanceReference(dialogueBox.AlternativeTalker);
								if (instanceReference)
								{
									dialogueBox.AlternativeTalker = instanceReference;
								}
							}
						}
					}
				}
			}
		}
	}

	// Token: 0x06006842 RID: 26690 RVA: 0x0028CA5C File Offset: 0x0028AC5C
	private TalkDoerLite GetInstanceReference(TalkDoerLite prefab)
	{
		if (!prefab)
		{
			return null;
		}
		for (int i = 0; i < this.ReassignPrefabReferences.Count; i++)
		{
			GameObject gameObject = this.ReassignPrefabReferences[i];
			if (gameObject.name.StartsWith(prefab.name))
			{
				TalkDoerLite component = gameObject.GetComponent<TalkDoerLite>();
				if (component)
				{
					return component;
				}
			}
		}
		return null;
	}

	// Token: 0x06006843 RID: 26691 RVA: 0x0028CACC File Offset: 0x0028ACCC
	private void OnEnable()
	{
		if (this && this.speakPoint)
		{
			TextBoxManager.ClearTextBoxImmediate(this.speakPoint);
		}
	}

	// Token: 0x06006844 RID: 26692 RVA: 0x0028CAF4 File Offset: 0x0028ACF4
	private void HandlePlayerTemporaryIncorporeality(CollisionData rigidbodyCollision)
	{
		if (rigidbodyCollision.OtherRigidbody.GetComponent<PlayerController>() != null)
		{
			this.m_collidedWithPlayerLastFrame = true;
			this.m_collidedWithPlayerTimer += BraveTime.DeltaTime;
			if (this.m_collidedWithPlayerTimer > 1f)
			{
				base.specRigidbody.RegisterTemporaryCollisionException(rigidbodyCollision.OtherRigidbody, 0.25f, null);
			}
		}
	}

	// Token: 0x06006845 RID: 26693 RVA: 0x0028CB60 File Offset: 0x0028AD60
	public void ConvertToGhost()
	{
		if (base.sprite && base.sprite.renderer)
		{
			base.sprite.usesOverrideMaterial = true;
			base.sprite.renderer.material.shader = ShaderCache.Acquire(PlayerController.DefaultShaderName);
			base.sprite.renderer.material.DisableKeyword("BRIGHTNESS_CLAMP_ON");
			base.sprite.renderer.material.EnableKeyword("BRIGHTNESS_CLAMP_OFF");
			base.sprite.renderer.material.SetColor("_FlatColor", new Color(0.2f, 0.25f, 0.67f, 1f));
			base.sprite.renderer.material.SetVector("_SpecialFlags", new Vector4(1f, 0f, 0f, 0f));
		}
	}

	// Token: 0x06006846 RID: 26694 RVA: 0x0028CC58 File Offset: 0x0028AE58
	private void Update()
	{
		if (this.IsTalking && GameManager.Instance.IsLoadingLevel)
		{
			EndConversation.ForceEndConversation(this);
		}
		this.m_collidedWithPlayerLastFrame = false;
		if (this.m_setInteractableTimer > 0f)
		{
			if (this.IsInteractable)
			{
				this.m_setInteractableTimer = -1f;
			}
			else
			{
				this.m_setInteractableTimer -= BraveTime.DeltaTime;
				if (this.m_setInteractableTimer <= 0f)
				{
					this.IsInteractable = true;
				}
			}
		}
		if (this.AllowWalkAways && this.m_talkingState == TalkDoerLite.TalkingState.Conversation && Vector2.Distance(this.TalkingPlayer.sprite.WorldCenter, base.sprite.WorldCenter) > this.conversationBreakRadius)
		{
			base.SendPlaymakerEvent("playerWalkedAway");
		}
		if (this.CompletedTalkingPlayer != null && !this.IsTalking && Vector2.Distance(this.CompletedTalkingPlayer.sprite.WorldCenter, base.sprite.WorldCenter) > this.conversationBreakRadius)
		{
			base.SendPlaymakerEvent("playerWalkedAwayPolitely");
			this.CompletedTalkingPlayer = null;
		}
		if (this.m_hasZombieTextBox && this.m_zombieBoxTimer > 0f)
		{
			this.m_zombieBoxTimer -= BraveTime.DeltaTime;
			if (this.m_zombieBoxTimer <= 0f)
			{
				this.CloseTextBox(true);
			}
		}
		if (this.IsPlayerInRange)
		{
			if (this.m_overheadUIElementDelay > 0f)
			{
				this.m_overheadUIElementDelay -= BraveTime.DeltaTime;
				if (this.m_overheadUIElementDelay <= 0f)
				{
					this.CreateOverheadUI();
				}
			}
		}
		else if (this.m_overheadUIElementDelay < this.OverheadUIElementDelay)
		{
			this.m_overheadUIElementDelay += BraveTime.DeltaTime;
		}
		if (GameManager.Instance.IsPaused && this.m_extantOverheadUIElement != null)
		{
			if (this.m_extantOverheadUIElement.IsVisible)
			{
				this.m_extantOverheadUIElement.IsVisible = false;
				tk2dBaseSprite[] componentsInChildren = this.m_extantOverheadUIElement.GetComponentsInChildren<tk2dBaseSprite>();
				for (int i = 0; i < componentsInChildren.Length; i++)
				{
					componentsInChildren[i].renderer.enabled = false;
				}
			}
		}
		else if (this.m_extantOverheadUIElement != null && !this.m_extantOverheadUIElement.IsVisible)
		{
			this.m_extantOverheadUIElement.IsVisible = true;
			tk2dBaseSprite[] componentsInChildren2 = this.m_extantOverheadUIElement.GetComponentsInChildren<tk2dBaseSprite>();
			for (int j = 0; j < componentsInChildren2.Length; j++)
			{
				componentsInChildren2[j].renderer.enabled = true;
			}
		}
		if (GameManager.Instance && GameManager.Instance.PrimaryPlayer && !GameManager.Instance.PrimaryPlayer.IsStealthed)
		{
			bool flag = Vector2.Distance(base.specRigidbody.UnitCenter, GameManager.Instance.PrimaryPlayer.specRigidbody.UnitCenter) < this.playerApproachRadius;
			if (!this.m_playerInsideApproachDistance && flag)
			{
				base.SendPlaymakerEvent("playerApproached");
			}
			else if (this.m_playerInsideApproachDistance && !flag)
			{
				base.SendPlaymakerEvent("playerUnapproached");
			}
			this.m_playerInsideApproachDistance = flag;
		}
		if (GameManager.Instance && GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER && GameManager.Instance.SecondaryPlayer && !GameManager.Instance.SecondaryPlayer.IsStealthed)
		{
			bool flag2 = Vector2.Distance(base.specRigidbody.UnitCenter, GameManager.Instance.SecondaryPlayer.specRigidbody.UnitCenter) < this.playerApproachRadius;
			if (!this.m_coopPlayerInsideApproachDistance && flag2)
			{
				base.SendPlaymakerEvent("coopPlayerApproached");
			}
			else if (this.m_coopPlayerInsideApproachDistance && !flag2)
			{
				base.SendPlaymakerEvent("coopPlayerUnapproached");
			}
			this.m_coopPlayerInsideApproachDistance = flag2;
		}
		if (GameManager.Instance && GameManager.Instance.PrimaryPlayer && GameManager.Instance.PrimaryPlayer.CurrentRoom == this.m_parentRoom)
		{
			Vector2 vector = Vector2.zero;
			if (GameManager.Instance.PrimaryPlayer.CurrentGun == null || GameManager.Instance.PrimaryPlayer.inventory.ForceNoGun)
			{
				vector = GameManager.Instance.PrimaryPlayer.NonZeroLastCommandedDirection;
			}
			else
			{
				vector = GameManager.Instance.PrimaryPlayer.unadjustedAimPoint - GameManager.Instance.PrimaryPlayer.LockedApproximateSpriteCenter;
			}
			Vector2 vector2 = base.specRigidbody.UnitCenter - GameManager.Instance.PrimaryPlayer.LockedApproximateSpriteCenter.XY();
			float num = Vector2.Dot(vector.normalized, vector2.normalized);
			bool flag3 = num > -0.25f || this.ForcePlayerLookAt;
			if (!this.m_playerFacingNPC && flag3)
			{
				base.SendPlaymakerEvent("playerStartedFacing");
			}
			else if (this.m_playerFacingNPC && !flag3)
			{
				base.SendPlaymakerEvent("playerStoppedFacing");
			}
			this.m_playerFacingNPC = flag3;
		}
	}

	// Token: 0x06006847 RID: 26695 RVA: 0x0028D1B8 File Offset: 0x0028B3B8
	protected void LateUpdate()
	{
		if (!this.m_collidedWithPlayerLastFrame)
		{
			this.m_collidedWithPlayerTimer = 0f;
		}
		if (!this.SuppressClear && this.m_clearTextFrameCountdown > 0)
		{
			this.m_clearTextFrameCountdown--;
			if (this.m_clearTextFrameCountdown <= 0)
			{
				TextBoxManager.ClearTextBox(this.speakPoint);
			}
		}
	}

	// Token: 0x06006848 RID: 26696 RVA: 0x0028D218 File Offset: 0x0028B418
	private void OnDisable()
	{
		if (this && this.speakPoint)
		{
			TextBoxManager.ClearTextBoxImmediate(this.speakPoint);
		}
		if (this.m_extantOverheadUIElement != null)
		{
			UnityEngine.Object.Destroy(this.m_extantOverheadUIElement.gameObject);
			this.m_extantOverheadUIElement = null;
		}
	}

	// Token: 0x06006849 RID: 26697 RVA: 0x0028D274 File Offset: 0x0028B474
	protected override void OnDestroy()
	{
		if (this.m_parentRoom != null)
		{
			this.m_parentRoom.Entered -= this.PlayerEnteredRoom;
			this.m_parentRoom.Exited -= this.PlayerExitedRoom;
		}
		StaticReferenceManager.AllNpcs.Remove(this);
		base.OnDestroy();
	}

	// Token: 0x0600684A RID: 26698 RVA: 0x0028D2CC File Offset: 0x0028B4CC
	public float GetDistanceToPoint(Vector2 point)
	{
		if (!this || !this.IsInteractable)
		{
			return 1000f;
		}
		if (this.ForceNonInteractable)
		{
			return 1000f;
		}
		if (this.PreventInteraction)
		{
			return 1000f;
		}
		if (!base.gameObject.activeSelf)
		{
			return 1000f;
		}
		if (base.aiActor && GameManager.Instance.BestActivePlayer.IsInCombat)
		{
			return 1000f;
		}
		if (this.usesOverrideInteractionRegion)
		{
			return BraveMathCollege.DistToRectangle(point, base.transform.position.XY() + this.overrideRegionOffset * 0.0625f, this.overrideRegionDimensions * 0.0625f);
		}
		float num;
		if (base.specRigidbody)
		{
			PixelCollider primaryPixelCollider = base.specRigidbody.PrimaryPixelCollider;
			num = BraveMathCollege.DistToRectangle(point, primaryPixelCollider.UnitBottomLeft, primaryPixelCollider.UnitDimensions);
		}
		else
		{
			Bounds bounds = base.sprite.GetBounds();
			bounds.center += base.sprite.transform.position;
			num = BraveMathCollege.DistToRectangle(point, bounds.min, bounds.size);
		}
		return num;
	}

	// Token: 0x0600684B RID: 26699 RVA: 0x0028D428 File Offset: 0x0028B628
	public float GetOverrideMaxDistance()
	{
		return this.overrideInteractionRadius;
	}

	// Token: 0x0600684C RID: 26700 RVA: 0x0028D430 File Offset: 0x0028B630
	private void CreateOverheadUI()
	{
		if (this.IsTalking)
		{
			return;
		}
		if (this.OverheadUIElementOnPreInteract != null && this.m_extantOverheadUIElement == null)
		{
			this.m_extantOverheadUIElement = GameUIRoot.Instance.Manager.AddPrefab(this.OverheadUIElementOnPreInteract);
			FoyerInfoPanelController component = this.m_extantOverheadUIElement.GetComponent<FoyerInfoPanelController>();
			if (component)
			{
				component.followTransform = base.transform;
				if (component.characterIdentity != PlayableCharacters.CoopCultist)
				{
					component.offset = new Vector3(0.75f, 1.625f, 0f);
				}
				else
				{
					component.offset = new Vector3(0.75f, 2.25f, 0f);
				}
			}
		}
	}

	// Token: 0x0600684D RID: 26701 RVA: 0x0028D4F0 File Offset: 0x0028B6F0
	private void DestroyOverheadUI()
	{
		if (this.OverheadUIElementOnPreInteract != null && this.m_extantOverheadUIElement != null)
		{
			UnityEngine.Object.Destroy(this.m_extantOverheadUIElement.gameObject);
			this.m_extantOverheadUIElement = null;
		}
	}

	// Token: 0x0600684E RID: 26702 RVA: 0x0028D52C File Offset: 0x0028B72C
	public void OnEnteredRange(PlayerController interactor)
	{
		this.IsPlayerInRange = true;
		this.UpdateOutlines();
	}

	// Token: 0x0600684F RID: 26703 RVA: 0x0028D53C File Offset: 0x0028B73C
	public void OnExitRange(PlayerController interactor)
	{
		this.DestroyOverheadUI();
		this.IsPlayerInRange = false;
		this.UpdateOutlines();
	}

	// Token: 0x06006850 RID: 26704 RVA: 0x0028D554 File Offset: 0x0028B754
	public void Interact(PlayerController interactor)
	{
		if (!interactor.IsPrimaryPlayer && this.PreventCoopInteraction)
		{
			return;
		}
		if (!this.IsInteractable)
		{
			return;
		}
		if (this.m_talkingState == TalkDoerLite.TalkingState.Conversation)
		{
			return;
		}
		if (GameManager.Instance.IsFoyer && interactor.WasTalkingThisFrame)
		{
			return;
		}
		if (GameManager.Instance.IsFoyer)
		{
			FoyerCharacterSelectFlag component = base.GetComponent<FoyerCharacterSelectFlag>();
			if (component && !component.CanBeSelected())
			{
				AkSoundEngine.PostEvent("Play_UI_menu_cancel_01", base.gameObject);
				return;
			}
		}
		if (GameManager.Instance.CurrentLevelOverrideState == GameManager.LevelOverrideState.FOYER)
		{
			GameManager.Instance.LastUsedInputDeviceForConversation = BraveInput.GetInstanceForPlayer(interactor.PlayerIDX).ActiveActions.Device;
		}
		if (this.m_extantOverheadUIElement != null)
		{
			UnityEngine.Object.Destroy(this.m_extantOverheadUIElement.gameObject);
			this.m_extantOverheadUIElement = null;
		}
		this.TalkingPlayer = interactor;
		EncounterTrackable component2 = base.GetComponent<EncounterTrackable>();
		if (this.m_numTimesSpokenTo == 0 && component2 != null)
		{
			GameStatsManager.Instance.HandleEncounteredObject(component2);
		}
		this.m_numTimesSpokenTo++;
		base.SendPlaymakerEvent("playerInteract");
	}

	// Token: 0x06006851 RID: 26705 RVA: 0x0028D68C File Offset: 0x0028B88C
	public string GetAnimationState(PlayerController interactor, out bool shouldBeFlipped)
	{
		shouldBeFlipped = false;
		return string.Empty;
	}

	// Token: 0x06006852 RID: 26706 RVA: 0x0028D698 File Offset: 0x0028B898
	private void OnRigidbodyCollision(CollisionData rigidbodyCollision)
	{
		if (this.m_talkingState == TalkDoerLite.TalkingState.Conversation)
		{
			return;
		}
		if (this.IsTalking)
		{
			return;
		}
		SpeculativeRigidbody otherRigidbody = rigidbodyCollision.OtherRigidbody;
		if (otherRigidbody.projectile && otherRigidbody.projectile.Owner is PlayerController)
		{
			base.SendPlaymakerEvent("takePlayerDamage");
		}
	}

	// Token: 0x06006853 RID: 26707 RVA: 0x0028D6F8 File Offset: 0x0028B8F8
	private void PlayerEnteredRoom(PlayerController p)
	{
		if (p.IsStealthed)
		{
			return;
		}
		if (this.SuppressRoomEnterExitEvents)
		{
			return;
		}
		base.SendPlaymakerEvent("playerEnteredRoom");
	}

	// Token: 0x06006854 RID: 26708 RVA: 0x0028D720 File Offset: 0x0028B920
	private void PlayerExitedRoom()
	{
		if (GameManager.Instance.PrimaryPlayer && GameManager.Instance.PrimaryPlayer.IsStealthed)
		{
			return;
		}
		if (this.SuppressRoomEnterExitEvents)
		{
			return;
		}
		base.SendPlaymakerEvent("playerExitedRoom");
	}

	// Token: 0x06006855 RID: 26709 RVA: 0x0028D770 File Offset: 0x0028B970
	protected void HandleAnimationEvent(tk2dSpriteAnimator animator, tk2dSpriteAnimationClip clip, int frameNo)
	{
		tk2dSpriteAnimationFrame frame = clip.GetFrame(frameNo);
		if (frame.eventOutline != tk2dSpriteAnimationFrame.OutlineModifier.Unspecified)
		{
			if (frame.eventOutline == tk2dSpriteAnimationFrame.OutlineModifier.TurnOn)
			{
				this.ShowOutlines = true;
			}
			else if (frame.eventOutline == tk2dSpriteAnimationFrame.OutlineModifier.TurnOff)
			{
				this.ShowOutlines = false;
			}
		}
	}

	// Token: 0x06006856 RID: 26710 RVA: 0x0028D7C0 File Offset: 0x0028B9C0
	public void SetZombieBoxTimer(float timer, string talkAnim)
	{
		this.m_hasZombieTextBox = true;
		this.m_zombieBoxTimer = timer;
		this.m_zombieBoxTalkAnim = talkAnim;
	}

	// Token: 0x06006857 RID: 26711 RVA: 0x0028D7D8 File Offset: 0x0028B9D8
	public void ShowText(Vector3 worldPosition, Transform parent, float duration, string text, bool instant = true, TextBoxManager.BoxSlideOrientation slideOrientation = TextBoxManager.BoxSlideOrientation.NO_ADJUSTMENT, bool showContinueText = false, bool isThoughtBox = false, string overrideSpeechAudioTag = null)
	{
		this.m_hasZombieTextBox = false;
		this.m_zombieBoxTimer = 0f;
		this.m_clearTextFrameCountdown = -1;
		if (isThoughtBox)
		{
			string text2 = ((overrideSpeechAudioTag == null) ? (this.audioCharacterSpeechTag ?? string.Empty) : overrideSpeechAudioTag);
			TextBoxManager.ShowThoughtBubble(worldPosition, parent, duration, text, instant, showContinueText, text2);
		}
		else
		{
			string text3 = ((overrideSpeechAudioTag == null) ? (this.audioCharacterSpeechTag ?? string.Empty) : overrideSpeechAudioTag);
			TextBoxManager.ShowTextBox(worldPosition, parent, duration, text, text3, instant, slideOrientation, showContinueText, this.SpeaksGleepGlorpenese);
		}
	}

	// Token: 0x06006858 RID: 26712 RVA: 0x0028D874 File Offset: 0x0028BA74
	public void CloseTextBox(bool overrideZombieBoxes)
	{
		if (overrideZombieBoxes)
		{
			this.m_hasZombieTextBox = false;
			this.m_zombieBoxTimer = 0f;
			if (base.aiAnimator)
			{
				base.aiAnimator.EndAnimationIf(this.m_zombieBoxTalkAnim);
			}
		}
		if (!this.m_hasZombieTextBox)
		{
			this.m_clearTextFrameCountdown = 2;
		}
	}

	// Token: 0x06006859 RID: 26713 RVA: 0x0028D8D0 File Offset: 0x0028BAD0
	private void UpdateOutlines()
	{
		bool flag = this.IsInteractable && this.State != TalkDoerLite.TalkingState.Conversation && this.IsPlayerInRange && !this.HasPlayerLocked;
		if (flag != this.m_isHighlighted || this.m_currentlyHasOutlines != this.ShowOutlines)
		{
			this.m_isHighlighted = flag;
			SpriteOutlineManager.RemoveOutlineFromSprite(base.sprite, false);
			if (this.ShowOutlines)
			{
				SpriteOutlineManager.AddOutlineToSprite(base.sprite, (!this.m_isHighlighted) ? Color.black : Color.white, this.OutlineDepth, (!this.m_isHighlighted) ? this.OutlineLuminanceCutoff : 0f, SpriteOutlineManager.OutlineType.NORMAL);
			}
			base.sprite.UpdateZDepth();
			this.m_currentlyHasOutlines = this.ShowOutlines;
		}
	}

	// Token: 0x17000F7D RID: 3965
	// (get) Token: 0x0600685A RID: 26714 RVA: 0x0028D9A8 File Offset: 0x0028BBA8
	// (set) Token: 0x0600685B RID: 26715 RVA: 0x0028D9B0 File Offset: 0x0028BBB0
	public Path CurrentPath
	{
		get
		{
			return this.m_currentPath;
		}
		set
		{
			this.m_currentPath = value;
		}
	}

	// Token: 0x17000F7E RID: 3966
	// (get) Token: 0x0600685C RID: 26716 RVA: 0x0028D9BC File Offset: 0x0028BBBC
	public IntVector2 PathTile
	{
		get
		{
			return base.specRigidbody.UnitBottomLeft.ToIntVector2(VectorConversions.Floor);
		}
	}

	// Token: 0x17000F7F RID: 3967
	// (get) Token: 0x0600685D RID: 26717 RVA: 0x0028D9D0 File Offset: 0x0028BBD0
	public IntVector2 Clearance
	{
		get
		{
			IntVector2? clearance = this.m_clearance;
			if (clearance == null)
			{
				this.m_clearance = new IntVector2?(base.specRigidbody.UnitDimensions.ToIntVector2(VectorConversions.Ceil));
			}
			return this.m_clearance.Value;
		}
	}

	// Token: 0x0600685E RID: 26718 RVA: 0x0028DA1C File Offset: 0x0028BC1C
	public Vector2 GetPathVelocityContribution(Vector2 lastPosition, int distanceThresholdPixels = 1)
	{
		if (this.m_currentPath == null || this.m_currentPath.Count == 0)
		{
			Vector2? overridePathEnd = this.m_overridePathEnd;
			if (overridePathEnd == null)
			{
				return Vector2.zero;
			}
		}
		Vector2 unitCenter = base.specRigidbody.UnitCenter;
		Vector2 vector;
		if (this.m_currentPath != null)
		{
			vector = this.m_currentPath.GetFirstCenterVector2();
		}
		else
		{
			vector = this.m_overridePathEnd.Value;
		}
		int num = ((this.m_currentPath != null) ? this.m_currentPath.Count : 0);
		Vector2? overridePathEnd2 = this.m_overridePathEnd;
		bool flag = num + ((overridePathEnd2 != null) ? 1 : 0) == 1;
		bool flag2 = false;
		int num2 = ((!flag) ? 1 : distanceThresholdPixels);
		if (Vector2.Distance(unitCenter, vector) < PhysicsEngine.PixelToUnit(num2))
		{
			flag2 = true;
		}
		else if (!flag)
		{
			Vector2 vector2 = BraveMathCollege.ClosestPointOnLineSegment(vector, lastPosition, unitCenter);
			if (Vector2.Distance(vector, vector2) < PhysicsEngine.PixelToUnit(num2))
			{
				flag2 = true;
			}
		}
		if (flag2)
		{
			if (this.m_currentPath != null && this.m_currentPath.Count > 0)
			{
				this.m_currentPath.RemoveFirst();
				if (this.m_currentPath.Count == 0)
				{
					this.m_currentPath = null;
					return Vector2.zero;
				}
			}
			else
			{
				Vector2? overridePathEnd3 = this.m_overridePathEnd;
				if (overridePathEnd3 != null)
				{
					this.m_overridePathEnd = null;
				}
			}
		}
		Vector2 vector3 = vector - unitCenter;
		if (flag && this.MovementSpeed > vector3.magnitude)
		{
			return vector3;
		}
		return this.MovementSpeed * vector3.normalized;
	}

	// Token: 0x0600685F RID: 26719 RVA: 0x0028DBD4 File Offset: 0x0028BDD4
	public void PathfindToPosition(Vector2 targetPosition, Vector2? overridePathEnd = null, CellValidator cellValidator = null)
	{
		Path path = null;
		if (Pathfinder.Instance.GetPath(this.PathTile, targetPosition.ToIntVector2(VectorConversions.Floor), out path, new IntVector2?(this.Clearance), this.PathableTiles, cellValidator, null, false))
		{
			this.m_currentPath = path;
			this.m_overridePathEnd = overridePathEnd;
			if (this.m_currentPath.Count == 0)
			{
				this.m_currentPath = null;
			}
			else
			{
				path.Smooth(base.specRigidbody.UnitCenter, base.specRigidbody.UnitDimensions / 2f, this.PathableTiles, false, this.Clearance);
			}
		}
	}

	// Token: 0x06006860 RID: 26720 RVA: 0x0028DC74 File Offset: 0x0028BE74
	public void ForceTimedSpeech(string words, float initialDelay, float duration, TextBoxManager.BoxSlideOrientation slideOrientation)
	{
		Debug.Log("starting forced timed speech: " + words);
		base.StartCoroutine(this.HandleForcedTimedSpeech(words, initialDelay, duration, slideOrientation));
	}

	// Token: 0x06006861 RID: 26721 RVA: 0x0028DC98 File Offset: 0x0028BE98
	private IEnumerator HandleForcedTimedSpeech(string words, float initialDelay, float duration, TextBoxManager.BoxSlideOrientation slideOrientation)
	{
		this.IsDoingForcedSpeech = true;
		while (initialDelay > 0f)
		{
			initialDelay -= BraveTime.DeltaTime;
			if (!this.IsDoingForcedSpeech)
			{
				Debug.Log("breaking forced timed speech: " + words);
				yield break;
			}
			yield return null;
		}
		TextBoxManager.ClearTextBox(this.speakPoint);
		base.aiAnimator.PlayUntilCancelled("talk", false, null, -1f, false);
		if (string.IsNullOrEmpty(this.audioCharacterSpeechTag))
		{
			TextBoxManager.ShowTextBox(this.speakPoint.position + new Vector3(0f, 0f, -4f), this.speakPoint, -1f, words, string.Empty, true, slideOrientation, false, this.SpeaksGleepGlorpenese);
		}
		else
		{
			TextBoxManager.ShowTextBox(this.speakPoint.position + new Vector3(0f, 0f, -4f), this.speakPoint, -1f, words, this.audioCharacterSpeechTag, false, slideOrientation, false, this.SpeaksGleepGlorpenese);
		}
		if (duration > 0f)
		{
			while (duration > 0f && this.IsDoingForcedSpeech)
			{
				duration -= BraveTime.DeltaTime;
				yield return null;
			}
		}
		else
		{
			while (this.IsDoingForcedSpeech)
			{
				yield return null;
			}
		}
		Debug.Log("ending forced timed speech: " + words);
		TextBoxManager.ClearTextBox(this.speakPoint);
		base.aiAnimator.EndAnimation();
		this.IsDoingForcedSpeech = false;
		yield break;
	}

	// Token: 0x04006447 RID: 25671
	private const float c_reinteractDelay = 0.5f;

	// Token: 0x04006448 RID: 25672
	[Header("Interactable Region")]
	public bool usesOverrideInteractionRegion;

	// Token: 0x04006449 RID: 25673
	[ShowInInspectorIf("usesOverrideInteractionRegion", false)]
	public Vector2 overrideRegionOffset = Vector2.zero;

	// Token: 0x0400644A RID: 25674
	[ShowInInspectorIf("usesOverrideInteractionRegion", false)]
	public Vector2 overrideRegionDimensions = Vector2.zero;

	// Token: 0x0400644B RID: 25675
	public float overrideInteractionRadius = -1f;

	// Token: 0x0400644C RID: 25676
	public bool PreventInteraction;

	// Token: 0x0400644D RID: 25677
	public bool AllowPlayerToPassEventually = true;

	// Token: 0x0400644E RID: 25678
	[Header("Speech Options")]
	public Transform speakPoint;

	// Token: 0x0400644F RID: 25679
	public bool SpeaksGleepGlorpenese;

	// Token: 0x04006450 RID: 25680
	public string audioCharacterSpeechTag = string.Empty;

	// Token: 0x04006451 RID: 25681
	public float playerApproachRadius = 5f;

	// Token: 0x04006452 RID: 25682
	public float conversationBreakRadius = 5f;

	// Token: 0x04006453 RID: 25683
	public TalkDoerLite echo1;

	// Token: 0x04006454 RID: 25684
	public TalkDoerLite echo2;

	// Token: 0x04006455 RID: 25685
	[Header("Other Options")]
	public bool PreventCoopInteraction;

	// Token: 0x04006456 RID: 25686
	public bool IsPaletteSwapped;

	// Token: 0x04006457 RID: 25687
	public Texture2D PaletteTexture;

	// Token: 0x04006458 RID: 25688
	public TalkDoerLite.TeleportSettings teleportInSettings;

	// Token: 0x04006459 RID: 25689
	public TalkDoerLite.TeleportSettings teleportOutSettings;

	// Token: 0x0400645A RID: 25690
	public List<GameObject> itemsToLeaveBehind;

	// Token: 0x0400645B RID: 25691
	public GameObject shadow;

	// Token: 0x0400645C RID: 25692
	public bool DisableOnShortcutRun;

	// Token: 0x0400645D RID: 25693
	public GameObject OptionalMinimapIcon;

	// Token: 0x0400645E RID: 25694
	public float OverheadUIElementDelay = 1f;

	// Token: 0x0400645F RID: 25695
	private float m_overheadUIElementDelay;

	// Token: 0x04006460 RID: 25696
	public GameObject OverheadUIElementOnPreInteract;

	// Token: 0x04006461 RID: 25697
	[NonSerialized]
	private dfControl m_extantOverheadUIElement;

	// Token: 0x04006462 RID: 25698
	public tk2dSprite OptionalCustomNotificationSprite;

	// Token: 0x04006463 RID: 25699
	public float OutlineDepth = 0.4f;

	// Token: 0x04006464 RID: 25700
	public float OutlineLuminanceCutoff = 0.05f;

	// Token: 0x04006465 RID: 25701
	public List<GameObject> ReassignPrefabReferences;

	// Token: 0x04006466 RID: 25702
	[NonSerialized]
	public Action OnGenericFSMActionA;

	// Token: 0x04006467 RID: 25703
	[NonSerialized]
	public Action OnGenericFSMActionB;

	// Token: 0x04006468 RID: 25704
	[NonSerialized]
	public Action OnGenericFSMActionC;

	// Token: 0x04006469 RID: 25705
	[NonSerialized]
	public Action OnGenericFSMActionD;

	// Token: 0x0400646A RID: 25706
	[NonSerialized]
	public bool ForcePlayerLookAt;

	// Token: 0x0400646B RID: 25707
	[NonSerialized]
	public bool ForceNonInteractable;

	// Token: 0x0400646F RID: 25711
	private PlayerController m_talkingPlayer;

	// Token: 0x04006472 RID: 25714
	[NonSerialized]
	public Tribool ShopStockStatus = Tribool.Unready;

	// Token: 0x04006473 RID: 25715
	private TalkDoerLite.TalkingState m_talkingState;

	// Token: 0x04006474 RID: 25716
	private bool m_isPlayerInRange;

	// Token: 0x04006475 RID: 25717
	private bool m_isInteractable = true;

	// Token: 0x04006476 RID: 25718
	private bool m_showOutlines = true;

	// Token: 0x04006477 RID: 25719
	private bool m_allowWalkAways = true;

	// Token: 0x04006478 RID: 25720
	private bool m_hasPlayerLocked;

	// Token: 0x04006479 RID: 25721
	private float m_setInteractableTimer;

	// Token: 0x0400647A RID: 25722
	private bool m_isHighlighted;

	// Token: 0x0400647B RID: 25723
	private bool m_currentlyHasOutlines = true;

	// Token: 0x0400647C RID: 25724
	private bool m_playerFacingNPC;

	// Token: 0x0400647D RID: 25725
	private bool m_playerInsideApproachDistance;

	// Token: 0x0400647E RID: 25726
	private bool m_coopPlayerInsideApproachDistance;

	// Token: 0x0400647F RID: 25727
	private int m_numTimesSpokenTo;

	// Token: 0x04006480 RID: 25728
	private RoomHandler m_parentRoom;

	// Token: 0x04006481 RID: 25729
	private bool m_hasZombieTextBox;

	// Token: 0x04006482 RID: 25730
	private float m_zombieBoxTimer;

	// Token: 0x04006483 RID: 25731
	private string m_zombieBoxTalkAnim;

	// Token: 0x04006484 RID: 25732
	[NonSerialized]
	public bool SuppressClear;

	// Token: 0x04006485 RID: 25733
	private int m_clearTextFrameCountdown = -1;

	// Token: 0x04006486 RID: 25734
	private bool m_collidedWithPlayerLastFrame;

	// Token: 0x04006487 RID: 25735
	private float m_collidedWithPlayerTimer;

	// Token: 0x04006488 RID: 25736
	public float MovementSpeed = 3f;

	// Token: 0x04006489 RID: 25737
	[EnumFlags]
	public CellTypes PathableTiles = CellTypes.FLOOR;

	// Token: 0x0400648A RID: 25738
	[NonSerialized]
	public bool m_isReadyForRepath = true;

	// Token: 0x0400648B RID: 25739
	[NonSerialized]
	private Path m_currentPath;

	// Token: 0x0400648C RID: 25740
	[NonSerialized]
	private Vector2? m_overridePathEnd;

	// Token: 0x0400648D RID: 25741
	private IntVector2? m_clearance;

	// Token: 0x0400648E RID: 25742
	public bool IsDoingForcedSpeech;

	// Token: 0x02001230 RID: 4656
	[Serializable]
	public class TeleportSettings
	{
		// Token: 0x0400648F RID: 25743
		public string anim;

		// Token: 0x04006490 RID: 25744
		public float animDelay;

		// Token: 0x04006491 RID: 25745
		public GameObject vfx;

		// Token: 0x04006492 RID: 25746
		public float vfxDelay;

		// Token: 0x04006493 RID: 25747
		public Teleport.Timing timing;

		// Token: 0x04006494 RID: 25748
		public GameObject vfxAnchor;
	}

	// Token: 0x02001231 RID: 4657
	public enum TalkingState
	{
		// Token: 0x04006496 RID: 25750
		None,
		// Token: 0x04006497 RID: 25751
		Passive,
		// Token: 0x04006498 RID: 25752
		Conversation
	}
}
