using System;

namespace InControl.NativeProfile
{
	// Token: 0x020006ED RID: 1773
	public class MadCatzSaitekAV8R02MacProfile : Xbox360DriverMacProfile
	{
		// Token: 0x06002918 RID: 10520 RVA: 0x000AE62C File Offset: 0x000AC82C
		public MadCatzSaitekAV8R02MacProfile()
		{
			base.Name = "Mad Catz Saitek AV8R02";
			base.Meta = "Mad Catz Saitek AV8R02 on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(1848),
					ProductID = new ushort?(52009)
				}
			};
		}
	}
}
