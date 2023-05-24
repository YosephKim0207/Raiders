using System;

namespace InControl.NativeProfile
{
	// Token: 0x0200070F RID: 1807
	public class RazerAtroxArcadeStickMacProfile : Xbox360DriverMacProfile
	{
		// Token: 0x0600293A RID: 10554 RVA: 0x000AF7B4 File Offset: 0x000AD9B4
		public RazerAtroxArcadeStickMacProfile()
		{
			base.Name = "Razer Atrox Arcade Stick";
			base.Meta = "Razer Atrox Arcade Stick on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(5426),
					ProductID = new ushort?(2560)
				},
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(9414),
					ProductID = new ushort?(20480)
				}
			};
		}
	}
}
