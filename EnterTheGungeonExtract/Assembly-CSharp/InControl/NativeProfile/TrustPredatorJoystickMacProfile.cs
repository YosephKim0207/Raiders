using System;

namespace InControl.NativeProfile
{
	// Token: 0x0200071E RID: 1822
	public class TrustPredatorJoystickMacProfile : Xbox360DriverMacProfile
	{
		// Token: 0x06002949 RID: 10569 RVA: 0x000AFEFC File Offset: 0x000AE0FC
		public TrustPredatorJoystickMacProfile()
		{
			base.Name = "Trust Predator Joystick";
			base.Meta = "Trust Predator Joystick on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(2064),
					ProductID = new ushort?(3)
				}
			};
		}
	}
}
