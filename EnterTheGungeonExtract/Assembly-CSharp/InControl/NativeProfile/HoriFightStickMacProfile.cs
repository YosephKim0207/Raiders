using System;

namespace InControl.NativeProfile
{
	// Token: 0x020006C8 RID: 1736
	public class HoriFightStickMacProfile : Xbox360DriverMacProfile
	{
		// Token: 0x060028F3 RID: 10483 RVA: 0x000AD7C0 File Offset: 0x000AB9C0
		public HoriFightStickMacProfile()
		{
			base.Name = "Hori Fight Stick";
			base.Meta = "Hori Fight Stick on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(3853),
					ProductID = new ushort?(13)
				}
			};
		}
	}
}
