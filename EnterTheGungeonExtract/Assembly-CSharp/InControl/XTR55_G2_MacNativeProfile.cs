using System;

namespace InControl
{
	// Token: 0x02000731 RID: 1841
	[AutoDiscover]
	public class XTR55_G2_MacNativeProfile : NativeInputDeviceProfile
	{
		// Token: 0x0600295C RID: 10588 RVA: 0x000B3294 File Offset: 0x000B1494
		public XTR55_G2_MacNativeProfile()
		{
			base.Name = "SAILI Simulator XTR5.5 G2 FMS Controller";
			base.Meta = "SAILI Simulator XTR5.5 G2 FMS Controller on OS X";
			base.DeviceClass = InputDeviceClass.Controller;
			base.IncludePlatforms = new string[] { "OS X" };
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(2971),
					ProductID = new ushort?(16402),
					NameLiterals = new string[] { "SAILI Simulator --- XTR5.5+G2+FMS Controller" }
				}
			};
		}
	}
}
