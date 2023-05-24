using System;

namespace InControl.NativeProfile
{
	// Token: 0x020006D2 RID: 1746
	public class HoriXbox360GemPadExMacProfile : Xbox360DriverMacProfile
	{
		// Token: 0x060028FD RID: 10493 RVA: 0x000ADB94 File Offset: 0x000ABD94
		public HoriXbox360GemPadExMacProfile()
		{
			base.Name = "Hori Xbox 360 Gem Pad Ex";
			base.Meta = "Hori Xbox 360 Gem Pad Ex on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(9414),
					ProductID = new ushort?(21773)
				}
			};
		}
	}
}
