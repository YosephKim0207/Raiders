using System;

namespace InControl.NativeProfile
{
	// Token: 0x020006E5 RID: 1765
	public class MadCatzFightStickTESPlusMacProfile : Xbox360DriverMacProfile
	{
		// Token: 0x06002910 RID: 10512 RVA: 0x000AE32C File Offset: 0x000AC52C
		public MadCatzFightStickTESPlusMacProfile()
		{
			base.Name = "Mad Catz Fight Stick TES Plus";
			base.Meta = "Mad Catz Fight Stick TES Plus on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(7085),
					ProductID = new ushort?(61506)
				}
			};
		}
	}
}
