using System;

namespace InControl.NativeProfile
{
	// Token: 0x02000714 RID: 1812
	public class RazerWildcatControllerMacProfile : Xbox360DriverMacProfile
	{
		// Token: 0x0600293F RID: 10559 RVA: 0x000AFA40 File Offset: 0x000ADC40
		public RazerWildcatControllerMacProfile()
		{
			base.Name = "Razer Wildcat Controller";
			base.Meta = "Razer Wildcat Controller on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(5426),
					ProductID = new ushort?(2563)
				}
			};
		}
	}
}
