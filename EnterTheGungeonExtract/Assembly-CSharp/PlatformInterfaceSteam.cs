using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Steamworks;
using UnityEngine;

// Token: 0x020015CF RID: 5583
public class PlatformInterfaceSteam : PlatformInterface
{
	// Token: 0x06008038 RID: 32824 RVA: 0x0033D304 File Offset: 0x0033B504
	public static bool IsSteamBuild()
	{
		return File.Exists(Path.Combine(Application.dataPath, "../steam_api64.dll")) || File.Exists(Path.Combine(Application.dataPath, "../steam_api.dll"));
	}

	// Token: 0x06008039 RID: 32825 RVA: 0x0033D33C File Offset: 0x0033B53C
	protected override void OnStart()
	{
		Debug.Log("Starting Steam platform interface.");
		if (!SteamManager.Initialized)
		{
			return;
		}
		this.m_GameID = new CGameID(SteamUtils.GetAppID());
		this.UnlockedDlc.Clear();
		this.m_UserStatsReceived = Callback<UserStatsReceived_t>.Create(new Callback<UserStatsReceived_t>.DispatchDelegate(this.OnUserStatsReceived));
		this.m_UserStatsStored = Callback<UserStatsStored_t>.Create(new Callback<UserStatsStored_t>.DispatchDelegate(this.OnUserStatsStored));
		this.m_UserAchievementStored = Callback<UserAchievementStored_t>.Create(new Callback<UserAchievementStored_t>.DispatchDelegate(this.OnAchievementStored));
		this.m_DLCInstalled = Callback<DlcInstalled_t>.Create(new Callback<DlcInstalled_t>.DispatchDelegate(this.OnDlcInstalled));
		for (int i = 0; i < this.m_dlc.Length; i++)
		{
			if (SteamApps.BIsDlcInstalled(this.m_dlc[i].appId))
			{
				this.UnlockedDlc.Add(this.m_dlc[i].dlc);
			}
		}
		this.m_bRequestedStats = false;
		this.m_bStatsValid = false;
	}

	// Token: 0x0600803A RID: 32826 RVA: 0x0033D42C File Offset: 0x0033B62C
	protected override void OnAchievementUnlock(Achievement achievement, int playerIndex)
	{
		if (!SteamManager.Initialized)
		{
			return;
		}
		if (achievement == Achievement.BEAT_FLOOR_ONE_MULTI || achievement == Achievement.BEAT_FLOOR_TWO_MULTI || achievement == Achievement.BEAT_FLOOR_THREE_MULTI || achievement == Achievement.BEAT_FLOOR_FOUR_MULTI || achievement == Achievement.BEAT_FLOOR_FIVE_MULTI || achievement == Achievement.MAP_MAIN_FLOORS || achievement == Achievement.ACE_WINCHESTER_MULTI || achievement == Achievement.COMPLETE_GUNSLING_MULTI || achievement == Achievement.POPULATE_BREACH || achievement == Achievement.STEAL_MULTI || achievement == Achievement.KILL_WITH_CHANDELIER_MULTI || achievement == Achievement.KILL_IN_MINE_CART_MULTI || achievement == Achievement.KILL_WITH_PITS_MULTI || achievement == Achievement.FLIP_TABLES_MULTI)
		{
			return;
		}
		if (achievement == Achievement.COMPLETE_FRIFLE_MULTI)
		{
			this.SetStat(PlatformStat.FRIFLE_CORE_COMPLETED, 15);
			return;
		}
		if (achievement == Achievement.SPEND_META_CURRENCY)
		{
			this.SetStat(PlatformStat.META_SPENT_AT_STORE, 100);
			return;
		}
		PlatformInterfaceSteam.AchievementData achievementData = null;
		for (int i = 0; i < this.m_achievements.Length; i++)
		{
			if (this.m_achievements[i].achievement == achievement)
			{
				achievementData = this.m_achievements[i];
			}
		}
		if (achievementData == null)
		{
			return;
		}
		achievementData.isUnlocked = true;
		SteamUserStats.SetAchievement(achievementData.ApiKey);
		this.m_bStoreStats = true;
	}

	// Token: 0x0600803B RID: 32827 RVA: 0x0033D534 File Offset: 0x0033B734
	public override bool IsAchievementUnlocked(Achievement achievement)
	{
		PlatformInterfaceSteam.AchievementData achievementData = null;
		for (int i = 0; i < this.m_achievements.Length; i++)
		{
			if (this.m_achievements[i].achievement == achievement)
			{
				achievementData = this.m_achievements[i];
			}
		}
		return achievementData != null && achievementData.isUnlocked;
	}

	// Token: 0x0600803C RID: 32828 RVA: 0x0033D588 File Offset: 0x0033B788
	public override void SetStat(PlatformStat stat, int value)
	{
		if (!SteamManager.Initialized)
		{
			return;
		}
		PlatformInterfaceSteam.StatData statData = null;
		for (int i = 0; i < this.m_stats.Length; i++)
		{
			if (this.m_stats[i].stat == stat)
			{
				statData = this.m_stats[i];
			}
		}
		if (statData == null)
		{
			return;
		}
		int value2 = statData.value;
		statData.value = value;
		SteamUserStats.SetStat(statData.ApiKey, value);
		this.MaybeShowProgress(stat, value2, value);
		this.MaybeStoreStats(stat, value2, value);
	}

	// Token: 0x0600803D RID: 32829 RVA: 0x0033D60C File Offset: 0x0033B80C
	public override void IncrementStat(PlatformStat stat, int delta)
	{
		if (!SteamManager.Initialized)
		{
			return;
		}
		PlatformInterfaceSteam.StatData statData = null;
		for (int i = 0; i < this.m_stats.Length; i++)
		{
			if (this.m_stats[i].stat == stat)
			{
				statData = this.m_stats[i];
			}
		}
		if (statData == null)
		{
			return;
		}
		int value = statData.value;
		statData.value = value + delta;
		SteamUserStats.SetStat(statData.ApiKey, statData.value);
		this.MaybeShowProgress(stat, value, statData.value);
		this.MaybeStoreStats(stat, value, statData.value);
	}

	// Token: 0x0600803E RID: 32830 RVA: 0x0033D6A0 File Offset: 0x0033B8A0
	private void MaybeShowProgress(PlatformStat stat, int prevValue, int newValue)
	{
		for (int i = 0; i < this.m_achievements.Length; i++)
		{
			PlatformInterfaceSteam.AchievementData achievementData = this.m_achievements[i];
			if (achievementData.hasProgressStat && achievementData.progressStat == stat && achievementData.subgoals != null && newValue < achievementData.goalValue)
			{
				if (achievementData.progressStat == PlatformStat.BREACH_POPULATION && prevValue == 0 && GameManager.Instance.CurrentLevelOverrideState == GameManager.LevelOverrideState.FOYER)
				{
					return;
				}
				for (int j = achievementData.subgoals.Length - 1; j >= 0; j--)
				{
					int num = achievementData.subgoals[j];
					if (prevValue < num && newValue >= num)
					{
						SteamUserStats.IndicateAchievementProgress(achievementData.ApiKey, (uint)newValue, (uint)achievementData.goalValue);
						return;
					}
				}
			}
		}
	}

	// Token: 0x0600803F RID: 32831 RVA: 0x0033D768 File Offset: 0x0033B968
	private void MaybeStoreStats(PlatformStat stat, int prevValue, int newValue)
	{
		if ((stat == PlatformStat.META_SPENT_AT_STORE && prevValue < 100 && newValue >= 100) || (stat == PlatformStat.FLOOR_ONE_CLEARS && prevValue < 50 && newValue >= 50) || (stat == PlatformStat.FLOOR_TWO_CLEARS && prevValue < 40 && newValue >= 40) || (stat == PlatformStat.FLOOR_THREE_CLEARS && prevValue < 30 && newValue >= 30) || (stat == PlatformStat.FLOOR_FOUR_CLEARS && prevValue < 20 && newValue >= 20) || (stat == PlatformStat.FLOOR_FIVE_CLEARS && prevValue < 10 && newValue >= 10) || (stat == PlatformStat.MAIN_FLOORS_MAPPED || stat == PlatformStat.FRIFLE_CORE_COMPLETED || stat == PlatformStat.WINCHESTER_ACED || stat == PlatformStat.GUNSLING_COMPLETED || stat == PlatformStat.BREACH_POPULATION || stat == PlatformStat.ITEMS_STOLEN || (stat == PlatformStat.CHANDELIER_KILLS && prevValue < 100 && newValue >= 100)) || (stat == PlatformStat.MINECART_KILLS && prevValue < 100 && newValue >= 100) || (stat == PlatformStat.PIT_KILLS && prevValue < 100 && newValue >= 100) || (stat == PlatformStat.TABLES_FLIPPED && prevValue < 500 && newValue >= 500))
		{
			this.m_bStoreStats = true;
		}
	}

	// Token: 0x06008040 RID: 32832 RVA: 0x0033D89C File Offset: 0x0033BA9C
	public override void StoreStats()
	{
		this.m_bStoreStats = true;
	}

	// Token: 0x06008041 RID: 32833 RVA: 0x0033D8A8 File Offset: 0x0033BAA8
	public override void ResetStats(bool achievementsToo)
	{
		SteamUserStats.ResetAllStats(achievementsToo);
	}

	// Token: 0x06008042 RID: 32834 RVA: 0x0033D8B4 File Offset: 0x0033BAB4
	protected override void OnLateUpdate()
	{
		if (!SteamManager.Initialized)
		{
			return;
		}
		if (!this.m_bRequestedStats)
		{
			if (!SteamManager.Initialized)
			{
				this.m_bRequestedStats = true;
				return;
			}
			bool flag = SteamUserStats.RequestCurrentStats();
			this.m_bRequestedStats = flag;
		}
		if (!this.m_bStatsValid)
		{
			return;
		}
		if (this.m_bStoreStats)
		{
			bool flag2 = SteamUserStats.StoreStats();
			this.m_bStoreStats = !flag2;
		}
	}

	// Token: 0x06008043 RID: 32835 RVA: 0x0033D920 File Offset: 0x0033BB20
	protected override StringTableManager.GungeonSupportedLanguages OnGetPreferredLanguage()
	{
		string steamUILanguage = SteamUtils.GetSteamUILanguage();
		if (this.SteamDefaultLanguageToGungeonLanguage.ContainsKey(steamUILanguage))
		{
			return this.SteamDefaultLanguageToGungeonLanguage[steamUILanguage];
		}
		return StringTableManager.GungeonSupportedLanguages.ENGLISH;
	}

	// Token: 0x06008044 RID: 32836 RVA: 0x0033D954 File Offset: 0x0033BB54
	private void OnUserStatsReceived(UserStatsReceived_t pCallback)
	{
		if (!SteamManager.Initialized)
		{
			return;
		}
		if ((ulong)this.m_GameID != pCallback.m_nGameID)
		{
			return;
		}
		if (pCallback.m_eResult == EResult.k_EResultOK)
		{
			Debug.Log("Received stats and achievements from Steam\n");
			this.m_bStatsValid = true;
			foreach (PlatformInterfaceSteam.AchievementData achievementData in this.m_achievements)
			{
				bool achievement = SteamUserStats.GetAchievement(achievementData.ApiKey, out achievementData.isUnlocked);
				if (achievement)
				{
					achievementData.name = SteamUserStats.GetAchievementDisplayAttribute(achievementData.ApiKey, "name");
					achievementData.description = SteamUserStats.GetAchievementDisplayAttribute(achievementData.ApiKey, "desc");
				}
				else
				{
					Debug.LogWarning("SteamUserStats.GetAchievement failed for Achievement " + achievementData.achievement + "\nIs it registered in the Steam Partner site?");
				}
			}
			foreach (PlatformInterfaceSteam.StatData statData in this.m_stats)
			{
				if (!SteamUserStats.GetStat(statData.ApiKey, out statData.value))
				{
					Debug.LogWarning("SteamUserStats.GetStat failed for Stat " + statData.stat + "\nIs it registered in the Steam Partner site?");
				}
			}
			base.CatchupAchievements();
		}
		else
		{
			Debug.Log("RequestStats - failed, " + pCallback.m_eResult);
		}
	}

	// Token: 0x06008045 RID: 32837 RVA: 0x0033DAB4 File Offset: 0x0033BCB4
	private void OnUserStatsStored(UserStatsStored_t pCallback)
	{
		if ((ulong)this.m_GameID != pCallback.m_nGameID)
		{
			return;
		}
		if (pCallback.m_eResult == EResult.k_EResultOK)
		{
			Debug.Log("StoreStats - success");
		}
		else if (pCallback.m_eResult == EResult.k_EResultInvalidParam)
		{
			Debug.Log("StoreStats - some failed to validate");
			this.OnUserStatsReceived(new UserStatsReceived_t
			{
				m_eResult = EResult.k_EResultOK,
				m_nGameID = (ulong)this.m_GameID
			});
		}
		else
		{
			Debug.Log("StoreStats - failed, " + pCallback.m_eResult);
		}
	}

	// Token: 0x06008046 RID: 32838 RVA: 0x0033DB58 File Offset: 0x0033BD58
	private void OnAchievementStored(UserAchievementStored_t pCallback)
	{
		if ((ulong)this.m_GameID != pCallback.m_nGameID)
		{
			return;
		}
		if (pCallback.m_nMaxProgress == 0U)
		{
			Debug.Log("Achievement '" + pCallback.m_rgchAchievementName + "' unlocked!");
		}
		else
		{
			Debug.Log(string.Concat(new object[] { "Achievement '", pCallback.m_rgchAchievementName, "' progress callback, (", pCallback.m_nCurProgress, ",", pCallback.m_nMaxProgress, ")" }));
		}
	}

	// Token: 0x06008047 RID: 32839 RVA: 0x0033DC04 File Offset: 0x0033BE04
	private void OnDlcInstalled(DlcInstalled_t pCallback)
	{
		for (int i = 0; i < this.m_dlc.Length; i++)
		{
			if (this.m_dlc[i].appId == pCallback.m_nAppID)
			{
				this.UnlockedDlc.Add(this.m_dlc[i].dlc);
			}
		}
	}

	// Token: 0x06008048 RID: 32840 RVA: 0x0033DC60 File Offset: 0x0033BE60
	public void DebugPrintAchievements()
	{
		StringBuilder stringBuilder = new StringBuilder();
		for (int i = 0; i < this.m_achievements.Length; i++)
		{
			PlatformInterfaceSteam.AchievementData achievementData = this.m_achievements[i];
			stringBuilder.AppendFormat("[{0}] {1}\n", (!achievementData.isUnlocked) ? " " : "X", achievementData.name);
			stringBuilder.AppendFormat("{0}\n", achievementData.description);
			if (achievementData.hasProgressStat)
			{
				PlatformInterfaceSteam.StatData statData = Array.Find<PlatformInterfaceSteam.StatData>(this.m_stats, (PlatformInterfaceSteam.StatData s) => s.stat == achievementData.progressStat);
				stringBuilder.AppendFormat("{0} of {1}\n", statData.value, achievementData.goalValue);
			}
			stringBuilder.AppendLine();
		}
		Debug.Log(stringBuilder.ToString());
	}

	// Token: 0x04008312 RID: 33554
	private CGameID m_GameID;

	// Token: 0x04008313 RID: 33555
	private bool m_bRequestedStats;

	// Token: 0x04008314 RID: 33556
	private bool m_bStatsValid;

	// Token: 0x04008315 RID: 33557
	private bool m_bStoreStats;

	// Token: 0x04008316 RID: 33558
	protected Callback<UserStatsReceived_t> m_UserStatsReceived;

	// Token: 0x04008317 RID: 33559
	protected Callback<UserStatsStored_t> m_UserStatsStored;

	// Token: 0x04008318 RID: 33560
	protected Callback<UserAchievementStored_t> m_UserAchievementStored;

	// Token: 0x04008319 RID: 33561
	protected Callback<DlcInstalled_t> m_DLCInstalled;

	// Token: 0x0400831A RID: 33562
	private PlatformInterfaceSteam.AchievementData[] m_achievements = new PlatformInterfaceSteam.AchievementData[]
	{
		new PlatformInterfaceSteam.AchievementData(Achievement.COLLECT_FIVE_MASTERY_TOKENS, "Lead God", "Collect five Master Rounds in one run"),
		new PlatformInterfaceSteam.AchievementData(Achievement.SPEND_META_CURRENCY, "Patron", "Spend big at the Acquisitions Department", PlatformStat.META_SPENT_AT_STORE, 100, new int[] { 25, 50, 75 }),
		new PlatformInterfaceSteam.AchievementData(Achievement.COMPLETE_GAME_WITH_ENCHANTED_GUN, "Gun Game", "Complete the game with the Sorceress's Enchanted Gun"),
		new PlatformInterfaceSteam.AchievementData(Achievement.BEAT_FLOOR_SIX, "Gungeon Master", "Clear the Sixth Chamber"),
		new PlatformInterfaceSteam.AchievementData(Achievement.BUILD_BULLET, "Gunsmith", "Construct the Bullet that can kill the Past"),
		new PlatformInterfaceSteam.AchievementData(Achievement.BEAT_PAST_ALL, "Historian", "Complete all 4 main character Pasts"),
		new PlatformInterfaceSteam.AchievementData(Achievement.BEAT_PAST_ROGUE, "Wingman", "Kill the Pilot's Past"),
		new PlatformInterfaceSteam.AchievementData(Achievement.BEAT_PAST_CONVICT, "Double Jeopardy", "Kill the Convict's Past"),
		new PlatformInterfaceSteam.AchievementData(Achievement.BEAT_PAST_MARINE, "Squad Captain", "Kill the Marine's Past"),
		new PlatformInterfaceSteam.AchievementData(Achievement.BEAT_PAST_GUIDE, "Deadliest Game", "Kill the Hunter's Past"),
		new PlatformInterfaceSteam.AchievementData(Achievement.BEAT_FLOOR_FIVE, "Slayer", "Defeat the Boss of the Fifth Chamber"),
		new PlatformInterfaceSteam.AchievementData(Achievement.BEAT_FLOOR_ONE_MULTI, "Castle Crasher", "Clear the First Chamber 50 Times", PlatformStat.FLOOR_ONE_CLEARS, 50, new int[] { 25 }),
		new PlatformInterfaceSteam.AchievementData(Achievement.BEAT_FLOOR_TWO_MULTI, "Dungeon Diver", "Clear the Second Chamber 40 Times", PlatformStat.FLOOR_TWO_CLEARS, 40, new int[] { 20 }),
		new PlatformInterfaceSteam.AchievementData(Achievement.BEAT_FLOOR_THREE_MULTI, "Mine Master", "Clear the Third Chamber 30 Times", PlatformStat.FLOOR_THREE_CLEARS, 30, new int[] { 15 }),
		new PlatformInterfaceSteam.AchievementData(Achievement.BEAT_FLOOR_FOUR_MULTI, "Hollowed Out", "Clear the Fourth Chamber 20 Times", PlatformStat.FLOOR_FOUR_CLEARS, 20, new int[] { 10 }),
		new PlatformInterfaceSteam.AchievementData(Achievement.BEAT_FLOOR_FIVE_MULTI, "Forger", "Clear the Fifth Chamber 10 Times", PlatformStat.FLOOR_FIVE_CLEARS, 10, new int[] { 5 }),
		new PlatformInterfaceSteam.AchievementData(Achievement.HAVE_MANY_COINS, "Biggest Wallet", "Carry a large amount of money at once"),
		new PlatformInterfaceSteam.AchievementData(Achievement.MAP_MAIN_FLOORS, "Cartographer's Assistant", "Map the first Five Chambers for the lost adventurer", PlatformStat.MAIN_FLOORS_MAPPED, 5, new int[] { 1, 2, 3, 4 }),
		new PlatformInterfaceSteam.AchievementData(Achievement.REACH_SEWERS, "Grate Hall", "Access the Oubliette"),
		new PlatformInterfaceSteam.AchievementData(Achievement.REACH_CATHEDRAL, "Reverence for the Dead", "Access the Temple"),
		new PlatformInterfaceSteam.AchievementData(Achievement.COMPLETE_GOLEM_ARM, "Re-Armed", "Deliver the Golem's replacement arm"),
		new PlatformInterfaceSteam.AchievementData(Achievement.COMPLETE_FRIFLE_MULTI, "Weird Tale", "Complete Frifle's Challenges", PlatformStat.FRIFLE_CORE_COMPLETED, 15, new int[]
		{
			1, 2, 3, 4, 5, 6, 7, 8, 9, 10,
			11, 12, 13, 14
		}),
		new PlatformInterfaceSteam.AchievementData(Achievement.ACE_WINCHESTER_MULTI, "Trickshot", "Ace Winchester's game 3 times", PlatformStat.WINCHESTER_ACED, 3, new int[] { 1, 2 }),
		new PlatformInterfaceSteam.AchievementData(Achievement.COMPLETE_GUNSLING_MULTI, "Hedge Slinger", "Win a wager against the Gunsling King 5 times", PlatformStat.GUNSLING_COMPLETED, 5, new int[] { 1, 2, 3, 4 }),
		new PlatformInterfaceSteam.AchievementData(Achievement.UNLOCK_BULLET, "Case Closed", "Unlock the Bullet"),
		new PlatformInterfaceSteam.AchievementData(Achievement.UNLOCK_ROBOT, "Beep", "Unlock the Robot"),
		new PlatformInterfaceSteam.AchievementData(Achievement.UNLOCK_FLOOR_TWO_SHORTCUT, "Going Down", "Open the shortcut to the Second Chamber"),
		new PlatformInterfaceSteam.AchievementData(Achievement.UNLOCK_FLOOR_THREE_SHORTCUT, "Going Downer", "Open the shortcut to the Third Chamber"),
		new PlatformInterfaceSteam.AchievementData(Achievement.UNLOCK_FLOOR_FOUR_SHORTCUT, "Going Downest", "Open the shortcut to the Fourth Chamber"),
		new PlatformInterfaceSteam.AchievementData(Achievement.UNLOCK_FLOOR_FIVE_SHORTCUT, "Last Stop", "Open the shortcut to the Fifth Chamber"),
		new PlatformInterfaceSteam.AchievementData(Achievement.BEAT_MANFREDS_RIVAL, "Sworn Gun", "Avenge Manuel"),
		new PlatformInterfaceSteam.AchievementData(Achievement.BEAT_TUTORIAL, "Gungeon Acolyte", "Complete the Tutorial"),
		new PlatformInterfaceSteam.AchievementData(Achievement.POPULATE_BREACH, "Great Hall", "Populate the Breach", PlatformStat.BREACH_POPULATION, 12, new int[] { 3, 6, 9 }),
		new PlatformInterfaceSteam.AchievementData(Achievement.PREFIRE_ON_MIMIC, "Not Just A Box", "Get the jump on a Mimic"),
		new PlatformInterfaceSteam.AchievementData(Achievement.KILL_FROZEN_ENEMY_WITH_ROLL, "Demolition Man", "Kill a frozen enemy by rolling into it"),
		new PlatformInterfaceSteam.AchievementData(Achievement.PUSH_TABLE_INTO_PIT, "I Knew Someone Would Do It", "Why"),
		new PlatformInterfaceSteam.AchievementData(Achievement.STEAL_MULTI, "Woodsie Lord", "Steal 10 things", PlatformStat.ITEMS_STOLEN, 10, new int[] { 5 }),
		new PlatformInterfaceSteam.AchievementData(Achievement.KILL_BOSS_WITH_GLITTER, "Day Ruiner", "Kill a boss after covering it with glitter"),
		new PlatformInterfaceSteam.AchievementData(Achievement.FALL_IN_END_TIMES, "Lion Leap", "Fall at the last second"),
		new PlatformInterfaceSteam.AchievementData(Achievement.KILL_WITH_CHANDELIER_MULTI, "Money Pit", "Kill 100 enemies by dropping chandeliers", PlatformStat.CHANDELIER_KILLS, 100, new int[] { 25, 50, 75 }),
		new PlatformInterfaceSteam.AchievementData(Achievement.KILL_IN_MINE_CART_MULTI, "Rider", "Kill 100 enemies while riding in a mine cart", PlatformStat.MINECART_KILLS, 100, new int[] { 25, 50, 75 }),
		new PlatformInterfaceSteam.AchievementData(Achievement.KILL_WITH_PITS_MULTI, "Pit Lord", "Kill 100 enemies by knocking them into pits", PlatformStat.PIT_KILLS, 100, new int[] { 25, 50, 75 }),
		new PlatformInterfaceSteam.AchievementData(Achievement.DIE_IN_PAST, "Time Paradox", "Die in the Past"),
		new PlatformInterfaceSteam.AchievementData(Achievement.BEAT_A_JAMMED_BOSS, "Exorcist", "Kill a Jammed Boss"),
		new PlatformInterfaceSteam.AchievementData(Achievement.REACH_BLACK_MARKET, "The Password", "Accessed the Hidden Market"),
		new PlatformInterfaceSteam.AchievementData(Achievement.HAVE_MAX_CURSE, "Jammed", "You've met with a terrible fate, haven't you"),
		new PlatformInterfaceSteam.AchievementData(Achievement.FLIP_TABLES_MULTI, "Rage Mode", "Always be flipping. Guns are for flippers", PlatformStat.TABLES_FLIPPED, 500, new int[] { 100, 200, 300, 400 }),
		new PlatformInterfaceSteam.AchievementData(Achievement.COMPLETE_GAME_WITH_BEAST_MODE, "Beast Master", "Complete the game with Beast Mode on"),
		new PlatformInterfaceSteam.AchievementData(Achievement.BEAT_PAST_ROBOT, "Terminated", "Kill the Robot's Past"),
		new PlatformInterfaceSteam.AchievementData(Achievement.BEAT_PAST_BULLET, "Hero of Gun", "Kill the Bullet's Past"),
		new PlatformInterfaceSteam.AchievementData(Achievement.COMPLETE_GAME_WITH_CHALLENGE_MODE, "Challenger", "Complete Daisuke's trial"),
		new PlatformInterfaceSteam.AchievementData(Achievement.BEAT_ADVANCED_DRAGUN, "Advanced Slayer", "Defeat an Advanced Boss"),
		new PlatformInterfaceSteam.AchievementData(Achievement.BEAT_METAL_GEAR_RAT, "Resourceful", "Take Revenge"),
		new PlatformInterfaceSteam.AchievementData(Achievement.COMPLETE_GAME_WITH_TURBO_MODE, "Sledge-Dog", "Complete Tonic's Challenge")
	};

	// Token: 0x0400831B RID: 33563
	private PlatformInterfaceSteam.StatData[] m_stats = new PlatformInterfaceSteam.StatData[]
	{
		new PlatformInterfaceSteam.StatData(PlatformStat.META_SPENT_AT_STORE),
		new PlatformInterfaceSteam.StatData(PlatformStat.FLOOR_ONE_CLEARS),
		new PlatformInterfaceSteam.StatData(PlatformStat.FLOOR_TWO_CLEARS),
		new PlatformInterfaceSteam.StatData(PlatformStat.FLOOR_THREE_CLEARS),
		new PlatformInterfaceSteam.StatData(PlatformStat.FLOOR_FOUR_CLEARS),
		new PlatformInterfaceSteam.StatData(PlatformStat.FLOOR_FIVE_CLEARS),
		new PlatformInterfaceSteam.StatData(PlatformStat.MAIN_FLOORS_MAPPED),
		new PlatformInterfaceSteam.StatData(PlatformStat.FRIFLE_CORE_COMPLETED),
		new PlatformInterfaceSteam.StatData(PlatformStat.WINCHESTER_ACED),
		new PlatformInterfaceSteam.StatData(PlatformStat.GUNSLING_COMPLETED),
		new PlatformInterfaceSteam.StatData(PlatformStat.BREACH_POPULATION),
		new PlatformInterfaceSteam.StatData(PlatformStat.ITEMS_STOLEN),
		new PlatformInterfaceSteam.StatData(PlatformStat.CHANDELIER_KILLS),
		new PlatformInterfaceSteam.StatData(PlatformStat.MINECART_KILLS),
		new PlatformInterfaceSteam.StatData(PlatformStat.PIT_KILLS),
		new PlatformInterfaceSteam.StatData(PlatformStat.TABLES_FLIPPED)
	};

	// Token: 0x0400831C RID: 33564
	private PlatformInterfaceSteam.DlcData[] m_dlc = new PlatformInterfaceSteam.DlcData[]
	{
		new PlatformInterfaceSteam.DlcData(PlatformDlc.EARLY_MTX_GUN, (AppId_t)457842U),
		new PlatformInterfaceSteam.DlcData(PlatformDlc.EARLY_COBALT_HAMMER, (AppId_t)457843U)
	};

	// Token: 0x0400831D RID: 33565
	private readonly Dictionary<string, StringTableManager.GungeonSupportedLanguages> SteamDefaultLanguageToGungeonLanguage = new Dictionary<string, StringTableManager.GungeonSupportedLanguages>
	{
		{
			"english",
			StringTableManager.GungeonSupportedLanguages.ENGLISH
		},
		{
			"french",
			StringTableManager.GungeonSupportedLanguages.FRENCH
		},
		{
			"german",
			StringTableManager.GungeonSupportedLanguages.GERMAN
		},
		{
			"italian",
			StringTableManager.GungeonSupportedLanguages.ITALIAN
		},
		{
			"japanese",
			StringTableManager.GungeonSupportedLanguages.JAPANESE
		},
		{
			"korean",
			StringTableManager.GungeonSupportedLanguages.KOREAN
		},
		{
			"portuguese",
			StringTableManager.GungeonSupportedLanguages.BRAZILIANPORTUGUESE
		},
		{
			"brazilian",
			StringTableManager.GungeonSupportedLanguages.BRAZILIANPORTUGUESE
		},
		{
			"spanish",
			StringTableManager.GungeonSupportedLanguages.SPANISH
		},
		{
			"russian",
			StringTableManager.GungeonSupportedLanguages.RUSSIAN
		},
		{
			"polish",
			StringTableManager.GungeonSupportedLanguages.POLISH
		},
		{
			"chinese",
			StringTableManager.GungeonSupportedLanguages.CHINESE
		}
	};

	// Token: 0x020015D0 RID: 5584
	private class AchievementData
	{
		// Token: 0x06008049 RID: 32841 RVA: 0x0033DD50 File Offset: 0x0033BF50
		public AchievementData(Achievement achievement, string name, string desc)
		{
			this.achievement = achievement;
			this.name = name;
			this.description = desc;
			this.isUnlocked = false;
		}

		// Token: 0x0600804A RID: 32842 RVA: 0x0033DD74 File Offset: 0x0033BF74
		public AchievementData(Achievement achievement, string name, string desc, PlatformStat progressStat, int goalValue, params int[] subgoals)
		{
			this.achievement = achievement;
			this.name = name;
			this.description = desc;
			this.isUnlocked = false;
			this.hasProgressStat = true;
			this.progressStat = progressStat;
			this.goalValue = goalValue;
			this.subgoals = subgoals;
		}

		// Token: 0x170012FD RID: 4861
		// (get) Token: 0x0600804B RID: 32843 RVA: 0x0033DDC4 File Offset: 0x0033BFC4
		public string ApiKey
		{
			get
			{
				if (this.m_cachedApiKey == null)
				{
					this.m_cachedApiKey = this.achievement.ToString();
				}
				return this.m_cachedApiKey;
			}
		}

		// Token: 0x0400831E RID: 33566
		public Achievement achievement;

		// Token: 0x0400831F RID: 33567
		public string name;

		// Token: 0x04008320 RID: 33568
		public string description;

		// Token: 0x04008321 RID: 33569
		public bool isUnlocked;

		// Token: 0x04008322 RID: 33570
		public bool hasProgressStat;

		// Token: 0x04008323 RID: 33571
		public PlatformStat progressStat;

		// Token: 0x04008324 RID: 33572
		public int goalValue;

		// Token: 0x04008325 RID: 33573
		public int[] subgoals;

		// Token: 0x04008326 RID: 33574
		private string m_cachedApiKey;
	}

	// Token: 0x020015D1 RID: 5585
	private class StatData
	{
		// Token: 0x0600804C RID: 32844 RVA: 0x0033DDF0 File Offset: 0x0033BFF0
		public StatData(PlatformStat stat)
		{
			this.stat = stat;
		}

		// Token: 0x170012FE RID: 4862
		// (get) Token: 0x0600804D RID: 32845 RVA: 0x0033DE00 File Offset: 0x0033C000
		public string ApiKey
		{
			get
			{
				if (this.m_cachedApiKey == null)
				{
					this.m_cachedApiKey = this.stat.ToString();
				}
				return this.m_cachedApiKey;
			}
		}

		// Token: 0x04008327 RID: 33575
		public PlatformStat stat;

		// Token: 0x04008328 RID: 33576
		public int value;

		// Token: 0x04008329 RID: 33577
		private string m_cachedApiKey;
	}

	// Token: 0x020015D2 RID: 5586
	private class DlcData
	{
		// Token: 0x0600804E RID: 32846 RVA: 0x0033DE2C File Offset: 0x0033C02C
		public DlcData(PlatformDlc dlc, AppId_t appId)
		{
			this.dlc = dlc;
			this.appId = appId;
		}

		// Token: 0x0400832A RID: 33578
		public PlatformDlc dlc;

		// Token: 0x0400832B RID: 33579
		public AppId_t appId;
	}
}
