using System;

namespace InControl.NativeProfile
{
	// Token: 0x020006FB RID: 1787
	public class MVCTEStickMacProfile : Xbox360DriverMacProfile
	{
		// Token: 0x06002926 RID: 10534 RVA: 0x000AED90 File Offset: 0x000ACF90
		public MVCTEStickMacProfile()
		{
			base.Name = "MVC TE Stick";
			base.Meta = "MVC TE Stick on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(7085),
					ProductID = new ushort?(61497)
				},
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(1848),
					ProductID = new ushort?(46904)
				}
			};
		}
	}
}
