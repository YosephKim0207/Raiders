using System;

namespace InControl.NativeProfile
{
	// Token: 0x020006C6 RID: 1734
	public class HoriFightingStickEX2MacProfile : Xbox360DriverMacProfile
	{
		// Token: 0x060028F1 RID: 10481 RVA: 0x000AD684 File Offset: 0x000AB884
		public HoriFightingStickEX2MacProfile()
		{
			base.Name = "Hori Fighting Stick EX2";
			base.Meta = "Hori Fighting Stick EX2 on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(3853),
					ProductID = new ushort?(10)
				},
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(7085),
					ProductID = new ushort?(62725)
				},
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(3853),
					ProductID = new ushort?(13)
				}
			};
		}
	}
}
