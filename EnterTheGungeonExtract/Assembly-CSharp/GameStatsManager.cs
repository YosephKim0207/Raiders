using System;
using System.Collections.Generic;
using FullSerializer;
using UnityEngine;

// Token: 0x020014FC RID: 5372
[fsObject]
public class GameStatsManager
{
	// Token: 0x1700120B RID: 4619
	// (get) Token: 0x06007A54 RID: 31316 RVA: 0x00310534 File Offset: 0x0030E734
	public bool IsRainbowRun
	{
		get
		{
			GameManager instance = GameManager.Instance;
			return instance.CurrentLevelOverrideState != GameManager.LevelOverrideState.CHARACTER_PAST && instance.CurrentLevelOverrideState != GameManager.LevelOverrideState.FOYER && instance.CurrentLevelOverrideState != GameManager.LevelOverrideState.TUTORIAL && this.rainbowRunToggled;
		}
	}

	// Token: 0x1700120C RID: 4620
	// (get) Token: 0x06007A55 RID: 31317 RVA: 0x00310574 File Offset: 0x0030E774
	public static GameStatsManager Instance
	{
		get
		{
			if (GameStatsManager.m_instance == null)
			{
				Debug.LogError("Trying to access GameStatsManager before it has been initialized.");
			}
			return GameStatsManager.m_instance;
		}
	}

	// Token: 0x1700120D RID: 4621
	// (get) Token: 0x06007A56 RID: 31318 RVA: 0x00310590 File Offset: 0x0030E790
	public static bool HasInstance
	{
		get
		{
			return GameStatsManager.m_instance != null;
		}
	}

	// Token: 0x1700120E RID: 4622
	// (get) Token: 0x06007A57 RID: 31319 RVA: 0x003105A0 File Offset: 0x0030E7A0
	public int PlaytimeMin
	{
		get
		{
			return Mathf.RoundToInt(this.GetPlayerStatValue(TrackedStats.TIME_PLAYED) / 60f);
		}
	}

	// Token: 0x1700120F RID: 4623
	// (get) Token: 0x06007A58 RID: 31320 RVA: 0x003105B8 File Offset: 0x0030E7B8
	public float NewPlayerEnemyCullFactor
	{
		get
		{
			if (this.GetFlag(GungeonFlags.BOSSKILLED_DRAGUN))
			{
				return 0f;
			}
			int num = Mathf.RoundToInt(this.GetPlayerStatValue(TrackedStats.TIMES_REACHED_FORGE));
			float num2 = 0.1f;
			return Mathf.Clamp(num2 - (float)num * 0.02f, 0f, 0.1f);
		}
	}

	// Token: 0x06007A59 RID: 31321 RVA: 0x0031060C File Offset: 0x0030E80C
	public static void Load()
	{
		SaveManager.Init();
		if (!SaveManager.Load<GameStatsManager>(SaveManager.GameSave, out GameStatsManager.m_instance, true, 0U, null, null))
		{
			GameStatsManager.m_instance = new GameStatsManager();
		}
		if (GameStatsManager.m_instance.huntProgress != null)
		{
			GameStatsManager.m_instance.huntProgress.OnLoaded();
		}
		else
		{
			GameStatsManager.m_instance.huntProgress = new MonsterHuntProgress();
			GameStatsManager.m_instance.huntProgress.OnLoaded();
		}
		if (GameStatsManager.s_pastFlags == null)
		{
			GameStatsManager.s_pastFlags = new List<GungeonFlags>();
			GameStatsManager.s_pastFlags.Add(GungeonFlags.BOSSKILLED_ROGUE_PAST);
			GameStatsManager.s_pastFlags.Add(GungeonFlags.BOSSKILLED_CONVICT_PAST);
			GameStatsManager.s_pastFlags.Add(GungeonFlags.BOSSKILLED_SOLDIER_PAST);
			GameStatsManager.s_pastFlags.Add(GungeonFlags.BOSSKILLED_GUIDE_PAST);
		}
		if (GameStatsManager.s_npcFoyerFlags == null)
		{
			GameStatsManager.s_npcFoyerFlags = new List<GungeonFlags>();
			GameStatsManager.s_npcFoyerFlags.Add(GungeonFlags.META_SHOP_ACTIVE_IN_FOYER);
			GameStatsManager.s_npcFoyerFlags.Add(GungeonFlags.GUNSLING_KING_ACTIVE_IN_FOYER);
			GameStatsManager.s_npcFoyerFlags.Add(GungeonFlags.SORCERESS_ACTIVE_IN_FOYER);
			GameStatsManager.s_npcFoyerFlags.Add(GungeonFlags.LOST_ADVENTURER_ACTIVE_IN_FOYER);
			GameStatsManager.s_npcFoyerFlags.Add(GungeonFlags.TUTORIAL_TALKED_AFTER_RIVAL_KILLED);
			GameStatsManager.s_npcFoyerFlags.Add(GungeonFlags.SHOP_TRUCK_ACTIVE);
			GameStatsManager.s_npcFoyerFlags.Add(GungeonFlags.SHERPA_ACTIVE_IN_ELEVATOR_ROOM);
			GameStatsManager.s_npcFoyerFlags.Add(GungeonFlags.WINCHESTER_MET_PREVIOUSLY);
			GameStatsManager.s_npcFoyerFlags.Add(GungeonFlags.LEDGEGOBLIN_ACTIVE_IN_FOYER);
			GameStatsManager.s_npcFoyerFlags.Add(GungeonFlags.FRIFLE_ACTIVE_IN_FOYER);
		}
		if (GameStatsManager.s_frifleHuntFlags == null)
		{
			GameStatsManager.s_frifleHuntFlags = new List<GungeonFlags>();
			GameStatsManager.s_frifleHuntFlags.Add(GungeonFlags.FRIFLE_MONSTERHUNT_01_COMPLETE);
			GameStatsManager.s_frifleHuntFlags.Add(GungeonFlags.FRIFLE_MONSTERHUNT_02_COMPLETE);
			GameStatsManager.s_frifleHuntFlags.Add(GungeonFlags.FRIFLE_MONSTERHUNT_03_COMPLETE);
			GameStatsManager.s_frifleHuntFlags.Add(GungeonFlags.FRIFLE_MONSTERHUNT_04_COMPLETE);
			GameStatsManager.s_frifleHuntFlags.Add(GungeonFlags.FRIFLE_MONSTERHUNT_05_COMPLETE);
			GameStatsManager.s_frifleHuntFlags.Add(GungeonFlags.FRIFLE_MONSTERHUNT_06_COMPLETE);
			GameStatsManager.s_frifleHuntFlags.Add(GungeonFlags.FRIFLE_MONSTERHUNT_07_COMPLETE);
			GameStatsManager.s_frifleHuntFlags.Add(GungeonFlags.FRIFLE_MONSTERHUNT_08_COMPLETE);
			GameStatsManager.s_frifleHuntFlags.Add(GungeonFlags.FRIFLE_MONSTERHUNT_09_COMPLETE);
			GameStatsManager.s_frifleHuntFlags.Add(GungeonFlags.FRIFLE_MONSTERHUNT_10_COMPLETE);
			GameStatsManager.s_frifleHuntFlags.Add(GungeonFlags.FRIFLE_MONSTERHUNT_11_COMPLETE);
			GameStatsManager.s_frifleHuntFlags.Add(GungeonFlags.FRIFLE_MONSTERHUNT_12_COMPLETE);
			GameStatsManager.s_frifleHuntFlags.Add(GungeonFlags.FRIFLE_MONSTERHUNT_13_COMPLETE);
			GameStatsManager.s_frifleHuntFlags.Add(GungeonFlags.FRIFLE_MONSTERHUNT_14_COMPLETE);
			GameStatsManager.s_frifleHuntFlags.Add(GungeonFlags.FRIFLE_CORE_HUNTS_COMPLETE);
		}
	}

	// Token: 0x06007A5A RID: 31322 RVA: 0x0031087C File Offset: 0x0030EA7C
	public static bool Save()
	{
		GameManager.Instance.platformInterface.StoreStats();
		bool flag = false;
		try
		{
			flag = SaveManager.Save<GameStatsManager>(GameStatsManager.m_instance, SaveManager.GameSave, GameStatsManager.m_instance.PlaytimeMin, 0U, null);
		}
		catch (Exception ex)
		{
			Debug.LogErrorFormat("SAVE FAILED: {0}", new object[] { ex });
		}
		return flag;
	}

	// Token: 0x06007A5B RID: 31323 RVA: 0x003108F0 File Offset: 0x0030EAF0
	public static void DANGEROUS_ResetAllStats()
	{
		GameStatsManager.m_instance = new GameStatsManager();
		GameStatsManager.m_instance.huntProgress = new MonsterHuntProgress();
		GameStatsManager.m_instance.huntProgress.OnLoaded();
		SaveManager.DeleteAllBackups(SaveManager.GameSave, null);
		SaveManager.ResetSaveSlot = false;
	}

	// Token: 0x06007A5C RID: 31324 RVA: 0x00310940 File Offset: 0x0030EB40
	public void BeginNewSession(PlayerController player)
	{
		if (this.IsInSession)
		{
			BraveUtility.Log("MODIFYING CHARACTER FOR SESSION STATS", Color.red, BraveUtility.LogVerbosity.IMPORTANT);
			this.m_sessionCharacter = player.characterIdentity;
			this.m_sessionSynergies.Clear();
			if (!this.m_characterStats.ContainsKey(player.characterIdentity))
			{
				this.m_characterStats.Add(player.characterIdentity, new GameStats());
			}
			foreach (int num in player.startingGunIds)
			{
				Gun gun = PickupObjectDatabase.GetById(num) as Gun;
				EncounterTrackable component = gun.GetComponent<EncounterTrackable>();
				if (component && this.QueryEncounterableDifferentiator(component) < 1)
				{
					this.HandleEncounteredObject(component);
					this.SetEncounterableDifferentiator(component, 1);
				}
			}
		}
		else
		{
			BraveUtility.Log("CREATING NEW SESSION STATS", Color.red, BraveUtility.LogVerbosity.IMPORTANT);
			this.m_sessionCharacter = player.characterIdentity;
			this.m_sessionSynergies.Clear();
			this.m_sessionStats = new GameStats();
			this.m_savedSessionStats = new GameStats();
			if (!this.m_characterStats.ContainsKey(player.characterIdentity))
			{
				this.m_characterStats.Add(player.characterIdentity, new GameStats());
			}
			foreach (int num2 in player.startingGunIds)
			{
				Gun gun2 = PickupObjectDatabase.GetById(num2) as Gun;
				EncounterTrackable component2 = gun2.GetComponent<EncounterTrackable>();
				if (component2 && this.QueryEncounterableDifferentiator(component2) < 1)
				{
					this.HandleEncounteredObject(component2);
					this.SetEncounterableDifferentiator(component2, 1);
				}
			}
		}
		if (!this.GetFlag(GungeonFlags.TONIC_ACTIVE_IN_FOYER) && GameManager.IsTurboMode)
		{
			GameStatsManager.Instance.isTurboMode = false;
		}
		if (!this.GetFlag(GungeonFlags.BOWLER_ACTIVE_IN_FOYER) && GameStatsManager.Instance.rainbowRunToggled)
		{
			GameStatsManager.Instance.rainbowRunToggled = false;
		}
	}

	// Token: 0x06007A5D RID: 31325 RVA: 0x00310B74 File Offset: 0x0030ED74
	public void AssignMidGameSavedSessionStats(GameStats source)
	{
		if (!this.IsInSession)
		{
			return;
		}
		if (this.m_savedSessionStats != null)
		{
			this.m_savedSessionStats.AddStats(source);
		}
	}

	// Token: 0x06007A5E RID: 31326 RVA: 0x00310B9C File Offset: 0x0030ED9C
	public GameStats MoveSessionStatsToSavedSessionStats()
	{
		if (!this.IsInSession)
		{
			return null;
		}
		if (this.m_sessionStats != null)
		{
			this.m_sessionStats.SetMax(TrackedMaximums.MOST_ENEMIES_KILLED, this.m_sessionStats.GetStatValue(TrackedStats.ENEMIES_KILLED) + this.m_savedSessionStats.GetStatValue(TrackedStats.ENEMIES_KILLED));
			if (this.m_characterStats.ContainsKey(this.m_sessionCharacter))
			{
				this.m_characterStats[this.m_sessionCharacter].AddStats(this.m_sessionStats);
			}
			this.m_savedSessionStats.AddStats(this.m_sessionStats);
			this.m_sessionStats.ClearAllState();
		}
		return this.m_savedSessionStats;
	}

	// Token: 0x06007A5F RID: 31327 RVA: 0x00310C3C File Offset: 0x0030EE3C
	public void EndSession(bool recordSessionStats, bool decrementDifferentiator = true)
	{
		if (!this.IsInSession)
		{
			return;
		}
		if (GameManager.Instance.CurrentLevelOverrideState == GameManager.LevelOverrideState.FOYER || GameManager.Instance.CurrentLevelOverrideState == GameManager.LevelOverrideState.TUTORIAL)
		{
			decrementDifferentiator = false;
		}
		BraveUtility.Log("ENDING SESSION. RIGHT NOW: " + decrementDifferentiator, Color.red, BraveUtility.LogVerbosity.IMPORTANT);
		if (this.m_sessionStats != null)
		{
			if (recordSessionStats)
			{
				this.m_sessionStats.SetMax(TrackedMaximums.MOST_ENEMIES_KILLED, this.m_sessionStats.GetStatValue(TrackedStats.ENEMIES_KILLED));
				if (this.m_characterStats.ContainsKey(this.m_sessionCharacter))
				{
					this.m_characterStats[this.m_sessionCharacter].AddStats(this.m_sessionStats);
				}
				else
				{
					Debug.LogWarning("Character stats for " + this.m_sessionCharacter + " were not found; session stats are being thrown away.");
				}
			}
			this.m_sessionStats = null;
			this.m_savedSessionStats = null;
		}
		if (this.m_singleProcessedEncounterTrackables != null)
		{
			this.m_singleProcessedEncounterTrackables.Clear();
		}
		if (decrementDifferentiator)
		{
			foreach (string text in this.m_encounteredTrackables.Keys)
			{
				if (this.m_encounteredTrackables[text].differentiator > 0)
				{
					this.m_encounteredTrackables[text].differentiator = Mathf.Min(this.m_encounteredTrackables[text].differentiator - 1, 3);
				}
			}
			foreach (string text2 in this.m_encounteredRooms.Keys)
			{
				if (this.m_encounteredRooms[text2].differentiator > 0)
				{
					this.m_encounteredRooms[text2].differentiator = Mathf.Min(3, this.m_encounteredRooms[text2].differentiator - 1);
				}
			}
			List<string> list = new List<string>();
			foreach (string text3 in this.m_encounteredFlows.Keys)
			{
				if (this.m_encounteredFlows[text3] > 0)
				{
					list.Add(text3);
				}
			}
			foreach (string text4 in list)
			{
				this.m_encounteredFlows[text4] = this.m_encounteredFlows[text4] - 1;
			}
		}
	}

	// Token: 0x17001210 RID: 4624
	// (get) Token: 0x06007A60 RID: 31328 RVA: 0x00310F24 File Offset: 0x0030F124
	[fsIgnore]
	public bool IsInSession
	{
		get
		{
			return this.m_sessionStats != null;
		}
	}

	// Token: 0x06007A61 RID: 31329 RVA: 0x00310F34 File Offset: 0x0030F134
	public void ClearAllStatsGlobal()
	{
		this.m_sessionStats.ClearAllState();
		this.m_savedSessionStats.ClearAllState();
		if (this.m_numCharacters <= 0)
		{
			this.m_numCharacters = Enum.GetValues(typeof(PlayableCharacters)).Length;
		}
		for (int i = 0; i < this.m_numCharacters; i++)
		{
			GameStats gameStats;
			if (this.m_characterStats.TryGetValue((PlayableCharacters)i, out gameStats))
			{
				gameStats.ClearAllState();
			}
		}
	}

	// Token: 0x06007A62 RID: 31330 RVA: 0x00310FB0 File Offset: 0x0030F1B0
	public void ClearStatValueGlobal(TrackedStats stat)
	{
		this.m_sessionStats.SetStat(stat, 0f);
		this.m_savedSessionStats.SetStat(stat, 0f);
		if (this.m_numCharacters <= 0)
		{
			this.m_numCharacters = Enum.GetValues(typeof(PlayableCharacters)).Length;
		}
		for (int i = 0; i < this.m_numCharacters; i++)
		{
			GameStats gameStats;
			if (this.m_characterStats.TryGetValue((PlayableCharacters)i, out gameStats))
			{
				gameStats.SetStat(stat, 0f);
			}
		}
	}

	// Token: 0x06007A63 RID: 31331 RVA: 0x0031103C File Offset: 0x0030F23C
	public void UpdateMaximum(TrackedMaximums stat, float val)
	{
		if (float.IsNaN(val))
		{
			return;
		}
		if (float.IsInfinity(val))
		{
			return;
		}
		if (this.m_sessionStats == null)
		{
			return;
		}
		this.m_sessionStats.SetMax(stat, val);
	}

	// Token: 0x06007A64 RID: 31332 RVA: 0x00311070 File Offset: 0x0030F270
	public void SetStat(TrackedStats stat, float value)
	{
		if (float.IsNaN(value))
		{
			return;
		}
		if (float.IsInfinity(value))
		{
			return;
		}
		if (this.m_sessionStats == null)
		{
			return;
		}
		this.m_sessionStats.SetStat(stat, value);
		this.HandleStatAchievements(stat);
		this.HandleSetPlatformStat(stat, this.GetPlayerStatValue(stat));
	}

	// Token: 0x06007A65 RID: 31333 RVA: 0x003110C4 File Offset: 0x0030F2C4
	public void RegisterStatChange(TrackedStats stat, float value)
	{
		if (this.m_sessionStats == null)
		{
			Debug.LogError("No session stats active and we're registering a stat change!");
			return;
		}
		if (float.IsNaN(value))
		{
			return;
		}
		if (float.IsInfinity(value))
		{
			return;
		}
		if (Mathf.Abs(value) > 10000f)
		{
			return;
		}
		float playerStatValue = this.GetPlayerStatValue(stat);
		this.m_sessionStats.IncrementStat(stat, value);
		this.HandleStatAchievements(stat);
		this.HandleIncrementPlatformStat(stat, value, playerStatValue);
		GameManager.Instance.platformInterface.SendEvent(PlatformInterface.GetTrackedStatEventString(stat), 1);
	}

	// Token: 0x06007A66 RID: 31334 RVA: 0x0031114C File Offset: 0x0030F34C
	public void SetNextFlag(params GungeonFlags[] flagList)
	{
		for (int i = 0; i < flagList.Length; i++)
		{
			if (!this.GetFlag(flagList[i]))
			{
				this.SetFlag(flagList[i], true);
				return;
			}
		}
	}

	// Token: 0x06007A67 RID: 31335 RVA: 0x00311188 File Offset: 0x0030F388
	public void SetFlag(GungeonFlags flag, bool value)
	{
		if (flag == GungeonFlags.NONE)
		{
			Debug.LogError("Something is attempting to set a NONE save flag!");
			return;
		}
		if (value)
		{
			this.m_flags.Add(flag);
		}
		else
		{
			this.m_flags.Remove(flag);
		}
		if (value)
		{
			this.HandleFlagAchievements(flag);
		}
		if (value && flag == GungeonFlags.BOSSKILLED_DRAGUN && GameManager.Options.m_beastmode)
		{
			GameManager.Instance.platformInterface.AchievementUnlock(Achievement.COMPLETE_GAME_WITH_BEAST_MODE, 0);
		}
	}

	// Token: 0x06007A68 RID: 31336 RVA: 0x0031120C File Offset: 0x0030F40C
	public void SetCharacterSpecificFlag(CharacterSpecificGungeonFlags flag, bool value)
	{
		this.SetCharacterSpecificFlag(this.m_sessionCharacter, flag, value);
	}

	// Token: 0x06007A69 RID: 31337 RVA: 0x0031121C File Offset: 0x0030F41C
	public void SetCharacterSpecificFlag(PlayableCharacters character, CharacterSpecificGungeonFlags flag, bool value)
	{
		if (flag == CharacterSpecificGungeonFlags.NONE)
		{
			Debug.LogError("Something is attempting to set a NONE character-specific save flag!");
			return;
		}
		if (!this.m_characterStats.ContainsKey(character))
		{
			this.m_characterStats.Add(character, new GameStats());
		}
		if (this.m_sessionStats != null && this.m_sessionCharacter == character)
		{
			this.m_sessionStats.SetFlag(flag, value);
		}
		else
		{
			this.m_characterStats[character].SetFlag(flag, value);
		}
		if (flag == CharacterSpecificGungeonFlags.KILLED_PAST)
		{
			PlayerController playerController = GameManager.Instance.PrimaryPlayer;
			if (character == PlayableCharacters.CoopCultist)
			{
				playerController = GameManager.Instance.SecondaryPlayer;
			}
			else if (playerController && playerController.IsTemporaryEeveeForUnlock)
			{
				GameStatsManager.Instance.SetFlag(GungeonFlags.FLAG_EEVEE_UNLOCKED, true);
			}
			if (playerController && playerController.IsUsingAlternateCostume)
			{
				if (this.m_sessionStats != null && this.m_sessionCharacter == character)
				{
					this.m_sessionStats.SetFlag(CharacterSpecificGungeonFlags.KILLED_PAST_ALTERNATE_COSTUME, value);
				}
				else
				{
					this.m_characterStats[character].SetFlag(CharacterSpecificGungeonFlags.KILLED_PAST_ALTERNATE_COSTUME, value);
				}
				if (value)
				{
					this.SetFlag(GungeonFlags.ITEMSPECIFIC_ALTERNATE_GUNS_UNLOCKED, true);
				}
			}
		}
	}

	// Token: 0x06007A6A RID: 31338 RVA: 0x00311358 File Offset: 0x0030F558
	public void SetPersistentStringLastIndex(string key, int value)
	{
		this.m_persistentStringsLastIndices[key] = value;
	}

	// Token: 0x06007A6B RID: 31339 RVA: 0x00311368 File Offset: 0x0030F568
	public void ForceUnlock(string encounterGuid)
	{
		this.forcedUnlocks.Add(encounterGuid);
	}

	// Token: 0x06007A6C RID: 31340 RVA: 0x00311378 File Offset: 0x0030F578
	public bool IsForceUnlocked(string encounterGuid)
	{
		return this.forcedUnlocks.Contains(encounterGuid);
	}

	// Token: 0x06007A6D RID: 31341 RVA: 0x00311388 File Offset: 0x0030F588
	public float GetPlayerMaximum(TrackedMaximums stat)
	{
		if (this.m_numCharacters <= 0)
		{
			this.m_numCharacters = Enum.GetValues(typeof(PlayableCharacters)).Length;
		}
		float num = 0f;
		if (this.m_sessionStats != null)
		{
			num = Mathf.Max(new float[]
			{
				num,
				this.m_sessionStats.GetMaximumValue(stat),
				this.m_savedSessionStats.GetMaximumValue(stat)
			});
		}
		for (int i = 0; i < this.m_numCharacters; i++)
		{
			GameStats gameStats;
			if (this.m_characterStats.TryGetValue((PlayableCharacters)i, out gameStats))
			{
				num = Mathf.Max(num, gameStats.GetMaximumValue(stat));
			}
		}
		return num;
	}

	// Token: 0x06007A6E RID: 31342 RVA: 0x00311434 File Offset: 0x0030F634
	public float GetSessionStatValue(TrackedStats stat)
	{
		return this.m_sessionStats.GetStatValue(stat) + this.m_savedSessionStats.GetStatValue(stat);
	}

	// Token: 0x06007A6F RID: 31343 RVA: 0x00311450 File Offset: 0x0030F650
	public float GetCharacterStatValue(TrackedStats stat)
	{
		return this.GetCharacterStatValue(this.GetCurrentCharacter(), stat);
	}

	// Token: 0x06007A70 RID: 31344 RVA: 0x00311460 File Offset: 0x0030F660
	public float GetCharacterStatValue(PlayableCharacters character, TrackedStats stat)
	{
		float num = 0f;
		if (this.m_sessionCharacter == character)
		{
			num += this.m_sessionStats.GetStatValue(stat);
		}
		if (this.m_characterStats.ContainsKey(character))
		{
			num += this.m_characterStats[character].GetStatValue(stat);
		}
		return num;
	}

	// Token: 0x06007A71 RID: 31345 RVA: 0x003114B8 File Offset: 0x0030F6B8
	public float GetPlayerStatValue(TrackedStats stat)
	{
		if (this.m_numCharacters <= 0)
		{
			this.m_numCharacters = Enum.GetValues(typeof(PlayableCharacters)).Length;
		}
		float num = 0f;
		if (this.m_sessionStats != null)
		{
			num += this.m_sessionStats.GetStatValue(stat);
		}
		for (int i = 0; i < this.m_numCharacters; i++)
		{
			GameStats gameStats;
			if (this.m_characterStats.TryGetValue((PlayableCharacters)i, out gameStats))
			{
				num += gameStats.GetStatValue(stat);
			}
		}
		return num;
	}

	// Token: 0x06007A72 RID: 31346 RVA: 0x00311540 File Offset: 0x0030F740
	public bool TestPastBeaten(PlayableCharacters character)
	{
		switch (character)
		{
		case PlayableCharacters.Pilot:
			return this.GetCharacterSpecificFlag(character, CharacterSpecificGungeonFlags.KILLED_PAST);
		case PlayableCharacters.Convict:
			return this.GetCharacterSpecificFlag(character, CharacterSpecificGungeonFlags.KILLED_PAST);
		case PlayableCharacters.Robot:
			return this.GetCharacterSpecificFlag(character, CharacterSpecificGungeonFlags.KILLED_PAST);
		case PlayableCharacters.Soldier:
			return this.GetCharacterSpecificFlag(character, CharacterSpecificGungeonFlags.KILLED_PAST);
		case PlayableCharacters.Guide:
			return this.GetCharacterSpecificFlag(character, CharacterSpecificGungeonFlags.KILLED_PAST);
		case PlayableCharacters.CoopCultist:
			return this.GetCharacterSpecificFlag(character, CharacterSpecificGungeonFlags.KILLED_PAST);
		case PlayableCharacters.Bullet:
			return this.GetCharacterSpecificFlag(character, CharacterSpecificGungeonFlags.KILLED_PAST);
		}
		return false;
	}

	// Token: 0x06007A73 RID: 31347 RVA: 0x003115D8 File Offset: 0x0030F7D8
	public bool AllCorePastsBeaten()
	{
		return this.GetCharacterSpecificFlag(PlayableCharacters.Pilot, CharacterSpecificGungeonFlags.KILLED_PAST) && this.GetCharacterSpecificFlag(PlayableCharacters.Convict, CharacterSpecificGungeonFlags.KILLED_PAST) && this.GetCharacterSpecificFlag(PlayableCharacters.Soldier, CharacterSpecificGungeonFlags.KILLED_PAST) && this.GetCharacterSpecificFlag(PlayableCharacters.Guide, CharacterSpecificGungeonFlags.KILLED_PAST);
	}

	// Token: 0x06007A74 RID: 31348 RVA: 0x00311628 File Offset: 0x0030F828
	public bool CheckLameyCostumeUnlocked()
	{
		return false;
	}

	// Token: 0x06007A75 RID: 31349 RVA: 0x0031162C File Offset: 0x0030F82C
	public bool CheckGunslingerCostumeUnlocked()
	{
		bool flag = GameStatsManager.Instance.GetFlag(GungeonFlags.BOSSKILLED_DRAGUN);
		flag &= GameStatsManager.Instance.GetFlag(GungeonFlags.BOSSKILLED_LICH);
		flag &= GameStatsManager.Instance.GetCharacterSpecificFlag(PlayableCharacters.Bullet, CharacterSpecificGungeonFlags.KILLED_PAST);
		flag &= GameStatsManager.Instance.GetCharacterSpecificFlag(PlayableCharacters.Convict, CharacterSpecificGungeonFlags.KILLED_PAST);
		flag &= GameStatsManager.Instance.GetCharacterSpecificFlag(PlayableCharacters.Guide, CharacterSpecificGungeonFlags.KILLED_PAST);
		flag &= GameStatsManager.Instance.GetCharacterSpecificFlag(PlayableCharacters.Pilot, CharacterSpecificGungeonFlags.KILLED_PAST);
		flag &= GameStatsManager.Instance.GetCharacterSpecificFlag(PlayableCharacters.Robot, CharacterSpecificGungeonFlags.KILLED_PAST);
		return flag & GameStatsManager.Instance.GetCharacterSpecificFlag(PlayableCharacters.Soldier, CharacterSpecificGungeonFlags.KILLED_PAST);
	}

	// Token: 0x06007A76 RID: 31350 RVA: 0x003116D0 File Offset: 0x0030F8D0
	public int GetNumberPastsBeaten()
	{
		int num = 0;
		if (this.GetCharacterSpecificFlag(PlayableCharacters.Pilot, CharacterSpecificGungeonFlags.KILLED_PAST))
		{
			num++;
		}
		if (this.GetCharacterSpecificFlag(PlayableCharacters.Convict, CharacterSpecificGungeonFlags.KILLED_PAST))
		{
			num++;
		}
		if (this.GetCharacterSpecificFlag(PlayableCharacters.Soldier, CharacterSpecificGungeonFlags.KILLED_PAST))
		{
			num++;
		}
		if (this.GetCharacterSpecificFlag(PlayableCharacters.Guide, CharacterSpecificGungeonFlags.KILLED_PAST))
		{
			num++;
		}
		if (this.GetCharacterSpecificFlag(PlayableCharacters.Robot, CharacterSpecificGungeonFlags.KILLED_PAST))
		{
			num++;
		}
		if (this.GetCharacterSpecificFlag(PlayableCharacters.Bullet, CharacterSpecificGungeonFlags.KILLED_PAST))
		{
			num++;
		}
		return num;
	}

	// Token: 0x06007A77 RID: 31351 RVA: 0x00311760 File Offset: 0x0030F960
	public bool AnyPastBeaten()
	{
		return this.GetCharacterSpecificFlag(PlayableCharacters.Pilot, CharacterSpecificGungeonFlags.KILLED_PAST) || this.GetCharacterSpecificFlag(PlayableCharacters.Convict, CharacterSpecificGungeonFlags.KILLED_PAST) || this.GetCharacterSpecificFlag(PlayableCharacters.Soldier, CharacterSpecificGungeonFlags.KILLED_PAST) || this.GetCharacterSpecificFlag(PlayableCharacters.Guide, CharacterSpecificGungeonFlags.KILLED_PAST) || this.GetCharacterSpecificFlag(PlayableCharacters.Robot, CharacterSpecificGungeonFlags.KILLED_PAST) || this.GetCharacterSpecificFlag(PlayableCharacters.Bullet, CharacterSpecificGungeonFlags.KILLED_PAST);
	}

	// Token: 0x06007A78 RID: 31352 RVA: 0x003117D4 File Offset: 0x0030F9D4
	public int GetNumberOfCompanionsUnlocked()
	{
		int num = 0;
		int num2 = 0;
		for (int i = 0; i < PickupObjectDatabase.Instance.Objects.Count; i++)
		{
			PickupObject pickupObject = PickupObjectDatabase.Instance.Objects[i];
			if (pickupObject && pickupObject is CompanionItem)
			{
				if (pickupObject.quality != PickupObject.ItemQuality.EXCLUDED)
				{
					if (pickupObject.contentSource != ContentSource.EXCLUDED)
					{
						num++;
						if (!pickupObject.encounterTrackable || pickupObject.encounterTrackable.PrerequisitesMet())
						{
							num2++;
						}
					}
				}
			}
		}
		return num2;
	}

	// Token: 0x06007A79 RID: 31353 RVA: 0x00311884 File Offset: 0x0030FA84
	public bool HasPast(PlayableCharacters id)
	{
		return id != PlayableCharacters.Eevee && id != PlayableCharacters.Gunslinger && id != PlayableCharacters.Cosmonaut;
	}

	// Token: 0x06007A7A RID: 31354 RVA: 0x003118AC File Offset: 0x0030FAAC
	public bool GetFlag(GungeonFlags flag)
	{
		if (flag == GungeonFlags.NONE)
		{
			Debug.LogError("Something is attempting to get a NONE save flag!");
			return false;
		}
		return this.m_flags.Contains(flag);
	}

	// Token: 0x06007A7B RID: 31355 RVA: 0x003118CC File Offset: 0x0030FACC
	public bool GetCharacterSpecificFlag(CharacterSpecificGungeonFlags flag)
	{
		return this.GetCharacterSpecificFlag(this.m_sessionCharacter, flag);
	}

	// Token: 0x06007A7C RID: 31356 RVA: 0x003118DC File Offset: 0x0030FADC
	public bool GetCharacterSpecificFlag(PlayableCharacters character, CharacterSpecificGungeonFlags flag)
	{
		if (flag == CharacterSpecificGungeonFlags.NONE)
		{
			Debug.LogError("Something is attempting to get a NONE character-specific save flag!");
			return false;
		}
		if (this.m_sessionStats != null && this.m_sessionCharacter == character)
		{
			if (this.m_sessionStats.GetFlag(flag))
			{
				return true;
			}
			if (this.m_savedSessionStats.GetFlag(flag))
			{
				return true;
			}
		}
		GameStats gameStats;
		return this.m_characterStats.TryGetValue(character, out gameStats) && gameStats.GetFlag(flag);
	}

	// Token: 0x06007A7D RID: 31357 RVA: 0x00311954 File Offset: 0x0030FB54
	public int GetPersistentStringLastIndex(string key)
	{
		int num;
		if (this.m_persistentStringsLastIndices.TryGetValue(key, out num))
		{
			return num;
		}
		return -1;
	}

	// Token: 0x06007A7E RID: 31358 RVA: 0x00311978 File Offset: 0x0030FB78
	public void EncounterFlow(string flowName)
	{
		Debug.Log("ENCOUNTERING FLOW: " + flowName);
		if (!this.m_encounteredFlows.ContainsKey(flowName))
		{
			this.m_encounteredFlows.Add(flowName, 2);
		}
		else
		{
			this.m_encounteredFlows[flowName] = this.m_encounteredFlows[flowName] + 2;
		}
	}

	// Token: 0x06007A7F RID: 31359 RVA: 0x003119D4 File Offset: 0x0030FBD4
	public int QueryFlowDifferentiator(string flowName)
	{
		if (BraveRandom.IgnoreGenerationDifferentiator)
		{
			return 0;
		}
		if (this.m_encounteredFlows.ContainsKey(flowName))
		{
			int num = this.m_encounteredFlows[flowName];
			if (num < 0 || num > 1000000)
			{
				this.m_encounteredFlows[flowName] = 0;
				num = 0;
			}
			return num;
		}
		return 0;
	}

	// Token: 0x06007A80 RID: 31360 RVA: 0x00311A30 File Offset: 0x0030FC30
	public int QueryRoomEncountered(PrototypeDungeonRoom prototype)
	{
		if (this.m_encounteredRooms.ContainsKey(prototype.GUID))
		{
			return this.m_encounteredRooms[prototype.GUID].encounterCount;
		}
		return 0;
	}

	// Token: 0x06007A81 RID: 31361 RVA: 0x00311A60 File Offset: 0x0030FC60
	public int QueryRoomEncountered(string GUID)
	{
		if (string.IsNullOrEmpty(GUID))
		{
			return 0;
		}
		if (this.m_encounteredRooms.ContainsKey(GUID))
		{
			return this.m_encounteredRooms[GUID].encounterCount;
		}
		return 0;
	}

	// Token: 0x06007A82 RID: 31362 RVA: 0x00311A94 File Offset: 0x0030FC94
	public int QueryRoomDifferentiator(PrototypeDungeonRoom prototype)
	{
		if (BraveRandom.IgnoreGenerationDifferentiator)
		{
			return 0;
		}
		if (string.IsNullOrEmpty(prototype.GUID))
		{
			return 0;
		}
		if (this.m_encounteredRooms.ContainsKey(prototype.GUID))
		{
			int num = this.m_encounteredRooms[prototype.GUID].differentiator;
			if (num < 0 || num > 1000000)
			{
				this.m_encounteredRooms[prototype.GUID].differentiator = 0;
				num = 0;
			}
			return num;
		}
		return 0;
	}

	// Token: 0x06007A83 RID: 31363 RVA: 0x00311B1C File Offset: 0x0030FD1C
	public void ClearAllDifferentiatorHistory(bool doYouReallyWantToDoThis = false)
	{
		this.ClearAllPickupDifferentiators(doYouReallyWantToDoThis);
		this.ClearAllRoomDifferentiators();
	}

	// Token: 0x06007A84 RID: 31364 RVA: 0x00311B2C File Offset: 0x0030FD2C
	public void ClearAllPickupDifferentiators(bool doYouReallyWantToDoThis = false)
	{
		if (doYouReallyWantToDoThis)
		{
			foreach (string text in this.m_encounteredTrackables.Keys)
			{
				this.m_encounteredTrackables[text].differentiator = 0;
			}
		}
	}

	// Token: 0x06007A85 RID: 31365 RVA: 0x00311BA0 File Offset: 0x0030FDA0
	public void ClearAllEncounterableHistory(bool doYouReallyWantToDoThis = false)
	{
		if (doYouReallyWantToDoThis)
		{
			foreach (string text in this.m_encounteredTrackables.Keys)
			{
				this.m_encounteredTrackables[text].differentiator = 0;
			}
			this.m_encounteredTrackables.Clear();
		}
	}

	// Token: 0x06007A86 RID: 31366 RVA: 0x00311C20 File Offset: 0x0030FE20
	public int QueryEncounterable(EncounterTrackable et)
	{
		if (this.m_encounteredTrackables.ContainsKey(et.EncounterGuid))
		{
			return this.m_encounteredTrackables[et.EncounterGuid].encounterCount;
		}
		return 0;
	}

	// Token: 0x06007A87 RID: 31367 RVA: 0x00311C50 File Offset: 0x0030FE50
	public int QueryEncounterable(EncounterDatabaseEntry et)
	{
		if (this.m_encounteredTrackables.ContainsKey(et.myGuid))
		{
			return this.m_encounteredTrackables[et.myGuid].encounterCount;
		}
		return 0;
	}

	// Token: 0x06007A88 RID: 31368 RVA: 0x00311C80 File Offset: 0x0030FE80
	public int QueryEncounterable(string encounterGuid)
	{
		if (this.m_encounteredTrackables.ContainsKey(encounterGuid))
		{
			return this.m_encounteredTrackables[encounterGuid].encounterCount;
		}
		return 0;
	}

	// Token: 0x06007A89 RID: 31369 RVA: 0x00311CA8 File Offset: 0x0030FEA8
	public void SetEncounterableDifferentiator(EncounterTrackable et, int val)
	{
		if (et.IgnoreDifferentiator)
		{
			return;
		}
		if (this.m_encounteredTrackables.ContainsKey(et.EncounterGuid))
		{
			this.m_encounteredTrackables[et.EncounterGuid].differentiator = val;
		}
	}

	// Token: 0x06007A8A RID: 31370 RVA: 0x00311CE4 File Offset: 0x0030FEE4
	public void MarkEncounterableAnnounced(EncounterDatabaseEntry et)
	{
		if (this.m_encounteredTrackables.ContainsKey(et.myGuid))
		{
			this.m_encounteredTrackables[et.myGuid].hasBeenAmmonomiconAnnounced = true;
		}
		else
		{
			EncounteredObjectData encounteredObjectData = new EncounteredObjectData();
			encounteredObjectData.hasBeenAmmonomiconAnnounced = true;
			this.m_encounteredTrackables.Add(et.myGuid, encounteredObjectData);
		}
	}

	// Token: 0x06007A8B RID: 31371 RVA: 0x00311D44 File Offset: 0x0030FF44
	public bool QueryEncounterableAnnouncement(string guid)
	{
		return this.m_encounteredTrackables.ContainsKey(guid) && this.m_encounteredTrackables[guid].hasBeenAmmonomiconAnnounced;
	}

	// Token: 0x06007A8C RID: 31372 RVA: 0x00311D6C File Offset: 0x0030FF6C
	public int QueryEncounterableDifferentiator(EncounterTrackable et)
	{
		return this.QueryEncounterableDifferentiator(et.EncounterGuid, et.IgnoreDifferentiator);
	}

	// Token: 0x06007A8D RID: 31373 RVA: 0x00311D80 File Offset: 0x0030FF80
	public int QueryEncounterableDifferentiator(EncounterDatabaseEntry encounterData)
	{
		return this.QueryEncounterableDifferentiator(encounterData.myGuid, encounterData.IgnoreDifferentiator);
	}

	// Token: 0x06007A8E RID: 31374 RVA: 0x00311D94 File Offset: 0x0030FF94
	public int QueryEncounterableDifferentiator(string encounterGuid, bool ignoreDifferentiator)
	{
		if (!this.m_encounteredTrackables.ContainsKey(encounterGuid))
		{
			return 0;
		}
		if (ignoreDifferentiator)
		{
			return 0;
		}
		int num = this.m_encounteredTrackables[encounterGuid].differentiator;
		if (num < 0 || num > 1000000)
		{
			this.m_encounteredTrackables[encounterGuid].differentiator = 0;
			num = 0;
		}
		return num;
	}

	// Token: 0x06007A8F RID: 31375 RVA: 0x00311DF8 File Offset: 0x0030FFF8
	public void ClearAllRoomDifferentiators()
	{
		foreach (string text in this.m_encounteredRooms.Keys)
		{
			this.m_encounteredRooms[text].differentiator = 0;
		}
	}

	// Token: 0x06007A90 RID: 31376 RVA: 0x00311E64 File Offset: 0x00310064
	public void HandleEncounteredRoom(RuntimePrototypeRoomData prototype)
	{
		if (prototype == null)
		{
			return;
		}
		if (string.IsNullOrEmpty(prototype.GUID))
		{
			return;
		}
		if (GameManager.Instance.CurrentGameMode == GameManager.GameMode.BOSSRUSH || GameManager.Instance.CurrentGameMode == GameManager.GameMode.SUPERBOSSRUSH)
		{
			return;
		}
		if (this.m_encounteredRooms.ContainsKey(prototype.GUID))
		{
			EncounteredObjectData encounteredObjectData = this.m_encounteredRooms[prototype.GUID];
		}
		else
		{
			EncounteredObjectData encounteredObjectData = new EncounteredObjectData();
			this.m_encounteredRooms.Add(prototype.GUID, encounteredObjectData);
		}
		this.m_encounteredRooms[prototype.GUID].encounterCount++;
		this.m_encounteredRooms[prototype.GUID].differentiator += 2;
	}

	// Token: 0x06007A91 RID: 31377 RVA: 0x00311F30 File Offset: 0x00310130
	public void HandleEncounteredObjectRaw(string encounterGuid)
	{
		EncounteredObjectData encounteredObjectData;
		if (this.m_encounteredTrackables.ContainsKey(encounterGuid))
		{
			encounteredObjectData = this.m_encounteredTrackables[encounterGuid];
		}
		else
		{
			encounteredObjectData = new EncounteredObjectData();
			this.m_encounteredTrackables.Add(encounterGuid, encounteredObjectData);
		}
		encounteredObjectData.encounterCount++;
	}

	// Token: 0x06007A92 RID: 31378 RVA: 0x00311F84 File Offset: 0x00310184
	public void SingleIncrementDifferentiator(EncounterTrackable et)
	{
		if (this.m_encounteredTrackables.ContainsKey(et.EncounterGuid))
		{
			EncounteredObjectData encounteredObjectData = this.m_encounteredTrackables[et.EncounterGuid];
		}
		else
		{
			EncounteredObjectData encounteredObjectData = new EncounteredObjectData();
			this.m_encounteredTrackables.Add(et.EncounterGuid, encounteredObjectData);
		}
		if (et && !string.IsNullOrEmpty(et.EncounterGuid) && !this.m_singleProcessedEncounterTrackables.Contains(et.EncounterGuid))
		{
			this.m_singleProcessedEncounterTrackables.Add(et.EncounterGuid);
		}
		if (!et.IgnoreDifferentiator)
		{
			this.m_encounteredTrackables[et.EncounterGuid].differentiator++;
		}
	}

	// Token: 0x06007A93 RID: 31379 RVA: 0x00312044 File Offset: 0x00310244
	public int GetNumberOfSynergiesEncounteredThisRun()
	{
		return this.m_sessionSynergies.Count;
	}

	// Token: 0x06007A94 RID: 31380 RVA: 0x00312054 File Offset: 0x00310254
	public void HandleEncounteredSynergy(int index)
	{
		if (index > 0)
		{
			if (!this.m_encounteredSynergiesByID.ContainsKey(index))
			{
				this.m_encounteredSynergiesByID.Add(index, 0);
			}
			this.m_sessionSynergies.Add(index);
			this.m_encounteredSynergiesByID[index] = this.m_encounteredSynergiesByID[index] + 1;
		}
	}

	// Token: 0x06007A95 RID: 31381 RVA: 0x003120B0 File Offset: 0x003102B0
	public void HandleEncounteredObject(EncounterTrackable et)
	{
		EncounteredObjectData encounteredObjectData;
		if (this.m_encounteredTrackables.ContainsKey(et.EncounterGuid))
		{
			encounteredObjectData = this.m_encounteredTrackables[et.EncounterGuid];
		}
		else
		{
			encounteredObjectData = new EncounteredObjectData();
			this.m_encounteredTrackables.Add(et.EncounterGuid, encounteredObjectData);
		}
		encounteredObjectData.encounterCount++;
		if (!et.IgnoreDifferentiator)
		{
			if (this.m_singleProcessedEncounterTrackables != null && this.m_singleProcessedEncounterTrackables.Contains(et.EncounterGuid))
			{
				this.m_encounteredTrackables[et.EncounterGuid].differentiator++;
			}
			else
			{
				this.m_encounteredTrackables[et.EncounterGuid].differentiator += 2;
			}
		}
	}

	// Token: 0x06007A96 RID: 31382 RVA: 0x00312180 File Offset: 0x00310380
	public GlobalDungeonData.ValidTilesets GetCurrentRobotArmTileset()
	{
		switch (this.CurrentRobotArmFloor)
		{
		case 0:
			return (GlobalDungeonData.ValidTilesets)(-1);
		case 1:
			return GlobalDungeonData.ValidTilesets.CASTLEGEON;
		case 2:
			return GlobalDungeonData.ValidTilesets.GUNGEON;
		case 3:
			return GlobalDungeonData.ValidTilesets.MINEGEON;
		case 4:
			return GlobalDungeonData.ValidTilesets.CATACOMBGEON;
		case 5:
			return GlobalDungeonData.ValidTilesets.FORGEGEON;
		default:
			return (GlobalDungeonData.ValidTilesets)(-1);
		}
	}

	// Token: 0x06007A97 RID: 31383 RVA: 0x003121C8 File Offset: 0x003103C8
	private PlayableCharacters GetCurrentCharacter()
	{
		return GameManager.Instance.PrimaryPlayer.characterIdentity;
	}

	// Token: 0x06007A98 RID: 31384 RVA: 0x003121DC File Offset: 0x003103DC
	public void HandleStatAchievements(TrackedStats stat)
	{
		if (stat == TrackedStats.GUNBERS_MUNCHED && this.GetPlayerStatValue(stat) >= 20f)
		{
			this.SetFlag(GungeonFlags.MUNCHER_MUTANT_ARM_UNLOCKED, true);
			this.SetFlag(GungeonFlags.MUNCHER_COLD45_UNLOCKED, true);
		}
		if (stat == TrackedStats.TIMES_REACHED_SEWERS)
		{
			if (this.GetPlayerStatValue(stat) >= 1f)
			{
				GameManager.Instance.platformInterface.AchievementUnlock(Achievement.REACH_SEWERS, 0);
			}
		}
		else if (stat == TrackedStats.TIMES_REACHED_CATHEDRAL)
		{
			if (this.GetPlayerStatValue(stat) >= 1f)
			{
				GameManager.Instance.platformInterface.AchievementUnlock(Achievement.REACH_CATHEDRAL, 0);
			}
		}
		else if (stat == TrackedStats.TIMES_CLEARED_CASTLE)
		{
			if (this.GetPlayerStatValue(stat) >= 50f)
			{
				GameManager.Instance.platformInterface.AchievementUnlock(Achievement.BEAT_FLOOR_ONE_MULTI, 0);
			}
		}
		else if (stat == TrackedStats.TIMES_CLEARED_GUNGEON)
		{
			if (this.GetPlayerStatValue(stat) >= 40f)
			{
				GameManager.Instance.platformInterface.AchievementUnlock(Achievement.BEAT_FLOOR_TWO_MULTI, 0);
			}
		}
		else if (stat == TrackedStats.TIMES_CLEARED_MINES)
		{
			if (this.GetPlayerStatValue(stat) >= 30f)
			{
				GameManager.Instance.platformInterface.AchievementUnlock(Achievement.BEAT_FLOOR_THREE_MULTI, 0);
			}
		}
		else if (stat == TrackedStats.TIMES_CLEARED_CATACOMBS)
		{
			if (this.GetPlayerStatValue(stat) >= 20f)
			{
				GameManager.Instance.platformInterface.AchievementUnlock(Achievement.BEAT_FLOOR_FOUR_MULTI, 0);
			}
		}
		else if (stat == TrackedStats.TIMES_CLEARED_FORGE)
		{
			if (this.GetPlayerStatValue(stat) >= 10f)
			{
				GameManager.Instance.platformInterface.AchievementUnlock(Achievement.BEAT_FLOOR_FIVE_MULTI, 0);
			}
		}
		else if (stat == TrackedStats.WINCHESTER_GAMES_ACED)
		{
			if (this.GetPlayerStatValue(stat) >= 3f)
			{
				GameManager.Instance.platformInterface.AchievementUnlock(Achievement.ACE_WINCHESTER_MULTI, 0);
			}
		}
		else if (stat == TrackedStats.GUNSLING_KING_CHALLENGES_COMPLETED)
		{
			if (this.GetPlayerStatValue(stat) >= 5f)
			{
				GameManager.Instance.platformInterface.AchievementUnlock(Achievement.COMPLETE_GUNSLING_MULTI, 0);
			}
		}
		else if (stat == TrackedStats.META_CURRENCY_SPENT_AT_META_SHOP)
		{
			if (this.GetPlayerStatValue(stat) >= 100f)
			{
				GameManager.Instance.platformInterface.AchievementUnlock(Achievement.SPEND_META_CURRENCY, 0);
			}
		}
		else if (stat == TrackedStats.MERCHANT_ITEMS_STOLEN)
		{
			if (this.GetPlayerStatValue(stat) >= 10f)
			{
				GameManager.Instance.platformInterface.AchievementUnlock(Achievement.STEAL_MULTI, 0);
			}
		}
		else if (stat == TrackedStats.ENEMIES_KILLED_WITH_CHANDELIERS)
		{
			if (this.GetPlayerStatValue(stat) >= 100f)
			{
				GameManager.Instance.platformInterface.AchievementUnlock(Achievement.KILL_WITH_CHANDELIER_MULTI, 0);
			}
		}
		else if (stat == TrackedStats.ENEMIES_KILLED_WHILE_IN_CARTS)
		{
			if (this.GetPlayerStatValue(stat) >= 100f)
			{
				GameManager.Instance.platformInterface.AchievementUnlock(Achievement.KILL_IN_MINE_CART_MULTI, 0);
			}
		}
		else if (stat == TrackedStats.ENEMIES_KILLED_WITH_PITS)
		{
			if (this.GetPlayerStatValue(stat) >= 100f)
			{
				GameManager.Instance.platformInterface.AchievementUnlock(Achievement.KILL_WITH_PITS_MULTI, 0);
			}
		}
		else if (stat == TrackedStats.TABLES_FLIPPED)
		{
			if (this.GetPlayerStatValue(stat) >= 500f)
			{
				GameManager.Instance.platformInterface.AchievementUnlock(Achievement.FLIP_TABLES_MULTI, 0);
			}
		}
		else if (stat == TrackedStats.MERCHANT_PURCHASES_GOOP)
		{
			this.UpdateAllNpcsAchievement();
		}
	}

	// Token: 0x06007A99 RID: 31385 RVA: 0x003124F8 File Offset: 0x003106F8
	private PlatformStat? ConvertToPlatformStat(TrackedStats stat)
	{
		if (stat == TrackedStats.META_CURRENCY_SPENT_AT_META_SHOP)
		{
			return new PlatformStat?(PlatformStat.META_SPENT_AT_STORE);
		}
		if (stat == TrackedStats.TIMES_CLEARED_CASTLE)
		{
			return new PlatformStat?(PlatformStat.FLOOR_ONE_CLEARS);
		}
		if (stat == TrackedStats.TIMES_CLEARED_GUNGEON)
		{
			return new PlatformStat?(PlatformStat.FLOOR_TWO_CLEARS);
		}
		if (stat == TrackedStats.TIMES_CLEARED_MINES)
		{
			return new PlatformStat?(PlatformStat.FLOOR_THREE_CLEARS);
		}
		if (stat == TrackedStats.TIMES_CLEARED_CATACOMBS)
		{
			return new PlatformStat?(PlatformStat.FLOOR_FOUR_CLEARS);
		}
		if (stat == TrackedStats.TIMES_CLEARED_FORGE)
		{
			return new PlatformStat?(PlatformStat.FLOOR_FIVE_CLEARS);
		}
		if (stat == TrackedStats.WINCHESTER_GAMES_ACED)
		{
			return new PlatformStat?(PlatformStat.WINCHESTER_ACED);
		}
		if (stat == TrackedStats.GUNSLING_KING_CHALLENGES_COMPLETED)
		{
			return new PlatformStat?(PlatformStat.GUNSLING_COMPLETED);
		}
		if (stat == TrackedStats.MERCHANT_ITEMS_STOLEN)
		{
			return new PlatformStat?(PlatformStat.ITEMS_STOLEN);
		}
		if (stat == TrackedStats.ENEMIES_KILLED_WITH_CHANDELIERS)
		{
			return new PlatformStat?(PlatformStat.CHANDELIER_KILLS);
		}
		if (stat == TrackedStats.ENEMIES_KILLED_WHILE_IN_CARTS)
		{
			return new PlatformStat?(PlatformStat.MINECART_KILLS);
		}
		if (stat == TrackedStats.ENEMIES_KILLED_WITH_PITS)
		{
			return new PlatformStat?(PlatformStat.PIT_KILLS);
		}
		if (stat == TrackedStats.TABLES_FLIPPED)
		{
			return new PlatformStat?(PlatformStat.TABLES_FLIPPED);
		}
		return null;
	}

	// Token: 0x06007A9A RID: 31386 RVA: 0x003125D8 File Offset: 0x003107D8
	private void HandleSetPlatformStat(TrackedStats stat, float value)
	{
		PlatformStat? platformStat = this.ConvertToPlatformStat(stat);
		if (platformStat != null)
		{
			GameManager.Instance.platformInterface.SetStat(platformStat.Value, Mathf.RoundToInt(value));
		}
	}

	// Token: 0x06007A9B RID: 31387 RVA: 0x00312618 File Offset: 0x00310818
	private void HandleIncrementPlatformStat(TrackedStats stat, float delta, float previousValue)
	{
		PlatformStat? platformStat = this.ConvertToPlatformStat(stat);
		if (platformStat != null)
		{
			GameManager.Instance.platformInterface.SetStat(platformStat.Value, Mathf.RoundToInt(previousValue));
			GameManager.Instance.platformInterface.IncrementStat(platformStat.Value, Mathf.RoundToInt(delta));
		}
	}

	// Token: 0x06007A9C RID: 31388 RVA: 0x00312674 File Offset: 0x00310874
	public void HandleFlagAchievements(GungeonFlags flag)
	{
		if (flag == GungeonFlags.BOSSKILLED_ROGUE_PAST)
		{
			GameManager.Instance.platformInterface.AchievementUnlock(Achievement.BEAT_PAST_ROGUE, 0);
		}
		else if (flag == GungeonFlags.BOSSKILLED_CONVICT_PAST)
		{
			GameManager.Instance.platformInterface.AchievementUnlock(Achievement.BEAT_PAST_CONVICT, 0);
		}
		else if (flag == GungeonFlags.BOSSKILLED_SOLDIER_PAST)
		{
			GameManager.Instance.platformInterface.AchievementUnlock(Achievement.BEAT_PAST_MARINE, 0);
		}
		else if (flag == GungeonFlags.BOSSKILLED_GUIDE_PAST)
		{
			GameManager.Instance.platformInterface.AchievementUnlock(Achievement.BEAT_PAST_GUIDE, 0);
		}
		else if (flag == GungeonFlags.BOSSKILLED_ROBOT_PAST)
		{
			GameManager.Instance.platformInterface.AchievementUnlock(Achievement.BEAT_PAST_ROBOT, 0);
		}
		else if (flag == GungeonFlags.BOSSKILLED_BULLET_PAST)
		{
			GameManager.Instance.platformInterface.AchievementUnlock(Achievement.BEAT_PAST_BULLET, 0);
		}
		else if (flag == GungeonFlags.BOSSKILLED_DRAGUN)
		{
			GameManager.Instance.platformInterface.AchievementUnlock(Achievement.BEAT_FLOOR_FIVE, 0);
		}
		else if (flag == GungeonFlags.BOSSKILLED_LICH)
		{
			GameManager.Instance.platformInterface.AchievementUnlock(Achievement.BEAT_FLOOR_SIX, 0);
		}
		else if (flag == GungeonFlags.BOSSKILLED_HIGHDRAGUN)
		{
			GameManager.Instance.platformInterface.AchievementUnlock(Achievement.BEAT_ADVANCED_DRAGUN, 0);
		}
		else if (flag == GungeonFlags.BOSSKILLED_RESOURCEFULRAT)
		{
			GameManager.Instance.platformInterface.AchievementUnlock(Achievement.BEAT_RESOURCEFUL_RAT, 0);
			GameManager.Instance.platformInterface.AchievementUnlock(Achievement.BEAT_METAL_GEAR_RAT, 0);
		}
		if (flag == GungeonFlags.BLACKSMITH_BULLET_COMPLETE)
		{
			GameManager.Instance.platformInterface.AchievementUnlock(Achievement.BUILD_BULLET, 0);
		}
		else if (flag == GungeonFlags.LOST_ADVENTURER_ACHIEVEMENT_REWARD_GIVEN)
		{
			GameManager.Instance.platformInterface.AchievementUnlock(Achievement.MAP_MAIN_FLOORS, 0);
		}
		else if (flag == GungeonFlags.META_SHOP_RECEIVED_ROBOT_ARM_REWARD)
		{
			GameManager.Instance.platformInterface.AchievementUnlock(Achievement.COMPLETE_GOLEM_ARM, 0);
		}
		else if (flag == GungeonFlags.FRIFLE_CORE_HUNTS_COMPLETE)
		{
			GameManager.Instance.platformInterface.AchievementUnlock(Achievement.COMPLETE_FRIFLE_MULTI, 0);
		}
		else if (flag == GungeonFlags.TUTORIAL_KILLED_MANFREDS_RIVAL)
		{
			GameManager.Instance.platformInterface.AchievementUnlock(Achievement.BEAT_MANFREDS_RIVAL, 0);
		}
		else if (flag == GungeonFlags.TUTORIAL_COMPLETED)
		{
			GameManager.Instance.platformInterface.AchievementUnlock(Achievement.BEAT_TUTORIAL, 0);
		}
		else if (flag == GungeonFlags.DAISUKE_CHALLENGE_COMPLETE)
		{
			GameManager.Instance.platformInterface.AchievementUnlock(Achievement.COMPLETE_GAME_WITH_CHALLENGE_MODE, 0);
		}
		else if (flag == GungeonFlags.BOSSKILLED_DRAGUN_TURBO_MODE)
		{
			GameManager.Instance.platformInterface.AchievementUnlock(Achievement.COMPLETE_GAME_WITH_TURBO_MODE, 0);
		}
		if (flag == GungeonFlags.SECRET_BULLETMAN_SEEN_05)
		{
			GameManager.Instance.platformInterface.AchievementUnlock(Achievement.UNLOCK_BULLET, 0);
		}
		else if (flag == GungeonFlags.BLACKSMITH_RECEIVED_BUSTED_TELEVISION)
		{
			GameManager.Instance.platformInterface.AchievementUnlock(Achievement.UNLOCK_ROBOT, 0);
		}
		if (flag == GungeonFlags.SHERPA_UNLOCK1_COMPLETE)
		{
			GameManager.Instance.platformInterface.AchievementUnlock(Achievement.UNLOCK_FLOOR_TWO_SHORTCUT, 0);
		}
		else if (flag == GungeonFlags.SHERPA_UNLOCK2_COMPLETE)
		{
			GameManager.Instance.platformInterface.AchievementUnlock(Achievement.UNLOCK_FLOOR_THREE_SHORTCUT, 0);
		}
		else if (flag == GungeonFlags.SHERPA_UNLOCK3_COMPLETE)
		{
			GameManager.Instance.platformInterface.AchievementUnlock(Achievement.UNLOCK_FLOOR_FOUR_SHORTCUT, 0);
		}
		else if (flag == GungeonFlags.SHERPA_UNLOCK4_COMPLETE)
		{
			GameManager.Instance.platformInterface.AchievementUnlock(Achievement.UNLOCK_FLOOR_FIVE_SHORTCUT, 0);
		}
		if (GameStatsManager.s_pastFlags.Contains(flag))
		{
			bool flag2 = true;
			for (int i = 0; i < GameStatsManager.s_pastFlags.Count; i++)
			{
				if (!this.GetFlag(GameStatsManager.s_pastFlags[i]))
				{
					flag2 = false;
					break;
				}
			}
			if (flag2)
			{
				GameManager.Instance.platformInterface.AchievementUnlock(Achievement.BEAT_PAST_ALL, 0);
			}
		}
		if (GameStatsManager.s_npcFoyerFlags.Contains(flag))
		{
			this.UpdateAllNpcsAchievement();
		}
		if (GameStatsManager.s_frifleHuntFlags.Contains(flag))
		{
			this.UpdateFrifleHuntAchievement();
		}
		if (flag == GungeonFlags.LOST_ADVENTURER_HELPED_CASTLE || flag == GungeonFlags.LOST_ADVENTURER_HELPED_GUNGEON || flag == GungeonFlags.LOST_ADVENTURER_HELPED_MINES || flag == GungeonFlags.LOST_ADVENTURER_HELPED_CATACOMBS || flag == GungeonFlags.LOST_ADVENTURER_HELPED_FORGE)
		{
			int num = 0;
			num += ((!this.GetFlag(GungeonFlags.LOST_ADVENTURER_HELPED_CASTLE)) ? 0 : 1);
			num += ((!this.GetFlag(GungeonFlags.LOST_ADVENTURER_HELPED_GUNGEON)) ? 0 : 1);
			num += ((!this.GetFlag(GungeonFlags.LOST_ADVENTURER_HELPED_MINES)) ? 0 : 1);
			num += ((!this.GetFlag(GungeonFlags.LOST_ADVENTURER_HELPED_CATACOMBS)) ? 0 : 1);
			num += ((!this.GetFlag(GungeonFlags.LOST_ADVENTURER_HELPED_FORGE)) ? 0 : 1);
			GameManager.Instance.platformInterface.SetStat(PlatformStat.MAIN_FLOORS_MAPPED, num);
		}
	}

	// Token: 0x06007A9D RID: 31389 RVA: 0x00312B0C File Offset: 0x00310D0C
	private void UpdateAllNpcsAchievement()
	{
		bool flag = true;
		int num = 0;
		for (int i = 0; i < GameStatsManager.s_npcFoyerFlags.Count; i++)
		{
			if (this.GetFlag(GameStatsManager.s_npcFoyerFlags[i]))
			{
				num++;
			}
			else
			{
				flag = false;
			}
		}
		if (this.GetFlag(GungeonFlags.TUTORIAL_TALKED_AFTER_RIVAL_KILLED))
		{
			num++;
		}
		if (this.GetFlag(GungeonFlags.SORCERESS_ACTIVE_IN_FOYER))
		{
			this.SetFlag(GungeonFlags.DAISUKE_IS_UNLOCKABLE, true);
		}
		if (this.GetPlayerStatValue(TrackedStats.MERCHANT_PURCHASES_GOOP) >= 1f)
		{
			num++;
		}
		else
		{
			flag = false;
		}
		GameManager.Instance.platformInterface.SetStat(PlatformStat.BREACH_POPULATION, num);
		if (flag)
		{
			GameManager.Instance.platformInterface.AchievementUnlock(Achievement.POPULATE_BREACH, 0);
		}
	}

	// Token: 0x06007A9E RID: 31390 RVA: 0x00312BD0 File Offset: 0x00310DD0
	private void UpdateFrifleHuntAchievement()
	{
		bool flag = true;
		int num = 0;
		for (int i = 0; i < GameStatsManager.s_frifleHuntFlags.Count; i++)
		{
			if (this.GetFlag(GameStatsManager.s_frifleHuntFlags[i]))
			{
				num++;
			}
			else
			{
				flag = false;
			}
		}
		GameManager.Instance.platformInterface.SetStat(PlatformStat.FRIFLE_CORE_COMPLETED, num);
		if (flag)
		{
			GameManager.Instance.platformInterface.AchievementUnlock(Achievement.COMPLETE_FRIFLE_MULTI, 0);
		}
	}

	// Token: 0x04007D12 RID: 32018
	[fsProperty]
	public Dictionary<PlayableCharacters, GameStats> m_characterStats = new Dictionary<PlayableCharacters, GameStats>(new PlayableCharactersComparer());

	// Token: 0x04007D13 RID: 32019
	[fsProperty]
	private Dictionary<string, EncounteredObjectData> m_encounteredTrackables = new Dictionary<string, EncounteredObjectData>();

	// Token: 0x04007D14 RID: 32020
	[fsProperty]
	public Dictionary<string, int> m_encounteredFlows = new Dictionary<string, int>();

	// Token: 0x04007D15 RID: 32021
	[fsProperty]
	public Dictionary<string, EncounteredObjectData> m_encounteredRooms = new Dictionary<string, EncounteredObjectData>();

	// Token: 0x04007D16 RID: 32022
	[fsProperty]
	public HashSet<GungeonFlags> m_flags = new HashSet<GungeonFlags>(new GungeonFlagsComparer());

	// Token: 0x04007D17 RID: 32023
	[fsProperty]
	public Dictionary<string, int> m_persistentStringsLastIndices = new Dictionary<string, int>();

	// Token: 0x04007D18 RID: 32024
	[fsProperty]
	public Dictionary<int, int> m_encounteredSynergiesByID = new Dictionary<int, int>();

	// Token: 0x04007D19 RID: 32025
	[fsProperty]
	public MonsterHuntProgress huntProgress;

	// Token: 0x04007D1A RID: 32026
	[fsProperty]
	public int CurrentResRatShopSeed = -1;

	// Token: 0x04007D1B RID: 32027
	[fsProperty]
	public int CurrentEeveeEquipSeed = -1;

	// Token: 0x04007D1C RID: 32028
	[fsProperty]
	public int CurrentAccumulatedGunderfuryExperience;

	// Token: 0x04007D1D RID: 32029
	[fsProperty]
	public int CurrentRobotArmFloor = 5;

	// Token: 0x04007D1E RID: 32030
	[fsProperty]
	public int NumberRunsValidCellWithoutSpawn;

	// Token: 0x04007D1F RID: 32031
	[fsProperty]
	public float AccumulatedBeetleMerchantChance;

	// Token: 0x04007D20 RID: 32032
	[fsProperty]
	public float AccumulatedUsedBeetleMerchantChance;

	// Token: 0x04007D21 RID: 32033
	[fsProperty]
	public Dictionary<GlobalDungeonData.ValidTilesets, string> LastBossEncounteredMap = new Dictionary<GlobalDungeonData.ValidTilesets, string>();

	// Token: 0x04007D22 RID: 32034
	[fsProperty]
	private HashSet<string> forcedUnlocks = new HashSet<string>();

	// Token: 0x04007D23 RID: 32035
	[fsProperty]
	public string midGameSaveGuid;

	// Token: 0x04007D24 RID: 32036
	[fsProperty]
	public int savedSystemHash = -1;

	// Token: 0x04007D25 RID: 32037
	[fsProperty]
	public bool isChump;

	// Token: 0x04007D26 RID: 32038
	[fsProperty]
	public bool isTurboMode;

	// Token: 0x04007D27 RID: 32039
	[fsProperty]
	public bool rainbowRunToggled;

	// Token: 0x04007D28 RID: 32040
	private int m_numCharacters = -1;

	// Token: 0x04007D29 RID: 32041
	private PlayableCharacters m_sessionCharacter;

	// Token: 0x04007D2A RID: 32042
	private GameStats m_sessionStats;

	// Token: 0x04007D2B RID: 32043
	private GameStats m_savedSessionStats;

	// Token: 0x04007D2C RID: 32044
	private HashSet<int> m_sessionSynergies = new HashSet<int>();

	// Token: 0x04007D2D RID: 32045
	private static GameStatsManager m_instance;

	// Token: 0x04007D2E RID: 32046
	private static List<GungeonFlags> s_pastFlags;

	// Token: 0x04007D2F RID: 32047
	private static List<GungeonFlags> s_npcFoyerFlags;

	// Token: 0x04007D30 RID: 32048
	private static List<GungeonFlags> s_frifleHuntFlags;

	// Token: 0x04007D31 RID: 32049
	[fsIgnore]
	private List<string> m_singleProcessedEncounterTrackables = new List<string>();
}
