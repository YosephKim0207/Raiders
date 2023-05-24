using System;

namespace InControl.NativeProfile
{
	// Token: 0x020006D6 RID: 1750
	public class KonamiDancePadMacProfile : Xbox360DriverMacProfile
	{
		// Token: 0x06002901 RID: 10497 RVA: 0x000ADD10 File Offset: 0x000ABF10
		public KonamiDancePadMacProfile()
		{
			base.Name = "Konami Dance Pad";
			base.Meta = "Konami Dance Pad on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(4779),
					ProductID = new ushort?(4)
				}
			};
		}
	}
}
