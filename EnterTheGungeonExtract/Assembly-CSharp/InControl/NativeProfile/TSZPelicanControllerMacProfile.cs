using System;

namespace InControl.NativeProfile
{
	// Token: 0x0200071F RID: 1823
	public class TSZPelicanControllerMacProfile : Xbox360DriverMacProfile
	{
		// Token: 0x0600294A RID: 10570 RVA: 0x000AFF58 File Offset: 0x000AE158
		public TSZPelicanControllerMacProfile()
		{
			base.Name = "TSZ Pelican Controller";
			base.Meta = "TSZ Pelican Controller on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(3695),
					ProductID = new ushort?(513)
				}
			};
		}
	}
}
