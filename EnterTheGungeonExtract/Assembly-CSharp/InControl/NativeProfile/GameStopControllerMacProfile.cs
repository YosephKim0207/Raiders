using System;

namespace InControl.NativeProfile
{
	// Token: 0x020006C0 RID: 1728
	public class GameStopControllerMacProfile : Xbox360DriverMacProfile
	{
		// Token: 0x060028EB RID: 10475 RVA: 0x000AD34C File Offset: 0x000AB54C
		public GameStopControllerMacProfile()
		{
			base.Name = "GameStop Controller";
			base.Meta = "GameStop Controller on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(3695),
					ProductID = new ushort?(1025)
				},
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(3695),
					ProductID = new ushort?(769)
				},
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(4779),
					ProductID = new ushort?(770)
				},
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(7085),
					ProductID = new ushort?(63745)
				}
			};
		}
	}
}
