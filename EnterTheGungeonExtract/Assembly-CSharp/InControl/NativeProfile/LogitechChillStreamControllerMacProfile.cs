using System;

namespace InControl.NativeProfile
{
	// Token: 0x020006D7 RID: 1751
	public class LogitechChillStreamControllerMacProfile : Xbox360DriverMacProfile
	{
		// Token: 0x06002902 RID: 10498 RVA: 0x000ADD6C File Offset: 0x000ABF6C
		public LogitechChillStreamControllerMacProfile()
		{
			base.Name = "Logitech Chill Stream Controller";
			base.Meta = "Logitech Chill Stream Controller on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(1133),
					ProductID = new ushort?(49730)
				}
			};
		}
	}
}
