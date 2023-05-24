using System;
using FullInspector;

// Token: 0x02000276 RID: 630
[InspectorDropdownName("Bosses/MetalGearRat/SidePoundRight1")]
public class MetalGearRatSidePoundRight1 : MetalGearRatSidePound1
{
	// Token: 0x17000238 RID: 568
	// (get) Token: 0x06000990 RID: 2448 RVA: 0x0002E21C File Offset: 0x0002C41C
	protected override float StartAngle
	{
		get
		{
			return 80f;
		}
	}

	// Token: 0x17000239 RID: 569
	// (get) Token: 0x06000991 RID: 2449 RVA: 0x0002E224 File Offset: 0x0002C424
	protected override float SweepAngle
	{
		get
		{
			return -100f;
		}
	}
}
