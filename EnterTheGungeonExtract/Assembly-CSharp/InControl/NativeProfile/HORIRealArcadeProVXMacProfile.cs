using System;

namespace InControl.NativeProfile
{
	// Token: 0x020006D0 RID: 1744
	public class HORIRealArcadeProVXMacProfile : Xbox360DriverMacProfile
	{
		// Token: 0x060028FB RID: 10491 RVA: 0x000ADAD8 File Offset: 0x000ABCD8
		public HORIRealArcadeProVXMacProfile()
		{
			base.Name = "HORI Real Arcade Pro VX";
			base.Meta = "HORI Real Arcade Pro VX on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(3853),
					ProductID = new ushort?(27)
				}
			};
		}
	}
}
