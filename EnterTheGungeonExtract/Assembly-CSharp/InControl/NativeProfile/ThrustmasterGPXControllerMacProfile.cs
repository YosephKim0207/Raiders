using System;

namespace InControl.NativeProfile
{
	// Token: 0x0200071D RID: 1821
	public class ThrustmasterGPXControllerMacProfile : Xbox360DriverMacProfile
	{
		// Token: 0x06002948 RID: 10568 RVA: 0x000AFE70 File Offset: 0x000AE070
		public ThrustmasterGPXControllerMacProfile()
		{
			base.Name = "Thrustmaster GPX Controller";
			base.Meta = "Thrustmaster GPX Controller on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(1103),
					ProductID = new ushort?(45862)
				},
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(9414),
					ProductID = new ushort?(23298)
				}
			};
		}
	}
}
