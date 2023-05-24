using System;

namespace InControl.NativeProfile
{
	// Token: 0x020006DB RID: 1755
	public class LogitechF510ControllerMacProfile : Xbox360DriverMacProfile
	{
		// Token: 0x06002906 RID: 10502 RVA: 0x000ADEEC File Offset: 0x000AC0EC
		public LogitechF510ControllerMacProfile()
		{
			base.Name = "Logitech F510 Controller";
			base.Meta = "Logitech F510 Controller on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(1133),
					ProductID = new ushort?(49694)
				}
			};
		}
	}
}
