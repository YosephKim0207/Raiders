using System;

namespace InControl.NativeProfile
{
	// Token: 0x02000713 RID: 1811
	public class RazerStrikeControllerMacProfile : Xbox360DriverMacProfile
	{
		// Token: 0x0600293E RID: 10558 RVA: 0x000AF9E4 File Offset: 0x000ADBE4
		public RazerStrikeControllerMacProfile()
		{
			base.Name = "Razer Strike Controller";
			base.Meta = "Razer Strike Controller on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(5769),
					ProductID = new ushort?(1)
				}
			};
		}
	}
}
