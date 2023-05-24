using System;
using System.Collections;
using System.Collections.Generic;
using InControl;
using UnityEngine;

// Token: 0x020017D8 RID: 6104
public class FullOptionsMenuController : MonoBehaviour
{
	// Token: 0x17001575 RID: 5493
	// (get) Token: 0x06008F61 RID: 36705 RVA: 0x003C9F60 File Offset: 0x003C8160
	// (set) Token: 0x06008F62 RID: 36706 RVA: 0x003C9F70 File Offset: 0x003C8170
	public bool IsVisible
	{
		get
		{
			return this.m_panel.IsVisible;
		}
		set
		{
			if (this.m_panel.IsVisible != value)
			{
				if (value)
				{
					this.EnableHierarchy();
					this.m_panel.IsVisible = value;
					this.ShwoopOpen();
					this.ShowOptionsMenu();
				}
				else
				{
					this.ShwoopClosed();
					if (dfGUIManager.GetModalControl() == this.m_panel)
					{
						dfGUIManager.PopModal();
					}
					else if (dfGUIManager.ModalStackContainsControl(this.m_panel))
					{
						dfGUIManager.PopModalToControl(this.m_panel, true);
					}
					else
					{
						Debug.LogError("failure.");
					}
				}
			}
		}
	}

	// Token: 0x17001576 RID: 5494
	// (get) Token: 0x06008F63 RID: 36707 RVA: 0x003CA008 File Offset: 0x003C8208
	public dfPanel MainPanel
	{
		get
		{
			return this.m_panel;
		}
	}

	// Token: 0x06008F64 RID: 36708 RVA: 0x003CA010 File Offset: 0x003C8210
	private void Awake()
	{
		this.m_panel = base.GetComponent<dfPanel>();
	}

	// Token: 0x06008F65 RID: 36709 RVA: 0x003CA020 File Offset: 0x003C8220
	public void EnableHierarchy()
	{
		if (base.gameObject.activeSelf)
		{
			return;
		}
		base.gameObject.SetActive(true);
		dfGUIManager guimanager = this.m_panel.GUIManager;
		Vector2 screenSize = guimanager.GetScreenSize();
		dfControl[] componentsInChildren = this.m_panel.GetComponentsInChildren<dfControl>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].OnResolutionChanged(this.m_cachedResolution, screenSize);
		}
		for (int j = 0; j < componentsInChildren.Length; j++)
		{
			componentsInChildren[j].PerformLayout();
		}
	}

	// Token: 0x06008F66 RID: 36710 RVA: 0x003CA0B0 File Offset: 0x003C82B0
	public void DisableHierarchy()
	{
		if (!base.gameObject.activeSelf)
		{
			return;
		}
		dfGUIManager guimanager = this.m_panel.GUIManager;
		float num = (float)guimanager.RenderCamera.pixelHeight;
		float num2 = ((!guimanager.FixedAspect) ? guimanager.RenderCamera.aspect : 1.7777778f);
		this.m_cachedResolution = new Vector2(num2 * num, num);
		base.gameObject.SetActive(false);
	}

	// Token: 0x06008F67 RID: 36711 RVA: 0x003CA124 File Offset: 0x003C8324
	public void DoModalKeyBindingDialog(string controlName)
	{
		this.m_cachedFocusedControl = dfGUIManager.ActiveControl;
		this.ModalKeyBindingDialog.IsVisible = true;
		this.m_panel.IsVisible = false;
		this.ModalKeyBindingDialog.BringToFront();
		dfGUIManager.PushModal(this.ModalKeyBindingDialog);
		if (GameManager.Options.CurrentLanguage == StringTableManager.GungeonSupportedLanguages.ENGLISH)
		{
			this.ModalKeyBindingDialog.transform.Find("TopLabel").GetComponent<dfLabel>().Text = "BIND " + controlName.ToUpperInvariant() + "?";
			this.ModalKeyBindingDialog.transform.Find("SecondaryLabel").GetComponent<dfLabel>().IsVisible = true;
		}
		else
		{
			this.ModalKeyBindingDialog.transform.Find("TopLabel").GetComponent<dfLabel>().Text = controlName.ToUpperInvariant();
			this.ModalKeyBindingDialog.transform.Find("SecondaryLabel").GetComponent<dfLabel>().IsVisible = false;
		}
		dfButton component = this.ModalKeyBindingDialog.transform.Find("Input Thing").GetComponent<dfButton>();
		component.Text = "___";
		component.PerformLayout();
	}

	// Token: 0x06008F68 RID: 36712 RVA: 0x003CA244 File Offset: 0x003C8444
	public void ToggleKeyBindingDialogState(BindingSource binding)
	{
		dfButton component = this.ModalKeyBindingDialog.transform.Find("Input Thing").GetComponent<dfButton>();
		if (binding is DeviceBindingSource)
		{
			GameOptions.ControllerSymbology currentSymbology = BraveInput.GetCurrentSymbology(FullOptionsMenuController.CurrentBindingPlayerTargetIndex);
			component.Text = UIControllerButtonHelper.GetUnifiedControllerButtonTag((binding as DeviceBindingSource).Control, currentSymbology, null);
		}
		else
		{
			component.Text = binding.Name;
		}
		component.PerformLayout();
		component.Focus(true);
		component.Click += new MouseEventHandler(this.ClearModalKeyBindingDialog);
		base.StartCoroutine(this.HandleTimedKeyBindingClear());
	}

	// Token: 0x06008F69 RID: 36713 RVA: 0x003CA2D8 File Offset: 0x003C84D8
	private IEnumerator HandleTimedKeyBindingClear()
	{
		float elapsed = 0f;
		float duration = 0.25f;
		while (elapsed < duration)
		{
			if (!this.ModalKeyBindingDialog.IsVisible)
			{
				yield break;
			}
			elapsed += GameManager.INVARIANT_DELTA_TIME;
			yield return null;
		}
		this.ClearModalKeyBindingDialog(null, null);
		yield break;
	}

	// Token: 0x06008F6A RID: 36714 RVA: 0x003CA2F4 File Offset: 0x003C84F4
	public void ClearModalKeyBindingDialog(dfControl source, dfControlEventArgs args)
	{
		if (!this.ModalKeyBindingDialog.IsVisible)
		{
			return;
		}
		this.m_panel.IsVisible = true;
		dfGUIManager.PopModalToControl(this.m_panel, false);
		this.ModalKeyBindingDialog.IsVisible = false;
		if (this.m_cachedFocusedControl != null)
		{
			this.m_cachedFocusedControl.Focus(true);
		}
	}

	// Token: 0x06008F6B RID: 36715 RVA: 0x003CA354 File Offset: 0x003C8554
	public void ReinitializeKeyboardBindings()
	{
		for (int i = 0; i < this.m_keyboardBindingLines.Count; i++)
		{
			this.m_keyboardBindingLines[i].Initialize();
		}
	}

	// Token: 0x06008F6C RID: 36716 RVA: 0x003CA390 File Offset: 0x003C8590
	public bool ActionIsMultibindable(GungeonActions.GungeonActionType actionType, GungeonActions activeActions)
	{
		return actionType == GungeonActions.GungeonActionType.DropGun || actionType == GungeonActions.GungeonActionType.DropItem || actionType == GungeonActions.GungeonActionType.SelectUp || actionType == GungeonActions.GungeonActionType.SelectDown || actionType == GungeonActions.GungeonActionType.SelectLeft || actionType == GungeonActions.GungeonActionType.SelectRight || actionType == GungeonActions.GungeonActionType.MenuInteract || actionType == GungeonActions.GungeonActionType.Cancel || actionType == GungeonActions.GungeonActionType.PunchoutDodgeLeft || actionType == GungeonActions.GungeonActionType.PunchoutDodgeRight || actionType == GungeonActions.GungeonActionType.PunchoutBlock || actionType == GungeonActions.GungeonActionType.PunchoutDuck || actionType == GungeonActions.GungeonActionType.PunchoutPunchLeft || actionType == GungeonActions.GungeonActionType.PunchoutPunchRight || actionType == GungeonActions.GungeonActionType.PunchoutSuper;
	}

	// Token: 0x06008F6D RID: 36717 RVA: 0x003CA434 File Offset: 0x003C8634
	public void ClearBindingFromAllControls(int targetPlayerIndex, BindingSource bindingSource)
	{
		GungeonActions activeActions = BraveInput.GetInstanceForPlayer(targetPlayerIndex).ActiveActions;
		for (int i = 0; i < this.m_keyboardBindingLines.Count; i++)
		{
			bool flag = false;
			GungeonActions.GungeonActionType actionType = this.m_keyboardBindingLines[i].ActionType;
			if (!this.ActionIsMultibindable(actionType, activeActions))
			{
				PlayerAction actionFromType = activeActions.GetActionFromType(actionType);
				for (int j = 0; j < actionFromType.Bindings.Count; j++)
				{
					BindingSource bindingSource2 = actionFromType.Bindings[j];
					if (bindingSource2 == bindingSource)
					{
						actionFromType.RemoveBinding(bindingSource2);
						flag = true;
					}
				}
				if (flag)
				{
					actionFromType.ForceUpdateVisibleBindings();
					this.m_keyboardBindingLines[i].Initialize();
				}
			}
		}
	}

	// Token: 0x06008F6E RID: 36718 RVA: 0x003CA500 File Offset: 0x003C8700
	public void SwitchBindingsMenuMode(bool isController)
	{
		int num = 0;
		BraveOptionsMenuItem component = this.TabKeyboardBindings.Controls[num].GetComponent<BraveOptionsMenuItem>();
		component.optionType = ((FullOptionsMenuController.CurrentBindingPlayerTargetIndex != 0) ? BraveOptionsMenuItem.BraveOptionsOptionType.CURRENT_BINDINGS_PRESET_P2 : BraveOptionsMenuItem.BraveOptionsOptionType.CURRENT_BINDINGS_PRESET);
		if (GameManager.Options.CurrentLanguage == StringTableManager.GungeonSupportedLanguages.ENGLISH)
		{
			component.labelControl.IsLocalized = false;
			component.labelControl.Text = "Binding Preset";
			component.labelControl.PerformLayout();
			component.labelControl.Parent.PerformLayout();
		}
		else
		{
			component.labelControl.IsLocalized = true;
			if (isController)
			{
				if (FullOptionsMenuController.CurrentBindingPlayerTargetIndex == 1)
				{
					component.labelControl.Text = "#OPTIONS_EDITP2BINDINGS";
				}
				else
				{
					component.labelControl.Text = "#OPTIONS_EDITP1BINDINGS";
				}
			}
			else
			{
				component.labelControl.Text = "#OPTIONS_EDITKEYBOARDBINDINGS";
			}
		}
		component.InitializeFromOptions();
		component.ForceRefreshDisplayLabel();
		for (int i = 0; i < this.m_keyboardBindingLines.Count; i++)
		{
			this.m_keyboardBindingLines[i].IsControllerMode = isController;
			this.m_keyboardBindingLines[i].Initialize();
		}
		if (this.TabKeyboardBindings != null)
		{
			this.TabKeyboardBindings.PerformLayout();
		}
	}

	// Token: 0x06008F6F RID: 36719 RVA: 0x003CA648 File Offset: 0x003C8848
	public void FullyReinitializeKeyboardBindings()
	{
		DebugTime.Log("FullyReinitializeKeyboardBindings", new object[0]);
		KeyboardBindingMenuOption componentInChildren = this.TabKeyboardBindings.GetComponentInChildren<KeyboardBindingMenuOption>();
		dfPanel component = componentInChildren.GetComponent<dfPanel>();
		for (int i = this.m_keyboardBindingLines.Count - 1; i >= 1; i--)
		{
			KeyboardBindingMenuOption keyboardBindingMenuOption = this.m_keyboardBindingLines[i];
			dfPanel component2 = keyboardBindingMenuOption.GetComponent<dfPanel>();
			component.RemoveControl(component2);
			KeyboardBindingMenuOption keyboardBindingMenuOption2 = this.m_keyboardBindingLines[i - 1];
			keyboardBindingMenuOption2.KeyButton.GetComponent<UIKeyControls>().down = keyboardBindingMenuOption.KeyButton.GetComponent<UIKeyControls>().down;
			keyboardBindingMenuOption2.AltKeyButton.GetComponent<UIKeyControls>().down = keyboardBindingMenuOption.AltKeyButton.GetComponent<UIKeyControls>().down;
			UnityEngine.Object.Destroy(component2.gameObject);
		}
		this.m_keyboardBindingLines.Clear();
		this.finishedInitialization = true;
		this.InitializeKeyboardBindingsPanel();
		KeyboardBindingMenuOption keyboardBindingMenuOption3 = this.m_keyboardBindingLines[this.m_keyboardBindingLines.Count - 1];
		keyboardBindingMenuOption3.KeyButton.GetComponent<UIKeyControls>().down = this.PrimaryConfirmButton;
		keyboardBindingMenuOption3.AltKeyButton.GetComponent<UIKeyControls>().down = this.PrimaryConfirmButton;
		this.PrimaryCancelButton.GetComponent<UIKeyControls>().up = keyboardBindingMenuOption3.KeyButton;
		this.PrimaryConfirmButton.GetComponent<UIKeyControls>().up = keyboardBindingMenuOption3.KeyButton;
		this.PrimaryResetDefaultsButton.GetComponent<UIKeyControls>().up = keyboardBindingMenuOption3.KeyButton;
	}

	// Token: 0x06008F70 RID: 36720 RVA: 0x003CA7B8 File Offset: 0x003C89B8
	private void InitializeKeyboardBindingsPanel()
	{
		KeyboardBindingMenuOption componentInChildren = this.TabKeyboardBindings.GetComponentInChildren<KeyboardBindingMenuOption>();
		dfPanel component = componentInChildren.GetComponent<dfPanel>();
		componentInChildren.KeyButton.GetComponent<UIKeyControls>().up = this.TabKeyboardBindings.Controls[0];
		componentInChildren.AltKeyButton.GetComponent<UIKeyControls>().up = this.TabKeyboardBindings.Controls[0];
		this.TabKeyboardBindings.Controls[0].GetComponent<BraveOptionsMenuItem>().down = componentInChildren.KeyButton;
		if (this.m_firstTimeBindingsInitialization)
		{
			componentInChildren.KeyButton.Click += new MouseEventHandler(componentInChildren.KeyClicked);
			componentInChildren.AltKeyButton.Click += new MouseEventHandler(componentInChildren.AltKeyClicked);
			this.m_firstTimeBindingsInitialization = false;
		}
		componentInChildren.Initialize();
		this.m_keyboardBindingLines.Add(componentInChildren);
		KeyboardBindingMenuOption keyboardBindingMenuOption = componentInChildren;
		keyboardBindingMenuOption = this.AddKeyboardBindingLine(component.Parent, component.gameObject, GungeonActions.GungeonActionType.DodgeRoll, "#OPTIONS_DODGEROLL", keyboardBindingMenuOption, false);
		keyboardBindingMenuOption = this.AddKeyboardBindingLine(component.Parent, component.gameObject, GungeonActions.GungeonActionType.Interact, "#OPTIONS_INTERACT", keyboardBindingMenuOption, false);
		keyboardBindingMenuOption = this.AddKeyboardBindingLine(component.Parent, component.gameObject, GungeonActions.GungeonActionType.Reload, "#OPTIONS_RELOAD", keyboardBindingMenuOption, false);
		keyboardBindingMenuOption = this.AddKeyboardBindingLine(component.Parent, component.gameObject, GungeonActions.GungeonActionType.Up, "#OPTIONS_MOVEUP", keyboardBindingMenuOption, false);
		keyboardBindingMenuOption = this.AddKeyboardBindingLine(component.Parent, component.gameObject, GungeonActions.GungeonActionType.Down, "#OPTIONS_MOVEDOWN", keyboardBindingMenuOption, false);
		keyboardBindingMenuOption = this.AddKeyboardBindingLine(component.Parent, component.gameObject, GungeonActions.GungeonActionType.Left, "#OPTIONS_MOVELEFT", keyboardBindingMenuOption, false);
		keyboardBindingMenuOption = this.AddKeyboardBindingLine(component.Parent, component.gameObject, GungeonActions.GungeonActionType.Right, "#OPTIONS_MOVERIGHT", keyboardBindingMenuOption, false);
		keyboardBindingMenuOption = this.AddKeyboardBindingLine(component.Parent, component.gameObject, GungeonActions.GungeonActionType.AimUp, "#OPTIONS_AIMUP", keyboardBindingMenuOption, false);
		keyboardBindingMenuOption = this.AddKeyboardBindingLine(component.Parent, component.gameObject, GungeonActions.GungeonActionType.AimDown, "#OPTIONS_AIMDOWN", keyboardBindingMenuOption, false);
		keyboardBindingMenuOption = this.AddKeyboardBindingLine(component.Parent, component.gameObject, GungeonActions.GungeonActionType.AimLeft, "#OPTIONS_AIMLEFT", keyboardBindingMenuOption, false);
		keyboardBindingMenuOption = this.AddKeyboardBindingLine(component.Parent, component.gameObject, GungeonActions.GungeonActionType.AimRight, "#OPTIONS_AIMRIGHT", keyboardBindingMenuOption, false);
		keyboardBindingMenuOption = this.AddKeyboardBindingLine(component.Parent, component.gameObject, GungeonActions.GungeonActionType.UseItem, "#OPTIONS_USEITEM", keyboardBindingMenuOption, false);
		keyboardBindingMenuOption = this.AddKeyboardBindingLine(component.Parent, component.gameObject, GungeonActions.GungeonActionType.Blank, "#OPTIONS_USEBLANK", keyboardBindingMenuOption, false);
		keyboardBindingMenuOption = this.AddKeyboardBindingLine(component.Parent, component.gameObject, GungeonActions.GungeonActionType.Map, "#OPTIONS_MAP", keyboardBindingMenuOption, false);
		bool flag = true;
		if (flag)
		{
			keyboardBindingMenuOption = this.AddKeyboardBindingLine(component.Parent, component.gameObject, GungeonActions.GungeonActionType.CycleGunUp, "#OPTIONS_CYCLEGUNUP", keyboardBindingMenuOption, false);
			keyboardBindingMenuOption = this.AddKeyboardBindingLine(component.Parent, component.gameObject, GungeonActions.GungeonActionType.CycleGunDown, "#OPTIONS_CYCLEGUNDOWN", keyboardBindingMenuOption, false);
			keyboardBindingMenuOption = this.AddKeyboardBindingLine(component.Parent, component.gameObject, GungeonActions.GungeonActionType.CycleItemUp, "#OPTIONS_CYCLEITEMUP", keyboardBindingMenuOption, false);
			keyboardBindingMenuOption = this.AddKeyboardBindingLine(component.Parent, component.gameObject, GungeonActions.GungeonActionType.CycleItemDown, "#OPTIONS_CYCLEITEMDOWN", keyboardBindingMenuOption, false);
		}
		keyboardBindingMenuOption = this.AddKeyboardBindingLine(component.Parent, component.gameObject, GungeonActions.GungeonActionType.GunQuickEquip, "#OPTIONS_GUNMENU", keyboardBindingMenuOption, false);
		bool flag2 = true;
		if (flag2)
		{
			keyboardBindingMenuOption = this.AddKeyboardBindingLine(component.Parent, component.gameObject, GungeonActions.GungeonActionType.DropGun, "#OPTIONS_DROPGUN", keyboardBindingMenuOption, false);
			keyboardBindingMenuOption = this.AddKeyboardBindingLine(component.Parent, component.gameObject, GungeonActions.GungeonActionType.DropItem, "#OPTIONS_DROPITEM", keyboardBindingMenuOption, false);
		}
		keyboardBindingMenuOption = this.AddKeyboardBindingLine(component.Parent, component.gameObject, GungeonActions.GungeonActionType.Pause, "#OPTIONS_PAUSE", keyboardBindingMenuOption, false);
		keyboardBindingMenuOption = this.AddKeyboardBindingLine(component.Parent, component.gameObject, GungeonActions.GungeonActionType.EquipmentMenu, "#OPTIONS_INVENTORY", keyboardBindingMenuOption, false);
		if (GameManager.Options.allowUnknownControllers)
		{
			keyboardBindingMenuOption = this.AddKeyboardBindingLine(component.Parent, component.gameObject, GungeonActions.GungeonActionType.SelectUp, "#OPTIONS_MENUUP", keyboardBindingMenuOption, false);
			keyboardBindingMenuOption = this.AddKeyboardBindingLine(component.Parent, component.gameObject, GungeonActions.GungeonActionType.SelectDown, "#OPTIONS_MENUDOWN", keyboardBindingMenuOption, false);
			keyboardBindingMenuOption = this.AddKeyboardBindingLine(component.Parent, component.gameObject, GungeonActions.GungeonActionType.SelectLeft, "#OPTIONS_MENULEFT", keyboardBindingMenuOption, false);
			keyboardBindingMenuOption = this.AddKeyboardBindingLine(component.Parent, component.gameObject, GungeonActions.GungeonActionType.SelectRight, "#OPTIONS_MENURIGHT", keyboardBindingMenuOption, false);
			keyboardBindingMenuOption = this.AddKeyboardBindingLine(component.Parent, component.gameObject, GungeonActions.GungeonActionType.MenuInteract, "#OPTIONS_MENUSELECT", keyboardBindingMenuOption, false);
			keyboardBindingMenuOption = this.AddKeyboardBindingLine(component.Parent, component.gameObject, GungeonActions.GungeonActionType.Cancel, "#OPTIONS_MENUCANCEL", keyboardBindingMenuOption, false);
		}
		if (PunchoutController.IsActive)
		{
			keyboardBindingMenuOption = this.AddKeyboardBindingLine(component.Parent, component.gameObject, GungeonActions.GungeonActionType.PunchoutDodgeLeft, "#OPTIONS_PUNCHOUT_DODGELEFT", keyboardBindingMenuOption, false);
			keyboardBindingMenuOption = this.AddKeyboardBindingLine(component.Parent, component.gameObject, GungeonActions.GungeonActionType.PunchoutDodgeRight, "#OPTIONS_PUNCHOUT_DODGERIGHT", keyboardBindingMenuOption, false);
			keyboardBindingMenuOption = this.AddKeyboardBindingLine(component.Parent, component.gameObject, GungeonActions.GungeonActionType.PunchoutBlock, "#OPTIONS_PUNCHOUT_BLOCK", keyboardBindingMenuOption, false);
			keyboardBindingMenuOption = this.AddKeyboardBindingLine(component.Parent, component.gameObject, GungeonActions.GungeonActionType.PunchoutDuck, "#OPTIONS_PUNCHOUT_DUCK", keyboardBindingMenuOption, false);
			keyboardBindingMenuOption = this.AddKeyboardBindingLine(component.Parent, component.gameObject, GungeonActions.GungeonActionType.PunchoutPunchLeft, "#OPTIONS_PUNCHOUT_PUNCHLEFT", keyboardBindingMenuOption, false);
			keyboardBindingMenuOption = this.AddKeyboardBindingLine(component.Parent, component.gameObject, GungeonActions.GungeonActionType.PunchoutPunchRight, "#OPTIONS_PUNCHOUT_PUNCHRIGHT", keyboardBindingMenuOption, false);
			keyboardBindingMenuOption = this.AddKeyboardBindingLine(component.Parent, component.gameObject, GungeonActions.GungeonActionType.PunchoutSuper, "#OPTIONS_PUNCHOUT_SUPER", keyboardBindingMenuOption, false);
		}
	}

	// Token: 0x06008F71 RID: 36721 RVA: 0x003CACA4 File Offset: 0x003C8EA4
	private KeyboardBindingMenuOption AddKeyboardBindingLine(dfControl parentPanel, GameObject prefabObject, GungeonActions.GungeonActionType actionType, string CommandStringKey, KeyboardBindingMenuOption previousMenuOption, bool nonbindable = false)
	{
		dfControl dfControl = parentPanel.AddPrefab(prefabObject);
		dfControl.transform.localScale = prefabObject.transform.localScale;
		KeyboardBindingMenuOption component = dfControl.GetComponent<KeyboardBindingMenuOption>();
		component.ActionType = actionType;
		component.CommandLabel.Text = CommandStringKey;
		component.NonBindable = nonbindable;
		component.KeyButton.GetComponent<UIKeyControls>().up = previousMenuOption.KeyButton;
		component.AltKeyButton.GetComponent<UIKeyControls>().up = previousMenuOption.AltKeyButton;
		previousMenuOption.KeyButton.GetComponent<UIKeyControls>().down = component.KeyButton;
		previousMenuOption.AltKeyButton.GetComponent<UIKeyControls>().down = component.AltKeyButton;
		component.KeyButton.Click += new MouseEventHandler(component.KeyClicked);
		component.AltKeyButton.Click += new MouseEventHandler(component.AltKeyClicked);
		component.Initialize();
		this.m_keyboardBindingLines.Add(component);
		return component;
	}

	// Token: 0x06008F72 RID: 36722 RVA: 0x003CAD90 File Offset: 0x003C8F90
	public void RegisterItem(BraveOptionsMenuItem item)
	{
		if (this.m_menuItems == null)
		{
			this.m_menuItems = new List<BraveOptionsMenuItem>();
		}
		this.m_menuItems.Add(item);
	}

	// Token: 0x06008F73 RID: 36723 RVA: 0x003CADB4 File Offset: 0x003C8FB4
	public void ReinitializeFromOptions()
	{
		for (int i = 0; i < this.m_menuItems.Count; i++)
		{
			this.m_menuItems[i].InitializeFromOptions();
		}
	}

	// Token: 0x06008F74 RID: 36724 RVA: 0x003CADF0 File Offset: 0x003C8FF0
	private void ShowOptionsMenu()
	{
		dfGUIManager.PushModal(this.m_panel);
		this.cloneOptions = GameOptions.CloneOptions(GameManager.Options);
		if (!this.finishedInitialization)
		{
			this.finishedInitialization = true;
			this.InitializeKeyboardBindingsPanel();
		}
	}

	// Token: 0x06008F75 RID: 36725 RVA: 0x003CAE28 File Offset: 0x003C9028
	private void Update()
	{
		if (this.m_panel.IsVisible)
		{
			this.HandleLanguageSpecificModifications();
			if (this.m_justResetToDefaultsWithPrompt > 0f)
			{
				this.m_justResetToDefaultsWithPrompt -= GameManager.INVARIANT_DELTA_TIME;
			}
		}
	}

	// Token: 0x06008F76 RID: 36726 RVA: 0x003CAE64 File Offset: 0x003C9064
	private void ResetToDefaultsWithPrompt()
	{
		if (this.m_justResetToDefaultsWithPrompt > 0f)
		{
			return;
		}
		this.m_justResetToDefaultsWithPrompt = 0.25f;
		this.m_cachedFocusedControl = dfGUIManager.ActiveControl;
		this.m_panel.IsVisible = false;
		GameUIRoot.Instance.DoAreYouSure("#AYS_RESETDEFAULTS", false, null);
		base.StartCoroutine(this.WaitForAreYouSure(delegate
		{
			this.m_panel.IsVisible = true;
			if (this.m_cachedFocusedControl != null)
			{
				this.m_cachedFocusedControl.Focus(true);
			}
			this.ResetToDefaults();
		}, delegate
		{
			this.m_panel.IsVisible = true;
			if (this.m_cachedFocusedControl != null)
			{
				this.m_cachedFocusedControl.Focus(true);
			}
		}));
	}

	// Token: 0x06008F77 RID: 36727 RVA: 0x003CAEDC File Offset: 0x003C90DC
	private void ResetToDefaults()
	{
		GameManager.Options.RevertToDefaults();
		BraveInput.ResetBindingsToDefaults();
		this.ReinitializeKeyboardBindings();
		this.ReinitializeFromOptions();
	}

	// Token: 0x06008F78 RID: 36728 RVA: 0x003CAEFC File Offset: 0x003C90FC
	public void CloseAndApplyChangesWithPrompt()
	{
		this.m_cachedFocusedControl = dfGUIManager.ActiveControl;
		this.m_panel.IsVisible = false;
		GameUIRoot.Instance.DoAreYouSure("#AYS_SAVEOPTIONS", false, null);
		base.StartCoroutine(this.WaitForAreYouSure(new Action(this.CloseAndApplyChanges), delegate
		{
			this.m_panel.IsVisible = true;
			if (this.m_cachedFocusedControl != null)
			{
				this.m_cachedFocusedControl.Focus(true);
			}
		}));
	}

	// Token: 0x06008F79 RID: 36729 RVA: 0x003CAF58 File Offset: 0x003C9158
	public void CloseAndMaybeApplyChangesWithPrompt()
	{
		if (this.cloneOptions == null)
		{
			return;
		}
		SaveManager.TargetSaveSlot = null;
		BraveInput.SaveBindingInfoToOptions();
		if (GameOptions.CompareSettings(this.cloneOptions, GameManager.Options))
		{
			this.CloseAndRevertChanges();
		}
		else
		{
			this.m_cachedFocusedControl = dfGUIManager.ActiveControl;
			this.m_panel.IsVisible = false;
			GameUIRoot.Instance.DoAreYouSure("#AYS_SAVEOPTIONS", false, null);
			base.StartCoroutine(this.WaitForAreYouSure(new Action(this.CloseAndApplyChanges), new Action(this.CloseAndRevertChanges)));
		}
	}

	// Token: 0x06008F7A RID: 36730 RVA: 0x003CAFF4 File Offset: 0x003C91F4
	private IEnumerator WaitForAreYouSure(Action OnYes, Action OnNo)
	{
		while (!GameUIRoot.Instance.HasSelectedAreYouSureOption())
		{
			yield return null;
		}
		if (GameUIRoot.Instance.GetAreYouSureOption())
		{
			if (OnYes != null)
			{
				OnYes();
			}
		}
		else if (OnNo != null)
		{
			OnNo();
		}
		yield break;
	}

	// Token: 0x06008F7B RID: 36731 RVA: 0x003CB018 File Offset: 0x003C9218
	private void CloseAndApplyChanges()
	{
		this.cloneOptions = null;
		BraveInput.SaveBindingInfoToOptions();
		GameOptions.Save();
		this.UpAllLevels();
	}

	// Token: 0x06008F7C RID: 36732 RVA: 0x003CB034 File Offset: 0x003C9234
	private void CloseAndRevertChangesWithPrompt()
	{
		BraveInput.SaveBindingInfoToOptions();
		if (GameOptions.CompareSettings(this.cloneOptions, GameManager.Options))
		{
			this.CloseAndRevertChanges();
		}
		else
		{
			this.m_cachedFocusedControl = dfGUIManager.ActiveControl;
			this.m_panel.IsVisible = false;
			GameUIRoot.Instance.DoAreYouSure("#AYS_MADECHANGES", true, null);
			base.StartCoroutine(this.WaitForAreYouSure(new Action(this.CloseAndRevertChanges), new Action(this.CloseAndApplyChanges)));
		}
	}

	// Token: 0x06008F7D RID: 36733 RVA: 0x003CB0B4 File Offset: 0x003C92B4
	private void CloseAndRevertChanges()
	{
		if (this.cloneOptions != null)
		{
			GameManager.Options.CurrentLanguage = this.cloneOptions.CurrentLanguage;
			GameManager.Options.ApplySettings(this.cloneOptions);
		}
		else
		{
			Debug.LogError("Clone Options is NULL: this should never happen.");
		}
		this.cloneOptions = null;
		this.ReinitializeFromOptions();
		StringTableManager.SetNewLanguage(GameManager.Options.CurrentLanguage, false);
		GameOptions.Save();
		this.UpAllLevels();
	}

	// Token: 0x06008F7E RID: 36734 RVA: 0x003CB12C File Offset: 0x003C932C
	private IEnumerator Start()
	{
		this.PrimaryCancelButton.GotFocus += this.BottomOptionFocused;
		this.PrimaryResetDefaultsButton.GotFocus += this.BottomOptionFocused;
		this.PrimaryConfirmButton.GotFocus += this.BottomOptionFocused;
		this.PrimaryCancelButton.Click += delegate(dfControl control, dfMouseEventArgs mouseEvent)
		{
			this.CloseAndRevertChangesWithPrompt();
		};
		this.PrimaryResetDefaultsButton.Click += delegate(dfControl control, dfMouseEventArgs mouseEvent)
		{
			this.ResetToDefaultsWithPrompt();
		};
		this.PrimaryConfirmButton.Click += delegate(dfControl control, dfMouseEventArgs mouseEvent)
		{
			this.CloseAndApplyChanges();
		};
		yield return null;
		yield return null;
		yield return null;
		this.DisableHierarchy();
		yield break;
	}

	// Token: 0x06008F7F RID: 36735 RVA: 0x003CB148 File Offset: 0x003C9348
	private void BottomOptionFocused(dfControl control, dfFocusEventArgs args)
	{
		this.m_lastSelectedBottomRowControl = control;
		if (this.TabAudio.IsVisible)
		{
			this.TabAudio.Controls[this.TabAudio.Controls.Count - 1].GetComponent<BraveOptionsMenuItem>().down = this.m_lastSelectedBottomRowControl;
		}
		else if (this.TabVideo.IsVisible)
		{
			this.TabVideo.Controls[this.TabVideo.Controls.Count - 1].GetComponent<BraveOptionsMenuItem>().down = this.m_lastSelectedBottomRowControl;
		}
		else if (this.TabControls.IsVisible)
		{
			this.TabControls.Controls[this.TabControls.Controls.Count - 2].GetComponent<BraveOptionsMenuItem>().down = this.m_lastSelectedBottomRowControl;
		}
		else if (this.TabGameplay.IsVisible)
		{
			this.TabGameplay.Controls[this.TabGameplay.Controls.Count - 1].GetComponent<BraveOptionsMenuItem>().down = this.m_lastSelectedBottomRowControl;
		}
		else if (this.TabKeyboardBindings.IsVisible)
		{
			this.TabKeyboardBindings.Controls[this.TabKeyboardBindings.Controls.Count - 1].GetComponent<KeyboardBindingMenuOption>().KeyButton.GetComponent<UIKeyControls>().down = this.m_lastSelectedBottomRowControl;
			this.TabKeyboardBindings.Controls[this.TabKeyboardBindings.Controls.Count - 1].GetComponent<KeyboardBindingMenuOption>().AltKeyButton.GetComponent<UIKeyControls>().down = this.m_lastSelectedBottomRowControl;
		}
	}

	// Token: 0x06008F80 RID: 36736 RVA: 0x003CB300 File Offset: 0x003C9500
	public void UpAllLevels()
	{
		InControlInputAdapter.CurrentlyUsingAllDevices = false;
		if (this.ModalKeyBindingDialog.IsVisible)
		{
			this.ClearModalKeyBindingDialog(null, null);
		}
		else
		{
			this.PreOptionsMenu.ReturnToPreOptionsMenu();
		}
	}

	// Token: 0x06008F81 RID: 36737 RVA: 0x003CB330 File Offset: 0x003C9530
	public void ToggleToHowToPlay()
	{
		this.TabGameplay.IsVisible = false;
		this.TabHowToPlay.IsVisible = true;
		this.TabHowToPlay.Controls[0].Focus(true);
	}

	// Token: 0x06008F82 RID: 36738 RVA: 0x003CB364 File Offset: 0x003C9564
	public void ToggleToCredits()
	{
		this.TabGameplay.IsVisible = false;
		this.TabCredits.IsVisible = true;
		this.TabCredits.Controls[0].Focus(true);
	}

	// Token: 0x06008F83 RID: 36739 RVA: 0x003CB398 File Offset: 0x003C9598
	public void ToggleToKeyboardBindingsPanel(bool isController)
	{
		this.FullyReinitializeKeyboardBindings();
		this.SwitchBindingsMenuMode(isController);
		this.TabHowToPlay.IsVisible = false;
		this.TabCredits.IsVisible = false;
		this.TabAudio.IsVisible = false;
		this.TabVideo.IsVisible = false;
		this.TabGameplay.IsVisible = false;
		this.TabControls.IsVisible = false;
		this.TabKeyboardBindings.IsVisible = true;
		int num = 0;
		BraveOptionsMenuItem component = this.TabKeyboardBindings.Controls[num].GetComponent<BraveOptionsMenuItem>();
		component.optionType = ((FullOptionsMenuController.CurrentBindingPlayerTargetIndex != 0) ? BraveOptionsMenuItem.BraveOptionsOptionType.CURRENT_BINDINGS_PRESET_P2 : BraveOptionsMenuItem.BraveOptionsOptionType.CURRENT_BINDINGS_PRESET);
		component.InitializeFromOptions();
		KeyboardBindingMenuOption component2 = this.TabKeyboardBindings.Controls[this.TabKeyboardBindings.Controls.Count - 1].GetComponent<KeyboardBindingMenuOption>();
		component2.KeyButton.GetComponent<UIKeyControls>().down = this.m_lastSelectedBottomRowControl ?? this.PrimaryConfirmButton;
		component2.AltKeyButton.GetComponent<UIKeyControls>().down = this.m_lastSelectedBottomRowControl ?? this.PrimaryConfirmButton;
		this.PrimaryCancelButton.GetComponent<UIKeyControls>().up = component2.KeyButton;
		this.PrimaryConfirmButton.GetComponent<UIKeyControls>().up = component2.KeyButton;
		this.PrimaryResetDefaultsButton.GetComponent<UIKeyControls>().up = component2.KeyButton;
		this.PrimaryCancelButton.GetComponent<UIKeyControls>().down = this.TabKeyboardBindings.Controls[0];
		this.PrimaryConfirmButton.GetComponent<UIKeyControls>().down = this.TabKeyboardBindings.Controls[0];
		this.PrimaryResetDefaultsButton.GetComponent<UIKeyControls>().down = this.TabKeyboardBindings.Controls[0];
		this.TabKeyboardBindings.Controls[0].GetComponent<BraveOptionsMenuItem>().up = this.PrimaryConfirmButton;
		this.TabKeyboardBindings.Controls[0].Focus(true);
		if (PunchoutController.IsActive)
		{
			this.TabKeyboardBindings.Controls[this.TabKeyboardBindings.Controls.Count - 7].GetComponent<KeyboardBindingMenuOption>().KeyButton.Focus(true);
			this.TabKeyboardBindings.ScrollToBottom();
		}
	}

	// Token: 0x06008F84 RID: 36740 RVA: 0x003CB5D0 File Offset: 0x003C97D0
	public void HandleLanguageSpecificModifications()
	{
		if (!this.m_hasCachedPositions)
		{
			this.m_hasCachedPositions = true;
			this.m_cachedRelativePositionPrimaryConfirm = this.PrimaryConfirmButton.RelativePosition;
			this.m_cachedRelativePositionPrimaryCancel = this.PrimaryCancelButton.RelativePosition;
		}
		if (GameManager.Options.CurrentLanguage != this.m_cachedLanguage)
		{
			if (GameManager.Options.CurrentLanguage == StringTableManager.GungeonSupportedLanguages.ENGLISH || GameManager.Options.CurrentLanguage == StringTableManager.GungeonSupportedLanguages.KOREAN || GameManager.Options.CurrentLanguage == StringTableManager.GungeonSupportedLanguages.CHINESE || GameManager.Options.CurrentLanguage == StringTableManager.GungeonSupportedLanguages.JAPANESE)
			{
				this.PrimaryConfirmButton.RelativePosition = this.m_cachedRelativePositionPrimaryConfirm;
				this.PrimaryCancelButton.RelativePosition = this.m_cachedRelativePositionPrimaryCancel;
			}
			else
			{
				this.PrimaryConfirmButton.RelativePosition = this.m_cachedRelativePositionPrimaryConfirm + new Vector2(15f, 0f);
				this.PrimaryCancelButton.RelativePosition = this.m_cachedRelativePositionPrimaryCancel + new Vector2(-45f, 0f);
			}
			this.m_cachedLanguage = GameManager.Options.CurrentLanguage;
		}
	}

	// Token: 0x06008F85 RID: 36741 RVA: 0x003CB708 File Offset: 0x003C9908
	public void ToggleToPanel(dfScrollPanel targetPanel, bool doFocus = false)
	{
		this.IsVisible = true;
		this.TabHowToPlay.IsVisible = false;
		this.TabCredits.IsVisible = false;
		if (this.TabKeyboardBindings.IsVisible)
		{
			BraveInput.SaveBindingInfoToOptions();
		}
		this.TabKeyboardBindings.IsVisible = false;
		this.TabAudio.IsVisible = targetPanel == this.TabAudio;
		this.TabVideo.IsVisible = targetPanel == this.TabVideo;
		this.TabGameplay.IsVisible = targetPanel == this.TabGameplay;
		this.TabControls.IsVisible = targetPanel == this.TabControls;
		this.PrimaryCancelButton.GetComponent<UIKeyControls>().down = targetPanel.Controls[0];
		this.PrimaryConfirmButton.GetComponent<UIKeyControls>().down = targetPanel.Controls[0];
		this.PrimaryResetDefaultsButton.GetComponent<UIKeyControls>().down = targetPanel.Controls[0];
		targetPanel.Controls[0].Focus(true);
		BraveOptionsMenuItem component = targetPanel.Controls[0].GetComponent<BraveOptionsMenuItem>();
		if (component)
		{
			component.up = this.PrimaryConfirmButton;
		}
		for (int i = targetPanel.Controls.Count - 1; i > 0; i--)
		{
			BraveOptionsMenuItem component2 = targetPanel.Controls[i].GetComponent<BraveOptionsMenuItem>();
			if (component2 != null && component2.GetComponent<dfControl>().IsEnabled)
			{
				component2.down = ((!(this.m_lastSelectedBottomRowControl != null)) ? this.PrimaryConfirmButton : this.m_lastSelectedBottomRowControl);
				this.PrimaryCancelButton.GetComponent<UIKeyControls>().up = targetPanel.Controls[i];
				this.PrimaryConfirmButton.GetComponent<UIKeyControls>().up = targetPanel.Controls[i];
				this.PrimaryResetDefaultsButton.GetComponent<UIKeyControls>().up = targetPanel.Controls[i];
				break;
			}
		}
		if (doFocus)
		{
			targetPanel.Controls[0].Focus(true);
		}
	}

	// Token: 0x06008F86 RID: 36742 RVA: 0x003CB928 File Offset: 0x003C9B28
	public void ShwoopOpen()
	{
		base.StartCoroutine(this.HandleShwoop(false));
	}

	// Token: 0x06008F87 RID: 36743 RVA: 0x003CB938 File Offset: 0x003C9B38
	private IEnumerator HandleShwoop(bool reverse)
	{
		this.m_justResetToDefaultsWithPrompt = 0f;
		float timer = 0.1f;
		float elapsed = 0f;
		Vector3 smallScale = new Vector3(0.01f, 0.01f, 1f);
		Vector3 bigScale = Vector3.one;
		PauseMenuController pmc = GameUIRoot.Instance.PauseMenuPanel.GetComponent<PauseMenuController>();
		while (elapsed < timer)
		{
			elapsed += GameManager.INVARIANT_DELTA_TIME;
			float t = elapsed / timer;
			AnimationCurve targetCurve = ((!reverse) ? pmc.ShwoopInCurve : pmc.ShwoopOutCurve);
			this.m_panel.transform.localScale = smallScale + bigScale * Mathf.Clamp01(targetCurve.Evaluate(t));
			this.m_panel.Opacity = Mathf.Lerp(0f, 1f, (!reverse) ? ((t - 0.5f) * 2f) : (1f - t * 2f));
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
			this.DisableHierarchy();
		}
		yield break;
	}

	// Token: 0x06008F88 RID: 36744 RVA: 0x003CB95C File Offset: 0x003C9B5C
	public void ShwoopClosed()
	{
		base.StartCoroutine(this.HandleShwoop(true));
	}

	// Token: 0x040097A8 RID: 38824
	public dfButton PrimaryCancelButton;

	// Token: 0x040097A9 RID: 38825
	public dfButton PrimaryResetDefaultsButton;

	// Token: 0x040097AA RID: 38826
	public dfButton PrimaryConfirmButton;

	// Token: 0x040097AB RID: 38827
	public dfScrollPanel TabAudio;

	// Token: 0x040097AC RID: 38828
	public dfScrollPanel TabVideo;

	// Token: 0x040097AD RID: 38829
	public dfScrollPanel TabGameplay;

	// Token: 0x040097AE RID: 38830
	public dfScrollPanel TabControls;

	// Token: 0x040097AF RID: 38831
	public dfScrollPanel TabKeyboardBindings;

	// Token: 0x040097B0 RID: 38832
	public dfScrollPanel TabCredits;

	// Token: 0x040097B1 RID: 38833
	public dfScrollPanel TabHowToPlay;

	// Token: 0x040097B2 RID: 38834
	public dfPanel ModalKeyBindingDialog;

	// Token: 0x040097B3 RID: 38835
	public PreOptionsMenuController PreOptionsMenu;

	// Token: 0x040097B4 RID: 38836
	protected GameOptions cloneOptions;

	// Token: 0x040097B5 RID: 38837
	protected dfPanel m_panel;

	// Token: 0x040097B6 RID: 38838
	private bool finishedInitialization;

	// Token: 0x040097B7 RID: 38839
	private List<BraveOptionsMenuItem> m_menuItems;

	// Token: 0x040097B8 RID: 38840
	private dfControl m_cachedFocusedControl;

	// Token: 0x040097B9 RID: 38841
	private dfControl m_lastSelectedBottomRowControl;

	// Token: 0x040097BA RID: 38842
	private List<KeyboardBindingMenuOption> m_keyboardBindingLines = new List<KeyboardBindingMenuOption>();

	// Token: 0x040097BB RID: 38843
	private Vector2 m_cachedResolution;

	// Token: 0x040097BC RID: 38844
	public static int CurrentBindingPlayerTargetIndex;

	// Token: 0x040097BD RID: 38845
	private bool m_firstTimeBindingsInitialization = true;

	// Token: 0x040097BE RID: 38846
	private float m_justResetToDefaultsWithPrompt;

	// Token: 0x040097BF RID: 38847
	private Vector2 m_cachedRelativePositionPrimaryConfirm;

	// Token: 0x040097C0 RID: 38848
	private Vector2 m_cachedRelativePositionPrimaryCancel;

	// Token: 0x040097C1 RID: 38849
	private StringTableManager.GungeonSupportedLanguages m_cachedLanguage;

	// Token: 0x040097C2 RID: 38850
	private bool m_hasCachedPositions;
}
