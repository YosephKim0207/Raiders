using System;

namespace InControl.NativeProfile
{
	// Token: 0x020006CD RID: 1741
	public class HoriRealArcadeProEXSEMacProfile : Xbox360DriverMacProfile
	{
		// Token: 0x060028F8 RID: 10488 RVA: 0x000AD998 File Offset: 0x000ABB98
		public HoriRealArcadeProEXSEMacProfile()
		{
			base.Name = "Hori Real Arcade Pro EX SE";
			base.Meta = "Hori Real Arcade Pro EX SE on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(3853),
					ProductID = new ushort?(22)
				}
			};
		}
	}
}
