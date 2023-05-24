using System;

namespace InControl.NativeProfile
{
	// Token: 0x020006FA RID: 1786
	public class MLGControllerMacProfile : Xbox360DriverMacProfile
	{
		// Token: 0x06002925 RID: 10533 RVA: 0x000AED30 File Offset: 0x000ACF30
		public MLGControllerMacProfile()
		{
			base.Name = "MLG Controller";
			base.Meta = "MLG Controller on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(7085),
					ProductID = new ushort?(61475)
				}
			};
		}
	}
}
