using System;
using System.Collections;
using System.Collections.Generic;
using Dungeonator;
using UnityEngine;

// Token: 0x02001265 RID: 4709
public class ChallengeManager : MonoBehaviour
{
	// Token: 0x17000FA6 RID: 4006
	// (get) Token: 0x06006981 RID: 27009 RVA: 0x0029542C File Offset: 0x0029362C
	public static ChallengeManager Instance
	{
		get
		{
			if (!ChallengeManager.m_instance)
			{
				ChallengeManager.m_instance = UnityEngine.Object.FindObjectOfType<ChallengeManager>();
			}
			return ChallengeManager.m_instance;
		}
	}

	// Token: 0x17000FA7 RID: 4007
	// (get) Token: 0x06006982 RID: 27010 RVA: 0x0029544C File Offset: 0x0029364C
	// (set) Token: 0x06006983 RID: 27011 RVA: 0x00295478 File Offset: 0x00293678
	public static ChallengeModeType ChallengeModeType
	{
		get
		{
			ChallengeManager instance = ChallengeManager.Instance;
			return (!instance) ? ChallengeModeType.None : instance.ChallengeMode;
		}
		set
		{
			ChallengeManager instance = ChallengeManager.Instance;
			if (instance)
			{
				if (instance.ChallengeMode == value)
				{
					return;
				}
				UnityEngine.Object.Destroy(instance.gameObject);
			}
			if (value == ChallengeModeType.GunslingKingTemporary)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>((GameObject)BraveResources.Load("Global Prefabs/_ChallengeManager", ".prefab"));
				gameObject.GetComponent<ChallengeManager>().ChallengeMode = ChallengeModeType.GunslingKingTemporary;
			}
			else if (value == ChallengeModeType.ChallengeMode)
			{
				UnityEngine.Object.Instantiate<GameObject>((GameObject)BraveResources.Load("Global Prefabs/_ChallengeManager", ".prefab"));
			}
			else if (value == ChallengeModeType.ChallengeMegaMode)
			{
				UnityEngine.Object.Instantiate<GameObject>((GameObject)BraveResources.Load("Global Prefabs/_ChallengeMegaManager", ".prefab"));
			}
		}
	}

	// Token: 0x17000FA8 RID: 4008
	// (get) Token: 0x06006984 RID: 27012 RVA: 0x00295528 File Offset: 0x00293728
	public List<ChallengeModifier> ActiveChallenges
	{
		get
		{
			return this.m_activeChallenges;
		}
	}

	// Token: 0x06006985 RID: 27013 RVA: 0x00295530 File Offset: 0x00293730
	private IEnumerator Start()
	{
		ChallengeManager.m_instance = this;
		if (this.ChallengeMode != ChallengeModeType.GunslingKingTemporary)
		{
			ChallengeManager.CHALLENGE_MODE_ACTIVE = true;
			GameStatsManager.Instance.RegisterStatChange(TrackedStats.CHALLENGE_MODE_ATTEMPTS, 1f);
			if (GameStatsManager.Instance.GetPlayerStatValue(TrackedStats.CHALLENGE_MODE_ATTEMPTS) >= 30f)
			{
				GameStatsManager.Instance.SetFlag(GungeonFlags.DAISUKE_CHALLENGE_ITEM_UNLOCK, true);
			}
			UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
			base.transform.parent = GameManager.Instance.transform;
		}
		while (GameManager.Instance.PrimaryPlayer == null)
		{
			yield return null;
		}
		this.m_init = true;
		this.m_primaryPlayer = GameManager.Instance.PrimaryPlayer;
		PlayerController primaryPlayer = this.m_primaryPlayer;
		primaryPlayer.OnEnteredCombat = (Action)Delegate.Combine(primaryPlayer.OnEnteredCombat, new Action(this.EnteredCombat));
		yield break;
	}

	// Token: 0x06006986 RID: 27014 RVA: 0x0029554C File Offset: 0x0029374C
	private void Update()
	{
		if (this.m_activeChallenges.Count > 0 && !this.m_primaryPlayer.IsInCombat)
		{
			this.CleanupChallenges();
		}
		if (this.m_init && GameManager.Instance.IsFoyer && this.m_primaryPlayer != GameManager.Instance.PrimaryPlayer)
		{
			if (this.m_primaryPlayer)
			{
				PlayerController primaryPlayer = this.m_primaryPlayer;
				primaryPlayer.OnEnteredCombat = (Action)Delegate.Remove(primaryPlayer.OnEnteredCombat, new Action(this.EnteredCombat));
			}
			this.m_primaryPlayer = GameManager.Instance.PrimaryPlayer;
			PlayerController primaryPlayer2 = this.m_primaryPlayer;
			primaryPlayer2.OnEnteredCombat = (Action)Delegate.Combine(primaryPlayer2.OnEnteredCombat, new Action(this.EnteredCombat));
		}
	}

	// Token: 0x06006987 RID: 27015 RVA: 0x00295624 File Offset: 0x00293824
	private void OnDestroy()
	{
		this.CleanupChallenges();
		if (this.m_primaryPlayer)
		{
			PlayerController primaryPlayer = this.m_primaryPlayer;
			primaryPlayer.OnEnteredCombat = (Action)Delegate.Remove(primaryPlayer.OnEnteredCombat, new Action(this.EnteredCombat));
		}
		if (ChallengeManager.m_instance == this)
		{
			ChallengeManager.m_instance = null;
			ChallengeManager.CHALLENGE_MODE_ACTIVE = false;
		}
	}

	// Token: 0x06006988 RID: 27016 RVA: 0x0029568C File Offset: 0x0029388C
	private ChallengeFloorData GetFloorData(GlobalDungeonData.ValidTilesets tilesetID)
	{
		for (int i = 0; i < this.FloorData.Count; i++)
		{
			if (this.FloorData[i].floorID == tilesetID)
			{
				return this.FloorData[i];
			}
		}
		return null;
	}

	// Token: 0x17000FA9 RID: 4009
	// (get) Token: 0x06006989 RID: 27017 RVA: 0x002956DC File Offset: 0x002938DC
	// (set) Token: 0x0600698A RID: 27018 RVA: 0x002956E4 File Offset: 0x002938E4
	public bool SuppressChallengeStart { get; set; }

	// Token: 0x0600698B RID: 27019 RVA: 0x002956F0 File Offset: 0x002938F0
	public void EnteredCombat()
	{
		if (GameManager.Instance.CurrentLevelOverrideState == GameManager.LevelOverrideState.CHARACTER_PAST)
		{
			return;
		}
		if (GameManager.Instance.InTutorial)
		{
			return;
		}
		if (this.SuppressChallengeStart)
		{
			return;
		}
		if (this.ChallengeMode == ChallengeModeType.GunslingKingTemporary && this.GunslingTargetRoom != null && GameManager.Instance.PrimaryPlayer.CurrentRoom != this.GunslingTargetRoom)
		{
			return;
		}
		this.CleanupChallenges();
		ChallengeFloorData floorData = this.GetFloorData(GameManager.Instance.Dungeon.tileIndices.tilesetId);
		int num;
		if (floorData != null)
		{
			num = UnityEngine.Random.Range(floorData.minChallenges, floorData.maxChallenges + 1);
		}
		else
		{
			GlobalDungeonData.ValidTilesets tilesetId = GameManager.Instance.Dungeon.tileIndices.tilesetId;
			if (tilesetId != GlobalDungeonData.ValidTilesets.GUNGEON)
			{
				if (tilesetId != GlobalDungeonData.ValidTilesets.CASTLEGEON)
				{
					if (tilesetId != GlobalDungeonData.ValidTilesets.MINEGEON)
					{
						if (tilesetId != GlobalDungeonData.ValidTilesets.CATACOMBGEON)
						{
							if (tilesetId != GlobalDungeonData.ValidTilesets.FORGEGEON)
							{
								num = 1;
							}
							else
							{
								num = 5;
							}
						}
						else
						{
							num = 4;
						}
					}
					else
					{
						num = 3;
					}
				}
				else
				{
					num = 1;
				}
			}
			else
			{
				num = 2;
			}
		}
		this.TriggerNewChallenges(num);
		base.StartCoroutine(this.HandleNewChallengeAnnouncements());
	}

	// Token: 0x0600698C RID: 27020 RVA: 0x0029581C File Offset: 0x00293A1C
	private IEnumerator HandleNewChallengeAnnouncements()
	{
		float elapsed = 0f;
		float duration = Mathf.Max(2.5f, (float)this.m_activeChallenges.Count);
		if (GameManager.Options.SLOW_TIME_ON_CHALLENGE_MODE_REVEAL)
		{
			BraveTime.RegisterTimeScaleMultiplier(0.1f, base.gameObject);
		}
		Vector3[] startPositions = new Vector3[this.m_activeChallenges.Count];
		for (int i = 0; i < this.m_activeChallenges.Count; i++)
		{
			dfGUIManager manager = GameUIRoot.Instance.Manager;
			dfLabel dfLabel = manager.AddControl<dfLabel>();
			dfLabel.Font = GameUIRoot.Instance.Manager.DefaultFont;
			dfLabel.TextScale = 3f;
			dfLabel.AutoSize = true;
			dfLabel.Pivot = dfPivotPoint.TopRight;
			GameUIAmmoController gameUIAmmoController = GameUIRoot.Instance.GetAmmoControllerForPlayerID(0);
			if (GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER)
			{
				gameUIAmmoController = GameUIRoot.Instance.GetAmmoControllerForPlayerID(1);
			}
			dfLabel.RelativePosition = new Vector3(gameUIAmmoController.DefaultAmmoFGSprite.GetAbsolutePosition().x + dfLabel.Width, (float)(Mathf.FloorToInt(manager.GetScreenSize().y / 2f) - 198 + 60 * i), 0f);
			this.m_activeChallenges[i].IconLabel = dfLabel;
			startPositions[i] = dfLabel.RelativePosition;
			dfLabel.IsLocalized = true;
			if (!string.IsNullOrEmpty(this.m_activeChallenges[i].AlternateLanguageDisplayName) && GameManager.Options.CurrentLanguage != StringTableManager.GungeonSupportedLanguages.ENGLISH)
			{
				dfLabel.Text = StringTableManager.GetEnemiesString(this.m_activeChallenges[i].AlternateLanguageDisplayName, -1);
			}
			else
			{
				dfLabel.Text = this.m_activeChallenges[i].DisplayName;
			}
			dfSprite dfSprite = manager.AddControl<dfSprite>();
			dfSprite.SpriteName = this.m_activeChallenges[i].AtlasSpriteName;
			dfSprite.Size = dfSprite.SpriteInfo.sizeInPixels * 3f;
			this.m_activeChallenges[i].IconSprite = dfSprite;
			dfSprite.BringToFront();
			dfLabel.AddControl(dfSprite);
			dfLabel.BringToFront();
			dfSprite.RelativePosition = new Vector3(6f + dfLabel.Width, -3f, 0f);
		}
		while (elapsed < duration)
		{
			elapsed += GameManager.INVARIANT_DELTA_TIME;
			GameUIAmmoController ammoController = GameUIRoot.Instance.GetAmmoControllerForPlayerID(0);
			if (GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER)
			{
				ammoController = GameUIRoot.Instance.GetAmmoControllerForPlayerID(1);
			}
			for (int j = 0; j < this.m_activeChallenges.Count; j++)
			{
				if (elapsed - 0.4f * (float)j > 0f && elapsed - GameManager.INVARIANT_DELTA_TIME - 0.4f * (float)j <= 0f)
				{
					AkSoundEngine.PostEvent(this.ChallengeInSFX, GameManager.Instance.PrimaryPlayer.gameObject);
				}
				dfLabel iconLabel = this.m_activeChallenges[j].IconLabel;
				iconLabel.RelativePosition = Vector3.Lerp(startPositions[j], new Vector3(ammoController.DefaultAmmoFGSprite.GetAbsolutePosition().x - iconLabel.Width - 42f, iconLabel.RelativePosition.y, iconLabel.RelativePosition.z), Mathf.SmoothStep(0f, 1f, Mathf.Clamp01(elapsed - 0.4f * (float)j)));
			}
			if (GameManager.Options.SLOW_TIME_ON_CHALLENGE_MODE_REVEAL)
			{
				BraveTime.ClearMultiplier(base.gameObject);
				BraveTime.RegisterTimeScaleMultiplier(Mathf.Lerp(0.1f, 1f, elapsed - (duration - 1f)), base.gameObject);
			}
			yield return null;
		}
		for (int k = 0; k < this.m_activeChallenges.Count; k++)
		{
			dfLabel iconLabel2 = this.m_activeChallenges[k].IconLabel;
			dfSprite iconSprite = this.m_activeChallenges[k].IconSprite;
			iconLabel2.RemoveControl(iconSprite);
			iconSprite.AddControl(iconLabel2);
			iconSprite.BringToFront();
		}
		if (GameManager.Options.SLOW_TIME_ON_CHALLENGE_MODE_REVEAL)
		{
			BraveTime.ClearMultiplier(base.gameObject);
		}
		elapsed = 0f;
		duration = 2f;
		while (elapsed < duration && this.m_activeChallenges.Count > 0 && (!GameManager.Instance.IsPaused || !GameUIRoot.Instance.PauseMenuPanel.IsVisible))
		{
			elapsed += GameManager.INVARIANT_DELTA_TIME;
			for (int l = 0; l < this.m_activeChallenges.Count; l++)
			{
				this.m_activeChallenges[l].IconLabel.Opacity = 1f - elapsed / duration;
			}
			yield return null;
		}
		while (this.m_activeChallenges.Count > 0)
		{
			if (GameManager.Instance.IsPaused && GameUIRoot.Instance.PauseMenuPanel && GameUIRoot.Instance.PauseMenuPanel.IsVisible)
			{
				for (int m = 0; m < this.m_activeChallenges.Count; m++)
				{
					this.m_activeChallenges[m].IconLabel.Opacity = 1f;
				}
			}
			else
			{
				for (int n = 0; n < this.m_activeChallenges.Count; n++)
				{
					this.m_activeChallenges[n].IconLabel.Opacity = 0f;
				}
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x0600698D RID: 27021 RVA: 0x00295838 File Offset: 0x00293A38
	private void TriggerNewChallenges(int numToAdd)
	{
		if (GameManager.Instance.InTutorial)
		{
			return;
		}
		RoomHandler currentRoom = GameManager.Instance.PrimaryPlayer.CurrentRoom;
		if (currentRoom.area.PrototypeRoomCategory == PrototypeDungeonRoom.RoomCategory.BOSS)
		{
			BossChallengeData bossChallengeData = null;
			List<AIActor> activeEnemies = currentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All);
			for (int i = 0; i < activeEnemies.Count; i++)
			{
				if (activeEnemies[i] && activeEnemies[i].healthHaver && activeEnemies[i].healthHaver.IsBoss)
				{
					for (int j = 0; j < this.BossChallenges.Count; j++)
					{
						BossChallengeData bossChallengeData2 = this.BossChallenges[j];
						for (int k = 0; k < bossChallengeData2.BossGuids.Length; k++)
						{
							if (bossChallengeData2.BossGuids[k] == activeEnemies[i].EnemyGuid)
							{
								bossChallengeData = bossChallengeData2;
								break;
							}
						}
					}
				}
			}
			if (bossChallengeData != null)
			{
				numToAdd = bossChallengeData.NumToSelect;
				int num = 0;
				while (this.m_activeChallenges.Count < numToAdd && num < 10000)
				{
					num++;
					ChallengeModifier challengeModifier = bossChallengeData.Modifiers[UnityEngine.Random.Range(0, bossChallengeData.Modifiers.Length)];
					bool flag = challengeModifier.IsValid(currentRoom);
					for (int l = 0; l < this.m_activeChallenges.Count; l++)
					{
						if (flag && this.m_activeChallenges[l].DisplayName == challengeModifier.DisplayName)
						{
							flag = false;
						}
						if (flag && this.m_activeChallenges[l].MutuallyExclusive.Contains(challengeModifier))
						{
							flag = false;
						}
					}
					if (flag)
					{
						ChallengeModifier component = UnityEngine.Object.Instantiate<GameObject>(challengeModifier.gameObject).GetComponent<ChallengeModifier>();
						this.m_activeChallenges.Add(component);
					}
				}
			}
		}
		if (this.m_activeChallenges.Count == 0)
		{
			int num2 = numToAdd;
			int num3 = 0;
			GlobalDungeonData.ValidTilesets tilesetId = GameManager.Instance.Dungeon.tileIndices.tilesetId;
			while (num2 > 0 && num3 < 10000)
			{
				num3++;
				ChallengeDataEntry challengeDataEntry = this.PossibleChallenges[UnityEngine.Random.Range(0, this.PossibleChallenges.Count)];
				ChallengeModifier challenge = challengeDataEntry.challenge;
				bool flag2 = challenge != null && challenge.IsValid(currentRoom) && challengeDataEntry.GetWeightForFloor(tilesetId) <= num2;
				if (flag2 && (challengeDataEntry.excludedTilesets | tilesetId) == challengeDataEntry.excludedTilesets)
				{
					flag2 = false;
				}
				if (flag2 && currentRoom.area.PrototypeRoomCategory == PrototypeDungeonRoom.RoomCategory.BOSS && !challenge.ValidInBossChambers)
				{
					flag2 = false;
				}
				for (int m = 0; m < this.m_activeChallenges.Count; m++)
				{
					if (flag2 && this.m_activeChallenges[m].DisplayName == challenge.DisplayName)
					{
						flag2 = false;
					}
					if (flag2 && this.m_activeChallenges[m].MutuallyExclusive.Contains(challenge))
					{
						flag2 = false;
					}
				}
				if (flag2)
				{
					ChallengeModifier component2 = UnityEngine.Object.Instantiate<GameObject>(challenge.gameObject).GetComponent<ChallengeModifier>();
					this.m_activeChallenges.Add(component2);
					num2 -= challengeDataEntry.GetWeightForFloor(tilesetId);
				}
			}
		}
	}

	// Token: 0x0600698E RID: 27022 RVA: 0x00295BDC File Offset: 0x00293DDC
	public void ForceStop()
	{
		this.CleanupChallenges();
	}

	// Token: 0x0600698F RID: 27023 RVA: 0x00295BE4 File Offset: 0x00293DE4
	private void CleanupChallenges()
	{
		bool flag = false;
		if (this.m_activeChallenges.Count > 0 && GameManager.Instance.PrimaryPlayer)
		{
			flag = true;
			AkSoundEngine.PostEvent("Play_UI_challenge_clear_01", GameManager.Instance.PrimaryPlayer.gameObject);
		}
		for (int i = 0; i < this.m_activeChallenges.Count; i++)
		{
			if (this.m_activeChallenges[i])
			{
				this.m_activeChallenges[i].ShatterIcon(this.ChallengeBurstClip);
				UnityEngine.Object.Destroy(this.m_activeChallenges[i].gameObject);
			}
		}
		this.m_activeChallenges.Clear();
		if (flag && this.ChallengeMode == ChallengeModeType.GunslingKingTemporary)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x040065EB RID: 26091
	public static bool CHALLENGE_MODE_ACTIVE;

	// Token: 0x040065EC RID: 26092
	private static ChallengeManager m_instance;

	// Token: 0x040065ED RID: 26093
	public ChallengeModeType ChallengeMode = ChallengeModeType.ChallengeMode;

	// Token: 0x040065EE RID: 26094
	[NonSerialized]
	public RoomHandler GunslingTargetRoom;

	// Token: 0x040065EF RID: 26095
	public string ChallengeInSFX = "Play_UI_menu_pause_01";

	// Token: 0x040065F0 RID: 26096
	public dfAnimationClip ChallengeBurstClip;

	// Token: 0x040065F1 RID: 26097
	public List<ChallengeFloorData> FloorData = new List<ChallengeFloorData>();

	// Token: 0x040065F2 RID: 26098
	[Header("Remember the other _ChallengeManager too!")]
	public List<ChallengeDataEntry> PossibleChallenges = new List<ChallengeDataEntry>();

	// Token: 0x040065F3 RID: 26099
	public List<BossChallengeData> BossChallenges = new List<BossChallengeData>();

	// Token: 0x040065F4 RID: 26100
	private List<ChallengeModifier> m_activeChallenges = new List<ChallengeModifier>();

	// Token: 0x040065F5 RID: 26101
	private PlayerController m_primaryPlayer;

	// Token: 0x040065F6 RID: 26102
	private bool m_init;
}
