using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Brave;
using Dungeonator;
using HutongGames.PlayMaker.Actions;
using InControl;
using Pathfinding;
using tk2dRuntime.TileMap;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Token: 0x020012EC RID: 4844
public class GameManager : BraveBehaviour
{
	// Token: 0x06006CBB RID: 27835 RVA: 0x002AD2EC File Offset: 0x002AB4EC
	public void DoSetResolution(int newWidth, int newHeight, bool newFullscreen)
	{
		UnityEngine.Debug.Log(string.Concat(new object[] { "Setting RESOLUTION internal to: ", newWidth, "|", newHeight, "|", newFullscreen }));
		if (newFullscreen != Screen.fullScreen)
		{
			bool flag = newFullscreen == Screen.fullScreen;
			Screen.SetResolution(newWidth, newHeight, newFullscreen);
			if (flag)
			{
				if (this.CurrentResolutionShiftCoroutine != null)
				{
					base.StopCoroutine(this.CurrentResolutionShiftCoroutine);
				}
				this.CurrentResolutionShiftCoroutine = base.StartCoroutine(this.SetResolutionPostFullscreenChange(newWidth, newHeight));
			}
		}
		else
		{
			Screen.SetResolution(newWidth, newHeight, Screen.fullScreen, Screen.currentResolution.refreshRate);
		}
	}

	// Token: 0x06006CBC RID: 27836 RVA: 0x002AD3A8 File Offset: 0x002AB5A8
	private IEnumerator SetResolutionPostFullscreenChange(int newWidth, int newHeight)
	{
		float delay = 0f;
		yield return null;
		yield return null;
		int lastW = Screen.width;
		int lastH = Screen.height;
		float ela = 0f;
		if (delay > 0f)
		{
			while (ela < delay)
			{
				ela += GameManager.INVARIANT_DELTA_TIME;
				if (lastW != Screen.width || lastH != Screen.height)
				{
					Screen.SetResolution(newWidth, newHeight, Screen.fullScreen);
					break;
				}
				yield return null;
			}
		}
		else
		{
			Screen.SetResolution(newWidth, newHeight, Screen.fullScreen);
		}
		this.CurrentResolutionShiftCoroutine = null;
		yield break;
	}

	// Token: 0x06006CBD RID: 27837 RVA: 0x002AD3D4 File Offset: 0x002AB5D4
	public static void LoadResolutionFromPS4()
	{
	}

	// Token: 0x06006CBE RID: 27838 RVA: 0x002AD3E4 File Offset: 0x002AB5E4
	private static void LoadResolutionFromOptions()
	{
		if (GameManager.IsReturningToBreach)
		{
			return;
		}
		GameOptions.PreferredFullscreenMode currentPreferredFullscreenMode = GameManager.Options.CurrentPreferredFullscreenMode;
		if (GameManager.Options.preferredResolutionX <= 50 || GameManager.Options.preferredResolutionY <= 50 || GameManager.Options.preferredResolutionX > 50000 || GameManager.Options.preferredResolutionY > 50000)
		{
			GameManager.Options.preferredResolutionX = -1;
			GameManager.Options.preferredResolutionY = -1;
		}
		if (GameManager.Options.preferredResolutionX <= 0)
		{
			Resolution resolution = Screen.resolutions[Screen.resolutions.Length - 1];
			GameManager.Options.preferredResolutionX = resolution.width;
			GameManager.Options.preferredResolutionY = resolution.height;
		}
		Resolution resolution2 = default(Resolution);
		resolution2.width = GameManager.Options.preferredResolutionX;
		resolution2.height = GameManager.Options.preferredResolutionY;
		resolution2.refreshRate = Screen.currentResolution.refreshRate;
		if (currentPreferredFullscreenMode != GameOptions.PreferredFullscreenMode.FULLSCREEN)
		{
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
			UnityEngine.Debug.Log("Invoking standard WIN startup methods to set fullscreen: " + flag);
			if (flag)
			{
				BraveOptionsMenuItem componentInChildren = GameUIRoot.Instance.PauseMenuPanel.GetComponent<PauseMenuController>().OptionsMenu.GetComponentInChildren<BraveOptionsMenuItem>();
				componentInChildren.StartCoroutine(componentInChildren.FrameDelayedWindowsShift(displayModes, resolution2));
			}
			else
			{
				BraveOptionsMenuItem.ResolutionManagerWin.TrySetDisplay(displayModes, resolution2, false, null);
			}
		}
		if (resolution2.width != Screen.width || resolution2.height != Screen.height || currentPreferredFullscreenMode == GameOptions.PreferredFullscreenMode.FULLSCREEN != Screen.fullScreen)
		{
			UnityEngine.Debug.Log(string.Concat(new object[]
			{
				"Invoking standard startup methods to set resolution: ",
				resolution2.width,
				"|",
				resolution2.height,
				"||",
				currentPreferredFullscreenMode.ToString()
			}));
			GameManager.Instance.DoSetResolution(resolution2.width, resolution2.height, currentPreferredFullscreenMode == GameOptions.PreferredFullscreenMode.FULLSCREEN);
		}
	}

	// Token: 0x17001035 RID: 4149
	// (get) Token: 0x06006CBF RID: 27839 RVA: 0x002AD628 File Offset: 0x002AB828
	// (set) Token: 0x06006CC0 RID: 27840 RVA: 0x002AD630 File Offset: 0x002AB830
	public static GameObject PlayerPrefabForNewGame
	{
		get
		{
			return GameManager.m_playerPrefabForNewGame;
		}
		set
		{
			GameManager.m_playerPrefabForNewGame = value;
			if (GameManager.m_playerPrefabForNewGame != null)
			{
				PlayableCharacters characterIdentity = GameManager.m_playerPrefabForNewGame.GetComponent<PlayerController>().characterIdentity;
				if (characterIdentity != PlayableCharacters.Eevee && characterIdentity != PlayableCharacters.Gunslinger)
				{
					GameManager.Options.LastPlayedCharacter = characterIdentity;
				}
				GameManager.LastUsedPlayerPrefab = GameManager.m_playerPrefabForNewGame;
				if (GameManager.LastUsedPlayerPrefab && GameManager.LastUsedPlayerPrefab.GetComponent<PlayerSpaceshipController>())
				{
					GameManager.LastUsedPlayerPrefab = (GameObject)BraveResources.Load("PlayerRogue", ".prefab");
				}
			}
		}
	}

	// Token: 0x17001036 RID: 4150
	// (get) Token: 0x06006CC1 RID: 27841 RVA: 0x002AD6C4 File Offset: 0x002AB8C4
	// (set) Token: 0x06006CC2 RID: 27842 RVA: 0x002AD6DC File Offset: 0x002AB8DC
	public static GameObject CoopPlayerPrefabForNewGame
	{
		get
		{
			return BraveResources.Load("PlayerCoopCultist", ".prefab") as GameObject;
		}
		set
		{
			GameManager.m_coopPlayerPrefabForNewGame = value;
			if (GameManager.m_coopPlayerPrefabForNewGame != null)
			{
				GameManager.LastUsedCoopPlayerPrefab = GameManager.m_coopPlayerPrefabForNewGame;
			}
		}
	}

	// Token: 0x17001037 RID: 4151
	// (get) Token: 0x06006CC3 RID: 27843 RVA: 0x002AD700 File Offset: 0x002AB900
	// (set) Token: 0x06006CC4 RID: 27844 RVA: 0x002AD72C File Offset: 0x002AB92C
	public static GameOptions Options
	{
		get
		{
			if (GameManager.m_options == null)
			{
				DebugTime.RecordStartTime();
				GameOptions.Load();
				DebugTime.Log("Load game options", new object[0]);
			}
			return GameManager.m_options;
		}
		set
		{
			GameManager.m_options = value;
		}
	}

	// Token: 0x17001038 RID: 4152
	// (get) Token: 0x06006CC5 RID: 27845 RVA: 0x002AD734 File Offset: 0x002AB934
	public DungeonFloorMusicController DungeonMusicController
	{
		get
		{
			return this.m_dungeonMusicController;
		}
	}

	// Token: 0x06006CC6 RID: 27846 RVA: 0x002AD73C File Offset: 0x002AB93C
	public static GameManager EnsureExistence()
	{
		if (GameManager.Instance == null)
		{
			return GameManager.Instance;
		}
		return GameManager.Instance;
	}

	// Token: 0x17001039 RID: 4153
	// (get) Token: 0x06006CC7 RID: 27847 RVA: 0x002AD75C File Offset: 0x002AB95C
	public static GameManager Instance
	{
		get
		{
			if (GameManager.PreventGameManagerExistence)
			{
				return null;
			}
			if (GameManager.mr_manager == null)
			{
				GameManager.mr_manager = (GameManager)UnityEngine.Object.FindObjectOfType(typeof(GameManager));
			}
			if (GameManager.mr_manager == null)
			{
				UnityEngine.Debug.Log("INSTANTRON");
				GameObject gameObject = new GameObject("_GameManager(temp)");
				GameManager.mr_manager = gameObject.AddComponent<GameManager>();
			}
			return GameManager.mr_manager;
		}
	}

	// Token: 0x1700103A RID: 4154
	// (get) Token: 0x06006CC8 RID: 27848 RVA: 0x002AD7D4 File Offset: 0x002AB9D4
	public static bool HasInstance
	{
		get
		{
			return GameManager.mr_manager != null && GameManager.mr_manager;
		}
	}

	// Token: 0x1700103B RID: 4155
	// (get) Token: 0x06006CC9 RID: 27849 RVA: 0x002AD7F4 File Offset: 0x002AB9F4
	// (set) Token: 0x06006CCA RID: 27850 RVA: 0x002AD7FC File Offset: 0x002AB9FC
	public static bool IsReturningToBreach { get; set; }

	// Token: 0x1700103C RID: 4156
	// (get) Token: 0x06006CCB RID: 27851 RVA: 0x002AD804 File Offset: 0x002ABA04
	public BossManager BossManager
	{
		get
		{
			return BraveResources.Load<BossManager>("AAA_BOSS_MANAGER", ".asset");
		}
	}

	// Token: 0x1700103D RID: 4157
	// (get) Token: 0x06006CCC RID: 27852 RVA: 0x002AD818 File Offset: 0x002ABA18
	public RewardManager RewardManager
	{
		get
		{
			if (GameManager.Options.CurrentGameLootProfile == GameOptions.GameLootProfile.ORIGINAL)
			{
				return this.OriginalRewardManager;
			}
			return this.CurrentRewardManager;
		}
	}

	// Token: 0x140000B0 RID: 176
	// (add) Token: 0x06006CCD RID: 27853 RVA: 0x002AD838 File Offset: 0x002ABA38
	// (remove) Token: 0x06006CCE RID: 27854 RVA: 0x002AD870 File Offset: 0x002ABA70
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event Action OnNewLevelFullyLoaded;

	// Token: 0x1700103E RID: 4158
	// (get) Token: 0x06006CCF RID: 27855 RVA: 0x002AD8A8 File Offset: 0x002ABAA8
	// (set) Token: 0x06006CD0 RID: 27856 RVA: 0x002AD8B0 File Offset: 0x002ABAB0
	public int CurrentRunSeed
	{
		get
		{
			return this.m_currentRunSeed;
		}
		set
		{
			int currentRunSeed = this.m_currentRunSeed;
			UnityEngine.Debug.LogError("SETTING GLOBAL RUN SEED TO: " + value);
			this.m_currentRunSeed = value;
			UnityEngine.Random.InitState(this.m_currentRunSeed);
			BraveRandom.IgnoreGenerationDifferentiator = true;
			BraveRandom.InitializeWithSeed(value);
			if (this.m_currentRunSeed != currentRunSeed || this.m_forceSeedUpdate)
			{
				this.m_forceSeedUpdate = false;
				UnityEngine.Debug.LogError("DOING STARTUP SEED DATA");
				MetaInjectionData.ClearBlueprint();
				GameManager.Instance.GlobalInjectionData.PreprocessRun(false);
				RewardManifest.ClearManifest(this.RewardManager);
				RewardManifest.Initialize(this.RewardManager);
			}
		}
	}

	// Token: 0x1700103F RID: 4159
	// (get) Token: 0x06006CD1 RID: 27857 RVA: 0x002AD94C File Offset: 0x002ABB4C
	public bool IsSeeded
	{
		get
		{
			return this.m_currentRunSeed != 0;
		}
	}

	// Token: 0x17001040 RID: 4160
	// (get) Token: 0x06006CD2 RID: 27858 RVA: 0x002AD95C File Offset: 0x002ABB5C
	public bool IsPaused
	{
		get
		{
			return this.m_paused;
		}
	}

	// Token: 0x17001041 RID: 4161
	// (get) Token: 0x06006CD3 RID: 27859 RVA: 0x002AD964 File Offset: 0x002ABB64
	public bool UnpausedThisFrame
	{
		get
		{
			return this.m_unpausedThisFrame;
		}
	}

	// Token: 0x17001042 RID: 4162
	// (get) Token: 0x06006CD4 RID: 27860 RVA: 0x002AD96C File Offset: 0x002ABB6C
	// (set) Token: 0x06006CD5 RID: 27861 RVA: 0x002AD974 File Offset: 0x002ABB74
	public bool IsLoadingLevel
	{
		get
		{
			return this.m_loadingLevel;
		}
		private set
		{
			if (!value)
			{
				GameManager.IsReturningToBreach = false;
			}
			this.m_loadingLevel = value;
		}
	}

	// Token: 0x17001043 RID: 4163
	// (get) Token: 0x06006CD6 RID: 27862 RVA: 0x002AD98C File Offset: 0x002ABB8C
	// (set) Token: 0x06006CD7 RID: 27863 RVA: 0x002AD994 File Offset: 0x002ABB94
	public bool IsFoyer
	{
		get
		{
			return this.m_isFoyer;
		}
		set
		{
			this.m_isFoyer = value;
			if (!this.m_isFoyer)
			{
				Foyer.ClearInstance();
			}
		}
	}

	// Token: 0x17001044 RID: 4164
	// (get) Token: 0x06006CD8 RID: 27864 RVA: 0x002AD9B0 File Offset: 0x002ABBB0
	public static bool IsTurboMode
	{
		get
		{
			return GameStatsManager.HasInstance && GameStatsManager.Instance.isTurboMode;
		}
	}

	// Token: 0x17001045 RID: 4165
	// (get) Token: 0x06006CD9 RID: 27865 RVA: 0x002AD9C8 File Offset: 0x002ABBC8
	// (set) Token: 0x06006CDA RID: 27866 RVA: 0x002AD9D0 File Offset: 0x002ABBD0
	public GameManager.GameMode CurrentGameMode
	{
		get
		{
			return this.m_currentGameMode;
		}
		set
		{
			this.m_currentGameMode = value;
		}
	}

	// Token: 0x17001046 RID: 4166
	// (get) Token: 0x06006CDB RID: 27867 RVA: 0x002AD9DC File Offset: 0x002ABBDC
	// (set) Token: 0x06006CDC RID: 27868 RVA: 0x002AD9E4 File Offset: 0x002ABBE4
	public GameManager.GameType CurrentGameType
	{
		get
		{
			return this.m_currentGameType;
		}
		set
		{
			this.m_currentGameType = value;
		}
	}

	// Token: 0x17001047 RID: 4167
	// (get) Token: 0x06006CDD RID: 27869 RVA: 0x002AD9F0 File Offset: 0x002ABBF0
	public GameManager.LevelOverrideState GeneratingLevelOverrideState
	{
		get
		{
			if (this.m_generatingLevelState != null)
			{
				return this.m_generatingLevelState.Value;
			}
			return this.CurrentLevelOverrideState;
		}
	}

	// Token: 0x17001048 RID: 4168
	// (get) Token: 0x06006CDE RID: 27870 RVA: 0x002ADA14 File Offset: 0x002ABC14
	public GameManager.LevelOverrideState CurrentLevelOverrideState
	{
		get
		{
			if (this.IsLoadingLevel && this.CurrentlyGeneratingDungeonPrefab != null)
			{
				return this.CurrentlyGeneratingDungeonPrefab.LevelOverrideType;
			}
			if (this.Dungeon == null)
			{
				if (this.BestGenerationDungeonPrefab != null)
				{
					return this.BestGenerationDungeonPrefab.LevelOverrideType;
				}
				return GameManager.LevelOverrideState.NONE;
			}
			else
			{
				if (this.Dungeon.IsEndTimes)
				{
					return GameManager.LevelOverrideState.END_TIMES;
				}
				return this.Dungeon.LevelOverrideType;
			}
		}
	}

	// Token: 0x17001049 RID: 4169
	// (get) Token: 0x06006CDF RID: 27871 RVA: 0x002ADA98 File Offset: 0x002ABC98
	public int CurrentFloor
	{
		get
		{
			if (this.IsFoyer)
			{
				return 0;
			}
			GameLevelDefinition lastLoadedLevelDefinition = this.GetLastLoadedLevelDefinition();
			int num = -1;
			if (lastLoadedLevelDefinition != null)
			{
				num = this.dungeonFloors.IndexOf(lastLoadedLevelDefinition);
			}
			return num;
		}
	}

	// Token: 0x1700104A RID: 4170
	// (get) Token: 0x06006CE0 RID: 27872 RVA: 0x002ADAD0 File Offset: 0x002ABCD0
	public Dungeon Dungeon
	{
		get
		{
			if (this.m_dungeon == null)
			{
				this.m_dungeon = UnityEngine.Object.FindObjectOfType<Dungeon>();
			}
			return this.m_dungeon;
		}
	}

	// Token: 0x06006CE1 RID: 27873 RVA: 0x002ADAF4 File Offset: 0x002ABCF4
	public void ClearGenerativeDungeonData()
	{
		this.CurrentlyGeneratingDungeonPrefab = null;
		this.PregeneratedDungeonData = null;
	}

	// Token: 0x1700104B RID: 4171
	// (get) Token: 0x06006CE2 RID: 27874 RVA: 0x002ADB04 File Offset: 0x002ABD04
	public Dungeon BestGenerationDungeonPrefab
	{
		get
		{
			if (this.IsLoadingLevel && this.CurrentlyGeneratingDungeonPrefab != null)
			{
				return this.CurrentlyGeneratingDungeonPrefab;
			}
			return this.m_dungeon;
		}
	}

	// Token: 0x1700104C RID: 4172
	// (get) Token: 0x06006CE3 RID: 27875 RVA: 0x002ADB30 File Offset: 0x002ABD30
	public CameraController MainCameraController
	{
		get
		{
			if (this.m_camera == null)
			{
				GameObject gameObject = GameObject.Find("Main Camera");
				if (gameObject != null)
				{
					this.m_camera = gameObject.GetComponent<CameraController>();
				}
				else if (Camera.main)
				{
					this.m_camera = Camera.main.GetComponent<CameraController>();
				}
			}
			return this.m_camera;
		}
	}

	// Token: 0x06006CE4 RID: 27876 RVA: 0x002ADB9C File Offset: 0x002ABD9C
	public bool HasPlayer(PlayerController p)
	{
		for (int i = 0; i < this.AllPlayers.Length; i++)
		{
			if (this.AllPlayers[i] == p)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x1700104D RID: 4173
	// (get) Token: 0x06006CE5 RID: 27877 RVA: 0x002ADBD8 File Offset: 0x002ABDD8
	public PlayerController[] AllPlayers
	{
		get
		{
			if (this.m_players != null)
			{
				for (int i = 0; i < this.m_players.Length; i++)
				{
					if (!this.m_players[i])
					{
						this.m_player = null;
						this.m_secondaryPlayer = null;
						this.m_players = null;
						break;
					}
				}
			}
			if (this.m_players == null || (this.m_players.Length == 0 && !this.IsSelectingCharacter))
			{
				List<PlayerController> list = new List<PlayerController>(UnityEngine.Object.FindObjectsOfType<PlayerController>());
				for (int j = 0; j < list.Count; j++)
				{
					if (!list[j])
					{
						list.RemoveAt(j);
						j--;
					}
				}
				this.m_players = list.ToArray();
				if (this.m_players != null && this.m_players.Length == 2 && this.m_players[1].IsPrimaryPlayer)
				{
					PlayerController playerController = this.m_players[0];
					this.m_players[0] = this.m_players[1];
					this.m_players[1] = playerController;
				}
			}
			return this.m_players;
		}
	}

	// Token: 0x1700104E RID: 4174
	// (get) Token: 0x06006CE6 RID: 27878 RVA: 0x002ADCF8 File Offset: 0x002ABEF8
	public int NumberOfLivingPlayers
	{
		get
		{
			int num = 0;
			for (int i = 0; i < this.AllPlayers.Length; i++)
			{
				if (!this.AllPlayers[i].IsGhost && !this.AllPlayers[i].healthHaver.IsDead)
				{
					num++;
				}
			}
			return num;
		}
	}

	// Token: 0x06006CE7 RID: 27879 RVA: 0x002ADD50 File Offset: 0x002ABF50
	public bool PlayerIsInRoom(RoomHandler targetRoom)
	{
		for (int i = 0; i < this.AllPlayers.Length; i++)
		{
			if (this.AllPlayers[i].CurrentRoom == targetRoom)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06006CE8 RID: 27880 RVA: 0x002ADD8C File Offset: 0x002ABF8C
	public bool PlayerIsNearRoom(RoomHandler targetRoom)
	{
		for (int i = 0; i < this.AllPlayers.Length; i++)
		{
			if (this.AllPlayers[i].CurrentRoom == targetRoom || (this.AllPlayers[i].CurrentRoom != null && this.AllPlayers[i].CurrentRoom.connectedRooms != null && this.AllPlayers[i].CurrentRoom.connectedRooms.Contains(targetRoom)))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06006CE9 RID: 27881 RVA: 0x002ADE10 File Offset: 0x002AC010
	public void RefreshAllPlayers()
	{
		this.m_players = null;
		this.m_players = this.AllPlayers;
	}

	// Token: 0x1700104F RID: 4175
	// (get) Token: 0x06006CEA RID: 27882 RVA: 0x002ADE28 File Offset: 0x002AC028
	// (set) Token: 0x06006CEB RID: 27883 RVA: 0x002ADEBC File Offset: 0x002AC0BC
	public PlayerController PrimaryPlayer
	{
		get
		{
			if (this.IsSelectingCharacter && this.IsFoyer)
			{
				return null;
			}
			if (GameManager.IsReturningToBreach && !GameManager.IsReturningToFoyerWithPlayer && this.IsFoyer)
			{
				return null;
			}
			if (this.m_player == null)
			{
				PlayerController[] array = UnityEngine.Object.FindObjectsOfType<PlayerController>();
				for (int i = 0; i < array.Length; i++)
				{
					if (array[i].IsPrimaryPlayer)
					{
						this.m_player = array[i];
						break;
					}
				}
			}
			return this.m_player;
		}
		set
		{
			this.m_player = value;
			if (this.m_players != null && this.m_players.Length > 0)
			{
				this.m_players[0] = value;
			}
		}
	}

	// Token: 0x17001050 RID: 4176
	// (get) Token: 0x06006CEC RID: 27884 RVA: 0x002ADEE8 File Offset: 0x002AC0E8
	// (set) Token: 0x06006CED RID: 27885 RVA: 0x002ADF70 File Offset: 0x002AC170
	public PlayerController SecondaryPlayer
	{
		get
		{
			if (GameManager.Instance.CurrentGameType == GameManager.GameType.SINGLE_PLAYER)
			{
				return null;
			}
			if (this.m_secondaryPlayer == null)
			{
				for (int i = 0; i < this.AllPlayers.Length; i++)
				{
					if (!this.AllPlayers[i].IsPrimaryPlayer && this.AllPlayers[i].characterIdentity == PlayableCharacters.CoopCultist)
					{
						this.m_secondaryPlayer = this.AllPlayers[i];
						break;
					}
				}
			}
			return this.m_secondaryPlayer;
		}
		set
		{
			this.m_secondaryPlayer = value;
			if (this.m_players != null && this.m_players.Length > 1)
			{
				this.m_players[1] = value;
			}
			if (this.m_players != null && this.m_players.Length < 2)
			{
				this.m_players = null;
			}
		}
	}

	// Token: 0x06006CEE RID: 27886 RVA: 0x002ADFC8 File Offset: 0x002AC1C8
	public PlayerController GetOtherPlayer(PlayerController p)
	{
		if (this.CurrentGameType == GameManager.GameType.SINGLE_PLAYER)
		{
			return null;
		}
		return (!(p == this.PrimaryPlayer)) ? this.PrimaryPlayer : this.SecondaryPlayer;
	}

	// Token: 0x17001051 RID: 4177
	// (get) Token: 0x06006CEF RID: 27887 RVA: 0x002ADFFC File Offset: 0x002AC1FC
	public PlayerController BestActivePlayer
	{
		get
		{
			if (!this.PrimaryPlayer && !this.SecondaryPlayer)
			{
				return null;
			}
			if (this.PrimaryPlayer.IsGhost || this.PrimaryPlayer.healthHaver.IsDead)
			{
				return this.SecondaryPlayer;
			}
			return this.PrimaryPlayer;
		}
	}

	// Token: 0x06006CF0 RID: 27888 RVA: 0x002AE060 File Offset: 0x002AC260
	public GlobalDungeonData.ValidTilesets GetNextTileset(GlobalDungeonData.ValidTilesets tilesetID)
	{
		switch (tilesetID)
		{
		case GlobalDungeonData.ValidTilesets.GUNGEON:
			return GlobalDungeonData.ValidTilesets.MINEGEON;
		case GlobalDungeonData.ValidTilesets.CASTLEGEON:
			return GlobalDungeonData.ValidTilesets.GUNGEON;
		default:
			if (tilesetID == GlobalDungeonData.ValidTilesets.MINEGEON)
			{
				return GlobalDungeonData.ValidTilesets.CATACOMBGEON;
			}
			if (tilesetID == GlobalDungeonData.ValidTilesets.CATACOMBGEON)
			{
				return GlobalDungeonData.ValidTilesets.FORGEGEON;
			}
			if (tilesetID == GlobalDungeonData.ValidTilesets.FORGEGEON)
			{
				return GlobalDungeonData.ValidTilesets.HELLGEON;
			}
			if (tilesetID == GlobalDungeonData.ValidTilesets.OFFICEGEON)
			{
				return GlobalDungeonData.ValidTilesets.FORGEGEON;
			}
			if (tilesetID != GlobalDungeonData.ValidTilesets.RATGEON)
			{
				return GlobalDungeonData.ValidTilesets.CASTLEGEON;
			}
			return GlobalDungeonData.ValidTilesets.CATACOMBGEON;
		case GlobalDungeonData.ValidTilesets.SEWERGEON:
			return GlobalDungeonData.ValidTilesets.GUNGEON;
		case GlobalDungeonData.ValidTilesets.CATHEDRALGEON:
			return GlobalDungeonData.ValidTilesets.MINEGEON;
		}
	}

	// Token: 0x06006CF1 RID: 27889 RVA: 0x002AE0E8 File Offset: 0x002AC2E8
	public int GetTargetLevelIndexFromSavedTileset(GlobalDungeonData.ValidTilesets tilesetID)
	{
		switch (tilesetID)
		{
		case GlobalDungeonData.ValidTilesets.GUNGEON:
			return 2;
		case GlobalDungeonData.ValidTilesets.CASTLEGEON:
			return 1;
		default:
			if (tilesetID == GlobalDungeonData.ValidTilesets.MINEGEON)
			{
				return 3;
			}
			if (tilesetID == GlobalDungeonData.ValidTilesets.CATACOMBGEON)
			{
				return 4;
			}
			if (tilesetID == GlobalDungeonData.ValidTilesets.FORGEGEON)
			{
				return 5;
			}
			if (tilesetID == GlobalDungeonData.ValidTilesets.HELLGEON)
			{
				return 6;
			}
			if (tilesetID == GlobalDungeonData.ValidTilesets.OFFICEGEON)
			{
				return 5;
			}
			if (tilesetID == GlobalDungeonData.ValidTilesets.FINALGEON)
			{
				return 6;
			}
			if (tilesetID != GlobalDungeonData.ValidTilesets.RATGEON)
			{
				return 1;
			}
			return 4;
		case GlobalDungeonData.ValidTilesets.SEWERGEON:
			return 2;
		case GlobalDungeonData.ValidTilesets.CATHEDRALGEON:
			return 3;
		}
	}

	// Token: 0x06006CF2 RID: 27890 RVA: 0x002AE180 File Offset: 0x002AC380
	public void SetNextLevelIndex(int index)
	{
		this.nextLevelIndex = index;
	}

	// Token: 0x17001052 RID: 4178
	// (get) Token: 0x06006CF3 RID: 27891 RVA: 0x002AE18C File Offset: 0x002AC38C
	// (set) Token: 0x06006CF4 RID: 27892 RVA: 0x002AE194 File Offset: 0x002AC394
	public string InjectedFlowPath
	{
		get
		{
			return this.m_injectedFlowPath;
		}
		set
		{
			this.m_injectedFlowPath = value;
		}
	}

	// Token: 0x17001053 RID: 4179
	// (get) Token: 0x06006CF5 RID: 27893 RVA: 0x002AE1A0 File Offset: 0x002AC3A0
	// (set) Token: 0x06006CF6 RID: 27894 RVA: 0x002AE1A8 File Offset: 0x002AC3A8
	public string InjectedLevelName
	{
		get
		{
			return this.m_injectedLevelName;
		}
		set
		{
			this.m_injectedLevelName = value;
		}
	}

	// Token: 0x17001054 RID: 4180
	// (get) Token: 0x06006CF7 RID: 27895 RVA: 0x002AE1B4 File Offset: 0x002AC3B4
	// (set) Token: 0x06006CF8 RID: 27896 RVA: 0x002AE1BC File Offset: 0x002AC3BC
	public bool PreventPausing
	{
		get
		{
			return this.m_preventUnpausing;
		}
		set
		{
			if (this.m_preventUnpausing == value)
			{
				return;
			}
			this.m_preventUnpausing = value;
			if (!this.m_preventUnpausing && !this.m_applicationHasFocus && !this.m_loadingLevel && !this.m_paused)
			{
				this.Pause();
				GameStatsManager.Save();
			}
		}
	}

	// Token: 0x17001055 RID: 4181
	// (get) Token: 0x06006CF9 RID: 27897 RVA: 0x002AE218 File Offset: 0x002AC418
	// (set) Token: 0x06006CFA RID: 27898 RVA: 0x002AE220 File Offset: 0x002AC420
	public bool InTutorial { get; set; }

	// Token: 0x06006CFB RID: 27899 RVA: 0x002AE22C File Offset: 0x002AC42C
	public void OnApplicationQuit()
	{
		GameManager.IsShuttingDown = true;
		if (this.ShouldDeleteSaveOnExit)
		{
			SaveManager.DeleteCurrentSlotMidGameSave(null);
		}
	}

	// Token: 0x17001056 RID: 4182
	// (get) Token: 0x06006CFC RID: 27900 RVA: 0x002AE258 File Offset: 0x002AC458
	public bool ShouldDeleteSaveOnExit
	{
		get
		{
			return !this.IsFoyer && !SaveManager.PreventMidgameSaveDeletionOnExit && !GameManager.BackgroundGenerationActive && !Dungeon.IsGenerating;
		}
	}

	// Token: 0x06006CFD RID: 27901 RVA: 0x002AE284 File Offset: 0x002AC484
	public void OnApplicationFocus(bool focusStatus)
	{
		if (!Application.isEditor && !MemoryTester.HasInstance)
		{
			if (!focusStatus && this.PrimaryPlayer != null && !this.PreventPausing && !this.m_loadingLevel && !this.m_paused)
			{
				this.Pause();
				GameStatsManager.Save();
			}
			this.m_applicationHasFocus = focusStatus;
		}
	}

	// Token: 0x06006CFE RID: 27902 RVA: 0x002AE2F0 File Offset: 0x002AC4F0
	protected void Update()
	{
		tk2dSpriteAnimator.InDungeonScene = this.m_dungeon != null;
		BraveTime.UpdateScaledTimeSinceStartup();
		if (GameManager.IsBossIntro)
		{
			if (GameManager.s_bossIntroTimeId < 0)
			{
				GameManager.s_bossIntroTimeId = Shader.PropertyToID("_BossIntroTime");
			}
			GameManager.s_bossIntroTime += new Vector4(0.05f, 1f, 2f, 3f) * GameManager.INVARIANT_DELTA_TIME;
			Shader.SetGlobalVector(GameManager.s_bossIntroTimeId, GameManager.s_bossIntroTime);
		}
		float realtimeSinceStartup = Time.realtimeSinceStartup;
		GameManager.m_deltaTime = Time.unscaledDeltaTime;
		GameManager.m_lastFrameRealtime = realtimeSinceStartup;
		GameManager.INVARIANT_DELTA_TIME = GameManager.m_deltaTime;
		this.InvariantUpdate(GameManager.m_deltaTime);
		this.m_frameTimes.Enqueue(GameManager.INVARIANT_DELTA_TIME);
		for (int i = 0; i < StaticReferenceManager.AllClusteredTimeInvariantBehaviours.Count; i++)
		{
			ClusteredTimeInvariantMonoBehaviour clusteredTimeInvariantMonoBehaviour = StaticReferenceManager.AllClusteredTimeInvariantBehaviours[i];
			if (!clusteredTimeInvariantMonoBehaviour)
			{
				StaticReferenceManager.AllClusteredTimeInvariantBehaviours.RemoveAt(i);
				i--;
			}
			else
			{
				clusteredTimeInvariantMonoBehaviour.DoUpdate(GameManager.INVARIANT_DELTA_TIME);
			}
		}
		if (GameManager.AUDIO_ENABLED)
		{
			if (!this.m_player && !this.m_audioListener)
			{
				UnityEngine.Debug.LogWarning("Adding a new GameManager audio listener");
				this.m_audioListener = base.gameObject.GetOrAddComponent<AkAudioListener>();
			}
			else if (this.m_player && this.m_audioListener)
			{
				UnityEngine.Debug.LogWarning("Destroying the GameManager's audio listener");
				UnityEngine.Object.Destroy(this.m_audioListener);
				this.m_audioListener = null;
			}
		}
	}

	// Token: 0x06006CFF RID: 27903 RVA: 0x002AE48C File Offset: 0x002AC68C
	private void LateUpdate()
	{
		this.platformInterface.LateUpdate();
		this.m_unpausedThisFrame = false;
	}

	// Token: 0x06006D00 RID: 27904 RVA: 0x002AE4A0 File Offset: 0x002AC6A0
	public PlayerController GetPlayerClosestToPoint(Vector2 point)
	{
		PlayerController playerController = null;
		float num = float.MaxValue;
		for (int i = 0; i < this.AllPlayers.Length; i++)
		{
			if (!this.AllPlayers[i].healthHaver.IsDead)
			{
				float num2 = Vector2.Distance(point, this.AllPlayers[i].CenterPosition);
				if (num2 < num)
				{
					num = num2;
					playerController = this.AllPlayers[i];
				}
			}
		}
		return playerController;
	}

	// Token: 0x06006D01 RID: 27905 RVA: 0x002AE514 File Offset: 0x002AC714
	public PlayerController GetPlayerClosestToPoint(Vector2 point, out float range)
	{
		PlayerController playerController = null;
		float num = float.MaxValue;
		for (int i = 0; i < this.AllPlayers.Length; i++)
		{
			if (!this.AllPlayers[i].healthHaver.IsDead)
			{
				float num2 = Vector2.Distance(point, this.AllPlayers[i].CenterPosition);
				if (num2 < num)
				{
					num = num2;
					playerController = this.AllPlayers[i];
				}
			}
		}
		range = num;
		return playerController;
	}

	// Token: 0x06006D02 RID: 27906 RVA: 0x002AE588 File Offset: 0x002AC788
	public PlayerController GetActivePlayerClosestToPoint(Vector2 point, bool allowStealth = false)
	{
		if (this.IsSelectingCharacter)
		{
			return null;
		}
		if (!GameManager.IsReturningToFoyerWithPlayer && GameManager.IsReturningToBreach && this.IsFoyer)
		{
			return null;
		}
		PlayerController playerController = null;
		float num = float.MaxValue;
		for (int i = 0; i < this.AllPlayers.Length; i++)
		{
			if (!this.AllPlayers[i].IsGhost && !this.AllPlayers[i].healthHaver.IsDead)
			{
				if (!this.AllPlayers[i].IsFalling)
				{
					if (!this.AllPlayers[i].IsCurrentlyCoopReviving)
					{
						if (allowStealth || !this.AllPlayers[i].IsStealthed)
						{
							float num2 = Vector2.Distance(point, this.AllPlayers[i].CenterPosition);
							if (num2 < num)
							{
								num = num2;
								playerController = this.AllPlayers[i];
							}
						}
					}
				}
			}
		}
		return playerController;
	}

	// Token: 0x06006D03 RID: 27907 RVA: 0x002AE684 File Offset: 0x002AC884
	public bool IsAnyPlayerInRoom(RoomHandler room)
	{
		for (int i = 0; i < this.AllPlayers.Length; i++)
		{
			if (!this.AllPlayers[i].healthHaver.IsDead && this.AllPlayers[i].CurrentRoom == room)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06006D04 RID: 27908 RVA: 0x002AE6D8 File Offset: 0x002AC8D8
	public PlayerController GetRandomActivePlayer()
	{
		if (this.CurrentGameType != GameManager.GameType.COOP_2_PLAYER)
		{
			return this.PrimaryPlayer;
		}
		if (this.PrimaryPlayer.healthHaver.IsAlive && this.SecondaryPlayer.healthHaver.IsAlive)
		{
			return (UnityEngine.Random.value <= 0.5f) ? this.SecondaryPlayer : this.PrimaryPlayer;
		}
		return this.BestActivePlayer;
	}

	// Token: 0x06006D05 RID: 27909 RVA: 0x002AE74C File Offset: 0x002AC94C
	public PlayerController GetRandomPlayer()
	{
		return this.AllPlayers[UnityEngine.Random.Range(0, this.AllPlayers.Length)];
	}

	// Token: 0x06006D06 RID: 27910 RVA: 0x002AE764 File Offset: 0x002AC964
	public void DelayedQuickRestart(float duration, QuickRestartOptions options = default(QuickRestartOptions))
	{
		base.StartCoroutine(this.DelayedQuickRestart_CR(duration, options));
	}

	// Token: 0x06006D07 RID: 27911 RVA: 0x002AE778 File Offset: 0x002AC978
	private IEnumerator DelayedQuickRestart_CR(float duration, QuickRestartOptions options)
	{
		if (this.m_loadingLevel)
		{
			yield break;
		}
		this.m_loadingLevel = true;
		float elapsed = 0f;
		while (elapsed < duration)
		{
			elapsed += GameManager.INVARIANT_DELTA_TIME;
			yield return null;
		}
		this.QuickRestart(options);
		yield break;
	}

	// Token: 0x06006D08 RID: 27912 RVA: 0x002AE7A4 File Offset: 0x002AC9A4
	public void QuickRestart(QuickRestartOptions options = default(QuickRestartOptions))
	{
		if (this.m_paused)
		{
			this.ForceUnpause();
		}
		this.m_loadingLevel = true;
		ChallengeManager componentInChildren = base.GetComponentInChildren<ChallengeManager>();
		if (componentInChildren)
		{
			UnityEngine.Object.Destroy(componentInChildren.gameObject);
		}
		SaveManager.DeleteCurrentSlotMidGameSave(null);
		if (options.BossRush)
		{
			this.CurrentGameMode = GameManager.GameMode.BOSSRUSH;
		}
		else if (this.CurrentGameMode == GameManager.GameMode.BOSSRUSH)
		{
			this.CurrentGameMode = GameManager.GameMode.NORMAL;
		}
		bool flag = GameManager.Instance.CurrentGameMode == GameManager.GameMode.SHORTCUT;
		if (this.PrimaryPlayer != null)
		{
			GameManager.ForceQuickRestartAlternateCostumeP1 = this.PrimaryPlayer.IsUsingAlternateCostume;
			GameManager.ForceQuickRestartAlternateGunP1 = this.PrimaryPlayer.UsingAlternateStartingGuns;
		}
		if (this.CurrentGameType == GameManager.GameType.COOP_2_PLAYER && this.SecondaryPlayer != null)
		{
			GameManager.ForceQuickRestartAlternateCostumeP2 = this.SecondaryPlayer.IsUsingAlternateCostume;
			GameManager.ForceQuickRestartAlternateGunP2 = this.SecondaryPlayer.UsingAlternateStartingGuns;
		}
		this.ClearPerLevelData();
		this.FlushAudio();
		this.ClearActiveGameData(false, true);
		if (this.TargetQuickRestartLevel != -1)
		{
			this.nextLevelIndex = this.TargetQuickRestartLevel;
		}
		else
		{
			this.nextLevelIndex = 1;
			if (flag)
			{
				this.nextLevelIndex += this.LastShortcutFloorLoaded;
				this.IsLoadingFirstShortcutFloor = true;
			}
		}
		if (GameManager.LastUsedPlayerPrefab != null)
		{
			GameManager.PlayerPrefabForNewGame = GameManager.LastUsedPlayerPrefab;
			PlayerController component = GameManager.PlayerPrefabForNewGame.GetComponent<PlayerController>();
			GameStatsManager.Instance.BeginNewSession(component);
		}
		if (this.CurrentGameType == GameManager.GameType.COOP_2_PLAYER && GameManager.LastUsedCoopPlayerPrefab != null)
		{
			GameManager.CoopPlayerPrefabForNewGame = GameManager.LastUsedCoopPlayerPrefab;
		}
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		this.m_preventUnpausing = false;
		if (this.m_currentRunSeed != 0)
		{
			this.m_forceSeedUpdate = true;
			this.CurrentRunSeed = this.CurrentRunSeed;
		}
		UnityEngine.Debug.Log("Quick Restarting...");
		if (this.CurrentGameMode == GameManager.GameMode.BOSSRUSH)
		{
			this.SetNextLevelIndex(1);
			this.InstantLoadBossRushFloor(1);
			this.nextLevelIndex++;
		}
		else
		{
			GameManager.Instance.LoadNextLevel();
		}
		base.StartCoroutine(this.PostQuickStartCR(options));
	}

	// Token: 0x06006D09 RID: 27913 RVA: 0x002AE9C8 File Offset: 0x002ACBC8
	private IEnumerator PostQuickStartCR(QuickRestartOptions options)
	{
		while (this.IsLoadingLevel || Dungeon.IsGenerating)
		{
			yield return null;
		}
		if (options.GunGame)
		{
			SetExoticPlayerFlag.SetGunGame(false);
		}
		if (options.ChallengeMode != ChallengeModeType.None)
		{
			ChallengeManager.ChallengeModeType = options.ChallengeMode;
		}
		GameStatsManager.Instance.SetStat(TrackedStats.TIME_PLAYED, 0f);
		yield break;
	}

	// Token: 0x06006D0A RID: 27914 RVA: 0x002AE9EC File Offset: 0x002ACBEC
	public AsyncOperation BeginAsyncLoadMainMenu()
	{
		AsyncOperation asyncOperation = SceneManager.LoadSceneAsync("MainMenu");
		asyncOperation.allowSceneActivation = false;
		return asyncOperation;
	}

	// Token: 0x06006D0B RID: 27915 RVA: 0x002AEA0C File Offset: 0x002ACC0C
	public void EndAsyncLoadMainMenu(AsyncOperation loader)
	{
		if (this.m_paused)
		{
			this.ForceUnpause();
		}
		this.ClearPerLevelData();
		this.FlushAudio();
		this.ClearActiveGameData(true, false);
		this.m_preventUnpausing = false;
		loader.allowSceneActivation = true;
	}

	// Token: 0x06006D0C RID: 27916 RVA: 0x002AEA44 File Offset: 0x002ACC44
	public void DelayedLoadMainMenu(float duration)
	{
		base.StartCoroutine(this.DelayedLoadMainMenu_CR(duration));
	}

	// Token: 0x06006D0D RID: 27917 RVA: 0x002AEA54 File Offset: 0x002ACC54
	private IEnumerator DelayedLoadMainMenu_CR(float duration)
	{
		if (this.m_loadingLevel)
		{
			yield break;
		}
		this.m_loadingLevel = true;
		float elapsed = 0f;
		while (elapsed < duration)
		{
			elapsed += GameManager.INVARIANT_DELTA_TIME;
			yield return null;
		}
		this.LoadMainMenu();
		yield break;
	}

	// Token: 0x06006D0E RID: 27918 RVA: 0x002AEA78 File Offset: 0x002ACC78
	public void LoadMainMenu()
	{
		if (this.m_paused)
		{
			this.ForceUnpause();
		}
		this.m_loadingLevel = true;
		this.ClearPerLevelData();
		this.FlushAudio();
		this.ClearActiveGameData(true, true);
		BraveCameraUtility.OverrideAspect = new float?(1.7777778f);
		this.m_preventUnpausing = false;
		this.IsLoadingLevel = false;
		Foyer.DoIntroSequence = false;
		Foyer.DoMainMenu = true;
		SceneManager.LoadScene("tt_foyer");
	}

	// Token: 0x06006D0F RID: 27919 RVA: 0x002AEAE4 File Offset: 0x002ACCE4
	public void FrameDelayedEnteredFoyer(PlayerController p)
	{
		if (Foyer.Instance != null)
		{
			Foyer.Instance.ProcessPlayerEnteredFoyer(p);
		}
		base.StartCoroutine(this.HandleFrameDelayedEnteredFoyer(p));
	}

	// Token: 0x06006D10 RID: 27920 RVA: 0x002AEB10 File Offset: 0x002ACD10
	private IEnumerator HandleFrameDelayedEnteredFoyer(PlayerController p)
	{
		yield return null;
		Foyer.Instance.ProcessPlayerEnteredFoyer(p);
		yield break;
	}

	// Token: 0x06006D11 RID: 27921 RVA: 0x002AEB2C File Offset: 0x002ACD2C
	public void DelayedReturnToFoyer(float delay)
	{
		this.m_preparingToDestroyThisGameManagerOnCollision = true;
		base.StartCoroutine(this.DelayedReturnToFoyer_CR(delay));
	}

	// Token: 0x06006D12 RID: 27922 RVA: 0x002AEB44 File Offset: 0x002ACD44
	private IEnumerator DelayedReturnToFoyer_CR(float delay)
	{
		if (this.m_loadingLevel)
		{
			yield break;
		}
		this.m_loadingLevel = true;
		yield return new WaitForSeconds(delay);
		if (GameUIRoot.Instance != null)
		{
			GameUIRoot.Instance.ToggleUICamera(false);
			yield return null;
		}
		this.ReturnToFoyer();
		yield break;
	}

	// Token: 0x06006D13 RID: 27923 RVA: 0x002AEB68 File Offset: 0x002ACD68
	public void ReturnToFoyer()
	{
		if (this.m_paused)
		{
			this.ForceUnpause();
		}
		GameManager.IsReturningToFoyerWithPlayer = true;
		this.ClearPerLevelData();
		this.FlushAudio();
		this.nextLevelIndex = 1;
		this.ClearActiveGameData(false, true);
		if (GameManager.LastUsedPlayerPrefab != null)
		{
			GameManager.PlayerPrefabForNewGame = GameManager.LastUsedPlayerPrefab;
			PlayerController component = GameManager.PlayerPrefabForNewGame.GetComponent<PlayerController>();
			GameStatsManager.Instance.BeginNewSession(component);
		}
		else
		{
			UnityEngine.Debug.LogError("Attempting to clear player data on foyer return, but LastUsedPlayer is null!");
		}
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		this.m_preventUnpausing = false;
		this.LoadNextLevel();
	}

	// Token: 0x06006D14 RID: 27924 RVA: 0x002AEC00 File Offset: 0x002ACE00
	public void LoadCustomFlowForDebug(string flowpath, string dungeonPrefab = "", string sceneName = "")
	{
		DungeonFlow dungeonFlow = FlowDatabase.GetOrLoadByName(flowpath);
		if (dungeonFlow == null)
		{
			dungeonFlow = FlowDatabase.GetOrLoadByName("Boss Rooms/" + flowpath);
		}
		if (dungeonFlow == null)
		{
			dungeonFlow = FlowDatabase.GetOrLoadByName("Boss Rush Flows/" + flowpath);
		}
		if (dungeonFlow == null)
		{
			dungeonFlow = FlowDatabase.GetOrLoadByName("Testing/" + flowpath);
		}
		if (dungeonFlow == null)
		{
			return;
		}
		this.m_loadingLevel = true;
		this.FlushAudio();
		this.ClearPerLevelData();
		float num = 1f;
		float num2 = 1f;
		if (!string.IsNullOrEmpty(sceneName))
		{
			for (int i = 0; i < this.customFloors.Count; i++)
			{
				if (this.customFloors[i].dungeonSceneName == sceneName)
				{
					num = this.customFloors[i].priceMultiplier;
					num2 = this.customFloors[i].enemyHealthMultiplier;
					break;
				}
			}
			for (int j = 0; j < this.dungeonFloors.Count; j++)
			{
				if (this.dungeonFloors[j].dungeonSceneName == sceneName)
				{
					num = this.dungeonFloors[j].priceMultiplier;
					num2 = this.dungeonFloors[j].enemyHealthMultiplier;
					break;
				}
			}
		}
		GameLevelDefinition gameLevelDefinition = new GameLevelDefinition();
		gameLevelDefinition.dungeonPrefabPath = ((!string.IsNullOrEmpty(dungeonPrefab)) ? dungeonPrefab : "Base_Gungeon");
		gameLevelDefinition.dungeonSceneName = ((!string.IsNullOrEmpty(sceneName)) ? sceneName : "BB_Beholster");
		gameLevelDefinition.priceMultiplier = num;
		gameLevelDefinition.enemyHealthMultiplier = num2;
		UnityEngine.Debug.Log(gameLevelDefinition.dungeonPrefabPath + "|" + gameLevelDefinition.dungeonSceneName);
		gameLevelDefinition.predefinedSeeds = new List<int>();
		gameLevelDefinition.flowEntries = new List<DungeonFlowLevelEntry>();
		DungeonFlowLevelEntry dungeonFlowLevelEntry = new DungeonFlowLevelEntry();
		dungeonFlowLevelEntry.flowPath = flowpath;
		dungeonFlowLevelEntry.forceUseIfAvailable = true;
		dungeonFlowLevelEntry.prerequisites = new DungeonPrerequisite[0];
		dungeonFlowLevelEntry.weight = 1f;
		gameLevelDefinition.flowEntries.Add(dungeonFlowLevelEntry);
		base.StartCoroutine(this.LoadNextLevelAsync_CR(gameLevelDefinition));
	}

	// Token: 0x06006D15 RID: 27925 RVA: 0x002AEE40 File Offset: 0x002AD040
	public void LoadCustomLevel(string custom)
	{
		if (this.dungeonFloors == null || this.dungeonFloors.Count == 0)
		{
			this.dungeonFloors = new List<GameLevelDefinition>();
			GameLevelDefinition gameLevelDefinition = new GameLevelDefinition();
			gameLevelDefinition.dungeonSceneName = SceneManager.GetActiveScene().name;
			this.dungeonFloors.Add(gameLevelDefinition);
		}
		this.m_loadingLevel = true;
		this.FlushAudio();
		this.ClearPerLevelData();
		GameLevelDefinition gameLevelDefinition2 = null;
		int num = -1;
		for (int i = 0; i < this.dungeonFloors.Count; i++)
		{
			if (this.dungeonFloors[i].dungeonSceneName == custom)
			{
				gameLevelDefinition2 = this.dungeonFloors[i];
				num = i + 1;
				break;
			}
		}
		if (gameLevelDefinition2 == null)
		{
			for (int j = 0; j < this.customFloors.Count; j++)
			{
				if (this.customFloors[j].dungeonSceneName == custom)
				{
					gameLevelDefinition2 = this.customFloors[j];
					break;
				}
			}
		}
		if (gameLevelDefinition2 != null && gameLevelDefinition2.dungeonPrefabPath == string.Empty)
		{
			if (gameLevelDefinition2.dungeonSceneName == "MainMenu")
			{
				this.LoadMainMenu();
				this.nextLevelIndex = 0;
			}
			else if (gameLevelDefinition2.dungeonSceneName == "Foyer")
			{
				SceneManager.LoadScene(gameLevelDefinition2.dungeonSceneName);
				this.IsLoadingLevel = false;
				this.nextLevelIndex = 1;
			}
			else
			{
				SceneManager.LoadScene(gameLevelDefinition2.dungeonSceneName);
				this.IsLoadingLevel = false;
			}
		}
		else
		{
			base.StartCoroutine(this.LoadNextLevelAsync_CR(gameLevelDefinition2));
			if (gameLevelDefinition2.dungeonSceneName == "tt_tutorial")
			{
				num = 0;
			}
			if (num != -1)
			{
				this.nextLevelIndex = num;
			}
		}
	}

	// Token: 0x06006D16 RID: 27926 RVA: 0x002AF01C File Offset: 0x002AD21C
	public static void InvalidateMidgameSave(bool saveStats)
	{
		MidGameSaveData midGameSaveData = null;
		if (GameManager.VerifyAndLoadMidgameSave(out midGameSaveData, false))
		{
			midGameSaveData.Invalidate();
			SaveManager.Save<MidGameSaveData>(midGameSaveData, SaveManager.MidGameSave, GameStatsManager.Instance.PlaytimeMin, 0U, null);
			GameStatsManager.Instance.midGameSaveGuid = midGameSaveData.midGameSaveGuid;
			if (saveStats)
			{
				GameStatsManager.Save();
			}
		}
	}

	// Token: 0x06006D17 RID: 27927 RVA: 0x002AF07C File Offset: 0x002AD27C
	public static void RevalidateMidgameSave()
	{
		MidGameSaveData midGameSaveData = null;
		if (GameManager.VerifyAndLoadMidgameSave(out midGameSaveData, false))
		{
			midGameSaveData.Revalidate();
			SaveManager.Save<MidGameSaveData>(midGameSaveData, SaveManager.MidGameSave, GameStatsManager.Instance.PlaytimeMin, 0U, null);
			GameStatsManager.Instance.midGameSaveGuid = midGameSaveData.midGameSaveGuid;
			GameStatsManager.Save();
		}
	}

	// Token: 0x06006D18 RID: 27928 RVA: 0x002AF0D4 File Offset: 0x002AD2D4
	public static void DoMidgameSave(GlobalDungeonData.ValidTilesets tileset)
	{
		string text = Guid.NewGuid().ToString();
		MidGameSaveData midGameSaveData = new MidGameSaveData(GameManager.Instance.PrimaryPlayer, GameManager.Instance.SecondaryPlayer, tileset, text);
		SaveManager.Save<MidGameSaveData>(midGameSaveData, SaveManager.MidGameSave, GameStatsManager.Instance.PlaytimeMin, 0U, null);
		GameStatsManager.Instance.midGameSaveGuid = text;
		GameStatsManager.Save();
	}

	// Token: 0x06006D19 RID: 27929 RVA: 0x002AF144 File Offset: 0x002AD344
	public static bool HasValidMidgameSave()
	{
		MidGameSaveData midGameSaveData;
		return GameManager.VerifyAndLoadMidgameSave(out midGameSaveData, true);
	}

	// Token: 0x06006D1A RID: 27930 RVA: 0x002AF15C File Offset: 0x002AD35C
	public static bool VerifyAndLoadMidgameSave(out MidGameSaveData midgameSave, bool checkValidity = true)
	{
		if (!SaveManager.Load<MidGameSaveData>(SaveManager.MidGameSave, out midgameSave, true, 0U, null, null))
		{
			UnityEngine.Debug.LogError("No mid game save found");
			return false;
		}
		if (midgameSave == null)
		{
			UnityEngine.Debug.LogError("Failed to load mid game save (0)");
			return false;
		}
		if (checkValidity && !midgameSave.IsValid())
		{
			return false;
		}
		if (GameStatsManager.Instance.midGameSaveGuid == null || GameStatsManager.Instance.midGameSaveGuid != midgameSave.midGameSaveGuid)
		{
			UnityEngine.Debug.LogError("Failed to load mid game save (1)");
			return false;
		}
		List<string> list = new List<string>(Brave.PlayerPrefs.GetStringArray("recent_mgs", ';'));
		if (list.Contains(midgameSave.midGameSaveGuid))
		{
			UnityEngine.Debug.LogError("Failed to load mid game save (2)");
			UnityEngine.Debug.LogError(Brave.PlayerPrefs.GetString("recent_mgs"));
			UnityEngine.Debug.LogError(midgameSave.midGameSaveGuid);
			return false;
		}
		return true;
	}

	// Token: 0x06006D1B RID: 27931 RVA: 0x002AF23C File Offset: 0x002AD43C
	public void DelayedLoadMidgameSave(float delay, MidGameSaveData saveToContinue)
	{
		GlobalDungeonData.ValidTilesets levelSaved = saveToContinue.levelSaved;
		if (levelSaved != GlobalDungeonData.ValidTilesets.SEWERGEON)
		{
			if (levelSaved != GlobalDungeonData.ValidTilesets.CATHEDRALGEON)
			{
				if (levelSaved != GlobalDungeonData.ValidTilesets.OFFICEGEON)
				{
					if (levelSaved != GlobalDungeonData.ValidTilesets.FINALGEON)
					{
						if (levelSaved != GlobalDungeonData.ValidTilesets.RATGEON)
						{
							this.DelayedLoadNextLevel(0.25f);
						}
						else
						{
							this.DelayedLoadCustomLevel(delay, "ss_resourcefulrat");
						}
					}
					else
					{
						switch (saveToContinue.playerOneData.CharacterIdentity)
						{
						case PlayableCharacters.Pilot:
							this.DelayedLoadCustomLevel(delay, "fs_pilot");
							break;
						case PlayableCharacters.Convict:
							this.DelayedLoadCustomLevel(delay, "fs_convict");
							break;
						case PlayableCharacters.Robot:
							this.DelayedLoadCustomLevel(delay, "fs_robot");
							break;
						case PlayableCharacters.Soldier:
							this.DelayedLoadCustomLevel(delay, "fs_soldier");
							break;
						case PlayableCharacters.Guide:
							this.DelayedLoadCustomLevel(delay, "fs_guide");
							break;
						case PlayableCharacters.Bullet:
							this.DelayedLoadCustomLevel(delay, "fs_bullet");
							break;
						case PlayableCharacters.Gunslinger:
							GameManager.IsGunslingerPast = true;
							this.DelayedLoadCustomLevel(delay, "tt_bullethell");
							break;
						}
					}
				}
				else
				{
					this.DelayedLoadCustomLevel(delay, "tt_nakatomi");
				}
			}
			else
			{
				this.DelayedLoadCustomLevel(delay, "tt_cathedral");
			}
		}
		else
		{
			this.DelayedLoadCustomLevel(delay, "tt_sewer");
		}
	}

	// Token: 0x06006D1C RID: 27932 RVA: 0x002AF3A0 File Offset: 0x002AD5A0
	public void GeneratePlayersFromMidGameSave(MidGameSaveData loadedSave)
	{
		GameManager.PlayerPrefabForNewGame = loadedSave.GetPlayerOnePrefab();
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(GameManager.PlayerPrefabForNewGame, Vector3.zero, Quaternion.identity);
		GameManager.PlayerPrefabForNewGame = null;
		gameObject.SetActive(true);
		PlayerController component = gameObject.GetComponent<PlayerController>();
		component.ActorName = "Player ID 0";
		component.PlayerIDX = 0;
		if (loadedSave.playerOneData.CostumeID == 1)
		{
			component.SwapToAlternateCostume(null);
		}
		this.CurrentGameType = loadedSave.savedGameType;
		if (loadedSave != null && loadedSave.playerOneData != null)
		{
			if (loadedSave.playerOneData.passiveItems != null)
			{
				for (int i = 0; i < loadedSave.playerOneData.passiveItems.Count; i++)
				{
					if (loadedSave.playerOneData.passiveItems[i].PickupID == GlobalItemIds.SevenLeafClover)
					{
						PassiveItem.IncrementFlag(component, typeof(SevenLeafCloverItem));
					}
				}
			}
			component.MasteryTokensCollectedThisRun = loadedSave.playerOneData.MasteryTokensCollected;
		}
		this.RefreshAllPlayers();
		if (this.CurrentGameType == GameManager.GameType.COOP_2_PLAYER)
		{
			GameObject gameObject2 = ResourceCache.Acquire("PlayerCoopCultist") as GameObject;
			GameObject gameObject3 = UnityEngine.Object.Instantiate<GameObject>(gameObject2, Vector3.zero, Quaternion.identity);
			GameManager.CoopPlayerPrefabForNewGame = null;
			gameObject3.SetActive(true);
			PlayerController component2 = gameObject3.GetComponent<PlayerController>();
			component2.ActorName = "Player ID 1";
			component2.PlayerIDX = 1;
			if (loadedSave.playerTwoData.CostumeID == 1)
			{
				component2.SwapToAlternateCostume(null);
			}
			if (loadedSave != null && loadedSave.playerTwoData != null)
			{
				if (loadedSave.playerTwoData.passiveItems != null)
				{
					for (int j = 0; j < loadedSave.playerTwoData.passiveItems.Count; j++)
					{
						if (loadedSave.playerTwoData.passiveItems[j].PickupID == GlobalItemIds.SevenLeafClover)
						{
							PassiveItem.IncrementFlag(component2, typeof(SevenLeafCloverItem));
						}
					}
				}
				component2.MasteryTokensCollectedThisRun = loadedSave.playerTwoData.MasteryTokensCollected;
			}
			this.RefreshAllPlayers();
		}
	}

	// Token: 0x06006D1D RID: 27933 RVA: 0x002AF5A4 File Offset: 0x002AD7A4
	public void DelayedLoadCharacterSelect(float delay, bool unloadGameData = false, bool doMainMenu = false)
	{
		base.StartCoroutine(this.DelayedLoadCharacterSelect_CR(delay, unloadGameData, doMainMenu));
	}

	// Token: 0x06006D1E RID: 27934 RVA: 0x002AF5B8 File Offset: 0x002AD7B8
	private IEnumerator DelayedLoadCharacterSelect_CR(float delay, bool unloadGameData, bool doMainMenu)
	{
		if (this.m_loadingLevel)
		{
			yield break;
		}
		this.m_loadingLevel = true;
		float elapsed = 0f;
		while (elapsed < delay)
		{
			elapsed += GameManager.INVARIANT_DELTA_TIME;
			yield return null;
		}
		if (GameUIRoot.Instance != null)
		{
			GameUIRoot.Instance.ToggleUICamera(false);
			yield return null;
		}
		this.LoadCharacterSelect(unloadGameData, doMainMenu);
		yield break;
	}

	// Token: 0x06006D1F RID: 27935 RVA: 0x002AF5E8 File Offset: 0x002AD7E8
	public void ClearPrimaryPlayer()
	{
		if (this.m_player != null)
		{
			UnityEngine.Object.Destroy(this.m_player.gameObject);
		}
		this.m_player = null;
	}

	// Token: 0x06006D20 RID: 27936 RVA: 0x002AF614 File Offset: 0x002AD814
	public void ClearSecondaryPlayer()
	{
		if (this.m_secondaryPlayer != null)
		{
			UnityEngine.Object.Destroy(this.m_secondaryPlayer.gameObject);
		}
		this.m_secondaryPlayer = null;
		this.m_players = null;
	}

	// Token: 0x06006D21 RID: 27937 RVA: 0x002AF648 File Offset: 0x002AD848
	public void ClearPlayers()
	{
		if (this.m_players != null)
		{
			for (int i = 0; i < this.m_players.Length; i++)
			{
				PlayerController playerController = this.m_players[i];
				if (playerController)
				{
					playerController.specRigidbody.PhysicsRegistration = SpeculativeRigidbody.RegistrationState.Unknown;
					UnityEngine.Object.Destroy(playerController.gameObject);
				}
			}
			this.m_players = null;
			this.m_player = null;
			this.m_secondaryPlayer = null;
		}
		else
		{
			if (this.m_player != null)
			{
				this.m_player.specRigidbody.PhysicsRegistration = SpeculativeRigidbody.RegistrationState.Unknown;
				UnityEngine.Object.Destroy(this.m_player.gameObject);
			}
			this.m_player = null;
			this.m_secondaryPlayer = null;
		}
	}

	// Token: 0x06006D22 RID: 27938 RVA: 0x002AF700 File Offset: 0x002AD900
	public void LoadCharacterSelect(bool unloadGameData = false, bool doMainMenu = false)
	{
		if (this.m_paused)
		{
			this.ForceUnpause();
		}
		this.m_loadingLevel = true;
		GameManager.IsReturningToBreach = true;
		this.ClearPerLevelData();
		this.FlushAudio();
		this.ClearActiveGameData(false, true);
		this.m_preventUnpausing = false;
		Foyer.DoIntroSequence = false;
		Foyer.DoMainMenu = doMainMenu;
		GameManager.IsReturningToBreach = true;
		GameManager.SKIP_FOYER = false;
		this.InjectedLevelName = string.Empty;
		this.SetNextLevelIndex(0);
		this.m_preparingToDestroyThisGameManagerOnCollision = true;
		this.LoadNextLevel();
	}

	// Token: 0x06006D23 RID: 27939 RVA: 0x002AF780 File Offset: 0x002AD980
	public void DelayedLoadBossrushFloor(float delay)
	{
		int num = this.nextLevelIndex;
		this.nextLevelIndex++;
		base.StartCoroutine(this.DelayedLoadBossrushFloor_CR(delay, num));
	}

	// Token: 0x06006D24 RID: 27940 RVA: 0x002AF7B4 File Offset: 0x002AD9B4
	public void DebugLoadBossrushFloor(int target)
	{
		base.StartCoroutine(this.DelayedLoadBossrushFloor_CR(0.5f, target));
	}

	// Token: 0x06006D25 RID: 27941 RVA: 0x002AF7CC File Offset: 0x002AD9CC
	private IEnumerator DelayedLoadBossrushFloor_CR(float delay, int bossrushTargetFloor)
	{
		if (this.m_loadingLevel)
		{
			yield break;
		}
		this.m_loadingLevel = true;
		yield return new WaitForSeconds(delay);
		this.InstantLoadBossRushFloor(bossrushTargetFloor);
		yield break;
	}

	// Token: 0x06006D26 RID: 27942 RVA: 0x002AF7F8 File Offset: 0x002AD9F8
	private void InstantLoadBossRushFloor(int bossrushTargetFloor)
	{
		this.m_loadingLevel = true;
		if (this.CurrentGameMode == GameManager.GameMode.SUPERBOSSRUSH)
		{
			switch (bossrushTargetFloor)
			{
			case 1:
				this.LoadCustomFlowForDebug("Bossrush_01_Castle", "Base_Castle", "tt_castle");
				break;
			case 2:
				this.LoadCustomFlowForDebug("Bossrush_01a_Sewer", "Base_Sewer", "tt_sewer");
				break;
			case 3:
				this.LoadCustomFlowForDebug("Bossrush_02_Gungeon", "Base_Gungeon", "tt5");
				break;
			case 4:
				this.LoadCustomFlowForDebug("Bossrush_02a_Cathedral", "Base_Cathedral", "tt_cathedral");
				break;
			case 5:
				this.LoadCustomFlowForDebug("Bossrush_03_Mines", "Base_Mines", "tt_mines");
				break;
			case 6:
				this.LoadCustomFlowForDebug("Bossrush_04_Catacombs", "Base_Catacombs", "tt_catacombs");
				break;
			case 7:
				this.LoadCustomFlowForDebug("Bossrush_05_Forge", "Base_Forge", "tt_forge");
				break;
			case 8:
				this.LoadCustomFlowForDebug("Bossrush_06_BulletHell", "Base_BulletHell", "tt_bullethell");
				break;
			default:
				this.LoadMainMenu();
				break;
			}
		}
		else
		{
			switch (bossrushTargetFloor)
			{
			case 1:
				this.LoadCustomFlowForDebug("Bossrush_01_Castle", "Base_Castle", "tt_castle");
				break;
			case 2:
				this.LoadCustomFlowForDebug("Bossrush_02_Gungeon", "Base_Gungeon", "tt5");
				break;
			case 3:
				this.LoadCustomFlowForDebug("Bossrush_03_Mines", "Base_Mines", "tt_mines");
				break;
			case 4:
				this.LoadCustomFlowForDebug("Bossrush_04_Catacombs", "Base_Catacombs", "tt_catacombs");
				break;
			case 5:
				this.LoadCustomFlowForDebug("Bossrush_05_Forge", "Base_Forge", "tt_forge");
				break;
			default:
				this.LoadMainMenu();
				break;
			}
		}
	}

	// Token: 0x06006D27 RID: 27943 RVA: 0x002AF9D4 File Offset: 0x002ADBD4
	public void DelayedLoadCustomLevel(float delay, string customLevel)
	{
		base.StartCoroutine(this.DelayedLoadCustomLevel_CR(delay, customLevel));
	}

	// Token: 0x06006D28 RID: 27944 RVA: 0x002AF9E8 File Offset: 0x002ADBE8
	private IEnumerator DelayedLoadCustomLevel_CR(float delay, string customLevel)
	{
		if (this.m_loadingLevel)
		{
			yield break;
		}
		this.m_loadingLevel = true;
		yield return new WaitForSeconds(delay);
		this.LoadCustomLevel(customLevel);
		yield break;
	}

	// Token: 0x06006D29 RID: 27945 RVA: 0x002AFA14 File Offset: 0x002ADC14
	public void DelayedLoadNextLevel(float delay)
	{
		base.StartCoroutine(this.DelayedLoadNextLevel_CR(delay));
	}

	// Token: 0x06006D2A RID: 27946 RVA: 0x002AFA24 File Offset: 0x002ADC24
	private IEnumerator DelayedLoadNextLevel_CR(float delay)
	{
		if (this.m_loadingLevel)
		{
			yield break;
		}
		this.m_loadingLevel = true;
		yield return new WaitForSeconds(delay);
		if (GameUIRoot.Instance != null)
		{
			GameUIRoot.Instance.ToggleUICamera(false);
		}
		yield return null;
		this.LoadNextLevel();
		yield break;
	}

	// Token: 0x06006D2B RID: 27947 RVA: 0x002AFA48 File Offset: 0x002ADC48
	private IEnumerator LoadLevelByIndex(int nextIndex)
	{
		SceneManager.LoadScene(this.dungeonFloors[this.nextLevelIndex].dungeonSceneName);
		yield return null;
		this.IsLoadingLevel = false;
		this.nextLevelIndex = nextIndex;
		yield break;
	}

	// Token: 0x06006D2C RID: 27948 RVA: 0x002AFA6C File Offset: 0x002ADC6C
	public void LoadNextLevel()
	{
		if (!string.IsNullOrEmpty(this.InjectedLevelName))
		{
			this.LoadCustomLevel(this.InjectedLevelName);
			this.InjectedLevelName = string.Empty;
			return;
		}
		if (GameManager.SKIP_FOYER && this.nextLevelIndex == 0)
		{
			this.nextLevelIndex = 1;
		}
		if (this.dungeonFloors == null || this.dungeonFloors.Count == 0)
		{
			this.dungeonFloors = new List<GameLevelDefinition>();
			GameLevelDefinition gameLevelDefinition = new GameLevelDefinition();
			gameLevelDefinition.dungeonSceneName = SceneManager.GetActiveScene().name;
			this.dungeonFloors.Add(gameLevelDefinition);
		}
		if (this.nextLevelIndex >= this.dungeonFloors.Count)
		{
			this.nextLevelIndex = 0;
		}
		this.m_loadingLevel = true;
		this.ClearPerLevelData();
		if (this.dungeonFloors[this.nextLevelIndex].dungeonPrefabPath == string.Empty)
		{
			if (this.dungeonFloors[this.nextLevelIndex].dungeonSceneName == "MainMenu")
			{
				this.LoadMainMenu();
				this.nextLevelIndex = 0;
			}
			else if (this.dungeonFloors[this.nextLevelIndex].dungeonSceneName == "Foyer")
			{
				base.StartCoroutine(this.LoadLevelByIndex(this.nextLevelIndex + 1));
			}
			else
			{
				base.StartCoroutine(this.LoadLevelByIndex(0));
			}
		}
		else
		{
			base.StartCoroutine(this.LoadNextLevelAsync_CR(this.dungeonFloors[this.nextLevelIndex]));
			this.nextLevelIndex++;
		}
	}

	// Token: 0x06006D2D RID: 27949 RVA: 0x002AFC0C File Offset: 0x002ADE0C
	public void DoGameOver(string gameOverSource = "")
	{
		this.PauseRaw(true);
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		AmmonomiconController.Instance.OpenAmmonomicon(true, false);
	}

	// Token: 0x06006D2E RID: 27950 RVA: 0x002AFC2C File Offset: 0x002ADE2C
	private IEnumerator LoadGameOver_CR(string gameOverSource)
	{
		BraveTime.ClearAllMultipliers();
		SceneManager.LoadScene("GameOver");
		while (BraveUtility.isLoadingLevel)
		{
			yield return null;
		}
		GameObject gameOverTextObject = GameObject.Find("GameOverTextLabel");
		dfLabel gameOverTextLabel = gameOverTextObject.GetComponent<dfLabel>();
		if (string.IsNullOrEmpty(gameOverSource))
		{
			gameOverTextLabel.Text = "You died.";
		}
		else
		{
			gameOverTextLabel.Text = "You were killed by: " + gameOverSource + " ";
		}
		gameOverTextLabel.Invalidate();
		this.IsLoadingLevel = false;
		yield break;
	}

	// Token: 0x06006D2F RID: 27951 RVA: 0x002AFC50 File Offset: 0x002ADE50
	public GameLevelDefinition GetLevelDefinitionFromName(string levelName)
	{
		for (int i = 0; i < this.dungeonFloors.Count; i++)
		{
			if (this.dungeonFloors[i].dungeonSceneName == levelName)
			{
				return this.dungeonFloors[i];
			}
		}
		for (int j = 0; j < this.customFloors.Count; j++)
		{
			if (this.customFloors[j].dungeonSceneName == levelName)
			{
				return this.customFloors[j];
			}
		}
		return null;
	}

	// Token: 0x06006D30 RID: 27952 RVA: 0x002AFCE8 File Offset: 0x002ADEE8
	public GameLevelDefinition GetLastLoadedLevelDefinition()
	{
		if (this.m_lastLoadedLevelDefinition == null)
		{
			return this.GetLevelDefinitionFromName(SceneManager.GetActiveScene().name);
		}
		return this.m_lastLoadedLevelDefinition;
	}

	// Token: 0x06006D31 RID: 27953 RVA: 0x002AFD1C File Offset: 0x002ADF1C
	private IEnumerator EndLoadNextLevelAsync_CR(AsyncOperation async, GameObject loadingSceneHierarchy, bool isHandoff = false)
	{
		this.IsLoadingLevel = true;
		while (!async.isDone || !async.allowSceneActivation)
		{
			yield return null;
		}
		for (int i = 0; i < this.BraveLevelLoadedListeners.Length; i++)
		{
			Type type = this.BraveLevelLoadedListeners[i];
			UnityEngine.Object[] array = UnityEngine.Object.FindObjectsOfType(type);
			for (int j = 0; j < array.Length; j++)
			{
				ILevelLoadedListener levelLoadedListener = array[j] as ILevelLoadedListener;
				if (levelLoadedListener != null)
				{
					levelLoadedListener.BraveOnLevelWasLoaded();
				}
			}
		}
		yield return null;
		while (Dungeon.IsGenerating)
		{
			yield return null;
		}
		if (this.m_player != null)
		{
			this.m_player.BraveOnLevelWasLoaded();
		}
		if (this.m_secondaryPlayer != null)
		{
			this.m_secondaryPlayer.BraveOnLevelWasLoaded();
		}
		AmmonomiconController.EnsureExistence();
		yield return null;
		Image fadeImage = null;
		float temporo = 0.15f;
		GameObject canvasObj = loadingSceneHierarchy.transform.Find("FadeCanvas").gameObject;
		fadeImage = canvasObj.GetComponentInChildren<Image>();
		canvasObj.transform.SetParent(null);
		float elapsed = 0f;
		while (elapsed < temporo)
		{
			elapsed += GameManager.INVARIANT_DELTA_TIME;
			float t = elapsed / temporo;
			if (fadeImage != null && fadeImage)
			{
				fadeImage.color = new Color(0f, 0f, 0f, t);
			}
			yield return null;
		}
		this.FlushAudio();
		UnityEngine.Object.Destroy(loadingSceneHierarchy);
		BraveTime.ClearAllMultipliers();
		yield return null;
		this.IsLoadingLevel = false;
		if (this.OnNewLevelFullyLoaded != null)
		{
			this.OnNewLevelFullyLoaded();
		}
		float defadeElapsed = 0f;
		while (defadeElapsed < temporo)
		{
			defadeElapsed += GameManager.INVARIANT_DELTA_TIME;
			float t2 = defadeElapsed / temporo;
			fadeImage.color = new Color(0f, 0f, 0f, 1f - t2);
			yield return null;
		}
		UnityEngine.Object.Destroy(fadeImage.transform.parent.gameObject);
		yield break;
	}

	// Token: 0x06006D32 RID: 27954 RVA: 0x002AFD48 File Offset: 0x002ADF48
	private IEnumerator LoadNextLevelAsync_CR(GameLevelDefinition gld)
	{
		SceneManager.LoadScene("LoadingDungeon");
		if (Time.timeScale != 0f)
		{
			BraveTime.ClearAllMultipliers();
		}
		if (this.m_preventUnpausing)
		{
			this.m_preventUnpausing = false;
		}
		if (this.m_paused)
		{
			this.m_paused = false;
		}
		while (BraveUtility.isLoadingLevel)
		{
			yield return null;
		}
		yield return null;
		AsyncOperation async;
		if (gld.dungeonSceneName == "tt_foyer")
		{
			AssetBundle assetBundle = ResourceManager.LoadAssetBundle("foyer_001");
			async = ResourceManager.LoadSceneAsyncFromBundle(assetBundle, LoadSceneMode.Additive);
		}
		else
		{
			AssetBundle assetBundle2 = ResourceManager.LoadAssetBundle("dungeon_scene_001");
			async = ResourceManager.LoadSceneAsyncFromBundle(assetBundle2, LoadSceneMode.Additive);
		}
		async.allowSceneActivation = false;
		if (gld.dungeonSceneName == "tt_foyer")
		{
			this.IsFoyer = true;
		}
		this.m_lastLoadedLevelDefinition = gld;
		GameManager.BackgroundGenerationActive = false;
		gld.lastSelectedFlowEntry = null;
		if (!string.IsNullOrEmpty(gld.dungeonPrefabPath))
		{
			this.CurrentlyGeneratingDungeonPrefab = DungeonDatabase.GetOrLoadByName(gld.dungeonPrefabPath);
		}
		if (this.CurrentlyGeneratingDungeonPrefab != null)
		{
			int dungeonSeed = this.CurrentlyGeneratingDungeonPrefab.GetDungeonSeed();
			UnityEngine.Random.InitState(dungeonSeed);
			BraveRandom.InitializeWithSeed(dungeonSeed);
			DungeonFlowLevelEntry flowEntry = null;
			DungeonFlow targetFlow = null;
			if (!string.IsNullOrEmpty(this.InjectedFlowPath))
			{
				targetFlow = FlowDatabase.GetOrLoadByName(this.InjectedFlowPath);
				this.InjectedFlowPath = null;
			}
			if (gld.flowEntries.Count > 0)
			{
				flowEntry = gld.LovinglySelectDungeonFlow();
				if (flowEntry != null)
				{
					DungeonFlow dungeonFlow = FlowDatabase.GetOrLoadByName(flowEntry.flowPath);
					if (dungeonFlow == null)
					{
						dungeonFlow = FlowDatabase.GetOrLoadByName("Boss Rooms/" + flowEntry.flowPath);
					}
					if (dungeonFlow == null)
					{
						dungeonFlow = FlowDatabase.GetOrLoadByName("Boss Rush Flows/" + flowEntry.flowPath);
					}
					if (dungeonFlow == null)
					{
						dungeonFlow = FlowDatabase.GetOrLoadByName("Testing/" + flowEntry.flowPath);
					}
					if (targetFlow == null)
					{
						targetFlow = dungeonFlow;
					}
				}
			}
			LoopDungeonGenerator ldg = new LoopDungeonGenerator(this.CurrentlyGeneratingDungeonPrefab, dungeonSeed);
			if (targetFlow != null)
			{
				ldg.AssignFlow(targetFlow);
			}
			gld.lastSelectedFlowEntry = flowEntry;
			IEnumerator tracker = ldg.GenerateDungeonLayoutDeferred().GetEnumerator();
			while (tracker.MoveNext())
			{
				yield return null;
			}
			AkSoundEngine.PostEvent("Stop_Foyer_Fade_01", GameManager.Instance.gameObject);
			this.PregeneratedDungeonData = ldg.DeferredGeneratedData;
			this.DungeonToAutoLoad = this.CurrentlyGeneratingDungeonPrefab;
			this.CurrentlyGeneratingDungeonPrefab = null;
		}
		else
		{
			for (int i = 0; i < this.AllPlayers.Length; i++)
			{
				if (this.AllPlayers[i])
				{
					UnityEngine.Object.Destroy(this.AllPlayers[i].gameObject);
				}
			}
		}
		GameObject loadingSceneHierarchy = GameObject.Find("LoadingMonster");
		BraveTime.RegisterTimeScaleMultiplier(0f, base.gameObject);
		if (this.m_preparingToDestroyThisGameManagerOnCollision)
		{
			this.m_shouldDestroyThisGameManagerOnCollision = true;
			this.m_preDestroyAsyncHolder = async;
			this.m_preDestroyLoadingHierarchyHolder = loadingSceneHierarchy;
		}
		async.allowSceneActivation = true;
		if (!this.m_shouldDestroyThisGameManagerOnCollision)
		{
			yield return base.StartCoroutine(this.EndLoadNextLevelAsync_CR(async, loadingSceneHierarchy, false));
		}
		yield break;
	}

	// Token: 0x06006D33 RID: 27955 RVA: 0x002AFD6C File Offset: 0x002ADF6C
	public void Pause()
	{
		Minimap.Instance.ToggleMinimap(false, false);
		GameUIRoot.Instance.ShowPauseMenu();
		BraveMemory.HandleGamePaused();
		GameStatsManager.Instance.MoveSessionStatsToSavedSessionStats();
		this.PauseRaw(false);
		if (GameManager.Options.CurrentFullscreenStyle == GameOptions.FullscreenStyle.BORDERLESS)
		{
			GameCursorController component = GameUIRoot.Instance.GetComponent<GameCursorController>();
			if (component != null)
			{
				component.ToggleClip(false);
			}
		}
		base.StartCoroutine(this.PixelateCR());
	}

	// Token: 0x06006D34 RID: 27956 RVA: 0x002AFDE4 File Offset: 0x002ADFE4
	public void PauseRaw(bool preventUnpausing = false)
	{
		GameUIRoot.Instance.levelNameUI.BanishLevelNameText();
		GameUIRoot.Instance.ForceClearReload(-1);
		GameUIRoot.Instance.ToggleLowerPanels(false, false, "gm_pause");
		GameUIRoot.Instance.HideCoreUI("gm_pause");
		GameUIRoot.Instance.ToggleAllDefaultLabels(false, "pause");
		BraveTime.RegisterTimeScaleMultiplier(0f, base.gameObject);
		if (!this.IsSelectingCharacter)
		{
			if (!this.MainCameraController.ManualControl)
			{
				this.MainCameraController.OverridePosition = this.MainCameraController.transform.position;
				this.MainCameraController.SetManualControl(true, false);
				this.m_pauseLockedCamera = true;
			}
			else
			{
				this.m_pauseLockedCamera = false;
			}
		}
		if (preventUnpausing)
		{
			this.m_preventUnpausing = true;
		}
		this.m_paused = true;
	}

	// Token: 0x06006D35 RID: 27957 RVA: 0x002AFEB8 File Offset: 0x002AE0B8
	public void ReturnToBasePauseState()
	{
		GameUIRoot.Instance.ReturnToBasePause();
	}

	// Token: 0x06006D36 RID: 27958 RVA: 0x002AFEC4 File Offset: 0x002AE0C4
	public void Unpause()
	{
		this.m_paused = false;
		this.m_unpausedThisFrame = true;
		if (this.m_pauseLockedCamera)
		{
			this.MainCameraController.SetManualControl(false, true);
		}
		GameUIRoot.Instance.ToggleLowerPanels(true, false, "gm_pause");
		GameUIRoot.Instance.ShowCoreUI("gm_pause");
		GameUIRoot.Instance.HidePauseMenu();
		GameUIRoot.Instance.ToggleAllDefaultLabels(true, "pause");
		BraveInput.FlushAll();
		if (this.AllPlayers != null)
		{
			for (int i = 0; i < this.AllPlayers.Length; i++)
			{
				if (this.AllPlayers[i])
				{
					this.AllPlayers[i].WasPausedThisFrame = true;
				}
			}
		}
		if (GameManager.Options.CurrentFullscreenStyle == GameOptions.FullscreenStyle.BORDERLESS)
		{
			GameCursorController component = GameUIRoot.Instance.GetComponent<GameCursorController>();
			if (component != null)
			{
				component.ToggleClip(true);
			}
		}
		if (Pixelator.Instance.saturation != 1f)
		{
			base.StartCoroutine(this.DepixelateCR());
		}
	}

	// Token: 0x06006D37 RID: 27959 RVA: 0x002AFFCC File Offset: 0x002AE1CC
	public void ForceUnpause()
	{
		this.m_paused = false;
		if (this.MainCameraController && this.m_pauseLockedCamera)
		{
			this.MainCameraController.SetManualControl(false, true);
		}
		if (GameUIRoot.Instance != null)
		{
			GameUIRoot.Instance.ToggleLowerPanels(true, false, "gm_pause");
			GameUIRoot.Instance.ShowCoreUI("gm_pause");
			GameUIRoot.Instance.HidePauseMenu();
		}
		BraveInput.FlushAll();
		if (Pixelator.Instance != null)
		{
			GameManager.Options.OverrideMotionEnhancementModeForPause = false;
			Pixelator.Instance.OnChangedMotionEnhancementMode(GameManager.Options.MotionEnhancementMode);
			Pixelator.Instance.overrideTileScale = 1;
			Pixelator.Instance.saturation = 1f;
		}
		BraveTime.ClearMultiplier(base.gameObject);
	}

	// Token: 0x06006D38 RID: 27960 RVA: 0x002B009C File Offset: 0x002AE29C
	private IEnumerator PixelateCR()
	{
		float elapsed = 0f;
		float duration = 0.15f;
		GameManager.Options.OverrideMotionEnhancementModeForPause = true;
		Pixelator.Instance.OnChangedMotionEnhancementMode(GameManager.Options.MotionEnhancementMode);
		while (elapsed < duration && this.m_paused)
		{
			elapsed += GameManager.m_deltaTime;
			Pixelator.Instance.saturation = 1f - Mathf.Clamp01(elapsed / duration);
			Pixelator.Instance.SetFadeColor(Color.black);
			Pixelator.Instance.fade = 1f - 1f * Mathf.Clamp01(elapsed / duration);
			yield return null;
		}
		yield break;
	}

	// Token: 0x06006D39 RID: 27961 RVA: 0x002B00B8 File Offset: 0x002AE2B8
	private IEnumerator DepixelateCR()
	{
		float elapsed = 0f;
		float duration = 0.075f;
		while (elapsed < 0.05f)
		{
			elapsed += GameManager.m_deltaTime;
			yield return null;
		}
		elapsed = 0f;
		while (elapsed < duration && !this.m_paused)
		{
			elapsed += GameManager.m_deltaTime;
			Pixelator.Instance.saturation = Mathf.Clamp01(elapsed / duration);
			Pixelator.Instance.fade = 1f * Mathf.Clamp01(elapsed / duration);
			yield return null;
		}
		GameManager.Options.OverrideMotionEnhancementModeForPause = false;
		Pixelator.Instance.OnChangedMotionEnhancementMode(GameManager.Options.MotionEnhancementMode);
		Pixelator.Instance.overrideTileScale = 1;
		Pixelator.Instance.saturation = 1f;
		Pixelator.Instance.fade = 1f;
		BraveTime.ClearMultiplier(base.gameObject);
		yield break;
	}

	// Token: 0x06006D3A RID: 27962 RVA: 0x002B00D4 File Offset: 0x002AE2D4
	public static void AttemptSoundEngineInitialization()
	{
		if (GameManager.AUDIO_ENABLED)
		{
			return;
		}
		DebugTime.RecordStartTime();
		uint num;
		AkSoundEngine.LoadBank("SFX.bnk", -1, out num);
		DebugTime.Log("GameManager.ASEI.LoadBank(SFX)", new object[0]);
		UnityEngine.Debug.LogError("loaded bank id: " + num);
		AkChannelConfig akChannelConfig = new AkChannelConfig();
		akChannelConfig.SetStandard(uint.MaxValue);
		AkSoundEngine.SetListenerSpatialization(null, true, akChannelConfig);
		GameManager.AUDIO_ENABLED = true;
	}

	// Token: 0x06006D3B RID: 27963 RVA: 0x002B0140 File Offset: 0x002AE340
	public static void AttemptSoundEngineInitializationAsync()
	{
		GameManager.c_asyncSoundStartTime = Time.realtimeSinceStartup;
		GameManager.c_asyncSoundStartFrame = Time.frameCount;
		uint num;
		AkSoundEngine.LoadBank("SFX.bnk", new AkCallbackManager.BankCallback(GameManager.BankCallback), null, -1, out num);
		UnityEngine.Debug.LogError("async loading bank id: " + num);
	}

	// Token: 0x06006D3C RID: 27964 RVA: 0x002B01A4 File Offset: 0x002AE3A4
	private static void BankCallback(uint in_bankID, IntPtr in_InMemoryBankPtr, AKRESULT in_eLoadResult, uint in_memPoolId, object in_Cookie)
	{
		DebugTime.Log(GameManager.c_asyncSoundStartTime, GameManager.c_asyncSoundStartFrame, "GameManager.ASEI.LoadBank(SFX)", new object[0]);
		AkChannelConfig akChannelConfig = new AkChannelConfig();
		akChannelConfig.SetStandard(uint.MaxValue);
		AkSoundEngine.SetListenerSpatialization(null, true, akChannelConfig);
		GameManager.AUDIO_ENABLED = true;
	}

	// Token: 0x06006D3D RID: 27965 RVA: 0x002B01E8 File Offset: 0x002AE3E8
	protected void LoadDungeonFloorsFromTargetPrefab(string resourcePath, bool assignNextLevelIndex)
	{
		GameManager component = (BraveResources.Load(resourcePath, ".prefab") as GameObject).GetComponent<GameManager>();
		this.GlobalInjectionData = component.GlobalInjectionData;
		this.CurrentRewardManager = component.CurrentRewardManager;
		this.OriginalRewardManager = component.OriginalRewardManager;
		this.SynergyManager = component.SynergyManager;
		this.DefaultAlienConversationFont = component.DefaultAlienConversationFont;
		this.DefaultNormalConversationFont = component.DefaultNormalConversationFont;
		this.dungeonFloors = component.dungeonFloors;
		this.customFloors = component.customFloors;
		if (assignNextLevelIndex)
		{
			for (int i = 0; i < this.dungeonFloors.Count; i++)
			{
				if (SceneManager.GetActiveScene().name == this.dungeonFloors[i].dungeonSceneName)
				{
					this.nextLevelIndex = i + 1;
					break;
				}
			}
		}
		this.COOP_ENEMY_HEALTH_MULTIPLIER = component.COOP_ENEMY_HEALTH_MULTIPLIER;
		this.COOP_ENEMY_PROJECTILE_SPEED_MULTIPLIER = component.COOP_ENEMY_PROJECTILE_SPEED_MULTIPLIER;
		this.PierceDamageScaling = component.PierceDamageScaling;
		this.BloodthirstOptions = component.BloodthirstOptions;
		this.EnemyReplacementTiers = component.EnemyReplacementTiers;
	}

	// Token: 0x06006D3E RID: 27966 RVA: 0x002B0300 File Offset: 0x002AE500
	public void InitializeForRunWithSeed(int seed)
	{
		this.CurrentRunSeed = seed;
	}

	// Token: 0x06006D3F RID: 27967 RVA: 0x002B030C File Offset: 0x002AE50C
	private void Awake()
	{
		DebugTime.Log("GameManager.Awake()", new object[0]);
		base.gameObject.AddComponent<EarlyUpdater>();
		if (!Application.isEditor && !GameManager.m_hasEnsuredHeapSize && SystemInfo.systemMemorySize > 1000)
		{
			if (SystemInfo.systemMemorySize > 3500)
			{
				BraveMemory.EnsureHeapSize(204800);
				GameManager.m_hasEnsuredHeapSize = true;
			}
			else
			{
				BraveMemory.EnsureHeapSize(102400);
				GameManager.m_hasEnsuredHeapSize = true;
			}
		}
		try
		{
			UnityEngine.Debug.Log("Version: " + VersionManager.UniqueVersionNumber);
			UnityEngine.Debug.LogFormat("Now: {0:MM.dd.yyyy HH:mm}", new object[] { DateTime.Now });
		}
		catch (Exception)
		{
		}
		if (this.platformInterface == null)
		{
			if (PlatformInterfaceSteam.IsSteamBuild())
			{
				this.platformInterface = new PlatformInterfaceSteam();
			}
			else if (PlatformInterfaceGalaxy.IsGalaxyBuild())
			{
				this.platformInterface = new PlatformInterfaceGalaxy();
			}
			else
			{
				this.platformInterface = new PlatformInterfaceGenericPC();
			}
		}
		this.platformInterface.Start();
		if (GameManager.Options == null)
		{
			GameOptions.Load();
		}
		string text = "_GameManager";
		DebugTime.RecordStartTime();
		if (this.dungeonFloors == null)
		{
			this.dungeonFloors = new List<GameLevelDefinition>();
			GameManager component = (BraveResources.Load(text, ".prefab") as GameObject).GetComponent<GameManager>();
			this.GlobalInjectionData = component.GlobalInjectionData;
			this.CurrentRewardManager = component.RewardManager;
			this.SynergyManager = component.SynergyManager;
			this.DefaultAlienConversationFont = component.DefaultAlienConversationFont;
			this.DefaultNormalConversationFont = component.DefaultNormalConversationFont;
		}
		DebugTime.Log("GameManager.Awake() load dungeon floors", new object[0]);
		GameManager[] array = UnityEngine.Object.FindObjectsOfType<GameManager>();
		if (array.Length > 1)
		{
			GameManager gameManager = null;
			GameManager gameManager2 = null;
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i] && (gameManager == null || array[i].dungeonFloors.Count > gameManager.dungeonFloors.Count) && !array[i].m_shouldDestroyThisGameManagerOnCollision)
				{
					gameManager = array[i];
				}
				if (array[i].m_shouldDestroyThisGameManagerOnCollision)
				{
					gameManager2 = array[i];
				}
			}
			if (gameManager != null && gameManager2 != null)
			{
				UnityEngine.Debug.Log("continuing from where my father left off");
				if (!GameManager.IsReturningToFoyerWithPlayer)
				{
					this.IsSelectingCharacter = true;
				}
				gameManager.StartCoroutine(gameManager.EndLoadNextLevelAsync_CR(gameManager2.m_preDestroyAsyncHolder, gameManager2.m_preDestroyLoadingHierarchyHolder, true));
			}
			for (int j = 0; j < array.Length; j++)
			{
				if (array[j] != gameManager)
				{
					UnityEngine.Object.Destroy(array[j].gameObject);
				}
			}
			GameManager.mr_manager = gameManager;
			if (!this)
			{
				return;
			}
		}
		if (GameManager.m_inputManager == null)
		{
			InControlManager inControlManager = UnityEngine.Object.FindObjectOfType<InControlManager>();
			if (inControlManager)
			{
				GameManager.m_inputManager = inControlManager;
				UnityEngine.Object.DontDestroyOnLoad(GameManager.m_inputManager.gameObject);
				InputManager.Enabled = true;
			}
			else
			{
				GameObject gameObject = new GameObject("_InputManager");
				GameManager.m_inputManager = gameObject.AddComponent<InControlManager>();
				UnityEngine.Object.DontDestroyOnLoad(gameObject);
				InputManager.Enabled = true;
			}
		}
		if (this.m_dungeonMusicController == null)
		{
			this.m_dungeonMusicController = base.GetComponent<DungeonFloorMusicController>();
			if (!this.m_dungeonMusicController)
			{
				this.m_dungeonMusicController = base.gameObject.AddComponent<DungeonFloorMusicController>();
			}
		}
		DebugTime.RecordStartTime();
		GameStatsManager.Load();
		DebugTime.Log("GameManager.Awake() load game stats", new object[0]);
		if (!GameManager.AUDIO_ENABLED)
		{
			GameManager.AttemptSoundEngineInitialization();
		}
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		UnityEngine.Debug.Log("Post GameManager.Awake.AudioInit");
		if (GameStatsManager.Instance.IsInSession)
		{
			this.StartEncounterableUnlockedChecks();
		}
		if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.LinuxPlayer || Application.platform == RuntimePlatform.MetroPlayerX64 || Application.platform == RuntimePlatform.MetroPlayerX86)
		{
			GameManager.LoadResolutionFromOptions();
		}
		else if (Application.platform == RuntimePlatform.PS4)
		{
			GameManager.LoadResolutionFromPS4();
		}
		UnityEngine.Debug.Log("Post GameManager.Awake.Resolution");
		StringTableManager.LoadTablesIfNecessary();
		this.RandomIntForCurrentRun = UnityEngine.Random.Range(0, 1000);
		UnityEngine.Debug.Log("Terminating GameManager Awake()");
	}

	// Token: 0x06006D40 RID: 27968 RVA: 0x002B075C File Offset: 0x002AE95C
	private IEnumerator Start()
	{
		DebugTime.Log("GameManager.Start()", new object[0]);
		GameManager.Options.MusicVolume = GameManager.Options.MusicVolume;
		GameManager.Options.SoundVolume = GameManager.Options.SoundVolume;
		GameManager.Options.UIVolume = GameManager.Options.UIVolume;
		GameManager.Options.AudioHardware = GameManager.Options.AudioHardware;
		Gun.s_DualWieldFactor = this.DUAL_WIELDING_DAMAGE_FACTOR;
		yield return null;
		if (GameOptions.RequiresLanguageReinitialization)
		{
			GameManager.Options.CurrentLanguage = this.platformInterface.GetPreferredLanguage();
			GameOptions.RequiresLanguageReinitialization = false;
		}
		if (!this.m_initializedDeviceCallbacks)
		{
			UnityInputDeviceManager.OnAllDevicesReattached = (Action)Delegate.Combine(UnityInputDeviceManager.OnAllDevicesReattached, new Action(this.HandleDeviceShift));
		}
		if (GameManager.Options.CurrentLanguage != StringTableManager.GungeonSupportedLanguages.ENGLISH)
		{
			StringTableManager.CurrentLanguage = GameManager.Options.CurrentLanguage;
		}
		yield break;
	}

	// Token: 0x06006D41 RID: 27969 RVA: 0x002B0778 File Offset: 0x002AE978
	private void HandleDeviceShift()
	{
		this.m_initializedDeviceCallbacks = true;
		BraveInput.ReassignAllControllers(null);
	}

	// Token: 0x06006D42 RID: 27970 RVA: 0x002B0788 File Offset: 0x002AE988
	private void OnEnable()
	{
		GameManager.m_lastFrameRealtime = Time.realtimeSinceStartup;
	}

	// Token: 0x06006D43 RID: 27971 RVA: 0x002B0794 File Offset: 0x002AE994
	protected override void OnDestroy()
	{
		base.OnDestroy();
		Shader.SetGlobalFloat("_MeduziReflectionsEnabled", 0f);
		GameManager[] array = UnityEngine.Object.FindObjectsOfType<GameManager>();
		if (array.Length < 1 || (array.Length == 1 && array[0] == this))
		{
			if (GameStatsManager.Instance.IsInSession)
			{
				GameStatsManager.Instance.EndSession(true, true);
			}
			if (SaveManager.ResetSaveSlot)
			{
				GameStatsManager.DANGEROUS_ResetAllStats();
			}
			GameStatsManager.Save();
			if (GameManager.Options != null)
			{
				GameOptions.Save();
			}
			UnityEngine.Debug.LogWarning("SAVING DATA");
			GameManager.Options = null;
			if (SaveManager.TargetSaveSlot != null)
			{
				SaveManager.ChangeSlot(SaveManager.TargetSaveSlot.Value);
				SaveManager.TargetSaveSlot = null;
			}
		}
		UnityInputDeviceManager.OnAllDevicesReattached = (Action)Delegate.Remove(UnityInputDeviceManager.OnAllDevicesReattached, new Action(this.HandleDeviceShift));
	}

	// Token: 0x06006D44 RID: 27972 RVA: 0x002B0878 File Offset: 0x002AEA78
	protected void InvariantUpdate(float realDeltaTime)
	{
		if (!this.m_bgChecksActive && GameStatsManager.Instance.IsInSession)
		{
			this.StartEncounterableUnlockedChecks();
		}
		if (this.m_dungeon != null && !this.m_preventUnpausing && !this.IsLoadingLevel && (!Foyer.DoMainMenu || !this.IsFoyer))
		{
			int num = this.AllPlayers.Length;
			if (this.IsSelectingCharacter)
			{
				num = 1;
			}
			for (int i = 0; i < num; i++)
			{
				int num2 = ((!this.IsSelectingCharacter) ? this.AllPlayers[i].PlayerIDX : 0);
				BraveInput braveInput = ((!this.IsSelectingCharacter) ? BraveInput.GetInstanceForPlayer(num2) : BraveInput.PlayerlessInstance);
				if (!(braveInput == null) && braveInput.ActiveActions != null)
				{
					bool flag = braveInput.ActiveActions.PauseAction.WasPressed;
					if (Minimap.HasInstance && Minimap.Instance.HeldOpen)
					{
						flag = false;
					}
					if (braveInput.ActiveActions.EquipmentMenuAction.WasPressed)
					{
						bool flag2 = this.IsSelectingCharacter && Foyer.IsCurrentlyPlayingCharacterSelect;
						if (!this.m_paused && !AmmonomiconController.Instance.IsOpen && !flag2)
						{
							this.LastPausingPlayerID = num2;
							this.Pause();
							GameUIRoot.Instance.PauseMenuPanel.GetComponent<PauseMenuController>().DoShowBestiary(null, null);
						}
						else if (!this.m_paused || AmmonomiconController.Instance.IsOpen)
						{
							if (this.m_paused && AmmonomiconController.Instance.IsOpen && !AmmonomiconController.Instance.IsClosing && !AmmonomiconController.Instance.IsOpening)
							{
								AmmonomiconController.Instance.CloseAmmonomicon(false);
								this.ReturnToBasePauseState();
								dfGUIManager.PushModal(GameUIRoot.Instance.PauseMenuPanel);
								this.Unpause();
							}
						}
					}
					else if (flag)
					{
						if (this.m_paused)
						{
							if (!GameUIRoot.Instance.AreYouSurePanel.IsVisible && !GameUIRoot.Instance.KeepMetasIsVisible)
							{
								if (GameUIRoot.Instance.HasOpenPauseSubmenu())
								{
									PauseMenuController component = GameUIRoot.Instance.PauseMenuPanel.GetComponent<PauseMenuController>();
									if (!component.OptionsMenu.ModalKeyBindingDialog.IsVisible)
									{
										if (component.OptionsMenu.IsVisible || component.OptionsMenu.ModalKeyBindingDialog.IsVisible)
										{
											component.OptionsMenu.CloseAndMaybeApplyChangesWithPrompt();
										}
										else
										{
											component.ForceMaterialVisibility();
											this.ReturnToBasePauseState();
										}
									}
								}
								else if (AmmonomiconController.Instance.IsOpen)
								{
									if (!AmmonomiconController.Instance.IsTurningPage && !AmmonomiconController.Instance.IsOpening && !AmmonomiconController.Instance.IsClosing)
									{
										AmmonomiconController.Instance.CloseAmmonomicon(false);
										this.ReturnToBasePauseState();
										dfGUIManager.PushModal(GameUIRoot.Instance.PauseMenuPanel);
									}
								}
								else
								{
									this.Unpause();
								}
							}
						}
						else
						{
							this.LastPausingPlayerID = num2;
							this.Pause();
						}
					}
					else if (this.m_paused && braveInput.ActiveActions.CancelAction.WasPressed)
					{
						if (!GameUIRoot.Instance.AreYouSurePanel.IsVisible && !GameUIRoot.Instance.KeepMetasIsVisible)
						{
							if (AmmonomiconController.Instance.IsOpen && !AmmonomiconController.Instance.IsClosing && !AmmonomiconController.Instance.BookmarkHasFocus)
							{
								AmmonomiconController.Instance.ReturnFocusToBookmark();
							}
							else if (GameUIRoot.Instance.HasOpenPauseSubmenu())
							{
								PauseMenuController component2 = GameUIRoot.Instance.PauseMenuPanel.GetComponent<PauseMenuController>();
								if (!component2.OptionsMenu.ModalKeyBindingDialog.IsVisible)
								{
									if (component2.OptionsMenu.IsVisible || component2.OptionsMenu.ModalKeyBindingDialog.IsVisible)
									{
										component2.OptionsMenu.CloseAndMaybeApplyChangesWithPrompt();
									}
									else
									{
										this.ReturnToBasePauseState();
									}
								}
							}
							else if (AmmonomiconController.Instance.IsOpen && !AmmonomiconController.Instance.IsClosing)
							{
								if (!AmmonomiconController.Instance.IsTurningPage)
								{
									AmmonomiconController.Instance.CloseAmmonomicon(false);
									this.ReturnToBasePauseState();
									dfGUIManager.PushModal(GameUIRoot.Instance.PauseMenuPanel);
								}
							}
							else
							{
								this.Unpause();
							}
						}
					}
				}
			}
		}
	}

	// Token: 0x06006D45 RID: 27973 RVA: 0x002B0D1C File Offset: 0x002AEF1C
	public void FlushMusicAudio()
	{
		AkSoundEngine.PostEvent("Stop_MUS_All", base.gameObject);
		if (this.m_dungeonMusicController)
		{
			this.m_dungeonMusicController.ClearCoreMusicEventID();
		}
	}

	// Token: 0x06006D46 RID: 27974 RVA: 0x002B0D4C File Offset: 0x002AEF4C
	public void FlushAudio()
	{
		AkSoundEngine.PostEvent("Stop_SND_All", base.gameObject);
		AkSoundEngine.ClearPreparedEvents();
		AkSoundEngine.StopAll();
		if (this.m_dungeonMusicController)
		{
			this.m_dungeonMusicController.ClearCoreMusicEventID();
		}
	}

	// Token: 0x06006D47 RID: 27975 RVA: 0x002B0D88 File Offset: 0x002AEF88
	public void ClearPerLevelData()
	{
		BraveCameraUtility.OverrideAspect = null;
		SuperReaperController.PreventShooting = false;
		BossKillCam.BossDeathCamRunning = false;
		PickupObject.ItemIsBeingTakenByRat = false;
		this.LastUsedInputDeviceForConversation = null;
		BossManager.PriorFloorSelectedBossRoom = null;
		if (GameManager.Instance.Dungeon != null)
		{
			for (int i = 0; i < GameManager.Instance.Dungeon.data.rooms.Count; i++)
			{
				GameManager.Instance.Dungeon.data.rooms[i].SetRoomActive(true);
			}
		}
		this.m_dungeon = null;
		this.m_camera = null;
		GameUIRoot.Instance = null;
		if (this.m_player != null)
		{
			this.m_player.ClearPerLevelData();
		}
		CheckEntireFloorVisited.IsQuestCallbackActive = false;
		SunglassesItem.SunglassesActive = false;
		AmmonomiconController.Instance = null;
		TileVFXManager.Instance = null;
		this.InTutorial = false;
		BossKillCam.ClearPerLevelData();
		LootEngine.ClearPerLevelData();
		RoomHandler.unassignedInteractableObjects.Clear();
		ShadowSystem.ClearPerLevelData();
		SecretRoomUtility.ClearPerLevelData();
		DeadlyDeadlyGoopManager.ClearPerLevelData();
		BroController.ClearPerLevelData();
		GlobalSparksDoer.CleanupOnSceneTransition();
		SilencerInstance.s_MaxRadiusLimiter = null;
		TextBoxManager.ClearPerLevelData();
		SpawnManager.LastPrefabPool = null;
		TimeTubeCreditsController.ClearPerLevelData();
		GameManager.PVP_ENABLED = false;
		if (TK2DTilemapChunkAnimator.PositionToAnimatorMap != null)
		{
			TK2DTilemapChunkAnimator.PositionToAnimatorMap.Clear();
		}
		SecretRoomDoorBeer.AllSecretRoomDoors = null;
		DebrisObject.ClearPerLevelData();
		ExplosionManager.ClearPerLevelData();
		StaticReferenceManager.ClearStaticPerLevelData();
		CollisionData.Pool.Clear();
		LinearCastResult.Pool.Clear();
		RaycastResult.Pool.Clear();
		SpriteChunk.ClearPerLevelData();
		AIActor.ClearPerLevelData();
		TalkDoerLite.ClearPerLevelData();
		Pathfinder.ClearPerLevelData();
		TakeCoverBehavior.ClearPerLevelData();
		if (Pixelator.Instance != null)
		{
			Pixelator.Instance.ClearCachedFrame();
		}
		this.ExtantShopTrackableGuids.Clear();
		EnemyDatabase.Instance.DropReferences();
		EncounterDatabase.Instance.DropReferences();
		GameStatsManager.Instance.MoveSessionStatsToSavedSessionStats();
		GameStatsManager.Save();
	}

	// Token: 0x06006D48 RID: 27976 RVA: 0x002B0F6C File Offset: 0x002AF16C
	public void ClearActiveGameData(bool destroyGameManager, bool endSession)
	{
		GameStatsManager.Instance.CurrentEeveeEquipSeed = -1;
		PickupObject.RatBeatenAtPunchout = false;
		BraveCameraUtility.OverrideAspect = null;
		SuperReaperController.PreventShooting = false;
		BossKillCam.BossDeathCamRunning = false;
		this.ClearPlayers();
		GameManager.IsCoopPast = false;
		GameManager.IsGunslingerPast = false;
		Exploder.OnExplosionTriggered = null;
		MetaInjectionData.ClearBlueprint();
		RewardManifest.ClearManifest(this.RewardManager);
		RewardManager.AdditionalHeartTierMagnificence = 0f;
		BossManager.HasOverriddenCoreBoss = false;
		RoomHandler.HasGivenRoomChestRewardThisRun = false;
		if (PassiveItem.ActiveFlagItems != null)
		{
			PassiveItem.ActiveFlagItems.Clear();
		}
		GameManager.PVP_ENABLED = false;
		Gun.ActiveReloadActivated = false;
		Gun.ActiveReloadActivatedPlayerTwo = false;
		SecretHandshakeItem.NumActive = 0;
		BossKillCam.ClearPerLevelData();
		BaseShopController.HasLockedShopProcedurally = false;
		Chest.HasDroppedResourcefulRatNoteThisSession = false;
		Chest.DoneResourcefulRatMimicThisSession = false;
		Chest.HasDroppedSerJunkanThisSession = false;
		Chest.ToggleCoopChests(false);
		PlayerItem.AllowDamageCooldownOnActive = false;
		AIActor.HealthModifier = 1f;
		Projectile.BaseEnemyBulletSpeedMultiplier = 1f;
		Projectile.ResetGlobalProjectileDepth();
		BasicBeamController.ResetGlobalBeamHeight();
		if (this.MainCameraController)
		{
			this.MainCameraController.enabled = false;
		}
		this.m_lastLoadedLevelDefinition = null;
		Cursor.visible = true;
		this.nextLevelIndex = 0;
		StaticReferenceManager.ForceClearAllStaticMemory();
		if (endSession)
		{
			GameStatsManager.Instance.EndSession(true, true);
		}
		if (destroyGameManager)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x06006D49 RID: 27977 RVA: 0x002B10B0 File Offset: 0x002AF2B0
	public static void BroadcastRoomFsmEvent(string eventName)
	{
		List<PlayMakerFSM> componentsAbsoluteInRoom = GameManager.Instance.BestActivePlayer.CurrentRoom.GetComponentsAbsoluteInRoom<PlayMakerFSM>();
		for (int i = 0; i < componentsAbsoluteInRoom.Count; i++)
		{
			componentsAbsoluteInRoom[i].SendEvent(eventName);
		}
	}

	// Token: 0x06006D4A RID: 27978 RVA: 0x002B10F8 File Offset: 0x002AF2F8
	public static void BroadcastRoomFsmEvent(string eventName, RoomHandler targetRoom)
	{
		List<PlayMakerFSM> componentsAbsoluteInRoom = targetRoom.GetComponentsAbsoluteInRoom<PlayMakerFSM>();
		for (int i = 0; i < componentsAbsoluteInRoom.Count; i++)
		{
			componentsAbsoluteInRoom[i].SendEvent(eventName);
		}
	}

	// Token: 0x06006D4B RID: 27979 RVA: 0x002B1130 File Offset: 0x002AF330
	public static void BroadcastRoomTalkDoerFsmEvent(string eventName)
	{
		for (int i = 0; i < StaticReferenceManager.AllNpcs.Count; i++)
		{
			TalkDoerLite talkDoerLite = StaticReferenceManager.AllNpcs[i];
			if (talkDoerLite)
			{
				if (GameManager.Instance.BestActivePlayer && GameManager.Instance.BestActivePlayer.CurrentRoom == talkDoerLite.ParentRoom)
				{
					talkDoerLite.SendPlaymakerEvent(eventName);
				}
			}
		}
	}

	// Token: 0x06006D4C RID: 27980 RVA: 0x002B11AC File Offset: 0x002AF3AC
	public void StartEncounterableUnlockedChecks()
	{
		this.m_bgChecksActive = true;
		this.ConstructSetOfKnownUnlocks();
		base.StartCoroutine(this.EncounterableUnlockedChecks());
	}

	// Token: 0x06006D4D RID: 27981 RVA: 0x002B11C8 File Offset: 0x002AF3C8
	private void ConstructSetOfKnownUnlocks()
	{
		int num = 0;
		int num2 = 0;
		for (int i = 0; i < EncounterDatabase.Instance.Entries.Count; i++)
		{
			EncounterDatabaseEntry encounterDatabaseEntry = EncounterDatabase.Instance.Entries[i];
			if (encounterDatabaseEntry != null)
			{
				if (!encounterDatabaseEntry.journalData.SuppressInAmmonomicon)
				{
					if (!EncounterDatabase.IsProxy(encounterDatabaseEntry.myGuid))
					{
						num++;
						if (encounterDatabaseEntry.PrerequisitesMet())
						{
							num2++;
							if (encounterDatabaseEntry.prerequisites == null || encounterDatabaseEntry.prerequisites.Length == 0 || GameStatsManager.Instance.QueryEncounterableAnnouncement(encounterDatabaseEntry.myGuid))
							{
								this.m_knownEncounterables.Add(encounterDatabaseEntry.myGuid);
							}
							else
							{
								this.m_queuedUnlocks.Add(encounterDatabaseEntry.myGuid);
							}
						}
						else if (encounterDatabaseEntry.prerequisites != null && encounterDatabaseEntry.prerequisites.Length > 0)
						{
							PickupObject byId = PickupObjectDatabase.GetById(encounterDatabaseEntry.pickupObjectId);
							if (byId == null || byId.quality == PickupObject.ItemQuality.EXCLUDED)
							{
								num2++;
							}
							else if (encounterDatabaseEntry.prerequisites.Length == 1 && encounterDatabaseEntry.prerequisites[0].requireFlag && encounterDatabaseEntry.prerequisites[0].saveFlagToCheck == GungeonFlags.COOP_PAST_REACHED)
							{
								num2++;
							}
						}
					}
				}
			}
		}
		if (num <= num2 + 1)
		{
			GameStatsManager.Instance.SetFlag(GungeonFlags.ITEMSPECIFIC_AMMONOMICON_COMPLETE, true);
		}
	}

	// Token: 0x06006D4E RID: 27982 RVA: 0x002B134C File Offset: 0x002AF54C
	public List<EncounterDatabaseEntry> GetQueuedTrackables()
	{
		List<EncounterDatabaseEntry> list = new List<EncounterDatabaseEntry>(this.m_queuedUnlocks.Count + this.m_newQueuedUnlocks.Count);
		for (int i = 0; i < this.m_queuedUnlocks.Count; i++)
		{
			list.Add(EncounterDatabase.GetEntry(this.m_queuedUnlocks[i]));
		}
		for (int j = 0; j < this.m_newQueuedUnlocks.Count; j++)
		{
			list.Add(EncounterDatabase.GetEntry(this.m_newQueuedUnlocks[j]));
		}
		return list;
	}

	// Token: 0x06006D4F RID: 27983 RVA: 0x002B13E0 File Offset: 0x002AF5E0
	public void AcknowledgeKnownTrackable(EncounterDatabaseEntry trackable)
	{
		GameStatsManager.Instance.MarkEncounterableAnnounced(trackable);
		this.m_queuedUnlocks.Remove(trackable.myGuid);
		this.m_newQueuedUnlocks.Remove(trackable.myGuid);
		this.m_knownEncounterables.Add(trackable.myGuid);
	}

	// Token: 0x06006D50 RID: 27984 RVA: 0x002B1430 File Offset: 0x002AF630
	private IEnumerator EncounterableUnlockedChecks()
	{
		int currentEncounterableIndex = 0;
		List<EncounterDatabaseEntry> allTrackables = EncounterDatabase.Instance.Entries;
		for (;;)
		{
			for (int i = 0; i < 20; i++)
			{
				currentEncounterableIndex = (currentEncounterableIndex + 1) % allTrackables.Count;
				if (allTrackables[currentEncounterableIndex] != null)
				{
					EncounterDatabaseEntry encounterDatabaseEntry = allTrackables[currentEncounterableIndex];
					if (encounterDatabaseEntry.prerequisites != null && encounterDatabaseEntry.prerequisites.Length != 0)
					{
						if (!encounterDatabaseEntry.journalData.SuppressInAmmonomicon)
						{
							if (!this.m_knownEncounterables.Contains(encounterDatabaseEntry.myGuid))
							{
								if (!this.m_queuedUnlocks.Contains(encounterDatabaseEntry.myGuid))
								{
									if (!this.m_newQueuedUnlocks.Contains(encounterDatabaseEntry.myGuid))
									{
										if (encounterDatabaseEntry.PrerequisitesMet())
										{
											BraveUtility.Log(encounterDatabaseEntry.name + " has been unlocked!!!", Color.cyan, BraveUtility.LogVerbosity.IMPORTANT);
											this.m_newQueuedUnlocks.Add(encounterDatabaseEntry.myGuid);
										}
									}
								}
							}
						}
					}
				}
			}
			if (!this.m_paused && this.PrimaryPlayer != null && this.PrimaryPlayer.CurrentRoom != null && !this.PrimaryPlayer.CurrentRoom.IsSealed && this.m_newQueuedUnlocks.Count > 0 && !GameUIRoot.Instance.notificationController.IsDoingNotification)
			{
				EncounterDatabaseEntry entry = EncounterDatabase.GetEntry(this.m_newQueuedUnlocks[0]);
				tk2dSpriteCollectionData encounterIconCollection = AmmonomiconController.Instance.EncounterIconCollection;
				int spriteIdByName = encounterIconCollection.GetSpriteIdByName(entry.journalData.AmmonomiconSprite);
				GameUIRoot.Instance.notificationController.DoCustomNotification(entry.journalData.GetPrimaryDisplayName(false), StringTableManager.GetString("#SMALL_NOTIFICATION_UNLOCKED"), encounterIconCollection, spriteIdByName, UINotificationController.NotificationColor.GOLD, false, false);
				this.m_queuedUnlocks.Add(this.m_newQueuedUnlocks[0]);
				this.m_newQueuedUnlocks.RemoveAt(0);
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x06006D51 RID: 27985 RVA: 0x002B144C File Offset: 0x002AF64C
	public void LaunchTimedEvent(float allowedTime, Action<bool> valueSetter)
	{
		base.StartCoroutine(this.HandleCustomTimer(allowedTime, valueSetter));
	}

	// Token: 0x06006D52 RID: 27986 RVA: 0x002B1460 File Offset: 0x002AF660
	private IEnumerator HandleCustomTimer(float allowedTime, Action<bool> valueSetter)
	{
		valueSetter(true);
		float elapsed = 0f;
		while (elapsed < allowedTime)
		{
			elapsed += BraveTime.DeltaTime;
			yield return null;
		}
		valueSetter(false);
		yield break;
	}

	// Token: 0x06006D53 RID: 27987 RVA: 0x002B1484 File Offset: 0x002AF684
	public static int GetHashByComputerID()
	{
		int num = GameStatsManager.Instance.savedSystemHash;
		if (num != -1)
		{
			return num;
		}
		num = SystemInfo.deviceUniqueIdentifier.GetHashCode();
		GameStatsManager.Instance.savedSystemHash = num;
		return num;
	}

	// Token: 0x06006D54 RID: 27988 RVA: 0x002B14BC File Offset: 0x002AF6BC
	public static DungeonData.Direction[] GetResourcefulRatSolution()
	{
		int hashByComputerID = GameManager.GetHashByComputerID();
		System.Random random = new System.Random(hashByComputerID);
		DungeonData.Direction[] array = new DungeonData.Direction[6];
		for (int i = 0; i < 6; i++)
		{
			int num = random.Next(0, 4);
			if (i == 0 && num == 3)
			{
				num = random.Next(0, 3);
			}
			if (num == 0)
			{
				array[i] = DungeonData.Direction.NORTH;
			}
			else if (num == 1)
			{
				array[i] = DungeonData.Direction.EAST;
			}
			else if (num == 2)
			{
				array[i] = DungeonData.Direction.SOUTH;
			}
			else if (num == 3)
			{
				array[i] = DungeonData.Direction.WEST;
			}
			else
			{
				UnityEngine.Debug.LogError("Error in RR Solution");
			}
		}
		return array;
	}

	// Token: 0x040069D4 RID: 27092
	public static bool BackgroundGenerationActive;

	// Token: 0x040069D5 RID: 27093
	public static bool DivertResourcesToGeneration;

	// Token: 0x040069D6 RID: 27094
	public static bool IsShuttingDown;

	// Token: 0x040069D7 RID: 27095
	public const int EEVEE_META_COST = 5;

	// Token: 0x040069D8 RID: 27096
	public const int GUNSLINGER_META_COST = 7;

	// Token: 0x040069D9 RID: 27097
	public const bool c_RESOURCEFUL_RAT_ACTIVE = false;

	// Token: 0x040069DA RID: 27098
	public const float CUSTOM_CULL_SQR_DIST_THRESHOLD = 420f;

	// Token: 0x040069DB RID: 27099
	private static string DEBUG_LABEL;

	// Token: 0x040069DC RID: 27100
	public static string SEED_LABEL = string.Empty;

	// Token: 0x040069DD RID: 27101
	public const float SCENE_TRANSITION_TIME = 0.15f;

	// Token: 0x040069DE RID: 27102
	public static bool AUDIO_ENABLED;

	// Token: 0x040069DF RID: 27103
	public static float PIT_DEPTH = -2.5f;

	// Token: 0x040069E0 RID: 27104
	public static float INVARIANT_DELTA_TIME;

	// Token: 0x040069E1 RID: 27105
	public static bool SKIP_FOYER;

	// Token: 0x040069E2 RID: 27106
	public static bool PVP_ENABLED;

	// Token: 0x040069E3 RID: 27107
	public static bool IsBossIntro;

	// Token: 0x040069E4 RID: 27108
	public PlatformInterface platformInterface;

	// Token: 0x040069E5 RID: 27109
	private Coroutine CurrentResolutionShiftCoroutine;

	// Token: 0x040069E6 RID: 27110
	private static GameObject m_playerPrefabForNewGame;

	// Token: 0x040069E7 RID: 27111
	private static GameObject m_coopPlayerPrefabForNewGame;

	// Token: 0x040069E8 RID: 27112
	public static GameObject LastUsedPlayerPrefab;

	// Token: 0x040069E9 RID: 27113
	public static GameObject LastUsedCoopPlayerPrefab;

	// Token: 0x040069EA RID: 27114
	private static GameOptions m_options;

	// Token: 0x040069EB RID: 27115
	private static GameManager mr_manager;

	// Token: 0x040069EC RID: 27116
	private static InControlManager m_inputManager;

	// Token: 0x040069ED RID: 27117
	private DungeonFloorMusicController m_dungeonMusicController;

	// Token: 0x040069EE RID: 27118
	public static bool PreventGameManagerExistence;

	// Token: 0x040069F0 RID: 27120
	public RewardManager CurrentRewardManager;

	// Token: 0x040069F1 RID: 27121
	public RewardManager OriginalRewardManager;

	// Token: 0x040069F2 RID: 27122
	public AdvancedSynergyDatabase SynergyManager;

	// Token: 0x040069F3 RID: 27123
	public MetaInjectionData GlobalInjectionData;

	// Token: 0x040069F4 RID: 27124
	[NonSerialized]
	public InputDevice LastUsedInputDeviceForConversation;

	// Token: 0x040069F5 RID: 27125
	public tk2dFontData DefaultAlienConversationFont;

	// Token: 0x040069F6 RID: 27126
	public tk2dFontData DefaultNormalConversationFont;

	// Token: 0x040069F8 RID: 27128
	public int RandomIntForCurrentRun;

	// Token: 0x040069F9 RID: 27129
	[NonSerialized]
	public bool IsLoadingFirstShortcutFloor;

	// Token: 0x040069FA RID: 27130
	public int LastShortcutFloorLoaded;

	// Token: 0x040069FB RID: 27131
	private bool m_forceSeedUpdate;

	// Token: 0x040069FC RID: 27132
	[NonSerialized]
	private int m_currentRunSeed;

	// Token: 0x040069FD RID: 27133
	private bool m_paused;

	// Token: 0x040069FE RID: 27134
	private bool m_unpausedThisFrame;

	// Token: 0x040069FF RID: 27135
	private bool m_pauseLockedCamera;

	// Token: 0x04006A00 RID: 27136
	private bool m_loadingLevel;

	// Token: 0x04006A01 RID: 27137
	private bool m_isFoyer;

	// Token: 0x04006A02 RID: 27138
	private GameManager.GameMode m_currentGameMode;

	// Token: 0x04006A03 RID: 27139
	public GameManager.ControlType controlType;

	// Token: 0x04006A04 RID: 27140
	private GameManager.GameType m_currentGameType;

	// Token: 0x04006A05 RID: 27141
	public bool IsSelectingCharacter;

	// Token: 0x04006A06 RID: 27142
	private GameManager.LevelOverrideState? m_generatingLevelState;

	// Token: 0x04006A07 RID: 27143
	public static bool IsCoopPast;

	// Token: 0x04006A08 RID: 27144
	public static bool IsGunslingerPast;

	// Token: 0x04006A09 RID: 27145
	private Dungeon m_dungeon;

	// Token: 0x04006A0A RID: 27146
	public Dungeon CurrentlyGeneratingDungeonPrefab;

	// Token: 0x04006A0B RID: 27147
	public DungeonData PregeneratedDungeonData;

	// Token: 0x04006A0C RID: 27148
	public Dungeon DungeonToAutoLoad;

	// Token: 0x04006A0D RID: 27149
	private CameraController m_camera;

	// Token: 0x04006A0E RID: 27150
	private PlayerController[] m_players;

	// Token: 0x04006A0F RID: 27151
	public int LastPausingPlayerID = -1;

	// Token: 0x04006A10 RID: 27152
	private PlayerController m_player;

	// Token: 0x04006A11 RID: 27153
	private PlayerController m_secondaryPlayer;

	// Token: 0x04006A12 RID: 27154
	[NonSerialized]
	public List<string> ExtantShopTrackableGuids = new List<string>();

	// Token: 0x04006A13 RID: 27155
	public List<GameLevelDefinition> dungeonFloors;

	// Token: 0x04006A14 RID: 27156
	public List<GameLevelDefinition> customFloors;

	// Token: 0x04006A15 RID: 27157
	private GameLevelDefinition m_lastLoadedLevelDefinition;

	// Token: 0x04006A16 RID: 27158
	private int nextLevelIndex = 1;

	// Token: 0x04006A17 RID: 27159
	[NonSerialized]
	private string m_injectedFlowPath;

	// Token: 0x04006A18 RID: 27160
	[NonSerialized]
	private string m_injectedLevelName;

	// Token: 0x04006A1A RID: 27162
	private bool m_preventUnpausing;

	// Token: 0x04006A1B RID: 27163
	public RunData RunData = new RunData();

	// Token: 0x04006A1C RID: 27164
	protected static float m_deltaTime;

	// Token: 0x04006A1D RID: 27165
	protected static float m_lastFrameRealtime;

	// Token: 0x04006A1E RID: 27166
	private bool m_applicationHasFocus = true;

	// Token: 0x04006A1F RID: 27167
	private static Vector4 s_bossIntroTime;

	// Token: 0x04006A20 RID: 27168
	private static int s_bossIntroTimeId = -1;

	// Token: 0x04006A21 RID: 27169
	private const int c_framesToCount = 4;

	// Token: 0x04006A22 RID: 27170
	private CircularBuffer<float> m_frameTimes = new CircularBuffer<float>(4);

	// Token: 0x04006A23 RID: 27171
	private AkAudioListener m_audioListener;

	// Token: 0x04006A24 RID: 27172
	public int TargetQuickRestartLevel = -1;

	// Token: 0x04006A25 RID: 27173
	public static bool ForceQuickRestartAlternateCostumeP1;

	// Token: 0x04006A26 RID: 27174
	public static bool ForceQuickRestartAlternateCostumeP2;

	// Token: 0x04006A27 RID: 27175
	public static bool ForceQuickRestartAlternateGunP1;

	// Token: 0x04006A28 RID: 27176
	public static bool ForceQuickRestartAlternateGunP2;

	// Token: 0x04006A29 RID: 27177
	public static bool IsReturningToFoyerWithPlayer;

	// Token: 0x04006A2A RID: 27178
	private bool m_preparingToDestroyThisGameManagerOnCollision;

	// Token: 0x04006A2B RID: 27179
	private bool m_shouldDestroyThisGameManagerOnCollision;

	// Token: 0x04006A2C RID: 27180
	private AsyncOperation m_preDestroyAsyncHolder;

	// Token: 0x04006A2D RID: 27181
	private GameObject m_preDestroyLoadingHierarchyHolder;

	// Token: 0x04006A2E RID: 27182
	private Type[] BraveLevelLoadedListeners = new Type[]
	{
		typeof(PlayerController),
		typeof(SpeculativeRigidbody),
		typeof(GameUIBlankController),
		typeof(AmmonomiconDeathPageController),
		typeof(GameUIHeartController),
		typeof(RingOfResourcefulRatItem),
		typeof(ReturnAmmoOnMissedShotItem),
		typeof(PlatinumBulletsItem),
		typeof(dfPoolManager),
		typeof(ChamberGunProcessor)
	};

	// Token: 0x04006A2F RID: 27183
	private const float PIXELATE_TIME = 0.15f;

	// Token: 0x04006A30 RID: 27184
	private const float PIXELATE_FADE_TARGET = 1f;

	// Token: 0x04006A31 RID: 27185
	private const float DEPIXELATE_TIME = 0.075f;

	// Token: 0x04006A32 RID: 27186
	private static float c_asyncSoundStartTime;

	// Token: 0x04006A33 RID: 27187
	private static int c_asyncSoundStartFrame;

	// Token: 0x04006A34 RID: 27188
	private static bool m_hasEnsuredHeapSize;

	// Token: 0x04006A35 RID: 27189
	private bool m_initializedDeviceCallbacks;

	// Token: 0x04006A36 RID: 27190
	public bool PREVENT_MAIN_MENU_TEXT;

	// Token: 0x04006A37 RID: 27191
	public bool DEBUG_UI_VISIBLE = true;

	// Token: 0x04006A38 RID: 27192
	public bool DAVE_INFO_VISIBLE;

	// Token: 0x04006A39 RID: 27193
	[Header("Convenient Balance Numbers")]
	public float COOP_ENEMY_HEALTH_MULTIPLIER = 1.25f;

	// Token: 0x04006A3A RID: 27194
	public float COOP_ENEMY_PROJECTILE_SPEED_MULTIPLIER = 0.9f;

	// Token: 0x04006A3B RID: 27195
	public float DUAL_WIELDING_DAMAGE_FACTOR = 0.75f;

	// Token: 0x04006A3C RID: 27196
	public float[] PierceDamageScaling;

	// Token: 0x04006A3D RID: 27197
	public BloodthirstSettings BloodthirstOptions;

	// Token: 0x04006A3E RID: 27198
	public List<AGDEnemyReplacementTier> EnemyReplacementTiers;

	// Token: 0x04006A3F RID: 27199
	[PickupIdentifier]
	public List<int> RainbowRunForceIncludedIDs;

	// Token: 0x04006A40 RID: 27200
	[PickupIdentifier]
	public List<int> RainbowRunForceExcludedIDs;

	// Token: 0x04006A41 RID: 27201
	private bool m_bgChecksActive;

	// Token: 0x04006A42 RID: 27202
	private HashSet<string> m_knownEncounterables = new HashSet<string>();

	// Token: 0x04006A43 RID: 27203
	private List<string> m_queuedUnlocks = new List<string>();

	// Token: 0x04006A44 RID: 27204
	private List<string> m_newQueuedUnlocks = new List<string>();

	// Token: 0x04006A45 RID: 27205
	private const int NUM_ENCOUNTERABLES_CHECKED_PER_FRAME = 20;

	// Token: 0x020012ED RID: 4845
	public enum ControlType
	{
		// Token: 0x04006A48 RID: 27208
		KEYBOARD,
		// Token: 0x04006A49 RID: 27209
		CONTROLLER
	}

	// Token: 0x020012EE RID: 4846
	public enum GameMode
	{
		// Token: 0x04006A4B RID: 27211
		NORMAL,
		// Token: 0x04006A4C RID: 27212
		SHORTCUT,
		// Token: 0x04006A4D RID: 27213
		BOSSRUSH,
		// Token: 0x04006A4E RID: 27214
		SUPERBOSSRUSH
	}

	// Token: 0x020012EF RID: 4847
	public enum GameType
	{
		// Token: 0x04006A50 RID: 27216
		SINGLE_PLAYER,
		// Token: 0x04006A51 RID: 27217
		COOP_2_PLAYER
	}

	// Token: 0x020012F0 RID: 4848
	public enum LevelOverrideState
	{
		// Token: 0x04006A53 RID: 27219
		NONE,
		// Token: 0x04006A54 RID: 27220
		FOYER,
		// Token: 0x04006A55 RID: 27221
		TUTORIAL,
		// Token: 0x04006A56 RID: 27222
		RESOURCEFUL_RAT,
		// Token: 0x04006A57 RID: 27223
		END_TIMES,
		// Token: 0x04006A58 RID: 27224
		CHARACTER_PAST,
		// Token: 0x04006A59 RID: 27225
		DEBUG_TEST
	}
}
