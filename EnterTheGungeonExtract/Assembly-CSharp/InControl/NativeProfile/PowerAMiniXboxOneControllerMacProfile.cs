using System;

namespace InControl.NativeProfile
{
	// Token: 0x0200070A RID: 1802
	public class PowerAMiniXboxOneControllerMacProfile : XboxOneDriverMacProfile
	{
		// Token: 0x06002935 RID: 10549 RVA: 0x000AF5D4 File Offset: 0x000AD7D4
		public PowerAMiniXboxOneControllerMacProfile()
		{
			base.Name = "Power A Mini Xbox One Controller";
			base.Meta = "Power A Mini Xbox One Controller on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(9414),
					ProductID = new ushort?(21562)
				}
			};
		}
	}
}
