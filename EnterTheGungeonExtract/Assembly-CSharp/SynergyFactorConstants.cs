using System;
using UnityEngine;

// Token: 0x0200155A RID: 5466
public static class SynergyFactorConstants
{
	// Token: 0x06007D3F RID: 32063 RVA: 0x00329F68 File Offset: 0x00328168
	public static float GetSynergyFactor()
	{
		int numberOfSynergiesEncounteredThisRun = GameStatsManager.Instance.GetNumberOfSynergiesEncounteredThisRun();
		float num = 0.6f;
		float num2 = 0.006260342f + 0.9935921f * Mathf.Exp(-1.626339f * (float)numberOfSynergiesEncounteredThisRun);
		if (numberOfSynergiesEncounteredThisRun == 0)
		{
			if (GameStatsManager.Instance.GetPlayerStatValue(TrackedStats.TIMES_REACHED_FORGE) >= 3f)
			{
				num = 0.8f;
			}
			else if (GameStatsManager.Instance.GetPlayerStatValue(TrackedStats.TIMES_REACHED_CATACOMBS) >= 3f)
			{
				num = 1f;
			}
			else if (GameStatsManager.Instance.GetPlayerStatValue(TrackedStats.TIMES_REACHED_MINES) >= 3f)
			{
				num = 1.5f;
			}
			else if (GameStatsManager.Instance.GetPlayerStatValue(TrackedStats.TIMES_REACHED_GUNGEON) >= 3f)
			{
				num = 3f;
			}
			else
			{
				num = 5f;
			}
		}
		return 1f + num * num2;
	}
}
