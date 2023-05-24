using System;

namespace InControl.NativeProfile
{
	// Token: 0x020006E7 RID: 1767
	public class MadCatzMicroConControllerMacProfile : Xbox360DriverMacProfile
	{
		// Token: 0x06002912 RID: 10514 RVA: 0x000AE3EC File Offset: 0x000AC5EC
		public MadCatzMicroConControllerMacProfile()
		{
			base.Name = "Mad Catz MicroCon Controller";
			base.Meta = "Mad Catz MicroCon Controller on Mac";
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
