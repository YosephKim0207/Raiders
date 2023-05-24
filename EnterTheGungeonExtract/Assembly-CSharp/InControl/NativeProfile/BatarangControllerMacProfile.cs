using System;

namespace InControl.NativeProfile
{
	// Token: 0x020006BC RID: 1724
	public class BatarangControllerMacProfile : Xbox360DriverMacProfile
	{
		// Token: 0x060028E7 RID: 10471 RVA: 0x000AD1CC File Offset: 0x000AB3CC
		public BatarangControllerMacProfile()
		{
			base.Name = "Batarang Controller";
			base.Meta = "Batarang Controller on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(5604),
					ProductID = new ushort?(16144)
				}
			};
		}
	}
}
