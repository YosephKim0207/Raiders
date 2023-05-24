using System;

namespace InControl.NativeProfile
{
	// Token: 0x0200070B RID: 1803
	public class PowerASpectraIlluminatedControllerMacProfile : Xbox360DriverMacProfile
	{
		// Token: 0x06002936 RID: 10550 RVA: 0x000AF634 File Offset: 0x000AD834
		public PowerASpectraIlluminatedControllerMacProfile()
		{
			base.Name = "PowerA Spectra Illuminated Controller";
			base.Meta = "PowerA Spectra Illuminated Controller on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(9414),
					ProductID = new ushort?(21546)
				}
			};
		}
	}
}
