using System;

namespace InControl.NativeProfile
{
	// Token: 0x02000718 RID: 1816
	public class RockCandyControllerMacProfile : Xbox360DriverMacProfile
	{
		// Token: 0x06002943 RID: 10563 RVA: 0x000AFBE4 File Offset: 0x000ADDE4
		public RockCandyControllerMacProfile()
		{
			base.Name = "Rock Candy Controller";
			base.Meta = "Rock Candy Controller on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(3695),
					ProductID = new ushort?(287)
				}
			};
		}
	}
}
