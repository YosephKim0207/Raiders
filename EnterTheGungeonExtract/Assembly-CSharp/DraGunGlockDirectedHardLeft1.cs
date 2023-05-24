using System;
using FullInspector;

// Token: 0x0200016B RID: 363
[InspectorDropdownName("Bosses/DraGun/GlockDirectedHardLeft1")]
public class DraGunGlockDirectedHardLeft1 : DraGunGlockDirected1
{
	// Token: 0x17000140 RID: 320
	// (get) Token: 0x06000571 RID: 1393 RVA: 0x0001A2B4 File Offset: 0x000184B4
	protected override string BulletName
	{
		get
		{
			return "glockLeft";
		}
	}

	// Token: 0x17000141 RID: 321
	// (get) Token: 0x06000572 RID: 1394 RVA: 0x0001A2BC File Offset: 0x000184BC
	protected override bool IsHard
	{
		get
		{
			return true;
		}
	}
}
