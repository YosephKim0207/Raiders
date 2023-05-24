using System;

namespace InControl.NativeProfile
{
	// Token: 0x020006CA RID: 1738
	public class HoriPadUltimateMacProfile : Xbox360DriverMacProfile
	{
		// Token: 0x060028F5 RID: 10485 RVA: 0x000AD878 File Offset: 0x000ABA78
		public HoriPadUltimateMacProfile()
		{
			base.Name = "HoriPad Ultimate";
			base.Meta = "HoriPad Ultimate on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(3853),
					ProductID = new ushort?(144)
				}
			};
		}
	}
}
