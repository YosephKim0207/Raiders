using System;
using System.Collections.Generic;
using System.Reflection;
using Brave;
using FullSerializer;
using UnityEngine;

// Token: 0x02001306 RID: 4870
public class GameOptions
{
	// Token: 0x1700107D RID: 4221
	// (get) Token: 0x06006DCE RID: 28110 RVA: 0x002B3514 File Offset: 0x002B1714
	public static bool SupportsStencil
	{
		get
		{
			bool? cachedSupportsStencil = GameOptions.m_cachedSupportsStencil;
			if (cachedSupportsStencil != null && GameOptions.m_cachedSupportsStencil != null)
			{
				return GameOptions.m_cachedSupportsStencil.Value;
			}
			bool flag = SystemInfo.supportsStencil > 0;
			if (flag)
			{
				string graphicsDeviceName = SystemInfo.graphicsDeviceName;
				if (!string.IsNullOrEmpty(graphicsDeviceName) && (graphicsDeviceName.Contains("HD Graphics 4000") || graphicsDeviceName.Contains("620M") || graphicsDeviceName.Contains("630M")))
				{
					flag = false;
				}
			}
			Debug.Log("BRV::StencilMode: " + flag);
			GameOptions.m_cachedSupportsStencil = new bool?(flag);
			return flag;
		}
	}

	// Token: 0x06006DCF RID: 28111 RVA: 0x002B35C0 File Offset: 0x002B17C0
	public static void SetStartupQualitySettings()
	{
		string graphicsDeviceName = SystemInfo.graphicsDeviceName;
		string graphicsDeviceVendor = SystemInfo.graphicsDeviceVendor;
		bool flag = false;
		flag |= graphicsDeviceVendor.Contains("NVIDIA");
		flag |= graphicsDeviceVendor.Contains("AMD");
		flag |= graphicsDeviceName.Contains("NVIDIA");
		flag |= graphicsDeviceName.Contains("AMD");
		Debug.Log("> = > = > BRAVE QUALITY: " + flag);
	}

	// Token: 0x06006DD0 RID: 28112 RVA: 0x002B3628 File Offset: 0x002B1828
	public static GameOptions CloneOptions(GameOptions source)
	{
		GameOptions gameOptions = new GameOptions();
		foreach (FieldInfo fieldInfo in typeof(GameOptions).GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy))
		{
			bool flag = false;
			if (fieldInfo.GetCustomAttributes(typeof(fsPropertyAttribute), false).Length > 0)
			{
				flag = true;
			}
			if (flag)
			{
				fieldInfo.SetValue(gameOptions, fieldInfo.GetValue(source));
			}
		}
		gameOptions.UpdateCmdArgs();
		return gameOptions;
	}

	// Token: 0x06006DD1 RID: 28113 RVA: 0x002B36A0 File Offset: 0x002B18A0
	public GameOptions.GenericHighMedLowOption GetDefaultRecommendedGraphicalQuality()
	{
		if (this.m_DefaultRecommendedQuality != null)
		{
			return this.m_DefaultRecommendedQuality.Value;
		}
		if (SystemInfo.graphicsMemorySize <= 512 || SystemInfo.systemMemorySize <= 1536)
		{
			return GameOptions.GenericHighMedLowOption.LOW;
		}
		string graphicsDeviceName = SystemInfo.graphicsDeviceName;
		if (!string.IsNullOrEmpty(graphicsDeviceName) && graphicsDeviceName.ToLowerInvariant().Contains("intel"))
		{
			this.m_DefaultRecommendedQuality = new GameOptions.GenericHighMedLowOption?(GameOptions.GenericHighMedLowOption.MEDIUM);
			return this.m_DefaultRecommendedQuality.Value;
		}
		string graphicsDeviceVendor = SystemInfo.graphicsDeviceVendor;
		if (!string.IsNullOrEmpty(graphicsDeviceVendor) && graphicsDeviceVendor.ToLowerInvariant().Contains("intel"))
		{
			this.m_DefaultRecommendedQuality = new GameOptions.GenericHighMedLowOption?(GameOptions.GenericHighMedLowOption.MEDIUM);
			return this.m_DefaultRecommendedQuality.Value;
		}
		this.m_DefaultRecommendedQuality = new GameOptions.GenericHighMedLowOption?(GameOptions.GenericHighMedLowOption.HIGH);
		return this.m_DefaultRecommendedQuality.Value;
	}

	// Token: 0x06006DD2 RID: 28114 RVA: 0x002B377C File Offset: 0x002B197C
	public void RevertToDefaults()
	{
		GameOptions gameOptions = new GameOptions();
		foreach (FieldInfo fieldInfo in typeof(GameOptions).GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy))
		{
			bool flag = false;
			if (fieldInfo.GetCustomAttributes(typeof(fsPropertyAttribute), false).Length > 0)
			{
				flag = true;
			}
			if (flag)
			{
				fieldInfo.SetValue(this, fieldInfo.GetValue(gameOptions));
			}
		}
		GameOptions.GenericHighMedLowOption defaultRecommendedGraphicalQuality = this.GetDefaultRecommendedGraphicalQuality();
		this.DoVsync = true;
		this.LightingQuality = ((defaultRecommendedGraphicalQuality != GameOptions.GenericHighMedLowOption.LOW) ? GameOptions.GenericHighMedLowOption.HIGH : GameOptions.GenericHighMedLowOption.LOW);
		this.ShaderQuality = defaultRecommendedGraphicalQuality;
		this.DebrisQuantity = defaultRecommendedGraphicalQuality;
		this.RealtimeReflections = defaultRecommendedGraphicalQuality != GameOptions.GenericHighMedLowOption.LOW;
		this.CurrentLanguage = GameManager.Instance.platformInterface.GetPreferredLanguage();
		StringTableManager.SetNewLanguage(GameManager.Options.CurrentLanguage, true);
		GameManager.Options.MusicVolume = GameManager.Options.MusicVolume;
		GameManager.Options.SoundVolume = GameManager.Options.SoundVolume;
		GameManager.Options.UIVolume = GameManager.Options.UIVolume;
		GameManager.Options.AudioHardware = GameManager.Options.AudioHardware;
		this.UpdateCmdArgs();
	}

	// Token: 0x06006DD3 RID: 28115 RVA: 0x002B38AC File Offset: 0x002B1AAC
	private void UpdateCmdArgs()
	{
		string commandLine = Environment.CommandLine;
		if (commandLine.Contains("-xinputOnly", true))
		{
			this.allowNonXinputControllers = false;
		}
		if (commandLine.Contains("-noXinput", true))
		{
			this.allowXinputControllers = false;
		}
		if (commandLine.Contains("-allowUnknownControllers", true))
		{
			this.allowUnknownControllers = true;
		}
	}

	// Token: 0x06006DD4 RID: 28116 RVA: 0x002B3908 File Offset: 0x002B1B08
	public static bool CompareSettings(GameOptions clone, GameOptions source)
	{
		if (clone == null || source == null)
		{
			Debug.LogError(string.Concat(new object[] { clone, "|", source, " OPTIONS ARE NULL!" }));
			return false;
		}
		bool flag = true;
		foreach (FieldInfo fieldInfo in typeof(GameOptions).GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy))
		{
			if (fieldInfo != null)
			{
				bool flag2 = false;
				if (fieldInfo.GetCustomAttributes(typeof(fsPropertyAttribute), false).Length > 0)
				{
					flag2 = true;
				}
				if (flag2)
				{
					object value = fieldInfo.GetValue(clone);
					object value2 = fieldInfo.GetValue(source);
					if (value != null && value2 != null)
					{
						bool flag3 = value.Equals(value2);
						flag = flag && flag3;
					}
				}
			}
		}
		return flag;
	}

	// Token: 0x06006DD5 RID: 28117 RVA: 0x002B39E0 File Offset: 0x002B1BE0
	public void ApplySettings(GameOptions clone)
	{
		foreach (FieldInfo fieldInfo in typeof(GameOptions).GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy))
		{
			bool flag = fieldInfo.GetCustomAttributes(typeof(fsPropertyAttribute), false).Length > 0;
			if (flag && fieldInfo.GetValue(this) != fieldInfo.GetValue(clone))
			{
				fieldInfo.SetValue(this, fieldInfo.GetValue(clone));
			}
		}
		this.playerOneBindingDataV2 = clone.playerOneBindingDataV2;
		this.playerTwoBindingDataV2 = clone.playerTwoBindingDataV2;
		if (this == GameManager.Options)
		{
			BraveInput.ForceLoadBindingInfoFromOptions();
		}
	}

	// Token: 0x1700107E RID: 4222
	// (get) Token: 0x06006DD6 RID: 28118 RVA: 0x002B3A80 File Offset: 0x002B1C80
	// (set) Token: 0x06006DD7 RID: 28119 RVA: 0x002B3A88 File Offset: 0x002B1C88
	[fsIgnore]
	public float MusicVolume
	{
		get
		{
			return this.m_musicVolume;
		}
		set
		{
			this.m_musicVolume = value;
			AkSoundEngine.SetRTPCValue("VOL_MUS", this.m_musicVolume);
		}
	}

	// Token: 0x1700107F RID: 4223
	// (get) Token: 0x06006DD8 RID: 28120 RVA: 0x002B3AA4 File Offset: 0x002B1CA4
	// (set) Token: 0x06006DD9 RID: 28121 RVA: 0x002B3AAC File Offset: 0x002B1CAC
	[fsIgnore]
	public float SoundVolume
	{
		get
		{
			return this.m_soundVolume;
		}
		set
		{
			this.m_soundVolume = value;
			AkSoundEngine.SetRTPCValue("VOL_SFX", this.m_soundVolume);
		}
	}

	// Token: 0x17001080 RID: 4224
	// (get) Token: 0x06006DDA RID: 28122 RVA: 0x002B3AC8 File Offset: 0x002B1CC8
	// (set) Token: 0x06006DDB RID: 28123 RVA: 0x002B3AD0 File Offset: 0x002B1CD0
	[fsIgnore]
	public float UIVolume
	{
		get
		{
			return this.m_uiVolume;
		}
		set
		{
			this.m_uiVolume = value;
			AkSoundEngine.SetRTPCValue("VOL_UI", this.m_uiVolume);
		}
	}

	// Token: 0x17001081 RID: 4225
	// (get) Token: 0x06006DDC RID: 28124 RVA: 0x002B3AEC File Offset: 0x002B1CEC
	// (set) Token: 0x06006DDD RID: 28125 RVA: 0x002B3AF4 File Offset: 0x002B1CF4
	[fsIgnore]
	public float Gamma
	{
		get
		{
			return this.m_gamma;
		}
		set
		{
			this.m_gamma = value;
		}
	}

	// Token: 0x17001082 RID: 4226
	// (get) Token: 0x06006DDE RID: 28126 RVA: 0x002B3B00 File Offset: 0x002B1D00
	[fsIgnore]
	public GameOptions.PixelatorMotionEnhancementMode MotionEnhancementMode
	{
		get
		{
			if (this.OverrideMotionEnhancementModeForPause)
			{
				return GameOptions.PixelatorMotionEnhancementMode.UNENHANCED_CHEAP;
			}
			if (this.ShaderQuality == GameOptions.GenericHighMedLowOption.HIGH)
			{
				return GameOptions.PixelatorMotionEnhancementMode.ENHANCED_EXPENSIVE;
			}
			return GameOptions.PixelatorMotionEnhancementMode.UNENHANCED_CHEAP;
		}
	}

	// Token: 0x17001083 RID: 4227
	// (get) Token: 0x06006DDF RID: 28127 RVA: 0x002B3B20 File Offset: 0x002B1D20
	// (set) Token: 0x06006DE0 RID: 28128 RVA: 0x002B3B28 File Offset: 0x002B1D28
	[fsIgnore]
	public GameOptions.AudioHardwareMode AudioHardware
	{
		get
		{
			return this.m_audioHardware;
		}
		set
		{
			this.m_audioHardware = value;
			GameOptions.AudioHardwareMode audioHardware = this.m_audioHardware;
			if (audioHardware != GameOptions.AudioHardwareMode.SPEAKERS)
			{
				if (audioHardware == GameOptions.AudioHardwareMode.HEADPHONES)
				{
					AkSoundEngine.SetPanningRule(AkPanningRule.AkPanningRule_Headphones);
				}
			}
			else
			{
				AkSoundEngine.SetPanningRule(AkPanningRule.AkPanningRule_Speakers);
			}
		}
	}

	// Token: 0x17001084 RID: 4228
	// (get) Token: 0x06006DE1 RID: 28129 RVA: 0x002B3B74 File Offset: 0x002B1D74
	// (set) Token: 0x06006DE2 RID: 28130 RVA: 0x002B3B7C File Offset: 0x002B1D7C
	[fsIgnore]
	public Minimap.MinimapDisplayMode MinimapDisplayMode
	{
		get
		{
			return this.m_minimapDisplayMode;
		}
		set
		{
			this.m_minimapDisplayMode = value;
		}
	}

	// Token: 0x06006DE3 RID: 28131 RVA: 0x002B3B88 File Offset: 0x002B1D88
	public static void Load()
	{
		SaveManager.Init();
		GameOptions gameOptions = null;
		bool flag = SaveManager.Load<GameOptions>(SaveManager.OptionsSave, out gameOptions, true, 0U, null, null);
		if (!flag)
		{
			int num = 0;
			while (num < 3 && !flag)
			{
				if (num != (int)SaveManager.CurrentSaveSlot)
				{
					gameOptions = null;
					SaveManager.SaveType optionsSave = SaveManager.OptionsSave;
					ref GameOptions ptr = ref gameOptions;
					bool flag2 = true;
					SaveManager.SaveSlot? saveSlot = new SaveManager.SaveSlot?((SaveManager.SaveSlot)num);
					flag = SaveManager.Load<GameOptions>(optionsSave, out ptr, flag2, 0U, null, saveSlot);
					flag &= gameOptions != null;
				}
				num++;
			}
		}
		if (!flag || gameOptions == null)
		{
			GameManager.Options = new GameOptions();
			GameOptions.RequiresLanguageReinitialization = true;
		}
		else
		{
			GameManager.Options = gameOptions;
			GameManager.Options.MusicVolume = GameManager.Options.MusicVolume;
			GameManager.Options.SoundVolume = GameManager.Options.SoundVolume;
			GameManager.Options.UIVolume = GameManager.Options.UIVolume;
			GameManager.Options.AudioHardware = GameManager.Options.AudioHardware;
		}
		GameManager.Options.UpdateCmdArgs();
		GameManager.Options.controllerAimAssistMultiplier = Mathf.Clamp(GameManager.Options.controllerAimAssistMultiplier, 0f, 1.25f);
		GameManager.Options.DisplaySafeArea = Mathf.Clamp(GameManager.Options.DisplaySafeArea, 0.9f, 1f);
		if (GameManager.Options.ShaderQuality == GameOptions.GenericHighMedLowOption.HIGH || GameManager.Options.ShaderQuality == GameOptions.GenericHighMedLowOption.MEDIUM)
		{
			Shader.SetGlobalFloat("_LowQualityMode", 0f);
		}
		else
		{
			Shader.SetGlobalFloat("_LowQualityMode", 1f);
		}
		if (Brave.PlayerPrefs.HasKey("UnitySelectMonitor"))
		{
			GameManager.Options.CurrentMonitorIndex = Brave.PlayerPrefs.GetInt("UnitySelectMonitor");
		}
	}

	// Token: 0x06006DE4 RID: 28132 RVA: 0x002B3D40 File Offset: 0x002B1F40
	public static bool Save()
	{
		return SaveManager.Save<GameOptions>(GameManager.Options, SaveManager.OptionsSave, 0, 0U, null);
	}

	// Token: 0x17001085 RID: 4229
	// (get) Token: 0x06006DE5 RID: 28133 RVA: 0x002B3D68 File Offset: 0x002B1F68
	// (set) Token: 0x06006DE6 RID: 28134 RVA: 0x002B3D70 File Offset: 0x002B1F70
	[fsIgnore]
	public GameOptions.FullscreenStyle CurrentFullscreenStyle
	{
		get
		{
			return this.m_fullscreenStyle;
		}
		set
		{
			this.m_fullscreenStyle = value;
			if (this.m_fullscreenStyle != GameOptions.FullscreenStyle.BORDERLESS)
			{
				GameCursorController component = GameUIRoot.Instance.GetComponent<GameCursorController>();
				if (component != null)
				{
					component.ToggleClip(false);
				}
			}
		}
	}

	// Token: 0x17001086 RID: 4230
	// (get) Token: 0x06006DE7 RID: 28135 RVA: 0x002B3DB0 File Offset: 0x002B1FB0
	// (set) Token: 0x06006DE8 RID: 28136 RVA: 0x002B3DB8 File Offset: 0x002B1FB8
	public int CurrentCursorIndex
	{
		get
		{
			return this.m_currentCursorIndex;
		}
		set
		{
			this.m_currentCursorIndex = value;
		}
	}

	// Token: 0x17001087 RID: 4231
	// (get) Token: 0x06006DE9 RID: 28137 RVA: 0x002B3DC4 File Offset: 0x002B1FC4
	// (set) Token: 0x06006DEA RID: 28138 RVA: 0x002B3DCC File Offset: 0x002B1FCC
	[fsIgnore]
	public GameOptions.VisualPresetMode CurrentVisualPreset
	{
		get
		{
			return this.m_visualPresetMode;
		}
		set
		{
			if (this.m_visualPresetMode == value)
			{
				return;
			}
			this.m_visualPresetMode = value;
			if (this.m_visualPresetMode == GameOptions.VisualPresetMode.RECOMMENDED)
			{
				Resolution recommendedResolution = this.GetRecommendedResolution();
				this.CurrentPreferredScalingMode = this.GetRecommendedScalingMode();
				this.CurrentPreferredFullscreenMode = GameOptions.PreferredFullscreenMode.FULLSCREEN;
				Debug.Log(string.Concat(new object[] { "Setting screen resolution RECOMMENDED: ", recommendedResolution.width, "|", recommendedResolution.height }));
				GameManager.Instance.DoSetResolution(recommendedResolution.width, recommendedResolution.height, true);
			}
		}
	}

	// Token: 0x17001088 RID: 4232
	// (get) Token: 0x06006DEB RID: 28139 RVA: 0x002B3E70 File Offset: 0x002B2070
	// (set) Token: 0x06006DEC RID: 28140 RVA: 0x002B3E78 File Offset: 0x002B2078
	[fsIgnore]
	public StringTableManager.GungeonSupportedLanguages CurrentLanguage
	{
		get
		{
			return this.m_currentLanguage;
		}
		set
		{
			if (this.m_currentLanguage != value)
			{
				this.m_currentLanguage = value;
				StringTableManager.CurrentLanguage = value;
				BraveInput.OnLanguageChanged();
			}
		}
	}

	// Token: 0x06006DED RID: 28141 RVA: 0x002B3E98 File Offset: 0x002B2098
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

	// Token: 0x17001089 RID: 4233
	// (get) Token: 0x06006DEE RID: 28142 RVA: 0x002B3EE8 File Offset: 0x002B20E8
	// (set) Token: 0x06006DEF RID: 28143 RVA: 0x002B3EF0 File Offset: 0x002B20F0
	public GameOptions.ControlPreset CurrentControlPreset
	{
		get
		{
			return this.m_currentControlPreset;
		}
		set
		{
			this.m_currentControlPreset = value;
			if (this.m_currentControlPreset == GameOptions.ControlPreset.RECOMMENDED)
			{
				this.GetBestInputInstance(0).ActiveActions.ReinitializeDefaults();
			}
			else if (this.m_currentControlPreset == GameOptions.ControlPreset.FLIPPED_TRIGGERS)
			{
				this.GetBestInputInstance(0).ActiveActions.InitializeSwappedTriggersPreset();
			}
		}
	}

	// Token: 0x1700108A RID: 4234
	// (get) Token: 0x06006DF0 RID: 28144 RVA: 0x002B3F44 File Offset: 0x002B2144
	// (set) Token: 0x06006DF1 RID: 28145 RVA: 0x002B3F4C File Offset: 0x002B214C
	public GameOptions.ControlPreset CurrentControlPresetP2
	{
		get
		{
			return this.m_currentControlPresetP2;
		}
		set
		{
			this.m_currentControlPresetP2 = value;
			if (this.m_currentControlPresetP2 == GameOptions.ControlPreset.RECOMMENDED)
			{
				this.GetBestInputInstance(1).ActiveActions.ReinitializeDefaults();
			}
			else if (this.m_currentControlPresetP2 == GameOptions.ControlPreset.FLIPPED_TRIGGERS)
			{
				this.GetBestInputInstance(1).ActiveActions.InitializeSwappedTriggersPreset();
			}
		}
	}

	// Token: 0x06006DF2 RID: 28146 RVA: 0x002B3FA0 File Offset: 0x002B21A0
	public GameOptions.PreferredScalingMode GetRecommendedScalingMode()
	{
		if (Screen.width % Pixelator.Instance.CurrentMacroResolutionX == 0 && Screen.height % Pixelator.Instance.CurrentMacroResolutionY == 0)
		{
			return GameOptions.PreferredScalingMode.PIXEL_PERFECT;
		}
		return GameOptions.PreferredScalingMode.FORCE_PIXEL_PERFECT;
	}

	// Token: 0x06006DF3 RID: 28147 RVA: 0x002B3FD0 File Offset: 0x002B21D0
	public Resolution GetRecommendedResolution()
	{
		Resolution[] resolutions = Screen.resolutions;
		Resolution resolution = resolutions[0];
		float num = 1.7777778f;
		bool flag = resolution.width % Pixelator.Instance.CurrentMacroResolutionX == 0 && resolution.height % Pixelator.Instance.CurrentMacroResolutionY == 0;
		foreach (Resolution resolution2 in resolutions)
		{
			if (resolution2.height >= resolution.height)
			{
				float num2 = (float)resolution.width / ((float)resolution.height * 1f);
				float num3 = (float)resolution2.width / ((float)resolution2.height * 1f);
				if (num2 != num || num3 == num)
				{
					if (num2 == num && num3 == num)
					{
						bool flag2 = resolution2.width % Pixelator.Instance.CurrentMacroResolutionX == 0 && resolution2.height % Pixelator.Instance.CurrentMacroResolutionY == 0;
						if (flag)
						{
							if (flag2 && (resolution2.height > resolution.height || resolution2.refreshRate > resolution.refreshRate))
							{
								resolution = resolution2;
								flag = true;
							}
						}
						else
						{
							resolution = resolution2;
							flag = flag2;
						}
					}
					else
					{
						bool flag3 = resolution2.width % Pixelator.Instance.CurrentMacroResolutionX == 0 && resolution2.height % Pixelator.Instance.CurrentMacroResolutionY == 0;
						resolution = resolution2;
						flag = flag3;
					}
				}
			}
		}
		return resolution;
	}

	// Token: 0x1700108B RID: 4235
	// (get) Token: 0x06006DF4 RID: 28148 RVA: 0x002B4174 File Offset: 0x002B2374
	// (set) Token: 0x06006DF5 RID: 28149 RVA: 0x002B417C File Offset: 0x002B237C
	[fsIgnore]
	public GameOptions.PreferredScalingMode CurrentPreferredScalingMode
	{
		get
		{
			return this.m_preferredScalingMode;
		}
		set
		{
			this.m_preferredScalingMode = value;
		}
	}

	// Token: 0x1700108C RID: 4236
	// (get) Token: 0x06006DF6 RID: 28150 RVA: 0x002B4188 File Offset: 0x002B2388
	// (set) Token: 0x06006DF7 RID: 28151 RVA: 0x002B4190 File Offset: 0x002B2390
	[fsIgnore]
	public GameOptions.PreferredFullscreenMode CurrentPreferredFullscreenMode
	{
		get
		{
			return this.m_preferredFullscreenMode;
		}
		set
		{
			this.m_preferredFullscreenMode = value;
		}
	}

	// Token: 0x1700108D RID: 4237
	// (get) Token: 0x06006DF8 RID: 28152 RVA: 0x002B419C File Offset: 0x002B239C
	// (set) Token: 0x06006DF9 RID: 28153 RVA: 0x002B41A4 File Offset: 0x002B23A4
	[fsIgnore]
	public bool DoVsync
	{
		get
		{
			return this.m_doVsync;
		}
		set
		{
			this.m_doVsync = value;
			QualitySettings.vSyncCount = ((!this.m_doVsync) ? 0 : 1);
		}
	}

	// Token: 0x1700108E RID: 4238
	// (get) Token: 0x06006DFA RID: 28154 RVA: 0x002B41C4 File Offset: 0x002B23C4
	// (set) Token: 0x06006DFB RID: 28155 RVA: 0x002B41CC File Offset: 0x002B23CC
	[fsIgnore]
	public GameOptions.GenericHighMedLowOption LightingQuality
	{
		get
		{
			return this.m_lightingQuality;
		}
		set
		{
			if (this.m_lightingQuality == value)
			{
				return;
			}
			this.m_lightingQuality = value;
			if (this.m_lightingQuality == GameOptions.GenericHighMedLowOption.VERY_LOW)
			{
				this.m_lightingQuality = GameOptions.GenericHighMedLowOption.LOW;
			}
			if (this.m_lightingQuality == GameOptions.GenericHighMedLowOption.MEDIUM)
			{
				this.m_lightingQuality = GameOptions.GenericHighMedLowOption.HIGH;
			}
			ShadowSystem.ForceAllLightsUpdate();
			if (Pixelator.Instance != null)
			{
				Pixelator.Instance.OnChangedLightingQuality(this.m_lightingQuality);
			}
		}
	}

	// Token: 0x1700108F RID: 4239
	// (get) Token: 0x06006DFC RID: 28156 RVA: 0x002B4238 File Offset: 0x002B2438
	// (set) Token: 0x06006DFD RID: 28157 RVA: 0x002B4240 File Offset: 0x002B2440
	[fsIgnore]
	public GameOptions.GenericHighMedLowOption ShaderQuality
	{
		get
		{
			return this.m_shaderQuality;
		}
		set
		{
			this.m_shaderQuality = value;
			if (this.m_shaderQuality == GameOptions.GenericHighMedLowOption.HIGH || this.m_shaderQuality == GameOptions.GenericHighMedLowOption.MEDIUM)
			{
				Shader.SetGlobalFloat("_LowQualityMode", 0f);
			}
			else
			{
				Shader.SetGlobalFloat("_LowQualityMode", 1f);
			}
			if (GameManager.HasInstance && GameManager.Instance.Dungeon)
			{
				RenderSettings.ambientIntensity = GameManager.Instance.Dungeon.TargetAmbientIntensity;
			}
		}
	}

	// Token: 0x17001090 RID: 4240
	// (get) Token: 0x06006DFE RID: 28158 RVA: 0x002B42C4 File Offset: 0x002B24C4
	// (set) Token: 0x06006DFF RID: 28159 RVA: 0x002B42CC File Offset: 0x002B24CC
	[fsIgnore]
	public bool RealtimeReflections
	{
		get
		{
			return this.m_realtimeReflections;
		}
		set
		{
			Shader.SetGlobalFloat("_GlobalReflectionsEnabled", (float)((!value) ? 0 : 1));
			this.m_realtimeReflections = value;
		}
	}

	// Token: 0x17001091 RID: 4241
	// (get) Token: 0x06006E00 RID: 28160 RVA: 0x002B42F0 File Offset: 0x002B24F0
	// (set) Token: 0x06006E01 RID: 28161 RVA: 0x002B42F8 File Offset: 0x002B24F8
	[fsIgnore]
	public GameOptions.GenericHighMedLowOption DebrisQuantity
	{
		get
		{
			return this.m_debrisQuantity;
		}
		set
		{
			this.m_debrisQuantity = value;
			if (SpawnManager.Instance != null)
			{
				SpawnManager.Instance.OnDebrisQuantityChanged();
			}
		}
	}

	// Token: 0x17001092 RID: 4242
	// (get) Token: 0x06006E02 RID: 28162 RVA: 0x002B431C File Offset: 0x002B251C
	// (set) Token: 0x06006E03 RID: 28163 RVA: 0x002B4324 File Offset: 0x002B2524
	[fsIgnore]
	public GameOptions.GenericHighMedLowOption TextSpeed
	{
		get
		{
			return this.m_textSpeed;
		}
		set
		{
			this.m_textSpeed = value;
		}
	}

	// Token: 0x17001093 RID: 4243
	// (get) Token: 0x06006E04 RID: 28164 RVA: 0x002B4330 File Offset: 0x002B2530
	// (set) Token: 0x06006E05 RID: 28165 RVA: 0x002B4338 File Offset: 0x002B2538
	[fsIgnore]
	public float ScreenShakeMultiplier
	{
		get
		{
			return this.m_screenShakeMultiplier;
		}
		set
		{
			this.m_screenShakeMultiplier = value;
		}
	}

	// Token: 0x17001094 RID: 4244
	// (get) Token: 0x06006E06 RID: 28166 RVA: 0x002B4344 File Offset: 0x002B2544
	// (set) Token: 0x06006E07 RID: 28167 RVA: 0x002B434C File Offset: 0x002B254C
	[fsIgnore]
	public bool CoopScreenShakeReduction
	{
		get
		{
			return this.m_coopScreenShakeReduction;
		}
		set
		{
			this.m_coopScreenShakeReduction = value;
		}
	}

	// Token: 0x17001095 RID: 4245
	// (get) Token: 0x06006E08 RID: 28168 RVA: 0x002B4358 File Offset: 0x002B2558
	// (set) Token: 0x06006E09 RID: 28169 RVA: 0x002B4360 File Offset: 0x002B2560
	[fsIgnore]
	public float StickyFrictionMultiplier
	{
		get
		{
			return this.m_stickyFrictionMultiplier;
		}
		set
		{
			this.m_stickyFrictionMultiplier = value;
		}
	}

	// Token: 0x17001096 RID: 4246
	// (get) Token: 0x06006E0A RID: 28170 RVA: 0x002B436C File Offset: 0x002B256C
	// (set) Token: 0x06006E0B RID: 28171 RVA: 0x002B4374 File Offset: 0x002B2574
	[fsIgnore]
	public GameOptions.ControllerSymbology PlayerOnePreferredSymbology
	{
		get
		{
			return this.m_playerOnePreferredSymbology;
		}
		set
		{
			this.m_playerOnePreferredSymbology = value;
		}
	}

	// Token: 0x17001097 RID: 4247
	// (get) Token: 0x06006E0C RID: 28172 RVA: 0x002B4380 File Offset: 0x002B2580
	// (set) Token: 0x06006E0D RID: 28173 RVA: 0x002B4388 File Offset: 0x002B2588
	[fsIgnore]
	public GameOptions.ControllerSymbology PlayerTwoPreferredSymbology
	{
		get
		{
			return this.m_playerTwoPreferredSymbology;
		}
		set
		{
			this.m_playerTwoPreferredSymbology = value;
		}
	}

	// Token: 0x17001098 RID: 4248
	// (get) Token: 0x06006E0E RID: 28174 RVA: 0x002B4394 File Offset: 0x002B2594
	// (set) Token: 0x06006E0F RID: 28175 RVA: 0x002B439C File Offset: 0x002B259C
	[fsIgnore]
	public bool PlayerOneControllerCursor
	{
		get
		{
			return this.m_playerOneControllerCursor;
		}
		set
		{
			this.m_playerOneControllerCursor = value;
		}
	}

	// Token: 0x17001099 RID: 4249
	// (get) Token: 0x06006E10 RID: 28176 RVA: 0x002B43A8 File Offset: 0x002B25A8
	// (set) Token: 0x06006E11 RID: 28177 RVA: 0x002B43B0 File Offset: 0x002B25B0
	[fsIgnore]
	public bool PlayerTwoControllerCursor
	{
		get
		{
			return this.m_playerTwoControllerCursor;
		}
		set
		{
			this.m_playerTwoControllerCursor = value;
		}
	}

	// Token: 0x04006AED RID: 27373
	private static bool? m_cachedSupportsStencil;

	// Token: 0x04006AEE RID: 27374
	[fsIgnore]
	private GameOptions.GenericHighMedLowOption? m_DefaultRecommendedQuality;

	// Token: 0x04006AEF RID: 27375
	[fsProperty]
	public bool SLOW_TIME_ON_CHALLENGE_MODE_REVEAL = true;

	// Token: 0x04006AF0 RID: 27376
	[fsProperty]
	private float m_gamma = 1f;

	// Token: 0x04006AF1 RID: 27377
	[fsProperty]
	public float DisplaySafeArea = 1f;

	// Token: 0x04006AF2 RID: 27378
	[fsProperty]
	public GameOptions.ControllerBlankControl additionalBlankControl = GameOptions.ControllerBlankControl.BOTH_STICKS_DOWN;

	// Token: 0x04006AF3 RID: 27379
	[fsProperty]
	public GameOptions.ControllerBlankControl additionalBlankControlTwo = GameOptions.ControllerBlankControl.BOTH_STICKS_DOWN;

	// Token: 0x04006AF4 RID: 27380
	[fsIgnore]
	public bool OverrideMotionEnhancementModeForPause;

	// Token: 0x04006AF5 RID: 27381
	[fsIgnore]
	public Dictionary<int, int> PlayerIDtoDeviceIndexMap = new Dictionary<int, int>();

	// Token: 0x04006AF6 RID: 27382
	[fsIgnore]
	public static bool RequiresLanguageReinitialization;

	// Token: 0x04006AF7 RID: 27383
	[fsProperty]
	public GameOptions.QuickstartCharacter PreferredQuickstartCharacter;

	// Token: 0x04006AF8 RID: 27384
	[fsProperty]
	public PlayableCharacters LastPlayedCharacter;

	// Token: 0x04006AF9 RID: 27385
	[fsProperty]
	public GameOptions.GameLootProfile CurrentGameLootProfile;

	// Token: 0x04006AFA RID: 27386
	[fsProperty]
	public bool IncreaseSpeedOutOfCombat;

	// Token: 0x04006AFB RID: 27387
	[fsProperty]
	private GameOptions.FullscreenStyle m_fullscreenStyle;

	// Token: 0x04006AFC RID: 27388
	[fsIgnore]
	public int CurrentMonitorIndex;

	// Token: 0x04006AFD RID: 27389
	[fsProperty]
	private int m_currentCursorIndex;

	// Token: 0x04006AFE RID: 27390
	[fsProperty]
	private GameOptions.VisualPresetMode m_visualPresetMode;

	// Token: 0x04006AFF RID: 27391
	[fsProperty]
	private GameOptions.ControlPreset m_currentControlPreset;

	// Token: 0x04006B00 RID: 27392
	[fsProperty]
	private GameOptions.ControlPreset m_currentControlPresetP2;

	// Token: 0x04006B01 RID: 27393
	[fsProperty]
	private StringTableManager.GungeonSupportedLanguages m_currentLanguage;

	// Token: 0x04006B02 RID: 27394
	[fsProperty]
	private bool m_doVsync = true;

	// Token: 0x04006B03 RID: 27395
	[fsProperty]
	private GameOptions.GenericHighMedLowOption m_lightingQuality = GameOptions.GenericHighMedLowOption.HIGH;

	// Token: 0x04006B04 RID: 27396
	[fsProperty]
	public bool QuickSelectEnabled = true;

	// Token: 0x04006B05 RID: 27397
	[fsProperty]
	public bool HideEmptyGuns = true;

	// Token: 0x04006B06 RID: 27398
	[fsProperty]
	private GameOptions.GenericHighMedLowOption m_shaderQuality = GameOptions.GenericHighMedLowOption.HIGH;

	// Token: 0x04006B07 RID: 27399
	[fsProperty]
	private bool m_realtimeReflections = true;

	// Token: 0x04006B08 RID: 27400
	[fsProperty]
	private GameOptions.GenericHighMedLowOption m_debrisQuantity = GameOptions.GenericHighMedLowOption.HIGH;

	// Token: 0x04006B09 RID: 27401
	[fsProperty]
	private GameOptions.GenericHighMedLowOption m_textSpeed = GameOptions.GenericHighMedLowOption.MEDIUM;

	// Token: 0x04006B0A RID: 27402
	[fsProperty]
	private float m_screenShakeMultiplier = 1f;

	// Token: 0x04006B0B RID: 27403
	[fsProperty]
	private bool m_coopScreenShakeReduction = true;

	// Token: 0x04006B0C RID: 27404
	[fsProperty]
	private float m_stickyFrictionMultiplier = 1f;

	// Token: 0x04006B0D RID: 27405
	[fsProperty]
	public bool HasEverSeenAmmonomicon;

	// Token: 0x04006B0E RID: 27406
	[fsProperty]
	public bool SpeedrunMode;

	// Token: 0x04006B0F RID: 27407
	[fsProperty]
	public bool RumbleEnabled = true;

	// Token: 0x04006B10 RID: 27408
	[fsProperty]
	public bool SmallUIEnabled;

	// Token: 0x04006B11 RID: 27409
	[fsProperty]
	public bool m_beastmode;

	// Token: 0x04006B12 RID: 27410
	[fsProperty]
	public bool mouseAimLook = true;

	// Token: 0x04006B13 RID: 27411
	[fsProperty]
	public bool SuperSmoothCamera;

	// Token: 0x04006B14 RID: 27412
	[fsProperty]
	public bool DisplaySpeedrunCentiseconds;

	// Token: 0x04006B15 RID: 27413
	[fsProperty]
	public bool DisableQuickGunKeys;

	// Token: 0x04006B16 RID: 27414
	[fsProperty]
	public bool AllowMoveKeysToChangeGuns;

	// Token: 0x04006B17 RID: 27415
	[fsProperty]
	public bool autofaceMovementDirection = true;

	// Token: 0x04006B18 RID: 27416
	[fsProperty]
	public float controllerAimLookMultiplier = 1f;

	// Token: 0x04006B19 RID: 27417
	[fsProperty]
	public GameOptions.ControllerAutoAim controllerAutoAim;

	// Token: 0x04006B1A RID: 27418
	[fsProperty]
	public float controllerAimAssistMultiplier = 1f;

	// Token: 0x04006B1B RID: 27419
	[fsProperty]
	public bool controllerBeamAimAssist;

	// Token: 0x04006B1C RID: 27420
	[fsProperty]
	public bool allowXinputControllers = true;

	// Token: 0x04006B1D RID: 27421
	[fsProperty]
	public bool allowNonXinputControllers = true;

	// Token: 0x04006B1E RID: 27422
	[fsProperty]
	public bool allowUnknownControllers;

	// Token: 0x04006B1F RID: 27423
	[fsProperty(DeserializeOnly = true)]
	public bool wipeAllAchievements;

	// Token: 0x04006B20 RID: 27424
	[fsProperty(DeserializeOnly = true)]
	public bool scanAchievementsForUnlocks;

	// Token: 0x04006B21 RID: 27425
	[fsProperty]
	public int preferredResolutionX = -1;

	// Token: 0x04006B22 RID: 27426
	[fsProperty]
	public int preferredResolutionY = -1;

	// Token: 0x04006B23 RID: 27427
	[fsProperty]
	private GameOptions.ControllerSymbology m_playerOnePreferredSymbology = GameOptions.ControllerSymbology.AutoDetect;

	// Token: 0x04006B24 RID: 27428
	[fsProperty]
	private GameOptions.ControllerSymbology m_playerTwoPreferredSymbology = GameOptions.ControllerSymbology.AutoDetect;

	// Token: 0x04006B25 RID: 27429
	[fsProperty]
	private bool m_playerOneControllerCursor;

	// Token: 0x04006B26 RID: 27430
	[fsProperty]
	private bool m_playerTwoControllerCursor;

	// Token: 0x04006B27 RID: 27431
	[fsProperty]
	private GameOptions.PreferredScalingMode m_preferredScalingMode = GameOptions.PreferredScalingMode.FORCE_PIXEL_PERFECT;

	// Token: 0x04006B28 RID: 27432
	[fsProperty]
	private GameOptions.PreferredFullscreenMode m_preferredFullscreenMode;

	// Token: 0x04006B29 RID: 27433
	[fsProperty]
	public float PreferredMapZoom;

	// Token: 0x04006B2A RID: 27434
	[fsProperty]
	public float PreferredMinimapZoom = 2f;

	// Token: 0x04006B2B RID: 27435
	[fsProperty]
	private Minimap.MinimapDisplayMode m_minimapDisplayMode = Minimap.MinimapDisplayMode.FADE_ON_ROOM_SEAL;

	// Token: 0x04006B2C RID: 27436
	[fsProperty]
	private GameOptions.AudioHardwareMode m_audioHardware;

	// Token: 0x04006B2D RID: 27437
	[fsProperty]
	private float m_musicVolume = 80f;

	// Token: 0x04006B2E RID: 27438
	[fsProperty]
	private float m_soundVolume = 80f;

	// Token: 0x04006B2F RID: 27439
	[fsProperty]
	private float m_uiVolume = 80f;

	// Token: 0x04006B30 RID: 27440
	[fsProperty]
	public string lastUsedShortcutTarget;

	// Token: 0x04006B31 RID: 27441
	[fsProperty]
	public string playerOneBindingData;

	// Token: 0x04006B32 RID: 27442
	[fsProperty]
	public string playerTwoBindingData;

	// Token: 0x04006B33 RID: 27443
	[fsProperty]
	public string playerOneBindingDataV2;

	// Token: 0x04006B34 RID: 27444
	[fsProperty]
	public string playerTwoBindingDataV2;

	// Token: 0x02001307 RID: 4871
	public enum ControllerBlankControl
	{
		// Token: 0x04006B36 RID: 27446
		NONE,
		// Token: 0x04006B37 RID: 27447
		BOTH_STICKS_DOWN,
		// Token: 0x04006B38 RID: 27448
		[Obsolete("Players should only see NONE and BOTH_STICKS_DOWN; this is kept for legacy conversions only.")]
		CIRCLE,
		// Token: 0x04006B39 RID: 27449
		[Obsolete("Players should only see NONE and BOTH_STICKS_DOWN; this is kept for legacy conversions only.")]
		L1
	}

	// Token: 0x02001308 RID: 4872
	public enum ControllerAutoAim
	{
		// Token: 0x04006B3B RID: 27451
		AUTO_DETECT,
		// Token: 0x04006B3C RID: 27452
		ALWAYS,
		// Token: 0x04006B3D RID: 27453
		NEVER,
		// Token: 0x04006B3E RID: 27454
		COOP_ONLY
	}

	// Token: 0x02001309 RID: 4873
	public enum QuickstartCharacter
	{
		// Token: 0x04006B40 RID: 27456
		LAST_USED,
		// Token: 0x04006B41 RID: 27457
		PILOT,
		// Token: 0x04006B42 RID: 27458
		CONVICT,
		// Token: 0x04006B43 RID: 27459
		SOLDIER,
		// Token: 0x04006B44 RID: 27460
		GUIDE,
		// Token: 0x04006B45 RID: 27461
		BULLET,
		// Token: 0x04006B46 RID: 27462
		ROBOT
	}

	// Token: 0x0200130A RID: 4874
	public enum AudioHardwareMode
	{
		// Token: 0x04006B48 RID: 27464
		SPEAKERS,
		// Token: 0x04006B49 RID: 27465
		HEADPHONES
	}

	// Token: 0x0200130B RID: 4875
	public enum PixelatorMotionEnhancementMode
	{
		// Token: 0x04006B4B RID: 27467
		ENHANCED_EXPENSIVE,
		// Token: 0x04006B4C RID: 27468
		UNENHANCED_CHEAP
	}

	// Token: 0x0200130C RID: 4876
	public enum GameLootProfile
	{
		// Token: 0x04006B4E RID: 27470
		CURRENT,
		// Token: 0x04006B4F RID: 27471
		ORIGINAL = 5
	}

	// Token: 0x0200130D RID: 4877
	public enum FullscreenStyle
	{
		// Token: 0x04006B51 RID: 27473
		FULLSCREEN,
		// Token: 0x04006B52 RID: 27474
		BORDERLESS,
		// Token: 0x04006B53 RID: 27475
		WINDOWED
	}

	// Token: 0x0200130E RID: 4878
	public enum VisualPresetMode
	{
		// Token: 0x04006B55 RID: 27477
		RECOMMENDED,
		// Token: 0x04006B56 RID: 27478
		CUSTOM
	}

	// Token: 0x0200130F RID: 4879
	public enum PreferredScalingMode
	{
		// Token: 0x04006B58 RID: 27480
		PIXEL_PERFECT,
		// Token: 0x04006B59 RID: 27481
		UNIFORM_SCALING,
		// Token: 0x04006B5A RID: 27482
		FORCE_PIXEL_PERFECT,
		// Token: 0x04006B5B RID: 27483
		UNIFORM_SCALING_FAST
	}

	// Token: 0x02001310 RID: 4880
	public enum PreferredFullscreenMode
	{
		// Token: 0x04006B5D RID: 27485
		FULLSCREEN,
		// Token: 0x04006B5E RID: 27486
		BORDERLESS,
		// Token: 0x04006B5F RID: 27487
		WINDOWED
	}

	// Token: 0x02001311 RID: 4881
	public enum ControlPreset
	{
		// Token: 0x04006B61 RID: 27489
		RECOMMENDED,
		// Token: 0x04006B62 RID: 27490
		FLIPPED_TRIGGERS,
		// Token: 0x04006B63 RID: 27491
		CUSTOM
	}

	// Token: 0x02001312 RID: 4882
	public enum GenericHighMedLowOption
	{
		// Token: 0x04006B65 RID: 27493
		LOW,
		// Token: 0x04006B66 RID: 27494
		MEDIUM,
		// Token: 0x04006B67 RID: 27495
		HIGH,
		// Token: 0x04006B68 RID: 27496
		VERY_LOW
	}

	// Token: 0x02001313 RID: 4883
	public enum ControllerSymbology
	{
		// Token: 0x04006B6A RID: 27498
		PS4,
		// Token: 0x04006B6B RID: 27499
		Xbox,
		// Token: 0x04006B6C RID: 27500
		AutoDetect,
		// Token: 0x04006B6D RID: 27501
		Switch
	}
}
