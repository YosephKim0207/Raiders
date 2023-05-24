using System;

namespace InControl.NativeProfile
{
	// Token: 0x0200070C RID: 1804
	public class ProEXXbox360ControllerMacProfile : Xbox360DriverMacProfile
	{
		// Token: 0x06002937 RID: 10551 RVA: 0x000AF694 File Offset: 0x000AD894
		public ProEXXbox360ControllerMacProfile()
		{
			base.Name = "Pro EX Xbox 360 Controller";
			base.Meta = "Pro EX Xbox 360 Controller on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(9414),
					ProductID = new ushort?(21258)
				}
			};
		}
	}
}
