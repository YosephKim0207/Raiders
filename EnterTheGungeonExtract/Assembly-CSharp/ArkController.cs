using System;
using System.Collections;
using System.Collections.Generic;
using Dungeonator;
using UnityEngine;

// Token: 0x020010EB RID: 4331
public class ArkController : BraveBehaviour, IPlayerInteractable
{
	// Token: 0x06005F61 RID: 24417 RVA: 0x0024ABBC File Offset: 0x00248DBC
	private IEnumerator Start()
	{
		this.m_parentRoom = GameManager.Instance.Dungeon.data.GetAbsoluteRoomFromPosition(base.transform.position.IntXY(VectorConversions.Floor));
		for (int i = 0; i < this.ParallaxTransforms.Count; i++)
		{
			this.ParallaxStartingPositions.Add(this.ParallaxTransforms[i].position);
			this.Bobbers.Add(this.ParallaxTransforms[i].GetComponent<DFGentleBob>());
		}
		yield return null;
		RoomHandler.unassignedInteractableObjects.Add(this);
		yield break;
	}

	// Token: 0x06005F62 RID: 24418 RVA: 0x0024ABD8 File Offset: 0x00248DD8
	private void Update()
	{
		if (GameManager.Instance.CurrentLevelOverrideState == GameManager.LevelOverrideState.END_TIMES)
		{
			float num = (float)this.m_parentRoom.area.basePosition.y;
			float num2 = (float)(this.m_parentRoom.area.basePosition.y + this.m_parentRoom.area.dimensions.y);
			float num3 = num2 - num;
			float x = GameManager.Instance.MainCameraController.transform.position.x;
			float y = GameManager.Instance.MainCameraController.transform.position.y;
			for (int i = 0; i < this.ParallaxTransforms.Count; i++)
			{
				float num4 = num3 * this.ParallaxFractions[i];
				float num5 = y - this.ParallaxStartingPositions[i].y;
				float num6 = x - this.ParallaxStartingPositions[i].x;
				float num7 = Mathf.Clamp(num5 / num3, -1f, 1f);
				float num8 = Mathf.Clamp(num6 / num3, -1f, 1f);
				float num9 = this.ParallaxStartingPositions[i].y + num7 * num4;
				float num10 = this.ParallaxStartingPositions[i].x + num8 * num4;
				Vector3 vector = this.ParallaxStartingPositions[i].WithY(num9).WithX(num10);
				if (this.Bobbers[i] != null)
				{
					this.Bobbers[i].AbsoluteStartPosition = vector;
				}
				else
				{
					this.ParallaxTransforms[i].position = vector;
				}
			}
		}
	}

	// Token: 0x06005F63 RID: 24419 RVA: 0x0024ADA8 File Offset: 0x00248FA8
	public float GetDistanceToPoint(Vector2 point)
	{
		if (this.m_hasBeenInteracted)
		{
			return 100000f;
		}
		return Vector2.Distance(point, base.specRigidbody.UnitCenter) / 2f;
	}

	// Token: 0x06005F64 RID: 24420 RVA: 0x0024ADD4 File Offset: 0x00248FD4
	public void OnEnteredRange(PlayerController interactor)
	{
		SpriteOutlineManager.AddOutlineToSprite(base.sprite, Color.white);
		SpriteOutlineManager.AddOutlineToSprite(this.LidAnimator.sprite, Color.white);
	}

	// Token: 0x06005F65 RID: 24421 RVA: 0x0024ADFC File Offset: 0x00248FFC
	public void OnExitRange(PlayerController interactor)
	{
		SpriteOutlineManager.RemoveOutlineFromSprite(base.sprite, true);
		SpriteOutlineManager.RemoveOutlineFromSprite(this.LidAnimator.sprite, true);
	}

	// Token: 0x06005F66 RID: 24422 RVA: 0x0024AE1C File Offset: 0x0024901C
	public void Interact(PlayerController interactor)
	{
		SpriteOutlineManager.RemoveOutlineFromSprite(base.sprite, false);
		SpriteOutlineManager.RemoveOutlineFromSprite(this.LidAnimator.sprite, false);
		if (!this.m_hasBeenInteracted)
		{
			this.m_hasBeenInteracted = true;
		}
		for (int i = 0; i < GameManager.Instance.AllPlayers.Length; i++)
		{
			GameManager.Instance.AllPlayers[i].RemoveBrokenInteractable(this);
		}
		BraveInput.DoVibrationForAllPlayers(Vibration.Time.Normal, Vibration.Strength.Medium);
		if (GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER)
		{
			PlayerController otherPlayer = GameManager.Instance.GetOtherPlayer(interactor);
			float num = Vector2.Distance(otherPlayer.CenterPosition, interactor.CenterPosition);
			if (num > 8f || num < 0.75f)
			{
				Vector2 vector = Vector2.right;
				if (interactor.CenterPosition.x < this.ChestAnimator.sprite.WorldCenter.x)
				{
					vector = Vector2.left;
				}
				otherPlayer.WarpToPoint(otherPlayer.transform.position.XY() + vector * 2f, true, false);
			}
		}
		base.StartCoroutine(this.Open(interactor));
	}

	// Token: 0x06005F67 RID: 24423 RVA: 0x0024AF44 File Offset: 0x00249144
	private IEnumerator HandleLightSprite()
	{
		yield return new WaitForSeconds(0.5f);
		float elapsed = 0f;
		float duration = 1f;
		this.LightSpriteBeam.renderer.enabled = true;
		while (elapsed < duration)
		{
			elapsed += BraveTime.DeltaTime;
			float t = Mathf.SmoothStep(0f, 1f, elapsed / duration);
			this.LightSpriteBeam.transform.localScale = new Vector3(1f, Mathf.Lerp(0f, 1f, t), 1f);
			this.LightSpriteBeam.transform.localPosition = new Vector3(0f, Mathf.Lerp(1.375f, 0f, t), 0f);
			this.LightSpriteBeam.UpdateZDepth();
			yield return null;
		}
		yield break;
	}

	// Token: 0x06005F68 RID: 24424 RVA: 0x0024AF60 File Offset: 0x00249160
	private IEnumerator Open(PlayerController interactor)
	{
		for (int i = 0; i < GameManager.Instance.AllPlayers.Length; i++)
		{
			if (GameManager.Instance.AllPlayers[i].healthHaver.IsAlive)
			{
				GameManager.Instance.AllPlayers[i].SetInputOverride("ark");
			}
		}
		this.LidAnimator.Play();
		this.ChestAnimator.Play();
		this.PoofAnimator.PlayAndDisableObject(string.Empty, null);
		base.specRigidbody.Reinitialize();
		GameManager.Instance.MainCameraController.OverrideRecoverySpeed = 2f;
		GameManager.Instance.MainCameraController.OverridePosition = this.ChestAnimator.sprite.WorldCenter + new Vector2(0f, 2f);
		GameManager.Instance.MainCameraController.SetManualControl(true, true);
		base.StartCoroutine(this.HandleLightSprite());
		while (this.LidAnimator.IsPlaying(this.LidAnimator.CurrentClip))
		{
			yield return null;
		}
		yield return base.StartCoroutine(this.HandleGun(interactor));
		yield return new WaitForSeconds(0.5f);
		Pixelator.Instance.DoFinalNonFadedLayer = true;
		yield return base.StartCoroutine(this.HandleClockhair(interactor));
		interactor.ClearInputOverride("ark");
		yield break;
	}

	// Token: 0x06005F69 RID: 24425 RVA: 0x0024AF84 File Offset: 0x00249184
	private Vector2 GetTargetClockhairPosition(BraveInput input, Vector2 currentClockhairPosition)
	{
		Vector2 vector;
		if (input.IsKeyboardAndMouse(false))
		{
			vector = GameManager.Instance.MainCameraController.Camera.ScreenToWorldPoint(Input.mousePosition).XY() + new Vector2(0.375f, -0.25f);
		}
		else
		{
			vector = currentClockhairPosition + input.ActiveActions.Aim.Vector * 10f * BraveTime.DeltaTime;
		}
		vector = Vector2.Max(GameManager.Instance.MainCameraController.MinVisiblePoint, vector);
		return Vector2.Min(GameManager.Instance.MainCameraController.MaxVisiblePoint, vector);
	}

	// Token: 0x06005F6A RID: 24426 RVA: 0x0024B030 File Offset: 0x00249230
	private void UpdateCameraPositionDuringClockhair(Vector2 targetPosition)
	{
		float num = Vector2.Distance(targetPosition, this.ChestAnimator.sprite.WorldCenter);
		if (num > 8f)
		{
			targetPosition = this.ChestAnimator.sprite.WorldCenter;
		}
		Vector2 vector = GameManager.Instance.MainCameraController.OverridePosition;
		if (Vector2.Distance(vector, targetPosition) > 10f)
		{
			vector = GameManager.Instance.MainCameraController.transform.position.XY();
		}
		GameManager.Instance.MainCameraController.OverridePosition = Vector3.MoveTowards(vector, targetPosition, BraveTime.DeltaTime);
	}

	// Token: 0x06005F6B RID: 24427 RVA: 0x0024B0D8 File Offset: 0x002492D8
	private bool CheckPlayerTarget(PlayerController target, Transform clockhairTransform)
	{
		Vector2 vector = clockhairTransform.position.XY() + new Vector2(-0.375f, 0.25f);
		return Vector2.Distance(vector, target.CenterPosition) < 0.625f;
	}

	// Token: 0x06005F6C RID: 24428 RVA: 0x0024B118 File Offset: 0x00249318
	private bool CheckHellTarget(tk2dBaseSprite hellTarget, Transform clockhairTransform)
	{
		if (hellTarget == null)
		{
			return false;
		}
		Vector2 vector = clockhairTransform.position.XY() + new Vector2(-0.375f, 0.25f);
		return Vector2.Distance(vector, hellTarget.WorldCenter) < 0.625f;
	}

	// Token: 0x06005F6D RID: 24429 RVA: 0x0024B168 File Offset: 0x00249368
	public void HandleHeldGunSpriteFlip(bool flipped)
	{
		tk2dSprite component = this.m_heldPastGun.GetComponent<tk2dSprite>();
		if (flipped)
		{
			if (!component.FlipY)
			{
				component.FlipY = true;
			}
		}
		else if (component.FlipY)
		{
			component.FlipY = false;
		}
		Transform transform = this.m_heldPastGun.Find("PrimaryHand");
		this.m_heldPastGun.localPosition = -transform.localPosition;
		if (flipped)
		{
			this.m_heldPastGun.localPosition = Vector3.Scale(this.m_heldPastGun.localPosition, new Vector3(1f, -1f, 1f));
		}
		this.m_heldPastGun.localPosition = BraveUtility.QuantizeVector(this.m_heldPastGun.localPosition, 16f);
		component.ForceRotationRebuild();
		component.UpdateZDepth();
	}

	// Token: 0x06005F6E RID: 24430 RVA: 0x0024B238 File Offset: 0x00249438
	private void PointGunAtClockhair(PlayerController interactor, Transform clockhairTransform)
	{
		Vector2 centerPosition = interactor.CenterPosition;
		Vector2 vector = clockhairTransform.position.XY() - centerPosition;
		if (this.m_isLocalPointing && vector.sqrMagnitude > 9f)
		{
			this.m_isLocalPointing = false;
		}
		else if (this.m_isLocalPointing || vector.sqrMagnitude < 4f)
		{
			this.m_isLocalPointing = true;
			float num = vector.sqrMagnitude / 4f - 0.05f;
			vector = Vector2.Lerp(Vector2.right, vector, num);
		}
		float num2 = BraveMathCollege.Atan2Degrees(vector);
		num2 = num2.Quantize(3f);
		interactor.GunPivot.rotation = Quaternion.Euler(0f, 0f, num2);
		interactor.ForceIdleFacePoint(vector, false);
		this.HandleHeldGunSpriteFlip(interactor.SpriteFlipped);
	}

	// Token: 0x06005F6F RID: 24431 RVA: 0x0024B30C File Offset: 0x0024950C
	private IEnumerator HandleClockhair(PlayerController interactor)
	{
		Transform clockhairTransform = ((GameObject)UnityEngine.Object.Instantiate(BraveResources.Load("Clockhair", ".prefab"))).transform;
		ClockhairController clockhair = clockhairTransform.GetComponent<ClockhairController>();
		float elapsed = 0f;
		float duration = clockhair.ClockhairInDuration;
		Vector3 clockhairTargetPosition = interactor.CenterPosition;
		Vector3 clockhairStartPosition = clockhairTargetPosition + new Vector3(-20f, 5f, 0f);
		clockhair.renderer.enabled = true;
		clockhair.spriteAnimator.alwaysUpdateOffscreen = true;
		clockhair.spriteAnimator.Play("clockhair_intro");
		clockhair.hourAnimator.Play("hour_hand_intro");
		clockhair.minuteAnimator.Play("minute_hand_intro");
		clockhair.secondAnimator.Play("second_hand_intro");
		BraveInput currentInput = BraveInput.GetInstanceForPlayer(interactor.PlayerIDX);
		while (elapsed < duration)
		{
			this.UpdateCameraPositionDuringClockhair(interactor.CenterPosition);
			if (GameManager.INVARIANT_DELTA_TIME == 0f)
			{
				elapsed += 0.05f;
			}
			elapsed += GameManager.INVARIANT_DELTA_TIME;
			float t = elapsed / duration;
			float smoothT = Mathf.SmoothStep(0f, 1f, t);
			clockhairTargetPosition = this.GetTargetClockhairPosition(currentInput, clockhairTargetPosition);
			Vector3 currentPosition = Vector3.Slerp(clockhairStartPosition, clockhairTargetPosition, smoothT);
			clockhairTransform.position = currentPosition.WithZ(0f);
			if (t > 0.5f)
			{
				clockhair.renderer.enabled = true;
			}
			if (t > 0.75f)
			{
				clockhair.hourAnimator.GetComponent<Renderer>().enabled = true;
				clockhair.minuteAnimator.GetComponent<Renderer>().enabled = true;
				clockhair.secondAnimator.GetComponent<Renderer>().enabled = true;
				GameCursorController.CursorOverride.SetOverride("ark", true, null);
			}
			clockhair.sprite.UpdateZDepth();
			this.PointGunAtClockhair(interactor, clockhairTransform);
			yield return null;
		}
		clockhair.SetMotionType(1f);
		float shotTargetTime = 0f;
		float holdDuration = 4f;
		PlayerController shotPlayer = null;
		bool didShootHellTrigger = false;
		Vector3 lastJitterAmount = Vector3.zero;
		bool m_isPlayingChargeAudio = false;
		for (;;)
		{
			this.UpdateCameraPositionDuringClockhair(interactor.CenterPosition);
			clockhair.transform.position = clockhair.transform.position - lastJitterAmount;
			clockhair.transform.position = this.GetTargetClockhairPosition(currentInput, clockhair.transform.position.XY());
			clockhair.sprite.UpdateZDepth();
			bool isTargetingValidTarget = this.CheckPlayerTarget(GameManager.Instance.PrimaryPlayer, clockhairTransform);
			shotPlayer = GameManager.Instance.PrimaryPlayer;
			if (!isTargetingValidTarget && GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER)
			{
				isTargetingValidTarget = this.CheckPlayerTarget(GameManager.Instance.SecondaryPlayer, clockhairTransform);
				shotPlayer = GameManager.Instance.SecondaryPlayer;
			}
			if (!isTargetingValidTarget && GameStatsManager.Instance.AllCorePastsBeaten())
			{
				isTargetingValidTarget = this.CheckHellTarget(this.HellCrackSprite, clockhairTransform);
				didShootHellTrigger = isTargetingValidTarget;
			}
			if (isTargetingValidTarget)
			{
				clockhair.SetMotionType(-10f);
			}
			else
			{
				clockhair.SetMotionType(1f);
			}
			if ((currentInput.ActiveActions.ShootAction.IsPressed || currentInput.ActiveActions.InteractAction.IsPressed) && isTargetingValidTarget)
			{
				if (!m_isPlayingChargeAudio)
				{
					m_isPlayingChargeAudio = true;
					AkSoundEngine.PostEvent("Play_OBJ_pastkiller_charge_01", base.gameObject);
				}
				shotTargetTime += BraveTime.DeltaTime;
			}
			else
			{
				shotTargetTime = Mathf.Max(0f, shotTargetTime - BraveTime.DeltaTime * 3f);
				if (m_isPlayingChargeAudio)
				{
					m_isPlayingChargeAudio = false;
					AkSoundEngine.PostEvent("Stop_OBJ_pastkiller_charge_01", base.gameObject);
				}
			}
			if ((currentInput.ActiveActions.ShootAction.WasReleased || currentInput.ActiveActions.InteractAction.WasReleased) && isTargetingValidTarget && shotTargetTime > holdDuration && !GameManager.Instance.IsPaused)
			{
				break;
			}
			if (shotTargetTime > 0f)
			{
				float num = Mathf.Lerp(0f, 0.35f, shotTargetTime / holdDuration);
				float num2 = 0.5f;
				float num3 = Mathf.Lerp(4f, 7f, shotTargetTime / holdDuration);
				clockhair.UpdateDistortion(num, num2, num3);
				float num4 = Mathf.Lerp(2f, 0.25f, shotTargetTime / holdDuration);
				clockhair.UpdateDesat(true, num4);
				shotTargetTime = Mathf.Min(holdDuration + 0.25f, shotTargetTime + BraveTime.DeltaTime);
				float num5 = Mathf.Lerp(0f, 0.5f, (shotTargetTime - 1f) / (holdDuration - 1f));
				Vector3 vector = (UnityEngine.Random.insideUnitCircle * num5).ToVector3ZUp(0f);
				BraveInput.DoSustainedScreenShakeVibration(shotTargetTime / holdDuration * 0.8f);
				clockhair.transform.position = clockhair.transform.position + vector;
				lastJitterAmount = vector;
				clockhair.SetMotionType(Mathf.Lerp(-10f, -2400f, shotTargetTime / holdDuration));
			}
			else
			{
				lastJitterAmount = Vector3.zero;
				clockhair.UpdateDistortion(0f, 0f, 0f);
				clockhair.UpdateDesat(false, 0f);
				shotTargetTime = 0f;
				BraveInput.DoSustainedScreenShakeVibration(0f);
			}
			this.PointGunAtClockhair(interactor, clockhairTransform);
			yield return null;
		}
		BraveInput.DoSustainedScreenShakeVibration(0f);
		BraveInput.DoVibrationForAllPlayers(Vibration.Time.Normal, Vibration.Strength.Hard);
		clockhair.StartCoroutine(clockhair.WipeoutDistortionAndFade(0.5f));
		clockhair.gameObject.SetLayerRecursively(LayerMask.NameToLayer("Unoccluded"));
		Pixelator.Instance.FadeToColor(1f, Color.white, true, 0.2f);
		Pixelator.Instance.DoRenderGBuffer = false;
		clockhair.spriteAnimator.Play("clockhair_fire");
		clockhair.hourAnimator.GetComponent<Renderer>().enabled = false;
		clockhair.minuteAnimator.GetComponent<Renderer>().enabled = false;
		clockhair.secondAnimator.GetComponent<Renderer>().enabled = false;
		yield return null;
		TimeTubeCreditsController ttcc = new TimeTubeCreditsController();
		bool isShortTunnel = didShootHellTrigger || shotPlayer.characterIdentity == PlayableCharacters.CoopCultist || this.CharacterStoryComplete(shotPlayer.characterIdentity);
		UnityEngine.Object.Destroy(this.m_heldPastGun.gameObject);
		interactor.ToggleGunRenderers(true, "ark");
		GameCursorController.CursorOverride.RemoveOverride("ark");
		Pixelator.Instance.LerpToLetterbox(0.35f, 0.25f);
		yield return base.StartCoroutine(ttcc.HandleTimeTubeCredits(clockhair.sprite.WorldCenter, isShortTunnel, clockhair.spriteAnimator, (!didShootHellTrigger) ? shotPlayer.PlayerIDX : 0, false));
		if (isShortTunnel)
		{
			Pixelator.Instance.FadeToBlack(1f, false, 0f);
			yield return new WaitForSeconds(1f);
		}
		if (didShootHellTrigger)
		{
			GameManager.DoMidgameSave(GlobalDungeonData.ValidTilesets.HELLGEON);
			GameManager.Instance.LoadCustomLevel("tt_bullethell");
		}
		else if (shotPlayer.characterIdentity == PlayableCharacters.CoopCultist)
		{
			GameManager.IsCoopPast = true;
			this.ResetPlayers(false);
			GameManager.Instance.LoadCustomLevel("fs_coop");
		}
		else if (this.CharacterStoryComplete(shotPlayer.characterIdentity) && shotPlayer.characterIdentity == PlayableCharacters.Gunslinger)
		{
			GameManager.DoMidgameSave(GlobalDungeonData.ValidTilesets.FINALGEON);
			GameManager.IsGunslingerPast = true;
			this.ResetPlayers(true);
			GameManager.Instance.LoadCustomLevel("tt_bullethell");
		}
		else if (this.CharacterStoryComplete(shotPlayer.characterIdentity))
		{
			bool flag = false;
			GameManager.DoMidgameSave(GlobalDungeonData.ValidTilesets.FINALGEON);
			switch (shotPlayer.characterIdentity)
			{
			case PlayableCharacters.Pilot:
				flag = true;
				this.ResetPlayers(false);
				GameManager.Instance.LoadCustomLevel("fs_pilot");
				break;
			case PlayableCharacters.Convict:
				flag = true;
				this.ResetPlayers(false);
				GameManager.Instance.LoadCustomLevel("fs_convict");
				break;
			case PlayableCharacters.Robot:
				flag = true;
				this.ResetPlayers(false);
				GameManager.Instance.LoadCustomLevel("fs_robot");
				break;
			case PlayableCharacters.Soldier:
				flag = true;
				this.ResetPlayers(false);
				GameManager.Instance.LoadCustomLevel("fs_soldier");
				break;
			case PlayableCharacters.Guide:
				flag = true;
				this.ResetPlayers(false);
				GameManager.Instance.LoadCustomLevel("fs_guide");
				break;
			case PlayableCharacters.Bullet:
				flag = true;
				this.ResetPlayers(false);
				GameManager.Instance.LoadCustomLevel("fs_bullet");
				break;
			}
			if (!flag)
			{
				AmmonomiconController.Instance.OpenAmmonomicon(true, true);
			}
			else
			{
				GameUIRoot.Instance.ToggleUICamera(false);
			}
		}
		else
		{
			AmmonomiconController.Instance.OpenAmmonomicon(true, true);
		}
		for (;;)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x06005F70 RID: 24432 RVA: 0x0024B330 File Offset: 0x00249530
	private void ResetPlayers(bool isGunslingerPast = false)
	{
		ArkController.IsResettingPlayers = true;
		for (int i = 0; i < GameManager.Instance.AllPlayers.Length; i++)
		{
			if (GameManager.Instance.AllPlayers[i].healthHaver.IsAlive)
			{
				if (!isGunslingerPast)
				{
					GameManager.Instance.AllPlayers[i].ResetToFactorySettings(true, true, false);
				}
				if (!isGunslingerPast)
				{
					GameManager.Instance.AllPlayers[i].CharacterUsesRandomGuns = false;
				}
				GameManager.Instance.AllPlayers[i].IsVisible = true;
				GameManager.Instance.AllPlayers[i].ClearInputOverride("ark");
				GameManager.Instance.AllPlayers[i].ClearAllInputOverrides();
			}
		}
		ArkController.IsResettingPlayers = false;
	}

	// Token: 0x06005F71 RID: 24433 RVA: 0x0024B3EC File Offset: 0x002495EC
	private void DestroyPlayers()
	{
		for (int i = 0; i < GameManager.Instance.AllPlayers.Length; i++)
		{
			UnityEngine.Object.Destroy(GameManager.Instance.AllPlayers[i].gameObject);
		}
	}

	// Token: 0x06005F72 RID: 24434 RVA: 0x0024B42C File Offset: 0x0024962C
	private bool CharacterStoryComplete(PlayableCharacters shotCharacter)
	{
		return GameStatsManager.Instance.GetFlag(GungeonFlags.BLACKSMITH_BULLET_COMPLETE) && GameManager.Instance.PrimaryPlayer.PastAccessible;
	}

	// Token: 0x06005F73 RID: 24435 RVA: 0x0024B454 File Offset: 0x00249654
	private void SpawnVFX(string vfxResourcePath, Vector2 pos)
	{
		GameObject gameObject = (GameObject)BraveResources.Load(vfxResourcePath, typeof(GameObject), ".prefab");
		GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(gameObject);
		tk2dSprite component = gameObject2.GetComponent<tk2dSprite>();
		component.PlaceAtPositionByAnchor(pos, tk2dBaseSprite.Anchor.MiddleCenter);
		component.UpdateZDepth();
	}

	// Token: 0x06005F74 RID: 24436 RVA: 0x0024B4A0 File Offset: 0x002496A0
	private IEnumerator HandleGun(PlayerController interactor)
	{
		interactor.ToggleGunRenderers(false, "ark");
		GameObject instanceGun = UnityEngine.Object.Instantiate<GameObject>(this.GunPrefab, this.GunSpawnPoint.position, Quaternion.identity);
		Material gunMaterial = instanceGun.transform.Find("GunThatCanKillThePast").GetComponent<MeshRenderer>().sharedMaterial;
		tk2dSprite instanceGunSprite = instanceGun.transform.Find("GunThatCanKillThePast").GetComponent<tk2dSprite>();
		instanceGunSprite.HeightOffGround = 5f;
		gunMaterial.SetColor("_OverrideColor", Color.white);
		float elapsed = 0f;
		float raiseTime = 4f;
		Vector3 targetMidHeightPosition = this.GunSpawnPoint.position + new Vector3(0f, 6.5f, 0f);
		interactor.ForceIdleFacePoint(new Vector2(1f, -1f), false);
		while (elapsed < raiseTime)
		{
			elapsed += BraveTime.DeltaTime;
			float t = Mathf.Clamp01(elapsed / raiseTime);
			t = BraveMathCollege.LinearToSmoothStepInterpolate(0f, 1f, t);
			instanceGun.transform.position = Vector3.Lerp(this.GunSpawnPoint.position, targetMidHeightPosition, t);
			instanceGun.transform.localScale = Vector3.Lerp(Vector3.one, Vector3.one * 2f, t);
			yield return null;
		}
		yield return new WaitForSeconds(1f);
		while (instanceGunSprite.spriteAnimator.CurrentFrame != 0)
		{
			yield return null;
		}
		instanceGunSprite.spriteAnimator.Pause();
		Pixelator.Instance.FadeToColor(0.2f, Color.white, true, 0.2f);
		yield return new WaitForSeconds(0.1f);
		Transform burstObject = instanceGun.transform.Find("GTCKTP_Burst");
		if (burstObject != null)
		{
			burstObject.gameObject.SetActive(true);
		}
		BraveInput.DoVibrationForAllPlayers(Vibration.Time.Slow, Vibration.Strength.Medium);
		yield return new WaitForSeconds(0.2f);
		instanceGunSprite.spriteAnimator.Resume();
		elapsed = 0f;
		float fadeTime = 1f;
		while (elapsed < fadeTime)
		{
			elapsed += BraveTime.DeltaTime;
			float t2 = Mathf.Clamp01(elapsed / fadeTime);
			gunMaterial.SetColor("_OverrideColor", Color.Lerp(Color.white, new Color(1f, 1f, 1f, 0f), t2));
			yield return null;
		}
		yield return new WaitForSeconds(2f);
		elapsed = 0f;
		float reraiseTime = 2f;
		while (elapsed < reraiseTime)
		{
			elapsed += BraveTime.DeltaTime;
			float t3 = Mathf.Clamp01(elapsed / reraiseTime);
			t3 = BraveMathCollege.SmoothStepToLinearStepInterpolate(0f, 1f, t3);
			instanceGun.transform.position = Vector3.Lerp(targetMidHeightPosition, interactor.CenterPosition.ToVector3ZUp(targetMidHeightPosition.z - 10f), t3);
			instanceGun.transform.localScale = Vector3.Lerp(Vector3.one * 2f, Vector3.one, t3);
			yield return null;
		}
		GameObject pickupVFXPrefab = ResourceCache.Acquire("Global VFX/VFX_Item_Pickup") as GameObject;
		interactor.PlayEffectOnActor(pickupVFXPrefab, Vector3.zero, true, false, false);
		GameObject instanceEquippedGun = UnityEngine.Object.Instantiate<GameObject>(this.HeldGunPrefab);
		AkSoundEngine.PostEvent("Play_OBJ_weapon_pickup_01", base.gameObject);
		tk2dSprite instanceEquippedSprite = instanceEquippedGun.GetComponent<tk2dSprite>();
		instanceEquippedSprite.HeightOffGround = 2f;
		instanceEquippedSprite.attachParent = interactor.sprite;
		this.m_heldPastGun = instanceEquippedGun.transform;
		this.m_heldPastGun.parent = interactor.GunPivot;
		Transform primaryHandXform = this.m_heldPastGun.Find("PrimaryHand");
		this.m_heldPastGun.localRotation = Quaternion.identity;
		this.m_heldPastGun.localPosition = -primaryHandXform.localPosition;
		instanceEquippedSprite.UpdateZDepth();
		UnityEngine.Object.Destroy(instanceGun);
		yield break;
	}

	// Token: 0x06005F75 RID: 24437 RVA: 0x0024B4C4 File Offset: 0x002496C4
	public string GetAnimationState(PlayerController interactor, out bool shouldBeFlipped)
	{
		shouldBeFlipped = false;
		return string.Empty;
	}

	// Token: 0x06005F76 RID: 24438 RVA: 0x0024B4D0 File Offset: 0x002496D0
	public float GetOverrideMaxDistance()
	{
		return -1f;
	}

	// Token: 0x06005F77 RID: 24439 RVA: 0x0024B4D8 File Offset: 0x002496D8
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x040059C8 RID: 22984
	public tk2dSpriteAnimator LidAnimator;

	// Token: 0x040059C9 RID: 22985
	public tk2dSpriteAnimator ChestAnimator;

	// Token: 0x040059CA RID: 22986
	public tk2dSpriteAnimator PoofAnimator;

	// Token: 0x040059CB RID: 22987
	public tk2dSprite LightSpriteBeam;

	// Token: 0x040059CC RID: 22988
	public tk2dSprite HellCrackSprite;

	// Token: 0x040059CD RID: 22989
	public Transform GunSpawnPoint;

	// Token: 0x040059CE RID: 22990
	public GameObject GunPrefab;

	// Token: 0x040059CF RID: 22991
	public GameObject HeldGunPrefab;

	// Token: 0x040059D0 RID: 22992
	public List<Transform> ParallaxTransforms;

	// Token: 0x040059D1 RID: 22993
	public List<float> ParallaxFractions;

	// Token: 0x040059D2 RID: 22994
	[NonSerialized]
	private List<Vector3> ParallaxStartingPositions = new List<Vector3>();

	// Token: 0x040059D3 RID: 22995
	[NonSerialized]
	private List<DFGentleBob> Bobbers = new List<DFGentleBob>();

	// Token: 0x040059D4 RID: 22996
	[NonSerialized]
	private RoomHandler m_parentRoom;

	// Token: 0x040059D5 RID: 22997
	[NonSerialized]
	private Transform m_heldPastGun;

	// Token: 0x040059D6 RID: 22998
	private bool m_hasBeenInteracted;

	// Token: 0x040059D7 RID: 22999
	protected bool m_isLocalPointing;

	// Token: 0x040059D8 RID: 23000
	public static bool IsResettingPlayers;
}
