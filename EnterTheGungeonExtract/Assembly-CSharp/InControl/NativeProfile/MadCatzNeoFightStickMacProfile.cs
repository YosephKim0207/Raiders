using System;

namespace InControl.NativeProfile
{
	// Token: 0x020006EA RID: 1770
	public class MadCatzNeoFightStickMacProfile : Xbox360DriverMacProfile
	{
		// Token: 0x06002915 RID: 10517 RVA: 0x000AE50C File Offset: 0x000AC70C
		public MadCatzNeoFightStickMacProfile()
		{
			base.Name = "Mad Catz Neo Fight Stick";
			base.Meta = "Mad Catz Neo Fight Stick on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(7085),
					ProductID = new ushort?(61498)
				}
			};
		}
	}
}
