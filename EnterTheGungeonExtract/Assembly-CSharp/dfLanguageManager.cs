using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

// Token: 0x020003DE RID: 990
public class dfLanguageManager : MonoBehaviour
{
	// Token: 0x17000460 RID: 1120
	// (get) Token: 0x060013F8 RID: 5112 RVA: 0x0005C7C0 File Offset: 0x0005A9C0
	public dfLanguageCode CurrentLanguage
	{
		get
		{
			return this.currentLanguage;
		}
	}

	// Token: 0x17000461 RID: 1121
	// (get) Token: 0x060013F9 RID: 5113 RVA: 0x0005C7C8 File Offset: 0x0005A9C8
	// (set) Token: 0x060013FA RID: 5114 RVA: 0x0005C7D0 File Offset: 0x0005A9D0
	public TextAsset DataFile
	{
		get
		{
			return this.dataFile;
		}
		set
		{
			if (value != this.dataFile)
			{
				this.dataFile = value;
				this.LoadLanguage(this.currentLanguage, false);
			}
			if (this.backupDataFile == null)
			{
				this.backupDataFile = StringTableManager.GetBackupUIDataFile();
			}
		}
	}

	// Token: 0x060013FB RID: 5115 RVA: 0x0005C820 File Offset: 0x0005AA20
	public static void ChangeGungeonLanguage()
	{
		dfLanguageCode languageCodeFromGungeon = dfLanguageManager.GetLanguageCodeFromGungeon();
		dfLanguageManager[] array = UnityEngine.Object.FindObjectsOfType<dfLanguageManager>();
		for (int i = 0; i < array.Length; i++)
		{
			array[i].LoadLanguage(languageCodeFromGungeon, true);
		}
	}

	// Token: 0x060013FC RID: 5116 RVA: 0x0005C858 File Offset: 0x0005AA58
	private static dfLanguageCode GetLanguageCodeFromGungeon()
	{
		switch (StringTableManager.CurrentLanguage)
		{
		case StringTableManager.GungeonSupportedLanguages.ENGLISH:
			return dfLanguageCode.EN;
		case StringTableManager.GungeonSupportedLanguages.RUBEL_TEST:
			return dfLanguageCode.QU;
		case StringTableManager.GungeonSupportedLanguages.FRENCH:
			return dfLanguageCode.FR;
		case StringTableManager.GungeonSupportedLanguages.SPANISH:
			return dfLanguageCode.ES;
		case StringTableManager.GungeonSupportedLanguages.ITALIAN:
			return dfLanguageCode.IT;
		case StringTableManager.GungeonSupportedLanguages.GERMAN:
			return dfLanguageCode.DE;
		case StringTableManager.GungeonSupportedLanguages.BRAZILIANPORTUGUESE:
			return dfLanguageCode.PT;
		case StringTableManager.GungeonSupportedLanguages.JAPANESE:
			return dfLanguageCode.JA;
		case StringTableManager.GungeonSupportedLanguages.KOREAN:
			return dfLanguageCode.KO;
		case StringTableManager.GungeonSupportedLanguages.RUSSIAN:
			return dfLanguageCode.RU;
		case StringTableManager.GungeonSupportedLanguages.POLISH:
			return dfLanguageCode.PL;
		case StringTableManager.GungeonSupportedLanguages.CHINESE:
			return dfLanguageCode.ZH;
		default:
			return dfLanguageCode.EN;
		}
	}

	// Token: 0x060013FD RID: 5117 RVA: 0x0005C8D0 File Offset: 0x0005AAD0
	public void Start()
	{
		this.currentLanguage = dfLanguageManager.GetLanguageCodeFromGungeon();
		dfLanguageCode dfLanguageCode = this.currentLanguage;
		if (this.currentLanguage == dfLanguageCode.None)
		{
			dfLanguageCode = this.SystemLanguageToLanguageCode(Application.systemLanguage);
		}
		this.LoadLanguage(dfLanguageCode, true);
	}

	// Token: 0x060013FE RID: 5118 RVA: 0x0005C910 File Offset: 0x0005AB10
	private void BraveChangeDataFile(dfLanguageCode language)
	{
		this.dataFile = StringTableManager.GetUIDataFile();
		if (this.backupDataFile == null)
		{
			this.backupDataFile = StringTableManager.GetBackupUIDataFile();
		}
	}

	// Token: 0x060013FF RID: 5119 RVA: 0x0005C93C File Offset: 0x0005AB3C
	public void LoadLanguage(dfLanguageCode language, bool forceReload = false)
	{
		this.currentLanguage = language;
		this.strings.Clear();
		this.BraveChangeDataFile(language);
		if (this.dataFile != null)
		{
			this.parseDataFile();
		}
		if (forceReload)
		{
			dfControl[] componentsInChildren = base.GetComponentsInChildren<dfControl>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].Localize();
			}
			for (int j = 0; j < componentsInChildren.Length; j++)
			{
				componentsInChildren[j].PerformLayout();
				if (componentsInChildren[j].LanguageChanged != null)
				{
					componentsInChildren[j].LanguageChanged(componentsInChildren[j]);
				}
			}
		}
	}

	// Token: 0x06001400 RID: 5120 RVA: 0x0005C9DC File Offset: 0x0005ABDC
	public string GetValue(string key)
	{
		if (this.strings.Count == 0)
		{
			dfLanguageCode dfLanguageCode = this.currentLanguage;
			if (this.currentLanguage == dfLanguageCode.None)
			{
				dfLanguageCode = this.SystemLanguageToLanguageCode(Application.systemLanguage);
			}
			this.LoadLanguage(dfLanguageCode, false);
		}
		string empty = string.Empty;
		if (this.strings.TryGetValue(key, out empty))
		{
			return empty;
		}
		return key;
	}

	// Token: 0x06001401 RID: 5121 RVA: 0x0005CA3C File Offset: 0x0005AC3C
	private void parseDataFile()
	{
		string text = this.dataFile.text.Replace("\r\n", "\n").Trim();
		List<string> list = new List<string>();
		int i = this.parseLine(text, list, 0);
		int num = list.IndexOf(this.currentLanguage.ToString());
		if (num < 0)
		{
			return;
		}
		List<string> list2 = new List<string>();
		while (i < text.Length)
		{
			i = this.parseLine(text, list2, i);
			if (list2.Count != 0)
			{
				string text2 = list2[0];
				string text3 = ((num >= list2.Count) ? string.Empty : list2[num]);
				this.strings[text2] = text3;
			}
		}
		string text4 = this.backupDataFile.text.Replace("\r\n", "\n").Trim();
		List<string> list3 = new List<string>();
		int j = this.parseLine(text4, list3, 0);
		int num2 = 1;
		List<string> list4 = new List<string>();
		while (j < text4.Length)
		{
			j = this.parseLine(text4, list4, j);
			if (list4.Count != 0)
			{
				string text5 = list4[0];
				string text6 = ((num2 >= list4.Count) ? string.Empty : list4[num2]);
				if (!this.strings.ContainsKey(text5))
				{
					this.strings[text5] = text6;
				}
			}
		}
	}

	// Token: 0x06001402 RID: 5122 RVA: 0x0005CBC8 File Offset: 0x0005ADC8
	private int parseLine(string data, List<string> values, int index)
	{
		values.Clear();
		bool flag = false;
		StringBuilder stringBuilder = new StringBuilder(256);
		while (index < data.Length)
		{
			char c = data[index];
			if (c == '"')
			{
				if (!flag)
				{
					flag = true;
				}
				else if (index + 1 < data.Length && data[index + 1] == c)
				{
					index++;
					stringBuilder.Append(c);
				}
				else
				{
					flag = false;
				}
			}
			else if (c == ',')
			{
				if (flag)
				{
					stringBuilder.Append(c);
				}
				else
				{
					values.Add(stringBuilder.ToString());
					stringBuilder.Length = 0;
				}
			}
			else if (c == '\n')
			{
				if (!flag)
				{
					index++;
					break;
				}
				stringBuilder.Append(c);
			}
			else
			{
				stringBuilder.Append(c);
			}
			index++;
		}
		if (stringBuilder.Length > 0)
		{
			values.Add(stringBuilder.ToString());
		}
		return index;
	}

	// Token: 0x06001403 RID: 5123 RVA: 0x0005CCD0 File Offset: 0x0005AED0
	private dfLanguageCode SystemLanguageToLanguageCode(SystemLanguage language)
	{
		switch (language)
		{
		case SystemLanguage.Afrikaans:
			return dfLanguageCode.AF;
		case SystemLanguage.Arabic:
			return dfLanguageCode.AR;
		case SystemLanguage.Basque:
			return dfLanguageCode.EU;
		case SystemLanguage.Belarusian:
			return dfLanguageCode.BE;
		case SystemLanguage.Bulgarian:
			return dfLanguageCode.BG;
		case SystemLanguage.Catalan:
			return dfLanguageCode.CA;
		case SystemLanguage.Chinese:
			return dfLanguageCode.ZH;
		case SystemLanguage.Czech:
			return dfLanguageCode.CS;
		case SystemLanguage.Danish:
			return dfLanguageCode.DA;
		case SystemLanguage.Dutch:
			return dfLanguageCode.NL;
		case SystemLanguage.English:
			return dfLanguageCode.EN;
		case SystemLanguage.Estonian:
			return dfLanguageCode.ES;
		case SystemLanguage.Faroese:
			return dfLanguageCode.FO;
		case SystemLanguage.Finnish:
			return dfLanguageCode.FI;
		case SystemLanguage.French:
			return dfLanguageCode.FR;
		case SystemLanguage.German:
			return dfLanguageCode.DE;
		case SystemLanguage.Greek:
			return dfLanguageCode.EL;
		case SystemLanguage.Hebrew:
			return dfLanguageCode.HE;
		case SystemLanguage.Hungarian:
			return dfLanguageCode.HU;
		case SystemLanguage.Icelandic:
			return dfLanguageCode.IS;
		case SystemLanguage.Indonesian:
			return dfLanguageCode.ID;
		case SystemLanguage.Italian:
			return dfLanguageCode.IT;
		case SystemLanguage.Japanese:
			return dfLanguageCode.JA;
		case SystemLanguage.Korean:
			return dfLanguageCode.KO;
		case SystemLanguage.Latvian:
			return dfLanguageCode.LV;
		case SystemLanguage.Lithuanian:
			return dfLanguageCode.LT;
		case SystemLanguage.Norwegian:
			return dfLanguageCode.NO;
		case SystemLanguage.Polish:
			return dfLanguageCode.PL;
		case SystemLanguage.Portuguese:
			return dfLanguageCode.PT;
		case SystemLanguage.Romanian:
			return dfLanguageCode.RO;
		case SystemLanguage.Russian:
			return dfLanguageCode.RU;
		case SystemLanguage.SerboCroatian:
			return dfLanguageCode.SH;
		case SystemLanguage.Slovak:
			return dfLanguageCode.SK;
		case SystemLanguage.Slovenian:
			return dfLanguageCode.SL;
		case SystemLanguage.Spanish:
			return dfLanguageCode.ES;
		case SystemLanguage.Swedish:
			return dfLanguageCode.SV;
		case SystemLanguage.Thai:
			return dfLanguageCode.TH;
		case SystemLanguage.Turkish:
			return dfLanguageCode.TR;
		case SystemLanguage.Ukrainian:
			return dfLanguageCode.UK;
		case SystemLanguage.Vietnamese:
			return dfLanguageCode.VI;
		case SystemLanguage.Unknown:
			return dfLanguageCode.EN;
		}
		throw new ArgumentException("Unknown system language: " + language);
	}

	// Token: 0x040011B4 RID: 4532
	[SerializeField]
	private dfLanguageCode currentLanguage;

	// Token: 0x040011B5 RID: 4533
	[SerializeField]
	private TextAsset dataFile;

	// Token: 0x040011B6 RID: 4534
	[NonSerialized]
	private TextAsset backupDataFile;

	// Token: 0x040011B7 RID: 4535
	private Dictionary<string, string> strings = new Dictionary<string, string>();
}
