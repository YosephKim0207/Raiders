using System;

namespace InControl.NativeProfile
{
	// Token: 0x020006FC RID: 1788
	public class NaconGC100XFControllerMacProfile : Xbox360DriverMacProfile
	{
		// Token: 0x06002927 RID: 10535 RVA: 0x000AEE1C File Offset: 0x000AD01C
		public NaconGC100XFControllerMacProfile()
		{
			base.Name = "Nacon GC-100XF Controller";
			base.Meta = "Nacon GC-100XF Controller on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(4553),
					ProductID = new ushort?(22000)
				}
			};
		}
	}
}
