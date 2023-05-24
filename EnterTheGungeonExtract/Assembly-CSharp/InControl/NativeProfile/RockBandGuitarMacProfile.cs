using System;

namespace InControl.NativeProfile
{
	// Token: 0x02000717 RID: 1815
	public class RockBandGuitarMacProfile : Xbox360DriverMacProfile
	{
		// Token: 0x06002942 RID: 10562 RVA: 0x000AFB88 File Offset: 0x000ADD88
		public RockBandGuitarMacProfile()
		{
			base.Name = "Rock Band Guitar";
			base.Meta = "Rock Band Guitar on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(7085),
					ProductID = new ushort?(2)
				}
			};
		}
	}
}
