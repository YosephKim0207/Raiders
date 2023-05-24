using System;

namespace InControl.NativeProfile
{
	// Token: 0x020006EC RID: 1772
	public class MadCatzProControllerMacProfile : Xbox360DriverMacProfile
	{
		// Token: 0x06002917 RID: 10519 RVA: 0x000AE5CC File Offset: 0x000AC7CC
		public MadCatzProControllerMacProfile()
		{
			base.Name = "Mad Catz Pro Controller";
			base.Meta = "Mad Catz Pro Controller on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(1848),
					ProductID = new ushort?(18214)
				}
			};
		}
	}
}
