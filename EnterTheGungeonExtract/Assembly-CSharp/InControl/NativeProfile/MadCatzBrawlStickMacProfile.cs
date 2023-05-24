using System;

namespace InControl.NativeProfile
{
	// Token: 0x020006DF RID: 1759
	public class MadCatzBrawlStickMacProfile : Xbox360DriverMacProfile
	{
		// Token: 0x0600290A RID: 10506 RVA: 0x000AE06C File Offset: 0x000AC26C
		public MadCatzBrawlStickMacProfile()
		{
			base.Name = "Mad Catz Brawl Stick";
			base.Meta = "Mad Catz Brawl Stick on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(7085),
					ProductID = new ushort?(61465)
				}
			};
		}
	}
}
