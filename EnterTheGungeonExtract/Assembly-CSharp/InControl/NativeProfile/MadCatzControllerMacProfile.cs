using System;

namespace InControl.NativeProfile
{
	// Token: 0x020006E1 RID: 1761
	public class MadCatzControllerMacProfile : Xbox360DriverMacProfile
	{
		// Token: 0x0600290C RID: 10508 RVA: 0x000AE12C File Offset: 0x000AC32C
		public MadCatzControllerMacProfile()
		{
			base.Name = "Mad Catz Controller";
			base.Meta = "Mad Catz Controller on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(1848),
					ProductID = new ushort?(18198)
				},
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(7085),
					ProductID = new ushort?(63746)
				},
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(7085),
					ProductID = new ushort?(61642)
				}
			};
		}
	}
}
