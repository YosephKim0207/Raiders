using System;

namespace InControl.NativeProfile
{
	// Token: 0x020006F9 RID: 1785
	public class MKKlassikFightStickMacProfile : Xbox360DriverMacProfile
	{
		// Token: 0x06002924 RID: 10532 RVA: 0x000AECD0 File Offset: 0x000ACED0
		public MKKlassikFightStickMacProfile()
		{
			base.Name = "MK Klassik Fight Stick";
			base.Meta = "MK Klassik Fight Stick on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(4779),
					ProductID = new ushort?(771)
				}
			};
		}
	}
}
