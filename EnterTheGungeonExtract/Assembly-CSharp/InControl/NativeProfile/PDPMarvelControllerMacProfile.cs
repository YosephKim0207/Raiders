using System;

namespace InControl.NativeProfile
{
	// Token: 0x020006FF RID: 1791
	public class PDPMarvelControllerMacProfile : Xbox360DriverMacProfile
	{
		// Token: 0x0600292A RID: 10538 RVA: 0x000AF0B8 File Offset: 0x000AD2B8
		public PDPMarvelControllerMacProfile()
		{
			base.Name = "PDP Marvel Controller";
			base.Meta = "PDP Marvel Controller on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(3695),
					ProductID = new ushort?(327)
				}
			};
		}
	}
}
