using System;

namespace InControl.NativeProfile
{
	// Token: 0x020006C3 RID: 1731
	public class HoriControllerMacProfile : Xbox360DriverMacProfile
	{
		// Token: 0x060028EE RID: 10478 RVA: 0x000AD4EC File Offset: 0x000AB6EC
		public HoriControllerMacProfile()
		{
			base.Name = "Hori Controller";
			base.Meta = "Hori Controller on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(7085),
					ProductID = new ushort?(21760)
				},
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(7085),
					ProductID = new ushort?(654)
				}
			};
		}
	}
}
