using System;

namespace InControl.NativeProfile
{
	// Token: 0x02000701 RID: 1793
	public class PDPTronControllerMacProfile : Xbox360DriverMacProfile
	{
		// Token: 0x0600292C RID: 10540 RVA: 0x000AF178 File Offset: 0x000AD378
		public PDPTronControllerMacProfile()
		{
			base.Name = "PDP Tron Controller";
			base.Meta = "PDP Tron Controller on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(7085),
					ProductID = new ushort?(63747)
				}
			};
		}
	}
}
