using System;

namespace InControl.NativeProfile
{
	// Token: 0x0200071C RID: 1820
	public class ThrustmasterFerrari458RacingWheelMacProfile : Xbox360DriverMacProfile
	{
		// Token: 0x06002947 RID: 10567 RVA: 0x000AFDE4 File Offset: 0x000ADFE4
		public ThrustmasterFerrari458RacingWheelMacProfile()
		{
			base.Name = "Thrustmaster Ferrari 458 Racing Wheel";
			base.Meta = "Thrustmaster Ferrari 458 Racing Wheel on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(9414),
					ProductID = new ushort?(23296)
				},
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(9414),
					ProductID = new ushort?(23299)
				}
			};
		}
	}
}
