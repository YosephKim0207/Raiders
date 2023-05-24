using System;

namespace InControl.NativeProfile
{
	// Token: 0x020006E9 RID: 1769
	public class MadCatzMLGFightStickTEMacProfile : Xbox360DriverMacProfile
	{
		// Token: 0x06002914 RID: 10516 RVA: 0x000AE4AC File Offset: 0x000AC6AC
		public MadCatzMLGFightStickTEMacProfile()
		{
			base.Name = "Mad Catz MLG Fight Stick TE";
			base.Meta = "Mad Catz MLG Fight Stick TE on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(7085),
					ProductID = new ushort?(61502)
				}
			};
		}
	}
}
