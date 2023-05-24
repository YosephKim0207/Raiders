using System;

namespace InControl.NativeProfile
{
	// Token: 0x020006E8 RID: 1768
	public class MadCatzMicroControllerMacProfile : Xbox360DriverMacProfile
	{
		// Token: 0x06002913 RID: 10515 RVA: 0x000AE44C File Offset: 0x000AC64C
		public MadCatzMicroControllerMacProfile()
		{
			base.Name = "Mad Catz Micro Controller";
			base.Meta = "Mad Catz Micro Controller on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(1848),
					ProductID = new ushort?(18230)
				}
			};
		}
	}
}
