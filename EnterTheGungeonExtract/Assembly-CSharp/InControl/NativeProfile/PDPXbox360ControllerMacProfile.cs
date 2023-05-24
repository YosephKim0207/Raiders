using System;

namespace InControl.NativeProfile
{
	// Token: 0x02000703 RID: 1795
	public class PDPXbox360ControllerMacProfile : Xbox360DriverMacProfile
	{
		// Token: 0x0600292E RID: 10542 RVA: 0x000AF238 File Offset: 0x000AD438
		public PDPXbox360ControllerMacProfile()
		{
			base.Name = "PDP Xbox 360 Controller";
			base.Meta = "PDP Xbox 360 Controller on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(3695),
					ProductID = new ushort?(1281)
				}
			};
		}
	}
}
