using System;

namespace InControl.NativeProfile
{
	// Token: 0x02000716 RID: 1814
	public class RockBandDrumsMacProfile : Xbox360DriverMacProfile
	{
		// Token: 0x06002941 RID: 10561 RVA: 0x000AFB2C File Offset: 0x000ADD2C
		public RockBandDrumsMacProfile()
		{
			base.Name = "Rock Band Drums";
			base.Meta = "Rock Band Drums on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(7085),
					ProductID = new ushort?(3)
				}
			};
		}
	}
}
