using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001822 RID: 6178
public static class BraveTime
{
	// Token: 0x170015DC RID: 5596
	// (get) Token: 0x06009207 RID: 37383 RVA: 0x003DBA40 File Offset: 0x003D9C40
	public static float DeltaTime
	{
		get
		{
			return BraveTime.m_cachedDeltaTime;
		}
	}

	// Token: 0x06009208 RID: 37384 RVA: 0x003DBA48 File Offset: 0x003D9C48
	public static void CacheDeltaTimeForFrame()
	{
		BraveTime.m_cachedDeltaTime = Mathf.Min(0.1f, Time.deltaTime);
	}

	// Token: 0x170015DD RID: 5597
	// (get) Token: 0x06009209 RID: 37385 RVA: 0x003DBA60 File Offset: 0x003D9C60
	public static float ScaledTimeSinceStartup
	{
		get
		{
			BraveTime.UpdateScaledTimeSinceStartup();
			return BraveTime.s_scaledTimeSinceStartup;
		}
	}

	// Token: 0x0600920A RID: 37386 RVA: 0x003DBA6C File Offset: 0x003D9C6C
	public static void UpdateScaledTimeSinceStartup()
	{
		if (BraveTime.s_lastScaledTimeFrameUpdate == Time.frameCount)
		{
			return;
		}
		BraveTime.s_scaledTimeSinceStartup += BraveTime.DeltaTime;
		BraveTime.s_lastScaledTimeFrameUpdate = Time.frameCount;
	}

	// Token: 0x0600920B RID: 37387 RVA: 0x003DBA98 File Offset: 0x003D9C98
	public static void RegisterTimeScaleMultiplier(float multiplier, GameObject source)
	{
		if (!BraveTime.m_sources.Contains(source))
		{
			BraveTime.m_sources.Add(source);
			BraveTime.m_multipliers.Add(1f);
		}
		int num = BraveTime.m_sources.IndexOf(source);
		BraveTime.m_multipliers[num] = BraveTime.m_multipliers[num] * multiplier;
		BraveTime.UpdateTimeScale();
	}

	// Token: 0x0600920C RID: 37388 RVA: 0x003DBAF8 File Offset: 0x003D9CF8
	public static void SetTimeScaleMultiplier(float multiplier, GameObject source)
	{
		if (!BraveTime.m_sources.Contains(source))
		{
			BraveTime.m_sources.Add(source);
			BraveTime.m_multipliers.Add(1f);
		}
		int num = BraveTime.m_sources.IndexOf(source);
		BraveTime.m_multipliers[num] = multiplier;
		BraveTime.UpdateTimeScale();
	}

	// Token: 0x0600920D RID: 37389 RVA: 0x003DBB4C File Offset: 0x003D9D4C
	public static void ClearMultiplier(GameObject source)
	{
		int num = BraveTime.m_sources.IndexOf(source);
		if (num >= 0)
		{
			BraveTime.m_sources.RemoveAt(num);
			BraveTime.m_multipliers.RemoveAt(num);
		}
		BraveTime.UpdateTimeScale();
	}

	// Token: 0x0600920E RID: 37390 RVA: 0x003DBB88 File Offset: 0x003D9D88
	public static void ClearAllMultipliers()
	{
		BraveTime.m_sources.Clear();
		BraveTime.m_multipliers.Clear();
		BraveTime.UpdateTimeScale();
	}

	// Token: 0x0600920F RID: 37391 RVA: 0x003DBBA4 File Offset: 0x003D9DA4
	private static void UpdateTimeScale()
	{
		float num = 1f;
		for (int i = 0; i < BraveTime.m_multipliers.Count; i++)
		{
			num = BraveTime.m_multipliers[i] * num;
		}
		if (float.IsNaN(num))
		{
			Debug.LogError("TIMESCALE WAS MY NAN ALL ALONG");
			num = 1f;
		}
		num = Mathf.Clamp(num, 0f, (!ChallengeManager.CHALLENGE_MODE_ACTIVE) ? 1f : 1.5f);
		Time.timeScale = num;
	}

	// Token: 0x040099E2 RID: 39394
	private static List<GameObject> m_sources = new List<GameObject>();

	// Token: 0x040099E3 RID: 39395
	private static List<float> m_multipliers = new List<float>();

	// Token: 0x040099E4 RID: 39396
	private static int s_lastScaledTimeFrameUpdate = -1;

	// Token: 0x040099E5 RID: 39397
	private static float s_scaledTimeSinceStartup = 0f;

	// Token: 0x040099E6 RID: 39398
	private static float m_cachedDeltaTime;
}
