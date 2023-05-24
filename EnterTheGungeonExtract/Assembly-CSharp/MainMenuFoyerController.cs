using System;
using System.Collections;
using System.Collections.Generic;
using Dungeonator;
using InControl;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x020017BB RID: 6075
public class MainMenuFoyerController : MonoBehaviour
{
	// Token: 0x06008E32 RID: 36402 RVA: 0x003BC7E0 File Offset: 0x003BA9E0
	private void Awake()
	{
		this.m_guiManager = base.GetComponent<dfGUIManager>();
		this.NewGameButton.forceUpperCase = true;
		this.ControlsButton.forceUpperCase = true;
		this.XboxLiveButton.forceUpperCase = true;
		this.QuitGameButton.forceUpperCase = true;
		this.ContinueGameButton.forceUpperCase = true;
		this.NewGameButton.ModifyLocalizedText(this.NewGameButton.Text.ToUpperInvariant());
		this.ControlsButton.ModifyLocalizedText(this.ControlsButton.Text.ToUpperInvariant());
		this.XboxLiveButton.ModifyLocalizedText(this.XboxLiveButton.Text.ToUpperInvariant());
		this.QuitGameButton.ModifyLocalizedText(this.QuitGameButton.Text.ToUpperInvariant());
		List<dfButton> list = new List<dfButton>();
		list.Add(this.ContinueGameButton);
		if (GameManager.HasValidMidgameSave())
		{
			this.ContinueGameButton.IsEnabled = true;
			this.ContinueGameButton.IsVisible = true;
		}
		else
		{
			this.ContinueGameButton.IsEnabled = false;
			this.ContinueGameButton.IsVisible = false;
		}
		list.Add(this.NewGameButton);
		list.Add(this.ControlsButton);
		this.XboxLiveButton.IsEnabled = false;
		this.XboxLiveButton.IsVisible = false;
		list.Add(this.QuitGameButton);
		int count = list.Count;
		if (count > 0)
		{
			dfButton dfButton = list[count - 1];
			for (int i = 0; i < list.Count; i++)
			{
				dfButton dfButton2 = list[i];
				dfButton2.GetComponent<UIKeyControls>().up = dfButton;
				dfButton.GetComponent<UIKeyControls>().down = dfButton2;
				dfButton = dfButton2;
			}
		}
		this.FixButtonPositions();
		if (!Foyer.DoMainMenu)
		{
			this.NewGameButton.GUIManager.RenderCamera.enabled = false;
			AkSoundEngine.PostEvent("Play_MUS_State_Reset", base.gameObject);
		}
		this.VersionLabel.Text = VersionManager.DisplayVersionNumber;
	}

	// Token: 0x06008E33 RID: 36403 RVA: 0x003BC9CC File Offset: 0x003BABCC
	private void FixButtonPositions()
	{
		this.NewGameButton.RelativePosition = this.NewGameButton.RelativePosition.WithX(this.QuitGameButton.RelativePosition.x).WithY(this.QuitGameButton.RelativePosition.y - 153f);
		this.ControlsButton.RelativePosition = this.ControlsButton.RelativePosition.WithX(this.QuitGameButton.RelativePosition.x).WithY(this.QuitGameButton.RelativePosition.y - 102f);
		this.XboxLiveButton.RelativePosition = this.XboxLiveButton.RelativePosition.WithX(this.QuitGameButton.RelativePosition.x).WithY(this.QuitGameButton.RelativePosition.y - 51f);
		this.ContinueGameButton.RelativePosition = this.ContinueGameButton.RelativePosition.WithX(this.QuitGameButton.RelativePosition.x).WithY(this.QuitGameButton.RelativePosition.y - 204f);
		if (!this.XboxLiveButton.IsEnabled)
		{
			this.ContinueGameButton.RelativePosition += new Vector3(0f, 51f, 0f);
			this.NewGameButton.RelativePosition += new Vector3(0f, 51f, 0f);
			this.ControlsButton.RelativePosition += new Vector3(0f, 51f, 0f);
		}
	}

	// Token: 0x06008E34 RID: 36404 RVA: 0x003BCB9C File Offset: 0x003BAD9C
	public void InitializeMainMenu()
	{
		GameManager.Instance.TargetQuickRestartLevel = -1;
		GameUIRoot.Instance.Manager.RenderCamera.enabled = false;
		this.FixButtonPositions();
		if (!this.Initialized)
		{
			this.NewGameButton.GotFocus += this.PlayFocusNoise;
			this.ControlsButton.GotFocus += this.PlayFocusNoise;
			this.XboxLiveButton.GotFocus += this.PlayFocusNoise;
			this.QuitGameButton.GotFocus += this.PlayFocusNoise;
			this.ContinueGameButton.GotFocus += this.PlayFocusNoise;
			this.NewGameButton.Click += this.OnNewGameSelected;
			this.ContinueGameButton.Click += this.OnContinueGameSelected;
			if (GameManager.HasValidMidgameSave())
			{
				this.ContinueGameButton.Focus(true);
			}
			this.ControlsButton.Click += this.ShowOptionsPanel;
			this.XboxLiveButton.Click += this.SignInToPlatform;
			this.QuitGameButton.Click += this.Quit;
			this.Initialized = true;
		}
		if (Time.timeScale != 1f)
		{
			BraveTime.ClearAllMultipliers();
		}
	}

	// Token: 0x06008E35 RID: 36405 RVA: 0x003BCCF4 File Offset: 0x003BAEF4
	private void PlayFocusNoise(dfControl control, dfFocusEventArgs args)
	{
		if (!Foyer.DoMainMenu)
		{
			return;
		}
		AkSoundEngine.PostEvent("Play_UI_menu_select_01", GameManager.Instance.gameObject);
	}

	// Token: 0x06008E36 RID: 36406 RVA: 0x003BCD18 File Offset: 0x003BAF18
	public void UpdateMainMenuText()
	{
		if (GameManager.HasValidMidgameSave())
		{
			this.ContinueGameButton.IsEnabled = true;
			this.ContinueGameButton.IsVisible = true;
		}
		else
		{
			this.ContinueGameButton.IsEnabled = false;
			this.ContinueGameButton.IsVisible = false;
		}
	}

	// Token: 0x06008E37 RID: 36407 RVA: 0x003BCD64 File Offset: 0x003BAF64
	public void DisableMainMenu()
	{
		BraveCameraUtility.OverrideAspect = null;
		GameUIRoot.Instance.Manager.RenderCamera.enabled = true;
		this.NewGameButton.GUIManager.RenderCamera.enabled = false;
		this.NewGameButton.GUIManager.enabled = false;
		this.NewGameButton.Click -= this.OnNewGameSelected;
		this.ControlsButton.Click -= this.ShowOptionsPanel;
		this.XboxLiveButton.Click -= this.SignInToPlatform;
		this.QuitGameButton.Click -= this.Quit;
		this.NewGameButton.IsInteractive = false;
		this.ControlsButton.IsInteractive = false;
		this.XboxLiveButton.IsInteractive = false;
		this.QuitGameButton.IsInteractive = false;
		if (this.NewGameButton && this.NewGameButton.GetComponent<UIKeyControls>())
		{
			this.NewGameButton.GetComponent<UIKeyControls>().enabled = false;
		}
		if (this.ControlsButton && this.ControlsButton.GetComponent<UIKeyControls>())
		{
			this.ControlsButton.GetComponent<UIKeyControls>().enabled = false;
		}
		if (this.XboxLiveButton && this.XboxLiveButton.GetComponent<UIKeyControls>())
		{
			this.XboxLiveButton.GetComponent<UIKeyControls>().enabled = false;
		}
		if (this.QuitGameButton && this.QuitGameButton.GetComponent<UIKeyControls>())
		{
			this.QuitGameButton.GetComponent<UIKeyControls>().enabled = false;
		}
		ShadowSystem.ForceAllLightsUpdate();
	}

	// Token: 0x06008E38 RID: 36408 RVA: 0x003BCF24 File Offset: 0x003BB124
	private void NewGameInternal()
	{
		this.DisableMainMenu();
		Pixelator.Instance.FadeToBlack(0.15f, true, 0.05f);
		GameManager.Instance.FlushAudio();
		Foyer.DoIntroSequence = false;
		Foyer.DoMainMenu = false;
		AkSoundEngine.PostEvent("Play_UI_menu_confirm_01", base.gameObject);
	}

	// Token: 0x06008E39 RID: 36409 RVA: 0x003BCF74 File Offset: 0x003BB174
	private bool IsDioramaRevealed(bool doReveal = false)
	{
		if (this.m_tdc == null)
		{
			this.m_tdc = UnityEngine.Object.FindObjectOfType<TitleDioramaController>();
		}
		return !this.m_tdc || this.m_tdc.IsRevealed(doReveal);
	}

	// Token: 0x06008E3A RID: 36410 RVA: 0x003BCFB0 File Offset: 0x003BB1B0
	private void OnContinueGameSelected(dfControl control, dfMouseEventArgs mouseEvent)
	{
		MidGameSaveData.ContinuePressedDevice = InputManager.ActiveDevice;
		if (this.m_faded || this.m_wasFadedThisFrame)
		{
			return;
		}
		if (!this.IsDioramaRevealed(true))
		{
			return;
		}
		if (!Foyer.DoMainMenu)
		{
			return;
		}
		MidGameSaveData midGameSaveData = null;
		GameManager.VerifyAndLoadMidgameSave(out midGameSaveData, true);
		Dungeon.ShouldAttemptToLoadFromMidgameSave = true;
		this.DisableMainMenu();
		Pixelator.Instance.FadeToBlack(0.15f, false, 0.05f);
		GameManager.Instance.FlushAudio();
		AkSoundEngine.PostEvent("Play_UI_menu_confirm_01", base.gameObject);
		GameManager.Instance.SetNextLevelIndex(GameManager.Instance.GetTargetLevelIndexFromSavedTileset(midGameSaveData.levelSaved));
		GameManager.Instance.GeneratePlayersFromMidGameSave(midGameSaveData);
		GameManager.Instance.IsFoyer = false;
		Foyer.DoIntroSequence = false;
		Foyer.DoMainMenu = false;
		GameManager.Instance.IsSelectingCharacter = false;
		GameManager.Instance.DelayedLoadMidgameSave(0.25f, midGameSaveData);
	}

	// Token: 0x06008E3B RID: 36411 RVA: 0x003BD094 File Offset: 0x003BB294
	private void OnNewGameSelected(dfControl control, dfMouseEventArgs mouseEvent)
	{
		if (this.m_faded || this.m_wasFadedThisFrame)
		{
			return;
		}
		if (!this.IsDioramaRevealed(true))
		{
			return;
		}
		GameManager.Instance.CurrentGameType = GameManager.GameType.SINGLE_PLAYER;
		this.NewGameInternal();
		GameManager.Instance.InjectedFlowPath = null;
	}

	// Token: 0x06008E3C RID: 36412 RVA: 0x003BD0E4 File Offset: 0x003BB2E4
	private IEnumerator ToggleFade(bool targetFade)
	{
		this.m_faded = targetFade;
		float ela = 0f;
		float dura = 1f;
		float startVal = ((!targetFade) ? 0f : 1f);
		float endVal = ((!targetFade) ? 1f : 0f);
		while (ela < dura)
		{
			ela += GameManager.INVARIANT_DELTA_TIME;
			float t = ela / dura;
			t = Mathf.Lerp(startVal, endVal, t);
			this.NewGameButton.Opacity = t;
			this.ControlsButton.Opacity = t;
			this.XboxLiveButton.Opacity = t;
			this.QuitGameButton.Opacity = t;
			this.ContinueGameButton.Opacity = t;
			this.VersionLabel.Opacity = t;
			this.TitleCard.Opacity = t;
			yield return null;
		}
		yield break;
	}

	// Token: 0x06008E3D RID: 36413 RVA: 0x003BD108 File Offset: 0x003BB308
	private void Update()
	{
		this.m_wasFadedThisFrame = this.m_faded;
		if (this.m_guiManager && !GameManager.Instance.IsLoadingLevel)
		{
			this.m_guiManager.UIScale = Pixelator.Instance.ScaleTileScale / 3f;
		}
		if (!Foyer.DoMainMenu && !Foyer.DoIntroSequence && !GameManager.Instance.IsSelectingCharacter && !GameManager.IsReturningToBreach)
		{
			return;
		}
		if (this.IsDioramaRevealed(false))
		{
			this.m_timeWithoutInput += GameManager.INVARIANT_DELTA_TIME;
		}
		if (Input.anyKeyDown || Input.mousePosition != this.m_cachedMousePosition)
		{
			this.m_timeWithoutInput = 0f;
		}
		this.m_cachedMousePosition = Input.mousePosition;
		if (BraveInput.PlayerlessInstance && BraveInput.PlayerlessInstance.ActiveActions != null && BraveInput.PlayerlessInstance.ActiveActions.AnyActionPressed())
		{
			this.m_timeWithoutInput = 0f;
		}
		if (GameManager.Instance.PREVENT_MAIN_MENU_TEXT)
		{
			this.NewGameButton.Opacity = 0f;
			this.ControlsButton.Opacity = 0f;
			this.XboxLiveButton.Opacity = 0f;
			this.QuitGameButton.Opacity = 0f;
			this.VersionLabel.Opacity = 0f;
			this.TitleCard.Opacity = 0f;
		}
		else if (this.m_timeWithoutInput > this.c_fadeTimer && !this.m_faded)
		{
			base.StartCoroutine(this.ToggleFade(true));
		}
		else if (this.m_timeWithoutInput < this.c_fadeTimer && this.m_faded)
		{
			base.StartCoroutine(this.ToggleFade(false));
		}
		if (Foyer.DoMainMenu && !this.m_optionsOpen && (!this.IsDioramaRevealed(false) || (!this.NewGameButton.HasFocus && !this.ControlsButton.HasFocus && !this.XboxLiveButton.HasFocus && !this.QuitGameButton.HasFocus && !this.ContinueGameButton.HasFocus)))
		{
			dfGUIManager.PopModalToControl(null, false);
			if (this.ContinueGameButton.IsEnabled && this.ContinueGameButton.IsVisible)
			{
				this.ContinueGameButton.Focus(true);
			}
			else
			{
				this.NewGameButton.Focus(true);
			}
		}
		if (this.m_optionsOpen)
		{
			if (!GameUIRoot.Instance.AreYouSurePanel.IsVisible)
			{
				if (BraveInput.PlayerlessInstance && BraveInput.PlayerlessInstance.ActiveActions != null && BraveInput.PlayerlessInstance.ActiveActions.CancelAction.WasPressed)
				{
					PauseMenuController component = GameUIRoot.Instance.PauseMenuPanel.GetComponent<PauseMenuController>();
					if (!component.OptionsMenu.ModalKeyBindingDialog.IsVisible)
					{
						this.HideOptionsPanel();
					}
				}
				else if (Input.GetKeyDown(KeyCode.Escape))
				{
					this.HideOptionsPanel();
				}
			}
		}
		if (Input.anyKeyDown && this.m_controlsPanelController != null && this.m_controlsPanelController.CanClose && !Input.GetMouseButtonDown(0))
		{
			this.HideControlsPanel();
		}
	}

	// Token: 0x06008E3E RID: 36414 RVA: 0x003BD474 File Offset: 0x003BB674
	private void SignInToPlatform(dfControl control, dfMouseEventArgs eventArg)
	{
		GameManager.Instance.platformInterface.SignIn();
	}

	// Token: 0x06008E3F RID: 36415 RVA: 0x003BD488 File Offset: 0x003BB688
	private void Quit(dfControl control, dfMouseEventArgs eventArg)
	{
		if (this.m_faded || this.m_wasFadedThisFrame)
		{
			return;
		}
		if (!this.IsDioramaRevealed(true))
		{
			return;
		}
		if (Foyer.DoMainMenu)
		{
			Application.Quit();
		}
	}

	// Token: 0x06008E40 RID: 36416 RVA: 0x003BD4C0 File Offset: 0x003BB6C0
	private void ShowOptionsPanel(dfControl control, dfMouseEventArgs eventArg)
	{
		if (this.m_faded || this.m_wasFadedThisFrame)
		{
			return;
		}
		if (!this.IsDioramaRevealed(true))
		{
			return;
		}
		if (!Foyer.DoMainMenu)
		{
			return;
		}
		this.m_optionsOpen = true;
		this.m_cachedDepth = GameUIRoot.Instance.Manager.RenderCamera.depth;
		GameUIRoot.Instance.Manager.RenderCamera.depth += 10f;
		GameUIRoot.Instance.Manager.RenderCamera.enabled = true;
		GameUIRoot.Instance.Manager.overrideClearFlags = CameraClearFlags.Color;
		GameUIRoot.Instance.Manager.RenderCamera.backgroundColor = Color.black;
		PauseMenuController component = GameUIRoot.Instance.PauseMenuPanel.GetComponent<PauseMenuController>();
		if (component != null)
		{
			component.OptionsMenu.PreOptionsMenu.IsVisible = true;
		}
	}

	// Token: 0x06008E41 RID: 36417 RVA: 0x003BD5A8 File Offset: 0x003BB7A8
	private void HideOptionsPanel()
	{
		PauseMenuController component = GameUIRoot.Instance.PauseMenuPanel.GetComponent<PauseMenuController>();
		if (component != null)
		{
			if (!GameUIRoot.Instance.AreYouSurePanel.IsVisible)
			{
				if (component.OptionsMenu.IsVisible)
				{
					component.OptionsMenu.CloseAndMaybeApplyChangesWithPrompt();
				}
				else
				{
					this.m_optionsOpen = false;
					GameUIRoot.Instance.Manager.RenderCamera.depth = this.m_cachedDepth;
					GameUIRoot.Instance.Manager.RenderCamera.enabled = false;
					GameUIRoot.Instance.Manager.overrideClearFlags = CameraClearFlags.Depth;
					if (component != null)
					{
						component.OptionsMenu.PreOptionsMenu.IsVisible = false;
					}
				}
			}
		}
		BraveInput.SavePlayerlessBindingsToOptions();
	}

	// Token: 0x06008E42 RID: 36418 RVA: 0x003BD674 File Offset: 0x003BB874
	private void ShowControlsPanel(dfControl control, dfMouseEventArgs eventArg)
	{
		if (!Foyer.DoMainMenu)
		{
			return;
		}
		if (this.m_extantControlsPanel != null)
		{
			return;
		}
		GameObject gameObject = this.TEMP_ControlsPrefab.gameObject;
		if (!BraveInput.GetInstanceForPlayer(0).IsKeyboardAndMouse(false))
		{
			gameObject = this.TEMP_ControlsSonyPrefab.gameObject;
		}
		GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(gameObject);
		this.m_extantControlsPanel = gameObject2;
		this.m_controlsPanelController = gameObject2.GetComponent<TempControlsController>();
		this.NewGameButton.GetManager().AddControl(gameObject2.GetComponent<dfSprite>());
	}

	// Token: 0x06008E43 RID: 36419 RVA: 0x003BD6F8 File Offset: 0x003BB8F8
	private void HideControlsPanel()
	{
		if (this.m_extantControlsPanel != null)
		{
			this.m_controlsPanelController = null;
			UnityEngine.Object.Destroy(this.m_extantControlsPanel);
		}
	}

	// Token: 0x0400961B RID: 38427
	public dfButton NewGameButton;

	// Token: 0x0400961C RID: 38428
	public dfButton ControlsButton;

	// Token: 0x0400961D RID: 38429
	public dfButton XboxLiveButton;

	// Token: 0x0400961E RID: 38430
	public dfButton QuitGameButton;

	// Token: 0x0400961F RID: 38431
	public dfButton ContinueGameButton;

	// Token: 0x04009620 RID: 38432
	[FormerlySerializedAs("BetaLabel")]
	public dfLabel VersionLabel;

	// Token: 0x04009621 RID: 38433
	public dfControl TitleCard;

	// Token: 0x04009622 RID: 38434
	public dfSprite TEMP_ControlsPrefab;

	// Token: 0x04009623 RID: 38435
	public dfSprite TEMP_ControlsSonyPrefab;

	// Token: 0x04009624 RID: 38436
	private GameObject m_extantControlsPanel;

	// Token: 0x04009625 RID: 38437
	private TempControlsController m_controlsPanelController;

	// Token: 0x04009626 RID: 38438
	private dfGUIManager m_guiManager;

	// Token: 0x04009627 RID: 38439
	private bool Initialized;

	// Token: 0x04009628 RID: 38440
	private TitleDioramaController m_tdc;

	// Token: 0x04009629 RID: 38441
	private float m_timeWithoutInput;

	// Token: 0x0400962A RID: 38442
	private Vector3 m_cachedMousePosition;

	// Token: 0x0400962B RID: 38443
	private bool m_faded;

	// Token: 0x0400962C RID: 38444
	private bool m_wasFadedThisFrame;

	// Token: 0x0400962D RID: 38445
	private float c_fadeTimer = 20f;

	// Token: 0x0400962E RID: 38446
	private bool m_optionsOpen;

	// Token: 0x0400962F RID: 38447
	private float m_cachedDepth = -1f;
}
