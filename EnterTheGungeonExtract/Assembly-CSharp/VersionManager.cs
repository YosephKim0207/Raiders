using System;
using System.IO;
using UnityEngine;

// Token: 0x0200183B RID: 6203
public class VersionManager
{
	// Token: 0x170015E3 RID: 5603
	// (get) Token: 0x060092DA RID: 37594 RVA: 0x003E0440 File Offset: 0x003DE640
	public static string DisplayVersionNumber
	{
		get
		{
			if (!VersionManager.s_initialized)
			{
				VersionManager.Initialize();
			}
			return VersionManager.s_displayVersionNumber;
		}
	}

	// Token: 0x170015E4 RID: 5604
	// (get) Token: 0x060092DB RID: 37595 RVA: 0x003E0458 File Offset: 0x003DE658
	public static string UniqueVersionNumber
	{
		get
		{
			if (!VersionManager.s_initialized)
			{
				VersionManager.Initialize();
			}
			return VersionManager.s_realVersionNumber ?? VersionManager.s_displayVersionNumber;
		}
	}

	// Token: 0x060092DC RID: 37596 RVA: 0x003E047C File Offset: 0x003DE67C
	private static void Initialize()
	{
		try
		{
			string text = Path.Combine(Application.streamingAssetsPath, "version.txt");
			if (File.Exists(text))
			{
				string[] array = File.ReadAllLines(text);
				if (array.Length > 0)
				{
					VersionManager.s_initialized = true;
					VersionManager.s_displayVersionNumber = array[0];
					VersionManager.s_realVersionNumber = ((array.Length <= 1) ? null : array[1]);
					return;
				}
			}
		}
		catch
		{
		}
		VersionManager.s_initialized = true;
		VersionManager.s_displayVersionNumber = string.Empty;
		VersionManager.s_realVersionNumber = null;
	}

	// Token: 0x04009A61 RID: 39521
	private static bool s_initialized;

	// Token: 0x04009A62 RID: 39522
	private static string s_displayVersionNumber = string.Empty;

	// Token: 0x04009A63 RID: 39523
	private static string s_realVersionNumber;
}
