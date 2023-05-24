using System;

namespace InControl.NativeProfile
{
	// Token: 0x020006BE RID: 1726
	public class BigBenControllerMacProfile : Xbox360DriverMacProfile
	{
		// Token: 0x060028E9 RID: 10473 RVA: 0x000AD28C File Offset: 0x000AB48C
		public BigBenControllerMacProfile()
		{
			base.Name = "Big Ben Controller";
			base.Meta = "Big Ben Controller on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(5227),
					ProductID = new ushort?(1537)
				}
			};
		}
	}
}
