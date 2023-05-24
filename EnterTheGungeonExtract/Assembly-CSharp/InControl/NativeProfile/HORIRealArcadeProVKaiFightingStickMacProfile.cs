using System;

namespace InControl.NativeProfile
{
	// Token: 0x020006CF RID: 1743
	public class HORIRealArcadeProVKaiFightingStickMacProfile : Xbox360DriverMacProfile
	{
		// Token: 0x060028FA RID: 10490 RVA: 0x000ADA50 File Offset: 0x000ABC50
		public HORIRealArcadeProVKaiFightingStickMacProfile()
		{
			base.Name = "HORI Real Arcade Pro V Kai Fighting Stick";
			base.Meta = "HORI Real Arcade Pro V Kai Fighting Stick on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(9414),
					ProductID = new ushort?(21774)
				},
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(3853),
					ProductID = new ushort?(120)
				}
			};
		}
	}
}
