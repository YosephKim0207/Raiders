using System;
using UnityEngine;

// Token: 0x02000C21 RID: 3105
public static class tk2dUITime
{
	// Token: 0x17000A1F RID: 2591
	// (get) Token: 0x060042DD RID: 17117 RVA: 0x0015AB54 File Offset: 0x00158D54
	public static float deltaTime
	{
		get
		{
			return tk2dUITime._deltaTime;
		}
	}

	// Token: 0x060042DE RID: 17118 RVA: 0x0015AB5C File Offset: 0x00158D5C
	public static void Init()
	{
		tk2dUITime.lastRealTime = (double)Time.realtimeSinceStartup;
		tk2dUITime._deltaTime = Time.maximumDeltaTime;
	}

	// Token: 0x060042DF RID: 17119 RVA: 0x0015AB74 File Offset: 0x00158D74
	public static void Update()
	{
		float realtimeSinceStartup = Time.realtimeSinceStartup;
		if (Time.timeScale < 0.001f)
		{
			tk2dUITime._deltaTime = Mathf.Min(0.06666667f, (float)((double)realtimeSinceStartup - tk2dUITime.lastRealTime));
		}
		else
		{
			tk2dUITime._deltaTime = BraveTime.DeltaTime / Time.timeScale;
		}
		tk2dUITime.lastRealTime = (double)realtimeSinceStartup;
	}

	// Token: 0x0400351F RID: 13599
	private static double lastRealTime;

	// Token: 0x04003520 RID: 13600
	private static float _deltaTime = 0.016666668f;
}
