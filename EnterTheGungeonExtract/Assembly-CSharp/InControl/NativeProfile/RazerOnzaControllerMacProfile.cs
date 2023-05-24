using System;

namespace InControl.NativeProfile
{
	// Token: 0x02000710 RID: 1808
	public class RazerOnzaControllerMacProfile : Xbox360DriverMacProfile
	{
		// Token: 0x0600293B RID: 10555 RVA: 0x000AF840 File Offset: 0x000ADA40
		public RazerOnzaControllerMacProfile()
		{
			base.Name = "Razer Onza Controller";
			base.Meta = "Razer Onza Controller on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(7085),
					ProductID = new ushort?(64769)
				},
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(5769),
					ProductID = new ushort?(64769)
				}
			};
		}
	}
}
