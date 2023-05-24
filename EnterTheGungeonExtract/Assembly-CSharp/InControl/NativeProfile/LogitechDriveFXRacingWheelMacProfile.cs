using System;

namespace InControl.NativeProfile
{
	// Token: 0x020006D9 RID: 1753
	public class LogitechDriveFXRacingWheelMacProfile : Xbox360DriverMacProfile
	{
		// Token: 0x06002904 RID: 10500 RVA: 0x000ADE2C File Offset: 0x000AC02C
		public LogitechDriveFXRacingWheelMacProfile()
		{
			base.Name = "Logitech DriveFX Racing Wheel";
			base.Meta = "Logitech DriveFX Racing Wheel on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(1133),
					ProductID = new ushort?(51875)
				}
			};
		}
	}
}
