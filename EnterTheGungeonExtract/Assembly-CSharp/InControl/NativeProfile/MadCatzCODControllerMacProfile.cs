using System;

namespace InControl.NativeProfile
{
	// Token: 0x020006E0 RID: 1760
	public class MadCatzCODControllerMacProfile : Xbox360DriverMacProfile
	{
		// Token: 0x0600290B RID: 10507 RVA: 0x000AE0CC File Offset: 0x000AC2CC
		public MadCatzCODControllerMacProfile()
		{
			base.Name = "Mad Catz COD Controller";
			base.Meta = "Mad Catz COD Controller on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(7085),
					ProductID = new ushort?(61477)
				}
			};
		}
	}
}
