using System;

namespace InControl.NativeProfile
{
	// Token: 0x020006C5 RID: 1733
	public class HoriEX2ControllerMacProfile : Xbox360DriverMacProfile
	{
		// Token: 0x060028F0 RID: 10480 RVA: 0x000AD5D4 File Offset: 0x000AB7D4
		public HoriEX2ControllerMacProfile()
		{
			base.Name = "Hori EX2 Controller";
			base.Meta = "Hori EX2 Controller on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(3853),
					ProductID = new ushort?(13)
				},
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(7085),
					ProductID = new ushort?(62721)
				},
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(9414),
					ProductID = new ushort?(21760)
				}
			};
		}
	}
}
