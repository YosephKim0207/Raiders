using System;

namespace InControl.NativeProfile
{
	// Token: 0x020006DA RID: 1754
	public class LogitechF310ControllerMacProfile : Xbox360DriverMacProfile
	{
		// Token: 0x06002905 RID: 10501 RVA: 0x000ADE8C File Offset: 0x000AC08C
		public LogitechF310ControllerMacProfile()
		{
			base.Name = "Logitech F310 Controller";
			base.Meta = "Logitech F310 Controller on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(1133),
					ProductID = new ushort?(49693)
				}
			};
		}
	}
}
