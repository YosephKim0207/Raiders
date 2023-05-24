using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using <PrivateImplementationDetails>{DE5600AD-6212-4D84-9A32-9D951E3289D1};
using Brave;
using FullInspector;
using UnityEngine;

// Token: 0x02001830 RID: 6192
public static class SaveManager
{
	// Token: 0x06009296 RID: 37526 RVA: 0x003DE840 File Offset: 0x003DCA40
	public static void Init()
	{
		if (SaveManager.s_hasBeenInitialized)
		{
			return;
		}
		if (string.IsNullOrEmpty(SaveManager.SavePath))
		{
			Debug.LogError("Application.persistentDataPath FAILED! " + SaveManager.SavePath);
			SaveManager.SavePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "../LocalLow/Dodge Roll/Enter the Gungeon");
		}
		if (!Directory.Exists(SaveManager.SavePath))
		{
			try
			{
				Debug.LogWarning("Manually create default save directory!");
				Directory.CreateDirectory(SaveManager.SavePath);
			}
			catch (Exception ex)
			{
				Debug.LogError("Failed to create default save directory: " + ex.Message);
			}
		}
		int num = Brave.PlayerPrefs.GetInt("saveslot", 0);
		Brave.PlayerPrefs.SetInt("saveslot", num);
		if (num < 0 || num > 3)
		{
			num = 0;
		}
		SaveManager.CurrentSaveSlot = (SaveManager.SaveSlot)num;
		for (int i = 0; i < 3; i++)
		{
			SaveManager.SaveSlot saveSlot = (SaveManager.SaveSlot)i;
			SaveManager.SafeMove(Path.Combine(SaveManager.OldSavePath, string.Format(SaveManager.GameSave.legacyFilePattern, saveSlot)), Path.Combine(SaveManager.OldSavePath, string.Format(SaveManager.GameSave.filePattern, saveSlot)), false);
			SaveManager.SafeMove(Path.Combine(SaveManager.OldSavePath, string.Format(SaveManager.OptionsSave.legacyFilePattern, saveSlot)), Path.Combine(SaveManager.OldSavePath, string.Format(SaveManager.OptionsSave.filePattern, saveSlot)), false);
			SaveManager.SafeMove(Path.Combine(SaveManager.OldSavePath, string.Format(SaveManager.GameSave.filePattern, saveSlot)), Path.Combine(SaveManager.SavePath, string.Format(SaveManager.GameSave.filePattern, saveSlot)), false);
			SaveManager.SafeMove(Path.Combine(SaveManager.OldSavePath, string.Format(SaveManager.OptionsSave.filePattern, saveSlot)), Path.Combine(SaveManager.SavePath, string.Format(SaveManager.OptionsSave.filePattern, saveSlot)), false);
			SaveManager.SafeMove(SaveManager.PathCombine(SaveManager.SavePath, "01", string.Format(SaveManager.GameSave.filePattern, saveSlot)), Path.Combine(SaveManager.SavePath, string.Format(SaveManager.GameSave.filePattern, saveSlot)), true);
			SaveManager.SafeMove(SaveManager.PathCombine(SaveManager.SavePath, "01", string.Format(SaveManager.OptionsSave.filePattern, saveSlot)), Path.Combine(SaveManager.SavePath, string.Format(SaveManager.OptionsSave.filePattern, saveSlot)), true);
		}
		SaveManager.s_hasBeenInitialized = true;
	}

	// Token: 0x06009297 RID: 37527 RVA: 0x003DEAD0 File Offset: 0x003DCCD0
	private static string PathCombine(string a, string b, string c)
	{
		return Path.Combine(Path.Combine(a, b), c);
	}

	// Token: 0x06009298 RID: 37528 RVA: 0x003DEAE0 File Offset: 0x003DCCE0
	public static void ChangeSlot(SaveManager.SaveSlot newSaveSlot)
	{
		if (!SaveManager.s_hasBeenInitialized)
		{
			Debug.LogErrorFormat("Tried to change save slots before before SaveManager was initialized! {0}", new object[] { newSaveSlot });
		}
		SaveManager.CurrentSaveSlot = newSaveSlot;
		Brave.PlayerPrefs.SetInt("saveslot", (int)SaveManager.CurrentSaveSlot);
	}

	// Token: 0x06009299 RID: 37529 RVA: 0x003DEB1C File Offset: 0x003DCD1C
	public static void DeleteCurrentSlotMidGameSave(SaveManager.SaveSlot? overrideSaveSlot = null)
	{
		Debug.LogError("DELETING CURRENT MID GAME SAVE");
		if (GameStatsManager.HasInstance)
		{
			GameStatsManager.Instance.midGameSaveGuid = null;
		}
		string text = string.Format(SaveManager.MidGameSave.filePattern, (overrideSaveSlot == null) ? SaveManager.CurrentSaveSlot : overrideSaveSlot.Value);
		string text2 = Path.Combine(SaveManager.SavePath, text);
		if (File.Exists(text2))
		{
			File.Delete(text2);
		}
	}

	// Token: 0x0600929A RID: 37530 RVA: 0x003DEB98 File Offset: 0x003DCD98
	public static bool Save<T>(T obj, SaveManager.SaveType saveType, int playTimeMin, uint versionNumber = 0U, SaveManager.SaveSlot? overrideSaveSlot = null)
	{
		bool encrypted = saveType.encrypted;
		if (!SaveManager.s_hasBeenInitialized)
		{
			Debug.LogErrorFormat("Tried to save data before SaveManager was initialized! {0} {1}", new object[]
			{
				obj.GetType(),
				saveType.filePattern
			});
			return false;
		}
		string text = string.Format(saveType.filePattern, (overrideSaveSlot == null) ? SaveManager.CurrentSaveSlot : overrideSaveSlot.Value);
		string text2 = Path.Combine(SaveManager.SavePath, text);
		string text3;
		try
		{
			bool prettyPrintSerializedJson = fiSettings.PrettyPrintSerializedJson;
			fiSettings.PrettyPrintSerializedJson = !encrypted;
			text3 = SerializationHelpers.SerializeToContent<T, FullSerializerSerializer>(obj);
			fiSettings.PrettyPrintSerializedJson = prettyPrintSerializedJson;
		}
		catch (Exception ex)
		{
			Debug.LogError("Failed to serialize save data: " + ex.Message);
			return false;
		}
		if (encrypted)
		{
			text3 = SaveManager.Encrypt(text3);
		}
		text3 = string.Format("version: {0}\n{1}", versionNumber, text3);
		if (!Directory.Exists(SaveManager.SavePath))
		{
			Directory.CreateDirectory(SaveManager.SavePath);
		}
		bool flag = false;
		if (File.Exists(text2))
		{
			try
			{
				File.Copy(text2, text2 + ".temp", true);
				flag = true;
			}
			catch (Exception ex2)
			{
				Debug.LogError("Failed to create a temporary copy of current save: " + ex2.Message);
				return false;
			}
		}
		try
		{
			SaveManager.WriteAllText(text2, text3);
		}
		catch (Exception ex3)
		{
			Debug.LogError("Failed to write new save data: " + ex3.Message);
			try
			{
				File.Delete(text2);
				File.Move(text2 + ".temp", text2);
			}
			catch (Exception ex4)
			{
				Debug.LogError("Failed to restore temp save data: " + ex4.Message);
			}
			return false;
		}
		if (flag)
		{
			try
			{
				if (File.Exists(text2 + ".temp"))
				{
					File.Delete(text2 + ".temp");
				}
			}
			catch (Exception ex5)
			{
				Debug.LogError("Failed to replace temp save file: " + ex5.Message);
			}
		}
		if (saveType.backupCount > 0)
		{
			int latestBackupPlaytimeMinutes = SaveManager.GetLatestBackupPlaytimeMinutes(saveType, overrideSaveSlot);
			if (playTimeMin >= latestBackupPlaytimeMinutes + saveType.backupMinTimeMin)
			{
				string text4 = string.Format("{0}h{1}m", playTimeMin / 60, playTimeMin % 60);
				string text5 = string.Format(saveType.backupPattern, (overrideSaveSlot == null) ? SaveManager.CurrentSaveSlot : overrideSaveSlot.Value, text4);
				string text6 = Path.Combine(SaveManager.SavePath, text5);
				try
				{
					SaveManager.WriteAllText(text6, text3);
				}
				catch (Exception ex6)
				{
					Debug.LogError("Failed to create new save backup: " + ex6.Message);
				}
				SaveManager.DeleteOldBackups(saveType, overrideSaveSlot);
			}
		}
		return true;
	}

	// Token: 0x0600929B RID: 37531 RVA: 0x003DEE98 File Offset: 0x003DD098
	public static bool Load<T>(SaveManager.SaveType saveType, out T obj, bool allowDecrypted, uint expectedVersion = 0U, Func<string, uint, string> versionUpdater = null, SaveManager.SaveSlot? overrideSaveSlot = null)
	{
		obj = default(T);
		if (!SaveManager.s_hasBeenInitialized)
		{
			Debug.LogErrorFormat("Tried to load data before SaveManager was initialized! {0} {1}", new object[]
			{
				saveType.filePattern,
				typeof(T)
			});
			return false;
		}
		string text = string.Format(saveType.filePattern, (overrideSaveSlot == null) ? SaveManager.CurrentSaveSlot : overrideSaveSlot.Value);
		string text2 = Path.Combine(SaveManager.SavePath, text);
		if (!File.Exists(text2))
		{
			Debug.LogWarning("Save data doesn't exist: " + text);
			return false;
		}
		string text3;
		try
		{
			text3 = SaveManager.ReadAllText(text2);
		}
		catch (Exception ex)
		{
			Debug.LogError("Failed to read save data: " + ex.Message);
			return false;
		}
		uint num = 0U;
		if (text3.StartsWith("version: "))
		{
			StringReader stringReader = new StringReader(text3);
			string text4 = stringReader.ReadLine();
			uint num2;
			if (!uint.TryParse(text4.Substring(9), out num2))
			{
				Debug.LogErrorFormat("Failed to read save version number (expected [{0}], got [{1}]", new object[]
				{
					expectedVersion,
					text4.Substring(9)
				});
				return false;
			}
			num = num2;
			text3 = stringReader.ReadToEnd();
		}
		if (SaveManager.IsDataEncrypted(text3))
		{
			text3 = SaveManager.Decrypt(text3);
		}
		else if (!allowDecrypted)
		{
			Debug.LogError("Save file corrupted!  Copying to a new file");
			text3 = string.Format("version: {0}\n{1}", num, text3);
			try
			{
				SaveManager.WriteAllText(text2 + ".corrupt", text3);
			}
			catch (Exception ex2)
			{
				Debug.LogError("Failed to save off the corrupted file: " + ex2.Message);
			}
			return false;
		}
		if (num < expectedVersion && versionUpdater != null)
		{
			text3 = versionUpdater(text3, num);
		}
		obj = SerializationHelpers.DeserializeFromContent<T, FullSerializerSerializer>(text3);
		if (obj == null)
		{
			Debug.LogError("Save file corrupted!  Copying to a new file");
			try
			{
				text3 = SaveManager.ReadAllText(text2);
			}
			catch (Exception ex3)
			{
				Debug.LogError("Failed to read corrupted save data: " + ex3.Message);
			}
			try
			{
				SaveManager.WriteAllText(text2 + ".corrupt", text3);
			}
			catch (Exception ex4)
			{
				Debug.LogError("Failed to save off the corrupted file: " + ex4.Message);
			}
			return false;
		}
		return true;
	}

	// Token: 0x0600929C RID: 37532 RVA: 0x003DF120 File Offset: 0x003DD320
	public static void WriteAllText(string path, string contents)
	{
		byte[] bytes = Encoding.UTF8.GetBytes(contents);
		string text = "null";
		try
		{
			text = Path.GetTempFileName();
			if (Directory.Exists(Path.GetDirectoryName(text)))
			{
				using (FileStream fileStream = File.Create(text, 4096, FileOptions.WriteThrough))
				{
					fileStream.Write(bytes, 0, bytes.Length);
				}
				File.Delete(path);
				File.Move(text, path);
				return;
			}
		}
		catch (Exception ex)
		{
			Debug.LogErrorFormat("Failed to write to temp file {0}: {1}", new object[] { text, ex });
		}
		text = Path.Combine(Path.GetDirectoryName(path), Path.GetFileName(text));
		using (FileStream fileStream2 = File.Create(text, 4096, FileOptions.WriteThrough))
		{
			fileStream2.Write(bytes, 0, bytes.Length);
		}
		File.Delete(path);
		File.Move(text, path);
	}

	// Token: 0x0600929D RID: 37533 RVA: 0x003DF234 File Offset: 0x003DD434
	public static string ReadAllText(string path)
	{
		return File.ReadAllText(path, Encoding.UTF8);
	}

	// Token: 0x0600929E RID: 37534 RVA: 0x003DF244 File Offset: 0x003DD444
	private static int GetLatestBackupPlaytimeMinutes(SaveManager.SaveType saveType, SaveManager.SaveSlot? overrideSaveSlot = null)
	{
		string text = string.Format(saveType.backupPattern, (overrideSaveSlot == null) ? SaveManager.CurrentSaveSlot : overrideSaveSlot.Value, string.Empty);
		string text2 = text + "(?<hour>\\d+)h(?<min>\\d+)m";
		string[] files = Directory.GetFiles(SaveManager.SavePath);
		int num = 0;
		for (int i = 0; i < files.Length; i++)
		{
			Match match = Regex.Match(files[i], text2, RegexOptions.Multiline);
			if (match.Success)
			{
				int num2 = Convert.ToInt32(match.Groups["hour"].Captures[0].Value) * 60;
				num2 += Convert.ToInt32(match.Groups["min"].Captures[0].Value);
				if (num2 > num)
				{
					num = num2;
				}
			}
		}
		return num;
	}

	// Token: 0x0600929F RID: 37535 RVA: 0x003DF330 File Offset: 0x003DD530
	private static void SafeMove(string oldPath, string newPath, bool allowOverwritting = false)
	{
		if (File.Exists(oldPath) && (allowOverwritting || !File.Exists(newPath)))
		{
			string text = SaveManager.ReadAllText(oldPath);
			try
			{
				SaveManager.WriteAllText(newPath, text);
			}
			catch (Exception ex)
			{
				Debug.LogErrorFormat("Failed to move {0} to {1}: {2}", new object[] { oldPath, newPath, ex });
				return;
			}
			try
			{
				File.Delete(oldPath);
			}
			catch (Exception ex2)
			{
				Debug.LogErrorFormat("Failed to delete old file {0}: {1}", new object[] { oldPath, newPath, ex2 });
				return;
			}
			if (File.Exists(oldPath + ".bak"))
			{
				File.Delete(oldPath + ".bak");
			}
		}
	}

	// Token: 0x060092A0 RID: 37536 RVA: 0x003DF3FC File Offset: 0x003DD5FC
	public static void DeleteAllBackups(SaveManager.SaveType saveType, SaveManager.SaveSlot? overrideSaveSlot = null)
	{
		string text = string.Format(saveType.backupPattern, (overrideSaveSlot == null) ? SaveManager.CurrentSaveSlot : overrideSaveSlot.Value, string.Empty);
		string text2 = text + "(?<hour>\\d+)h(?<min>\\d+)m";
		string[] files = Directory.GetFiles(SaveManager.SavePath);
		for (int i = 0; i < files.Length; i++)
		{
			Match match = Regex.Match(files[i], text2, RegexOptions.Multiline);
			if (match.Success)
			{
				try
				{
					File.Delete(files[i]);
				}
				catch (Exception ex)
				{
					Debug.LogError("Failed to remove backup file: " + ex.Message);
					break;
				}
			}
		}
	}

	// Token: 0x060092A1 RID: 37537 RVA: 0x003DF4BC File Offset: 0x003DD6BC
	private static void SafeMoveBackups(SaveManager.SaveType saveType, string oldPath, string newPath, SaveManager.SaveSlot? overrideSaveSlot = null)
	{
		string text = string.Format(saveType.backupPattern, (overrideSaveSlot == null) ? SaveManager.CurrentSaveSlot : overrideSaveSlot.Value, string.Empty);
		string text2 = text + "(?<hour>\\d+)h(?<min>\\d+)m";
		string[] files = Directory.GetFiles(oldPath);
		for (int i = 0; i < files.Length; i++)
		{
			Match match = Regex.Match(files[i], text2, RegexOptions.Multiline);
			if (match.Success)
			{
				SaveManager.SafeMove(files[i], Path.Combine(newPath, Path.GetFileName(files[i])), false);
			}
		}
	}

	// Token: 0x060092A2 RID: 37538 RVA: 0x003DF554 File Offset: 0x003DD754
	private static void DeleteOldBackups(SaveManager.SaveType saveType, SaveManager.SaveSlot? overrideSaveSlot = null)
	{
		string text = string.Format(saveType.backupPattern, (overrideSaveSlot == null) ? SaveManager.CurrentSaveSlot : overrideSaveSlot.Value, string.Empty);
		string text2 = text + "(?<hour>\\d+)h(?<min>\\d+)m";
		List<Tuple<string, int>> list = new List<Tuple<string, int>>();
		string[] files = Directory.GetFiles(SaveManager.SavePath);
		for (int i = 0; i < files.Length; i++)
		{
			Match match = Regex.Match(files[i], text2, RegexOptions.Multiline);
			if (match.Success)
			{
				int num = Convert.ToInt32(match.Groups["hour"].Captures[0].Value) * 60;
				num += Convert.ToInt32(match.Groups["min"].Captures[0].Value);
				list.Add(Tuple.Create<string, int>(files[i], num));
			}
		}
		list.Sort((Tuple<string, int> a, Tuple<string, int> b) => b.Second - a.Second);
		while (list.Count > saveType.backupCount && list.Count > 0)
		{
			try
			{
				File.Delete(list[list.Count - 1].First);
			}
			catch (Exception ex)
			{
				Debug.LogError("Failed to remove backup file: " + ex.Message);
				break;
			}
			list.RemoveAt(list.Count - 1);
		}
	}

	// Token: 0x060092A3 RID: 37539 RVA: 0x003DF6E8 File Offset: 0x003DD8E8
	private static string Encrypt(string plaintext)
	{
		SaveManager.FixSecret();
		StringBuilder stringBuilder = new StringBuilder(plaintext.Length);
		for (int i = 0; i < plaintext.Length; i++)
		{
			stringBuilder.Append(plaintext[i] ^ SaveManager.secret[i % SaveManager.secret.Length]);
		}
		return stringBuilder.ToString();
	}

	// Token: 0x060092A4 RID: 37540 RVA: 0x003DF74C File Offset: 0x003DD94C
	private static string Decrypt(string cypertext)
	{
		SaveManager.FixSecret();
		return SaveManager.Encrypt(cypertext);
	}

	// Token: 0x060092A5 RID: 37541 RVA: 0x003DF75C File Offset: 0x003DD95C
	private static bool IsDataEncrypted(string data)
	{
		SaveManager.FixSecret();
		return !data.StartsWith('{'.ToString()) && data.StartsWith(('{' ^ SaveManager.secret[0]).ToString());
	}

	// Token: 0x060092A6 RID: 37542 RVA: 0x003DF7B8 File Offset: 0x003DD9B8
	private static void FixSecret()
	{
		if (SaveManager.secret.StartsWith("å") && SaveManager.secret.EndsWith("å"))
		{
			SaveManager.secret = SaveManager.secret.Substring(1, SaveManager.secret.Length - 2);
		}
	}

	// Token: 0x04009A1B RID: 39451
	public static SaveManager.SaveType GameSave = new SaveManager.SaveType
	{
		filePattern = "Slot{0}.save",
		encrypted = true,
		backupCount = 3,
		backupPattern = "Slot{0}.backup.{1}",
		backupMinTimeMin = 45,
		legacyFilePattern = "gameStatsSlot{0}.txt"
	};

	// Token: 0x04009A1C RID: 39452
	public static SaveManager.SaveType OptionsSave = new SaveManager.SaveType
	{
		filePattern = "Slot{0}.options",
		legacyFilePattern = "optionsSlot{0}.txt"
	};

	// Token: 0x04009A1D RID: 39453
	public static SaveManager.SaveType MidGameSave = new SaveManager.SaveType
	{
		filePattern = "Active{0}.game",
		legacyFilePattern = "activeSlot{0}.txt",
		encrypted = true,
		backupCount = 0,
		backupPattern = "Active{0}.backup.{1}",
		backupMinTimeMin = 60
	};

	// Token: 0x04009A1E RID: 39454
	public static SaveManager.SaveSlot CurrentSaveSlot;

	// Token: 0x04009A1F RID: 39455
	public static SaveManager.SaveSlot? TargetSaveSlot;

	// Token: 0x04009A20 RID: 39456
	public static bool ResetSaveSlot;

	// Token: 0x04009A21 RID: 39457
	public static bool PreventMidgameSaveDeletionOnExit;

	// Token: 0x04009A22 RID: 39458
	private static bool s_hasBeenInitialized;

	// Token: 0x04009A23 RID: 39459
	public static string OldSavePath = Path.Combine(Application.dataPath, "saves");

	// Token: 0x04009A24 RID: 39460
	public static string SavePath = Application.persistentDataPath;

	// Token: 0x04009A25 RID: 39461
	private static string secret = <PrivateImplementationDetails>{DE5600AD-6212-4D84-9A32-9D951E3289D1}.Decrypt.DecryptLiteral(new byte[]
	{
		107, 164, 67, 89, 49, 25, 207, 88, 51, 60,
		248, 156, 50, 78, 62, 211, 54, 174, 103, 13,
		39, 68, 125, 41, 212, 32, 206, 226, 34, 63,
		197, 19, 19, 117, 113, 209, 103, 3, 1, 163,
		61, 192, 126, 0, 244, 203, 3, 4, 11, 108,
		159, 196, 108, 214, 227, 208, 152, 145, 17, 137,
		89, 180, 195, 87, 96, 118, 244, 44, 199, 223,
		239, 184, 22, 82, 128, 135, 64, 240, 94, 185,
		88, 205, 243, 96, 62, 87, 155, 104, 144, 192,
		34, 70, 1, 239, 161, 188, 14, 153, 124, 2,
		246, 184, 50, 132, 244, 9, 206, 79, 200, 158,
		157, 211, 245, 131, 63, 188, 198, 235, 132, 123,
		7, 13, 79, 198, 171, 90, 107, 236, 70, 239,
		119, 197, 158, 76, 83, 10, 84, 218, 232, 25,
		170, 217, 88, 66, 198, 250, 184, 192, 176, 105,
		243, 82, 25, 247, 177, 63, 181, 102, 253, 247,
		214, 105, 219, 211, 176, 131, 156, 84, 224, 32,
		229, 183, 82, 186, 243, 41, 165, 59, 238, 55,
		229, 239, 53, 57, 253, 139, 100, 135, 34, 235,
		11, 133, 93, 172, 63, 83, 0, 152, 227, 53,
		44, 3, 123, 81, 39, 204, 1, 22, 52, 97,
		222, byte.MaxValue, 125, 39, 214, 138, 77, 75, 103, 7,
		156, 155, 67, 97, 184, 169, 80, 31, 69, 109,
		67, 226, 79, 110, 76, 182, 224, 186, 22, 101,
		232, 81, 224, 77, 4, 98, 97, 103, 131, 61,
		71, 4, 99, 206, 0, 14, 95, 73, 235, 147,
		40, 79, 233, 6, 102, 85, 70, 225, 163, 63,
		160, 182, 233, 37, 148, 56, 205, 109, 155, 0,
		10, 243, 34, 10, 12, 97, 103, 208, 119, 134,
		48, 61, 52, 69, 172, 234, 68, 57, 166, 56,
		200, 156, 208, 23, 44, 65, 247, 229, 41, 254,
		213, 44, 138, 242, 224, 126, 192, 90, 108, 194,
		124, 130, 123, 166, 114, 136, 36, 173, 235, 13,
		82, 108, 19, 120, 168, 62, 61, 122, 111, 176,
		173, 186, 40, 90, 80, 74, 253, 219, 206, 156,
		117, 12, 28, 77, 229, 173, 218, 10, 33, 44,
		207, 111, 164, 212, 133, 237, 87, 0, 233, 201,
		143, 214, 221, 233, 86, 153, 49, 64, 151, 69,
		1, 17, 50, 191, 59, 239, 43, 243, 197, 129,
		190, 47, 237, 161, 69, 195, 136, 223, 174, 98,
		171, byte.MaxValue, 75, 174, 101, 177, 69, 71, 115, 63,
		228, 67, 89, 114, 66, 42, 160, 226, 61, 213,
		254, 151, 66, 222, 47, 247, 59, 130, 47, 53,
		101, 12, 140, 207, 11, 150, 172, 9, 147, 162,
		240, 61, 29, 156, 223, 49, 162, 105, 19, 232,
		212, 177, 184, 91, 49, 106, 8, 130, 151, 213,
		81, 23, 154, 45, 8, 252, 212, 186, 70, 94,
		51, 148, 7, 99, 155, 117, 74, 51, 30, 170,
		203, 200, 46, 51, 146, 214, 94, 14, 84, 30,
		89, 23, 193, 141, 63, 13, 162, 19, 27, 199,
		80, 206, 186, 115, 52, 128, 227, 139, 123, 247,
		24, 20
	});

	// Token: 0x02001831 RID: 6193
	public class SaveType
	{
		// Token: 0x04009A27 RID: 39463
		public string legacyFilePattern;

		// Token: 0x04009A28 RID: 39464
		public string filePattern;

		// Token: 0x04009A29 RID: 39465
		public bool encrypted;

		// Token: 0x04009A2A RID: 39466
		public int backupCount;

		// Token: 0x04009A2B RID: 39467
		public int backupMinTimeMin;

		// Token: 0x04009A2C RID: 39468
		public string backupPattern;
	}

	// Token: 0x02001832 RID: 6194
	public enum SaveSlot
	{
		// Token: 0x04009A2E RID: 39470
		A,
		// Token: 0x04009A2F RID: 39471
		B,
		// Token: 0x04009A30 RID: 39472
		C,
		// Token: 0x04009A31 RID: 39473
		D
	}
}
