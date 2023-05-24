using System;

namespace InControl.NativeProfile
{
	// Token: 0x020006EF RID: 1775
	public class MadCatzSF4FightStickSEMacProfile : Xbox360DriverMacProfile
	{
		// Token: 0x0600291A RID: 10522 RVA: 0x000AE6EC File Offset: 0x000AC8EC
		public MadCatzSF4FightStickSEMacProfile()
		{
			base.Name = "Mad Catz SF4 Fight Stick SE";
			base.Meta = "Mad Catz SF4 Fight Stick SE on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(1848),
					ProductID = new ushort?(18200)
				}
			};
		}
	}
}
