using System;
using System.Collections;
using Reaktion;
using UnityEngine;

// Token: 0x0200171C RID: 5916
public class TimeTubeCreditsController
{
	// Token: 0x06008961 RID: 35169 RVA: 0x00390D4C File Offset: 0x0038EF4C
	protected Vector4 GetCenterPointInScreenUV(Vector2 centerPoint, float dIntensity, float dRadius)
	{
		Vector3 vector = GameManager.Instance.MainCameraController.Camera.WorldToViewportPoint(centerPoint.ToVector3ZUp(0f));
		return new Vector4(vector.x, vector.y, dRadius, dIntensity);
	}

	// Token: 0x06008962 RID: 35170 RVA: 0x00390D90 File Offset: 0x0038EF90
	public void Cleanup()
	{
		for (int i = 0; i < GameManager.Instance.AllPlayers.Length; i++)
		{
			GameManager.Instance.AllPlayers[i].ClearInputOverride("time tube");
		}
		Pixelator.Instance.AdditionalCoreStackRenderPass = null;
		UnityEngine.Object.Destroy(this.m_decayMaterial);
	}

	// Token: 0x06008963 RID: 35171 RVA: 0x00390DE8 File Offset: 0x0038EFE8
	public void CleanupLich()
	{
		Pixelator.Instance.AdditionalCoreStackRenderPass = null;
		UnityEngine.Object.Destroy(this.m_decayMaterial);
	}

	// Token: 0x06008964 RID: 35172 RVA: 0x00390E00 File Offset: 0x0038F000
	public static void AcquireTunnelInstanceInAdvance()
	{
		TimeTubeCreditsController.PreAcquiredTunnelInstance = (GameObject)UnityEngine.Object.Instantiate(BraveResources.Load("Global Prefabs/ModTunnel_02", ".prefab"), new Vector3(-100f, -700f, 0f), Quaternion.identity);
		TimeTubeCreditsController.PreAcquiredTunnelInstance.SetActive(false);
	}

	// Token: 0x06008965 RID: 35173 RVA: 0x00390E50 File Offset: 0x0038F050
	public static void AcquirePastDioramaInAdvance()
	{
		string text = ((!GameManager.IsGunslingerPast) ? "GungeonPastDiorama" : "GungeonTruePastDiorama");
		GameObject gameObject = BraveResources.Load(text, ".prefab") as GameObject;
		TitleDioramaController component = UnityEngine.Object.Instantiate<GameObject>(gameObject, gameObject.transform.position, Quaternion.identity).GetComponent<TitleDioramaController>();
		TimeTubeCreditsController.PreAcquiredPastDiorama = component.gameObject;
		TimeTubeCreditsController.PreAcquiredPastDiorama.SetActive(false);
	}

	// Token: 0x06008966 RID: 35174 RVA: 0x00390EBC File Offset: 0x0038F0BC
	public static void ClearPerLevelData()
	{
		TimeTubeCreditsController.PreAcquiredTunnelInstance = null;
		TimeTubeCreditsController.PreAcquiredPastDiorama = null;
	}

	// Token: 0x06008967 RID: 35175 RVA: 0x00390ECC File Offset: 0x0038F0CC
	public void ClearDebris()
	{
		for (int i = 0; i < StaticReferenceManager.AllDebris.Count; i++)
		{
			if (StaticReferenceManager.AllDebris[i])
			{
				Vector2 vector = StaticReferenceManager.AllDebris[i].transform.position.XY();
				if (GameManager.Instance.MainCameraController.PointIsVisible(vector))
				{
					StaticReferenceManager.AllDebris[i].TriggerDestruction(false);
				}
			}
		}
	}

	// Token: 0x06008968 RID: 35176 RVA: 0x00390F50 File Offset: 0x0038F150
	public IEnumerator HandleTimeTubeLightFX()
	{
		float hValue = 0f;
		while (this.m_shouldTimefall)
		{
			hValue = (hValue + GameManager.INVARIANT_DELTA_TIME / 4f) % 1f;
			PlatformInterface.SetAlienFXAmbientColor(new HSBColor(hValue, 1f, 1f).ToColor());
			yield return null;
		}
		yield break;
	}

	// Token: 0x06008969 RID: 35177 RVA: 0x00390F6C File Offset: 0x0038F16C
	public IEnumerator HandleTimeTubeCredits(Vector2 decayCenter, bool skipCredits, tk2dSpriteAnimator optionalAnimatorToDisable, int shotPlayerID, bool quickEndShatter = false)
	{
		TimeTubeCreditsController.IsTimeTubing = true;
		GameUIRoot.Instance.HideCoreUI(string.Empty);
		GameUIRoot.Instance.ToggleLowerPanels(false, false, string.Empty);
		GameManager.Instance.PreventPausing = true;
		for (int i = 0; i < GameManager.Instance.AllPlayers.Length; i++)
		{
			GameManager.Instance.AllPlayers[i].SetInputOverride("time tube");
		}
		Shader.SetGlobalFloat("_Tubiness", 1f);
		Pixelator.Instance.DoOcclusionLayer = false;
		this.m_decayMaterial = new Material(ShaderCache.Acquire("Brave/Internal/WorldDecay"));
		Vector4 decaySettings = this.GetCenterPointInScreenUV(decayCenter, 0f, -1f);
		this.m_decayMaterial.SetVector("_WaveCenter", decaySettings);
		this.m_decayMaterial.SetTexture("_NoiseTex", BraveResources.Load("Global VFX/shatter_mask", ".prefab") as Texture2D);
		Pixelator.Instance.AdditionalCoreStackRenderPass = this.m_decayMaterial;
		float DecayDuration = 3f;
		GameObject TunnelInstance = null;
		if (TimeTubeCreditsController.PreAcquiredTunnelInstance)
		{
			TunnelInstance = TimeTubeCreditsController.PreAcquiredTunnelInstance;
			TunnelInstance.SetActive(true);
		}
		else
		{
			TunnelInstance = (GameObject)UnityEngine.Object.Instantiate(BraveResources.Load("Global Prefabs/ModTunnel_02", ".prefab"), new Vector3(-100f, -700f, 0f), Quaternion.identity);
		}
		TunnelThatCanKillThePast TunnelController = TunnelInstance.GetComponent<TunnelThatCanKillThePast>();
		Camera TunnelCamera = TunnelController.GetComponentInChildren<Camera>();
		Pixelator.Instance.AdditionalPreBGCamera = TunnelCamera;
		BraveCameraUtility.MaintainCameraAspect(TunnelCamera);
		yield return null;
		GameManager.Instance.FlushMusicAudio();
		AkSoundEngine.PostEvent("Play_MUS_Anthem_Winner_Short_01", GameManager.Instance.gameObject);
		AkSoundEngine.PostEvent("Play_ENV_time_shatter_01", GameManager.Instance.gameObject);
		yield return null;
		this.m_shouldTimefall = true;
		for (int j = 0; j < GameManager.Instance.AllPlayers.Length; j++)
		{
			if (!this.ForceNoTimefallForCoop || GameManager.Instance.AllPlayers[j].IsPrimaryPlayer)
			{
				GameManager.Instance.StartCoroutine(this.HandleTimefallCorpse(GameManager.Instance.AllPlayers[j], GameManager.Instance.AllPlayers[j].PlayerIDX == shotPlayerID, TunnelCamera, TunnelController.transform));
			}
		}
		GameManager.Instance.StartCoroutine(this.HandleTimeTubeLightFX());
		float elapsed = 0f;
		float duration = 1f;
		float maxDecayPower = ((GameManager.Instance.PrimaryPlayer.characterIdentity != PlayableCharacters.Convict || GameManager.Instance.CurrentLevelOverrideState != GameManager.LevelOverrideState.CHARACTER_PAST) ? 2.5f : 3.5f);
		bool BGCameraIsActive = GameManager.Instance.CurrentLevelOverrideState == GameManager.LevelOverrideState.END_TIMES || GameManager.Instance.Dungeon.tileIndices.tilesetId == GlobalDungeonData.ValidTilesets.HELLGEON;
		if (skipCredits)
		{
			do
			{
				BraveCameraUtility.MaintainCameraAspect(TunnelCamera);
				elapsed += BraveTime.DeltaTime;
				if (optionalAnimatorToDisable != null && optionalAnimatorToDisable.CurrentFrame == optionalAnimatorToDisable.CurrentClip.frames.Length - 1)
				{
					optionalAnimatorToDisable.renderer.enabled = false;
				}
				decaySettings = this.GetCenterPointInScreenUV(decayCenter, 0f, BraveMathCollege.LinearToSmoothStepInterpolate(-1f, maxDecayPower, elapsed / DecayDuration));
				this.m_decayMaterial.SetVector("_WaveCenter", decaySettings);
				if (BGCameraIsActive && elapsed > DecayDuration)
				{
					BGCameraIsActive = false;
					EndTimesNebulaController endTimesNebulaController = UnityEngine.Object.FindObjectOfType<EndTimesNebulaController>();
					if (endTimesNebulaController)
					{
						endTimesNebulaController.BecomeInactive(true);
					}
				}
				yield return null;
			}
			while (elapsed <= 2f);
		}
		else
		{
			dfSprite creditsTopBar = GameUIRoot.Instance.Manager.transform.Find("CreditsTopBar").GetComponent<dfSprite>();
			dfSprite creditsBottomBar = GameUIRoot.Instance.Manager.transform.Find("CreditsBottomBar").GetComponent<dfSprite>();
			creditsTopBar.IsVisible = true;
			creditsBottomBar.IsVisible = true;
			creditsTopBar.BringToFront();
			creditsBottomBar.BringToFront();
			GameUIRoot.Instance.notificationController.ForceToFront();
			dfScrollPanel creditsPanel = GameUIRoot.Instance.Manager.transform.Find("CreditsPanel").GetComponent<dfScrollPanel>();
			creditsPanel.IsVisible = true;
			creditsPanel.RelativePosition = new Vector3(60f, 0f, 0f);
			int modPad = Mathf.FloorToInt((float)GameUIRoot.Instance.Manager.RenderCamera.pixelHeight / GameUIRoot.Instance.Manager.UIScale);
			creditsPanel.ScrollPadding = new RectOffset(0, 0, modPad, modPad + 90);
			creditsPanel.IsInteractive = false;
			creditsPanel.SendToBack();
			Vector2 maxScrollSize = creditsPanel.GetMaxScrollPositionDimensions();
			float accelCounter = 0f;
			float m_accumScrollAmount = 0f;
			do
			{
				float modDeltaTime = BraveTime.DeltaTime;
				GungeonActions activeActions = BraveInput.GetInstanceForPlayer(GameManager.Instance.PrimaryPlayer.PlayerIDX).ActiveActions;
				GungeonActions secondaryActions = ((GameManager.Instance.CurrentGameType != GameManager.GameType.COOP_2_PLAYER) ? null : BraveInput.GetInstanceForPlayer(GameManager.Instance.SecondaryPlayer.PlayerIDX).ActiveActions);
				if (activeActions.ShootAction.IsPressed || activeActions.InteractAction.IsPressed || (secondaryActions != null && (secondaryActions.ShootAction.IsPressed || secondaryActions.InteractAction.IsPressed)))
				{
					accelCounter += BraveTime.DeltaTime;
				}
				else
				{
					accelCounter = 0f;
				}
				accelCounter = Mathf.Clamp01(accelCounter);
				modDeltaTime *= Mathf.Lerp(1f, 50f, accelCounter);
				BraveCameraUtility.MaintainCameraAspect(TunnelCamera);
				elapsed += modDeltaTime;
				if (optionalAnimatorToDisable != null && optionalAnimatorToDisable.CurrentFrame == optionalAnimatorToDisable.CurrentClip.frames.Length - 1)
				{
					optionalAnimatorToDisable.renderer.enabled = false;
				}
				decaySettings = this.GetCenterPointInScreenUV(decayCenter, 0f, BraveMathCollege.LinearToSmoothStepInterpolate(-1f, maxDecayPower, elapsed / DecayDuration));
				this.m_decayMaterial.SetVector("_WaveCenter", decaySettings);
				if (GameManager.Instance.CurrentLevelOverrideState == GameManager.LevelOverrideState.CHARACTER_PAST)
				{
					float num = Mathf.Clamp01(1f - (elapsed - DecayDuration) / DecayDuration);
					Pixelator.Instance.SetFreezeFramePower(num, false);
				}
				if (BGCameraIsActive && elapsed > DecayDuration)
				{
					BGCameraIsActive = false;
					EndTimesNebulaController endTimesNebulaController2 = UnityEngine.Object.FindObjectOfType<EndTimesNebulaController>();
					if (endTimesNebulaController2)
					{
						endTimesNebulaController2.BecomeInactive(true);
					}
				}
				if (elapsed > DecayDuration + 1f)
				{
					m_accumScrollAmount += modDeltaTime * 35f;
					if (m_accumScrollAmount > 1f)
					{
						float num2 = Mathf.Floor(m_accumScrollAmount);
						m_accumScrollAmount -= num2;
						creditsPanel.ScrollPosition += new Vector2(0f, num2);
					}
				}
				yield return null;
			}
			while (creditsPanel.ScrollPosition.y < maxScrollSize.y - 60f);
			creditsTopBar.IsVisible = false;
			creditsBottomBar.IsVisible = false;
			UnityEngine.Object.Destroy(creditsPanel.gameObject);
		}
		Pixelator.Instance.ClearFreezeFrame();
		if (quickEndShatter)
		{
			TunnelController.shattering = true;
			elapsed = 0f;
			duration = 4f;
			Vector3 lastScreenShakeAmount = Vector3.zero;
			while (elapsed < duration)
			{
				elapsed += BraveTime.DeltaTime;
				float targetValue = 0f;
				if (elapsed < 1f)
				{
					targetValue = BraveMathCollege.LinearToSmoothStepInterpolate(0f, 0.2f, elapsed / 1f);
				}
				else
				{
					targetValue = 0.2f + Mathf.PingPong(elapsed - 1f, 1f) / 30f;
				}
				lastScreenShakeAmount = GameManager.Instance.MainCameraController.DoFrameScreenShake(Mathf.Lerp(0f, 0.3f, elapsed / duration), Mathf.Lerp(3f, 8f, elapsed / duration), Vector2.zero, lastScreenShakeAmount, Time.realtimeSinceStartup);
				TunnelController.ManuallySetShatterAmount(targetValue);
				yield return null;
			}
			Shader.SetGlobalFloat("_Tubiness", 0f);
			elapsed = 0f;
			float shatterDuration = 1f;
			this.m_shouldTimefall = false;
			TitleDioramaController tdc = null;
			if (TimeTubeCreditsController.PreAcquiredPastDiorama)
			{
				TimeTubeCreditsController.PreAcquiredPastDiorama.SetActive(true);
				tdc = TimeTubeCreditsController.PreAcquiredPastDiorama.GetComponent<TitleDioramaController>();
			}
			else
			{
				string text = ((!GameManager.IsGunslingerPast) ? "GungeonPastDiorama" : "GungeonTruePastDiorama");
				GameObject gameObject = BraveResources.Load(text, ".prefab") as GameObject;
				tdc = UnityEngine.Object.Instantiate<GameObject>(gameObject, gameObject.transform.position, Quaternion.identity).GetComponent<TitleDioramaController>();
			}
			tdc.ManualTrigger();
			while (elapsed < shatterDuration)
			{
				elapsed += BraveTime.DeltaTime;
				TunnelController.ManuallySetShatterAmount(Mathf.Lerp(0.2f, -100f, elapsed / shatterDuration));
				lastScreenShakeAmount = GameManager.Instance.MainCameraController.DoFrameScreenShake(Mathf.Lerp(0.5f, 0.05f, elapsed / shatterDuration), Mathf.Lerp(8f, 3f, elapsed / shatterDuration), Vector3.right, lastScreenShakeAmount, Time.realtimeSinceStartup);
				BraveCameraUtility.MaintainCameraAspect(TunnelCamera);
				this.m_timefallJitterMultiplier = Mathf.Lerp(1f, 0f, elapsed / duration);
				decaySettings = new Vector4(0.5f, 0.5f, BraveMathCollege.LinearToSmoothStepInterpolate(2.5f, -1f, elapsed / shatterDuration), 0f);
				this.m_decayMaterial.SetVector("_WaveCenter", decaySettings);
				yield return null;
			}
			GameManager.Instance.MainCameraController.ClearFrameScreenShake(lastScreenShakeAmount);
		}
		else
		{
			elapsed = 0f;
			duration = 6f;
			TunnelController.Shatter();
			dfPanel thanksPanel = null;
			if (!skipCredits)
			{
				if (GameStatsManager.Instance.GetCharacterSpecificFlag(GameManager.Instance.PrimaryPlayer.characterIdentity, CharacterSpecificGungeonFlags.KILLED_PAST))
				{
					thanksPanel = GameUIRoot.Instance.Manager.AddPrefab(BraveResources.Load("Global Prefabs/PastGameCompletePanel", ".prefab") as GameObject) as dfPanel;
					if (GameManager.Options.CurrentLanguage == StringTableManager.GungeonSupportedLanguages.GERMAN)
					{
						Transform transform = thanksPanel.transform.Find("Label (1)");
						if (transform)
						{
							dfLabel component = transform.GetComponent<dfLabel>();
							if (component)
							{
								component.AutoSize = true;
								component.Anchor = dfAnchorStyle.Bottom | dfAnchorStyle.CenterHorizontal;
							}
						}
					}
				}
				else
				{
					thanksPanel = GameUIRoot.Instance.Manager.AddPrefab(BraveResources.Load("Global Prefabs/StandardGameCompletePanel", ".prefab") as GameObject) as dfPanel;
				}
				thanksPanel.BringToFront();
				thanksPanel.PerformLayout();
			}
			Shader.SetGlobalFloat("_Tubiness", 0f);
			float shatterDuration2 = ((!skipCredits) ? 10f : 4f);
			while (elapsed < shatterDuration2)
			{
				elapsed += BraveTime.DeltaTime;
				BraveCameraUtility.MaintainCameraAspect(TunnelCamera);
				this.m_timefallJitterMultiplier = Mathf.Lerp(1f, 0f, elapsed / duration);
				if (thanksPanel != null)
				{
					thanksPanel.Opacity = Mathf.Lerp(0f, 1f, elapsed / duration);
				}
				if (elapsed > shatterDuration2 - 1f)
				{
					this.m_shouldTimefall = false;
				}
				yield return null;
			}
		}
		TimeTubeCreditsController.IsTimeTubing = false;
		yield break;
	}

	// Token: 0x0600896A RID: 35178 RVA: 0x00390FAC File Offset: 0x0038F1AC
	private IEnumerator HandleTimefallCorpse(PlayerController sourcePlayer, bool isShotPlayer, Camera TunnelCamera, Transform TunnelTransform)
	{
		if (sourcePlayer.healthHaver.IsDead)
		{
			yield break;
		}
		sourcePlayer.IsVisible = false;
		sourcePlayer.IsOnFire = false;
		sourcePlayer.CurrentPoisonMeterValue = 0f;
		sourcePlayer.ToggleFollowerRenderers(false);
		GameObject timefallCorpseInstance = (GameObject)UnityEngine.Object.Instantiate(BraveResources.Load("Global Prefabs/TimefallCorpse", ".prefab"), sourcePlayer.sprite.transform.position, Quaternion.identity);
		timefallCorpseInstance.SetLayerRecursively(LayerMask.NameToLayer("Unoccluded"));
		tk2dSpriteAnimator targetTimefallAnimator = timefallCorpseInstance.GetComponent<tk2dSpriteAnimator>();
		SpriteOutlineManager.AddOutlineToSprite(targetTimefallAnimator.Sprite, Color.black);
		tk2dSpriteAnimation timefallGenericLibrary = targetTimefallAnimator.Library;
		tk2dSpriteAnimation timefallSpecificLibrary = ((!(sourcePlayer is PlayerSpaceshipController)) ? sourcePlayer.sprite.spriteAnimator.Library : (sourcePlayer as PlayerSpaceshipController).TimefallCorpseLibrary);
		targetTimefallAnimator.Library = timefallSpecificLibrary;
		if (isShotPlayer)
		{
			if (sourcePlayer.ArmorlessAnimations && sourcePlayer.healthHaver.Armor == 0f)
			{
				targetTimefallAnimator.Play("death_shot_armorless");
			}
			else
			{
				targetTimefallAnimator.Play("death_shot");
			}
		}
		int iterator = 0;
		tk2dSpriteAnimationClip clip = null;
		float timePosition = 0f;
		Vector2[] noiseVectors = new Vector2[3];
		for (int i = 0; i < 3; i++)
		{
			float num = UnityEngine.Random.value * 3.1415927f * 2f;
			noiseVectors[i].Set(Mathf.Cos(num), Mathf.Sin(num));
		}
		Vector3 FallCenterPosOffset = Vector3.zero;
		if (!isShotPlayer)
		{
			FallCenterPosOffset = new Vector3(0.25f, -1.25f, 3f);
		}
		if (!sourcePlayer.IsPrimaryPlayer)
		{
			FallCenterPosOffset += new Vector3(0f, 0f, 1f);
		}
		Vector3 initialPosition = sourcePlayer.transform.position;
		float timefallElapsed = 0f;
		while (this.m_shouldTimefall)
		{
			timefallElapsed += GameManager.INVARIANT_DELTA_TIME;
			float positionOffsetStrength = 3f * this.m_timefallJitterMultiplier;
			float positionOffsetSpeed = 0.25f;
			timePosition += BraveTime.DeltaTime * positionOffsetSpeed;
			Vector3 p = new Vector3(JitterMotion.Fbm(noiseVectors[0] * timePosition, 2), JitterMotion.Fbm(noiseVectors[1] * timePosition, 2), 0f);
			p = p * positionOffsetStrength * 2f;
			Vector3 screenPoint = TunnelCamera.WorldToViewportPoint(TunnelTransform.position);
			Vector3 worldPoint = GameManager.Instance.MainCameraController.Camera.ViewportToWorldPoint(screenPoint);
			targetTimefallAnimator.transform.position = Vector3.Lerp(initialPosition, worldPoint + FallCenterPosOffset + p, Mathf.SmoothStep(0f, 1f, Mathf.Clamp01(timefallElapsed)));
			if (!targetTimefallAnimator.IsPlaying(targetTimefallAnimator.CurrentClip))
			{
				targetTimefallAnimator.ForceClearCurrentClip();
				float num2 = 0.5f;
				switch (iterator)
				{
				case 0:
					targetTimefallAnimator.Library = timefallSpecificLibrary;
					clip = targetTimefallAnimator.GetClipByName("timefall");
					if (clip != null)
					{
						targetTimefallAnimator.PlayForDuration("timefall", clip.BaseClipLength);
					}
					break;
				case 1:
					targetTimefallAnimator.Library = timefallSpecificLibrary;
					clip = targetTimefallAnimator.GetClipByName("timefall");
					if (clip != null)
					{
						targetTimefallAnimator.PlayForDuration("timefall", clip.BaseClipLength);
					}
					break;
				case 2:
					targetTimefallAnimator.Library = timefallGenericLibrary;
					clip = targetTimefallAnimator.GetClipByName("timefall_skull");
					if (clip != null)
					{
						targetTimefallAnimator.PlayForDuration("timefall_skull", clip.BaseClipLength * 2f);
					}
					num2 = 1f;
					break;
				case 3:
					targetTimefallAnimator.Library = timefallGenericLibrary;
					clip = targetTimefallAnimator.GetClipByName("timefall_meat");
					if (clip != null)
					{
						targetTimefallAnimator.PlayForDuration("timefall_meat", clip.BaseClipLength * 2f);
					}
					num2 = 1f;
					break;
				case 4:
					targetTimefallAnimator.Library = timefallGenericLibrary;
					clip = targetTimefallAnimator.GetClipByName("timefall_nerve");
					if (clip != null)
					{
						targetTimefallAnimator.PlayForDuration("timefall_nerve", clip.BaseClipLength * 2f);
					}
					num2 = 1f;
					break;
				default:
					targetTimefallAnimator.Library = timefallSpecificLibrary;
					clip = targetTimefallAnimator.GetClipByName("timefall");
					if (clip != null)
					{
						targetTimefallAnimator.PlayForDuration("timefall", clip.BaseClipLength);
					}
					break;
				}
				iterator = (iterator + ((UnityEngine.Random.value >= num2) ? 0 : 1)) % 5;
			}
			yield return null;
		}
		float elapsed = 0f;
		float duration = 1f;
		while (elapsed < duration)
		{
			elapsed += BraveTime.DeltaTime;
			timefallCorpseInstance.transform.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, elapsed / duration);
			yield return null;
		}
		UnityEngine.Object.Destroy(timefallCorpseInstance);
		yield break;
	}

	// Token: 0x04008F69 RID: 36713
	public static bool IsTimeTubing;

	// Token: 0x04008F6A RID: 36714
	protected Material m_decayMaterial;

	// Token: 0x04008F6B RID: 36715
	public static GameObject PreAcquiredTunnelInstance;

	// Token: 0x04008F6C RID: 36716
	public static GameObject PreAcquiredPastDiorama;

	// Token: 0x04008F6D RID: 36717
	[NonSerialized]
	public bool ForceNoTimefallForCoop;

	// Token: 0x04008F6E RID: 36718
	private bool m_shouldTimefall;

	// Token: 0x04008F6F RID: 36719
	private float m_timefallJitterMultiplier = 1f;
}
