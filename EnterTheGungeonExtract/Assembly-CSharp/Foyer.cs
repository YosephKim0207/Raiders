using System;
using System.Collections;
using System.Collections.Generic;
using Dungeonator;
using InControl;
using UnityEngine;

// Token: 0x02000E8F RID: 3727
public class Foyer : MonoBehaviour
{
	// Token: 0x17000B1F RID: 2847
	// (get) Token: 0x06004EF1 RID: 20209 RVA: 0x001B5508 File Offset: 0x001B3708
	public static Foyer Instance
	{
		get
		{
			if (!Foyer.m_instance)
			{
				Foyer.m_instance = UnityEngine.Object.FindObjectOfType<Foyer>();
			}
			return Foyer.m_instance;
		}
	}

	// Token: 0x06004EF2 RID: 20210 RVA: 0x001B5528 File Offset: 0x001B3728
	public static void ClearInstance()
	{
		Foyer.m_instance = null;
	}

	// Token: 0x06004EF3 RID: 20211 RVA: 0x001B5530 File Offset: 0x001B3730
	private void Awake()
	{
		DebugTime.Log("Foyer.Awake()", new object[0]);
		GameManager.EnsureExistence();
		GameManager.Instance.IsFoyer = true;
	}

	// Token: 0x06004EF4 RID: 20212 RVA: 0x001B5554 File Offset: 0x001B3754
	private void CheckHeroStatue()
	{
		this.PrimerSprite.enabled = GameStatsManager.Instance.GetFlag(GungeonFlags.BLACKSMITH_ELEMENT1);
		this.PowderSprite.enabled = GameStatsManager.Instance.GetFlag(GungeonFlags.BLACKSMITH_ELEMENT2);
		this.SlugSprite.enabled = GameStatsManager.Instance.GetFlag(GungeonFlags.BLACKSMITH_ELEMENT3);
		this.CasingSprite.enabled = GameStatsManager.Instance.GetFlag(GungeonFlags.BLACKSMITH_ELEMENT4);
		if (this.PowderSprite.enabled && GameStatsManager.Instance.GetFlag(GungeonFlags.BOSSKILLED_HIGHDRAGUN))
		{
			this.PowderSprite.GetComponent<tk2dBaseSprite>().SetSprite("statue_of_time_gunpowder_gold_001");
		}
		if (this.PrimerSprite.enabled && GameStatsManager.Instance.GetFlag(GungeonFlags.BOSSKILLED_HIGHDRAGUN))
		{
			this.PrimerSprite.GetComponent<tk2dBaseSprite>().SetSprite("statue_of_time_shield_gold_001");
		}
		if (this.CasingSprite.enabled && GameStatsManager.Instance.GetFlag(GungeonFlags.BOSSKILLED_HIGHDRAGUN))
		{
			this.CasingSprite.GetComponent<tk2dBaseSprite>().SetSprite("statue_of_time_shell_gold_001");
		}
		if (this.StatueSprite && GameStatsManager.Instance.GetFlag(GungeonFlags.BOSSKILLED_HIGHDRAGUN))
		{
			this.StatueSprite.GetComponent<tk2dBaseSprite>().SetSprite("statue_of_time_dragun_001");
			Transform transform = this.StatueSprite.transform.Find("shadow");
			if (transform)
			{
				transform.GetComponent<tk2dBaseSprite>().SetSprite("statue_of_time_dragun_shadow_001");
			}
		}
	}

	// Token: 0x06004EF5 RID: 20213 RVA: 0x001B56E4 File Offset: 0x001B38E4
	private IEnumerator Start()
	{
		yield return null;
		while (Dungeon.IsGenerating || GameManager.Instance.IsLoadingLevel)
		{
			yield return null;
		}
		RenderSettings.ambientIntensity = 1f;
		this.CheckHeroStatue();
		GameManager.IsReturningToBreach = false;
		this.OnPlayerCharacterChanged = (Action<PlayerController>)Delegate.Combine(this.OnPlayerCharacterChanged, new Action<PlayerController>(this.ToggleTutorialBlocker));
		this.OnCoopModeChanged = (Action)Delegate.Combine(this.OnCoopModeChanged, new Action(delegate
		{
			this.ToggleTutorialBlocker(null);
		}));
		this.OnCoopModeChanged = (Action)Delegate.Combine(this.OnCoopModeChanged, new Action(GameUIRoot.Instance.PauseMenuPanel.GetComponent<PauseMenuController>().ToggleExitCoopButtonOnCoopChange));
		AmmonomiconController.EnsureExistence();
		if (Foyer.DoIntroSequence || Foyer.DoMainMenu)
		{
			BraveCameraUtility.OverrideAspect = new float?(1.7777778f);
		}
		if (Foyer.DoIntroSequence)
		{
			GameManager.Instance.IsSelectingCharacter = true;
			yield return base.StartCoroutine(this.HandleIntroSequence());
			this.IntroDoer.transform.parent.gameObject.SetActive(false);
		}
		else
		{
			this.IntroDoer = UnityEngine.Object.FindObjectOfType<FinalIntroSequenceManager>();
			if (this.IntroDoer != null)
			{
				this.IntroDoer.transform.parent.gameObject.SetActive(false);
			}
		}
		Foyer.DoIntroSequence = false;
		if (Foyer.DoMainMenu)
		{
			AkSoundEngine.PostEvent("Play_MUS_title_theme_01", base.gameObject);
			yield return base.StartCoroutine(this.HandleMainMenu());
		}
		else
		{
			MainMenuFoyerController mmfc = UnityEngine.Object.FindObjectOfType<MainMenuFoyerController>();
			if (mmfc)
			{
				mmfc.DisableMainMenu();
			}
			TitleDioramaController tdc = UnityEngine.Object.FindObjectOfType<TitleDioramaController>();
			if (tdc)
			{
				tdc.ForceHideFadeQuad();
			}
			this.ToggleTutorialBlocker(GameManager.Instance.PrimaryPlayer);
			yield return null;
			Pixelator.Instance.FadeToBlack(0.125f, true, 0.05f);
		}
		while (GameManager.Instance.IsLoadingLevel)
		{
			yield return null;
		}
		bool didCharacterSelect = true;
		GameManager.Instance.MainCameraController.OverrideZoomScale = 1f;
		GameManager.Instance.MainCameraController.CurrentZoomScale = 1f;
		GameManager.Instance.DungeonMusicController.ResetForNewFloor(GameManager.Instance.Dungeon);
		if (GameManager.Instance.PrimaryPlayer == null)
		{
			base.StartCoroutine(this.HandleCharacterSelect());
		}
		else
		{
			didCharacterSelect = false;
			this.SetUpCharacterCallbacks();
			this.DisableActiveCharacterSelectCharacter();
			GameManager.Instance.IsSelectingCharacter = false;
		}
		for (int i = 0; i < GameManager.Instance.AllPlayers.Length; i++)
		{
			this.ProcessPlayerEnteredFoyer(GameManager.Instance.AllPlayers[i]);
		}
		Component[] ixables = base.GetComponentsInChildren<Component>();
		for (int j = 0; j < ixables.Length; j++)
		{
			if (ixables[j] is IPlayerInteractable && !(ixables[j] is PickupObject))
			{
				RoomHandler.unassignedInteractableObjects.Add(ixables[j] as IPlayerInteractable);
			}
		}
		yield return null;
		if (!didCharacterSelect)
		{
			ShadowSystem.ForceAllLightsUpdate();
		}
		this.FlagPitSRBsAsUnpathableCells();
		yield break;
	}

	// Token: 0x06004EF6 RID: 20214 RVA: 0x001B5700 File Offset: 0x001B3900
	private void ToggleTutorialBlocker(PlayerController player)
	{
		bool flag = false;
		if (player != null)
		{
			flag = player.characterIdentity == PlayableCharacters.Convict || player.characterIdentity == PlayableCharacters.Guide || player.characterIdentity == PlayableCharacters.Pilot || player.characterIdentity == PlayableCharacters.Soldier;
		}
		if (GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER)
		{
			flag = false;
		}
		if (flag)
		{
			this.TutorialBlocker.gameObject.SetActive(false);
			this.TutorialBlocker.enabled = false;
		}
		else
		{
			this.TutorialBlocker.enabled = true;
			this.TutorialBlocker.gameObject.SetActive(true);
		}
	}

	// Token: 0x06004EF7 RID: 20215 RVA: 0x001B57A4 File Offset: 0x001B39A4
	private IEnumerator HandleIntroSequence()
	{
		yield return null;
		this.IntroDoer = UnityEngine.Object.FindObjectOfType<FinalIntroSequenceManager>();
		if (this.IntroDoer != null)
		{
			this.IntroDoer.TriggerSequence();
			while (this.IntroDoer.IsDoingIntro)
			{
				yield return null;
			}
		}
		yield break;
	}

	// Token: 0x06004EF8 RID: 20216 RVA: 0x001B57C0 File Offset: 0x001B39C0
	private IEnumerator HandleMainMenu()
	{
		MainMenuFoyerController mmfc = UnityEngine.Object.FindObjectOfType<MainMenuFoyerController>();
		if (mmfc)
		{
			mmfc.InitializeMainMenu();
		}
		GameUIRoot.Instance.Manager.RenderCamera.enabled = false;
		GameManager.Instance.IsSelectingCharacter = true;
		while (Foyer.DoMainMenu)
		{
			yield return null;
			if (Foyer.DoMainMenu)
			{
				mmfc.NewGameButton.GUIManager.RenderCamera.enabled = true;
			}
		}
		GameUIRoot.Instance.Manager.RenderCamera.enabled = true;
		yield break;
	}

	// Token: 0x06004EF9 RID: 20217 RVA: 0x001B57D4 File Offset: 0x001B39D4
	private void DisableActiveCharacterSelectCharacter()
	{
		FoyerCharacterSelectFlag[] array = UnityEngine.Object.FindObjectsOfType<FoyerCharacterSelectFlag>();
		List<FoyerCharacterSelectFlag> list = new List<FoyerCharacterSelectFlag>();
		while (list.Count < array.Length)
		{
			FoyerCharacterSelectFlag foyerCharacterSelectFlag = null;
			for (int i = 0; i < array.Length; i++)
			{
				if (!list.Contains(array[i]))
				{
					if (foyerCharacterSelectFlag == null)
					{
						foyerCharacterSelectFlag = array[i];
					}
					else
					{
						foyerCharacterSelectFlag = ((foyerCharacterSelectFlag.transform.position.x >= array[i].transform.position.x) ? array[i] : foyerCharacterSelectFlag);
					}
				}
			}
			list.Add(foyerCharacterSelectFlag);
		}
		for (int j = 0; j < list.Count; j++)
		{
			if (list[j].IsCoopCharacter)
			{
				list.RemoveAt(j);
				j--;
			}
			else if (!list[j].PrerequisitesFulfilled())
			{
				list.RemoveAt(j);
				j--;
			}
		}
		for (int k = 0; k < list.Count; k++)
		{
			list[k].OnSelectedCharacterCallback(GameManager.Instance.PrimaryPlayer);
		}
	}

	// Token: 0x06004EFA RID: 20218 RVA: 0x001B5918 File Offset: 0x001B3B18
	private List<FoyerCharacterSelectFlag> SetUpCharacterCallbacks()
	{
		FoyerCharacterSelectFlag[] array = UnityEngine.Object.FindObjectsOfType<FoyerCharacterSelectFlag>();
		List<FoyerCharacterSelectFlag> list = new List<FoyerCharacterSelectFlag>();
		while (list.Count < array.Length)
		{
			FoyerCharacterSelectFlag foyerCharacterSelectFlag = null;
			for (int i = 0; i < array.Length; i++)
			{
				if (!list.Contains(array[i]))
				{
					if (foyerCharacterSelectFlag == null)
					{
						foyerCharacterSelectFlag = array[i];
					}
					else
					{
						foyerCharacterSelectFlag = ((foyerCharacterSelectFlag.transform.position.x >= array[i].transform.position.x) ? array[i] : foyerCharacterSelectFlag);
					}
				}
			}
			list.Add(foyerCharacterSelectFlag);
		}
		for (int j = 0; j < list.Count; j++)
		{
			if (list[j].IsCoopCharacter)
			{
				this.OnCoopModeChanged = (Action)Delegate.Combine(this.OnCoopModeChanged, new Action(list[j].OnCoopChangedCallback));
				list.RemoveAt(j);
				j--;
			}
			else if (!list[j].PrerequisitesFulfilled())
			{
				FoyerCharacterSelectFlag foyerCharacterSelectFlag2 = list[j];
				UnityEngine.Object.Destroy(foyerCharacterSelectFlag2.gameObject);
				list.RemoveAt(j);
				j--;
			}
		}
		for (int k = 0; k < list.Count; k++)
		{
			this.OnPlayerCharacterChanged = (Action<PlayerController>)Delegate.Combine(this.OnPlayerCharacterChanged, new Action<PlayerController>(list[k].OnSelectedCharacterCallback));
			tk2dBaseSprite sprite = list[k].sprite;
			sprite.usesOverrideMaterial = true;
			Renderer renderer = sprite.renderer;
			if (!renderer.material.shader.name.Contains("PlayerPalettized"))
			{
				renderer.material.shader = ShaderCache.Acquire("Brave/LitTk2dCustomFalloffTiltedCutout");
			}
		}
		return list;
	}

	// Token: 0x06004EFB RID: 20219 RVA: 0x001B5B08 File Offset: 0x001B3D08
	private IEnumerator HandleAmmonomiconLabel()
	{
		float ela = 0f;
		while (GameManager.Instance.IsSelectingCharacter)
		{
			ela += BraveTime.DeltaTime;
			int counter = 0;
			while (AmmonomiconController.Instance == null && counter < 15)
			{
				counter++;
				yield return null;
			}
			GameUIRoot.Instance.FoyerAmmonomiconLabel.IsVisible = !AmmonomiconController.Instance.IsOpen && !GameManager.Instance.IsPaused;
			GameUIRoot.Instance.FoyerAmmonomiconLabel.Opacity = Mathf.Clamp01(ela);
			string targetLabelString = GameUIRoot.Instance.FoyerAmmonomiconLabel.ForceGetLocalizedValue("#OPTIONS_INVENTORY") + " (";
			if (BraveInput.PlayerlessInstance.IsKeyboardAndMouse(false))
			{
				targetLabelString += StringTableManager.GetBindingText(GungeonActions.GungeonActionType.EquipmentMenu);
			}
			else if (BraveInput.PlayerOneCurrentSymbology == GameOptions.ControllerSymbology.PS4)
			{
				targetLabelString += UIControllerButtonHelper.GetUnifiedControllerButtonTag(InputControlType.TouchPadButton, BraveInput.PlayerOneCurrentSymbology, null);
			}
			else if (Application.platform == RuntimePlatform.XboxOne || Application.platform == RuntimePlatform.MetroPlayerX64 || Application.platform == RuntimePlatform.MetroPlayerX86)
			{
				targetLabelString += UIControllerButtonHelper.GetUnifiedControllerButtonTag(InputControlType.Select, BraveInput.PlayerOneCurrentSymbology, null);
			}
			else
			{
				targetLabelString += UIControllerButtonHelper.GetUnifiedControllerButtonTag(InputControlType.Pause, BraveInput.PlayerOneCurrentSymbology, null);
			}
			targetLabelString += "\t)";
			GameUIRoot.Instance.FoyerAmmonomiconLabel.TabSize = 1;
			GameUIRoot.Instance.FoyerAmmonomiconLabel.ProcessMarkup = true;
			GameUIRoot.Instance.FoyerAmmonomiconLabel.Text = targetLabelString;
			yield return null;
		}
		ela = 0f;
		while (ela < 0.5f)
		{
			ela += BraveTime.DeltaTime;
			GameUIRoot.Instance.FoyerAmmonomiconLabel.Opacity = 1f - ela / 0.5f;
			yield return null;
		}
		yield break;
	}

	// Token: 0x06004EFC RID: 20220 RVA: 0x001B5B1C File Offset: 0x001B3D1C
	private IEnumerator HandleCharacterSelect()
	{
		GameManager.Instance.IsSelectingCharacter = true;
		base.StartCoroutine(this.HandleAmmonomiconLabel());
		GameManager.Instance.Dungeon.data.Entrance.visibility = RoomHandler.VisibilityStatus.CURRENT;
		Pixelator.Instance.ProcessOcclusionChange(IntVector2.Zero, 1f, GameManager.Instance.Dungeon.data.Entrance, false);
		yield return null;
		GameManager.Instance.Dungeon.data.Entrance.visibility = RoomHandler.VisibilityStatus.CURRENT;
		Pixelator.Instance.ProcessOcclusionChange(IntVector2.Zero, 1f, GameManager.Instance.Dungeon.data.Entrance, false);
		GameUIRoot.Instance.HideCoreUI(string.Empty);
		GameUIRoot.Instance.ToggleLowerPanels(false, false, string.Empty);
		GameManager.Instance.MainCameraController.SetManualControl(true, false);
		GameManager.Instance.MainCameraController.OverridePosition = new Vector3(27f, 25f, 0f);
		yield return null;
		Pixelator.Instance.SetOcclusionDirty();
		GameManager.Instance.Dungeon.data.Entrance.OnBecameVisible(null);
		List<FoyerCharacterSelectFlag> sortedByXAxis = this.SetUpCharacterCallbacks();
		bool hasSelected = false;
		int currentSelected = 0;
		int m_queuedChange = 0;
		Vector2 m_lastMousePosition = Vector2.zero;
		int m_lastMouseSelected = -1;
		FoyerCharacterSelectFlag currentlySelectedCharacter = sortedByXAxis[currentSelected];
		yield return new WaitForSeconds(0.25f);
		currentlySelectedCharacter.CreateOverheadElement();
		Action HandleShiftLeft = delegate
		{
			if (FoyerInfoPanelController.IsTransitioning)
			{
				m_queuedChange = -1;
			}
			else
			{
				currentSelected = (currentSelected - 1 + sortedByXAxis.Count) % sortedByXAxis.Count;
				AkSoundEngine.PostEvent("Play_UI_menu_select_01", this.gameObject);
			}
		};
		Action HandleShiftRight = delegate
		{
			if (FoyerInfoPanelController.IsTransitioning)
			{
				m_queuedChange = 1;
			}
			else
			{
				currentSelected = (currentSelected + 1 + sortedByXAxis.Count) % sortedByXAxis.Count;
				AkSoundEngine.PostEvent("Play_UI_menu_select_01", this.gameObject);
			}
		};
		Action HandleSelect = delegate
		{
			if (hasSelected || GameManager.Instance.PrimaryPlayer != null)
			{
				return;
			}
			this.CurrentSelectedCharacterFlag = null;
			hasSelected = true;
			AkSoundEngine.PostEvent("Play_UI_menu_characterselect_01", this.gameObject);
			CharacterSelectIdleDoer component = sortedByXAxis[currentSelected].GetComponent<CharacterSelectIdleDoer>();
			component.enabled = false;
			float num4 = 0.25f;
			if (!component.IsEevee && !sortedByXAxis[currentSelected].IsAlternateCostume && component != null && !string.IsNullOrEmpty(component.onSelectedAnimation))
			{
				tk2dSpriteAnimationClip clipByName = component.spriteAnimator.GetClipByName(component.onSelectedAnimation);
				if (clipByName != null)
				{
					num4 = (float)clipByName.frames.Length / clipByName.fps;
					component.spriteAnimator.Play(clipByName);
				}
				else
				{
					num4 = 1f;
				}
			}
			else if (component.IsEevee)
			{
				num4 = 1f;
			}
			this.StartCoroutine(this.OnSelectedCharacter(num4, sortedByXAxis[currentSelected]));
		};
		bool pauseMenuWasJustOpen = false;
		while (!hasSelected)
		{
			GameManager.Instance.MainCameraController.OverrideZoomScale = 1f;
			GameManager.Instance.MainCameraController.CurrentZoomScale = 1f;
			GungeonActions activeActions = BraveInput.GetInstanceForPlayer(0).ActiveActions;
			if (GameManager.Instance.IsPaused)
			{
				pauseMenuWasJustOpen = true;
				sortedByXAxis[currentSelected].ToggleOverheadElementVisibility(false);
				yield return null;
			}
			else if (AmmonomiconController.Instance && AmmonomiconController.Instance.IsOpen)
			{
				pauseMenuWasJustOpen = true;
				if (activeActions.EquipmentMenuAction.WasPressed || activeActions.PauseAction.WasPressed)
				{
					AmmonomiconController.Instance.CloseAmmonomicon(false);
				}
				yield return null;
			}
			else
			{
				if (currentSelected >= 0 && currentSelected < sortedByXAxis.Count)
				{
					this.CurrentSelectedCharacterFlag = sortedByXAxis[currentSelected];
				}
				else
				{
					this.CurrentSelectedCharacterFlag = null;
				}
				int cachedSelected = currentSelected;
				if (activeActions.EquipmentMenuAction.WasPressed)
				{
					AmmonomiconController.Instance.OpenAmmonomicon(false, false);
					yield return null;
				}
				else
				{
					if (activeActions.SelectLeft.WasPressedAsDpadRepeating)
					{
						HandleShiftLeft();
					}
					if (activeActions.SelectRight.WasPressedAsDpadRepeating)
					{
						HandleShiftRight();
					}
					if (activeActions.MenuSelectAction.WasPressed || activeActions.InteractAction.WasPressed || Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Space))
					{
						bool flag = this.CurrentSelectedCharacterFlag.CanBeSelected();
						if (!pauseMenuWasJustOpen && flag)
						{
							HandleSelect();
						}
						else if (!flag)
						{
							AkSoundEngine.PostEvent("Play_UI_menu_cancel_01", base.gameObject);
						}
					}
					Vector2 mouseDelta = Input.mousePosition.XY() - m_lastMousePosition;
					m_lastMousePosition = Input.mousePosition.XY();
					if (mouseDelta.magnitude > 2f)
					{
						int num = -1;
						float num2 = float.MaxValue;
						Vector2 vector = GameManager.Instance.MainCameraController.Camera.ScreenToWorldPoint(Input.mousePosition).XY();
						for (int i = 0; i < sortedByXAxis.Count; i++)
						{
							tk2dBaseSprite sprite = sortedByXAxis[i].GetComponent<CharacterSelectIdleDoer>().sprite;
							float num3 = Vector2.Distance(vector, sprite.WorldCenter);
							if (num3 < num2 && num3 < 1.5f)
							{
								num2 = num3;
								num = i;
							}
						}
						if (!FoyerInfoPanelController.IsTransitioning)
						{
							if (num != -1 && num != currentSelected)
							{
								currentSelected = num;
								AkSoundEngine.PostEvent("Play_UI_menu_select_01", base.gameObject);
							}
							m_lastMouseSelected = num;
						}
					}
					if (Input.GetMouseButtonDown(0) && m_lastMouseSelected != -1)
					{
						currentSelected = m_lastMouseSelected;
						if (currentSelected >= 0 && currentSelected < sortedByXAxis.Count && sortedByXAxis[currentSelected].CanBeSelected())
						{
							HandleSelect();
						}
						else
						{
							AkSoundEngine.PostEvent("Play_UI_menu_cancel_01", base.gameObject);
						}
					}
					if (m_queuedChange != 0 && !FoyerInfoPanelController.IsTransitioning)
					{
						if (cachedSelected == currentSelected)
						{
							currentSelected = (currentSelected + m_queuedChange + sortedByXAxis.Count) % sortedByXAxis.Count;
							AkSoundEngine.PostEvent("Play_UI_menu_select_01", base.gameObject);
						}
						m_queuedChange = 0;
					}
					if (cachedSelected != currentSelected)
					{
						sortedByXAxis[currentSelected].CreateOverheadElement();
					}
					else
					{
						sortedByXAxis[currentSelected].ToggleOverheadElementVisibility(true);
					}
					if (Time.frameCount % 5 == 0 && Time.timeSinceLevelLoad < 15f)
					{
						Pixelator.Instance.ProcessOcclusionChange(IntVector2.Zero, 1f, GameManager.Instance.Dungeon.data.Entrance, false);
						Pixelator.Instance.SetOcclusionDirty();
					}
					yield return null;
					pauseMenuWasJustOpen = false;
				}
			}
		}
		yield break;
	}

	// Token: 0x06004EFD RID: 20221 RVA: 0x001B5B38 File Offset: 0x001B3D38
	public IEnumerator OnSelectedCharacter(float delayTime, FoyerCharacterSelectFlag flag)
	{
		Foyer.IsCurrentlyPlayingCharacterSelect = true;
		GameManager.Instance.MainCameraController.OverrideRecoverySpeed = 3f;
		float ela = 0f;
		Vector2 startCamPos = GameManager.Instance.MainCameraController.OverridePosition;
		while (ela < delayTime)
		{
			ela += GameManager.INVARIANT_DELTA_TIME;
			float t = Mathf.SmoothStep(0f, 1f, ela / delayTime);
			GameManager.Instance.MainCameraController.OverridePosition = Vector2.Lerp(startCamPos, flag.transform.position.XY(), t);
			yield return null;
		}
		GameManager.PlayerPrefabForNewGame = (GameObject)BraveResources.Load(flag.CharacterPrefabPath, ".prefab");
		PlayerController playerController = GameManager.PlayerPrefabForNewGame.GetComponent<PlayerController>();
		GameStatsManager.Instance.BeginNewSession(playerController);
		PlayerController extantPlayer = null;
		if (extantPlayer == null)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(GameManager.PlayerPrefabForNewGame, flag.transform.position, Quaternion.identity);
			GameManager.PlayerPrefabForNewGame = null;
			gameObject.SetActive(true);
			extantPlayer = gameObject.GetComponent<PlayerController>();
		}
		extantPlayer.PlayerIDX = 0;
		GameManager.Instance.PrimaryPlayer = extantPlayer;
		if (flag.IsAlternateCostume)
		{
			extantPlayer.SwapToAlternateCostume(null);
		}
		this.PlayerCharacterChanged(extantPlayer);
		Foyer.IsCurrentlyPlayingCharacterSelect = false;
		GameManager.Instance.IsSelectingCharacter = false;
		GameUIRoot.Instance.ShowCoreUI(string.Empty);
		GameManager.Instance.MainCameraController.OverrideRecoverySpeed = 3f;
		GameManager.Instance.MainCameraController.SetManualControl(false, true);
		base.StartCoroutine(this.HandleInputDelay(extantPlayer, 0.33f));
		yield return null;
		PhysicsEngine.Instance.RegisterOverlappingGhostCollisionExceptions(extantPlayer.specRigidbody, null, false);
		yield break;
	}

	// Token: 0x06004EFE RID: 20222 RVA: 0x001B5B64 File Offset: 0x001B3D64
	private IEnumerator HandleInputDelay(PlayerController p, float d)
	{
		p.SetInputOverride("extra foyer delay");
		yield return new WaitForSeconds(d);
		p.ClearInputOverride("extra foyer delay");
		yield break;
	}

	// Token: 0x06004EFF RID: 20223 RVA: 0x001B5B88 File Offset: 0x001B3D88
	public void PlayerCharacterChanged(PlayerController newCharacter)
	{
		if (this.OnPlayerCharacterChanged != null)
		{
			this.OnPlayerCharacterChanged(newCharacter);
		}
	}

	// Token: 0x06004F00 RID: 20224 RVA: 0x001B5BA4 File Offset: 0x001B3DA4
	public void ProcessPlayerEnteredFoyer(PlayerController p)
	{
		if (Dungeon.ShouldAttemptToLoadFromMidgameSave && GameManager.Instance.IsLoadingLevel)
		{
			return;
		}
		if (p)
		{
			p.ForceStaticFaceDirection(Vector2.up);
			if (p.characterIdentity != PlayableCharacters.Eevee)
			{
				p.SetOverrideShader(ShaderCache.Acquire("Brave/LitTk2dCustomFalloffTiltedCutout"));
			}
			if (p.CurrentGun != null)
			{
				p.CurrentGun.gameObject.SetActive(false);
			}
			if (p.inventory != null)
			{
				p.inventory.ForceNoGun = true;
			}
			p.ProcessHandAttachment();
		}
	}

	// Token: 0x06004F01 RID: 20225 RVA: 0x001B5C40 File Offset: 0x001B3E40
	public void OnDepartedFoyer()
	{
		GameManager.Instance.IsFoyer = false;
		for (int i = 0; i < GameManager.Instance.AllPlayers.Length; i++)
		{
			GameManager.Instance.AllPlayers[i].inventory.ForceNoGun = false;
			GameManager.Instance.AllPlayers[i].CurrentGun.gameObject.SetActive(true);
			GameManager.Instance.AllPlayers[i].ProcessHandAttachment();
			GameManager.Instance.AllPlayers[i].ClearOverrideShader();
			GameManager.Instance.AllPlayers[i].AlternateCostumeLibrary = null;
		}
	}

	// Token: 0x06004F02 RID: 20226 RVA: 0x001B5CDC File Offset: 0x001B3EDC
	private void PlacePlayerAtStart(PlayerController extantPlayer, Vector2 spot)
	{
		Vector3 vector = new Vector3(spot.x + 0.5f, spot.y + 0.5f, -0.1f);
		extantPlayer.transform.position = vector;
		extantPlayer.Reinitialize();
	}

	// Token: 0x06004F03 RID: 20227 RVA: 0x001B5D24 File Offset: 0x001B3F24
	private void FlagPitSRBsAsUnpathableCells()
	{
		RoomHandler entrance = GameManager.Instance.Dungeon.data.Entrance;
		for (int i = entrance.area.basePosition.x; i < entrance.area.basePosition.x + entrance.area.dimensions.x; i++)
		{
			for (int j = entrance.area.basePosition.y; j < entrance.area.basePosition.y + entrance.area.dimensions.y; j++)
			{
				for (int k = 0; k < DebrisObject.SRB_Pits.Count; k++)
				{
					Vector2 vector = new Vector2((float)i + 0.5f, (float)j + 0.5f);
					if (DebrisObject.SRB_Pits[k].ContainsPoint(vector, 2147483647, true))
					{
						GameManager.Instance.Dungeon.data[i, j].isOccupied = true;
					}
				}
				for (int l = 0; l < DebrisObject.SRB_Walls.Count; l++)
				{
					Vector2 vector2 = new Vector2((float)i + 0.5f, (float)j + 0.5f);
					if (DebrisObject.SRB_Walls[l].ContainsPoint(vector2, 2147483647, true))
					{
						GameManager.Instance.Dungeon.data[i, j].isOccupied = true;
					}
				}
			}
		}
	}

	// Token: 0x0400462A RID: 17962
	public static bool DoIntroSequence = true;

	// Token: 0x0400462B RID: 17963
	public static bool DoMainMenu = true;

	// Token: 0x0400462C RID: 17964
	private static Foyer m_instance;

	// Token: 0x0400462D RID: 17965
	public SpeculativeRigidbody TutorialBlocker;

	// Token: 0x0400462E RID: 17966
	public FinalIntroSequenceManager IntroDoer;

	// Token: 0x0400462F RID: 17967
	public Action<PlayerController> OnPlayerCharacterChanged;

	// Token: 0x04004630 RID: 17968
	public Action OnCoopModeChanged;

	// Token: 0x04004631 RID: 17969
	public Renderer PrimerSprite;

	// Token: 0x04004632 RID: 17970
	public Renderer PowderSprite;

	// Token: 0x04004633 RID: 17971
	public Renderer SlugSprite;

	// Token: 0x04004634 RID: 17972
	public Renderer CasingSprite;

	// Token: 0x04004635 RID: 17973
	public Renderer StatueSprite;

	// Token: 0x04004636 RID: 17974
	public FoyerCharacterSelectFlag CurrentSelectedCharacterFlag;

	// Token: 0x04004637 RID: 17975
	public static bool IsCurrentlyPlayingCharacterSelect;
}
