using System;
using System.Collections;
using System.Diagnostics;
using InControl;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020017B9 RID: 6073
public class MainMenuController : MonoBehaviour
{
	// Token: 0x06008E1D RID: 36381 RVA: 0x003BC2AC File Offset: 0x003BA4AC
	private void Start()
	{
		GameManager.Instance.TargetQuickRestartLevel = -1;
		PhysicsEngine.Instance = null;
		Pixelator.Instance = null;
		GameUIRoot.Instance = null;
		SpawnManager.Instance = null;
		Minimap.Instance = null;
		this.NewGameButton.Click += this.OnNewGameSelected;
		this.CoopGameButton.Click += this.OnNewCoopGameSelected;
		this.ControlsButton.Click += this.ShowControlsPanel;
		if (this.PlayVideoButton != null)
		{
			this.PlayVideoButton.Click += delegate(dfControl control, dfMouseEventArgs mouseEvent)
			{
				this.PlayWindowsMediaPlayerMovie();
			};
		}
		this.QuitGameButton.Click += this.Quit;
		if (Time.timeScale != 1f)
		{
			BraveTime.ClearAllMultipliers();
		}
	}

	// Token: 0x06008E1E RID: 36382 RVA: 0x003BC37C File Offset: 0x003BA57C
	private void OnNewCoopGameSelected(dfControl control, dfMouseEventArgs mouseEvent)
	{
		this.DoQuickStart();
	}

	// Token: 0x06008E1F RID: 36383 RVA: 0x003BC384 File Offset: 0x003BA584
	private void OnStageModeSelected(dfControl control, dfMouseEventArgs mouseEvent)
	{
		GameManager.Instance.CurrentGameType = GameManager.GameType.COOP_2_PLAYER;
		this.NewGameInternal();
	}

	// Token: 0x06008E20 RID: 36384 RVA: 0x003BC398 File Offset: 0x003BA598
	private void OnStageModeBackupSelected(dfControl control, dfMouseEventArgs mouseEvent)
	{
		GameManager.Instance.CurrentGameType = GameManager.GameType.COOP_2_PLAYER;
		this.NewGameInternal();
	}

	// Token: 0x06008E21 RID: 36385 RVA: 0x003BC3AC File Offset: 0x003BA5AC
	private void DoQuickStart()
	{
		GameManager.SKIP_FOYER = true;
		GameManager.Instance.ClearPerLevelData();
		GameManager.Instance.ClearPlayers();
		uint num = 1U;
		AkSoundEngine.LoadBank("SFX.bnk", -1, out num);
		GameManager.PlayerPrefabForNewGame = (GameObject)BraveResources.Load(CharacterSelectController.GetCharacterPathFromQuickStart(), ".prefab");
		PlayerController component = GameManager.PlayerPrefabForNewGame.GetComponent<PlayerController>();
		GameStatsManager.Instance.BeginNewSession(component);
		base.StartCoroutine(this.LerpFadeAlpha(0f, 1f, 0.15f));
		GameManager.Instance.FlushAudio();
		GameManager.Instance.GlobalInjectionData.PreprocessRun(false);
		GameManager.Instance.DelayedLoadNextLevel(0.15f);
		AkSoundEngine.PostEvent("Play_UI_menu_confirm_01", base.gameObject);
	}

	// Token: 0x06008E22 RID: 36386 RVA: 0x003BC468 File Offset: 0x003BA668
	private void NewGameInternal()
	{
		base.StartCoroutine(this.LerpFadeAlpha(0f, 1f, 0.15f));
		GameManager.Instance.DelayedLoadNextLevel(0.15f);
		AkSoundEngine.PostEvent("Play_UI_menu_confirm_01", base.gameObject);
	}

	// Token: 0x06008E23 RID: 36387 RVA: 0x003BC4A8 File Offset: 0x003BA6A8
	private void OnNewGameSelected(dfControl control, dfMouseEventArgs mouseEvent)
	{
		GameManager.Instance.CurrentGameType = GameManager.GameType.SINGLE_PLAYER;
		this.NewGameInternal();
	}

	// Token: 0x06008E24 RID: 36388 RVA: 0x003BC4BC File Offset: 0x003BA6BC
	private IEnumerator LerpFadeAlpha(float startAlpha, float targetAlpha, float duration)
	{
		float elapsed = 0f;
		Color startColor = new Color(0f, 0f, 0f, startAlpha);
		Color endColor = new Color(0f, 0f, 0f, targetAlpha);
		while (elapsed < duration)
		{
			elapsed += BraveTime.DeltaTime;
			float t = elapsed / duration;
			this.FadeImage.color = Color.Lerp(startColor, endColor, t);
			yield return null;
		}
		yield break;
	}

	// Token: 0x06008E25 RID: 36389 RVA: 0x003BC4EC File Offset: 0x003BA6EC
	private void Update()
	{
		if ((InputManager.ActiveDevice != null && InputManager.ActiveDevice.Action4.WasPressed) || Input.GetKeyDown(KeyCode.Q))
		{
			this.DoQuickStart();
		}
		if (InputManager.ActiveDevice != null && InputManager.ActiveDevice.LeftStickDown.IsPressed && InputManager.ActiveDevice.RightStickDown.WasPressed)
		{
			this.OnNewCoopGameSelected(null, null);
		}
		if (Input.anyKeyDown && this.m_controlsPanelController != null && this.m_controlsPanelController.CanClose && !Input.GetMouseButtonDown(0))
		{
			this.HideControlsPanel();
		}
	}

	// Token: 0x06008E26 RID: 36390 RVA: 0x003BC5A0 File Offset: 0x003BA7A0
	private void Quit(dfControl control, dfMouseEventArgs eventArg)
	{
		Application.Quit();
	}

	// Token: 0x06008E27 RID: 36391 RVA: 0x003BC5A8 File Offset: 0x003BA7A8
	private void PlayWindowsMediaPlayerMovie()
	{
		string text = Application.streamingAssetsPath + "/SonyVidya.mp4";
		ProcessStartInfo processStartInfo = new ProcessStartInfo("wmplayer.exe", "\"" + text + "\"");
		Process.Start(processStartInfo);
	}

	// Token: 0x06008E28 RID: 36392 RVA: 0x003BC5E8 File Offset: 0x003BA7E8
	private void ShowControlsPanel(dfControl control, dfMouseEventArgs eventArg)
	{
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

	// Token: 0x06008E29 RID: 36393 RVA: 0x003BC660 File Offset: 0x003BA860
	private void HideControlsPanel()
	{
		if (this.m_extantControlsPanel != null)
		{
			this.m_controlsPanelController = null;
			UnityEngine.Object.Destroy(this.m_extantControlsPanel);
		}
	}

	// Token: 0x04009603 RID: 38403
	public dfButton NewGameButton;

	// Token: 0x04009604 RID: 38404
	public dfButton CoopGameButton;

	// Token: 0x04009605 RID: 38405
	public dfButton NewGameDebugModeButton;

	// Token: 0x04009606 RID: 38406
	public dfButton ControlsButton;

	// Token: 0x04009607 RID: 38407
	public dfButton PlayVideoButton;

	// Token: 0x04009608 RID: 38408
	public dfButton QuitGameButton;

	// Token: 0x04009609 RID: 38409
	public dfSprite TEMP_ControlsPrefab;

	// Token: 0x0400960A RID: 38410
	public dfSprite TEMP_ControlsSonyPrefab;

	// Token: 0x0400960B RID: 38411
	public Image FadeImage;

	// Token: 0x0400960C RID: 38412
	public RawImage SizzleImage;

	// Token: 0x0400960D RID: 38413
	public AudioClip movieAudio;

	// Token: 0x0400960E RID: 38414
	private GameObject m_extantControlsPanel;

	// Token: 0x0400960F RID: 38415
	private TempControlsController m_controlsPanelController;
}
