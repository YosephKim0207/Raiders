using System;

namespace InControl.NativeProfile
{
	// Token: 0x02000715 RID: 1813
	public class RedOctaneControllerMacProfile : Xbox360DriverMacProfile
	{
		// Token: 0x06002940 RID: 10560 RVA: 0x000AFAA0 File Offset: 0x000ADCA0
		public RedOctaneControllerMacProfile()
		{
			base.Name = "Red Octane Controller";
			base.Meta = "Red Octane Controller on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(5168),
					ProductID = new ushort?(63489)
				},
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(5168),
					ProductID = new ushort?(672)
				}
			};
		}
	}
}
