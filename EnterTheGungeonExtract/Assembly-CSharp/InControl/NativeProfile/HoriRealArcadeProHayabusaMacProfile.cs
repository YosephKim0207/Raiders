using System;

namespace InControl.NativeProfile
{
	// Token: 0x020006CE RID: 1742
	public class HoriRealArcadeProHayabusaMacProfile : Xbox360DriverMacProfile
	{
		// Token: 0x060028F9 RID: 10489 RVA: 0x000AD9F4 File Offset: 0x000ABBF4
		public HoriRealArcadeProHayabusaMacProfile()
		{
			base.Name = "Hori Real Arcade Pro Hayabusa";
			base.Meta = "Hori Real Arcade Pro Hayabusa on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(3853),
					ProductID = new ushort?(99)
				}
			};
		}
	}
}
