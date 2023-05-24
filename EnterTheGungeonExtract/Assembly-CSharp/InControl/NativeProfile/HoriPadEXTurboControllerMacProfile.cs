using System;

namespace InControl.NativeProfile
{
	// Token: 0x020006C9 RID: 1737
	public class HoriPadEXTurboControllerMacProfile : Xbox360DriverMacProfile
	{
		// Token: 0x060028F4 RID: 10484 RVA: 0x000AD81C File Offset: 0x000ABA1C
		public HoriPadEXTurboControllerMacProfile()
		{
			base.Name = "Hori Pad EX Turbo Controller";
			base.Meta = "Hori Pad EX Turbo Controller on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(3853),
					ProductID = new ushort?(12)
				}
			};
		}
	}
}
