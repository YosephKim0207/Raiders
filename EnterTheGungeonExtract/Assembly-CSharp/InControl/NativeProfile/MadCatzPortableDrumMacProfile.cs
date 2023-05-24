using System;

namespace InControl.NativeProfile
{
	// Token: 0x020006EB RID: 1771
	public class MadCatzPortableDrumMacProfile : Xbox360DriverMacProfile
	{
		// Token: 0x06002916 RID: 10518 RVA: 0x000AE56C File Offset: 0x000AC76C
		public MadCatzPortableDrumMacProfile()
		{
			base.Name = "Mad Catz Portable Drum";
			base.Meta = "Mad Catz Portable Drum on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(1848),
					ProductID = new ushort?(39025)
				}
			};
		}
	}
}
