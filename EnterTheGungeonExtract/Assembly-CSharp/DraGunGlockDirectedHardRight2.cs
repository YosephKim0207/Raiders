using System;
using FullInspector;

// Token: 0x02000172 RID: 370
[InspectorDropdownName("Bosses/DraGun/GlockDirectedHardRight2")]
public class DraGunGlockDirectedHardRight2 : DraGunGlockDirected2
{
	// Token: 0x1700014C RID: 332
	// (get) Token: 0x06000588 RID: 1416 RVA: 0x0001A694 File Offset: 0x00018894
	protected override string BulletName
	{
		get
		{
			return "glockRight";
		}
	}

	// Token: 0x1700014D RID: 333
	// (get) Token: 0x06000589 RID: 1417 RVA: 0x0001A69C File Offset: 0x0001889C
	protected override bool IsHard
	{
		get
		{
			return true;
		}
	}
}
