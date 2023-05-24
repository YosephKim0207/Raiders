using System;

namespace InControl.NativeProfile
{
	// Token: 0x020006E6 RID: 1766
	public class MadCatzFPSProMacProfile : Xbox360DriverMacProfile
	{
		// Token: 0x06002911 RID: 10513 RVA: 0x000AE38C File Offset: 0x000AC58C
		public MadCatzFPSProMacProfile()
		{
			base.Name = "Mad Catz FPS Pro";
			base.Meta = "Mad Catz FPS Pro on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(7085),
					ProductID = new ushort?(61479)
				}
			};
		}
	}
}
