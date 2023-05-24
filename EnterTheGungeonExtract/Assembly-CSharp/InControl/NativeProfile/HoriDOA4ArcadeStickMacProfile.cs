using System;

namespace InControl.NativeProfile
{
	// Token: 0x020006C4 RID: 1732
	public class HoriDOA4ArcadeStickMacProfile : Xbox360DriverMacProfile
	{
		// Token: 0x060028EF RID: 10479 RVA: 0x000AD578 File Offset: 0x000AB778
		public HoriDOA4ArcadeStickMacProfile()
		{
			base.Name = "Hori DOA4 Arcade Stick";
			base.Meta = "Hori DOA4 Arcade Stick on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(3853),
					ProductID = new ushort?(10)
				}
			};
		}
	}
}
