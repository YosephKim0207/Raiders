using System;
using System.Collections;
using System.Collections.Generic;
using Dungeonator;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x02001031 RID: 4145
public class GenericIntroDoer : TimeInvariantMonoBehaviour, IPlaceConfigurable
{
	// Token: 0x17000D35 RID: 3381
	// (get) Token: 0x06005AE4 RID: 23268 RVA: 0x0022C6EC File Offset: 0x0022A8EC
	public Vector2 BossCenter
	{
		get
		{
			if (this.m_specificIntroDoer != null)
			{
				Vector2? overrideIntroPosition = this.m_specificIntroDoer.OverrideIntroPosition;
				if (overrideIntroPosition != null)
				{
					return overrideIntroPosition.Value;
				}
			}
			if (base.specRigidbody)
			{
				return base.specRigidbody.UnitCenter;
			}
			if (base.dungeonPlaceable)
			{
				return base.transform.position.XY() + new Vector2((float)base.dungeonPlaceable.placeableWidth / 2f, (float)base.dungeonPlaceable.placeableHeight / 2f);
			}
			return base.transform.position;
		}
	}

	// Token: 0x17000D36 RID: 3382
	// (get) Token: 0x06005AE5 RID: 23269 RVA: 0x0022C7A8 File Offset: 0x0022A9A8
	// (set) Token: 0x06005AE6 RID: 23270 RVA: 0x0022C7B0 File Offset: 0x0022A9B0
	public bool SkipFinalizeAnimation { get; set; }

	// Token: 0x17000D37 RID: 3383
	// (get) Token: 0x06005AE7 RID: 23271 RVA: 0x0022C7BC File Offset: 0x0022A9BC
	// (set) Token: 0x06005AE8 RID: 23272 RVA: 0x0022C7C4 File Offset: 0x0022A9C4
	public bool SuppressSkipping { get; set; }

	// Token: 0x06005AE9 RID: 23273 RVA: 0x0022C7D0 File Offset: 0x0022A9D0
	private void Awake()
	{
		this.m_specificIntroDoer = base.GetComponent<SpecificIntroDoer>();
	}

	// Token: 0x06005AEA RID: 23274 RVA: 0x0022C7E0 File Offset: 0x0022A9E0
	protected override void InvariantUpdate(float realDeltaTime)
	{
		if (!this.m_isRunning)
		{
			return;
		}
		if (!base.enabled)
		{
			return;
		}
		if (GenericIntroDoer.SkipFrame)
		{
			GenericIntroDoer.SkipFrame = false;
			return;
		}
		for (int i = 0; i < this.m_animators.Count; i++)
		{
			this.m_animators[i].UpdateAnimation(realDeltaTime);
		}
		for (int j = 0; j < this.activeMotions.Count; j++)
		{
			CutsceneMotion cutsceneMotion = this.activeMotions[j];
			Vector2? lerpEnd = cutsceneMotion.lerpEnd;
			Vector2 vector = ((lerpEnd == null) ? GameManager.Instance.MainCameraController.GetIdealCameraPosition() : cutsceneMotion.lerpEnd.Value);
			float num = Vector2.Distance(vector, cutsceneMotion.lerpStart);
			float num2 = cutsceneMotion.speed * realDeltaTime;
			float num3 = num2 / num;
			cutsceneMotion.lerpProgress = Mathf.Clamp01(cutsceneMotion.lerpProgress + num3);
			float num4 = cutsceneMotion.lerpProgress;
			if (cutsceneMotion.isSmoothStepped)
			{
				num4 = Mathf.SmoothStep(0f, 1f, num4);
			}
			Vector2 vector2 = Vector2.Lerp(cutsceneMotion.lerpStart, vector, num4);
			if (cutsceneMotion.camera != null)
			{
				cutsceneMotion.camera.OverridePosition = vector2.ToVector3ZUp(cutsceneMotion.zOffset);
			}
			else
			{
				cutsceneMotion.transform.position = BraveUtility.QuantizeVector(vector2.ToVector3ZUp(cutsceneMotion.zOffset), (float)PhysicsEngine.Instance.PixelsPerUnit);
			}
			if (cutsceneMotion.lerpProgress >= 1f)
			{
				this.activeMotions.RemoveAt(j);
				j--;
				this.m_currentPhase++;
				this.m_phaseComplete = true;
			}
		}
		bool flag = BraveInput.PrimaryPlayerInstance.MenuInteractPressed;
		if (GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER)
		{
			flag = flag || BraveInput.SecondaryPlayerInstance.MenuInteractPressed;
		}
		if (this.SuppressSkipping)
		{
			flag = false;
		}
		if (this.m_singleFrameSkipDelay != Tribool.Unready)
		{
			flag = false;
		}
		if (flag)
		{
			BraveMemory.HandleBossCardSkip();
			this.m_singleFrameSkipDelay = Tribool.Ready;
		}
		else if (this.m_singleFrameSkipDelay == Tribool.Ready)
		{
			this.m_singleFrameSkipDelay = Tribool.Complete;
			if (GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER && !this.m_hasCoopTeleported)
			{
				this.TeleportCoopPlayers();
			}
			AkSoundEngine.PostEvent("STOP_SND_Diagetic", base.gameObject);
			this.m_currentPhase = GenericIntroDoer.Phase.CameraOutro;
			this.bossUI.EndBossPortraitEarly();
			this.m_phaseComplete = true;
			this.activeMotions.Clear();
			SpeculativeRigidbody[] componentsInChildren = base.GetComponentsInChildren<SpeculativeRigidbody>();
			for (int k = 0; k < componentsInChildren.Length; k++)
			{
				componentsInChildren[k].CollideWithOthers = true;
			}
			AIActor[] componentsInChildren2 = base.GetComponentsInChildren<AIActor>();
			for (int l = 0; l < componentsInChildren2.Length; l++)
			{
				componentsInChildren2[l].IsGone = false;
			}
			if (base.aiActor)
			{
				base.aiActor.State = AIActor.ActorState.Normal;
			}
			if (this.InvisibleBeforeIntroAnim)
			{
				base.aiActor.ToggleRenderers(true);
			}
			if (!string.IsNullOrEmpty(this.preIntroDirectionalAnim))
			{
				((!this.specifyIntroAiAnimator) ? base.aiAnimator : this.specifyIntroAiAnimator).EndAnimationIf(this.preIntroDirectionalAnim);
			}
			if (this.m_specificIntroDoer != null)
			{
				this.m_specificIntroDoer.EndIntro();
			}
			tk2dSpriteAnimator[] componentsInChildren3 = base.GetComponentsInChildren<tk2dSpriteAnimator>();
			for (int m = 0; m < componentsInChildren3.Length; m++)
			{
				if (componentsInChildren3[m])
				{
					componentsInChildren3[m].alwaysUpdateOffscreen = true;
				}
			}
		}
		if (this.m_phaseComplete)
		{
			DirectionalAnimation directionalAnimation = null;
			if (base.aiAnimator)
			{
				if (base.aiAnimator.IdleAnimation.HasAnimation)
				{
					directionalAnimation = base.aiAnimator.IdleAnimation;
				}
				else if (base.aiAnimator.MoveAnimation.HasAnimation)
				{
					directionalAnimation = base.aiAnimator.MoveAnimation;
				}
			}
			switch (this.m_currentPhase)
			{
			case GenericIntroDoer.Phase.CameraIntro:
			{
				CutsceneMotion cutsceneMotion2 = new CutsceneMotion(this.m_cameraTransform, new Vector2?(this.BossCenter), this.cameraMoveSpeed, 0f);
				cutsceneMotion2.camera = this.m_camera;
				this.activeMotions.Add(cutsceneMotion2);
				this.m_phaseComplete = false;
				if (base.spriteAnimator)
				{
					this.m_animators.Add(base.spriteAnimator);
					base.spriteAnimator.enabled = false;
				}
				if (base.aiAnimator && base.aiAnimator.ChildAnimator)
				{
					this.m_animators.Add(base.aiAnimator.ChildAnimator.spriteAnimator);
					base.aiAnimator.ChildAnimator.spriteAnimator.enabled = false;
				}
				if (this.m_specificIntroDoer != null)
				{
					this.m_specificIntroDoer.OnCameraIntro();
				}
				break;
			}
			case GenericIntroDoer.Phase.InitialDelay:
				this.m_phaseCountdown = this.initialDelay;
				this.m_phaseComplete = false;
				if (GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER && !this.m_hasCoopTeleported)
				{
					this.TeleportCoopPlayers();
				}
				break;
			case GenericIntroDoer.Phase.PreIntroAnim:
				if (this.InvisibleBeforeIntroAnim)
				{
					base.aiActor.ToggleRenderers(true);
				}
				if (this.m_specificIntroDoer != null)
				{
					this.m_specificIntroDoer.StartIntro(this.m_animators);
					this.m_phaseCountdown = float.MaxValue;
					this.m_phaseComplete = false;
					this.m_waitingForSpecificIntroCompletion = !this.m_specificIntroDoer.IsIntroFinished;
					if (this.m_waitingForSpecificIntroCompletion)
					{
						break;
					}
				}
				if (!string.IsNullOrEmpty(this.introAnim))
				{
					base.spriteAnimator.Play(base.spriteAnimator.GetClipByName(this.introAnim));
					this.m_phaseCountdown = (float)base.spriteAnimator.CurrentClip.frames.Length / base.spriteAnimator.CurrentClip.fps;
					this.m_phaseCountdown += 0.25f;
					this.m_phaseComplete = false;
				}
				else if (!string.IsNullOrEmpty(this.introDirectionalAnim))
				{
					AIAnimator aianimator = ((!this.specifyIntroAiAnimator) ? base.aiAnimator : this.specifyIntroAiAnimator);
					aianimator.PlayUntilFinished(this.introDirectionalAnim, false, null, -1f, false);
					tk2dSpriteAnimator tk2dSpriteAnimator = aianimator.spriteAnimator;
					if (aianimator.ChildAnimator && aianimator.ChildAnimator.HasDirectionalAnimation(this.introDirectionalAnim))
					{
						tk2dSpriteAnimator = aianimator.ChildAnimator.spriteAnimator;
					}
					this.m_phaseCountdown = (float)tk2dSpriteAnimator.CurrentClip.frames.Length / tk2dSpriteAnimator.CurrentClip.fps;
					this.m_phaseCountdown += 0.25f;
					this.m_phaseComplete = false;
				}
				else
				{
					this.m_phaseCountdown = 0f;
					this.m_phaseComplete = false;
				}
				break;
			case GenericIntroDoer.Phase.BossCard:
				if (!this.SkipBossCard)
				{
					AkSoundEngine.PostEvent("Play_UI_boss_intro_01", base.gameObject);
					base.StartCoroutine(this.WaitForBossCard());
					this.m_phaseCountdown = float.MaxValue;
					this.m_phaseComplete = false;
					if (this.m_specificIntroDoer != null)
					{
						this.m_specificIntroDoer.OnBossCard();
					}
				}
				break;
			case GenericIntroDoer.Phase.CameraOutro:
			{
				if (this.cameraFocus || this.roomPositionCameraFocus != Vector2.zero || this.fusebombLock)
				{
					this.ModifyCamera(true);
				}
				if (this.restrictPlayerMotionToRoom)
				{
					this.RestrictMotion(true);
				}
				Vector2? vector3 = null;
				if (this.m_specificIntroDoer)
				{
					Vector2? overrideOutroPosition = this.m_specificIntroDoer.OverrideOutroPosition;
					if (overrideOutroPosition != null)
					{
						vector3 = new Vector2?(overrideOutroPosition.Value);
					}
				}
				GameManager.Instance.MainCameraController.ForceUpdateControllerCameraState(CameraController.ControllerCameraState.RoomLock);
				CutsceneMotion cutsceneMotion3 = new CutsceneMotion(this.m_cameraTransform, vector3, this.cameraMoveSpeed, 0f);
				cutsceneMotion3.camera = this.m_camera;
				this.activeMotions.Add(cutsceneMotion3);
				this.m_phaseComplete = false;
				if (!this.continueAnimDuringOutro)
				{
					if (this.AdditionalHeightOffset != 0f)
					{
						foreach (tk2dBaseSprite tk2dBaseSprite in base.GetComponentsInChildren<tk2dBaseSprite>())
						{
							tk2dBaseSprite.HeightOffGround -= this.AdditionalHeightOffset;
						}
						base.sprite.UpdateZDepth();
					}
					if (!string.IsNullOrEmpty(this.introDirectionalAnim))
					{
						AIAnimator aianimator2 = ((!this.specifyIntroAiAnimator) ? base.aiAnimator : this.specifyIntroAiAnimator);
						aianimator2.EndAnimationIf(this.introDirectionalAnim);
					}
					if (directionalAnimation != null && !this.SkipFinalizeAnimation)
					{
						base.spriteAnimator.Play(directionalAnimation.GetInfo(-90f, false).name);
					}
				}
				if (this.m_specificIntroDoer != null)
				{
					this.m_specificIntroDoer.OnCameraOutro();
				}
				break;
			}
			case GenericIntroDoer.Phase.Cleanup:
				if (this.continueAnimDuringOutro)
				{
					if (this.AdditionalHeightOffset != 0f)
					{
						foreach (tk2dBaseSprite tk2dBaseSprite2 in base.GetComponentsInChildren<tk2dBaseSprite>())
						{
							tk2dBaseSprite2.HeightOffGround -= this.AdditionalHeightOffset;
						}
						base.sprite.UpdateZDepth();
					}
					if (!string.IsNullOrEmpty(this.introDirectionalAnim))
					{
						AIAnimator aianimator3 = ((!this.specifyIntroAiAnimator) ? base.aiAnimator : this.specifyIntroAiAnimator);
						aianimator3.EndAnimationIf(this.introDirectionalAnim);
					}
					if (directionalAnimation != null && !this.SkipFinalizeAnimation)
					{
						base.spriteAnimator.Play(directionalAnimation.GetInfo(-90f, false).name);
					}
				}
				if (base.spriteAnimator)
				{
					this.m_animators.Remove(base.spriteAnimator);
					base.spriteAnimator.enabled = true;
				}
				if (base.aiAnimator && base.aiAnimator.ChildAnimator)
				{
					base.aiAnimator.ChildAnimator.spriteAnimator.enabled = true;
				}
				if (this.m_specificIntroDoer != null)
				{
					this.m_specificIntroDoer.OnCleanup();
				}
				this.EndSequence(false);
				return;
			}
		}
		if (this.m_currentPhase > GenericIntroDoer.Phase.Cleanup)
		{
			this.m_currentPhase = GenericIntroDoer.Phase.Cleanup;
		}
		if (this.m_currentPhase == GenericIntroDoer.Phase.PreIntroAnim)
		{
			if (this.m_waitingForSpecificIntroCompletion && this.m_specificIntroDoer.IsIntroFinished)
			{
				this.m_phaseCountdown = 0f;
				this.m_currentPhase++;
				this.m_phaseComplete = true;
			}
			if (!string.IsNullOrEmpty(this.preIntroDirectionalAnim))
			{
				((!this.specifyIntroAiAnimator) ? base.aiAnimator : this.specifyIntroAiAnimator).EndAnimationIf(this.preIntroDirectionalAnim);
			}
		}
		else if (this.m_currentPhase == GenericIntroDoer.Phase.BossCard && !this.m_waitingForBossCard)
		{
			this.m_phaseCountdown = 0f;
			this.m_currentPhase++;
			this.m_phaseComplete = true;
		}
		if (this.m_phaseCountdown > 0f)
		{
			this.m_phaseCountdown -= realDeltaTime;
			if (this.m_phaseCountdown <= 0f)
			{
				this.m_phaseCountdown = 0f;
				this.m_currentPhase++;
				this.m_phaseComplete = true;
			}
		}
	}

	// Token: 0x06005AEB RID: 23275 RVA: 0x0022D3C0 File Offset: 0x0022B5C0
	protected override void OnDestroy()
	{
		if (this.m_room != null)
		{
			this.m_room.Entered -= this.PlayerEntered;
		}
		if (this.m_isCameraModified)
		{
			this.ModifyCamera(false);
		}
		if (this.m_isMotionRestricted)
		{
			this.RestrictMotion(false);
		}
		base.OnDestroy();
	}

	// Token: 0x06005AEC RID: 23276 RVA: 0x0022D41C File Offset: 0x0022B61C
	public void ConfigureOnPlacement(RoomHandler room)
	{
		this.m_room = room;
		this.m_room.Entered += this.PlayerEntered;
	}

	// Token: 0x06005AED RID: 23277 RVA: 0x0022D43C File Offset: 0x0022B63C
	public void PlayerEntered(PlayerController player)
	{
		if (GameManager.HasInstance && GameManager.Instance.RunData.SpawnAngryToadie && base.healthHaver && !base.healthHaver.IsSubboss && GameManager.Instance.CurrentFloor < 5 && !base.name.StartsWith("BossStatues", StringComparison.Ordinal) && !base.name.StartsWith("DemonWall", StringComparison.Ordinal))
		{
			Vector2 vector = base.specRigidbody.UnitBottomRight + new Vector2(2.5f, 0.25f);
			AIActor.Spawn(EnemyDatabase.GetOrLoadByGuid(GlobalEnemyGuids.BulletKingToadieAngry), vector, base.aiActor.ParentRoom, true, AIActor.AwakenAnimationType.Default, true);
			GameManager.Instance.RunData.SpawnAngryToadie = false;
		}
		if (this.triggerType == GenericIntroDoer.TriggerType.PlayerEnteredRoom)
		{
			this.TriggerSequence(player);
		}
	}

	// Token: 0x06005AEE RID: 23278 RVA: 0x0022D528 File Offset: 0x0022B728
	public void TriggerSequence(PlayerController enterer)
	{
		base.StartCoroutine(this.FrameDelayedTriggerSequence(enterer));
	}

	// Token: 0x06005AEF RID: 23279 RVA: 0x0022D538 File Offset: 0x0022B738
	public IEnumerator FrameDelayedTriggerSequence(PlayerController enterer)
	{
		if (GameManager.Instance.PreventPausing)
		{
			yield break;
		}
		if (!base.enabled || (base.aiActor && !base.aiActor.enabled))
		{
			yield break;
		}
		this.m_room.Entered -= this.PlayerEntered;
		List<AIActor> enemiesInRoom = this.m_room.GetActiveEnemies(RoomHandler.ActiveEnemyType.All);
		for (int i = 0; i < enemiesInRoom.Count; i++)
		{
			if (enemiesInRoom[i].gameObject != base.gameObject)
			{
				GenericIntroDoer component = enemiesInRoom[i].gameObject.GetComponent<GenericIntroDoer>();
				if (component && component.m_isRunning)
				{
					yield break;
				}
			}
		}
		if (!this.PreventBossMusic)
		{
			string text = this.BossMusicEvent;
			if (this.m_specificIntroDoer && this.m_specificIntroDoer.OverrideBossMusicEvent != null)
			{
				text = this.m_specificIntroDoer.OverrideBossMusicEvent;
			}
			GameManager.Instance.DungeonMusicController.SwitchToBossMusic((!string.IsNullOrEmpty(text)) ? text : "Play_MUS_Boss_Theme_Beholster", base.gameObject);
		}
		Minimap.Instance.ToggleMinimap(false, false);
		BraveTime.RegisterTimeScaleMultiplier(0f, base.gameObject);
		GameManager.IsBossIntro = true;
		for (int j = 0; j < GameManager.Instance.AllPlayers.Length; j++)
		{
			if (GameManager.Instance.AllPlayers[j])
			{
				GameManager.Instance.AllPlayers[j].SetInputOverride("BossIntro");
			}
		}
		GameManager.Instance.PreventPausing = true;
		GameUIRoot.Instance.HideCoreUI(string.Empty);
		GameUIRoot.Instance.ToggleLowerPanels(false, false, string.Empty);
		if (base.healthHaver && base.healthHaver.UsesVerticalBossBar)
		{
			this.bossUI = GameUIRoot.Instance.bossControllerSide;
		}
		else if (base.healthHaver && base.healthHaver.UsesSecondaryBossBar)
		{
			this.bossUI = GameUIRoot.Instance.bossController2;
		}
		else
		{
			this.bossUI = GameUIRoot.Instance.bossController;
		}
		if (base.aiAnimator)
		{
			base.aiAnimator.enabled = false;
		}
		if (base.renderer)
		{
			base.renderer.enabled = true;
		}
		if (this.HideGunAndHand && base.aiShooter)
		{
			base.aiShooter.ToggleGunAndHandRenderers(false, "genericIntro");
		}
		StaticReferenceManager.DestroyAllProjectiles();
		this.HandlePlayerWalkIn(enterer);
		this.m_camera = GameManager.Instance.MainCameraController;
		this.m_camera.StopTrackingPlayer();
		this.m_camera.SetManualControl(true, false);
		this.m_camera.OverridePosition = this.m_camera.transform.position;
		this.m_cameraTransform = this.m_camera.transform;
		if (this.AdditionalHeightOffset != 0f)
		{
			foreach (tk2dBaseSprite tk2dBaseSprite in base.GetComponentsInChildren<tk2dBaseSprite>())
			{
				tk2dBaseSprite.HeightOffGround += this.AdditionalHeightOffset;
			}
			base.sprite.UpdateZDepth();
		}
		if (this.InvisibleBeforeIntroAnim)
		{
			base.aiActor.ToggleRenderers(false);
		}
		if (!string.IsNullOrEmpty(this.preIntroAnim))
		{
			base.spriteAnimator.Play(this.preIntroAnim);
		}
		if (!string.IsNullOrEmpty(this.preIntroDirectionalAnim))
		{
			((!this.specifyIntroAiAnimator) ? base.aiAnimator : this.specifyIntroAiAnimator).PlayUntilFinished(this.preIntroDirectionalAnim, false, null, -1f, false);
		}
		if (this.m_specificIntroDoer)
		{
			this.m_specificIntroDoer.PlayerWalkedIn(enterer, this.m_animators);
		}
		yield return null;
		yield return null;
		Minimap.Instance.TemporarilyPreventMinimap = true;
		this.m_isRunning = true;
		yield break;
	}

	// Token: 0x06005AF0 RID: 23280 RVA: 0x0022D55C File Offset: 0x0022B75C
	private void HandlePlayerWalkIn(PlayerController leadPlayer)
	{
		if (this.m_hasTriggeredWalkIn)
		{
			return;
		}
		this.m_hasTriggeredWalkIn = true;
		this.m_hasCoopTeleported = false;
		RoomHandler roomHandler = null;
		for (int i = 0; i < this.m_room.connectedRooms.Count; i++)
		{
			if (this.m_room.connectedRooms[i].distanceFromEntrance <= this.m_room.distanceFromEntrance)
			{
				roomHandler = this.m_room.connectedRooms[i];
				break;
			}
		}
		if (roomHandler != null)
		{
			float num = float.MaxValue;
			RuntimeExitDefinition runtimeExitDefinition = null;
			for (int j = 0; j < this.m_room.area.instanceUsedExits.Count; j++)
			{
				PrototypeRoomExit prototypeRoomExit = this.m_room.area.instanceUsedExits[j];
				if (this.m_room.area.exitToLocalDataMap.ContainsKey(prototypeRoomExit))
				{
					RuntimeRoomExitData runtimeRoomExitData = this.m_room.area.exitToLocalDataMap[prototypeRoomExit];
					if (this.m_room.exitDefinitionsByExit.ContainsKey(runtimeRoomExitData))
					{
						RuntimeExitDefinition runtimeExitDefinition2 = this.m_room.exitDefinitionsByExit[runtimeRoomExitData];
						IntVector2 intVector = ((runtimeExitDefinition2.upstreamRoom != this.m_room) ? runtimeExitDefinition2.GetDownstreamNearDoorPosition() : runtimeExitDefinition2.GetUpstreamNearDoorPosition());
						float num2 = Vector2.Distance(leadPlayer.CenterPosition, intVector.ToCenterVector2());
						if (num2 < num)
						{
							num = num2;
							runtimeExitDefinition = runtimeExitDefinition2;
						}
					}
				}
			}
			if (runtimeExitDefinition == null || num > 10f)
			{
				runtimeExitDefinition = this.m_room.GetExitDefinitionForConnectedRoom(roomHandler);
			}
			DungeonData.Direction direction = DungeonData.InvertDirection(runtimeExitDefinition.GetDirectionFromRoom(this.m_room));
			IntVector2 intVector2 = ((runtimeExitDefinition.upstreamRoom != this.m_room) ? runtimeExitDefinition.GetDownstreamNearDoorPosition() : runtimeExitDefinition.GetUpstreamNearDoorPosition());
			if (this.m_specificIntroDoer)
			{
				intVector2 = this.m_specificIntroDoer.OverrideExitBasePosition(direction, intVector2);
			}
			float num3 = (float)((direction != DungeonData.Direction.NORTH && direction != DungeonData.Direction.SOUTH) ? intVector2.x : intVector2.y);
			if (direction == DungeonData.Direction.EAST || direction == DungeonData.Direction.NORTH)
			{
				num3 += 3f;
			}
			else
			{
				num3 -= 3f;
			}
			Debug.LogError(direction + "|" + num3);
			leadPlayer.ForceWalkInDirectionWhilePaused(direction, num3);
			if (GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER)
			{
				PlayerController otherPlayer = GameManager.Instance.GetOtherPlayer(leadPlayer);
				float num4;
				if (direction == DungeonData.Direction.NORTH || direction == DungeonData.Direction.SOUTH)
				{
					num4 = Mathf.Abs(num3 - leadPlayer.CenterPosition.y);
				}
				else
				{
					num4 = Mathf.Abs(num3 - leadPlayer.CenterPosition.x);
				}
				IntVector2 zero = IntVector2.Zero;
				int num5 = Mathf.RoundToInt(num4 * 16f);
				if (direction == DungeonData.Direction.NORTH)
				{
					zero = new IntVector2(0, num5);
				}
				else if (direction == DungeonData.Direction.EAST)
				{
					zero = new IntVector2(num5, 0);
				}
				else if (direction == DungeonData.Direction.SOUTH)
				{
					zero = new IntVector2(0, -num5);
				}
				else if (direction == DungeonData.Direction.WEST)
				{
					zero = new IntVector2(-num5, 0);
				}
				CollisionData collisionData;
				if (PhysicsEngine.Instance.RigidbodyCast(otherPlayer.specRigidbody, zero, out collisionData, true, true, null, false))
				{
					num4 = PhysicsEngine.PixelToUnit(collisionData.NewPixelsToMove).magnitude;
				}
				CollisionData.Pool.Free(ref collisionData);
				if (direction == DungeonData.Direction.NORTH)
				{
					num3 = otherPlayer.CenterPosition.y + num4;
				}
				else if (direction == DungeonData.Direction.EAST)
				{
					num3 = otherPlayer.CenterPosition.x + num4;
				}
				else if (direction == DungeonData.Direction.SOUTH)
				{
					num3 = otherPlayer.CenterPosition.y - num4;
				}
				else if (direction == DungeonData.Direction.WEST)
				{
					num3 = otherPlayer.CenterPosition.x - num4;
				}
				otherPlayer.ForceWalkInDirectionWhilePaused(direction, num3);
				this.m_idealStartingPositions = new Vector2[2];
				IntVector2 intVector3 = ((direction != DungeonData.Direction.NORTH && direction != DungeonData.Direction.SOUTH) ? (intVector2 + IntVector2.Up) : (intVector2 + IntVector2.Right));
				float num6 = 3f;
				switch (direction)
				{
				case DungeonData.Direction.NORTH:
					this.m_idealStartingPositions[0] = intVector3.ToVector2() + new Vector2(-0.5f, 0f) + new Vector2(0f, num6 + 0.5f);
					this.m_idealStartingPositions[1] = intVector3.ToVector2() + new Vector2(0.25f, -0.25f) + new Vector2(0f, num6 - 0.25f);
					break;
				case DungeonData.Direction.EAST:
					this.m_idealStartingPositions[0] = intVector3.ToVector2() + new Vector2(num6 + 0.5f, 0f);
					this.m_idealStartingPositions[1] = intVector3.ToVector2() + new Vector2(-0.25f, -1f) + new Vector2(num6 - 0.25f, 0f);
					break;
				case DungeonData.Direction.SOUTH:
					this.m_idealStartingPositions[0] = intVector3.ToVector2() + new Vector2(-0.5f, 0f) - new Vector2(0f, num6 + 0.5f);
					this.m_idealStartingPositions[1] = intVector3.ToVector2() + new Vector2(0.25f, 0.25f) - new Vector2(0f, num6 - 0.25f);
					break;
				case DungeonData.Direction.WEST:
					this.m_idealStartingPositions[0] = intVector3.ToVector2() - new Vector2(num6 + 0.5f, 0f);
					this.m_idealStartingPositions[1] = intVector3.ToVector2() + new Vector2(0.25f, -1f) - new Vector2(num6 - 0.25f, 0f);
					break;
				}
			}
		}
	}

	// Token: 0x06005AF1 RID: 23281 RVA: 0x0022DBE4 File Offset: 0x0022BDE4
	private void EndSequence(bool isChildSequence = false)
	{
		if (!isChildSequence)
		{
			List<AIActor> activeEnemies = this.m_room.GetActiveEnemies(RoomHandler.ActiveEnemyType.All);
			for (int i = 0; i < activeEnemies.Count; i++)
			{
				if (activeEnemies[i].gameObject != base.gameObject)
				{
					GenericIntroDoer component = activeEnemies[i].gameObject.GetComponent<GenericIntroDoer>();
					if (component)
					{
						component.EndSequence(true);
					}
				}
			}
			this.bossUI.EndBossPortraitEarly();
			this.m_camera.StartTrackingPlayer();
			this.m_camera.SetManualControl(false, true);
		}
		if (this.HideGunAndHand && base.aiShooter)
		{
			base.aiShooter.ToggleGunAndHandRenderers(true, "genericIntro");
		}
		if (base.aiAnimator)
		{
			base.aiAnimator.enabled = true;
		}
		if (base.renderer)
		{
			base.renderer.enabled = true;
		}
		if (base.spriteAnimator)
		{
			base.spriteAnimator.enabled = true;
		}
		SpeculativeRigidbody[] componentsInChildren = base.GetComponentsInChildren<SpeculativeRigidbody>();
		for (int j = 0; j < componentsInChildren.Length; j++)
		{
			componentsInChildren[j].CollideWithOthers = true;
		}
		AIActor[] componentsInChildren2 = base.GetComponentsInChildren<AIActor>();
		for (int k = 0; k < componentsInChildren2.Length; k++)
		{
			componentsInChildren2[k].IsGone = false;
		}
		if (this.m_specificIntroDoer != null)
		{
			this.m_specificIntroDoer.EndIntro();
		}
		if (base.aiActor)
		{
			base.aiActor.State = AIActor.ActorState.Normal;
		}
		if (this.InvisibleBeforeIntroAnim)
		{
			base.aiActor.ToggleRenderers(true);
		}
		if (this.m_room != null)
		{
			Minimap.Instance.RevealMinimapRoom(this.m_room, true, true, true);
		}
		for (int l = 0; l < GameManager.Instance.AllPlayers.Length; l++)
		{
			if (!GameManager.Instance.AllPlayers[l].healthHaver.IsDead)
			{
				GameManager.Instance.AllPlayers[l].ToggleGunRenderers(true, string.Empty);
			}
		}
		GameManager.Instance.PreventPausing = false;
		for (int m = 0; m < GameManager.Instance.AllPlayers.Length; m++)
		{
			if (GameManager.Instance.AllPlayers[m])
			{
				GameManager.Instance.AllPlayers[m].ClearInputOverride("BossIntro");
			}
		}
		GameUIRoot.Instance.ToggleLowerPanels(true, false, string.Empty);
		GameUIRoot.Instance.ShowCoreUI(string.Empty);
		tk2dSpriteAnimator[] componentsInChildren3 = base.GetComponentsInChildren<tk2dSpriteAnimator>();
		for (int n = 0; n < componentsInChildren3.Length; n++)
		{
			if (componentsInChildren3[n])
			{
				componentsInChildren3[n].alwaysUpdateOffscreen = true;
			}
		}
		BraveTime.ClearMultiplier(base.gameObject);
		GameManager.IsBossIntro = false;
		SuperReaperController.PreventShooting = false;
		Minimap.Instance.TemporarilyPreventMinimap = false;
		this.m_isRunning = false;
		if (this.OnIntroFinished != null)
		{
			this.OnIntroFinished();
		}
	}

	// Token: 0x06005AF2 RID: 23282 RVA: 0x0022DF10 File Offset: 0x0022C110
	private IEnumerator WaitForBossCard()
	{
		this.m_waitingForBossCard = true;
		yield return base.StartCoroutine(this.bossUI.TriggerBossPortraitCR(this.portraitSlideSettings));
		this.m_waitingForBossCard = false;
		yield break;
	}

	// Token: 0x06005AF3 RID: 23283 RVA: 0x0022DF2C File Offset: 0x0022C12C
	private void TeleportCoopPlayers()
	{
		if (this.m_hasCoopTeleported)
		{
			return;
		}
		if (this.m_idealStartingPositions == null || this.m_idealStartingPositions.Length < 1)
		{
			return;
		}
		Vector2 centerPosition = GameManager.Instance.PrimaryPlayer.CenterPosition;
		Vector2 centerPosition2 = GameManager.Instance.SecondaryPlayer.CenterPosition;
		if (Vector2.Distance(centerPosition2, this.m_idealStartingPositions[0]) < Vector2.Distance(centerPosition, this.m_idealStartingPositions[0]))
		{
			Vector2 vector = this.m_idealStartingPositions[0];
			this.m_idealStartingPositions[0] = this.m_idealStartingPositions[1];
			this.m_idealStartingPositions[1] = vector;
		}
		if (Vector3.Distance(centerPosition, this.m_idealStartingPositions[0]) > 2f)
		{
			GameManager.Instance.PrimaryPlayer.WarpToPoint(this.m_idealStartingPositions[0], true, false);
		}
		if (Vector3.Distance(centerPosition2, this.m_idealStartingPositions[1]) > 2f)
		{
			GameManager.Instance.SecondaryPlayer.WarpToPoint(this.m_idealStartingPositions[1], true, false);
		}
	}

	// Token: 0x06005AF4 RID: 23284 RVA: 0x0022E094 File Offset: 0x0022C294
	public void SkipWalkIn()
	{
		this.m_hasTriggeredWalkIn = true;
	}

	// Token: 0x06005AF5 RID: 23285 RVA: 0x0022E0A0 File Offset: 0x0022C2A0
	private void ModifyCamera(bool value)
	{
		if (this.m_isCameraModified == value || !GameManager.HasInstance)
		{
			return;
		}
		CameraController mainCameraController = GameManager.Instance.MainCameraController;
		if (!mainCameraController)
		{
			return;
		}
		if (value)
		{
			if (this.cameraFocus)
			{
				mainCameraController.LockToRoom = true;
				mainCameraController.AddFocusPoint(this.cameraFocus);
			}
			if (this.roomPositionCameraFocus != Vector2.zero)
			{
				this.m_roomCameraFocus = new GameObject("room camera focus");
				this.m_roomCameraFocus.transform.position = base.aiActor.ParentRoom.area.basePosition.ToVector2() + this.roomPositionCameraFocus;
				this.m_roomCameraFocus.transform.parent = base.aiActor.ParentRoom.hierarchyParent;
				mainCameraController.LockToRoom = true;
				mainCameraController.AddFocusPoint(this.m_roomCameraFocus);
			}
			if (this.fusebombLock)
			{
				mainCameraController.PreventFuseBombAimOffset = true;
			}
			mainCameraController.LockToRoom = true;
			this.m_isCameraModified = true;
			if (base.aiActor && base.aiActor.healthHaver)
			{
				base.aiActor.healthHaver.OnDeath += this.OnDeath;
			}
		}
		else
		{
			if (this.cameraFocus)
			{
				mainCameraController.LockToRoom = false;
				mainCameraController.RemoveFocusPoint(this.cameraFocus);
			}
			if (this.roomPositionCameraFocus != Vector2.zero && this.m_roomCameraFocus)
			{
				mainCameraController.LockToRoom = false;
				mainCameraController.RemoveFocusPoint(this.m_roomCameraFocus);
			}
			if (this.fusebombLock)
			{
				mainCameraController.PreventFuseBombAimOffset = false;
			}
			mainCameraController.LockToRoom = false;
			this.m_isCameraModified = false;
			if (base.aiActor && base.aiActor.healthHaver)
			{
				base.aiActor.healthHaver.OnDeath -= this.OnDeath;
			}
		}
	}

	// Token: 0x06005AF6 RID: 23286 RVA: 0x0022E2B8 File Offset: 0x0022C4B8
	public void RestrictMotion(bool value)
	{
		if (this.m_isMotionRestricted == value)
		{
			return;
		}
		if (value)
		{
			if (!GameManager.HasInstance || GameManager.Instance.IsLoadingLevel || GameManager.IsReturningToBreach)
			{
				return;
			}
			CellArea area = base.aiActor.ParentRoom.area;
			this.m_minPlayerX = area.basePosition.x * 16;
			this.m_minPlayerY = area.basePosition.y * 16 + 8;
			this.m_maxPlayerX = (area.basePosition.x + area.dimensions.x) * 16;
			this.m_maxPlayerY = (area.basePosition.y + area.dimensions.y - 1) * 16;
			for (int i = 0; i < GameManager.Instance.AllPlayers.Length; i++)
			{
				SpeculativeRigidbody specRigidbody = GameManager.Instance.AllPlayers[i].specRigidbody;
				specRigidbody.MovementRestrictor = (SpeculativeRigidbody.MovementRestrictorDelegate)Delegate.Combine(specRigidbody.MovementRestrictor, new SpeculativeRigidbody.MovementRestrictorDelegate(this.PlayerMovementRestrictor));
			}
		}
		else
		{
			if (!GameManager.HasInstance || GameManager.IsReturningToBreach)
			{
				return;
			}
			for (int j = 0; j < GameManager.Instance.AllPlayers.Length; j++)
			{
				PlayerController playerController = GameManager.Instance.AllPlayers[j];
				if (playerController)
				{
					SpeculativeRigidbody specRigidbody2 = playerController.specRigidbody;
					specRigidbody2.MovementRestrictor = (SpeculativeRigidbody.MovementRestrictorDelegate)Delegate.Remove(specRigidbody2.MovementRestrictor, new SpeculativeRigidbody.MovementRestrictorDelegate(this.PlayerMovementRestrictor));
				}
			}
		}
		this.m_isMotionRestricted = value;
	}

	// Token: 0x06005AF7 RID: 23287 RVA: 0x0022E448 File Offset: 0x0022C648
	private void PlayerMovementRestrictor(SpeculativeRigidbody playerSpecRigidbody, IntVector2 prevPixelOffset, IntVector2 pixelOffset, ref bool validLocation)
	{
		if (!validLocation)
		{
			return;
		}
		if (pixelOffset.y < prevPixelOffset.y && playerSpecRigidbody.PixelColliders[0].MinY + pixelOffset.y < this.m_minPlayerY)
		{
			validLocation = false;
		}
		if (pixelOffset.y > prevPixelOffset.y && playerSpecRigidbody.PixelColliders[0].MaxY + pixelOffset.y >= this.m_maxPlayerY)
		{
			validLocation = false;
		}
		if (pixelOffset.x < prevPixelOffset.x && playerSpecRigidbody.PixelColliders[0].MinX + pixelOffset.x < this.m_minPlayerX)
		{
			validLocation = false;
		}
		if (pixelOffset.x > prevPixelOffset.x && playerSpecRigidbody.PixelColliders[0].MaxX + pixelOffset.x >= this.m_maxPlayerX)
		{
			validLocation = false;
		}
	}

	// Token: 0x06005AF8 RID: 23288 RVA: 0x0022E54C File Offset: 0x0022C74C
	private void OnDeath(Vector2 deathDir)
	{
		if (this.m_isCameraModified)
		{
			this.ModifyCamera(false);
		}
		if (this.m_isMotionRestricted)
		{
			this.RestrictMotion(false);
		}
	}

	// Token: 0x17000D38 RID: 3384
	// (get) Token: 0x06005AF9 RID: 23289 RVA: 0x0022E574 File Offset: 0x0022C774
	// (set) Token: 0x06005AFA RID: 23290 RVA: 0x0022E57C File Offset: 0x0022C77C
	public static bool SkipFrame { get; set; }

	// Token: 0x04005464 RID: 21604
	public GenericIntroDoer.TriggerType triggerType = GenericIntroDoer.TriggerType.PlayerEnteredRoom;

	// Token: 0x04005465 RID: 21605
	public float initialDelay = 1f;

	// Token: 0x04005466 RID: 21606
	public float cameraMoveSpeed = 5f;

	// Token: 0x04005467 RID: 21607
	public AIAnimator specifyIntroAiAnimator;

	// Token: 0x04005468 RID: 21608
	public string BossMusicEvent = "Play_MUS_Boss_Theme_Beholster";

	// Token: 0x04005469 RID: 21609
	public bool PreventBossMusic;

	// Token: 0x0400546A RID: 21610
	public bool InvisibleBeforeIntroAnim;

	// Token: 0x0400546B RID: 21611
	[CheckAnimation(null)]
	public string preIntroAnim = string.Empty;

	// Token: 0x0400546C RID: 21612
	[CheckDirectionalAnimation(null)]
	public string preIntroDirectionalAnim = string.Empty;

	// Token: 0x0400546D RID: 21613
	[FormerlySerializedAs("preIntroAnimationName")]
	[CheckAnimation(null)]
	public string introAnim = string.Empty;

	// Token: 0x0400546E RID: 21614
	[FormerlySerializedAs("preIntroDirectionalAnimation")]
	[CheckDirectionalAnimation(null)]
	public string introDirectionalAnim = string.Empty;

	// Token: 0x0400546F RID: 21615
	public bool continueAnimDuringOutro;

	// Token: 0x04005470 RID: 21616
	public GameObject cameraFocus;

	// Token: 0x04005471 RID: 21617
	public Vector2 roomPositionCameraFocus;

	// Token: 0x04005472 RID: 21618
	public bool restrictPlayerMotionToRoom;

	// Token: 0x04005473 RID: 21619
	public bool fusebombLock;

	// Token: 0x04005474 RID: 21620
	public float AdditionalHeightOffset;

	// Token: 0x04005475 RID: 21621
	public bool SkipBossCard;

	// Token: 0x04005476 RID: 21622
	[HideInInspectorIf("SkipBossCard", false)]
	public PortraitSlideSettings portraitSlideSettings;

	// Token: 0x04005477 RID: 21623
	public bool HideGunAndHand;

	// Token: 0x0400547A RID: 21626
	public Action OnIntroFinished;

	// Token: 0x0400547B RID: 21627
	private Tribool m_singleFrameSkipDelay = Tribool.Unready;

	// Token: 0x0400547C RID: 21628
	private bool m_isRunning;

	// Token: 0x0400547D RID: 21629
	private bool m_waitingForBossCard;

	// Token: 0x0400547E RID: 21630
	private bool m_hasTriggeredWalkIn;

	// Token: 0x0400547F RID: 21631
	private GenericIntroDoer.Phase m_currentPhase;

	// Token: 0x04005480 RID: 21632
	private bool m_phaseComplete = true;

	// Token: 0x04005481 RID: 21633
	private float m_phaseCountdown;

	// Token: 0x04005482 RID: 21634
	private CameraController m_camera;

	// Token: 0x04005483 RID: 21635
	private Transform m_cameraTransform;

	// Token: 0x04005484 RID: 21636
	private RoomHandler m_room;

	// Token: 0x04005485 RID: 21637
	private GameUIBossHealthController bossUI;

	// Token: 0x04005486 RID: 21638
	private List<tk2dSpriteAnimator> m_animators = new List<tk2dSpriteAnimator>();

	// Token: 0x04005487 RID: 21639
	private List<CutsceneMotion> activeMotions = new List<CutsceneMotion>();

	// Token: 0x04005488 RID: 21640
	private SpecificIntroDoer m_specificIntroDoer;

	// Token: 0x04005489 RID: 21641
	private bool m_waitingForSpecificIntroCompletion;

	// Token: 0x0400548A RID: 21642
	private GameObject m_roomCameraFocus;

	// Token: 0x0400548B RID: 21643
	private bool m_isCameraModified;

	// Token: 0x0400548C RID: 21644
	private bool m_isMotionRestricted;

	// Token: 0x0400548D RID: 21645
	private int m_minPlayerX;

	// Token: 0x0400548E RID: 21646
	private int m_minPlayerY;

	// Token: 0x0400548F RID: 21647
	private int m_maxPlayerX;

	// Token: 0x04005490 RID: 21648
	private int m_maxPlayerY;

	// Token: 0x04005491 RID: 21649
	private Vector2[] m_idealStartingPositions;

	// Token: 0x04005492 RID: 21650
	private bool m_hasCoopTeleported;

	// Token: 0x02001032 RID: 4146
	public enum TriggerType
	{
		// Token: 0x04005495 RID: 21653
		PlayerEnteredRoom = 10,
		// Token: 0x04005496 RID: 21654
		BossTriggerZone = 20
	}

	// Token: 0x02001033 RID: 4147
	private enum Phase
	{
		// Token: 0x04005498 RID: 21656
		CameraIntro,
		// Token: 0x04005499 RID: 21657
		InitialDelay,
		// Token: 0x0400549A RID: 21658
		PreIntroAnim,
		// Token: 0x0400549B RID: 21659
		BossCard,
		// Token: 0x0400549C RID: 21660
		CameraOutro,
		// Token: 0x0400549D RID: 21661
		Cleanup
	}
}
