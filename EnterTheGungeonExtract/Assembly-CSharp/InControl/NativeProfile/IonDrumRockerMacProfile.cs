using System;

namespace InControl.NativeProfile
{
	// Token: 0x020006D4 RID: 1748
	public class IonDrumRockerMacProfile : Xbox360DriverMacProfile
	{
		// Token: 0x060028FF RID: 10495 RVA: 0x000ADC50 File Offset: 0x000ABE50
		public IonDrumRockerMacProfile()
		{
			base.Name = "Ion Drum Rocker";
			base.Meta = "Ion Drum Rocker on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(7085),
					ProductID = new ushort?(304)
				}
			};
		}
	}
}
