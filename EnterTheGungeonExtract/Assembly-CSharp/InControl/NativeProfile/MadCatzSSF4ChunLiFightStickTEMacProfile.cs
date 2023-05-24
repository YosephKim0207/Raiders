using System;

namespace InControl.NativeProfile
{
	// Token: 0x020006F2 RID: 1778
	public class MadCatzSSF4ChunLiFightStickTEMacProfile : Xbox360DriverMacProfile
	{
		// Token: 0x0600291D RID: 10525 RVA: 0x000AE80C File Offset: 0x000ACA0C
		public MadCatzSSF4ChunLiFightStickTEMacProfile()
		{
			base.Name = "Mad Catz SSF4 Chun-Li Fight Stick TE";
			base.Meta = "Mad Catz SSF4 Chun-Li Fight Stick TE on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(7085),
					ProductID = new ushort?(61501)
				}
			};
		}
	}
}
