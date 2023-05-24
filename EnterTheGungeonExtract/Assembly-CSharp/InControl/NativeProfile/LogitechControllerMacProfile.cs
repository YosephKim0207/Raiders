using System;

namespace InControl.NativeProfile
{
	// Token: 0x020006D8 RID: 1752
	public class LogitechControllerMacProfile : Xbox360DriverMacProfile
	{
		// Token: 0x06002903 RID: 10499 RVA: 0x000ADDCC File Offset: 0x000ABFCC
		public LogitechControllerMacProfile()
		{
			base.Name = "Logitech Controller";
			base.Meta = "Logitech Controller on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(1133),
					ProductID = new ushort?(62209)
				}
			};
		}
	}
}
