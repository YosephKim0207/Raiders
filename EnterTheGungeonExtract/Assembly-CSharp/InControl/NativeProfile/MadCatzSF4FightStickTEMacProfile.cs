using System;

namespace InControl.NativeProfile
{
	// Token: 0x020006F0 RID: 1776
	public class MadCatzSF4FightStickTEMacProfile : Xbox360DriverMacProfile
	{
		// Token: 0x0600291B RID: 10523 RVA: 0x000AE74C File Offset: 0x000AC94C
		public MadCatzSF4FightStickTEMacProfile()
		{
			base.Name = "Mad Catz SF4 Fight Stick TE";
			base.Meta = "Mad Catz SF4 Fight Stick TE on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(1848),
					ProductID = new ushort?(18232)
				}
			};
		}
	}
}
