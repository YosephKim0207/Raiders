using System;

namespace InControl.NativeProfile
{
	// Token: 0x020006F4 RID: 1780
	public class MatCatzControllerMacProfile : Xbox360DriverMacProfile
	{
		// Token: 0x0600291F RID: 10527 RVA: 0x000AE8CC File Offset: 0x000ACACC
		public MatCatzControllerMacProfile()
		{
			base.Name = "Mat Catz Controller";
			base.Meta = "Mat Catz Controller on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(7085),
					ProductID = new ushort?(61462)
				}
			};
		}
	}
}
