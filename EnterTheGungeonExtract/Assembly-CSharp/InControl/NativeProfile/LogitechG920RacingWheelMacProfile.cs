using System;

namespace InControl.NativeProfile
{
	// Token: 0x020006DD RID: 1757
	public class LogitechG920RacingWheelMacProfile : Xbox360DriverMacProfile
	{
		// Token: 0x06002908 RID: 10504 RVA: 0x000ADFAC File Offset: 0x000AC1AC
		public LogitechG920RacingWheelMacProfile()
		{
			base.Name = "Logitech G920 Racing Wheel";
			base.Meta = "Logitech G920 Racing Wheel on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(1133),
					ProductID = new ushort?(49761)
				}
			};
		}
	}
}
