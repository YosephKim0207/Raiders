using System;
using UnityEngine;

// Token: 0x0200125A RID: 4698
[Serializable]
public class BloodthirstSettings
{
	// Token: 0x040065AF RID: 26031
	public int NumKillsForHealRequiredBase = 5;

	// Token: 0x040065B0 RID: 26032
	public int NumKillsAddedPerHealthGained = 5;

	// Token: 0x040065B1 RID: 26033
	public int NumKillsRequiredCap = 50;

	// Token: 0x040065B2 RID: 26034
	public float Radius = 5f;

	// Token: 0x040065B3 RID: 26035
	public float DamagePerSecond = 30f;

	// Token: 0x040065B4 RID: 26036
	[Range(0f, 1f)]
	public float PercentAffected = 0.5f;
}
