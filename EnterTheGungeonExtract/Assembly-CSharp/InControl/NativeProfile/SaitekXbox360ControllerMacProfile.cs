using System;

namespace InControl.NativeProfile
{
	// Token: 0x0200071B RID: 1819
	public class SaitekXbox360ControllerMacProfile : Xbox360DriverMacProfile
	{
		// Token: 0x06002946 RID: 10566 RVA: 0x000AFD84 File Offset: 0x000ADF84
		public SaitekXbox360ControllerMacProfile()
		{
			base.Name = "Saitek Xbox 360 Controller";
			base.Meta = "Saitek Xbox 360 Controller on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(1848),
					ProductID = new ushort?(51970)
				}
			};
		}
	}
}
