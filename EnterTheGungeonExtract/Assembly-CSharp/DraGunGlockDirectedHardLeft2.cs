using System;
using FullInspector;

// Token: 0x02000171 RID: 369
[InspectorDropdownName("Bosses/DraGun/GlockDirectedHardLeft2")]
public class DraGunGlockDirectedHardLeft2 : DraGunGlockDirected2
{
	// Token: 0x1700014A RID: 330
	// (get) Token: 0x06000585 RID: 1413 RVA: 0x0001A680 File Offset: 0x00018880
	protected override string BulletName
	{
		get
		{
			return "glockLeft";
		}
	}

	// Token: 0x1700014B RID: 331
	// (get) Token: 0x06000586 RID: 1414 RVA: 0x0001A688 File Offset: 0x00018888
	protected override bool IsHard
	{
		get
		{
			return true;
		}
	}
}
