using System;

namespace InControl
{
	// Token: 0x0200074E RID: 1870
	[AutoDiscover]
	public class XTR_G2_WindowsNativeProfile : NativeInputDeviceProfile
	{
		// Token: 0x06002979 RID: 10617 RVA: 0x000BC928 File Offset: 0x000BAB28
		public XTR_G2_WindowsNativeProfile()
		{
			base.Name = "KMODEL Simulator XTR G2 FMS Controller";
			base.Meta = "KMODEL Simulator XTR G2 FMS Controller on Windows";
			base.DeviceClass = InputDeviceClass.Controller;
			base.IncludePlatforms = new string[] { "Windows" };
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(2971),
					ProductID = new ushort?(16402),
					NameLiterals = new string[] { "KMODEL Simulator - XTR+G2+FMS Controller" }
				}
			};
		}
	}
}
