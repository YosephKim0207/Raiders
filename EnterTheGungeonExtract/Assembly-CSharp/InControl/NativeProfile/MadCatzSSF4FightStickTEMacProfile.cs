using System;

namespace InControl.NativeProfile
{
	// Token: 0x020006F3 RID: 1779
	public class MadCatzSSF4FightStickTEMacProfile : Xbox360DriverMacProfile
	{
		// Token: 0x0600291E RID: 10526 RVA: 0x000AE86C File Offset: 0x000ACA6C
		public MadCatzSSF4FightStickTEMacProfile()
		{
			base.Name = "Mad Catz SSF4 Fight Stick TE";
			base.Meta = "Mad Catz SSF4 Fight Stick TE on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(1848),
					ProductID = new ushort?(63288)
				}
			};
		}
	}
}
