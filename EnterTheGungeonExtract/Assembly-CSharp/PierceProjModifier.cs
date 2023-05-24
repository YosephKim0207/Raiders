using System;
using UnityEngine;

// Token: 0x0200165B RID: 5723
public class PierceProjModifier : MonoBehaviour
{
	// Token: 0x0600858C RID: 34188 RVA: 0x00371824 File Offset: 0x0036FA24
	public bool HandleBossImpact()
	{
		if (this.UsesMaxBossImpacts)
		{
			this.m_bossImpacts++;
			if (this.m_bossImpacts >= this.MaxBossImpacts)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x040089C2 RID: 35266
	public int penetration = 1;

	// Token: 0x040089C3 RID: 35267
	public bool penetratesBreakables;

	// Token: 0x040089C4 RID: 35268
	public bool preventPenetrationOfActors;

	// Token: 0x040089C5 RID: 35269
	public PierceProjModifier.BeastModeStatus BeastModeLevel;

	// Token: 0x040089C6 RID: 35270
	public bool UsesMaxBossImpacts;

	// Token: 0x040089C7 RID: 35271
	public int MaxBossImpacts = -1;

	// Token: 0x040089C8 RID: 35272
	private int m_bossImpacts;

	// Token: 0x0200165C RID: 5724
	public enum BeastModeStatus
	{
		// Token: 0x040089CA RID: 35274
		NOT_BEAST_MODE,
		// Token: 0x040089CB RID: 35275
		BEAST_MODE_LEVEL_ONE
	}
}
