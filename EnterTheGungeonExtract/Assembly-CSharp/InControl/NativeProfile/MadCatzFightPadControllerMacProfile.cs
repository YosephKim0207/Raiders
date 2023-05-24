using System;

namespace InControl.NativeProfile
{
	// Token: 0x020006E2 RID: 1762
	public class MadCatzFightPadControllerMacProfile : Xbox360DriverMacProfile
	{
		// Token: 0x0600290D RID: 10509 RVA: 0x000AE1E0 File Offset: 0x000AC3E0
		public MadCatzFightPadControllerMacProfile()
		{
			base.Name = "Mad Catz FightPad Controller";
			base.Meta = "Mad Catz FightPad Controller on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(7085),
					ProductID = new ushort?(61480)
				},
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(1848),
					ProductID = new ushort?(18216)
				}
			};
		}
	}
}
