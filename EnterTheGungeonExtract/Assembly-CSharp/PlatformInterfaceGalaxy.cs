using System;
using System.IO;
using System.Text;
using Galaxy.Api;
using UnityEngine;

// Token: 0x020015C8 RID: 5576
public class PlatformInterfaceGalaxy : PlatformInterface
{
	// Token: 0x06008019 RID: 32793 RVA: 0x0033C4B8 File Offset: 0x0033A6B8
	public static bool IsGalaxyBuild()
	{
		return File.Exists(Path.Combine(Application.dataPath, "../Galaxy.dll"));
	}

	// Token: 0x0600801A RID: 32794 RVA: 0x0033C4D8 File Offset: 0x0033A6D8
	protected override void OnStart()
	{
		Debug.Log("Starting GOG Galaxy platform interface.");
	}

	// Token: 0x0600801B RID: 32795 RVA: 0x0033C4E4 File Offset: 0x0033A6E4
	protected override void OnAchievementUnlock(Achievement achievement, int playerIndex)
	{
		if (!GalaxyManager.Initialized || !this.m_isInitialized)
		{
			return;
		}
		PlatformInterfaceGalaxy.AchievementData achievementData = null;
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
		try
		{
			GalaxyInstance.Stats().SetAchievement(achievementData.ApiKey);
		}
		catch (Exception ex)
		{
			Debug.LogError(ex);
		}
		this.m_bStoreStats = true;
	}

	// Token: 0x0600801C RID: 32796 RVA: 0x0033C584 File Offset: 0x0033A784
	public override bool IsAchievementUnlocked(Achievement achievement)
	{
		PlatformInterfaceGalaxy.AchievementData achievementData = null;
		for (int i = 0; i < this.m_achievements.Length; i++)
		{
			if (this.m_achievements[i].achievement == achievement)
			{
				achievementData = this.m_achievements[i];
			}
		}
		return achievementData != null && achievementData.isUnlocked;
	}

	// Token: 0x0600801D RID: 32797 RVA: 0x0033C5D8 File Offset: 0x0033A7D8
	public override void SetStat(PlatformStat stat, int value)
	{
		if (!GalaxyManager.Initialized || !this.m_isInitialized)
		{
			return;
		}
		PlatformInterfaceGalaxy.StatData statData = null;
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
		if (value <= statData.value)
		{
			return;
		}
		statData.value = value;
		try
		{
			GalaxyInstance.Stats().SetStatInt(statData.ApiKey, value);
		}
		catch (Exception ex)
		{
			Debug.LogError(ex);
		}
	}

	// Token: 0x0600801E RID: 32798 RVA: 0x0033C680 File Offset: 0x0033A880
	public override void IncrementStat(PlatformStat stat, int delta)
	{
		if (!GalaxyManager.Initialized || !this.m_isInitialized)
		{
			return;
		}
		PlatformInterfaceGalaxy.StatData statData = null;
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
		try
		{
			GalaxyInstance.Stats().SetStatInt(statData.ApiKey, statData.value);
		}
		catch (Exception ex)
		{
			Debug.LogError(ex);
		}
	}

	// Token: 0x0600801F RID: 32799 RVA: 0x0033C728 File Offset: 0x0033A928
	public override void StoreStats()
	{
		this.m_bStoreStats = true;
	}

	// Token: 0x06008020 RID: 32800 RVA: 0x0033C734 File Offset: 0x0033A934
	public override void ResetStats(bool achievementsToo)
	{
		try
		{
			GalaxyInstance.Stats().ResetStatsAndAchievements();
		}
		catch (Exception ex)
		{
			Debug.LogError(ex);
		}
		this.m_bRequestedStats = false;
		this.m_bStatsValid = false;
	}

	// Token: 0x06008021 RID: 32801 RVA: 0x0033C77C File Offset: 0x0033A97C
	protected override void OnLateUpdate()
	{
		if (!GalaxyManager.Initialized)
		{
			return;
		}
		if (GalaxyManager.Initialized && !this.m_isInitialized)
		{
			this.m_achievementsReceivedListener = new PlatformInterfaceGalaxy.AchievementsReceivedListener(this);
			this.m_storeStatsCallback = new PlatformInterfaceGalaxy.StatsStoredListener();
			this.m_isInitialized = true;
			return;
		}
		if (!this.m_bRequestedStats)
		{
			if (!GalaxyManager.Initialized)
			{
				this.m_bRequestedStats = true;
				return;
			}
			try
			{
				GalaxyInstance.Stats().RequestUserStatsAndAchievements();
				this.m_bRequestedStats = true;
			}
			catch (Exception ex)
			{
				Debug.LogError(ex);
			}
		}
		if (!this.m_bStatsValid)
		{
			return;
		}
		if (this.m_bStoreStats)
		{
			try
			{
				GalaxyInstance.Stats().StoreStatsAndAchievements();
				this.m_bStoreStats = false;
			}
			catch (Exception ex2)
			{
				Debug.LogError(ex2);
			}
		}
	}

	// Token: 0x06008022 RID: 32802 RVA: 0x0033C85C File Offset: 0x0033AA5C
	protected override StringTableManager.GungeonSupportedLanguages OnGetPreferredLanguage()
	{
		return StringTableManager.GungeonSupportedLanguages.ENGLISH;
	}

	// Token: 0x06008023 RID: 32803 RVA: 0x0033C860 File Offset: 0x0033AA60
	public void DebugPrintAchievements()
	{
		StringBuilder stringBuilder = new StringBuilder();
		for (int i = 0; i < this.m_achievements.Length; i++)
		{
			PlatformInterfaceGalaxy.AchievementData achievementData = this.m_achievements[i];
			stringBuilder.AppendFormat("[{0}] {1}\n", (!achievementData.isUnlocked) ? " " : "X", achievementData.name);
			stringBuilder.AppendFormat("{0}\n", achievementData.description);
			if (achievementData.hasProgressStat)
			{
				PlatformInterfaceGalaxy.StatData statData = Array.Find<PlatformInterfaceGalaxy.StatData>(this.m_stats, (PlatformInterfaceGalaxy.StatData s) => s.stat == achievementData.progressStat);
				stringBuilder.AppendFormat("{0} of {1}\n", statData.value, achievementData.goalValue);
			}
			stringBuilder.AppendLine();
		}
		Debug.Log(stringBuilder.ToString());
	}

	// Token: 0x06008024 RID: 32804 RVA: 0x0033C950 File Offset: 0x0033AB50
	public void OnUserStatsReceived()
	{
		Debug.Log("Received stats and achievements from Galaxy\n");
		this.m_bStatsValid = true;
		foreach (PlatformInterfaceGalaxy.AchievementData achievementData in this.m_achievements)
		{
			try
			{
				GalaxyInstance.Stats().GetAchievement(achievementData.ApiKey, ref achievementData.isUnlocked, ref achievementData.unlockTime);
				achievementData.name = GalaxyInstance.Stats().GetAchievementDisplayName(achievementData.ApiKey);
				achievementData.description = GalaxyInstance.Stats().GetAchievementDescription(achievementData.ApiKey);
			}
			catch (Exception ex)
			{
				Debug.LogError(ex);
			}
		}
		foreach (PlatformInterfaceGalaxy.StatData statData in this.m_stats)
		{
			try
			{
				statData.value = GalaxyInstance.Stats().GetStatInt(statData.ApiKey);
			}
			catch (Exception ex2)
			{
				Debug.LogError(ex2);
			}
		}
	}

	// Token: 0x040082FC RID: 33532
	private bool m_isInitialized;

	// Token: 0x040082FD RID: 33533
	private bool m_bRequestedStats;

	// Token: 0x040082FE RID: 33534
	private bool m_bStatsValid;

	// Token: 0x040082FF RID: 33535
	private bool m_bStoreStats;

	// Token: 0x04008300 RID: 33536
	private PlatformInterfaceGalaxy.AchievementsReceivedListener m_achievementsReceivedListener;

	// Token: 0x04008301 RID: 33537
	private PlatformInterfaceGalaxy.StatsStoredListener m_storeStatsCallback;

	// Token: 0x04008302 RID: 33538
	private PlatformInterfaceGalaxy.AchievementData[] m_achievements = new PlatformInterfaceGalaxy.AchievementData[]
	{
		new PlatformInterfaceGalaxy.AchievementData(Achievement.COLLECT_FIVE_MASTERY_TOKENS, "Lead God", "Collect five Master Rounds in one run"),
		new PlatformInterfaceGalaxy.AchievementData(Achievement.SPEND_META_CURRENCY, "Patron", "Spend big at the Acquisitions Department", PlatformStat.META_SPENT_AT_STORE, 100),
		new PlatformInterfaceGalaxy.AchievementData(Achievement.COMPLETE_GAME_WITH_ENCHANTED_GUN, "Gun Game", "Complete the game with the Sorceress's Enchanted Gun"),
		new PlatformInterfaceGalaxy.AchievementData(Achievement.BEAT_FLOOR_SIX, "Gungeon Master", "Clear the Sixth Chamber"),
		new PlatformInterfaceGalaxy.AchievementData(Achievement.BUILD_BULLET, "Gunsmith", "Construct the Bullet that can kill the Past"),
		new PlatformInterfaceGalaxy.AchievementData(Achievement.BEAT_PAST_ALL, "Historian", "Complete all 4 main character Pasts"),
		new PlatformInterfaceGalaxy.AchievementData(Achievement.BEAT_PAST_ROGUE, "Wingman", "Kill the Pilot's Past"),
		new PlatformInterfaceGalaxy.AchievementData(Achievement.BEAT_PAST_CONVICT, "Double Jeopardy", "Kill the Convict's Past"),
		new PlatformInterfaceGalaxy.AchievementData(Achievement.BEAT_PAST_MARINE, "Squad Captain", "Kill the Marine's Past"),
		new PlatformInterfaceGalaxy.AchievementData(Achievement.BEAT_PAST_GUIDE, "Deadliest Game", "Kill the Hunter's Past"),
		new PlatformInterfaceGalaxy.AchievementData(Achievement.BEAT_FLOOR_FIVE, "Slayer", "Defeat the Boss of the Fifth Chamber"),
		new PlatformInterfaceGalaxy.AchievementData(Achievement.BEAT_FLOOR_ONE_MULTI, "Castle Crasher", "Clear the First Chamber 50 Times", PlatformStat.FLOOR_ONE_CLEARS, 50),
		new PlatformInterfaceGalaxy.AchievementData(Achievement.BEAT_FLOOR_TWO_MULTI, "Dungeon Diver", "Clear the Second Chamber 40 Times", PlatformStat.FLOOR_TWO_CLEARS, 40),
		new PlatformInterfaceGalaxy.AchievementData(Achievement.BEAT_FLOOR_THREE_MULTI, "Mine Master", "Clear the Third Chamber 30 Times", PlatformStat.FLOOR_THREE_CLEARS, 30),
		new PlatformInterfaceGalaxy.AchievementData(Achievement.BEAT_FLOOR_FOUR_MULTI, "Hollowed Out", "Clear the Fourth Chamber 20 Times", PlatformStat.FLOOR_FOUR_CLEARS, 20),
		new PlatformInterfaceGalaxy.AchievementData(Achievement.BEAT_FLOOR_FIVE_MULTI, "Forger", "Clear the Fifth Chamber 10 Times", PlatformStat.FLOOR_FIVE_CLEARS, 10),
		new PlatformInterfaceGalaxy.AchievementData(Achievement.HAVE_MANY_COINS, "Biggest Wallet", "Carry a large amount of money at once"),
		new PlatformInterfaceGalaxy.AchievementData(Achievement.MAP_MAIN_FLOORS, "Cartographer's Assistant", "Map the first Five Chambers for the lost adventurer", PlatformStat.MAIN_FLOORS_MAPPED, 5),
		new PlatformInterfaceGalaxy.AchievementData(Achievement.REACH_SEWERS, "Grate Hall", "Access the Oubliette"),
		new PlatformInterfaceGalaxy.AchievementData(Achievement.REACH_CATHEDRAL, "Reverence for the Dead", "Access the Temple"),
		new PlatformInterfaceGalaxy.AchievementData(Achievement.COMPLETE_GOLEM_ARM, "Re-Armed", "Deliver the Golem's replacement arm"),
		new PlatformInterfaceGalaxy.AchievementData(Achievement.COMPLETE_FRIFLE_MULTI, "Weird Tale", "Complete Frifle's Challenges", PlatformStat.FRIFLE_CORE_COMPLETED, 15),
		new PlatformInterfaceGalaxy.AchievementData(Achievement.ACE_WINCHESTER_MULTI, "Trickshot", "Ace Winchester's game 3 times", PlatformStat.WINCHESTER_ACED, 3),
		new PlatformInterfaceGalaxy.AchievementData(Achievement.COMPLETE_GUNSLING_MULTI, "Hedge Slinger", "Win a wager against the Gunsling King 5 times", PlatformStat.GUNSLING_COMPLETED, 5),
		new PlatformInterfaceGalaxy.AchievementData(Achievement.UNLOCK_BULLET, "Case Closed", "Unlock the Bullet"),
		new PlatformInterfaceGalaxy.AchievementData(Achievement.UNLOCK_ROBOT, "Beep", "Unlock the Robot"),
		new PlatformInterfaceGalaxy.AchievementData(Achievement.UNLOCK_FLOOR_TWO_SHORTCUT, "Going Down", "Open the shortcut to the Second Chamber"),
		new PlatformInterfaceGalaxy.AchievementData(Achievement.UNLOCK_FLOOR_THREE_SHORTCUT, "Going Downer", "Open the shortcut to the Third Chamber"),
		new PlatformInterfaceGalaxy.AchievementData(Achievement.UNLOCK_FLOOR_FOUR_SHORTCUT, "Going Downest", "Open the shortcut to the Fourth Chamber"),
		new PlatformInterfaceGalaxy.AchievementData(Achievement.UNLOCK_FLOOR_FIVE_SHORTCUT, "Last Stop", "Open the shortcut to the Fifth Chamber"),
		new PlatformInterfaceGalaxy.AchievementData(Achievement.BEAT_MANFREDS_RIVAL, "Sworn Gun", "Avenge Manuel"),
		new PlatformInterfaceGalaxy.AchievementData(Achievement.BEAT_TUTORIAL, "Gungeon Acolyte", "Complete the Tutorial"),
		new PlatformInterfaceGalaxy.AchievementData(Achievement.POPULATE_BREACH, "Great Hall", "Populate the Breach", PlatformStat.BREACH_POPULATION, 12),
		new PlatformInterfaceGalaxy.AchievementData(Achievement.PREFIRE_ON_MIMIC, "Not Just A Box", "Get the jump on a Mimic"),
		new PlatformInterfaceGalaxy.AchievementData(Achievement.KILL_FROZEN_ENEMY_WITH_ROLL, "Demolition Man", "Kill a frozen enemy by rolling into it"),
		new PlatformInterfaceGalaxy.AchievementData(Achievement.PUSH_TABLE_INTO_PIT, "I Knew Someone Would Do It", "Why"),
		new PlatformInterfaceGalaxy.AchievementData(Achievement.STEAL_MULTI, "Woodsie Lord", "Steal 10 things", PlatformStat.ITEMS_STOLEN, 10),
		new PlatformInterfaceGalaxy.AchievementData(Achievement.KILL_BOSS_WITH_GLITTER, "Day Ruiner", "Kill a boss after covering it with glitter"),
		new PlatformInterfaceGalaxy.AchievementData(Achievement.FALL_IN_END_TIMES, "Lion Leap", "Fall at the last second"),
		new PlatformInterfaceGalaxy.AchievementData(Achievement.KILL_WITH_CHANDELIER_MULTI, "Money Pit", "Kill 100 enemies by dropping chandeliers", PlatformStat.CHANDELIER_KILLS, 100),
		new PlatformInterfaceGalaxy.AchievementData(Achievement.KILL_IN_MINE_CART_MULTI, "Rider", "Kill 100 enemies while riding in a mine cart", PlatformStat.MINECART_KILLS, 100),
		new PlatformInterfaceGalaxy.AchievementData(Achievement.KILL_WITH_PITS_MULTI, "Pit Lord", "Kill 100 enemies by knocking them into pits", PlatformStat.PIT_KILLS, 100),
		new PlatformInterfaceGalaxy.AchievementData(Achievement.DIE_IN_PAST, "Time Paradox", "Die in the Past"),
		new PlatformInterfaceGalaxy.AchievementData(Achievement.BEAT_A_JAMMED_BOSS, "Exorcist", "Kill a Jammed Boss"),
		new PlatformInterfaceGalaxy.AchievementData(Achievement.REACH_BLACK_MARKET, "The Password", "Accessed the Hidden Market"),
		new PlatformInterfaceGalaxy.AchievementData(Achievement.HAVE_MAX_CURSE, "Jammed", "You've met with a terrible fate, haven't you"),
		new PlatformInterfaceGalaxy.AchievementData(Achievement.FLIP_TABLES_MULTI, "Rage Mode", "Always be flipping. Guns are for flippers", PlatformStat.TABLES_FLIPPED, 500),
		new PlatformInterfaceGalaxy.AchievementData(Achievement.COMPLETE_GAME_WITH_BEAST_MODE, "Beast Master", "Complete the game with Beast Mode on"),
		new PlatformInterfaceGalaxy.AchievementData(Achievement.BEAT_PAST_ROBOT, "Terminated", "Kill the Robot's Past"),
		new PlatformInterfaceGalaxy.AchievementData(Achievement.BEAT_PAST_BULLET, "Hero of Gun", "Kill the Bullet's Past"),
		new PlatformInterfaceGalaxy.AchievementData(Achievement.COMPLETE_GAME_WITH_CHALLENGE_MODE, "Challenger", "Complete Daisuke's trial"),
		new PlatformInterfaceGalaxy.AchievementData(Achievement.BEAT_ADVANCED_DRAGUN, "Advanced Slayer", "Defeat an Advanced Boss"),
		new PlatformInterfaceGalaxy.AchievementData(Achievement.BEAT_METAL_GEAR_RAT, "Resourceful", "Take Revenge"),
		new PlatformInterfaceGalaxy.AchievementData(Achievement.COMPLETE_GAME_WITH_TURBO_MODE, "Sledge-Dog", "Complete Tonic's Challenge")
	};

	// Token: 0x04008303 RID: 33539
	private PlatformInterfaceGalaxy.StatData[] m_stats = new PlatformInterfaceGalaxy.StatData[]
	{
		new PlatformInterfaceGalaxy.StatData(PlatformStat.META_SPENT_AT_STORE),
		new PlatformInterfaceGalaxy.StatData(PlatformStat.FLOOR_ONE_CLEARS),
		new PlatformInterfaceGalaxy.StatData(PlatformStat.FLOOR_TWO_CLEARS),
		new PlatformInterfaceGalaxy.StatData(PlatformStat.FLOOR_THREE_CLEARS),
		new PlatformInterfaceGalaxy.StatData(PlatformStat.FLOOR_FOUR_CLEARS),
		new PlatformInterfaceGalaxy.StatData(PlatformStat.FLOOR_FIVE_CLEARS),
		new PlatformInterfaceGalaxy.StatData(PlatformStat.MAIN_FLOORS_MAPPED),
		new PlatformInterfaceGalaxy.StatData(PlatformStat.FRIFLE_CORE_COMPLETED),
		new PlatformInterfaceGalaxy.StatData(PlatformStat.WINCHESTER_ACED),
		new PlatformInterfaceGalaxy.StatData(PlatformStat.GUNSLING_COMPLETED),
		new PlatformInterfaceGalaxy.StatData(PlatformStat.BREACH_POPULATION),
		new PlatformInterfaceGalaxy.StatData(PlatformStat.ITEMS_STOLEN),
		new PlatformInterfaceGalaxy.StatData(PlatformStat.CHANDELIER_KILLS),
		new PlatformInterfaceGalaxy.StatData(PlatformStat.MINECART_KILLS),
		new PlatformInterfaceGalaxy.StatData(PlatformStat.PIT_KILLS),
		new PlatformInterfaceGalaxy.StatData(PlatformStat.TABLES_FLIPPED)
	};

	// Token: 0x020015C9 RID: 5577
	public class AchievementsReceivedListener : GlobalUserStatsAndAchievementsRetrieveListener
	{
		// Token: 0x06008025 RID: 32805 RVA: 0x0033CA58 File Offset: 0x0033AC58
		public AchievementsReceivedListener(PlatformInterfaceGalaxy platformInterface)
		{
			this.m_platformInterface = platformInterface;
		}

		// Token: 0x06008026 RID: 32806 RVA: 0x0033CA68 File Offset: 0x0033AC68
		public override void OnUserStatsAndAchievementsRetrieveSuccess(GalaxyID userID)
		{
			Debug.Log("Received achievement data!");
			this.m_platformInterface.OnUserStatsReceived();
			this.m_platformInterface.CatchupAchievements();
		}

		// Token: 0x06008027 RID: 32807 RVA: 0x0033CA8C File Offset: 0x0033AC8C
		public override void OnUserStatsAndAchievementsRetrieveFailure(GalaxyID userID, IUserStatsAndAchievementsRetrieveListener.FailureReason failureReason)
		{
			Debug.LogErrorFormat("OnUserStatsAndAchievementsRetrieveFailure() Error: {0} ", new object[] { failureReason });
		}

		// Token: 0x04008304 RID: 33540
		private PlatformInterfaceGalaxy m_platformInterface;
	}

	// Token: 0x020015CA RID: 5578
	public class StatsStoredListener : GlobalStatsAndAchievementsStoreListener
	{
		// Token: 0x06008029 RID: 32809 RVA: 0x0033CAB0 File Offset: 0x0033ACB0
		public override void OnUserStatsAndAchievementsStoreSuccess()
		{
			Debug.Log("Stats and achievements stored!");
		}

		// Token: 0x0600802A RID: 32810 RVA: 0x0033CABC File Offset: 0x0033ACBC
		public override void OnUserStatsAndAchievementsStoreFailure(IStatsAndAchievementsStoreListener.FailureReason failureReason)
		{
			Debug.LogErrorFormat("OnUserStatsAndAchievementsStoreFailure() Error: {0} ", new object[] { failureReason });
		}
	}

	// Token: 0x020015CB RID: 5579
	private class AchievementData
	{
		// Token: 0x0600802B RID: 32811 RVA: 0x0033CAD8 File Offset: 0x0033ACD8
		public AchievementData(Achievement achievement, string name, string desc)
		{
			this.achievement = achievement;
			this.name = name;
			this.description = desc;
			this.isUnlocked = false;
		}

		// Token: 0x0600802C RID: 32812 RVA: 0x0033CAFC File Offset: 0x0033ACFC
		public AchievementData(Achievement achievement, string name, string desc, PlatformStat progressStat, int goalValue)
		{
			this.achievement = achievement;
			this.name = name;
			this.description = desc;
			this.isUnlocked = false;
			this.hasProgressStat = true;
			this.progressStat = progressStat;
			this.goalValue = goalValue;
		}

		// Token: 0x170012FB RID: 4859
		// (get) Token: 0x0600802D RID: 32813 RVA: 0x0033CB38 File Offset: 0x0033AD38
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

		// Token: 0x04008305 RID: 33541
		public Achievement achievement;

		// Token: 0x04008306 RID: 33542
		public string name;

		// Token: 0x04008307 RID: 33543
		public string description;

		// Token: 0x04008308 RID: 33544
		public bool isUnlocked;

		// Token: 0x04008309 RID: 33545
		public uint unlockTime;

		// Token: 0x0400830A RID: 33546
		public bool hasProgressStat;

		// Token: 0x0400830B RID: 33547
		public PlatformStat progressStat;

		// Token: 0x0400830C RID: 33548
		public int goalValue;

		// Token: 0x0400830D RID: 33549
		private string m_cachedApiKey;
	}

	// Token: 0x020015CC RID: 5580
	private class StatData
	{
		// Token: 0x0600802E RID: 32814 RVA: 0x0033CB64 File Offset: 0x0033AD64
		public StatData(PlatformStat stat)
		{
			this.stat = stat;
		}

		// Token: 0x170012FC RID: 4860
		// (get) Token: 0x0600802F RID: 32815 RVA: 0x0033CB74 File Offset: 0x0033AD74
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

		// Token: 0x0400830E RID: 33550
		public PlatformStat stat;

		// Token: 0x0400830F RID: 33551
		public int value;

		// Token: 0x04008310 RID: 33552
		private string m_cachedApiKey;
	}
}
