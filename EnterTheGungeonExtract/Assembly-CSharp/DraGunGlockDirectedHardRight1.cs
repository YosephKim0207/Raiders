using System;
using FullInspector;

// Token: 0x0200016C RID: 364
[InspectorDropdownName("Bosses/DraGun/GlockDirectedHardRight1")]
public class DraGunGlockDirectedHardRight1 : DraGunGlockDirected1
{
	// Token: 0x17000142 RID: 322
	// (get) Token: 0x06000574 RID: 1396 RVA: 0x0001A2C8 File Offset: 0x000184C8
	protected override string BulletName
	{
		get
		{
			return "glockRight";
		}
	}

	// Token: 0x17000143 RID: 323
	// (get) Token: 0x06000575 RID: 1397 RVA: 0x0001A2D0 File Offset: 0x000184D0
	protected override bool IsHard
	{
		get
		{
			return true;
		}
	}
}
