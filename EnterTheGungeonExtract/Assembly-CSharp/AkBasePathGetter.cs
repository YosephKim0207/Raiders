using System;
using System.IO;
using UnityEngine;

// Token: 0x020018BF RID: 6335
public class AkBasePathGetter
{
	// Token: 0x06009C6C RID: 40044 RVA: 0x003EC278 File Offset: 0x003EA478
	public static string GetPlatformName()
	{
		string empty = string.Empty;
		if (!string.IsNullOrEmpty(empty))
		{
			return empty;
		}
		return "Windows";
	}

	// Token: 0x06009C6D RID: 40045 RVA: 0x003EC2A0 File Offset: 0x003EA4A0
	public static string GetPlatformBasePath()
	{
		string platformName = AkBasePathGetter.GetPlatformName();
		string text = Path.Combine(AkBasePathGetter.GetFullSoundBankPath(), platformName);
		AkBasePathGetter.FixSlashes(ref text);
		return text;
	}

	// Token: 0x06009C6E RID: 40046 RVA: 0x003EC2C8 File Offset: 0x003EA4C8
	public static string GetFullSoundBankPath()
	{
		string text = Path.Combine(Application.streamingAssetsPath, AkInitializer.GetBasePath());
		AkBasePathGetter.FixSlashes(ref text);
		return text;
	}

	// Token: 0x06009C6F RID: 40047 RVA: 0x003EC2F0 File Offset: 0x003EA4F0
	public static void FixSlashes(ref string path, char separatorChar, char badChar, bool addTrailingSlash)
	{
		if (string.IsNullOrEmpty(path))
		{
			return;
		}
		path = path.Trim().Replace(badChar, separatorChar).TrimStart(new char[] { '\\' });
		if (addTrailingSlash && !path.EndsWith(separatorChar.ToString()))
		{
			path += separatorChar;
		}
	}

	// Token: 0x06009C70 RID: 40048 RVA: 0x003EC358 File Offset: 0x003EA558
	public static void FixSlashes(ref string path)
	{
		char directorySeparatorChar = Path.DirectorySeparatorChar;
		char c = ((directorySeparatorChar != '\\') ? '\\' : '/');
		AkBasePathGetter.FixSlashes(ref path, directorySeparatorChar, c, true);
	}

	// Token: 0x06009C71 RID: 40049 RVA: 0x003EC388 File Offset: 0x003EA588
	public static string GetSoundbankBasePath()
	{
		string platformBasePath = AkBasePathGetter.GetPlatformBasePath();
		bool flag = true;
		string text = Path.Combine(platformBasePath, "Init.bnk");
		if (!File.Exists(text))
		{
			flag = false;
		}
		if (platformBasePath == string.Empty || !flag)
		{
			Debug.Log("WwiseUnity: Looking for SoundBanks in " + platformBasePath);
			Debug.LogError("WwiseUnity: Could not locate the SoundBanks. Did you make sure to copy them to the StreamingAssets folder?");
		}
		return platformBasePath;
	}
}
