using System;

namespace InControl.NativeProfile
{
	// Token: 0x020006C7 RID: 1735
	public class HoriFightingStickVXMacProfile : Xbox360DriverMacProfile
	{
		// Token: 0x060028F2 RID: 10482 RVA: 0x000AD734 File Offset: 0x000AB934
		public HoriFightingStickVXMacProfile()
		{
			base.Name = "Hori Fighting Stick VX";
			base.Meta = "Hori Fighting Stick VX on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(7085),
					ProductID = new ushort?(62723)
				},
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(9414),
					ProductID = new ushort?(21762)
				}
			};
		}
	}
}
