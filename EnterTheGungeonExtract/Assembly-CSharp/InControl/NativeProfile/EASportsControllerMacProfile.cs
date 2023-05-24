using System;

namespace InControl.NativeProfile
{
	// Token: 0x020006BF RID: 1727
	public class EASportsControllerMacProfile : Xbox360DriverMacProfile
	{
		// Token: 0x060028EA RID: 10474 RVA: 0x000AD2EC File Offset: 0x000AB4EC
		public EASportsControllerMacProfile()
		{
			base.Name = "EA Sports Controller";
			base.Meta = "EA Sports Controller on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(3695),
					ProductID = new ushort?(305)
				}
			};
		}
	}
}
