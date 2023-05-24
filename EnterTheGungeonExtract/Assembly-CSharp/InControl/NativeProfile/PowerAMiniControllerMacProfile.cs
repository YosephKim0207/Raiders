using System;

namespace InControl.NativeProfile
{
	// Token: 0x02000708 RID: 1800
	public class PowerAMiniControllerMacProfile : Xbox360DriverMacProfile
	{
		// Token: 0x06002933 RID: 10547 RVA: 0x000AF4C0 File Offset: 0x000AD6C0
		public PowerAMiniControllerMacProfile()
		{
			base.Name = "PowerA Mini Controller";
			base.Meta = "PowerA Mini Controller on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(9414),
					ProductID = new ushort?(21530)
				}
			};
		}
	}
}
