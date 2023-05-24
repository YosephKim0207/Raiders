using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using InControl;
using UnityEngine;

// Token: 0x020017F1 RID: 6129
public static class StringTableManager
{
	// Token: 0x06009042 RID: 36930 RVA: 0x003CF9C8 File Offset: 0x003CDBC8
	public static void ReloadAllTables()
	{
		StringTableManager.m_coreTable = null;
		StringTableManager.m_enemiesTable = null;
		StringTableManager.m_itemsTable = null;
		StringTableManager.m_uiTable = null;
		StringTableManager.m_introTable = null;
		StringTableManager.m_synergyTable = null;
		StringTableManager.m_backupCoreTable = null;
		StringTableManager.m_backupEnemiesTable = null;
		StringTableManager.m_backupIntroTable = null;
		StringTableManager.m_backupItemsTable = null;
		StringTableManager.m_backupSynergyTable = null;
		StringTableManager.m_backupUiTable = null;
	}

	// Token: 0x06009043 RID: 36931 RVA: 0x003CFA20 File Offset: 0x003CDC20
	public static string GetString(string key)
	{
		if (StringTableManager.m_coreTable == null)
		{
			StringTableManager.m_coreTable = StringTableManager.LoadTables(StringTableManager.m_currentFile);
		}
		if (StringTableManager.m_backupCoreTable == null)
		{
			StringTableManager.m_backupCoreTable = StringTableManager.LoadTables("english");
		}
		if (StringTableManager.m_coreTable.ContainsKey(key))
		{
			return StringTableManager.PostprocessString(StringTableManager.m_coreTable[key].GetWeightedString());
		}
		if (StringTableManager.m_backupCoreTable.ContainsKey(key))
		{
			return StringTableManager.PostprocessString(StringTableManager.m_backupCoreTable[key].GetWeightedString());
		}
		return "STRING_NOT_FOUND";
	}

	// Token: 0x06009044 RID: 36932 RVA: 0x003CFAB0 File Offset: 0x003CDCB0
	public static string GetExactString(string key, int index)
	{
		if (StringTableManager.m_coreTable == null)
		{
			StringTableManager.m_coreTable = StringTableManager.LoadTables(StringTableManager.m_currentFile);
		}
		if (StringTableManager.m_backupCoreTable == null)
		{
			StringTableManager.m_backupCoreTable = StringTableManager.LoadTables("english");
		}
		if (StringTableManager.m_coreTable.ContainsKey(key))
		{
			return StringTableManager.PostprocessString(StringTableManager.m_coreTable[key].GetExactString(index));
		}
		if (StringTableManager.m_backupCoreTable.ContainsKey(key))
		{
			return StringTableManager.PostprocessString(StringTableManager.m_backupCoreTable[key].GetExactString(index));
		}
		return "STRING_NOT_FOUND";
	}

	// Token: 0x06009045 RID: 36933 RVA: 0x003CFB44 File Offset: 0x003CDD44
	public static string GetEnemiesLongDescription(string key)
	{
		if (StringTableManager.m_enemiesTable == null)
		{
			StringTableManager.m_enemiesTable = StringTableManager.LoadEnemiesTable(StringTableManager.m_currentSubDirectory);
		}
		if (StringTableManager.m_backupEnemiesTable == null)
		{
			StringTableManager.m_backupEnemiesTable = StringTableManager.LoadEnemiesTable("english_items");
		}
		if (StringTableManager.m_enemiesTable.ContainsKey(key))
		{
			return StringTableManager.PostprocessString(StringTableManager.m_enemiesTable[key].GetCombinedString());
		}
		if (StringTableManager.m_backupEnemiesTable.ContainsKey(key))
		{
			return StringTableManager.PostprocessString(StringTableManager.m_backupEnemiesTable[key].GetCombinedString());
		}
		return "ENEMIES_STRING_NOT_FOUND";
	}

	// Token: 0x06009046 RID: 36934 RVA: 0x003CFBD4 File Offset: 0x003CDDD4
	public static string GetItemsLongDescription(string key)
	{
		if (StringTableManager.m_itemsTable == null)
		{
			StringTableManager.m_itemsTable = StringTableManager.LoadItemsTable(StringTableManager.m_currentSubDirectory);
		}
		if (StringTableManager.m_backupItemsTable == null)
		{
			StringTableManager.m_backupItemsTable = StringTableManager.LoadItemsTable("english_items");
		}
		if (StringTableManager.m_itemsTable.ContainsKey(key))
		{
			return StringTableManager.PostprocessString(StringTableManager.m_itemsTable[key].GetCombinedString());
		}
		if (StringTableManager.m_backupItemsTable.ContainsKey(key))
		{
			return StringTableManager.PostprocessString(StringTableManager.m_backupItemsTable[key].GetCombinedString());
		}
		return "ITEMS_STRING_NOT_FOUND";
	}

	// Token: 0x06009047 RID: 36935 RVA: 0x003CFC64 File Offset: 0x003CDE64
	public static string GetEnemiesString(string key, int index = -1)
	{
		if (StringTableManager.m_enemiesTable == null)
		{
			StringTableManager.m_enemiesTable = StringTableManager.LoadEnemiesTable(StringTableManager.m_currentSubDirectory);
		}
		if (StringTableManager.m_backupEnemiesTable == null)
		{
			StringTableManager.m_backupEnemiesTable = StringTableManager.LoadEnemiesTable("english_items");
		}
		if (StringTableManager.m_enemiesTable.ContainsKey(key))
		{
			if (index == -1)
			{
				string weightedString = StringTableManager.m_enemiesTable[key].GetWeightedString();
				return StringTableManager.PostprocessString(weightedString);
			}
			return StringTableManager.PostprocessString(StringTableManager.m_enemiesTable[key].GetExactString(index));
		}
		else
		{
			if (!StringTableManager.m_backupEnemiesTable.ContainsKey(key))
			{
				return "ENEMIES_STRING_NOT_FOUND";
			}
			if (index == -1)
			{
				string weightedString2 = StringTableManager.m_backupEnemiesTable[key].GetWeightedString();
				return StringTableManager.PostprocessString(weightedString2);
			}
			return StringTableManager.PostprocessString(StringTableManager.m_backupEnemiesTable[key].GetExactString(index));
		}
	}

	// Token: 0x06009048 RID: 36936 RVA: 0x003CFD34 File Offset: 0x003CDF34
	public static string GetIntroString(string key)
	{
		if (StringTableManager.m_introTable == null)
		{
			StringTableManager.m_introTable = StringTableManager.LoadIntroTable(StringTableManager.m_currentSubDirectory);
		}
		if (StringTableManager.m_backupIntroTable == null)
		{
			StringTableManager.m_backupIntroTable = StringTableManager.LoadIntroTable("english_items");
		}
		if (StringTableManager.m_introTable.ContainsKey(key))
		{
			string weightedString = StringTableManager.m_introTable[key].GetWeightedString();
			return StringTableManager.PostprocessString(weightedString);
		}
		if (StringTableManager.m_backupIntroTable.ContainsKey(key))
		{
			string weightedString2 = StringTableManager.m_backupIntroTable[key].GetWeightedString();
			return StringTableManager.PostprocessString(weightedString2);
		}
		return "INTRO_STRING_NOT_FOUND";
	}

	// Token: 0x06009049 RID: 36937 RVA: 0x003CFDC8 File Offset: 0x003CDFC8
	public static string GetItemsString(string key, int index = -1)
	{
		if (StringTableManager.m_itemsTable == null)
		{
			StringTableManager.m_itemsTable = StringTableManager.LoadItemsTable(StringTableManager.m_currentSubDirectory);
		}
		if (StringTableManager.m_backupItemsTable == null)
		{
			StringTableManager.m_backupItemsTable = StringTableManager.LoadItemsTable("english_items");
		}
		if (StringTableManager.m_itemsTable.ContainsKey(key))
		{
			if (index == -1)
			{
				string weightedString = StringTableManager.m_itemsTable[key].GetWeightedString();
				return StringTableManager.PostprocessString(weightedString);
			}
			return StringTableManager.PostprocessString(StringTableManager.m_itemsTable[key].GetExactString(index));
		}
		else
		{
			if (!StringTableManager.m_backupItemsTable.ContainsKey(key))
			{
				return "ITEMS_STRING_NOT_FOUND";
			}
			if (index == -1)
			{
				string weightedString2 = StringTableManager.m_backupItemsTable[key].GetWeightedString();
				return StringTableManager.PostprocessString(weightedString2);
			}
			return StringTableManager.PostprocessString(StringTableManager.m_backupItemsTable[key].GetExactString(index));
		}
	}

	// Token: 0x0600904A RID: 36938 RVA: 0x003CFE98 File Offset: 0x003CE098
	public static string GetUIString(string key, int index = -1)
	{
		if (StringTableManager.m_uiTable == null)
		{
			StringTableManager.m_uiTable = StringTableManager.LoadUITable(StringTableManager.m_currentSubDirectory);
		}
		if (StringTableManager.m_backupUiTable == null)
		{
			StringTableManager.m_backupUiTable = StringTableManager.LoadUITable(StringTableManager.m_currentSubDirectory);
		}
		if (StringTableManager.m_uiTable.ContainsKey(key))
		{
			if (index == -1)
			{
				return StringTableManager.PostprocessString(StringTableManager.m_uiTable[key].GetWeightedString());
			}
			return StringTableManager.PostprocessString(StringTableManager.m_uiTable[key].GetExactString(index));
		}
		else
		{
			if (!StringTableManager.m_backupUiTable.ContainsKey(key))
			{
				return "ITEMS_STRING_NOT_FOUND";
			}
			if (index == -1)
			{
				return StringTableManager.PostprocessString(StringTableManager.m_backupUiTable[key].GetWeightedString());
			}
			return StringTableManager.PostprocessString(StringTableManager.m_backupUiTable[key].GetExactString(index));
		}
	}

	// Token: 0x0600904B RID: 36939 RVA: 0x003CFF64 File Offset: 0x003CE164
	public static string GetSynergyString(string key, int index = -1)
	{
		if (StringTableManager.m_synergyTable == null)
		{
			StringTableManager.m_synergyTable = StringTableManager.LoadSynergyTable(StringTableManager.m_currentSubDirectory);
		}
		if (StringTableManager.m_backupSynergyTable == null)
		{
			StringTableManager.m_backupSynergyTable = StringTableManager.LoadSynergyTable("english_items");
		}
		if (!StringTableManager.m_synergyTable.ContainsKey(key))
		{
			return string.Empty;
		}
		if (index == -1)
		{
			string weightedString = StringTableManager.m_synergyTable[key].GetWeightedString();
			return StringTableManager.PostprocessString(weightedString);
		}
		return StringTableManager.PostprocessString(StringTableManager.m_synergyTable[key].GetExactString(index));
	}

	// Token: 0x0600904C RID: 36940 RVA: 0x003CFFF0 File Offset: 0x003CE1F0
	public static string GetStringSequential(string key, ref int lastIndex, bool repeatLast = false)
	{
		bool flag;
		return StringTableManager.GetStringSequential(key, ref lastIndex, out flag, repeatLast);
	}

	// Token: 0x0600904D RID: 36941 RVA: 0x003D0008 File Offset: 0x003CE208
	public static string GetStringSequential(string key, ref int lastIndex, out bool isLast, bool repeatLast = false)
	{
		isLast = false;
		if (StringTableManager.m_coreTable == null)
		{
			StringTableManager.m_coreTable = StringTableManager.LoadTables(StringTableManager.m_currentFile);
		}
		if (StringTableManager.m_backupCoreTable == null)
		{
			StringTableManager.m_backupCoreTable = StringTableManager.LoadTables("english");
		}
		if (StringTableManager.m_coreTable.ContainsKey(key))
		{
			string weightedStringSequential = StringTableManager.m_coreTable[key].GetWeightedStringSequential(ref lastIndex, out isLast, repeatLast);
			return StringTableManager.PostprocessString(weightedStringSequential);
		}
		if (StringTableManager.m_backupCoreTable.ContainsKey(key))
		{
			string weightedStringSequential2 = StringTableManager.m_backupCoreTable[key].GetWeightedStringSequential(ref lastIndex, out isLast, repeatLast);
			return StringTableManager.PostprocessString(weightedStringSequential2);
		}
		return "STRING_NOT_FOUND";
	}

	// Token: 0x0600904E RID: 36942 RVA: 0x003D00A8 File Offset: 0x003CE2A8
	public static string GetStringPersistentSequential(string key)
	{
		if (StringTableManager.m_coreTable == null)
		{
			StringTableManager.m_coreTable = StringTableManager.LoadTables(StringTableManager.m_currentFile);
		}
		if (StringTableManager.m_backupCoreTable == null)
		{
			StringTableManager.m_backupCoreTable = StringTableManager.LoadTables("english");
		}
		if (StringTableManager.m_coreTable.ContainsKey(key))
		{
			int persistentStringLastIndex = GameStatsManager.Instance.GetPersistentStringLastIndex(key);
			bool flag;
			string weightedStringSequential = StringTableManager.m_coreTable[key].GetWeightedStringSequential(ref persistentStringLastIndex, out flag, false);
			GameStatsManager.Instance.SetPersistentStringLastIndex(key, persistentStringLastIndex);
			return StringTableManager.PostprocessString(weightedStringSequential);
		}
		if (StringTableManager.m_backupCoreTable.ContainsKey(key))
		{
			int persistentStringLastIndex2 = GameStatsManager.Instance.GetPersistentStringLastIndex(key);
			bool flag2;
			string weightedStringSequential2 = StringTableManager.m_backupCoreTable[key].GetWeightedStringSequential(ref persistentStringLastIndex2, out flag2, false);
			GameStatsManager.Instance.SetPersistentStringLastIndex(key, persistentStringLastIndex2);
			return StringTableManager.PostprocessString(weightedStringSequential2);
		}
		return "STRING_NOT_FOUND";
	}

	// Token: 0x0600904F RID: 36943 RVA: 0x003D017C File Offset: 0x003CE37C
	public static int GetNumStrings(string key)
	{
		if (StringTableManager.m_coreTable == null)
		{
			StringTableManager.m_coreTable = StringTableManager.LoadTables(StringTableManager.m_currentFile);
		}
		if (StringTableManager.m_backupCoreTable == null)
		{
			StringTableManager.m_backupCoreTable = StringTableManager.LoadTables("english");
		}
		if (StringTableManager.m_coreTable.ContainsKey(key))
		{
			return StringTableManager.m_coreTable[key].Count();
		}
		if (StringTableManager.m_backupCoreTable.ContainsKey(key))
		{
			return StringTableManager.m_backupCoreTable[key].Count();
		}
		return 0;
	}

	// Token: 0x06009050 RID: 36944 RVA: 0x003D0200 File Offset: 0x003CE400
	public static string GetLongString(string key)
	{
		if (StringTableManager.m_coreTable == null)
		{
			StringTableManager.m_coreTable = StringTableManager.LoadTables(StringTableManager.m_currentFile);
		}
		if (StringTableManager.m_backupCoreTable == null)
		{
			StringTableManager.m_backupCoreTable = StringTableManager.LoadTables("english");
		}
		if (StringTableManager.m_coreTable.ContainsKey(key))
		{
			return StringTableManager.PostprocessString(StringTableManager.m_coreTable[key].GetCombinedString());
		}
		if (StringTableManager.m_backupCoreTable.ContainsKey(key))
		{
			return StringTableManager.PostprocessString(StringTableManager.m_backupCoreTable[key].GetCombinedString());
		}
		return "STRING_NOT_FOUND";
	}

	// Token: 0x06009051 RID: 36945 RVA: 0x003D0290 File Offset: 0x003CE490
	public static void LoadTablesIfNecessary()
	{
		if (StringTableManager.m_coreTable == null)
		{
			StringTableManager.m_coreTable = StringTableManager.LoadTables(StringTableManager.m_currentFile);
		}
		if (StringTableManager.m_backupCoreTable == null)
		{
			StringTableManager.m_backupCoreTable = StringTableManager.LoadTables("english");
		}
	}

	// Token: 0x06009052 RID: 36946 RVA: 0x003D02C4 File Offset: 0x003CE4C4
	public static Dictionary<string, StringTableManager.StringCollection> LoadTables(string currentFile)
	{
		TextAsset textAsset = (TextAsset)BraveResources.Load("strings/" + currentFile, typeof(TextAsset), ".txt");
		if (textAsset == null)
		{
			Debug.LogError("Failed to load string table.");
			return null;
		}
		StringReader stringReader = new StringReader(textAsset.text);
		Dictionary<string, StringTableManager.StringCollection> dictionary = new Dictionary<string, StringTableManager.StringCollection>();
		StringTableManager.StringCollection stringCollection = null;
		string text;
		while ((text = stringReader.ReadLine()) != null)
		{
			if (!text.StartsWith("//"))
			{
				if (text.StartsWith("#"))
				{
					stringCollection = new StringTableManager.ComplexStringCollection();
					if (dictionary.ContainsKey(text))
					{
						Debug.LogError("Attempting to add the key " + text + " twice to the string table!");
					}
					else
					{
						dictionary.Add(text, stringCollection);
					}
				}
				else
				{
					string[] array = text.Split(new char[] { '|' });
					if (array.Length == 1)
					{
						stringCollection.AddString(array[0], 1f);
					}
					else
					{
						CultureInfo invariantCulture = CultureInfo.InvariantCulture;
						stringCollection.AddString(array[1], float.Parse(array[0], NumberStyles.Any, invariantCulture.NumberFormat));
					}
				}
			}
		}
		return dictionary;
	}

	// Token: 0x17001596 RID: 5526
	// (get) Token: 0x06009053 RID: 36947 RVA: 0x003D03F4 File Offset: 0x003CE5F4
	// (set) Token: 0x06009054 RID: 36948 RVA: 0x003D0400 File Offset: 0x003CE600
	public static StringTableManager.GungeonSupportedLanguages CurrentLanguage
	{
		get
		{
			return GameManager.Options.CurrentLanguage;
		}
		set
		{
			StringTableManager.SetNewLanguage(value, true);
		}
	}

	// Token: 0x06009055 RID: 36949 RVA: 0x003D040C File Offset: 0x003CE60C
	public static void SetNewLanguage(StringTableManager.GungeonSupportedLanguages language, bool force = false)
	{
		if (!force && StringTableManager.CurrentLanguage == language)
		{
			return;
		}
		switch (language)
		{
		case StringTableManager.GungeonSupportedLanguages.ENGLISH:
			StringTableManager.m_currentFile = "english";
			StringTableManager.m_currentSubDirectory = "english_items";
			goto IL_179;
		case StringTableManager.GungeonSupportedLanguages.FRENCH:
			StringTableManager.m_currentFile = "french";
			StringTableManager.m_currentSubDirectory = "french_items";
			goto IL_179;
		case StringTableManager.GungeonSupportedLanguages.SPANISH:
			StringTableManager.m_currentFile = "spanish";
			StringTableManager.m_currentSubDirectory = "spanish_items";
			goto IL_179;
		case StringTableManager.GungeonSupportedLanguages.ITALIAN:
			StringTableManager.m_currentFile = "italian";
			StringTableManager.m_currentSubDirectory = "italian_items";
			goto IL_179;
		case StringTableManager.GungeonSupportedLanguages.GERMAN:
			StringTableManager.m_currentFile = "german";
			StringTableManager.m_currentSubDirectory = "german_items";
			goto IL_179;
		case StringTableManager.GungeonSupportedLanguages.BRAZILIANPORTUGUESE:
			StringTableManager.m_currentFile = "portuguese";
			StringTableManager.m_currentSubDirectory = "portuguese_items";
			goto IL_179;
		case StringTableManager.GungeonSupportedLanguages.JAPANESE:
			StringTableManager.m_currentFile = "japanese";
			StringTableManager.m_currentSubDirectory = "japanese_items";
			goto IL_179;
		case StringTableManager.GungeonSupportedLanguages.KOREAN:
			StringTableManager.m_currentFile = "korean";
			StringTableManager.m_currentSubDirectory = "korean_items";
			goto IL_179;
		case StringTableManager.GungeonSupportedLanguages.RUSSIAN:
			StringTableManager.m_currentFile = "russian";
			StringTableManager.m_currentSubDirectory = "russian_items";
			goto IL_179;
		case StringTableManager.GungeonSupportedLanguages.POLISH:
			StringTableManager.m_currentFile = "polish";
			StringTableManager.m_currentSubDirectory = "polish_items";
			goto IL_179;
		case StringTableManager.GungeonSupportedLanguages.CHINESE:
			StringTableManager.m_currentFile = "chinese";
			StringTableManager.m_currentSubDirectory = "chinese_items";
			goto IL_179;
		}
		StringTableManager.m_currentFile = "english";
		StringTableManager.m_currentSubDirectory = "english_items";
		IL_179:
		StringTableManager.ReloadAllTables();
		dfLanguageManager.ChangeGungeonLanguage();
		JournalEntry.ReloadDataSemaphore++;
	}

	// Token: 0x06009056 RID: 36950 RVA: 0x003D05A8 File Offset: 0x003CE7A8
	private static Dictionary<string, StringTableManager.StringCollection> LoadEnemiesTable(string subDirectory)
	{
		TextAsset textAsset = (TextAsset)BraveResources.Load("strings/" + subDirectory + "/enemies", typeof(TextAsset), ".txt");
		if (textAsset == null)
		{
			Debug.LogError("Failed to load string table: ENEMIES.");
			return null;
		}
		StringReader stringReader = new StringReader(textAsset.text);
		Dictionary<string, StringTableManager.StringCollection> dictionary = new Dictionary<string, StringTableManager.StringCollection>();
		StringTableManager.StringCollection stringCollection = null;
		string text;
		while ((text = stringReader.ReadLine()) != null)
		{
			if (!text.StartsWith("//"))
			{
				if (text.StartsWith("#"))
				{
					stringCollection = new StringTableManager.ComplexStringCollection();
					if (dictionary.ContainsKey(text))
					{
						Debug.LogError("Failed to add duplicate key to items table: " + text);
					}
					else
					{
						dictionary.Add(text, stringCollection);
					}
				}
				else
				{
					string[] array = text.Split(new char[] { '|' });
					if (array.Length == 1)
					{
						stringCollection.AddString(array[0], 1f);
					}
					else
					{
						stringCollection.AddString(array[1], float.Parse(array[0]));
					}
				}
			}
		}
		return dictionary;
	}

	// Token: 0x06009057 RID: 36951 RVA: 0x003D06C4 File Offset: 0x003CE8C4
	public static TextAsset GetUIDataFile()
	{
		return (TextAsset)BraveResources.Load("strings/" + StringTableManager.m_currentSubDirectory + "/ui", typeof(TextAsset), ".txt");
	}

	// Token: 0x06009058 RID: 36952 RVA: 0x003D0700 File Offset: 0x003CE900
	public static TextAsset GetBackupUIDataFile()
	{
		return (TextAsset)BraveResources.Load("strings/english_items/ui", typeof(TextAsset), ".txt");
	}

	// Token: 0x06009059 RID: 36953 RVA: 0x003D0730 File Offset: 0x003CE930
	private static Dictionary<string, StringTableManager.StringCollection> LoadSynergyTable(string subDirectory)
	{
		TextAsset textAsset = (TextAsset)BraveResources.Load("strings/" + subDirectory + "/synergies", typeof(TextAsset), ".txt");
		Dictionary<string, StringTableManager.StringCollection> dictionary = new Dictionary<string, StringTableManager.StringCollection>();
		if (textAsset == null)
		{
			Debug.LogError("Failed to load string table: ITEMS.");
			return dictionary;
		}
		StringReader stringReader = new StringReader(textAsset.text);
		StringTableManager.StringCollection stringCollection = null;
		string text;
		while ((text = stringReader.ReadLine()) != null)
		{
			if (!text.StartsWith("//"))
			{
				if (text.StartsWith("#"))
				{
					stringCollection = new StringTableManager.ComplexStringCollection();
					if (dictionary.ContainsKey(text))
					{
						Debug.LogError("Failed to add duplicate key to synergies table: " + text);
					}
					else
					{
						dictionary.Add(text, stringCollection);
					}
				}
				else
				{
					string[] array = text.Split(new char[] { '|' });
					if (array.Length == 1)
					{
						stringCollection.AddString(array[0], 1f);
					}
					else
					{
						stringCollection.AddString(array[1], float.Parse(array[0]));
					}
				}
			}
		}
		return dictionary;
	}

	// Token: 0x0600905A RID: 36954 RVA: 0x003D084C File Offset: 0x003CEA4C
	private static Dictionary<string, StringTableManager.StringCollection> LoadUITable(string subDirectory)
	{
		TextAsset textAsset = (TextAsset)BraveResources.Load("strings/" + subDirectory + "/ui", typeof(TextAsset), ".txt");
		if (textAsset == null)
		{
			Debug.LogError("Failed to load string table: ITEMS.");
			return null;
		}
		StringReader stringReader = new StringReader(textAsset.text);
		Dictionary<string, StringTableManager.StringCollection> dictionary = new Dictionary<string, StringTableManager.StringCollection>();
		StringTableManager.StringCollection stringCollection = null;
		string text;
		while ((text = stringReader.ReadLine()) != null)
		{
			if (!text.StartsWith("//"))
			{
				if (text.StartsWith("#"))
				{
					stringCollection = new StringTableManager.ComplexStringCollection();
					if (dictionary.ContainsKey(text))
					{
						Debug.LogError("Failed to add duplicate key to items table: " + text);
					}
					else
					{
						dictionary.Add(text, stringCollection);
					}
				}
				else
				{
					string[] array = text.Split(new char[] { '|' });
					if (array.Length == 1)
					{
						stringCollection.AddString(array[0], 1f);
					}
					else
					{
						stringCollection.AddString(array[1], float.Parse(array[0]));
					}
				}
			}
		}
		return dictionary;
	}

	// Token: 0x0600905B RID: 36955 RVA: 0x003D0968 File Offset: 0x003CEB68
	private static Dictionary<string, StringTableManager.StringCollection> LoadIntroTable(string subDirectory)
	{
		TextAsset textAsset = (TextAsset)BraveResources.Load("strings/" + subDirectory + "/intro", typeof(TextAsset), ".txt");
		if (textAsset == null)
		{
			Debug.LogError("Failed to load string table: INTRO.");
			return null;
		}
		StringReader stringReader = new StringReader(textAsset.text);
		Dictionary<string, StringTableManager.StringCollection> dictionary = new Dictionary<string, StringTableManager.StringCollection>();
		StringTableManager.StringCollection stringCollection = null;
		string text;
		while ((text = stringReader.ReadLine()) != null)
		{
			if (!text.StartsWith("//"))
			{
				if (text.StartsWith("#"))
				{
					stringCollection = new StringTableManager.ComplexStringCollection();
					if (dictionary.ContainsKey(text))
					{
						Debug.LogError("Failed to add duplicate key to items table: " + text);
					}
					else
					{
						dictionary.Add(text, stringCollection);
					}
				}
				else
				{
					string[] array = text.Split(new char[] { '|' });
					if (array.Length == 1)
					{
						stringCollection.AddString(array[0], 1f);
					}
					else
					{
						stringCollection.AddString(array[1], float.Parse(array[0]));
					}
				}
			}
		}
		return dictionary;
	}

	// Token: 0x0600905C RID: 36956 RVA: 0x003D0A84 File Offset: 0x003CEC84
	private static Dictionary<string, StringTableManager.StringCollection> LoadItemsTable(string subDirectory)
	{
		TextAsset textAsset = (TextAsset)BraveResources.Load("strings/" + subDirectory + "/items", typeof(TextAsset), ".txt");
		if (textAsset == null)
		{
			Debug.LogError("Failed to load string table: ITEMS.");
			return null;
		}
		StringReader stringReader = new StringReader(textAsset.text);
		Dictionary<string, StringTableManager.StringCollection> dictionary = new Dictionary<string, StringTableManager.StringCollection>();
		StringTableManager.StringCollection stringCollection = null;
		string text;
		while ((text = stringReader.ReadLine()) != null)
		{
			if (!text.StartsWith("//"))
			{
				if (text.StartsWith("#"))
				{
					stringCollection = new StringTableManager.ComplexStringCollection();
					if (dictionary.ContainsKey(text))
					{
						Debug.LogError("Failed to add duplicate key to items table: " + text);
					}
					else
					{
						dictionary.Add(text, stringCollection);
					}
				}
				else
				{
					string[] array = text.Split(new char[] { '|' });
					if (array.Length == 1)
					{
						stringCollection.AddString(array[0], 1f);
					}
					else
					{
						stringCollection.AddString(array[1], float.Parse(array[0]));
					}
				}
			}
		}
		return dictionary;
	}

	// Token: 0x0600905D RID: 36957 RVA: 0x003D0BA0 File Offset: 0x003CEDA0
	public static string GetBindingText(GungeonActions.GungeonActionType ActionType)
	{
		GungeonActions gungeonActions;
		if (!GameManager.Instance.IsSelectingCharacter)
		{
			gungeonActions = BraveInput.GetInstanceForPlayer(GameManager.Instance.PrimaryPlayer.PlayerIDX).ActiveActions;
		}
		else
		{
			gungeonActions = BraveInput.PlayerlessInstance.ActiveActions;
		}
		if (gungeonActions == null)
		{
			return string.Empty;
		}
		PlayerAction actionFromType = gungeonActions.GetActionFromType(ActionType);
		if (actionFromType == null || actionFromType.Bindings == null)
		{
			return string.Empty;
		}
		bool flag = false;
		string text = "-";
		for (int i = 0; i < actionFromType.Bindings.Count; i++)
		{
			BindingSource bindingSource = actionFromType.Bindings[i];
			if ((bindingSource.BindingSourceType == BindingSourceType.KeyBindingSource || bindingSource.BindingSourceType == BindingSourceType.MouseBindingSource) && !flag)
			{
				text = bindingSource.Name;
				break;
			}
		}
		return text.Trim();
	}

	// Token: 0x0600905E RID: 36958 RVA: 0x003D0C80 File Offset: 0x003CEE80
	private static PlayerController GetTalkingPlayer()
	{
		List<TalkDoerLite> allNpcs = StaticReferenceManager.AllNpcs;
		for (int i = 0; i < allNpcs.Count; i++)
		{
			if (allNpcs[i])
			{
				if (!allNpcs[i].IsTalking || !allNpcs[i].TalkingPlayer || GameManager.Instance.HasPlayer(allNpcs[i].TalkingPlayer))
				{
					if (allNpcs[i].IsTalking && allNpcs[i].TalkingPlayer)
					{
						return allNpcs[i].TalkingPlayer;
					}
				}
			}
		}
		return GameManager.Instance.PrimaryPlayer;
	}

	// Token: 0x0600905F RID: 36959 RVA: 0x003D0D48 File Offset: 0x003CEF48
	private static string GetTalkingPlayerName()
	{
		PlayerController talkingPlayer = StringTableManager.GetTalkingPlayer();
		if (talkingPlayer.IsThief)
		{
			return "#THIEF_NAME";
		}
		if (talkingPlayer.characterIdentity == PlayableCharacters.Eevee)
		{
			return "#PLAYER_NAME_RANDOM";
		}
		if (talkingPlayer.characterIdentity == PlayableCharacters.Gunslinger)
		{
			return "#PLAYER_NAME_GUNSLINGER";
		}
		return "#PLAYER_NAME_" + talkingPlayer.characterIdentity.ToString().ToUpperInvariant();
	}

	// Token: 0x06009060 RID: 36960 RVA: 0x003D0DB4 File Offset: 0x003CEFB4
	private static string GetTalkingPlayerNick()
	{
		PlayerController talkingPlayer = StringTableManager.GetTalkingPlayer();
		if (talkingPlayer.IsThief)
		{
			return "#THIEF_NAME";
		}
		if (talkingPlayer.characterIdentity == PlayableCharacters.Eevee)
		{
			return "#PLAYER_NICK_RANDOM";
		}
		if (talkingPlayer.characterIdentity == PlayableCharacters.Gunslinger)
		{
			return "#PLAYER_NICK_GUNSLINGER";
		}
		return "#PLAYER_NICK_" + talkingPlayer.characterIdentity.ToString().ToUpperInvariant();
	}

	// Token: 0x06009061 RID: 36961 RVA: 0x003D0E20 File Offset: 0x003CF020
	public static string GetPlayerName(PlayableCharacters player)
	{
		return StringTableManager.GetString("#PLAYER_NAME_" + player.ToString().ToUpperInvariant());
	}

	// Token: 0x06009062 RID: 36962 RVA: 0x003D0E44 File Offset: 0x003CF044
	public static string EvaluateReplacementToken(string input)
	{
		BraveInput primaryPlayerInstance = BraveInput.PrimaryPlayerInstance;
		GungeonActions gungeonActions = ((!(primaryPlayerInstance != null)) ? null : primaryPlayerInstance.ActiveActions);
		if (input == "%META_CURRENCY_SYMBOL")
		{
			return "[sprite \"hbux_text_icon\"]";
		}
		if (input == "%CURRENCY_SYMBOL")
		{
			return "[sprite \"ui_coin\"]";
		}
		if (input == "%KEY_SYMBOL")
		{
			return "[sprite \"ui_key\"]";
		}
		if (input == "%BLANK_SYMBOL")
		{
			return "[sprite \"ui_blank\"]";
		}
		if (input == "%PLAYER_NAME")
		{
			return StringTableManager.GetString(StringTableManager.GetTalkingPlayerName());
		}
		if (input == "%PLAYER_NICK")
		{
			return StringTableManager.GetString(StringTableManager.GetTalkingPlayerNick());
		}
		if (input == "%BRACELETRED_ENCNAME")
		{
			return StringTableManager.GetItemsString("#BRACELETRED_ENCNAME", -1);
		}
		if (input == "%PLAYER_THIEF")
		{
			return StringTableManager.GetString("#THIEF_NAME");
		}
		if (input == "%INSULT")
		{
			return StringTableManager.GetString("#INSULT_NAME");
		}
		if (input == "%CONTROL_INTERACT_MAP")
		{
			if (primaryPlayerInstance.IsKeyboardAndMouse(false))
			{
				return StringTableManager.GetBindingText(GungeonActions.GungeonActionType.Interact);
			}
			return UIControllerButtonHelper.GetUnifiedControllerButtonTag(InputControlType.Action1, BraveInput.PlayerOneCurrentSymbology, null);
		}
		else if (input == "%CONTROL_INTERACT")
		{
			if (primaryPlayerInstance.IsKeyboardAndMouse(false))
			{
				return StringTableManager.GetBindingText(GungeonActions.GungeonActionType.Interact);
			}
			if (gungeonActions != null && gungeonActions.InteractAction.Bindings.Count > 0)
			{
				DeviceBindingSource deviceBindingSource = gungeonActions.InteractAction.Bindings[0] as DeviceBindingSource;
				if (deviceBindingSource != null && deviceBindingSource.Control != InputControlType.None)
				{
					return UIControllerButtonHelper.GetUnifiedControllerButtonTag(deviceBindingSource.Control, BraveInput.PlayerOneCurrentSymbology, null);
				}
			}
			return UIControllerButtonHelper.GetUnifiedControllerButtonTag(InputControlType.Action1, BraveInput.PlayerOneCurrentSymbology, null);
		}
		else if (input == "%CONTROL_DODGEROLL")
		{
			if (primaryPlayerInstance.IsKeyboardAndMouse(false))
			{
				return StringTableManager.GetBindingText(GungeonActions.GungeonActionType.DodgeRoll);
			}
			if (gungeonActions != null && gungeonActions.DodgeRollAction.Bindings.Count > 0)
			{
				DeviceBindingSource deviceBindingSource2 = gungeonActions.DodgeRollAction.Bindings[0] as DeviceBindingSource;
				if (deviceBindingSource2 != null && deviceBindingSource2.Control != InputControlType.None)
				{
					return UIControllerButtonHelper.GetUnifiedControllerButtonTag(deviceBindingSource2.Control, BraveInput.PlayerOneCurrentSymbology, null);
				}
			}
			if (GameManager.Options.CurrentControlPreset == GameOptions.ControlPreset.FLIPPED_TRIGGERS)
			{
				return UIControllerButtonHelper.GetUnifiedControllerButtonTag(InputControlType.LeftTrigger, BraveInput.PlayerOneCurrentSymbology, null);
			}
			return UIControllerButtonHelper.GetUnifiedControllerButtonTag(InputControlType.LeftBumper, BraveInput.PlayerOneCurrentSymbology, null);
		}
		else if (input == "%CONTROL_PAUSE")
		{
			if (primaryPlayerInstance.IsKeyboardAndMouse(false))
			{
				return StringTableManager.GetBindingText(GungeonActions.GungeonActionType.Pause);
			}
			if (gungeonActions != null && gungeonActions.PauseAction.Bindings.Count > 0)
			{
				DeviceBindingSource deviceBindingSource3 = gungeonActions.PauseAction.Bindings[0] as DeviceBindingSource;
				if (deviceBindingSource3 != null && deviceBindingSource3.Control != InputControlType.None)
				{
					return UIControllerButtonHelper.GetUnifiedControllerButtonTag(deviceBindingSource3.Control, BraveInput.PlayerOneCurrentSymbology, null);
				}
			}
			return UIControllerButtonHelper.GetUnifiedControllerButtonTag(InputControlType.Start, BraveInput.PlayerOneCurrentSymbology, null);
		}
		else if (input == "%CONTROL_USEITEM")
		{
			if (primaryPlayerInstance.IsKeyboardAndMouse(false))
			{
				return StringTableManager.GetBindingText(GungeonActions.GungeonActionType.UseItem);
			}
			if (gungeonActions != null && gungeonActions.UseItemAction.Bindings.Count > 0)
			{
				DeviceBindingSource deviceBindingSource4 = gungeonActions.UseItemAction.Bindings[0] as DeviceBindingSource;
				if (deviceBindingSource4 != null && deviceBindingSource4.Control != InputControlType.None)
				{
					return UIControllerButtonHelper.GetUnifiedControllerButtonTag(deviceBindingSource4.Control, BraveInput.PlayerOneCurrentSymbology, null);
				}
			}
			if (GameManager.Options.CurrentControlPreset == GameOptions.ControlPreset.FLIPPED_TRIGGERS)
			{
				return UIControllerButtonHelper.GetUnifiedControllerButtonTag(InputControlType.RightBumper, BraveInput.PlayerOneCurrentSymbology, null);
			}
			return UIControllerButtonHelper.GetUnifiedControllerButtonTag(InputControlType.RightTrigger, BraveInput.PlayerOneCurrentSymbology, null);
		}
		else
		{
			if (input == "%CONTROL_USEBLANK")
			{
				if (gungeonActions != null && gungeonActions.BlankAction.Bindings.Count > 0)
				{
					DeviceBindingSource deviceBindingSource5 = gungeonActions.BlankAction.Bindings[0] as DeviceBindingSource;
					if (deviceBindingSource5 != null && deviceBindingSource5.Control != InputControlType.None)
					{
						return UIControllerButtonHelper.GetUnifiedControllerButtonTag(deviceBindingSource5.Control, BraveInput.PlayerOneCurrentSymbology, null);
					}
				}
				return StringTableManager.GetBindingText(GungeonActions.GungeonActionType.Blank);
			}
			if (input == "%CONTROL_R_STICK_DOWN")
			{
				if (BraveInput.PlayerOneCurrentSymbology == GameOptions.ControllerSymbology.Xbox)
				{
					return "[sprite \"xbone_RS\"]";
				}
				if (BraveInput.PlayerOneCurrentSymbology == GameOptions.ControllerSymbology.Switch)
				{
					return "[sprite \"switch_r3\"]";
				}
				return "[sprite \"ps4_R3\"]";
			}
			else if (input == "%CONTROL_L_STICK_DOWN")
			{
				if (BraveInput.PlayerOneCurrentSymbology == GameOptions.ControllerSymbology.Xbox)
				{
					return "[sprite \"xbone_LS\"]";
				}
				if (BraveInput.PlayerOneCurrentSymbology == GameOptions.ControllerSymbology.Switch)
				{
					return "[sprite \"switch_l3\"]";
				}
				return "[sprite \"ps4_L3\"]";
			}
			else if (input == "%CONTROL_ALT_DODGEROLL")
			{
				if (primaryPlayerInstance.IsKeyboardAndMouse(false))
				{
					return "Circle";
				}
				return UIControllerButtonHelper.GetUnifiedControllerButtonTag(InputControlType.Action2, BraveInput.PlayerOneCurrentSymbology, null);
			}
			else if (input == "%CONTROL_AIM")
			{
				if (primaryPlayerInstance.IsKeyboardAndMouse(false))
				{
					return "Mouse";
				}
				return UIControllerButtonHelper.GetUnifiedControllerButtonTag("RightStick", BraveInput.PlayerOneCurrentSymbology);
			}
			else
			{
				if (input == "%SYMBOL_TELEPORTER")
				{
					return UIControllerButtonHelper.GetUnifiedControllerButtonTag("Teleporter", BraveInput.PlayerOneCurrentSymbology);
				}
				if (input == "%CONTROL_FIRE")
				{
					if (primaryPlayerInstance.IsKeyboardAndMouse(false))
					{
						return StringTableManager.GetBindingText(GungeonActions.GungeonActionType.Shoot);
					}
					if (gungeonActions != null && gungeonActions.ShootAction.Bindings.Count > 0)
					{
						DeviceBindingSource deviceBindingSource6 = gungeonActions.ShootAction.Bindings[0] as DeviceBindingSource;
						if (deviceBindingSource6 != null && deviceBindingSource6.Control != InputControlType.None)
						{
							return UIControllerButtonHelper.GetUnifiedControllerButtonTag(deviceBindingSource6.Control, BraveInput.PlayerOneCurrentSymbology, null);
						}
					}
					if (GameManager.Options.CurrentControlPreset == GameOptions.ControlPreset.FLIPPED_TRIGGERS)
					{
						return UIControllerButtonHelper.GetUnifiedControllerButtonTag(InputControlType.RightTrigger, BraveInput.PlayerOneCurrentSymbology, null);
					}
					return UIControllerButtonHelper.GetUnifiedControllerButtonTag(InputControlType.RightBumper, BraveInput.PlayerOneCurrentSymbology, null);
				}
				else if (input == "%CONTROL_MAP")
				{
					if (primaryPlayerInstance.IsKeyboardAndMouse(false))
					{
						return StringTableManager.GetBindingText(GungeonActions.GungeonActionType.Map);
					}
					if (gungeonActions != null && gungeonActions.MapAction.Bindings.Count > 0)
					{
						DeviceBindingSource deviceBindingSource7 = gungeonActions.MapAction.Bindings[0] as DeviceBindingSource;
						if (deviceBindingSource7 != null && deviceBindingSource7.Control != InputControlType.None)
						{
							return UIControllerButtonHelper.GetUnifiedControllerButtonTag(deviceBindingSource7.Control, BraveInput.PlayerOneCurrentSymbology, null);
						}
					}
					if (GameManager.Options.CurrentControlPreset == GameOptions.ControlPreset.FLIPPED_TRIGGERS)
					{
						return UIControllerButtonHelper.GetUnifiedControllerButtonTag(InputControlType.LeftBumper, BraveInput.PlayerOneCurrentSymbology, null);
					}
					return UIControllerButtonHelper.GetUnifiedControllerButtonTag(InputControlType.LeftTrigger, BraveInput.PlayerOneCurrentSymbology, null);
				}
				else if (input == "%CONTROL_RELOAD")
				{
					if (primaryPlayerInstance.IsKeyboardAndMouse(false))
					{
						return StringTableManager.GetBindingText(GungeonActions.GungeonActionType.Reload);
					}
					if (gungeonActions != null && gungeonActions.ReloadAction.Bindings.Count > 0)
					{
						DeviceBindingSource deviceBindingSource8 = gungeonActions.ReloadAction.Bindings[0] as DeviceBindingSource;
						if (deviceBindingSource8 != null && deviceBindingSource8.Control != InputControlType.None)
						{
							return UIControllerButtonHelper.GetUnifiedControllerButtonTag(deviceBindingSource8.Control, BraveInput.PlayerOneCurrentSymbology, null);
						}
					}
					return UIControllerButtonHelper.GetUnifiedControllerButtonTag(InputControlType.Action3, BraveInput.PlayerOneCurrentSymbology, null);
				}
				else if (input == "%CONTROL_QUICKSWITCHGUN")
				{
					if (primaryPlayerInstance.IsKeyboardAndMouse(false))
					{
						return StringTableManager.GetBindingText(GungeonActions.GungeonActionType.GunQuickEquip);
					}
					if (gungeonActions != null && gungeonActions.GunQuickEquipAction.Bindings.Count > 0)
					{
						DeviceBindingSource deviceBindingSource9 = gungeonActions.GunQuickEquipAction.Bindings[0] as DeviceBindingSource;
						if (deviceBindingSource9 != null && deviceBindingSource9.Control != InputControlType.None)
						{
							return UIControllerButtonHelper.GetUnifiedControllerButtonTag(deviceBindingSource9.Control, BraveInput.PlayerOneCurrentSymbology, null);
						}
					}
					return UIControllerButtonHelper.GetUnifiedControllerButtonTag(InputControlType.Action4, BraveInput.PlayerOneCurrentSymbology, null);
				}
				else if (input == "%CONTROL_SWITCHGUN_ALT")
				{
					if (primaryPlayerInstance.IsKeyboardAndMouse(false))
					{
						return StringTableManager.GetBindingText(GungeonActions.GungeonActionType.DropGun);
					}
					return UIControllerButtonHelper.GetUnifiedControllerButtonTag(InputControlType.DPadDown, BraveInput.PlayerOneCurrentSymbology, null);
				}
				else
				{
					if (input == "%CONTROL_CIRCLE")
					{
						return UIControllerButtonHelper.GetUnifiedControllerButtonTag(InputControlType.Action2, BraveInput.PlayerOneCurrentSymbology, null);
					}
					if (input == "%CONTROL_L1")
					{
						return UIControllerButtonHelper.GetUnifiedControllerButtonTag(InputControlType.LeftBumper, BraveInput.PlayerOneCurrentSymbology, null);
					}
					if (input == "%CONTROL_L2")
					{
						return UIControllerButtonHelper.GetUnifiedControllerButtonTag(InputControlType.LeftTrigger, BraveInput.PlayerOneCurrentSymbology, null);
					}
					if (input == "%CONTROL_ALL_FACE_BUTTONS")
					{
						return "[sprite \"switch_single_all\"]";
					}
					if (input == "%ESCAPE_ROPE_SYMBOL")
					{
						return "[sprite \"escape_rope_text_icon_001\"]";
					}
					if (input == "%ARMOR_SYMBOL")
					{
						return "[sprite \"armor_money_icon_001\"]";
					}
					if (input == "%CHAMBER1_MASTERY_TOKEN_SYMBOL")
					{
						return "[sprite \"master_token_icon_001\"]";
					}
					if (input == "%CHAMBER2_MASTERY_TOKEN_SYMBOL")
					{
						return "[sprite \"master_token_icon_002\"]";
					}
					if (input == "%CHAMBER3_MASTERY_TOKEN_SYMBOL")
					{
						return "[sprite \"master_token_icon_003\"]";
					}
					if (input == "%CHAMBER4_MASTERY_TOKEN_SYMBOL")
					{
						return "[sprite \"master_token_icon_004\"]";
					}
					if (input == "%CHAMBER5_MASTERY_TOKEN_SYMBOL")
					{
						return "[sprite \"master_token_icon_005\"]";
					}
					if (input == "%HEART_SYMBOL")
					{
						return "[sprite \"heart_big_idle_001\"]";
					}
					if (input == "%JUNK_SYMBOL")
					{
						return "[sprite \"poopsack_001\"]";
					}
					if (input == "%BTCKTP_PRIMER")
					{
						return "[sprite \"forged_bullet_primer_001\"]";
					}
					if (input == "%BTCKTP_POWDER")
					{
						return "[sprite \"forged_bullet_powder_001\"]";
					}
					if (input == "%BTCKTP_SLUG")
					{
						return "[sprite \"forged_bullet_slug_001\"]";
					}
					if (input == "%BTCKTP_CASING")
					{
						return "[sprite \"forged_bullet_case_001\"]";
					}
					if (input == "%PLAYTIMEHOURS")
					{
						return string.Format("{0:0.0}", GameStatsManager.Instance.GetPlayerStatValue(TrackedStats.TIME_PLAYED) / 3600f);
					}
					return input;
				}
			}
		}
	}

	// Token: 0x06009063 RID: 36963 RVA: 0x003D17E0 File Offset: 0x003CF9E0
	private static bool CharIsEnglishAlphaNumeric(char c)
	{
		return char.IsLetterOrDigit(c) && c < '׀';
	}

	// Token: 0x06009064 RID: 36964 RVA: 0x003D17F8 File Offset: 0x003CF9F8
	public static string PostprocessString(string input)
	{
		if (StringTableManager.m_postprocessors == null)
		{
			StringTableManager.m_postprocessors = new Stack<StringBuilder>();
		}
		if (StringTableManager.m_tokenLists == null)
		{
			StringTableManager.m_tokenLists = new Stack<List<string>>();
		}
		StringBuilder stringBuilder = ((StringTableManager.m_postprocessors.Count <= 0) ? new StringBuilder() : StringTableManager.m_postprocessors.Pop());
		List<string> list = ((StringTableManager.m_tokenLists.Count <= 0) ? new List<string>() : StringTableManager.m_tokenLists.Pop());
		int num = 0;
		for (int i = 0; i < input.Length; i++)
		{
			char c = input[i];
			if (!StringTableManager.CharIsEnglishAlphaNumeric(c) && c != '_')
			{
				list.Add(input.Substring(num, i - num));
				num = i;
			}
		}
		list.Add(input.Substring(num, input.Length - num));
		for (int j = 0; j < list.Count; j++)
		{
			if (list[j] != null && list[j].Length != 0)
			{
				if (list[j][0] == '%')
				{
					bool flag = false;
					if (j < list.Count - 1 && list[j + 1].Length > 0 && list[j] == "%KEY_SYMBOL" && list[j + 1][0] == '?')
					{
						flag = true;
					}
					string text = StringTableManager.EvaluateReplacementToken(list[j]);
					stringBuilder.Append(text);
					if (flag)
					{
						stringBuilder.Append(' ');
					}
				}
				else
				{
					stringBuilder.Append(list[j]);
				}
			}
		}
		string text2 = stringBuilder.ToString();
		stringBuilder.Length = 0;
		list.Clear();
		StringTableManager.m_postprocessors.Push(stringBuilder);
		StringTableManager.m_tokenLists.Push(list);
		return text2;
	}

	// Token: 0x17001597 RID: 5527
	// (get) Token: 0x06009065 RID: 36965 RVA: 0x003D19EC File Offset: 0x003CFBEC
	public static Dictionary<string, StringTableManager.StringCollection> IntroTable
	{
		get
		{
			if (StringTableManager.m_introTable == null)
			{
				StringTableManager.m_introTable = StringTableManager.LoadIntroTable(StringTableManager.m_currentSubDirectory);
			}
			if (StringTableManager.m_backupIntroTable == null)
			{
				StringTableManager.m_backupIntroTable = StringTableManager.LoadIntroTable("english_items");
			}
			return StringTableManager.m_introTable;
		}
	}

	// Token: 0x17001598 RID: 5528
	// (get) Token: 0x06009066 RID: 36966 RVA: 0x003D1A28 File Offset: 0x003CFC28
	public static Dictionary<string, StringTableManager.StringCollection> CoreTable
	{
		get
		{
			if (StringTableManager.m_coreTable == null)
			{
				StringTableManager.m_coreTable = StringTableManager.LoadTables(StringTableManager.m_currentFile);
			}
			if (StringTableManager.m_backupCoreTable == null)
			{
				StringTableManager.m_backupCoreTable = StringTableManager.LoadTables("english");
			}
			return StringTableManager.m_coreTable;
		}
	}

	// Token: 0x17001599 RID: 5529
	// (get) Token: 0x06009067 RID: 36967 RVA: 0x003D1A64 File Offset: 0x003CFC64
	public static Dictionary<string, StringTableManager.StringCollection> ItemTable
	{
		get
		{
			if (StringTableManager.m_itemsTable == null)
			{
				StringTableManager.m_itemsTable = StringTableManager.LoadItemsTable(StringTableManager.m_currentSubDirectory);
			}
			if (StringTableManager.m_backupItemsTable == null)
			{
				StringTableManager.m_backupItemsTable = StringTableManager.LoadItemsTable("english_items");
			}
			return StringTableManager.m_itemsTable;
		}
	}

	// Token: 0x1700159A RID: 5530
	// (get) Token: 0x06009068 RID: 36968 RVA: 0x003D1AA0 File Offset: 0x003CFCA0
	public static Dictionary<string, StringTableManager.StringCollection> EnemyTable
	{
		get
		{
			if (StringTableManager.m_enemiesTable == null)
			{
				StringTableManager.m_enemiesTable = StringTableManager.LoadEnemiesTable(StringTableManager.m_currentSubDirectory);
			}
			if (StringTableManager.m_backupEnemiesTable == null)
			{
				StringTableManager.m_backupEnemiesTable = StringTableManager.LoadEnemiesTable("english_items");
			}
			return StringTableManager.m_enemiesTable;
		}
	}

	// Token: 0x04009861 RID: 39009
	private static string m_currentFile = "english";

	// Token: 0x04009862 RID: 39010
	private static string m_currentSubDirectory = "english_items";

	// Token: 0x04009863 RID: 39011
	private static Stack<List<string>> m_tokenLists;

	// Token: 0x04009864 RID: 39012
	private static Stack<StringBuilder> m_postprocessors;

	// Token: 0x04009865 RID: 39013
	private static Dictionary<string, StringTableManager.StringCollection> m_coreTable;

	// Token: 0x04009866 RID: 39014
	private static Dictionary<string, StringTableManager.StringCollection> m_itemsTable;

	// Token: 0x04009867 RID: 39015
	private static Dictionary<string, StringTableManager.StringCollection> m_enemiesTable;

	// Token: 0x04009868 RID: 39016
	private static Dictionary<string, StringTableManager.StringCollection> m_uiTable;

	// Token: 0x04009869 RID: 39017
	private static Dictionary<string, StringTableManager.StringCollection> m_introTable;

	// Token: 0x0400986A RID: 39018
	private static Dictionary<string, StringTableManager.StringCollection> m_synergyTable;

	// Token: 0x0400986B RID: 39019
	private static Dictionary<string, StringTableManager.StringCollection> m_backupCoreTable;

	// Token: 0x0400986C RID: 39020
	private static Dictionary<string, StringTableManager.StringCollection> m_backupItemsTable;

	// Token: 0x0400986D RID: 39021
	private static Dictionary<string, StringTableManager.StringCollection> m_backupEnemiesTable;

	// Token: 0x0400986E RID: 39022
	private static Dictionary<string, StringTableManager.StringCollection> m_backupUiTable;

	// Token: 0x0400986F RID: 39023
	private static Dictionary<string, StringTableManager.StringCollection> m_backupIntroTable;

	// Token: 0x04009870 RID: 39024
	private static Dictionary<string, StringTableManager.StringCollection> m_backupSynergyTable;

	// Token: 0x020017F2 RID: 6130
	public enum GungeonSupportedLanguages
	{
		// Token: 0x04009872 RID: 39026
		ENGLISH,
		// Token: 0x04009873 RID: 39027
		RUBEL_TEST,
		// Token: 0x04009874 RID: 39028
		FRENCH,
		// Token: 0x04009875 RID: 39029
		SPANISH,
		// Token: 0x04009876 RID: 39030
		ITALIAN,
		// Token: 0x04009877 RID: 39031
		GERMAN,
		// Token: 0x04009878 RID: 39032
		BRAZILIANPORTUGUESE,
		// Token: 0x04009879 RID: 39033
		JAPANESE,
		// Token: 0x0400987A RID: 39034
		KOREAN,
		// Token: 0x0400987B RID: 39035
		RUSSIAN,
		// Token: 0x0400987C RID: 39036
		POLISH,
		// Token: 0x0400987D RID: 39037
		CHINESE
	}

	// Token: 0x020017F3 RID: 6131
	public abstract class StringCollection
	{
		// Token: 0x0600906B RID: 36971
		public abstract int Count();

		// Token: 0x0600906C RID: 36972
		public abstract void AddString(string value, float weight);

		// Token: 0x0600906D RID: 36973
		public abstract string GetCombinedString();

		// Token: 0x0600906E RID: 36974
		public abstract string GetExactString(int index);

		// Token: 0x0600906F RID: 36975
		public abstract string GetWeightedString();

		// Token: 0x06009070 RID: 36976
		public abstract string GetWeightedStringSequential(ref int lastIndex, out bool isLast, bool repeatLast = false);
	}

	// Token: 0x020017F4 RID: 6132
	public class SimpleStringCollection : StringTableManager.StringCollection
	{
		// Token: 0x06009072 RID: 36978 RVA: 0x003D1B04 File Offset: 0x003CFD04
		public override int Count()
		{
			return 1;
		}

		// Token: 0x06009073 RID: 36979 RVA: 0x003D1B08 File Offset: 0x003CFD08
		public override void AddString(string value, float weight)
		{
			this.singleString = value;
		}

		// Token: 0x06009074 RID: 36980 RVA: 0x003D1B14 File Offset: 0x003CFD14
		public override string GetCombinedString()
		{
			return this.singleString;
		}

		// Token: 0x06009075 RID: 36981 RVA: 0x003D1B1C File Offset: 0x003CFD1C
		public override string GetExactString(int index)
		{
			return this.singleString;
		}

		// Token: 0x06009076 RID: 36982 RVA: 0x003D1B24 File Offset: 0x003CFD24
		public override string GetWeightedString()
		{
			return this.singleString;
		}

		// Token: 0x06009077 RID: 36983 RVA: 0x003D1B2C File Offset: 0x003CFD2C
		public override string GetWeightedStringSequential(ref int lastIndex, out bool isLast, bool repeatLast = false)
		{
			isLast = true;
			return this.singleString;
		}

		// Token: 0x0400987E RID: 39038
		private string singleString;
	}

	// Token: 0x020017F5 RID: 6133
	public class ComplexStringCollection : StringTableManager.StringCollection
	{
		// Token: 0x06009078 RID: 36984 RVA: 0x003D1B38 File Offset: 0x003CFD38
		public ComplexStringCollection()
		{
			this.strings = new List<string>();
			this.weights = new List<float>();
			this.maxWeight = 0f;
		}

		// Token: 0x06009079 RID: 36985 RVA: 0x003D1B64 File Offset: 0x003CFD64
		public override int Count()
		{
			return this.strings.Count;
		}

		// Token: 0x0600907A RID: 36986 RVA: 0x003D1B74 File Offset: 0x003CFD74
		public override void AddString(string value, float weight)
		{
			this.strings.Add(value);
			this.weights.Add(weight);
			this.maxWeight += weight;
		}

		// Token: 0x0600907B RID: 36987 RVA: 0x003D1B9C File Offset: 0x003CFD9C
		public override string GetCombinedString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < this.strings.Count; i++)
			{
				stringBuilder.AppendLine(this.strings[i]);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0600907C RID: 36988 RVA: 0x003D1BE4 File Offset: 0x003CFDE4
		public override string GetExactString(int index)
		{
			if (this.strings.Count == 0)
			{
				return string.Empty;
			}
			return this.strings[index % this.strings.Count];
		}

		// Token: 0x0600907D RID: 36989 RVA: 0x003D1C14 File Offset: 0x003CFE14
		public override string GetWeightedString()
		{
			float num = UnityEngine.Random.value * this.maxWeight;
			float num2 = 0f;
			for (int i = 0; i < this.strings.Count; i++)
			{
				num2 += this.weights[i];
				if (num2 >= num)
				{
					return this.strings[i];
				}
			}
			if (this.strings.Count == 0)
			{
				return null;
			}
			return this.strings[0];
		}

		// Token: 0x0600907E RID: 36990 RVA: 0x003D1C94 File Offset: 0x003CFE94
		public override string GetWeightedStringSequential(ref int lastIndex, out bool isLast, bool repeatLast = false)
		{
			int num = lastIndex + 1;
			isLast = num == this.strings.Count - 1;
			if (num >= this.strings.Count)
			{
				if (repeatLast)
				{
					num = lastIndex;
					isLast = true;
				}
				else
				{
					num = 0;
				}
			}
			if (num < 0)
			{
				num = 0;
			}
			if (num >= this.strings.Count)
			{
				num = this.strings.Count - 1;
			}
			if (this.strings.Count == 0)
			{
				return string.Empty;
			}
			lastIndex = num;
			return this.strings[num];
		}

		// Token: 0x0400987F RID: 39039
		private List<string> strings;

		// Token: 0x04009880 RID: 39040
		private List<float> weights;

		// Token: 0x04009881 RID: 39041
		private float maxWeight;
	}
}
