using System;

namespace InControl.NativeProfile
{
	// Token: 0x02000720 RID: 1824
	public class Xbox360ControllerMacProfile : Xbox360DriverMacProfile
	{
		// Token: 0x0600294B RID: 10571 RVA: 0x000AFFB8 File Offset: 0x000AE1B8
		public Xbox360ControllerMacProfile()
		{
			base.Name = "Xbox 360 Controller";
			base.Meta = "Xbox 360 Controller on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(3695),
					ProductID = new ushort?(62721)
				}
			};
		}
	}
}
