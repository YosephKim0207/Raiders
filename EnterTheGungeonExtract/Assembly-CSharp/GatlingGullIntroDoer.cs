using System;
using System.Collections;
using System.Collections.Generic;
using Dungeonator;
using UnityEngine;

// Token: 0x0200102B RID: 4139
public class GatlingGullIntroDoer : TimeInvariantMonoBehaviour, IPlaceConfigurable
{
	// Token: 0x06005ABB RID: 23227 RVA: 0x0022A490 File Offset: 0x00228690
	public void ConfigureOnPlacement(RoomHandler room)
	{
		this.m_room = room;
		this.m_room.Entered += this.TriggerSequence;
	}

	// Token: 0x06005ABC RID: 23228 RVA: 0x0022A4B0 File Offset: 0x002286B0
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
			RuntimeExitDefinition exitDefinitionForConnectedRoom = this.m_room.GetExitDefinitionForConnectedRoom(roomHandler);
			DungeonData.Direction exitDirection = exitDefinitionForConnectedRoom.upstreamExit.referencedExit.exitDirection;
			IntVector2 intVector = exitDefinitionForConnectedRoom.upstreamExit.referencedExit.GetExitAttachPoint() - IntVector2.One + DungeonData.GetIntVector2FromDirection(exitDefinitionForConnectedRoom.upstreamExit.referencedExit.exitDirection);
			intVector += exitDefinitionForConnectedRoom.upstreamRoom.area.basePosition;
			float num = (float)((exitDirection != DungeonData.Direction.NORTH && exitDirection != DungeonData.Direction.SOUTH) ? intVector.x : intVector.y);
			if (exitDirection == DungeonData.Direction.EAST || exitDirection == DungeonData.Direction.NORTH)
			{
				num += (float)(exitDefinitionForConnectedRoom.upstreamExit.TotalExitLength + exitDefinitionForConnectedRoom.downstreamExit.TotalExitLength);
			}
			else
			{
				num -= (float)(exitDefinitionForConnectedRoom.upstreamExit.TotalExitLength + exitDefinitionForConnectedRoom.downstreamExit.TotalExitLength);
			}
			leadPlayer.ForceWalkInDirectionWhilePaused(exitDirection, num);
			if (GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER)
			{
				PlayerController otherPlayer = GameManager.Instance.GetOtherPlayer(leadPlayer);
				float num2;
				if (exitDirection == DungeonData.Direction.NORTH || exitDirection == DungeonData.Direction.SOUTH)
				{
					num2 = Mathf.Abs(num - leadPlayer.CenterPosition.y);
				}
				else
				{
					num2 = Mathf.Abs(num - leadPlayer.CenterPosition.x);
				}
				IntVector2 zero = IntVector2.Zero;
				int num3 = Mathf.RoundToInt(num2 * 16f);
				if (exitDirection == DungeonData.Direction.NORTH)
				{
					zero = new IntVector2(0, num3);
				}
				else if (exitDirection == DungeonData.Direction.EAST)
				{
					zero = new IntVector2(num3, 0);
				}
				else if (exitDirection == DungeonData.Direction.SOUTH)
				{
					zero = new IntVector2(0, -num3);
				}
				else if (exitDirection == DungeonData.Direction.WEST)
				{
					zero = new IntVector2(-num3, 0);
				}
				CollisionData collisionData;
				if (PhysicsEngine.Instance.RigidbodyCast(otherPlayer.specRigidbody, zero, out collisionData, true, true, null, false))
				{
					num2 = PhysicsEngine.PixelToUnit(collisionData.NewPixelsToMove).magnitude;
				}
				CollisionData.Pool.Free(ref collisionData);
				if (exitDirection == DungeonData.Direction.NORTH)
				{
					num = otherPlayer.CenterPosition.y + num2;
				}
				else if (exitDirection == DungeonData.Direction.EAST)
				{
					num = otherPlayer.CenterPosition.x + num2;
				}
				else if (exitDirection == DungeonData.Direction.SOUTH)
				{
					num = otherPlayer.CenterPosition.y - num2;
				}
				else if (exitDirection == DungeonData.Direction.WEST)
				{
					num = otherPlayer.CenterPosition.x - num2;
				}
				otherPlayer.ForceWalkInDirectionWhilePaused(exitDirection, num);
				this.m_idealStartingPositions = new Vector2[2];
				IntVector2 intVector2 = ((exitDirection != DungeonData.Direction.NORTH && exitDirection != DungeonData.Direction.SOUTH) ? (intVector + IntVector2.Up) : (intVector + IntVector2.Right));
				float num4 = (float)(exitDefinitionForConnectedRoom.upstreamExit.TotalExitLength + exitDefinitionForConnectedRoom.downstreamExit.TotalExitLength);
				switch (exitDirection)
				{
				case DungeonData.Direction.NORTH:
					this.m_idealStartingPositions[0] = intVector2.ToVector2() + new Vector2(-0.5f, 0f) + new Vector2(0f, num4 + 0.5f);
					this.m_idealStartingPositions[1] = intVector2.ToVector2() + new Vector2(0.25f, -0.25f) + new Vector2(0f, num4 - 0.25f);
					break;
				case DungeonData.Direction.EAST:
					this.m_idealStartingPositions[0] = intVector2.ToVector2() + new Vector2(num4 + 0.5f, 0f);
					this.m_idealStartingPositions[1] = intVector2.ToVector2() + new Vector2(-0.25f, -1f) + new Vector2(num4 - 0.25f, 0f);
					break;
				case DungeonData.Direction.SOUTH:
					this.m_idealStartingPositions[0] = intVector2.ToVector2() + new Vector2(-0.5f, 0f) - new Vector2(0f, num4 + 0.5f);
					this.m_idealStartingPositions[1] = intVector2.ToVector2() + new Vector2(0.25f, 0.25f) - new Vector2(0f, num4 - 0.25f);
					break;
				case DungeonData.Direction.WEST:
					this.m_idealStartingPositions[0] = intVector2.ToVector2() - new Vector2(num4 + 0.5f, 0f);
					this.m_idealStartingPositions[1] = intVector2.ToVector2() + new Vector2(0.25f, -1f) - new Vector2(num4 - 0.25f, 0f);
					break;
				}
			}
		}
	}

	// Token: 0x06005ABD RID: 23229 RVA: 0x0022AA44 File Offset: 0x00228C44
	public void TriggerSequence(PlayerController enterer)
	{
		GameManager.Instance.StartCoroutine(this.FrameDelayedTriggerSequence(enterer));
	}

	// Token: 0x06005ABE RID: 23230 RVA: 0x0022AA58 File Offset: 0x00228C58
	public IEnumerator FrameDelayedTriggerSequence(PlayerController enterer)
	{
		if (!base.enabled)
		{
			yield break;
		}
		if (GameManager.Instance.PreventPausing)
		{
			yield break;
		}
		this.m_room.Entered -= this.TriggerSequence;
		if (GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER)
		{
			if (enterer.IsPrimaryPlayer)
			{
				if (!GameManager.Instance.SecondaryPlayer.healthHaver.IsDead)
				{
					GameManager.Instance.SecondaryPlayer.ReuniteWithOtherPlayer(enterer, false);
				}
			}
			else if (!GameManager.Instance.PrimaryPlayer.healthHaver.IsDead)
			{
				GameManager.Instance.PrimaryPlayer.ReuniteWithOtherPlayer(enterer, false);
			}
		}
		GameManager.Instance.DungeonMusicController.SwitchToBossMusic("Play_MUS_Boss_Theme_Gull", base.gameObject);
		BraveTime.RegisterTimeScaleMultiplier(0f, base.gameObject);
		GameManager.IsBossIntro = true;
		GameManager.Instance.PreventPausing = true;
		GameUIRoot.Instance.HideCoreUI(string.Empty);
		GameUIRoot.Instance.ToggleLowerPanels(false, false, string.Empty);
		this.bossUI = GameUIRoot.Instance.bossController;
		base.aiAnimator.enabled = false;
		SpriteOutlineManager.ToggleOutlineRenderers(base.sprite, false);
		base.renderer.enabled = false;
		StaticReferenceManager.DestroyAllProjectiles();
		this.HandlePlayerWalkIn(enterer);
		this.m_camera = GameManager.Instance.MainCameraController;
		this.m_camera.StopTrackingPlayer();
		this.m_camera.SetManualControl(true, false);
		this.m_camera.OverridePosition = this.m_camera.transform.position;
		this.m_cameraTransform = this.m_camera.transform;
		if (this.gullAnimator == null)
		{
			GameObject gameObject = new GameObject("gull_placeholder");
			gameObject.transform.position = base.transform.position;
			tk2dSprite tk2dSprite = tk2dSprite.AddComponent(gameObject, base.sprite.Collection, base.sprite.spriteId);
			this.gullAnimator = tk2dSpriteAnimator.AddComponent(gameObject, base.spriteAnimator.Library, base.spriteAnimator.DefaultClipId);
			SpriteOutlineManager.AddOutlineToSprite(tk2dSprite, Color.black, 0.5f, 0f, SpriteOutlineManager.OutlineType.NORMAL);
			DepthLookupManager.ProcessRenderer(tk2dSprite.renderer);
			tk2dSpriteAnimator tk2dSpriteAnimator = this.gullAnimator;
			tk2dSpriteAnimator.AnimationEventTriggered = (Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip, int>)Delegate.Combine(tk2dSpriteAnimator.AnimationEventTriggered, new Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip, int>(this.HandleAnimationEvent));
			tk2dSprite.UpdateZDepth();
			this.gunObject = new GameObject("gull_gun_placeholder");
			this.gunObject.transform.position = base.transform.position;
			tk2dSprite.AddComponent(this.gunObject, base.sprite.Collection, base.sprite.Collection.GetSpriteIdByName("gatling_gun_stationary"));
			DepthLookupManager.ProcessRenderer(this.gunObject.GetComponent<Renderer>());
			this.gullAnimator.GetComponent<Renderer>().enabled = false;
		}
		yield return null;
		yield return null;
		Minimap.Instance.TemporarilyPreventMinimap = true;
		this.m_isRunning = true;
		yield break;
	}

	// Token: 0x06005ABF RID: 23231 RVA: 0x0022AA7C File Offset: 0x00228C7C
	protected void HandleAnimationEvent(tk2dSpriteAnimator animator, tk2dSpriteAnimationClip clip, int frameNo)
	{
		tk2dSpriteAnimationFrame frame = clip.GetFrame(frameNo);
		for (int i = 0; i < base.aiActor.animationAudioEvents.Count; i++)
		{
			if (base.aiActor.animationAudioEvents[i].eventTag == frame.eventInfo && GameManager.AUDIO_ENABLED)
			{
				AkSoundEngine.PostEvent(base.aiActor.animationAudioEvents[i].eventName, base.gameObject);
			}
		}
	}

	// Token: 0x06005AC0 RID: 23232 RVA: 0x0022AB04 File Offset: 0x00228D04
	protected void EndSequence()
	{
		this.bossUI.EndBossPortraitEarly();
		this.m_camera.StartTrackingPlayer();
		this.m_camera.SetManualControl(false, true);
		base.aiAnimator.enabled = true;
		SpriteOutlineManager.ToggleOutlineRenderers(base.sprite, true);
		base.renderer.enabled = true;
		if (this.m_shadowTransform != null)
		{
			this.m_shadowTransform.position = base.specRigidbody.UnitCenter;
		}
		if (this.m_shadowAnimator != null)
		{
			this.m_shadowAnimator.Play("shadow_static");
			this.m_shadowAnimator.Sprite.independentOrientation = true;
			this.m_shadowAnimator.Sprite.IsPerpendicular = false;
			this.m_shadowAnimator.Sprite.HeightOffGround = -1f;
		}
		base.specRigidbody.CollideWithOthers = true;
		if (base.aiActor)
		{
			base.aiActor.IsGone = false;
			base.aiActor.State = AIActor.ActorState.Normal;
		}
		for (int i = 0; i < GameManager.Instance.AllPlayers.Length; i++)
		{
			if (!GameManager.Instance.AllPlayers[i].healthHaver.IsDead)
			{
				GameManager.Instance.AllPlayers[i].ToggleGunRenderers(true, string.Empty);
			}
		}
		if (this.feathersSystem != null)
		{
			UnityEngine.Object.Destroy(this.feathersSystem.gameObject);
		}
		GameUIRoot.Instance.ShowCoreUI(string.Empty);
		GameUIRoot.Instance.ToggleLowerPanels(true, false, string.Empty);
		GameManager.Instance.PreventPausing = false;
		BraveTime.ClearMultiplier(base.gameObject);
		GameManager.IsBossIntro = false;
		tk2dSpriteAnimator[] componentsInChildren = base.GetComponentsInChildren<tk2dSpriteAnimator>();
		for (int j = 0; j < componentsInChildren.Length; j++)
		{
			if (componentsInChildren[j])
			{
				componentsInChildren[j].alwaysUpdateOffscreen = true;
			}
		}
		Minimap.Instance.TemporarilyPreventMinimap = false;
		this.m_isRunning = false;
	}

	// Token: 0x06005AC1 RID: 23233 RVA: 0x0022AD04 File Offset: 0x00228F04
	private IEnumerator DelayedTriggerAnimation(tk2dSpriteAnimator anim, string animName, float delay)
	{
		float elapsed = 0f;
		while (elapsed < delay)
		{
			elapsed += this.m_deltaTime;
			yield return null;
		}
		anim.Play(animName);
		yield break;
	}

	// Token: 0x06005AC2 RID: 23234 RVA: 0x0022AD34 File Offset: 0x00228F34
	private IEnumerator WaitForBossCard()
	{
		this.m_waitingForBossCard = true;
		yield return base.StartCoroutine(this.bossUI.TriggerBossPortraitCR(this.portraitSlideSettings));
		this.m_waitingForBossCard = false;
		yield break;
	}

	// Token: 0x06005AC3 RID: 23235 RVA: 0x0022AD50 File Offset: 0x00228F50
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
		SpriteOutlineManager.ToggleOutlineRenderers(base.sprite, false);
		if (this.m_shadowTransform == null && base.aiActor.ShadowObject)
		{
			this.m_shadowTransform = base.aiActor.ShadowObject.transform;
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
			if (cutsceneMotion.lerpProgress == 1f)
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
		if (flag && !this.m_hasSkipped)
		{
			if (GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER && !this.m_hasCoopTeleported)
			{
				this.TeleportCoopPlayers();
			}
			this.m_currentPhase = 13;
			this.m_phaseComplete = true;
			this.activeMotions.Clear();
			this.m_hasSkipped = false;
			base.specRigidbody.CollideWithOthers = true;
			if (base.aiActor)
			{
				base.aiActor.IsGone = false;
				base.aiActor.State = AIActor.ActorState.Normal;
			}
			tk2dSpriteAnimator[] componentsInChildren = base.GetComponentsInChildren<tk2dSpriteAnimator>();
			for (int k = 0; k < componentsInChildren.Length; k++)
			{
				if (componentsInChildren[k])
				{
					componentsInChildren[k].alwaysUpdateOffscreen = true;
				}
			}
		}
		Vector3 vector3 = new Vector3(0f, 1f, 0f);
		if (this.m_phaseComplete)
		{
			switch (this.m_currentPhase)
			{
			case 0:
			{
				this.gullAnimator.transform.position = new Vector3(30f, 5f, 0f) + base.transform.position + vector3;
				SpriteOutlineManager.ToggleOutlineRenderers(this.gullAnimator.GetComponent<tk2dSprite>(), false);
				CutsceneMotion cutsceneMotion2 = new CutsceneMotion(this.m_cameraTransform, new Vector2?(base.specRigidbody.UnitCenter), this.cameraMoveSpeed, 0f);
				cutsceneMotion2.camera = this.m_camera;
				this.activeMotions.Add(cutsceneMotion2);
				this.m_phaseComplete = false;
				break;
			}
			case 1:
				this.m_phaseCountdown = this.initialDelay;
				this.m_phaseComplete = false;
				if (GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER && !this.m_hasCoopTeleported)
				{
					this.TeleportCoopPlayers();
				}
				break;
			case 2:
			{
				this.m_shadowAnimator = base.aiActor.ShadowObject.GetComponent<tk2dSpriteAnimator>();
				this.m_animators.Add(this.m_shadowAnimator);
				this.m_animators.Add(this.gullAnimator);
				CutsceneMotion cutsceneMotion3 = new CutsceneMotion(this.gullAnimator.transform, new Vector2?(new Vector2(-60f, 0f) + this.gullAnimator.transform.position.XY()), 27f, 0f);
				this.activeMotions.Add(cutsceneMotion3);
				AkSoundEngine.PostEvent("Play_ANM_Gull_Shadow_01", base.gameObject);
				this.m_phaseComplete = false;
				break;
			}
			case 3:
				this.m_phaseCountdown = this.initialDelay;
				this.m_phaseComplete = false;
				break;
			case 4:
			{
				this.gullAnimator.GetComponent<Renderer>().enabled = true;
				SpriteOutlineManager.ToggleOutlineRenderers(this.gullAnimator.GetComponent<tk2dSprite>(), true);
				this.m_shadowAnimator.Play("shadow_out");
				AkSoundEngine.PostEvent("Play_ANM_Gull_Intro_01", base.gameObject);
				this.gullAnimator.enabled = false;
				this.gullAnimator.Play(this.gullAnimator.GetClipByName("fly"));
				CutsceneMotion cutsceneMotion4 = new CutsceneMotion(this.gullAnimator.transform, new Vector2?(base.transform.position + vector3), 20f, 0f);
				cutsceneMotion4.isSmoothStepped = false;
				this.activeMotions.Add(cutsceneMotion4);
				this.m_phaseComplete = false;
				break;
			}
			case 5:
				UnityEngine.Object.Destroy(this.gunObject);
				this.gullAnimator.transform.position -= vector3;
				this.gullAnimator.Play(this.gullAnimator.GetClipByName("pick_up"));
				AkSoundEngine.PostEvent("Play_ANM_Gull_Lift_01", base.gameObject);
				this.m_phaseCountdown = (float)this.gullAnimator.CurrentClip.frames.Length / this.gullAnimator.CurrentClip.fps;
				this.m_phaseComplete = false;
				break;
			case 6:
			{
				this.m_shadowAnimator.Play("shadow_in");
				this.gullAnimator.Play(this.gullAnimator.GetClipByName("fly_pick_up"));
				this.gullAnimator.Sprite.HeightOffGround += 3f;
				CutsceneMotion cutsceneMotion5 = new CutsceneMotion(this.gullAnimator.transform, new Vector2?(base.transform.position + new Vector3(20f, 20f, 0f)), 15f, 0f);
				cutsceneMotion5.isSmoothStepped = false;
				this.activeMotions.Add(cutsceneMotion5);
				this.m_phaseComplete = false;
				break;
			}
			case 7:
				this.m_phaseCountdown = 1f;
				this.m_phaseComplete = false;
				this.gullAnimator.Sprite.HeightOffGround -= 3f;
				this.gullAnimator.Play("land_feathered");
				this.gullAnimator.Stop();
				break;
			case 8:
			{
				base.aiActor.ToggleShadowVisiblity(true);
				this.m_shadowAnimator.Play("shadow_out");
				AkSoundEngine.PostEvent("Play_ANM_Gull_Descend_01", base.gameObject);
				base.StartCoroutine(this.DelayedTriggerAnimation(this.gullAnimator, "land_feathered", 0.8f));
				this.gullAnimator.transform.position = base.transform.position + new Vector3(0f, 50f, 0f);
				CutsceneMotion cutsceneMotion6 = new CutsceneMotion(this.gullAnimator.transform, new Vector2?(base.transform.position), 50f, 0f);
				this.activeMotions.Add(cutsceneMotion6);
				this.m_phaseComplete = false;
				break;
			}
			case 9:
				this.m_camera.DoScreenShake(this.landingShakeSettings, null, false);
				this.m_phaseCountdown = 1.5f;
				this.m_phaseComplete = false;
				break;
			case 10:
				this.m_animators.Remove(this.m_shadowAnimator);
				this.gullAnimator.Play(this.gullAnimator.GetClipByName("awaken_feathered"));
				AkSoundEngine.PostEvent("Play_ANM_Gull_Flex_01", base.gameObject);
				this.m_phaseCountdown = (float)this.gullAnimator.CurrentClip.frames.Length / this.gullAnimator.CurrentClip.fps;
				this.m_phaseComplete = false;
				break;
			case 11:
			{
				Vector3 vector4 = base.transform.position + base.sprite.GetBounds().center + new Vector3(0f, 0f, -5f);
				this.feathersSystem = SpawnManager.SpawnVFX(this.feathersVFX, vector4, this.feathersVFX.transform.rotation).GetComponent<ParticleSystem>();
				this.feathersSystem.Play();
				for (int l = 0; l < this.numFeathersToSpawn; l++)
				{
					float num5 = (float)l * (360f / (float)this.numFeathersToSpawn);
					DebrisObject component = SpawnManager.SpawnDebris(this.feathersDebris, vector4, Quaternion.identity).GetComponent<DebrisObject>();
					float num6 = UnityEngine.Random.Range(4f, 10f);
					float num7 = UnityEngine.Random.Range(2f, 5f);
					float num8 = UnityEngine.Random.Range(0.5f, 2f);
					component.Trigger((Quaternion.Euler(0f, 0f, num5) * Vector2.right * num6).WithZ(num7), num8, 1f);
				}
				this.m_camera.DoScreenShake(this.featherShakeSettings, null, false);
				this.gullAnimator.Play(this.gullAnimator.GetClipByName("awaken_plucked"));
				this.m_phaseCountdown = (float)this.gullAnimator.CurrentClip.frames.Length / this.gullAnimator.CurrentClip.fps;
				this.m_phaseComplete = false;
				break;
			}
			case 12:
				AkSoundEngine.PostEvent("Play_UI_boss_intro_01", base.gameObject);
				base.StartCoroutine(this.WaitForBossCard());
				this.m_phaseCountdown = 1E+10f;
				this.m_phaseComplete = false;
				break;
			case 13:
			{
				this.gullAnimator.enabled = true;
				this.m_animators.Remove(this.gullAnimator);
				GameManager.Instance.MainCameraController.ForceUpdateControllerCameraState(CameraController.ControllerCameraState.RoomLock);
				CutsceneMotion cutsceneMotion7 = new CutsceneMotion(this.m_cameraTransform, null, this.cameraMoveSpeed, 0f);
				cutsceneMotion7.camera = this.m_camera;
				this.activeMotions.Add(cutsceneMotion7);
				this.m_phaseComplete = false;
				break;
			}
			case 14:
				if (this.gunObject)
				{
					UnityEngine.Object.Destroy(this.gunObject);
				}
				UnityEngine.Object.Destroy(this.gullAnimator.gameObject);
				this.EndSequence();
				return;
			}
		}
		if (this.m_currentPhase > 14)
		{
			this.m_currentPhase = 14;
		}
		Bounds untrimmedBounds = this.gullAnimator.Sprite.GetUntrimmedBounds();
		if (this.m_shadowTransform != null)
		{
			this.m_shadowTransform.position = this.m_shadowTransform.position.WithX(this.gullAnimator.transform.position.x + untrimmedBounds.extents.x);
		}
		if (this.m_currentPhase == 12 && !this.m_waitingForBossCard)
		{
			this.m_phaseCountdown = 0f;
			this.m_currentPhase++;
			this.m_phaseComplete = true;
		}
		if (this.feathersSystem != null)
		{
			this.feathersSystem.Simulate(realDeltaTime, true, false);
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
		this.gullAnimator.GetComponent<tk2dSprite>().UpdateZDepth();
	}

	// Token: 0x06005AC4 RID: 23236 RVA: 0x0022B9B0 File Offset: 0x00229BB0
	protected override void OnDestroy()
	{
		if (this.m_room != null)
		{
			this.m_room.Entered -= this.TriggerSequence;
		}
		base.OnDestroy();
	}

	// Token: 0x06005AC5 RID: 23237 RVA: 0x0022B9DC File Offset: 0x00229BDC
	private void TeleportCoopPlayers()
	{
		if (this.m_hasCoopTeleported)
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

	// Token: 0x0400542C RID: 21548
	public float initialDelay = 1f;

	// Token: 0x0400542D RID: 21549
	public float cameraMoveSpeed = 5f;

	// Token: 0x0400542E RID: 21550
	public PortraitSlideSettings portraitSlideSettings;

	// Token: 0x0400542F RID: 21551
	public ScreenShakeSettings landingShakeSettings;

	// Token: 0x04005430 RID: 21552
	public ScreenShakeSettings featherShakeSettings;

	// Token: 0x04005431 RID: 21553
	[HideInInspector]
	public tk2dSpriteAnimator gullAnimator;

	// Token: 0x04005432 RID: 21554
	public GameObject feathersVFX;

	// Token: 0x04005433 RID: 21555
	public GameObject feathersDebris;

	// Token: 0x04005434 RID: 21556
	public int numFeathersToSpawn = 15;

	// Token: 0x04005435 RID: 21557
	protected bool m_isRunning;

	// Token: 0x04005436 RID: 21558
	protected List<tk2dSpriteAnimator> m_animators = new List<tk2dSpriteAnimator>();

	// Token: 0x04005437 RID: 21559
	protected float m_elapsedFrameTime;

	// Token: 0x04005438 RID: 21560
	protected CameraController m_camera;

	// Token: 0x04005439 RID: 21561
	protected Transform m_cameraTransform;

	// Token: 0x0400543A RID: 21562
	protected List<CutsceneMotion> activeMotions = new List<CutsceneMotion>();

	// Token: 0x0400543B RID: 21563
	protected RoomHandler m_room;

	// Token: 0x0400543C RID: 21564
	protected Transform m_shadowTransform;

	// Token: 0x0400543D RID: 21565
	protected tk2dSpriteAnimator m_shadowAnimator;

	// Token: 0x0400543E RID: 21566
	protected int m_currentPhase;

	// Token: 0x0400543F RID: 21567
	protected bool m_phaseComplete = true;

	// Token: 0x04005440 RID: 21568
	protected bool m_hasSkipped;

	// Token: 0x04005441 RID: 21569
	protected float m_phaseCountdown;

	// Token: 0x04005442 RID: 21570
	protected GameObject gunObject;

	// Token: 0x04005443 RID: 21571
	protected ParticleSystem feathersSystem;

	// Token: 0x04005444 RID: 21572
	protected GameUIBossHealthController bossUI;

	// Token: 0x04005445 RID: 21573
	private bool m_hasTriggeredWalkIn;

	// Token: 0x04005446 RID: 21574
	private bool m_waitingForBossCard;

	// Token: 0x04005447 RID: 21575
	private Vector2[] m_idealStartingPositions;

	// Token: 0x04005448 RID: 21576
	private bool m_hasCoopTeleported;
}
