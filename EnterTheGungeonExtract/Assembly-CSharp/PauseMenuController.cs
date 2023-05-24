using System;
using System.Collections;
using System.Collections.Generic;
using Dungeonator;
using UnityEngine;

// Token: 0x020017E1 RID: 6113
public class PauseMenuController : MonoBehaviour
{
	// Token: 0x06008FCB RID: 36811 RVA: 0x003CCFBC File Offset: 0x003CB1BC
	private void Start()
	{
		this.m_panel = base.GetComponent<dfPanel>();
		this.AdditionalMenuElementsToClear = new List<GameObject>();
		this.m_panel.IsVisibleChanged += this.OnVisibilityChanged;
		this.ExitToMainMenuButton.Click += this.DoExitToMainMenu;
		if (GameManager.Instance.CurrentLevelOverrideState == GameManager.LevelOverrideState.FOYER)
		{
			this.ExitToMainMenuButton.Text = "#EXIT_COOP";
			if (GameManager.Instance.CurrentGameType == GameManager.GameType.SINGLE_PLAYER)
			{
				this.ExitToMainMenuButton.Disable();
				UIKeyControls component = this.ExitToMainMenuButton.GetComponent<UIKeyControls>();
				component.up.GetComponent<UIKeyControls>().down = component.down;
				component.down.GetComponent<UIKeyControls>().up = component.up;
			}
		}
		this.ReturnToGameButton.Click += this.DoReturnToGame;
		this.BestiaryButton.Click += this.DoShowBestiary;
		this.QuitGameButton.Click += this.DoQuitGameEntirely;
		if (GameManager.Instance.CurrentLevelOverrideState == GameManager.LevelOverrideState.FOYER || GameManager.Instance.CurrentLevelOverrideState == GameManager.LevelOverrideState.TUTORIAL || GameManager.Instance.CurrentGameMode == GameManager.GameMode.SUPERBOSSRUSH)
		{
			this.QuickRestartButton.Disable();
			UIKeyControls component2 = this.QuickRestartButton.GetComponent<UIKeyControls>();
			component2.up.GetComponent<UIKeyControls>().down = component2.down;
			component2.down.GetComponent<UIKeyControls>().up = component2.up;
		}
		else
		{
			this.QuickRestartButton.Click += this.DoQuickRestart;
		}
		this.OptionsButton.Click += this.DoShowOptions;
		if (GameManager.Instance.CurrentLevelOverrideState == GameManager.LevelOverrideState.FOYER)
		{
			this.metaCurrencyPanel.IsVisible = true;
		}
		else
		{
			GameUIRoot.Instance.AddControlToMotionGroups(this.metaCurrencyPanel, DungeonData.Direction.EAST, true);
			GameUIRoot.Instance.MoveNonCoreGroupOnscreen(this.metaCurrencyPanel, true);
		}
	}

	// Token: 0x06008FCC RID: 36812 RVA: 0x003CD1B0 File Offset: 0x003CB3B0
	private void OnVisibilityChanged(dfControl control, bool value)
	{
		if (value)
		{
			if (GameManager.Options.CurrentLanguage == StringTableManager.GungeonSupportedLanguages.FRENCH)
			{
				this.BestiaryButton.Text = this.BestiaryButton.ForceGetLocalizedValue("#AMMONOMICON");
				this.BestiaryButton.ModifyLocalizedText(this.BestiaryButton.Text.Replace(" ", "\n"));
				this.BestiaryButton.AutoSize = false;
				this.BestiaryButton.TextAlignment = TextAlignment.Center;
				this.BestiaryButton.Width = Mathf.Max(240f, this.BestiaryButton.Width);
				if (!this.m_buttonsOffsetForDoubleHeight)
				{
					this.m_buttonsOffsetForDoubleHeight = true;
					this.BestiaryButton.RelativePosition = this.BestiaryButton.RelativePosition - new Vector3(0f, 18f, 0f);
					this.ReturnToGameButton.RelativePosition = this.ReturnToGameButton.RelativePosition - new Vector3(0f, 18f, 0f);
					this.OptionsButton.RelativePosition = this.OptionsButton.RelativePosition + new Vector3(0f, 24f, 0f);
					this.ExitToMainMenuButton.RelativePosition = this.ExitToMainMenuButton.RelativePosition + new Vector3(0f, 24f, 0f);
					this.QuickRestartButton.RelativePosition = this.QuickRestartButton.RelativePosition + new Vector3(0f, 24f, 0f);
					this.QuitGameButton.RelativePosition = this.QuitGameButton.RelativePosition + new Vector3(0f, 24f, 0f);
				}
			}
			else if (this.m_buttonsOffsetForDoubleHeight)
			{
				this.BestiaryButton.Text = this.BestiaryButton.ForceGetLocalizedValue("#AMMONOMICON");
				this.BestiaryButton.AutoSize = true;
				this.BestiaryButton.TextAlignment = TextAlignment.Left;
				this.m_buttonsOffsetForDoubleHeight = false;
				this.BestiaryButton.RelativePosition = this.BestiaryButton.RelativePosition + new Vector3(0f, 18f, 0f);
				this.ReturnToGameButton.RelativePosition = this.ReturnToGameButton.RelativePosition + new Vector3(0f, 18f, 0f);
				this.OptionsButton.RelativePosition = this.OptionsButton.RelativePosition - new Vector3(0f, 24f, 0f);
				this.ExitToMainMenuButton.RelativePosition = this.ExitToMainMenuButton.RelativePosition - new Vector3(0f, 24f, 0f);
				this.QuickRestartButton.RelativePosition = this.QuickRestartButton.RelativePosition - new Vector3(0f, 24f, 0f);
				this.QuitGameButton.RelativePosition = this.QuitGameButton.RelativePosition - new Vector3(0f, 24f, 0f);
			}
		}
	}

	// Token: 0x06008FCD RID: 36813 RVA: 0x003CD4DC File Offset: 0x003CB6DC
	private void RemoveQuitButtonAndRealignVertically()
	{
		this.QuitGameButton.Disable();
		UnityEngine.Object.Destroy(this.QuitGameButton.gameObject);
		this.ReturnToGameButton.RelativePosition = this.ReturnToGameButton.RelativePosition + new Vector3(0f, 9f, 0f);
		this.BestiaryButton.RelativePosition = this.BestiaryButton.RelativePosition + new Vector3(0f, 12f, 0f);
		this.OptionsButton.RelativePosition = this.OptionsButton.RelativePosition + new Vector3(0f, 15f, 0f);
		this.QuickRestartButton.RelativePosition = this.QuickRestartButton.RelativePosition + new Vector3(0f, 21f, 0f);
		this.ExitToMainMenuButton.RelativePosition = this.ExitToMainMenuButton.RelativePosition + new Vector3(0f, 24f, 0f);
	}

	// Token: 0x06008FCE RID: 36814 RVA: 0x003CD5F0 File Offset: 0x003CB7F0
	public void ForceRevealMetaCurrencyPanel()
	{
		if (GameManager.Instance.CurrentLevelOverrideState == GameManager.LevelOverrideState.FOYER)
		{
			GameUIRoot.Instance.MoveNonCoreGroupOnscreen(this.metaCurrencyPanel, true);
		}
	}

	// Token: 0x06008FCF RID: 36815 RVA: 0x003CD614 File Offset: 0x003CB814
	public void ForceHideMetaCurrencyPanel()
	{
		if (GameManager.Instance.CurrentLevelOverrideState == GameManager.LevelOverrideState.FOYER)
		{
			GameUIRoot.Instance.AddControlToMotionGroups(this.metaCurrencyPanel, DungeonData.Direction.EAST, true);
			GameUIRoot.Instance.MoveNonCoreGroupOnscreen(this.metaCurrencyPanel, false);
		}
	}

	// Token: 0x06008FD0 RID: 36816 RVA: 0x003CD64C File Offset: 0x003CB84C
	public void ToggleExitCoopButtonOnCoopChange()
	{
		if (GameManager.Instance.CurrentGameType == GameManager.GameType.SINGLE_PLAYER)
		{
			this.ExitToMainMenuButton.Disable();
			UIKeyControls component = this.ExitToMainMenuButton.GetComponent<UIKeyControls>();
			component.up.GetComponent<UIKeyControls>().down = component.down;
			component.down.GetComponent<UIKeyControls>().up = component.up;
		}
		else
		{
			this.ExitToMainMenuButton.Enable();
			UIKeyControls component2 = this.ExitToMainMenuButton.GetComponent<UIKeyControls>();
			if (component2.up && !component2.up.IsEnabled)
			{
				component2.up = component2.up.GetComponent<UIKeyControls>().up;
			}
			if (component2.up)
			{
				component2.up.GetComponent<UIKeyControls>().down = this.ExitToMainMenuButton;
			}
			if (component2.down)
			{
				component2.down.GetComponent<UIKeyControls>().up = this.ExitToMainMenuButton;
			}
		}
	}

	// Token: 0x06008FD1 RID: 36817 RVA: 0x003CD74C File Offset: 0x003CB94C
	public void ToggleVisibility(bool value)
	{
		if (value)
		{
			this.m_panel.IsVisible = value;
			this.PauseBGSprite.Parent.IsVisible = value;
		}
		else
		{
			this.m_panel.IsVisible = value;
			this.PauseBGSprite.Parent.IsVisible = value;
		}
	}

	// Token: 0x06008FD2 RID: 36818 RVA: 0x003CD7A0 File Offset: 0x003CB9A0
	private void HandleVisibilityChange(dfControl control, bool value)
	{
	}

	// Token: 0x06008FD3 RID: 36819 RVA: 0x003CD7A4 File Offset: 0x003CB9A4
	private void DoQuickRestart(dfControl control, dfMouseEventArgs mouseEvent)
	{
		base.StartCoroutine(this.HandleQuickRestart());
	}

	// Token: 0x06008FD4 RID: 36820 RVA: 0x003CD7B4 File Offset: 0x003CB9B4
	private IEnumerator HandleQuickRestart()
	{
		GameUIRoot.Instance.DoAreYouSure("#AYS_QUICKRESTART", false, null);
		this.ToggleVisibility(false);
		while (!GameUIRoot.Instance.HasSelectedAreYouSureOption())
		{
			yield return null;
		}
		if (GameUIRoot.Instance.GetAreYouSureOption())
		{
			if (GameManager.LastUsedPlayerPrefab && GameManager.LastUsedPlayerPrefab.GetComponent<PlayerController>().characterIdentity == PlayableCharacters.Gunslinger && !GameStatsManager.Instance.GetFlag(GungeonFlags.GUNSLINGER_UNLOCKED))
			{
				GameManager.LastUsedPlayerPrefab = (GameObject)ResourceCache.Acquire("PlayerEevee");
			}
			QuickRestartOptions qrOptions = AmmonomiconDeathPageController.GetNumMetasToQuickRestart();
			if (qrOptions.NumMetas > 0)
			{
				GameUIRoot.Instance.CheckKeepModifiersQuickRestart(qrOptions.NumMetas);
				while (!GameUIRoot.Instance.HasSelectedAreYouSureOption())
				{
					yield return null;
				}
				if (!GameUIRoot.Instance.GetAreYouSureOption())
				{
					qrOptions = default(QuickRestartOptions);
					if (GameManager.LastUsedPlayerPrefab && (GameManager.LastUsedPlayerPrefab.GetComponent<PlayerController>().characterIdentity == PlayableCharacters.Eevee || GameManager.LastUsedPlayerPrefab.GetComponent<PlayerController>().characterIdentity == PlayableCharacters.Gunslinger))
					{
						GameManager.LastUsedPlayerPrefab = (GameObject)ResourceCache.Acquire(CharacterSelectController.GetCharacterPathFromQuickStart());
					}
				}
			}
			GameUIRoot.Instance.ToggleUICamera(false);
			if (GameManager.Instance.Dungeon && GameManager.Instance.Dungeon.tileIndices.tilesetId == GlobalDungeonData.ValidTilesets.CASTLEGEON)
			{
				GameStatsManager.Instance.isChump = true;
			}
			GameManager.Instance.DelayedQuickRestart(0.05f, qrOptions);
		}
		else
		{
			this.ToggleVisibility(true);
			this.QuickRestartButton.Focus(true);
		}
		yield break;
	}

	// Token: 0x06008FD5 RID: 36821 RVA: 0x003CD7D0 File Offset: 0x003CB9D0
	private IEnumerator HandleCloseGameEntirely()
	{
		GameUIRoot.Instance.DoAreYouSure("#AYS_QUITTODESKTOP", false, null);
		this.ToggleVisibility(false);
		while (!GameUIRoot.Instance.HasSelectedAreYouSureOption())
		{
			yield return null;
		}
		if (GameUIRoot.Instance.GetAreYouSureOption())
		{
			Application.Quit();
		}
		else
		{
			this.ToggleVisibility(true);
			this.QuitGameButton.Focus(true);
		}
		yield break;
	}

	// Token: 0x06008FD6 RID: 36822 RVA: 0x003CD7EC File Offset: 0x003CB9EC
	public void ToggleBG(dfButton target)
	{
		if (StringTableManager.CurrentLanguage == StringTableManager.GungeonSupportedLanguages.ENGLISH)
		{
			target.BackgroundSprite = string.Empty;
			target.Padding = new RectOffset(0, 0, 0, 0);
		}
		else
		{
			target.BackgroundSprite = "chamber_flash_small_001";
			target.Padding = new RectOffset(6, 6, 0, 0);
			target.NormalBackgroundColor = Color.black;
			target.FocusBackgroundColor = Color.black;
			target.HoverBackgroundColor = Color.black;
			target.DisabledColor = Color.black;
			target.PressedBackgroundColor = Color.black;
		}
	}

	// Token: 0x06008FD7 RID: 36823 RVA: 0x003CD88C File Offset: 0x003CBA8C
	public void HandleBGs()
	{
		this.ToggleBG(this.OptionsButton);
		this.ToggleBG(this.QuickRestartButton);
		this.ToggleBG(this.ReturnToGameButton);
		this.ToggleBG(this.BestiaryButton);
		this.ToggleBG(this.ExitToMainMenuButton);
		if (this.QuitGameButton)
		{
			this.ToggleBG(this.QuitGameButton);
		}
	}

	// Token: 0x06008FD8 RID: 36824 RVA: 0x003CD8F4 File Offset: 0x003CBAF4
	public void ShwoopOpen()
	{
		float num = (float)((!PunchoutController.IsActive || !PunchoutController.OverrideControlsButton) ? 1 : 4);
		this.HandleBGs();
		if (GameManager.Instance.CurrentLevelOverrideState != GameManager.LevelOverrideState.FOYER)
		{
			this.metaCurrencyPanel.IsVisible = true;
			if (this.metaCurrencyPanel && this.metaCurrencyPanel.Parent && this.metaCurrencyPanel.Parent.Parent)
			{
				this.metaCurrencyPanel.Parent.Parent.BringToFront();
			}
			GameUIRoot.Instance.MoveNonCoreGroupOnscreen(this.metaCurrencyPanel, false);
		}
		this.ForceMaterialInvisibility();
		base.StartCoroutine(this.DelayTriggerAnimators(num));
		base.StartCoroutine(this.HandleBlockReveal(false, num));
		base.StartCoroutine(this.HandleShwoop(false, num));
		if (this.m_panel.ZOrder < this.PauseBGSprite.Parent.ZOrder)
		{
			this.m_panel.ZOrder = this.PauseBGSprite.Parent.ZOrder + 1;
		}
	}

	// Token: 0x06008FD9 RID: 36825 RVA: 0x003CDA14 File Offset: 0x003CBC14
	private Material GrabBGRenderMaterial()
	{
		Material material = this.PauseBGSprite.RenderMaterial;
		for (int i = 0; i < this.PauseBGSprite.GUIManager.MeshRenderer.sharedMaterials.Length; i++)
		{
			Material material2 = this.PauseBGSprite.GUIManager.MeshRenderer.sharedMaterials[i];
			if (material2 != null && material2.shader != null && material2.shader.name.Contains("MaskReveal"))
			{
				material = material2;
				break;
			}
		}
		return material;
	}

	// Token: 0x06008FDA RID: 36826 RVA: 0x003CDAAC File Offset: 0x003CBCAC
	private IEnumerator DelayTriggerAnimators(float timeMultiplier = 1f)
	{
		float ela = 0f;
		while (ela < this.DelayDFAnimatorsTime)
		{
			ela += GameManager.INVARIANT_DELTA_TIME * timeMultiplier;
			yield return null;
		}
		dfSprite[] childSprites = this.PauseBGSprite.GetComponentsInChildren<dfSprite>();
		for (int i = 0; i < childSprites.Length; i++)
		{
			childSprites[i].IsVisible = true;
			childSprites[i].GetComponent<dfSpriteAnimation>().Play();
		}
		yield break;
	}

	// Token: 0x06008FDB RID: 36827 RVA: 0x003CDAD0 File Offset: 0x003CBCD0
	private IEnumerator HandleBlockReveal(bool reverse, float timeMultiplier = 1f)
	{
		float timer = 0.3f;
		float elapsed = 0f;
		if (reverse)
		{
			timer = 0.075f;
		}
		if (reverse)
		{
			dfSprite[] componentsInChildren = this.PauseBGSprite.GetComponentsInChildren<dfSprite>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].IsVisible = false;
				componentsInChildren[i].GetComponent<dfSpriteAnimation>().Stop();
			}
		}
		while (elapsed < timer)
		{
			elapsed += GameManager.INVARIANT_DELTA_TIME * timeMultiplier;
			float t = Mathf.Clamp01(elapsed / timer);
			if (reverse)
			{
				t = 1f - t;
			}
			Material targetMaterial = this.GrabBGRenderMaterial();
			if (this.PauseBGSprite.Material != null)
			{
				this.PauseBGSprite.Material.SetFloat("_RevealPercent", t);
			}
			if (this.PauseBGSprite.RenderMaterial != null)
			{
				this.PauseBGSprite.RenderMaterial.SetFloat("_RevealPercent", t);
			}
			if (targetMaterial != null)
			{
				targetMaterial.SetFloat("_RevealPercent", t);
			}
			yield return null;
			if (!reverse)
			{
				this.PauseBGSprite.Parent.IsVisible = true;
			}
		}
		if (reverse)
		{
			this.ForceMaterialInvisibility();
		}
		else
		{
			this.ForceMaterialVisibility();
		}
		this.PauseBGSprite.Parent.IsVisible = this.m_panel.IsVisible;
		if (PunchoutController.IsActive && PunchoutController.OverrideControlsButton && !reverse)
		{
			Debug.Log("aaa visibility");
			this.ToggleVisibility(false);
			Debug.Log("aaa MakeVisibleWithoutAnim");
			this.OptionsMenu.PreOptionsMenu.MakeVisibleWithoutAnim();
			Debug.Log("aaa ToggleToPanel");
			this.OptionsMenu.PreOptionsMenu.ToggleToPanel(this.OptionsMenu.TabControls, false, true);
			Debug.Log("aaa ToggleToKeyboardBindingsPanel");
			FullOptionsMenuController.CurrentBindingPlayerTargetIndex = 0;
			this.OptionsMenu.ToggleToKeyboardBindingsPanel(!BraveInput.PrimaryPlayerInstance.IsKeyboardAndMouse(false));
		}
		yield break;
	}

	// Token: 0x06008FDC RID: 36828 RVA: 0x003CDAFC File Offset: 0x003CBCFC
	private IEnumerator HandleShwoop(bool reverse, float timeMultlier = 1f)
	{
		float timer = 0.1f;
		float elapsed = 0f;
		if (reverse)
		{
			timer = 0.075f;
		}
		Vector3 smallScale = new Vector3(0.01f, 0.01f, 1f);
		Vector3 bigScale = Vector3.one;
		while (elapsed < timer)
		{
			elapsed += GameManager.INVARIANT_DELTA_TIME * timeMultlier;
			float t = Mathf.Clamp01(elapsed / timer);
			AnimationCurve targetCurve = ((!reverse) ? this.ShwoopInCurve : this.ShwoopOutCurve);
			this.m_panel.Opacity = Mathf.Lerp(0f, 1f, (!reverse) ? (t * 2f) : (1f - t * 2f));
			this.m_panel.transform.localScale = smallScale + bigScale * Mathf.Clamp01(targetCurve.Evaluate(t));
			yield return null;
		}
		if (!reverse)
		{
			this.m_panel.transform.localScale = Vector3.one;
			this.m_panel.MakePixelPerfect();
		}
		if (reverse)
		{
			this.m_panel.IsVisible = false;
			this.m_panel.IsInteractive = false;
			this.m_panel.IsEnabled = false;
		}
		yield break;
	}

	// Token: 0x06008FDD RID: 36829 RVA: 0x003CDB28 File Offset: 0x003CBD28
	public void ShwoopClosed()
	{
		base.StartCoroutine(this.HandleShwoop(true, 1f));
		base.StartCoroutine(this.HandleBlockReveal(true, 1f));
		if (GameManager.Instance.CurrentLevelOverrideState != GameManager.LevelOverrideState.FOYER)
		{
			GameUIRoot.Instance.MoveNonCoreGroupOnscreen(this.metaCurrencyPanel, true);
		}
	}

	// Token: 0x06008FDE RID: 36830 RVA: 0x003CDB7C File Offset: 0x003CBD7C
	private void DoQuitGameEntirely(dfControl control, dfMouseEventArgs mouseEvent)
	{
		base.StartCoroutine(this.HandleCloseGameEntirely());
	}

	// Token: 0x06008FDF RID: 36831 RVA: 0x003CDB8C File Offset: 0x003CBD8C
	public void ForceMaterialInvisibility()
	{
		Material material = this.GrabBGRenderMaterial();
		if (this.PauseBGSprite.Material != null)
		{
			this.PauseBGSprite.Material.SetFloat("_RevealPercent", 0f);
		}
		if (this.PauseBGSprite.RenderMaterial != null)
		{
			this.PauseBGSprite.RenderMaterial.SetFloat("_RevealPercent", 0f);
		}
		if (material != null)
		{
			material.SetFloat("_RevealPercent", 0f);
		}
	}

	// Token: 0x06008FE0 RID: 36832 RVA: 0x003CDC1C File Offset: 0x003CBE1C
	public void ForceMaterialVisibility()
	{
		Material material = this.GrabBGRenderMaterial();
		if (this.PauseBGSprite.Material != null)
		{
			this.PauseBGSprite.Material.SetFloat("_RevealPercent", 1f);
		}
		if (this.PauseBGSprite.RenderMaterial != null)
		{
			this.PauseBGSprite.RenderMaterial.SetFloat("_RevealPercent", 1f);
		}
		if (material != null)
		{
			material.SetFloat("_RevealPercent", 1f);
		}
	}

	// Token: 0x06008FE1 RID: 36833 RVA: 0x003CDCAC File Offset: 0x003CBEAC
	public void DoShowBestiaryToTarget(EncounterTrackable target)
	{
		this.ToggleVisibility(false);
		if (dfGUIManager.GetModalControl() != null)
		{
			dfGUIManager.PopModal();
		}
		AmmonomiconController.Instance.OpenAmmonomiconToTrackable(target);
	}

	// Token: 0x06008FE2 RID: 36834 RVA: 0x003CDCD8 File Offset: 0x003CBED8
	public void DoShowBestiary(dfControl control, dfMouseEventArgs mouseEvent)
	{
		if (AmmonomiconController.Instance.IsClosing || AmmonomiconController.Instance.IsOpening)
		{
			return;
		}
		this.ToggleVisibility(false);
		if (dfGUIManager.GetModalControl() != null)
		{
			dfGUIManager.PopModal();
		}
		AmmonomiconController.Instance.OpenAmmonomicon(false, false);
	}

	// Token: 0x06008FE3 RID: 36835 RVA: 0x003CDD2C File Offset: 0x003CBF2C
	private void DoReturnToGame(dfControl control, dfMouseEventArgs mouseEvent)
	{
		if (GameManager.Instance.IsLoadingLevel)
		{
			return;
		}
		GameManager.Instance.Unpause();
	}

	// Token: 0x06008FE4 RID: 36836 RVA: 0x003CDD48 File Offset: 0x003CBF48
	private void DoShowOptions(dfControl control, dfMouseEventArgs mouseEvent)
	{
		if (GameManager.Instance.IsLoadingLevel)
		{
			return;
		}
		this.ToggleVisibility(false);
		this.OptionsMenu.PreOptionsMenu.IsVisible = true;
	}

	// Token: 0x06008FE5 RID: 36837 RVA: 0x003CDD74 File Offset: 0x003CBF74
	private void DoExitToMainMenu(dfControl control, dfMouseEventArgs mouseEvent)
	{
		base.StartCoroutine(this.HandleExitToMainMenu());
	}

	// Token: 0x06008FE6 RID: 36838 RVA: 0x003CDD84 File Offset: 0x003CBF84
	private IEnumerator HandleExitToMainMenu()
	{
		if (GameManager.Instance.IsLoadingLevel)
		{
			yield break;
		}
		GameUIRoot.Instance.DoAreYouSure("#AREYOUSURE", false, null);
		this.ToggleVisibility(false);
		while (!GameUIRoot.Instance.HasSelectedAreYouSureOption())
		{
			yield return null;
		}
		if (GameUIRoot.Instance.GetAreYouSureOption())
		{
			GameUIRoot.Instance.ToggleUICamera(false);
			if (GameManager.Instance.IsFoyer)
			{
				Foyer.Instance.OnDepartedFoyer();
			}
			else
			{
				SaveManager.DeleteCurrentSlotMidGameSave(null);
			}
			Pixelator.Instance.FadeToBlack(0.15f, false, 0f);
			GameManager.Instance.DelayedLoadCharacterSelect(0.15f, true, false);
		}
		else
		{
			this.ToggleVisibility(true);
			this.ExitToMainMenuButton.Focus(true);
		}
		yield break;
	}

	// Token: 0x06008FE7 RID: 36839 RVA: 0x003CDDA0 File Offset: 0x003CBFA0
	public void SetDefaultFocus()
	{
		this.ReturnToGameButton.Focus(true);
	}

	// Token: 0x06008FE8 RID: 36840 RVA: 0x003CDDB0 File Offset: 0x003CBFB0
	public void RevertToBaseState()
	{
		this.HandleBGs();
		this.ToggleVisibility(true);
		if (GameManager.Instance.CurrentLevelOverrideState != GameManager.LevelOverrideState.FOYER)
		{
			this.metaCurrencyPanel.IsVisible = this.m_panel.IsVisible;
		}
		this.m_panel.IsInteractive = true;
		this.m_panel.IsEnabled = true;
		this.OptionsMenu.IsVisible = false;
		this.OptionsMenu.PreOptionsMenu.IsVisible = false;
		for (int i = 0; i < this.AdditionalMenuElementsToClear.Count; i++)
		{
			UnityEngine.Object.Destroy(this.AdditionalMenuElementsToClear[i]);
		}
		this.AdditionalMenuElementsToClear.Clear();
		dfGUIManager.PopModalToControl(this.m_panel, false);
		this.ForceMaterialVisibility();
		this.SetDefaultFocus();
		AkSoundEngine.PostEvent("Play_UI_menu_back_01", base.gameObject);
	}

	// Token: 0x040097F9 RID: 38905
	public dfButton ExitToMainMenuButton;

	// Token: 0x040097FA RID: 38906
	public dfButton ReturnToGameButton;

	// Token: 0x040097FB RID: 38907
	public dfButton BestiaryButton;

	// Token: 0x040097FC RID: 38908
	public dfButton QuickRestartButton;

	// Token: 0x040097FD RID: 38909
	public dfButton QuitGameButton;

	// Token: 0x040097FE RID: 38910
	public dfButton OptionsButton;

	// Token: 0x040097FF RID: 38911
	public dfTextureSprite PauseBGSprite;

	// Token: 0x04009800 RID: 38912
	public FullOptionsMenuController OptionsMenu;

	// Token: 0x04009801 RID: 38913
	public List<GameObject> AdditionalMenuElementsToClear;

	// Token: 0x04009802 RID: 38914
	public dfPanel metaCurrencyPanel;

	// Token: 0x04009803 RID: 38915
	public AnimationCurve ShwoopInCurve;

	// Token: 0x04009804 RID: 38916
	public AnimationCurve ShwoopOutCurve;

	// Token: 0x04009805 RID: 38917
	public float DelayDFAnimatorsTime = 0.3f;

	// Token: 0x04009806 RID: 38918
	private dfPanel m_panel;

	// Token: 0x04009807 RID: 38919
	private bool m_buttonsOffsetForDoubleHeight;

	// Token: 0x04009808 RID: 38920
	private const float c_FrenchVertOffsetUp = 18f;

	// Token: 0x04009809 RID: 38921
	private const float c_FrenchVertOffsetDown = 24f;
}
