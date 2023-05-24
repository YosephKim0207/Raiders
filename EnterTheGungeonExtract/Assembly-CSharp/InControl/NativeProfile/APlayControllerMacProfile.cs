using System;

namespace InControl.NativeProfile
{
	// Token: 0x020006BB RID: 1723
	public class APlayControllerMacProfile : Xbox360DriverMacProfile
	{
		// Token: 0x060028E6 RID: 10470 RVA: 0x000AD16C File Offset: 0x000AB36C
		public APlayControllerMacProfile()
		{
			base.Name = "A Play Controller";
			base.Meta = "A Play Controller on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(9414),
					ProductID = new ushort?(64251)
				}
			};
		}
	}
}
