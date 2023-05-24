using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using InControl;
using UnityEngine;
using UnityEngine.Analytics;

// Token: 0x020017C9 RID: 6089
public class BraveOptionsMenuItem : MonoBehaviour
{
	// Token: 0x17001567 RID: 5479
	// (get) Token: 0x06008EDF RID: 36575 RVA: 0x003C4738 File Offset: 0x003C2938
	public static BraveOptionsMenuItem.WindowsResolutionManager ResolutionManagerWin
	{
		get
		{
			if (BraveOptionsMenuItem.m_windowsResolutionManager == null)
			{
				BraveOptionsMenuItem.m_windowsResolutionManager = new BraveOptionsMenuItem.WindowsResolutionManager("Enter the Gungeon");
			}
			return BraveOptionsMenuItem.m_windowsResolutionManager;
		}
	}

	// Token: 0x06008EE0 RID: 36576 RVA: 0x003C4758 File Offset: 0x003C2958
	private void OnDestroy()
	{
		if (this.optionType == BraveOptionsMenuItem.BraveOptionsOptionType.PLAYER_ONE_CONTROL_PORT || this.optionType == BraveOptionsMenuItem.BraveOptionsOptionType.PLAYER_TWO_CONTROL_PORT)
		{
			InputManager.OnDeviceAttached -= this.InputManager_OnDeviceAttached;
			InputManager.OnDeviceDetached -= this.InputManager_OnDeviceDetached;
		}
	}

	// Token: 0x06008EE1 RID: 36577 RVA: 0x003C4798 File Offset: 0x003C2998
	private void ToggleAbledness(bool value)
	{
		if (!value)
		{
			if (this.m_self)
			{
				this.m_self.Disable();
				this.m_self.CanFocus = false;
				this.m_self.IsInteractive = false;
				this.m_self.DisabledColor = new Color(0.2f, 0.2f, 0.2f, 1f);
			}
			if (this.labelControl)
			{
				this.labelControl.DisabledColor = new Color(0.2f, 0.2f, 0.2f, 1f);
			}
			if (this.left)
			{
				this.left.DisabledColor = new Color(0.2f, 0.2f, 0.2f, 1f);
			}
			if (this.right)
			{
				this.right.DisabledColor = new Color(0.2f, 0.2f, 0.2f, 1f);
			}
			if (this.selectedLabelControl)
			{
				this.selectedLabelControl.DisabledColor = new Color(0.2f, 0.2f, 0.2f, 1f);
			}
			if (this.buttonControl)
			{
				this.buttonControl.Disable();
				this.buttonControl.CanFocus = false;
				this.buttonControl.IsInteractive = false;
				this.buttonControl.DisabledColor = new Color(0.2f, 0.2f, 0.2f, 1f);
				this.buttonControl.DisabledTextColor = new Color(0.2f, 0.2f, 0.2f, 1f);
			}
			if (this.up && this.down)
			{
				this.up.GetComponent<BraveOptionsMenuItem>().down = this.down;
				this.down.GetComponent<BraveOptionsMenuItem>().up = this.up;
			}
			else if (this.up)
			{
				this.up.GetComponent<BraveOptionsMenuItem>().down = null;
			}
			else if (this.down)
			{
				this.down.GetComponent<BraveOptionsMenuItem>().up = null;
			}
		}
	}

	// Token: 0x06008EE2 RID: 36578 RVA: 0x003C4A08 File Offset: 0x003C2C08
	private bool DisablePlatformSpecificOptions()
	{
		if (this.optionType == BraveOptionsMenuItem.BraveOptionsOptionType.GAMEPLAY_SPEED)
		{
			this.DelControl();
			return true;
		}
		if (this.optionType == BraveOptionsMenuItem.BraveOptionsOptionType.SWITCH_PERFORMANCE_MODE || this.optionType == BraveOptionsMenuItem.BraveOptionsOptionType.SWITCH_REASSIGN_CONTROLLERS)
		{
			this.DelControl();
			return true;
		}
		if (this.optionType == BraveOptionsMenuItem.BraveOptionsOptionType.ADDITIONAL_BLANK_CONTROL_PS4 || this.optionType == BraveOptionsMenuItem.BraveOptionsOptionType.STICKY_FRICTION_AMOUNT || this.optionType == BraveOptionsMenuItem.BraveOptionsOptionType.BOTH_CONTROLLER_CURSOR)
		{
			this.DelControl();
			return true;
		}
		return false;
	}

	// Token: 0x06008EE3 RID: 36579 RVA: 0x003C4A7C File Offset: 0x003C2C7C
	public void Awake()
	{
		if (this.optionType == BraveOptionsMenuItem.BraveOptionsOptionType.FULLSCREEN)
		{
			if (BraveOptionsMenuItem.m_windowsResolutionManager == null)
			{
				BraveOptionsMenuItem.m_windowsResolutionManager = new BraveOptionsMenuItem.WindowsResolutionManager("Enter the Gungeon");
			}
			this.labelOptions = new string[] { "Fullscreen", "Borderless", "Windowed" };
		}
		dfControl component = base.GetComponent<dfControl>();
		this.m_self = component;
		component.IsVisibleChanged += this.self_IsVisibleChanged;
		this.m_isLocalized = component.IsLocalized;
		component.CanFocus = true;
		component.GotFocus += this.DoFocus;
		component.LostFocus += this.LostFocus;
		if ((this.optionType == BraveOptionsMenuItem.BraveOptionsOptionType.RESET_SAVE_SLOT || this.optionType == BraveOptionsMenuItem.BraveOptionsOptionType.SAVE_SLOT) && GameManager.Instance.CurrentLevelOverrideState != GameManager.LevelOverrideState.FOYER)
		{
			this.ToggleAbledness(false);
		}
		if (this.optionType == BraveOptionsMenuItem.BraveOptionsOptionType.LOOT_PROFILE && GameManager.Instance.CurrentLevelOverrideState != GameManager.LevelOverrideState.FOYER)
		{
			this.ToggleAbledness(false);
		}
		if (this.labelOptions != null && this.labelOptions.Length > 0 && this.labelOptions[0].StartsWith("#"))
		{
			this.selectedLabelControl.IsLocalized = true;
			this.selectedLabelControl.Localize();
		}
		this.RelocalizeOptions();
		if (this.optionType == BraveOptionsMenuItem.BraveOptionsOptionType.PLAYER_ONE_CONTROL_PORT || this.optionType == BraveOptionsMenuItem.BraveOptionsOptionType.PLAYER_TWO_CONTROL_PORT)
		{
			InputManager.OnDeviceAttached += this.InputManager_OnDeviceAttached;
			InputManager.OnDeviceDetached += this.InputManager_OnDeviceDetached;
		}
		if (this.optionType == BraveOptionsMenuItem.BraveOptionsOptionType.RESOLUTION || this.optionType == BraveOptionsMenuItem.BraveOptionsOptionType.SCALING_MODE || this.optionType == BraveOptionsMenuItem.BraveOptionsOptionType.VISUAL_PRESET)
		{
			dfControl dfControl = component;
			dfControl.ResolutionChangedPostLayout = (Action<dfControl, Vector3, Vector3>)Delegate.Combine(dfControl.ResolutionChangedPostLayout, new Action<dfControl, Vector3, Vector3>(this.HandleResolutionChanged));
		}
		if (this.itemType == BraveOptionsMenuItem.BraveOptionsMenuItemType.LeftRightArrow || this.itemType == BraveOptionsMenuItem.BraveOptionsMenuItemType.LeftRightArrowInfo)
		{
			if (this.left != null)
			{
				this.left.HotZoneScale = Vector2.one * 3f;
				this.left.Click += this.DecrementArrow;
			}
			if (this.right != null)
			{
				this.right.HotZoneScale = Vector2.one * 3f;
				this.right.Click += this.IncrementArrow;
			}
		}
		else if (this.itemType == BraveOptionsMenuItem.BraveOptionsMenuItemType.Fillbar)
		{
			this.fillbarControl.Click += this.HandleFillbarClick;
			this.fillbarControl.MouseDown += this.HandleFillbarDown;
			this.fillbarControl.MouseMove += this.HandleFillbarMove;
			this.fillbarControl.MouseHover += this.HandleFillbarHover;
		}
		else if (this.itemType == BraveOptionsMenuItem.BraveOptionsMenuItemType.Checkbox)
		{
			component.Click += this.ToggleCheckbox;
			this.checkboxUnchecked.Click += this.ToggleCheckbox;
			this.checkboxUnchecked.IsInteractive = false;
			this.checkboxChecked.IsInteractive = false;
		}
		else if (this.itemType == BraveOptionsMenuItem.BraveOptionsMenuItemType.Button)
		{
			this.buttonControl.Click += this.OnButtonClicked;
		}
		if (this.itemType == BraveOptionsMenuItem.BraveOptionsMenuItemType.LeftRightArrow || this.itemType == BraveOptionsMenuItem.BraveOptionsMenuItemType.LeftRightArrowInfo)
		{
			this.left.Color = BraveOptionsMenuItem.m_unselectedColor;
			this.right.Color = BraveOptionsMenuItem.m_unselectedColor;
			this.selectedLabelControl.Color = BraveOptionsMenuItem.m_unselectedColor;
		}
		else if (this.itemType == BraveOptionsMenuItem.BraveOptionsMenuItemType.Checkbox)
		{
			this.checkboxChecked.Color = BraveOptionsMenuItem.m_unselectedColor;
			this.checkboxUnchecked.Color = BraveOptionsMenuItem.m_unselectedColor;
		}
	}

	// Token: 0x06008EE4 RID: 36580 RVA: 0x003C4E60 File Offset: 0x003C3060
	private void HandleLocalTextChangeReposition(dfControl control, string value)
	{
		if (this.labelControl)
		{
			this.labelControl.Pivot = dfPivotPoint.BottomLeft;
		}
	}

	// Token: 0x06008EE5 RID: 36581 RVA: 0x003C4E80 File Offset: 0x003C3080
	private void InputManager_OnDeviceAttached(InputDevice obj)
	{
		this.InitializeFromOptions();
	}

	// Token: 0x06008EE6 RID: 36582 RVA: 0x003C4E88 File Offset: 0x003C3088
	private void InputManager_OnDeviceDetached(InputDevice obj)
	{
		this.InitializeFromOptions();
	}

	// Token: 0x06008EE7 RID: 36583 RVA: 0x003C4E90 File Offset: 0x003C3090
	private void OnButtonClicked(dfControl control, dfMouseEventArgs mouseEvent)
	{
		this.DoSelectedAction();
	}

	// Token: 0x06008EE8 RID: 36584 RVA: 0x003C4E98 File Offset: 0x003C3098
	private void ToggleCheckbox(dfControl control, dfMouseEventArgs args)
	{
		if (this.m_changedThisFrame || (args != null && args.Used))
		{
			return;
		}
		this.m_changedThisFrame = true;
		if (args != null)
		{
			args.Use();
		}
		this.m_selectedIndex = (this.m_selectedIndex + 1) % 2;
		this.checkboxChecked.IsVisible = this.m_selectedIndex == 1;
		this.HandleCheckboxValueChanged();
	}

	// Token: 0x06008EE9 RID: 36585 RVA: 0x003C4F00 File Offset: 0x003C3100
	private void self_IsVisibleChanged(dfControl control, bool value)
	{
		if (value)
		{
			this.ConvertPivots();
			dfControl component = base.GetComponent<dfControl>();
			if (component)
			{
				component.PerformLayout();
			}
			this.UpdateSelectedLabelText();
			this.UpdateInfoControl();
		}
	}

	// Token: 0x06008EEA RID: 36586 RVA: 0x003C4F40 File Offset: 0x003C3140
	private void RelocalizeOptions()
	{
		if (this.m_isLocalized && this.optionType == BraveOptionsMenuItem.BraveOptionsOptionType.SCALING_MODE)
		{
			for (int i = 0; i < this.labelOptions.Length; i++)
			{
				string text = this.labelOptions[i];
				string value = this.m_self.GetLanguageManager().GetValue(text);
				this.labelOptions[i] = value;
			}
			for (int j = 0; j < this.infoOptions.Length; j++)
			{
				string text2 = this.infoOptions[j];
				this.infoOptions[j] = this.m_self.GetLanguageManager().GetValue(text2);
			}
		}
	}

	// Token: 0x06008EEB RID: 36587 RVA: 0x003C4FE0 File Offset: 0x003C31E0
	public void LateUpdate()
	{
		this.m_changedThisFrame = false;
	}

	// Token: 0x06008EEC RID: 36588 RVA: 0x003C4FEC File Offset: 0x003C31EC
	public void Update()
	{
		if (this.labelControl != null && this.labelControl.IsVisible)
		{
			if (GameManager.Options.CurrentVisualPreset == GameOptions.VisualPresetMode.RECOMMENDED)
			{
				if (this.optionType == BraveOptionsMenuItem.BraveOptionsOptionType.RESOLUTION)
				{
					Resolution recommendedResolution = GameManager.Options.GetRecommendedResolution();
					if (Screen.width != recommendedResolution.width || Screen.height != recommendedResolution.height)
					{
						BraveOptionsMenuItem.HandleScreenDataChanged(recommendedResolution.width, recommendedResolution.height);
					}
				}
				else if (this.optionType == BraveOptionsMenuItem.BraveOptionsOptionType.SCALING_MODE)
				{
					if (GameManager.Options.CurrentPreferredScalingMode != GameManager.Options.GetRecommendedScalingMode())
					{
						GameManager.Options.CurrentPreferredScalingMode = GameManager.Options.GetRecommendedScalingMode();
						BraveOptionsMenuItem.HandleScreenDataChanged(Screen.width, Screen.height);
					}
				}
				else if (this.optionType == BraveOptionsMenuItem.BraveOptionsOptionType.FULLSCREEN && GameManager.Options.CurrentPreferredFullscreenMode != GameOptions.PreferredFullscreenMode.FULLSCREEN)
				{
					GameManager.Options.CurrentPreferredFullscreenMode = GameOptions.PreferredFullscreenMode.FULLSCREEN;
					BraveOptionsMenuItem.HandleScreenDataChanged(Screen.width, Screen.height);
				}
			}
			if (this.optionType == BraveOptionsMenuItem.BraveOptionsOptionType.FULLSCREEN)
			{
				int indexFromFullscreenMode = this.GetIndexFromFullscreenMode(GameManager.Options.CurrentPreferredFullscreenMode);
				if (this.m_selectedIndex != indexFromFullscreenMode)
				{
					this.m_selectedIndex = indexFromFullscreenMode;
					this.DetermineAvailableOptions();
				}
			}
		}
		if (this.optionType == BraveOptionsMenuItem.BraveOptionsOptionType.SCALING_MODE && this.m_scalingModes != null && this.m_scalingModes.Count > 0)
		{
			if (this.m_selectedIndex < 0 || this.m_selectedIndex >= this.m_scalingModes.Count)
			{
				this.m_selectedIndex = this.GetScalingIndex(GameManager.Options.CurrentPreferredScalingMode);
			}
			if (this.m_selectedIndex < 0 || this.m_selectedIndex >= this.m_scalingModes.Count)
			{
				this.m_selectedIndex = this.GetScalingIndex(GameOptions.PreferredScalingMode.PIXEL_PERFECT);
			}
			if (this.m_selectedIndex < 0 || this.m_selectedIndex >= this.m_scalingModes.Count)
			{
				this.m_selectedIndex = this.GetScalingIndex(GameOptions.PreferredScalingMode.FORCE_PIXEL_PERFECT);
			}
			if (this.m_selectedIndex < 0 || this.m_selectedIndex >= this.m_scalingModes.Count)
			{
				if (this.m_scalingModes.Contains(GameOptions.PreferredScalingMode.FORCE_PIXEL_PERFECT))
				{
					this.m_selectedIndex = this.GetScalingIndex(GameOptions.PreferredScalingMode.FORCE_PIXEL_PERFECT);
				}
				else
				{
					this.m_selectedIndex = 0;
				}
			}
			if (this.m_selectedIndex < this.m_scalingModes.Count && this.m_scalingModes[this.m_selectedIndex] != GameManager.Options.CurrentPreferredScalingMode)
			{
				this.m_selectedIndex = this.GetScalingIndex(GameManager.Options.CurrentPreferredScalingMode);
				if (this.m_selectedIndex < this.labelOptions.Length && this.m_selectedIndex >= 0)
				{
					this.UpdateSelectedLabelText();
				}
			}
		}
		if (this.optionType == BraveOptionsMenuItem.BraveOptionsOptionType.VISUAL_PRESET && this.m_selectedIndex != (int)GameManager.Options.CurrentVisualPreset)
		{
			this.m_selectedIndex = (int)GameManager.Options.CurrentVisualPreset;
			if (this.m_selectedIndex >= 0 && this.m_selectedIndex < this.labelOptions.Length)
			{
				this.UpdateSelectedLabelText();
			}
		}
		if (this.optionType == BraveOptionsMenuItem.BraveOptionsOptionType.RESOLUTION && GameManager.Options.CurrentPreferredFullscreenMode == GameOptions.PreferredFullscreenMode.BORDERLESS)
		{
			int width = Screen.currentResolution.width;
			int height = Screen.currentResolution.height;
			BraveOptionsMenuItem.HandleScreenDataChanged(width, height);
		}
	}

	// Token: 0x06008EED RID: 36589 RVA: 0x003C5348 File Offset: 0x003C3548
	private void HandleResolutionChanged(dfControl arg1, Vector3 arg2, Vector3 arg3)
	{
		if (this.optionType == BraveOptionsMenuItem.BraveOptionsOptionType.RESOLUTION)
		{
			this.DetermineAvailableOptions();
		}
		else if (this.optionType == BraveOptionsMenuItem.BraveOptionsOptionType.SCALING_MODE)
		{
			this.DetermineAvailableOptions();
			if (this.m_scalingModes[this.m_selectedIndex] == GameOptions.PreferredScalingMode.PIXEL_PERFECT)
			{
				GameManager.Options.CurrentPreferredScalingMode = GameOptions.PreferredScalingMode.FORCE_PIXEL_PERFECT;
			}
			else if (this.m_scalingModes[this.m_selectedIndex] == GameOptions.PreferredScalingMode.UNIFORM_SCALING)
			{
				GameManager.Options.CurrentPreferredScalingMode = GameOptions.PreferredScalingMode.UNIFORM_SCALING;
			}
			else if (this.m_scalingModes[this.m_selectedIndex] == GameOptions.PreferredScalingMode.UNIFORM_SCALING_FAST)
			{
				GameManager.Options.CurrentPreferredScalingMode = GameOptions.PreferredScalingMode.UNIFORM_SCALING_FAST;
			}
			else if (this.m_scalingModes[this.m_selectedIndex] == GameOptions.PreferredScalingMode.FORCE_PIXEL_PERFECT)
			{
				GameManager.Options.CurrentPreferredScalingMode = GameOptions.PreferredScalingMode.FORCE_PIXEL_PERFECT;
			}
			BraveOptionsMenuItem.HandleScreenDataChanged(Screen.width, Screen.height);
		}
		else if (this.optionType == BraveOptionsMenuItem.BraveOptionsOptionType.FULLSCREEN)
		{
			this.m_selectedIndex = this.GetIndexFromFullscreenMode(GameManager.Options.CurrentPreferredFullscreenMode);
			this.DetermineAvailableOptions();
		}
	}

	// Token: 0x06008EEE RID: 36590 RVA: 0x003C5450 File Offset: 0x003C3650
	private IEnumerator DeferFunctionNumberOfFrames(int numFrames, Action action)
	{
		for (int i = 0; i < numFrames; i++)
		{
			yield return null;
		}
		action();
		yield break;
	}

	// Token: 0x06008EEF RID: 36591 RVA: 0x003C5474 File Offset: 0x003C3674
	private void UpdateSelectedLabelText()
	{
		if (!this.selectedLabelControl || this.labelOptions == null || this.labelOptions.Length == 0)
		{
			return;
		}
		if (this.m_selectedIndex < 0 || this.m_selectedIndex >= this.labelOptions.Length)
		{
			return;
		}
		string text = this.labelOptions[this.m_selectedIndex];
		if (text.StartsWith("%"))
		{
			string[] array = text.Split(new char[] { ' ' });
			string text2 = string.Empty;
			for (int i = 0; i < array.Length; i++)
			{
				text2 += StringTableManager.EvaluateReplacementToken(array[i]);
			}
			this.selectedLabelControl.ModifyLocalizedText(text2);
		}
		else
		{
			this.selectedLabelControl.Text = this.labelOptions[this.m_selectedIndex];
		}
		if (this.left.IsVisible && this.right.IsVisible)
		{
			if (this.m_cachedLeftArrowRelativePosition == null && (this.itemType == BraveOptionsMenuItem.BraveOptionsMenuItemType.LeftRightArrow || this.itemType == BraveOptionsMenuItem.BraveOptionsMenuItemType.LeftRightArrowInfo) && this.left)
			{
				this.m_cachedLeftArrowRelativePosition = new Vector3?(this.left.RelativePosition);
			}
			if (this.m_cachedRightArrowRelativePosition == null && (this.itemType == BraveOptionsMenuItem.BraveOptionsMenuItemType.LeftRightArrow || this.itemType == BraveOptionsMenuItem.BraveOptionsMenuItemType.LeftRightArrowInfo) && this.right)
			{
				this.m_cachedRightArrowRelativePosition = new Vector3?(this.right.RelativePosition);
			}
			this.left.PerformLayout();
			this.right.PerformLayout();
			if (this.m_cachedLeftArrowRelativePosition != null && this.m_cachedRightArrowRelativePosition != null)
			{
				float num = this.m_cachedRightArrowRelativePosition.Value.x - this.m_cachedLeftArrowRelativePosition.Value.x;
				float num2 = this.selectedLabelControl.Width + 45f;
				float num3 = Mathf.Max(num, num2).Quantize(3f, VectorConversions.Ceil);
				float num4 = this.right.RelativePosition.x - this.left.RelativePosition.x;
				float num5 = (num3 - num4) / 2f;
				this.left.RelativePosition = this.left.RelativePosition + new Vector3(-num5, 0f, 0f);
				this.right.RelativePosition = this.right.RelativePosition + new Vector3(num5, 0f, 0f);
			}
		}
	}

	// Token: 0x06008EF0 RID: 36592 RVA: 0x003C5724 File Offset: 0x003C3924
	private void InitializeVisualPreset()
	{
		Resolution recommendedResolution = GameManager.Options.GetRecommendedResolution();
		GameOptions.PreferredScalingMode recommendedScalingMode = GameManager.Options.GetRecommendedScalingMode();
		if (Screen.width == recommendedResolution.width && Screen.height == recommendedResolution.height && recommendedScalingMode == GameManager.Options.CurrentPreferredScalingMode && GameManager.Options.CurrentPreferredFullscreenMode == GameOptions.PreferredFullscreenMode.FULLSCREEN)
		{
			this.m_selectedIndex = 0;
		}
		else
		{
			this.m_selectedIndex = 1;
		}
		GameManager.Options.CurrentVisualPreset = (GameOptions.VisualPresetMode)this.m_selectedIndex;
		this.UpdateSelectedLabelText();
	}

	// Token: 0x06008EF1 RID: 36593 RVA: 0x003C57B4 File Offset: 0x003C39B4
	private StringTableManager.GungeonSupportedLanguages IntToLanguage(int index)
	{
		switch (index)
		{
		case 0:
			return StringTableManager.GungeonSupportedLanguages.ENGLISH;
		case 1:
			return StringTableManager.GungeonSupportedLanguages.SPANISH;
		case 2:
			return StringTableManager.GungeonSupportedLanguages.FRENCH;
		case 3:
			return StringTableManager.GungeonSupportedLanguages.ITALIAN;
		case 4:
			return StringTableManager.GungeonSupportedLanguages.GERMAN;
		case 5:
			return StringTableManager.GungeonSupportedLanguages.BRAZILIANPORTUGUESE;
		case 6:
			return StringTableManager.GungeonSupportedLanguages.POLISH;
		case 7:
			return StringTableManager.GungeonSupportedLanguages.RUSSIAN;
		case 8:
			return StringTableManager.GungeonSupportedLanguages.JAPANESE;
		case 9:
			return StringTableManager.GungeonSupportedLanguages.KOREAN;
		case 10:
			return StringTableManager.GungeonSupportedLanguages.CHINESE;
		default:
			return StringTableManager.GungeonSupportedLanguages.ENGLISH;
		}
	}

	// Token: 0x06008EF2 RID: 36594 RVA: 0x003C5814 File Offset: 0x003C3A14
	private int LanguageToInt(StringTableManager.GungeonSupportedLanguages language)
	{
		switch (language)
		{
		case StringTableManager.GungeonSupportedLanguages.ENGLISH:
			return 0;
		case StringTableManager.GungeonSupportedLanguages.FRENCH:
			return 2;
		case StringTableManager.GungeonSupportedLanguages.SPANISH:
			return 1;
		case StringTableManager.GungeonSupportedLanguages.ITALIAN:
			return 3;
		case StringTableManager.GungeonSupportedLanguages.GERMAN:
			return 4;
		case StringTableManager.GungeonSupportedLanguages.BRAZILIANPORTUGUESE:
			return 5;
		case StringTableManager.GungeonSupportedLanguages.JAPANESE:
			return 8;
		case StringTableManager.GungeonSupportedLanguages.KOREAN:
			return 9;
		case StringTableManager.GungeonSupportedLanguages.RUSSIAN:
			return 7;
		case StringTableManager.GungeonSupportedLanguages.POLISH:
			return 6;
		case StringTableManager.GungeonSupportedLanguages.CHINESE:
			return 10;
		}
		return 0;
	}

	// Token: 0x06008EF3 RID: 36595 RVA: 0x003C5878 File Offset: 0x003C3A78
	private void DetermineAvailableOptions()
	{
		BraveOptionsMenuItem.BraveOptionsOptionType braveOptionsOptionType = this.optionType;
		switch (braveOptionsOptionType)
		{
		case BraveOptionsMenuItem.BraveOptionsOptionType.RESOLUTION:
			this.HandleResolutionDetermination();
			break;
		case BraveOptionsMenuItem.BraveOptionsOptionType.SCALING_MODE:
		{
			int width = Screen.width;
			int height = Screen.height;
			int num = BraveMathCollege.GreatestCommonDivisor(width, height);
			int num2 = width / num;
			int num3 = height / num;
			List<string> list = new List<string>();
			if (this.m_scalingModes == null)
			{
				this.m_scalingModes = new List<GameOptions.PreferredScalingMode>();
			}
			this.m_scalingModes.Clear();
			if (num2 == 16 && num3 == 9)
			{
				if (width % 480 == 0 && height % 270 == 0)
				{
					list.Add("#OPTIONS_PIXELPERFECT");
					this.m_scalingModes.Add(GameOptions.PreferredScalingMode.PIXEL_PERFECT);
				}
				else
				{
					list.Add("#OPTIONS_UNIFORMSCALING");
					this.m_scalingModes.Add(GameOptions.PreferredScalingMode.UNIFORM_SCALING);
					list.Add("#OPTIONS_FORCEPIXELPERFECT");
					this.m_scalingModes.Add(GameOptions.PreferredScalingMode.FORCE_PIXEL_PERFECT);
					list.Add("#OPTIONS_UNIFORMSCALINGFAST");
					this.m_scalingModes.Add(GameOptions.PreferredScalingMode.UNIFORM_SCALING_FAST);
				}
			}
			else
			{
				list.Add("#OPTIONS_UNIFORMSCALING");
				this.m_scalingModes.Add(GameOptions.PreferredScalingMode.UNIFORM_SCALING);
				list.Add("#OPTIONS_FORCEPIXELPERFECT");
				this.m_scalingModes.Add(GameOptions.PreferredScalingMode.FORCE_PIXEL_PERFECT);
				list.Add("#OPTIONS_UNIFORMSCALINGFAST");
				this.m_scalingModes.Add(GameOptions.PreferredScalingMode.UNIFORM_SCALING_FAST);
			}
			this.labelOptions = list.ToArray();
			if (this.m_scalingModes.Contains(GameManager.Options.CurrentPreferredScalingMode))
			{
				this.m_selectedIndex = this.GetScalingIndex(GameManager.Options.CurrentPreferredScalingMode);
			}
			else
			{
				this.m_selectedIndex = 0;
				if (list.Count >= 2 && GameManager.Options.CurrentPreferredScalingMode == GameOptions.PreferredScalingMode.PIXEL_PERFECT)
				{
					this.m_selectedIndex = 1;
				}
			}
			this.RelocalizeOptions();
			break;
		}
		default:
			switch (braveOptionsOptionType)
			{
			case BraveOptionsMenuItem.BraveOptionsOptionType.PLAYER_ONE_CONTROL_PORT:
			{
				List<string> list2 = new List<string>();
				for (int i = 0; i < InputManager.Devices.Count; i++)
				{
					string name = InputManager.Devices[i].Name;
					int num4 = 1;
					string text = name;
					while (list2.Contains(text))
					{
						num4++;
						text = name + " " + num4.ToString();
					}
					list2.Add(text);
				}
				list2.Add(this.m_self.ForceGetLocalizedValue("#OPTIONS_KEYBOARDMOUSE"));
				this.labelOptions = list2.ToArray();
				break;
			}
			default:
				switch (braveOptionsOptionType)
				{
				case BraveOptionsMenuItem.BraveOptionsOptionType.LANGUAGE:
					this.labelOptions = new List<string>
					{
						"#LANGUAGE_ENGLISH", "#LANGUAGE_SPANISH", "#LANGUAGE_FRENCH", "#LANGUAGE_ITALIAN", "#LANGUAGE_GERMAN", "#LANGUAGE_PORTUGUESE", "#LANGUAGE_POLISH", "#LANGUAGE_RUSSIAN", "#LANGUAGE_JAPANESE", "#LANGUAGE_KOREAN",
						"#LANGUAGE_CHINESE"
					}.ToArray();
					this.RelocalizeOptions();
					break;
				case BraveOptionsMenuItem.BraveOptionsOptionType.QUICKSTART_CHARACTER:
				{
					if (this.m_quickStartCharacters == null)
					{
						this.m_quickStartCharacters = new List<GameOptions.QuickstartCharacter>();
					}
					else
					{
						this.m_quickStartCharacters.Clear();
					}
					List<string> list3 = new List<string>(7);
					this.m_quickStartCharacters.Add(GameOptions.QuickstartCharacter.LAST_USED);
					list3.Add("#CHAR_LASTUSED");
					this.m_quickStartCharacters.Add(GameOptions.QuickstartCharacter.PILOT);
					list3.Add("#CHAR_ROGUE");
					this.m_quickStartCharacters.Add(GameOptions.QuickstartCharacter.CONVICT);
					list3.Add("#CHAR_CONVICT");
					this.m_quickStartCharacters.Add(GameOptions.QuickstartCharacter.SOLDIER);
					list3.Add("#CHAR_MARINE");
					this.m_quickStartCharacters.Add(GameOptions.QuickstartCharacter.GUIDE);
					list3.Add("#CHAR_GUIDE");
					if (GameStatsManager.HasInstance && GameStatsManager.Instance.GetFlag(GungeonFlags.SECRET_BULLETMAN_SEEN_05))
					{
						this.m_quickStartCharacters.Add(GameOptions.QuickstartCharacter.BULLET);
						list3.Add("#CHAR_BULLET");
					}
					if (GameStatsManager.HasInstance && GameStatsManager.Instance.GetFlag(GungeonFlags.BLACKSMITH_RECEIVED_BUSTED_TELEVISION))
					{
						this.m_quickStartCharacters.Add(GameOptions.QuickstartCharacter.ROBOT);
						list3.Add("#CHAR_ROBOT");
					}
					this.labelOptions = list3.ToArray();
					this.m_selectedIndex = this.GetQuickStartCharIndex(GameManager.Options.PreferredQuickstartCharacter);
					if (this.m_selectedIndex < 0 || this.m_selectedIndex >= this.labelOptions.Length)
					{
						this.m_selectedIndex = 0;
					}
					this.UpdateSelectedLabelText();
					break;
				}
				}
				break;
			case BraveOptionsMenuItem.BraveOptionsOptionType.PLAYER_TWO_CONTROL_PORT:
			{
				List<string> list4 = new List<string>();
				for (int j = 0; j < InputManager.Devices.Count; j++)
				{
					string name2 = InputManager.Devices[j].Name;
					int num5 = 1;
					string text2 = name2;
					while (list4.Contains(text2))
					{
						num5++;
						text2 = name2 + " " + num5.ToString();
					}
					list4.Add(text2);
				}
				list4.Add(this.m_self.ForceGetLocalizedValue("#OPTIONS_KEYBOARDMOUSE"));
				this.labelOptions = list4.ToArray();
				break;
			}
			}
			break;
		case BraveOptionsMenuItem.BraveOptionsOptionType.MONITOR_SELECT:
		{
			List<string> list5 = new List<string>();
			for (int k = 0; k < Display.displays.Length; k++)
			{
				list5.Add((k + 1).ToString());
			}
			this.labelOptions = list5.ToArray();
			break;
		}
		}
		if (this.itemType == BraveOptionsMenuItem.BraveOptionsMenuItemType.LeftRightArrow || this.itemType == BraveOptionsMenuItem.BraveOptionsMenuItemType.LeftRightArrowInfo)
		{
			if (this.m_selectedIndex >= this.labelOptions.Length)
			{
				this.m_selectedIndex = 0;
			}
			if (this.labelOptions != null && this.m_selectedIndex > -1 && this.m_selectedIndex < this.labelOptions.Length)
			{
				this.UpdateSelectedLabelText();
				if (this.itemType == BraveOptionsMenuItem.BraveOptionsMenuItemType.LeftRightArrowInfo)
				{
					this.UpdateInfoControl();
				}
			}
			else
			{
				this.selectedLabelControl.Text = "?";
			}
		}
		if (this.itemType == BraveOptionsMenuItem.BraveOptionsMenuItemType.Checkbox)
		{
			this.RepositionCheckboxControl();
		}
		if (this.labelControl)
		{
			this.labelControl.PerformLayout();
		}
		this.UpdateSelectedLabelText();
		this.UpdateInfoControl();
	}

	// Token: 0x06008EF4 RID: 36596 RVA: 0x003C5EE8 File Offset: 0x003C40E8
	private void UpdateInfoControl()
	{
		if (this.optionType == BraveOptionsMenuItem.BraveOptionsOptionType.RESOLUTION)
		{
			List<Resolution> availableResolutions = this.GetAvailableResolutions();
			this.m_selectedIndex = Mathf.Clamp(this.m_selectedIndex, 0, availableResolutions.Count - 1);
			int width = availableResolutions[this.m_selectedIndex].width;
			int height = availableResolutions[this.m_selectedIndex].height;
			int num = BraveMathCollege.GreatestCommonDivisor(width, height);
			int num2 = width / num;
			int num3 = height / num;
			bool flag = num2 == 16 && num3 == 9;
			float num4 = (float)width / 480f;
			float num5 = (float)height / 270f;
			bool flag2 = Mathf.Min(num4, num5) % 1f == 0f;
			if (flag && flag2)
			{
				this.infoControl.Color = Color.green;
				this.infoControl.Text = this.infoControl.ForceGetLocalizedValue("#OPTIONS_RESOLUTION_BEST");
			}
			else if (flag2)
			{
				this.infoControl.Color = Color.green;
				this.infoControl.Text = this.infoControl.ForceGetLocalizedValue("#OPTIONS_RESOLUTION_GOOD");
			}
			else
			{
				this.infoControl.Color = Color.red;
				this.infoControl.Text = this.infoControl.ForceGetLocalizedValue("#OPTIONS_RESOLUTION_BAD");
			}
		}
		else if (this.optionType == BraveOptionsMenuItem.BraveOptionsOptionType.SCALING_MODE)
		{
			if (GameManager.Options.CurrentPreferredScalingMode == GameOptions.PreferredScalingMode.PIXEL_PERFECT)
			{
				this.infoControl.Color = Color.green;
				this.infoControl.Text = this.infoControl.ForceGetLocalizedValue("#OPTIONS_PIXELPERFECT_INFO");
			}
			else if (GameManager.Options.CurrentPreferredScalingMode == GameOptions.PreferredScalingMode.UNIFORM_SCALING || GameManager.Options.CurrentPreferredScalingMode == GameOptions.PreferredScalingMode.UNIFORM_SCALING_FAST)
			{
				float num6 = (float)Screen.width / 480f;
				float num7 = (float)Screen.height / 270f;
				bool flag3 = Mathf.Min(num6, num7) % 1f == 0f;
				if (flag3)
				{
					this.infoControl.Color = Color.green;
					this.infoControl.Text = this.infoControl.ForceGetLocalizedValue("#OPTIONS_UNIFORMSCALING_INFOGOOD");
				}
				else
				{
					this.infoControl.Color = Color.red;
					this.infoControl.Text = this.infoControl.ForceGetLocalizedValue("#OPTIONS_UNIFORMSCALING_INFOBAD");
				}
			}
			else
			{
				this.infoControl.Color = Color.green;
				this.infoControl.Text = this.infoControl.ForceGetLocalizedValue("#OPTIONS_FORCEPIXELPERFECT_INFO");
			}
		}
		this.UpdateInfoControlHeight();
	}

	// Token: 0x06008EF5 RID: 36597 RVA: 0x003C61A4 File Offset: 0x003C43A4
	private int GetScalingIndex(GameOptions.PreferredScalingMode scalingMode)
	{
		for (int i = 0; i < this.m_scalingModes.Count; i++)
		{
			if (this.m_scalingModes[i] == scalingMode)
			{
				return i;
			}
		}
		return -1;
	}

	// Token: 0x06008EF6 RID: 36598 RVA: 0x003C61E4 File Offset: 0x003C43E4
	private int GetQuickStartCharIndex(GameOptions.QuickstartCharacter quickstartChar)
	{
		for (int i = 0; i < this.m_quickStartCharacters.Count; i++)
		{
			if (this.m_quickStartCharacters[i] == quickstartChar)
			{
				return i;
			}
		}
		return -1;
	}

	// Token: 0x06008EF7 RID: 36599 RVA: 0x003C6224 File Offset: 0x003C4424
	private void HandleResolutionDetermination()
	{
		List<Resolution> availableResolutions = this.GetAvailableResolutions();
		this.labelOptions = new string[availableResolutions.Count];
		this.m_selectedIndex = 0;
		for (int i = 0; i < availableResolutions.Count; i++)
		{
			int width = availableResolutions[i].width;
			int height = availableResolutions[i].height;
			int num = BraveMathCollege.GreatestCommonDivisor(width, height);
			int num2 = width / num;
			int num3 = height / num;
			this.labelOptions[i] = string.Concat(new object[]
			{
				width.ToString(),
				" x ",
				height.ToString(),
				" (",
				num2,
				":",
				num3,
				")"
			});
			if (width == Screen.width && height == Screen.height)
			{
				this.m_selectedIndex = i;
			}
		}
	}

	// Token: 0x06008EF8 RID: 36600 RVA: 0x003C6328 File Offset: 0x003C4528
	private void HandleFillbarClick(dfControl control, dfMouseEventArgs mouseEvent)
	{
		if (mouseEvent.Buttons.IsSet(dfMouseButtons.Left))
		{
			Collider component = control.GetComponent<Collider>();
			RaycastHit raycastHit;
			bool flag = component.Raycast(mouseEvent.Ray, out raycastHit, 1000f);
			Vector2 vector = Vector2.zero;
			if (flag)
			{
				vector = raycastHit.point;
			}
			else
			{
				Plane plane = new Plane(Vector3.back, component.bounds.center.WithZ(0f));
				float num;
				bool flag2 = plane.Raycast(mouseEvent.Ray, out num);
				if (flag2)
				{
					vector = BraveMathCollege.ClosestPointOnRectangle(mouseEvent.Ray.GetPoint(num), component.bounds.min, component.bounds.extents * 2f);
				}
			}
			float num2 = control.Width * control.transform.localScale.x * control.PixelsToUnits();
			float num3 = (vector.x - (control.transform.position.x - num2 / 2f)) / num2;
			this.m_actualFillbarValue = Mathf.Clamp(num3 + this.FillbarDelta / 2f, 0f, 1f).Quantize(this.FillbarDelta);
			this.fillbarControl.Value = this.m_actualFillbarValue * 0.98f + 0.01f;
			this.HandleFillbarValueChanged();
			mouseEvent.Use();
		}
	}

	// Token: 0x06008EF9 RID: 36601 RVA: 0x003C64B0 File Offset: 0x003C46B0
	private void HandleFillbarDown(dfControl control, dfMouseEventArgs mouseEvent)
	{
		this.HandleFillbarClick(control, mouseEvent);
	}

	// Token: 0x06008EFA RID: 36602 RVA: 0x003C64BC File Offset: 0x003C46BC
	private void HandleFillbarMove(dfControl control, dfMouseEventArgs mouseEvent)
	{
		this.HandleFillbarClick(control, mouseEvent);
	}

	// Token: 0x06008EFB RID: 36603 RVA: 0x003C64C8 File Offset: 0x003C46C8
	private void HandleFillbarHover(dfControl control, dfMouseEventArgs mouseEvent)
	{
		this.HandleFillbarClick(control, mouseEvent);
	}

	// Token: 0x06008EFC RID: 36604 RVA: 0x003C64D4 File Offset: 0x003C46D4
	private void DelControl()
	{
		BraveOptionsMenuItem braveOptionsMenuItem = ((!(this.up != null)) ? null : this.up.GetComponent<BraveOptionsMenuItem>());
		BraveOptionsMenuItem braveOptionsMenuItem2 = ((!(this.down != null)) ? null : this.down.GetComponent<BraveOptionsMenuItem>());
		if (braveOptionsMenuItem != null)
		{
			braveOptionsMenuItem.down = this.down;
		}
		else
		{
			UIKeyControls uikeyControls = ((!(this.up != null)) ? null : this.up.GetComponent<UIKeyControls>());
			if (uikeyControls != null)
			{
				uikeyControls.down = this.down;
			}
		}
		if (braveOptionsMenuItem2 != null)
		{
			braveOptionsMenuItem2.up = this.up;
		}
		else
		{
			UIKeyControls uikeyControls2 = ((!(this.down != null)) ? null : this.down.GetComponent<UIKeyControls>());
			if (uikeyControls2 != null)
			{
				uikeyControls2.up = this.up;
			}
		}
		this.m_self.Parent.RemoveControl(this.m_self);
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x06008EFD RID: 36605 RVA: 0x003C65F8 File Offset: 0x003C47F8
	public IEnumerator Start()
	{
		yield return null;
		bool deletedControl = this.DisablePlatformSpecificOptions();
		if (deletedControl)
		{
			yield break;
		}
		if (this.buttonControl != null)
		{
			this.buttonControl.AutoSize = true;
		}
		if (this.itemType == BraveOptionsMenuItem.BraveOptionsMenuItemType.Checkbox)
		{
			dfScrollPanel component = this.m_self.Parent.GetComponent<dfScrollPanel>();
			component.ScrollPositionChanged += delegate(dfControl c, Vector2 v)
			{
				this.checkboxChecked.Invalidate();
				this.checkboxUnchecked.Invalidate();
			};
		}
		this.SetUnselectedColors();
		this.InitializeFromOptions();
		GameUIRoot.Instance.PauseMenuPanel.GetComponent<PauseMenuController>().OptionsMenu.RegisterItem(this);
		if (this.itemType == BraveOptionsMenuItem.BraveOptionsMenuItemType.LeftRightArrow || this.itemType == BraveOptionsMenuItem.BraveOptionsMenuItemType.LeftRightArrowInfo)
		{
			this.left.HotZoneScale = Vector2.one * 3f;
			this.right.HotZoneScale = this.left.HotZoneScale;
			this.left.MouseEnter += this.ArrowHoverGrow;
			this.left.MouseLeave += this.ArrowReturnScale;
			this.right.MouseEnter += this.ArrowHoverGrow;
			this.right.MouseLeave += this.ArrowReturnScale;
		}
		if (GameManager.Instance.CurrentLevelOverrideState == GameManager.LevelOverrideState.FOYER && (this.optionType == BraveOptionsMenuItem.BraveOptionsOptionType.PLAYER_ONE_CONTROL_PORT || this.optionType == BraveOptionsMenuItem.BraveOptionsOptionType.PLAYER_TWO_CONTROL_PORT))
		{
			Foyer instance = Foyer.Instance;
			instance.OnCoopModeChanged = (Action)Delegate.Combine(instance.OnCoopModeChanged, new Action(this.InitializeFromOptions));
		}
		this.m_panelStartHeight = base.GetComponent<dfControl>().Height;
		this.m_additionalStartHeight = ((!this.infoControl) ? (-1f) : this.infoControl.Height);
		this.UpdateInfoControlHeight();
		yield break;
	}

	// Token: 0x06008EFE RID: 36606 RVA: 0x003C6614 File Offset: 0x003C4814
	private void UpdateInfoControlHeight()
	{
		if (!this.infoControl)
		{
			return;
		}
		if (this.m_panelStartHeight < 0f)
		{
			this.m_panelStartHeight = base.GetComponent<dfControl>().Height;
		}
		if (this.m_additionalStartHeight < 0f)
		{
			this.m_additionalStartHeight = this.infoControl.Height;
		}
		if ((Application.platform != RuntimePlatform.PS4 || Application.platform != RuntimePlatform.XboxOne) && (this.optionType == BraveOptionsMenuItem.BraveOptionsOptionType.RESOLUTION || this.optionType == BraveOptionsMenuItem.BraveOptionsOptionType.SCALING_MODE))
		{
			if (GameManager.Options.CurrentLanguage == StringTableManager.GungeonSupportedLanguages.KOREAN || GameManager.Options.CurrentLanguage == StringTableManager.GungeonSupportedLanguages.CHINESE || GameManager.Options.CurrentLanguage == StringTableManager.GungeonSupportedLanguages.RUSSIAN)
			{
				if (!this.m_infoControlHeightModified)
				{
					base.GetComponent<dfControl>().Height = this.m_panelStartHeight + 30f;
					this.infoControl.Height = this.m_additionalStartHeight + 30f;
					this.infoControl.RelativePosition = this.infoControl.RelativePosition + new Vector3(0f, 30f, 0f);
					this.m_infoControlHeightModified = true;
				}
			}
			else if (this.m_infoControlHeightModified)
			{
				base.GetComponent<dfControl>().Height = this.m_panelStartHeight;
				this.infoControl.Height = this.m_additionalStartHeight;
				this.infoControl.RelativePosition = this.infoControl.RelativePosition - new Vector3(0f, 30f, 0f);
				this.m_infoControlHeightModified = false;
			}
			this.infoControl.PerformLayout();
		}
	}

	// Token: 0x06008EFF RID: 36607 RVA: 0x003C67B8 File Offset: 0x003C49B8
	private void ConvertPivots()
	{
		if (this.labelControl)
		{
			this.labelControl.Pivot = dfPivotPoint.BottomLeft;
		}
		if (this.selectedLabelControl)
		{
			this.selectedLabelControl.Pivot = dfPivotPoint.BottomLeft;
		}
		if (this.infoControl)
		{
			this.infoControl.Pivot = dfPivotPoint.BottomLeft;
		}
		if (this.buttonControl)
		{
			this.buttonControl.Pivot = dfPivotPoint.BottomLeft;
		}
	}

	// Token: 0x06008F00 RID: 36608 RVA: 0x003C6838 File Offset: 0x003C4A38
	private int GetIndexFromFullscreenMode(GameOptions.PreferredFullscreenMode fMode)
	{
		return (fMode != GameOptions.PreferredFullscreenMode.FULLSCREEN) ? ((fMode != GameOptions.PreferredFullscreenMode.BORDERLESS) ? 2 : 1) : 0;
	}

	// Token: 0x06008F01 RID: 36609 RVA: 0x003C6854 File Offset: 0x003C4A54
	public void ForceRefreshDisplayLabel()
	{
		if (!this.selectedLabelControl)
		{
			return;
		}
		this.UpdateSelectedLabelText();
		this.selectedLabelControl.PerformLayout();
	}

	// Token: 0x06008F02 RID: 36610 RVA: 0x003C6878 File Offset: 0x003C4A78
	public void InitializeFromOptions()
	{
		switch (this.optionType)
		{
		case BraveOptionsMenuItem.BraveOptionsOptionType.MUSIC_VOLUME:
			this.m_actualFillbarValue = GameManager.Options.MusicVolume / 100f;
			this.fillbarControl.Value = this.m_actualFillbarValue * 0.98f + 0.01f;
			break;
		case BraveOptionsMenuItem.BraveOptionsOptionType.SOUND_VOLUME:
			this.m_actualFillbarValue = GameManager.Options.SoundVolume / 100f;
			this.fillbarControl.Value = this.m_actualFillbarValue * 0.98f + 0.01f;
			break;
		case BraveOptionsMenuItem.BraveOptionsOptionType.UI_VOLUME:
			this.m_actualFillbarValue = GameManager.Options.UIVolume / 100f;
			this.fillbarControl.Value = this.m_actualFillbarValue * 0.98f + 0.01f;
			break;
		case BraveOptionsMenuItem.BraveOptionsOptionType.SPEAKER_TYPE:
			this.m_selectedIndex = (int)GameManager.Options.AudioHardware;
			this.UpdateSelectedLabelText();
			break;
		case BraveOptionsMenuItem.BraveOptionsOptionType.VISUAL_PRESET:
			this.InitializeVisualPreset();
			break;
		case BraveOptionsMenuItem.BraveOptionsOptionType.FULLSCREEN:
			this.m_selectedIndex = this.GetIndexFromFullscreenMode(GameManager.Options.CurrentPreferredFullscreenMode);
			break;
		case BraveOptionsMenuItem.BraveOptionsOptionType.MONITOR_SELECT:
			this.m_selectedIndex = GameManager.Options.CurrentMonitorIndex;
			this.DetermineAvailableOptions();
			this.UpdateSelectedLabelText();
			break;
		case BraveOptionsMenuItem.BraveOptionsOptionType.VSYNC:
			if (QualitySettings.vSyncCount > 0 && !GameManager.Options.DoVsync)
			{
				GameManager.Options.DoVsync = false;
			}
			this.m_selectedIndex = ((!GameManager.Options.DoVsync) ? 0 : 1);
			this.HandleCheckboxValueChanged();
			break;
		case BraveOptionsMenuItem.BraveOptionsOptionType.LIGHTING_QUALITY:
			this.m_selectedIndex = ((GameManager.Options.LightingQuality != GameOptions.GenericHighMedLowOption.HIGH) ? 1 : 0);
			this.UpdateSelectedLabelText();
			break;
		case BraveOptionsMenuItem.BraveOptionsOptionType.SHADER_QUALITY:
			switch (GameManager.Options.ShaderQuality)
			{
			case GameOptions.GenericHighMedLowOption.LOW:
				this.m_selectedIndex = 2;
				break;
			case GameOptions.GenericHighMedLowOption.MEDIUM:
				this.m_selectedIndex = 3;
				break;
			case GameOptions.GenericHighMedLowOption.HIGH:
				this.m_selectedIndex = 0;
				break;
			case GameOptions.GenericHighMedLowOption.VERY_LOW:
				this.m_selectedIndex = 1;
				break;
			default:
				this.m_selectedIndex = 0;
				break;
			}
			this.UpdateSelectedLabelText();
			break;
		case BraveOptionsMenuItem.BraveOptionsOptionType.DEBRIS_QUANTITY:
			switch (GameManager.Options.DebrisQuantity)
			{
			case GameOptions.GenericHighMedLowOption.LOW:
				this.m_selectedIndex = 2;
				break;
			case GameOptions.GenericHighMedLowOption.MEDIUM:
				this.m_selectedIndex = 3;
				break;
			case GameOptions.GenericHighMedLowOption.HIGH:
				this.m_selectedIndex = 0;
				break;
			case GameOptions.GenericHighMedLowOption.VERY_LOW:
				this.m_selectedIndex = 1;
				break;
			default:
				this.m_selectedIndex = 0;
				break;
			}
			this.UpdateSelectedLabelText();
			break;
		case BraveOptionsMenuItem.BraveOptionsOptionType.SCREEN_SHAKE_AMOUNT:
			this.m_actualFillbarValue = GameManager.Options.ScreenShakeMultiplier * 0.5f;
			this.fillbarControl.Value = this.m_actualFillbarValue * 0.98f + 0.01f;
			break;
		case BraveOptionsMenuItem.BraveOptionsOptionType.STICKY_FRICTION_AMOUNT:
			this.m_actualFillbarValue = GameManager.Options.StickyFrictionMultiplier * 0.8f;
			this.fillbarControl.Value = this.m_actualFillbarValue * 0.98f + 0.01f;
			break;
		case BraveOptionsMenuItem.BraveOptionsOptionType.TEXT_SPEED:
			this.m_selectedIndex = ((GameManager.Options.TextSpeed != GameOptions.GenericHighMedLowOption.MEDIUM) ? ((GameManager.Options.TextSpeed != GameOptions.GenericHighMedLowOption.LOW) ? 1 : 2) : 0);
			this.UpdateSelectedLabelText();
			break;
		case BraveOptionsMenuItem.BraveOptionsOptionType.CONTROLLER_AIM_ASSIST_AMOUNT:
			this.m_actualFillbarValue = GameManager.Options.controllerAimAssistMultiplier * 0.8f;
			this.fillbarControl.Value = this.m_actualFillbarValue * 0.98f + 0.01f;
			break;
		case BraveOptionsMenuItem.BraveOptionsOptionType.BEASTMODE:
			this.m_selectedIndex = ((!GameManager.Options.m_beastmode) ? 0 : 1);
			this.HandleCheckboxValueChanged();
			break;
		case BraveOptionsMenuItem.BraveOptionsOptionType.PLAYER_ONE_CONTROL_PORT:
			if (GameManager.Instance.PrimaryPlayer == null)
			{
				this.m_selectedIndex = 0;
			}
			else if (GameManager.Options.PlayerIDtoDeviceIndexMap.ContainsKey(GameManager.Instance.PrimaryPlayer.PlayerIDX))
			{
				this.m_selectedIndex = GameManager.Options.PlayerIDtoDeviceIndexMap[GameManager.Instance.PrimaryPlayer.PlayerIDX];
			}
			else
			{
				this.m_selectedIndex = 0;
			}
			break;
		case BraveOptionsMenuItem.BraveOptionsOptionType.PLAYER_ONE_CONTROLLER_SYMBOLOGY:
			this.m_selectedIndex = (int)GameManager.Options.PlayerOnePreferredSymbology;
			this.UpdateSelectedLabelText();
			break;
		case BraveOptionsMenuItem.BraveOptionsOptionType.PLAYER_TWO_CONTROL_PORT:
			if (GameManager.Instance.AllPlayers.Length > 1)
			{
				this.m_selectedIndex = GameManager.Options.PlayerIDtoDeviceIndexMap[GameManager.Instance.SecondaryPlayer.PlayerIDX];
			}
			break;
		case BraveOptionsMenuItem.BraveOptionsOptionType.PLAYER_TWO_CONTROLLER_SYMBOLOGY:
			this.m_selectedIndex = (int)GameManager.Options.PlayerTwoPreferredSymbology;
			this.UpdateSelectedLabelText();
			break;
		case BraveOptionsMenuItem.BraveOptionsOptionType.MINIMAP_STYLE:
			this.m_selectedIndex = ((GameManager.Options.MinimapDisplayMode != Minimap.MinimapDisplayMode.FADE_ON_ROOM_SEAL) ? ((GameManager.Options.MinimapDisplayMode != Minimap.MinimapDisplayMode.ALWAYS) ? 1 : 2) : 0);
			this.UpdateSelectedLabelText();
			break;
		case BraveOptionsMenuItem.BraveOptionsOptionType.COOP_SCREEN_SHAKE_AMOUNT:
			this.m_selectedIndex = ((!GameManager.Options.CoopScreenShakeReduction) ? 0 : 1);
			this.HandleCheckboxValueChanged();
			break;
		case BraveOptionsMenuItem.BraveOptionsOptionType.CONTROLLER_AIM_LOOK:
			this.m_actualFillbarValue = GameManager.Options.controllerAimLookMultiplier * 0.8f;
			this.fillbarControl.Value = this.m_actualFillbarValue * 0.98f + 0.01f;
			break;
		case BraveOptionsMenuItem.BraveOptionsOptionType.GAMMA:
			this.m_actualFillbarValue = GameManager.Options.Gamma - 0.5f;
			this.fillbarControl.Value = this.m_actualFillbarValue * 0.98f + 0.01f;
			break;
		case BraveOptionsMenuItem.BraveOptionsOptionType.REALTIME_REFLECTIONS:
			this.m_selectedIndex = ((!GameManager.Options.RealtimeReflections) ? 0 : 1);
			this.HandleCheckboxValueChanged();
			break;
		case BraveOptionsMenuItem.BraveOptionsOptionType.QUICKSELECT:
			this.m_selectedIndex = ((!GameManager.Options.QuickSelectEnabled) ? 0 : 1);
			this.HandleCheckboxValueChanged();
			break;
		case BraveOptionsMenuItem.BraveOptionsOptionType.HIDE_EMPTY_GUNS:
			this.m_selectedIndex = ((!GameManager.Options.HideEmptyGuns) ? 0 : 1);
			this.HandleCheckboxValueChanged();
			break;
		case BraveOptionsMenuItem.BraveOptionsOptionType.LANGUAGE:
			this.m_selectedIndex = this.LanguageToInt(GameManager.Options.CurrentLanguage);
			if (this.m_selectedIndex >= this.labelOptions.Length)
			{
				this.DetermineAvailableOptions();
			}
			this.UpdateSelectedLabelText();
			break;
		case BraveOptionsMenuItem.BraveOptionsOptionType.SPEEDRUN:
			this.m_selectedIndex = ((!GameManager.Options.SpeedrunMode) ? 0 : 1);
			this.HandleCheckboxValueChanged();
			break;
		case BraveOptionsMenuItem.BraveOptionsOptionType.QUICKSTART_CHARACTER:
			if (this.m_quickStartCharacters != null)
			{
				this.m_selectedIndex = this.GetQuickStartCharIndex(GameManager.Options.PreferredQuickstartCharacter);
				if (this.m_selectedIndex < 0 || this.m_selectedIndex >= this.m_quickStartCharacters.Count)
				{
					this.m_selectedIndex = 0;
				}
				this.UpdateSelectedLabelText();
			}
			break;
		case BraveOptionsMenuItem.BraveOptionsOptionType.ADDITIONAL_BLANK_CONTROL:
			this.m_selectedIndex = (int)GameManager.Options.additionalBlankControl;
			this.UpdateSelectedLabelText();
			break;
		case BraveOptionsMenuItem.BraveOptionsOptionType.ADDITIONAL_BLANK_CONTROL_TWO:
			this.m_selectedIndex = (int)GameManager.Options.additionalBlankControlTwo;
			this.UpdateSelectedLabelText();
			break;
		case BraveOptionsMenuItem.BraveOptionsOptionType.CURRENT_BINDINGS_PRESET:
			this.UpdateLabelOptions(0);
			this.m_selectedIndex = Mathf.Clamp((int)GameManager.Options.CurrentControlPreset, 0, this.labelOptions.Length - 1);
			this.selectedLabelControl.PerformLayout();
			break;
		case BraveOptionsMenuItem.BraveOptionsOptionType.CURRENT_BINDINGS_PRESET_P2:
			this.UpdateLabelOptions(1);
			this.m_selectedIndex = Mathf.Clamp((int)GameManager.Options.CurrentControlPresetP2, 0, this.labelOptions.Length - 1);
			this.selectedLabelControl.PerformLayout();
			break;
		case BraveOptionsMenuItem.BraveOptionsOptionType.SAVE_SLOT:
			this.m_selectedIndex = (int)SaveManager.CurrentSaveSlot;
			this.UpdateSelectedLabelText();
			break;
		case BraveOptionsMenuItem.BraveOptionsOptionType.RUMBLE:
			this.m_selectedIndex = ((!GameManager.Options.RumbleEnabled) ? 0 : 1);
			this.HandleCheckboxValueChanged();
			break;
		case BraveOptionsMenuItem.BraveOptionsOptionType.CURSOR_VARIATION:
			this.m_selectedIndex = GameManager.Options.CurrentCursorIndex;
			this.UpdateSelectedLabelText();
			break;
		case BraveOptionsMenuItem.BraveOptionsOptionType.ADDITIONAL_BLANK_CONTROL_PS4:
			this.UpdateLabelOptions(FullOptionsMenuController.CurrentBindingPlayerTargetIndex);
			if (FullOptionsMenuController.CurrentBindingPlayerTargetIndex == 0)
			{
				this.m_selectedIndex = (int)GameManager.Options.additionalBlankControl;
			}
			else
			{
				this.m_selectedIndex = (int)GameManager.Options.additionalBlankControlTwo;
			}
			this.UpdateSelectedLabelText();
			break;
		case BraveOptionsMenuItem.BraveOptionsOptionType.PLAYER_ONE_CONTROLLER_CURSOR:
			this.m_selectedIndex = ((!GameManager.Options.PlayerOneControllerCursor) ? 0 : 1);
			this.HandleCheckboxValueChanged();
			break;
		case BraveOptionsMenuItem.BraveOptionsOptionType.PLAYER_TWO_CONTROLLER_CURSOR:
			this.m_selectedIndex = ((!GameManager.Options.PlayerTwoControllerCursor) ? 0 : 1);
			this.HandleCheckboxValueChanged();
			break;
		case BraveOptionsMenuItem.BraveOptionsOptionType.ALLOWED_CONTROLLER_TYPES:
			if (GameManager.Options.allowXinputControllers && GameManager.Options.allowNonXinputControllers)
			{
				this.m_selectedIndex = 0;
			}
			else if (!GameManager.Options.allowNonXinputControllers)
			{
				this.m_selectedIndex = 1;
			}
			else
			{
				this.m_selectedIndex = 2;
			}
			this.UpdateSelectedLabelText();
			break;
		case BraveOptionsMenuItem.BraveOptionsOptionType.ALLOW_UNKNOWN_CONTROLLERS:
			this.m_selectedIndex = ((!GameManager.Options.allowUnknownControllers) ? 0 : 1);
			this.HandleCheckboxValueChanged();
			break;
		case BraveOptionsMenuItem.BraveOptionsOptionType.SMALL_UI:
			this.m_selectedIndex = ((!GameManager.Options.SmallUIEnabled) ? 0 : 1);
			this.HandleCheckboxValueChanged();
			break;
		case BraveOptionsMenuItem.BraveOptionsOptionType.BOTH_CONTROLLER_CURSOR:
			this.m_selectedIndex = ((!GameManager.Options.PlayerOneControllerCursor) ? 0 : 1);
			this.HandleCheckboxValueChanged();
			break;
		case BraveOptionsMenuItem.BraveOptionsOptionType.DISPLAY_SAFE_AREA:
			this.m_actualFillbarValue = (GameManager.Options.DisplaySafeArea - 0.9f) * 10f;
			this.fillbarControl.Value = this.m_actualFillbarValue * 0.98f + 0.01f;
			break;
		case BraveOptionsMenuItem.BraveOptionsOptionType.OUT_OF_COMBAT_SPEED_INCREASE:
			this.m_selectedIndex = ((!GameManager.Options.IncreaseSpeedOutOfCombat) ? 0 : 1);
			this.HandleCheckboxValueChanged();
			break;
		case BraveOptionsMenuItem.BraveOptionsOptionType.CONTROLLER_BEAM_AIM_ASSIST:
			this.m_selectedIndex = ((!GameManager.Options.controllerBeamAimAssist) ? 0 : 1);
			this.HandleCheckboxValueChanged();
			break;
		case BraveOptionsMenuItem.BraveOptionsOptionType.LOOT_PROFILE:
		{
			GameOptions.GameLootProfile currentGameLootProfile = GameManager.Options.CurrentGameLootProfile;
			if (currentGameLootProfile != GameOptions.GameLootProfile.CURRENT)
			{
				if (currentGameLootProfile != GameOptions.GameLootProfile.ORIGINAL)
				{
					this.m_selectedIndex = 0;
				}
				else
				{
					this.m_selectedIndex = 1;
				}
			}
			else
			{
				this.m_selectedIndex = 0;
			}
			this.UpdateSelectedLabelText();
			break;
		}
		case BraveOptionsMenuItem.BraveOptionsOptionType.AUTOAIM:
			switch (GameManager.Options.controllerAutoAim)
			{
			case GameOptions.ControllerAutoAim.AUTO_DETECT:
				this.m_selectedIndex = 0;
				break;
			case GameOptions.ControllerAutoAim.ALWAYS:
				this.m_selectedIndex = 1;
				break;
			case GameOptions.ControllerAutoAim.NEVER:
				this.m_selectedIndex = 2;
				break;
			case GameOptions.ControllerAutoAim.COOP_ONLY:
				this.m_selectedIndex = 3;
				break;
			}
			this.UpdateSelectedLabelText();
			break;
		}
		this.DetermineAvailableOptions();
		if (this.itemType == BraveOptionsMenuItem.BraveOptionsMenuItemType.Checkbox)
		{
			this.labelControl.TextChanged += delegate(dfControl a, string b)
			{
				this.RepositionCheckboxControl();
			};
			dfLabel dfLabel = this.labelControl;
			dfLabel.LanguageChanged = (Action<dfControl>)Delegate.Combine(dfLabel.LanguageChanged, new Action<dfControl>(delegate(dfControl a)
			{
				this.RepositionCheckboxControl();
			}));
			this.RepositionCheckboxControl();
		}
	}

	// Token: 0x06008F03 RID: 36611 RVA: 0x003C73C8 File Offset: 0x003C55C8
	private void RepositionCheckboxControl()
	{
		this.labelControl.AutoSize = true;
		dfPanel component = this.labelControl.Parent.GetComponent<dfPanel>();
		float num = this.checkboxChecked.Width + 21f + this.labelControl.Width;
		component.Width = num;
		this.checkboxChecked.Parent.RelativePosition = this.checkboxChecked.Parent.RelativePosition.WithX(0f).WithY(6f);
		this.checkboxChecked.RelativePosition = new Vector3(0f, 0f, 0f);
		this.checkboxUnchecked.RelativePosition = new Vector3(0f, 0f, 0f);
		this.labelControl.RelativePosition = this.labelControl.RelativePosition.WithX(this.checkboxChecked.Width + 21f);
	}

	// Token: 0x06008F04 RID: 36612 RVA: 0x003C74B8 File Offset: 0x003C56B8
	private void DoFocus(dfControl control, dfFocusEventArgs args)
	{
		if (this.labelControl != null)
		{
			this.labelControl.Color = new Color(1f, 1f, 1f, 1f);
		}
		if (this.buttonControl != null)
		{
			this.buttonControl.TextColor = new Color(1f, 1f, 1f, 1f);
		}
		if (this.optionType == BraveOptionsMenuItem.BraveOptionsOptionType.PLAYER_ONE_CONTROL_PORT || this.optionType == BraveOptionsMenuItem.BraveOptionsOptionType.PLAYER_TWO_CONTROL_PORT || this.optionType == BraveOptionsMenuItem.BraveOptionsOptionType.ALLOWED_CONTROLLER_TYPES)
		{
			InControlInputAdapter.CurrentlyUsingAllDevices = true;
			InControlInputAdapter.SkipInputForRestOfFrame = true;
		}
		if (this.itemType == BraveOptionsMenuItem.BraveOptionsMenuItemType.LeftRightArrow || this.itemType == BraveOptionsMenuItem.BraveOptionsMenuItemType.LeftRightArrowInfo)
		{
			this.left.Color = new Color(1f, 1f, 1f, 1f);
			this.right.Color = new Color(1f, 1f, 1f, 1f);
			this.selectedLabelControl.Color = new Color(1f, 1f, 1f, 1f);
		}
		else if (this.itemType == BraveOptionsMenuItem.BraveOptionsMenuItemType.Checkbox)
		{
			this.checkboxUnchecked.Color = new Color(1f, 1f, 1f, 1f);
			this.checkboxChecked.Color = new Color(1f, 1f, 1f, 1f);
		}
		if (control.Parent is dfScrollPanel)
		{
			dfScrollPanel dfScrollPanel = control.Parent as dfScrollPanel;
			BraveInput bestInputInstance = this.GetBestInputInstance(GameManager.Instance.LastPausingPlayerID);
			if (bestInputInstance == null || bestInputInstance.ActiveActions == null || Input.anyKeyDown || bestInputInstance.ActiveActions.AnyActionPressed())
			{
				dfScrollPanel.ScrollIntoView(control);
			}
		}
	}

	// Token: 0x06008F05 RID: 36613 RVA: 0x003C76C8 File Offset: 0x003C58C8
	private void DoArrowBounce(dfControl targetControl)
	{
		base.StartCoroutine(this.HandleArrowBounce(targetControl));
	}

	// Token: 0x06008F06 RID: 36614 RVA: 0x003C76D8 File Offset: 0x003C58D8
	private IEnumerator HandleArrowBounce(dfControl targetControl)
	{
		float elapsed = 0f;
		float duration = 0.15f;
		while (elapsed < duration)
		{
			elapsed += GameManager.INVARIANT_DELTA_TIME;
			float t = Mathf.PingPong(elapsed / (duration / 2f), 1f);
			targetControl.transform.localScale = Vector3.Lerp(Vector3.one, new Vector3(1.25f, 1.25f, 1.25f), t);
			yield return null;
		}
		yield break;
	}

	// Token: 0x06008F07 RID: 36615 RVA: 0x003C76F4 File Offset: 0x003C58F4
	private void ArrowReturnScale(dfControl control, dfMouseEventArgs mouseEvent)
	{
		control.transform.localScale = Vector3.one;
	}

	// Token: 0x06008F08 RID: 36616 RVA: 0x003C7708 File Offset: 0x003C5908
	private void ArrowHoverGrow(dfControl control, dfMouseEventArgs mouseEvent)
	{
		control.transform.localScale = new Vector3(1.25f, 1.25f, 1.25f);
	}

	// Token: 0x06008F09 RID: 36617 RVA: 0x003C772C File Offset: 0x003C592C
	private void SetUnselectedColors()
	{
		if (this.labelControl != null)
		{
			this.labelControl.Color = BraveOptionsMenuItem.m_unselectedColor;
		}
		if (this.buttonControl != null)
		{
			this.buttonControl.TextColor = BraveOptionsMenuItem.m_unselectedColor;
		}
		if (this.itemType == BraveOptionsMenuItem.BraveOptionsMenuItemType.LeftRightArrow || this.itemType == BraveOptionsMenuItem.BraveOptionsMenuItemType.LeftRightArrowInfo)
		{
			this.left.Color = BraveOptionsMenuItem.m_unselectedColor;
			this.right.Color = BraveOptionsMenuItem.m_unselectedColor;
			this.selectedLabelControl.Color = BraveOptionsMenuItem.m_unselectedColor;
		}
		else if (this.itemType == BraveOptionsMenuItem.BraveOptionsMenuItemType.Checkbox)
		{
			this.checkboxUnchecked.Color = BraveOptionsMenuItem.m_unselectedColor;
			this.checkboxChecked.Color = BraveOptionsMenuItem.m_unselectedColor;
		}
	}

	// Token: 0x06008F0A RID: 36618 RVA: 0x003C7818 File Offset: 0x003C5A18
	public void LostFocus(dfControl control, dfFocusEventArgs args)
	{
		this.SetUnselectedColors();
		InControlInputAdapter.CurrentlyUsingAllDevices = false;
		if (this.optionType == BraveOptionsMenuItem.BraveOptionsOptionType.RESOLUTION)
		{
			this.DoChangeResolution();
		}
		if (this.optionType == BraveOptionsMenuItem.BraveOptionsOptionType.SAVE_SLOT)
		{
			SaveManager.SaveSlot? targetSaveSlot = SaveManager.TargetSaveSlot;
			if (targetSaveSlot != null)
			{
				if (SaveManager.TargetSaveSlot.Value != SaveManager.CurrentSaveSlot)
				{
					this.AskToChangeSaveSlot();
				}
				else
				{
					SaveManager.TargetSaveSlot = null;
				}
			}
		}
	}

	// Token: 0x06008F0B RID: 36619 RVA: 0x003C7890 File Offset: 0x003C5A90
	private void AskToChangeSaveSlot()
	{
		FullOptionsMenuController OptionsMenu = GameUIRoot.Instance.PauseMenuPanel.GetComponent<PauseMenuController>().OptionsMenu;
		OptionsMenu.MainPanel.IsVisible = false;
		GameUIRoot.Instance.DoAreYouSure("#AYS_CHANGESAVESLOT", false, null);
		base.StartCoroutine(this.WaitForAreYouSure(new Action(this.ChangeSaveSlot), delegate
		{
			this.m_selectedIndex = (int)SaveManager.CurrentSaveSlot;
			this.HandleValueChanged();
			SaveManager.TargetSaveSlot = null;
			OptionsMenu.MainPanel.IsVisible = true;
			if (this.up)
			{
				this.up.GetComponent<BraveOptionsMenuItem>().LostFocus(null, null);
			}
			if (this.down)
			{
				BraveOptionsMenuItem component = this.down.GetComponent<BraveOptionsMenuItem>();
				if (component)
				{
					component.LostFocus(null, null);
				}
				else
				{
					this.down.Focus(true);
					this.down.Unfocus();
				}
			}
			this.m_self.Focus(true);
		}));
	}

	// Token: 0x06008F0C RID: 36620 RVA: 0x003C790C File Offset: 0x003C5B0C
	private void ChangeSaveSlot()
	{
		GameManager.Instance.LoadMainMenu();
	}

	// Token: 0x06008F0D RID: 36621 RVA: 0x003C7918 File Offset: 0x003C5B18
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
			GameOptions.Save();
		}
		else if (OnNo != null)
		{
			OnNo();
		}
		yield break;
	}

	// Token: 0x06008F0E RID: 36622 RVA: 0x003C793C File Offset: 0x003C5B3C
	private void DoSelectedAction()
	{
		if (this.itemType == BraveOptionsMenuItem.BraveOptionsMenuItemType.Checkbox)
		{
			this.ToggleCheckbox(null, null);
		}
		else if (this.itemType == BraveOptionsMenuItem.BraveOptionsMenuItemType.Button)
		{
			FullOptionsMenuController OptionsMenu = GameUIRoot.Instance.PauseMenuPanel.GetComponent<PauseMenuController>().OptionsMenu;
			if (this.optionType == BraveOptionsMenuItem.BraveOptionsOptionType.EDIT_KEYBOARD_BINDINGS)
			{
				FullOptionsMenuController.CurrentBindingPlayerTargetIndex = 0;
				OptionsMenu.ToggleToKeyboardBindingsPanel(false);
			}
			else if (this.optionType == BraveOptionsMenuItem.BraveOptionsOptionType.PLAYER_ONE_CONTROLLER_BINDINGS)
			{
				FullOptionsMenuController.CurrentBindingPlayerTargetIndex = 0;
				OptionsMenu.ToggleToKeyboardBindingsPanel(true);
			}
			else if (this.optionType == BraveOptionsMenuItem.BraveOptionsOptionType.PLAYER_TWO_CONTROLLER_BINDINGS)
			{
				FullOptionsMenuController.CurrentBindingPlayerTargetIndex = ((GameManager.Instance.AllPlayers.Length <= 1) ? 0 : GameManager.Instance.SecondaryPlayer.PlayerIDX);
				OptionsMenu.ToggleToKeyboardBindingsPanel(true);
			}
			else if (this.optionType == BraveOptionsMenuItem.BraveOptionsOptionType.VIEW_CREDITS)
			{
				OptionsMenu.ToggleToCredits();
			}
			else if (this.optionType == BraveOptionsMenuItem.BraveOptionsOptionType.VIEW_PRIVACY)
			{
				DataPrivacy.FetchPrivacyUrl(delegate(string url)
				{
					Application.OpenURL(url);
				}, null);
			}
			else if (this.optionType == BraveOptionsMenuItem.BraveOptionsOptionType.HOW_TO_PLAY)
			{
				OptionsMenu.ToggleToHowToPlay();
			}
			else if (this.optionType == BraveOptionsMenuItem.BraveOptionsOptionType.RESET_SAVE_SLOT)
			{
				OptionsMenu.MainPanel.IsVisible = false;
				GameUIRoot.Instance.DoAreYouSure("#AYS_RESETSAVESLOT", false, "#AYS_RESETSAVESLOT2");
				base.StartCoroutine(this.WaitForAreYouSure(delegate
				{
					GameUIRoot.Instance.DoAreYouSure("#AREYOUSURE", false, null);
					this.StartCoroutine(this.WaitForAreYouSure(delegate
					{
						SaveManager.ResetSaveSlot = true;
						GameManager.Instance.LoadMainMenu();
					}, delegate
					{
						OptionsMenu.MainPanel.IsVisible = true;
						this.m_self.Focus(true);
					}));
				}, delegate
				{
					OptionsMenu.MainPanel.IsVisible = true;
					this.m_self.Focus(true);
				}));
			}
		}
		else if (this.optionType == BraveOptionsMenuItem.BraveOptionsOptionType.RESOLUTION)
		{
			this.DoChangeResolution();
		}
		else if (this.optionType == BraveOptionsMenuItem.BraveOptionsOptionType.SAVE_SLOT)
		{
			SaveManager.SaveSlot? targetSaveSlot = SaveManager.TargetSaveSlot;
			if (targetSaveSlot != null && SaveManager.TargetSaveSlot.Value != SaveManager.CurrentSaveSlot)
			{
				this.AskToChangeSaveSlot();
			}
		}
	}

	// Token: 0x06008F0F RID: 36623 RVA: 0x003C7B44 File Offset: 0x003C5D44
	private void IncrementArrow(dfControl control, dfMouseEventArgs mouseEvent)
	{
		AkSoundEngine.PostEvent("Play_UI_menu_select_01", base.gameObject);
		this.m_selectedIndex = (this.m_selectedIndex + 1) % this.labelOptions.Length;
		this.HandleValueChanged();
	}

	// Token: 0x06008F10 RID: 36624 RVA: 0x003C7B74 File Offset: 0x003C5D74
	private void DecrementArrow(dfControl control, dfMouseEventArgs mouseEvent)
	{
		AkSoundEngine.PostEvent("Play_UI_menu_select_01", base.gameObject);
		this.m_selectedIndex = (this.m_selectedIndex - 1 + this.labelOptions.Length) % this.labelOptions.Length;
		this.HandleValueChanged();
	}

	// Token: 0x06008F11 RID: 36625 RVA: 0x003C7BB0 File Offset: 0x003C5DB0
	private void HandleValueChanged()
	{
		switch (this.itemType)
		{
		case BraveOptionsMenuItem.BraveOptionsMenuItemType.LeftRightArrow:
			this.HandleLeftRightArrowValueChanged();
			break;
		case BraveOptionsMenuItem.BraveOptionsMenuItemType.LeftRightArrowInfo:
			this.HandleLeftRightArrowValueChanged();
			break;
		case BraveOptionsMenuItem.BraveOptionsMenuItemType.Fillbar:
			this.HandleFillbarValueChanged();
			break;
		case BraveOptionsMenuItem.BraveOptionsMenuItemType.Checkbox:
			this.HandleCheckboxValueChanged();
			break;
		}
		if (this.selectedLabelControl)
		{
			this.selectedLabelControl.PerformLayout();
		}
		if (this.infoControl)
		{
			this.infoControl.PerformLayout();
		}
	}

	// Token: 0x06008F12 RID: 36626 RVA: 0x003C7C50 File Offset: 0x003C5E50
	private void HandleCheckboxValueChanged()
	{
		this.checkboxChecked.IsVisible = this.m_selectedIndex == 1;
		this.checkboxUnchecked.IsVisible = this.m_selectedIndex != 1;
		BraveOptionsMenuItem.BraveOptionsOptionType braveOptionsOptionType = this.optionType;
		switch (braveOptionsOptionType)
		{
		case BraveOptionsMenuItem.BraveOptionsOptionType.RUMBLE:
			GameManager.Options.RumbleEnabled = this.m_selectedIndex == 1;
			break;
		default:
			switch (braveOptionsOptionType)
			{
			case BraveOptionsMenuItem.BraveOptionsOptionType.COOP_SCREEN_SHAKE_AMOUNT:
				GameManager.Options.CoopScreenShakeReduction = this.m_selectedIndex == 1;
				break;
			default:
				if (braveOptionsOptionType != BraveOptionsMenuItem.BraveOptionsOptionType.VSYNC)
				{
					if (braveOptionsOptionType == BraveOptionsMenuItem.BraveOptionsOptionType.BEASTMODE)
					{
						GameManager.Options.m_beastmode = this.m_selectedIndex == 1;
					}
				}
				else
				{
					GameManager.Options.DoVsync = this.m_selectedIndex == 1;
				}
				break;
			case BraveOptionsMenuItem.BraveOptionsOptionType.REALTIME_REFLECTIONS:
				GameManager.Options.RealtimeReflections = this.m_selectedIndex == 1;
				break;
			case BraveOptionsMenuItem.BraveOptionsOptionType.QUICKSELECT:
				GameManager.Options.QuickSelectEnabled = this.m_selectedIndex == 1;
				break;
			case BraveOptionsMenuItem.BraveOptionsOptionType.HIDE_EMPTY_GUNS:
				GameManager.Options.HideEmptyGuns = this.m_selectedIndex == 1;
				break;
			case BraveOptionsMenuItem.BraveOptionsOptionType.SPEEDRUN:
				GameManager.Options.SpeedrunMode = this.m_selectedIndex == 1;
				break;
			}
			break;
		case BraveOptionsMenuItem.BraveOptionsOptionType.PLAYER_ONE_CONTROLLER_CURSOR:
			GameManager.Options.PlayerOneControllerCursor = this.m_selectedIndex == 1;
			break;
		case BraveOptionsMenuItem.BraveOptionsOptionType.PLAYER_TWO_CONTROLLER_CURSOR:
			GameManager.Options.PlayerTwoControllerCursor = this.m_selectedIndex == 1;
			break;
		case BraveOptionsMenuItem.BraveOptionsOptionType.ALLOW_UNKNOWN_CONTROLLERS:
			GameManager.Options.allowUnknownControllers = this.m_selectedIndex == 1;
			break;
		case BraveOptionsMenuItem.BraveOptionsOptionType.SMALL_UI:
			GameManager.Options.SmallUIEnabled = this.m_selectedIndex == 1;
			break;
		case BraveOptionsMenuItem.BraveOptionsOptionType.BOTH_CONTROLLER_CURSOR:
			GameManager.Options.PlayerOneControllerCursor = this.m_selectedIndex == 1;
			GameManager.Options.PlayerTwoControllerCursor = this.m_selectedIndex == 1;
			break;
		case BraveOptionsMenuItem.BraveOptionsOptionType.OUT_OF_COMBAT_SPEED_INCREASE:
			GameManager.Options.IncreaseSpeedOutOfCombat = this.m_selectedIndex == 1;
			break;
		case BraveOptionsMenuItem.BraveOptionsOptionType.CONTROLLER_BEAM_AIM_ASSIST:
			GameManager.Options.controllerBeamAimAssist = this.m_selectedIndex == 1;
			break;
		}
	}

	// Token: 0x06008F13 RID: 36627 RVA: 0x003C7E90 File Offset: 0x003C6090
	private List<Resolution> GetAvailableResolutions()
	{
		if (GameManager.Options.CurrentPreferredFullscreenMode == GameOptions.PreferredFullscreenMode.BORDERLESS)
		{
			return new List<Resolution>(new Resolution[] { Screen.currentResolution });
		}
		List<Resolution> list = new List<Resolution>();
		Resolution[] resolutions = Screen.resolutions;
		int refreshRate = Screen.currentResolution.refreshRate;
		foreach (Resolution resolution in resolutions)
		{
			this.AddResolutionInOrder(list, new Resolution
			{
				width = resolution.width,
				height = resolution.height,
				refreshRate = refreshRate
			});
		}
		if (GameManager.Options.CurrentPreferredFullscreenMode == GameOptions.PreferredFullscreenMode.WINDOWED || Application.platform == RuntimePlatform.OSXPlayer)
		{
			this.AddResolutionInOrder(list, new Resolution
			{
				width = Screen.width,
				height = Screen.height,
				refreshRate = Screen.currentResolution.refreshRate
			});
		}
		if (GameManager.Options.CurrentPreferredFullscreenMode == GameOptions.PreferredFullscreenMode.WINDOWED)
		{
			int num = 0;
			int num2 = 0;
			if (list.Count > 0)
			{
				num = list[list.Count - 1].width;
				num2 = list[list.Count - 1].height;
			}
			int num3 = 480;
			int num4 = 270;
			while (num3 <= num && num4 <= num2)
			{
				this.AddResolutionInOrder(list, new Resolution
				{
					width = num3,
					height = num4,
					refreshRate = refreshRate
				});
				num3 += 480;
				num4 += 270;
			}
		}
		return list;
	}

	// Token: 0x06008F14 RID: 36628 RVA: 0x003C8054 File Offset: 0x003C6254
	private void AddResolutionInOrder(List<Resolution> resolutions, Resolution newResolution)
	{
		for (int i = 0; i < resolutions.Count; i++)
		{
			if (resolutions[i].width == newResolution.width && resolutions[i].height == newResolution.height)
			{
				return;
			}
			if (resolutions[i].width > newResolution.width || (resolutions[i].width == newResolution.width && resolutions[i].height > newResolution.height))
			{
				resolutions.Insert(i, newResolution);
				return;
			}
		}
		resolutions.Add(newResolution);
	}

	// Token: 0x06008F15 RID: 36629 RVA: 0x003C8114 File Offset: 0x003C6314
	private void DoChangeResolution()
	{
		List<Resolution> availableResolutions = this.GetAvailableResolutions();
		this.m_selectedIndex = Mathf.Clamp(this.m_selectedIndex, 0, availableResolutions.Count - 1);
		if (availableResolutions[this.m_selectedIndex].width != Screen.width || availableResolutions[this.m_selectedIndex].height != Screen.height)
		{
			GameManager.Options.CurrentVisualPreset = GameOptions.VisualPresetMode.CUSTOM;
			BraveOptionsMenuItem.HandleScreenDataChanged(availableResolutions[this.m_selectedIndex].width, availableResolutions[this.m_selectedIndex].height);
		}
	}

	// Token: 0x06008F16 RID: 36630 RVA: 0x003C81B8 File Offset: 0x003C63B8
	private BraveInput GetBestInputInstance(int targetPlayerIndex)
	{
		BraveInput braveInput;
		if (GameManager.Instance.CurrentLevelOverrideState == GameManager.LevelOverrideState.FOYER && Foyer.DoMainMenu)
		{
			braveInput = BraveInput.PlayerlessInstance;
		}
		else if (targetPlayerIndex == -1)
		{
			braveInput = BraveInput.PrimaryPlayerInstance;
		}
		else
		{
			braveInput = BraveInput.GetInstanceForPlayer(targetPlayerIndex);
		}
		return braveInput;
	}

	// Token: 0x06008F17 RID: 36631 RVA: 0x003C8208 File Offset: 0x003C6408
	private void HandleLeftRightArrowValueChanged()
	{
		this.UpdateSelectedLabelText();
		BraveOptionsMenuItem.BraveOptionsOptionType braveOptionsOptionType = this.optionType;
		switch (braveOptionsOptionType)
		{
		case BraveOptionsMenuItem.BraveOptionsOptionType.SPEAKER_TYPE:
			GameManager.Options.AudioHardware = (GameOptions.AudioHardwareMode)this.m_selectedIndex;
			break;
		case BraveOptionsMenuItem.BraveOptionsOptionType.VISUAL_PRESET:
			GameManager.Options.CurrentVisualPreset = (GameOptions.VisualPresetMode)this.m_selectedIndex;
			break;
		case BraveOptionsMenuItem.BraveOptionsOptionType.RESOLUTION:
			break;
		case BraveOptionsMenuItem.BraveOptionsOptionType.SCALING_MODE:
			GameManager.Options.CurrentVisualPreset = GameOptions.VisualPresetMode.CUSTOM;
			if (this.m_scalingModes[this.m_selectedIndex] == GameOptions.PreferredScalingMode.PIXEL_PERFECT)
			{
				GameManager.Options.CurrentPreferredScalingMode = GameOptions.PreferredScalingMode.FORCE_PIXEL_PERFECT;
			}
			else if (this.m_scalingModes[this.m_selectedIndex] == GameOptions.PreferredScalingMode.UNIFORM_SCALING)
			{
				GameManager.Options.CurrentPreferredScalingMode = GameOptions.PreferredScalingMode.UNIFORM_SCALING;
			}
			else if (this.m_scalingModes[this.m_selectedIndex] == GameOptions.PreferredScalingMode.UNIFORM_SCALING_FAST)
			{
				GameManager.Options.CurrentPreferredScalingMode = GameOptions.PreferredScalingMode.UNIFORM_SCALING_FAST;
			}
			else if (this.m_scalingModes[this.m_selectedIndex] == GameOptions.PreferredScalingMode.FORCE_PIXEL_PERFECT)
			{
				GameManager.Options.CurrentPreferredScalingMode = GameOptions.PreferredScalingMode.FORCE_PIXEL_PERFECT;
			}
			BraveOptionsMenuItem.HandleScreenDataChanged(Screen.width, Screen.height);
			break;
		case BraveOptionsMenuItem.BraveOptionsOptionType.FULLSCREEN:
			GameManager.Options.CurrentVisualPreset = GameOptions.VisualPresetMode.CUSTOM;
			GameManager.Options.CurrentPreferredFullscreenMode = ((this.m_selectedIndex != 0) ? ((this.m_selectedIndex != 1) ? GameOptions.PreferredFullscreenMode.WINDOWED : GameOptions.PreferredFullscreenMode.BORDERLESS) : GameOptions.PreferredFullscreenMode.FULLSCREEN);
			BraveOptionsMenuItem.HandleScreenDataChanged(Screen.width, Screen.height);
			break;
		case BraveOptionsMenuItem.BraveOptionsOptionType.MONITOR_SELECT:
		{
			GameManager.Options.CurrentMonitorIndex = this.m_selectedIndex;
			PlayerPrefs.SetInt("UnitySelectMonitor", this.m_selectedIndex);
			this.DoChangeResolution();
			Resolution recommendedResolution = GameManager.Options.GetRecommendedResolution();
			if (Screen.width != recommendedResolution.width || Screen.height != recommendedResolution.height)
			{
				BraveOptionsMenuItem.HandleScreenDataChanged(recommendedResolution.width, recommendedResolution.height);
			}
			break;
		}
		default:
			if (braveOptionsOptionType != BraveOptionsMenuItem.BraveOptionsOptionType.LOOT_PROFILE)
			{
				if (braveOptionsOptionType == BraveOptionsMenuItem.BraveOptionsOptionType.AUTOAIM)
				{
					if (this.m_selectedIndex == 0)
					{
						GameManager.Options.controllerAutoAim = GameOptions.ControllerAutoAim.AUTO_DETECT;
					}
					if (this.m_selectedIndex == 1)
					{
						GameManager.Options.controllerAutoAim = GameOptions.ControllerAutoAim.ALWAYS;
					}
					if (this.m_selectedIndex == 2)
					{
						GameManager.Options.controllerAutoAim = GameOptions.ControllerAutoAim.NEVER;
					}
					if (this.m_selectedIndex == 3)
					{
						GameManager.Options.controllerAutoAim = GameOptions.ControllerAutoAim.COOP_ONLY;
					}
				}
			}
			else
			{
				if (this.m_selectedIndex == 0)
				{
					GameManager.Options.CurrentGameLootProfile = GameOptions.GameLootProfile.CURRENT;
				}
				if (this.m_selectedIndex == 1)
				{
					GameManager.Options.CurrentGameLootProfile = GameOptions.GameLootProfile.ORIGINAL;
				}
			}
			break;
		case BraveOptionsMenuItem.BraveOptionsOptionType.LIGHTING_QUALITY:
			GameManager.Options.LightingQuality = ((this.m_selectedIndex != 0) ? GameOptions.GenericHighMedLowOption.LOW : GameOptions.GenericHighMedLowOption.HIGH);
			break;
		case BraveOptionsMenuItem.BraveOptionsOptionType.SHADER_QUALITY:
			if (this.m_selectedIndex == 0)
			{
				GameManager.Options.ShaderQuality = GameOptions.GenericHighMedLowOption.HIGH;
			}
			if (this.m_selectedIndex == 1)
			{
				GameManager.Options.ShaderQuality = GameOptions.GenericHighMedLowOption.VERY_LOW;
			}
			if (this.m_selectedIndex == 2)
			{
				GameManager.Options.ShaderQuality = GameOptions.GenericHighMedLowOption.LOW;
			}
			if (this.m_selectedIndex == 3)
			{
				GameManager.Options.ShaderQuality = GameOptions.GenericHighMedLowOption.MEDIUM;
			}
			if (GameManager.Options.ShaderQuality == GameOptions.GenericHighMedLowOption.LOW || GameManager.Options.ShaderQuality == GameOptions.GenericHighMedLowOption.VERY_LOW)
			{
				GameManager.Options.RealtimeReflections = false;
			}
			break;
		case BraveOptionsMenuItem.BraveOptionsOptionType.DEBRIS_QUANTITY:
			if (this.m_selectedIndex == 0)
			{
				GameManager.Options.DebrisQuantity = GameOptions.GenericHighMedLowOption.HIGH;
			}
			if (this.m_selectedIndex == 1)
			{
				GameManager.Options.DebrisQuantity = GameOptions.GenericHighMedLowOption.VERY_LOW;
			}
			if (this.m_selectedIndex == 2)
			{
				GameManager.Options.DebrisQuantity = GameOptions.GenericHighMedLowOption.LOW;
			}
			if (this.m_selectedIndex == 3)
			{
				GameManager.Options.DebrisQuantity = GameOptions.GenericHighMedLowOption.MEDIUM;
			}
			break;
		case BraveOptionsMenuItem.BraveOptionsOptionType.TEXT_SPEED:
			if (this.m_selectedIndex == 0)
			{
				GameManager.Options.TextSpeed = GameOptions.GenericHighMedLowOption.MEDIUM;
			}
			if (this.m_selectedIndex == 1)
			{
				GameManager.Options.TextSpeed = GameOptions.GenericHighMedLowOption.HIGH;
			}
			if (this.m_selectedIndex == 2)
			{
				GameManager.Options.TextSpeed = GameOptions.GenericHighMedLowOption.LOW;
			}
			break;
		case BraveOptionsMenuItem.BraveOptionsOptionType.PLAYER_ONE_CONTROL_PORT:
			BraveInput.ReassignPlayerPort(0, this.m_selectedIndex);
			this.m_ignoreLeftRightUntilReleased = true;
			break;
		case BraveOptionsMenuItem.BraveOptionsOptionType.PLAYER_ONE_CONTROLLER_SYMBOLOGY:
			if (this.m_selectedIndex == 0)
			{
				GameManager.Options.PlayerOnePreferredSymbology = GameOptions.ControllerSymbology.PS4;
			}
			if (this.m_selectedIndex == 1)
			{
				GameManager.Options.PlayerOnePreferredSymbology = GameOptions.ControllerSymbology.Xbox;
			}
			if (this.m_selectedIndex == 2)
			{
				GameManager.Options.PlayerOnePreferredSymbology = GameOptions.ControllerSymbology.AutoDetect;
			}
			if (this.m_selectedIndex == 3)
			{
				GameManager.Options.PlayerOnePreferredSymbology = GameOptions.ControllerSymbology.Switch;
			}
			break;
		case BraveOptionsMenuItem.BraveOptionsOptionType.PLAYER_TWO_CONTROL_PORT:
			if (GameManager.Instance.AllPlayers.Length > 1)
			{
				BraveInput.ReassignPlayerPort(GameManager.Instance.SecondaryPlayer.PlayerIDX, this.m_selectedIndex);
			}
			break;
		case BraveOptionsMenuItem.BraveOptionsOptionType.PLAYER_TWO_CONTROLLER_SYMBOLOGY:
			if (this.m_selectedIndex == 0)
			{
				GameManager.Options.PlayerTwoPreferredSymbology = GameOptions.ControllerSymbology.PS4;
			}
			if (this.m_selectedIndex == 1)
			{
				GameManager.Options.PlayerTwoPreferredSymbology = GameOptions.ControllerSymbology.Xbox;
			}
			if (this.m_selectedIndex == 2)
			{
				GameManager.Options.PlayerTwoPreferredSymbology = GameOptions.ControllerSymbology.AutoDetect;
			}
			if (this.m_selectedIndex == 3)
			{
				GameManager.Options.PlayerOnePreferredSymbology = GameOptions.ControllerSymbology.Switch;
			}
			break;
		case BraveOptionsMenuItem.BraveOptionsOptionType.MINIMAP_STYLE:
			if (this.m_selectedIndex == 0)
			{
				GameManager.Options.MinimapDisplayMode = Minimap.MinimapDisplayMode.FADE_ON_ROOM_SEAL;
			}
			if (this.m_selectedIndex == 1)
			{
				GameManager.Options.MinimapDisplayMode = Minimap.MinimapDisplayMode.NEVER;
			}
			if (this.m_selectedIndex == 2)
			{
				GameManager.Options.MinimapDisplayMode = Minimap.MinimapDisplayMode.ALWAYS;
			}
			break;
		case BraveOptionsMenuItem.BraveOptionsOptionType.LANGUAGE:
			GameManager.Options.CurrentLanguage = this.IntToLanguage(this.m_selectedIndex);
			break;
		case BraveOptionsMenuItem.BraveOptionsOptionType.QUICKSTART_CHARACTER:
			if (this.m_selectedIndex >= 0 && this.m_selectedIndex < this.m_quickStartCharacters.Count)
			{
				GameManager.Options.PreferredQuickstartCharacter = this.m_quickStartCharacters[this.m_selectedIndex];
			}
			else
			{
				GameManager.Options.PreferredQuickstartCharacter = GameOptions.QuickstartCharacter.LAST_USED;
			}
			break;
		case BraveOptionsMenuItem.BraveOptionsOptionType.ADDITIONAL_BLANK_CONTROL:
			GameManager.Options.additionalBlankControl = (GameOptions.ControllerBlankControl)this.m_selectedIndex;
			break;
		case BraveOptionsMenuItem.BraveOptionsOptionType.ADDITIONAL_BLANK_CONTROL_TWO:
			GameManager.Options.additionalBlankControlTwo = (GameOptions.ControllerBlankControl)this.m_selectedIndex;
			break;
		case BraveOptionsMenuItem.BraveOptionsOptionType.CURRENT_BINDINGS_PRESET:
		{
			FullOptionsMenuController optionsMenu = GameUIRoot.Instance.PauseMenuPanel.GetComponent<PauseMenuController>().OptionsMenu;
			GameManager.Options.CurrentControlPreset = (GameOptions.ControlPreset)this.m_selectedIndex;
			optionsMenu.ReinitializeKeyboardBindings();
			this.selectedLabelControl.PerformLayout();
			break;
		}
		case BraveOptionsMenuItem.BraveOptionsOptionType.CURRENT_BINDINGS_PRESET_P2:
		{
			FullOptionsMenuController optionsMenu2 = GameUIRoot.Instance.PauseMenuPanel.GetComponent<PauseMenuController>().OptionsMenu;
			GameManager.Options.CurrentControlPresetP2 = (GameOptions.ControlPreset)this.m_selectedIndex;
			optionsMenu2.ReinitializeKeyboardBindings();
			this.selectedLabelControl.PerformLayout();
			break;
		}
		case BraveOptionsMenuItem.BraveOptionsOptionType.SAVE_SLOT:
			if (this.m_selectedIndex == 0)
			{
				SaveManager.TargetSaveSlot = new SaveManager.SaveSlot?(SaveManager.SaveSlot.A);
			}
			if (this.m_selectedIndex == 1)
			{
				SaveManager.TargetSaveSlot = new SaveManager.SaveSlot?(SaveManager.SaveSlot.B);
			}
			if (this.m_selectedIndex == 2)
			{
				SaveManager.TargetSaveSlot = new SaveManager.SaveSlot?(SaveManager.SaveSlot.C);
			}
			if (this.m_selectedIndex == 3)
			{
				SaveManager.TargetSaveSlot = new SaveManager.SaveSlot?(SaveManager.SaveSlot.D);
			}
			break;
		case BraveOptionsMenuItem.BraveOptionsOptionType.CURSOR_VARIATION:
			GameManager.Options.CurrentCursorIndex = this.m_selectedIndex;
			break;
		case BraveOptionsMenuItem.BraveOptionsOptionType.ADDITIONAL_BLANK_CONTROL_PS4:
			if (FullOptionsMenuController.CurrentBindingPlayerTargetIndex == 0)
			{
				GameManager.Options.additionalBlankControl = (GameOptions.ControllerBlankControl)this.m_selectedIndex;
			}
			else
			{
				GameManager.Options.additionalBlankControlTwo = (GameOptions.ControllerBlankControl)this.m_selectedIndex;
			}
			break;
		case BraveOptionsMenuItem.BraveOptionsOptionType.ALLOWED_CONTROLLER_TYPES:
			if (this.m_selectedIndex == 0)
			{
				GameManager.Options.allowXinputControllers = true;
				GameManager.Options.allowNonXinputControllers = true;
			}
			if (this.m_selectedIndex == 1)
			{
				GameManager.Options.allowXinputControllers = true;
				GameManager.Options.allowNonXinputControllers = false;
			}
			if (this.m_selectedIndex == 2)
			{
				GameManager.Options.allowXinputControllers = false;
				GameManager.Options.allowNonXinputControllers = true;
			}
			InControlInputAdapter.SkipInputForRestOfFrame = true;
			break;
		}
		if (this.itemType == BraveOptionsMenuItem.BraveOptionsMenuItemType.LeftRightArrowInfo)
		{
			this.UpdateInfoControl();
		}
	}

	// Token: 0x06008F18 RID: 36632 RVA: 0x003C8A08 File Offset: 0x003C6C08
	private static void HandleScreenDataChanged(int screenWidth, int screenHeight)
	{
		if (GameManager.Instance.IsLoadingLevel || GameManager.IsReturningToBreach)
		{
			return;
		}
		GameOptions.PreferredFullscreenMode currentPreferredFullscreenMode = GameManager.Options.CurrentPreferredFullscreenMode;
		Resolution resolution = default(Resolution);
		if (currentPreferredFullscreenMode == GameOptions.PreferredFullscreenMode.BORDERLESS)
		{
			screenWidth = Screen.currentResolution.width;
			screenHeight = Screen.currentResolution.height;
		}
		GameManager.Options.preferredResolutionX = screenWidth;
		GameManager.Options.preferredResolutionY = screenHeight;
		resolution.width = screenWidth;
		resolution.height = screenHeight;
		resolution.refreshRate = Screen.currentResolution.refreshRate;
		BraveOptionsMenuItem.WindowsResolutionManager.DisplayModes displayModes = BraveOptionsMenuItem.WindowsResolutionManager.DisplayModes.Fullscreen;
		if (currentPreferredFullscreenMode == GameOptions.PreferredFullscreenMode.BORDERLESS)
		{
			displayModes = BraveOptionsMenuItem.WindowsResolutionManager.DisplayModes.Borderless;
		}
		if (currentPreferredFullscreenMode == GameOptions.PreferredFullscreenMode.WINDOWED)
		{
			displayModes = BraveOptionsMenuItem.WindowsResolutionManager.DisplayModes.Windowed;
		}
		bool flag = Screen.fullScreen != (currentPreferredFullscreenMode == GameOptions.PreferredFullscreenMode.FULLSCREEN);
		if (flag)
		{
			BraveOptionsMenuItem componentInChildren = GameUIRoot.Instance.PauseMenuPanel.GetComponent<PauseMenuController>().OptionsMenu.GetComponentInChildren<BraveOptionsMenuItem>();
			componentInChildren.StartCoroutine(componentInChildren.FrameDelayedWindowsShift(displayModes, resolution));
		}
		else
		{
			BraveOptionsMenuItem.ResolutionManagerWin.TrySetDisplay(displayModes, resolution, false, null);
		}
		if (screenWidth != Screen.width || screenHeight != Screen.height || currentPreferredFullscreenMode == GameOptions.PreferredFullscreenMode.FULLSCREEN != Screen.fullScreen)
		{
			Debug.Log(string.Concat(new object[]
			{
				"BOMI setting resolution to: ",
				screenWidth,
				"|",
				screenHeight,
				"||",
				currentPreferredFullscreenMode.ToString()
			}));
			GameManager.Instance.DoSetResolution(screenWidth, screenHeight, currentPreferredFullscreenMode == GameOptions.PreferredFullscreenMode.FULLSCREEN);
		}
	}

	// Token: 0x06008F19 RID: 36633 RVA: 0x003C8B9C File Offset: 0x003C6D9C
	public IEnumerator FrameDelayedWindowsShift(BraveOptionsMenuItem.WindowsResolutionManager.DisplayModes targetDisplayMode, Resolution targetRes)
	{
		yield return null;
		BraveOptionsMenuItem.ResolutionManagerWin.TrySetDisplay(targetDisplayMode, targetRes, false, null);
		yield return null;
		yield break;
	}

	// Token: 0x06008F1A RID: 36634 RVA: 0x003C8BC0 File Offset: 0x003C6DC0
	private void HandleFillbarValueChanged()
	{
		BraveOptionsMenuItem.BraveOptionsOptionType braveOptionsOptionType = this.optionType;
		switch (braveOptionsOptionType)
		{
		case BraveOptionsMenuItem.BraveOptionsOptionType.MUSIC_VOLUME:
			GameManager.Options.MusicVolume = Mathf.Clamp(this.m_actualFillbarValue * 100f, 0f, 100f);
			break;
		case BraveOptionsMenuItem.BraveOptionsOptionType.SOUND_VOLUME:
			GameManager.Options.SoundVolume = Mathf.Clamp(this.m_actualFillbarValue * 100f, 0f, 100f);
			break;
		case BraveOptionsMenuItem.BraveOptionsOptionType.UI_VOLUME:
			GameManager.Options.UIVolume = Mathf.Clamp(this.m_actualFillbarValue * 100f, 0f, 100f);
			break;
		default:
			switch (braveOptionsOptionType)
			{
			case BraveOptionsMenuItem.BraveOptionsOptionType.SCREEN_SHAKE_AMOUNT:
				GameManager.Options.ScreenShakeMultiplier = this.m_actualFillbarValue / 0.5f;
				break;
			case BraveOptionsMenuItem.BraveOptionsOptionType.STICKY_FRICTION_AMOUNT:
				GameManager.Options.StickyFrictionMultiplier = this.m_actualFillbarValue / 0.8f;
				break;
			default:
				if (braveOptionsOptionType != BraveOptionsMenuItem.BraveOptionsOptionType.CONTROLLER_AIM_LOOK)
				{
					if (braveOptionsOptionType != BraveOptionsMenuItem.BraveOptionsOptionType.GAMMA)
					{
						if (braveOptionsOptionType == BraveOptionsMenuItem.BraveOptionsOptionType.DISPLAY_SAFE_AREA)
						{
							GameManager.Options.DisplaySafeArea = Mathf.Clamp(BraveMathCollege.QuantizeFloat(this.m_actualFillbarValue, 0.2f) * 0.1f + 0.9f, 0.9f, 1f);
						}
					}
					else
					{
						GameManager.Options.Gamma = Mathf.Clamp(this.m_actualFillbarValue + 0.5f, 0.5f, 1.5f);
					}
				}
				else
				{
					GameManager.Options.controllerAimLookMultiplier = Mathf.Clamp(this.m_actualFillbarValue / 0.8f, 0f, 1.25f);
				}
				break;
			case BraveOptionsMenuItem.BraveOptionsOptionType.CONTROLLER_AIM_ASSIST_AMOUNT:
				GameManager.Options.controllerAimAssistMultiplier = Mathf.Clamp(this.m_actualFillbarValue / 0.8f, 0f, 1.25f);
				break;
			}
			break;
		}
	}

	// Token: 0x06008F1B RID: 36635 RVA: 0x003C8D90 File Offset: 0x003C6F90
	public void OnKeyUp(dfControl sender, dfKeyEventArgs args)
	{
		if (!args.Used)
		{
			if (args.KeyCode == KeyCode.LeftArrow)
			{
				this.m_ignoreLeftRightUntilReleased = false;
			}
			else if (args.KeyCode == KeyCode.RightArrow)
			{
				this.m_ignoreLeftRightUntilReleased = false;
			}
		}
	}

	// Token: 0x17001568 RID: 5480
	// (get) Token: 0x06008F1C RID: 36636 RVA: 0x003C8DD0 File Offset: 0x003C6FD0
	private float FillbarDelta
	{
		get
		{
			return (this.optionType != BraveOptionsMenuItem.BraveOptionsOptionType.DISPLAY_SAFE_AREA) ? 0.05f : 0.2f;
		}
	}

	// Token: 0x06008F1D RID: 36637 RVA: 0x003C8DF0 File Offset: 0x003C6FF0
	public void OnKeyDown(dfControl sender, dfKeyEventArgs args)
	{
		if (!args.Used)
		{
			if (args.KeyCode == KeyCode.UpArrow && this.up)
			{
				if (this.optionType == BraveOptionsMenuItem.BraveOptionsOptionType.RESOLUTION)
				{
					this.DoChangeResolution();
				}
				if (this.OnNewControlSelected != null)
				{
					this.OnNewControlSelected(this.up);
				}
				this.up.Focus(true);
			}
			else if (args.KeyCode == KeyCode.DownArrow && this.down)
			{
				if (this.optionType == BraveOptionsMenuItem.BraveOptionsOptionType.RESOLUTION)
				{
					this.DoChangeResolution();
				}
				if (this.OnNewControlSelected != null)
				{
					this.OnNewControlSelected(this.down);
				}
				this.down.Focus(true);
			}
			else if (args.KeyCode == KeyCode.LeftArrow)
			{
				if (this.itemType == BraveOptionsMenuItem.BraveOptionsMenuItemType.Fillbar)
				{
					this.m_actualFillbarValue = Mathf.Clamp01(this.m_actualFillbarValue - this.FillbarDelta);
					this.fillbarControl.Value = this.m_actualFillbarValue * 0.98f + 0.01f;
					this.HandleValueChanged();
				}
				else if (this.itemType == BraveOptionsMenuItem.BraveOptionsMenuItemType.LeftRightArrow)
				{
					if (!this.m_ignoreLeftRightUntilReleased)
					{
						this.DecrementArrow(null, null);
						this.DoArrowBounce(this.left);
					}
				}
				else if (this.itemType == BraveOptionsMenuItem.BraveOptionsMenuItemType.LeftRightArrowInfo)
				{
					if (!this.m_ignoreLeftRightUntilReleased)
					{
						this.DecrementArrow(null, null);
						this.DoArrowBounce(this.left);
					}
				}
				else if (this.left)
				{
					if (this.OnNewControlSelected != null)
					{
						this.OnNewControlSelected(this.left);
					}
					this.left.Focus(true);
				}
			}
			else if (args.KeyCode == KeyCode.RightArrow)
			{
				if (this.itemType == BraveOptionsMenuItem.BraveOptionsMenuItemType.Fillbar)
				{
					this.m_actualFillbarValue = Mathf.Clamp01(this.m_actualFillbarValue + this.FillbarDelta);
					this.fillbarControl.Value = this.m_actualFillbarValue * 0.98f + 0.01f;
					this.HandleValueChanged();
				}
				else if (this.itemType == BraveOptionsMenuItem.BraveOptionsMenuItemType.LeftRightArrow)
				{
					if (!this.m_ignoreLeftRightUntilReleased)
					{
						this.IncrementArrow(null, null);
						this.DoArrowBounce(this.right);
					}
				}
				else if (this.itemType == BraveOptionsMenuItem.BraveOptionsMenuItemType.LeftRightArrowInfo)
				{
					if (!this.m_ignoreLeftRightUntilReleased)
					{
						this.IncrementArrow(null, null);
						this.DoArrowBounce(this.right);
					}
				}
				else if (this.right)
				{
					if (this.OnNewControlSelected != null)
					{
						this.OnNewControlSelected(this.right);
					}
					this.right.Focus(true);
				}
			}
			if (this.selectOnAction && args.KeyCode == KeyCode.Return)
			{
				this.DoSelectedAction();
				args.Use();
			}
		}
	}

	// Token: 0x06008F1E RID: 36638 RVA: 0x003C90D8 File Offset: 0x003C72D8
	public void UpdateLabelOptions(int playerIndex)
	{
		BraveOptionsMenuItem.BraveOptionsOptionType braveOptionsOptionType = this.optionType;
		if (braveOptionsOptionType != BraveOptionsMenuItem.BraveOptionsOptionType.CURRENT_BINDINGS_PRESET && braveOptionsOptionType != BraveOptionsMenuItem.BraveOptionsOptionType.CURRENT_BINDINGS_PRESET_P2)
		{
			if (braveOptionsOptionType == BraveOptionsMenuItem.BraveOptionsOptionType.ADDITIONAL_BLANK_CONTROL_PS4)
			{
				this.labelOptions = new string[]
				{
					this.selectedLabelControl.ForceGetLocalizedValue("#OPTIONS_NONE"),
					"%CONTROL_L_STICK_DOWN %CONTROL_R_STICK_DOWN"
				};
			}
		}
		else
		{
			this.labelOptions = new string[]
			{
				this.selectedLabelControl.ForceGetLocalizedValue("#OPTIONS_RECOMMENDED") + " 1",
				this.selectedLabelControl.ForceGetLocalizedValue("#OPTIONS_RECOMMENDED") + " 2",
				this.selectedLabelControl.ForceGetLocalizedValue("#OPTIONS_CUSTOM")
			};
			if (GameManager.Options.CurrentLanguage == StringTableManager.GungeonSupportedLanguages.ENGLISH)
			{
				this.labelOptions[0] = "Recommended";
				this.labelOptions[1] = "Flipped Triggers";
			}
		}
	}

	// Token: 0x040096E9 RID: 38633
	public BraveOptionsMenuItem.BraveOptionsOptionType optionType;

	// Token: 0x040096EA RID: 38634
	[Header("Control Options")]
	public BraveOptionsMenuItem.BraveOptionsMenuItemType itemType;

	// Token: 0x040096EB RID: 38635
	public dfLabel labelControl;

	// Token: 0x040096EC RID: 38636
	[Space(5f)]
	public dfLabel selectedLabelControl;

	// Token: 0x040096ED RID: 38637
	public dfLabel infoControl;

	// Token: 0x040096EE RID: 38638
	public dfProgressBar fillbarControl;

	// Token: 0x040096EF RID: 38639
	public dfButton buttonControl;

	// Token: 0x040096F0 RID: 38640
	public dfControl checkboxChecked;

	// Token: 0x040096F1 RID: 38641
	public dfControl checkboxUnchecked;

	// Token: 0x040096F2 RID: 38642
	public string[] labelOptions;

	// Token: 0x040096F3 RID: 38643
	public string[] infoOptions;

	// Token: 0x040096F4 RID: 38644
	[Header("UI Key Controls")]
	public dfControl up;

	// Token: 0x040096F5 RID: 38645
	public dfControl down;

	// Token: 0x040096F6 RID: 38646
	public dfControl left;

	// Token: 0x040096F7 RID: 38647
	public dfControl right;

	// Token: 0x040096F8 RID: 38648
	public bool selectOnAction;

	// Token: 0x040096F9 RID: 38649
	public Action<dfControl> OnNewControlSelected;

	// Token: 0x040096FA RID: 38650
	private int m_selectedIndex;

	// Token: 0x040096FB RID: 38651
	private dfControl m_self;

	// Token: 0x040096FC RID: 38652
	private bool m_isLocalized;

	// Token: 0x040096FD RID: 38653
	private float m_actualFillbarValue;

	// Token: 0x040096FE RID: 38654
	private const float c_arrowScale = 3f;

	// Token: 0x040096FF RID: 38655
	private static BraveOptionsMenuItem.WindowsResolutionManager m_windowsResolutionManager;

	// Token: 0x04009700 RID: 38656
	private bool m_changedThisFrame;

	// Token: 0x04009701 RID: 38657
	private Vector3? m_cachedLeftArrowRelativePosition;

	// Token: 0x04009702 RID: 38658
	private Vector3? m_cachedRightArrowRelativePosition;

	// Token: 0x04009703 RID: 38659
	private List<GameOptions.PreferredScalingMode> m_scalingModes;

	// Token: 0x04009704 RID: 38660
	private List<GameOptions.QuickstartCharacter> m_quickStartCharacters;

	// Token: 0x04009705 RID: 38661
	private float m_panelStartHeight = -1f;

	// Token: 0x04009706 RID: 38662
	private float m_additionalStartHeight = -1f;

	// Token: 0x04009707 RID: 38663
	private bool m_infoControlHeightModified;

	// Token: 0x04009708 RID: 38664
	private static Color m_unselectedColor = new Color(0.5f, 0.5f, 0.5f, 1f);

	// Token: 0x04009709 RID: 38665
	private bool m_ignoreLeftRightUntilReleased;

	// Token: 0x020017CA RID: 6090
	public enum BraveOptionsMenuItemType
	{
		// Token: 0x0400970C RID: 38668
		LeftRightArrow,
		// Token: 0x0400970D RID: 38669
		LeftRightArrowInfo,
		// Token: 0x0400970E RID: 38670
		Fillbar,
		// Token: 0x0400970F RID: 38671
		Checkbox,
		// Token: 0x04009710 RID: 38672
		Button
	}

	// Token: 0x020017CB RID: 6091
	public enum BraveOptionsOptionType
	{
		// Token: 0x04009712 RID: 38674
		NONE,
		// Token: 0x04009713 RID: 38675
		MUSIC_VOLUME,
		// Token: 0x04009714 RID: 38676
		SOUND_VOLUME,
		// Token: 0x04009715 RID: 38677
		UI_VOLUME,
		// Token: 0x04009716 RID: 38678
		SPEAKER_TYPE,
		// Token: 0x04009717 RID: 38679
		VISUAL_PRESET,
		// Token: 0x04009718 RID: 38680
		RESOLUTION,
		// Token: 0x04009719 RID: 38681
		SCALING_MODE,
		// Token: 0x0400971A RID: 38682
		FULLSCREEN,
		// Token: 0x0400971B RID: 38683
		MONITOR_SELECT,
		// Token: 0x0400971C RID: 38684
		VSYNC,
		// Token: 0x0400971D RID: 38685
		LIGHTING_QUALITY,
		// Token: 0x0400971E RID: 38686
		SHADER_QUALITY,
		// Token: 0x0400971F RID: 38687
		DEBRIS_QUANTITY,
		// Token: 0x04009720 RID: 38688
		SCREEN_SHAKE_AMOUNT,
		// Token: 0x04009721 RID: 38689
		STICKY_FRICTION_AMOUNT,
		// Token: 0x04009722 RID: 38690
		TEXT_SPEED,
		// Token: 0x04009723 RID: 38691
		CONTROLLER_AIM_ASSIST_AMOUNT,
		// Token: 0x04009724 RID: 38692
		BEASTMODE,
		// Token: 0x04009725 RID: 38693
		EDIT_KEYBOARD_BINDINGS,
		// Token: 0x04009726 RID: 38694
		PLAYER_ONE_CONTROL_PORT,
		// Token: 0x04009727 RID: 38695
		PLAYER_ONE_CONTROLLER_SYMBOLOGY,
		// Token: 0x04009728 RID: 38696
		PLAYER_ONE_CONTROLLER_BINDINGS,
		// Token: 0x04009729 RID: 38697
		PLAYER_TWO_CONTROL_PORT,
		// Token: 0x0400972A RID: 38698
		PLAYER_TWO_CONTROLLER_SYMBOLOGY,
		// Token: 0x0400972B RID: 38699
		PLAYER_TWO_CONTROLLER_BINDINGS,
		// Token: 0x0400972C RID: 38700
		VIEW_CREDITS,
		// Token: 0x0400972D RID: 38701
		MINIMAP_STYLE,
		// Token: 0x0400972E RID: 38702
		COOP_SCREEN_SHAKE_AMOUNT,
		// Token: 0x0400972F RID: 38703
		CONTROLLER_AIM_LOOK,
		// Token: 0x04009730 RID: 38704
		GAMMA,
		// Token: 0x04009731 RID: 38705
		REALTIME_REFLECTIONS,
		// Token: 0x04009732 RID: 38706
		QUICKSELECT,
		// Token: 0x04009733 RID: 38707
		HIDE_EMPTY_GUNS,
		// Token: 0x04009734 RID: 38708
		HOW_TO_PLAY,
		// Token: 0x04009735 RID: 38709
		LANGUAGE,
		// Token: 0x04009736 RID: 38710
		SPEEDRUN,
		// Token: 0x04009737 RID: 38711
		QUICKSTART_CHARACTER,
		// Token: 0x04009738 RID: 38712
		ADDITIONAL_BLANK_CONTROL,
		// Token: 0x04009739 RID: 38713
		ADDITIONAL_BLANK_CONTROL_TWO,
		// Token: 0x0400973A RID: 38714
		CURRENT_BINDINGS_PRESET,
		// Token: 0x0400973B RID: 38715
		CURRENT_BINDINGS_PRESET_P2,
		// Token: 0x0400973C RID: 38716
		SAVE_SLOT,
		// Token: 0x0400973D RID: 38717
		RESET_SAVE_SLOT,
		// Token: 0x0400973E RID: 38718
		RUMBLE,
		// Token: 0x0400973F RID: 38719
		CURSOR_VARIATION,
		// Token: 0x04009740 RID: 38720
		ADDITIONAL_BLANK_CONTROL_PS4,
		// Token: 0x04009741 RID: 38721
		PLAYER_ONE_CONTROLLER_CURSOR,
		// Token: 0x04009742 RID: 38722
		PLAYER_TWO_CONTROLLER_CURSOR,
		// Token: 0x04009743 RID: 38723
		ALLOWED_CONTROLLER_TYPES,
		// Token: 0x04009744 RID: 38724
		ALLOW_UNKNOWN_CONTROLLERS,
		// Token: 0x04009745 RID: 38725
		SMALL_UI,
		// Token: 0x04009746 RID: 38726
		BOTH_CONTROLLER_CURSOR,
		// Token: 0x04009747 RID: 38727
		DISPLAY_SAFE_AREA,
		// Token: 0x04009748 RID: 38728
		GAMEPLAY_SPEED,
		// Token: 0x04009749 RID: 38729
		OUT_OF_COMBAT_SPEED_INCREASE,
		// Token: 0x0400974A RID: 38730
		CONTROLLER_BEAM_AIM_ASSIST,
		// Token: 0x0400974B RID: 38731
		SWITCH_PERFORMANCE_MODE,
		// Token: 0x0400974C RID: 38732
		SWITCH_REASSIGN_CONTROLLERS,
		// Token: 0x0400974D RID: 38733
		LOOT_PROFILE,
		// Token: 0x0400974E RID: 38734
		AUTOAIM,
		// Token: 0x0400974F RID: 38735
		VIEW_PRIVACY
	}

	// Token: 0x020017CC RID: 6092
	public class WindowsResolutionManager
	{
		// Token: 0x06008F23 RID: 36643 RVA: 0x003C91F4 File Offset: 0x003C73F4
		public WindowsResolutionManager(string title)
		{
			this._title = title;
		}

		// Token: 0x06008F24 RID: 36644
		[DllImport("user32.dll", CharSet = CharSet.Auto, EntryPoint = "SetWindowLong", SetLastError = true)]
		public static extern IntPtr SetWindowLongPtr(IntPtr hWnd, int nIndex, int dwNewLong);

		// Token: 0x06008F25 RID: 36645
		[DllImport("user32.dll", CharSet = CharSet.Auto, EntryPoint = "GetWindowLong", SetLastError = true)]
		public static extern IntPtr GetWindowLongPtr(IntPtr hWnd, int nIndex);

		// Token: 0x06008F26 RID: 36646
		[DllImport("user32.dll", EntryPoint = "FindWindow", SetLastError = true)]
		public static extern IntPtr FindWindowByCaption(IntPtr ZeroOnly, string lpWindowName);

		// Token: 0x06008F27 RID: 36647
		[DllImport("user32.dll", SetLastError = true)]
		public static extern IntPtr GetDesktopWindow();

		// Token: 0x06008F28 RID: 36648
		[DllImport("user32.dll", SetLastError = true)]
		public static extern IntPtr SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int x, int Y, int cx, int cy, int uFlags);

		// Token: 0x06008F29 RID: 36649
		[DllImport("user32.dll", SetLastError = true)]
		public static extern bool GetWindowRect(IntPtr hWnd, out BraveOptionsMenuItem.WindowsResolutionManager.RECT rect);

		// Token: 0x06008F2A RID: 36650
		[DllImport("user32.dll", SetLastError = true)]
		public static extern IntPtr MonitorFromWindow(IntPtr hWnd, uint flags);

		// Token: 0x06008F2B RID: 36651
		[DllImport("user32.dll", SetLastError = true)]
		public static extern bool GetClientRect(IntPtr hWnd, out BraveOptionsMenuItem.WindowsResolutionManager.RECT rect);

		// Token: 0x06008F2C RID: 36652
		[DllImport("User32.dll", CharSet = CharSet.Auto)]
		public static extern bool GetMonitorInfo(IntPtr hmonitor, [In] [Out] BraveOptionsMenuItem.WindowsResolutionManager.MONITORINFOEX info);

		// Token: 0x06008F2D RID: 36653 RVA: 0x003C9204 File Offset: 0x003C7404
		public Position? GetWindowPosition()
		{
			BraveOptionsMenuItem.WindowsResolutionManager.RECT rect;
			if (!BraveOptionsMenuItem.WindowsResolutionManager.GetWindowRect(this.Window, out rect))
			{
				return null;
			}
			return new Position?(new Position(rect.Left, rect.Top));
		}

		// Token: 0x06008F2E RID: 36654 RVA: 0x003C9248 File Offset: 0x003C7448
		public Position? GetCenteredPosition(Resolution resolution, BraveOptionsMenuItem.WindowsResolutionManager.DisplayModes displayMode)
		{
			BraveOptionsMenuItem.WindowsResolutionManager.RECT rect;
			if (!BraveOptionsMenuItem.WindowsResolutionManager.GetWindowRect(this.Desktop, out rect))
			{
				return null;
			}
			int num = rect.Right - rect.Left;
			int num2 = rect.Bottom - rect.Top;
			int num3 = 0;
			int num4 = 0;
			IntPtr intPtr = BraveOptionsMenuItem.WindowsResolutionManager.MonitorFromWindow(this.Window, 2U);
			BraveOptionsMenuItem.WindowsResolutionManager.MONITORINFOEX monitorinfoex = new BraveOptionsMenuItem.WindowsResolutionManager.MONITORINFOEX();
			if (BraveOptionsMenuItem.WindowsResolutionManager.GetMonitorInfo(intPtr, monitorinfoex))
			{
				num = monitorinfoex.rcMonitor.Right - monitorinfoex.rcMonitor.Left;
				num2 = monitorinfoex.rcMonitor.Bottom - monitorinfoex.rcMonitor.Top;
				num3 = monitorinfoex.rcMonitor.Left;
				num4 = monitorinfoex.rcMonitor.Top;
			}
			int num5;
			int num6;
			if (displayMode == BraveOptionsMenuItem.WindowsResolutionManager.DisplayModes.Windowed)
			{
				num5 = (num - (resolution.width + this._borderWidth * 2)) / 2;
				num6 = (num2 - (resolution.height + this._borderWidth * 2 + this._captionHeight)) / 2;
			}
			else
			{
				num5 = (num - resolution.width) / 2;
				num6 = (num2 - resolution.height) / 2;
			}
			num5 += num3;
			num6 += num4;
			return new Position?(new Position(num5, num6));
		}

		// Token: 0x06008F2F RID: 36655 RVA: 0x003C9380 File Offset: 0x003C7580
		private void UpdateWindowRect(IntPtr window, int x, int y, int width, int height)
		{
			BraveOptionsMenuItem.WindowsResolutionManager.SetWindowPos(window, -2, x, y, width, height, 32);
		}

		// Token: 0x06008F30 RID: 36656 RVA: 0x003C9394 File Offset: 0x003C7594
		private bool UpdateDecorationSize(IntPtr window)
		{
			BraveOptionsMenuItem.WindowsResolutionManager.RECT rect;
			if (!BraveOptionsMenuItem.WindowsResolutionManager.GetWindowRect(this.Window, out rect))
			{
				return false;
			}
			BraveOptionsMenuItem.WindowsResolutionManager.RECT rect2;
			if (!BraveOptionsMenuItem.WindowsResolutionManager.GetClientRect(this.Window, out rect2))
			{
				return false;
			}
			int num = rect.Right - rect.Left - (rect2.Right - rect2.Left);
			int num2 = rect.Bottom - rect.Top - (rect2.Bottom - rect2.Top);
			this._borderWidth = num / 2;
			this._captionHeight = num2 - this._borderWidth * 2;
			return true;
		}

		// Token: 0x06008F31 RID: 36657 RVA: 0x003C9424 File Offset: 0x003C7624
		public bool TrySetDisplay(BraveOptionsMenuItem.WindowsResolutionManager.DisplayModes targetDisplayMode, Resolution targetResolution, bool setPosition, Position? position)
		{
			int num = (int)BraveOptionsMenuItem.WindowsResolutionManager.GetWindowLongPtr(this.Window, -16);
			if (targetDisplayMode == BraveOptionsMenuItem.WindowsResolutionManager.DisplayModes.Windowed && BraveOptionsMenuItem.WindowsResolutionManager.Flags.Contains(num, 8388608))
			{
				position = this.GetWindowPosition();
			}
			switch (targetDisplayMode)
			{
			case BraveOptionsMenuItem.WindowsResolutionManager.DisplayModes.Fullscreen:
				return true;
			case BraveOptionsMenuItem.WindowsResolutionManager.DisplayModes.Borderless:
				BraveOptionsMenuItem.WindowsResolutionManager.Flags.Unset<int>(ref num, 8388608);
				BraveOptionsMenuItem.WindowsResolutionManager.Flags.Unset<int>(ref num, 262144);
				BraveOptionsMenuItem.WindowsResolutionManager.Flags.Unset<int>(ref num, 12582912);
				BraveOptionsMenuItem.WindowsResolutionManager.SetWindowLongPtr(this.Window, -16, num);
				if (!setPosition || position == null)
				{
					position = this.GetCenteredPosition(targetResolution, targetDisplayMode);
				}
				this.UpdateWindowRect(this.Window, position.Value.X, position.Value.Y, targetResolution.width, targetResolution.height);
				BraveOptionsMenuItem.WindowsResolutionManager.SetWindowLongPtr(this.Window, -16, num);
				BraveOptionsMenuItem.WindowsResolutionManager.SetWindowLongPtr(this.Window, -16, num);
				return true;
			case BraveOptionsMenuItem.WindowsResolutionManager.DisplayModes.Windowed:
			{
				BraveOptionsMenuItem.WindowsResolutionManager.Flags.Set<int>(ref num, 8388608);
				BraveOptionsMenuItem.WindowsResolutionManager.Flags.Set<int>(ref num, 262144);
				BraveOptionsMenuItem.WindowsResolutionManager.Flags.Set<int>(ref num, 12582912);
				BraveOptionsMenuItem.WindowsResolutionManager.SetWindowLongPtr(this.Window, -16, num);
				this.UpdateDecorationSize(this.Window);
				if (position == null)
				{
					position = this.GetCenteredPosition(targetResolution, targetDisplayMode);
				}
				int num2 = targetResolution.width + this._borderWidth * 2;
				int num3 = targetResolution.height + this._captionHeight + this._borderWidth * 2;
				this.UpdateWindowRect(this.Window, position.Value.X, position.Value.Y, num2, num3);
				return true;
			}
			default:
				return false;
			}
		}

		// Token: 0x17001569 RID: 5481
		// (get) Token: 0x06008F32 RID: 36658 RVA: 0x003C95D8 File Offset: 0x003C77D8
		private IntPtr Window
		{
			get
			{
				return BraveOptionsMenuItem.WindowsResolutionManager.FindWindowByCaption(IntPtr.Zero, this._title);
			}
		}

		// Token: 0x1700156A RID: 5482
		// (get) Token: 0x06008F33 RID: 36659 RVA: 0x003C95EC File Offset: 0x003C77EC
		private IntPtr Desktop
		{
			get
			{
				return BraveOptionsMenuItem.WindowsResolutionManager.GetDesktopWindow();
			}
		}

		// Token: 0x04009750 RID: 38736
		private string _title;

		// Token: 0x04009751 RID: 38737
		private int _borderWidth;

		// Token: 0x04009752 RID: 38738
		private int _captionHeight;

		// Token: 0x04009753 RID: 38739
		private const int WS_BORDER = 8388608;

		// Token: 0x04009754 RID: 38740
		private const int WS_CAPTION = 12582912;

		// Token: 0x04009755 RID: 38741
		private const int WS_CHILD = 1073741824;

		// Token: 0x04009756 RID: 38742
		private const int WS_CHILDWINDOW = 1073741824;

		// Token: 0x04009757 RID: 38743
		private const int WS_CLIPCHILDREN = 33554432;

		// Token: 0x04009758 RID: 38744
		private const int WS_CLIPSIBLINGS = 67108864;

		// Token: 0x04009759 RID: 38745
		private const int WS_DISABLED = 134217728;

		// Token: 0x0400975A RID: 38746
		private const int WS_DLGFRAME = 4194304;

		// Token: 0x0400975B RID: 38747
		private const int WS_GROUP = 131072;

		// Token: 0x0400975C RID: 38748
		private const int WS_HSCROLL = 1048576;

		// Token: 0x0400975D RID: 38749
		private const int WS_ICONIC = 536870912;

		// Token: 0x0400975E RID: 38750
		private const int WS_MAXIMIZE = 16777216;

		// Token: 0x0400975F RID: 38751
		private const int WS_MAXIMIZEBOX = 65536;

		// Token: 0x04009760 RID: 38752
		private const int WS_MINIMIZE = 536870912;

		// Token: 0x04009761 RID: 38753
		private const int WS_MINIMIZEBOX = 131072;

		// Token: 0x04009762 RID: 38754
		private const int WS_OVERLAPPED = 0;

		// Token: 0x04009763 RID: 38755
		private const int WS_OVERLAPPEDWINDOW = 13565952;

		// Token: 0x04009764 RID: 38756
		private const int WS_POPUP = -2147483648;

		// Token: 0x04009765 RID: 38757
		private const int WS_POPUPWINDOW = -2138570752;

		// Token: 0x04009766 RID: 38758
		private const int WS_SIZEBOX = 262144;

		// Token: 0x04009767 RID: 38759
		private const int WS_SYSMENU = 524288;

		// Token: 0x04009768 RID: 38760
		private const int WS_TABSTOP = 65536;

		// Token: 0x04009769 RID: 38761
		private const int WS_THICKFRAME = 262144;

		// Token: 0x0400976A RID: 38762
		private const int WS_TILED = 0;

		// Token: 0x0400976B RID: 38763
		private const int WS_TILEDWINDOW = 13565952;

		// Token: 0x0400976C RID: 38764
		private const int WS_VISIBLE = 268435456;

		// Token: 0x0400976D RID: 38765
		private const int WS_VSCROLL = 2097152;

		// Token: 0x0400976E RID: 38766
		private const int WS_EX_DLGMODALFRAME = 1;

		// Token: 0x0400976F RID: 38767
		private const int WS_EX_CLIENTEDGE = 512;

		// Token: 0x04009770 RID: 38768
		private const int WS_EX_STATICEDGE = 131072;

		// Token: 0x04009771 RID: 38769
		private const int SWP_FRAMECHANGED = 32;

		// Token: 0x04009772 RID: 38770
		private const int SWP_NOMOVE = 2;

		// Token: 0x04009773 RID: 38771
		private const int SWP_NOSIZE = 1;

		// Token: 0x04009774 RID: 38772
		private const int SWP_NOZORDER = 4;

		// Token: 0x04009775 RID: 38773
		private const int SWP_NOOWNERZORDER = 512;

		// Token: 0x04009776 RID: 38774
		private const int SWP_SHOWWINDOW = 64;

		// Token: 0x04009777 RID: 38775
		private const int SWP_NOSENDCHANGING = 1024;

		// Token: 0x04009778 RID: 38776
		private const int GWL_STYLE = -16;

		// Token: 0x04009779 RID: 38777
		private const int GWL_EXSTYLE = -20;

		// Token: 0x020017CD RID: 6093
		public enum DisplayModes
		{
			// Token: 0x0400977B RID: 38779
			Fullscreen,
			// Token: 0x0400977C RID: 38780
			Borderless,
			// Token: 0x0400977D RID: 38781
			Windowed
		}

		// Token: 0x020017CE RID: 6094
		public struct RECT
		{
			// Token: 0x0400977E RID: 38782
			public int Left;

			// Token: 0x0400977F RID: 38783
			public int Top;

			// Token: 0x04009780 RID: 38784
			public int Right;

			// Token: 0x04009781 RID: 38785
			public int Bottom;
		}

		// Token: 0x020017CF RID: 6095
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto, Pack = 4)]
		public class MONITORINFOEX
		{
			// Token: 0x04009782 RID: 38786
			public int cbSize = Marshal.SizeOf(typeof(BraveOptionsMenuItem.WindowsResolutionManager.MONITORINFOEX));

			// Token: 0x04009783 RID: 38787
			public BraveOptionsMenuItem.WindowsResolutionManager.RECT rcMonitor = default(BraveOptionsMenuItem.WindowsResolutionManager.RECT);

			// Token: 0x04009784 RID: 38788
			public BraveOptionsMenuItem.WindowsResolutionManager.RECT rcWork = default(BraveOptionsMenuItem.WindowsResolutionManager.RECT);

			// Token: 0x04009785 RID: 38789
			public int dwFlags;

			// Token: 0x04009786 RID: 38790
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
			public char[] szDevice = new char[32];
		}

		// Token: 0x020017D0 RID: 6096
		internal static class Flags
		{
			// Token: 0x06008F35 RID: 36661 RVA: 0x003C9648 File Offset: 0x003C7848
			public static void Set<T>(ref T mask, T flag) where T : struct
			{
				int num = (int)((object)mask);
				int num2 = (int)((object)flag);
				mask = (T)((object)(num | num2));
			}

			// Token: 0x06008F36 RID: 36662 RVA: 0x003C9688 File Offset: 0x003C7888
			public static void Unset<T>(ref T mask, T flag) where T : struct
			{
				int num = (int)((object)mask);
				int num2 = (int)((object)flag);
				mask = (T)((object)(num & ~num2));
			}

			// Token: 0x06008F37 RID: 36663 RVA: 0x003C96C8 File Offset: 0x003C78C8
			public static void Toggle<T>(ref T mask, T flag) where T : struct
			{
				if (BraveOptionsMenuItem.WindowsResolutionManager.Flags.Contains<T>(mask, flag))
				{
					BraveOptionsMenuItem.WindowsResolutionManager.Flags.Unset<T>(ref mask, flag);
				}
				else
				{
					BraveOptionsMenuItem.WindowsResolutionManager.Flags.Set<T>(ref mask, flag);
				}
			}

			// Token: 0x06008F38 RID: 36664 RVA: 0x003C96F0 File Offset: 0x003C78F0
			public static bool Contains<T>(T mask, T flag) where T : struct
			{
				return BraveOptionsMenuItem.WindowsResolutionManager.Flags.Contains((int)((object)mask), (int)((object)flag));
			}

			// Token: 0x06008F39 RID: 36665 RVA: 0x003C9710 File Offset: 0x003C7910
			public static bool Contains(int mask, int flag)
			{
				return (mask & flag) != 0;
			}
		}
	}
}
