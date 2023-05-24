using System;

namespace InControl.NativeProfile
{
	// Token: 0x020006CB RID: 1739
	public class HoriRealArcadeProEXMacProfile : Xbox360DriverMacProfile
	{
		// Token: 0x060028F6 RID: 10486 RVA: 0x000AD8D8 File Offset: 0x000ABAD8
		public HoriRealArcadeProEXMacProfile()
		{
			base.Name = "Hori Real Arcade Pro EX";
			base.Meta = "Hori Real Arcade Pro EX on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(7085),
					ProductID = new ushort?(62724)
				}
			};
		}
	}
}
