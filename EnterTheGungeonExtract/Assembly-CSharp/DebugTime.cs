using System;

// Token: 0x02000E61 RID: 3681
public static class DebugTime
{
	// Token: 0x06004E56 RID: 20054 RVA: 0x001B1590 File Offset: 0x001AF790
	public static void RecordStartTime()
	{
	}

	// Token: 0x06004E57 RID: 20055 RVA: 0x001B1594 File Offset: 0x001AF794
	public static void Log(string str, params object[] args)
	{
	}

	// Token: 0x06004E58 RID: 20056 RVA: 0x001B1598 File Offset: 0x001AF798
	public static void Log(float startTime, int startFrame, string str, params object[] args)
	{
	}

	// Token: 0x06004E59 RID: 20057 RVA: 0x001B159C File Offset: 0x001AF79C
	public static string GetLogHistory()
	{
		return string.Empty;
	}
}
