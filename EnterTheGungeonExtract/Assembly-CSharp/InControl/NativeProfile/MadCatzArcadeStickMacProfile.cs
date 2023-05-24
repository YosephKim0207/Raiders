using System;

namespace InControl.NativeProfile
{
	// Token: 0x020006DE RID: 1758
	public class MadCatzArcadeStickMacProfile : Xbox360DriverMacProfile
	{
		// Token: 0x06002909 RID: 10505 RVA: 0x000AE00C File Offset: 0x000AC20C
		public MadCatzArcadeStickMacProfile()
		{
			base.Name = "Mad Catz Arcade Stick";
			base.Meta = "Mad Catz Arcade Stick on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(1848),
					ProductID = new ushort?(18264)
				}
			};
		}
	}
}
