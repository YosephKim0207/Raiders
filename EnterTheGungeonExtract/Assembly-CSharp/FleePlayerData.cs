using System;

// Token: 0x020013E6 RID: 5094
[Serializable]
public class FleePlayerData
{
	// Token: 0x17001165 RID: 4453
	// (get) Token: 0x0600739C RID: 29596 RVA: 0x002DFE38 File Offset: 0x002DE038
	// (set) Token: 0x0600739D RID: 29597 RVA: 0x002DFE40 File Offset: 0x002DE040
	public PlayerController Player { get; set; }

	// Token: 0x04007534 RID: 30004
	public float StartDistance = 6f;

	// Token: 0x04007535 RID: 30005
	public float DeathDistance = 9f;

	// Token: 0x04007536 RID: 30006
	public float StopDistance = 12f;
}
