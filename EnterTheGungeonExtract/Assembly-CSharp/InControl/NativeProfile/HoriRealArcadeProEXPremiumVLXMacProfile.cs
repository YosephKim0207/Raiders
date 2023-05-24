using System;

namespace InControl.NativeProfile
{
	// Token: 0x020006CC RID: 1740
	public class HoriRealArcadeProEXPremiumVLXMacProfile : Xbox360DriverMacProfile
	{
		// Token: 0x060028F7 RID: 10487 RVA: 0x000AD938 File Offset: 0x000ABB38
		public HoriRealArcadeProEXPremiumVLXMacProfile()
		{
			base.Name = "Hori Real Arcade Pro EX Premium VLX";
			base.Meta = "Hori Real Arcade Pro EX Premium VLX on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(7085),
					ProductID = new ushort?(62726)
				}
			};
		}
	}
}
