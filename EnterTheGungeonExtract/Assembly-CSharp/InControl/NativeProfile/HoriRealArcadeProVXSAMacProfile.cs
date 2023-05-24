using System;

namespace InControl.NativeProfile
{
	// Token: 0x020006D1 RID: 1745
	public class HoriRealArcadeProVXSAMacProfile : Xbox360DriverMacProfile
	{
		// Token: 0x060028FC RID: 10492 RVA: 0x000ADB34 File Offset: 0x000ABD34
		public HoriRealArcadeProVXSAMacProfile()
		{
			base.Name = "Hori Real Arcade Pro VX SA";
			base.Meta = "Hori Real Arcade Pro VX SA on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(7085),
					ProductID = new ushort?(62722)
				}
			};
		}
	}
}
