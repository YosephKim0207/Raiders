using System;

namespace InControl.NativeProfile
{
	// Token: 0x02000711 RID: 1809
	public class RazerOnzaTEControllerMacProfile : Xbox360DriverMacProfile
	{
		// Token: 0x0600293C RID: 10556 RVA: 0x000AF8CC File Offset: 0x000ADACC
		public RazerOnzaTEControllerMacProfile()
		{
			base.Name = "Razer Onza TE Controller";
			base.Meta = "Razer Onza TE Controller on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(7085),
					ProductID = new ushort?(64768)
				},
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(5769),
					ProductID = new ushort?(64768)
				}
			};
		}
	}
}
