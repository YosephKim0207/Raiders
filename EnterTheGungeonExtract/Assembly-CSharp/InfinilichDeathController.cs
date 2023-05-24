using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200104B RID: 4171
public class InfinilichDeathController : BraveBehaviour
{
	// Token: 0x06005B9B RID: 23451 RVA: 0x00230EFC File Offset: 0x0022F0FC
	public void Start()
	{
		base.healthHaver.ManualDeathHandling = true;
		base.healthHaver.OnPreDeath += this.OnBossDeath;
		base.healthHaver.SuppressContinuousKillCamBulletDestruction = true;
	}

	// Token: 0x06005B9C RID: 23452 RVA: 0x00230F30 File Offset: 0x0022F130
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x06005B9D RID: 23453 RVA: 0x00230F38 File Offset: 0x0022F138
	private void OnBossDeath(Vector2 dir)
	{
		if (ChallengeManager.CHALLENGE_MODE_ACTIVE)
		{
			ChallengeManager.Instance.ForceStop();
		}
		base.aiAnimator.PlayUntilCancelled("death", true, null, -1f, false);
		GameManager.Instance.StartCoroutine(this.OnDeathExplosionsCR());
	}

	// Token: 0x06005B9E RID: 23454 RVA: 0x00230F78 File Offset: 0x0022F178
	protected Vector2 GetTargetClockhairPosition(Vector2 currentClockhairPosition)
	{
		BraveInput instanceForPlayer = BraveInput.GetInstanceForPlayer(GameManager.Instance.PrimaryPlayer.PlayerIDX);
		Vector2 vector;
		if (instanceForPlayer.IsKeyboardAndMouse(false))
		{
			vector = GameManager.Instance.MainCameraController.Camera.ScreenToWorldPoint(Input.mousePosition).XY() + new Vector2(0.375f, -0.25f);
		}
		else
		{
			vector = currentClockhairPosition + instanceForPlayer.ActiveActions.Aim.Vector * 10f * BraveTime.DeltaTime;
		}
		vector = Vector2.Max(GameManager.Instance.MainCameraController.MinVisiblePoint, vector);
		return Vector2.Min(GameManager.Instance.MainCameraController.MaxVisiblePoint, vector);
	}

	// Token: 0x06005B9F RID: 23455 RVA: 0x00231038 File Offset: 0x0022F238
	private bool CheckTarget(GameActor target, Transform clockhairTransform)
	{
		Vector2 vector = clockhairTransform.position.XY() + new Vector2(-0.375f, 0.25f);
		return Vector2.Distance(vector, target.CenterPosition + new Vector2(0f, -1.25f)) < 0.875f;
	}

	// Token: 0x06005BA0 RID: 23456 RVA: 0x0023108C File Offset: 0x0022F28C
	private IEnumerator HandleClockhair(PlayerController interactor)
	{
		GameManager.Instance.PrimaryPlayer.SetInputOverride("past");
		if (GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER)
		{
			GameManager.Instance.SecondaryPlayer.SetInputOverride("past");
		}
		GameUIRoot.Instance.ToggleLowerPanels(false, false, string.Empty);
		GameUIRoot.Instance.HideCoreUI(string.Empty);
		CameraController camera = GameManager.Instance.MainCameraController;
		Vector2 currentCamCenter = camera.OverridePosition;
		Vector2 desiredCamCenter = base.aiAnimator.sprite.WorldCenter;
		camera.SetManualControl(true, true);
		if (Vector2.Distance(currentCamCenter, desiredCamCenter) > 2.5f)
		{
			camera.OverridePosition = desiredCamCenter;
		}
		Transform clockhairTransform = ((GameObject)UnityEngine.Object.Instantiate(BraveResources.Load("Clockhair", ".prefab"))).transform;
		ClockhairController clockhair = clockhairTransform.GetComponent<ClockhairController>();
		float elapsed = 0f;
		float duration = clockhair.ClockhairInDuration;
		Vector3 clockhairTargetPosition = GameManager.Instance.PrimaryPlayer.CenterPosition;
		Vector3 clockhairStartPosition = clockhairTargetPosition + new Vector3(-20f, 5f, 0f);
		clockhair.renderer.enabled = true;
		clockhair.spriteAnimator.alwaysUpdateOffscreen = true;
		clockhair.spriteAnimator.Play("clockhair_intro");
		clockhair.hourAnimator.Play("hour_hand_intro");
		clockhair.minuteAnimator.Play("minute_hand_intro");
		clockhair.secondAnimator.Play("second_hand_intro");
		while (elapsed < duration)
		{
			if (GameManager.INVARIANT_DELTA_TIME == 0f)
			{
				elapsed += 0.05f;
			}
			elapsed += GameManager.INVARIANT_DELTA_TIME;
			float t = elapsed / duration;
			float smoothT = Mathf.SmoothStep(0f, 1f, t);
			clockhairTargetPosition = this.GetTargetClockhairPosition(clockhairTargetPosition);
			Vector3 currentPosition = Vector3.Slerp(clockhairStartPosition, clockhairTargetPosition, smoothT);
			clockhairTransform.position = currentPosition.WithZ(0f);
			this.UpdateEyes(clockhairTransform.position, false);
			if (t > 0.5f)
			{
				clockhair.renderer.enabled = true;
			}
			if (t > 0.75f)
			{
				clockhair.hourAnimator.GetComponent<Renderer>().enabled = true;
				clockhair.minuteAnimator.GetComponent<Renderer>().enabled = true;
				clockhair.secondAnimator.GetComponent<Renderer>().enabled = true;
			}
			clockhair.sprite.UpdateZDepth();
			yield return null;
		}
		clockhair.SetMotionType(1f);
		BraveInput currentInput = BraveInput.GetInstanceForPlayer(GameManager.Instance.PrimaryPlayer.PlayerIDX);
		float shotTargetTime = 0f;
		float holdDuration = 4f;
		Vector3 lastScreenShakeAmount = Vector3.zero;
		Vector3 lastJitterAmount = Vector3.zero;
		for (;;)
		{
			clockhair.transform.position = clockhair.transform.position - lastJitterAmount;
			clockhair.transform.position = this.GetTargetClockhairPosition(clockhair.transform.position.XY());
			clockhair.sprite.UpdateZDepth();
			bool isTargetingValidTarget = this.CheckTarget(base.aiActor, clockhairTransform);
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
				shotTargetTime += BraveTime.DeltaTime;
			}
			else
			{
				shotTargetTime = Mathf.Max(0f, shotTargetTime - BraveTime.DeltaTime * 3f);
			}
			this.UpdateEyes(clockhair.transform.position, currentInput.ActiveActions.ShootAction.IsPressed || currentInput.ActiveActions.InteractAction.IsPressed);
			if ((currentInput.ActiveActions.ShootAction.WasReleased || currentInput.ActiveActions.InteractAction.WasReleased) && isTargetingValidTarget && shotTargetTime > holdDuration && !GameManager.Instance.IsPaused)
			{
				break;
			}
			if (shotTargetTime > 0f)
			{
				lastScreenShakeAmount = camera.DoFrameScreenShake(Mathf.Lerp(0f, 0.5f, shotTargetTime / holdDuration), Mathf.Lerp(3f, 8f, shotTargetTime / holdDuration), Vector3.right, lastScreenShakeAmount, Time.realtimeSinceStartup);
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
				camera.ClearFrameScreenShake(lastScreenShakeAmount);
				lastScreenShakeAmount = Vector3.zero;
				clockhair.UpdateDistortion(0f, 0f, 0f);
				clockhair.UpdateDesat(false, 0f);
				shotTargetTime = 0f;
				BraveInput.DoSustainedScreenShakeVibration(0f);
			}
			yield return null;
		}
		camera.ClearFrameScreenShake(lastScreenShakeAmount);
		lastScreenShakeAmount = Vector3.zero;
		BraveInput.DoSustainedScreenShakeVibration(0f);
		BraveInput.DoVibrationForAllPlayers(Vibration.Time.Quick, Vibration.Strength.Hard);
		clockhair.StartCoroutine(clockhair.WipeoutDistortionAndFade(0.5f));
		clockhair.gameObject.SetLayerRecursively(LayerMask.NameToLayer("Unoccluded"));
		Pixelator.Instance.FadeToColor(0.2f, Color.white, true, 0.2f);
		Pixelator.Instance.DoRenderGBuffer = false;
		clockhair.spriteAnimator.PlayAndDisableRenderer("clockhair_fire");
		clockhair.hourAnimator.GetComponent<Renderer>().enabled = false;
		clockhair.minuteAnimator.GetComponent<Renderer>().enabled = false;
		clockhair.secondAnimator.GetComponent<Renderer>().enabled = false;
		yield return null;
		yield break;
	}

	// Token: 0x06005BA1 RID: 23457 RVA: 0x002310A8 File Offset: 0x0022F2A8
	private void UpdateEyes(Vector2 clockhairPosition, bool isInDanger)
	{
		Vector2 vector = clockhairPosition - this.eyePos.transform.position.XY();
		if (isInDanger && vector.magnitude < 7f)
		{
			if (!base.aiAnimator.IsPlaying("clockhair_target"))
			{
				base.aiAnimator.PlayUntilCancelled("clockhair_target", false, null, -1f, false);
			}
			return;
		}
		base.aiAnimator.LockFacingDirection = true;
		base.aiAnimator.FacingDirection = vector.ToAngle();
		if (Mathf.Abs(vector.x) < 4f && Mathf.Abs(vector.y) < 5f)
		{
			if (vector.y > 2f)
			{
				if (!base.aiAnimator.IsPlaying("clockhair_up"))
				{
					base.aiAnimator.PlayUntilCancelled("clockhair_up", false, null, -1f, false);
				}
			}
			else if (vector.y < -2f)
			{
				if (!base.aiAnimator.IsPlaying("clockhair_down"))
				{
					base.aiAnimator.PlayUntilCancelled("clockhair_down", false, null, -1f, false);
				}
			}
			else if (!base.aiAnimator.IsPlaying("clockhair_mid"))
			{
				base.aiAnimator.PlayUntilCancelled("clockhair_mid", false, null, -1f, false);
			}
		}
		else if (!base.aiAnimator.IsPlaying("clockhair"))
		{
			base.aiAnimator.PlayUntilCancelled("clockhair", false, null, -1f, false);
		}
	}

	// Token: 0x06005BA2 RID: 23458 RVA: 0x0023124C File Offset: 0x0022F44C
	private IEnumerator OnDeathExplosionsCR()
	{
		SuperReaperController.PreventShooting = true;
		while (base.aiAnimator.IsPlaying("death"))
		{
			yield return null;
		}
		Pixelator.Instance.DoMinimap = false;
		BossKillCam extantCam = UnityEngine.Object.FindObjectOfType<BossKillCam>();
		if (extantCam)
		{
			extantCam.ForceCancelSequence();
		}
		Vector2 lichCenter = base.aiAnimator.sprite.WorldCenter;
		Pixelator.Instance.DoFinalNonFadedLayer = true;
		yield return base.StartCoroutine(this.HandleClockhair(GameManager.Instance.BestActivePlayer));
		if (GameManager.Instance.PrimaryPlayer != null)
		{
			GameStatsManager.Instance.SetCharacterSpecificFlag(CharacterSpecificGungeonFlags.CLEARED_BULLET_HELL, true);
			if (GameManager.Instance.PrimaryPlayer.characterIdentity == PlayableCharacters.Eevee && !GameStatsManager.Instance.GetFlag(GungeonFlags.GUNSLINGER_UNLOCKED))
			{
				GameManager.LastUsedPlayerPrefab = (GameObject)BraveResources.Load("PlayerGunslinger", ".prefab");
				QuickRestartOptions opts = default(QuickRestartOptions);
				opts.ChallengeMode = ChallengeModeType.None;
				opts.GunGame = false;
				opts.BossRush = false;
				opts.NumMetas = 0;
				Material glitchPass = new Material(Shader.Find("Brave/Internal/GlitchUnlit"));
				Pixelator.Instance.RegisterAdditionalRenderPass(glitchPass);
				yield return new WaitForSeconds(4f);
				Pixelator.Instance.FadeToBlack(1f, false, 0f);
				yield return new WaitForSeconds(1.25f);
				GameManager.Instance.QuickRestart(opts);
				yield break;
			}
		}
		GameManager.Instance.MainCameraController.DoScreenShake(1.25f, 8f, 0.5f, 0.75f, null);
		GameObject spawnedExplosion = SpawnManager.SpawnVFX(this.finalExplosionVfx, base.specRigidbody.HitboxPixelCollider.UnitCenter + new Vector2(-0.25f, -0.25f), Quaternion.identity);
		tk2dBaseSprite explosionSprite = spawnedExplosion.GetComponent<tk2dSprite>();
		explosionSprite.HeightOffGround = 0.8f;
		base.sprite.AttachRenderer(explosionSprite);
		base.sprite.UpdateZDepth();
		base.aiActor.StealthDeath = true;
		base.healthHaver.persistsOnDeath = true;
		base.healthHaver.DeathAnimationComplete(null, null);
		if (GameManager.Instance.CurrentGameMode == GameManager.GameMode.SUPERBOSSRUSH)
		{
			yield return null;
			UnityEngine.Object.Destroy(base.gameObject);
			GameManager.Instance.MainCameraController.SetManualControl(true, true);
			GameStatsManager.Instance.SetFlag(GungeonFlags.SHERPA_SUPERBOSSRUSH_COMPLETE, true);
			GameStatsManager.Instance.SetFlag(GungeonFlags.BOSSKILLED_SUPERBOSSRUSH, true);
			GameStatsManager.Instance.RegisterStatChange(TrackedStats.META_CURRENCY, 10f);
			GameUIRoot.Instance.ToggleLowerPanels(false, false, string.Empty);
			GameUIRoot.Instance.HideCoreUI(string.Empty);
			for (int i = 0; i < GameManager.Instance.AllPlayers.Length; i++)
			{
				GameManager.Instance.AllPlayers[i].SetInputOverride("game complete");
			}
			Pixelator.Instance.FadeToColor(0.15f, Color.white, true, 0.15f);
			AmmonomiconController.Instance.OpenAmmonomicon(true, true);
		}
		else
		{
			UnityEngine.Object.Destroy(base.gameObject);
			Pixelator.Instance.FadeToColor(0.15f, Color.white, true, 0.15f);
			if (GameManager.Instance.PrimaryPlayer.IsTemporaryEeveeForUnlock)
			{
				GameStatsManager.Instance.SetFlag(GungeonFlags.FLAG_EEVEE_UNLOCKED, true);
			}
			for (int j = 0; j < StaticReferenceManager.AllDebris.Count; j++)
			{
				if (StaticReferenceManager.AllDebris[j])
				{
					Vector2 vector = StaticReferenceManager.AllDebris[j].transform.position.XY();
					if (GameManager.Instance.MainCameraController.PointIsVisible(vector))
					{
						StaticReferenceManager.AllDebris[j].TriggerDestruction(false);
					}
				}
			}
			TimeTubeCreditsController ttcc = new TimeTubeCreditsController();
			Pixelator.Instance.LerpToLetterbox(0.35f, 0.25f);
			yield return new WaitForSeconds(0.4f);
			yield return GameManager.Instance.StartCoroutine(ttcc.HandleTimeTubeCredits(lichCenter, false, null, -1, true));
			ttcc.CleanupLich();
			Pixelator.Instance.DoFinalNonFadedLayer = true;
			BraveCameraUtility.OverrideAspect = new float?(1.7777778f);
			yield return GameManager.Instance.StartCoroutine(this.HandlePastBeingShot());
		}
		yield break;
	}

	// Token: 0x06005BA3 RID: 23459 RVA: 0x00231268 File Offset: 0x0022F468
	private IEnumerator HandlePastBeingShot()
	{
		Minimap.Instance.TemporarilyPreventMinimap = true;
		Pixelator.Instance.LerpToLetterbox(1f, 2.5f);
		yield return new WaitForSeconds(3f);
		Pixelator.Instance.FadeToColor(0.15f, Color.white, true, 0f);
		AkSoundEngine.PostEvent("Play_ENV_final_flash_01", GameManager.Instance.gameObject);
		yield return new WaitForSeconds(0.15f);
		yield return null;
		Pixelator.Instance.FadeToColor(0.25f, Color.white, true, 0.25f);
		TitleDioramaController tdc = UnityEngine.Object.FindObjectOfType<TitleDioramaController>();
		yield return new WaitForSeconds(1.25f);
		if (tdc.VFX_BulletImpact != null)
		{
			tdc.VFX_BulletImpact.SetActive(true);
			tdc.VFX_BulletImpact.GetComponent<tk2dSpriteAnimator>().PlayAndDisableObject(string.Empty, null);
			tdc.VFX_BulletImpact.GetComponent<tk2dSprite>().UpdateZDepth();
		}
		if (tdc.VFX_TrailParticles != null)
		{
			tdc.VFX_TrailParticles.SetActive(true);
			BraveUtility.EnableEmission(tdc.VFX_TrailParticles.GetComponent<ParticleSystem>(), true);
		}
		AkSoundEngine.PostEvent("Play_ENV_final_shot_01", GameManager.Instance.gameObject);
		tdc.PastIslandSprite.SetSprite("marsh_of_gungeon_past_hit_001");
		yield return new WaitForSeconds(1.25f);
		if (tdc.VFX_Splash != null)
		{
			yield return GameManager.Instance.StartCoroutine(this.HandleSplashBody(GameManager.Instance.PrimaryPlayer, true, tdc));
		}
		yield return new WaitForSeconds(2f);
		float elapsed = 0f;
		float duration = 10f;
		Transform targetXform = tdc.PastIslandSprite.transform.parent;
		while (elapsed < duration)
		{
			elapsed += BraveTime.DeltaTime;
			float t = elapsed / duration;
			tdc.SkyRenderer.material.SetFloat("_SkyBoost", Mathf.Lerp(0.88f, 0f, t));
			tdc.SkyRenderer.material.SetColor("_OverrideColor", Color.Lerp(new Color(1f, 0.55f, 0.2f, 1f), new Color(0.05f, 0.08f, 0.15f, 1f), t));
			tdc.SkyRenderer.material.SetFloat("_CurvePower", Mathf.Lerp(0.3f, -0.25f, t));
			tdc.SkyRenderer.material.SetFloat("_DitherCohesionFactor", Mathf.Lerp(0.3f, 1f, t));
			tdc.SkyRenderer.material.SetFloat("_StepValue", Mathf.Lerp(0.2f, 0.01f, t));
			targetXform.localPosition = Vector3.Lerp(Vector3.zero, new Vector3(0f, -60f, 0f), BraveMathCollege.SmoothStepToLinearStepInterpolate(0f, 1f, t));
			yield return null;
		}
		AmmonomiconController.Instance.OpenAmmonomicon(true, true);
		yield break;
	}

	// Token: 0x06005BA4 RID: 23460 RVA: 0x00231284 File Offset: 0x0022F484
	private IEnumerator HandleSplashBody(PlayerController sourcePlayer, bool isPrimaryPlayer, TitleDioramaController diorama)
	{
		AkSoundEngine.PostEvent("Play_CHR_forever_fall_01", GameManager.Instance.gameObject);
		if (sourcePlayer.healthHaver.IsDead)
		{
			yield break;
		}
		GameObject timefallCorpseInstance = (GameObject)UnityEngine.Object.Instantiate(BraveResources.Load("Global Prefabs/TimefallCorpse", ".prefab"), sourcePlayer.sprite.transform.position, Quaternion.identity);
		timefallCorpseInstance.SetLayerRecursively(LayerMask.NameToLayer("Unoccluded"));
		tk2dSpriteAnimator targetTimefallAnimator = timefallCorpseInstance.GetComponent<tk2dSpriteAnimator>();
		SpriteOutlineManager.AddOutlineToSprite(targetTimefallAnimator.Sprite, Color.black);
		tk2dSpriteAnimation timefallSpecificLibrary = ((!(sourcePlayer is PlayerSpaceshipController)) ? sourcePlayer.sprite.spriteAnimator.Library : (sourcePlayer as PlayerSpaceshipController).TimefallCorpseLibrary);
		targetTimefallAnimator.Library = timefallSpecificLibrary;
		targetTimefallAnimator.Library = timefallSpecificLibrary;
		tk2dSpriteAnimationClip clip = targetTimefallAnimator.GetClipByName("timefall");
		if (clip != null)
		{
			targetTimefallAnimator.Play("timefall");
		}
		float elapsed = 0f;
		float duration = 3f;
		while (elapsed < duration)
		{
			elapsed += BraveTime.DeltaTime;
			Vector3 startPoint = diorama.VFX_Splash.transform.position + new Vector3(-8f, 40f, 0f);
			Vector3 endPoint = diorama.VFX_Splash.GetComponent<tk2dBaseSprite>().WorldCenter.ToVector3ZUp(startPoint.z);
			targetTimefallAnimator.transform.position = Vector3.Lerp(startPoint, endPoint, Mathf.Clamp01(elapsed / duration));
			timefallCorpseInstance.transform.localScale = Vector3.Lerp(Vector3.one * 1.25f, new Vector3(0.125f, 0.125f, 0.125f), Mathf.Clamp01(elapsed / duration));
			yield return null;
		}
		AkSoundEngine.PostEvent("Play_CHR_final_splash_01", GameManager.Instance.gameObject);
		diorama.VFX_Splash.SetActive(true);
		diorama.VFX_Splash.GetComponent<tk2dSpriteAnimator>().PlayAndDisableObject(string.Empty, null);
		diorama.VFX_Splash.GetComponent<tk2dSprite>().UpdateZDepth();
		UnityEngine.Object.Destroy(timefallCorpseInstance);
		yield break;
	}

	// Token: 0x0400551D RID: 21789
	public GameObject bigExplosionVfx;

	// Token: 0x0400551E RID: 21790
	public GameObject finalExplosionVfx;

	// Token: 0x0400551F RID: 21791
	public GameObject eyePos;
}
