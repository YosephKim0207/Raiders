using System;

// Token: 0x0200040F RID: 1039
public static class dfFontManager
{
	// Token: 0x0600178D RID: 6029 RVA: 0x00070218 File Offset: 0x0006E418
	public static void FlagPendingRequests(dfFontBase font)
	{
		dfDynamicFont dfDynamicFont = font as dfDynamicFont;
		if (dfDynamicFont != null && !dfFontManager.rebuildList.Contains(dfDynamicFont))
		{
			dfFontManager.rebuildList.Add(dfDynamicFont);
		}
	}

	// Token: 0x0600178E RID: 6030 RVA: 0x00070254 File Offset: 0x0006E454
	public static void Invalidate(dfFontBase font)
	{
		if (font == null || !(font is dfDynamicFont))
		{
			return;
		}
		if (!dfFontManager.dirty.Contains(font))
		{
			dfFontManager.dirty.Add(font);
		}
	}

	// Token: 0x0600178F RID: 6031 RVA: 0x0007028C File Offset: 0x0006E48C
	public static bool IsDirty(dfFontBase font)
	{
		return dfFontManager.dirty.Contains(font);
	}

	// Token: 0x06001790 RID: 6032 RVA: 0x0007029C File Offset: 0x0006E49C
	public static bool RebuildDynamicFonts()
	{
		dfFontManager.rebuildList.Clear();
		dfList<dfControl> activeInstances = dfControl.ActiveInstances;
		for (int i = 0; i < activeInstances.Count; i++)
		{
			IRendersText rendersText = activeInstances[i] as IRendersText;
			if (rendersText != null)
			{
				rendersText.UpdateFontInfo();
			}
		}
		bool flag = dfFontManager.rebuildList.Count > 0;
		for (int j = 0; j < dfFontManager.rebuildList.Count; j++)
		{
			dfDynamicFont dfDynamicFont = dfFontManager.rebuildList[j] as dfDynamicFont;
			if (dfDynamicFont != null)
			{
				dfDynamicFont.FlushCharacterRequests();
			}
		}
		dfFontManager.rebuildList.Clear();
		dfFontManager.dirty.Clear();
		return flag;
	}

	// Token: 0x040012F7 RID: 4855
	private static dfList<dfFontBase> dirty = new dfList<dfFontBase>();

	// Token: 0x040012F8 RID: 4856
	private static dfList<dfFontBase> rebuildList = new dfList<dfFontBase>();
}
