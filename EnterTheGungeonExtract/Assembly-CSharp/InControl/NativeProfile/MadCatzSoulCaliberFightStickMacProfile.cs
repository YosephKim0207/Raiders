using System;

namespace InControl.NativeProfile
{
	// Token: 0x020006F1 RID: 1777
	public class MadCatzSoulCaliberFightStickMacProfile : Xbox360DriverMacProfile
	{
		// Token: 0x0600291C RID: 10524 RVA: 0x000AE7AC File Offset: 0x000AC9AC
		public MadCatzSoulCaliberFightStickMacProfile()
		{
			base.Name = "Mad Catz Soul Caliber Fight Stick";
			base.Meta = "Mad Catz Soul Caliber Fight Stick on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(7085),
					ProductID = new ushort?(61503)
				}
			};
		}
	}
}
