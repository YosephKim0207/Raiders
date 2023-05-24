using System;

namespace InControl
{
	// Token: 0x0200074F RID: 1871
	[AutoDiscover]
	public class XTR55_G2_WindowsNativeProfile : NativeInputDeviceProfile
	{
		// Token: 0x0600297A RID: 10618 RVA: 0x000BC9B8 File Offset: 0x000BABB8
		public XTR55_G2_WindowsNativeProfile()
		{
			base.Name = "SAILI Simulator XTR5.5 G2 FMS Controller";
			base.Meta = "SAILI Simulator XTR5.5 G2 FMS Controller on Windows";
			base.DeviceClass = InputDeviceClass.Controller;
			base.IncludePlatforms = new string[] { "Windows" };
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
