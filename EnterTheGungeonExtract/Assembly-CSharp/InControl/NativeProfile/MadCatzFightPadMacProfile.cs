using System;

namespace InControl.NativeProfile
{
	// Token: 0x020006E3 RID: 1763
	public class MadCatzFightPadMacProfile : Xbox360DriverMacProfile
	{
		// Token: 0x0600290E RID: 10510 RVA: 0x000AE26C File Offset: 0x000AC46C
		public MadCatzFightPadMacProfile()
		{
			base.Name = "Mad Catz FightPad";
			base.Meta = "Mad Catz FightPad on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(7085),
					ProductID = new ushort?(61486)
				}
			};
		}
	}
}
