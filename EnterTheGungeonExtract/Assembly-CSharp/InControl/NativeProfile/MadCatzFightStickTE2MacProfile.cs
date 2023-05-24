using System;

namespace InControl.NativeProfile
{
	// Token: 0x020006E4 RID: 1764
	public class MadCatzFightStickTE2MacProfile : Xbox360DriverMacProfile
	{
		// Token: 0x0600290F RID: 10511 RVA: 0x000AE2CC File Offset: 0x000AC4CC
		public MadCatzFightStickTE2MacProfile()
		{
			base.Name = "Mad Catz Fight Stick TE2";
			base.Meta = "Mad Catz Fight Stick TE2 on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(7085),
					ProductID = new ushort?(61568)
				}
			};
		}
	}
}
