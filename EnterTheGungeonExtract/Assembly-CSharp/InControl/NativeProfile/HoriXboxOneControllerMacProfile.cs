using System;

namespace InControl.NativeProfile
{
	// Token: 0x020006D3 RID: 1747
	public class HoriXboxOneControllerMacProfile : XboxOneDriverMacProfile
	{
		// Token: 0x060028FE RID: 10494 RVA: 0x000ADBF4 File Offset: 0x000ABDF4
		public HoriXboxOneControllerMacProfile()
		{
			base.Name = "Hori Xbox One Controller";
			base.Meta = "Hori Xbox One Controller on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(3853),
					ProductID = new ushort?(103)
				}
			};
		}
	}
}
