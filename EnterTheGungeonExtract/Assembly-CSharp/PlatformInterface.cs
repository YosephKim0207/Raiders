using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

// Token: 0x020015C1 RID: 5569
public abstract class PlatformInterface
{
	// Token: 0x06007FF7 RID: 32759 RVA: 0x0033B32C File Offset: 0x0033952C
	public void Start()
	{
		this.OnStart();
		PlatformInterface.InitializeAlienFXController();
	}

	// Token: 0x06007FF8 RID: 32760 RVA: 0x0033B33C File Offset: 0x0033953C
	public virtual void SignIn()
	{
	}

	// Token: 0x06007FF9 RID: 32761 RVA: 0x0033B340 File Offset: 0x00339540
	public void AchievementUnlock(Achievement achievement, int playerIndex = 0)
	{
		this.SetGungeonFlagForAchievement(achievement);
		this.OnAchievementUnlock(achievement, playerIndex);
	}

	// Token: 0x06007FFA RID: 32762 RVA: 0x0033B354 File Offset: 0x00339554
	public virtual bool IsAchievementUnlocked(Achievement achievement)
	{
		return false;
	}

	// Token: 0x06007FFB RID: 32763 RVA: 0x0033B358 File Offset: 0x00339558
	public virtual void SetStat(PlatformStat stat, int value)
	{
	}

	// Token: 0x06007FFC RID: 32764 RVA: 0x0033B35C File Offset: 0x0033955C
	public virtual void IncrementStat(PlatformStat stat, int delta)
	{
	}

	// Token: 0x06007FFD RID: 32765 RVA: 0x0033B360 File Offset: 0x00339560
	public virtual void SendEvent(string eventName, int value)
	{
	}

	// Token: 0x06007FFE RID: 32766 RVA: 0x0033B364 File Offset: 0x00339564
	public virtual void SetPresence(string presence)
	{
	}

	// Token: 0x06007FFF RID: 32767 RVA: 0x0033B368 File Offset: 0x00339568
	public virtual void StoreStats()
	{
	}

	// Token: 0x06008000 RID: 32768 RVA: 0x0033B36C File Offset: 0x0033956C
	public virtual void ResetStats(bool achievementsToo)
	{
	}

	// Token: 0x06008001 RID: 32769 RVA: 0x0033B370 File Offset: 0x00339570
	public void ProcessDlcUnlocks()
	{
		if (Time.realtimeSinceStartup < this.m_lastDlcCheckTime + 1f)
		{
			return;
		}
		this.GalaxyMtxGunHack();
		for (int i = 0; i < this.UnlockedDlc.Count; i++)
		{
			PlatformDlc platformDlc = this.UnlockedDlc[i];
			for (int j = 0; j < this.m_dlcUnlockedItems.Length; j++)
			{
				if (this.m_dlcUnlockedItems[j].PlatformDlc == platformDlc)
				{
					PlatformInterface.DlcUnlockedItem dlcUnlockedItem = this.m_dlcUnlockedItems[j];
					EncounterDatabaseEntry entry = EncounterDatabase.GetEntry(dlcUnlockedItem.encounterGuid);
					if (entry != null && !entry.PrerequisitesMet())
					{
						GameStatsManager.Instance.ForceUnlock(dlcUnlockedItem.encounterGuid);
					}
					if (dlcUnlockedItem.flag != GungeonFlags.NONE && !GameStatsManager.Instance.GetFlag(dlcUnlockedItem.flag))
					{
						GameStatsManager.Instance.SetFlag(dlcUnlockedItem.flag, true);
					}
				}
			}
		}
		this.m_lastDlcCheckTime = Time.realtimeSinceStartup;
	}

	// Token: 0x06008002 RID: 32770 RVA: 0x0033B464 File Offset: 0x00339664
	public void LateUpdate()
	{
		this.OnLateUpdate();
		PlatformInterface.UpdateAlienFXController();
	}

	// Token: 0x06008003 RID: 32771 RVA: 0x0033B474 File Offset: 0x00339674
	public void CatchupAchievements()
	{
		if (this.m_hasCaughtUpAchievements)
		{
			return;
		}
		if (GameManager.Options.wipeAllAchievements)
		{
			this.ResetStats(true);
			GameManager.Options.wipeAllAchievements = false;
		}
		if (GameManager.Options.scanAchievementsForUnlocks)
		{
			if (this.IsAchievementUnlocked(Achievement.COLLECT_FIVE_MASTERY_TOKENS))
			{
				GameStatsManager.Instance.SetFlag(GungeonFlags.ACHIEVEMENT_LEAD_GOD, true);
			}
			GameManager.Options.scanAchievementsForUnlocks = false;
		}
		IEnumerator enumerator = Enum.GetValues(typeof(TrackedStats)).GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				TrackedStats trackedStats = (TrackedStats)obj;
				GameStatsManager.Instance.HandleStatAchievements(trackedStats);
			}
		}
		finally
		{
			IDisposable disposable;
			if ((disposable = enumerator as IDisposable) != null)
			{
				disposable.Dispose();
			}
		}
		foreach (GungeonFlags gungeonFlags in GameStatsManager.Instance.m_flags)
		{
			GameStatsManager.Instance.HandleFlagAchievements(gungeonFlags);
		}
		if (GameStatsManager.Instance.GetFlag(GungeonFlags.ACHIEVEMENT_TABLE_PIT))
		{
			this.AchievementUnlock(Achievement.PUSH_TABLE_INTO_PIT, 0);
		}
		if (GameStatsManager.Instance.GetFlag(GungeonFlags.ACHIEVEMENT_BEASTMODE))
		{
			this.AchievementUnlock(Achievement.COMPLETE_GAME_WITH_BEAST_MODE, 0);
		}
		if (GameStatsManager.Instance.GetFlag(GungeonFlags.ACHIEVEMENT_CONSTRUCT_BULLET))
		{
			this.AchievementUnlock(Achievement.BUILD_BULLET, 0);
		}
		if (GameStatsManager.Instance.GetFlag(GungeonFlags.ACHIEVEMENT_ACCESS_OUBLIETTE))
		{
			this.AchievementUnlock(Achievement.REACH_SEWERS, 0);
		}
		if (GameStatsManager.Instance.GetFlag(GungeonFlags.ACHIEVEMENT_ACCESS_ABBEY))
		{
			this.AchievementUnlock(Achievement.REACH_CATHEDRAL, 0);
		}
		if (GameStatsManager.Instance.GetFlag(GungeonFlags.ACHIEVEMENT_SURPRISE_MIMIC))
		{
			this.AchievementUnlock(Achievement.PREFIRE_ON_MIMIC, 0);
		}
		if (GameStatsManager.Instance.GetFlag(GungeonFlags.ACHIEVEMENT_KILL_JAMMED_BOSS))
		{
			this.AchievementUnlock(Achievement.BEAT_A_JAMMED_BOSS, 0);
		}
		if (GameStatsManager.Instance.GetFlag(GungeonFlags.ACHIEVEMENT_BIGGEST_WALLET))
		{
			this.AchievementUnlock(Achievement.HAVE_MANY_COINS, 0);
		}
		if (GameStatsManager.Instance.GetFlag(GungeonFlags.ACHIEVEMENT_LEAD_GOD))
		{
			this.AchievementUnlock(Achievement.COLLECT_FIVE_MASTERY_TOKENS, 0);
		}
		if (GameStatsManager.Instance.GetFlag(GungeonFlags.BLACKSMITH_RECEIVED_BUSTED_TELEVISION))
		{
			this.AchievementUnlock(Achievement.UNLOCK_ROBOT, 0);
		}
		if (GameStatsManager.Instance.GetFlag(GungeonFlags.SECRET_BULLETMAN_SEEN_05))
		{
			this.AchievementUnlock(Achievement.UNLOCK_BULLET, 0);
		}
		this.m_hasCaughtUpAchievements = true;
	}

	// Token: 0x06008004 RID: 32772 RVA: 0x0033B6E4 File Offset: 0x003398E4
	public StringTableManager.GungeonSupportedLanguages GetPreferredLanguage()
	{
		return this.OnGetPreferredLanguage();
	}

	// Token: 0x06008005 RID: 32773 RVA: 0x0033B6EC File Offset: 0x003398EC
	public static void InitializeAlienFXController()
	{
	}

	// Token: 0x06008006 RID: 32774 RVA: 0x0033B6FC File Offset: 0x003398FC
	public static void SetAlienFXAmbientColor(Color32 color)
	{
		if (PlatformInterface.m_useLightFX)
		{
			PlatformInterface.m_AlienFXAmbientEffect = new PlatformInterface.LightFXUnit(color, 1f);
		}
	}

	// Token: 0x06008007 RID: 32775 RVA: 0x0033B718 File Offset: 0x00339918
	public static void SetAlienFXColor(Color32 color, float duration)
	{
		if (PlatformInterface.m_useLightFX)
		{
			PlatformInterface.LightFXUnit lightFXUnit = new PlatformInterface.LightFXUnit(color, duration);
			PlatformInterface.m_AlienFXExtantEffects.Add(lightFXUnit);
		}
	}

	// Token: 0x06008008 RID: 32776 RVA: 0x0033B744 File Offset: 0x00339944
	public static void UpdateAlienFXController()
	{
		if (!PlatformInterface.m_useLightFX)
		{
			return;
		}
		if (PlatformInterface.m_AlienFXExtantEffects.Count > 0)
		{
			Color32 color = new Color32(0, 0, 0, 0);
			for (int i = 0; i < PlatformInterface.m_AlienFXExtantEffects.Count; i++)
			{
				PlatformInterface.LightFXUnit lightFXUnit = PlatformInterface.m_AlienFXExtantEffects[i];
				lightFXUnit.remainingDuration -= BraveTime.DeltaTime;
				if (lightFXUnit.remainingDuration <= 0f)
				{
					PlatformInterface.m_AlienFXExtantEffects.RemoveAt(i);
					i--;
				}
				else
				{
					byte b = (byte)Mathf.Lerp(0f, (float)lightFXUnit.TargetLightColor.a, lightFXUnit.remainingDuration / lightFXUnit.startDuration);
					color.a = (byte)Mathf.Min(255, (int)(color.a + b));
					color.r = (byte)Mathf.Min(255, (int)(color.r + lightFXUnit.TargetLightColor.r));
					color.g = (byte)Mathf.Min(255, (int)(color.g + lightFXUnit.TargetLightColor.g));
					color.b = (byte)Mathf.Min(255, (int)(color.b + lightFXUnit.TargetLightColor.b));
					PlatformInterface.m_AlienFXExtantEffects[i] = lightFXUnit;
				}
			}
			float num = (float)color.a / 255f;
			Color color2 = PlatformInterface.m_AlienFXAmbientEffect.TargetLightColor;
			if (GameManager.Instance.DungeonMusicController && GameManager.Instance.DungeonMusicController.ShouldPulseLightFX)
			{
				float num2 = Mathf.Clamp01(Mathf.Lerp(color2.a, color2.a - 0.25f, Mathf.PingPong(Time.realtimeSinceStartup, 5f) / 5f));
				color2.a = num2;
			}
			color = Color32.Lerp(color2, color, num);
			AlienFXInterface._LFX_COLOR lfx_COLOR = new AlienFXInterface._LFX_COLOR(color);
			uint num3 = 0U;
			if (AlienFXInterface.LFX_GetNumDevices(ref num3) == 0U)
			{
				for (uint num4 = 0U; num4 < num3; num4 += 1U)
				{
					uint num5 = 0U;
					if (AlienFXInterface.LFX_GetNumLights(num4, ref num5) == 0U)
					{
						for (uint num6 = 0U; num6 < num5; num6 += 1U)
						{
							AlienFXInterface.LFX_SetLightColor(num4, num6, ref lfx_COLOR);
						}
					}
				}
			}
		}
		else if (PlatformInterface.m_AlienFXAmbientEffect.TargetLightColor.a > 0)
		{
			Color color3 = PlatformInterface.m_AlienFXAmbientEffect.TargetLightColor;
			if (GameManager.Instance.DungeonMusicController && GameManager.Instance.DungeonMusicController.ShouldPulseLightFX)
			{
				float num7 = Mathf.Clamp01(Mathf.Lerp(color3.a, 0f, Mathf.PingPong(Time.realtimeSinceStartup, 2f) / 2f));
				color3.a = num7;
			}
			AlienFXInterface._LFX_COLOR lfx_COLOR2 = new AlienFXInterface._LFX_COLOR(color3);
			uint num8 = 0U;
			if (AlienFXInterface.LFX_GetNumDevices(ref num8) == 0U)
			{
				for (uint num9 = 0U; num9 < num8; num9 += 1U)
				{
					uint num10 = 0U;
					if (AlienFXInterface.LFX_GetNumLights(num9, ref num10) == 0U)
					{
						for (uint num11 = 0U; num11 < num10; num11 += 1U)
						{
							AlienFXInterface.LFX_SetLightColor(num9, num11, ref lfx_COLOR2);
						}
					}
				}
			}
		}
		else
		{
			AlienFXInterface.LFX_Reset();
		}
		AlienFXInterface.LFX_Update();
	}

	// Token: 0x06008009 RID: 32777 RVA: 0x0033BAA4 File Offset: 0x00339CA4
	public static void CleanupAlienFXController()
	{
		if (!PlatformInterface.m_useLightFX)
		{
			return;
		}
		AlienFXInterface.LFX_Release();
	}

	// Token: 0x0600800A RID: 32778 RVA: 0x0033BAB8 File Offset: 0x00339CB8
	protected void SetGungeonFlagForAchievement(Achievement achievement)
	{
		switch (achievement)
		{
		case Achievement.POPULATE_BREACH:
			GameStatsManager.Instance.SetFlag(GungeonFlags.ACHIEVEMENT_BREACH_POPULATED, true);
			break;
		case Achievement.PREFIRE_ON_MIMIC:
			GameStatsManager.Instance.SetFlag(GungeonFlags.ACHIEVEMENT_SURPRISE_MIMIC, true);
			break;
		default:
			switch (achievement)
			{
			case Achievement.BEAT_FLOOR_ONE_MULTI:
				GameStatsManager.Instance.SetFlag(GungeonFlags.ACHIEVEMENT_CASTLE_MANYTIMES, true);
				break;
			case Achievement.BEAT_FLOOR_TWO_MULTI:
				GameStatsManager.Instance.SetFlag(GungeonFlags.ACHIEVEMENT_GUNGEON_MANYTIMES, true);
				break;
			case Achievement.BEAT_FLOOR_THREE_MULTI:
				GameStatsManager.Instance.SetFlag(GungeonFlags.ACHIEVEMENT_MINES_MANYTIMES, true);
				break;
			case Achievement.BEAT_FLOOR_FOUR_MULTI:
				GameStatsManager.Instance.SetFlag(GungeonFlags.ACHIEVEMENT_HOLLOW_MANYTIMES, true);
				break;
			case Achievement.BEAT_FLOOR_FIVE_MULTI:
				GameStatsManager.Instance.SetFlag(GungeonFlags.ACHIEVEMENT_FORGE_MANYTIMES, true);
				break;
			case Achievement.COMPLETE_GAME_WITH_STARTER_GUN:
				GameStatsManager.Instance.SetFlag(GungeonFlags.ACHIEVEMENT_STARTING_GUN, true);
				break;
			default:
				switch (achievement)
				{
				case Achievement.COMPLETE_GAME_WITH_ENCHANTED_GUN:
					GameStatsManager.Instance.SetFlag(GungeonFlags.ACHIEVEMENT_GUNGAME_COMPLETE, true);
					break;
				default:
					if (achievement == Achievement.COLLECT_FIVE_MASTERY_TOKENS)
					{
						GameStatsManager.Instance.SetFlag(GungeonFlags.ACHIEVEMENT_LEAD_GOD, true);
					}
					break;
				case Achievement.BUILD_BULLET:
					GameStatsManager.Instance.SetFlag(GungeonFlags.ACHIEVEMENT_CONSTRUCT_BULLET, true);
					break;
				case Achievement.BEAT_PAST_ALL:
					GameStatsManager.Instance.SetFlag(GungeonFlags.ACHIEVEMENT_COMPLETE_FOUR_PASTS, true);
					break;
				}
				break;
			case Achievement.REACH_SEWERS:
				GameStatsManager.Instance.SetFlag(GungeonFlags.ACHIEVEMENT_ACCESS_OUBLIETTE, true);
				break;
			case Achievement.REACH_CATHEDRAL:
				GameStatsManager.Instance.SetFlag(GungeonFlags.ACHIEVEMENT_ACCESS_ABBEY, true);
				break;
			}
			break;
		case Achievement.PUSH_TABLE_INTO_PIT:
			GameStatsManager.Instance.SetFlag(GungeonFlags.ACHIEVEMENT_TABLE_PIT, true);
			break;
		case Achievement.HAVE_MANY_COINS:
			GameStatsManager.Instance.SetFlag(GungeonFlags.ACHIEVEMENT_BIGGEST_WALLET, true);
			break;
		case Achievement.STEAL_MULTI:
			GameStatsManager.Instance.SetFlag(GungeonFlags.ACHIEVEMENT_STEAL_THINGS, true);
			break;
		case Achievement.KILL_WITH_PITS_MULTI:
			GameStatsManager.Instance.SetFlag(GungeonFlags.ACHIEVEMENT_PIT_LORD, true);
			break;
		case Achievement.BEAT_A_JAMMED_BOSS:
			GameStatsManager.Instance.SetFlag(GungeonFlags.ACHIEVEMENT_KILL_JAMMED_BOSS, true);
			break;
		case Achievement.FLIP_TABLES_MULTI:
			GameStatsManager.Instance.SetFlag(GungeonFlags.ACHIEVEMENT_TABLES_FLIPPED, true);
			break;
		case Achievement.COMPLETE_GAME_WITH_BEAST_MODE:
			GameStatsManager.Instance.SetFlag(GungeonFlags.ACHIEVEMENT_BEASTMODE, true);
			break;
		case Achievement.COMPLETE_GAME_WITH_CHALLENGE_MODE:
			GameStatsManager.Instance.SetFlag(GungeonFlags.ACHIEVEMENT_CHALLENGE_MODE_COMPLETE, true);
			break;
		}
	}

	// Token: 0x0600800B RID: 32779
	protected abstract void OnStart();

	// Token: 0x0600800C RID: 32780
	protected abstract void OnAchievementUnlock(Achievement achievement, int playerIndex);

	// Token: 0x0600800D RID: 32781
	protected abstract void OnLateUpdate();

	// Token: 0x0600800E RID: 32782
	protected abstract StringTableManager.GungeonSupportedLanguages OnGetPreferredLanguage();

	// Token: 0x0600800F RID: 32783 RVA: 0x0033BD44 File Offset: 0x00339F44
	private void GalaxyMtxGunHack()
	{
		if (this.m_hasCheckedForGalaxyMtx)
		{
			return;
		}
		this.m_hasCheckedForGalaxyMtx = true;
		if (PlatformInterfaceSteam.IsSteamBuild())
		{
			return;
		}
		string text = Path.Combine(Application.dataPath, "../Unlock MTX Gun.dat");
		if (text == null)
		{
			return;
		}
		if (!File.Exists(text))
		{
			return;
		}
		byte[] array = File.ReadAllBytes(text);
		byte[] array2 = PlatformInterface.StringToByteArray("e226 87d5 f590 279d 38f5 fe7b 07fe cdf5 41c8 1c7d 257f 6ad5 d293 985e 994e 3032 c91d 8d6e 5697 5abb 8ee6 15ab 9afc 12e2 f8cf d5dd 8339 f987 6bcb ba0e 6280 1386 2881 c560 5980 457f c52f 1378 18ad f5da c8ec a283 f32e 8e78 0970 ea11 213a ed71 66d2 6ab2 7124 2c4e 6778 0e61 ada5 f225 e921 6326 2126 cd37 183b db48 3110 c14b 3358 c772 fbce a89b bde0 5ba9 6458 3acf 9307 2496 3be6 825d 1d75 84db 379e c360 7da9 0342 1042 7f5f 89ba 77e3 e74c 1195 f896 ff9a b1db 1350 2dce b368 7884 d5ad 5e6e 5957 fe74 1980 fabe 0e90 bf57 e29d 0239 0355 8ca7 d212 450b c426 10c2 7098 63a6 769b e827 d5e0 0d65 d6d7 fb3c e531 d0e8 bf83 5d2a bc83 388d 4b8f 8a22 b424");
		if (array.Length != array2.Length)
		{
			return;
		}
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i] != array2[i])
			{
				return;
			}
		}
		if (!this.UnlockedDlc.Contains(PlatformDlc.EARLY_MTX_GUN))
		{
			this.UnlockedDlc.Add(PlatformDlc.EARLY_MTX_GUN);
		}
	}

	// Token: 0x06008010 RID: 32784 RVA: 0x0033BDF0 File Offset: 0x00339FF0
	public static byte[] StringToByteArray(string hex)
	{
		hex = hex.Replace(" ", string.Empty);
		return (from x in Enumerable.Range(0, hex.Length)
			where x % 2 == 0
			select Convert.ToByte(hex.Substring(x, 2), 16)).ToArray<byte>();
	}

	// Token: 0x06008011 RID: 32785 RVA: 0x0033BE70 File Offset: 0x0033A070
	public static string GetTrackedStatEventString(TrackedStats stat)
	{
		string text = string.Empty;
		if (stat != TrackedStats.NUMBER_DEATHS)
		{
			if (stat != TrackedStats.ENEMIES_KILLED)
			{
				if (stat != TrackedStats.TIMES_KILLED_PAST)
				{
					if (stat == TrackedStats.TABLES_FLIPPED)
					{
						text = "TablesFlipped";
					}
				}
				else
				{
					text = "PastsKilled";
				}
			}
			else
			{
				text = "EnemiesKilled";
			}
		}
		else
		{
			text = "Deaths";
		}
		return text;
	}

	// Token: 0x04008295 RID: 33429
	public List<PlatformDlc> UnlockedDlc = new List<PlatformDlc>();

	// Token: 0x04008296 RID: 33430
	public static float LastManyCoinsUnlockTime = 0f;

	// Token: 0x04008297 RID: 33431
	private float m_lastDlcCheckTime;

	// Token: 0x04008298 RID: 33432
	private bool m_hasCaughtUpAchievements;

	// Token: 0x04008299 RID: 33433
	private static bool m_useLightFX = false;

	// Token: 0x0400829A RID: 33434
	private static List<PlatformInterface.LightFXUnit> m_AlienFXExtantEffects = new List<PlatformInterface.LightFXUnit>();

	// Token: 0x0400829B RID: 33435
	private static PlatformInterface.LightFXUnit m_AlienFXAmbientEffect;

	// Token: 0x0400829C RID: 33436
	private PlatformInterface.DlcUnlockedItem[] m_dlcUnlockedItems = new PlatformInterface.DlcUnlockedItem[]
	{
		new PlatformInterface.DlcUnlockedItem(PlatformDlc.EARLY_MTX_GUN, "5c2241fc117740d59ad8e29f5324b773", GungeonFlags.BLUEPRINTMETA_MTXGUN),
		new PlatformInterface.DlcUnlockedItem(PlatformDlc.EARLY_COBALT_HAMMER, "2d91904ba70a4c0d861dac03a6417591", GungeonFlags.NONE)
	};

	// Token: 0x0400829D RID: 33437
	private bool m_hasCheckedForGalaxyMtx;

	// Token: 0x020015C2 RID: 5570
	private struct LightFXUnit
	{
		// Token: 0x06008014 RID: 32788 RVA: 0x0033BEF8 File Offset: 0x0033A0F8
		public LightFXUnit(Color32 sourceColor, float sourceDuration)
		{
			this.TargetLightColor = sourceColor;
			this.remainingDuration = sourceDuration;
			this.startDuration = sourceDuration;
		}

		// Token: 0x0400829F RID: 33439
		public Color32 TargetLightColor;

		// Token: 0x040082A0 RID: 33440
		public float remainingDuration;

		// Token: 0x040082A1 RID: 33441
		public float startDuration;
	}

	// Token: 0x020015C3 RID: 5571
	private class DlcUnlockedItem
	{
		// Token: 0x06008015 RID: 32789 RVA: 0x0033BF10 File Offset: 0x0033A110
		public DlcUnlockedItem(PlatformDlc PlatformDlc, string encounterGuid, GungeonFlags flag = GungeonFlags.NONE)
		{
			this.PlatformDlc = PlatformDlc;
			this.encounterGuid = encounterGuid;
			this.flag = flag;
		}

		// Token: 0x040082A2 RID: 33442
		public PlatformDlc PlatformDlc;

		// Token: 0x040082A3 RID: 33443
		public string encounterGuid;

		// Token: 0x040082A4 RID: 33444
		public GungeonFlags flag;
	}
}
