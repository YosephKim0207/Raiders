using System;

namespace InControl.NativeProfile
{
	// Token: 0x020006DC RID: 1756
	public class LogitechF710ControllerMacProfile : Xbox360DriverMacProfile
	{
		// Token: 0x06002907 RID: 10503 RVA: 0x000ADF4C File Offset: 0x000AC14C
		public LogitechF710ControllerMacProfile()
		{
			base.Name = "Logitech F710 Controller";
			base.Meta = "Logitech F710 Controller on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(1133),
					ProductID = new ushort?(49695)
				}
			};
		}
	}
}
