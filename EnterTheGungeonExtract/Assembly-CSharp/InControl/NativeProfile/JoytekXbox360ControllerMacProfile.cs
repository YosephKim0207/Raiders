using System;

namespace InControl.NativeProfile
{
	// Token: 0x020006D5 RID: 1749
	public class JoytekXbox360ControllerMacProfile : Xbox360DriverMacProfile
	{
		// Token: 0x06002900 RID: 10496 RVA: 0x000ADCB0 File Offset: 0x000ABEB0
		public JoytekXbox360ControllerMacProfile()
		{
			base.Name = "Joytek Xbox 360 Controller";
			base.Meta = "Joytek Xbox 360 Controller on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(5678),
					ProductID = new ushort?(48879)
				}
			};
		}
	}
}
