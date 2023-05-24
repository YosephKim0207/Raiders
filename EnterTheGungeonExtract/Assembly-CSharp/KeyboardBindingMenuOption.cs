using System;
using System.Collections;
using InControl;
using UnityEngine;

// Token: 0x020017DE RID: 6110
public class KeyboardBindingMenuOption : MonoBehaviour
{
	// Token: 0x1700157F RID: 5503
	// (get) Token: 0x06008FB2 RID: 36786 RVA: 0x003CC364 File Offset: 0x003CA564
	// (set) Token: 0x06008FB3 RID: 36787 RVA: 0x003CC36C File Offset: 0x003CA56C
	public bool NonBindable { get; set; }

	// Token: 0x17001580 RID: 5504
	// (get) Token: 0x06008FB4 RID: 36788 RVA: 0x003CC378 File Offset: 0x003CA578
	// (set) Token: 0x06008FB5 RID: 36789 RVA: 0x003CC380 File Offset: 0x003CA580
	public string OverrideKeyString { get; set; }

	// Token: 0x17001581 RID: 5505
	// (get) Token: 0x06008FB6 RID: 36790 RVA: 0x003CC38C File Offset: 0x003CA58C
	// (set) Token: 0x06008FB7 RID: 36791 RVA: 0x003CC394 File Offset: 0x003CA594
	public string OverrideAltKeyString { get; set; }

	// Token: 0x06008FB8 RID: 36792 RVA: 0x003CC3A0 File Offset: 0x003CA5A0
	public void Initialize()
	{
		if (this.m_parentOptionsMenu == null)
		{
			this.m_parentOptionsMenu = GameUIRoot.Instance.PauseMenuPanel.GetComponent<PauseMenuController>().OptionsMenu;
		}
		if (this.IsControllerMode)
		{
			this.InitializeController();
		}
		else
		{
			this.InitializeKeyboard();
		}
		if (this.NonBindable)
		{
			this.CommandLabel.IsInteractive = false;
			this.CommandLabel.Color = new Color(0.25f, 0.25f, 0.25f);
		}
		else
		{
			this.CommandLabel.IsInteractive = true;
			this.CommandLabel.Color = new Color(0.596f, 0.596f, 0.596f, 1f);
		}
	}

	// Token: 0x06008FB9 RID: 36793 RVA: 0x003CC46C File Offset: 0x003CA66C
	private void InitializeController()
	{
		GungeonActions activeActions = this.GetBestInputInstance().ActiveActions;
		PlayerAction actionFromType = activeActions.GetActionFromType(this.ActionType);
		bool flag = false;
		string text = "-";
		bool flag2 = false;
		string text2 = "-";
		for (int i = 0; i < actionFromType.Bindings.Count; i++)
		{
			BindingSource bindingSource = actionFromType.Bindings[i];
			if (bindingSource.BindingSourceType == BindingSourceType.DeviceBindingSource)
			{
				DeviceBindingSource deviceBindingSource = bindingSource as DeviceBindingSource;
				GameOptions.ControllerSymbology currentSymbology = BraveInput.GetCurrentSymbology(FullOptionsMenuController.CurrentBindingPlayerTargetIndex);
				if (!flag)
				{
					flag = true;
					text = UIControllerButtonHelper.GetUnifiedControllerButtonTag(deviceBindingSource.Control, currentSymbology, activeActions);
				}
				else if (!flag2)
				{
					text2 = UIControllerButtonHelper.GetUnifiedControllerButtonTag(deviceBindingSource.Control, currentSymbology, activeActions);
					break;
				}
			}
			if (bindingSource.BindingSourceType == BindingSourceType.UnknownDeviceBindingSource)
			{
				UnknownDeviceBindingSource unknownDeviceBindingSource = bindingSource as UnknownDeviceBindingSource;
				if (!flag)
				{
					flag = true;
					text = unknownDeviceBindingSource.Control.Control.ToString();
				}
				else if (!flag2)
				{
					flag2 = true;
					text2 = unknownDeviceBindingSource.Control.Control.ToString();
				}
			}
		}
		this.KeyButton.Text = (string.IsNullOrEmpty(this.OverrideKeyString) ? text.Trim() : this.OverrideKeyString);
		this.AltKeyButton.Text = (string.IsNullOrEmpty(this.OverrideAltKeyString) ? text2.Trim() : this.OverrideAltKeyString);
		this.AltKeyButton.transform.position = this.AltKeyButton.transform.position.WithX(this.AltAlignLabel.GetCenter().x);
		if (GameManager.Options.CurrentLanguage == StringTableManager.GungeonSupportedLanguages.ITALIAN || GameManager.Options.CurrentLanguage == StringTableManager.GungeonSupportedLanguages.GERMAN)
		{
			this.KeyButton.Padding = new RectOffset(60, 0, 0, 0);
		}
		else if (GameManager.Options.CurrentLanguage != StringTableManager.GungeonSupportedLanguages.ENGLISH)
		{
			this.KeyButton.Padding = new RectOffset(180, 0, 0, 0);
		}
		else
		{
			this.KeyButton.Padding = new RectOffset(0, 0, 0, 0);
		}
		if (this.CenterColumnLabel)
		{
			this.CenterColumnLabel.Padding = this.KeyButton.Padding;
		}
		base.GetComponent<dfPanel>().PerformLayout();
		this.CommandLabel.RelativePosition = this.CommandLabel.RelativePosition.WithX(0f);
	}

	// Token: 0x06008FBA RID: 36794 RVA: 0x003CC6F8 File Offset: 0x003CA8F8
	public BraveInput GetBestInputInstance()
	{
		BraveInput braveInput;
		if (GameManager.Instance.CurrentLevelOverrideState == GameManager.LevelOverrideState.FOYER && Foyer.DoMainMenu)
		{
			braveInput = BraveInput.PlayerlessInstance;
		}
		else
		{
			braveInput = BraveInput.GetInstanceForPlayer(FullOptionsMenuController.CurrentBindingPlayerTargetIndex);
		}
		return braveInput;
	}

	// Token: 0x06008FBB RID: 36795 RVA: 0x003CC738 File Offset: 0x003CA938
	private void InitializeKeyboard()
	{
		if (GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER)
		{
			BraveInput instanceForPlayer = BraveInput.GetInstanceForPlayer(0);
			BraveInput instanceForPlayer2 = BraveInput.GetInstanceForPlayer(1);
			if (instanceForPlayer && instanceForPlayer2)
			{
				GungeonActions activeActions = instanceForPlayer.ActiveActions;
				GungeonActions activeActions2 = instanceForPlayer2.ActiveActions;
				if (activeActions != null && activeActions2 != null)
				{
					PlayerAction actionFromType = activeActions.GetActionFromType(this.ActionType);
					PlayerAction actionFromType2 = activeActions2.GetActionFromType(this.ActionType);
					actionFromType2.ClearBindingsOfType(BindingSourceType.KeyBindingSource);
					for (int i = 0; i < actionFromType.Bindings.Count; i++)
					{
						BindingSource bindingSource = actionFromType.Bindings[i];
						if (bindingSource.BindingSourceType == BindingSourceType.KeyBindingSource && bindingSource is KeyBindingSource)
						{
							BindingSource bindingSource2 = new KeyBindingSource((bindingSource as KeyBindingSource).Control);
							actionFromType2.AddBinding(bindingSource2);
						}
					}
				}
			}
		}
		BraveInput bestInputInstance = this.GetBestInputInstance();
		GungeonActions activeActions3 = bestInputInstance.ActiveActions;
		PlayerAction actionFromType3 = activeActions3.GetActionFromType(this.ActionType);
		bool flag = false;
		string text = "-";
		bool flag2 = false;
		string text2 = "-";
		for (int j = 0; j < actionFromType3.Bindings.Count; j++)
		{
			BindingSource bindingSource3 = actionFromType3.Bindings[j];
			if (bindingSource3.BindingSourceType == BindingSourceType.KeyBindingSource || bindingSource3.BindingSourceType == BindingSourceType.MouseBindingSource)
			{
				if (!flag)
				{
					flag = true;
					text = bindingSource3.Name;
				}
				else if (!flag2)
				{
					text2 = bindingSource3.Name;
					break;
				}
			}
		}
		this.KeyButton.Text = text.Trim();
		this.AltKeyButton.Text = text2.Trim();
		this.AltKeyButton.transform.position = this.AltKeyButton.transform.position.WithX(this.AltAlignLabel.GetCenter().x);
		if (GameManager.Options.CurrentLanguage == StringTableManager.GungeonSupportedLanguages.ITALIAN || GameManager.Options.CurrentLanguage == StringTableManager.GungeonSupportedLanguages.GERMAN)
		{
			this.KeyButton.Padding = new RectOffset(60, 0, 0, 0);
		}
		else if (GameManager.Options.CurrentLanguage != StringTableManager.GungeonSupportedLanguages.ENGLISH)
		{
			this.KeyButton.Padding = new RectOffset(180, 0, 0, 0);
		}
		else
		{
			this.KeyButton.Padding = new RectOffset(0, 0, 0, 0);
		}
		if (this.CenterColumnLabel)
		{
			this.CenterColumnLabel.Padding = this.KeyButton.Padding;
		}
		base.GetComponent<dfPanel>().PerformLayout();
		this.CommandLabel.RelativePosition = this.CommandLabel.RelativePosition.WithX(0f);
	}

	// Token: 0x06008FBC RID: 36796 RVA: 0x003CC9F8 File Offset: 0x003CABF8
	public void KeyClicked(dfControl source, dfControlEventArgs args)
	{
		if (this.NonBindable)
		{
			return;
		}
		this.EnterAssignmentMode(false);
		GameUIRoot.Instance.PauseMenuPanel.GetComponent<PauseMenuController>().OptionsMenu.DoModalKeyBindingDialog(this.CommandLabel.Text);
		base.StartCoroutine(this.WaitForAssignmentModeToEnd());
	}

	// Token: 0x06008FBD RID: 36797 RVA: 0x003CCA4C File Offset: 0x003CAC4C
	public void AltKeyClicked(dfControl source, dfControlEventArgs args)
	{
		if (this.NonBindable)
		{
			return;
		}
		this.EnterAssignmentMode(true);
		GameUIRoot.Instance.PauseMenuPanel.GetComponent<PauseMenuController>().OptionsMenu.DoModalKeyBindingDialog(this.CommandLabel.Text);
		base.StartCoroutine(this.WaitForAssignmentModeToEnd());
	}

	// Token: 0x06008FBE RID: 36798 RVA: 0x003CCAA0 File Offset: 0x003CACA0
	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Delete))
		{
			if (this.KeyButton.HasFocus)
			{
				GungeonActions activeActions = this.GetBestInputInstance().ActiveActions;
				PlayerAction actionFromType = activeActions.GetActionFromType(this.ActionType);
				if (this.IsControllerMode)
				{
					actionFromType.ClearSpecificBindingByType(0, new BindingSourceType[] { BindingSourceType.DeviceBindingSource });
					this.InitializeController();
				}
				else
				{
					actionFromType.ClearSpecificBindingByType(0, new BindingSourceType[]
					{
						BindingSourceType.KeyBindingSource,
						BindingSourceType.MouseBindingSource
					});
					this.InitializeKeyboard();
				}
			}
			else if (this.AltKeyButton.HasFocus)
			{
				GungeonActions activeActions2 = this.GetBestInputInstance().ActiveActions;
				PlayerAction actionFromType2 = activeActions2.GetActionFromType(this.ActionType);
				if (this.IsControllerMode)
				{
					actionFromType2.ClearSpecificBindingByType(1, new BindingSourceType[] { BindingSourceType.DeviceBindingSource });
					this.InitializeController();
				}
				else
				{
					actionFromType2.ClearSpecificBindingByType(1, new BindingSourceType[]
					{
						BindingSourceType.KeyBindingSource,
						BindingSourceType.MouseBindingSource
					});
					this.InitializeKeyboard();
				}
			}
		}
	}

	// Token: 0x06008FBF RID: 36799 RVA: 0x003CCB94 File Offset: 0x003CAD94
	private IEnumerator WaitForAssignmentModeToEnd()
	{
		GungeonActions activeActions = this.GetBestInputInstance().ActiveActions;
		PlayerAction targetAction = activeActions.GetActionFromType(this.ActionType);
		while (targetAction.IsListeningForBinding)
		{
			yield return null;
		}
		this.Initialize();
		yield break;
	}

	// Token: 0x06008FC0 RID: 36800 RVA: 0x003CCBB0 File Offset: 0x003CADB0
	public void EnterAssignmentMode(bool isAlternateKey)
	{
		GungeonActions activeActions = this.GetBestInputInstance().ActiveActions;
		PlayerAction actionFromType = activeActions.GetActionFromType(this.ActionType);
		BindingListenOptions bindingOptions = new BindingListenOptions();
		if (this.IsControllerMode)
		{
			bindingOptions.IncludeControllers = true;
			bindingOptions.IncludeNonStandardControls = true;
			bindingOptions.IncludeKeys = true;
			bindingOptions.IncludeMouseButtons = false;
			bindingOptions.IncludeMouseScrollWheel = false;
			bindingOptions.IncludeModifiersAsFirstClassKeys = false;
			bindingOptions.IncludeUnknownControllers = GameManager.Options.allowUnknownControllers;
		}
		else
		{
			bindingOptions.IncludeControllers = false;
			bindingOptions.IncludeNonStandardControls = false;
			bindingOptions.IncludeKeys = true;
			bindingOptions.IncludeMouseButtons = true;
			bindingOptions.IncludeMouseScrollWheel = true;
			bindingOptions.IncludeModifiersAsFirstClassKeys = true;
		}
		bindingOptions.MaxAllowedBindingsPerType = 2U;
		bindingOptions.OnBindingFound = delegate(PlayerAction action, BindingSource binding)
		{
			if (binding == new KeyBindingSource(new Key[] { Key.Escape }))
			{
				action.StopListeningForBinding();
				GameUIRoot.Instance.PauseMenuPanel.GetComponent<PauseMenuController>().OptionsMenu.ClearModalKeyBindingDialog(null, null);
				return false;
			}
			if (binding == new KeyBindingSource(new Key[] { Key.Delete }))
			{
				action.StopListeningForBinding();
				GameUIRoot.Instance.PauseMenuPanel.GetComponent<PauseMenuController>().OptionsMenu.ClearModalKeyBindingDialog(null, null);
				return false;
			}
			if (this.IsControllerMode && binding is KeyBindingSource)
			{
				return false;
			}
			action.StopListeningForBinding();
			if (!this.m_parentOptionsMenu.ActionIsMultibindable(this.ActionType, activeActions))
			{
				this.m_parentOptionsMenu.ClearBindingFromAllControls(FullOptionsMenuController.CurrentBindingPlayerTargetIndex, binding);
			}
			action.SetBindingOfTypeByNumber(binding, binding.BindingSourceType, (!isAlternateKey) ? 0 : 1, bindingOptions.OnBindingAdded);
			GameUIRoot.Instance.PauseMenuPanel.GetComponent<PauseMenuController>().OptionsMenu.ToggleKeyBindingDialogState(binding);
			this.Initialize();
			return false;
		};
		bindingOptions.OnBindingAdded = delegate(PlayerAction action, BindingSource binding)
		{
			if (FullOptionsMenuController.CurrentBindingPlayerTargetIndex == 1)
			{
				GameManager.Options.CurrentControlPresetP2 = GameOptions.ControlPreset.CUSTOM;
			}
			else
			{
				GameManager.Options.CurrentControlPreset = GameOptions.ControlPreset.CUSTOM;
			}
			BraveOptionsMenuItem[] componentsInChildren = this.CenterColumnLabel.Parent.Parent.Parent.GetComponentsInChildren<BraveOptionsMenuItem>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].InitializeFromOptions();
				componentsInChildren[i].ForceRefreshDisplayLabel();
			}
			this.Initialize();
		};
		actionFromType.ListenOptions = bindingOptions;
		if (!actionFromType.IsListeningForBinding)
		{
			actionFromType.ListenForBinding();
		}
	}

	// Token: 0x040097E3 RID: 38883
	public dfLabel CenterColumnLabel;

	// Token: 0x040097E4 RID: 38884
	public dfLabel CommandLabel;

	// Token: 0x040097E5 RID: 38885
	public dfButton KeyButton;

	// Token: 0x040097E6 RID: 38886
	public dfButton AltKeyButton;

	// Token: 0x040097E7 RID: 38887
	public dfLabel AltAlignLabel;

	// Token: 0x040097E8 RID: 38888
	public GungeonActions.GungeonActionType ActionType;

	// Token: 0x040097E9 RID: 38889
	public bool IsControllerMode;

	// Token: 0x040097EA RID: 38890
	private FullOptionsMenuController m_parentOptionsMenu;

	// Token: 0x040097EB RID: 38891
	private Vector2 m_cachedKeyButtonPosition;
}
