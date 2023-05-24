using System;

namespace InControl.NativeProfile
{
	// Token: 0x020006BA RID: 1722
	public class AfterglowPrismaticXboxOneControllerMacProfile : XboxOneDriverMacProfile
	{
		// Token: 0x060028E5 RID: 10469 RVA: 0x000AD10C File Offset: 0x000AB30C
		public AfterglowPrismaticXboxOneControllerMacProfile()
		{
			base.Name = "Afterglow Prismatic Xbox One Controller";
			base.Meta = "Afterglow Prismatic Xbox One Controller on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(3695),
					ProductID = new ushort?(313)
				}
			};
		}
	}
}
