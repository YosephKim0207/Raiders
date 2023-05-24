using System;
using UnityEngine;

// Token: 0x02000E12 RID: 3602
public static class BraveMemory
{
	// Token: 0x06004C53 RID: 19539 RVA: 0x001A0588 File Offset: 0x0019E788
	public static void EnsureHeapSize(int kilobytes)
	{
		object[] array = new object[kilobytes];
		for (int i = 0; i < kilobytes; i++)
		{
			array[i] = new byte[1024];
		}
		GC.Collect();
	}

	// Token: 0x06004C54 RID: 19540 RVA: 0x001A05C4 File Offset: 0x0019E7C4
	public static void DoCollect()
	{
		BraveMemory.LastGcTime = Time.realtimeSinceStartup;
		GC.Collect();
	}

	// Token: 0x06004C55 RID: 19541 RVA: 0x001A05D8 File Offset: 0x0019E7D8
	private static void MaybeDoCollect()
	{
		BraveMemory.LastGcTime = Time.realtimeSinceStartup;
	}

	// Token: 0x06004C56 RID: 19542 RVA: 0x001A05E4 File Offset: 0x0019E7E4
	private static bool CheckTime(float threshold)
	{
		return Time.realtimeSinceStartup - BraveMemory.LastGcTime > threshold;
	}

	// Token: 0x06004C57 RID: 19543 RVA: 0x001A05F4 File Offset: 0x0019E7F4
	private static void TestPC()
	{
		for (int i = 0; i < 5; i++)
		{
			GC.Collect();
		}
	}

	// Token: 0x06004C58 RID: 19544 RVA: 0x001A0618 File Offset: 0x0019E818
	public static void HandleBossCardFlashAnticipation()
	{
		if (BraveMemory.CheckTime(20f))
		{
			BraveMemory.MaybeDoCollect();
			GenericIntroDoer.SkipFrame = true;
		}
	}

	// Token: 0x06004C59 RID: 19545 RVA: 0x001A0634 File Offset: 0x0019E834
	public static void HandleBossCardSkip()
	{
		if (BraveMemory.CheckTime(20f))
		{
			BraveMemory.MaybeDoCollect();
			GenericIntroDoer.SkipFrame = true;
		}
	}

	// Token: 0x06004C5A RID: 19546 RVA: 0x001A0650 File Offset: 0x0019E850
	public static void HandleRoomEntered(int numOfEnemies)
	{
		float num = 360f;
		if (numOfEnemies >= 8)
		{
			num = 240f;
		}
		if (BraveMemory.CheckTime(num))
		{
			BraveMemory.MaybeDoCollect();
		}
	}

	// Token: 0x06004C5B RID: 19547 RVA: 0x001A0680 File Offset: 0x0019E880
	public static void HandleGamePaused()
	{
		if (GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER && BraveMemory.CheckTime(360f))
		{
			BraveMemory.MaybeDoCollect();
		}
	}

	// Token: 0x06004C5C RID: 19548 RVA: 0x001A06A8 File Offset: 0x0019E8A8
	public static void HandleTeleportation()
	{
		if (BraveMemory.CheckTime(300f))
		{
			BraveMemory.MaybeDoCollect();
		}
	}

	// Token: 0x0400423E RID: 16958
	private static float LastGcTime;
}
