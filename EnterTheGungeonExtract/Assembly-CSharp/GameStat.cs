using System;

// Token: 0x020014FF RID: 5375
public class GameStat
{
	// Token: 0x06007AA5 RID: 31397 RVA: 0x00312C70 File Offset: 0x00310E70
	public GameStat(string name, float val)
	{
		this.statName = name;
		this.statValue = val;
	}

	// Token: 0x06007AA6 RID: 31398 RVA: 0x00312C94 File Offset: 0x00310E94
	public void Modify(float change)
	{
		this.statValue += change;
	}

	// Token: 0x04007D32 RID: 32050
	public string statName = string.Empty;

	// Token: 0x04007D33 RID: 32051
	public float statValue;
}
